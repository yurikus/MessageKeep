using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Http;

namespace MessageKeep.Controllers
{
    [RoutePrefix("service")]
    public class StatusController : ApiController
    {
        [HttpGet, Route("status")]
        public IHttpActionResult GetStatus()
        {
            return Json(new
            {
                status = "running"
            });
        }
    }
}
