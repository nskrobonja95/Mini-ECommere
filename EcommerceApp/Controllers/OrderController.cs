using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Orders;
using Application.Orders.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers;

[Authorize]
public class OrderController : ApiControllerBase
{

    private readonly ICurrentUserService _currentUserService;

    public OrderController(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    [HttpPost]
    public Task<OrderDto> CreateOrder([FromBody] OrderDto orderDto)
    {
        if (!Guid.TryParse(_currentUserService.UserId, out Guid customerId)) throw new ValidationException();
        orderDto.CustomerId = customerId;
        return Mediator.Send(new CreateOrderCommand(orderDto));
    }

}
