
using Domain.Models;

namespace Application.Orders;

public interface IOrderService
{

    public Task<double> CalculateDiscount(Guid customerId);
    public double CalculateDiscount(string phonenumber);
    public Order CreateOrder(List<CartItem> cartItems, Guid customerId);
    public OrderItem CreateOrderItem(CartItem cartItem);
    public bool DiscountEligibility();

}
