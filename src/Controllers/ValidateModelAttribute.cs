using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using MessageKeep.Types;

namespace MessageKeep.Controllers
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext ctx_)
        {
            if (!ctx_.ModelState.IsValid)
            {
                ctx_.Response = ctx_.Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    OpStatus.InvalidArguments,
                    "application/json");
                return;
            }
        }
    }
}
