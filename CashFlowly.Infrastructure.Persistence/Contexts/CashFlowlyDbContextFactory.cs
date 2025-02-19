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

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new CashFlowlyDbContext(optionsBuilder.Options);
        }
    }
}
