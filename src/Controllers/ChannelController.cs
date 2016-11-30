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

namespace MessageKeep.Controllers
{
    [RoutePrefix("channels")]
    public class ChannelController : ApiController
    {
        readonly IBackStore m_store;

        public ChannelController(IBackStore store_)
        {
            m_store = store_;
        }

        [HttpGet, Route("")]
        public HttpResponseMessage ChannelList()
        {
            return this.JsonStream(jtw =>
            {
                jtw.WriteStartArray();

                foreach (var user in m_store.Channels)
                    jtw.WriteValue(user);

                jtw.WriteEnd();
            });
        }


        [HttpGet, Route("{channel_}/users")]
        public HttpResponseMessage ChannelUserList(string channel_)
        {
            return this.JsonStream(jtw =>
            {
                jtw.WriteStartArray();

                foreach (var user in m_store.ChannelUsers(channel_))
                    jtw.WriteValue(user);

                jtw.WriteEnd();
            });
        }

        [HttpGet, Route("{channel_}/messages")]
        public HttpResponseMessage ChannelMessageList(string channel_)
        {
            return this.JsonStream(jtw =>
            {
                jtw.WriteStartArray();

                foreach (var msg in m_store.ChannelMessages(channel_))
                    msg.ToJson(jtw);

                jtw.WriteEnd();
            });
        }
    }
}
