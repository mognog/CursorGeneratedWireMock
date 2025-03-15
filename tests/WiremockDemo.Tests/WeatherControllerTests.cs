using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using WiremockDemo.Api.Models;

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
    
    [Fact]
    public async Task GetWeatherCamelCase_ReturnsJsonWithCamelCaseProperties()
    {
        // Act
        var response = await _client.GetAsync(CamelCaseEndpoint);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Get the raw JSON string
        var jsonString = await response.Content.ReadAsStringAsync();
        
        // Verify it contains camelCase property names
        jsonString.Should().Contain("\"date\":");
        jsonString.Should().Contain("\"temperatureC\":");
        jsonString.Should().Contain("\"summary\":");
        jsonString.Should().Contain("\"temperatureF\":");
        
        // Verify it doesn't contain PascalCase property names
        jsonString.Should().NotContain("\"Date\":");
        jsonString.Should().NotContain("\"TemperatureC\":");
        jsonString.Should().NotContain("\"Summary\":");
        jsonString.Should().NotContain("\"TemperatureF\":");
    }
    
    [Fact]
    public async Task GetWeatherPascalCase_ReturnsJsonWithPascalCaseProperties()
    {
        // Act
        var response = await _client.GetAsync(PascalCaseEndpoint);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Get the raw JSON string
        var jsonString = await response.Content.ReadAsStringAsync();
        
        // Verify it contains PascalCase property names
        jsonString.Should().Contain("\"Date\":");
        jsonString.Should().Contain("\"TemperatureC\":");
        jsonString.Should().Contain("\"Summary\":");
        jsonString.Should().Contain("\"TemperatureF\":");
        
        // Verify it doesn't contain camelCase property names
        jsonString.Should().NotContain("\"date\":");
        jsonString.Should().NotContain("\"temperatureC\":");
        jsonString.Should().NotContain("\"summary\":");
        jsonString.Should().NotContain("\"temperatureF\":");
    }
    
    [Fact]
    public async Task BothEndpoints_ReturnSameDataWithDifferentCasing()
    {
        // Act
        var camelCaseResponse = await _client.GetAsync(CamelCaseEndpoint);
        var pascalCaseResponse = await _client.GetAsync(PascalCaseEndpoint);
        
        // Assert
        camelCaseResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        pascalCaseResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var camelCaseData = await camelCaseResponse.Content.ReadFromJsonAsync<WeatherData>(_jsonOptionsCamelCase);
        var pascalCaseData = await pascalCaseResponse.Content.ReadFromJsonAsync<WeatherData>(_jsonOptionsPascalCase);
        
        // Verify both endpoints return the same data values
        camelCaseData.Should().NotBeNull();
        pascalCaseData.Should().NotBeNull();
        
        pascalCaseData!.Date.Should().Be(camelCaseData!.Date);
        pascalCaseData.TemperatureC.Should().Be(camelCaseData.TemperatureC);
        pascalCaseData.Summary.Should().Be(camelCaseData.Summary);
        pascalCaseData.TemperatureF.Should().Be(camelCaseData.TemperatureF);
    }
    
    [Fact]
    public async Task WeatherData_HasValidCalculatedTemperatureF()
    {
        // Act
        var response = await _client.GetAsync(CamelCaseEndpoint);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var weatherData = await response.Content.ReadFromJsonAsync<WeatherData>(_jsonOptionsCamelCase);
        
        weatherData.Should().NotBeNull();
        
        // Verify TemperatureF is calculated correctly (32 + (int)(TemperatureC / 0.5556))
        int expectedTempF = 32 + (int)(weatherData!.TemperatureC / 0.5556);
        weatherData.TemperatureF.Should().Be(expectedTempF);
    }
} 