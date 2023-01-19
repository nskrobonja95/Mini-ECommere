using Application.Common.Mappings;
using Domain.Models;

namespace Application.ShoppingCart;

public class CartItemDto : IMapFrom<CartItem>
{

    public CartItemDto(){}

    public CartItemDto(CartItem cartItem)
    {
        Id = cartItem.Id;
        CustomerId = cartItem.CustomerId;
        ProductId = cartItem.ProductId;
        NumberOfUnits = cartItem.NumberOfUnits;
    }

    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
    public Double NumberOfUnits { get; set; }

}
