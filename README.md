<<<<<<< HEAD
# Student Management REST API — Experiment 10

ASP.NET Core Web API project based on the uploaded .NET Technologies Experiment 10 PDF.

## Requirements
- Visual Studio 2022 (ASP.NET and web development workload), or .NET 8 SDK
- Internet connection for the first NuGet package restore

## Open and run in Visual Studio
1. Extract the ZIP file.
2. Open `StudentManagementAPI.csproj` in Visual Studio 2022.
3. Wait for NuGet packages to restore.
4. Press **F5** or choose **Debug > Start Without Debugging**.
5. Open the Swagger page shown in the browser. If it does not open automatically, browse to `/swagger`.

## Run from the terminal
Open a terminal in this folder and run:

```bash
dotnet restore
dotnet run --urls http://localhost:5000
```

Then open: http://localhost:5000/swagger

## API endpoints

| Method | URL | Purpose |
|---|---|---|
| GET | `/api/students` | Get all students |
| GET | `/api/students/{id}` | Get one student |
| POST | `/api/students` | Add a student |
| PUT | `/api/students/{id}` | Update a student |
| DELETE | `/api/students/{id}` | Delete a student |

Example POST body:

```json
{
  "name": "David Miller",
  "email": "david.miller@example.com",
  "course": "Cloud Computing"
}
```

## Database note
The PDF's SQL script uses Microsoft SQL Server. To make this project easy to run on a new Visual Studio installation without first installing/configuring SQL Server, the runnable API uses SQLite and automatically creates `StudentManagement.db` and the `Students` table on first launch. Four sample students are inserted automatically. The original SQL Server schema is also included at `Database/Schema.sql` for reference.

The database file is created in the application's working directory. Delete `StudentManagement.db` if you want to reset the sample data.

## Project structure

```text
StudentManagementAPI/
├── Controllers/
│   └── StudentsController.cs
├── Database/
│   ├── DatabaseInitializer.cs
│   └── Schema.sql
├── Models/
│   └── Student.cs
├── appsettings.json
├── Program.cs
├── StudentManagementAPI.csproj
├── StudentManagementAPI.http
└── README.md
```
=======
# StudentManagementAPI
>>>>>>> aa3d4142bf50e59ea2da4f62dd4498d3d1f7f289
