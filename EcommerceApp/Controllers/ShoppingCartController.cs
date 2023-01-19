using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.ShoppingCart;
using Application.ShoppingCart.Commands.AddItemCommand;
using Application.ShoppingCart.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers;

[Authorize]
public class ShoppingCartController : ApiControllerBase
{

    private readonly ICurrentUserService _currentUserService;

    public ShoppingCartController(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public Task<List<CartItemDto>> GetShoppingCartItems()
    {
        string? userId = _currentUserService.UserId;
        if (!Guid.TryParse(userId, out Guid customerId)) throw new ValidationException();
        return Mediator.Send(new GetShoppingCartItemsQuery(customerId));
    }

    [HttpPost]
    public Task<CartItemDto> GetShoppingCartItems(CartItemDto cartItemDto)
    {
        if (!Guid.TryParse(_currentUserService.UserId, out Guid customerId)) throw new ValidationException();
        cartItemDto.CustomerId = customerId;
        return Mediator.Send(new AddItemToCartCommand(cartItemDto));
    }

}
