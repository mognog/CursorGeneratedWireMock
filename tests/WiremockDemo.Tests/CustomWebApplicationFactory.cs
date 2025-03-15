using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WiremockDemo.Api.Services;

namespace WiremockDemo.Tests;

/// <summary>
/// Custom WebApplicationFactory for integration tests
/// Replaces real services with test mocks
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ReplaceRealServicesWithMocks(services);
        });
    }
    
    /// <summary>
    /// Replaces real services with test mocks
    /// </summary>
    private static void ReplaceRealServicesWithMocks(IServiceCollection services)
    {
        // Remove the real ExternalService
        var descriptor = services.SingleOrDefault(
            d => d.ServiceType == typeof(IExternalService));
            
        if (descriptor != null)
        {
            services.Remove(descriptor);
        }
        
        // Add the mock service
        services.AddScoped<IExternalService, MockExternalService>();
    }
} 