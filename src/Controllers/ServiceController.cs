using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using MessageKeep.Types;

namespace MessageKeep.Controllers
{
    [RoutePrefix("service")]
    public class ServiceController : ApiController
    {
        readonly IServiceConfig m_cfg;

        public ServiceController(IServiceConfig cfg_)
        {
            m_cfg = cfg_;
        }

        [HttpGet, Route("")]
        public IHttpActionResult ShowStatus()
        {
            return Json(new
            {
                root = true
            });
        }

        [HttpGet, Route("config")]
        public IHttpActionResult ShowConfig()
        {
            return Json(m_cfg);
        }
    }
}
