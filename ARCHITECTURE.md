# PlcAssistant - Architecture Documentation

## Overview
PlcAssistant is a .NET 10 MAUI Hybrid application that simulates a Programmable Logic Controller (PLC). The solution follows Domain-Driven Design (DDD) principles implemented as a modular monolith.

## Architecture Layers

```
┌─────────────────────────────────────────────────────────┐
│                    Presentation Layer                    │
│  ┌─────────────────────┐      ┌──────────────────────┐ │
│  │  PlcAssistant.      │      │  PlcAssistant.       │ │
│  │  MauiApp            │      │  Server              │ │
│  │  (Blazor Hybrid UI) │      │  (Background Service)│ │
│  └─────────────────────┘      └──────────────────────┘ │
└─────────────────────────────────────────────────────────┘
                            │
                            │ Uses
                            ▼
┌─────────────────────────────────────────────────────────┐
│                   Infrastructure Layer                   │
│              PlcAssistant.Infrastructure                 │
│  ┌─────────────────────────────────────────────────┐   │
│  │  Repositories (LiteDB)                          │   │
│  │  - PlcMemoryAddressRepository                   │   │
│  │  - ServerConfigurationRepository                │   │
│  ├─────────────────────────────────────────────────┤   │
│  │  Services                                       │   │
│  │  - PlcServerService                             │   │
│  │  - DataSeeder                                   │   │
│  └─────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
                            │
                            │ Implements
                            ▼
┌─────────────────────────────────────────────────────────┐
│                      Domain Layer                        │
│               PlcAssistant.Domain                        │
│  ┌─────────────────────────────────────────────────┐   │
│  │  Entities                                       │   │
│  │  - PlcMemoryAddress                             │   │
│  │  - ServerConfiguration                          │   │
│  │  - PlcDataType (Enum)                           │   │
│  ├─────────────────────────────────────────────────┤   │
│  │  Interfaces                                     │   │
│  │  - IPlcMemoryAddressRepository                  │   │
│  │  - IServerConfigurationRepository               │   │
│  │  - IPlcServerService                            │   │
│  └─────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

## Domain Layer (PlcAssistant.Domain)

### Purpose
Contains the core business logic and domain entities. This layer is completely independent and has no external dependencies.

### Key Components

#### Entities
- **PlcMemoryAddress**: Represents a PLC memory location with address, data type, and current value
- **ServerConfiguration**: Server settings (IP, port, update interval, auto-start)
- **PlcDataType**: Enumeration of supported PLC data types

#### Interfaces
- **IPlcMemoryAddressRepository**: Contract for memory address CRUD operations
- **IServerConfigurationRepository**: Contract for configuration persistence
- **IPlcServerService**: Contract for PLC server control

### Design Principles
- Pure domain logic
- No framework dependencies
- No database concerns
- Technology-agnostic

## Infrastructure Layer (PlcAssistant.Infrastructure)

### Purpose
Implements domain interfaces using concrete technologies (LiteDB, logging, etc.).

### Key Components

#### Repositories
- **PlcMemoryAddressRepository**: LiteDB implementation for memory addresses
- **ServerConfigurationRepository**: LiteDB implementation for configuration

#### Services
- **PlcServerService**: Manages PLC server lifecycle (start, stop, status)
- **DataSeeder**: Seeds initial sample data for testing

#### Data
- **PlcDbContext**: LiteDB database context wrapper

### Technologies
- LiteDB 5.0.21
- Microsoft.Extensions.Logging.Abstractions 10.0.1

## Presentation Layer

### PlcAssistant.Server
Background service that hosts the PLC server.

#### Responsibilities
- Host the PLC server as a Windows Service or console application
- Auto-start based on configuration
- Periodic status logging
- Data seeding (with `--seed` flag)

#### Key Files
- **Program.cs**: Service host configuration and DI setup
- **Worker.cs**: Background service implementation

### PlcAssistant.MauiApp
MAUI Hybrid application with Blazor UI.

#### Responsibilities
- User interface for PLC simulation
- Server control (start/stop)
- Memory address management (CRUD)
- Configuration management

#### Structure
```
MauiApp/
├── Components/
│   ├── Layout/
│   │   ├── MainLayout.razor       # Main app layout
│   │   └── NavMenu.razor          # Navigation menu
│   ├── Routes.razor               # Routing configuration
│   └── _Imports.razor             # Global using statements
├── Pages/
│   ├── Home.razor                 # Dashboard
│   ├── ServerControl.razor        # Server start/stop
│   ├── MemoryAddresses.razor      # CRUD for addresses
│   └── Configuration.razor        # Settings
├── Resources/                     # App resources (icons, fonts, etc.)
├── wwwroot/                       # Static web assets
├── App.xaml/xaml.cs              # MAUI app definition
├── MainPage.xaml/xaml.cs         # Main page with BlazorWebView
└── MauiProgram.cs                # App configuration
```

#### Technologies
- .NET MAUI 10.0.0
- Blazor WebView
- Syncfusion Blazor Components 27.1.51
  - Grid (for memory addresses table)
  - Buttons
  - Inputs (TextBox, NumericTextBox)

## Data Flow

### Read Operation (Example: Get Memory Addresses)
```
UI (MemoryAddresses.razor)
    │
    │ @inject IPlcMemoryAddressRepository
    │
    ▼
Repository.GetAllAsync()
    │
    │ LiteDB Query
    │
    ▼
LiteDB Database (plc.db)
    │
    │ Returns List<PlcMemoryAddress>
    │
    ▼
UI renders in Syncfusion Grid
```

### Write Operation (Example: Update Configuration)
```
UI (Configuration.razor)
    │
    │ User clicks "Save"
    │
    ▼
Repository.UpdateConfigurationAsync(config)
    │
    │ LiteDB Update
    │
    ▼
LiteDB Database (plc.db)
    │
    │ Success
    │
    ▼
UI shows success message
```

### Server Control (Example: Start PLC Server)
```
UI (ServerControl.razor)
    │
    │ User clicks "Start Server"
    │
    ▼
PlcServerService.StartAsync()
    │
    │ Creates background task
    │
    ▼
Server Loop Running
    │
    │ Periodic operations
    │
    ▼
UI shows "Running" status
```

## Database Schema (LiteDB)

### Collection: memoryAddresses
```json
{
  "_id": "guid",
  "Name": "string",
  "Address": "string",
  "DataType": "int (enum)",
  "CurrentValue": "string (nullable)",
  "Description": "string (nullable)",
  "IsReadOnly": "bool",
  "CreatedAt": "datetime",
  "LastModifiedAt": "datetime (nullable)"
}
```

### Collection: serverConfiguration
```json
{
  "_id": "guid",
  "Port": "int",
  "IpAddress": "string",
  "UpdateIntervalMs": "int",
  "AutoStart": "bool",
  "LastModifiedAt": "datetime"
}
```

## Dependency Injection

### Server (PlcAssistant.Server)
```csharp
services.AddSingleton<IPlcMemoryAddressRepository>(...)
services.AddSingleton<IServerConfigurationRepository>(...)
services.AddSingleton<IPlcServerService, PlcServerService>()
services.AddSingleton<DataSeeder>()
services.AddHostedService<Worker>()
```

### MAUI App (PlcAssistant.MauiApp)
```csharp
services.AddSingleton<IPlcMemoryAddressRepository>(...)
services.AddSingleton<IServerConfigurationRepository>(...)
services.AddSingleton<IPlcServerService, PlcServerService>()
services.AddSingleton<DataSeeder>()
services.AddSyncfusionBlazor()
services.AddMauiBlazorWebView()
```

## Cross-Platform Considerations

### Database Location
- **Server**: `%LocalApplicationData%/PlcAssistant/plc.db`
- **MAUI App**: `{FileSystem.AppDataDirectory}/plc.db`

Both can share the same database if needed by configuring the same path.

### Platform Support
- **Server**: Windows, Linux, macOS (any .NET 10 supported platform)
- **MAUI App**: macOS (maccatalyst), Windows

### Concurrency
LiteDB handles concurrent access automatically with the `Connection=shared` parameter in the connection string.

## Security Considerations

1. **Database**: LiteDB file is stored in user's local app data folder (not encrypted by default)
2. **Network**: PLC server uses configured IP/Port (default: 127.0.0.1:502)
3. **Authentication**: Currently no authentication implemented (local use only)

## Performance Characteristics

- **LiteDB**: Fast for small to medium datasets (< 100,000 records)
- **UI**: Blazor Hybrid provides native performance
- **Server**: Lightweight background service with minimal overhead
- **Memory**: Typical usage < 50MB RAM

## Future Enhancements

Potential areas for expansion:
1. Real protocol implementation (e.g., Modbus TCP)
2. Network communication between server and clients
3. Real-time value updates
4. Data logging and history
5. Authentication and authorization
6. Web-based remote access
7. Export/import of memory addresses
8. Simulation scenarios
