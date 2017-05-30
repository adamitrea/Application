using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
//#if !DEBUG
//                .UseEnvironment("Publish")
//#endif
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .CaptureStartupErrors(true)
                .UseSetting("detailedErrors", "true")
                .Build();

            host.Run();
        }
    }
}
