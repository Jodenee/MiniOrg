# MiniOrg

MiniOrg is a toy REST API I created to show off my capabilities with ASP.NET Core.

## Features

* 📄 Pagination
* 🛑 Ratelimiting
* 💾 Output Caching
* 🗃️ SQL Server database
* ⚡ Asynchronous
* 📖 Swagger documentation
* ✨ MVC Pattern
* 🔥 .NET 8.0

## Hosting locally

### Prerequisites

* A connection string to an SQL Server database
* Visual Studio

### Guide

1. Clone the master branch.
2. Open the solution in Visual Studio.
3. Ensure all Nuget dependencies are installed.
4. Open `MiniOrg/MiniOrg/appsettings.json`.
5. Set your connection string as the defualt connection string.
6. Open a Package Manager Console instance.
7. Run `Update-Database`.
8. Open a Command Line instance.
9. Cd into `MiniOrg/MiniOrg`.
10. Run `dotnet run seeddb`.
11. Close both consoles.
12. Run the project using one of the following methods.
	* Press the run button in Visual Studio. (Ensure the https profile is used)
	* Run `dotnet run --environment Development --launch-profile https`.
13. Open a web browser of your chosing and visit `https://localhost:7163/swagger/index.html`.