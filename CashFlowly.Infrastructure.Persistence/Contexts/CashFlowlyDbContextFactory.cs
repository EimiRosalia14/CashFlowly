using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CashFlowly.Infrastructure.Persistence.Contexts
{
    public class CashFlowlyDbContextFactory : IDesignTimeDbContextFactory<CashFlowlyDbContext>
    {
        public CashFlowlyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CashFlowlyDbContext>();

            // Configura la cadena de conexión
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../CashFlowly.API"))
                .AddJsonFile("appsettings.json")
                .Build();

            var connection = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
                ? configuration.GetConnectionString("DefaultConnection") 
                : Environment.GetEnvironmentVariable("PRODUCTION_DB_CONNECTION");
            optionsBuilder.UseSqlServer(connection);

            return new CashFlowlyDbContext(optionsBuilder.Options);
        }
    }
}
