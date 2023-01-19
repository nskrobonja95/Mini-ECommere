using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Products;

public class ProductService : IProductService
{

    private readonly IApplicationDbContext _context;

    public ProductService(IApplicationDbContext context)
    {
        _context = context;
    }

    public double GetProductActiveUnitPrice(Guid productId)
    {
        var price = _context.ProductPrices.FirstOrDefault(p => p.ProductId == productId && p.ValidTo == null);
        if (price == null) throw new NotFoundException(nameof(ProductPrice));
        return price.UnitPrice;
    }
}
