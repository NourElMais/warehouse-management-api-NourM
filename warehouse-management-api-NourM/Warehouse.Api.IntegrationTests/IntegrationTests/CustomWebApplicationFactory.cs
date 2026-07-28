using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Warehouse.Api.IntegrationTests.FakeData;
using Warehouse.Domain.Repositories;

namespace Warehouse.Api.IntegrationTests.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IProductRepository>();
            services.AddSingleton<IProductRepository, FakeProductRepository>();
            services.RemoveAll<ISupplierRepository>();
            services.AddSingleton<ISupplierRepository, FakeSupplierRepository>();

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAssertion(_ => true).Build();

                options.AddPolicy("UserOrAdmin", policy => policy.RequireAssertion(_ => true));

                options.AddPolicy("Admin", policy => policy.RequireAssertion(_ => true));
            });
        });
    }
}
//Note: WebApplicationFactory<Program> starts the real API using the real Program.cs.