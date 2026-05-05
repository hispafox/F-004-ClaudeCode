# Demo 0.1 — Setup del proyecto OrderManagement (referencia / sin screencast)

> **Versión:** v3 | **Módulo:** 0 | **Sub:** 0.1 | **Estado:** ✅ Versión final
> **Archivo:** `demo_M00-S0.1-setup-ordermanagement-windows-v3.md`
> **Repo:** `F-004-ClaudeCode` (el repo del curso). El proyecto vive como **subcarpeta** `ordermanagement/` dentro del repo, **sin git anidado** y **compartiendo historia con la documentación del curso**.
> **Rama:** los commits C1–C7 van **directamente a `main`** del repo del curso. M0 NO usa rama `demo/*` — es el setup base sobre el que se construye todo. Las ramas `demo/X.Y-before` / `demo/X.Y-after` aparecen a partir del Módulo 1.
> **Tipo:** Demo de referencia / setup — sin screencast pedagógico. **Excepción al patrón before/after** (ver [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md)): M0 es punto de origen del curso.
> **Plataforma:** Windows (PowerShell 7 + Git for Windows + .NET 10 SDK + Node 22 vía nvm)

---

## 1. Contexto

Las 28 secciones (M0–M5) del curso de Claude Code asumen un proyecto demo OrderManagement (.NET 10 + Angular 19) como hilo conductor. **El proyecto vive como subcarpeta `ordermanagement/` dentro del propio repo del curso `F-004-ClaudeCode`** — comparte git e historia con la documentación. No hay repo separado.

Hasta aquí, **el código del proyecto solo existía descrito en prosa** dentro de las demos M01–M03 (la descripción más completa está en el bloque «Estado del repo al empezar» de la 1.1, líneas 65–153, y se confirma cruzadamente en la 2.1a). Ninguna demo lo construía. La 1.1 lo declaraba explícitamente como *«trabajo previo»*.

Esta demo M0 cubre ese hueco: documenta verbatim el proceso de construcción del proyecto desde cero hasta el estado que la 1.1 asume.

**Por qué no se graba como screencast:**

- Es construcción de sustrato, no escenificación de un concepto del producto Claude Code. Pedagógicamente vacío para el alumno típico.
- Si se grabara, anularía la frase clave del bloque 3 de la 1.1 (*«esta es la primera vez que arrancáis Claude Code»*) y obligaría a retocar la 1.1 ya aprobada.
- A cambio, se distribuye con **commits granulares por capa directamente en `main`** para que el alumno avanzado pueda recorrer la construcción con `git log --oneline` o `git checkout <hash>` por commit.

**Cómo encaja con la 1.1 y con el resto del curso:**

- Los commits C1–C7 van **directamente a `main`** del repo del curso. M0 NO usa rama `demo/*`.
- Cuando M0 cierra, `main` ya contiene el código completo del proyecto en su estado de partida — exactamente lo que la 1.1 espera.
- Las demás demos siguen el patrón **before/after** definido en [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md): cada sección no conceptual tiene `demo/X.Y-before` (estado de partida del screencast) y `demo/X.Y-after` (estado final que la siguiente clase asume). Las ramas `demo/*` parten de `main` (que ya tiene M0 dentro) y avanzan el proyecto módulo a módulo.

> **M0 es la base del curso.** No tiene rama propia ni patrón before/after — es el suelo sobre el que se construye todo. De aquí parten todas las cadenas de ramas de los Módulos 1–5.

---

## 2. Objetivo

Al terminar M0, la subcarpeta `ordermanagement/` del repo del curso cumple verbatim el estado descrito en la sección 5 del demo 1.1 ([demo_M01-S1.1-...:65-153](demo_M01-S1.1-ciclo-agentic-en-accion-v3.md#L65)):

- API REST .NET 10 funcional con cinco endpoints CRUD (`OrdersController`).
- Capas separadas: `Domain`, `Application` (MediatR + FluentValidation), `Infrastructure` (EF Core In-Memory + mocks), `Api`.
- Lógica de cancelación implementada en handler **pero no expuesta como endpoint independiente** (eso es lo que la 1.1 pide al agente que añada en vivo, y luego se descarta).
- Frontend Angular 19 con dos componentes standalone (`OrdersListComponent`, `OrderDetailComponent`), signals y un `_tokens.scss` mínimo.
- Carpeta `tests/OrderManagement.Tests/` **vacía** con `.csproj` configurado pero sin tests reales.
- **Sin** `CLAUDE.md`, **sin** `.claude/`, **sin** documentación XML, **sin** scripts ni hooks. Todo ese contenido es de demos posteriores.

Compila con `dotnet build` y `npm run build` sin warnings ni errores.

---

## 3. Punto de partida

Repo del curso `F-004-ClaudeCode` ya iniciado, en rama `main`, con `docs/` ya commiteado. La subcarpeta `ordermanagement/` aún no existe (o existe vacía). No hay código todavía.

Comprobación previa:

```powershell
cd C:\w\repos\F-004-ClaudeCode
git status
# Esperado: rama main, working tree clean (o solo ordermanagement/ untracked sin contenido)
```

Si la subcarpeta `ordermanagement/` ya tiene contenido de un setup anterior, **detente y verifica** antes de tocar nada — M0 asume que la subcarpeta está vacía o no existe.

```powershell
mkdir ordermanagement -Force | Out-Null   # crea la subcarpeta si no existe
```

---

## 4. Destino de los commits

Los siete commits C1–C7 (sección 6) van **directamente a `main`** del repo del curso. NO hay rama `demo/0.1` ni merge posterior — los commits se aplican secuencialmente sobre la línea principal del repo.

Tras los siete commits:

- `main` contiene `docs/` (curso) + `ordermanagement/` (proyecto demo completo).
- `git log --oneline main` muestra los siete commits del proyecto sobre el commit del curso (`docs: contenido inicial del curso`).
- A partir de aquí, las ramas `demo/X.Y-before` / `demo/X.Y-after` de los Módulos 1–5 parten de `main` y avanzan el código del proyecto siguiendo el patrón documentado en [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md).

---

## 5. Estado del repo al terminar

Árbol relevante de `F-004-ClaudeCode/` tras los siete commits (la subcarpeta `ordermanagement/` queda completa; el resto del repo del curso —`docs/`, `.git/` raíz, etc.— se mantiene intacto):

```
F-004-ClaudeCode/                              (repo del curso, rama main)
├── .git/                                      (raíz, gestiona TODO el repo)
├── .gitignore                                 (raíz del repo del curso)
├── docs/                                      (contenido del curso, ya commiteado antes)
└── ordermanagement/                           (NUEVO tras M0 — proyecto demo)
    ├── README.md                              (placeholder; la 1.1 lo sustituye)
    ├── global.json                            (SDK .NET 10 fijado)
    ├── OrderManagement.slnx                   (formato .slnx de .NET 10)
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

**Verificación funcional al terminar** (todos los comandos desde `c:\w\repos\F-004-ClaudeCode\ordermanagement\`):

- `dotnet build` desde la subcarpeta del proyecto: 0 warnings, 0 errors.
- `dotnet run --project src/OrderManagement.Api` arranca la API en `https://localhost:5001` (HTTPS dev cert) — no se ejecuta en M0, solo se valida que arranca si Pedro lo prueba manualmente.
- `cd frontend; npm install; npm run build` produce build limpio.
- Desde la raíz del repo del curso: `git log --oneline main` muestra los siete commits de M0 sobre el commit del curso.
- **No existe** `ordermanagement/CLAUDE.md`, **no existe** `ordermanagement/.claude/`, **no existe** documentación XML, **no existe** endpoint `cancel` ni excepción `InvalidOrderStateException`, **no existen** tests reales.

---

## 6. Plan de commits granulares en `main`

Siete commits, cada uno cierra una capa. Mensajes en imperativo presente, en español, prefijo de capa entre paréntesis:

| # | Mensaje | Contenido |
|---|---|---|
| C1 | `(init) ordermanagement: solución vacía OrderManagement.slnx + README inicial + global.json` | Andamiaje del proyecto dentro de la subcarpeta. Sin código aún. (El `.gitignore` ya lo gestiona el repo del curso, no se crea aquí.) |
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

> **Premisa común:** todos los comandos asumen que estás en `c:\w\repos\F-004-ClaudeCode\ordermanagement\` (la subcarpeta del proyecto dentro del repo del curso). El repo ya está iniciado y la rama activa es `main`. **No hace falta `git init` ni crear ramas** — los commits se aplican directamente a `main`. Para entrar a la subcarpeta:
>
> ```powershell
> Set-Location c:\w\repos\F-004-ClaudeCode\ordermanagement
> ```

---

### 7.1 — C1 init: solución, README y global.json

**Comandos** (desde `c:\w\repos\F-004-ClaudeCode\ordermanagement\`):

```powershell
# .NET SDK fijado
dotnet new globaljson --sdk-version 10.0.100 --roll-forward latestFeature

# Solución vacía (con .NET 10 SDK genera formato .slnx por defecto)
dotnet new sln -n OrderManagement
```

> **Nota sobre `.gitignore`:** NO se ejecuta `dotnet new gitignore` aquí. El repo del curso ya tiene su `.gitignore` raíz que aplica a toda la jerarquía. Si hace falta añadir reglas específicas de .NET / Angular / VS, se añaden al `.gitignore` raíz del repo del curso, no a la subcarpeta.

**Contenido verbatim de `ordermanagement/global.json`:**

```json
{
  "sdk": {
    "version": "10.0.100",
    "rollForward": "latestFeature"
  }
}
```

**Contenido verbatim de `ordermanagement/README.md`** (placeholder mínimo; la demo 1.1 lo sustituye en su rama):

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

**Reglas adicionales que conviene añadir al `.gitignore` raíz del repo del curso** (si no están ya — desde `c:\w\repos\F-004-ClaudeCode\`):

```gitignore
# Proyecto demo (Frontend / .NET / editor)
ordermanagement/frontend/node_modules/
ordermanagement/frontend/dist/
ordermanagement/frontend/.angular/
ordermanagement/frontend/coverage/
ordermanagement/**/bin/
ordermanagement/**/obj/
.vs/
.vscode/
*.user
```

**Commit** (desde la subcarpeta del proyecto; git encuentra el repo raíz solo):

```powershell
git add ordermanagement/README.md ordermanagement/global.json ordermanagement/OrderManagement.slnx
git commit -m "(init) ordermanagement: solución vacía OrderManagement.slnx + README inicial + global.json"
```

> Nota: los `git add` se ejecutan con paths relativos desde la raíz del repo del curso (`c:\w\repos\F-004-ClaudeCode\`). Aunque el cwd sea la subcarpeta, conviene escribir paths con prefijo `ordermanagement/` para que sean inequívocos en el log.

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
git add ordermanagement/src/OrderManagement.Domain ordermanagement/OrderManagement.slnx
git commit -m "(domain) ordermanagement: entidades Order, OrderItem, Customer y enum OrderStatus"
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
git add ordermanagement/src/OrderManagement.Application ordermanagement/OrderManagement.slnx
git commit -m "(application) ordermanagement: commands, queries, handlers MediatR, validator FluentValidation y exceptions tipadas"
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
git add ordermanagement/src/OrderManagement.Infrastructure ordermanagement/OrderManagement.slnx
git commit -m "(infrastructure) ordermanagement: AppDbContext EF Core In-Memory, repositorios y mocks de IEmailService / IPaymentService"
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
using OrderRepository = OrderManagement.Infrastructure.Repositories.OrderRepository;
using CustomerRepository = OrderManagement.Infrastructure.Repositories.CustomerRepository;
using EmailService = OrderManagement.Infrastructure.Services.EmailService;
using PaymentService = OrderManagement.Infrastructure.Services.PaymentService;

// Nota: usamos type aliases (en vez de `using` simple de los namespaces de
// Infrastructure) para evitar la ambigüedad de IOrderRepository entre
// Application.Abstractions (la interfaz real) e Infrastructure.Repositories
// (el re-export vacío que la extiende). Las interfaces del AddScoped quedan
// bajo Application.Abstractions; las implementaciones concretas se traen
// por alias.

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
git add ordermanagement/src/OrderManagement.Api ordermanagement/OrderManagement.slnx
git commit -m "(api) ordermanagement: OrdersController con 5 endpoints REST + Program.cs con DI, EF Core In-Memory y MediatR"
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

      @let o = order();
      @if (loading()) {
        <p>Loading…</p>
      } @else if (error()) {
        <p style="color: var(--color-danger);">{{ error() }}</p>
      } @else if (o) {
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
git add ordermanagement/frontend
git commit -m "(frontend) ordermanagement: Angular 19 standalone + signals: OrdersListComponent, OrderDetailComponent, app.routes, app.config y _tokens.scss"
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
git add ordermanagement/tests ordermanagement/OrderManagement.slnx
git commit -m "(tests) ordermanagement: scaffold OrderManagement.Tests con xUnit + NSubstitute + FluentAssertions (sin tests)"
```

**Verificación parcial:** `dotnet build` desde la raíz → 0 warnings, 0 errors. `dotnet test` → *"No tests found"* (esperado, sin clases de test).

---

## 8. Prompt para Claude Code

> Bloque autocontenido para pegar en una sesión limpia de Claude Code arrancada **en la raíz del repo del curso** (`c:\w\repos\F-004-ClaudeCode\`). Ejecuta las siete fases y deja `main` con el proyecto completo en `ordermanagement/`. **No crea ramas — los commits van directamente a `main`**.

````
Estoy preparando la demo M0 (setup) del curso de Claude Code para devs
.NET + Angular. Trabajo en Windows con PowerShell 7. Esta demo construye
desde cero el proyecto OrderManagement como subcarpeta del repo del
curso. Es la base sobre la que se construyen todas las demás demos.

# Contexto

Estoy en la raíz del repo del curso:

  c:\w\repos\F-004-ClaudeCode

Está en rama `main`, con `docs/` ya commiteado. La subcarpeta
`ordermanagement/` ya existe pero está vacía o solo contiene
`global.json` + `OrderManagement.slnx` previos del C1 (verificar antes
de empezar; si están, saltarse C1 al `git add`). Tengo instalado:

- .NET 10 SDK (10.0.300-preview o similar)
- Node 22 LTS (vía nvm — `nvm use 22.18.0` antes de C6)
- Angular CLI 19 (verificar; si no, `npm install -g @angular/cli@19`)
- Git for Windows
- PowerShell 7

Necesito que ejecutes el setup completo creando 7 commits granulares
DIRECTAMENTE EN `main` (sin crear rama `demo/*`). Cada commit cierra
una capa. Todos los `dotnet new` se ejecutan desde la subcarpeta
`ordermanagement/`; los `git add`/`git commit` desde la raíz del repo
del curso o con paths con prefijo `ordermanagement/`.

# Lo que necesito

## Tarea 0: cwd y verificaciones previas

```powershell
Set-Location c:\w\repos\F-004-ClaudeCode\ordermanagement
git status   # debería estar limpio o con ordermanagement/ untracked
```

NO ejecutes `git init`, NO crees rama `demo/*`. Trabaja en `main`.

## Tarea 1: C1 — init

(Si `global.json` y `OrderManagement.slnx` ya existen porque se
crearon antes, salta los `dotnet new` y pasa directamente al
`git add`/`commit`.)

- `dotnet new globaljson --sdk-version 10.0.100 --roll-forward latestFeature`
- `dotnet new sln -n OrderManagement` (con .NET 10 genera `.slnx`)
- Crear `README.md` placeholder dentro de `ordermanagement/` con el
  contenido de la sección 7.1.
- NO crear `.gitignore` dentro de `ordermanagement/` — el repo del
  curso ya gestiona el suyo. Si hace falta añadir reglas de Frontend/
  .NET/editor, hacerlo al `.gitignore` raíz del curso.
- Commit:

```powershell
git add ordermanagement/README.md ordermanagement/global.json ordermanagement/OrderManagement.slnx
git commit -m "(init) ordermanagement: solución vacía OrderManagement.slnx + README inicial + global.json"
```

## Tarea 2: C2 — domain

(Desde `ordermanagement/`)

- `dotnet new classlib -n OrderManagement.Domain -o src/OrderManagement.Domain -f net10.0`
- `dotnet sln add src/OrderManagement.Domain/OrderManagement.Domain.csproj`
- Borrar `src/OrderManagement.Domain/Class1.cs`.
- Crear `src/OrderManagement.Domain/Entities/` y `src/OrderManagement.Domain/Enums/`.
- Crear `OrderStatus.cs`, `Customer.cs`, `OrderItem.cs`, `Order.cs` con
  el contenido VERBATIM de la sección 7.2 del demo M0.
- Verificar `dotnet build src/OrderManagement.Domain`.
- Commit:

```powershell
git add ordermanagement/src/OrderManagement.Domain ordermanagement/OrderManagement.slnx
git commit -m "(domain) ordermanagement: entidades Order, OrderItem, Customer y enum OrderStatus"
```

## Tarea 3: C3 — application

- `dotnet new classlib -n OrderManagement.Application -o src/OrderManagement.Application -f net10.0`
- `dotnet sln add src/OrderManagement.Application/OrderManagement.Application.csproj`
- Borrar `Class1.cs`.
- Referencia a Domain y paquetes:
  - MediatR 12.5.0
  - FluentValidation 11.11.0
- Crear `Commands/`, `Queries/`, `Handlers/`, `Validators/`, `Exceptions/`, `Abstractions/`.
- Crear todos los ficheros listados en la sección 7.3 del demo M0 con
  contenido VERBATIM.
- Verificar `dotnet build src/OrderManagement.Application`.
- Commit:

```powershell
git add ordermanagement/src/OrderManagement.Application ordermanagement/OrderManagement.slnx
git commit -m "(application) ordermanagement: commands, queries, handlers MediatR, validator FluentValidation y exceptions tipadas"
```

## Tarea 4: C4 — infrastructure

- `dotnet new classlib -n OrderManagement.Infrastructure -o src/OrderManagement.Infrastructure -f net10.0`
- `dotnet sln add src/OrderManagement.Infrastructure/OrderManagement.Infrastructure.csproj`
- Borrar `Class1.cs`.
- Referencias a Domain y Application; paquetes EF Core 10 + Logging.
- Crear `Persistence/`, `Repositories/`, `Services/` con el VERBATIM de §7.4.
- Verificar build.
- Commit:

```powershell
git add ordermanagement/src/OrderManagement.Infrastructure ordermanagement/OrderManagement.slnx
git commit -m "(infrastructure) ordermanagement: AppDbContext EF Core In-Memory, repositorios y mocks de IEmailService / IPaymentService"
```

## Tarea 5: C5 — api

- `dotnet new web -n OrderManagement.Api -o src/OrderManagement.Api -f net10.0`
- `dotnet sln add src/OrderManagement.Api/OrderManagement.Api.csproj`
- Referencias a Application e Infrastructure; paquetes MediatR 12.5.0,
  FluentValidation.AspNetCore 11.3.0, Microsoft.AspNetCore.OpenApi 10.0.0.
- Sustituir `Program.cs` y crear `Controllers/OrdersController.cs`,
  `appsettings.json`, `appsettings.Development.json` y
  `Properties/launchSettings.json` con VERBATIM de §7.5.
- Verificar `dotnet build` sobre la solución entera (ya tienes 4 proyectos).
- Commit:

```powershell
git add ordermanagement/src/OrderManagement.Api ordermanagement/OrderManagement.slnx
git commit -m "(api) ordermanagement: OrdersController con 5 endpoints REST + Program.cs con DI, EF Core In-Memory y MediatR"
```

## Tarea 6: C6 — frontend

> Antes de empezar: `nvm use 22.18.0` para activar Node 22.

- `ng new frontend --routing=true --style=scss --standalone=true --strict=true --package-manager=npm --skip-git=true`
- Sustituir / crear los ficheros listados en la sección 7.6 con VERBATIM:
  package.json, src/styles.scss, src/styles/_tokens.scss, src/main.ts,
  src/app/app.component.ts/.html, src/app/app.config.ts, src/app/app.routes.ts,
  src/app/orders/orders-list.component.ts, src/app/orders/order-detail.component.ts.
- `cd frontend; npm install; npm run build` — debe pasar limpio.
- Commit:

```powershell
git add ordermanagement/frontend
git commit -m "(frontend) ordermanagement: Angular 19 standalone + signals: OrdersListComponent, OrderDetailComponent, app.routes, app.config y _tokens.scss"
```

## Tarea 7: C7 — tests

- `dotnet new xunit -n OrderManagement.Tests -o tests/OrderManagement.Tests -f net10.0`
- `dotnet sln add tests/OrderManagement.Tests/OrderManagement.Tests.csproj`
- Borrar `UnitTest1.cs`.
- Referencias a Domain, Application, Infrastructure, Api; paquetes
  NSubstitute 5.3.0, FluentAssertions 7.0.0, Microsoft.AspNetCore.Mvc.Testing 10.0.0.
- El `.csproj` debe quedar EXACTAMENTE como en §7.7.
- `dotnet build` (limpio) y `dotnet test` (debe decir «No tests found»).
- Commit:

```powershell
git add ordermanagement/tests ordermanagement/OrderManagement.slnx
git commit -m "(tests) ordermanagement: scaffold OrderManagement.Tests con xUnit + NSubstitute + FluentAssertions (sin tests)"
```

# Restricciones (importantes)

- NO ejecutar `git init`, NO crear ramas `demo/*`. M0 va directamente a `main`.
- NO crear `.gitignore` dentro de `ordermanagement/`. El repo del curso ya tiene el suyo raíz.
- NO crear `CLAUDE.md`. Eso es la demo 1.2b.
- NO crear `.claude/` ni `.claude/settings.json`. Eso es la demo 1.2b.
- NO crear `scripts/`. Eso es la demo 1.3a.
- NO añadir endpoint `cancel` al controller. Eso es la demo 1.1 (en vivo, descartado).
- NO añadir excepción `InvalidOrderStateException`. Eso es la demo 1.3b.
- NO añadir documentación XML en métodos públicos. Eso es la demo 1.3a o posterior.
- NO añadir tests reales en `ordermanagement/tests/OrderManagement.Tests/`. Eso es el módulo 5.
- NO hacer `git push`. Yo lo haré tras revisar.
- NO modificar el README más allá del placeholder de C1 (la demo 1.1 lo sustituye en su rama).
- NO inventar versiones de paquetes — usa las exactas de cada tarea.

# Cuando termines, dime

1. Que `git log --oneline main -10` muestra los 7 commits del proyecto sobre el commit del curso, en orden.
2. Que `dotnet build` desde `ordermanagement/` pasa con 0 warnings y 0 errores.
3. Que `cd ordermanagement/frontend; npm run build` pasa sin errores.
4. Que `git ls-files ordermanagement/` confirma que NO existen `CLAUDE.md`, `.claude/`, tests reales, endpoint cancel ni `InvalidOrderStateException`.

Si algo falla durante el proceso (un paquete que no resuelve, un error
de versión, una inconsistencia entre el contenido verbatim que te paso
y lo que `dotnet new` genera), PARA y dímelo antes de seguir. No
inventes soluciones.
````

---

## 9. Artefactos esperados al terminar

**Tienen que existir:**

```
ordermanagement/README.md
ordermanagement/global.json
ordermanagement/OrderManagement.slnx
ordermanagement/src/OrderManagement.Domain/                       (csproj + Entities + Enums)
ordermanagement/src/OrderManagement.Application/                  (csproj + Commands + Queries + Handlers + Validators + Exceptions + Abstractions)
ordermanagement/src/OrderManagement.Infrastructure/               (csproj + Persistence + Repositories + Services)
ordermanagement/src/OrderManagement.Api/                          (csproj + Controllers + Program.cs + appsettings + launchSettings)
ordermanagement/frontend/                                         (Angular 19 con orders-list, order-detail, _tokens.scss, app.config, app.routes)
ordermanagement/tests/OrderManagement.Tests/                      (csproj con xUnit + NSubstitute + FluentAssertions, sin .cs)
```

Siete commits en `main` con los mensajes y orden de la sección 6, sobre el commit del curso (`docs: contenido inicial del curso`).

**No deben existir:**

- `ordermanagement/CLAUDE.md`
- `ordermanagement/.claude/` (ni carpeta vacía)
- `ordermanagement/scripts/`
- `ordermanagement/.gitignore` propio (lo gestiona el repo del curso)
- Endpoint `POST /api/orders/{id}/cancel` ni cualquier variante en `OrdersController.cs`
- `InvalidOrderStateException.cs`
- Documentación XML (`/// <summary>`) en métodos públicos
- Ficheros `.cs` con clases de test dentro de `ordermanagement/tests/`

---

## 10. Verificación final (criterios de aceptación)

Comandos para validar el estado tras los siete commits:

```powershell
# Posicionarse en la subcarpeta del proyecto
Set-Location c:\w\repos\F-004-ClaudeCode\ordermanagement

# 1. Build .NET
dotnet restore
dotnet build
# Esperado: 0 warnings, 0 errors

# 2. Build Angular (con Node 22 activo: nvm use 22.18.0)
cd frontend
npm install
npm run build
cd ..
# Esperado: build limpio

# 3. Tests (sin tests reales)
dotnet test
# Esperado: "No tests found" o equivalente — pasa porque no hay tests

# 4. Estado git (desde la raíz del repo del curso)
Set-Location c:\w\repos\F-004-ClaudeCode
git status
# Esperado: working tree clean

git log --oneline main -10
# Esperado (más reciente arriba), tras el commit del curso:
#   <hash> (tests) ordermanagement: scaffold OrderManagement.Tests ...
#   <hash> (frontend) ordermanagement: Angular 19 standalone + signals ...
#   <hash> (api) ordermanagement: OrdersController + Program.cs ...
#   <hash> (infrastructure) ordermanagement: AppDbContext + repos + mocks
#   <hash> (application) ordermanagement: commands, queries, handlers ...
#   <hash> (domain) ordermanagement: entidades Order, OrderItem, Customer y enum OrderStatus
#   <hash> (init) ordermanagement: solución vacía OrderManagement.slnx + README + global.json
#   <hash> docs: contenido inicial del curso
#   09868d1 Initial commit

# 5. Ausencias confirmadas (filtrando solo la subcarpeta del proyecto)
git ls-files ordermanagement/ | Select-String -Pattern "CLAUDE\.md$"          # Esperado: vacío
git ls-files ordermanagement/ | Select-String -Pattern "\.claude/"            # Esperado: vacío
git ls-files ordermanagement/ | Select-String -Pattern "InvalidOrderState"    # Esperado: vacío
git ls-files ordermanagement/tests/ | Select-String -Pattern "\.cs$"          # Esperado: vacío (solo .csproj)

# 6. Endpoint cancel ausente del controller
Select-String -Path ordermanagement/src/OrderManagement.Api/Controllers/OrdersController.cs -Pattern "cancel" -SimpleMatch
# Esperado: vacío
```

Si los seis pasos pasan, M0 está cerrado y `main` queda lista para que la **demo 1.1** parta de aquí. Las ramas `demo/X.Y-before` / `demo/X.Y-after` de los Módulos 1–5 nacerán a partir de `main` siguiendo el patrón de [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md).

---

## 11. (Sin merge — M0 vive en `main`)

A diferencia del modelo previo (donde M0 vivía en una rama `demo/0.1` que se mergeaba al cerrar), en la versión actual los commits C1–C7 van directamente a `main`. **No hay nada que mergear**.

Si por algún motivo se quisiera reservar M0 en una rama histórica de inspección, basta con etiquetar el último commit:

```powershell
git tag m0-setup-completed
```

Pero no es necesario para que el curso funcione. La 1.1 parte de `main`, y todas las ramas posteriores también.

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

- Mensajes en imperativo presente, en español, con prefijo de capa entre paréntesis seguido de `ordermanagement:` para indicar el ámbito (la subcarpeta del proyecto, distinguible de los commits de `docs:`).
- Cada commit incluye `Co-Authored-By: Claude Opus 4.7 (1M context) <noreply@anthropic.com>` si lo ejecutaste con Claude Code activo y autorizaste la firma. Si lo prefieres sin firma, lo quitas de los `git commit -m`.

**Estructura del repo y ramas:**

- `main` contiene **TODO**: docs del curso + código del proyecto OrderManagement, compartiendo historia. No hay separación.
- M0 NO usa rama `demo/*` — es base. Los siete commits viven en `main` directamente.
- A partir del Módulo 1, las ramas `demo/X.Y-before` / `demo/X.Y-after` parten de `main` (que ya tiene M0 dentro) siguiendo el patrón de [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md).
- El alumno hace `git clone <repo del curso>` y obtiene el curso completo + el código en su estado de partida (rama main). Para inspeccionar un punto intermedio: `git checkout demo/X.Y-before` o `git checkout demo/X.Y-after`.

**Si alguno de los puntos anteriores no encaja con cómo grabarías el curso, el sitio para corregir es esta sección 12 + los ficheros verbatim afectados — no las demás demos.**
