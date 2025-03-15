# WiremockDemo

A demonstration project showing how to use WireMock.NET for mocking external API dependencies in a .NET application.

## Project Creation

This entire project was generated from scratch using:
- **Cursor IDE** (version 0.46.11)
- **Claude 3.7 Sonnet** LLM
- Windows 11 PC

The initial project generation took approximately 10 minutes, with an additional 20 minutes for tweaks and refinements. This demonstrates the power of AI-assisted development for rapidly creating functional demo applications.

The idea for this project was inspired by an amazing coder and good friend, Adrian Cope.

## Overview

This project demonstrates how to use WireMock.NET to create mock HTTP responses for testing and development purposes. It includes:

- A .NET API with controllers that return JSON data in different formats (camelCase and PascalCase)
- A WireMock server that simulates an external API
- Unit tests that use a mock service implementation

## Project Structure

- **src/WiremockDemo.Api**: The main API project
  - **Controllers**: Contains the API controllers
  - **Models**: Contains the data models
  - **Services**: Contains the service layer that calls the external API
  - **Wiremock**: Contains the WireMock server implementation

- **tests/WiremockDemo.Tests**: The test project
  - Contains unit tests for the API controllers
  - Includes a mock implementation of the external service

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

![image](https://github.com/user-attachments/assets/d0320725-fe10-4f64-8193-4528878daefb)


### Running the Tests

```
dotnet test
```

## How It Works

1. The API controllers receive requests and call the external service
2. The external service makes HTTP requests to the WireMock server
3. The WireMock server returns mock responses with the appropriate JSON formatting
4. The API controllers return the data to the client

This demonstrates how to use WireMock.NET to simulate external dependencies during development and testing.

## License

This project is licensed under the MIT License - see the LICENSE file for details. 
