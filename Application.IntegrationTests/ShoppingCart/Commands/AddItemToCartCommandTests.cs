using Application.Orders;
using Application.ShoppingCart;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Application.IntegrationTests.ShoppingCart.Commands;

[Collection("Web app factory collection")]
public class AddItemToCartCommandTests
{

    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public AddItemToCartCommandTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public async Task AddItemTest()
    {
        var claimsProvider = TestClaimsProvider.WithSpecificRole("Customer");
        var client = _factory.CreateClientWithTestAuth(claimsProvider);
        var userIdClaim = claimsProvider.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
        Assert.NotNull(userIdClaim);
        Assert.True(Guid.TryParse(userIdClaim!.Value.ToString(), out Guid userId));
        Guid productId;
        var sp = _factory.Services;
        using (var scope = sp.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();
            var product = db.Products.FirstOrDefault();
            Assert.NotNull(product);
            productId = product!.Id;
        }
        CartItemDto order = new CartItemDto()
        {
            NumberOfUnits = 12,
            ProductId = productId
        };

        var stringPayload = JsonSerializer.Serialize(order);
        var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/shoppingcart", httpContent);

        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<CartItemDto>(responseString, _jsonSerializerOptions);

        Assert.IsType<CartItemDto>(result);
    }

    [Fact]
    public async Task NumberOfUnitsMissingTest()
    {
        var claimsProvider = TestClaimsProvider.WithSpecificRole("Customer");
        var client = _factory.CreateClientWithTestAuth(claimsProvider);
        var userIdClaim = claimsProvider.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
        Assert.NotNull(userIdClaim);
        Assert.True(Guid.TryParse(userIdClaim!.Value.ToString(), out Guid userId));
        Guid productId;
        var sp = _factory.Services;
        using (var scope = sp.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();
            var product = db.Products.FirstOrDefault();
            Assert.NotNull(product);
            productId = product!.Id;
        }
        CartItemDto order = new CartItemDto()
        {
            ProductId = productId
        };

        var stringPayload = JsonSerializer.Serialize(order);
        var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/shoppingcart", httpContent);

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

    }

}
