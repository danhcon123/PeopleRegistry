### `task_2_webapp/README.md`

# Task 2 – Web App (Frontend + Backend)

Task 2 is a .NET web app with:
- **Backend API** (ASP.NET Core, EF Core)
- **Frontend** (Blazor Server)
- Uses DB: `PeopleDb_Task2` (migrations run automatically on backend startup)

## Run with Docker (recommended)
```bash
cd docker
docker compose up -d --build
```

Endpoints:

* Backend health: [http://localhost:8080/health](http://localhost:8080/health)
* Frontend UI:    [http://localhost:5173](http://localhost:5173)

## Configuration

* Backend connection string is provided via Docker env:

  ```
  ConnectionStrings__Default=Server=mssql;Database=PeopleDb_Task2;User Id=sa;Password=${MSSQL_SA_PASSWORD};TrustServerCertificate=True;Encrypt=False;
  ```
* Frontend calls backend via Docker DNS:

  ```
  Backend__BaseUrl=http://backend:8080
  ```

## Local dev (optional)

You can also run without Docker:

* Start SQL in Docker (or local SQL Server)
* Set `ConnectionStrings:Default` in `appsettings.Development.json`
* Run backend:

  ```bash
  cd task_2_webapp/src/Backend/PeopleRegistry.Api
  dotnet clean
  dotnet build
  dotnet run
  ```
* Run frontend:

  ```bash
  cd task_2_webapp/src/Frontend
  dotnet clean
  dotnet build
  dotnet run
  ```

> Ensure `Backend:BaseUrl` in the frontend config points to your backend (e.g., `http://localhost:8080`).



If you want, I can also generate a tiny `.env.example` for `docker/`:

```ini
# docker/.env.example
MSSQL_SA_PASSWORD=PeopleRegistry!1
