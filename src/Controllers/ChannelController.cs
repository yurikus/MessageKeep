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
        readonly IBackStore m_store;

        public ChannelController(IServiceConfig cfg_, IBackStore store_)
        {
            m_cfg = cfg_;
        }
    }
}
