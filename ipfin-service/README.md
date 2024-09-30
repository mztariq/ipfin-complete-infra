# Postcode API

This is a simple ASP.NET Core web service that provides postcode lookup and autocomplete functionality using the [Postcodes.io API](https://postcodes.io/).

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [JetBrains Rider](https://www.jetbrains.com/rider/)

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/mztariq/ipfin-service.git
    cd ipfn-web-service
    ```

2. Restore the dependencies:
    ```sh
    dotnet restore
    ```

### Running the Application

1. Run the application:
    ```sh
    dotnet run
    ```

2. The application will start and be accessible at `http://localhost:5180`. The Swagger UI will be available at `http://localhost:5180/swagger`.

### Configuration

The application is configured to run with the following settings in `launchSettings.json`:

```json
{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:41658",
      "sslPort": 44377
    }
  },
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "http://localhost:5180",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "https://localhost:7120;http://localhost:5180",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
