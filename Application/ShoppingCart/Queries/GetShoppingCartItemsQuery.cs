using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.ShoppingCart.Queries;

public class GetShoppingCartItemsQuery : IRequest<List<CartItemDto>>
{
    public GetShoppingCartItemsQuery(Guid customerId)
    {
        CustomerId = customerId;
    }

    public Guid CustomerId { get; set; }

}


public class GetShoppingCartItemsQueryHandler : IRequestHandler<GetShoppingCartItemsQuery, List<CartItemDto>>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetShoppingCartItemsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<CartItemDto>> Handle(GetShoppingCartItemsQuery request, CancellationToken cancellationToken)
    {
        var cartItems = _context.CartItems.Where(ci => ci.CustomerId == request.CustomerId)
                            .ProjectTo<CartItemDto>(_mapper.ConfigurationProvider).ToList();
        return Task.FromResult(cartItems);
    }
}
