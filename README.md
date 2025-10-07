# PeopleRegistry

PeopleRegistry ist eine einfache End-to-End-Anwendung zur Verwaltung von Personen und deren Kontaktinformationen. Sie bietet Funktionen zum Speichern und Abfragen persönlicher Daten wie Namen, Adressen und Telefonnummern.

## Funktionen

- 📇 Verwaltung von Personen mit grundlegenden Details (Vorname, Nachname, Geburtsdatum)
- 🏠 Speicherung mehrerer Adressen pro Person
- 📞 Verwaltung von Telefonnummern
- 🔎 Einfaches Abfragen und Filtern von Personen
- 🐳 Docker-ready für schnelle Bereitstellung
- 🗄️ Zwei separate Datenbanken für verschiedene Lernaufgaben

## Tech Stack

- **Backend**: .NET 8+ mit Entity Framework Core
- **Frontend**: Modernes Web-Framework (Blazor)
- **Datenbank**: Microsoft SQL Server
- **Containerisierung**: Docker & Docker Compose
## Aufgaben-spezifische Dokumentation

Dieses Repository enthält zwei separate Lernaufgaben mit jeweils eigener detaillierter Dokumentation:

### 📁 Aufgabe 1: SQL-Grundlagen
**Speicherort**: `task_1_sql/README.md`

Diese README enthält:
- SQL-Übungsaufgaben und Herausforderungen
- Schritt-für-Schritt-Anleitungen für SQL-Abfragen
- Erwartete Ergebnisse und Lösungshinweise
- Best Practices für SQL-Entwicklung

### 📁 Aufgabe 2: Full-Stack-Entwicklung
**Speicherort**: `task_2_webapp/README.md`

Diese README enthält:
- Backend-Architektur und API-Dokumentation
- Frontend-Komponentenstruktur
- Entwicklungsworkflow und Coding-Standards
- Testing-Strategien
- Deployment-Anweisungen

## Repository-Struktur

```
PeopleRegistry/
├── task_1_sql/          # Nur-SQL-Übungen für Aufgabe 1
│   ├── README.md        # Detaillierte Anweisungen für Aufgabe 1
│   ├── 01_create_tables.sql
│   └── 02_seed_data.sql
│   └── ...
├── task_2_webapp/       # Full-Stack-Anwendung für Aufgabe 2
│   ├── README.md        # Detaillierte Anweisungen für Aufgabe 2
│   ├── backend/         # .NET API mit EF Core
│   └── frontend/        # Web-UI
└── docker/              # Docker Compose-Konfiguration
    ├── docker-compose.yml
    ├── docker-compose.database.yml
    └── .env.example
```

### Datenbank-Architektur

Die Anwendung verwendet **zwei separate Datenbanken**:

- **`PeopleDb_Task1`** - Erstellt über SQL-Skripte für SQL-Lernübungen
- **`PeopleDb_Task2`** - Erstellt über EF Core-Migrationen für die vollständige Anwendung

## Schnellstart

### Voraussetzungen

- [Docker](https://docs.docker.com/get-docker/) (Version 20.10+)
- [Docker Compose](https://docs.docker.com/compose/install/) (Version 2.0+)
- (Optional) .NET SDK 8.0+ für lokale Entwicklung

### Mit Docker ausführen (Empfohlen)

1. **Repository klonen**
   ```bash
   git clone https://github.com/danhcon123/PeopleRegistry.git
   cd PeopleRegistry
   ```

2. **Umgebungsvariablen einrichten**
   ```bash
   cd docker
   cp .env.example .env
   ```
   
   Oder `.env` manuell erstellen:
   ```bash
   echo "MSSQL_SA_PASSWORD=PeopleRegistry!1" > .env
   ```

3. **Alle Services starten**
   ```bash
   docker compose up -d --build
   ```

   Dieser Befehl wird:
   - SQL Server-Container starten
   - `PeopleDb_Task1` mit SQL-Skripten initialisieren
   - Backend-API erstellen und starten (Port 8080)
   - Frontend erstellen und starten (Port 5173)
   - `PeopleDb_Task2` über EF Core-Migrationen erstellen

4. **Anwendung aufrufen**

   - Frontend: [http://localhost:5173](http://localhost:5173)
   - Backend health: [http://localhost:8080/health](http://localhost:8080/health)
   - Backend-API: [http://localhost:8080/swagger](http://localhost:8080/swagger)

### Überprüfung

Prüfen, ob alle Services laufen:
```bash
docker compose ps
```

Sie sollten drei Container sehen: `mssql`, `backend` und `frontend` - alle mit Status "Up".

## Mit der Anwendung arbeiten

### Aufgabe 1: SQL-Übungen

**Weitere Details**: Siehe `task_1_sql/README.md` für spezifische Übungsanweisungen und Lösungshinweise.

### Aufgabe 2: Web-Anwendung

**Weitere Details**: Siehe `task_2_webapp/README.md` für Backend-/Frontend-Architektur, API-Endpunkte und Entwicklungsrichtlinien.

## Docker-Befehle Referenz

```bash
# Logs anzeigen
docker compose logs -f              # Alle Services
docker compose logs -f mssql        # Nur SQL Server
docker compose logs -f backend      # Nur Backend-API
docker compose logs -f frontend     # Nur Frontend

# Services stoppen
docker compose stop                 # Stoppen ohne Container zu entfernen
docker compose down                 # Stoppen und Container entfernen

# Vollständiger Reset (inkl. Datenbankdaten)
docker compose down -v              # Volumes entfernen (⚠️ löscht alle Daten)

# Nach Code-Änderungen neu erstellen
docker compose up -d --build

# Service-Status prüfen
docker compose ps
```

## Troubleshooting

### Port-Konflikte
Wenn die Ports 8080, 5173 oder 1433 bereits belegt sind:
```bash
# Prüfen, was den Port verwendet (Beispiel für Port 8080)
lsof -i :8080          # macOS/Linux
netstat -ano | find "8080"  # Windows

# Ports in docker-compose.yml anpassen vor dem Start
```

### Datenbankverbindungsprobleme
```bash
# Prüfen, ob SQL Server bereit ist
docker compose logs mssql | grep "SQL Server is now ready"

# SQL Server-Container neu starten
docker compose restart mssql
```

### Backend startet nicht
```bash
# Backend-Logs prüfen
docker compose logs backend

# Häufige Probleme:
# - Datenbank nicht bereit (30s warten und erneut versuchen)
# - Migrationsfehler (Connection-String prüfen)
```

### Frontend kann Backend nicht erreichen
Überprüfen Sie, ob die Backend-URL in der Frontend-Konfiguration `http://localhost:8080` entspricht

## Datenbankschema

### Haupttabellen
- **People** - Speichert Personendaten (ID, FirstName, LastName, DateOfBirth)
- **Addresses** - Mehrere Adressen pro Person (Street, City, State, ZipCode)
- **PhoneNumbers** - Telefonkontakte (Number, Type)

Siehe `task_1_sql/01_create_tables.sql` für das vollständige Schema.