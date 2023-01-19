using FluentValidation;

namespace Application.Orders.Commands;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{

    public CreateOrderCommandValidator()
    {
        RuleFor(o => o.Order).NotNull().NotEmpty();
        RuleFor(o => o.Order.PhoneNumber).NotNull().NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(o => o.Order.CustomerId).NotNull().NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(o => o.Order.Street).NotNull().NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(o => o.Order.City).NotNull().NotEmpty().WithMessage("{PropertyName} is required.");
    }

}
