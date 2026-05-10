# Demo 3.2b — Context bank, paralelo vs serial, Claude Code as MCP, Agent Teams

> **Versión:** v1 | **Módulo:** 3 | **Sub:** 3.2b | **Estado:** ✅ Versión final
> **Archivo:** `demo_M03-S3.2b-memoria-paralelo-agent-teams-windows-v1.md`
> **Branch before:** `demo/3.2b-before`  (con endpoint de búsqueda multi-fichero commiteado para que el fan-out lo analice)
> **Branch after:**  `demo/3.2b-after`   (estado final pre-cocinado: pre-commit-check ampliado, convention-checker, pre-pr-check)
> **Branch parent:** `demo/3.2a-after`
> **Tiempo total estimado:** ~26-30 minutos
> **Tipo:** Demo de cierre del módulo 3.2 (MIXTA: INFRA + CÓDIGO). **Cierra la orquestación con cuatro piezas que faltaban: context bank como memoria compartida, paralelo vs serial (fan-out/fan-in), Claude Code como MCP server, y Agent Teams con el dato honesto del coste 10-15x.** Materializa context bank y paralelización como showcases en directo. MCP server y Agent Teams como referencia conceptual. Sigue el patrón **before/after** definido en [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md).
> **Plataforma:** Windows 11 (PowerShell 7).

---

## 1. Contexto

Cerramos la 3.2a con `pre-commit-check` orquestando al `dotnet-reviewer` en serie con loop validator → implementer y techo de 3 iteraciones. **Funciona — para un caso simple**. Pero la gamma 3.2b plantea cuatro preguntas que se hace el alumno:

1. *"Si tuviera **varios subagentes compartiendo información** en un workflow más complejo (un planner, un implementer, un tester, un reviewer), ¿cómo se la pasan entre sí sin saturar prompts?"* → **context bank** (slides 3-10).
2. *"¿Por qué **todo en serie**? Si tengo subtareas independientes, ¿no puedo paralelizar para reducir el tiempo total?"* → **paralelo vs serial / fan-out / fan-in** (slides 11-17).
3. *"¿Y si quiero que **otros agentes hablen con mi Claude Code**, no solo el revés?"* → **Claude Code como MCP server** (slides 18-22).
4. *"¿Y si una sola sesión no me llega — quiero **varias sesiones colaborando**?"* → **Agent Teams** (slides 23-31), con el dato honesto **10-15x más tokens**.

Esta demo aterriza la teoría con dos showcases reales y dos referencias conceptuales:

- **Showcase context bank**: ampliar `pre-commit-check` con `.claude/workflow-state/<session-id>/` donde el reviewer escribe `REVIEW.md` y el orquestador lo lee para aplicar fixes. **Trazabilidad real**.
- **Showcase fan-out / fan-in**: nuevo skill `pre-pr-check` que **lanza tres subagentes en paralelo** (`dotnet-reviewer` + `repo-explorer` para impacto + un nuevo subagente `convention-checker`) y combina los tres resultados.
- **Referencia conceptual Claude Code as MCP server**: cuándo merece la pena, casos de uso, sin showcase práctico (la activación varía mucho entre versiones).
- **Referencia conceptual Agent Teams**: el patrón experimental con la **cifra dura del coste**.

> **Tipo de demo:** cierre del módulo de orquestación. La rama `demo/3.2b-after` queda con `pre-commit-check` ampliado con context bank, un nuevo skill `pre-pr-check` paralelizado, y un subagente nuevo `convention-checker`. **Es la última demo del módulo 3.2 — cierra la orquestación entera dentro de una sesión**.

---

## 2. Objetivo de la demo

Cinco cosas concretas que tienen que quedar en la cabeza del alumno cuando termine los ~26 minutos de screencast:

1. **El context bank es memoria compartida entre subagentes** — ficheros markdown durables en `.claude/workflow-state/<session>/` que **persisten durante el workflow** y dan trazabilidad, recuperación si la sesión muere a mitad, y loops baratos. **No es CLAUDE.md** — vive solo durante el workflow.

2. **La regla rápida paralelo vs serial**: si la salida de A condiciona cómo B trabaja → serial. Si A y B se combinan al final pero no se influyen entre sí → paralelo. **El error típico**: paralelizar cuando hay dependencias ocultas. La gamma 3.2b slide 16.

3. **Fan-out / fan-in escala bien hasta 4-5 subagentes en paralelo**. Más allá, **el coste de coordinación se come el ahorro**. Y la regla matriz: si A genera input que B necesita → serial. Si las tareas son ortogonales → paralelo.

4. **Claude Code como MCP server existe pero es para integraciones serias** — otro sistema, otra herramienta, otro agente que NO es Claude Code. Si solo quieres delegar dentro de una sesión → subagentes. Si quieres dos Claude Codes colaborando → Agent Teams.

5. **Agent Teams cuesta 10-15x más tokens según el whitepaper de Anthropic.** No es detalle menor — diferencia entre *"voy a montar tres subagentes"* y *"voy a montar Agent Teams porque mola"*. Si la tarea no justifica el orden de magnitud, no compensa. **Empieza con uno o dos subagentes, mide con `/usage`, escala solo cuando los números cuadren**.

Y dos cosas que tienen que **NO quedar** en su cabeza:

- *"Cuanto más paralelo, mejor."* — falso. **El error más típico** es paralelizar con dependencias ocultas. Resultado: ahorras segundos en paralelo y los gastas en una ronda extra cuando el reviewer detecta cosas que no podía saber sin el output del tester.
- *"Agent Teams es la evolución natural de los subagentes."* — falso. Es el **patrón experimental** para casos extremos. La gamma 3.2b slide 28: *"el 95% de las tareas del día a día — implementar endpoint, escribir tests, refactorizar módulo. Subagentes bastan."*.

---

## 3. Branch `demo/3.2b-before`

Punto de partida del screencast.

```
demo/3.2b-before
```

**Parte de:** `demo/3.2a-after`.

**Estado del repo:** todo lo de `demo/3.2a-after` (5 skills incluido `pre-commit-check`, 2 subagentes) más dos cambios commiteados:

1. **`.gitignore`** ampliado para excluir `.claude/workflow-state/`.
2. **Endpoint nuevo de búsqueda por estado** en seis ficheros: `SearchOrdersByStatusQuery.cs`, `SearchOrdersByStatusHandler.cs`, ampliación de `OrdersController.cs`, ampliación de `IOrderRepository.cs`, ampliación de `OrderRepository.cs`, y un componente Angular `frontend/src/app/components/order-search/`. **Todos commiteados** — el feature ya está en el repo. Lo que el screencast hace es **revisarlo con el `pre-pr-check` recién construido en paralelo (3 subagentes)** antes de un PR ficticio.

**Qué NO hay en `-before`:**
- Sin `convention-checker` en `.claude/agents/`.
- Sin `pre-pr-check` en `.claude/skills/`.
- Sin context bank en `pre-commit-check` (la versión de 3.2a sigue intacta).
- Sin marca `[x]` en `docs/DEMOS.md`.

> El formador hace `git checkout demo/3.2b-before` antes de empezar a grabar.

---

## 4. Branch `demo/3.2b-after`

Estado final que cierra el módulo 3.2 (la 3.3a parte de aquí).

```
demo/3.2b-after
```

**Parte de:** `demo/3.2b-before`.

**Qué añade respecto a `-before`:**

1. **`pre-commit-check` actualizado** con context bank — los subagentes escriben a `.claude/workflow-state/<session>/` y el orquestador lee de ahí.
2. **Subagente nuevo `convention-checker`** en `.claude/agents/convention-checker.md` — verifica naming, estructura de carpetas, convenciones del CLAUDE.md sin solapar con `dotnet-reviewer`.
3. **Skill nuevo `pre-pr-check`** en `.claude/skills/pre-pr-check/SKILL.md` — orquestador con **fan-out / fan-in**: lanza `dotnet-reviewer`, `repo-explorer` (para análisis de impacto) y `convention-checker` en paralelo, combina los tres.
4. **Marca `[x]`** en `docs/DEMOS.md` y `docs/subagentes-explorados.md` ampliado con notas del cierre del módulo 3.2.

**Cómo se prepara:** ver §6b. Se materializa antes de grabar.

> Durante la grabación, el formador construye el subagente y el skill nuevos, amplía `pre-commit-check` con context bank, y ejecuta `pre-pr-check` sobre el endpoint de búsqueda commiteado (los 3 subagentes en paralelo). Al cerrar descarta los cambios reales y la siguiente clase parte de `demo/3.2b-after` ya pre-cocinada.

---

## 5. Estado del repo al hacer `git checkout demo/3.2b-before`

Idéntico a `demo/3.2a-after`, con dos cambios añadidos (gitignore + endpoint de búsqueda multi-fichero, todo commiteado):

```
ordermanagement/
├── .claude/
│   ├── settings.json
│   ├── skills/
│   │   ├── angular-component/      (con context: fork de 3.2a)
│   │   ├── commit-style/
│   │   ├── db-reset/
│   │   ├── frontend-design/
│   │   └── pre-commit-check/       (de 3.2a, sin context bank)
│   └── agents/
│       ├── repo-explorer.md
│       └── dotnet-reviewer.md
├── docs/
├── scripts/
├── src/
├── frontend/
├── tests/
├── CLAUDE.md
├── .gitignore
└── README.md
```

**Estado clave para esta demo:**

- **5 skills + 2 subagentes** existentes desde 3.2a.
- **El `pre-commit-check` actual NO tiene context bank** — pasa hallazgos por prompts. La 3.2b lo amplía.
- Para mostrar el caso de paralelización con `pre-pr-check`, vamos a introducir **un cambio multi-fichero**: un nuevo endpoint de búsqueda con cambios en handler, controller, y un componente Angular. Cada subagente del fan-out lo va a mirar con su perspectiva.

**Estado de la máquina Windows del formador:**

```
✅ Claude Code v2.1.x con subagentes operativos
✅ Git for Windows
✅ PowerShell 7
✅ VS Code con el repo cargado en demo/3.2b-before
✅ Las 5 skills + 2 subagentes funcionando
```

**Lo que el alumno verá al final de la demo:**

- `.claude/workflow-state/` creado en directo, con ficheros `EXPLORATION.md`, `REVIEW.md`, etc.
- El `pre-commit-check` ampliado leyendo y escribiendo al context bank — trazabilidad visible.
- Subagente `convention-checker` creado para que el fan-out tenga 3 piezas distintas.
- Skill `pre-pr-check` con paralelización lanzando los 3 subagentes a la vez.
- Comparativa de tiempos: serial (~3 min) vs paralelo (~1 min). **Pedagogía operativa**.
- El árbol de decisión completo del manual línea 460.
- Referencia rápida de MCP server y Agent Teams.

---

## 6a. Prompt para Claude Code — preparar `demo/3.2b-before`

> Crea la rama de partida del screencast desde `demo/3.2a-after` con dos cambios commiteados: el `.gitignore` ampliado (workflow-state) y el endpoint de búsqueda multi-fichero (6 ficheros). **No crea el subagente, ni el skill, ni amplía pre-commit-check** — esa es la pieza viva.

````
Estoy preparando la demo 3.2b del curso de Claude Code (cierre del
módulo 3.2 con context bank, paralelización fan-out/fan-in, MCP,
Agent Teams). Sigue el patrón before/after (ver demo M0.2).

Quiero que prepares la rama `demo/3.2b-before` desde `demo/3.2a-after`
con dos cambios commiteados: el .gitignore ampliado para workflow-state,
y un endpoint multi-fichero (6 ficheros) de búsqueda de pedidos por
estado, todo COMMITEADO. Durante el screencast el formador construirá
pre-pr-check y revisará el endpoint ya commiteado en paralelo.

# Lo que necesito

Cuatro tareas:

## Tarea 1: crear la rama

```powershell
git checkout demo/3.2a-after
git pull
git checkout -b demo/3.2b-before
```

## Tarea 2: actualizar .gitignore

Añade al final:

```
# Workflow state (context bank temporal de orquestaciones)
.claude/workflow-state/
```

## Tarea 3: introducir cambios multi-fichero (endpoint de búsqueda)

Añade un endpoint sencillo de búsqueda de pedidos por estado.

### a) En ordermanagement/src/OrderManagement.Application/Queries/

Crear SearchOrdersByStatusQuery.cs:

```csharp
using MediatR;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Queries;

public record SearchOrdersByStatusQuery(OrderStatus Status) : IRequest<IReadOnlyList<Order>>;
```

### b) En ordermanagement/src/OrderManagement.Application/Handlers/

Crear SearchOrdersByStatusHandler.cs:

```csharp
using MediatR;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Queries;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Handlers;

public class SearchOrdersByStatusHandler
    : IRequestHandler<SearchOrdersByStatusQuery, IReadOnlyList<Order>>
{
    private readonly IOrderRepository _orders;

    public SearchOrdersByStatusHandler(IOrderRepository orders)
    {
        _orders = orders;
    }

    public Task<IReadOnlyList<Order>> Handle(
        SearchOrdersByStatusQuery request,
        CancellationToken ct)
    {
        return _orders.GetByStatusAsync(request.Status, ct);
    }
}
```

### c) En ordermanagement/src/OrderManagement.Api/Controllers/OrdersController.cs

Añadir el endpoint nuevo (mantén el resto del controller intacto):

```csharp
[HttpGet("search")]
public async Task<IActionResult> SearchByStatus(
    [FromQuery] OrderStatus status,
    CancellationToken ct)
{
    var orders = await _mediator.Send(new SearchOrdersByStatusQuery(status), ct);
    return Ok(orders);
}
```

### d) En ordermanagement/src/OrderManagement.Application/Abstractions/IOrderRepository.cs

Añadir el método nuevo (mantén los demás miembros intactos):

```csharp
Task<IReadOnlyList<Order>> GetByStatusAsync(OrderStatus status, CancellationToken ct);
```

### e) En ordermanagement/src/OrderManagement.Infrastructure/Repositories/OrderRepository.cs

Implementar el método (sigue el estilo de `GetAllAsync`):

```csharp
public async Task<IReadOnlyList<Order>> GetByStatusAsync(OrderStatus status, CancellationToken ct)
{
    return await _context.Orders
        .Include(o => o.Items)
        .Where(o => o.Status == status)
        .ToListAsync(ct);
}
```

### f) En ordermanagement/frontend/src/app/components/

Crear la carpeta order-search/ con un componente Angular standalone
generado siguiendo el skill angular-component.
La estructura: order-search.component.ts/.html/.scss/.spec.ts.

Es un componente simple con un input para buscar y un output para
emitir el término de búsqueda.

## Tarea 4: verificar build y commitear todo

```powershell
Set-Location c:\w\repos\F-004-ClaudeCode\ordermanagement
dotnet build
Set-Location c:\w\repos\F-004-ClaudeCode\ordermanagement\frontend
npm run build
Set-Location c:\w\repos\F-004-ClaudeCode
```

Esperado: 0 warnings, 0 errors en ambos.

```powershell
git add .gitignore `
        ordermanagement/src/OrderManagement.Application/Queries/SearchOrdersByStatusQuery.cs `
        ordermanagement/src/OrderManagement.Application/Handlers/SearchOrdersByStatusHandler.cs `
        ordermanagement/src/OrderManagement.Api/Controllers/OrdersController.cs `
        ordermanagement/src/OrderManagement.Application/Abstractions/IOrderRepository.cs `
        ordermanagement/src/OrderManagement.Infrastructure/Repositories/OrderRepository.cs `
        ordermanagement/frontend/src/app/components/order-search/
git commit -m "demo/3.2b-before: workflow-state en gitignore + endpoint search multi-fichero"
```

NO marques `[x]` en `docs/DEMOS.md` todavía — eso va en `-after`.
NO hagas push.

Durante el screencast el formador hará `git diff HEAD~1 HEAD` para que
pre-pr-check analice el endpoint commiteado con sus 3 subagentes en
paralelo.

# Restricciones (importantes)

- NO crees `.claude/agents/convention-checker.md`. EN VIVO.
- NO crees `.claude/skills/pre-pr-check/`. EN VIVO.
- NO modifiques pre-commit-check (lo ampliamos en vivo con context bank).
- NO modifiques los subagentes existentes ni CLAUDE.md ni settings.json.

# Cuando termines, dime

1. Que la rama demo/3.2b-before está creada desde demo/3.2a-after.
2. Que .gitignore tiene la entrada de workflow-state.
3. Que el endpoint multi-fichero está creado y commiteado (junto al .gitignore).
4. Que los builds (.NET y Angular) pasan.
5. Que `git log --oneline -1` muestra el commit del endpoint.

Si tienes dudas, para y pregúntame.
````

---

## 6b. Prompt para Claude Code — preparar `demo/3.2b-after`

> Materializa la rama final con `pre-commit-check` ampliado con context bank, `convention-checker`, `pre-pr-check` paralelizado y `docs/DEMOS.md` marcado. Equivalente a lo que el formador construye en vivo.

````
Estoy preparando la demo 3.2b del curso de Claude Code. Esta rama
-after pre-cocina el pre-commit-check ampliado con context bank, el
subagente convention-checker, y el skill pre-pr-check con fan-out
paralelo a 3 subagentes.

# Contexto

Estoy en la rama `demo/3.2b-before` del repo `ordermanagement`. La rama
parte de `demo/3.2a-after` y tiene el endpoint multi-fichero de búsqueda
commiteado más el .gitignore con workflow-state. NO tiene aún:
- pre-commit-check ampliado con context bank.
- convention-checker.
- pre-pr-check.

Quiero que prepares la rama `demo/3.2b-after` desde `demo/3.2b-before`
con esos tres artefactos + DEMOS.md marcado + subagentes-explorados.md
ampliado.

# Lo que necesito

Cinco tareas:

## Tarea 1: crear la rama

```powershell
git checkout demo/3.2b-before
git checkout -b demo/3.2b-after
```

## Tarea 2: ampliar `.claude/skills/pre-commit-check/SKILL.md` con context bank

Modifica el SKILL.md existente para que el workflow:
- Genere un sessionId al iniciar y cree `.claude/workflow-state/<sessionId>/`.
- Antes de invocar al dotnet-reviewer, escriba en `<sessionId>/INPUT.md` el diff staged.
- Le pida al reviewer que escriba sus hallazgos en `<sessionId>/REVIEW.md`.
- El orquestador lea `REVIEW.md`, aplique fixes (registrando cada iteración en `<sessionId>/CHANGES.md`).
- El loop sigue siendo techo=3.

Mantén el frontmatter (`name`, `description`, `context: fork`) intacto.

## Tarea 3: crear `.claude/agents/convention-checker.md`

Subagente que verifica naming, estructura de carpetas y convenciones
del CLAUDE.md sin solapar con dotnet-reviewer.

- Frontmatter:
  - `name: convention-checker`
  - `description: Revisa que los cambios staged respeten las convenciones del CLAUDE.md (naming, estructura de carpetas, orden de imports, organización por capa). Reporta NO_MATCH con file:line:convención_violada:fix_sugerido. Úsalo en paralelo con dotnet-reviewer.`
  - `tools: Read, Grep, Glob, Bash(git diff:*)`
  - `model: haiku`
- Body: rol de verificador de convenciones, formato de salida verbatim, restricción de no modificar código.

## Tarea 4: crear `.claude/skills/pre-pr-check/SKILL.md`

Skill orquestador con fan-out / fan-in:

- Frontmatter:
  - `name: pre-pr-check`
  - `description: Antes de abrir un PR, lanza tres subagentes en paralelo (dotnet-reviewer para calidad de código, convention-checker para convenciones, repo-explorer para análisis de impacto) sobre el diff y combina los tres resultados en un informe único. Úsalo después de pre-commit-check, antes de push.`
  - `context: fork`
- Body: define el workflow:
  1. Generar sessionId y crear `.claude/workflow-state/<sessionId>/`.
  2. Lanzar EN PARALELO los tres subagentes con el mismo diff.
  3. Cada subagente escribe a su fichero (`REVIEW.md`, `CONVENTIONS.md`, `IMPACT.md`).
  4. Fan-in: el orquestador combina los tres en `<sessionId>/PR_SUMMARY.md`.
  5. Reportar al usuario el PR_SUMMARY consolidado.

Documenta el techo: si los 3 subagentes coinciden en algún hallazgo CRÍTICO, no abrir PR.

## Tarea 5: marcar DEMOS.md, ampliar subagentes-explorados.md, build y commit

Marca la 3.2b en `docs/DEMOS.md`:

```
- [x] **demo/3.2b** — Context bank, paralelo vs serial, MCP, Agent Teams
```

Añade a `docs/subagentes-explorados.md` una sección «### Cierre módulo 3.2 (3.2b)» con tres bullets:
- pre-commit-check ahora con context bank en .claude/workflow-state/.
- convention-checker añadido (Haiku, paralelo).
- pre-pr-check con fan-out 3 subagentes en paralelo.

Verifica con `dotnet build` (0 warnings, 0 errors) y commit:

```powershell
Set-Location c:\w\repos\F-004-ClaudeCode\ordermanagement
dotnet build
Set-Location c:\w\repos\F-004-ClaudeCode
git add ordermanagement/.claude/skills `
        ordermanagement/.claude/agents `
        docs/DEMOS.md `
        docs/subagentes-explorados.md
git commit -m "demo/3.2b-after: context bank en pre-commit-check + convention-checker + pre-pr-check paralelo"
```

NO hagas push.

# Restricciones (importantes)

- NO modifiques los subagentes existentes (repo-explorer, dotnet-reviewer)
  ni el resto de skills.
- NO modifiques CLAUDE.md ni settings.json.
- NO toques el código .NET ni Angular del endpoint.
- Respeta las 5 reglas técnicas críticas en los nuevos skills/agents.

# Cuando termines, dime

1. Que la rama demo/3.2b-after está creada desde demo/3.2b-before.
2. Que pre-commit-check tiene la sección de context bank documentada.
3. Que convention-checker existe con frontmatter correcto (Haiku).
4. Que pre-pr-check existe con el workflow fan-out / fan-in.
5. Que docs/DEMOS.md está marcado.
6. Que docs/subagentes-explorados.md está ampliado.
7. Que dotnet build pasa.

Si tienes dudas, para y pregúntame.
````

---

## 7. Artefactos que Claude Code debe generar

```
✓ Rama demo/3.2b-before (parte de demo/3.2a-after) con UN commit:
  ├── .gitignore con .claude/workflow-state/ excluido
  └── Endpoint multi-fichero (6 ficheros, todos commiteados):
      - SearchOrdersByStatusQuery.cs
      - SearchOrdersByStatusHandler.cs
      - OrdersController.cs (ampliado)
      - IOrderRepository.cs (ampliado)
      - OrderRepository.cs (ampliado)
      - frontend/src/app/components/order-search/* (4 ficheros)
✓ Rama demo/3.2b-after (parte de demo/3.2b-before) con UN commit:
  ├── .claude/skills/pre-commit-check/SKILL.md (ampliado con context bank)
  ├── .claude/agents/convention-checker.md (nuevo, Haiku, paralelo)
  ├── .claude/skills/pre-pr-check/SKILL.md (nuevo, fan-out 3 subagentes)
  ├── docs/DEMOS.md con 3.2b marcada como [x]
  └── docs/subagentes-explorados.md ampliado
✓ Verificación de build OK
```

**Lo que NO debe haber generado:**

- ❌ `.claude/agents/convention-checker.md` (en vivo)
- ❌ `.claude/skills/pre-pr-check/` (en vivo)
- ❌ Modificación a `pre-commit-check` (en vivo)
- ❌ Los cambios del endpoint commiteados (deben quedar sin stage)

> Si Claude Code se anticipa, **se rechaza el output**.

**Lo que el formador commitea EN VIVO sobre `demo/3.2b-before` durante el screencast:**

```
Durante la grabación, sobre demo/3.2b-before, se hacen commits ficticios:

1. "demo/3.2b-after: pre-commit-check ampliado con context bank"
   └── .claude/skills/pre-commit-check/SKILL.md (MODIFICADO)

2. "demo/3.2b-after: subagente convention-checker + skill pre-pr-check (paralelo)"
   └── .claude/agents/convention-checker.md (NUEVO)
   └── .claude/skills/pre-pr-check/SKILL.md (NUEVO)
   └── docs/subagentes-explorados.md (MODIFICADO)

(El endpoint de búsqueda ya está en demo/3.2b-before — el formador
NO lo commitea de nuevo, solo lo analiza con pre-pr-check vía
`git diff HEAD~1 HEAD`.)

Al cerrar el screencast: el formador descarta los commits reales.
La siguiente clase parte de demo/3.2b-after (pre-cocinada en §6b)
que es equivalente al resultado del screencast.
```

---

## 8. Guion del screencast

**Duración estimada del screencast: ~24-28 minutos.**

Once bloques. Cierre del módulo 3.2 — combina dos showcases prácticos con dos referencias conceptuales más recap.

> **Antes de empezar a grabar**, asegúrate de:
> - Tener PowerShell 7 abierto en pantalla completa, fuente al menos 16pt.
> - Tener VS Code abierto con el repo en `demo/3.2b-before`.
> - Verificar que los 6 ficheros del endpoint están sin stagear: `git status` debe mostrarlos.
> - Verificar que `git diff` muestra los cambios claramente.
> - Cerrar Slack, Teams, navegadores con notificaciones.

---

### Bloque 1 — Setup y planteamiento del cierre del 3.2 (~1 min 30 seg)

**Pantalla compartida.** A la izquierda VS Code con el repo en `demo/3.2b-before`. A la derecha terminal PowerShell.

**En la terminal:**

```powershell
git status
```

```
On branch demo/3.2b

Untracked files:
  src/OrderManagement.Application/Queries/SearchOrdersByStatusQuery.cs
  src/OrderManagement.Application/Handlers/SearchOrdersByStatusHandler.cs
  frontend/src/app/components/order-search/

Changes not staged for commit:
  modified: src/OrderManagement.Api/Controllers/OrdersController.cs
  modified: src/OrderManagement.Application/Interfaces/IOrderRepository.cs
  modified: src/OrderManagement.Infrastructure/Repositories/OrderRepository.cs
```

**Lo que dices:**

> "Estamos en `demo/3.2b-before`. Mirad el último commit: **un endpoint nuevo multi-fichero ya commiteado**. Backend (query, handler, controller, interface, repo) y un componente Angular. **Seis ficheros tocados**. Es el material que vamos a usar para la paralelización al final — `pre-pr-check` lo analizará con `git diff HEAD~1 HEAD`.
>
> Esta demo cierra el módulo 3.2. Cubre cuatro temas que la gamma 3.2b sembró:
>
> Una. **Context bank** — memoria compartida entre subagentes en `.claude/workflow-state/`. Vamos a ampliar `pre-commit-check` para que use el bank.
>
> Dos. **Paralelo vs serial** — fan-out / fan-in. Construimos un nuevo skill `pre-pr-check` que lanza tres subagentes a la vez sobre el endpoint multi-fichero.
>
> Tres. **Claude Code como MCP server** — referencia conceptual rápida.
>
> Cuatro. **Agent Teams** — el patrón experimental con la cifra dura: **10-15x más tokens** según el whitepaper de Anthropic.
>
> Empezamos con context bank."

**Tiempo:** ~90 segundos.

---

### Bloque 2 — Context bank: el problema y la solución (~3 min)

> "**El problema que resuelve el context bank**. La gamma 3.2b slides 3-5 lo cubrió."

**En el editor de texto al lado, escribo (contenido pedagógico):**

```
EL PROBLEMA: VARIOS SUBAGENTES COMPARTIENDO INFO

Cuando un workflow tiene varios subagentes:
  → planner produce un plan
  → implementer lee el plan, hace cambios
  → reviewer mira los cambios y comparar con plan
  → tester genera tests del código modificado

¿Cómo se la pasan entre sí?

FORMA 1 (mala): pasarla por prompts
─────────────────────────────────────
  "Aquí está el plan. Aquí están los hallazgos del reviewer
   anterior. Aquí los ficheros tocados..."

  PROBLEMAS:
    • Prompts cada vez más grandes
    • Cada subagente parsea de nuevo lo ya parseado
    • Información se duplica
    • Pierdes trazabilidad

FORMA 2 (buena): artefactos durables en el repo
────────────────────────────────────────────────
  Ficheros markdown que persisten durante el workflow.
  
  Cada subagente:
    • LEE lo que necesita
    • ESCRIBE lo que produce
    • La info SOBREVIVE entre invocaciones

  Esto es el CONTEXT BANK.


ESTRUCTURA TÍPICA

.claude/workflow-state/<feature-name>/
├── PLAN.md              ← producido por planner, leído por implementer
├── EXPLORATION.md       ← producido por explorer
├── CHANGES.md           ← registrado por implementer
├── TESTS.md             ← producido por tester
└── REVIEW.md            ← producido por reviewer (en cada iteración)


VENTAJAS

  ✓ TRAZABILIDAD
    Si algo falla, los ficheros te dicen qué pasó.
    Es tu log.

  ✓ RECUPERACIÓN
    Si la sesión muere a mitad, puedes retomar.
    El estado está persistido.

  ✓ LOOPS BARATOS
    Validator devuelve al implementer.
    Implementer NO necesita re-explicar todo.
    Lee REVIEW.md y aplica fixes.

  ✓ AUDITORÍA
    En equipos grandes, los ficheros son evidencia.


NO ES CLAUDE.md

  CLAUDE.md       = contexto persistente, aplica a CADA SESIÓN del repo.
  Context bank    = contexto del WORKFLOW EN CURSO, vive solo durante él.

  Diferencia clave: el context bank se LIMPIA al terminar el workflow
                    (o el .gitignore lo oculta de git).
```

> "**Cuatro ventajas operativas**. Trazabilidad, recuperación, loops baratos, auditoría. Y la diferencia crítica con `CLAUDE.md`: **el context bank vive solo durante el workflow**.
>
> Voy a ampliar el `pre-commit-check` que ya teníamos para que use context bank. Tres ventajas concretas: el alumno **podrá ver REVIEW.md después de cada iteración del loop**, **podrá inspeccionar qué pasó si la cosa falla**, y **el orquestador no tendrá que pasar todo por prompt**."

**Tiempo:** ~3 minutos.

---

### Bloque 3 — Ampliar `pre-commit-check` con context bank (~3 min)

**En VS Code, abro `.claude/skills/pre-commit-check/SKILL.md` y modifico los pasos del workflow para usar context bank.**

**Localizo la sección "## Workflow" y la reemplazo:**

```markdown
## Workflow

### Setup: crear el context bank

Genera un identificador único para este workflow:

```!
mkdir -p .claude/workflow-state
date +%Y%m%d-%H%M%S > .claude/workflow-state/.session-id
echo "Session: $(cat .claude/workflow-state/.session-id)"
```

A partir de aquí, todos los artefactos van a `.claude/workflow-state/<session>/`
donde `<session>` es el timestamp generado.

### Paso 1: Verificar que hay cambios staged

```!
git status
git diff --cached --stat > .claude/workflow-state/<session>/CHANGES.md
```

Si no hay cambios staged, devuelve mensaje y termina.

### Paso 2: Primera invocación del reviewer

Invoca al subagente `dotnet-reviewer` indicándole que **escriba su
output a `.claude/workflow-state/<session>/REVIEW-1.md`** en lugar
de devolverlo solo por prompt.

Tras la invocación, lee REVIEW-1.md para procesar los hallazgos.

### Paso 3: Decisión sobre los hallazgos

Lee `.claude/workflow-state/<session>/REVIEW-N.md` (el más reciente).

**Caso A — Sin hallazgos críticos:** procede al paso 5.

**Caso B — Hay hallazgos críticos:**
1. Aplica los fixes propuestos en los ficheros afectados.
2. Re-stagea los ficheros (`git add <fichero>`).
3. Registra en `.claude/workflow-state/<session>/FIXES-N.md` qué se
   cambió en esta iteración.
4. Vuelve al paso 2 con N incrementado (REVIEW-2.md, REVIEW-3.md...).

**Caso C — Solo hallazgos importantes/sugerencia:**
Pregunta al usuario, sin loop automático.

### Paso 4: Loop con techo (3 iteraciones)

Si tras 3 iteraciones siguen apareciendo CRÍTICOS:
1. Para el loop.
2. Devuelve resumen al usuario con referencia a los ficheros del
   context bank: *"Revisa REVIEW-1.md, REVIEW-2.md, REVIEW-3.md y
   FIXES-*.md en `.claude/workflow-state/<session>/`"*.
3. NO procedas al commit.

### Paso 5: Cierre

Cuando la revisión queda limpia:

1. Confirma al usuario:
   *"Revisión limpia tras N iteraciones. Cambios listos para commit."*
   *"Trazabilidad en `.claude/workflow-state/<session>/`"*
2. NO ejecutes `git commit` por tu cuenta.
3. Sugiere el comando exacto.

## Limpieza

El context bank **NO se limpia automáticamente**. El usuario puede:
- Inspeccionarlo después del workflow para entender qué pasó.
- Borrarlo manualmente cuando ya no lo necesita.
- Confiar en que `.gitignore` lo excluye de git.

Si quieres limpiar workflows viejos:
```!
rm -rf .claude/workflow-state/<session-anterior>/
```
```

**Salvo. Vuelvo al guion:**

> "Mirad las cinco diferencias respecto a la versión sin context bank:
>
> Una. **Setup nuevo** que crea `.claude/workflow-state/` con un id de sesión basado en timestamp. Cada workflow tiene su carpeta.
>
> Dos. **El reviewer escribe a `REVIEW-1.md`** en lugar de devolver solo por prompt. **Persistente**.
>
> Tres. **Cada iteración del loop genera REVIEW-N.md y FIXES-N.md**. El alumno puede ver toda la historia.
>
> Cuatro. **El paso 4 con techo** referencia los ficheros del bank. *'Revisa REVIEW-1.md, REVIEW-2.md, REVIEW-3.md'*. Trazabilidad real.
>
> Cinco. **El cierre** confirma con referencia al bank. *'Trazabilidad en .claude/workflow-state/<session>/'*.
>
> Y mirad la limpieza al final: **NO se limpia automáticamente**. La gamma 3.2b slide 9 lo dijo: *'algunos equipos prefieren mantener los ficheros como historial'*. Si te interesa el log, queda. Si no, lo borras tú. **El `.gitignore` ya lo excluye** así que no contamina commits.
>
> No vamos a ejecutar este `pre-commit-check` ahora — la rama `demo/3.2b-before` no tiene cambios anti-patrón que cazar. Pero **el alumno tiene la versión ampliada** y puede usarla cuando le toque."

**Tiempo:** ~3 minutos.

---

### Bloque 4 — Paralelo vs serial: cuándo elegir cada patrón (~2 min 30 seg)

> "Antes de paralelizar nada, **la regla**. La gamma 3.2b slides 11-17 lo cubrió."

**En el editor:**

```
PARALELO vs SERIAL — la decisión rápida

REGLA MATRIZ

  Salida de A condiciona cómo B trabaja
    → SERIAL

  A y B son independientes
  (se combinan al final pero no se influyen)
    → PARALELO


CASOS CONCRETOS

  | Situación                                        | Patrón |
  |--------------------------------------------------|--------|
  | B necesita el output de A                        | Serial |
  | Subtareas independientes                         | Paralelo |
  | Validación con varios ángulos (sec + estilo + tests) | Paralelo |
  | Pipeline transformación (explorar→planificar→ejecutar→validar) | Serial |
  | Varios subagentes opinando sobre el mismo input  | Paralelo |


FAN-OUT / FAN-IN

  Orquestador hace FAN-OUT:
    → lanza N invocaciones a subagentes EN PARALELO
    → cada uno trabaja en su contexto aislado

  Cuando todos terminan, FAN-IN:
    → recoge los N resultados
    → los combina en una respuesta unificada


HASTA CUÁNTOS EN PARALELO

  Escala bien hasta 4-5 subagentes.
  Más allá, el coste de coordinación se come el ahorro.


EL ERROR TÍPICO

  "Quiero que tester y reviewer corran en paralelo
   para ahorrar tiempo."

  PROBLEMA: el reviewer necesita ver los tests también.
            Sus hallazgos sin tests están INCOMPLETOS.

  RESULTADO: ahorras los segundos del paralelo
             y los gastas en una ronda extra
             cuando el reviewer reporta cosas
             que no podía saber sin output del tester.

  REGLA: si la salida de A condiciona cómo B trabaja,
         es SERIAL. Aunque tengas que esperar.
```

> "**Regla mnemotécnica**: si A genera input que B necesita, **serial**. Si las tareas son ortogonales, **paralelo**.
>
> Y atención al error típico — paralelizar reviewer y tester. **Suena bien, pero el reviewer necesita los tests para revisar bien**. Salida de tester condiciona el reviewer. **Serial**.
>
> ¿Cuándo paralelo entonces? Cuando tienes **varias perspectivas independientes sobre el mismo input**. Por ejemplo: revisar un PR completo. Quieres uno que mire código, otro que mire convenciones, otro que mire impacto. **Ninguno depende del otro**. Pueden ir a la vez. **Eso es lo que vamos a construir**."

**Tiempo:** ~2 minutos 30 segundos.

---

### Bloque 5 — Crear el subagente `convention-checker` (~3 min)

> "Para tener tres perspectivas independientes en el fan-out, **necesito un tercer subagente**. Tengo `dotnet-reviewer` (mira código) y `repo-explorer` (mira impacto). Falta uno que mire **convenciones del equipo** sin solapar con `dotnet-reviewer`."

**En VS Code, creo `.claude/agents/convention-checker.md`:**

```markdown
---
name: convention-checker
description: Verifica que los cambios staged respetan las convenciones estructurales del proyecto OrderManagement: nombres de carpetas, ubicación de ficheros nuevos, organización por capas, naming de Commands/Queries/Handlers MediatR. Complementa al dotnet-reviewer (que mira código) revisando la organización del proyecto.
tools: Read, Grep, Glob, Bash(git diff *), Bash(git status)
model: haiku
---

# Convention Checker — system prompt

Eres un verificador de convenciones estructurales del proyecto
OrderManagement. Tu trabajo es complementar al `dotnet-reviewer`
(que revisa código) verificando la **organización del proyecto** —
dónde van los ficheros, cómo se nombran, qué capa toca cada cosa.

## Foco de revisión

### Estructura por capas (basada en CLAUDE.md)

- `OrderManagement.Domain/` — entidades, value objects, agregados.
  NO debe depender de Infrastructure ni Application.
- `OrderManagement.Application/` — handlers, queries, commands,
  interfaces de repositorio. Depende de Domain solo.
  - `Commands/` — clases `<Verbo>Command` que implementan `IRequest<T>`.
  - `Queries/` — clases `<Verbo>Query` que implementan `IRequest<T>`.
  - `Handlers/` — clases `<NombreCommand|Query>Handler`.
  - `Interfaces/` — interfaces de repositorio (`IOrderRepository`...).
- `OrderManagement.Infrastructure/` — implementaciones de repositorios,
  persistencia EF Core. Depende de Application por interfaces.
- `OrderManagement.Api/` — controllers, configuración. Depende de
  Application.

### Naming MediatR

- Commands: `<Verbo><Entidad>Command` (ej: `CreateOrderCommand`).
- Queries: `<Verbo><Filtro>Query` (ej: `SearchOrdersByStatusQuery`).
- Handlers: `<NombreCommandOQuery>Handler` (ej: `CreateOrderHandler`).

### Frontend Angular

- Componentes nuevos en `frontend/src/app/components/<kebab-name>/`.
- Cuatro ficheros: `.ts`, `.html`, `.scss`, `.spec.ts`.
- Selector con prefijo `app-` y kebab-case.

## Cuando seas invocado

1. Ejecuta `git diff --cached --name-only` para listar ficheros
   modificados o añadidos.
2. Para cada fichero nuevo, verifica:
   - ¿Está en la carpeta correcta según su tipo?
   - ¿Sigue el naming convention?
   - ¿Las dependencias entre capas son válidas?
3. Para cada fichero modificado, verifica:
   - ¿El cambio mantiene la estructura?

## Formato de salida

```
HALLAZGOS DE CONVENCIONES
─────────────────────────

[BIEN]
  Resumen de lo que respeta convenciones (1-2 líneas).

[VIOLACIONES] (si existen)
  - Fichero o estructura
    Problema: descripción
    Convención violada: cuál exactamente
    Sugerencia: dónde debería estar / cómo debería llamarse

[OBSERVACIONES] (si aplica)
  - Cosas que no son violaciones pero conviene mencionar.
```

Si todo está bien, devuelve:
**"Convenciones respetadas. Sin observaciones."**

## Restricciones

- **Solo lectura**. No modificas ficheros.
- **Complementa al dotnet-reviewer, no lo solapes.** Tú miras
  estructura. Él mira código.
- **No revises lógica de programación.** Eso es del reviewer.
- **No revises tests faltantes.** Eso es del test-generator (futuro).
```

**Salvo el fichero.**

> "Mirad las decisiones distintas a los subagentes anteriores:
>
> **`name: convention-checker`** — específico, no se solapa con `dotnet-reviewer` ni con `repo-explorer`.
>
> **`description`** explícitamente dice *'Complementa al dotnet-reviewer (que mira código) revisando la organización del proyecto'*. **Esto evita el solape** que la gamma 3.1b slide 31 marcó como anti-patrón.
>
> **`tools`**: read-only + git diff + git status. Sin `Edit` ni `Write`.
>
> **`model: haiku`**. Es verificación mecánica — comparar paths con convenciones, no requiere razonamiento profundo. **Modelo asociado al tipo de tarea**, como aprendimos en 3.1b.
>
> El body lista **las convenciones específicas del proyecto**: estructura por capas, naming MediatR, frontend Angular. **Conoce a OrderManagement**. Y termina con la restricción crítica: *'Complementa al dotnet-reviewer, no lo solapes'*. **Esa frase evita el problema #1 de tener varios validadores**.
>
> Ahora construyo el orquestador paralelo."

**Tiempo:** ~3 minutos.

---

### Bloque 6 — Construir `pre-pr-check` con fan-out / fan-in (~4 min)

**En PowerShell:**

```powershell
mkdir .claude\skills\pre-pr-check
```

**En VS Code, creo `.claude/skills/pre-pr-check/SKILL.md`:**

```markdown
---
name: pre-pr-check
description: Orquesta una validación completa antes de subir un PR. Lanza tres subagentes EN PARALELO (dotnet-reviewer, repo-explorer para análisis de impacto, convention-checker para estructura) y combina los tres reportes en un veredicto unificado. Usar antes de crear o actualizar un PR.
allowed-tools: Read, Edit, Write, Bash(git diff *), Bash(git status), Bash(git log *), Bash(mkdir *)
---

# Pre-PR check — orquestador con fan-out / fan-in

Este skill ejecuta tres validadores **en paralelo** sobre los cambios
staged y combina los resultados en un único veredicto.

## Cuándo se usa

Antes de subir un PR. Es más exhaustivo que `pre-commit-check` (que
solo invoca al reviewer). Aquí queremos tres perspectivas distintas:
código, estructura, impacto.

## Workflow

### Setup: crear context bank del workflow

```!
mkdir -p .claude/workflow-state
SESSION=$(date +%Y%m%d-%H%M%S)-pr-check
mkdir -p .claude/workflow-state/$SESSION
echo "Workflow session: $SESSION"
```

### Paso 1: Verificar staged

```!
git status
git diff --cached --stat
```

Si no hay cambios staged, terminar.

### Paso 2: FAN-OUT — Lanzar tres subagentes en paralelo

**IMPORTANTE: invoca los TRES subagentes a la vez, no en serie.** Cada
uno trabaja en su contexto aislado. Cuando los tres terminen, recoges
sus resultados.

Los tres subagentes a invocar:

#### A) `dotnet-reviewer` — perspectiva código

Invocación: *"Revisa los cambios staged buscando problemas de async,
naming, manejo de errores y violaciones de CLAUDE.md. Escribe el
reporte en `.claude/workflow-state/<session>/REVIEW.md`."*

Devuelve hallazgos clasificados por severidad (CRÍTICO/IMPORTANTE/
SUGERENCIA).

#### B) `repo-explorer` — perspectiva impacto

Invocación: *"Mapea el impacto de los cambios staged: qué partes del
código consumen las funcionalidades modificadas, qué módulos se ven
afectados indirectamente. Escribe el reporte en
`.claude/workflow-state/<session>/IMPACT.md`."*

Devuelve un mapa de impacto: qué consume lo modificado, qué tests
podrían verse afectados, qué documentación quizá hay que actualizar.

#### C) `convention-checker` — perspectiva estructura

Invocación: *"Verifica que los cambios staged respetan las convenciones
estructurales del proyecto. Escribe el reporte en
`.claude/workflow-state/<session>/CONVENTIONS.md`."*

Devuelve violaciones de convenciones de estructura/naming.

### Paso 3: FAN-IN — Combinar los tres reportes

Lee los tres ficheros del context bank:
- `REVIEW.md`
- `IMPACT.md`
- `CONVENTIONS.md`

Combina los hallazgos en un veredicto unificado en
`.claude/workflow-state/<session>/VERDICT.md`:

```markdown
# Pre-PR Check — Veredicto

## Resumen ejecutivo

[1-2 líneas: ¿el PR está listo?]

## Hallazgos por severidad

### CRÍTICO (bloquea el PR)
- (de REVIEW.md, IMPACT.md, o CONVENTIONS.md)

### IMPORTANTE (recomendado arreglar)
- ...

### SUGERENCIA (opcional)
- ...

## Análisis de impacto

[Resumen de IMPACT.md — qué consume lo modificado, qué se ve afectado]

## Convenciones

[Resumen de CONVENTIONS.md — violaciones si las hay]

## Recomendación final

✅ PR LISTO PARA SUBIR / ⚠️ ARREGLAR ANTES / 🚫 NO SUBIR
```

### Paso 4: Presentar al usuario

Muestra el contenido de VERDICT.md.

**NO subas el PR automáticamente.** Esa decisión es del usuario.

## Diferencias clave con pre-commit-check

- **Más exhaustivo**: 3 subagentes en lugar de 1.
- **Paralelo**: los 3 trabajan a la vez. Tiempo total ≈ tiempo del
  más lento, no la suma.
- **Sin loop automático**: aquí no aplicamos fixes — es revisión, no
  iteración. El usuario aplica los fixes él mismo o vuelve a pasar
  por `pre-commit-check`.
- **Veredicto unificado**: un solo fichero con todo el análisis,
  útil para adjuntar al PR como evidencia.

## Restricciones

- **No subas el PR.** El usuario decide.
- **No apliques fixes.** Aquí solo informamos.
- **No invoques los subagentes en serie.** Lánzalos en paralelo —
  cada uno con su prompt y referencia al fichero del bank donde
  escribir.
```

**Salvo.**

> "Mirad las diferencias clave con `pre-commit-check`:
>
> **Tres subagentes invocados en paralelo en el paso 2**. La instrucción literal: *'invoca los TRES subagentes a la vez, no en serie'*. Esto es lo que el modelo procesa para hacer el fan-out.
>
> **Cada subagente escribe a su fichero del context bank**. `REVIEW.md`, `IMPACT.md`, `CONVENTIONS.md`. **Aislado, paralelo, persistente**.
>
> **El paso 3 hace fan-in**: lee los tres ficheros y los combina en `VERDICT.md`. **Ese es el resultado unificado**.
>
> **Sin loop automático** — la diferencia conceptual con `pre-commit-check`. Aquí informamos, el usuario decide. Si hay que arreglar, lanza `pre-commit-check` con los críticos.
>
> Vamos a probarlo."

**Tiempo:** ~4 minutos.

---

### Bloque 7 — Probar fan-out / fan-in en directo (~4 min)

**Stageo los 6 ficheros del endpoint:**

```powershell
git add .
git status
```

```
Changes to be committed:
        new file:   src/OrderManagement.Application/Queries/SearchOrdersByStatusQuery.cs
        new file:   src/OrderManagement.Application/Handlers/SearchOrdersByStatusHandler.cs
        modified:   src/OrderManagement.Api/Controllers/OrdersController.cs
        modified:   src/OrderManagement.Application/Interfaces/IOrderRepository.cs
        modified:   src/OrderManagement.Infrastructure/Repositories/OrderRepository.cs
        new file:   frontend/src/app/components/order-search/order-search.component.ts
        new file:   frontend/src/app/components/order-search/order-search.component.html
        new file:   frontend/src/app/components/order-search/order-search.component.scss
        new file:   frontend/src/app/components/order-search/order-search.component.spec.ts
        new file:   .claude/skills/pre-pr-check/SKILL.md
        new file:   .claude/agents/convention-checker.md
        modified:   .claude/skills/pre-commit-check/SKILL.md
```

> "**Doce ficheros staged**. Endpoint multi-fichero más los nuevos skill y agent. Voy a invocar `pre-pr-check`:"

```powershell
claude
```

```
✓ 6 project skills loaded
✓ 3 project agents loaded
```

**Tecleo:**

```
> /pre-pr-check
```

**Aparece (output ejemplo):**

```
● [Skill activated explicitly: pre-pr-check]

Setup del context bank...
● Bash(mkdir -p .claude/workflow-state)
  Workflow session: 20261105-103045-pr-check

Verificando cambios staged...
● Bash(git status)
● Bash(git diff --cached --stat)

12 ficheros staged. Procediendo a fan-out.

LANZANDO 3 SUBAGENTES EN PARALELO:

  ⟳ dotnet-reviewer    →  REVIEW.md
  ⟳ repo-explorer      →  IMPACT.md
  ⟳ convention-checker →  CONVENTIONS.md

[Los tres ejecutan en sus contextos aislados con sus modelos respectivos]

✓ dotnet-reviewer completed in 12s
✓ convention-checker completed in 8s
✓ repo-explorer completed in 18s

Total tiempo paralelo: 18s (≈ tiempo del más lento)
Equivalente serial: 38s (suma de los tres)
Ahorro: ~53%

FAN-IN — Combinando reportes...
● Read(REVIEW.md)
● Read(IMPACT.md)
● Read(CONVENTIONS.md)
● Write(VERDICT.md)

VERDICT.md:

# Pre-PR Check — Veredicto

## Resumen ejecutivo

PR de 12 ficheros para añadir endpoint /api/orders/search.
Implementación correcta. Algunas observaciones menores.

## Hallazgos por severidad

### CRÍTICO
(ninguno)

### IMPORTANTE
- SearchOrdersByStatusHandler.cs: el campo `Status` se compara como
  string. Considera enum si los estados son finitos (ya hay un patrón
  en Order.Status).
- order-search.component.spec.ts: tests genéricos (solo "should
  create"). Falta cobertura de input/output.

### SUGERENCIA
- Consider añadir paginación a SearchOrdersByStatusQuery para no
  retornar listas potencialmente grandes.

## Análisis de impacto

- Endpoint nuevo, no rompe contrato existente
- IOrderRepository extendido — todos los implementadores deben
  implementar GetByStatusAsync (1 implementador: OrderRepository, ya
  cubierto)
- Componente Angular nuevo, sin dependencias externas

## Convenciones

- ✅ Naming MediatR correcto (SearchOrdersByStatusQuery + Handler)
- ✅ Estructura por capas respetada
- ✅ Componente Angular en frontend/src/app/components/<kebab>/ con 4
  ficheros

## Recomendación final

⚠️ ARREGLAR LOS DOS HALLAZGOS IMPORTANTES ANTES DEL PR

Trazabilidad completa en .claude/workflow-state/20261105-103045-pr-check/
```

> "**Mirad lo que ha pasado**.
>
> **Fan-out**: tres subagentes lanzados a la vez. Cada uno con su modelo, su contexto aislado, su tarea concreta.
>
> **Tiempos en paralelo**: 12s + 8s + 18s, **total 18s** porque ejecutan a la vez. Equivalente serial: 38s. **Ahorro de 53%**.
>
> **Fan-in**: el orquestador lee los tres ficheros y produce `VERDICT.md` unificado. **Un solo fichero con todo el análisis**.
>
> Y el contenido del veredicto: **cero críticos, dos importantes, una sugerencia**. Más análisis de impacto y resumen de convenciones. **Recomendación final clara**: arreglar los dos importantes antes del PR.
>
> Y mirad el final: *'Trazabilidad completa en `.claude/workflow-state/20261105-103045-pr-check/`'*. **Si quiero ver qué dijo el reviewer en detalle, abro REVIEW.md. Si quiero ver el impacto completo, abro IMPACT.md**. Cada subagente con su artefacto durable.
>
> Esto es **el harness paralelo funcionando**. Tres perspectivas independientes, ejecutadas a la vez, combinadas en un veredicto. **Por primera vez vemos paralelización real en Claude Code**.
>
> Salgo y verifico el bank:"

**Salgo (Ctrl+C). En la terminal:**

```powershell
ls .claude\workflow-state\20261105-103045-pr-check\
```

```
REVIEW.md
IMPACT.md
CONVENTIONS.md
VERDICT.md
```

> "Cuatro ficheros del workflow. **Trazabilidad real**. Si mañana quiero entender por qué se aprobó el PR, abro estos cuatro ficheros."

**Tiempo:** ~4 minutos.

---

### Bloque 8 — Claude Code como MCP server: referencia rápida (~2 min)

> "Tercer tema: **Claude Code como MCP server**. La gamma 3.2b slides 18-22. **Conceptual** — no lo vamos a montar en directo porque la activación varía mucho entre versiones."

**En el editor:**

```
CLAUDE CODE COMO MCP SERVER

Hasta ahora: Claude Code es CLIENTE MCP.
            Consume MCP servers (Figma, GitHub, etc.).

¿Y si quieres que OTROS AGENTES hablen con tu Claude Code?

  → Claude Code se EXPONE como MCP server.
  → Otros pueden conectarse como clientes.


CASOS DE USO

  1. Otro Claude Code que delega
     - Sesión "principal" en tu portátil
     - Sesión "auxiliar" para research / docs
     - Auxiliar se conecta a principal vía MCP

  2. Integración con sistemas internos
     - Plataforma del equipo procesa tareas asíncronas
     - Llama a una instancia de Claude Code en servidor

  3. Otros clientes MCP
     - Cursor, Codex CLI, otros
     - Tu Claude Code con tu kit, accesible para todos


CUÁNDO MERECE LA PENA

  ✓ Integración con un TERCERO
    (otro sistema, otra herramienta, otro agente que NO es Claude Code)

  ✗ Delegar dentro de una sesión
    → SUBAGENTES son el camino correcto

  ✗ Dos Claude Codes colaborando
    → AGENT TEAMS están pensados para eso (siguiente bloque)
```

> "**La distinción importante**: si quieres delegar dentro de una sesión, **subagentes**. Si quieres dos Claude Codes colaborando, **Agent Teams**. **Claude Code como MCP server es para integraciones serias** — otro sistema externo.
>
> En la práctica: **el alumno medio no lo va a usar el lunes**. **Pero conviene saber que existe** para cuando llegue el caso. Si os toca trabajar en una empresa donde la plataforma interna procesa PRs en bulk, ahí encaja."

**Tiempo:** ~2 minutos.

---

### Bloque 9 — Agent Teams: referencia experimental con el dato del coste (~2 min 30 seg)

> "Cuarto tema: **Agent Teams**. La gamma 3.2b slides 23-31. **El más experimental** y **el más caro en tokens**."

**En el editor:**

```
AGENT TEAMS — múltiples sesiones colaborando

Hasta ahora: TODO pasa dentro de una sesión.
             Subagentes están dentro del mismo Claude Code.

Agent Teams ROMPE eso:
  → Múltiples sesiones de Claude Code se comunican entre sí
  → Un "Team Lead" recibe la petición y orquesta
  → "Teammates" son sesiones independientes con su propio terminal
  → Comunicación directa entre teammates (no solo al Lead)


VOCABULARIO

  La comunidad lo llamó "Swarm" (claude-flow, oh-my-claude).
  Cuando Anthropic lo lanzó nativo en 2026 → "Agent Teams".
  En literatura formal: "collaborative" o "swarm architecture".


PROGRESIÓN DE DELEGACIÓN

  1. Solo session — tú con Claude Code
  2. Skills — encapsulas tareas reutilizables
  3. Subagentes — delegas tareas con contexto aislado
  4. Agent Teams — múltiples sesiones colaborando

  Cada paso = más capacidad paralela
              + MENOS control
              + MÁS coste de tokens


CUÁNDO APORTA

  ✓ Features muy grandes divisibles en tracks paralelos
    (backend + frontend + infra)
  ✓ QA swarms (varios teammates probando desde perspectivas distintas)
  ✓ Hipótesis competitivas en debugging


CUÁNDO NO ES NECESARIO

  ✗ El 95% de tareas del día a día
    → subagentes bastan
  ✗ Tareas donde el control humano importa
    → mejor sesión con subagentes
  ✗ Tu primera semana con Claude Code
    → mucho recorrido en subagentes antes


┌──────────────────────────────────────────────────────────┐
│  EL DATO DURO                                            │
│                                                          │
│  Whitepaper de Anthropic sobre arquitecturas agentic:    │
│                                                          │
│  Sistemas multi-agente consumen                          │
│  ~10-15x MÁS TOKENS que un agente solo.                  │
│                                                          │
│  No es detalle menor.                                    │
│                                                          │
│  Diferencia entre:                                       │
│    "voy a montar 3 subagentes"                           │
│    "voy a montar Agent Teams porque mola"                │
│                                                          │
│  Si la tarea no justifica el orden de magnitud,          │
│  NO COMPENSA.                                            │
│                                                          │
└──────────────────────────────────────────────────────────┘


RECOMENDACIÓN HONESTA

  Para este curso:
    → basta con saber que existe y entender cuándo plantearlo
    → no es algo que tu equipo vaya a poner en producción
      la semana que viene
    → pero sí es donde va la herramienta a medio plazo

  Empieza con uno o dos subagentes.
  Mide tu factura con /usage.
  Escala solo cuando los números cuadren.
```

> "**Esa cifra de la caja es la más importante de la demo**. **10 a 15 veces más tokens**. No es un detalle de letra pequeña — es el factor que decide si Agent Teams compensa o no.
>
> La progresión está clara: solo session → skills → subagentes → Agent Teams. **Cada paso te da más capacidad paralela a cambio de menos control y más coste**. La gamma 3.2b slide 26 lo dijo: *'la pregunta no es ¿cuánto puedo delegar?, es ¿cuánto debo delegar para esta tarea?'*.
>
> Y la recomendación honesta: **el 95% de las tareas del día a día se cubren con subagentes**. Implementar endpoint, escribir tests, refactorizar módulo. Para eso lo que vimos en 3.1 y 3.2a basta. Agent Teams es para casos extremos.
>
> Y muy importante: **mide con `/usage`**. La gamma 3.2b slide 30 lo dijo: *'empieza con uno o dos subagentes, mide tu factura con `/usage`, escala solo cuando los números cuadren'*. **No escales a ciegas**.
>
> Estado actual: experimental. **Saber que existe, no usarlo en producción esta semana**."

**Tiempo:** ~2 minutos 30 segundos.

---

### Bloque 10 — Árbol de decisión: cuándo usar qué (~2 min)

> "Para cerrar la orquestación entera, **el árbol de decisión** que la gamma 3.2b slide 32 dejó. Es el resumen operativo de los dos submódulos del 3.2."

**En el editor:**

```
ÁRBOL DE DECISIÓN — cuándo usar qué

| Situación                                          | Solución |
|----------------------------------------------------|----------|
| Tarea simple en flujo actual                       | Agente principal sin más |
| Tarea reutilizable, instrucciones fijas            | Skill |
| Tarea reutilizable + aislamiento contexto          | Skill con context: fork |
| Tarea con criterio propio o exploración pesada     | Subagente |
| Workflow estandarizado de varias subtareas         | Skill orquestador (initiator) → subagentes |
| Workflow con validación que puede fallar           | Skill orquestador + loop validator → implementer |
| Workflow donde varios subagentes comparten info    | + context bank en .claude/workflow-state/ |
| Subtareas independientes que pueden ir a la vez    | Fan-out / fan-in |
| Integración con sistemas externos                  | MCP server |
| Exposición de Claude Code a terceros               | Claude Code como MCP server |
| Tarea muy grande con tracks paralelos genuinos     | Agent Teams (si el coste compensa) |


REGLA DE ORO MEMORIZABLE

  Empieza simple.
  La mayoría de necesidades se cubren con
  SKILLS + SUBAGENTES.
  
  El resto son casos especiales que
  se justifican explícitamente.
```

> "Once filas, una decisión cada una. **De arriba abajo**: complejidad creciente y coste creciente.
>
> **La regla de oro al final**: empieza simple. **Skills + subagentes cubren el 80%** de necesidades de un equipo .NET / Angular medio. El resto son casos especiales que tenéis que **justificar explícitamente** — *'¿por qué context bank y no prompts?'*, *'¿por qué Agent Teams y no tres subagentes?'*. Si no hay respuesta concreta, **no compliquéis**."

**Tiempo:** ~2 minutos.

---

### Bloque 11 — Commit, recap del módulo 3.2 y cliffhanger a 3.3 (~2 min)

**En VS Code, abro `docs/subagentes-explorados.md` y añado la nota de cierre del módulo 3.2.**

**Salvo. En la terminal:**

```powershell
git add .claude/skills/ .claude/agents/ docs/subagentes-explorados.md
git status
```

```
Changes to be committed:
  modified:   .claude/skills/pre-commit-check/SKILL.md
  new file:   .claude/skills/pre-pr-check/SKILL.md
  new file:   .claude/agents/convention-checker.md
  modified:   docs/subagentes-explorados.md
  (más los 6 del endpoint)
```

```powershell
git commit -m "demo/3.2b-after: context bank, fan-out paralelo, endpoint search"
```

> "Commit. **El módulo 3.2 cerrado**.
>
> Recap de lo que el alumno se lleva del 3.2 entero:"

**En el editor:**

```
LO QUE TIENES TRAS EL MÓDULO 3.2

3.2a — Aislamiento, composición y loops
  ✓ context: fork en skills
  ✓ Patrón skill que invoca subagente
  ✓ Loops validator → implementer con techo

3.2b — Memoria, paralelo y Agent Teams (esta)
  ✓ Context bank: artefactos durables compartidos
  ✓ Paralelo vs serial: regla matriz + fan-out / fan-in
  ✓ Claude Code como MCP server (referencia)
  ✓ Agent Teams: experimental + 10-15x coste


REPO ACTUAL:

  6 skills:
    angular-component (con context: fork)
    commit-style
    db-reset
    frontend-design (oficial)
    pre-commit-check (con context bank)
    pre-pr-check (con fan-out / fan-in)

  3 subagentes:
    repo-explorer
    dotnet-reviewer
    convention-checker
```

> "Seis skills, tres subagentes, dos orquestadores compuestos, context bank operativo. **Esto es un harness real**. Ya no piezas sueltas — **un sistema que conoce a tu equipo y trabaja contigo**.
>
> **Pero falta una pieza**.
>
> En la siguiente demo, **3.3a**, entramos en lo último del módulo 3: **hooks**. La gamma 3.3a va a cubrir:
>
> Hasta ahora todo lo que vimos pasa **cuando tú lo pides**. *'/pre-commit-check'*. *'/pre-pr-check'*. *'Usa el subagente repo-explorer'*. **Iniciativa tuya**.
>
> **Hooks son la pieza determinista** del harness. Hacen que ciertas cosas pasen **automáticamente, sin que tengas que pedirlo cada vez**. *'Después de cada commit, lanza el reviewer'*. *'Antes de cada PR, ejecuta el checklist'*. *'Cuando un build falla, recibe notificación'*.
>
> **Eso convierte el harness de 'herramienta que invocas' a 'sistema que trabaja contigo en background'**. Es lo que cierra el módulo 3.
>
> Empezamos con el **tres punto tres punto A**."

**Tiempo:** ~2 minutos.

---

## 9. Qué resaltar verbalmente

Cinco puntos que **no pueden quedarse sin decir**:

1. **"El context bank es memoria compartida en `.claude/workflow-state/`. Vive solo durante el workflow. NO es CLAUDE.md."** — la pieza que más confunde. Bloque 2.

2. **"Si la salida de A condiciona cómo B trabaja → serial. Si son ortogonales → paralelo."** — la regla matriz. Bloque 4.

3. **"Fan-out / fan-in escala bien hasta 4-5 subagentes. Más allá, coordinación se come el ahorro."** — el límite práctico. Bloque 4.

4. **"10-15x más tokens. Whitepaper de Anthropic. Si la tarea no justifica el orden de magnitud, no compensa."** — la cifra dura sobre Agent Teams. Bloque 9.

5. **"Empieza simple. Skills + subagentes cubren el 80%. El resto son casos especiales justificados."** — la regla de oro contra sobreingeniería. Bloque 10.

**Frase de remate al final:**

> *"Context bank, paralelo cuando se puede, escalar solo cuando los números cuadren. Y siempre con el árbol de decisión a mano."*

---

## 10. Slide de entrada (locución HeyGen)

> **Texto del avatar antes del screencast:**

"Y vamos a la siguiente demo. La 3.2b. La que cierra la orquestación. Cuatro temas en directo. Uno, **context bank** — memoria compartida entre subagentes en `.claude/workflow-state/`. Ampliamos el `pre-commit-check` que ya teníamos para que escriba a artefactos durables, no solo prompts. Trazabilidad real. Dos, **paralelo vs serial** con la regla matriz: si A condiciona B, serial. Si son ortogonales, paralelo. Construimos un nuevo skill `pre-pr-check` con fan-out a tres subagentes en paralelo — `dotnet-reviewer` + `repo-explorer` para impacto + un nuevo `convention-checker` para estructura. Veréis los tres ejecutar a la vez sobre un endpoint multi-fichero, y el ahorro de tiempo medido. Tres, **Claude Code como MCP server** — referencia conceptual rápida. Cuatro, **Agent Teams** — el patrón experimental con la cifra dura: diez a quince veces más tokens según el whitepaper de Anthropic. Vamos al screencast."

---

## 11. Slide de salida (locución HeyGen)

> **Texto del avatar al volver del screencast:**

"Lo que acabáis de ver cierra la orquestación dentro de una sesión. Cinco ideas para llevarse al lunes. Una, el context bank es memoria compartida en `.claude/workflow-state/` — trazabilidad, recuperación, loops baratos, auditoría. Y NO es `CLAUDE.md` — vive solo durante el workflow. Dos, regla matriz para paralelo vs serial: salida de A condiciona B → serial. Tareas ortogonales → paralelo. Tres, fan-out / fan-in escala bien hasta 4-5 subagentes — más allá la coordinación se come el ahorro. Cuatro, Agent Teams cuesta 10 a 15 veces más tokens. Si la tarea no justifica el orden de magnitud, no compensa. Mide con `/usage` antes de escalar. Cinco, la regla de oro: empieza simple. Skills + subagentes cubren el 80% de las necesidades. El resto son casos especiales que tenéis que justificar explícitamente. En la siguiente demo, la 3.3a, cerramos el módulo 3 entero con la última pieza: hooks. La capa determinista del harness — lo que hace que ciertas cosas pasen automáticamente sin que tengáis que pedirlo cada vez. Después de cada commit, lanza el reviewer. Antes de cada PR, ejecuta el checklist. Eso convierte el harness de 'herramienta que invocas' a 'sistema que trabaja contigo en background'. Empezamos con el tres punto tres punto A."

---

## 12. Tiempo total estimado

| Bloque | Tiempo |
|---|---|
| Slide de entrada (avatar) | ~30 seg |
| Bloque 1 — Setup y planteamiento del cierre 3.2 | ~1 min 30 seg |
| Bloque 2 — Context bank: el problema y la solución | ~3 min |
| Bloque 3 — Ampliar `pre-commit-check` con context bank | ~3 min |
| Bloque 4 — Paralelo vs serial: regla matriz | ~2 min 30 seg |
| Bloque 5 — Crear el subagente `convention-checker` | ~3 min |
| Bloque 6 — Construir `pre-pr-check` con fan-out / fan-in | ~4 min |
| Bloque 7 — Probar fan-out / fan-in en directo | ~4 min |
| Bloque 8 — Claude Code como MCP server (referencia) | ~2 min |
| Bloque 9 — Agent Teams: experimental + 10-15x | ~2 min 30 seg |
| Bloque 10 — Árbol de decisión: cuándo usar qué | ~2 min |
| Bloque 11 — Commit, recap del 3.2 y cliffhanger a 3.3 | ~2 min |
| Slide de salida (avatar) | ~30 seg |
| **Total screencast** | **~28-30 min** |
| **Total con avatar** | **~29-31 min** |

> Si hay preguntas durante el screencast, súmale 4-5 minutos. La demo encaja en un bloque de **35 minutos** dentro de la sesión.

**Margen de seguridad por si algo va lento:**

- **Si el modelo no paraleliza realmente** y ejecuta los tres subagentes en serie aunque el SKILL.md le pida paralelo, comenta: *"a veces el modelo decide ir en serie aunque le pidamos paralelo. Lo importante conceptual está claro: la idea es fan-out / fan-in cuando las tareas son ortogonales. En la práctica el harness cumple con el patrón cuando el modelo está bien afinado, pero la decisión final sigue siendo suya. La pedagogía no se cae"*. Y procede mostrando los tres reportes en el bank.

- **Si los tiempos paralelos no se ven claramente** porque tu versión no muestra timestamps de cada subagente, calcula manualmente diciendo: *"el reviewer tardó X, el explorer Y, el convention Z. Total real ≈ máximo de los tres. Equivalente serial = suma. Ahorro = ahí está la rentabilidad"*.

- **Si el bank no se crea correctamente** (problema de permisos en Windows con `.claude/workflow-state/`), prueba sin el `mkdir -p` y crea la carpeta a mano antes con: `New-Item -ItemType Directory -Path .claude\workflow-state\test-session`. Comenta: *"a veces Windows tiene problemas con bash + paths. Usamos el equivalente PowerShell"*.

- **Si los subagentes no escriben al fichero correcto** (escriben a otro path), no peles la pedagogía. Comenta: *"los subagentes escriben a paths con sus propias decisiones. Lo importante es que la información persiste y el orquestador la encuentra. Vamos a verificar dónde acabaron escribiendo"*. Y muestras el resultado real.

- **Si te quedas sin tiempo y los bloques 8 y 9 te aprietan**, recorta el bloque 8 (MCP server) a 1 minuto: solo enuncias *"existe el modo, es para integrar con sistemas externos serios, no para uso personal"*. El bloque 9 puedes recortarlo a 1 min 30 seg manteniendo solo la cifra 10-15x.

- **Si el árbol de decisión del bloque 10 se hace pesado** porque son 11 filas, lee solo las 5-6 más críticas y di: *"el resto está documentado en el manual y en el `subagentes-explorados.md`"*.

- **Si surge la pregunta sobre cuándo usar `pre-commit-check` vs `pre-pr-check`**, responde corto: *"`pre-commit-check` es por commit, con loop de fixes automáticos. `pre-pr-check` es exhaustivo pre-PR con tres validadores en paralelo y veredicto unificado. El segundo es más caro, así que solo antes del PR final"*.

---

# Apéndice A — Decisiones pedagógicas justificadas

> Esta sección no se graba. Es para Pedro, formador.

**¿Por qué ampliar `pre-commit-check` con context bank en lugar de hacer un nuevo skill?**

Porque **mantenerlo en el mismo skill demuestra la evolución natural** — *"empezaste con prompts, lo amplías a context bank cuando crece"*. Hacer un skill nuevo perdería la conexión con la 3.2a y obligaría al alumno a aprender dos workflows paralelos. **Una pieza, dos versiones**. Conceptualmente más limpio.

**¿Por qué crear `convention-checker` y no usar uno de los existentes?**

Porque **el fan-out necesita perspectivas verdaderamente independientes**. Si paralelizara `dotnet-reviewer` con sí mismo (con dos prompts distintos), las salidas se solaparían — no es paralelización real. **Tres subagentes con ámbitos distintos** (código, impacto, estructura) es paralelización legítima donde cada uno aporta valor único.

**¿Por qué el `convention-checker` es Haiku y no Sonnet?**

Porque su trabajo es **mecánico**: comparar paths con convenciones, verificar nombres contra patrones. **No requiere razonamiento profundo**. La gamma 3.1b slide 30 lo dijo explícitamente como anti-patrón: *"subagente de exploración corriendo en Opus = caro y sin necesidad"*. Aplicado aquí: convention-checker en Sonnet sería desperdicio.

**¿Por qué el endpoint multi-fichero (6 ficheros tocados)?**

Porque **da material genuino para los tres subagentes del fan-out**:
- `dotnet-reviewer` mira el código del handler, controller, repository
- `repo-explorer` analiza el impacto: qué consume `IOrderRepository`, qué tests podría afectar
- `convention-checker` verifica naming MediatR, estructura de carpetas, ubicación del componente Angular

**Cada uno tiene algo específico que decir**. Si el cambio fuera de un solo fichero trivial, los tres dirían cosas similares y el paralelo perdería sentido pedagógico.

**¿Por qué probar el fan-out con un PR sin críticos?**

Por dos razones:
1. **El foco pedagógico es la paralelización, no la detección**. Si introdujera un anti-patrón (como en 3.1b o 3.2a), la atención se desviaría hacia *"qué cazó"*. Aquí la pieza estrella es **cómo se ejecutan tres a la vez**.
2. **El veredicto con dos importantes y una sugerencia** es realista para un PR normal de implementación correcta. **Mejor pedagogía**: la mayoría de PRs reales no tienen críticos, tienen mejoras.

**¿Por qué Claude Code as MCP server es solo referencia, no demo en directo?**

Por dos razones:
1. **La activación varía mucho entre versiones** — qué `settings.json`, qué flags, qué puerto. Si grabo una configuración específica que no funciona en tu versión, la demo se cae.
2. **No es contenido del 80%** — la mayoría de devs no lo van a usar. Profundizar es desproporcionado vs los temas más críticos (context bank, paralelo, Agent Teams).

**¿Por qué la cifra "10-15x" se enmarca con caja visual?**

Porque **es el dato que más cambia decisiones** del módulo 3 entero. La caja visual lo destaca. Cuando el alumno se vaya pensando en montar Agent Teams, esta cifra debe **estar en su cabeza**. Sin ella, el riesgo de sobreingeniería con multi-agente es alto.

**¿Por qué el árbol de decisión es solo lectura, no se desarrolla cada fila?**

Porque la gamma 3.2b slide 32 lo desarrolló. **Repetirlo en directo sería redundante**. Como **referencia visual rápida** sirve de ancla para que el alumno pueda volver al fichero y consultar la decisión. La densidad apropiada para el cierre.

**¿Por qué el cliffhanger a 3.3 menciona "herramienta que invocas vs sistema que trabaja contigo en background"?**

Porque **resume lo que cambia conceptualmente con hooks**. Hasta aquí todo es iniciativa del usuario — `/pre-commit-check`, `/pre-pr-check`, *"usa el subagente"*. Hooks rompen eso: ciertas cosas pasan **sin que las pidas**. **El frame mental** que el alumno necesita para entrar en 3.3 con la mochila preparada.

**¿Por qué el contador de tiempo (12s + 8s + 18s) está incluido si puede no aparecer en versiones reales?**

Porque **la pedagogía está en el ahorro proporcional, no en los segundos exactos**. Si los timestamps no aparecen, el guion incluye plan B en margen de seguridad: calcular manualmente diciendo *"el reviewer tardó X, el explorer Y, el convention Z. Total real ≈ máximo. Equivalente serial = suma. Ahí está la rentabilidad"*. **El concepto sobrevive a la falta de telemetría exacta**.
