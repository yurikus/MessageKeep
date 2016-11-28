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
        readonly IUserStore m_users;
        readonly IChannelStore m_channels;

        public UserController(IServiceConfig cfg_, IUserStore users_, IChannelStore channels_)
        {
            m_cfg = cfg_;
            m_users = users_;
            m_channels = channels_;
        }

        [HttpPut, Route("{username_}/channel/{channelTitle_}")]
        IHttpActionResult Subscribe(string username_, string channelTitle_)
        {
            

            return Ok();
        }

        [HttpDelete, Route("{username_}/channel/{channelTitle_}")]
        IHttpActionResult UnSubscribe(string username_, string channelTitle_)
        {
            return Ok();
        }
    }
}
