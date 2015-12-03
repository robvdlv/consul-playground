using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SomeService.Web.Support
{
    internal static class Network
    {
        internal static string LocalIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var localIp = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            if (localIp == null)
                throw new Exception("Failed to determine local ip address");
            return localIp.ToString();
        }

        internal static int AvailablePort(int portStart = 1000, int portEnd = 9999)
        {
            if (portEnd < portStart) throw new InvalidOperationException("endport must be greater than startport");

            var properties = IPGlobalProperties.GetIPGlobalProperties();
            var tcpEndPoints = properties.GetActiveTcpListeners();

            var usedPorts = tcpEndPoints.Select(p => p.Port).ToList();
            var unusedPort = 0;

            for (var port = portStart; port < portEnd; port++)
            {
                if (!usedPorts.Contains(port))
                {
                    unusedPort = port;
                    break;
                }
            }
            return unusedPort;
        }
    }
}