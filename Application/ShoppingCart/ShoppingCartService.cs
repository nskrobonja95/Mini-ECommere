
using Application.Common.Interfaces;

namespace Application.ShoppingCart;

public class ShoppingCartService : IShoppingCartService
{

    private readonly IApplicationDbContext _context;

    public ShoppingCartService(IApplicationDbContext context)
    {
        _context = context;
    }

    public void ClearShoppingCart(Guid customerId)
    {
        var cartItems = _context.CartItems.Where(ci => ci.CustomerId == customerId).ToList();
        _context.CartItems.RemoveRange(cartItems);
    }
}
