using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Core.Service
{
    public class DynamicBaseObject<T> : DynamicObject where T : new()
    {
        private T _containedObject = default(T);

        [JsonExtensionData]
        private Dictionary<string, object> _dynamicMembers = new Dictionary<string, object>();

        private List<PropertyInfo> _propertyInfos = new List<PropertyInfo>(typeof(T).GetProperties());

        public DynamicBaseObject()
        {
        }
        public DynamicBaseObject(T containedObject)
        {
            _containedObject = containedObject;
        }
        public Dictionary<string, object> DynamicMembers
        {
            get
            {
                return _dynamicMembers;
            }

            set
            {
                _dynamicMembers = value;
            }
        }
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (_dynamicMembers.ContainsKey(binder.Name) && _dynamicMembers[binder.Name] is Delegate)
            {
                result = (_dynamicMembers[binder.Name] as Delegate).DynamicInvoke(args);
                return true;
            }

            return base.TryInvokeMember(binder, args, out result);
        }

        public override IEnumerable<string> GetDynamicMemberNames() => _dynamicMembers.Keys;

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            var propertyInfo = _propertyInfos.Where(pi => pi.Name == binder.Name).FirstOrDefault();

            // Make sure this member isn't a property on the object yet
            if (propertyInfo == null)
            {
                // look in the additional items collection for it
                if (_dynamicMembers.Keys.Contains(binder.Name))
                {
                    // return the dynamic item
                    result = _dynamicMembers[binder.Name];
                    return true;
                }
            }
            else
            {
                // get it from the contained object
                if (_containedObject != null)
                {
                    result = propertyInfo.GetValue(_containedObject);
                    return true;
                }
            }
            return base.TryGetMember(binder, out result);
        }

        public object GetMember(string propName)
        {
            var binder = Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None,
                  propName, this.GetType(),
                  new List<CSharpArgumentInfo>{
                       CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)});
            var callsite = CallSite<Func<CallSite, object, object>>.Create(binder);

            return callsite.Target(callsite, this);
        }

        public void SetMember(string propName, object val)
        {
            var binder = Microsoft.CSharp.RuntimeBinder.Binder.SetMember(CSharpBinderFlags.None,
                   propName, this.GetType(),
                   new List<CSharpArgumentInfo>{
                       CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                       CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)});
            var callsite = CallSite<Func<CallSite, object, object, object>>.Create(binder);

            callsite.Target(callsite, this, val);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var propertyInfo = _propertyInfos.Where(pi => pi.Name == binder.Name).FirstOrDefault();
            // Make sure this member isn't a property on the object yet
            if (propertyInfo == null)
            {
                // look in the additional items collection for it
                if (_dynamicMembers.Keys.Contains(binder.Name))
                {
                    // set the dynamic item
                    _dynamicMembers[binder.Name] = value;
                    return true;
                }
                else
                {
                    _dynamicMembers.Add(binder.Name, value);
                    return true;
                }
            }
            else
            {
                // put it in the contained object
                if (_containedObject != null)
                {
                    propertyInfo.SetValue(_containedObject, value);
                    return true;
                }
            }
            return base.TrySetMember(binder, value);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var propInfo in _propertyInfos)
            {
                if (_containedObject != null)
                    builder.AppendFormat("{0}:{1}{2}", propInfo.Name, propInfo.GetValue(_containedObject), Environment.NewLine);
                else
                    builder.AppendFormat("{0}:{1}{2}", propInfo.Name, propInfo.GetValue(this), Environment.NewLine);
            }
            foreach (var addlItem in _dynamicMembers)
            {
                // exclude methods that are added from the description
                Type itemType = addlItem.Value.GetType();
                Type genericType = itemType.IsGenericType ? itemType.GetGenericTypeDefinition() : null;
                if (genericType != null)
                {
                    if (genericType != typeof(Func<>) &&
                        genericType != typeof(Action<>))
                        builder.AppendFormat("{0}:{1}{2}", addlItem.Key, addlItem.Value, Environment.NewLine);
                }
                else
                    builder.AppendFormat("{0}:{1}{2}", addlItem.Key, addlItem.Value, Environment.NewLine);
            }
            return builder.ToString();
        }

    }
}
