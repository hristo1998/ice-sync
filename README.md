# IceSync

IceSync is a **.NET 8 + Angular 18** application that integrates with the **Universal Loader API** to manage workflows for **The Ice Cream Company**.  
It provides a backend for syncing workflow data and a frontend for monitoring and running workflows.

---

## 🚀 Features

### Backend (IceSync.Api, .NET 8)
- Connects to **Universal Loader API** using **JWT authentication** (auto refreshes tokens).
- Exposes clean REST endpoints under `/api/...` for the Angular frontend.
- Provides workflow operations:
  - List workflows
  - Run workflow
  - View workflow executions and steps
  - Manage workflow states (CRUD)
- Background service to **sync workflows every 30 minutes** with SQL Server:
  - Insert new workflows
  - Update existing workflows (only if changed)
  - Delete missing workflows
- Repository + Unit of Work patterns for data access.
- EF Core **code-first** with migrations.

### Frontend (IceSync.UI, Angular 18)
- Single-page Angular app.
- Displays workflows in a **Material table**:
  - Workflow Id  
  - Workflow Name  
  - Is Active  
  - Multi Exec Behavior  
- Button to run each workflow directly.
- Shows success/error notifications.
- Uses Angular `HttpClient` and service layer to consume backend.

---

## 🛠️ Tech Stack

- **Backend**
  - .NET 8 Web API
  - EF Core (SQL Server)
  - Repository & Unit of Work pattern
  - BackgroundService for scheduled sync
- **Frontend**
  - Angular 18 (standalone components)
  - Angular Material
  - HttpClient for API communication
- **Database**
  - SQL Server (code-first migrations)

---

## ⚙️ Setup

### Backend
1. Update `appsettings.json` with:
   - SQL Server connection string
   - Universal Loader API credentials
2. Run EF migrations:
   ```bash
   dotnet ef database update -p IceSync.Data -s IceSync.Api
3. Start the API:
    ```bash
    dotnet run --project IceSync.Api

### Frontend
1. Install dependencies
    ```bash
    npm install
2. Run Angular dev server
    ```bash
    ng serve
3. Open http://localhost:4000

## 🔄 Sync Process
Every 30 minutes, the backend:
1. Calls Universal Loader /workflows endpoint.
2. nserts new workflows into SQL Server.
3. Updates existing ones only if changes are detected.
4. Deletes workflows missing from the API.