using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Products;
using Application.ShoppingCart;
using Domain.Models;
using MediatR;

namespace Application.Orders.Commands;

public class CreateOrderCommand : IRequest<OrderDto>
{

    public CreateOrderCommand(OrderDto order)
    {
        Order = order;
    }

    public OrderDto Order { get; set; }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{

    private readonly IApplicationDbContext _context;
    private readonly IOrderService _orderService;
    private readonly IShoppingCartService _shoppingCartService;

    public CreateOrderCommandHandler(IApplicationDbContext context, IOrderService orderService, IShoppingCartService shoppingCartService)
    {
        _context = context;
        _orderService = orderService;
        _shoppingCartService = shoppingCartService;
    }

    public Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        bool discount = _orderService.DiscountEligibility();        
        var cartItems = _context.CartItems.Where(ci => ci.CustomerId == request.Order.CustomerId).ToList();
        if (!cartItems.Any()) throw new NotFoundException(nameof(ShoppingCart));
        var order = _orderService.CreateOrder(cartItems, request.Order.CustomerId);
        if(discount) order.TotalPrice = order.TotalPrice * _orderService.CalculateDiscount(request.Order.PhoneNumber);
        _context.Orders.Add(order);
        _shoppingCartService.ClearShoppingCart(order.CustomerId);

        _context.SaveChangesAsync(cancellationToken);

        return Task.FromResult(new OrderDto(order));
    }
}
