# ASP.NET Core WebApi Agricola Users Managment
Responsible for managing Farms, Lots and Groups for Agricola technical testing

![alt text](<Screenshot 2024-07-16 at 12.24.06 PM.png>)

### How to run the app:

- Clone repository.
- Install .NET SDK 8.0.
- Create SqlServer database.
- Update app `settings.json` with the connection string on the ConnectionStrings::DefaultConnection.
- Run migration with inside AgricolaApi.Infrastructure `dotnet ef database update --startup-project ../Agricola.Api`
- Run de app with `dotnet run`