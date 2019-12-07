using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ascsite.Core;
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

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            string password = File.ReadAllText(@"D:\main\ASC-community\cert\password.txt", Encoding.UTF8);

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(options =>
                    {
                        options.Listen(IPAddress.Loopback, 64321, listenOptions =>
                        {
                            listenOptions.UseHttps(@"D:\main\ASC-community\cert\cert.pfx", password);
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(
                        "http://*:64321",
                        "https://*:443");

                    webBuilder.UseWebRoot(Const.PATH_WEBROOT);
                });
        }
    }
}
