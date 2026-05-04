# Demo 0.1 — Setup del proyecto OrderManagement (referencia / sin screencast)

> **Versión:** v3 | **Módulo:** 0 | **Sub:** 0.1 | **Estado:** ✅ Versión final
> **Archivo:** `demo_M00-S0.1-setup-ordermanagement-windows-v3.md`
> **Branch destino:** `demo/0.1` (mergeable a `main` al cerrar M0)
> **Branch de partida:** repo `ordermanagement` recién inicializado (`git init`), `main` sin código
> **Tipo:** Demo de referencia / setup — sin screencast pedagógico. **Excepción al patrón before/after** (ver [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md)): M0 es punto de origen del curso, rama única.
> **Plataforma:** Windows (PowerShell 7 + Git for Windows + .NET 10 SDK + Node 22)

---

## 1. Contexto

Las 28 demos del curso de Claude Code asumen un proyecto demo `ordermanagement` (.NET 10 + Angular 19) como hilo conductor. Cada demo deja una rama nueva (`demo/X.Y`) que parte de la anterior y añade lo de su gamma.

Hasta aquí, **ese repo solo existía descrito en prosa** dentro de las demos M01–M03 (la descripción más completa está en el bloque «Estado del repo al empezar» de la 1.1, líneas 65–153, y se confirma cruzadamente en la 2.1a). Ninguna demo lo construía. La 1.1 lo declaraba explícitamente como *«trabajo previo»*.

Esta demo M0 cubre ese hueco: documenta verbatim el proceso de construcción del repo desde cero hasta el estado que la 1.1 asume.

**Por qué no se graba como screencast:**

- Es construcción de sustrato, no escenificación de un concepto del producto Claude Code. Pedagógicamente vacío para el alumno típico.
- Si se grabara, anularía la frase clave del bloque 3 de la 1.1 (*«esta es la primera vez que arrancáis Claude Code»*) y obligaría a retocar la 1.1 ya aprobada.
- A cambio, se distribuye con **commits granulares por capa** en `demo/0.1` para que el alumno avanzado pueda recorrer la construcción con `git log --oneline` o `git checkout` por commit.

**Cómo encaja con la 1.1:**

- Al cerrar M0, `demo/0.1` se mergea a `main`. `main` queda con el repo completo en el estado de partida que la 1.1 espera.
- La rama `demo/0.1` queda intacta como artefacto histórico (los siete commits granulares siguen siendo `git checkout`-ables).
- La 1.1 conserva su línea *«Branch de partida: main»* sin ningún ajuste.
- Las demás demos siguen el patrón **before/after** definido en [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md): cada sección no conceptual tiene `demo/X.Y-before` (estado de partida del screencast) y `demo/X.Y-after` (estado final que la siguiente clase asume). M0.2 documenta la convención completa.

> **M0 es excepción al patrón.** Es punto de origen del curso: rama única `demo/0.1`, sin before/after — no hay screencast del que distinguir un «antes» y un «después». De aquí parte toda la cadena.

---

## 2. Objetivo

Al terminar M0, el repo `ordermanagement` cumple verbatim el estado descrito en la sección 5 del demo 1.1 ([demo_M01-S1.1-...:65-153](demo_M01-S1.1-ciclo-agentic-en-accion-v3.md#L65)):

- API REST .NET 10 funcional con cinco endpoints CRUD (`OrdersController`).
- Capas separadas: `Domain`, `Application` (MediatR + FluentValidation), `Infrastructure` (EF Core In-Memory + mocks), `Api`.
- Lógica de cancelación implementada en handler **pero no expuesta como endpoint independiente** (eso es lo que la 1.1 pide al agente que añada en vivo, y luego se descarta).
- Frontend Angular 19 con dos componentes standalone (`OrdersListComponent`, `OrderDetailComponent`), signals y un `_tokens.scss` mínimo.
- Carpeta `tests/OrderManagement.Tests/` **vacía** con `.csproj` configurado pero sin tests reales.
- **Sin** `CLAUDE.md`, **sin** `.claude/`, **sin** documentación XML, **sin** scripts ni hooks. Todo ese contenido es de demos posteriores.

Compila con `dotnet build` y `npm run build` sin warnings ni errores.

---

## 3. Branch de partida

Repo `ordermanagement` recién inicializado:

```powershell
mkdir C:\Users\pedro\projects\ordermanagement
cd C:\Users\pedro\projects\ordermanagement
git init -b main
```

`main` está vacío. No hay ficheros todavía. El primer commit lo creará la fase C1.

> Si el repo ya existe en disco con commits previos, **detente y pregunta a Pedro** antes de tocar nada. M0 asume punto cero.

---

## 4. Branch destino

`demo/0.1` parte de `main` (vacío) y recibe los siete commits granulares de la sección 7. Al validar la sección 10, se cierra M0 mergeando `demo/0.1` a `main` (sección 11).

Tras ese merge:

- `main` queda con todo el código en un único linaje cronológico (los siete commits visibles).
- `demo/0.1` queda como rama histórica (no se borra).
- `demo/1.1` y las siguientes parten de `main` como estaba previsto en la 1.1.

---

## 5. Estado del repo al terminar

Árbol completo tras el merge a `main`:

```
ordermanagement/
├── .gitignore
├── README.md
├── global.json                            (SDK .NET 10 fijado)
├── OrderManagement.sln
├── src/
│   ├── OrderManagement.Api/
│   │   ├── Controllers/
│   │   │   └── OrdersController.cs        (5 endpoints REST: GET, GET/{id}, POST, PUT, DELETE)
│   │   ├── Properties/
│   │   │   └── launchSettings.json
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json
│   │   ├── Program.cs
│   │   └── OrderManagement.Api.csproj
│   ├── OrderManagement.Application/
│   │   ├── Commands/
│   │   │   ├── CreateOrderCommand.cs
│   │   │   ├── UpdateOrderCommand.cs
│   │   │   └── CancelOrderCommand.cs
│   │   ├── Queries/
│   │   │   ├── GetOrderByIdQuery.cs
│   │   │   └── GetOrdersQuery.cs
│   │   ├── Handlers/
│   │   │   ├── CreateOrderHandler.cs
│   │   │   ├── UpdateOrderHandler.cs
│   │   │   ├── CancelOrderHandler.cs
│   │   │   ├── GetOrderByIdHandler.cs
│   │   │   └── GetOrdersHandler.cs
│   │   ├── Validators/
│   │   │   └── CreateOrderValidator.cs
│   │   ├── Exceptions/
│   │   │   ├── CustomerNotFoundException.cs
│   │   │   └── OrderNotFoundException.cs
│   │   └── OrderManagement.Application.csproj
│   ├── OrderManagement.Domain/
│   │   ├── Entities/
│   │   │   ├── Order.cs
│   │   │   ├── OrderItem.cs
│   │   │   └── Customer.cs
│   │   ├── Enums/
│   │   │   └── OrderStatus.cs
│   │   └── OrderManagement.Domain.csproj
│   └── OrderManagement.Infrastructure/
│       ├── Persistence/
│       │   └── AppDbContext.cs
│       ├── Repositories/
│       │   ├── IOrderRepository.cs
│       │   ├── OrderRepository.cs
│       │   ├── ICustomerRepository.cs
│       │   └── CustomerRepository.cs
│       ├── Services/
│       │   ├── IEmailService.cs
│       │   ├── EmailService.cs            (mock que solo loguea)
│       │   ├── IPaymentService.cs
│       │   └── PaymentService.cs          (mock)
│       └── OrderManagement.Infrastructure.csproj
├── frontend/
│   ├── angular.json
│   ├── package.json
│   ├── tsconfig.json
│   ├── tsconfig.app.json
│   ├── tsconfig.spec.json
│   ├── src/
│   │   ├── index.html
│   │   ├── main.ts
│   │   ├── styles.scss
│   │   ├── styles/
│   │   │   └── _tokens.scss
│   │   └── app/
│   │       ├── app.component.ts
│   │       ├── app.component.html
│   │       ├── app.config.ts
│   │       ├── app.routes.ts
│   │       └── orders/
│   │           ├── orders-list.component.ts
│   │           └── order-detail.component.ts
│   └── public/
│       └── favicon.ico
└── tests/
    └── OrderManagement.Tests/
        └── OrderManagement.Tests.csproj   (xUnit + NSubstitute + FluentAssertions, sin tests)
```

**Verificación funcional al terminar:**

- `dotnet build` desde la raíz: 0 warnings, 0 errors.
- `dotnet run --project src/OrderManagement.Api` arranca la API en `https://localhost:5001` (HTTPS dev cert) — no se ejecuta en M0, solo se valida que arranca si Pedro lo prueba manualmente.
- `cd frontend; npm install; npm run build` produce build limpio.
- `git log --oneline` muestra los siete commits de M0 en orden.
- **No existe** `CLAUDE.md`, **no existe** `.claude/`, **no existe** documentación XML, **no existe** endpoint `cancel` ni excepción `InvalidOrderStateException`, **no existen** tests reales.

---

## 6. Plan de commits granulares en `demo/0.1`

Siete commits, cada uno cierra una capa. Mensajes en imperativo presente, en español, prefijo de capa entre paréntesis:

| # | Mensaje | Contenido |
|---|---|---|
| C1 | `(init) solución vacía OrderManagement.sln + .gitignore + README inicial + global.json` | Andamiaje raíz del repo. Sin código aún. |
| C2 | `(domain) entidades Order, OrderItem, Customer y enum OrderStatus` | Capa Domain pura, sin dependencias. |
| C3 | `(application) commands, queries, handlers MediatR, validator FluentValidation y exceptions tipadas` | Capa Application referencia Domain. |
| C4 | `(infrastructure) AppDbContext EF Core In-Memory, repositorios y mocks de IEmailService / IPaymentService` | Capa Infrastructure referencia Domain y Application. |
| C5 | `(api) OrdersController con 5 endpoints REST + Program.cs con DI, EF Core In-Memory y MediatR` | Capa Api referencia las tres anteriores. Compila la solución entera. |
| C6 | `(frontend) Angular 19 standalone + signals: OrdersListComponent, OrderDetailComponent, app.routes, app.config y _tokens.scss` | Frontend totalmente independiente del backend. |
| C7 | `(tests) scaffold OrderManagement.Tests con xUnit + NSubstitute + FluentAssertions (sin tests)` | Carpeta lista para módulo 5, vacía. |

Cada commit deja la solución compilable (a partir de C5 con `dotnet build` limpio; antes de C5 los `.csproj` compilan independientemente).

---

## 7. Proceso de construcción documentado paso a paso

Cada subapartado cubre un commit. Comandos PowerShell exactos + contenido verbatim de cada fichero + verificación parcial cuando aplica.

> **Premisa común:** todos los comandos asumen que estás en `C:\Users\pedro\projects\ordermanagement` con `main` recién inicializado (sección 3) y que has creado primero la rama `demo/0.1` con:
>
> ```powershell
> git checkout -b demo/0.1
> ```

---

### 7.1 — C1 init: solución, gitignore, README y global.json

**Comandos:**

```powershell
# .NET SDK fijado
dotnet new globaljson --sdk-version 10.0.100 --roll-forward latestFeature

# Solución vacía
dotnet new sln -n OrderManagement

# .gitignore estándar para Visual Studio + Node + frontend
dotnet new gitignore
```

**Contenido verbatim de `global.json`:**

```json
{
  "sdk": {
    "version": "10.0.100",
    "rollForward": "latestFeature"
  }
}
```

**Contenido verbatim de `README.md`** (placeholder mínimo; la demo 1.1 lo sustituye en su rama):

```markdown
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
```

**Adición al `.gitignore`** (concatenar al final del que genera `dotnet new gitignore`):

```gitignore

# Frontend
frontend/node_modules/
frontend/dist/
frontend/.angular/
frontend/coverage/

# Editor
.vs/
.vscode/
*.user
```

**Commit:**

```powershell
git add .gitignore README.md global.json OrderManagement.sln
git commit -m "(init) solución vacía OrderManagement.sln + .gitignore + README inicial + global.json"
```

**Verificación parcial:**

```powershell
dotnet sln list
# Esperado: "No projects found in the solution."
```

---

### 7.2 — C2 domain: entidades + enum

**Comandos:**

```powershell
dotnet new classlib -n OrderManagement.Domain -o src/OrderManagement.Domain -f net10.0
dotnet sln add src/OrderManagement.Domain/OrderManagement.Domain.csproj

# Eliminar Class1.cs por defecto
Remove-Item src/OrderManagement.Domain/Class1.cs

# Crear estructura
New-Item -ItemType Directory -Path src/OrderManagement.Domain/Entities -Force
New-Item -ItemType Directory -Path src/OrderManagement.Domain/Enums -Force
```

**Contenido verbatim de `src/OrderManagement.Domain/OrderManagement.Domain.csproj`:**

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

**Contenido verbatim de `src/OrderManagement.Domain/Enums/OrderStatus.cs`:**

```csharp
namespace OrderManagement.Domain.Enums;

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Shipped = 2,
    Delivered = 3,
    Cancelled = 4
}
```

**Contenido verbatim de `src/OrderManagement.Domain/Entities/Customer.cs`:**

```csharp
namespace OrderManagement.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
```

**Contenido verbatim de `src/OrderManagement.Domain/Entities/OrderItem.cs`:**

```csharp
namespace OrderManagement.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public decimal LineTotal => Quantity * UnitPrice;
}
```

**Contenido verbatim de `src/OrderManagement.Domain/Entities/Order.cs`:**

```csharp
using OrderManagement.Domain.Enums;

namespace OrderManagement.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public decimal Total => Items.Sum(i => i.LineTotal);
}
```

**Commit:**

```powershell
dotnet build src/OrderManagement.Domain
git add src/OrderManagement.Domain OrderManagement.sln
git commit -m "(domain) entidades Order, OrderItem, Customer y enum OrderStatus"
```

**Verificación parcial:** `dotnet build src/OrderManagement.Domain` → 0 warnings, 0 errors.

---

### 7.3 — C3 application: commands, queries, handlers, validator, exceptions

**Comandos:**

```powershell
dotnet new classlib -n OrderManagement.Application -o src/OrderManagement.Application -f net10.0
dotnet sln add src/OrderManagement.Application/OrderManagement.Application.csproj

Remove-Item src/OrderManagement.Application/Class1.cs

# Referencia a Domain
dotnet add src/OrderManagement.Application reference src/OrderManagement.Domain

# Paquetes
dotnet add src/OrderManagement.Application package MediatR --version 12.5.0
dotnet add src/OrderManagement.Application package FluentValidation --version 11.11.0

# Estructura
New-Item -ItemType Directory -Path src/OrderManagement.Application/Commands -Force
New-Item -ItemType Directory -Path src/OrderManagement.Application/Queries -Force
New-Item -ItemType Directory -Path src/OrderManagement.Application/Handlers -Force
New-Item -ItemType Directory -Path src/OrderManagement.Application/Validators -Force
New-Item -ItemType Directory -Path src/OrderManagement.Application/Exceptions -Force
```

**Contenido verbatim de `src/OrderManagement.Application/OrderManagement.Application.csproj`:**

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="MediatR" Version="12.5.0" />
    <PackageReference Include="FluentValidation" Version="11.11.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\OrderManagement.Domain\OrderManagement.Domain.csproj" />
  </ItemGroup>

</Project>
```

**Contenido verbatim de `src/OrderManagement.Application/Exceptions/CustomerNotFoundException.cs`:**

```csharp
namespace OrderManagement.Application.Exceptions;

public class CustomerNotFoundException : Exception
{
    public int CustomerId { get; }

    public CustomerNotFoundException(int customerId)
        : base($"Customer {customerId} not found.")
    {
        CustomerId = customerId;
    }
}
```

**Contenido verbatim de `src/OrderManagement.Application/Exceptions/OrderNotFoundException.cs`:**

```csharp
namespace OrderManagement.Application.Exceptions;

public class OrderNotFoundException : Exception
{
    public int OrderId { get; }

    public OrderNotFoundException(int orderId)
        : base($"Order {orderId} not found.")
    {
        OrderId = orderId;
    }
}
```

**Contenido verbatim de `src/OrderManagement.Application/Commands/CreateOrderCommand.cs`:**

```csharp
using MediatR;

namespace OrderManagement.Application.Commands;

public record CreateOrderItemDto(string ProductName, int Quantity, decimal UnitPrice);

public record CreateOrderCommand(int CustomerId, List<CreateOrderItemDto> Items) : IRequest<int>;
```

**Contenido verbatim de `src/OrderManagement.Application/Commands/UpdateOrderCommand.cs`:**

```csharp
using MediatR;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Commands;

public record UpdateOrderCommand(int OrderId, OrderStatus NewStatus) : IRequest;
```

**Contenido verbatim de `src/OrderManagement.Application/Commands/CancelOrderCommand.cs`:**

```csharp
using MediatR;

namespace OrderManagement.Application.Commands;

public record CancelOrderCommand(int OrderId) : IRequest;
```

**Contenido verbatim de `src/OrderManagement.Application/Queries/GetOrderByIdQuery.cs`:**

```csharp
using MediatR;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Queries;

public record GetOrderByIdQuery(int OrderId) : IRequest<Order>;
```

**Contenido verbatim de `src/OrderManagement.Application/Queries/GetOrdersQuery.cs`:**

```csharp
using MediatR;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Queries;

public record GetOrdersQuery() : IRequest<IReadOnlyList<Order>>;
```

**Contenido verbatim de `src/OrderManagement.Application/Validators/CreateOrderValidator.cs`:**

```csharp
using FluentValidation;
using OrderManagement.Application.Commands;

namespace OrderManagement.Application.Validators;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("CustomerId must be greater than zero.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Order must contain at least one item.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductName).NotEmpty();
            item.RuleFor(i => i.Quantity).GreaterThan(0);
            item.RuleFor(i => i.UnitPrice).GreaterThanOrEqualTo(0);
        });
    }
}
```

> Las firmas de los handlers dependen de `IOrderRepository` e `ICustomerRepository`, que viven en `Infrastructure`. Para evitar dependencia circular, los handlers definen interfaces **abstractas mínimas** dentro de `Application` (patrón Ports & Adapters). Las implementaciones EF Core llegan en C4. Esto mantiene la capa Application desacoplada del ORM.

**Contenido verbatim de `src/OrderManagement.Application/Abstractions/IOrderRepository.cs`** *(crear carpeta `Abstractions` primero: `New-Item -ItemType Directory -Path src/OrderManagement.Application/Abstractions -Force`)*:

```csharp
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Abstractions;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken ct);
    Task<int> AddAsync(Order order, CancellationToken ct);
    Task UpdateAsync(Order order, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}
```

**Contenido verbatim de `src/OrderManagement.Application/Abstractions/ICustomerRepository.cs`:**

```csharp
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Abstractions;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id, CancellationToken ct);
}
```

**Contenido verbatim de `src/OrderManagement.Application/Abstractions/IEmailService.cs`:**

```csharp
namespace OrderManagement.Application.Abstractions;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string body, CancellationToken ct);
}
```

**Contenido verbatim de `src/OrderManagement.Application/Abstractions/IPaymentService.cs`:**

```csharp
namespace OrderManagement.Application.Abstractions;

public interface IPaymentService
{
    Task<bool> ChargeAsync(int customerId, decimal amount, CancellationToken ct);
}
```

**Contenido verbatim de `src/OrderManagement.Application/Handlers/CreateOrderHandler.cs`:**

```csharp
using MediatR;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Commands;
using OrderManagement.Application.Exceptions;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Handlers;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly IOrderRepository _orders;
    private readonly ICustomerRepository _customers;
    private readonly IEmailService _email;

    public CreateOrderHandler(
        IOrderRepository orders,
        ICustomerRepository customers,
        IEmailService email)
    {
        _orders = orders;
        _customers = customers;
        _email = email;
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        var customer = await _customers.GetByIdAsync(request.CustomerId, ct)
            ?? throw new CustomerNotFoundException(request.CustomerId);

        var order = new Order
        {
            CustomerId = customer.Id,
            Items = request.Items
                .Select(i => new OrderItem
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                })
                .ToList()
        };

        var id = await _orders.AddAsync(order, ct);

        await _email.SendAsync(
            customer.Email,
            "Order received",
            $"Your order {id} has been received.",
            ct);

        return id;
    }
}
```

**Contenido verbatim de `src/OrderManagement.Application/Handlers/UpdateOrderHandler.cs`:**

```csharp
using MediatR;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Commands;
using OrderManagement.Application.Exceptions;

namespace OrderManagement.Application.Handlers;

public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository _orders;

    public UpdateOrderHandler(IOrderRepository orders) => _orders = orders;

    public async Task Handle(UpdateOrderCommand request, CancellationToken ct)
    {
        var order = await _orders.GetByIdAsync(request.OrderId, ct)
            ?? throw new OrderNotFoundException(request.OrderId);

        order.Status = request.NewStatus;
        await _orders.UpdateAsync(order, ct);
    }
}
```

**Contenido verbatim de `src/OrderManagement.Application/Handlers/CancelOrderHandler.cs`:**

> **Importante:** este handler lanza `InvalidOperationException` cuando el estado del pedido no permite cancelar. La 1.1 explota esa inconsistencia (el resto del proyecto usa excepciones tipadas) en su demo en vivo y el agente la propone refactorizar a `InvalidOrderStateException`. **Aquí dejamos `InvalidOperationException` deliberadamente** — la refactorización es de la 1.3b.

```csharp
using MediatR;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Commands;
using OrderManagement.Application.Exceptions;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Handlers;

public class CancelOrderHandler : IRequestHandler<CancelOrderCommand>
{
    private readonly IOrderRepository _orders;

    public CancelOrderHandler(IOrderRepository orders) => _orders = orders;

    public async Task Handle(CancelOrderCommand request, CancellationToken ct)
    {
        var order = await _orders.GetByIdAsync(request.OrderId, ct)
            ?? throw new OrderNotFoundException(request.OrderId);

        if (order.Status is not (OrderStatus.Pending or OrderStatus.Confirmed))
        {
            throw new InvalidOperationException(
                $"Order {order.Id} is in state {order.Status} and cannot be cancelled.");
        }

        order.Status = OrderStatus.Cancelled;
        await _orders.UpdateAsync(order, ct);
    }
}
```

**Contenido verbatim de `src/OrderManagement.Application/Handlers/GetOrderByIdHandler.cs`:**

```csharp
using MediatR;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Queries;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Handlers;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, Order>
{
    private readonly IOrderRepository _orders;

    public GetOrderByIdHandler(IOrderRepository orders) => _orders = orders;

    public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken ct)
    {
        return await _orders.GetByIdAsync(request.OrderId, ct)
            ?? throw new OrderNotFoundException(request.OrderId);
    }
}
```

**Contenido verbatim de `src/OrderManagement.Application/Handlers/GetOrdersHandler.cs`:**

```csharp
using MediatR;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Queries;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Handlers;

public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, IReadOnlyList<Order>>
{
    private readonly IOrderRepository _orders;

    public GetOrdersHandler(IOrderRepository orders) => _orders = orders;

    public Task<IReadOnlyList<Order>> Handle(GetOrdersQuery request, CancellationToken ct)
        => _orders.GetAllAsync(ct);
}
```

**Commit:**

```powershell
dotnet build src/OrderManagement.Application
git add src/OrderManagement.Application OrderManagement.sln
git commit -m "(application) commands, queries, handlers MediatR, validator FluentValidation y exceptions tipadas"
```

**Verificación parcial:** `dotnet build src/OrderManagement.Application` → 0 warnings, 0 errors.

---

### 7.4 — C4 infrastructure: AppDbContext, repos EF Core, mocks

**Comandos:**

```powershell
dotnet new classlib -n OrderManagement.Infrastructure -o src/OrderManagement.Infrastructure -f net10.0
dotnet sln add src/OrderManagement.Infrastructure/OrderManagement.Infrastructure.csproj

Remove-Item src/OrderManagement.Infrastructure/Class1.cs

dotnet add src/OrderManagement.Infrastructure reference src/OrderManagement.Domain
dotnet add src/OrderManagement.Infrastructure reference src/OrderManagement.Application

dotnet add src/OrderManagement.Infrastructure package Microsoft.EntityFrameworkCore --version 10.0.0
dotnet add src/OrderManagement.Infrastructure package Microsoft.EntityFrameworkCore.InMemory --version 10.0.0
dotnet add src/OrderManagement.Infrastructure package Microsoft.Extensions.Logging.Abstractions --version 10.0.0

New-Item -ItemType Directory -Path src/OrderManagement.Infrastructure/Persistence -Force
New-Item -ItemType Directory -Path src/OrderManagement.Infrastructure/Repositories -Force
New-Item -ItemType Directory -Path src/OrderManagement.Infrastructure/Services -Force
```

**Contenido verbatim de `src/OrderManagement.Infrastructure/OrderManagement.Infrastructure.csproj`:**

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="10.0.0" />
    <PackageReference Include="Microsoft.Extensions.Logging.Abstractions" Version="10.0.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\OrderManagement.Domain\OrderManagement.Domain.csproj" />
    <ProjectReference Include="..\OrderManagement.Application\OrderManagement.Application.csproj" />
  </ItemGroup>

</Project>
```

**Contenido verbatim de `src/OrderManagement.Infrastructure/Persistence/AppDbContext.cs`:**

```csharp
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .Ignore(o => o.Total);

        modelBuilder.Entity<OrderItem>()
            .Ignore(i => i.LineTotal)
            .Property(i => i.UnitPrice).HasPrecision(18, 2);

        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, Name = "Acme", Email = "billing@acme.test" },
            new Customer { Id = 2, Name = "Globex", Email = "ops@globex.test" });
    }
}
```

**Contenido verbatim de `src/OrderManagement.Infrastructure/Repositories/IOrderRepository.cs`** *(re-export del que vive en Application — mantenemos el namespace de Infrastructure por compatibilidad cuando el código del controller importe desde Infrastructure)*:

```csharp
namespace OrderManagement.Infrastructure.Repositories;

// Re-export por conveniencia. La interfaz vive en Application.Abstractions.
public interface IOrderRepository : Application.Abstractions.IOrderRepository { }
```

**Contenido verbatim de `src/OrderManagement.Infrastructure/Repositories/OrderRepository.cs`:**

```csharp
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Abstractions;
using OrderManagement.Domain.Entities;
using OrderManagement.Infrastructure.Persistence;

namespace OrderManagement.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;

    public OrderRepository(AppDbContext db) => _db = db;

    public async Task<Order?> GetByIdAsync(int id, CancellationToken ct)
        => await _db.Orders
            .Include(o => o.Items)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == id, ct);

    public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken ct)
        => await _db.Orders
            .Include(o => o.Items)
            .Include(o => o.Customer)
            .ToListAsync(ct);

    public async Task<int> AddAsync(Order order, CancellationToken ct)
    {
        _db.Orders.Add(order);
        await _db.SaveChangesAsync(ct);
        return order.Id;
    }

    public async Task UpdateAsync(Order order, CancellationToken ct)
    {
        _db.Orders.Update(order);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var existing = await _db.Orders.FindAsync(new object[] { id }, ct);
        if (existing is null) return;
        _db.Orders.Remove(existing);
        await _db.SaveChangesAsync(ct);
    }
}
```

**Contenido verbatim de `src/OrderManagement.Infrastructure/Repositories/ICustomerRepository.cs`:**

```csharp
namespace OrderManagement.Infrastructure.Repositories;

public interface ICustomerRepository : Application.Abstractions.ICustomerRepository { }
```

**Contenido verbatim de `src/OrderManagement.Infrastructure/Repositories/CustomerRepository.cs`:**

```csharp
using OrderManagement.Application.Abstractions;
using OrderManagement.Domain.Entities;
using OrderManagement.Infrastructure.Persistence;

namespace OrderManagement.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _db;

    public CustomerRepository(AppDbContext db) => _db = db;

    public async Task<Customer?> GetByIdAsync(int id, CancellationToken ct)
        => await _db.Customers.FindAsync(new object[] { id }, ct);
}
```

**Contenido verbatim de `src/OrderManagement.Infrastructure/Services/IEmailService.cs`:**

```csharp
namespace OrderManagement.Infrastructure.Services;

public interface IEmailService : Application.Abstractions.IEmailService { }
```

**Contenido verbatim de `src/OrderManagement.Infrastructure/Services/EmailService.cs`:**

```csharp
using Microsoft.Extensions.Logging;
using OrderManagement.Application.Abstractions;

namespace OrderManagement.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger) => _logger = logger;

    public Task SendAsync(string to, string subject, string body, CancellationToken ct)
    {
        _logger.LogInformation(
            "[mock email] to={To} subject={Subject} body={Body}", to, subject, body);
        return Task.CompletedTask;
    }
}
```

**Contenido verbatim de `src/OrderManagement.Infrastructure/Services/IPaymentService.cs`:**

```csharp
namespace OrderManagement.Infrastructure.Services;

public interface IPaymentService : Application.Abstractions.IPaymentService { }
```

**Contenido verbatim de `src/OrderManagement.Infrastructure/Services/PaymentService.cs`:**

```csharp
using Microsoft.Extensions.Logging;
using OrderManagement.Application.Abstractions;

namespace OrderManagement.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(ILogger<PaymentService> logger) => _logger = logger;

    public Task<bool> ChargeAsync(int customerId, decimal amount, CancellationToken ct)
    {
        _logger.LogInformation(
            "[mock payment] customerId={CustomerId} amount={Amount}", customerId, amount);
        return Task.FromResult(true);
    }
}
```

**Commit:**

```powershell
dotnet build src/OrderManagement.Infrastructure
git add src/OrderManagement.Infrastructure OrderManagement.sln
git commit -m "(infrastructure) AppDbContext EF Core In-Memory, repositorios y mocks de IEmailService / IPaymentService"
```

**Verificación parcial:** `dotnet build src/OrderManagement.Infrastructure` → 0 warnings, 0 errors.

---

### 7.5 — C5 api: OrdersController + Program.cs

**Comandos:**

```powershell
dotnet new web -n OrderManagement.Api -o src/OrderManagement.Api -f net10.0
dotnet sln add src/OrderManagement.Api/OrderManagement.Api.csproj

dotnet add src/OrderManagement.Api reference src/OrderManagement.Application
dotnet add src/OrderManagement.Api reference src/OrderManagement.Infrastructure

dotnet add src/OrderManagement.Api package MediatR --version 12.5.0
dotnet add src/OrderManagement.Api package FluentValidation.AspNetCore --version 11.3.0
dotnet add src/OrderManagement.Api package Microsoft.AspNetCore.OpenApi --version 10.0.0

New-Item -ItemType Directory -Path src/OrderManagement.Api/Controllers -Force
```

**Contenido verbatim de `src/OrderManagement.Api/OrderManagement.Api.csproj`:**

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="MediatR" Version="12.5.0" />
    <PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="10.0.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\OrderManagement.Application\OrderManagement.Application.csproj" />
    <ProjectReference Include="..\OrderManagement.Infrastructure\OrderManagement.Infrastructure.csproj" />
  </ItemGroup>

</Project>
```

**Contenido verbatim de `src/OrderManagement.Api/Program.cs`:**

```csharp
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Validators;
using OrderManagement.Infrastructure.Persistence;
using OrderManagement.Infrastructure.Repositories;
using OrderManagement.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("OrderManagement"));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateOrderValidator).Assembly));

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderValidator>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

// Asegura creación del esquema in-memory y aplicación de seed data.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();

public partial class Program { }
```

**Contenido verbatim de `src/OrderManagement.Api/Controllers/OrdersController.cs`:**

> **Importante:** cinco endpoints REST estándar. **No hay endpoint dedicado de cancel** — la 1.1 lo añade en vivo y luego se descarta.

```csharp
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Commands;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Queries;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Order>>> GetAll(CancellationToken ct)
    {
        var orders = await _mediator.Send(new GetOrdersQuery(), ct);
        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Order>> GetById(int id, CancellationToken ct)
    {
        try
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(id), ct);
            return Ok(order);
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(
        [FromBody] CreateOrderCommand command,
        CancellationToken ct)
    {
        try
        {
            var id = await _mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        catch (CustomerNotFoundException ex)
        {
            return UnprocessableEntity(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateOrderCommand body,
        CancellationToken ct)
    {
        if (id != body.OrderId)
        {
            return BadRequest(new { message = "Route id and body OrderId mismatch." });
        }

        try
        {
            await _mediator.Send(body, ct);
            return NoContent();
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        try
        {
            await _mediator.Send(new GetOrderByIdQuery(id), ct);
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }

        // Por simplicidad pedagógica delegamos en el repositorio directamente.
        var repo = HttpContext.RequestServices
            .GetRequiredService<OrderManagement.Application.Abstractions.IOrderRepository>();
        await repo.DeleteAsync(id, ct);
        return NoContent();
    }
}
```

**Contenido verbatim de `src/OrderManagement.Api/appsettings.json`:**

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Contenido verbatim de `src/OrderManagement.Api/appsettings.Development.json`:**

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Information"
    }
  }
}
```

**Contenido verbatim de `src/OrderManagement.Api/Properties/launchSettings.json`:**

```json
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "profiles": {
    "OrderManagement.Api": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "https://localhost:5001;http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

**Commit:**

```powershell
dotnet build
git add src/OrderManagement.Api OrderManagement.sln
git commit -m "(api) OrdersController con 5 endpoints REST + Program.cs con DI, EF Core In-Memory y MediatR"
```

**Verificación parcial:** `dotnet build` desde la raíz → **0 warnings, 0 errors** sobre la solución entera.

---

### 7.6 — C6 frontend: Angular 19 standalone + signals

**Comandos:**

```powershell
# Asume Node 22 LTS y Angular CLI 19 instalado globalmente:
#   npm install -g @angular/cli@19

ng new frontend `
    --routing=true `
    --style=scss `
    --standalone=true `
    --strict=true `
    --package-manager=npm `
    --skip-git=true `
    --skip-install=false

cd frontend
New-Item -ItemType Directory -Path src/styles -Force
New-Item -ItemType Directory -Path src/app/orders -Force
```

> El comando `ng new` genera la mayoría del andamiaje. **Sustituye** los ficheros listados a continuación por su contenido verbatim. El resto del andamiaje generado por `ng new` se mantiene tal cual.

**Contenido verbatim de `frontend/package.json`** (versiones fijadas):

```json
{
  "name": "frontend",
  "version": "0.0.0",
  "scripts": {
    "ng": "ng",
    "start": "ng serve",
    "build": "ng build",
    "watch": "ng build --watch --configuration development",
    "test": "ng test"
  },
  "private": true,
  "dependencies": {
    "@angular/animations": "^19.2.0",
    "@angular/common": "^19.2.0",
    "@angular/compiler": "^19.2.0",
    "@angular/core": "^19.2.0",
    "@angular/forms": "^19.2.0",
    "@angular/platform-browser": "^19.2.0",
    "@angular/platform-browser-dynamic": "^19.2.0",
    "@angular/router": "^19.2.0",
    "rxjs": "~7.8.0",
    "tslib": "^2.3.0",
    "zone.js": "~0.15.0"
  },
  "devDependencies": {
    "@angular-devkit/build-angular": "^19.2.0",
    "@angular/cli": "^19.2.0",
    "@angular/compiler-cli": "^19.2.0",
    "@types/jasmine": "~5.1.0",
    "jasmine-core": "~5.4.0",
    "karma": "~6.4.0",
    "karma-chrome-launcher": "~3.2.0",
    "karma-coverage": "~2.2.0",
    "karma-jasmine": "~5.1.0",
    "karma-jasmine-html-reporter": "~2.1.0",
    "typescript": "~5.6.0"
  }
}
```

**Contenido verbatim de `frontend/src/styles/_tokens.scss`:**

```scss
// Design tokens — ampliados a partir del módulo 4.
:root {
  --color-primary: #0d6efd;
  --color-primary-contrast: #ffffff;
  --color-surface: #ffffff;
  --color-surface-muted: #f5f7fa;
  --color-text: #1f2937;
  --color-text-muted: #6b7280;
  --color-border: #e5e7eb;
  --color-success: #16a34a;
  --color-warning: #f59e0b;
  --color-danger: #dc2626;

  --space-1: 4px;
  --space-2: 8px;
  --space-3: 12px;
  --space-4: 16px;
  --space-6: 24px;
  --space-8: 32px;

  --radius-sm: 4px;
  --radius-md: 8px;
  --radius-lg: 12px;

  --font-family-base: system-ui, -apple-system, "Segoe UI", Roboto, sans-serif;
  --font-size-base: 14px;
  --line-height-base: 1.5;
}
```

**Contenido verbatim de `frontend/src/styles.scss`:**

```scss
@use 'styles/tokens';

html, body {
  margin: 0;
  font-family: var(--font-family-base);
  font-size: var(--font-size-base);
  line-height: var(--line-height-base);
  color: var(--color-text);
  background: var(--color-surface-muted);
}
```

**Contenido verbatim de `frontend/src/app/app.config.ts`:**

```typescript
import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withFetch } from '@angular/common/http';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withFetch()),
  ],
};
```

**Contenido verbatim de `frontend/src/app/app.routes.ts`:**

```typescript
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'orders',
  },
  {
    path: 'orders',
    loadComponent: () =>
      import('./orders/orders-list.component').then(m => m.OrdersListComponent),
  },
  {
    path: 'orders/:id',
    loadComponent: () =>
      import('./orders/order-detail.component').then(m => m.OrderDetailComponent),
  },
];
```

**Contenido verbatim de `frontend/src/app/app.component.ts`:**

```typescript
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
})
export class AppComponent {}
```

**Contenido verbatim de `frontend/src/app/app.component.html`:**

```html
<router-outlet />
```

**Contenido verbatim de `frontend/src/app/orders/orders-list.component.ts`:**

```typescript
import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { RouterLink } from '@angular/router';

interface OrderListItem {
  id: number;
  customerId: number;
  status: number;
  total: number;
  createdAt: string;
}

@Component({
  selector: 'app-orders-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <section style="padding: var(--space-6);">
      <h1>Orders</h1>

      @if (loading()) {
        <p>Loading…</p>
      } @else if (error()) {
        <p style="color: var(--color-danger);">{{ error() }}</p>
      } @else {
        <ul>
          @for (order of orders(); track order.id) {
            <li>
              <a [routerLink]="['/orders', order.id]">
                Order #{{ order.id }} — customer {{ order.customerId }} — {{ order.total | number:'1.2-2' }}
              </a>
            </li>
          } @empty {
            <li>No orders yet.</li>
          }
        </ul>
      }
    </section>
  `,
})
export class OrdersListComponent implements OnInit {
  private readonly http = inject(HttpClient);

  readonly orders = signal<OrderListItem[]>([]);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.http.get<OrderListItem[]>('/api/orders').subscribe({
      next: data => {
        this.orders.set(data);
        this.loading.set(false);
      },
      error: err => {
        this.error.set(err?.message ?? 'Failed to load orders.');
        this.loading.set(false);
      },
    });
  }
}
```

**Contenido verbatim de `frontend/src/app/orders/order-detail.component.ts`:**

```typescript
import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, RouterLink } from '@angular/router';

interface OrderDetail {
  id: number;
  customerId: number;
  status: number;
  total: number;
  createdAt: string;
  items: Array<{
    productName: string;
    quantity: number;
    unitPrice: number;
  }>;
}

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <section style="padding: var(--space-6);">
      <a routerLink="/orders">← Back</a>

      @if (loading()) {
        <p>Loading…</p>
      } @else if (error()) {
        <p style="color: var(--color-danger);">{{ error() }}</p>
      } @else if (order(); as o) {
        <h1>Order #{{ o.id }}</h1>
        <p>Customer: {{ o.customerId }}</p>
        <p>Status: {{ o.status }}</p>
        <p>Total: {{ o.total | number:'1.2-2' }}</p>

        <h2>Items</h2>
        <ul>
          @for (item of o.items; track $index) {
            <li>
              {{ item.productName }} — {{ item.quantity }} × {{ item.unitPrice | number:'1.2-2' }}
            </li>
          }
        </ul>
      }
    </section>
  `,
})
export class OrderDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly http = inject(HttpClient);

  readonly order = signal<OrderDetail | null>(null);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.error.set('Missing order id.');
      this.loading.set(false);
      return;
    }

    this.http.get<OrderDetail>(`/api/orders/${id}`).subscribe({
      next: data => {
        this.order.set(data);
        this.loading.set(false);
      },
      error: err => {
        this.error.set(err?.message ?? 'Failed to load order.');
        this.loading.set(false);
      },
    });
  }
}
```

**Contenido verbatim de `frontend/src/main.ts`:**

```typescript
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { appConfig } from './app/app.config';

bootstrapApplication(AppComponent, appConfig)
  .catch(err => console.error(err));
```

**Commit:**

```powershell
cd frontend
npm install
npm run build
cd ..
git add frontend
git commit -m "(frontend) Angular 19 standalone + signals: OrdersListComponent, OrderDetailComponent, app.routes, app.config y _tokens.scss"
```

**Verificación parcial:** `cd frontend; npm run build` → build limpio, sin errores TypeScript.

---

### 7.7 — C7 tests: scaffold OrderManagement.Tests vacío

**Comandos:**

```powershell
dotnet new xunit -n OrderManagement.Tests -o tests/OrderManagement.Tests -f net10.0
dotnet sln add tests/OrderManagement.Tests/OrderManagement.Tests.csproj

# Eliminar test placeholder de la plantilla
Remove-Item tests/OrderManagement.Tests/UnitTest1.cs

dotnet add tests/OrderManagement.Tests reference src/OrderManagement.Application
dotnet add tests/OrderManagement.Tests reference src/OrderManagement.Domain
dotnet add tests/OrderManagement.Tests reference src/OrderManagement.Infrastructure
dotnet add tests/OrderManagement.Tests reference src/OrderManagement.Api

dotnet add tests/OrderManagement.Tests package NSubstitute --version 5.3.0
dotnet add tests/OrderManagement.Tests package FluentAssertions --version 7.0.0
dotnet add tests/OrderManagement.Tests package Microsoft.AspNetCore.Mvc.Testing --version 10.0.0
```

**Contenido verbatim de `tests/OrderManagement.Tests/OrderManagement.Tests.csproj`** (autogenerado por `dotnet new xunit` y enriquecido con paquetes y references):

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
    <PackageReference Include="xunit" Version="2.9.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
    <PackageReference Include="NSubstitute" Version="5.3.0" />
    <PackageReference Include="FluentAssertions" Version="7.0.0" />
    <PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="10.0.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\src\OrderManagement.Domain\OrderManagement.Domain.csproj" />
    <ProjectReference Include="..\..\src\OrderManagement.Application\OrderManagement.Application.csproj" />
    <ProjectReference Include="..\..\src\OrderManagement.Infrastructure\OrderManagement.Infrastructure.csproj" />
    <ProjectReference Include="..\..\src\OrderManagement.Api\OrderManagement.Api.csproj" />
  </ItemGroup>

</Project>
```

> **No se añade ningún `.cs` con tests reales.** La carpeta queda preparada para que el módulo 5 vaya escribiendo sus tests dentro.

**Commit:**

```powershell
dotnet build
git add tests OrderManagement.sln
git commit -m "(tests) scaffold OrderManagement.Tests con xUnit + NSubstitute + FluentAssertions (sin tests)"
```

**Verificación parcial:** `dotnet build` desde la raíz → 0 warnings, 0 errors. `dotnet test` → *"No tests found"* (esperado, sin clases de test).

---

## 8. Prompt para Claude Code

> Bloque autocontenido para pegar en una sesión limpia de Claude Code arrancada en el directorio del repo. Ejecuta las siete fases y deja `demo/0.1` lista para mergear a `main`.

````
Estoy preparando la demo M0 (setup) del curso de Claude Code para devs
.NET + Angular. Trabajo en Windows con PowerShell 7. Esta demo construye
desde cero el repo `ordermanagement` que las 28 demos siguientes asumen
como punto de partida.

# Contexto

Estoy en la raíz de un repo `ordermanagement` recién inicializado:

  C:\Users\pedro\projects\ordermanagement

La rama `main` está vacía — `git init -b main` y nada más. Tengo
instalado:

- .NET 10 SDK (10.0.100)
- Node 22 LTS
- Angular CLI 19 (`npm install -g @angular/cli@19`)
- Git for Windows
- PowerShell 7

Necesito que ejecutes el setup completo creando 7 commits granulares en
una rama nueva `demo/0.1`. Cada commit cierra una capa.

# Lo que necesito

## Tarea 0: rama de trabajo

```powershell
git checkout -b demo/0.1
```

## Tarea 1: C1 — init

- `dotnet new globaljson --sdk-version 10.0.100 --roll-forward latestFeature`
- `dotnet new sln -n OrderManagement`
- `dotnet new gitignore`
- Añadir al final del `.gitignore` el bloque adicional para frontend
  (node_modules, dist, .angular, coverage, .vs, .vscode, *.user).
- Crear `README.md` con el contenido placeholder que te paso al final.
- Commit: `(init) solución vacía OrderManagement.sln + .gitignore + README inicial + global.json`

## Tarea 2: C2 — domain

- `dotnet new classlib -n OrderManagement.Domain -o src/OrderManagement.Domain -f net10.0`
- `dotnet sln add src/OrderManagement.Domain/OrderManagement.Domain.csproj`
- Borrar `Class1.cs`.
- Crear las carpetas `Entities/` y `Enums/`.
- Crear los ficheros `OrderStatus.cs`, `Customer.cs`, `OrderItem.cs`, `Order.cs`
  con el contenido VERBATIM que aparece en la sección 7.2 del demo M0.
- Verificar que `dotnet build src/OrderManagement.Domain` pasa sin warnings.
- Commit: `(domain) entidades Order, OrderItem, Customer y enum OrderStatus`

## Tarea 3: C3 — application

- `dotnet new classlib -n OrderManagement.Application -o src/OrderManagement.Application -f net10.0`
- `dotnet sln add src/OrderManagement.Application/OrderManagement.Application.csproj`
- Borrar `Class1.cs`.
- `dotnet add src/OrderManagement.Application reference src/OrderManagement.Domain`
- `dotnet add src/OrderManagement.Application package MediatR --version 12.5.0`
- `dotnet add src/OrderManagement.Application package FluentValidation --version 11.11.0`
- Crear las carpetas `Commands/`, `Queries/`, `Handlers/`, `Validators/`,
  `Exceptions/` y `Abstractions/`.
- Crear todos los ficheros listados en la sección 7.3 del demo M0 con
  su contenido VERBATIM.
- Verificar que `dotnet build src/OrderManagement.Application` pasa.
- Commit: `(application) commands, queries, handlers MediatR, validator FluentValidation y exceptions tipadas`

## Tarea 4: C4 — infrastructure

- `dotnet new classlib -n OrderManagement.Infrastructure -o src/OrderManagement.Infrastructure -f net10.0`
- `dotnet sln add src/OrderManagement.Infrastructure/OrderManagement.Infrastructure.csproj`
- Borrar `Class1.cs`.
- Referencias a Domain y Application.
- Paquetes:
  - Microsoft.EntityFrameworkCore 10.0.0
  - Microsoft.EntityFrameworkCore.InMemory 10.0.0
  - Microsoft.Extensions.Logging.Abstractions 10.0.0
- Crear carpetas `Persistence/`, `Repositories/`, `Services/`.
- Crear ficheros con el contenido VERBATIM de la sección 7.4 del demo M0:
  AppDbContext.cs, IOrderRepository.cs (re-export), OrderRepository.cs,
  ICustomerRepository.cs (re-export), CustomerRepository.cs,
  IEmailService.cs (re-export), EmailService.cs, IPaymentService.cs (re-export),
  PaymentService.cs.
- Verificar build.
- Commit: `(infrastructure) AppDbContext EF Core In-Memory, repositorios y mocks de IEmailService / IPaymentService`

## Tarea 5: C5 — api

- `dotnet new web -n OrderManagement.Api -o src/OrderManagement.Api -f net10.0`
- `dotnet sln add src/OrderManagement.Api/OrderManagement.Api.csproj`
- Referencias a Application e Infrastructure.
- Paquetes:
  - MediatR 12.5.0
  - FluentValidation.AspNetCore 11.3.0
  - Microsoft.AspNetCore.OpenApi 10.0.0
- Sustituir `Program.cs` y crear `Controllers/OrdersController.cs`,
  `appsettings.json`, `appsettings.Development.json` y
  `Properties/launchSettings.json` con el contenido VERBATIM
  de la sección 7.5 del demo M0.
- Verificar `dotnet build` sobre la solución entera.
- Commit: `(api) OrdersController con 5 endpoints REST + Program.cs con DI, EF Core In-Memory y MediatR`

## Tarea 6: C6 — frontend

- `ng new frontend --routing=true --style=scss --standalone=true --strict=true --package-manager=npm --skip-git=true`
- Sustituir / crear los ficheros listados en la sección 7.6 del demo M0
  con su contenido VERBATIM:
  - frontend/package.json
  - frontend/src/styles.scss
  - frontend/src/styles/_tokens.scss
  - frontend/src/main.ts
  - frontend/src/app/app.component.ts
  - frontend/src/app/app.component.html
  - frontend/src/app/app.config.ts
  - frontend/src/app/app.routes.ts
  - frontend/src/app/orders/orders-list.component.ts
  - frontend/src/app/orders/order-detail.component.ts
- `cd frontend; npm install; npm run build`
- Commit: `(frontend) Angular 19 standalone + signals: OrdersListComponent, OrderDetailComponent, app.routes, app.config y _tokens.scss`

## Tarea 7: C7 — tests

- `dotnet new xunit -n OrderManagement.Tests -o tests/OrderManagement.Tests -f net10.0`
- `dotnet sln add tests/OrderManagement.Tests/OrderManagement.Tests.csproj`
- Borrar `UnitTest1.cs`.
- Referencias a Domain, Application, Infrastructure y Api.
- Paquetes: NSubstitute 5.3.0, FluentAssertions 7.0.0, Microsoft.AspNetCore.Mvc.Testing 10.0.0.
- El `.csproj` tiene que quedar EXACTAMENTE como aparece en la sección 7.7 del demo M0.
- Verificar `dotnet build` y `dotnet test` (este último debe decir "No tests found").
- Commit: `(tests) scaffold OrderManagement.Tests con xUnit + NSubstitute + FluentAssertions (sin tests)`

# Restricciones (importantes)

- NO crear `CLAUDE.md`. Eso es la demo 1.2b.
- NO crear `.claude/` ni `.claude/settings.json`. Eso es la demo 1.2b.
- NO crear `scripts/`. Eso es la demo 1.3a.
- NO añadir endpoint `cancel` al controller. Eso es la demo 1.1 (en vivo, descartado).
- NO añadir excepción `InvalidOrderStateException`. Eso es la demo 1.3b.
- NO añadir documentación XML en métodos públicos. Eso es la demo 1.3a o posterior.
- NO añadir tests reales en `tests/OrderManagement.Tests/`. Eso es el módulo 5.
- NO hacer `git push`. Yo lo haré tras revisar.
- NO mergear `demo/0.1` a `main` automáticamente. La sección 11 lo cubre y lo decide Pedro.
- NO modificar el README más allá del placeholder de C1 (la demo 1.1 lo sustituye en su rama).
- NO inventar versiones de paquetes — usa las exactas de cada tarea.

# Cuando termines, dime

1. Que la rama `demo/0.1` está creada y tiene los 7 commits en orden.
2. Que `dotnet build` desde la raíz pasa con 0 warnings y 0 errores.
3. Que `cd frontend; npm run build` pasa sin errores.
4. Que `git ls-files` confirma que NO existen `CLAUDE.md`, `.claude/`,
   tests reales, endpoint cancel ni `InvalidOrderStateException`.
5. Resumen breve del log: `git log --oneline demo/0.1`.

Si algo falla durante el proceso (un paquete que no resuelve, un error
de versión, una inconsistencia entre el contenido verbatim que te paso
y lo que `dotnet new` genera), PARA y dímelo antes de seguir. No
inventes soluciones.
````

---

## 9. Artefactos esperados al terminar

**Tienen que existir:**

```
.gitignore
README.md
global.json
OrderManagement.sln
src/OrderManagement.Domain/                       (csproj + Entities + Enums)
src/OrderManagement.Application/                  (csproj + Commands + Queries + Handlers + Validators + Exceptions + Abstractions)
src/OrderManagement.Infrastructure/               (csproj + Persistence + Repositories + Services)
src/OrderManagement.Api/                          (csproj + Controllers + Program.cs + appsettings + launchSettings)
frontend/                                         (Angular 19 con orders-list, order-detail, _tokens.scss, app.config, app.routes)
tests/OrderManagement.Tests/                      (csproj con xUnit + NSubstitute + FluentAssertions, sin .cs)
```

Siete commits en `demo/0.1` con los mensajes y orden de la sección 6.

**No deben existir:**

- `CLAUDE.md`
- `.claude/` (ni carpeta vacía)
- `scripts/`
- Endpoint `POST /api/orders/{id}/cancel` ni cualquier variante en `OrdersController.cs`
- `InvalidOrderStateException.cs`
- Documentación XML (`/// <summary>`) en métodos públicos
- Ficheros `.cs` con clases de test dentro de `tests/`
- `docs/` (ese se crea en la 1.1)

---

## 10. Verificación final (criterios de aceptación)

Comandos para validar el estado de `demo/0.1` antes de mergear:

```powershell
# 1. Build .NET
dotnet restore
dotnet build
# Esperado: 0 warnings, 0 errors

# 2. Build Angular
cd frontend
npm install
npm run build
cd ..
# Esperado: build limpio

# 3. Tests (sin tests reales)
dotnet test
# Esperado: "No tests found" o equivalente — pasa porque no hay tests

# 4. Estado git
git status
# Esperado: working tree clean

git log --oneline demo/0.1
# Esperado, en orden inverso (más reciente arriba):
#   <hash> (tests) scaffold OrderManagement.Tests con xUnit + NSubstitute + FluentAssertions (sin tests)
#   <hash> (frontend) Angular 19 standalone + signals: OrdersListComponent, OrderDetailComponent, app.routes, app.config y _tokens.scss
#   <hash> (api) OrdersController con 5 endpoints REST + Program.cs con DI, EF Core In-Memory y MediatR
#   <hash> (infrastructure) AppDbContext EF Core In-Memory, repositorios y mocks de IEmailService / IPaymentService
#   <hash> (application) commands, queries, handlers MediatR, validator FluentValidation y exceptions tipadas
#   <hash> (domain) entidades Order, OrderItem, Customer y enum OrderStatus
#   <hash> (init) solución vacía OrderManagement.sln + .gitignore + README inicial + global.json

# 5. Ausencias confirmadas
git ls-files | Select-String -Pattern "CLAUDE\.md$"          # Esperado: vacío
git ls-files | Select-String -Pattern "\.claude/"            # Esperado: vacío
git ls-files | Select-String -Pattern "InvalidOrderState"    # Esperado: vacío
git ls-files tests/ | Select-String -Pattern "\.cs$"         # Esperado: vacío (solo .csproj indexado)

# 6. Endpoint cancel ausente del controller
Select-String -Path src/OrderManagement.Api/Controllers/OrdersController.cs -Pattern "cancel" -SimpleMatch
# Esperado: vacío
```

Si los seis pasos pasan, la rama está lista. Si alguno falla, **investigar antes de mergear**.

---

## 11. Cierre M0 — merge a `main`

Cuando la sección 10 pasa entera, se mergea `demo/0.1` a `main`:

```powershell
git checkout main
git merge --ff-only demo/0.1
```

> Se usa `--ff-only` para mantener el historial lineal — `main` partía de un commit vacío y `demo/0.1` lo extiende, así que el fast-forward es posible y deseable. Los siete commits quedan visibles en `main` con sus mensajes originales.

**No se borra `demo/0.1`** — queda como rama histórica para quien quiera ver la construcción capa a capa.

Tras el merge, el repo queda listo para que la 1.1 ejecute su flujo:

```powershell
git checkout -b demo/1.1   # parte de main, como dice la 1.1
```

Y el resto del curso sigue su curso normal.

---

## 12. Notas para Pedro (decisiones tomadas)

**Versiones fijadas y por qué:**

- **`global.json` con SDK 10.0.100 y `rollForward: latestFeature`** — fija la familia .NET 10 y permite minor versions hacia adelante. Cualquiera con .NET 10 SDK instalado compila sin sorpresas.
- **MediatR 12.5.0** — última versión libre antes del cambio comercial anunciado por Jimmy Bogard. Si en algún punto el curso quiere migrar al fork comunitario (`MediatR.Contracts` u otro), basta con cambiar el paquete y los `using`. **Decidí 12.5.0** para mantener el código del curso ejecutable sin licencias adicionales.
- **FluentValidation 11.11.0** y **FluentValidation.AspNetCore 11.3.0** — versiones estables compatibles entre sí.
- **EF Core 10.0.0 In-Memory** — alineado con .NET 10. EF In-Memory tiene limitaciones conocidas (no respeta transacciones, no genera SQL real) pero es perfecto para curso y demos. Si surge en el módulo 5 alguna casuística que choque, está documentado el cambio a SQLite In-Memory como alternativa.
- **NSubstitute 5.3.0** + **FluentAssertions 7.0.0** + **Microsoft.AspNetCore.Mvc.Testing 10.0.0** — el stack que la 5.3a asume. Fijados aquí para que el módulo 5 no tenga que añadirlos.
- **Angular 19.2.x** — la familia que el curso declara. Standalone components y signals son nativos.

**Decisiones de diseño:**

- **Abstracciones (`IOrderRepository`, etc.) en `Application/Abstractions/`** y re-exports vacíos en `Infrastructure/Repositories/` y `Infrastructure/Services/`. Mantiene la dependencia *Application → Infrastructure* invertida (Ports & Adapters). Si te parece sobreingeniería, se puede simplificar moviendo las interfaces directamente a Infrastructure y haciendo que Application referencie Infrastructure — pero eso rompe el principio de capas que muchos cursos enseñan.
- **`CancelOrderHandler` lanza `InvalidOperationException`** y NO una excepción tipada. **Es deliberado.** La 1.1 explota esa inconsistencia en su demo en vivo (el agente la detecta y propone refactor). Si arreglamos aquí esa excepción tipada, **rompemos el caso pedagógico de la 1.1**.
- **No hay endpoint `cancel`** en el controller. Mismo motivo — la 1.1 lo añade en vivo y luego se descarta.
- **Customer seed data** en `OnModelCreating` con dos clientes (Acme y Globex) — el frontend se puede probar manualmente con `customerId=1` o `customerId=2` desde el primer minuto. Sin esto, el alumno no podría hacer un `POST /api/orders` válido.
- **EF Core In-Memory + `EnsureCreated()` al arrancar** — sin migraciones porque no hay base de datos real. Cuando el curso introduzca SQL Server o PostgreSQL (no en M0–M5), habrá que añadir migraciones.

**Tono y forma de los commits:**

- Mensajes en imperativo presente, en español, con prefijo de capa entre paréntesis. Coherente con el resto del curso.
- Sin firma `Co-Authored-By: Claude` porque la sección 11 (merge) no genera commit nuevo (`--ff-only`). Los siete commits ya están autorizados por Pedro al ejecutar el prompt.

**Si alguno de los puntos anteriores no encaja con cómo grabarías el curso, el sitio para corregir es esta sección 12 + los ficheros verbatim afectados — no las demás demos.**
