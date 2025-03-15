using System.Text.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WiremockDemo.Api.Models;

namespace WiremockDemo.Api.Wiremock;

public class WiremockServer : IDisposable
{
    private readonly WireMockServer _server;
    private readonly ILogger<WiremockServer> _logger;
    private readonly JsonSerializerOptions _jsonOptionsCamelCase;
    private readonly JsonSerializerOptions _jsonOptionsPascalCase;
    private readonly WeatherData _weatherData;
    private const int DefaultServerPort = 9090;

    public WiremockServer(ILogger<WiremockServer> logger, int? port = null)
    {
        _logger = logger;
        
        // Initialize JSON serialization options
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

        // Create sample weather data
        _weatherData = new WeatherData
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = 25,
            Summary = "Sunny"
        };

        // Start WireMock server
        int serverPort = port ?? DefaultServerPort;
        _server = WireMockServer.Start(serverPort);
        _logger.LogInformation("WireMock server started on port {Port}", serverPort);
        
        // Set up mock responses
        SetupMockResponses();
    }

    private void SetupMockResponses()
    {
        // Set up mock responses for different JSON formatting options
        SetupWeatherEndpoint("/api/weatherCamelCase", _jsonOptionsCamelCase);
        SetupWeatherEndpoint("/api/weatherPascalCase", _jsonOptionsPascalCase);
    }
    
    private void SetupWeatherEndpoint(string path, JsonSerializerOptions jsonOptions)
    {
        _server
            .Given(Request.Create().WithPath(path).UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(JsonSerializer.Serialize(_weatherData, jsonOptions))
            );
        
        _logger.LogInformation("Mock response set up for GET {Path}", path);
    }

    public void Dispose()
    {
        _server?.Dispose();
        _logger.LogInformation("WireMock server stopped");
        GC.SuppressFinalize(this);
    }
} 