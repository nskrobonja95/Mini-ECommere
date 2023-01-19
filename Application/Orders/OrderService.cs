
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Products;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Orders;

public class OrderService : IOrderService
{

    public readonly IApplicationDbContext _context;
    public readonly IProductService _productService;
    public readonly IIdentityService _identityService;

    public OrderService(IApplicationDbContext context, IProductService productService, IIdentityService identityService)
    {
        _context = context;
        _productService = productService;
        _identityService = identityService;
    }

    public async Task<double> CalculateDiscount(Guid customerId)
    {
        var phoneNumber = await _identityService.GetUserPhonunumber(customerId.ToString());
        if (String.IsNullOrEmpty(phoneNumber)) throw new NotFoundException(nameof(IdentityUser));
        return CalculateDiscount(phoneNumber);
    }

    public double CalculateDiscount(string phonenumber)
    {
        double lastDigit = Char.GetNumericValue(phonenumber.Last<char>());
        if (lastDigit == -1) throw new ValidationException();
        if (lastDigit == 0) return 0.7;
        else if (lastDigit % 2 == 0) return 0.8;
        else return 0.9;
    }

    public Order CreateOrder(List<CartItem> cartItems, Guid customerId)
    {
        Order order = new Order() { OrderItems = new(), CustomerId = customerId};
        foreach (var cartItem in cartItems)
        {
            order.OrderItems.Add(CreateOrderItem(cartItem));
            order.TotalPrice += cartItem.NumberOfUnits * _productService.GetProductActiveUnitPrice(cartItem.ProductId);
        }
        return order;
    }

    public OrderItem CreateOrderItem(CartItem cartItem)
    {
        OrderItem orderItem = new OrderItem();
        orderItem.ProductId = cartItem.ProductId;
        orderItem.NumberOfUnits = cartItem.NumberOfUnits;
        return orderItem;
    }

    public bool DiscountEligibility()
    {
        DateTime now = DateTime.Now;
        var today = DateTime.Today;
        var fourPm = GetDateTimeFromHour(16);
        var fivePm = GetDateTimeFromHour(17);
        return now >= fourPm && now <= fivePm;
    }

    private DateTime GetDateTimeFromHour(int hour)
    {
        if (hour < 0 || hour > 23) return DateTime.MinValue;
        var today = DateTime.Today;
        return new DateTime(
            today.Year,
            today.Month,
            today.Day,
            hour,
            0,
            0,
            0
        );
    }
}
