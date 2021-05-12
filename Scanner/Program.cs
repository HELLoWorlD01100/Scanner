using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using McMaster.Extensions.CommandLineUtils;
using ResultOf;

namespace Scanner
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 3)
                ErrorHandler.ThrowError($"Where is args...");
            if (!IPAddress.TryParse(args[0], out var ipAddr))
                ErrorHandler.ThrowError($"Ip wrong");
            if (!int.TryParse(args[1], out var sPort))
                ErrorHandler.ThrowError($"Start port wrong");
                
            if (!int.TryParse(args[2], out var ePort))
                ErrorHandler.ThrowError($"End port wrong");

            new TcpListener(IPAddress.Loopback, 2).Start();
            new TcpListener(IPAddress.Loopback, 3).Start();
            SequentialScanner.Scan(ipAddr,
                Enumerable.Range(sPort, ePort - sPort));

            // var e = new SequentialScanner();
            // var s = new TcpListener(IPAddress.Loopback, 5);
            // s.Start();
            // SequentialScanner.Scan(IPAddress.Loopback, Enumerable.Range(1, 10));
        }
    }

    public class AppSettings
    {
        public IPAddress IpAddr { get; private set; } = IPAddress.Loopback;
        public int StartPort { get; private set; }
        public int EndPort { get; private set; } = 1;

        public AppSettings WithIpAddr(IPAddress ipAddr)
        {
            return new()
            {
                IpAddr = ipAddr,
                StartPort = StartPort,
                EndPort = EndPort
            };
        }

        public AppSettings WithStartPort(int startPort)
        {
            return new()
            {
                IpAddr = IpAddr,
                StartPort = startPort,
                EndPort = EndPort
            };
        }

        public AppSettings WithEndPort(int endPort)
        {
            return new()
            {
                IpAddr = IpAddr,
                StartPort = StartPort,
                EndPort = endPort
            };
        }
    }

    public static class AppSettingsExtensions
    {
        public static void ConfigureInput(this AppSettings appSettings, CommandLineApplication app)
        {
            var ipAddr = app.Option("-i|--ipaddr <IPADDR>", "Ip address", CommandOptionType.SingleValue);
            var startPort = app.Option("-s|--start <SPORT>", "Start port", CommandOptionType.SingleValue);
            var endPort = app.Option("-e|--end <EPORT>", "End port", CommandOptionType.SingleValue);
            
            // return Result.Ok(appSettings)
            //     .Then(x => appSettings.SetUpIpAddr(ipAddr.Value()))
            //     .Then(x => appSettings.SetUpStartPort(startPort.Value()))
            //     .Then(x => appSettings.SetUpEndPort(endPort.Value()))
            //     .OnFail(ErrorHandler.ThrowError);
        }

        public static Result<AppSettings> SetUpIpAddr(this AppSettings appSettings, string input)
        {
            if (input is null)
                return appSettings;

            return IPAddress.TryParse(input, out var ipAddr)
                ? Result.Ok(appSettings.WithIpAddr(ipAddr))
                : Result.Fail<AppSettings>($"ArgumentException: Wrong IP address{input}");
        }

        private static Result<AppSettings> SetUpStartPort(this AppSettings appSettings, string input)
        {
            if (input is null)
                return appSettings;

            return int.TryParse(input, out var startPort) && startPort >= 0
                ? Result.Ok(appSettings.WithStartPort(startPort))
                : Result.Fail<AppSettings>($"ArgumentException: Wrong start port {input}");
        }

        private static Result<AppSettings> SetUpEndPort(this AppSettings appSettings, string input)
        {
            if (input is null)
                return appSettings;

            return int.TryParse(input, out var endPort) && endPort >= 0 && endPort > appSettings.StartPort
                ? Result.Ok(appSettings.WithEndPort(endPort))
                : Result.Fail<AppSettings>($"ArgumentException: Wrong end port {input}");
        }
    }
}