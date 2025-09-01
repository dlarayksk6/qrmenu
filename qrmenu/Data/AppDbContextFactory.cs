using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace qrmenu.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath("D:\\qrmenu\\qrmenu")
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var serverVersion = new MySqlServerVersion(new Version(8, 0, 36));
            optionsBuilder.UseMySql(connectionString, serverVersion);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
