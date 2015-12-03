using System;
using Microsoft.Owin.Hosting;
using Nancy;
using Nancy.Owin;
using Owin;
using PXR.Configuration;
using PXR.Logging;
using SomeService.Web.Support;
using SomeService.Web.Wiring;

namespace SomeService.Web.Web
{
    /// <summary>
    /// The process that hosts dataset creation. Exposes a REST API over http to manage dataset creation runs.
    /// <para>
    /// As such, can be <see cref="Start">started</see> and <see cref="Stop">stopped</see>.
    /// </para>
    /// </summary>
    public class WebNode
    {
        static readonly Version AssemblyVersion = typeof(WebNode).Assembly.GetName().Version;

        readonly ILogger _log = LoggerManager.GetLogger<WebNode>();

        public IDisposable Start(Uri uri)
        {
            try
            {
                _log.Info("Starting web node version [{0}] @ {1}", AssemblyVersion, uri);
                var host = WebApp.Start(uri.ToString(), ConfigurationWith(uri));
                _log.Info("Started");
                return new AdhocDisposable(() =>
                {
                    try
                    {
                        _log.Info("Stopping");
                        host.Dispose();
                        host = null;
                    }
                    catch (Exception ex)
                    {
                        _log.Fatal(Exceptions.UnwrapIfTargetInvocationException(ex), "Failed to stop");
                        throw;
                    }
                });
            }
            catch (Exception ex)
            {
                _log.Fatal(Exceptions.UnwrapIfTargetInvocationException(ex), "Failed to start");
                throw;
            }
        }

        Action<IAppBuilder> ConfigurationWith(Uri uri)
        {
            return app =>
            {
                var config = new FirstMatchConfigurationReader(
                    new StubConfigurationReader()
                        .Add("ip", uri.IdnHost)
                        .Add("port", uri.Port),
                    new EnvironmentVariableConfigurationReader
                    {
                        OnLoadConfigurationFailed = Console.Error.WriteLine,
                        OnUsingFallback = Console.Error.WriteLine,
                    },
                    new AppSettingConfigurationReader()
                );
                app.UseNancy(new NancyOptions {Bootstrapper = new AppWiring(config)});
            };
        }

        class AdhocDisposable : IDisposable
        {
            readonly Action _dispose;

            public AdhocDisposable(Action dispose)
            {
                _dispose = dispose;
            }

            public void Dispose()
            {
                _dispose();
            }
        }
    }
}
