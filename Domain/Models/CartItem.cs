using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class CartItem
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
    public Double NumberOfUnits { get; set; }
    public Product? Product { get; set; }

}
