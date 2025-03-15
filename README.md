# WiremockDemo

A demonstration project showing how to use WireMock.NET for mocking external API dependencies in a .NET application.

## Project Creation

This entire project was generated from scratch using:
- **Cursor IDE** (version 0.46.11)
- **Claude 3.7 Sonnet** LLM
- Windows 11 PC

The initial project generation took approximately 10 minutes, with an additional 30 minutes for tweaks and refinements (mainly to this readme file!). This demonstrates the power of AI-assisted development for rapidly creating functional demo applications.

The idea for this project was inspired by an amazing coder and good friend, Adrian Cope.

## Overview

This project demonstrates how to use WireMock.NET to create mock HTTP responses for testing and development purposes. It includes:

- A .NET API with controllers that return JSON data in different formats (camelCase and PascalCase)
- A WireMock server that simulates an external API
- Unit tests that use a mock service implementation

## Project Structure

- **src/WiremockDemo.Api**: The main API project
  - **Controllers**: Contains the API controllers
    - `WeatherController.cs`: Handles HTTP requests and returns weather data
  - **Models**: Contains the data models
    - `WeatherData.cs`: Defines the weather data structure
  - **Services**: Contains the service layer that calls the external API
    - `ExternalService.cs`: Implements the IExternalService interface to call the external API
  - **Wiremock**: Contains the WireMock server implementation
    - `WiremockServer.cs`: Sets up and configures the WireMock server

- **tests/WiremockDemo.Tests**: The test project
  - `WeatherControllerTests.cs`: Contains unit tests for the API controllers
  - `MockExternalService.cs`: Includes a mock implementation of the external service
  - `CustomWebApplicationFactory.cs`: Configures the test environment

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or another compatible IDE

### Running the Application

1. Clone the repository
2. Navigate to the project directory
3. Run the API:

```
cd src/WiremockDemo.Api
dotnet run
```

4. Access the API endpoints:
   - `http://localhost:5049/api/weatherCamelCase` - Returns JSON with camelCase properties
   - `http://localhost:5049/api/weatherPascalCase` - Returns JSON with PascalCase properties

![image](https://github.com/user-attachments/assets/f71c052a-856b-4dbd-8627-97a95fc55aa8)

![image](https://github.com/user-attachments/assets/09b86c79-4fcb-41f8-af20-7fb5912e61b1)


### Running the Tests

```
cd tests/WiremockDemo.Tests
dotnet test
```

Or from the root directory:

```
dotnet test
```

## How It Works

### WireMock Server

The WireMock server is implemented in `WiremockServer.cs` and is responsible for:

1. Starting a WireMock.NET server on port 9090
2. Setting up mock responses for different API endpoints
3. Configuring JSON serialization options for different response formats

The server is registered as a singleton in the dependency injection container and is automatically started when the application runs in development mode.

### External Service

The `ExternalService` class is responsible for:

1. Making HTTP requests to the WireMock server
2. Deserializing the responses using the appropriate JSON options
3. Returning the data to the controllers

In a real-world scenario, this service would call an actual external API, but for demonstration purposes, it calls the WireMock server running locally.

### Controllers

The `WeatherController` class exposes two endpoints:

1. `GET /api/weatherCamelCase` - Returns weather data with camelCase property names
2. `GET /api/weatherPascalCase` - Returns weather data with PascalCase property names

### Testing

The test project includes:

1. Unit tests for the `WeatherController` class
2. A mock implementation of the `IExternalService` interface
3. A custom `WebApplicationFactory` for integration testing

The tests verify that the controllers return the expected data and handle errors correctly.

## License

This project is licensed under the MIT License - see the LICENSE file for details. 
