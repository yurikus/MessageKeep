using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using Owin;
using Microsoft.Owin.Hosting;

using DryIoc.Owin;
using DryIoc.WebApi;

namespace MessageKeep
{
    public class AppCore
    {
        IDisposable m_svc;
        CmdLineOptions m_opts;

        public void Start(CmdLineOptions opts_)
        {
            m_opts = opts_;

            var url = "http://" + (m_opts.IsPublic ? "+" : "localhost") + ":" + m_opts.Port;
            m_svc = WebApp.Start(url, Configure);
        }

        public void Stop()
        {
            m_svc?.Dispose();
        }

        void Configure(IAppBuilder app_)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            var di = new DryIoc.Container();
            di.WithWebApi(config);
            app_.UseDryIocOwinMiddleware(di);

            app_.UseWebApi(config);
        }
    }
}
