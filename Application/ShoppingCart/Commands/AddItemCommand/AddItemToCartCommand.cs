using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Orders;
using Domain.Models;
using FluentValidation.Results;
using MediatR;
using System.Net.Http;
using System.Text.Json;

namespace Application.ShoppingCart.Commands.AddItemCommand;

public class AddItemToCartCommand : IRequest<CartItemDto>
{

    public AddItemToCartCommand(CartItemDto itemDto)
    {
        CustomerId = itemDto.CustomerId;
        ProductId = itemDto.ProductId;
        Quantity = itemDto.NumberOfUnits;
    }

    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
    public Double Quantity { get; set; }

}


public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, CartItemDto>
{

    private readonly IApplicationDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public AddItemToCartCommandHandler(IApplicationDbContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<CartItemDto> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == request.ProductId);
        if (product == null) throw new NotFoundException(nameof(Product));

        if(product.Quantity <= request.Quantity)
        {
            var httpClient = _httpClientFactory.CreateClient("Supplier");
            var httpResponseMessage = await httpClient.GetAsync($"?num=1&min={request.Quantity-20}&max={request.Quantity+20}&col=1&base=10&format=plain");
            int randNum = new Random().Next();
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();
                randNum = JsonSerializer.Deserialize<int>(contentStream);
            }
            if (randNum <= request.Quantity)
                throw new ValidationException(new List<ValidationFailure>() {
                    new ValidationFailure("InsufficientResources", "Requested quantity exceeds available stock.")});
        } else
        {
            product.Quantity -= request.Quantity;
        }

        CartItem cartItem = new CartItem()
        {
            CustomerId = request.CustomerId,
            ProductId = request.ProductId,
            NumberOfUnits = request.Quantity
        };
        _context.CartItems.Add(cartItem);
        _context.SaveChangesAsync(cancellationToken);

        CartItemDto cartItemDto = new CartItemDto(cartItem);
        return cartItemDto;
    }
}


