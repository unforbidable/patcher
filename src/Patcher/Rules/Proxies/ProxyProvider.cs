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
using Patcher.Rules.Compiled.Forms;
using Patcher.Rules.Proxies.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Patcher.Data.Plugins.Content;
using Patcher.Rules.Compiled.Fields;
using Patcher.Rules.Proxies.Fields;
using Patcher.Data.Plugins.Content.Records.Skyrim;
using Patcher.Rules.Compiled.Fields.Skyrim;

namespace Patcher.Rules.Proxies
{
    public sealed class ProxyProvider
    {
        IDictionary<string, ProxyInfo> proxies = new SortedDictionary<string, ProxyInfo>();
        IDictionary<FormKind, string> formKindMap = new SortedDictionary<FormKind, string>();
        IDictionary<string, string> interfaceMap = new SortedDictionary<string, string>();
        IDictionary<string, string> backingRecordMap = new SortedDictionary<string, string>();
        IDictionary<string, string> backingFieldMap = new SortedDictionary<string, string>();

        IDictionary<string, Stack<Proxy>> disposed = new SortedDictionary<string, Stack<Proxy>>();

        readonly RuleEngine engine;

        public RuleEngine Engine { get { return engine; } }

        // Created by engine
        internal ProxyProvider(RuleEngine engine)
        {
            this.engine = engine;

            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(Proxy).IsAssignableFrom(t) && !t.IsAbstract && !t.IsGenericType);
            foreach (Type type in types)
            {
                // Fetch attribute
                ProxyAttribute a = (ProxyAttribute)type.GetCustomAttributes(typeof(ProxyAttribute), false).FirstOrDefault();
                if (a == null)
                    throw new InvalidProgramException("Proxy type " + type.FullName + " is missing a ProxyAttribute");

                Type backingRecordType = null;
                Type searchType = type.BaseType;
                while (searchType != null)
                {
                    if (searchType.IsGenericType && searchType.GetGenericTypeDefinition() == typeof(FormProxy<>))
                    {
                        backingRecordType = type.BaseType.GetGenericArguments().Single();
                        break;
                    }

                    searchType = searchType.BaseType;
                }

                Type backingFieldType = null;
                searchType = type.BaseType;
                while (searchType != null)
                {
                    if (searchType.IsGenericType && searchType.GetGenericTypeDefinition() == typeof(FieldProxy<>))
                    {
                        backingFieldType = type.BaseType.GetGenericArguments().Single();
                        break;
                    }

                    searchType = searchType.BaseType;
                }


                FormKind backingFormKind = backingRecordType != null ? engine.Context.GetRecordFormKind(backingRecordType) : FormKind.Any;
                Type implementationType = type;

                proxies.Add(implementationType.FullName, new ProxyInfo()
                {
                    ImplementationType = implementationType,
                    InterfaceType = a.Interface,
                    BackingFormKind = backingFormKind,
                    BackingRecordType = backingRecordType,
                    BackingFieldType = backingFieldType
                });

                interfaceMap.Add(a.Interface.FullName, implementationType.FullName);

                if (backingRecordType != null)
                {
                    formKindMap.Add(backingFormKind, implementationType.FullName);
                    backingRecordMap.Add(backingRecordType.FullName, implementationType.FullName);
                }
                else if (backingFieldType != null)
                {
                    backingFieldMap.Add(backingFieldType.FullName, implementationType.FullName);
                }
            }
        }
        
        public FieldProxy<T> CreateFieldProxy<T>(ProxyMode mode) where T : Field
        {
            string typeFullName = typeof(T).FullName;
            if (!backingFieldMap.ContainsKey(typeFullName))
                throw new InvalidProgramException("Could not find proxy implmentation for field " + typeFullName + ".");

            var info = proxies[backingFieldMap[typeFullName]];
            return CreateProxy<FieldProxy<T>>(info, mode);
        }

        public FieldProxy<T> CreateFieldProxy<T>(T field, ProxyMode mode) where T : Field
        {
            var proxy = CreateFieldProxy<T>(mode);
            proxy.Field = field;
            return proxy;
        }

        /// <summary>
        /// Creates form proxy from a reference or returns null if specified Form ID is 0.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="formId"></param>
        /// <returns></returns>
        public T CreateReferenceProxy<T>(uint formId) where T : IForm
        {
            if (formId == 0)
            {
                return default(T);
            }
            else
            {
                return CreateFormProxy<T>(formId, ProxyMode.Referenced);
            }
        }

        public T CreateProxy<T>(ProxyMode mode) where T : Proxy
        {
            string typeFullName = typeof(T).FullName;
            if (!proxies.ContainsKey(typeFullName))
                throw new NotImplementedException("Proxy implementation not found: " + typeFullName);
            return (T)CreateProxy(proxies[typeFullName], mode);
        }

        public FormCollectionProxy<T> CreateFormCollectionProxy<T>(ProxyMode mode, IEnumerable<uint> items) where T : IForm
        {
            return new FormCollectionProxy<T>()
            {
                Provider = this,
                Mode = mode,
                Items = items
            };
        }

        public T CreateFormProxy<T>(uint formId, ProxyMode mode) where T : IForm
        {
            if (formId == 0 || !engine.Context.Forms.Contains(formId))
            {
                // Create dummy proxy, setting formID 
                var proxy = CreateFormProxy<T>(FormKind.Any, mode);
                ((FormProxy)(IForm)proxy).WithForm(formId);
                return proxy;
            }
            else
            {
                return CreateFormProxy<T>(engine.Context.Forms[formId], mode);
            }
        }

        /// <summary>
        /// Creates a new proxy loaded with form that has the specified Form ID.
        /// If provided Form ID is zero, returned proxy will be a null form proxy.
        /// If provided Form ID is not resolved, returned proxy will be an unresolved form proxy.
        /// </summary>
        /// <param name="formId"></param>
        /// <param name="mode">Proxy mode of created proxy</param>
        /// <returns></returns>
        public FormProxy CreateFormProxy(uint formId, ProxyMode mode)
        {
            if (formId == 0 || !engine.Context.Forms.Contains(formId))
            {
                // Create dummy proxy, setting formID 
                return CreateFormProxy(FormKind.Any, mode).WithForm(formId);
            }
            else
            {
                return CreateFormProxy(engine.Context.Forms[formId], mode);
            }
        }

        /// <summary>
        /// Creates a new proxy loaded with the specified form.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="form"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public T CreateFormProxy<T>(Form form, ProxyMode mode) where T : IForm
        {
            var proxy = CreateFormProxy<T>(form.FormKind, mode);

            // Force cast to FormProxy via a common IForm interface to set Form property
            // This will work because any class that implements IForm does it through the FormProxy class
            ((FormProxy)(IForm)proxy).WithForm(form);

            return proxy;
        }

        /// <summary>
        /// Creates a new proxy based on the specified FormType without loading a form. 
        /// A form needs to be loaded with method WithForm() before the proxy can be used.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="kind"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public T CreateFormProxy<T>(FormKind kind, ProxyMode mode) where T : IForm
        {
            var proxy = CreateProxy<T>(GetFormProxyInfo(kind), mode);
            return proxy;
        }

        /// <summary>
        /// Creates a new proxy loaded with the specified form.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public FormProxy CreateFormProxy(Form form, ProxyMode mode)
        {
            var proxy = CreateFormProxy(form.FormKind, mode);
            proxy.WithForm(form);
            return proxy;
        }

        /// <summary>
        /// Creates a new proxy based on the specified FormType without loading a form. 
        /// A form needs to be loaded with method WithForm() before the proxy can be used.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public FormProxy CreateFormProxy(FormKind kind, ProxyMode mode)
        {
            return CreateProxy<FormProxy>(GetFormProxyInfo(kind), mode);
        }

        private ProxyInfo GetFormProxyInfo(FormKind kind)
        {
            if (formKindMap.ContainsKey(kind))
                return proxies[formKindMap[kind]];

            //throw new InvalidProgramException("Proxy not found for form type " + formType);
            // Form proxy not found - not supported. Return DummyProxy info
            return proxies[typeof(DummyFormProxy).FullName];
        }

        private T CreateProxy<T>(ProxyInfo info, ProxyMode mode)
        {
            // Ensure T is compatible info.ImplementationType
            // Otherwise cast to T will fail anyway
            if (!typeof(T).IsAssignableFrom(info.ImplementationType))
                throw new InvalidProgramException("Created proxy " + info.ImplementationType.FullName + " is not compatible with requested type " + typeof(T).FullName);

            return (T)CreateProxy(info, mode);
        }

        private object CreateProxy(ProxyInfo info, ProxyMode mode)
        {
            var proxy = (Proxy)Activator.CreateInstance(info.ImplementationType);
            proxy.Provider = this;
            proxy.Mode = mode;
            return proxy;
        }

        public Type GetInterface(FormKind kind)
        {
            if (kind == FormKind.Any)
            {
                return typeof(object);
            }
            else
            {
                return GetFormProxyInfo(kind).InterfaceType;
            }
        }

        public FormKind GetFormKindOfInterface(Type type)
        {
            return proxies.Values.Where(p => p.InterfaceType == type).Select(p => p.BackingFormKind).SingleOrDefault();
        }

        class ProxyInfo
        {
            public Type InterfaceType { get; set; }
            public Type ImplementationType { get; set; }
            public Type BackingRecordType { get; set; }
            public Type BackingFieldType { get; set; }
            public FormKind BackingFormKind { get; set; }
        }
    }
}

