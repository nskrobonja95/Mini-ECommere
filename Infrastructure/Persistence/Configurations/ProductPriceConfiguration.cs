using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProductPriceConfiguration : IEntityTypeConfiguration<ProductPrice>
{
    public void Configure(EntityTypeBuilder<ProductPrice> builder)
    {
        builder.HasOne(p => p.Product)
                .WithMany(p => p.ProductPrices)
                .HasForeignKey(p => p.ProductId)
                .HasConstraintName("FK_PRODUCTPRICE_PRODUCT");
    }
}
