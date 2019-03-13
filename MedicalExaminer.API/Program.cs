using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MedicalExaminer.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            // Listen on any address so we can accept localhost and host.docker.internal from the python front end.
            // Not using https for now until we resolve certificates.
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://0.0.0.0:37449/");
        }
    }
}