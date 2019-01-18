using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace StarryNight.EntityFrameworkCore
{
    public static class StarryNightDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<StarryNightDbContext> builder, string connectionString)
        {
            builder.UseLazyLoadingProxies().UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<StarryNightDbContext> builder, DbConnection connection)
        {
            builder.UseLazyLoadingProxies().UseSqlServer(connection);
        }
    }
}
