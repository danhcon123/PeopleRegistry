# Task 1 – Datenbanksysteme und SQL

Dieses Verzeichnis enthält SQL-Skripte zur Erstellung, Befüllung und Abfrage einer Datenbank für die Verwaltung von Personen, Anschriften und Telefonverbindungen.

## 📋 Aufgabenstellung

Entwicklung eines relationalen Datenbankmodells im Bereich Energie- und Wasserwirtschaft mit folgenden Anforderungen:

### Entitäten und Beziehungen
- **Person** (Name, Vorname, Geburtsdatum)
- **Anschrift** (Postleitzahl, Ort, Straße, Hausnummer)
- **Telefonverbindung** (Nummer)

**Beziehungen:**
- Eine Person kann mehrere Anschriften haben
- Eine Person kann mehrere Telefonverbindungen haben
- Referentielle Integrität: Personen können nicht gelöscht werden, solange Anschriften oder Telefonnummern existieren

## 🗄️ Datenbank

- **Name:** `PeopleDb_Task1`
- **DBMS:** Microsoft SQL Server 2022
- **Automatische Initialisierung:** Die Datenbank wird beim Start des Docker-Stacks automatisch erstellt und mit Musterdaten befüllt

## 📁 Skript-Übersicht

| Skript | Beschreibung |
|--------|--------------|
| `01_create_tables.sql` | Erstellt die Datenbank und Tabellen mit Primär- und Fremdschlüsseln |
| `02_seed_datas.sql` | Befüllt die Tabellen mit Musterdaten (3 Personen) |
| `03_queries.sql` | Auswerteabfragen (Anzahl Personen, Personen in Dresden, Personen mit mehreren Nummern) |
| `04_number_of_person_in_the_same_city.sql` | Gruppierte Abfrage: Anzahl Personen pro Ort |
| `05_persons_adress_tel_view.sql` | View mit allen Personen inkl. Anschriften und Telefonnummern |
| `06_delete_command.sql` | Löscht Telefonnummern, die nicht mit '0' oder '+' beginnen |
| `07_add_uppercol.sql` | Fügt Spalte `NameUpper` hinzu und befüllt sie mit Großbuchstaben |

## 🚀 Verwendung

### Voraussetzungen
- Docker und Docker Compose

### Konfiguration

**Wichtig:** Vor dem ersten Start muss das Datenbank-Passwort konfiguriert werden.

1. Erstellen Sie eine `.env` Datei im `docker/` Verzeichnis:
```bash
cd docker
cp .env.example .env
```

2. Passen Sie das Passwort in der `.env` Datei an (optional):
```bash
# .env Datei
MSSQL_SA_PASSWORD="PeopleRegistry!1"
```

**Hinweis:** Wenn keine `.env` Datei vorhanden ist, wird automatisch das Default-Passwort `PeopleRegistry!1` verwendet.

### Datenbank starten
```bash
cd docker
docker compose up -d
```
Hierbei startet `Frontend + Backend + MSSQL` gemeinsam.
Während des Startvorgangs werden die Schemas/DBs `Task1_PeopleDb` und `Task2_PeopleDb` erstellt.

Oder für nur die Datenbank (ohne Backend/Frontend - **Lokales Deployment**):
```bash
docker compose -f docker-compose.database.yml up -d
```
Beim Hochfahren werden die Schemas/DBs `Task1_PeopleDb` und `Task2_PeopleDb` erstellt und die Seed-Daten eingespielt, damit Sie sofort Abfragen testen können.

### Musterdaten überprüfen
```bash
# Verwenden Sie das Passwort aus Ihrer .env Datei oder das Default-Passwort

docker exec -i mssql bash -lc '/opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "PeopleRegistry!1" -C \
  -d PeopleDb_Task1 \
  -Q "SET NOCOUNT ON; SELECT Id, Vorname, Nachname, Geburtsdatum FROM dbo.Person ORDER BY Nachname;" \
  -W -s "," -w 200'

```

**Hinweis:** Falls Sie ein anderes Passwort in der `.env` Datei gesetzt haben, ersetzen Sie `PeopleRegistry!1` in allen Befehlen entsprechend.

### Einzelnes Skript ausführen
Beispiel für die Ausführung der Abfragen:
```bash
docker exec -i mssql bash -lc '/opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "PeopleRegistry!1" \
  -C \
  -i /scripts/03_queries.sql'
```

### Interaktive SQL-Shell
```bash
docker exec -it mssql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'PeopleRegistry!1' -C -d PeopleDb_Task1

# Zum Verlassen
QUIT
```

In der Shell können Sie SQL-Befehle direkt eingeben:
```sql
SELECT * FROM dbo.Person;
GO
```

## 📊 Musterdaten

Die Datenbank wird mit folgenden Testdaten befüllt:

**Personen:**
- Anna Müller (geb. 12.04.1997) – 2 Anschriften in Dresden, 2 Telefonnummern
- Peter Schmidt (geb. 30.09.1999) – 1 Anschrift in Berlin, 1 Telefonnummer
- Lena Keller (geb. 05.01.1998) – 1 Anschrift in München, 1 Telefonnummer


## 🛠️ Technische Details

- **Container-Name:** `mssql`
- **Port:** 1433
- **SA-Passwort:** Konfigurierbar über `.env` Datei (Default: `PeopleRegistry!1`)
- **Skript-Mount:** `task_1_sql/` → `/scripts` (read-only)
- **Datentypen:** UNIQUEIDENTIFIER (PKs), NVARCHAR (Texte), DATE (Geburtsdatum)

### Passwort-Konfiguration
- Das Passwort wird über die Umgebungsvariable `MSSQL_SA_PASSWORD` gesetzt
- Beide Compose-Dateien (`docker-compose.yml` und `docker-compose.database.yml`) verwenden diese Variable
- Ohne `.env` Datei wird automatisch `PeopleRegistry!1` als Default verwendet
- **Wichtig:** In Produktionsumgebungen sollte ein sicheres Passwort verwendet werden!

## ✅ Erfüllte Anforderungen

- ✓ Relationales Datenbankmodell mit 3 Tabellen
- ✓ Geeignete Datentypen für alle Attribute
- ✓ 1:n-Beziehungen zwischen Person und Anschrift/Telefonverbindung
- ✓ Referentielle Integrität (ON DELETE NO ACTION)
- ✓ Musterdaten zur Demonstration
- ✓ Alle geforderten Abfragen implementiert
- ✓ View für kombinierte Ausgabe
- ✓ Löschbefehl für ungültige Telefonnummern
- ✓ Erweiterung der Person-Tabelle um NameUpper-Spalte

## Other useful Docker commands

```bash
# Check laufende Containers
docker ps

# Schaue Container logs an
docker logs mssql

# Open interactive shell inside the container
docker exec -it mssql /bin/bash

# Liste alle Docker-Netzwerk
docker network ls

# Stopp und Lösch Speicher
docker compose -f docker-compose.databases.yml down -v

# Schnell Query zum Laden alle Personen
docker exec -i mssql /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'PeopleRegistry!1' -C -d PeopleDb_Task1 \
  -Q "SET NOCOUNT ON; SELECT Id, Vorname, Nachname, Geburtsdatum FROM dbo.Person ORDER BY Nachname;" \
  -W -s "," -w 200

```