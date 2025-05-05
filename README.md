# AdventureArchive.Api

## Overview
AdventureArchive.Api is a .NET 8 Web API for managing and retrieving information about outdoor trips, tracks, and huts. It integrates with the New Zealand Department of Conservation (DOC) API to provide up-to-date data on tracks and huts, and allows users to create and manage their own trips.

## Features
- Retrieve DOC tracks and huts by region
- Create and manage trips with waypoints and date ranges
- OpenAPI/Swagger documentation
- Structured logging with Serilog

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Setup
1. Clone the repository:
   ```sh
   git clone <repo-url>
   cd AdventureArchive.Api
   ```
2. Restore dependencies:
   ```sh
   dotnet restore
   ```
3. (Optional) Update `appsettings.json` or `appsettings.Development.json` for API keys and endpoints.
4. Run the API:
   ```sh
   dotnet run --project AdventureArchive.Api.csproj
   ```
5. Access Swagger UI at [http://localhost:5130/swagger](http://localhost:5130/swagger) (default).

## Configuration
- `appsettings.json` contains configuration for DOC API, logging, and allowed hosts.
- Environment variables can override settings (see `Properties/launchSettings.json`).

## API Endpoints
- `GET /api/doc/tracks?regionCode=...` — List tracks by region
- `GET /api/doc/huts?regionCode=...` — List huts by region
- (Trip management endpoints can be added as the project evolves)

## Dependencies
- ASP.NET Core 8
- Serilog (Console, Seq)
- Swashbuckle.AspNetCore (Swagger/OpenAPI)
- Microsoft.Extensions.*

## Development
- Code is organized by layer: `src/Api`, `src/Application`, `src/Domain`, `src/Infrastructure`.
- Dependency injection is configured in `src/Api/Extensions/ServiceExtensions.cs`.
- Logging is configured in `src/Api/Program.cs`.

## Testing
- (Add test instructions if/when tests are implemented)

## License
- (Add license information here)
