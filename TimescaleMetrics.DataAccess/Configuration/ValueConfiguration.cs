using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimescaleMetrics.DataAccess.Entities;

namespace TimescaleMetrics.DataAccess.Configuration
{
    public class ValueConfiguration : IEntityTypeConfiguration<ValueEntity>
    {
        public void Configure(EntityTypeBuilder<ValueEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(v => v.Date)
                .IsRequired();

            builder.Property(v => v.ExecutionTime)
                .IsRequired();

            builder.Property(v => v.Value)
                .IsRequired();

            builder.Property(v => v.FileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(v => v.FileName);

            builder.HasIndex(v => v.Date);
        }
    }
}
