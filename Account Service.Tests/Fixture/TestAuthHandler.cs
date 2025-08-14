using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;

namespace Account_Service.Tests.Fixture
{
    internal class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string TestUserRolesHeader = "X-TestUserRoles";

        /// <inheritdoc />
        [Obsolete("Obsolete")]
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var rolesHeader = Request.Headers[TestUserRolesHeader].FirstOrDefault() ?? "User";
            var roles = rolesHeader.Split([','], StringSplitOptions.RemoveEmptyEntries);

            var claims = new List<Claim> { new(ClaimTypes.Name, "TestUser") };

            // Add role claims after trimming whitespace
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Trim()));
            }

            var identity = new ClaimsIdentity(claims, "TestScheme");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");

            return Task.FromResult(AuthenticateResult.Success(ticket));

        }
    }
}