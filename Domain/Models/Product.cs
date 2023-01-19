namespace Domain.Models;

public class Product
{

    public Product()
    {
        Name = String.Empty;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public Double Quantity { get; set; }
    public Guid MeasureUnitId { get; set; }
    public MeasureUnit? MeasureUnit { get; set; }
    public List<OrderItem> ?OrderItems { get; set; }
    public List<ProductPrice>? ProductPrices { get; set; }

}
