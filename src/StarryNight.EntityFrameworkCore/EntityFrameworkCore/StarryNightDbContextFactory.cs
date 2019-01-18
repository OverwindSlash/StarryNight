using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using StarryNight.Configuration;
using StarryNight.Web;

namespace StarryNight.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class StarryNightDbContextFactory : IDesignTimeDbContextFactory<StarryNightDbContext>
    {
        public StarryNightDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<StarryNightDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            StarryNightDbContextConfigurer.Configure(builder, configuration.GetConnectionString(StarryNightConsts.ConnectionStringName));

            return new StarryNightDbContext(builder.Options);
        }
    }
}
