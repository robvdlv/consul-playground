using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using NetMQ;

namespace ConsulConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Starting....{args[0]}");

            //            PutValue(client);
            //            Register(client);

            var context = NetMQContext.Create();

            Task.Factory.StartNew(() => StartServer(context));
            Thread.Sleep(4444);
            StartClient(context);

            Thread.Sleep(3333);
            Console.WriteLine("THE END.");
            Console.ReadLine();
        }

        private static void StartClient(NetMQContext context)
        {
            Console.WriteLine("Starting Client.....");
            var _sendSocket = context.CreateDealerSocket();
            _sendSocket.Connect($"tcp://localhost:{GetAddress()}"); 

            _sendSocket.Send("TEST");
            _sendSocket.Send("QUIT");

        }

        private static void StartServer(NetMQContext context)
        {
            using (var serverSocket = context.CreateDealerSocket())
            {
                var port = 777;
                Register(port);
                serverSocket.Bind($"tcp://*:{port}");

                var quit = false;
                while (!quit) // && !_cancel.IsCancellationRequested)
                {
                    try
                    {

                        var payload = serverSocket.ReceiveString(Encoding.UTF8);

                        Console.WriteLine($"SERVER: {payload}");

                        quit = payload.Equals("QUIT");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex);
                    }
                }
            }
        }

        private static void Register(int port)
        {
            var client = new Client(new ConsulClientConfiguration {Address = "172.17.0.1:8500"});
            var reg = new AgentServiceRegistration { ID = "Svc.4", Name = "ZeroSvc", Port = port };
            Console.WriteLine("REGISTERING SERVICE.");
            var result = client.Agent.ServiceRegister(reg);
            Console.WriteLine("SERVICE REGISTERED");
            Console.WriteLine($"Writeresult: {result}");

        }

        private static int GetAddress()
        {
            var client = new Client(new ConsulClientConfiguration { Address = "172.17.0.1:8500" });

            var z = client.Catalog.Service("ZeroSvc");
            return z.Response.First().ServicePort;

        }

        private static void PutValue(Client client)
        {
            var putpair = new KVPair("key") { Value = Encoding.UTF8.GetBytes("TestValue") };
            var putattempt = client.KV.Put(putpair);
            if (putattempt.Response)
            {
                var getPair = client.KV.Get("key");
                Console.WriteLine(Encoding.UTF8.GetString(getPair.Response.Value, 0, getPair.Response.Value.Length));
            }
        }
    }
}
