using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using StarryNight.Authorization.Roles;
using StarryNight.Authorization.Users;
using StarryNight.Entities;
using StarryNight.MultiTenancy;

namespace StarryNight.EntityFrameworkCore
{
    public class StarryNightDbContext : AbpZeroDbContext<Tenant, Role, User, StarryNightDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TagTarget> TagTargets { get; set; }
        public virtual DbSet<Target> Targets { get; set; }

        public StarryNightDbContext(DbContextOptions<StarryNightDbContext> options)
            : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<TagTarget>().HasKey(t => new { t.TagId, t.TargetId });
        //}
    }
}
