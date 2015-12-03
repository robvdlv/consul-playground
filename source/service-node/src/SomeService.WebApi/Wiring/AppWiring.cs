using System;
using ManagedStartStop;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using PXR.Configuration;
using PXR.Logging;
using SomeService.Web.Support;

namespace SomeService.Web.Wiring
{
    public class AppWiring : DefaultNancyBootstrapper
    {
        readonly IConfigurationReader _config;

        public AppWiring(IConfigurationReader config)
        {
            StaticConfiguration.DisableErrorTraces = false;

            _config = config;
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer ioc)
        {
            var managed = new ManyManaged()
                .UseListeners(new LoggingManagedListener("Api"));

            ioc.Register(new ConsulRegistrar(_config));
            ioc.Register(managed, "WIRING"); // allows lookup upon 'ApplicationStartup'
            ioc.Register<IDisposable>(managed); // make sure managed components shuts down upon Nancy shutdown
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            container.Resolve<ManyManaged>("WIRING").Start();
            container.Resolve<ConsulRegistrar>().Register();
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            // log any incoming request uri, so we know got hit
            pipelines.BeforeRequest.AddItemToEndOfPipeline(nancyContext =>
            {
                LoggerManager.GetLogger("Nancy").Debug("Incoming [{0}] @ [{1}]", nancyContext.Request.Method, nancyContext.Request.Url);
                return null;
            });

            pipelines.OnError.AddItemToEndOfPipeline((nancyContext, exception) =>
            {
                LoggerManager.GetLogger("Nancy").Error(exception);
                // write stacktrace as response in case of an exception, useful for development
                StaticConfiguration.DisableErrorTraces = false;
                return null;
            });
        }
    }
}
