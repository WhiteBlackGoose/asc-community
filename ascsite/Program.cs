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
            string password = File.ReadAllText(Const.CERTIFICATE_PATH + @"\password.txt", Encoding.UTF8);
            
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, 443, listenOptions =>
                        {
                            listenOptions.UseHttps(Const.CERTIFICATE_PATH + @"\cert.pfx", password);
                        }); // https
                    });
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseWebRoot(Const.PATH_WEBROOT);
                });
        }
    }
}
