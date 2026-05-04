# Demo 1.1 — El ciclo agentic en acción sobre OrderManagement

> **Versión:** v3 | **Módulo:** 1 | **Sub:** 1.1 | **Estado:** ✅ Versión final
> **Archivo:** `demo_M01-S1.1-ciclo-agentic-en-accion-v3.md`
> **Branch destino:** `demo/1.1`
> **Branch de partida:** `main`
> **Tiempo total estimado:** ~18-22 minutos
> **Tipo:** Demo conceptual — el formador arranca Claude Code y comenta lo que hace, **sin enseñar todavía cómo se instala ni cómo se configura**.
> **Plataforma:** Windows (PowerShell 7 + Git Bash disponibles).

---

## 1. Contexto

En la gamma 1.1 hemos cubierto el modelo conceptual completo de Claude Code. **30 minutos de teoría puros**: el malentendido del primer día, la diferencia entre asistente y agente, las tres consecuencias de la iniciativa delegada, las seis herramientas internas (Read, Write, Edit, Bash, Glob, Grep), las tres trazas reales que aparecen en los slides 17-19, las cuatro fases del ciclo agentic en los slides 21-25, la comparativa con Copilot y Cursor, y los modelos y planes.

Lo que falta es **aterrizarlo**. Que el alumno **vea con sus ojos** lo que hasta ahora solo le hemos contado. La traza del slide 19 — *"crea un endpoint para cancelar un pedido"*, los once pasos del agente — es teoría hasta que la ve ocurrir delante. Cuando la ve, hace clic.

Esta demo está deliberadamente **diseñada para que el alumno no se pierda en mecánica**. No vamos a explicar cómo se instala Claude Code (eso es 1.2a), ni cómo se configura el `CLAUDE.md` (eso es 1.2b), ni los modos de uso (eso es 1.3a), ni los slash commands (eso también es 1.3a), ni los permisos (1.3b). Vamos a **arrancar la herramienta y centrar toda la atención del alumno en el ciclo agentic**: cómo el agente lee, razona, actúa y verifica.

Es la demo más importante del módulo desde el punto de vista pedagógico. Es la que decide si el alumno va a entender el resto del curso o si va a estar dos semanas pensando *"esto es Copilot pero más lento"*.

> **Tipo de demo:** prueba de vida pura. El alumno ve, no toca.
> Las demos en vivo del alumno empiezan en 1.2a.

---

## 2. Objetivo de la demo

Tres cosas concretas que tienen que quedar en la cabeza del alumno cuando termine los ~15 minutos de screencast:

1. **El agente lee el repo por su cuenta antes de hacer nada.** Las cuatro fases del ciclo agentic (lectura → razonamiento → acción → verificación) son visibles en pantalla. No es marketing — es lo que pasa.

2. **La diferencia entre asistente y agente es operativa, no estética.** Una tarea como *"añade un endpoint para cancelar un pedido"* en Copilot y en Claude Code se resuelve en mundos distintos. Tras la demo, el alumno tiene que poder explicárselo a un colega del equipo.

3. **El alumno se ve a sí mismo lanzando esto el lunes.** No es magia, no es ciencia ficción, no requiere infraestructura. Es una herramienta de terminal que en cinco minutos podría estar leyendo su repo.

Y dos cosas que tienen que **NO quedar** en su cabeza:

- *"Esto sustituye al dev"* → no, sigue necesitando criterio humano para validar el resultado.
- *"Pues ya sé usar Claude Code"* → no, esto es solo el ciclo. El curso entero es construir el harness encima.

---

## 3. Branch de partida

```
main
```

> Estado inicial del repositorio. El proyecto OrderManagement existe, compila, y ejecuta. **No hay nada de Claude Code todavía.** No hay `CLAUDE.md`, no hay `.claude/`, no hay configuración. El proyecto está en estado virgen.

---

## 4. Branch destino

```
demo/1.1
```

> Tras la demo, la rama `demo/1.1` queda con dos cambios respecto a `main`: un `README.md` actualizado describiendo el proyecto y un `docs/DEMOS.md` con el roadmap de las 28 secciones (M0–M5) del curso. **Los cambios que el formador hace en vivo durante el screencast (el endpoint nuevo, los tests, etc.) NO se commitean** — se descartan al final. La rama queda lista para que cualquier alumno haga `git checkout demo/1.1` y vea el mismo punto de partida.

---

## 5. Estado del repo al empezar

```
ordermanagement/
├── src/
│   ├── OrderManagement.Api/
│   │   ├── Controllers/
│   │   │   └── OrdersController.cs       (5 endpoints REST CRUD)
│   │   ├── Program.cs
│   │   └── OrderManagement.Api.csproj    (.NET 10, ASP.NET Core)
│   ├── OrderManagement.Application/
│   │   ├── Handlers/
│   │   │   ├── CreateOrderHandler.cs     (con MediatR)
│   │   │   ├── UpdateOrderHandler.cs
│   │   │   └── CancelOrderHandler.cs
│   │   ├── Validators/
│   │   │   └── CreateOrderValidator.cs   (con FluentValidation)
│   │   ├── Exceptions/
│   │   │   ├── CustomerNotFoundException.cs
│   │   │   └── OrderNotFoundException.cs
│   │   └── OrderManagement.Application.csproj
│   ├── OrderManagement.Domain/
│   │   ├── Entities/
│   │   │   ├── Order.cs                  (con OrderStatus enum)
│   │   │   ├── OrderItem.cs
│   │   │   └── Customer.cs
│   │   └── OrderManagement.Domain.csproj
│   └── OrderManagement.Infrastructure/
│       ├── Repositories/
│       │   ├── IOrderRepository.cs
│       │   ├── OrderRepository.cs        (EF Core In-Memory)
│       │   ├── ICustomerRepository.cs
│       │   └── CustomerRepository.cs
│       ├── Services/
│       │   ├── IEmailService.cs
│       │   ├── EmailService.cs           (mock que solo loguea)
│       │   ├── IPaymentService.cs
│       │   └── PaymentService.cs         (mock)
│       └── OrderManagement.Infrastructure.csproj
├── frontend/
│   ├── src/
│   │   ├── app/
│   │   │   ├── orders/
│   │   │   │   ├── orders-list.component.ts
│   │   │   │   └── order-detail.component.ts
│   │   │   ├── app.routes.ts
│   │   │   └── app.config.ts
│   │   └── styles/
│   │       └── _tokens.scss
│   ├── package.json                      (Angular 19)
│   └── angular.json
├── tests/
│   └── OrderManagement.Tests/            (carpeta vacía, sin tests todavía)
│       └── OrderManagement.Tests.csproj
├── .gitignore
└── README.md                             (mínimo, sin info detallada)
```

**Estado funcional al empezar:**

- API REST funcional con cinco endpoints: `GET /api/orders`, `GET /api/orders/{id}`, `POST /api/orders`, `PUT /api/orders/{id}`, `DELETE /api/orders/{id}`. La acción de cancelación está implementada en el handler (`CancelOrderHandler`) pero **no expuesta como endpoint independiente**. Eso lo aprovechamos en la demo.
- Handlers MediatR con la lógica de negocio: crear, actualizar y cancelar pedidos.
- Validador `CreateOrderValidator` con tres reglas FluentValidation: `CustomerId > 0`, items no vacíos, total no negativo.
- Repositorios con EF Core In-Memory configurado en `Program.cs`. Sin base de datos real, todo en memoria.
- Servicios mock para email y pagos: implementan la interfaz, logean a consola, no hacen nada real.
- Frontend Angular 19 con dos componentes standalone básicos: `OrdersListComponent` y `OrderDetailComponent`.
- **Sin tests**. La carpeta `tests/OrderManagement.Tests/` existe pero está vacía. Solo el `.csproj`.
- **Sin `.claude/`**. El proyecto no conoce Claude Code todavía.
- **Sin `CLAUDE.md`**. No hay contexto persistente.
- **Sin documentación XML** en métodos públicos del controller.

**Comandos para que el formador verifique antes de la demo (PowerShell):**

```powershell
# En PowerShell, desde la raíz del repo
cd C:\Users\pedro\projects\ordermanagement
git checkout main
git pull

# Verificar build
dotnet build
# Esperado: 0 warnings, 0 errors

# Verificar frontend (en una terminal aparte)
cd frontend
npm install
npm run build
# Esperado: build limpio
```

**Lo que el alumno verá al hacer `git checkout demo/1.1` antes de empezar:**

- Misma estructura del árbol de arriba.
- README.md con descripción decente del proyecto y roadmap del curso.
- `docs/DEMOS.md` con el registro de las 28 secciones (M0–M5).
- Compila con `dotnet build` y arranca con `dotnet run --project src/OrderManagement.Api`.

---

## 6. Prompt para Claude Code

> Lo que tú, formador, copias y pegas en Claude Code para preparar la rama `demo/1.1` antes de grabar.
>
> El proyecto base ya tiene que existir en `main`. Este prompt no crea el proyecto — asume que ya está construido. Si no existe aún, antes de la demo 1.1 hay que crearlo en `main` (eso es trabajo previo).

````
Estoy preparando la primera demo del curso de Claude Code para devs .NET + 
Angular. La demo se titula "El ciclo agentic en acción sobre OrderManagement".
Trabajo en Windows.

Esta demo es la PRIMERA del curso entero. Es deliberadamente CONCEPTUAL: 
el alumno ya ha visto los 30 minutos de teoría del 1.1 (paradigma agentic, 
asistente vs agente, ciclo de 4 fases, las 3 trazas del manual, etc.) y 
en esta demo VE arrancar Claude Code en pantalla y comprueba con sus ojos 
las 4 fases del ciclo agentic sobre OrderManagement.

# Contexto del proyecto

Estoy en la rama `main` del repo `ordermanagement`. El repo ya contiene un 
proyecto .NET 10 + Angular 19 funcional con esta estructura:

- src/OrderManagement.Api/                    (ASP.NET Core)
  └── Controllers/OrdersController.cs         (5 endpoints REST)
- src/OrderManagement.Application/            (handlers MediatR + validators)
  ├── Handlers/CreateOrderHandler.cs
  ├── Handlers/UpdateOrderHandler.cs
  ├── Handlers/CancelOrderHandler.cs
  ├── Validators/CreateOrderValidator.cs
  └── Exceptions/CustomerNotFoundException.cs
- src/OrderManagement.Domain/                 (entidades Order, OrderItem, Customer)
- src/OrderManagement.Infrastructure/         (repos EF Core In-Memory + mocks)
- frontend/                                   (Angular 19, componentes standalone)
- tests/OrderManagement.Tests/                (carpeta vacía, sin tests)

El proyecto compila y arranca. NO tiene CLAUDE.md ni .claude/settings.json — 
es deliberado. La configuración llega en la demo 1.2b.

# Lo que necesito

Hay 5 tareas concretas:

## Tarea 1: crear la rama demo/1.1

```powershell
git checkout main
git pull
git checkout -b demo/1.1
```

## Tarea 2: actualizar README.md

Sustituye el README.md actual por uno con esta estructura exacta:

### Sección "OrderManagement"

Una frase clara: "Sistema de gestión de pedidos. Proyecto demo del curso 
Claude Code para devs .NET + Angular."

### Sección "Stack"

Lista en bullets:

- .NET 10
- ASP.NET Core (API REST)
- MediatR (CQRS)
- FluentValidation
- EF Core In-Memory (sin DB real, todo en memoria)
- Angular 19 con standalone components y Signals
- xUnit + NSubstitute + FluentAssertions (preparados, sin tests todavía)

### Sección "Estructura"

Árbol con una frase por carpeta principal:

```
src/
├── OrderManagement.Api/             API REST con OrdersController y 5 endpoints
├── OrderManagement.Application/     Handlers MediatR + validators FluentValidation
├── OrderManagement.Domain/          Entidades de negocio: Order, OrderItem, Customer
└── OrderManagement.Infrastructure/  Repositorios EF Core + servicios mock
frontend/                            Angular 19 con componentes standalone
tests/                               Carpeta preparada para tests, vacía por ahora
```

### Sección "Cómo ejecutar"

Tres bloques de código en PowerShell (Windows):

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

### Sección "Estado actual"

Texto literal:

"API REST funcional con CRUD de pedidos. Frontend Angular con listado 
y detalle. Tres puntos a mejorar a lo largo del curso:

1. **Sin tests todavía**. La carpeta tests/ está preparada pero vacía. 
   La cobertura es cero.
2. **Sin documentación XML** en los métodos públicos. El Swagger se 
   genera pero los endpoints están sin describir.
3. **Sin configuración de Claude Code todavía**. No hay CLAUDE.md, 
   no hay .claude/. El proyecto está en estado virgen para que se vea 
   el contraste cuando empecemos a configurarlo."

### Sección "Curso"

Una frase: "Cada gamma del curso tiene una demo asociada en una rama dedicada 
(`demo/X.Y`). Ver `docs/DEMOS.md` para el registro completo."

## Tarea 3: crear docs/DEMOS.md

Si la carpeta `docs/` no existe, créala.

Contenido del fichero `docs/DEMOS.md`:

```markdown
# Registro de demos del curso

Cada gamma del curso tiene una demo asociada que avanza el proyecto
OrderManagement de alguna forma concreta. La mayoría de demos siguen el
patrón **before/after** (ver `docs/4-Demos/demo_M00-S0.2-...`): cada
sección no conceptual tiene dos ramas hermanas — `demo/X.Y-before`
(estado de partida del screencast) y `demo/X.Y-after` (estado final que
la siguiente clase asume). Las demos puramente conceptuales mantienen
rama única `demo/X.Y` (los cambios del screencast se descartan al final).

## Módulo 0 — Setup

- [x] **demo/0.1** — Setup del proyecto OrderManagement (rama única, sin screencast)

## Módulo 1 — Claude Code básico

- [x] **demo/1.1** — Hello Claude Code: el ciclo agentic en acción (CONCEPTUAL, rama única)
- [ ] demo/1.2a-before / demo/1.2a-after — Instalación, autenticación y primer arranque
- [ ] demo/1.2b-before / demo/1.2b-after — CLAUDE.md y settings.json para .NET 10 + Angular 19
- [ ] demo/1.3a-before / demo/1.3a-after — Tres modos de uso, slash commands, /compact
- [ ] demo/1.3b-before / demo/1.3b-after — Workflow completo con permisos sanos

## Módulo 2 — Skills

- [ ] demo/2.1a — Primer skill leído por dentro (CONCEPTUAL, rama única)
- [ ] demo/2.1b — Skill propio diseccionado (CONCEPTUAL, rama única)
- [ ] demo/2.2a-before / demo/2.2a-after — Primer skill creado: angular-component-generator
- [ ] demo/2.2b-before / demo/2.2b-after — Skill con scripts y plantillas
- [ ] demo/2.2c-before / demo/2.2c-after — Skill con scopes user vs proyecto
- [ ] demo/2.3 — Skill desplegado al equipo (CONCEPTUAL, rama única)

## Módulo 3 — Agent harness

- [ ] demo/3.1a — Subagentes integrados en acción (CONCEPTUAL, rama única)
- [ ] demo/3.1b-before / demo/3.1b-after — Subagente custom: dotnet-reviewer
- [ ] demo/3.2a-before / demo/3.2a-after — Orquestación: aislamiento, composición, loops
- [ ] demo/3.2b-before / demo/3.2b-after — Memoria, paralelo, agent teams
- [ ] demo/3.3a-before / demo/3.3a-after — Primer hook PostToolUse
- [ ] demo/3.3b-before / demo/3.3b-after — Hooks completos

## Módulo 4 — Diseño integrado

- [ ] demo/4.1a-before / demo/4.1a-after — Figma MCP conectado
- [ ] demo/4.1b-before / demo/4.1b-after — Tokens extraídos a _tokens.scss
- [ ] demo/4.2a-before / demo/4.2a-after — Claude Design creando notificaciones
- [ ] demo/4.2b-before / demo/4.2b-after — Onboarding del design system
- [ ] demo/4.3a-before / demo/4.3a-after — DESIGN.md anatomía completa
- [ ] demo/4.3b-before / demo/4.3b-after — CLI design.md en CI

## Módulo 5 — Handoff y testing

- [ ] demo/5.1a-before / demo/5.1a-after — Handoff bundle generado
- [ ] demo/5.1b-before / demo/5.1b-after — Handoff completo importado
- [ ] demo/5.2-before / demo/5.2-after — Flujo combinado en acción
- [ ] demo/5.3a-before / demo/5.3a-after — Tests xUnit autogenerados
- [ ] demo/5.3b-before / demo/5.3b-after — Workflow completo: feature de cancelación
```

Marca M0.1 y la 1.1 como hechas (`[x]`) y deja el resto en `[ ]`. El resto
de entradas están con la nomenclatura `-before` / `-after` salvo las
puramente conceptuales (1.1, 2.1a, 2.1b, 2.3, 3.1a) que mantienen rama única.

## Tarea 4: verificar que compila

Antes de commitear, ejecuta en PowerShell:

```powershell
dotnet restore
dotnet build
```

Esperado: 0 warnings, 0 errors.

Y desde frontend/:

```powershell
cd frontend
npm install
npm run build
```

Esperado: build limpio.

Si algo falla, **para y dime antes de commitear**.

## Tarea 5: commit y resumen final

Si todo compila, haz un único commit:

```powershell
git add README.md docs/DEMOS.md
git commit -m "demo/1.1: README actualizado y registro de demos creado"
```

NO hagas push. Yo lo hago manualmente cuando lo revise.

# Restricciones (importantes)

- NO añadas CLAUDE.md ni .claude/settings.json. Eso es la demo 1.2b.
- NO añadas skills, subagentes ni hooks. Esos son módulos 2 y 3.
- NO añadas tests. Eso es del módulo 5.
- NO toques el código de la app: ni los .csproj, ni Program.cs, ni los 
  handlers, ni los componentes Angular. La demo se hace sobre el código 
  tal cual está.
- NO añadas un `.claude/` aunque sea para meter algo "vacío".
- NO añadas un endpoint nuevo "by-customer" ni "cancel" ni nada. La demo 
  los crea EN VIVO durante el screencast y luego se descartan.

# Cuando termines, dime

1. Que la rama demo/1.1 está creada.
2. Que README.md y docs/DEMOS.md están commiteados.
3. Que tanto `dotnet build` como `npm run build` compilan limpio.
4. Un resumen breve de lo que el alumno verá al hacer `git checkout demo/1.1`.

Si en algún punto tienes dudas (por ejemplo, si ya existe un docs/DEMOS.md 
con otro contenido), para y pregúntame antes de sobrescribir.
````

---

## 7. Artefactos que Claude Code debe generar

```
✓ Rama nueva: demo/1.1
✓ README.md actualizado en raíz (sustituyendo el anterior)
✓ docs/DEMOS.md creado con el registro de las 28 secciones (M0–M5) del curso
✓ Verificación de build OK:
  ├── dotnet build     → 0 warnings, 0 errors
  └── npm run build    → build limpio
✓ Commit único: "demo/1.1: README actualizado y registro de demos creado"
✓ Resumen al formador de lo que el alumno verá al hacer checkout
```

**Lo que NO debe haber generado:**

- ❌ `CLAUDE.md` (eso es la demo 1.2b)
- ❌ `.claude/settings.json` o `.claude/` carpeta vacía
- ❌ Skills, subagentes, hooks (módulos siguientes)
- ❌ Tests nuevos (módulo 5)
- ❌ Cambios en `Program.cs`, controllers, handlers, frontend
- ❌ Cambios en `.csproj` o `package.json`
- ❌ Endpoints nuevos en `OrdersController` (esos se crean en vivo en el screencast y se descartan)

> Si Claude Code se anticipa y crea cualquiera de estos, **se rechaza el output** y se le dice que solo haga lo del prompt. La demo 1.1 tiene que mostrar el repo "en frío" para que el contraste con la 1.2b sea claro.

**Estado final del árbol después del prompt:**

```
ordermanagement/
├── docs/
│   └── DEMOS.md                    ← NUEVO
├── src/                            ← sin cambios
├── frontend/                       ← sin cambios
├── tests/                          ← sin cambios
├── .gitignore                      ← sin cambios
└── README.md                       ← MODIFICADO
```

---

## 8. Guion del screencast

**Duración estimada del screencast: ~15-18 minutos.**

Seis bloques. Cada uno con setup visual, lo que tecleas, lo que dices al alumno palabra por palabra, y lo que tienes que resaltar.

> **Antes de empezar a grabar**, asegúrate de:
> - Tener PowerShell 7 abierto en pantalla completa, fuente al menos 16pt para que se lea en grabación.
> - Tener VS Code abierto al lado con el repo `ordermanagement` cargado en la rama `demo/1.1`.
> - Cerrar Slack, Teams, navegadores con notificaciones.
> - Tener Claude Code ya autenticado (si te pide login, lo ven los alumnos y rompemos el guión — eso es de la demo 1.2a).

---

### Bloque 1 — Setup visible y orientación al alumno (~1 min 30 seg)

**Pantalla compartida.** A la izquierda VS Code abierto en `demo/1.1`, mostrando el árbol de carpetas en el explorador. A la derecha, una terminal PowerShell vacía en la raíz del proyecto.

**Antes de teclear nada**, paseas el cursor por el árbol del proyecto en VS Code. Click en `src/OrderManagement.Api/Controllers/OrdersController.cs`. Lo abres tres segundos. Click en `src/OrderManagement.Application/Handlers/CreateOrderHandler.cs`. Lo abres tres segundos. Click en `src/OrderManagement.Application/Handlers/CancelOrderHandler.cs`. **Importante: este último lo abres y lo dejas un momento más, lo señalas con el cursor.** Cierras los tres.

**Lo que dices, mientras paseas el cursor:**

> "Aquí tenemos OrderManagement. Es el proyecto que vamos a usar como hilo conductor durante todo el curso. Vais a ver este repo crecer durante diez horas. Cada gamma deja una rama nueva en el repositorio. Hoy estamos en la rama `demo/1.1`, la primera.
>
> Os doy treinta segundos de orientación al proyecto antes de empezar. Es .NET 10 con Angular 19, lo típico que se ve en clientes corporativos hoy en día. Tenemos una API con cinco endpoints — un CRUD de pedidos. Tenemos handlers de MediatR con la lógica. Vemos `CreateOrderHandler`, `UpdateOrderHandler`, y...
>
> Atentos a este de aquí: `CancelOrderHandler`. La lógica de cancelación **ya está implementada en el handler**. Pero — y esto importa para lo que vamos a ver — **no hay un endpoint dedicado en el controller para cancelar**. Si abrimos el `OrdersController`, veréis los cinco endpoints típicos: GET, GET por id, POST, PUT, DELETE. Pero no hay un `POST /api/orders/{id}/cancel`. La lógica está, falta exponerla.
>
> Tres cosas más que conviene fijar antes de empezar. Una: no hay tests todavía. La carpeta `tests/` existe pero está vacía. Eso lo abordamos en el módulo 5. Dos: no hay documentación XML en los métodos públicos. Y tres, la más importante para esta demo: **no hay nada de Claude Code configurado**. No hay `CLAUDE.md`, no hay `.claude/settings.json`. El proyecto está virgen.
>
> Y eso es deliberado. Porque lo que vamos a ver es Claude Code 'en frío'. Sin configuración, sin contexto preparado, sin trucos. Solo el agente y el repositorio. La razón: cuando en la demo 1.2b empecemos a meter `CLAUDE.md`, vais a notar el contraste."

**Tiempo:** ~90 segundos.

---

### Bloque 2 — Recordatorio de la teoría (~1 min)

Antes de arrancar Claude Code, **abres en otra ventana o ventana flotante de la grabación las trazas del slide 19** que el alumno ya vio en la gamma 1.1. Las dejas visibles unos segundos.

```
Tarea: "crea un nuevo endpoint para cancelar un pedido"

 1. Glob OrdersController.cs                   → localiza
 2. Read OrdersController.cs                   → ve patrones existentes
 3. Read Order.cs                              → ¿hay método CancelOrder?
 4. Grep "CancelOrder" en /src                 → ¿ya existe lógica?
 5. Read OrderService.cs                       → cómo se gestiona la lógica
 6. Edit Order.cs                              → añade método CancelOrder
 7. Edit OrderService.cs                       → añade orquestación
 8. Edit OrdersController.cs                   → añade endpoint
 9. Write OrdersControllerCancelTests.cs       → tests
10. Bash dotnet build                          → comprueba que compila
11. Bash dotnet test                           → ejecuta tests
[... iteración hasta que todo verde ...]
```

**Lo que dices:**

> "Os recuerdo lo que vimos en la teoría. Esta es la traza que vimos en el slide diecinueve. Once pasos. Once. Cuando vemos esto en una slide queda muy bonito y muy ordenado, pero suena lejano. Suena a marketing.
>
> Lo que voy a hacer ahora es ejecutar exactamente esa tarea — *'crea un endpoint para cancelar un pedido'* — sobre OrderManagement. Y vais a ver Claude Code haciendo esos pasos en pantalla. No exactamente en este orden, no exactamente con estos nombres de método, pero el ciclo es el mismo. Lectura. Razonamiento. Acción. Verificación. Las cuatro fases que vimos en los slides veintiuno al veinticinco.
>
> Una cosa muy importante antes de empezar: **no os fijéis en cómo se instala**. No os fijéis en el comando para arrancar. No os fijéis en los slash commands ni en si tengo permisos configurados. Todo eso es de las siguientes demos. Concentraos en una sola pregunta: ¿está el agente leyendo el repo antes de actuar? Si la respuesta es sí, el modelo conceptual del 1.1 es real."

**Tiempo:** ~60 segundos.

---

### Bloque 3 — Traza 1: explicar un fichero (la lectura pura) (~3 min)

**En la terminal PowerShell, tecleas:**

```powershell
claude
```

> "Es así. `claude`. Si no estuviera autenticado, ahora me abriría el navegador. Eso lo veremos en la demo 1.2a en detalle. Aquí ya estoy logueado, así que arranca directo."

Aparece el prompt de Claude Code:

```
 Welcome to Claude Code v2.x.x

 cwd: C:\Users\pedro\projects\ordermanagement
 model: claude-opus-4.7
 Type / for commands, ? for help

>
```

> "Mirad lo que aparece arriba. El `cwd`, en Windows aparece con la barra invertida — sabe que estamos en `ordermanagement`. El modelo, Opus 4.7. Y el prompt esperando. Listo.
>
> Vamos a empezar por lo más fácil: vamos a pedirle que **explique** un fichero. Solo lectura, sin tocar nada. Es la primera traza del slide diecisiete del manual."

**Tecleas, despacio para que el alumno lea:**

```
> Explícame qué hace OrdersController.cs. No quiero que toques nada,
  solo que lo leas y me cuentes qué endpoints expone, qué dependencias
  tiene, y qué patrones sigue.
```

> "Ojo a una cosa. **No le he abierto el fichero. No le he pasado contenido. No le he dicho dónde está.** Solo el nombre. Vamos a ver qué hace."

**Pulsas Enter. Empiezan a aparecer las herramientas en pantalla.**

```
● Glob(**/OrdersController.cs)
● Read(src/OrderManagement.Api/Controllers/OrdersController.cs)
● Read(src/OrderManagement.Domain/Entities/Order.cs)
● Read(src/OrderManagement.Application/Handlers/CreateOrderHandler.cs)
● Read(src/OrderManagement.Application/Handlers/CancelOrderHandler.cs)
● Grep(IOrderRepository)
● Read(src/OrderManagement.Infrastructure/Repositories/IOrderRepository.cs)
```

**Mientras las herramientas van apareciendo, vas comentando despacio.**

> "Mirad. **Glob**. Es la primera herramienta de las seis que vimos. Busca el fichero por patrón. Ya sabe dónde está sin que yo le haya dicho la ruta.
>
> **Read**. Lee el controller. Lo está construyendo en su contexto.
>
> **Read** otra vez, pero esto es interesante: ahora lee `Order.cs`. ¿Por qué? Porque está leyendo el controller, ve que devuelve cosas tipo `Order`, y se va a ver qué es un Order. **Está siguiendo las dependencias.** No le he pedido que mire la entidad, pero la mira por su cuenta.
>
> Lee `CreateOrderHandler.cs`. Lee `CancelOrderHandler.cs`. ¿Por qué? Porque ve que el controller hace `_sender.Send(...)` con MediatR, así que se va a buscar los handlers que se invocan.
>
> **Grep**. Busca `IOrderRepository` en el repo. Quiere saber dónde está la interfaz del repositorio.
>
> **Read** la interfaz. Lee la firma del repositorio.
>
> Ese paseo que veis durante diez segundos, **eso es la fase 1 del ciclo agentic**. Lectura de contexto. Lo vimos en el slide veintidós. Cómo decide qué leer? Combinando señales: el `csproj`, el nombre del fichero, las clases que importa, las dependencias que va descubriendo. No lee todo el repo. Lee lo necesario para responder."

Cuando termina la lectura, aparece la respuesta:

```
OrdersController.cs es el controlador REST principal del proyecto. Te 
desgloso lo que hace.

## Endpoints

Cinco endpoints estándar de un CRUD REST:

- GET /api/orders                  → lista de pedidos
- GET /api/orders/{id}             → pedido por id (404 si no existe)
- POST /api/orders                 → crea pedido (valida con CreateOrderValidator)
- PUT /api/orders/{id}             → actualiza pedido
- DELETE /api/orders/{id}          → elimina pedido

## Dependencias

Inyecta IMediator (de MediatR) en el constructor. Cada endpoint construye 
un comando o query y lo despacha vía _mediator.Send(). El controller 
está deliberadamente "delgado" — no contiene lógica de negocio.

## Patrones que sigue

- CQRS con MediatR. Cada operación tiene su comando/query y handler.
- Manejo de errores: try-catch en cada endpoint para excepciones específicas 
  (CustomerNotFoundException, OrderNotFoundException) que se traducen a 
  códigos HTTP correspondientes (404, 422).
- Cancellation tokens propagados desde la firma del action method hasta 
  el handler.

## Observación

Veo que existe CancelOrderHandler en src/OrderManagement.Application/Handlers/, 
pero NO veo un endpoint dedicado en el controller para cancelar (POST 
/api/orders/{id}/cancel o similar). La operación de cancelación parece 
hacerse vía PUT /api/orders/{id} cambiando el status, lo cual es un patrón 
discutible. Habría que considerar exponer un endpoint específico de cancel 
para clarificar la intención.
```

**Cuando termina la respuesta, los señalas con el cursor:**

> "Y ahora la respuesta. Mirad esto bien.
>
> No es genérico. Habla del controlador en concreto. Habla de los cinco endpoints reales. Habla de **`CreateOrderValidator`** — ese nombre — porque lo ha leído. Habla de **`IMediator`** y de cómo se despachan los comandos. Habla de **`CustomerNotFoundException`** y **`OrderNotFoundException`** — esos nombres exactos — porque ha visto cómo se manejan los errores en cada endpoint.
>
> Pero lo más interesante es la sección 'Observación' al final. Sin que yo le haya preguntado, **se ha dado cuenta de que existe un `CancelOrderHandler` pero no hay endpoint dedicado de cancel**. Esa observación no la ha sacado del manual ni del entrenamiento. La ha sacado **leyendo dos sitios y comparándolos**. El handler existe en una carpeta. El endpoint correspondiente no existe en otra. Cruzar esa información es razonamiento sobre el codebase, no autocompletado.
>
> Esto es lo que diferencia un asistente de un agente. Un asistente espera tus instrucciones. Un agente investiga por su cuenta y te trae conclusiones."

**Tiempo:** ~3 minutos.

---

### Bloque 4 — Traza 2: el endpoint de cancelar (las 4 fases en vivo) (~7 min)

Esta es la pieza central de la demo. Aquí el alumno ve las cuatro fases del ciclo agentic en orden, en vivo, sobre el código real. **No metas prisa**. Es donde se gana la demo.

> "Lo que voy a hacer ahora es exactamente la traza del slide diecinueve. La que tiene los once pasos. Le voy a pedir que cree el endpoint de cancelación que el agente mismo ha detectado que falta. Vais a ver las cuatro fases. Lectura. Razonamiento. Acción. Verificación.
>
> Y le voy a forzar a que me presente el plan antes de actuar, porque eso hace la fase 2 explícita. Para esta demo me interesa que la veáis."

**Tecleas, despacio:**

```
> Vamos a añadir el endpoint que has detectado que falta: un POST
  /api/orders/{id}/cancel. Tiene que llamar al CancelOrderHandler que
  ya existe. Si el pedido no existe, devuelve 404. Si el pedido ya está
  en un estado que no permite cancelación, devuelve 422 con un mensaje
  claro. Sigue el patrón que has visto en los demás endpoints.

  Antes de tocar nada, dime el plan: qué ficheros vas a tocar y qué
  vas a poner en cada uno. No actúes hasta que yo apruebe.
```

> "Le he añadido al final 'antes de tocar nada, dime el plan'. Esto **fuerza la fase de razonamiento explícito**. Sin esta línea Claude Code podría ir directo a actuar. Forzando el plan vemos en pantalla la fase 2."

**Pulsas Enter. Claude Code arranca con la fase 1 — más lectura.**

```
● Read(src/OrderManagement.Api/Controllers/OrdersController.cs)
● Read(src/OrderManagement.Application/Handlers/CancelOrderHandler.cs)
● Read(src/OrderManagement.Domain/Entities/Order.cs)
● Read(src/OrderManagement.Application/Exceptions/OrderNotFoundException.cs)
● Glob(src/**/CancelOrderCommand.cs)
● Read(src/OrderManagement.Application/Handlers/CancelOrderHandler.cs)
```

> "Veis la fase uno otra vez. Lectura. Pero atentos a lo que hace ahora — está leyendo cosas distintas que antes. Lee el controller para entender el patrón. Lee el handler que va a invocar. Lee la entidad `Order` para saber qué estados son cancelables. Lee la excepción `OrderNotFoundException` para saber qué se lanza cuando no se encuentra. Y busca con Glob si ya existe un comando `CancelOrderCommand`.
>
> Esto no es paseíto. **Está construyendo el contexto que necesita para razonar bien sobre la tarea.** Si yo me hubiera saltado este paso y le hubiera dictado *'crea un método CancelOrder, mete un try, mete un catch'*, no habría hecho nada de esto. Habría escrito código a ciegas."

**Aparece el plan:**

```
Plan para añadir POST /api/orders/{id}/cancel:

He examinado el código y aquí está mi plan, paso a paso.

1. **Verificar el comando existente**:
   He visto que CancelOrderHandler ya implementa
   IRequestHandler<CancelOrderCommand, Unit>. El comando CancelOrderCommand
   ya existe en src/OrderManagement.Application/Commands/CancelOrderCommand.cs
   con el campo OrderId. NO hay que crear comando nuevo.

2. **Añadir endpoint al controller**:
   src/OrderManagement.Api/Controllers/OrdersController.cs
   - Atributo [HttpPost("{id:int}/cancel")]
   - Método: public async Task<IActionResult> Cancel(int id, CancellationToken ct)
   - Llama: await _mediator.Send(new CancelOrderCommand(id), ct)
   - try/catch OrderNotFoundException → return NotFound(...)
   - try/catch InvalidOrderStateException → return UnprocessableEntity(...)
   - Éxito: return NoContent() (204)

3. **Verificar excepción de estado inválido**:
   He visto que en CancelOrderHandler se valida que Order.Status sea Pending
   o Confirmed. Si no, el handler lanza... mira esto, lanza
   InvalidOperationException, no una excepción específica. Dos opciones:
     a) Crear InvalidOrderStateException nueva para semántica clara
        (recomendado, sigue el patrón de CustomerNotFoundException).
     b) Capturar InvalidOperationException directamente en el controller
        (más rápido, peor diseño).

   Recomiendo opción (a). Crearía:
   - src/OrderManagement.Application/Exceptions/InvalidOrderStateException.cs
   - Modificación de CancelOrderHandler para lanzar la excepción nueva
     en vez de InvalidOperationException.

4. **Verificación**:
   - dotnet build (debe compilar sin warnings)
   - El endpoint queda accesible en POST /api/orders/{id}/cancel

¿Procedo con la opción (a) o prefieres (b) o algo distinto?
```

**Cuando aparece el plan, lo lees con voz alta señalando con el cursor:**

> "Aquí está la **fase 2: razonamiento**. Mirad bien. Cuatro pasos.
>
> Paso uno. **Ha verificado que el comando ya existe.** Yo no le dije que existiera. Lo ha encontrado leyendo el handler y siguiendo la dependencia. Eso le ahorra trabajo y a vosotros os ahorra un fichero duplicado.
>
> Paso dos. El endpoint en el controller. Mira el detalle: usa `[HttpPost(\"{id:int}/cancel\")]`. **Ese `:int` no se lo he dicho.** Lo añade porque es el patrón correcto en ASP.NET Core para validar tipo en la ruta. Si alguien llama con `/api/orders/abc/cancel`, ASP.NET devuelve 404 antes de entrar al método. Eso es conocimiento de framework aplicado por iniciativa propia.
>
> Paso tres. Y aquí está lo bueno. **Ha detectado un mini-bug de diseño.** Ha visto que `CancelOrderHandler` lanza `InvalidOperationException` cuando el estado del pedido no permite cancelar. Y se ha dado cuenta de que el resto del proyecto usa excepciones tipadas — `CustomerNotFoundException`, `OrderNotFoundException`. Hay inconsistencia. Me da dos opciones: crear una excepción nueva siguiendo el patrón, o atrapar la genérica. **Y me recomienda la primera con razón.**
>
> Esto es razonamiento sobre código. Esto es, palabra por palabra, lo que un senior haría revisando el repo. Un junior pondría el try-catch de `InvalidOperationException` y a casa. El agente ha visto la inconsistencia y la propone arreglar.
>
> Y mirad el final: **'¿procedo con (a) o (b)?'**. Está esperando mi decisión. La fase 2 termina con aprobación. La fase 3 no empieza hasta que yo digo que sí."

**Tecleas:**

```
> Procede con la opción (a). Crea la excepción tipada y modifica el
  handler. Ejecuta el plan completo.
```

**Claude Code empieza a actuar. Aparecen los Write y los Edit:**

```
● Write(src/OrderManagement.Application/Exceptions/InvalidOrderStateException.cs)
● Edit(src/OrderManagement.Application/Handlers/CancelOrderHandler.cs)
● Edit(src/OrderManagement.Api/Controllers/OrdersController.cs)
● Bash(dotnet build)
```

> "Y ahora arranca la **fase 3: acción**. Crea la excepción tipada con `Write`. Modifica el handler con `Edit` para lanzar la excepción nueva. Modifica el controller con `Edit` para añadir el endpoint y el manejo de errores.
>
> Y aquí viene la **fase 4** que es la que más diferencia a Claude Code de un autocompletado. Atentos al último paso. Hace `dotnet build` por su cuenta. **Yo no se lo he pedido.** Lo lanza solo para verificar que lo que ha escrito compila. Es la fase de verificación."

**Esperamos al output del build. Aparece:**

```
Build succeeded.
    0 Warning(s)
    0 Error(s)

He añadido el endpoint POST /api/orders/{id}/cancel siguiendo el plan
acordado. Resumen.

Nuevos ficheros:
- src/OrderManagement.Application/Exceptions/InvalidOrderStateException.cs

Ficheros modificados:
- src/OrderManagement.Application/Handlers/CancelOrderHandler.cs
- src/OrderManagement.Api/Controllers/OrdersController.cs

El build pasa: 0 warnings, 0 errors.

Cambios concretos.

InvalidOrderStateException nueva: hereda de Exception, recibe el orderId
y el estado actual del pedido en el constructor. Mensaje formateado tipo
"Order {id} is in state {state} and cannot be cancelled".

CancelOrderHandler: línea donde antes lanzaba InvalidOperationException
ahora lanza InvalidOrderStateException con el orderId y el estado actual.

OrdersController: endpoint nuevo POST /api/orders/{id:int}/cancel. Llama
al handler, captura las dos excepciones específicas (OrderNotFoundException
y InvalidOrderStateException), devuelve 404 y 422 respectivamente. Caso
éxito devuelve 204 NoContent que es el código HTTP correcto para una
operación que se ejecuta sin devolver cuerpo.

¿Quieres que probemos el endpoint con curl, o pasamos a otra cosa?
```

> "Verde. Compila. Cero warnings. Cero errores. Y mirad el resumen que me da. No solo me dice 'lo he hecho'. Me dice exactamente **qué** ha tocado en cada fichero, **por qué** lo ha tocado así, y **qué código HTTP** devuelve cada caso con justificación. 204 NoContent porque la operación no devuelve cuerpo. Eso es decisión técnica, no recetario.
>
> Esto, las cuatro fases, es **el ciclo agentic**. Lo que vimos en la teoría desde los slides veintiuno al veinticinco. Lectura. Razonamiento. Acción. Verificación. Lo acabáis de ver en pantalla durar tres minutos. Y esa es la magnitud real del beneficio.
>
> Si yo hubiera tenido que hacer esto a mano: leer el controller, leer el handler, darme cuenta de la inconsistencia con `InvalidOperationException`, decidir crear una excepción tipada nueva, escribir esa clase, modificar el handler, modificar el controller, lanzar el build, comprobar que compila — quince a veinte minutos. Tres minutos del agente. **Y respeta el patrón del proyecto, no inventa nombres ni introduce inconsistencias.**"

**Tiempo:** ~7 minutos.

---

### Bloque 5 — Mirar lo que se ha generado (~2 min)

Vuelves al VS Code (manteniendo Claude Code abierto en el terminal). Abres el `OrdersController.cs` y bajas hasta el final, donde está el método nuevo:

```csharp
[HttpPost("{id:int}/cancel")]
public async Task<IActionResult> Cancel(int id, CancellationToken ct)
{
    try
    {
        await _mediator.Send(new CancelOrderCommand(id), ct);
        return NoContent();
    }
    catch (OrderNotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
    catch (InvalidOrderStateException ex)
    {
        return UnprocessableEntity(new { message = ex.Message });
    }
}
```

> "Aquí está. Mismo patrón que los demás endpoints. Inyecta `_mediator`, despacha el comando con su `CancellationToken`, atrapa las dos excepciones específicas. **No genérica.** Específicas, una para 404 y otra para 422. El cuerpo del response usa el patrón `new { message = ... }` que es el que ya estaba en el resto del controller.
>
> Si yo le hubiera dicho 'pon try-catch y devuelve los códigos', habría hecho lo mismo. Pero hay una diferencia importante: **no se lo he dicho yo, lo ha decidido él**. Leyendo cómo lo hacen los otros endpoints del controller. Eso es lo que escala con cualquier codebase, no solo con este. Si vuestro repo tiene quinientos endpoints siguiendo un patrón concreto, el agente lo va a respetar."

Abres la excepción nueva:

```csharp
namespace OrderManagement.Application.Exceptions;

public class InvalidOrderStateException : Exception
{
    public int OrderId { get; }
    public OrderStatus CurrentState { get; }

    public InvalidOrderStateException(int orderId, OrderStatus currentState)
        : base($"Order {orderId} is in state {currentState} and cannot be cancelled.")
    {
        OrderId = orderId;
        CurrentState = currentState;
    }
}
```

> "Y la excepción nueva. Hereda de `Exception`. Constructor que recibe el `orderId` y el `currentState`. Las dos como propiedades para que cualquier middleware de error pueda introspeccionarlas. Mensaje formateado.
>
> Esto es exactamente lo que hace la `CustomerNotFoundException` que ya estaba en el proyecto. Ha replicado el patrón sin que yo se lo dijera."

**Tiempo:** ~2 minutos.

---

### Bloque 6 — Limpieza, recap y cliffhanger (~2 min)

Vuelves al terminal donde está Claude Code. Le respondes:

```
> Perfecto, no hagas nada más. No commitees los cambios — esto es una
  demo y los voy a descartar para que la rama demo/1.1 quede limpia.
```

> "Importante esto. Los cambios que ha hecho no los voy a commitear. Esta demo es la demo conceptual del 1.1 — la rama queda solo con README y `docs/DEMOS.md`. El endpoint de cancelación es solo para que vosotros vierais el ciclo agentic en pantalla. Lo descartamos para que cuando cualquier alumno haga `git checkout demo/1.1` vea el mismo punto de partida."

**Sales de Claude Code:**

```
> /exit
```

**Verificas en otra terminal PowerShell que los cambios están sin commitear:**

```powershell
git status
```

Verás algo así:

```
On branch demo/1.1
Changes not staged for commit:
        modified:   src/OrderManagement.Api/Controllers/OrdersController.cs
        modified:   src/OrderManagement.Application/Handlers/CancelOrderHandler.cs

Untracked files:
        src/OrderManagement.Application/Exceptions/InvalidOrderStateException.cs
```

**Descartas los cambios:**

```powershell
git checkout -- src/
git clean -fd src/
```

Verifica:

```powershell
git status
```

```
On branch demo/1.1
nothing to commit, working tree clean
```

> "Limpio. La rama `demo/1.1` queda solo con el README y el `docs/DEMOS.md`. El endpoint que acabáis de ver no está en la rama. Era solo demostración del ciclo.
>
> **Recap de lo que acabáis de ver, en cuatro puntos.**
>
> Uno. Le he hecho una pregunta — *'explícame qué hace OrdersController'*— y ha respondido **leyendo el código**, no inventando. Sin que le pase ficheros. Sin que le diga rutas. Eso es **el agente leyendo el contexto por su cuenta**, la fase uno.
>
> Dos. Le he pedido una tarea — *'añade el endpoint de cancelación'* — y antes de tocar nada me ha presentado un plan. Ese plan ha incluido **una observación que no le pedí**: que el handler usaba una excepción genérica donde el resto del proyecto usa excepciones tipadas. Eso es **razonamiento sobre el repo**, la fase dos.
>
> Tres. Cuando aprobé, ha actuado: ha creado la excepción nueva, ha modificado el handler, ha modificado el controller. Cada toque siguiendo el patrón del proyecto. **Esa es la fase tres**, acción.
>
> Y cuatro, la que más diferencia a Claude Code de Copilot: ha lanzado **`dotnet build` por su cuenta**. Sin que se lo pida. Para verificar que lo que escribió compila. **Esa es la fase cuatro**, verificación.
>
> Las cuatro fases del ciclo agentic. Vistas en pantalla, sobre código real, en menos de cinco minutos.
>
> Y todo esto sin haber configurado nada. Sin `CLAUDE.md`. Sin permisos especiales. Sin skills. Sin subagentes. **Solo `claude` desde la raíz del repo.**
>
> En la siguiente demo, la 1.2a, vais a ver vosotros mismos cómo se instala Claude Code en una máquina virgen. Tres comandos. Y a partir de ahí ya empezáis a usarlo en vuestro propio repo el lunes."

**Tiempo:** ~2 minutos.

---

## 9. Qué resaltar verbalmente

Cinco puntos que **no pueden quedarse sin decir** durante la demo. Si te ves corto de tiempo, recortas otras partes pero estos cinco son **obligatorios**:

1. **"No le he pasado ningún fichero. Lee el repo por su cuenta."** — el cambio mental más importante respecto a chats con código pegado. Tiene que quedar grabado. Aparece en el bloque 3 cuando hace los `Read` y `Glob`.

2. **"Las cuatro fases del ciclo agentic visibles en pantalla."** — lectura, razonamiento, acción, verificación. Aparecen en el bloque 4. **Nombrarlas a medida que ocurren** — si las nombras al final cuando ya pasó todo, pierde fuerza.

3. **"La observación de la inconsistencia con `InvalidOperationException`."** — el agente detecta un detalle de diseño que un junior no detectaría. Es el momento más valioso del screencast desde el punto de vista pedagógico. Pararse aquí dos segundos extra.

4. **"`dotnet build` por iniciativa propia."** — la verificación automática. La diferencia entre Copilot y Claude Code. Aparece en el bloque 4 al final.

5. **"Esto es sin configuración. Solo `claude` desde la raíz del repo."** — recalcar que aún no hemos hecho nada de los módulos siguientes. El curso entero va a hacer esto cada vez mejor. Es un *teaser* deliberado.

**Frase de remate al final, que conviene memorizar:**

> *"Y todo esto sin haber configurado nada. Sin `CLAUDE.md`. Sin permisos. Sin skills. Sin subagentes. Solo `claude` desde la raíz del repo. En la siguiente demo lo instaláis vosotros."*

---

## 10. Slide de entrada (locución HeyGen)

> **Texto del avatar antes del screencast:**

"Y vamos a la primera demo del curso. Hasta aquí han sido treinta minutos de teoría. El paradigma agentic, las trazas del agente, el ciclo de cuatro fases, la comparativa con Copilot. Toca verlo. Lo vais a ver en frío sobre el proyecto OrderManagement. Sin configuración previa, sin `CLAUDE.md`, sin trucos. Solo el agente arrancando en un repo .NET 10 más Angular 19 y el ciclo agentic en pantalla. Tres cosas a las que tenéis que prestar atención. Una: el agente lee el repo por su cuenta. Dos: las cuatro fases del ciclo en orden, lectura, razonamiento, acción, verificación. Tres: el código que genera respeta el patrón del proyecto. Esta es la prueba de vida. La que diferencia un chat con código pegado de un agente que entiende vuestro repo. No os fijéis en el comando para arrancar ni en si tengo permisos configurados, eso es de las siguientes demos. Concentraos en el ciclo. Vamos al screencast."

---

## 11. Slide de salida (locución HeyGen)

> **Texto del avatar al volver del screencast:**

"Lo que acabáis de ver es Claude Code en su forma más simple. Ningún `CLAUDE.md`, ningún skill, ningún hook, ningún permiso configurado. Solo el agente leyendo el repo y respondiendo a dos peticiones concretas. Y aún así habéis visto las cuatro fases del ciclo agentic en orden. Lectura, con los `Read` y `Glob` apareciendo en pantalla. Razonamiento, con el plan que el agente presentó antes de actuar — incluyendo una observación de diseño que detectó por su cuenta. Acción, con los `Edit` y los `Write`. Y verificación, con el `dotnet build` que lanzó por iniciativa propia. Si os habéis quedado con la sensación de *'esto va más allá de un autocompletado'*, ese era el objetivo. La pregunta natural ahora es: cómo se instala esto. Y eso es lo siguiente. Empezamos con el cinco punto uno punto dos."

---

## 12. Tiempo total estimado

| Bloque | Tiempo |
|---|---|
| Slide de entrada (avatar) | ~30 seg |
| Bloque 1 — Setup visible y orientación | ~1 min 30 seg |
| Bloque 2 — Recordatorio de la teoría | ~1 min |
| Bloque 3 — Traza 1: explicar OrdersController | ~3 min |
| Bloque 4 — Traza 2: el endpoint de cancelar (las 4 fases) | ~7 min |
| Bloque 5 — Mirar lo que se ha generado | ~2 min |
| Bloque 6 — Limpieza, recap y cliffhanger | ~2 min |
| Slide de salida (avatar) | ~30 seg |
| **Total screencast** | **~16-18 min** |
| **Total con avatar** | **~17-19 min** |

> Si hay preguntas del alumno durante el screencast, súmale 2-3 minutos. La demo está pensada para encajar en un bloque de **20 minutos** dentro de la sesión.

**Margen de seguridad por si algo va lento:**

- **Si Claude Code tarda más de 30 segundos en leer el codebase en el bloque 3,** no rellenes con relleno. Aprovecha y comenta lo que está leyendo: *"fíjate, ahora ha entrado en `CancelOrderHandler.cs`, está construyendo el modelo mental del proyecto"*. Eso es contenido pedagógico, no relleno.

- **Si el plan tarda en aparecer en el bloque 4,** lee con voz alta los ficheros que va abriendo. Cada `Read` es una decisión que merece ser comentada. *"Mira, ahora abre `OrderNotFoundException`. Está pensando en cómo va a manejar el 404."*

- **Si Claude Code propone un plan distinto al esperado** (por ejemplo, no detecta la inconsistencia de `InvalidOperationException` o la detecta de otra forma), **adapta el guion al plan real**. No leas el plan literal del manual — léelo en pantalla. La pedagogía es el ciclo, no el plan exacto. Si propone algo razonable y distinto, comentas igual.

- **Si `dotnet build` falla** por algo inesperado, **no improvises silencio**. Di al alumno: *"ojo, esto es un caso real — vamos a ver cómo se recupera"*. Y deja que Claude Code lo arregle. Eso es valioso por sí mismo: ven el ciclo completo iterando hasta verde.

- **Si Claude Code se anticipa y crea código sin pedir permiso**, no detengas la demo. Comenta: *"a veces va directo a actuar — para esta demo me hubiera gustado que esperara, pero el ciclo es el mismo. Lo que veis en pantalla son las cuatro fases comprimidas"*. Y sigue.

---

# Apéndice A — Decisiones pedagógicas justificadas

> Esta sección no se graba. Es para Pedro, formador, justificando las decisiones del manual de demo.

**¿Por qué arrancar Claude Code si el 1.1 es teoría pura?**

Porque la teoría pura sin demo deja al alumno con una sensación de "esto que me cuentas suena bien, pero a ver". La demo le permite **comprobar lo que se le contó en la gamma**. La diferencia con la 1.2a es que aquí **el alumno NO instala**. Solo ve. La instalación es la 1.2a.

**¿Por qué la traza es "crear endpoint de cancelar" y no otra?**

Porque es exactamente la traza del slide 19 de la gamma 1.1. El alumno ya la ha visto en abstracto. Ahora la ve materializada. La conexión es directa.

**¿Por qué descartar los cambios al final?**

Para que cualquier alumno que clone el repo y haga `git checkout demo/1.1` vea exactamente el mismo punto de partida que las demás demos asumen. Si dejáramos los cambios, la rama `demo/1.1` ya tendría un endpoint de cancelación que las demos siguientes no esperan. Rompería el hilo.

**¿Por qué pedir el plan explícitamente con "antes de tocar nada, dime el plan"?**

Para que la fase 2 (razonamiento) sea **visible**. En el modo por defecto Claude Code a veces planifica internamente y va directo a actuar. Forzar el plan garantiza que el alumno vea las cuatro fases en orden, no tres seguidas y una al final.

**¿Por qué el endpoint de cancelar es la inconsistencia detectada?**

Porque es un caso pedagógicamente perfecto: el agente detecta algo que **un dev senior detectaría** (inconsistencia de patrón de excepciones), lo señala como observación, y propone arreglo. Es el momento donde el alumno ve la diferencia entre un autocompletado y un agente. Si la traza fuera trivial (*"añade un endpoint y ya"*), el contraste se perdería.

**¿Por qué Windows y no cross-platform?**

Porque Pedro graba en Windows. El alumno que ve el screencast ve PowerShell. Si la audiencia es .NET, la mayoría también está en Windows. Si surge alguna pregunta de Mac/Linux, se aborda en preguntas, no en el screencast.

**¿Por qué no hay tests en esta demo?**

Porque generación de tests es del módulo 5. Meter aquí un test "para mostrar" sería invadir terreno de otra demo. La demo 1.1 se centra en el ciclo agentic. Los tests son del 5.3a.
