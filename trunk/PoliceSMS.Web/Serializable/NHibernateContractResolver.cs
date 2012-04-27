using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using NHibernate.Proxy;
using Newtonsoft.Json;

namespace PoliceSMS.Web.Serializable
{
    public class NHibernateContractResolver : DefaultContractResolver
    {
        private string[] exceptMemberName;//例外
        private static readonly MemberInfo[] NHibernateProxyInterfaceMembers = typeof(INHibernateProxy).GetMembers();
        public NHibernateContractResolver()
        {

        }
        public NHibernateContractResolver(string[] exceptMemberName)
        {
            this.exceptMemberName = exceptMemberName;
        }
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            try
            {

                var members = new List<PropertyInfo>(objectType.GetProperties());
                var actualMemberInfos = new List<MemberInfo>();

                //if (objectType.Name == "DefaultLazyInitializer")
                //{
                //    return actualMemberInfos;
                //}

                members.RemoveAll(memberInfo =>
                    //(IsMemberNames(memberInfo)) ||
                  (IsMemberPartOfNHibernateProxyInterface(memberInfo)) ||
                  (IsMemberDynamicProxyMixin(memberInfo)) ||
                  (IsMemberMarkedWithIgnoreAttribute(memberInfo, objectType)) ||
                  (IsInheritedIList(memberInfo)) ||
                  (IsInheritedEntity(memberInfo))
                    //IsMemberMarkedJsonPropertyAttribute(memberInfo,objectType) ||
                    //IsMemberDynamicProxyMixin(memberInfo)
                  );
                members = Distinct(members);

                foreach (var memberInfo in members)
                {
                    if (memberInfo.DeclaringType.BaseType != null)
                    {
                        var infos = memberInfo.DeclaringType.BaseType.GetMember(memberInfo.Name);
                        actualMemberInfos.Add(infos.Length == 0 ? memberInfo : infos[0]);
                    }
                    else
                    {
                        actualMemberInfos.Add(memberInfo);
                    }
                    //Debug.WriteLine(memberInfo.Name);
                }
                return actualMemberInfos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static bool IsMemberDynamicProxyMixin(PropertyInfo memberInfo)
        {
            return memberInfo.Name == "__interceptors";
        }
        private static bool IsMemberPartOfNHibernateProxyInterface(PropertyInfo memberInfo)
        {
            bool b = Array.Exists(NHibernateProxyInterfaceMembers, mi => memberInfo.Name == mi.Name);
            return b;
        }


        private static bool IsMemberMarkedWithIgnoreAttribute(PropertyInfo memberInfo, Type objectType)
        {
            var infos = typeof(INHibernateProxy).IsAssignableFrom(objectType) ?
              objectType.BaseType.GetMember(memberInfo.Name) :
              objectType.GetMember(memberInfo.Name);
            bool b = infos[0].GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Length > 0;
            return b;
        }

        private static bool IsMemberMarkedJsonPropertyAttribute(PropertyInfo memberInfo, Type objectType)
        {
            //var infos = typeof(INHibernateProxy).IsAssignableFrom(objectType) ?
            //  objectType.BaseType.GetMember(memberInfo.Name) :
            //  objectType.GetMember(memberInfo.Name);

            object[] attribs = memberInfo.GetCustomAttributes(true);

            foreach (object o in attribs)
            {
                if (o.GetType().Name == "JsonProperty")
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsExceptMember(PropertyInfo memberInfo)
        {
            if (exceptMemberName == null)
                return false;
            return Array.Exists(exceptMemberName, i => memberInfo.Name == i);
        }
        private bool IsInheritedIList(PropertyInfo memberInfo)
        {
            bool b = (memberInfo.PropertyType.Name == "IList`1" && !IsExceptMember(memberInfo));
            return b;
        }
        private bool IsInheritedEntity(PropertyInfo memberInfo)
        {
            bool b = (FindBaseType(memberInfo.PropertyType).Name == "Entity" && !IsExceptMember(memberInfo));
            return b;
        }
        private static Type FindBaseType(Type type)
        {
            if (!type.IsClass)
                return type;
            if (type.Name == "Entity" || type.Name == "Object")
            {
                return type;
            }
            return FindBaseType(type.BaseType);
        }
        private List<PropertyInfo> Distinct(List<PropertyInfo> list)
        {
            List<PropertyInfo> result = new List<PropertyInfo>();
            List<string> names = new List<string>();
            foreach (PropertyInfo p in list)
            {
                if (!names.Contains(p.Name))
                {
                    names.Add(p.Name);
                    result.Add(p);
                }
            }
            return result;
        }
    }
}