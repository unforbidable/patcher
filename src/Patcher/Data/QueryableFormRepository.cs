/// Copyright(C) 2015 Unforbidable Works
///
/// This program is free software; you can redistribute it and/or
/// modify it under the terms of the GNU General Public License
/// as published by the Free Software Foundation; either version 2
/// of the License, or(at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with this program; if not, write to the Free Software
/// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using Patcher.Data.Plugins;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Patcher.Data
{
    class QueryableFormRepository : IOrderedQueryable<Form>
    {
        public Type ElementType { get { return typeof(Form); } }
        public Expression Expression { get; private set; }
        public IQueryProvider Provider { get; private set; }

        public QueryableFormRepository(FormRepository forms)
        {
            Expression = Expression.Constant(this);
            Provider = new QueryProvider(forms);
        }

        internal QueryableFormRepository(IQueryProvider provider, Expression expression)
        {
            Provider = provider;
            Expression = expression;
        }

        public IEnumerator<Form> GetEnumerator()
        {
            return Provider.Execute<IEnumerable<Form>>(Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class QueryProvider : IQueryProvider
        {
            readonly FormRepository forms;

            public QueryProvider(FormRepository forms)
            {
                this.forms = forms;
            }

            public IQueryable CreateQuery(Expression expression)
            {
                return new QueryableFormRepository(this, expression);
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return (IQueryable<TElement>)new QueryableFormRepository(this, expression);
            }

            public object Execute(Expression expression)
            {
                return Execute<Form>(expression);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                var isEnumerable = (typeof(TResult).Name == "IEnumerable`1");
                return (TResult)ExecuteExpression(expression, isEnumerable);
            }

            private object ExecuteExpression(Expression expression, bool isEnumerable)
            {
                var queriable = forms.AsQueryable();

                // Optimalize expression
                var optimalizer = new Optimalizer(queriable);
                expression = optimalizer.Visit(expression);

                if (optimalizer.WhereEditorIdEquals != null)
                {
                    if (forms.Contains(optimalizer.WhereEditorIdEquals))
                    {
                        var form = forms[optimalizer.WhereEditorIdEquals];
                        if (!isEnumerable)
                            return form;
                        else
                            return new Form[] { form }.ToList();
                    }
                    else
                    {
                        if (!isEnumerable)
                            return null;
                        else
                            return Enumerable.Empty<Form>();
                    }
                }
                else if (optimalizer.WhereFormIdEquals != null)
                {
                    if (forms.Contains(optimalizer.WhereFormIdEquals.Value))
                    {
                        var form = forms[optimalizer.WhereFormIdEquals.Value];
                        if (!isEnumerable)
                            return form;
                        else
                            return new Form[] { form }.ToList();
                    }
                    else
                    {
                        if (!isEnumerable)
                            return null;
                        else
                            return Enumerable.Empty<Form>();
                    }
                }
                else if (optimalizer.WhereFormKindEquals != null)
                {
                    queriable = forms.OfKind(optimalizer.WhereFormKindEquals.Value).AsQueryable();
                }
                else if (optimalizer.WherePluginNumberEquals != null)
                {
                    queriable = forms.OfPlugin(optimalizer.WherePluginNumberEquals.Value).AsQueryable();
                }

                // Replace constant queriable
                var replacer = new QueryableReplacer(queriable);
                expression = replacer.Visit(expression);

                if (isEnumerable)
                {
                    return queriable.Provider.CreateQuery(expression);
                }
                else
                {
                    return queriable.Provider.Execute(expression);
                }
            }

            class Optimalizer : ExpressionVisitor
            {
                readonly IQueryable<Form> queryable;

                public byte? WherePluginNumberEquals { get; private set; }
                public FormKind? WhereFormKindEquals { get; private set; }
                public uint? WhereFormIdEquals { get; private set; }
                public string WhereEditorIdEquals { get; private set; }

                public Optimalizer(IQueryable<Form> queryable)
                {
                    this.queryable = queryable;
                }

                protected override Expression VisitBinary(BinaryExpression node)
                {
                    if (node.NodeType == ExpressionType.Equal)
                    {
                        if (IsFormMemberAccess("PluginNumber", node.Left))
                        {
                            WherePluginNumberEquals = Convert.ToByte(queryable.Provider.Execute(node.Right));
                        }
                        else if (IsFormMemberAccess("PluginNumber", node.Right))
                        {
                            WherePluginNumberEquals = Convert.ToByte(queryable.Provider.Execute(node.Left));
                        }
                        else if (IsFormMemberAccess("FormType", node.Left))
                        {
                            WhereFormKindEquals = (FormKind)Convert.ToString(queryable.Provider.Execute(node.Right));
                        }
                        else if (IsFormMemberAccess("FormType", node.Right))
                        {
                            WhereFormKindEquals = (FormKind)Convert.ToString(queryable.Provider.Execute(node.Left));
                        }
                        else if (IsFormMemberAccess("FormId", node.Left))
                        {
                            WhereFormIdEquals = Convert.ToUInt32(queryable.Provider.Execute(node.Right));
                        }
                        else if (IsFormMemberAccess("FormId", node.Right))
                        {
                            WhereFormIdEquals = Convert.ToUInt32(queryable.Provider.Execute(node.Left));
                        }
                        else if (IsFormMemberAccess("EditorId", node.Left))
                        {
                            WhereEditorIdEquals = Convert.ToString(queryable.Provider.Execute(node.Right));
                        }
                        else if (IsFormMemberAccess("EditorId", node.Right))
                        {
                            WhereEditorIdEquals = Convert.ToString(queryable.Provider.Execute(node.Left));
                        }
                    }

                    return base.VisitBinary(node);
                }

                private bool IsFormMemberAccess(string memberName, Expression expression)
                {
                    if (expression.NodeType == ExpressionType.Convert)
                    {
                        var convert = (UnaryExpression)expression;
                        return IsFormMemberAccess(memberName, convert.Operand);
                    }
                    else if (expression.NodeType == ExpressionType.MemberAccess)
                    {
                        var memberAccess = (MemberExpression)expression;
                        return memberAccess.Expression.NodeType == ExpressionType.Parameter &&
                            memberAccess.Expression.Type == typeof(Form) &&
                            memberAccess.Member.Name == memberName;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            class QueryableReplacer : ExpressionVisitor
            {
                readonly IQueryable<Form> queryable;

                public QueryableReplacer(IQueryable<Form> queryable)
                {
                    this.queryable = queryable;
                }

                protected override Expression VisitConstant(ConstantExpression node)
                {
                    if (node.Type == typeof(QueryableFormRepository))
                        return Expression.Constant(queryable);

                    return node;
                }
            }
        }
    }
}
