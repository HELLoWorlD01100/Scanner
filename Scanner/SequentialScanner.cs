using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using NMAP;

namespace Scanner
{
    public class SequentialScanner
    {
        public static void Scan(IPAddress ip, IEnumerable<int> ports)
        {
            if (PingAddr(ip) != IPStatus.Success)
                return;

            foreach (var port in ports)
            {
                CheckPortTcp(ip, port);
            }
        }

        private static IPStatus PingAddr(IPAddress ipAddr, int timeout = 3000)
        {
            using var ping = new Ping();
            var status = ping.Send(ipAddr, timeout).Status;
            return status;
        }
        private static void CheckPortTcp(IPAddress ipAddr, int port, int timeout = 3000)
        {
            using var tcpClient = new TcpClient();
            Console.WriteLine($"Checking {ipAddr}:{port}");
            var connectTask = tcpClient.ConnectWithTimeout(ipAddr, port, timeout);
            var portStatus = connectTask.Status switch
            {
                TaskStatus.RanToCompletion => PortStatus.Open,
                TaskStatus.Faulted => PortStatus.Closed,
                _ => PortStatus.Filtered
            };
            Console.WriteLine($"Checked {ipAddr}:{port} - {portStatus}");
        }
    }
    
    
}