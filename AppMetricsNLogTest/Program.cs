using App.Metrics;
using App.Metrics.AspNetCore;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace AppMetricsNLogTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var metrics = AppMetrics.CreateDefaultBuilder()
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                .ConfigureMetrics(metrics)
                .UseMetrics()
                .UseMetricsWebTracking()
                .ConfigureLogging(
                    (context, builder) =>
                    {
                        builder.ClearProviders();
                        builder.AddConfiguration(context.Configuration.GetSection("Logging"));
                    })
                .UseStartup<Startup>()
                .UseNLog();
        }
    }
}