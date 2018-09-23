using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace DoorWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

            if (args.Length == 1 && int.TryParse(args[0], out var port))
            {
                builder.UseUrls($"http://*:{port}");
            }

            return builder;
        }
    }
}
