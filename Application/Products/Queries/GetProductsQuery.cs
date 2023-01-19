using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Products.Queries;

public class GetProductsQuery : IRequest<List<ProductDto>>
{

}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = _context.Products.ProjectTo<ProductDto>(_mapper.ConfigurationProvider).ToList();
        return Task.FromResult(products);
    }
}
