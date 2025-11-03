# ClientPortalApi (ASP.NET Core) - Demo (SQLite)

This is a demo ASP.NET Core Web API that supports the React client. It uses:
- .NET 8 (net8.0)
- EF Core with SQLite (clientportal.db)
- JWT authentication (demo mode accepts any email/password and creates a user)
- SignalR for notifications
- File uploads saved to `wwwroot/uploads`

How to run:
1. Install .NET 8 SDK.
2. From project folder:
   ```bash
   dotnet restore
   dotnet tool install --global dotnet-ef --version 8.0.0
   dotnet ef migrations add Init
   dotnet ef database update
   dotnet run
   ```
**The Strip's secret key is read through the project's secret storage
In order to run the project properly, you need to store your Stripe secret key in the secret storage.**
Follow these steps to do so:
1) Enable secret storage
    ```.NET CLI
    dotnet user-secrets init
    ```
2) Set the Stripe `SecretKey` secret

    ``` .NET CLI
    dotnet user-secrets set "Stripe:SecretKey" "your-stripe-secret-key"
    ```
**That's it, you're ready to go!**
