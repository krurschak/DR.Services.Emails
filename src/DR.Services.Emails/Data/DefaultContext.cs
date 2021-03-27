using DR.Services.Emails.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DR.Services.Emails.Data
{
    public class DefaultContext : DbContext
    {
        private readonly string defaultConnection;

        public DefaultContext()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json")
                .AddEnvironmentVariables();

            var config = configBuilder.Build();
            defaultConnection = config.GetConnectionString("DefaultConnection");
        }

        public DefaultContext(DbContextOptions<DefaultContext> options)
            : base(options)
        {
        }

        public DbSet<Email> Emails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(defaultConnection);
            }
        }
    }
}
