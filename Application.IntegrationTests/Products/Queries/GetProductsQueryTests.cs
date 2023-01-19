using Application.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using Xunit;

namespace Application.IntegrationTests.Products.Queries;

public class GetProductsQueryTests : IClassFixture<WebApplicationFactory<Program>>
{

    private readonly WebApplicationFactory<Program> _factory;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public GetProductsQueryTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

    }

    [Fact]
    public async Task ReturnsSuccessResultProductss()
    {
        var client = _factory.CreateClientWithTestAuth(TestClaimsProvider.WithSpecificRole("Admin"));

        var response = await client.GetAsync("/api/products");

        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<List<ProductDto>>(responseString);

        Assert.IsType<List<ProductDto>>(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result!.Count);
    }


}
