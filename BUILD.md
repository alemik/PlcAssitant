# Build Instructions

## Local Development Build

### Prerequisites
- .NET 10 SDK installed
- For MAUI app: MAUI workload installed on macOS or Windows

### Building All Projects (except MAUI on Linux)

**Windows/macOS:**
```bash
# Restore packages
dotnet restore

# Build all projects
dotnet build

# Or build in Release mode
dotnet build -c Release
```

**Linux (server only):**
```bash
# Build Domain, Infrastructure, and Server projects only
cd src/PlcAssistant.Server
dotnet build

# This will automatically build dependencies:
# - PlcAssistant.Domain
# - PlcAssistant.Infrastructure
```

### Building Individual Projects

#### Domain Layer
```bash
cd src/PlcAssistant.Domain
dotnet build
```

#### Infrastructure Layer
```bash
cd src/PlcAssistant.Infrastructure
dotnet build
```

#### Server Service
```bash
cd src/PlcAssistant.Server
dotnet build

# Run with seed data
dotnet run -- --seed
```

#### MAUI App (macOS)
```bash
cd src/PlcAssistant.MauiApp

# Install MAUI workload if not already installed
dotnet workload install maui

# Build for macOS
dotnet build -f net10.0-maccatalyst

# Run
dotnet run -f net10.0-maccatalyst
```

#### MAUI App (Windows)
```bash
cd src/PlcAssistant.MauiApp

# Install MAUI workload if not already installed
dotnet workload install maui

# Build for Windows
dotnet build -f net10.0-windows10.0.19041.0

# Run
dotnet run -f net10.0-windows10.0.19041.0
```

## Publishing

### Publish Server as Self-Contained
```bash
cd src/PlcAssistant.Server

# Windows
dotnet publish -c Release -r win-x64 --self-contained

# Linux
dotnet publish -c Release -r linux-x64 --self-contained

# macOS
dotnet publish -c Release -r osx-x64 --self-contained
```

Output will be in: `src/PlcAssistant.Server/bin/Release/net10.0/{runtime}/publish/`

### Publish MAUI App

#### macOS
```bash
cd src/PlcAssistant.MauiApp
dotnet publish -f net10.0-maccatalyst -c Release
```

#### Windows
```bash
cd src/PlcAssistant.MauiApp
dotnet publish -f net10.0-windows10.0.19041.0 -c Release
```

## Testing

### Run Server Tests
```bash
cd src/PlcAssistant.Server
dotnet test
```

Currently, no unit tests are included in the solution. To add tests:
1. Create test projects (e.g., `PlcAssistant.Domain.Tests`)
2. Add xUnit or NUnit packages
3. Write unit tests for domain logic and services

## Continuous Integration

### GitHub Actions

The solution can be built on GitHub Actions with some limitations:

**Linux runners**: Can build Domain, Infrastructure, and Server projects
**macOS runners**: Can build all projects including MAUI app
**Windows runners**: Can build all projects including MAUI app

See `.github/workflows/build.yml` for an example workflow.

### Azure DevOps

Similar to GitHub Actions:
- Use Ubuntu agents for server builds
- Use macOS agents for MAUI macOS builds
- Use Windows agents for MAUI Windows builds

## Common Issues

### MAUI Workload Not Found
```bash
dotnet workload install maui
```

### LiteDB File Lock Issues
If you get "database file is locked" errors:
1. Close all applications using the database
2. Delete the `plc.db` file (will be recreated)
3. Restart the application

### Syncfusion License
Syncfusion components require a license:
1. Register for free Community License at https://www.syncfusion.com/sales/communitylicense
2. Add license key in your code (if required by version)

## Clean Build

```bash
# Remove all bin and obj folders
dotnet clean

# Or manually
find . -name "bin" -o -name "obj" | xargs rm -rf  # Linux/macOS
# For Windows PowerShell:
# Get-ChildItem -Path . -Include bin,obj -Recurse | Remove-Item -Recurse -Force
```

## Performance Optimization

### Release Build
```bash
dotnet build -c Release
```

### AOT Compilation (for MAUI)
Not currently configured, but can be enabled in the MAUI csproj:
```xml
<PublishAot>true</PublishAot>
```

### Trim Unused Code
```bash
dotnet publish -c Release -p:PublishTrimmed=true
```

## Docker Support

### Dockerfile for Server (Example)
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/PlcAssistant.Server/PlcAssistant.Server.csproj", "src/PlcAssistant.Server/"]
COPY ["src/PlcAssistant.Infrastructure/PlcAssistant.Infrastructure.csproj", "src/PlcAssistant.Infrastructure/"]
COPY ["src/PlcAssistant.Domain/PlcAssistant.Domain.csproj", "src/PlcAssistant.Domain/"]
RUN dotnet restore "src/PlcAssistant.Server/PlcAssistant.Server.csproj"
COPY . .
WORKDIR "/src/src/PlcAssistant.Server"
RUN dotnet build "PlcAssistant.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlcAssistant.Server.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:10.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlcAssistant.Server.dll"]
```

Build and run:
```bash
docker build -t plcassistant-server .
docker run -d plcassistant-server
```
