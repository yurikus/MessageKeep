using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using Newtonsoft.Json;

using MessageKeep;
using MessageKeep.Types;

namespace MessageKeep.Controllers
{
    [RoutePrefix("mk/users"), ValidateModel]
    public class UserController : ApiController
    {
        readonly IBackStore m_store;

        public UserController(IBackStore store_)
        {
            m_store = store_;
        }

        [HttpGet, Route("")]
        public HttpResponseMessage UserList()
        {
            return this.JsonStream(jtw =>
            {
                jtw.WriteStartArray();

                foreach (var user in m_store.Users)
                    jtw.WriteValue(user);

                jtw.WriteEnd();
            });
        }

        [HttpGet, Route("{username_}")]
        public IHttpActionResult UserSummary(string username_)
        {
            return Json(new
            {
                username = username_,
                channels = m_store.UserChannels(username_)
            });
        }

        [HttpGet, Route("{username_}/messages")]
        public HttpResponseMessage UserMessageList(string username_)
        {
            return this.JsonStream(jtw =>
            {
                jtw.WriteStartArray();

                foreach (var msg in m_store.UserMessages(username_))
                    msg.ToJson(jtw);

                jtw.WriteEnd();
            });
        }

        [HttpGet, Route("{username_}/messages/since/{startDate_}")]
        public HttpResponseMessage UserMessageList(string username_, DateTime startDate_)
        {
            return this.JsonStream(jtw =>
            {
                jtw.WriteStartArray();

                foreach (var msg in m_store.UserMessagesSince(username_, startDate_))
                    msg.ToJson(jtw);

                jtw.WriteEnd();
            });
        }

        [HttpGet, Route("{sender_}/messages/to/{recipient_}")]
        public HttpResponseMessage DirectMessagesSenderToRecipient(string sender_, string recipient_)
        {
            return this.JsonStream(jtw =>
            {
                jtw.WriteStartArray();

                foreach (var msg in m_store.DirectMessages(sender_, recipient_))
                    msg.ToJson(jtw);

                jtw.WriteEnd();
            });
        }

        [HttpPut, Route("{username_}/channel/{channel_}")]
        public IHttpActionResult Subscribe(string username_, string channel_)
        {
            return Ok(m_store.Subscribe(username_, channel_));
        }

        [HttpDelete, Route("{username_}/channel/{channel_}")]
        public IHttpActionResult UnSubscribe(string username_, string channel_)
        {
            return Ok(m_store.UnSubscribe(username_, channel_));
        }

        [HttpPost, Route("{sender_}/messages/to/{recipient_}")]
        public IHttpActionResult PushDirect(string sender_, string recipient_, [FromBody] string content_)
        {
            return Ok(m_store.PushDirect(sender_, recipient_, content_));
        }

        [HttpPost, Route("{sender_}/channel/{channel_}")]
        public IHttpActionResult PushBroadcast(string sender_, string channel_, [FromBody] string content_)
        {
            return Ok(m_store.PushBroadcast(sender_, channel_, content_));
        }
    }
}
