using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WiremockDemo.Api.Models;
using WiremockDemo.Api.Services;

namespace WiremockDemo.Api.Controllers;

[ApiController]
[Route("api")]
public class WeatherController : ControllerBase
{
    private readonly IExternalService _externalService;
    private readonly ILogger<WeatherController> _logger;
    private readonly JsonSerializerOptions _jsonOptionsCamelCase;
    private readonly JsonSerializerOptions _jsonOptionsPascalCase;

    public WeatherController(
        IExternalService externalService,
        ILogger<WeatherController> logger)
    {
        _externalService = externalService;
        _logger = logger;
        
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

    [HttpGet("weatherCamelCase")]
    public async Task<IActionResult> GetWeatherCamelCase()
    {
        try
        {
            _logger.LogInformation("Received request for weather data (camelCase)");
            
            // Get weather data from external service with camelCase formatting
            var weatherData = await _externalService.GetWeatherDataAsync(usePascalCase: false);
            
            // Return with camelCase formatting
            return Ok(weatherData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving weather data");
            return StatusCode(500, "An error occurred while retrieving weather data");
        }
    }
    
    [HttpGet("weatherPascalCase")]
    public async Task<IActionResult> GetWeatherPascalCase()
    {
        try
        {
            _logger.LogInformation("Received request for weather data (PascalCase)");
            
            // Get weather data from external service with PascalCase formatting
            var weatherData = await _externalService.GetWeatherDataAsync(usePascalCase: true);
            
            // Return with PascalCase formatting
            var result = JsonSerializer.Serialize(weatherData, _jsonOptionsPascalCase);
            return Content(result, "application/json");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving weather data");
            return StatusCode(500, "An error occurred while retrieving weather data");
        }
    }
    
    // Keep the original endpoint for backward compatibility
    [HttpGet("Weather")]
    public async Task<IActionResult> GetWeather()
    {
        return await GetWeatherCamelCase();
    }
} 