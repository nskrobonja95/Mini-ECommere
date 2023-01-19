using Application.Common.Mappings;
using Domain.Models;

namespace Application.Products;

public class ProductDto : IMapFrom<Product>
{

    public ProductDto()
    {
        Name = String.Empty;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public Double Quantity { get; set; }
    public Guid MeasureUnitId { get; set; }
}
