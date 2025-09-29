# PeopleRegistry
PeopleRegistry is a simple end-to-end application for managing people and their contact information.  
It provides functionality to store and query personal details such as names, addresses, and phone numbers.  

## Features
- 📇 Manage people with basic details (first name, last name, date of birth)
- 🏠 Store multiple addresses per person
- 📞 Keep track of phone numbers
- 🔎 Query and filter people easily
- 🛠️ Ready for integration with backend frameworks (e.g., .NET, Java, Python)

## Getting Started

### Prerequisites
- Docker & Docker Compose
- SQL Server (or run via Docker container)
- .NET SDK (if using the .NET backend)

### Run Database
1. Start container

```bash
cd docker
docker compose up -d
```

2. Create databases (both for Task 1 and 2) and seed initial data for Task 1
```
docker exec -i mssql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "PeopleRegistry!" -C -d PeopleDb_Task1 -i /scripts/01_create_tables.sql
```
**Validate if schemas, tables created and data got seeded successfully**
```
docker exec -i mssql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "PeopleRegistry!" -C -d PeopleDb_Task1 -Q "SET NOCOUNT ON; SELECT Id, Vorname, Nachname, Geburtsdatum FROM dbo.Person ORDER BY Nachname;" -W -s "," -w 200

```
⚠️the other .sql script for task 1 cant be successfully execute if this create database step got skipped, cause there will be no database, tables or seeded data to work with

### Run any Task in for Task 1 (Datenbanken and SQL)
**Run any task command**
```
docker exec -i mssql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "PeopleRegistry!" -C -d PeopleDb_Task1 -i /scripts/task_to_run
```
For example:
```
docker exec -i mssql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "PeopleRegistry!" -C -d PeopleDb_Task1 -i /scripts/03_queries.sql
```

**Access to the SQL-CLI of Database PeopleDb_Task1 from MS SQL Container**
```
docker exec -it mssql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "PeopleRegistry!" -d PeopleDb_Task1 -C
```
