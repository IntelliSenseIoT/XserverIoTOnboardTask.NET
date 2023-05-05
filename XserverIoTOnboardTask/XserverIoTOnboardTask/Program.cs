using System.Runtime.InteropServices;

namespace XserverIoTOnboardTask
{
    public class Program
    {
        bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        bool IsLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();
                })
                .Build();

            host.Run();
        }
    }
}