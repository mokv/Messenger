using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(Messenger.WebApi.Startup))]

namespace Messenger.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var hubConfiguration = new HubConfiguration { EnableDetailedErrors = true };
            app.MapSignalR(hubConfiguration);
        }
    }
}
