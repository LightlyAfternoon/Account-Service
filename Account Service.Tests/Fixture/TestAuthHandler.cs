using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Account_Service.Tests.Fixture
    // ReSharper disable once ArrangeNamespaceBody
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
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.Trim())));

            // Add role claims after trimming whitespace

            var identity = new ClaimsIdentity(claims, "TestScheme");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");

            return Task.FromResult(AuthenticateResult.Success(ticket));

        }
    }
}