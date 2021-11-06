using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DataLayer
{
    public class EfDbContextFactory : IDesignTimeDbContextFactory<EfDbContext>
    {
        public EfDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetFullPath(@"..\WebApi"))
                .AddJsonFile("appsettings.Development.json", true)
                .Build();

            DbContextOptionsBuilder<EfDbContext> optionsBuilder = new DbContextOptionsBuilder<EfDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("localhostTeam"));
            return new EfDbContext(optionsBuilder.Options);
        }
    }
}