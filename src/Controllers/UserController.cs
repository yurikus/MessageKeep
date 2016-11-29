using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using MessageKeep.Types;

namespace MessageKeep.Controllers
{
    [RoutePrefix("users")]
    public class UserController : ApiController
    {
        readonly IServiceConfig m_cfg;
        readonly IBackStore m_store;

        public UserController(IServiceConfig cfg_, IBackStore store_)
        {
            m_cfg = cfg_;
            m_store = store_;
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

        [HttpPost, Route("{username_}/messages/to/{recipient_}")]
        public IHttpActionResult PushDirect(string username_, string recipient_, [FromBody] string content_)
        {
            return Ok(m_store.PushDirect(recipient_, username_, content_));
        }

        [HttpPost, Route("{username_}/channel/{channel_}")]
        public IHttpActionResult PushBroadcast(string username_, string channel_, [FromBody] string content_)
        {
            return Ok(m_store.PushBroadcast(username_, channel_, content_));
        }
    }
}
