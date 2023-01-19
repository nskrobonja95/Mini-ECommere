using Application.Common.Interfaces;
using Application.Orders;
using Application.Products;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Application.IntegrationTests.Orders.Commands;

[Collection("Web app factory collection")]
public class CreateOrderCommandTests
{

    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public CreateOrderCommandTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public async Task ReturnsSuccessResultCreateOrder()
    {
        var claimsProvider = TestClaimsProvider.WithSpecificRole("Customer");
        var client = _factory.CreateClientWithTestAuth(claimsProvider);
        var userIdClaim = claimsProvider.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
        Assert.NotNull(userIdClaim);
        Assert.True(Guid.TryParse(userIdClaim!.Value.ToString(), out Guid userId));
        var sp = _factory.Services;
        using (var scope = sp.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();
            Utilities.AddCartItemsForCustomer(db, userId);
        }
        OrderDto order = new OrderDto()
        {
            City = "Novi Sad",
            Street = "My city",
            PhoneNumber = "+38163111111"
        };

        var stringPayload = JsonSerializer.Serialize(order);
        var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/api/order", httpContent);

        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<OrderDto>(responseString, _jsonSerializerOptions);

        Assert.IsType<OrderDto>(result);
        Assert.NotNull(result);
        var dbContext = Mock.Of<IApplicationDbContext>();
        var prodService = Mock.Of<IProductService>();
        var idService = Mock.Of<IIdentityService>();
        var orderService = new Mock<OrderService>(dbContext, prodService, idService).Object;
        double discount = 1;
        if (orderService.DiscountEligibility())
        {
            discount = orderService.CalculateDiscount(result!.PhoneNumber);
        }
        Assert.Equal((40*20+40*100)*discount, result!.TotalPrice);
    }

    [Fact]
    public async Task EmptyShoppingCart()
    {
        var claimsProvider = TestClaimsProvider.WithSpecificRole("OrderCustomer");
        var client = _factory.CreateClientWithTestAuth(claimsProvider);
        var userIdClaim = claimsProvider.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
        Assert.NotNull(userIdClaim);
        Assert.True(Guid.TryParse(userIdClaim!.Value.ToString(), out Guid userId));
        var sp = _factory.Services;
        using (var scope = sp.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();
            db.CartItems.RemoveRange(db.CartItems.Where(c => c.CustomerId == userId).ToList());
            db.SaveChanges();
        }
        OrderDto order = new OrderDto()
        {
            City = "Novi Sad",
            Street = "My city",
            PhoneNumber = "+38163111111"
        };

        var stringPayload = JsonSerializer.Serialize(order);
        var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/api/order", httpContent);

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ReturnsValidationErrorMissingPhonuNumber()
    {
        var claimsProvider = TestClaimsProvider.WithSpecificRole("OrderCustomer1");
        var client = _factory.CreateClientWithTestAuth(claimsProvider);
        OrderDto order = new OrderDto()
        {
            City = "Novi Sad",
            Street = "My city",
        };

        var stringPayload = JsonSerializer.Serialize(order);
        var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/api/order", httpContent);

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

    }

}
