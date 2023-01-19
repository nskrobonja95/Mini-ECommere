using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasOne(c => c.Product)
                    .WithMany()
                    .HasForeignKey(c => c.ProductId)
                    .HasConstraintName("FK_CARTITEM_PRODUCT");
    }
}
