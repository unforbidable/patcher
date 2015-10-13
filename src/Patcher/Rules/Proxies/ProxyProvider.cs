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

namespace Patcher.Rules.Proxies
{
    public sealed class ProxyProvider
    {
        IDictionary<string, ProxyInfo> proxies = new SortedDictionary<string, ProxyInfo>();
        IDictionary<FormKind, string> formKindMap = new SortedDictionary<FormKind, string>();
        IDictionary<string, string> interfaceMap = new SortedDictionary<string, string>();

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

                bool isForm = type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(FormProxy<>);
                Type backingFormType = isForm ? type.BaseType.GetGenericArguments()[0] : null;
                FormKind formKind = isForm ? engine.Context.GetRecordFormKind(backingFormType) : FormKind.None;
                Type localType = type;// isForm ? backingFormType : type;

                proxies.Add(localType.FullName, new ProxyInfo()
                {
                    ImplementationType = type,
                    InterfaceType = a.Interface,
                    IsFormProxy = isForm,
                    FormKind = formKind,
                    BackingFormType = backingFormType,
                });

                interfaceMap.Add(a.Interface.FullName, localType.FullName);

                if (isForm)
                    formKindMap.Add(formKind, localType.FullName);
            }
        }

        public T CreateProxy<T>(ProxyMode mode) where T : Proxy
        {
            if (!proxies.ContainsKey(typeof(T).FullName))
                throw new NotImplementedException("Proxy implementation not found: " + typeof(T).FullName);
            return (T)CreateProxy(proxies[typeof(T).FullName], mode);
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
                var proxy = CreateFormProxy<T>(FormKind.None, mode);
                ((FormProxy)(object)proxy).WithForm(formId);
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
                return CreateFormProxy(FormKind.None, mode).WithForm(formId);
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
            if (kind == FormKind.None)
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
            return proxies.Values.Where(p => p.InterfaceType == type).Single().FormKind;
        }

        class ProxyInfo
        {
            public Type BackingFormType { get; set; }
            public Type InterfaceType { get; set; }
            public Type ImplementationType { get; set; }
            public bool IsFormProxy { get; set; }
            public FormKind FormKind { get; set; }
        }
    }
}

