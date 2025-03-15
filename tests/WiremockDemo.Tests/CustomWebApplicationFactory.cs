using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using WiremockDemo.Api.Services;
using WiremockDemo.Api.Wiremock;

namespace WiremockDemo.Tests;

/// <summary>
/// Custom WebApplicationFactory for integration tests
/// Replaces real services with test mocks
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    // Use a different port for tests to avoid conflicts with the running application
    private const int TestWiremockPort = 9091;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Configure test settings
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"ExternalService:BaseUrl", $"http://localhost:{TestWiremockPort}"}
            });
        });
        
        builder.ConfigureServices(services =>
        {
            // Replace real services with test mocks
            ReplaceRealServicesWithMocks(services);
            
            // Configure WireMock to use a different port for tests
            ConfigureWiremockForTests(services);
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
    
    /// <summary>
    /// Configures WireMock to use a different port for tests
    /// </summary>
    private static void ConfigureWiremockForTests(IServiceCollection services)
    {
        // Remove the real WiremockServer
        var descriptor = services.SingleOrDefault(
            d => d.ServiceType == typeof(WiremockServer));
            
        if (descriptor != null)
        {
            services.Remove(descriptor);
        }
        
        // Add WiremockServer with test port
        services.AddSingleton<WiremockServer>(sp => 
            new WiremockServer(
                sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<WiremockServer>>(), 
                TestWiremockPort));
    }
} 