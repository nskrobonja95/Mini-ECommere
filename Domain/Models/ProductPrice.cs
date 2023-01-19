namespace Domain.Models;

public class ProductPrice
{

    public Guid Id { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public Double UnitPrice { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

}
