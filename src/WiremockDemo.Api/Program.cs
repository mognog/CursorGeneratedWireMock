using WiremockDemo.Api.Services;
using WiremockDemo.Api.Wiremock;

// Make the Program class accessible to the test project
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("WiremockDemo.Tests")]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure System.Text.Json for camelCase
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HttpClient for external service calls
builder.Services.AddHttpClient();

// Register services
builder.Services.AddScoped<IExternalService, ExternalService>();

// Register WireMock server as a singleton
builder.Services.AddSingleton<WiremockServer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // Start WireMock server in development environment
    app.Services.GetRequiredService<WiremockServer>();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Explicitly declare the Program class as public
public partial class Program { }
