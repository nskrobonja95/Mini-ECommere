using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Application.IntegrationTests;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IList<Claim> _claims;

    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, TestClaimsProvider claimsProvider)
        : base(options, logger, encoder, clock)
    {
        _claims = claimsProvider.Claims;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var identity = new ClaimsIdentity(_claims, Utilities.SCHEMENAME);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Utilities.SCHEMENAME);

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }

}
