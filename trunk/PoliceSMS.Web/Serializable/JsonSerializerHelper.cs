using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace PoliceSMS.Web.Serializable
{
    public class JsonSerializerHelper
    {
        public static TEntity JsonToEntity<TEntity>(string jsonStr)
        {
            using (StringReader sw = new StringReader(jsonStr))
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                };
                using (JsonReader jw = new JsonTextReader(sw))
                {
                    TEntity entity = serializer.Deserialize<TEntity>(jw);

                    return entity;
                }

            }
        }

        public static object JsonToEntity(string json, string typeName)
        {
            Type t = Type.GetType(typeName);
            using (StringReader sw = new StringReader(json))
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                };
                using (JsonReader jw = new JsonTextReader(sw))
                {
                    object entity = serializer.Deserialize(jw, t);

                    return entity;
                }

            }

        }

        public static string EntityToJson(object obj)
        {
            return EntityToJson(obj, null);
        }

        public static string EntityToJson(object obj, string[] exceptMemberName)
        {
            //这里没有递归判断，可能存在bug，暂时这样
            //if (obj is IEnumerable)
            //{
            //    foreach (var i in (obj as IEnumerable))
            //        NhbSerialzableHelper.RemoveLazyProxyObj(i);
            //}
            //else
            //{
            //    NhbSerialzableHelper.RemoveLazyProxyObj(obj);
            //}
            using (StringWriter sw = new StringWriter(System.Globalization.CultureInfo.InvariantCulture))
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new NHibernateContractResolver(exceptMemberName),
                };

                //serializer.Converters.Add(new NHibernateProxyJsonConverter());

                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;
                    try
                    {
                        serializer.Serialize(jw, obj);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return sw.ToString();
            }
        }
    }
}