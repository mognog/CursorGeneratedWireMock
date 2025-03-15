using System.Text.Json;
using WiremockDemo.Api.Models;

namespace WiremockDemo.Api.Services;

public interface IExternalService
{
    Task<WeatherData> GetWeatherDataAsync(bool usePascalCase = false);
}

public class ExternalService : IExternalService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExternalService> _logger;
    private readonly IConfiguration _configuration;
    private readonly JsonSerializerOptions _jsonOptionsCamelCase;
    private readonly JsonSerializerOptions _jsonOptionsPascalCase;

    public ExternalService(
        HttpClient httpClient,
        ILogger<ExternalService> logger,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
        
        // Configure JSON options for camelCase deserialization
        _jsonOptionsCamelCase = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        
        // Configure JSON options for PascalCase deserialization
        _jsonOptionsPascalCase = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null, // Uses property names as-is (PascalCase)
            WriteIndented = true
        };
    }

    public async Task<WeatherData> GetWeatherDataAsync(bool usePascalCase = false)
    {
        try
        {
            // In a real application, this would be configured in appsettings.json
            string baseUrl = _configuration["ExternalService:BaseUrl"] ?? "http://localhost:9090";
            
            // Choose the appropriate endpoint based on the desired format
            string endpoint = usePascalCase ? "/api/weatherPascalCase" : "/api/weatherCamelCase";
            
            _logger.LogInformation("Calling external weather service at {BaseUrl}{Endpoint}", baseUrl, endpoint);
            
            // Make the HTTP request to the external service
            var response = await _httpClient.GetAsync($"{baseUrl}{endpoint}");
            
            // Ensure we got a successful response
            response.EnsureSuccessStatusCode();
            
            // Choose the appropriate JSON options based on the format
            var jsonOptions = usePascalCase ? _jsonOptionsPascalCase : _jsonOptionsCamelCase;
            
            // Deserialize the response using the appropriate JSON options
            var weatherData = await response.Content.ReadFromJsonAsync<WeatherData>(jsonOptions);
            
            if (weatherData == null)
            {
                throw new InvalidOperationException("Failed to deserialize weather data");
            }
            
            _logger.LogInformation("Successfully retrieved weather data: {Temperature}°C, {Summary}", 
                weatherData.TemperatureC, weatherData.Summary);
            
            return weatherData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling external weather service");
            throw;
        }
    }
} 