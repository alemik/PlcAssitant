# PlcAssistant

Una soluzione .NET 10 MAUI Hybrid per simulare un PLC (Programmable Logic Controller).

## Architettura

La soluzione implementa un'architettura **DDD (Domain-Driven Design) modular monolite** con i seguenti progetti:

### Progetti

1. **PlcAssistant.Domain** - Layer del dominio
   - Contiene le entità del dominio (`PlcMemoryAddress`, `ServerConfiguration`)
   - Definisce le interfacce (repository e servizi)
   - Non ha dipendenze esterne

2. **PlcAssistant.Infrastructure** - Layer dell'infrastruttura
   - Implementa i repository utilizzando **LiteDB**
   - Implementa il servizio `PlcServerService`
   - Gestisce la persistenza dei dati

3. **PlcAssistant.Server** - PLC Server Service
   - Servizio eseguibile come background worker
   - Può essere configurato per avviarsi automaticamente
   - Gestisce il ciclo di vita del server PLC

4. **PlcAssistant.MauiApp** - MAUI Hybrid UI
   - Applicazione MAUI Hybrid con Blazor
   - Utilizza componenti **Syncfusion** per l'interfaccia utente
   - Supporta macOS (maccatalyst)
   - Interfaccia per:
     - Avvio/arresto del server PLC
     - Gestione CRUD degli indirizzi di memoria PLC
     - Configurazione del server

## Tecnologie Utilizzate

- **.NET 10**
- **MAUI** (Multi-platform App UI)
- **Blazor** (Hybrid WebView)
- **LiteDB** - Database NoSQL embedded per configurazione e dati
- **Syncfusion Blazor Components** - Componenti UI (Grid, Inputs, Buttons)
- **DDD Pattern** - Domain-Driven Design

## Database

Tutti i dati (configurazione e indirizzi di memoria PLC) vengono salvati in un database **LiteDB** locale:
- Server: `%LocalApplicationData%/PlcAssistant/plc.db`
- MAUI App: `{FileSystem.AppDataDirectory}/plc.db`

## Prerequisiti

### Per il Server
- .NET 10 SDK
- Nessun workload aggiuntivo richiesto

### Per l'app MAUI
- .NET 10 SDK
- MAUI workload installato
- Per macOS: Xcode e MAUI workload per maccatalyst

## Build

### Server
```bash
cd src/PlcAssistant.Server
dotnet build
dotnet run
```

### MAUI App (su macOS)
```bash
cd src/PlcAssistant.MauiApp
dotnet build -f net10.0-maccatalyst
dotnet run -f net10.0-maccatalyst
```

## Funzionalità

### Server PLC
- Esecuzione come servizio background
- Auto-start configurabile
- Gestione stato (Start/Stop)
- Logging delle operazioni

### UI MAUI Hybrid
1. **Home** - Dashboard principale
2. **Server Control** - Controllo start/stop del server e visualizzazione stato
3. **Memory Addresses** - CRUD completo per indirizzi di memoria PLC
   - Aggiungi, modifica, elimina indirizzi
   - Grid con paginazione e ordinamento
   - Supporto per vari tipi di dati (Bool, Byte, Int, DInt, Real, String, Word, DWord)
4. **Configuration** - Gestione configurazione server
   - IP Address
   - Port
   - Update Interval
   - Auto-start

## Struttura del Codice

```
PlcAssistant/
├── src/
│   ├── PlcAssistant.Domain/
│   │   ├── Entities/
│   │   │   ├── PlcMemoryAddress.cs
│   │   │   ├── PlcDataType.cs
│   │   │   └── ServerConfiguration.cs
│   │   └── Interfaces/
│   │       ├── IPlcMemoryAddressRepository.cs
│   │       ├── IServerConfigurationRepository.cs
│   │       └── IPlcServerService.cs
│   │
│   ├── PlcAssistant.Infrastructure/
│   │   ├── Data/
│   │   │   └── PlcDbContext.cs
│   │   ├── Repositories/
│   │   │   ├── PlcMemoryAddressRepository.cs
│   │   │   └── ServerConfigurationRepository.cs
│   │   └── Services/
│   │       └── PlcServerService.cs
│   │
│   ├── PlcAssistant.Server/
│   │   ├── Program.cs
│   │   └── Worker.cs
│   │
│   └── PlcAssistant.MauiApp/
│       ├── Components/
│       │   ├── Layout/
│       │   │   ├── MainLayout.razor
│       │   │   └── NavMenu.razor
│       │   └── Routes.razor
│       ├── Pages/
│       │   ├── Home.razor
│       │   ├── ServerControl.razor
│       │   ├── MemoryAddresses.razor
│       │   └── Configuration.razor
│       ├── MauiProgram.cs
│       └── App.xaml
│
└── PlcAssistant.sln
```

## Note

- L'app MAUI può essere compilata solo su un Mac con i workload MAUI installati
- Il server può essere eseguito su qualsiasi piattaforma supportata da .NET 10
- LiteDB gestisce automaticamente la concorrenza tra server e app MAUI
- I componenti Syncfusion richiedono una licenza (community/commerciale)
