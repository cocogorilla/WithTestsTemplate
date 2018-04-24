using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Dummy.Core;
using Dummy.Infrastructure;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Dummy.Api.Startup))]

namespace Dummy.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // integration of owin into IIS and ASP.NET pipeline
            BootStrap.Configure(app, GlobalConfiguration.Configuration);
        }
    }
    public class SelfHostStartup
    {
        public void Configuration(IAppBuilder app)
        {
            // plain jane new http config for self hosted integration tests
            BootStrap.Configure(app, new HttpConfiguration());
        }
    }

    public static class BootStrap
    {
        public static void Configure(IAppBuilder app, HttpConfiguration config)
        {
            config.Services.Replace(typeof(IHttpControllerActivator), new CompositionRoot());
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }
    }

    public class CompositionRoot : IHttpControllerActivator
    {
        private readonly AppConfig config = new AppConfig
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["AppConnection"].ConnectionString
        };
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            if (controllerType == typeof(DummyController))
            {
                return new DummyController(
                    new DummyModelProcessor(),
                    new ModelRepo(config));
            }
            throw new InvalidOperationException();
        }
    }
}
