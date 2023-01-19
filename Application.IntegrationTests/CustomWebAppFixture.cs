namespace Application.IntegrationTests;

[CollectionDefinition("Web app factory collection")]
public class CustomWebAppFixture : ICollectionFixture<CustomWebApplicationFactory<Program>>
{
}
