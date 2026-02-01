using Microsoft.EntityFrameworkCore;
using TimescaleMetrics.DataAccess.Configuration;
using TimescaleMetrics.DataAccess.Entities;

namespace TimescaleMetrics.DataAccess
{
    public class TimescaleDbContext : DbContext
    {
        public TimescaleDbContext(DbContextOptions<TimescaleDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ValueConfiguration());
            modelBuilder.ApplyConfiguration(new ResultConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ValueEntity> Values { get; set; }
        public DbSet<ResultEntity> Results { get; set; }
    }
}
