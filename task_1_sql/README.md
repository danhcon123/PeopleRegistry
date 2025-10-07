### `task_1_sql/README.md`

# Task 1 – SQL

This folder contains SQL scripts for creating/seeding the **Task 1** database and running queries.

## Database
- Name: `PeopleDb_Task1`
- Created & seeded automatically by the Docker stack via:
  - `01_create_tables.sql`
  - `02_seed_datas.sql`

## Validate seeded data
```bash
# From repo root or docker/
docker exec -i mssql /opt/mssql-tools/bin/sqlcmd \
  -S mssql -U sa -P "PeopleRegistry!1" \
  -Q "SET NOCOUNT ON; SELECT Id, Vorname, Nachname, Geburtsdatum FROM dbo.Person ORDER BY Nachname;"
```
## Run a specific task script

Replace the filename with the task you want (e.g., `03_queries.sql`):

```bash
docker exec -i mssql /opt/mssql-tools/bin/sqlcmd \
  -S mssql -U sa -P "PeopleRegistry!1" -d PeopleDb_Task1 \
  -i /scripts/03_queries.sql
```

## Interactive SQL shell

```bash
docker exec -it mssql /opt/mssql-tools/bin/sqlcmd \
  -S mssql -U sa -P "PeopleRegistry!1" -d PeopleDb_Task1
```

> The container name is `mssql`. SQL scripts are mounted read-only from `task_1_sql/` into `/scripts` inside the `mssql_init` runner.