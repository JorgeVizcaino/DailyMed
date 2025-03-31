
# Demo
https://github.com/user-attachments/assets/e0d899f6-fd91-4f95-95c1-43c1b0d7ceca

# Dupixent Data Extraction & Copay API

This repository implements a microservice-based application that:

1. **Extracts drug indications** from DailyMed (specifically for Dupixent).  
2. **Uses AI** (OpenAI) to parse, summarize, and infer details for coverage requirements or free-text fields.  
3. **Maps** them to ICD-10 codes.  
4. **Parses** MyWay Copay Card JSON or data to store structured information in a database.  
5. Provides a **CRUD** API (with `GET`, `POST`, etc.) that returns **structured JSON** for the copay program.  
6. Implements **JWT-based** authentication and role-based authorization.  
7. Uses **TDD** principles and follows aspects of **Clean Architecture**.

Below is an overview of the solution structure, usage instructions, and database setup.

---

## 1. Architecture & Folder Structure

This project uses a simplified **Clean Architecture** approach with the following projects/folders:

1. **`DailyMed.Core`**  
   - Contains **domain entities**, **interfaces**, and **Models**.  
   - E.g., `IIndicationsParser`, `IDailyMedRepository`, **models** like `SplDailyMedData`.

2. **`DailyMed.Infrastructure`**  
   - Houses **data access** and **implementation** details (e.g., ADO.NET code for reading/writing to SQL).  
   - E.g., `DailyMedRepository.cs` for hitting the DailyMed or FDA endpoints, `ProgramDataRepository.cs` for storing and retrieving copay program data in SQL.

3. **`DailyMed.API`**  
   - The **ASP.NET Core** Web API project.  
   - Contains **Controllers**, **JWT** Auth setup, **startup** code.  
   - Exposes endpoints like `POST /api/coprogram` or `GET /api/coprogram/{id}`.

4. **`DailyMed.Tests`**  
   - **Unit tests** for the Core services and/or Infrastructure repositories.  
   - Uses e.g. `xUnit` or `MSTest` with TDD approach.

Example solution structure:

```
- DailyMed.sln
- DailyMed.Core/
  - Interfaces/
  - Models/
  - Services/
- DailyMed.Infrastructure/
  - Data/
  - Repositories/
- DailyMed.API/
  - Controllers/
  - Program.cs or Startup.cs
  - appsettings.json
- DailyMed.Tests/
  - CoreTests/
  - InfrastructureTests/
  - ...
- README.md
```

---

## 2. Important Patterns & AI Integration

1. **Clean Architecture**  
   - Separate domain logic (Core) from data/implementation details (Infrastructure) and from the Web API (Presentation layer).

2. **Test-Driven Development (TDD)**  
   - Unit tests in `DailyMed.Tests` that validate the `Infrastructure`.

3. **JWT Authentication**  
   - We protect certain endpoints with `[Authorize]` and roles (`[Authorize(Roles="admin")]`).

4. **ADO.NET**  
   - Used for direct **SQL** queries and DDL statements to manage the data layer.  
   - We prefer parameterized queries to avoid injection.

5. **Serialization/Deserialization**  
   - We store nested arrays and objects in the DB as JSON strings (e.g., coverageEligibilities, forms, etc.).

6. **OpenAI / Generative AI**  
   - We call the **OpenAI API** from within certain services (e.g., to parse or summarize free-text eligibility).  
   - You **must** provide an **OpenAI API Key** in your `appsettings.json` for these AI calls to function.

---

## 3. How to Run

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet).  
- SQL Server or local SQL instance for the database.  
- `Visual Studio`, `VS Code`, or any .NET-friendly editor.  
- A valid **OpenAI API Key** if you want to use the AI features.

### Steps

1. **Clone** the repository:
   ```bash
   git clone https://github.com/JorgeVizcaino/DailyMed.git
   cd DailyMed
   ```

2. **Set up** the database:
   - Ensure you have a connection string in `appsettings.json` or in your user-secrets that matches your local DB.
   - Run the **table creation** scripts below.

3. **Add your OpenAI Key**:
   - In `appsettings.json`, add a section similar to:
     ```json
     "OpenAI": {
       "ApiKey": "<YOUR_OPENAI_API_KEY_HERE>"
     }
     ```
   - Or store it in **User Secrets** to avoid committing secrets to source control.

4. **Build** the solution:
   ```bash
   dotnet build
   ```

5. **Run tests** (if you have xUnit or MSTest):
   ```bash
   dotnet test
   ```

6. **Run** the Web API:
   ```bash
   cd DailyMed.WebAPI
   dotnet run
   ```
   or press **F5** in Visual Studio.

7. **Swagger** UI  
   - Navigate to <http://localhost:5081/swagger> (or whichever port the app logs to).  
   - You can test endpoints there, including passing a **Bearer** token for protected routes.

---

## 4. Creating the SQL Tables

Below is an example **T-SQL script** to create the main table `ProgramData` for storing the copay program JSON structure:

```sql
CREATE TABLE [dbo].[DrugIndications] 
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [DrugName] NVARCHAR(200) NOT NULL,
    [Indication] NVARCHAR(500) NOT NULL,
    [Setid] NVARCHAR(80) NOT NULL
);


CREATE TABLE [dbo].[Users]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Username] NVARCHAR(100) NOT NULL,
    [PasswordHash] NVARCHAR(200) NOT NULL,
    [Role] NVARCHAR(50) NULL
);


CREATE TABLE [dbo].[ProgramData] (
    [ProgramId]                  INT            IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [ProgramName]                NVARCHAR(200)  NOT NULL,
    [CoverageEligibilitiesJson]  NVARCHAR(MAX)  NULL,
    [ProgramType]                NVARCHAR(100)  NULL,
    [RequirementsJson]           NVARCHAR(MAX)  NULL,
    [BenefitsJson]               NVARCHAR(MAX)  NULL,
    [FormsJson]                  NVARCHAR(MAX)  NULL,
    [FundingEvergreen]           BIT            NULL,
    [FundingCurrentFundingLevel] NVARCHAR(200)  NULL,
    [DetailsJson]                NVARCHAR(MAX)  NULL
);
```
## 5. API Endpoints

Here is a quick reference of the primary endpoints:

- **`POST /api/coprogram`**  
  Creates a new program in the DB. Expects a JSON body matching `ProgramData`.

- **`GET /api/coprogram/{id}`**  
  Retrieves a single program by `ProgramId`. Returns the entire structured JSON.

- **`PUT /api/coprogram/{id}`**  
  Updates an existing program.

- **`DELETE /api/coprogram/{id}`**  
   Removes the program from DB.

- **`POST /api/users/register`**  
   Registers a user, hashing their password.
    
      {
        "username": "Jorge",
        "password": "P@$$w0rd",
        "role": "admin"
      } 

- **`POST /api/users/login`**  
  Logs a user in, returning a **JWT** token.
      
      {
        "username": "jorge",
        "password": "P@$$w0rd"
      }

- AI-specific endpoints that return coverage eligibilities and requirements after analysis by AI.
  - `GET /api/Programs/StandardizeAIRequirements` (this calls OpenAI).

- AI-specific Get dupixent-indications endpoints to get the dupixent indication:
  - `GET //api/Programs/dupixent-indications` (that calls OpenAI and "admin" Roles).

- Insert mock data in the program table to return the structure:
  - `GET /api/Programs/CreateMockProgram` ("admin" Roles).


    
    
---

## 6. Authentication & Authorization

1. **JWT**:  
   - After login, you get a token. You must include it as `Authorization: Bearer <token>` to call secured endpoints.

2. **Role-based**:  
   - Certain endpoints use `[Authorize(Roles = "admin")]`. You must be an **admin** user in your token claims.

---

## 7. AI & OpenAI Integration

- The application uses **OpenAI** to parse, summarize, or infer missing data.  
- Make sure your **OpenAI API key** is set in `appsettings.json` under an `"OpenAI": { "ApiKey": "..." }` section, or in user secrets.  
- If the AI calls fail or you omit the key, the relevant features (like free-text summarizing) will not work.

---

## 8. Unit Testing (TDD)

- The `DailyMed.Infrastructure.Tests` project includes sample tests covering the **repositories** (DailyMed, ProgramData) and the **services** (e.g., IndicationsParser, AI summation).
- You can run them using:
  ```bash
  dotnet test
  ```

---


## 9. Running with Docker Compose

This repository also includes a `Dockerfile` (to build the .NET 8 image for the API) and a `docker-compose.yml` that orchestrates the containers.

### Prerequisites

- [Docker](https://www.docker.com/) installed on your machine.
- [Docker Compose](https://docs.docker.com/compose/) (usually bundled with Docker Desktop).

### Steps

1. **Add** your connection strings and OpenAI key in an environment.  
   
2. **Build & Run**:

   ```bash
   docker-compose build
   docker-compose up
   ```

   - `docker-compose build` uses your local `Dockerfile` and `docker-compose.yml`.
   - `docker-compose up` starts the containers.

3. **Confirm** containers are running:
   - `docker ps` to see the running containers.
   - The `.NET API` container typically listens on port 80 or 8080 (depending on your Dockerfile).  
   - The `SQL Server` container, if included, typically listens on port 1433.

4. **Access** the API via:
   - <http://localhost:8081/swagger> 
