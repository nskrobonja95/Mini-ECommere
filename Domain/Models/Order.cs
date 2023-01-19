using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class Order
{

    public Order()
    {
        City = String.Empty;
        Street = String.Empty;
        HouseNumber = String.Empty;
        PhoneNumber = String.Empty;
    }

    public Guid Id { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string PhoneNumber { get; set; }
    public double TotalPrice { get; set; }
    public Guid CustomerId { get; set; }
    public List<OrderItem>? OrderItems { get; set; }

}
