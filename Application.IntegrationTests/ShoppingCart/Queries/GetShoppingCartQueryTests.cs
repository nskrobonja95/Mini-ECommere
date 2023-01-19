using Application.Orders;
using Application.ShoppingCart;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace Application.IntegrationTests.ShoppingCart.Queries;

[Collection("Web app factory collection")]
public class GetShoppingCartQueryTests
{

    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public GetShoppingCartQueryTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public async Task ReturnsSuccessResultGetShoppingCart()
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

        var response = await client.GetAsync("/api/shoppingcart");

        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<List<CartItemDto>>(responseString, _jsonSerializerOptions);

        Assert.IsType<List<CartItemDto>>(result);

        using (var scope = sp.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();
            Utilities.ClearCartItemsForCustomer(db, userId);
        }
    }

}
