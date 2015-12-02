using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;

namespace SomeService
{
    class Program
    {
        static void Main(string[] args)
        {
            HelloConsul();
        }

        static void HelloConsul()
        {
//            var someWebServiceReg = new AgentServiceRegistration
//            {
//                ID = "some-web-service",
//                Name = "web service @ port 9004",
//                Address = "http://localhost:9004",
//                Check = new AgentServiceCheck
//                {
//                    // relevant to checks
//                    HTTP = "http://localhost:9004/status",
//                    Timeout = TimeSpan.FromSeconds(1),
//                    Interval = TimeSpan.FromSeconds(10),
//                    Status = CheckStatus.Passing,
//                    // how long is the service considered 'alive' without receiving a check response?
//                    TTL = TimeSpan.FromMinutes(1)
//                },
//
//            };
//            var registrationResult = client.Agent.ServiceRegister(someWebServiceReg);

            var client = new Client();

            var queryResult = client.Catalog.Services();
            foreach (var services in queryResult.Response)
            {
                var serviceName = services.Key;
                Console.WriteLine(serviceName);
                var serviceDetails = client.Catalog.Service(serviceName);
                Array.ForEach(serviceDetails.Response, c =>
                {
                    Console.WriteLine("node [{0}]", c.Node);
                    Console.WriteLine("node address [{0}]", c.Address);
                    Console.WriteLine("service id [{0}]", c.ServiceID);
                    Console.WriteLine("service address [{0}]", c.ServiceAddress);
                    Console.WriteLine("service name [{0}]", c.ServiceName);
                    Console.WriteLine("service port [{0}]", c.ServicePort);
                    Console.WriteLine("service tags [{0}]", c.ServiceTags == null ? "" : string.Join(", ", c.ServiceTags));
                });
            }

//            var putPair = new KVPair("hello")
//            {
//                Value = Encoding.UTF8.GetBytes("Hello Consul")
//            };
//
//            var putAttempt = client.KV.Put(putPair);
//
//            if (putAttempt.Response)
//            {
//                var getPair = client.KV.Get("hello");
//                return Encoding.UTF8.GetString(getPair.Response.Value, 0, getPair.Response.Value.Length);
//            }
//
//            return "";
        }
    }
}
