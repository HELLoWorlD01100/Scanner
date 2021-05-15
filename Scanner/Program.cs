using System.Linq;
using System.Net;
using System.Net.Sockets;
using CommandLine;

namespace Scanner
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new TcpListener(2).Start();
            Parser.Default.ParseArguments<AppSettings>(args).WithParsed(appSettings =>
            {
                if (!IPAddress.TryParse(appSettings.IpAddr, out var ip))
                    ErrorHandler.ThrowError("ArgumentException: Ip has wrong format");
                
                if (appSettings.StartPort > appSettings.EndPort)
                    ErrorHandler.ThrowError("ArgumentException: Start port should be less then End port");
                
                SequentialScanner.Scan(ip,
                    Enumerable.Range(appSettings.StartPort, appSettings.EndPort - appSettings.StartPort));
            });
        }
    }
    
    public class AppSettings
    {
        [Option(shortName:'i', longName:"ipAddr", Required = true, HelpText = "Ip Address")]
        public string IpAddr { get; set; }
        
        [Option(shortName:'s', longName:"start", Required = true, HelpText = "Start Port", Default = 0)]
        public int StartPort { get; set; }
        
        [Option(shortName:'e', longName:"end", Required = true, HelpText = "End Port", Default = 1)]
        public int EndPort { get; set; }
        
    }
}