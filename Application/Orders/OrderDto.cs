using Application.Common.Mappings;
using Domain.Models;

namespace Application.Orders;

public class OrderDto : IMapFrom<Order>
{
    public OrderDto()
    {
        City = String.Empty;
        Street = String.Empty;
        HouseNumber = String.Empty;
        PhoneNumber = String.Empty;
    }

    public OrderDto(Order order)
    {
        Id = order.Id;
        City = order.City;
        Street = order.Street;
        HouseNumber = order.HouseNumber;
        PhoneNumber = order.PhoneNumber;
        CustomerId = order.CustomerId;
        TotalPrice = order.TotalPrice;
    }

    public Guid Id { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string PhoneNumber { get; set; }
    public double TotalPrice { get; set; }
    public Guid CustomerId { get; set; }
}
