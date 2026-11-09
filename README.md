# RaceDay — Part 1 + Part 2

RaceDay is a race-event management REST API built with ASP.NET Core Web API, Entity Framework Core, SQL Server, JWT authentication, role-based authorization, Swagger/OpenAPI, and automated tests.

## Part 2 implementation

- Registration and login with PBKDF2 password hashing; plain-text passwords are never stored or returned.
- JWT bearer authentication with `Organiser` and `Participant` roles.
- User profile retrieval and update.
- Public event listing/details; organisers can create, update, and delete their own events.
- Event categories for distance/age groups.
- Participant enrolment, with duplicate-enrolment protection.
- Organiser-only enrolment listing for their events.
- Organiser result capture and participant-only personal results.
- Route metadata including GPX URL, elevation, start coordinates, and a weather lookup reference.
- Swagger UI with JWT bearer authorization.
- xUnit tests and GitHub Actions CI build/test workflow.

## Requirements

- .NET 8 SDK
- Visual Studio 2022 (ASP.NET and web development workload) or VS Code
- SQL Server / SQL Server Express / LocalDB
- Git and a GitHub account

## Run locally in Visual Studio

1. Open `RaceDay.sln`.
2. Confirm the connection string in `src/RaceDay.Api/appsettings.json` matches your SQL Server installation.
3. Set `RaceDay.Api` as the startup project.
4. Run the project. In Development, EF Core creates the database schema if the database is new.
5. Open Swagger at the launch URL, normally `https://localhost:7190/swagger`.

The default connection string uses SQL Server LocalDB. If you use SQL Server Express, change it to an appropriate connection string, for example:
`Server=.\SQLEXPRESS;Database=RaceDayDB;Trusted_Connection=True;TrustServerCertificate=True;`

> Important: `EnsureCreated()` is intended for a fresh development database and does not update an existing database schema. If you already created the Part 1 database manually, use a fresh database for this EF Code-First demo or create and apply EF Core migrations before using an existing database.

## First-time API use

1. `POST /api/auth/register` to create an account.
2. For the role, use `Organiser` or `Participant`.
3. `POST /api/auth/login` with the same email/password.
4. Copy the returned `token`.
5. In Swagger, click **Authorize** and paste the token (or `Bearer <token>` if prompted).
6. Test the endpoints permitted to that role.

Example registration body:
```json
{
  "firstName": "Ayesha",
  "lastName": "Runner",
  "email": "ayesha@example.test",
  "password": "StrongPass!123",
  "role": "Participant"
}
```

Registration currently allows a caller to choose either supported role to align with the assignment's registration requirement. For a real public deployment, organiser accounts should instead be approved by an administrator.

## API endpoint overview

| Method | Endpoint | Access |
|---|---|---|
| POST | `/api/auth/register` | Public |
| POST | `/api/auth/login` | Public |
| GET / PUT | `/api/users/profile` | Authenticated |
| GET | `/api/events` | Public |
| GET | `/api/events/{id}` | Public |
| POST | `/api/events` | Organiser |
| PUT / DELETE | `/api/events/{id}` | Owning Organiser |
| POST | `/api/events/{id}/categories` | Owning Organiser |
| GET | `/api/events/{id}/enrolments` | Owning Organiser |
| POST | `/api/enrolments` | Participant |
| GET | `/api/enrolments/me` | Participant |
| POST | `/api/results` | Organiser of event |
| GET | `/api/results/me` | Participant |
| GET | `/api/categories/{categoryId}/route` | Public |
| PUT | `/api/categories/{categoryId}/route` | Organiser |
| GET | `/health` | Public |

## Database model

The Code-First model includes the seven Part 1 entities: Roles, Users, Events, Categories, Routes, Enrolments, and Results. Entity relationships, unique constraints, and seeded roles are configured in `RaceDayDbContext`.

The original Part 1 planning deliverables remain in `docs/`:
- `docs/ERD.svg`
- `docs/EndpointPlan.md`
- `docs/schema.sql`

## Build and test

From the repository root:

```bash
dotnet restore RaceDay.sln
dotnet build RaceDay.sln --configuration Release
dotnet test RaceDay.sln --configuration Release
```

The tests cover password hashing/verification, seeded roles and relationships, registration response safety, event creation, and role-based rejection. GitHub Actions runs restore, build, and tests on pushes and pull requests.

## GitHub submission checklist

- [ ] Push the repository to your own GitHub account.
- [ ] Confirm the **RaceDay API CI** workflow passes under the Actions tab.
- [ ] Add a screenshot of the successful green workflow run to this README or `docs/ci-success.png` after your own workflow passes.
- [ ] Record and add your unlisted YouTube demonstration link after recording the running API, Swagger endpoints, authentication, role restrictions, and tests.
- [ ] Make at least 20 meaningful commits from your own GitHub account as required by the assignment. Commits should reflect genuine development steps, not empty/repeated changes.

## Suggested demonstration flow

1. Show the solution and folder structure.
2. Run the API and open Swagger.
3. Register/login as Participant and show that organiser-only event creation is forbidden.
4. Register/login as Organiser and create an event/category.
5. Enrol the participant and record/view results.
6. Run `dotnet test` and show the passing test output.
7. Show the successful GitHub Actions run.

## Security and deployment notes

The key in `appsettings.json` is for local development only. Replace it with a long random secret stored in user-secrets or environment variables before deployment. Use HTTPS in production, configure a production SQL Server connection securely, and do not commit real credentials or personal data.


## First-time setup (Visual Studio 2022)

1. Install the **.NET 8 SDK** and Visual Studio 2022 with the **ASP.NET and web development** workload.
2. Open `RaceDay.sln`. In Solution Explorer, right-click the solution and choose **Restore NuGet Packages**.
3. In SQL Server Management Studio (SSMS), run `docs/schema.sql` against your local SQL Server. This script recreates the tables and includes the columns used by the API. **It deletes/recreates the project tables and seed data in RaceDayDB**, so do not run it against data you need to keep.
4. Check that `src/RaceDay.Api/appsettings.json` uses a SQL Server instance available on your computer. The default is `(localdb)\\MSSQLLocalDB`, which is available with many Visual Studio installations. If you use a different instance, update the `RaceDay` connection string.
5. Set `RaceDay.Api` as the startup project and press **F5**. Swagger should open. If it does not, browse to `https://localhost:7190/swagger` or use the URL shown in the Output window.
6. To run tests, choose **Test → Run All Tests** in Visual Studio, or run `dotnet test` in a terminal from the folder containing `RaceDay.sln`.

### Important setup notes

- Register new test accounts through `POST /api/auth/register`; the mock password hashes in the SQL seed data are placeholders and cannot be used to log in.
- The development JWT key is for local coursework testing only. Do not deploy it to a public production service.
- If the database already exists with an older schema, rerun the updated `docs/schema.sql` after backing up anything important. EF Core `EnsureCreated()` does not upgrade existing tables.
- I corrected the C# LocalDB connection-string escape issue in `Program.cs`. Run the build and tests on your computer before submitting, because the .NET SDK is not available in this file-generation environment.

# Part 3 — RaceDay MVC web application

Part 3 adds an ASP.NET Core MVC front end which consumes the Part 2 REST API. The MVC application does not access the SQL database directly; all business operations are sent to the API over HTTP.

## Part 3 features implemented

- MVC home page with upcoming event cards and event-type/date filters.
- Registration and login through `/api/auth/register` and `/api/auth/login`.
- JWT stored in server-side ASP.NET Core Session; role and display name are kept in session. Protected MVC actions check the session role before calling organiser/participant API operations.
- Separate Organiser and Participant dashboards and role-specific navigation.
- Organiser event creation, editing and deletion; category creation; entrant list; result capture.
- Participant event detail/category view, enrolment, personal enrolment list, and personal results.
- Profile editing and profile-picture upload.
- Azure Blob Storage API endpoints for organiser event banners and authenticated user profile images. Only JPG, PNG and WebP are accepted, with a 5 MB limit.
- Responsive RaceDay visual theme, validation messages, confirmation before event deletion, and consistent navigation.
- Dockerfiles for the API and MVC application plus Docker Compose for SQL Server, API and MVC.

## Run the complete solution locally

1. Install the .NET 8 SDK and SQL Server (or use the Docker option below).
2. Open `RaceDay.sln` in Visual Studio 2022. Set `RaceDay.Api` and `RaceDay.Mvc` as multiple startup projects, or start the API first and then MVC.
3. Ensure the API connection string in `src/RaceDay.Api/appsettings.json` points to your SQL Server. The MVC app calls `http://localhost:8080/` by default; for local Visual Studio use `Api:BaseUrl` in `src/RaceDay.Mvc/appsettings.json` and set it to the API HTTP launch URL `http://localhost:5190/` (the default is already configured).
4. Run the API and then MVC. Browse to the MVC launch URL. The API Swagger UI is available at `/swagger` in Development.
5. Register an Organiser account, sign in, create an event, add one or more categories, then sign out and register a Participant account to enrol. Sign back in as the Organiser to view enrolments and capture results.

## Existing Part 2 database upgrade

If you are keeping your existing Part 2 database, run `docs/part3-upgrade.sql` against `RaceDayDB` before starting the updated API. It adds only the two nullable image URL columns and preserves existing records. For a brand-new database, the EF model creates these columns automatically. `docs/schema.sql` has also been updated for a fresh manual database setup.

## Azure Blob Storage setup

Create an Azure Storage account and obtain its connection string. Configure it as an environment variable on the API process:

- Windows PowerShell: `$env:AzureBlobStorage__ConnectionString = "<your connection string>"`
- Linux/macOS: `export AzureBlobStorage__ConnectionString="<your connection string>"`

The API creates `event-banners` and `profile-images` containers when first used. Do not commit a real storage connection string to GitHub. If storage is not configured, image-upload requests return a clear service-unavailable response; the rest of the application can still be used.

## Run with Docker Compose

From the repository root, create a `.env` file (do not commit it) with strong local values:

```env
MSSQL_SA_PASSWORD=Replace_With_A_Strong_Local_Password_123!
JWT_KEY=Replace_With_A_Random_Secret_At_Least_32_Characters
AZURE_BLOB_CONNECTION_STRING=
```

Then run:

```bash
docker compose up --build
```

- MVC website: `http://localhost:8081`
- API: `http://localhost:8080`
- Swagger: `http://localhost:8080/swagger`
- SQL Server: `localhost,1433`

The Compose API runs in Development so EF Core creates the schema for a new Docker database. For a previously populated database, apply the SQL upgrade script before switching to the new application version.

## CI/CD

The existing `.github/workflows/ci.yml` builds and tests the API. It must also build the MVC project. Replace the workflow with the updated solution-level restore/build/test commands below, or add the MVC project to the existing commands:

```yaml
- name: Restore
  run: dotnet restore RaceDay.sln
- name: Build
  run: dotnet build RaceDay.sln --configuration Release --no-restore
- name: Test
  run: dotnet test RaceDay.sln --configuration Release --no-build
```

The workflow must run on pushes and pull requests. The included MVC application has no direct database connection and uses the same solution-level build pipeline.

## Submission items you must complete from your own accounts

- Push the project to your own ARC/GitHub repository and submit that repository URL. I cannot create commits or publish to your account from this workspace.
- Make at least 20 meaningful commits as required by the brief. Commit genuine milestones (MVC setup, API client, authentication, dashboards, event CRUD, categories, enrolments, results, Blob Storage, Docker, styling, documentation and fixes); do not manufacture empty commits.
- Run the solution and verify the GitHub Actions workflow is green before taking a screenshot.
- Record and upload an unlisted YouTube demonstration. Show the solution structure, API/MVC running, both roles, event creation, category enrolment, organiser entrant list, result capture, Blob upload (when configured), and Docker startup. Add your real video URL to the submission.

## Important verification note

The development environment used to prepare this archive did not have the .NET SDK installed, so `dotnet restore`, `dotnet build`, and `dotnet test` could not be executed here. Run those commands in Visual Studio or on a machine with .NET 8 before submitting, and fix any environment-specific connection strings or launch ports. Never submit real secrets in source control.

For Azure Storage accounts that block anonymous access by default, enable blob-level public access on the storage account if you want the returned image URLs to render directly in browsers. Use a dedicated account/container for assignment media and do not store private documents in these public image containers.
