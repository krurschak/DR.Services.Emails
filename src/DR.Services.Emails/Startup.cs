using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using MassTransit;
using DR.Packages.MassTransit;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using DR.Services.Emails.Data;
using Microsoft.EntityFrameworkCore;

namespace DR.Services.Emails
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add health checks
            services.AddHealthChecks();
            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(2);
                options.Predicate = (check) => check.Tags.Contains("ready");
            });

            // Configure Options
            services
                .AddOptions()
                .Configure<EmailSettingsOptions>(Configuration.GetSection("EmailSettings"));

            // Add Database
            services
                .AddDbContext<DefaultContext>(config =>
                {
                    config.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
                })
                .AddScoped<Data.UnitOfWork.IUnitOfWork, Data.UnitOfWork.EFUnitOfWork>()
                .AddScoped(typeof(Data.Repositories.IRepository<>), typeof(Data.Repositories.Repository<>));

            // Register MassTransit and Subscriptions
            services
                .AddMassTransit(x =>
                {
                    x.AddConsumers(Assembly.GetExecutingAssembly());
                    x.UseRabbitMq(Configuration.GetConnectionString("RabbitMq"), Assembly.GetExecutingAssembly());
                })
                .AddMassTransitHostedService();

            // Register Controllers
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // update database on application start
            using (var scope = app.ApplicationServices.CreateScope())
            {
                using var context = scope.ServiceProvider.GetService<DefaultContext>();
                context.Database.Migrate();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("ready"),
                });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions());

                endpoints.MapControllers();
            });
        }
    }
}
