using System;
using log4net.Core;
using PXR.Logging.Log4Net1213;
using SomeService.Web.Support;
using SomeService.Web.Web;
using LoggerManager = PXR.Logging.LoggerManager;

namespace SomeService.Web
{
    public static class Program
    {
        public static int Main()
        {
            if (Environment.UserInteractive)
                LoggerManager.SetFactoryResolver(() => new Log4NetLoggerFactory(Init.ColoredConsole(Level.Debug)));

            var ip = Network.LocalIp();
            var port = Network.AvailablePort();

            using (new WebNode().Start(new Uri($"http://{ip}:{port}")))
            {
                string cmd;
                while ((cmd = Console.ReadLine()) != null)
                {
                    if ("q".Equals(cmd))
                        break;
                }
            }

            LoggerManager.Reset();
            return 0;
        }
    }
}
