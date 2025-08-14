using Account_Service.Infrastructure.Db;
using Account_Service.Infrastructure.Db.Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Account_Service.Tests.Fixture
{
    public class CustomWebApplicationFactory<TEntryPoint>(string dbConnectionString) : WebApplicationFactory<TEntryPoint>
        where TEntryPoint : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<HangfireContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ApplicationContext));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(HangfireContext));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseNpgsql(dbConnectionString);
                });

                services.AddDbContext<HangfireContext>(options =>
                {
                    options.UseNpgsql(dbConnectionString);
                });

                services.AddAuthentication("TestScheme")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    "TestScheme", (_) => { });

                services.AddAuthorization();
            });
        }
    }
}