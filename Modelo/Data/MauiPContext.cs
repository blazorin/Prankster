using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Model.Data
{
    public class MauiPContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserPerms> UserPerms { get; set; }
        public DbSet<Call> Calls { get; set; }
        public DbSet<Prank> Pranks { get; set; }
        public DbSet<Refer> Refers { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<IpAddress> IpAddresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite("Filename=MauiPContext.db",
                opt => opt.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<User>().HasIndex(u => u.Identifier).IsUnique(true);
            model.Entity<Call>().HasIndex(c => c.DateRequested).IsUnique(false);
            model.Entity<Transaction>().HasIndex(t => t.Date).IsUnique(false);
            model.Entity<Refer>().HasIndex(r => r.ReferId).IsUnique(true);
            model.Entity<UserLog>().HasIndex(l => l.Date).IsUnique(false);
        }
    }
}