using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using SnowEmergency.Models;
using SnowEmergency.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SnowEmergency.DataLoader
{
    class Program
    {
        private static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            // Create service collection and configure our services
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File("logs/log-dataloader-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            serviceProvider.GetService<StartUp>().Run(args);
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            Configuration = LoadConfiguration();
            services.AddSingleton(Configuration);
            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(opt =>
                    opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IWorkerService, WorkerService>();
            services.AddTransient<StartUp>();
            return services;
        }

        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false,
                             reloadOnChange: true);
            return builder.Build();
        }
    }
}
