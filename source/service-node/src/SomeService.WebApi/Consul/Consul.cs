using System;
using Consul;
using PXR.Logging;

namespace SomeService.Web.Consul
{
    public class Consul
    {
        readonly ILogger _log = LoggerManager.GetLogger<Consul>();

        public void RegisterService(string id, string ipAddress, int port)
        {
            var reg = new AgentServiceRegistration
            {
                ID = id,
                Name = "ss-name-"+ port,
                Address = ipAddress,
                Port = port,
                Check = new AgentServiceCheck
                {
                    // relevant to checks
                    HTTP = "http://" + ipAddress + ":" + port +"/status",
                    Timeout = TimeSpan.FromSeconds(1),
                    Interval = TimeSpan.FromSeconds(5),
                    Status = CheckStatus.Passing,
                    // service is to invoke consul http api to let consul know its alive
                    //TTL = TimeSpan.FromMinutes(1)
                }
            };
            new Client().Agent.ServiceRegister(reg);
            _log.Info("Registered service [{0}] @ {1}:{2}", reg.ID, reg.Address, reg.Port);
        }

        public void Unregister(string id)
        {
            new Client().Agent.ServiceDeregister(id);
            _log.Info("unregistered service [{0}]", id);
        }
    }
}