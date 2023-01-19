using FluentValidation;

namespace Application.ShoppingCart.Commands.AddItemCommand;

public class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
    public AddItemToCartCommandValidator()
    {
        RuleFor(c => c.ProductId).NotNull().NotEmpty();
        RuleFor(c => c.Quantity).NotNull().NotEmpty();
        RuleFor(c => c.CustomerId).NotNull().NotEmpty();
    }
}
