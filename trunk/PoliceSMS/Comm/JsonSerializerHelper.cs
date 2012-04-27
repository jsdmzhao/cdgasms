using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Text;
using System.Json;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;

namespace PoliceSMS.Comm
{
    public static class JsonSerializerHelper
    {
        public static T JsonToEntity<T>(string json)
        {
            try
            {
                var mStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
                JsonValue items = JsonArray.Load(mStream);
                T entity = default(T);

                foreach (KeyValuePair<string, JsonValue> item in items)
                {

                    if (item.Key == "Data")
                    {
                        entity = item.Value == null ? default(T) : Newtonsoft.Json.JsonConvert.DeserializeObject<T>(item.Value.ToString());
                    }
                    else if (item.Key == "Message")
                    {
                        if (!string.IsNullOrEmpty(item.Value))
                        {
                            //ReturnMessage message = JsonToMessage(json);
                            Tools.ShowMessage(item.Value, "", false);
                        }
                    }
                }

                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string EntityToJson<T>(T Entity)
        {
            string s = JsonConvert.SerializeObject(Entity);

            return s;
        }

        public static string ObjectToJson(object obj)
        {
            using (StringWriter sw = new StringWriter(System.Globalization.CultureInfo.InvariantCulture))
            {
                JsonSerializer serializer = JsonSerializer.Create(
                new JsonSerializerSettings
                {
                    Converters = new JsonConverter[] { new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter() },
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                }
                );
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    serializer.Serialize(jw, obj);
                }
                return sw.ToString();
            }
        }

        public static IList<T> JsonToEntities<T>(string json, out int totalCount)
        {

            try
            {

                var mStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
                JsonValue items = JsonArray.Load(mStream);
                IList<T> entities = new List<T>();
                totalCount = 0;
                foreach (KeyValuePair<string, JsonValue> item in items)
                {
                    if (item.Key == "Data")
                    {
                        JsonArray array = (JsonArray)item.Value;

                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

                        foreach (JsonObject child in array)
                        {
                            using (MemoryStream jsonStream = new MemoryStream(Encoding.Unicode.GetBytes(child.ToString())))
                            {
                                T Entity = (T)serializer.ReadObject(jsonStream);
                                entities.Add(Entity);
                            }
                        }
                    }
                    else if (item.Key == "Datas")
                    {
                        JsonArray array = (JsonArray)item.Value;

                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

                        foreach (JsonArray child in array)
                        {
                            using (MemoryStream jsonStream = new MemoryStream(Encoding.Unicode.GetBytes(child.ToString())))
                            {
                                T Entity = (T)serializer.ReadObject(jsonStream);
                                entities.Add(Entity);
                            }
                        }
                    }
                    else if (item.Key == "Total")
                    {
                        totalCount = item.Value;
                    }
                    else if (item.Key == "Message")
                    {
                        if (!string.IsNullOrEmpty(item.Value))
                        {
                            //ReturnMessage message = JsonToMessage(json);
                            Tools.ShowMessage(item.Value, "", false);
                        }
                    }
                }

                return entities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ReturnMessage JsonToMessage(string messageStr)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ReturnMessage));
            using (MemoryStream jsonStream = new MemoryStream(Encoding.Unicode.GetBytes(messageStr)))
            {
                ReturnMessage message = (ReturnMessage)serializer.ReadObject(jsonStream);
                return message;
            }
        }
    }
}
