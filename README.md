# ContactHub
ContactHub is a simple end-to-end application for managing people and their contact information.  
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
docker-compose.yml up -d
```

2. Create both databases and seed initial data for Task 1
```
docker exec -i mssql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "PeopleRegistry!" -C -i /scripts/00_create_databases.sql
docker exec -i mssql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "PeopleRegistry!" -C -d PeopleDb_Task1 -i /scripts/01_create_table.sql
```

3. run Task 1 commands
```
docker exec -i mssql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Admin!" -C -d PeopleDb_Task1 -i /scripts/task_to_run
```
For example:
```
docker exec -i mssql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Admin!" -C -d PeopleDb_Task1 -i /scripts/03_queries.sql
```

4. Access to the SQL-command inside container
```
docker exec -it mssql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong!Passw0rd" -d PeopleDb_Task1 -C
```
