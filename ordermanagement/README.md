# OrderManagement

Sistema de gestión de pedidos. Proyecto demo del curso Claude Code para devs .NET + Angular.

## Stack

- .NET 10
- ASP.NET Core (API REST)
- MediatR (CQRS)
- FluentValidation
- EF Core In-Memory (sin DB real, todo en memoria)
- Angular 19 con standalone components y Signals
- xUnit + NSubstitute + FluentAssertions (preparados, sin tests todavía)

## Estructura

```
src/
├── OrderManagement.Api/             API REST con OrdersController y 5 endpoints
├── OrderManagement.Application/     Handlers MediatR + validators FluentValidation
├── OrderManagement.Domain/          Entidades de negocio: Order, OrderItem, Customer
└── OrderManagement.Infrastructure/  Repositorios EF Core + servicios mock
frontend/                            Angular 19 con componentes standalone
tests/                               Carpeta preparada para tests, vacía por ahora
```

## Cómo ejecutar

```powershell
# API
dotnet build
dotnet run --project src/OrderManagement.Api

# Frontend (en otra terminal)
cd frontend
npm install
npm start

# Acceso
# API: http://localhost:5000
# Frontend: http://localhost:4200
```

## Estado actual

API REST funcional con CRUD de pedidos. Frontend Angular con listado y detalle. Tres puntos a mejorar a lo largo del curso:

1. **Sin tests todavía**. La carpeta tests/ está preparada pero vacía. La cobertura es cero.
2. **Sin documentación XML** en los métodos públicos. El Swagger se genera pero los endpoints están sin describir.
3. **Sin configuración de Claude Code todavía**. No hay CLAUDE.md, no hay .claude/. El proyecto está en estado virgen para que se vea el contraste cuando empecemos a configurarlo.

## Curso

Cada gamma del curso tiene una demo asociada en una rama dedicada (`demo/X.Y`, o el par `demo/X.Y-before` / `demo/X.Y-after` para las no conceptuales). Ver `docs/DEMOS.md` (en la raíz del repo del curso) para el registro completo.
