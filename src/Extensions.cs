using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using Newtonsoft.Json;

using MessageKeep.Types;
using MessageKeep.Core;

namespace MessageKeep
{
    public static class JsonTextWriterExtensions
    {
        public static JsonTextWriter KeyValue<T>(this JsonTextWriter jtw_, string key_, T data_)
        {
            jtw_.WritePropertyName(key_);
            jtw_.WriteValue(data_);
            return jtw_;
        }
    }

    public static class ApiControllerExtensions
    {
        public static HttpResponseMessage JsonStream(this ApiController ac_, Action<JsonTextWriter> generator_)
        {
            return new HttpResponseMessage()
            {
                Content = new PushStreamContent(
                    (stream_, content_, context_) =>
                    {
                        using (var sw = new StreamWriter(stream_))
                        using (var jtw = new JsonTextWriter(sw))
                        {
                            generator_(jtw);
                        }
                    }, "application/json")
            };
        }
    }

    public static class MessageExtensions
    {
        public static void ToJson(this IMessage msg_, JsonTextWriter jtw_)
        {
            jtw_.WriteStartObject();
            jtw_.KeyValue("id", msg_.Id);
            jtw_.KeyValue("delivered_utc", msg_.DeliveredUtc);
            jtw_.KeyValue("is_direct", msg_.IsDirect);
            jtw_.KeyValue("sender", msg_.Sender);
            jtw_.KeyValue(msg_.IsDirect ? "recipient" : "channel", msg_.Recipient);
            jtw_.KeyValue("content", msg_.Content);
            jtw_.WriteEnd();
        }
    }
}

