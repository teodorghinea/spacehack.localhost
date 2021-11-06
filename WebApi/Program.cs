using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Console.WriteLine("App starting...");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(webBuilder =>
                       {
                       webBuilder.UseStartup<Startup>();
                       });
        }
    }
}
