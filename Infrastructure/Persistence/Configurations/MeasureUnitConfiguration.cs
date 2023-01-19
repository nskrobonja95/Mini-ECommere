using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class MeasureUnitConfiguration : IEntityTypeConfiguration<MeasureUnit>
{
    public void Configure(EntityTypeBuilder<MeasureUnit> builder)
    {
        builder.Property(m => m.Name).HasMaxLength(50);
    }
}
