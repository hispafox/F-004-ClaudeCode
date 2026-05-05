# Proyecto: OrderManagement

Sistema de gestión de pedidos B2B. API REST en .NET 10 + frontend Angular 19.
Proyecto demo del curso Claude Code para devs .NET + Angular.

## Estructura

- `src/OrderManagement.Api` — proyecto ASP.NET Core con los endpoints REST.
  Solo presentación; sin lógica de negocio.
- `src/OrderManagement.Application` — handlers MediatR (CQRS), validators
  FluentValidation, excepciones tipadas del dominio.
- `src/OrderManagement.Domain` — entidades (Order, OrderItem, Customer)
  y enum OrderStatus. Sin dependencias a otras capas.
- `src/OrderManagement.Infrastructure` — repositorios EF Core In-Memory,
  servicios mock (EmailService, PaymentService).
- `frontend/` — aplicación Angular 19 con componentes standalone y Signals.
- `tests/OrderManagement.Tests/` — carpeta preparada para tests, vacía
  por ahora. La cobertura es cero hasta el módulo 5.

## Comandos

- `dotnet build` — compilar la solución completa.
- `dotnet test` — ejecutar todos los tests (cuando existan).
- `dotnet run --project src/OrderManagement.Api` — arrancar la API en
  http://localhost:5000.
- `cd frontend; npm install` — instalar dependencias frontend.
- `cd frontend; npm start` — levantar Angular en :4200.
- `cd frontend; npm run lint` — linter Angular con eslint.
- `cd frontend; npm run build` — build de producción del frontend.

## Convenciones .NET

- Naming: PascalCase para clases y métodos públicos, _camelCase con
  guion bajo para campos privados (`_orderRepository`, `_logger`).
- Async/await siempre. Nunca `.Result` ni `.Wait()`. CancellationToken
  propagado desde la firma del controller hasta el repositorio.
- Manejo de errores: excepciones tipadas en Application
  (CustomerNotFoundException, OrderNotFoundException,
  InvalidOrderStateException), capturadas en el controller para traducir
  a códigos HTTP correspondientes (404, 422). Nada de catch genérico
  con `Exception ex`.
- DTOs en `src/OrderManagement.Api/Contracts/` con nombres terminados
  en `Dto`. La capa Domain NO usa DTOs.
- CQRS con MediatR: cada operación tiene su Command/Query y Handler.
  Los handlers viven en `src/OrderManagement.Application/Handlers/`.
- Validación con FluentValidation: validators en
  `src/OrderManagement.Application/Validators/` con nombre
  `<Command>Validator`.
- Tests: xUnit + NSubstitute + FluentAssertions. **Nunca Moq.**
  Patrón de naming: `MétodoBajoTest_Escenario_ResultadoEsperado`.

## Convenciones Angular

- Componentes standalone siempre. Nada de NgModules nuevos.
- Signals para estado local; SignalStore para estado compartido.
- Reactive Forms con tipado estricto cuando aplique.
- HTTP requests vía HttpClient inyectado, retornando Observables.
- Estilos con SCSS, tokens en `frontend/src/styles/_tokens.scss`.
- Tests: Karma + Jasmine para unit, Playwright para E2E (cuando aplique).

## Reglas duras

- No tocar `src/OrderManagement.Api/Generated/` si existe. Es código
  autogenerado desde OpenAPI.
- Nunca editar una migración EF Core ya aplicada en `main`. Si hace
  falta cambio, crear migración nueva.
- Nunca crear branches con prefijo `release/*`. Las gestiona el pipeline.
- Servicios externos (EmailService, PaymentService) son mocks. **No
  intentes implementarlos como reales** sin pedirme antes — la lógica
  de envío de email y de cobro no está en este repo.
- Frontend solo se ejecuta tras instalar dependencias con `npm install`
  desde `frontend/`. No asumas que `node_modules` está disponible.

## Estado actual

- API funcional con CRUD de pedidos. Cinco endpoints REST en OrdersController.
- Frontend con dos componentes básicos: orders-list y order-detail.
- Sin tests todavía. La cobertura es cero hasta el módulo 5 del curso.
- Sin documentación XML en métodos públicos. Swagger se genera pero
  los endpoints están sin describir.
