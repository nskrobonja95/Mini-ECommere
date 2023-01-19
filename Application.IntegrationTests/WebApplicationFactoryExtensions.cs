using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests;

public static class WebApplicationFactoryExtensions
{
    private static WebApplicationFactory<T> WithAuthentication<T>(this WebApplicationFactory<T> factory, TestClaimsProvider claimsProvider) where T : class
    {
        return factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = Utilities.SCHEMENAME;
                    options.DefaultChallengeScheme = Utilities.SCHEMENAME;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(Utilities.SCHEMENAME, op => { });

                services.AddScoped<TestClaimsProvider>(_ => claimsProvider);
            });
        });
    }

    public static HttpClient CreateClientWithTestAuth<T>(this WebApplicationFactory<T> factory, TestClaimsProvider claimsProvider) where T : class
    {
        var client = factory.WithAuthentication(claimsProvider).CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        return client;
    }

}
