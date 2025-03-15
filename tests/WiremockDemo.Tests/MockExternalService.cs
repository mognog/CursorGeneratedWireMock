using WiremockDemo.Api.Models;
using WiremockDemo.Api.Services;

namespace WiremockDemo.Tests;

/// <summary>
/// Mock implementation of IExternalService for testing purposes
/// </summary>
public class MockExternalService : IExternalService
{
    // Shared test data to ensure consistency across tests
    private static readonly WeatherData TestWeatherData = new()
    {
        Date = DateOnly.FromDateTime(DateTime.Now),
        TemperatureC = 25,
        Summary = "Sunny"
    };
    
    public Task<WeatherData> GetWeatherDataAsync(bool usePascalCase = false)
    {
        // For testing purposes, we return the same data regardless of the formatting option
        // In a more complex test, we might return different data based on the parameter
        return Task.FromResult(TestWeatherData);
    }
} 