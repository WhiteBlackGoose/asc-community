using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ascsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://*:64321");
                    webBuilder.UseKestrel(options =>
                    {
                        options.Listen(IPAddress.Loopback, 443);
                        options.Listen(IPAddress.Loopback, 64320, listenOptions => {
                            listenOptions.UseHttps("", "testcert");
                        });
                    }
                    );
                });
    }
}
