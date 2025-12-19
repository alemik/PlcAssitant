# Getting Started with PlcAssistant

## Installazione Prerequisiti

### Windows
```powershell
# Installa .NET 10 SDK
winget install Microsoft.DotNet.SDK.10

# Installa MAUI workload (per l'app UI)
dotnet workload install maui
```

### macOS
```bash
# Installa .NET 10 SDK
brew install --cask dotnet-sdk

# Installa MAUI workload
dotnet workload install maui

# Assicurati di avere Xcode installato
xcode-select --install
```

### Linux (solo Server)
```bash
# Installa .NET 10 SDK
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 10.0

# Note: MAUI non è supportato su Linux
```

## Clonare il Repository

```bash
git clone https://github.com/alemik/PlcAssitant.git
cd PlcAssitant
```

## Build della Soluzione

### Build completo (Server only su Linux)
```bash
dotnet restore
dotnet build
```

### Build Server
```bash
cd src/PlcAssistant.Server
dotnet build
```

### Build MAUI App (solo su macOS/Windows)
```bash
cd src/PlcAssistant.MauiApp
dotnet build -f net10.0-maccatalyst  # macOS
# oppure
dotnet build -f net10.0-windows10.0.19041.0  # Windows
```

## Esecuzione

### Avviare il Server
```bash
cd src/PlcAssistant.Server
dotnet run
```

Per avviare con dati di esempio:
```bash
cd src/PlcAssistant.Server
dotnet run -- --seed
```

Il server si avvierà in background e:
- Creerà automaticamente il database LiteDB
- Leggerà la configurazione dal database
- Se `AutoStart` è abilitato, avvierà automaticamente il server PLC
- Loggerà lo stato ogni 5 secondi
- Con `--seed`, popolerà il database con 5 indirizzi di memoria di esempio

### Avviare l'app MAUI

#### macOS
```bash
cd src/PlcAssistant.MauiApp
dotnet run -f net10.0-maccatalyst
```

#### Windows
```bash
cd src/PlcAssistant.MauiApp
dotnet run -f net10.0-windows10.0.19041.0
```

## Configurazione del Server

### Via UI
1. Avvia l'app MAUI
2. Vai alla pagina "Configuration"
3. Modifica le impostazioni:
   - IP Address (default: 127.0.0.1)
   - Port (default: 502)
   - Update Interval in ms (default: 100)
   - Auto-start (checkbox)
4. Clicca "Save Configuration"

### Via Database
Il file del database si trova in:
- **Server**: `%LocalApplicationData%/PlcAssistant/plc.db` (Windows) o `~/.local/share/PlcAssistant/plc.db` (Linux/macOS)
- **MAUI App**: Dipende dalla piattaforma

Puoi modificare il database usando LiteDB Studio o qualsiasi altro tool per LiteDB.

## Gestione Indirizzi di Memoria PLC

### Via UI (MAUI App)
1. Vai alla pagina "Memory Addresses"
2. Utilizza la toolbar della grid per:
   - **Add**: Aggiungere un nuovo indirizzo
   - **Edit**: Modificare un indirizzo esistente
   - **Delete**: Eliminare un indirizzo

### Campi dell'indirizzo
- **Name**: Nome descrittivo
- **Address**: Indirizzo PLC (es: DB1.DBW0)
- **Data Type**: Bool, Byte, Int, DInt, Real, String, Word, DWord
- **Current Value**: Valore corrente (stringa)
- **Description**: Descrizione opzionale
- **Read Only**: Checkbox per indicare se è in sola lettura

## Controllo del Server PLC

### Via UI (MAUI App)
1. Vai alla pagina "Server Control"
2. Utilizza i pulsanti:
   - **Start Server**: Avvia il server PLC
   - **Stop Server**: Ferma il server PLC
   - **Refresh**: Aggiorna lo stato

Lo stato mostrerà:
- Se il server è in esecuzione o fermo
- Quanti indirizzi di memoria sono configurati

## Troubleshooting

### Il server non si avvia
1. Verifica che la porta configurata non sia già in uso
2. Controlla i log nel terminale
3. Verifica i permessi di scrittura sulla cartella del database

### L'app MAUI non compila
1. Assicurati di avere installato il workload MAUI:
   ```bash
   dotnet workload install maui
   ```
2. Su macOS, assicurati di avere Xcode installato
3. Su Windows, assicurati di avere il Windows SDK installato

### Errori di database
1. Chiudi tutti i processi che accedono al database
2. Elimina il file `plc.db` (verrà ricreato automaticamente)
3. Riavvia l'applicazione

## Sviluppo

### Aggiungere nuove funzionalità
1. Le entità del dominio vanno in `PlcAssistant.Domain/Entities`
2. Le interfacce vanno in `PlcAssistant.Domain/Interfaces`
3. Le implementazioni vanno in `PlcAssistant.Infrastructure`
4. Le pagine Blazor vanno in `PlcAssistant.MauiApp/Pages`

### Modificare l'UI
Le pagine Blazor si trovano in `src/PlcAssistant.MauiApp/Pages/` e utilizzano componenti Syncfusion.

Documentazione Syncfusion: https://blazor.syncfusion.com/documentation/

## Licenza Syncfusion

I componenti Syncfusion richiedono una licenza. Opzioni disponibili:
1. **Community License** - Gratuita per progetti individuali/startup con fatturato < $1M
2. **Commercial License** - Per uso commerciale

Registrati su: https://www.syncfusion.com/sales/communitylicense
