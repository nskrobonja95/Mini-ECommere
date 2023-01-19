using Application.Common.Behaviours;
using Application.Orders;
using Application.Products;
using Application.ShoppingCart;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class ConfigureServices
{

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IShoppingCartService, ShoppingCartService>();

        return services;
    }

}
