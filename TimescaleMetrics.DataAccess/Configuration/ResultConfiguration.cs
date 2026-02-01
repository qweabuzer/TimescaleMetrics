using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimescaleMetrics.DataAccess.Entities;

namespace TimescaleMetrics.DataAccess.Configuration
{
    public class ResultConfiguration : IEntityTypeConfiguration<ResultEntity>
    {
        public void Configure(EntityTypeBuilder<ResultEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(r => r.FileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(r => r.DeltaTime)
                .IsRequired();

            builder.Property(r => r.MinDate)
                .IsRequired();

            builder.Property(r => r.AvgExecutionTime)
                .IsRequired();

            builder.Property(r => r.AvgValue)
                .IsRequired();

            builder.Property(r => r.MedianValue)
                .IsRequired();

            builder.Property(r => r.MaxValue)
                .IsRequired();

            builder.Property(r => r.MinValue)
                .IsRequired();

            builder.HasIndex(r => r.FileName)
                .IsUnique();
        }
    }
   
}
