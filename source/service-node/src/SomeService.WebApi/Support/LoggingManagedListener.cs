using ManagedStartStop;
using PXR.Logging;

namespace SomeService.Web.Support
{
    public class LoggingManagedListener : AdhocManagedListener
    {
        public LoggingManagedListener(string loggerName)
        {
            var log = LoggerManager.GetLogger(loggerName);

            OnStarting = m => log.Debug("Starting {0}", m);
            OnStarted = m => log.Debug("Started {0}", m);
            OnStartFailed = (m, e) => log.Fatal(e, "Failed to start {0}", m);
            OnStopping = m => log.Debug("Stopping {0}", m);
            OnStopped = m => log.Debug("Stopped {0}", m);
            OnStopFailed = (m, e) => log.Fatal(e, "Failed to stop {0}", m);
        }
    }
}
