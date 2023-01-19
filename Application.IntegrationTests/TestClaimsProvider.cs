using IdentityModel;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.IntegrationTests;

public class TestClaimsProvider
{
    public IList<Claim> Claims { get; }

    public TestClaimsProvider(IList<Claim> claims)
    {
        Claims = claims;
    }

    public TestClaimsProvider()
    {
        Claims = new List<Claim>();
    }

    public static TestClaimsProvider WithSpecificRoles(IList<string>? roles = null)
    {
        var provider = new TestClaimsProvider();
        provider.Claims.Add(new Claim(ClaimTypes.Name, "User"));
        foreach (var role in roles ?? new List<string>())
        {
            provider.Claims.Add(new Claim(ClaimTypes.Role, role));
        }
        provider.Claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        return provider;
    }

    public static TestClaimsProvider WithSpecificRole(string? role = null)
    {
        var provider = new TestClaimsProvider();
        provider.Claims.Add(new Claim(ClaimTypes.Name, "User"));
        if (role != null)
            provider.Claims.Add(new Claim(ClaimTypes.Role, role));
        provider.Claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        return provider;
    }
}

