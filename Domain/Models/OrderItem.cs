namespace Domain.Models;

public class OrderItem
{

    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid OrderId { get; set; }
    public Double NumberOfUnits { get; set; }

    public Product? Product { get; set; }
    public Order? Order { get; set; }

}