using System;
using PXR.Configuration;

namespace SomeService.Web.Wiring
{
    public class ConsulRegistrar : IDisposable
    {
        readonly IConfigurationReader _config;
        readonly Consul.Consul _consul;

        public ConsulRegistrar(IConfigurationReader config)
        {
            _config = config;
            _consul = new Consul.Consul();
        }

        public void Register()
        {
            var ipAddress = _config.Get<string>("ip");
            var port = _config.Get<int>("port");

            var id = Id(_config);

            _consul.RegisterService(id, ipAddress, port);
        }

        public void Unregister()
        {
            var id = Id(_config);

            _consul.Unregister(id);
        }

        public void Dispose()
        {
            Unregister();
        }

        string Id(IConfigurationReader config)
        {
            return "someservice-"+ config.Get<string>("port");
        }
    }
}