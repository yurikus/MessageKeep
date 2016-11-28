using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using MessageKeep.Types;

namespace MessageKeep.Controllers
{
    [RoutePrefix("channels")]
    public class ChannelController : ApiController
    {
        readonly IServiceConfig m_cfg;

        public ChannelController(IServiceConfig cfg_)
        {
            m_cfg = cfg_;
        }
    }
}
