using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using LuxaforService.BusyChecks;

namespace LuxaforService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient<LuxaforClient>(client => client.BaseAddress = new Uri("https://api.luxafor.com"));
                    services.AddSingleton<IBusyCheck, ZoomBusyCheck>();
                    services.AddHostedService<Worker>();
                });
    }
}
