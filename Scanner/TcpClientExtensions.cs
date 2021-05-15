using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Scanner
{
    public static class TcpClientExtensions
    {
        public static Task ConnectWithTimeout(this TcpClient tcpClient, IPAddress ipAddr, int port, int timeout = 3000)
        {
            var connectTask = tcpClient.ConnectAsync(ipAddr, port);
            Task.WaitAny(connectTask, Task.Delay(timeout));
            return connectTask;
        }
    }
}