using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasOne(p => p.MeasureUnit)
                .WithMany(m => m.Products)
                .HasForeignKey(p => p.MeasureUnitId)
                .HasConstraintName("FK_PRODUCT_MEASUREUNIT");

        builder.Property(p => p.Name).HasMaxLength(50);
    }
}
