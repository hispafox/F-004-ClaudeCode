# OrderManagement

Proyecto demo del curso Claude Code para devs .NET + Angular.

## Stack

- .NET 10 (ASP.NET Core, MediatR, FluentValidation, EF Core In-Memory)
- Angular 19 (standalone components + Signals)
- xUnit + NSubstitute + FluentAssertions (preparados, sin tests todavía)

## Cómo ejecutar

```powershell
dotnet build
dotnet run --project src/OrderManagement.Api

# Frontend
cd frontend
npm install
npm start
```
