# Demo 1.2b — CLAUDE.md, settings.json y permisos para OrderManagement

> **Versión:** v1 | **Módulo:** 1 | **Sub:** 1.2b | **Estado:** ✅ Versión final
> **Archivo:** `demo_M01-S1.2b-claudemd-settings-permisos-windows-v1.md`
> **Branch before:** `demo/1.2b-before`  (estado al hacer `git checkout` antes de grabar)
> **Branch after:**  `demo/1.2b-after`   (estado final que la siguiente clase asume)
> **Branch parent:** `demo/1.2a-after`
> **Tiempo total estimado:** ~26-32 minutos
> **Tipo:** Demo de configuración del proyecto (INFRA). **Aquí vive la pieza pedagógica estrella del módulo 1: el ejercicio ANTES vs DESPUÉS.** El alumno hace la **misma pregunta** que el formador hizo en la demo 1.1 (*"¿qué hace OrdersController.cs?"*), una vez **sin** `CLAUDE.md` (en `demo/1.2b-before`) y otra vez **con** `CLAUDE.md` decente (tras escribirlo en pantalla; `demo/1.2b-after` ya lo tiene pre-cocinado para la siguiente clase). Sigue el patrón **before/after** definido en [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md).
> **Plataforma:** Windows 11 (PowerShell 7).

---

## 1. Contexto

En la 1.1 vimos qué hace el agente. En la 1.2a vimos cómo se instala. **Aquí vemos cómo se le da contexto del proyecto** — y es donde más rentabilidad pedagógica vamos a sacar de toda la sesión 1 del curso.

Lo que el alumno ya sabe llegado a este punto:

- Qué es Claude Code, cómo funciona el ciclo agentic.
- Cómo se instala (PowerShell, `irm install.ps1 | iex`, OAuth).
- Las trampas del primer arranque (PATH cacheado, `claude doctor`).

Lo que la gamma 1.2b ya le ha contado en sus 47 slides:

- **CLAUDE.md** — qué es, qué meter, qué no, anatomía completa para .NET + Angular, los tres patrones (greenfield, legacy, monorepo).
- **AGENTS.md** — el estándar cross-tool, cuándo usarlo y cuándo no compensa.
- **`.claude/settings.json`** — los tres scopes (user, project, local) y casos típicos de tropiezo.
- **Permisos** — `allow`, `deny`, patrones por tipo de proyecto, modo autónomo y cuándo NUNCA usarlo.
- **Plantilla CLAUDE.md** lista para llevarse al puesto.

Esta demo aterriza **todo eso** sobre OrderManagement. Y lo hace con la pieza pedagógica más fuerte de la sesión: el alumno **observa**, en pantalla, la diferencia entre Claude Code respondiendo a una pregunta sin `CLAUDE.md` y respondiendo a la misma pregunta **con** `CLAUDE.md`. La diferencia es **operativa, no estética**, y es el momento donde el alumno deja de ver el `CLAUDE.md` como "papeleo" y empieza a verlo como la pieza más rentable del módulo.

> **Decisión deliberada:** la rama `demo/1.2b-after` queda como **punto de partida real** para todas las demos siguientes del curso. A partir de aquí, **todas las ramas heredan** el `CLAUDE.md` y los `settings.json` que construimos en este screencast. Es la primera demo que **deja huella permanente** en el repo.

---

## 2. Objetivo de la demo

Cinco cosas concretas que tienen que quedar en la cabeza del alumno cuando termine los ~22 minutos de screencast:

1. **El `CLAUDE.md` no es opcional.** Sin él, Claude Code es un cocinero sin menú. Con él, conoce vuestras convenciones desde el segundo cero. La diferencia es **medible**: cinco minutos antes y después de la misma pregunta.

2. **Los cinco bloques de un `CLAUDE.md` decente.** Visión general, estructura de carpetas, comandos clave, convenciones de código, reglas duras. **No más, no menos.** Si te pasas, metes ruido. Si te quedas corto, metes ambigüedad.

3. **Los tres scopes de `settings.json`.** User, project, local. Cada uno tiene su sitio. **Permisos de equipo en `project`. Cuenta API personal en `local`.** Confundirlos genera fricción.

4. **`allow` y `deny` con criterio.** Permisos por patrón. `Bash(dotnet test)` permitido siempre. `Bash(rm -rf *)` denegado siempre. La granularidad correcta marca la diferencia entre fricción y seguridad.

5. **Modo autónomo: nunca en máquina de cliente.** El alumno debe salir de la demo con esa frase grabada. `--dangerously-skip-permissions` es para sandbox y CI controlado. **Nunca portátil de trabajo.**

Y dos cosas que tienen que **NO quedar** en su cabeza:

- *"El `CLAUDE.md` perfecto requiere días."* — no, treinta minutos honestos para la primera versión y se va refinando.
- *"Si tengo `AGENTS.md` no necesito `CLAUDE.md`."* — el manual lo dice claro: en equipo .NET + Angular, **`CLAUDE.md` solo**. `AGENTS.md` es para equipos multi-herramienta.

---

## 3. Branch `demo/1.2b-before`

Punto de partida del screencast.

```
demo/1.2b-before
```

**Parte de:** `demo/1.2a-after`.

**Estado del repo:** idéntico a `demo/1.2a-after`. El proyecto OrderManagement **no tiene aún `CLAUDE.md` ni `.claude/settings.json`** — esa es justamente la pregunta pedagógica de la demo: ¿qué responde Claude Code cuando le preguntas sobre el repo y no tiene contexto persistente?

**Estado de la máquina:** Claude Code instalado y autenticado (de la demo 1.2a). Ya operativo.

> El formador hace `git checkout demo/1.2b-before` antes de empezar a grabar. La pregunta «¿qué hace `OrdersController.cs`?» se hace en directo desde aquí, **sin CLAUDE.md**. Después se escribe el `CLAUDE.md` en pantalla y se vuelve a preguntar para mostrar el contraste.

---

## 4. Branch `demo/1.2b-after`

Estado final que la siguiente clase (1.3a) asume.

```
demo/1.2b-after
```

**Parte de:** `demo/1.2b-before`.

**Qué añade respecto a `-before`:** tres cosas al repo — el fichero `CLAUDE.md` en la raíz (~140 líneas), el fichero `.claude/settings.json` con permisos sanos, y la entrada en `.gitignore` que excluye `.claude/settings.local.json` (no commiteado, sólo mostrado en pantalla durante la demo). **Es la primera rama del curso que añade configuración estructural al repo.** Todas las demos siguientes parten de aquí.

**Cómo se prepara:** ver §6b. Se materializa antes de grabar.

> Durante la grabación, el formador escribe el `CLAUDE.md` y los `settings.json` en directo desde `demo/1.2b-before`, lanza la pregunta de contraste, y al cerrar descarta los cambios reales — la rama `demo/1.2b-after` ya tiene el resultado pre-cocinado para la siguiente clase.

---

## 5. Estado del repo al hacer `git checkout demo/1.2b-before`

Idéntico a la rama `demo/1.2a-after`. La estructura del proyecto no cambia respecto a 1.1.

```
ordermanagement/
├── docs/
│   └── DEMOS.md                        (con demo 1.2a marcada)
├── src/                                (sin cambios)
├── frontend/                           (sin cambios)
├── tests/                              (sin cambios)
├── .gitignore                          (sin entrada para .claude/settings.local.json)
└── README.md                           (descripción del proyecto)
```

**Estado de la máquina Windows del formador:**

```
✅ Claude Code instalado y autenticado (de la demo 1.2a)
✅ Git for Windows
✅ PowerShell 7
✅ VS Code con el repo cargado
❌ Sin CLAUDE.md en este repo
❌ Sin .claude/ en este repo
```

**Lo que el alumno verá al final de la demo:**

- `CLAUDE.md` en la raíz del repo, con los cinco bloques (visión, estructura, comandos, convenciones .NET, convenciones Angular, reglas duras).
- `.claude/settings.json` commiteado con `allow` y `deny` apropiados para el equipo.
- `.claude/settings.local.json` en su máquina pero **no commiteado** (en `.gitignore`).
- `.gitignore` actualizado para ignorar `.claude/settings.local.json`.
- Una segunda ejecución de la pregunta "*¿qué hace OrdersController.cs?*" que demuestra el contraste con la primera (sin CLAUDE.md).

---

## 6a. Prompt para Claude Code — preparar `demo/1.2b-before`

> Crea la rama de partida del screencast desde `demo/1.2a-after`, sin tocar nada más. La pieza viva (escribir `CLAUDE.md` y `settings.json`) se demuestra en directo desde aquí; los artefactos finales viven en `demo/1.2b-after` (ver §6b), preparados aparte.

````
Estoy preparando la demo 1.2b del curso de Claude Code para devs .NET +
Angular. Esta demo es la configuración del proyecto OrderManagement con
CLAUDE.md, settings.json y permisos. Sigue el patrón before/after
(ver demo M0.2).

Quiero que prepares la rama `demo/1.2b-before` desde `demo/1.2a-after`.
Esta rama es el punto de partida del screencast: el repo NO debe tener
CLAUDE.md ni .claude/ — ese estado virgen es la pregunta pedagógica
de la demo (¿qué responde Claude Code sin contexto persistente?).

## Tarea única

```powershell
git checkout demo/1.2a-after
git pull
git checkout -b demo/1.2b-before
```

NO crees CLAUDE.md, NO crees .claude/settings.json, NO toques .gitignore.
Esos artefactos van en `demo/1.2b-after` (ver el siguiente prompt §6b).

NO hagas commit. La rama `demo/1.2b-before` es exactamente igual a
`demo/1.2a-after` excepto en el nombre.

# Cuando termines, dime

1. Que la rama demo/1.2b-before está creada.
2. Que `git diff demo/1.2a-after demo/1.2b-before` no muestra cambios.
````

---

## 6b. Prompt para Claude Code — preparar `demo/1.2b-after`

> Lo que tú, formador, copias y pegas en Claude Code para materializar la rama final con todos los artefactos pre-cocinados. Durante el screencast vas a **mostrar el `CLAUDE.md` creándose paso a paso** desde `demo/1.2b-before`; este prompt asegura que `demo/1.2b-after` ya tiene el contenido validado y la siguiente clase parte de él aunque el directo no salga clavado.

````
Estoy preparando la demo 1.2b del curso de Claude Code para devs .NET +
Angular. Esta demo es la configuración del proyecto OrderManagement con
CLAUDE.md, settings.json y permisos.

# Contexto

Estoy en la rama `demo/1.2b-before` del repo `ordermanagement`. La rama
parte de `demo/1.2a-after` y NO tiene aún CLAUDE.md ni .claude/settings.json.

Quiero que prepares la rama `demo/1.2b-after` desde `demo/1.2b-before`
con tres ficheros nuevos:

1. CLAUDE.md en la raíz del repo
2. .claude/settings.json (compartido con el equipo, va a git)
3. .gitignore actualizado para excluir .claude/settings.local.json

NO crees el .claude/settings.local.json — eso lo voy a hacer en vivo
durante el screencast como ejemplo de configuración personal.

# Lo que necesito

Cinco tareas:

## Tarea 1: crear la rama

```powershell
git checkout demo/1.2b-before
git checkout -b demo/1.2b-after
```

## Tarea 2: crear CLAUDE.md en la raíz

Contenido exacto del fichero (140-160 líneas):

```markdown
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
```

## Tarea 3: crear .claude/settings.json

Crea la carpeta `.claude/` si no existe. Contenido del fichero:

```json
{
  "permissions": {
    "allow": [
      "Read",
      "Write",
      "Edit",
      "Glob",
      "Grep",
      "Bash(dotnet build)",
      "Bash(dotnet test*)",
      "Bash(dotnet run*)",
      "Bash(dotnet ef migrations*)",
      "Bash(dotnet ef database*)",
      "Bash(dotnet restore)",
      "Bash(dotnet format*)",
      "Bash(npm install)",
      "Bash(npm run *)",
      "Bash(npm test)",
      "Bash(npm ci)",
      "Bash(npm start)",
      "Bash(git status)",
      "Bash(git diff*)",
      "Bash(git log*)",
      "Bash(git add*)",
      "Bash(git commit -m*)",
      "Bash(git checkout*)",
      "Bash(git branch*)",
      "Bash(git fetch*)",
      "Bash(git pull*)"
    ],
    "deny": [
      "Bash(rm -rf*)",
      "Bash(Remove-Item -Recurse*)",
      "Bash(git push --force*)",
      "Bash(git push -f*)",
      "Bash(git push origin main)",
      "Bash(git push origin master)",
      "Bash(git reset --hard*)",
      "Bash(git clean -fdx*)",
      "Read(./.env)",
      "Read(./.env.*)",
      "Read(./secrets/**)",
      "Read(./appsettings.Production.json)",
      "Write(./appsettings.Production.json)",
      "Write(./.env*)"
    ]
  }
}
```

## Tarea 4: actualizar .gitignore

Añade al final del .gitignore actual estas líneas (si no están ya):

```
# Claude Code
.claude/settings.local.json
.claude/cache/
```

## Tarea 5: actualizar docs/DEMOS.md

Localiza la línea:

```
- [ ] demo/1.2b — CLAUDE.md y settings.json para .NET 10 + Angular 19
```

Y cámbiala por:

```
- [x] **demo/1.2b** — CLAUDE.md y settings.json para .NET 10 + Angular 19
```

## Tarea 6: verificar build y commitear

Antes de commitear, verifica que el proyecto sigue compilando:

```powershell
dotnet build
```

Esperado: 0 warnings, 0 errors.

Si todo bien, commit único:

```powershell
git add CLAUDE.md .claude/settings.json .gitignore docs/DEMOS.md
git commit -m "demo/1.2b-after: CLAUDE.md, settings.json y permisos para OrderManagement"
```

NO hagas push. Yo lo hago manualmente cuando lo revise.

# Restricciones (importantes)

- NO crees `.claude/settings.local.json`. Eso lo voy a crear yo en vivo
  durante el screencast como ejemplo de configuración personal.
- NO añadas skills, subagentes ni hooks. Esos son módulos 2 y 3.
- NO toques el código de la app, ni los .csproj, ni Program.cs.
- NO modifiques el README.md.
- El `CLAUDE.md` debe respetar la regla de las 200 líneas máximo —
  si te queda más largo, recorta los bloques de convenciones.

# Cuando termines, dime

1. Que la rama demo/1.2b-after está creada desde demo/1.2b-before.
2. Que CLAUDE.md está creado y tiene los cinco bloques (estructura,
   comandos, convenciones .NET, convenciones Angular, reglas duras).
3. Que .claude/settings.json tiene allow y deny apropiados.
4. Que .gitignore excluye .claude/settings.local.json.
5. Que docs/DEMOS.md tiene la 1.2b marcada como hecha.
6. Que dotnet build pasa limpio.
7. Que el commit está hecho.

Si en algún punto tienes dudas, para y pregúntame antes de continuar.
````

---

## 7. Artefactos que Claude Code debe generar

```
✓ Rama nueva: demo/1.2b-after (parte de demo/1.2b-before)
✓ CLAUDE.md en raíz (~140-160 líneas, 5 bloques)
✓ .claude/settings.json con allow y deny
✓ .gitignore actualizado (excluye .claude/settings.local.json)
✓ docs/DEMOS.md con 1.2b marcada como hecha
✓ Verificación de build OK: dotnet build limpio
✓ Commit único: "demo/1.2b-after: CLAUDE.md, settings.json y permisos para OrderManagement"
```

**Lo que NO debe haber generado:**

- ❌ `.claude/settings.local.json` (eso se crea en vivo durante el screencast)
- ❌ Skills, subagentes, hooks (módulos 2 y 3)
- ❌ Cambios en código de la app (controllers, handlers, frontend)
- ❌ Cambios en README.md
- ❌ Cambios en `.csproj` o `package.json`
- ❌ Tests nuevos (módulo 5)
- ❌ Endpoints nuevos en controllers

> Si Claude Code se anticipa y crea algo de la lista de prohibidos, **se rechaza el output**. La demo 1.2b es estrictamente CLAUDE.md + settings.json + permisos.

**Estado final del árbol después del prompt:**

```
ordermanagement/
├── .claude/
│   └── settings.json               ← NUEVO
├── docs/
│   └── DEMOS.md                    ← MODIFICADO (1 línea)
├── src/                            ← sin cambios
├── frontend/                       ← sin cambios
├── tests/                          ← sin cambios
├── .gitignore                      ← MODIFICADO (3 líneas añadidas)
├── CLAUDE.md                       ← NUEVO
└── README.md                       ← sin cambios
```

---

## 8. Guion del screencast

**Duración estimada del screencast: ~22-25 minutos.**

Esta es la demo más densa del módulo 1. Nueve bloques. La pieza pedagógica estrella (ANTES vs DESPUÉS) está en los bloques 4 y 8.

> **Antes de empezar a grabar**, asegúrate de:
> - Tener PowerShell 7 abierto en pantalla completa, fuente al menos 16pt.
> - Tener VS Code abierto al lado con el repo `ordermanagement` cargado en `demo/1.2b-before` (la rama de partida del screencast, **no en `demo/1.2b-after`** que ya está pre-cocinada para la siguiente clase).
> - **Importante:** asegúrate de que **NO existe** `CLAUDE.md` ni `.claude/` en el repo todavía. Si los tienes de pruebas anteriores, bórralos antes de grabar.
> - Cerrar Slack, Teams, navegadores con notificaciones.
> - Tener la plantilla `CLAUDE.md` del entregable de la sesión a mano (en otra ventana o impresa) por si necesitas consultarla.

---

### Bloque 1 — Setup visible y orientación al alumno (~1 min 30 seg)

**Pantalla compartida.** A la izquierda, VS Code con el repo en `demo/1.2b-before`. A la derecha, una terminal PowerShell vacía en la raíz del proyecto.

**Antes de teclear nada,** muestras visualmente que el repo NO tiene `CLAUDE.md` ni `.claude/`. Esto es el punto de partida y conviene fijarlo en la cabeza del alumno.

**En la terminal PowerShell, tecleas:**

```powershell
ls
```

```
    Directorio: C:\Users\pedro\projects\ordermanagement

Mode    LastWriteTime    Length Name
----    -------------    ------ ----
d----   ...                     docs
d----   ...                     frontend
d----   ...                     src
d----   ...                     tests
-a---   ...           1024      .gitignore
-a---   ...           2456      README.md
```

> "Estamos en la rama `demo/1.2a`. Repaso del estado: el repo tiene `src`, `frontend`, `tests`, `docs`. Tiene un README, un `.gitignore`. **No tiene `CLAUDE.md`. No tiene `.claude/`.** El proyecto está exactamente como lo vimos en la demo 1.1 — virgen para Claude Code.
>
> Pero ahora hay una diferencia importante respecto a la 1.1: **mi máquina sí tiene Claude Code instalado y autenticado**. Eso lo hicimos en la demo 1.2a. Si lanzo `claude --version` aquí, me responde."

**Tecleas:**

```powershell
claude --version
```

```
2.1.x
```

> "Versión 2.1. Listo. Y ahora, antes de configurar nada, vamos a hacer una cosa que va a ser **el momento pedagógico más importante de toda la sesión**.
>
> Vamos a hacer al agente la **misma pregunta** que vimos en la demo 1.1 — *'explícame qué hace OrdersController.cs'* — pero esta vez **sin `CLAUDE.md`**. Vamos a ver cómo responde. Y luego, después de configurar el `CLAUDE.md`, le vamos a hacer **exactamente la misma pregunta**. Y comparamos las dos respuestas.
>
> El contraste va a hablar por sí solo."

**Tiempo:** ~90 segundos.

---

### Bloque 2 — ANTES: la pregunta sin CLAUDE.md (~3 min)

> "Arranco Claude Code en frío, sin configuración. Primera ejecución."

**Tecleas:**

```powershell
claude
```

Aparece el banner de Claude Code:

```
 Welcome to Claude Code v2.1.x

 cwd: C:\Users\pedro\projects\ordermanagement
 model: claude-opus-4.7
 No CLAUDE.md found in current directory or parents.
 Type / for commands, ? for help

>
```

> "Atentos a la línea: **'No CLAUDE.md found in current directory or parents'**. El agente me dice explícitamente que no ha encontrado contexto del proyecto. Está en frío. Es lo mismo que en la demo 1.1.
>
> Le hago la pregunta:"

**Tecleas:**

```
> Explícame qué hace OrdersController.cs y qué patrones sigue.
  Quiero saber tanto los endpoints como las convenciones del equipo.
```

**Pulsas Enter. Empiezan a aparecer los `Read`:**

```
● Glob(**/OrdersController.cs)
● Read(src/OrderManagement.Api/Controllers/OrdersController.cs)
● Read(src/OrderManagement.Application/Handlers/CreateOrderHandler.cs)
● Read(src/OrderManagement.Application/Handlers/CancelOrderHandler.cs)
● Read(src/OrderManagement.Domain/Entities/Order.cs)
● Grep(IOrderRepository)
● Read(src/OrderManagement.Infrastructure/Repositories/IOrderRepository.cs)
```

> "Igual que en la demo 1.1. Lee el controller. Lee los handlers. Lee la entidad. Lee la interfaz del repo. Construye contexto leyendo el código.
>
> Tarda... unos quince segundos."

Aparece la respuesta:

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

## Dependencias y patrones

Inyecta IMediator (MediatR) en el constructor. Cada endpoint construye 
un comando/query y lo despacha. Patrón CQRS clásico.

## Convenciones que detecto

- Manejo de errores con try-catch específicos (CustomerNotFoundException,
  OrderNotFoundException) traducidos a códigos HTTP. Esto sugiere que
  el equipo prefiere excepciones tipadas sobre Result<T> o similar.
- CancellationToken propagado en cada endpoint. Buena señal.
- Métodos async, sin Wait() ni Result.

Algunas cosas que no puedo determinar sin más contexto:
- Qué framework de tests usáis (no veo tests ni mención).
- Si los DTOs van en una carpeta concreta o vienen del Domain
  directamente.
- El patrón de naming para validators y handlers.
- Si hay reglas sobre qué carpetas no tocar.

¿Hay algún CLAUDE.md o documentación de equipo que quieras que lea?
```

> "Y aquí está la respuesta. Mirad bien el final.
>
> Lo bueno: ha hecho el trabajo. Lee el código real. Habla del `CreateOrderValidator`, de las dos excepciones tipadas, de MediatR. Identifica los patrones leyéndolos.
>
> Pero atentos a las **'cosas que no puedo determinar sin más contexto'**. Las marca explícitamente. Cuatro huecos:
>
> Uno. **Qué framework de tests usáis.** No tiene cómo saberlo. La carpeta `tests/` está vacía. Podría ser xUnit, MSTest, NUnit. Probablemente acertaría con xUnit por convención .NET, pero no tiene certeza.
>
> Dos. **Dónde van los DTOs.** No los ve, no sabe si en `Contracts/`, en el Domain directamente, o en una carpeta a parte.
>
> Tres. **El patrón de naming.** Sospecha por intuición pero no tiene confirmación.
>
> Cuatro. **Qué carpetas no tocar.** Esto es importante. Si yo le pidiera que toque algo en `Generated/`, lo haría sin saber que hay equipos donde es código autogenerado y se rompe el pipeline.
>
> Y pregunta al final: *'¿hay algún CLAUDE.md o documentación de equipo?'*. El propio agente está pidiendo el contexto que no tiene.
>
> Esta respuesta es **buena**. Razonable. Útil. Pero está incompleta y el agente lo sabe. Vamos a comparar después con la respuesta cuando le demos `CLAUDE.md`."

**Salgo de Claude Code (Ctrl+C):**

> "Salgo. Ahora vamos a darle el contexto que pide."

**Tiempo:** ~3 minutos.

---

### Bloque 3 — Construir CLAUDE.md paso a paso (~8 min)

Esta es la pieza central de la demo. **Construyes el `CLAUDE.md` en vivo, comentando cada bloque mientras lo escribes.** No copias y pegas un fichero completo — eso pierde valor pedagógico. Vas escribiendo bloque a bloque y explicando.

> "Vamos a construir el `CLAUDE.md` para OrderManagement. Voy a hacerlo en vivo y os voy a contar cada decisión. Esto es lo que vais a hacer vosotros el lunes en vuestro repo, así que prestad atención al **proceso**, no solo al resultado."

**En VS Code, abres una pestaña nueva, creas el fichero `CLAUDE.md` en la raíz del repo.**

> "Cinco bloques. Visión general, estructura, comandos, convenciones, reglas duras. La gamma 1.2b lo cubrió en los slides 5 al 10. Vamos por orden."

**Bloque 3a — Visión general (slide 6 de la gamma) (~1 min)**

Tecleas en VS Code:

```markdown
# Proyecto: OrderManagement

Sistema de gestión de pedidos B2B. API REST en .NET 10 + frontend Angular 19.
Proyecto demo del curso Claude Code para devs .NET + Angular.
```

> "Visión general. Tres líneas. **Tres.** No me enrollo. *'Sistema de gestión de pedidos B2B. API REST en .NET 10 más frontend Angular 19. Proyecto demo del curso'*. Eso es. Si yo metiera aquí cuatro párrafos contando que el proyecto se inició en 2023 con la motivación de digitalizar el sector mayorista... estaría metiendo ruido. **Al agente le da igual la motivación**, solo le importa qué tiene que hacer. La gamma 1.2b lo dijo claro en el slide 16 — *'documentación pensada para humanos'* es trampa."

**Bloque 3b — Estructura de carpetas (slide 7, 18) (~1 min 30 seg)**

Tecleas:

```markdown
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
```

> "Estructura de carpetas. Aquí no listo todo el árbol, solo lo que **no es obvio**. Si el agente abre `src/` y ve cuatro carpetas con nombres claros, ya lo deduce. Pero hay tres detalles que quiero que entienda **explícitamente**.
>
> Uno. *'Solo presentación; sin lógica de negocio'* en `Api`. Esto evita que cuando le pida 'añadir un endpoint', meta lógica de validación dentro del controller. Lo va a poner en el handler.
>
> Dos. *'Sin dependencias a otras capas'* en `Domain`. Le digo explícitamente la regla de Clean Architecture. Si le pido tocar el dominio, no me va a meter `using Microsoft.EntityFrameworkCore` ahí.
>
> Tres. *'Servicios mock'* en `Infrastructure`. Le aviso de que `EmailService` y `PaymentService` no son reales. Si me genera un test que asume que mandan emails de verdad, va a romperse.
>
> Esto son tres frases en total. Pero cambian completamente lo que el agente va a hacer cuando le pida cosas."

**Bloque 3c — Comandos clave (slide 8 — esto es oro) (~2 min)**

Tecleas:

```markdown
## Comandos

- `dotnet build` — compilar la solución completa.
- `dotnet test` — ejecutar todos los tests (cuando existan).
- `dotnet run --project src/OrderManagement.Api` — arrancar la API en
  http://localhost:5000.
- `cd frontend; npm install` — instalar dependencias frontend.
- `cd frontend; npm start` — levantar Angular en :4200.
- `cd frontend; npm run lint` — linter Angular con eslint.
- `cd frontend; npm run build` — build de producción del frontend.
```

> "Comandos clave. La gamma lo marcó en el slide 8 con la frase: **'esto es oro'**. Y lo es, por dos razones.
>
> Una. **Le evita inventar comandos.** Si yo le digo 'lanza los tests' y no le he dicho que existe `dotnet test`, va a probar variantes — `dotnet test`, `dotnet xunit run`, `npm test`. A veces acierta, a veces no. Aquí lo sabe desde el segundo cero.
>
> Dos. Mirad el detalle de Windows. **`cd frontend; npm install`** con punto y coma. **No `cd frontend && npm install`**. ¿Por qué? Porque el `&&` solo funciona en PowerShell desde la versión 7. En CMD legacy no funciona, en PowerShell 5 falla. **El punto y coma es cross-shell en Windows**. Si yo le pongo `cd frontend && npm install` y mi máquina o la del compañero usa CMD, falla.
>
> Esto es lo que la gamma dijo en el slide 8: 'esto es oro'. Sin estos comandos, el agente inventa. Con estos comandos, ejecuta directamente lo que vuestro equipo ejecuta. Hábito de equipo, no decisión del agente."

**Bloque 3d — Convenciones .NET (slide 9, 20) (~2 min)**

Tecleas:

```markdown
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
```

> "Convenciones .NET. Aquí me extiendo más, pero con criterio. Cada bullet es una decisión que el agente va a respetar.
>
> Mirad el detalle: **'Tests: xUnit más NSubstitute más FluentAssertions. Nunca Moq.'** Esto vale oro. Hay equipos que migraron de Moq a NSubstitute por la polémica de telemetría hace unos meses. Si yo no le digo al agente *'nunca Moq'*, me va a generar tests con `Mock<IOrderRepository>` porque es lo que más vio en su entrenamiento. **Una sola línea — 'nunca Moq' — ahorra rehacer cien tests.**
>
> Y la regla de naming de tests: **`MétodoBajoTest_Escenario_ResultadoEsperado`**. Esto se aplica desde la primera vez que le pida tests. Sin esto, va a inventar tres convenciones distintas en la misma suite."

**Bloque 3e — Convenciones Angular (slide 21) (~1 min)**

Tecleas:

```markdown
## Convenciones Angular

- Componentes standalone siempre. Nada de NgModules nuevos.
- Signals para estado local; SignalStore para estado compartido.
- Reactive Forms con tipado estricto cuando aplique.
- HTTP requests vía HttpClient inyectado, retornando Observables.
- Estilos con SCSS, tokens en `frontend/src/styles/_tokens.scss`.
- Tests: Karma + Jasmine para unit, Playwright para E2E (cuando aplique).
```

> "Convenciones Angular. Más cortas porque el frontend en este curso no es el foco — es el módulo 5 que sí cubre Angular pero con foco en handoff de Claude Design.
>
> Lo importante: **'componentes standalone siempre. Nada de NgModules nuevos'**. Sin esto, el agente puede crear NgModules porque era el patrón estándar hasta Angular 14. **Línea explícita.**"

**Bloque 3f — Reglas duras (slide 10, 22) (~1 min 30 seg)**

Tecleas:

```markdown
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
```

> "Reglas duras. Esto es lo que la gamma marcó como **'lo que un junior haría mal'** y la diferencia entre que el agente meta la pata y no la meta.
>
> Mirad la cuarta: *'Servicios externos son mocks. No intentes implementarlos como reales sin pedirme antes — la lógica de envío de email y de cobro no está en este repo'*. Sin esta línea, si yo le pido *'implementa el flujo de cancelación con notificación al cliente'*, va a meter código que asume que `EmailService` realmente manda emails. Va a integrar SendGrid, SMTP, lo que sea. Y se va a romper.
>
> Con esta línea, el agente sabe que esos servicios son cables sueltos y no tira de ellos sin preguntar."

> "Y eso es. Cinco bloques. Lo guardo."

**Guardas el fichero.** En VS Code aparece `CLAUDE.md` en la raíz.

**Tiempo:** ~8 minutos.

---

### Bloque 4 — DESPUÉS: la misma pregunta con CLAUDE.md (~3 min)

> "Y aquí viene el momento de la verdad. Misma pregunta que antes. Pero ahora con `CLAUDE.md` en su sitio."

**Tecleas:**

```powershell
claude
```

Aparece el banner de Claude Code:

```
 Welcome to Claude Code v2.1.x

 cwd: C:\Users\pedro\projects\ordermanagement
 model: claude-opus-4.7
 ✓ CLAUDE.md loaded (147 lines)
 Type / for commands, ? for help

>
```

> "Mirad. **'CLAUDE.md loaded, 147 líneas'**. La línea de 'No CLAUDE.md found' que vimos antes ha desaparecido. El agente lo está cargando antes incluso de hablar conmigo.
>
> Le hago **exactamente la misma pregunta que antes**:"

**Tecleas:**

```
> Explícame qué hace OrdersController.cs y qué patrones sigue.
  Quiero saber tanto los endpoints como las convenciones del equipo.
```

**Pulsas Enter. Aparecen los `Read`:**

```
● Read(src/OrderManagement.Api/Controllers/OrdersController.cs)
● Read(src/OrderManagement.Application/Handlers/CreateOrderHandler.cs)
```

> "Mirad esto. Antes leyó **siete ficheros**. Ahora ha leído **dos**. ¿Por qué? Porque el `CLAUDE.md` ya le ha dicho dónde están los handlers, qué patrón siguen, qué convenciones tiene el equipo. **No tiene que ir a buscar lo que ya sabe.** El propio CLAUDE.md le ha ahorrado cinco lecturas.
>
> Eso son cinco lecturas menos de tokens. En sesiones largas, eso se acumula."

Aparece la respuesta:

```
OrdersController.cs es el controlador REST principal del proyecto.

## Endpoints

Cinco endpoints estándar siguiendo el patrón CQRS con MediatR (como
indica el CLAUDE.md):

- GET /api/orders, GET /api/orders/{id}, POST /api/orders,
  PUT /api/orders/{id}, DELETE /api/orders/{id}

## Convenciones que aplica

He cruzado lo que veo en el código con las reglas del CLAUDE.md:

✓ Naming: PascalCase para métodos públicos (Get, Create, Update,
  Delete), _camelCase con guion bajo para los campos privados
  (`_mediator`).
✓ Async/await en todo, sin .Wait() ni .Result. CancellationToken
  propagado correctamente.
✓ Manejo de errores: try-catch específicos (CustomerNotFoundException,
  OrderNotFoundException) traducidos a códigos HTTP. Sigue la regla
  del CLAUDE.md de "nada de catch genérico".
✓ CQRS con MediatR — handlers en src/OrderManagement.Application/Handlers/
  como dice el CLAUDE.md.

## Lo que falta o no encaja

He detectado dos cosas que conviene comentarte:

1. **Falta documentación XML en métodos públicos**. El controller no
   tiene `<summary>` en ningún método. Aunque las reglas del CLAUDE.md
   no lo prohíben, los métodos públicos en una API beneficiarían de
   ello especialmente para Swagger.

2. **Falta endpoint dedicado de cancelar pedido**. Veo que existe un
   CancelOrderHandler en Handlers/, pero no hay POST /api/orders/{id}/cancel
   en el controller. La operación se hace por PUT genérico cambiando
   estado, lo cual es discutible.

## Lo que NO me cuestiono

- El framework de tests (xUnit + NSubstitute + FluentAssertions, según
  CLAUDE.md). No miro tests porque la carpeta está vacía a propósito.
- La carpeta de DTOs (src/OrderManagement.Api/Contracts/). Si tuviera
  que crear un DTO nuevo, lo metería ahí siguiendo el naming Dto.
- Que los servicios externos son mocks (no intentaría implementar
  EmailService como real sin pedirte antes).
```

> "Y aquí está la respuesta con `CLAUDE.md`. Comparemos con la de antes.
>
> **Antes** decía *'no puedo determinar qué framework de tests usáis, dónde van los DTOs, el patrón de naming, qué carpetas no tocar'*. Cuatro huecos.
>
> **Ahora** los cuatro están resueltos. Y mirad la sección final: **'Lo que NO me cuestiono'**. xUnit más NSubstitute más FluentAssertions. La carpeta de DTOs en `Contracts/`. Que los servicios externos son mocks. **Toda esa información venía del CLAUDE.md** y ya está en el modelo del agente sin que tenga que ir a buscarla.
>
> Y mirad lo que **gana**. La sección 'Lo que falta o no encaja' es nueva. Detecta dos cosas:
>
> Una. **Falta documentación XML.** Sin `CLAUDE.md` no pudo decir si era importante. Con `CLAUDE.md` sí — sabe que es una API, que se genera Swagger, y que falta documentación.
>
> Dos. **Falta el endpoint dedicado de cancelar.** La misma observación que vimos en la demo 1.1, pero esta vez **encuadrada por las reglas del proyecto**.
>
> Esta es la diferencia. Con `CLAUDE.md`, el agente:
>
> - Lee menos ficheros (más rápido).
> - Aplica las convenciones desde el segundo cero (sin sorpresas).
> - Detecta inconsistencias contra **vuestras reglas**, no contra reglas genéricas.
> - Os indica qué información ya tiene asumida (sección 'Lo que NO me cuestiono').
>
> Esto que acabáis de ver — esta diferencia entre la respuesta de antes y la de ahora — es lo que la gamma 1.2b dijo cuando empezó: **'la diferencia entre un dev nuevo en Claude Code que se frustra y uno que cierra triple de tickets que antes no está en haber aprendido más comandos. Está, casi siempre, en haber escrito un CLAUDE.md decente'**. Lo acabáis de ver con vuestros ojos."

**Salgo de Claude Code (Ctrl+C):**

> "Salgo. Vamos al siguiente bloque: configurar `settings.json` y permisos."

**Tiempo:** ~3 minutos.

---

### Bloque 5 — Configurar settings.json y los tres scopes (~3 min)

> "Y vamos al `settings.json`. La gamma 1.2b lo cubrió en los slides 31 al 36. Tres scopes — user, project, local. Vamos a verlos en orden."

**Crear .claude/settings.json (project scope) en VS Code:**

> "Creo la carpeta `.claude/` en la raíz del proyecto y dentro un fichero `settings.json`."

```powershell
mkdir .claude
```

**En VS Code creas `.claude/settings.json` y escribes:**

```json
{
  "permissions": {
    "allow": [
      "Read",
      "Write",
      "Edit",
      "Glob",
      "Grep",
      "Bash(dotnet build)",
      "Bash(dotnet test*)",
      "Bash(dotnet run*)",
      "Bash(dotnet ef migrations*)",
      "Bash(dotnet ef database*)",
      "Bash(dotnet restore)",
      "Bash(dotnet format*)",
      "Bash(npm install)",
      "Bash(npm run *)",
      "Bash(npm test)",
      "Bash(npm ci)",
      "Bash(npm start)",
      "Bash(git status)",
      "Bash(git diff*)",
      "Bash(git log*)",
      "Bash(git add*)",
      "Bash(git commit -m*)",
      "Bash(git checkout*)",
      "Bash(git branch*)",
      "Bash(git fetch*)",
      "Bash(git pull*)"
    ],
    "deny": [
      "Bash(rm -rf*)",
      "Bash(Remove-Item -Recurse*)",
      "Bash(git push --force*)",
      "Bash(git push -f*)",
      "Bash(git push origin main)",
      "Bash(git push origin master)",
      "Bash(git reset --hard*)",
      "Bash(git clean -fdx*)",
      "Read(./.env)",
      "Read(./.env.*)",
      "Read(./secrets/**)",
      "Read(./appsettings.Production.json)",
      "Write(./appsettings.Production.json)",
      "Write(./.env*)"
    ]
  }
}
```

> "Esto es **scope project**. Vive en `.claude/settings.json` en la raíz del repo. **Va a git.** Lo comparte todo el equipo. Cualquier dev que clone el repo arranca con estos permisos.
>
> Vamos a leerlo juntos."

**Bloque 5a — `allow` (~1 min 30 seg)**

> "Sección **`allow`** primero. La gamma slide 39. Estos son los comandos que el agente puede ejecutar **sin pedir permiso**. Promovidos a 'Yes always' por defecto.
>
> Mirad la lógica:
>
> **Read, Write, Edit, Glob, Grep**. Las herramientas internas. Las cinco básicas que vimos en la 1.1. **Sin estas permitidas, el agente pediría aprobación cada vez que lee un fichero**. Sería insoportable.
>
> **`Bash(dotnet build)`, `Bash(dotnet test*)`, `Bash(dotnet run*)`, `Bash(dotnet ef migrations*)`, `Bash(dotnet ef database*)`, `Bash(dotnet restore)`, `Bash(dotnet format*)`**. Todo lo de .NET que vamos a usar a diario. Build, tests, run, migraciones, format. **No le pongo `Bash(dotnet *)` con asterisco genérico**. Eso le dejaría hacer `dotnet new`, `dotnet nuget`, `dotnet publish` — comandos que no quiero que ejecute solo. **Granularidad concreta.**
>
> **`Bash(npm install)`, `Bash(npm run *)`, etc.** Lo equivalente para el frontend.
>
> **`Bash(git status)`, `Bash(git diff*)`, `Bash(git add*)`, `Bash(git commit -m*)`**. Comandos de git que son operaciones de lectura o escritura local. Aquí está bien permitirlos.
>
> Lo que **NO** está en allow: **`Bash(git push *)`**. Es deliberado. El push lo hago yo, no el agente. Si el agente intenta `git push origin main`, salta a `deny`."

**Bloque 5b — `deny` (~1 min)**

> "Y la sección **`deny`**. Estos comandos están **prohibidos siempre**. El agente no puede pedirme aprobación para ejecutarlos. Si los intenta, el sistema los bloquea.
>
> Mirad lo que hay:
>
> **`Bash(rm -rf*)`** y **`Bash(Remove-Item -Recurse*)`**. Borrado recursivo. En Windows uso los dos por si Claude Code está usando Bash via Git for Windows o PowerShell directamente. Ambos cubiertos.
>
> **`Bash(git push --force*)`, `Bash(git push -f*)`, `Bash(git push origin main)`, `Bash(git push origin master)`**. Estas son las que más miedo me dan. Un `git push --force` automático puede destruir el trabajo del equipo. Un `git push origin main` puede romper la rama protegida. **Denegadas explícitamente.**
>
> **`Bash(git reset --hard*)`** y **`Bash(git clean -fdx*)`**. Operaciones que pueden perder datos.
>
> **`Read(./.env)`, `Read(./secrets/**)`, `Write(./appsettings.Production.json)`**. Ficheros sensibles. El agente no debería ni leer ni escribir aquí. Aunque el `.env` esté gitignored, no quiero que el agente lo lea por si alguien comete el error de meter una API key dentro.
>
> Esto que acabáis de ver es **el modelo de seguridad por defecto** que la gamma marcó como recomendado para proyecto cliente o producción."

**Bloque 5c — Mostrar pero NO crear settings.local.json (~30 seg)**

> "Y el tercer scope. **`local`**. Que vive en `.claude/settings.local.json`. Este fichero **va a `.gitignore`**, no se commitea. Es para preferencias que solo aplican a vuestra máquina."

**Abres `.gitignore` en VS Code y al final del fichero añades:**

```
# Claude Code
.claude/settings.local.json
.claude/cache/
```

> "Lo añado al `.gitignore`. Cuando haga `git add`, esto no se va a incluir.
>
> ¿Qué metería en `.claude/settings.local.json`? Un ejemplo típico: si vuestro equipo usa OAuth pero alguno de vosotros prefiere usar API key personal por algún motivo, lo metería aquí en lugar de en project. Otro caso: alguno usa Opus por defecto, otro usa Sonnet — preferencia personal, va a local.
>
> Pero **para esta demo no creo el fichero**. La rama `demo/1.2b-after` no necesita uno. Lo importante es que sepáis que existe el scope y que va al gitignore."

**Tiempo:** ~3 minutos.

---

### Bloque 6 — Probar permisos en runtime (~3 min)

> "Y ahora vamos a comprobar que los permisos funcionan. Voy a arrancar Claude Code y pedirle algo que está en `allow`, algo que no, y algo que está en `deny`. Tres pruebas."

**Tecleas:**

```powershell
claude
```

> "Tarea uno: pedirle que ejecute `dotnet build`. Está en `allow`."

**Tecleas:**

```
> Ejecuta dotnet build y dime el resultado.
```

**Aparece directamente la ejecución sin pedir permiso:**

```
● Bash(dotnet build)

Microsoft (R) Build Engine version 17.10.0...
Restore complete (0.5s)
  OrderManagement.Domain succeeded (0.4s)
  OrderManagement.Application succeeded (0.6s)
  OrderManagement.Infrastructure succeeded (0.8s)
  OrderManagement.Api succeeded (1.2s)

Build succeeded.
    0 Warning(s)
    0 Error(s)
```

> "Mirad. **No me ha pedido aprobación.** Lo ha lanzado directamente. `dotnet build` está en `allow`. La gamma slide 39: 'estas operaciones pasan sin aprobación'."

> "Tarea dos: pedirle algo que **no está en `allow`** ni en `deny`. Por ejemplo, un comando random."

**Tecleas:**

```
> Lánzame el comando 'dotnet --info' para ver detalles del runtime.
```

**Aparece el prompt de aprobación:**

```
The agent wants to run:
  Bash: dotnet --info

This command is not in your allow list. Allow it?

[A]llow once  [Y]es, allow always  [N]o, deny  [E]dit
>
```

> "Mirad. **`dotnet --info`** no está en mi `allow` ni en `deny`. Es un comando que casi nunca uso. El agente me pregunta — me da las cuatro opciones que vimos en la gamma 1.3b. Allow once, allow always, deny, edit.
>
> Yo le doy **'a' — Allow once**. Es seguro pero no quiero promoverlo a always porque casi nunca lo uso."

**Tecleas `a` y Enter. Se ejecuta:**

```
.NET SDK:
 Version:           10.0.100
 Commit:            abc123...
...
```

> "Bien. Una vez. La próxima vez que se lo pida, me volverá a preguntar. Esto es 'aprobar todo individualmente sin promover a always' — el patrón B de la gamma 1.3b. Para esto, está bien."

> "Tarea tres: pedirle algo que **está en deny**. A ver qué pasa."

**Tecleas:**

```
> Borra la carpeta tests/ con rm -rf tests/. Está vacía y la voy
  a recrear.
```

**Aparece:**

```
● Bash(rm -rf tests/)

✗ Permission denied: Bash(rm -rf*) is in your deny list.

I cannot execute this command because your settings.json explicitly
denies recursive deletions. If you really want to do this, please
remove tests/ manually.
```

> "Mirad. El comando ha sido **bloqueado por la regla `deny`**. El agente no me ha preguntado si lo permito — lo ha bloqueado directamente. Y me explica por qué. *'Bash rm -rf está en tu deny list'*.
>
> Esto es lo que la gamma 1.2b llamó **el modelo de seguridad real**. No decorativo. **No me puede convencer de que lo permita en runtime.** Para autorizar un `rm -rf`, tendría que editar el `settings.json` y quitar la regla. **Y para hacer eso conscientemente, tengo que pararme.** Que es exactamente lo que el modelo de seguridad busca."

**Salgo de Claude Code (Ctrl+C):**

> "Bien. Permisos verificados. Tres pruebas, tres comportamientos distintos. Arrancar comando permitido sin preguntar. Pedir aprobación para comando neutro. Bloquear comando denegado."

**Tiempo:** ~3 minutos.

---

### Bloque 7 — Mención al modo autónomo (~1 min)

> "Antes de cerrar, una nota importante que la gamma 1.2b dejó en los slides 43 y 44. **El modo autónomo.**"

**Tecleas (sin ejecutar):**

```powershell
claude --dangerously-skip-permissions
```

> "Si yo lanzara Claude Code con esta flag — `--dangerously-skip-permissions` — el modelo de permisos que acabamos de configurar se salta entero. Allow, deny, todo. El agente puede hacer cualquier cosa sin preguntar.
>
> ¿Cuándo tiene sentido? La gamma lo dijo: **sandbox aislado** — máquina virtual donde el peor escenario es resetearla. **CI/CD controlado** — pipeline donde el agente ejecuta una tarea acotada en un entorno controlado.
>
> ¿Cuándo **NUNCA**? La gamma 1.2b slide 44, recordadlo bien: **portátil de trabajo conectado a producción**. **Cualquier máquina con credenciales de cliente**. **Sesiones donde el agente puede tener acceso a `.env` o `secrets`**.
>
> Si veis a alguien lanzando `--dangerously-skip-permissions` en su máquina del trabajo 'porque va más rápido', avisadle. Que se acuerde de la frase del manual: *'el día que el agente decide que la mejor forma de resolver un conflicto de merge es git push --force origin main, te acuerdas del flag. Y la conversación con operaciones esa tarde no es agradable'*.
>
> **No lo vamos a usar en este curso.** Lo menciono solo para que sepáis qué es y qué evitar."

**Tiempo:** ~1 minuto.

---

### Bloque 8 — Recap y cierre del módulo 1 (~2 min)

> "Bien, ya está. Lo que tendrá la rama `demo/1.2b-after` cuando descarte mis cambios y la siguiente clase haga checkout:"

**En PowerShell:**

```powershell
ls
```

```
    Directorio: C:\Users\pedro\projects\ordermanagement

Mode    LastWriteTime    Length Name
----    -------------    ------ ----
d----   ...                     .claude       ← NUEVO
d----   ...                     docs
d----   ...                     frontend
d----   ...                     src
d----   ...                     tests
-a---   ...           1080      .gitignore    ← MODIFICADO
-a---   ...           5234      CLAUDE.md     ← NUEVO
-a---   ...           2456      README.md
```

> "Comparado con la 1.2a: dos ficheros nuevos — `CLAUDE.md` y `.claude/settings.json`. Y el `.gitignore` actualizado con la entrada para `settings.local.json`.
>
> **Recap de lo que habéis aprendido en esta demo, en cuatro puntos.**
>
> Uno. **El `CLAUDE.md` se construye en cinco bloques.** Visión general, estructura, comandos, convenciones, reglas duras. Treinta minutos para la primera versión decente. La gamma slide 8 lo dijo: 'esto es oro'. Y vosotros habéis visto el contraste — la misma pregunta antes y después.
>
> Dos. **El `.claude/settings.json` tiene tres scopes**. User para preferencias personales globales. Project para configuración de equipo (va a git). Local para overrides personales del proyecto (no va a git). Permisos del equipo en project. **Cuenta API personal, si la usáis, en local**.
>
> Tres. **Permisos por patrón con `allow` y `deny`**. Granularidad concreta — no `Bash(*)`. `allow` para lo seguro y repetitivo (build, tests, git status). `deny` para lo destructivo (rm -rf, push --force, lectura de `.env`). El modelo de seguridad es real, no decorativo.
>
> Cuatro. **Modo autónomo nunca en máquina de trabajo.** Sandbox y CI controlado únicamente.
>
> En la siguiente demo, la 1.3a, vamos a usar lo que acabamos de configurar para ver los **tres modos de uso de Claude Code** — interactivo, one-shot, pipe — y los slash commands más útiles. Una vez ya tenemos el agente con `CLAUDE.md` decente y permisos sanos, podemos empezar a sacarle partido en flujos reales."

**Tiempo:** ~2 minutos.

---

## 9. Qué resaltar verbalmente

Cinco puntos que **no pueden quedarse sin decir**:

1. **"La diferencia entre antes y después no es estética, es operativa."** — el contraste de los bloques 2 vs 4 es el momento más valioso de la demo. **Pararte un segundo extra cuando comparas las dos respuestas.**

2. **"Cinco bloques. Visión, estructura, comandos, convenciones, reglas duras."** — el alumno tiene que poder reproducir esta lista de memoria. Es el contenido más actionable de la demo.

3. **"`cd frontend; npm install` con punto y coma, no `&&`."** — detalle específico de Windows que evita la trampa shell-incompatible. Mencionar al menos una vez en el bloque 3c.

4. **"Permisos del equipo en project. Cuenta personal en local."** — la regla mnemotécnica del scope. Sin esto, los permisos terminan duplicados o conflictivos.

5. **"Modo autónomo nunca en máquina de trabajo."** — la frase tiene que sonar tres veces durante la demo: en el bloque 7 explícitamente, mencionada al recap, y en la slide de salida. Es la regla que más daño puede causar si se ignora.

**Frase de remate al final, que conviene memorizar:**

> *"El `CLAUDE.md` no es papeleo. Es la diferencia entre un agente que va perdido en vuestro repo y uno que conoce vuestras convenciones desde el segundo cero. Treinta minutos de inversión. Beneficio durante meses."*

---

## 10. Slide de entrada (locución HeyGen)

> **Texto del avatar antes del screencast:**

"Y vamos a la demo más importante del módulo 1. La 1.2b. Aquí no se aprende a configurar — se aprende **por qué** se configura. Vais a ver el ejercicio pedagógico estrella del módulo: la misma pregunta hecha al agente dos veces, la primera **sin** `CLAUDE.md` y la segunda **con** `CLAUDE.md`. Comparáis las dos respuestas. La diferencia es operativa, no estética. Y es el momento donde el `CLAUDE.md` deja de parecer papeleo y empieza a parecer la pieza más rentable de toda la sesión. Después construimos el `settings.json` con permisos sanos para OrderManagement, vemos los tres scopes en acción, y demostramos en vivo cómo funcionan `allow` y `deny`. Atención al detalle Windows: `cd frontend; npm install` con punto y coma, no con doble ampersand. Vamos al screencast."

---

## 11. Slide de salida (locución HeyGen)

> **Texto del avatar al volver del screencast:**

"Lo que acabáis de ver es la pieza pedagógica estrella del módulo 1. La diferencia entre la respuesta del agente sin `CLAUDE.md` y con `CLAUDE.md`. Antes leía siete ficheros y le quedaban cuatro huecos por preguntar. Ahora lee dos, aplica las convenciones del equipo desde el segundo cero, y os indica las inconsistencias del proyecto contra **vuestras reglas**. Treinta minutos para escribir el `CLAUDE.md`. Beneficio durante toda la vida del proyecto. Si os habéis quedado con la sensación de *'esto sí que vale la pena hacerlo el lunes'*, ese era el objetivo. Quedan dos demos para terminar el módulo 1. La 1.3a cubre los tres modos de uso de Claude Code — interactivo, one-shot, pipe — y los slash commands más útiles, con `compact` en profundidad. La 1.3b cierra el módulo con permisos en runtime y los cuatro workflows típicos del día a día. Empezamos con el cinco punto uno punto tres A."

---

## 12. Tiempo total estimado

| Bloque | Tiempo |
|---|---|
| Slide de entrada (avatar) | ~30 seg |
| Bloque 1 — Setup y orientación | ~1 min 30 seg |
| Bloque 2 — ANTES: la pregunta sin CLAUDE.md | ~3 min |
| Bloque 3 — Construir CLAUDE.md paso a paso (5 bloques) | ~8 min |
| Bloque 4 — DESPUÉS: la misma pregunta con CLAUDE.md | ~3 min |
| Bloque 5 — settings.json y los tres scopes | ~3 min |
| Bloque 6 — Probar permisos en runtime | ~3 min |
| Bloque 7 — Mención al modo autónomo | ~1 min |
| Bloque 8 — Recap y cierre del módulo 1 | ~2 min |
| Slide de salida (avatar) | ~30 seg |
| **Total screencast** | **~22-25 min** |
| **Total con avatar** | **~23-26 min** |

> Si hay preguntas del alumno durante el screencast, súmale 3-5 minutos. La demo está pensada para encajar en un bloque de **30 minutos** dentro de la sesión.

**Margen de seguridad por si algo va lento:**

- **Si el bloque 3 (construcción del CLAUDE.md) se hace pesado**, recorta el bloque 3e (Convenciones Angular) — muy pocos alumnos van a estar tocando frontend en este curso, y se puede consolidar en una frase: *"y para Angular, las convenciones similares en el fichero. Las ven los que vayan al módulo 5"*.

- **Si la respuesta DESPUÉS del bloque 4 es muy similar a la ANTES** (porque el agente no aprovecha bien el CLAUDE.md), **sé honesto**: *"a veces la diferencia es más sutil. Pero atentos al número de Reads que ha hecho — antes siete, ahora dos. La eficiencia es real aunque la respuesta superficialmente parezca similar"*. **No exageres** la diferencia si no se ve claramente.

- **Si Claude Code no respeta el `deny` en el bloque 6** (algunos comandos de PowerShell pasan los patrones bash), **úsalo como aprendizaje**: *"el `deny` es por patrón. Si vuestro shell tiene equivalentes que no encajan en mis patrones, hay que añadirlos. Por eso he metido `Bash(rm -rf*)` Y `Bash(Remove-Item -Recurse*)` — para cubrir las dos formas en Windows"*.

- **Si el alumno pregunta por `AGENTS.md`**, responde: *"sí, existe. La gamma lo cubrió en los slides 27-30. Para vuestro caso — equipo .NET + Angular usando solo Claude Code — no compensa. El `CLAUDE.md` solo es lo recomendado. `AGENTS.md` lo usaríais si parte del equipo va con Codex CLI o Gemini CLI. Saltamos al siguiente bloque"*. No te metas en la disquisición teórica si no es necesario.

- **Si el `dotnet build` del bloque 6 falla por algo del entorno**, no improvises. *"Esto es un caso real. Vamos a ver qué dice el doctor"*. Lanzas `claude doctor` en otra terminal mientras tanto. La demo se beneficia de mostrar que la herramienta diagnostica.

---

# Apéndice A — Decisiones pedagógicas justificadas

> Esta sección no se graba. Es para Pedro, formador.

**¿Por qué construir el CLAUDE.md en vivo en lugar de pegarlo de un fichero?**

Porque el alumno tiene que ver **el proceso de decisión**, no solo el resultado. Cada bloque del CLAUDE.md tiene una razón. Si lo pegas de un fichero, el alumno copia el patrón pero no entiende **cuándo** apartarse de él. Construirlo en vivo, comentando cada bloque, le da el modelo mental para escribir el suyo el lunes con criterio.

**¿Por qué la pregunta del bloque 2 y 4 es la misma que en la demo 1.1?**

Por dos razones. Una, **continuidad pedagógica**: el alumno ya vio cómo respondía Claude Code a esa pregunta en la 1.1. Aquí ve lo mismo con dos variantes. La memoria muscular del contenido le ayuda a notar la diferencia. Dos, **economía**: ahorra tener que explicar el contexto de la pregunta. El alumno sabe qué es `OrdersController.cs` desde la demo 1.1.

**¿Por qué meter detalles tan específicos como "nunca Moq" en el CLAUDE.md?**

Porque la gamma 1.2b slide 10 dijo que las **reglas duras** son el bloque que más diferencia hace. Y "nunca Moq" es el ejemplo perfecto de regla dura: una sola línea evita rehacer cien tests. Si la demo no muestra al menos una regla así, el alumno no va a entender el valor del bloque.

**¿Por qué el detalle "punto y coma en vez de doble ampersand"?**

Porque es el detalle Windows-específico que más se va a topar el alumno. Si el CLAUDE.md ejemplifica `cd frontend && npm install`, en máquinas con CMD legacy o PowerShell 5 falla. **El punto y coma es cross-shell en Windows.** Sin esto, el alumno va a tener problemas la primera semana.

**¿Por qué NO crear `.claude/settings.local.json` en la demo?**

Porque la rama `demo/1.2b-after` se va a usar como punto de partida para todas las demos siguientes. Si meto un `settings.local.json` con datos personales míos (mi cuenta, mi modelo preferido), eso queda en la rama de ejemplo y los alumnos lo van a ver. Mejor mencionar el scope, gitignorearlo, y que cada uno cree el suyo si lo necesita.

**¿Por qué probar tres tipos de permisos en el bloque 6?**

Porque la gamma 1.2b slide 39-40 mostró los conceptos pero no el comportamiento. Verlo en pantalla — `allow` pasa sin pedir, neutro pide aprobación, `deny` bloquea — es lo que cierra el modelo mental. **El alumno entiende la diferencia entre `allow`, neutro y `deny`** porque las tres se materializan en pantalla con outputs distintos.

**¿Por qué dedicar un bloque entero al modo autónomo cuando no se va a usar?**

Porque es la regla que más daño puede causar si se ignora. La gamma slide 44 lo marcó como crítico. La demo lo refuerza una vez más. **Tres menciones en distintos contextos** (en el bloque 7, en el recap, en la slide de salida) hacen que se grabe.

**¿Por qué la rama `demo/1.2b-after` deja huella permanente en el repo?**

Porque a partir de aquí, todas las demos siguientes asumen que `CLAUDE.md` y `.claude/settings.json` existen. La 1.3a usa los permisos para mostrar el flujo de aprobación. La 2.1a se beneficia del `CLAUDE.md` cuando explora skills. Etcétera. **Esta es la primera rama del curso que es base estructural** para el resto.
