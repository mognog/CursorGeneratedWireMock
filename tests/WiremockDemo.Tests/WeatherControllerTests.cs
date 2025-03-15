using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using WiremockDemo.Api.Models;
using Xunit;

namespace WiremockDemo.Tests;

public class WeatherControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptionsCamelCase;
    private readonly JsonSerializerOptions _jsonOptionsPascalCase;
    private const string CamelCaseEndpoint = "/api/weatherCamelCase";
    private const string PascalCaseEndpoint = "/api/weatherPascalCase";

    public WeatherControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        
        _jsonOptionsCamelCase = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        
        _jsonOptionsPascalCase = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null, // Uses property names as-is (PascalCase)
            WriteIndented = true
        };
    }

    [Theory]
    [InlineData(CamelCaseEndpoint, true)]
    [InlineData(PascalCaseEndpoint, false)]
    public async Task GetWeather_ReturnsSuccessAndWeatherData(string endpoint, bool useCamelCase)
    {
        // Arrange
        var options = useCamelCase ? _jsonOptionsCamelCase : _jsonOptionsPascalCase;

        // Act
        var response = await _client.GetAsync(endpoint);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var weatherData = await response.Content.ReadFromJsonAsync<WeatherData>(options);
        
        weatherData.Should().NotBeNull();
        weatherData!.TemperatureC.Should().Be(25);
        weatherData.Summary.Should().Be("Sunny");
        
        // Verify date is current
        weatherData.Date.Year.Should().Be(DateOnly.FromDateTime(DateTime.Now).Year);
        weatherData.Date.Month.Should().Be(DateOnly.FromDateTime(DateTime.Now).Month);
        weatherData.Date.Day.Should().Be(DateOnly.FromDateTime(DateTime.Now).Day);
    }
} 