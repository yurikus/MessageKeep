using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using Owin;
using Microsoft.Owin.Hosting;

using DryIoc;
using DryIoc.Owin;
using DryIoc.WebApi;

using MessageKeep.Types;
using MessageKeep.Core;

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

            Console.WriteLine("Listening on " + url);
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
            di.RegisterInstance<IBackStore>(new BackStore(), Reuse.Singleton);

            di.WithWebApi(config);
            app_.UseDryIocOwinMiddleware(di);

            app_.UseWebApi(config);
        }
    }
}
