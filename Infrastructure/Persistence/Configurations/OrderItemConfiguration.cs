using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasOne(o => o.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(o => o.OrderId)
                .HasConstraintName("FK_ORDERITEM_ORDER");

        builder.HasOne(o => o.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(o => o.ProductId)
                .HasConstraintName("FK_ORDERITEM_PRODUCT");
    }
}
