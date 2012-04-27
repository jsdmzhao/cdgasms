using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Proxy;

namespace PoliceSMS.Web.Serializable
{
    public class NHibernateProxyJsonConverter : JsonConverter
    {
        public ISession Session { get; set; }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (NHibernateUtil.IsInitialized(value))
            {

                serializer.Serialize(writer, value);
            }
            else
            {
                serializer.Serialize(writer, null);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            bool b = typeof(INHibernateProxy).IsAssignableFrom(objectType);

            return b;
        }
    }
}