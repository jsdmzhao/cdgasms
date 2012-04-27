using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using PoliceSMS.Lib.Core;
using System.Collections;

namespace PoliceSMS.Web.Serializable
{
    internal class NhbSerialzableHelper
    {
        public static void RemoveLazyProxyObj(object obj)
        {
            Type t = obj.GetType();
            foreach (var p in t.GetProperties())
            {
                object value = p.GetValue(obj, null);
                bool flag = NHibernateUtil.IsInitialized(value);
                if (value != null)
                {
                    if (!flag)
                    {
                        if (value is IItem)
                        {
                            //这里用依赖注入，现在先不实现
                            //IItem iitem = ModelFactory.CreateModel(p.PropertyType) as IItem;
                            //iitem.Id = (value as IItem).Id;
                            p.SetValue(obj, null, null);
                        }
                        else
                        {
                            p.SetValue(obj, null, null);
                        }
                    }
                    else
                    {
                        if (value is IEnumerable)
                        {
                            foreach (var v in value as IEnumerable)
                            {
                                if (v is IItem)
                                {
                                    foreach (var p1 in v.GetType().GetProperties())
                                    {
                                        object value1 = p1.GetValue(v, null);
                                        bool flag1 = NHibernateUtil.IsInitialized(value1);
                                        if (!flag1 && value1 != null)
                                        {
                                            p1.SetValue(v, null, null);
                                        }
                                    }
                                }
                            }
                        }
                        else if (value is IItem)
                        {
                            foreach (var p1 in value.GetType().GetProperties())
                            {
                                object value1 = p1.GetValue(value, null);
                                bool flag1 = NHibernateUtil.IsInitialized(value1);
                                if (!flag1 && value1 != null)
                                {
                                    p1.SetValue(value, null, null);
                                }
                            }
                        }
                    }
                }
                //else
                //{
                //    if (p.PropertyType.GetInterfaces().Contains(typeof(IItem)))
                //        p.SetValue(obj, ModelFactory.CreateModel(p.PropertyType), null);
                //}
            }
        }

        public static void RemoveNotExistObj(object obj)
        {
            Type t = obj.GetType();
            foreach (var p in t.GetProperties())
            {
                object value = p.GetValue(obj, null);
                if (value != null && value is IItem && ((IItem)value).Id == 0)
                {
                    p.SetValue(obj, null, null);
                }
            }
        }
    }
}