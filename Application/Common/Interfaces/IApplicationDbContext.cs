using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{

    DbSet<CartItem> CartItems { get; }
    DbSet<MeasureUnit> MeasureUnits { get; }
    DbSet<Product> Products { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<ProductPrice> ProductPrices { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

}
