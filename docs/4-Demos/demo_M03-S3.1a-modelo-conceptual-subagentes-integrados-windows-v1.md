# Demo 3.1a — Modelo conceptual y subagentes integrados: Explore, Plan, general-purpose

> **Versión:** v1 | **Módulo:** 3 | **Sub:** 3.1a | **Estado:** ✅ Versión final
> **Archivo:** `demo_M03-S3.1a-modelo-conceptual-subagentes-integrados-windows-v1.md`
> **Branch destino:** `demo/3.1a`
> **Branch de partida:** `demo/2.3`
> **Tiempo total estimado:** ~24-28 minutos
> **Tipo:** Demo de exploración y showcase. **Es la primera demo del curso donde el alumno ve subagentes funcionando — el problema que resuelven (aislamiento de contexto), el frame del agent harness, y los tres built-in en acción.** Aún no construimos un subagente propio (eso es 3.1b). Aquí sembramos el modelo conceptual con casos reales sobre OrderManagement.
> **Plataforma:** Windows 11 (PowerShell 7).

---

## 1. Contexto

Cerramos el módulo 2 con cuatro skills coexistiendo en el repo y una pregunta abierta: *"¿qué tarea de vuestro día a día necesita un agente con su propio contexto, separado del vuestro principal?"*. Esa pregunta es la entrada al módulo 3.

La gamma 3.1a (30 slides, ~30 min) cubrió cuatro piezas conceptuales:

1. **El problema que resuelven los subagentes** — la contaminación del contexto principal cuando una sesión larga acumula exploraciones que no eran del scope original.
2. **El frame del agent harness** — `agent = model + harness`. Cuando personalizamos Claude Code con skills, subagentes y hooks, **estamos construyendo nuestro propio harness encima del de Anthropic**.
3. **El mental model**: `CLAUDE.md` = persistente, skills = bajo demanda, subagentes = aislados.
4. **Los tres built-in**: Explore (lectura/exploración con Haiku), Plan (planificación), general-purpose (comodín).

Esta demo aterriza la teoría con cuatro showcases en directo sobre el repo OrderManagement:

- Una sesión donde **provocamos la contaminación de contexto** para que el alumno la vea ocurrir (recreación del caso típico del manual).
- Lanzamos **Explore explícitamente** para mapear un módulo del repo sin contaminar.
- Lanzamos **`/plan`** para diseñar un refactor antes de actuar.
- Comparamos los **dos comportamientos** uno al lado del otro.

> **Tipo de demo:** showcase educativo. La rama `demo/3.1a` queda con un fichero `docs/subagentes-explorados.md` con notas del experimento y los hallazgos de la auto-delegación. **NO crea ningún subagente propio** — eso es 3.1b. Aquí solo trabajamos con los integrados.

---

## 2. Objetivo de la demo

Cinco cosas concretas que tienen que quedar en la cabeza del alumno cuando termine los ~24 minutos de screencast:

1. **El problema que resuelve un subagente: aislamiento de contexto.** El alumno lo ve en pantalla — la sesión sin subagente acaba con 30 ficheros cargados que no eran relevantes. La sesión que delega a Explore termina con el contexto principal limpio. **La diferencia es operativa, no teórica**.

2. **El frame del agent harness: `agent = model + harness`.** El alumno tiene que entender que cuando personaliza Claude Code está construyendo su propio harness — skills (módulo 2), subagentes (3.1, 3.2), hooks (3.3). **Esto vertebra todo el módulo 3**.

3. **El mental model en tres piezas.** `CLAUDE.md` = persistente, skills = bajo demanda en el contexto principal, subagentes = aislados con su propio contexto. **La analogía de la oficina** del manual línea 38 anclada con un caso concreto.

4. **Los tres built-in y cuándo se activa cada uno.** Explore para lectura/exploración. Plan para planificación. General-purpose para tareas mixtas. **El alumno los ve en directo y entiende cuándo invocar cada uno**.

5. **La auto-delegación NO es perfecta.** El alumno tiene que aceptar que a veces el agente principal **no delega cuando debería** — y que la solución es **invocarlos explícitamente**. Sin esto, va a quedar frustrado cuando vea su sesión cargada con exploraciones que deberían haberse aislado.

Y dos cosas que tienen que **NO quedar** en su cabeza:

- *"Subagente para todo."* — no, **subagente para tareas que se benefician de aislamiento**. Si la tarea está en el flujo natural del trabajo, hazla directo. La 3.1b cubrirá los casos.
- *"Subagentes son lo mismo que skills."* — no. **Skills definen cómo se hace una tarea. Subagentes son agentes con criterio propio**. Ambos pueden coexistir y combinarse (3.2).

---

## 3. Branch de partida

```
demo/2.3
```

> Estado actual: el repo con cuatro skills (`angular-component`, `commit-style`, `db-reset`, `frontend-design`), `CLAUDE.md` con las convenciones del equipo, `settings.json` operativo, y los componentes `OrderSummary` y `OrderFilter` generados como prueba.

---

## 4. Branch destino

```
demo/3.1a
```

> Tras la demo, la rama `demo/3.1a` añade dos cosas mínimas: la marca `[x]` en `docs/DEMOS.md` y un fichero `docs/subagentes-explorados.md` con los hallazgos del experimento de contaminación de contexto y las observaciones de los tres built-in. **No se crea ningún `.claude/agents/`** — eso empieza en la 3.1b.

---

## 5. Estado del repo al empezar

Idéntico a `demo/2.3`:

```
ordermanagement/
├── .claude/
│   ├── settings.json
│   └── skills/
│       ├── angular-component/
│       ├── commit-style/
│       ├── db-reset/
│       └── frontend-design/
├── docs/
│   ├── DEMOS.md
│   ├── skills-explorados.md
│   └── auditoria-skills-comunidad.md
├── scripts/
├── src/
│   ├── OrderManagement.Api/
│   ├── OrderManagement.Application/
│   ├── OrderManagement.Domain/
│   └── OrderManagement.Infrastructure/
├── frontend/
├── tests/
├── .gitignore
├── CLAUDE.md
└── README.md
```

**Estado clave para la demo:**

- **No hay `.claude/agents/`** todavía. Solo trabajamos con los **integrados** (Explore, Plan, general-purpose) que vienen con Claude Code.
- El proyecto OrderManagement tiene varios módulos para explorar — `OrderManagement.Domain` (entidades), `OrderManagement.Application` (handlers), `OrderManagement.Infrastructure` (repos EF Core in-memory). Cualquiera sirve como "módulo desconocido" para el caso típico.

**Estado de la máquina Windows del formador:**

```
✅ Claude Code v2.1.x con subagentes built-in disponibles
✅ Git for Windows
✅ PowerShell 7
✅ VS Code con el repo cargado en demo/3.1a
✅ CLAUDE.md y settings.json operativos
✅ Cuatro skills cargables desde .claude/skills/
```

**Lo que el alumno verá al final de la demo:**

- Una sesión "antes" donde el agente principal explora 15+ ficheros para responder una pregunta sobre el módulo `Infrastructure` y termina con el contexto cargado.
- La misma pregunta resuelta delegando a Explore: el principal recibe un resumen estructurado, **el contexto principal se queda limpio**.
- `/plan` invocado para diseñar un refactor de los handlers — produce un plan estructurado sin escribir nada.
- El truco para verificar qué se delegó: preguntar al agente *"¿qué subagentes has usado?"*.
- El fichero `docs/subagentes-explorados.md` con los hallazgos para repaso.

---

## 6. Prompt para Claude Code

> Lo que tú, formador, copias y pegas en Claude Code para preparar la rama `demo/3.1a` antes de grabar.

````
Estoy preparando la demo 3.1a del curso de Claude Code para devs .NET +
Angular. Esta demo es la primera del módulo 3 — modelo conceptual de
subagentes y los tres built-in (Explore, Plan, general-purpose).
Hacemos un experimento de contaminación de contexto y comparamos con
delegación a Explore.

# Contexto

Estoy en la rama `demo/2.3` del repo `ordermanagement`. El módulo 2 está
cerrado con cuatro skills funcionales. NO hay ningún .claude/agents/
todavía.

Quiero que prepares la rama demo/3.1a con un cambio mínimo: marcar la
demo en docs/DEMOS.md y dejar preparado docs/subagentes-explorados.md
con la estructura para que yo rellene los hallazgos en vivo durante el
screencast.

# Lo que necesito

Tres tareas:

## Tarea 1: crear la rama

```powershell
git checkout demo/2.3
git pull
git checkout -b demo/3.1a
```

## Tarea 2: actualizar docs/DEMOS.md

Localiza la línea:

```
- [ ] demo/3.1a — Modelo conceptual de subagentes integrados
```

Cámbiala por:

```
- [x] **demo/3.1a** — Subagentes integrados: Explore, Plan, general-purpose
```

## Tarea 3: crear docs/subagentes-explorados.md

Contenido:

```markdown
# Subagentes — notas de exploración

Notas de los experimentos con los subagentes integrados que vienen con
Claude Code. Sirven como referencia para escribir subagentes propios a
partir de la demo 3.1b.

## Los tres built-in

### Explore

- **Para qué**: lectura y exploración. Solo lee, no modifica.
- **Modelo**: Haiku por defecto (rápido y barato).
- **Cuándo activa**: cuando una tarea principal requiere entender una
  zona del repo que no es la que estás tocando.

### Plan

- **Para qué**: planifica antes de actuar. No escribe código.
- **Cuándo se usa**: tareas que tocan más de tres ficheros, decisiones
  de diseño implícitas, o donde un error a mitad sería costoso.
- **Invocación**: `/plan` o automáticamente cuando el agente principal
  detecta complejidad.

### General-purpose

- **Para qué**: el comodín. Puede explorar Y modificar.
- **Cuándo se usa**: cuando una tarea requiere ambas pero el principal
  quiere mantener su contexto limpio.

## Hallazgos del experimento de contaminación

(Esta sección la rellena Pedro durante el screencast con los datos
reales que obtenga.)

### Sin delegación (sesión "antes")

- Pregunta: …
- Ficheros leídos por el principal: …
- Estado del contexto al final: …

### Con delegación a Explore (sesión "después")

- Misma pregunta: …
- Ficheros leídos por el principal: …
- Resumen recibido del subagente Explore: …
- Estado del contexto al final: …

## Lecciones extraídas

1. **El problema que resuelve el subagente es operativo, no teórico**:
   sin delegación, X ficheros pesan en el contexto. Con delegación,
   solo el resumen.
2. **Auto-delegación no es perfecta**: a veces el principal no delega
   cuando debería. Solución: invocar explícitamente.
3. **Cada built-in tiene su nicho**: Explore para entender, Plan para
   diseñar, general-purpose para tareas mixtas.

## Próximo paso

En la demo 3.1b vamos a crear nuestro primer subagente custom: un
`repo-explorer` para OrderManagement con su propio rol y su propio
system prompt.
```

## Tarea 4: verificar y commitear

```powershell
dotnet build
```

Esperado: 0 warnings, 0 errors.

```powershell
git add docs/DEMOS.md docs/subagentes-explorados.md
git commit -m "demo/3.1a: marca demo y notas pre-grabación de subagentes built-in"
```

NO hagas push.

# Restricciones (importantes)

- NO crees `.claude/agents/`. Esta demo es exploración de los built-in,
  no creación. La creación de subagentes propios empieza en 3.1b.
- NO modifiques los skills existentes ni CLAUDE.md ni settings.json.
- NO toques el código de la app.

# Cuando termines, dime

1. Que la rama demo/3.1a está creada desde demo/2.3.
2. Que docs/DEMOS.md tiene 3.1a marcada.
3. Que docs/subagentes-explorados.md está creado.
4. Que el build pasa.
5. Que el commit está hecho.

Si tienes dudas, para y pregúntame.
````

---

## 7. Artefactos que Claude Code debe generar

```
✓ Rama nueva: demo/3.1a (parte de demo/2.3)
✓ docs/DEMOS.md con 3.1a marcada como [x]
✓ docs/subagentes-explorados.md con la estructura y los hallazgos por rellenar en vivo
✓ Verificación de build OK: dotnet build limpio
✓ Commit único pre-grabación: "demo/3.1a: marca demo y notas pre-grabación de subagentes built-in"
```

**Lo que NO debe haber generado:**

- ❌ Ningún `.claude/agents/` (la creación es 3.1b)
- ❌ Ningún subagente custom
- ❌ Cambios en código de la app
- ❌ Cambios en skills, CLAUDE.md o settings.json

> Si Claude Code se anticipa y crea un subagente, **se rechaza el output**. La 3.1a es deliberadamente "ver, no crear".

**Lo que el formador commitea EN VIVO durante el screencast:**

```
Después de la grabación, la rama tendrá un commit adicional:
- "demo/3.1a: hallazgos del experimento de contaminación de contexto"
  └── docs/subagentes-explorados.md (sección "Hallazgos" rellenada)
```

**Estado final del árbol después del screencast (no del prompt):**

```
ordermanagement/
├── .claude/
│   ├── settings.json
│   └── skills/                                 (sin cambios)
├── docs/
│   ├── DEMOS.md                                ← MODIFICADO (pre-grabación)
│   ├── skills-explorados.md                    (sin cambios)
│   ├── auditoria-skills-comunidad.md           (sin cambios)
│   └── subagentes-explorados.md                ← NUEVO (pre-grabación + en vivo)
└── ... (resto sin cambios)
```

---

## 8. Guion del screencast

**Duración estimada del screencast: ~22-26 minutos.**

Diez bloques. La demo materializa los conceptos de la gamma 3.1a con dos experimentos comparativos sobre OrderManagement.

> **Antes de empezar a grabar**, asegúrate de:
> - Tener PowerShell 7 abierto en pantalla completa, fuente al menos 16pt.
> - Tener VS Code abierto con el repo en `demo/3.1a`.
> - Cerrar Slack, Teams, navegadores con notificaciones.
> - **Verificar** que la versión de Claude Code que uses tenga los subagentes built-in disponibles. Lánzalo y comprueba que `/plan` responde y que aparecen mensajes tipo "Launching Explore agent..." en sesiones largas.
> - **Plan B**: si los built-in no están disponibles en tu versión por alguna razón, comenta al alumno: *"a veces los built-in no aparecen anunciados explícitamente — se ven en la práctica cuando el agente decide delegarles"*. La pedagogía conceptual no se cae.

---

### Bloque 1 — Setup y la pregunta del módulo 2 (~1 min 30 seg)

**Pantalla compartida.** A la izquierda VS Code con el repo en `demo/3.1a`. A la derecha terminal PowerShell.

**En la terminal:**

```powershell
git status
git log --oneline -3
```

```
On branch demo/3.1a
nothing to commit, working tree clean

abc1234 (HEAD -> demo/3.1a) demo/3.1a: marca demo y notas pre-grabación de subagentes built-in
xyz9876 (demo/2.3) demo/2.3: instala frontend-design oficial + nota de cierre del módulo 2
def5678 (demo/2.2c) demo/2.2c: skills db-reset y commit-style + notas operativas
```

**Lo que dices:**

> "Estamos en la rama `demo/3.1a`. **Primera demo del módulo 3.** Cerramos el módulo 2 con una pregunta abierta: *'¿qué tarea de vuestro día a día necesita un agente con su propio contexto, separado del vuestro principal?'*. Esa pregunta es la entrada a este módulo.
>
> En estos minutos vamos a ver dos cosas. Primero, **el problema que resuelven los subagentes** — y lo vamos a ver en directo provocando la contaminación de contexto que la gamma 3.1a slide 2 describió. Segundo, **los tres subagentes built-in** que vienen con Claude Code: Explore, Plan, general-purpose. Cómo funcionan, cuándo se activan, cuándo invocarlos explícitamente.
>
> Y antes de tocar nada, **el frame que vertebra el módulo 3**. Lo vemos primero porque es lo que da sentido a todo lo que viene."

**Tiempo:** ~90 segundos.

---

### Bloque 2 — El frame: agent = model + harness (~2 min)

> "La gamma 3.1a slide 6 lo introdujo. Hay una fórmula que vais a leer en blogs de Anthropic, en threads técnicos, en la documentación oficial:"

**En el editor de texto al lado, escribo (contenido pedagógico):**

```
              agent = model + harness
              ─────────────────────────

Un MODELO en bruto no es un agente.
Lo es solo cuando le rodeas de:

  ├── Tools (Read, Write, Bash, ...)
  ├── Contexto (CLAUDE.md, system prompts, ...)
  ├── Skills (capacidades bajo demanda)
  ├── Subagentes (workers especializados)
  ├── Hooks (acciones automáticas)
  └── Feedback loops (verificación, validación)

Todo eso junto = el HARNESS.

Claude Code, Cursor, Codex, Aider —
todos son harnesses construidos sobre el modelo.


LA IDEA CLAVE:

Cuando personalizas Claude Code con
  → skills (módulo 2)
  → subagentes (módulo 3.1, 3.2)
  → hooks (módulo 3.3)

Estás CONSTRUYENDO TU PROPIO HARNESS
encima del de Anthropic.
```

> "**Idea clave**. Cuando vosotros personalizáis Claude Code, no estáis "configurando una herramienta". Estáis **construyendo vuestro propio harness encima del de Anthropic**. Vuestro harness sabe a vuestro equipo. Sabe vuestras convenciones (skills, módulo 2). Sabe delegar a workers especializados (subagentes, módulo 3.1 y 3.2). Sabe hacer cosas automáticamente (hooks, módulo 3.3).
>
> Esta es la idea que vertebra el módulo 3 entero. **No estamos aprendiendo features sueltas — estamos montando el harness**.
>
> Y dentro del harness, los **subagentes son los workers**. Vamos a ver qué problema resuelven con un experimento."

**Tiempo:** ~2 minutos.

---

### Bloque 3 — El experimento de contaminación: provocar el problema (~3 min 30 seg)

> "La gamma 3.1a slide 3 describió el caso típico: alguien implementa una feature compleja y a mitad necesita entender un módulo del repo que no ha tocado. **Vamos a recrearlo en directo**. Lanzo Claude Code y le pido que entienda el módulo `Infrastructure` que tiene varios repositorios EF Core in-memory."

**En la terminal:**

```powershell
claude
```

```
✓ 4 project skills loaded: angular-component, commit-style, db-reset, frontend-design
```

> "Lanzo la sesión y le hago la pregunta de exploración:"

**Tecleo:**

```
> Necesito entender bien cómo está estructurado el módulo
  OrderManagement.Infrastructure. Qué repositorios hay, qué patrones
  usan, cómo se inyectan en la API. Hazme un mapeo completo.
```

**Aparecen las herramientas — secuencia larga:**

```
● Glob(src/OrderManagement.Infrastructure/**/*.cs)
● Read(src/OrderManagement.Infrastructure/Persistence/OrderManagementContext.cs)
● Read(src/OrderManagement.Infrastructure/Persistence/Configurations/OrderConfiguration.cs)
● Read(src/OrderManagement.Infrastructure/Persistence/Configurations/CustomerConfiguration.cs)
● Read(src/OrderManagement.Infrastructure/Persistence/Configurations/OrderItemConfiguration.cs)
● Read(src/OrderManagement.Infrastructure/Repositories/OrderRepository.cs)
● Read(src/OrderManagement.Infrastructure/Repositories/CustomerRepository.cs)
● Read(src/OrderManagement.Infrastructure/Repositories/OrderItemRepository.cs)
● Read(src/OrderManagement.Infrastructure/DependencyInjection.cs)
● Read(src/OrderManagement.Api/Program.cs)
● Read(src/OrderManagement.Application/Interfaces/IOrderRepository.cs)
● Read(src/OrderManagement.Application/Interfaces/ICustomerRepository.cs)
● Glob(src/OrderManagement.Application/Handlers/*.cs)
● Read(src/OrderManagement.Application/Handlers/CreateOrderHandler.cs)
● Read(src/OrderManagement.Application/Handlers/UpdateOrderHandler.cs)
... (~15 reads)
```

**Aparece la respuesta:**

```
He mapeado el módulo OrderManagement.Infrastructure:

[Resumen detallado de 15-20 líneas con la estructura, patrones de
repositorios, inyección de dependencias, etc.]
```

> "**Mirad las herramientas.** El agente principal ha leído **quince ficheros aproximadamente**. Tres configuraciones EF Core, tres repositorios, el `Program.cs`, las interfaces de la capa de aplicación, varios handlers. Todo eso **está ahora en el contexto principal**.
>
> Y mirad la respuesta — es buena, técnicamente correcta. Pero **el coste**: si ahora cambio de tema y le pido que implemente una feature en otro lugar del proyecto, **arrastra estos quince ficheros** en su contexto. La gamma 3.1a slide 2 lo dijo: *'la ventana de contexto se está llenando, y cada nueva exploración añade ruido al razonamiento principal'*.
>
> Vamos a confirmarlo. Le pregunto qué tiene en contexto:"

**Tecleo:**

```
> ¿Qué ficheros tienes cargados en tu contexto ahora mismo?
```

**Aparece:**

```
He cargado los siguientes ficheros del módulo Infrastructure durante
mi exploración:

- OrderManagementContext.cs
- OrderConfiguration.cs
- CustomerConfiguration.cs
- OrderItemConfiguration.cs
- OrderRepository.cs
- CustomerRepository.cs
- OrderItemRepository.cs
- DependencyInjection.cs
- Program.cs (de la API)
- IOrderRepository.cs, ICustomerRepository.cs
- CreateOrderHandler.cs, UpdateOrderHandler.cs

Más el CLAUDE.md, settings.json y los SKILL.md de los skills cargados.
```

> "**Trece ficheros del módulo Infrastructure cargados**. Más todo lo que ya estaba (CLAUDE.md, los skills). Esto es **la contaminación**. Ahora si quiero hacer otra cosa — pongamos modificar el frontend Angular — el agente arrastra todo este peso en su razonamiento, **aunque no sea relevante**.
>
> Salimos. Vamos a hacer el mismo experimento delegando a Explore."

**Salgo (Ctrl+C):**

**Tiempo:** ~3 minutos 30 segundos.

---

### Bloque 4 — La misma pregunta delegando a Explore (~3 min)

> "Misma pregunta. Pero esta vez **invoco explícitamente Explore**. La gamma 3.1a slide 21 lo dijo: si quiero ser explícito, lo pido."

**Arranco una sesión limpia:**

```powershell
claude
```

```
✓ 4 project skills loaded: angular-component, commit-style, db-reset, frontend-design
```

**Tecleo:**

```
> Usa el subagente Explore para mapear el módulo
  OrderManagement.Infrastructure. Quiero saber qué repositorios hay,
  qué patrones usan, y cómo se inyectan en la API. Devuelve un
  resumen estructurado, no me cargues los ficheros completos.
```

**Aparece — atención a la diferencia:**

```
● Launching Explore subagent to investigate OrderManagement.Infrastructure...

  [Subagent runs in isolated context with Haiku model]

● Explore subagent returned:

  ESTRUCTURA DEL MÓDULO Infrastructure
  ────────────────────────────────────
  
  Capa de persistencia:
  - OrderManagementContext: DbContext con DbSets de Order, Customer, OrderItem
  - 3 configuraciones EF Core (una por entidad) con Fluent API
  
  Repositorios (patrón Repository + UoW implícito):
  - OrderRepository implementa IOrderRepository (de la capa Application)
  - CustomerRepository implementa ICustomerRepository
  - OrderItemRepository implementa IOrderItemRepository
  - Todos usan inyección del DbContext y exponen async/await
  
  Inyección de dependencias:
  - DependencyInjection.cs: extension method AddInfrastructure
  - Registra DbContext con UseInMemoryDatabase
  - Registra los 3 repositorios como Scoped
  
  Patrones detectados:
  - Repository por entidad (no genérico)
  - In-memory DB (sin migraciones)
  - Mapeo manual entre entidades (sin AutoMapper)
  
  Notas de cuidado:
  - 0 tests para Infrastructure todavía
  - Mock manual de repositorios para tests (cuando se hagan)
```

> "**Mirad la diferencia con la sesión anterior.**
>
> Una. **El subagente Explore ejecuta en contexto aislado**. Su propio razonamiento. Su propio modelo (Haiku, según el manual línea 70). Lee los ficheros que necesita.
>
> Dos. **Solo recibo el resumen estructurado**, no los ficheros completos. El resumen son aproximadamente quince líneas.
>
> Tres. **Mi contexto principal queda limpio**. Vamos a verificarlo:"

**Tecleo:**

```
> ¿Qué ficheros tienes cargados ahora mismo en tu contexto?
```

**Aparece:**

```
He cargado:
- CLAUDE.md
- settings.json
- Los SKILL.md de angular-component, commit-style, db-reset, frontend-design
- Un resumen estructurado del módulo Infrastructure (devuelto por Explore)

NO tengo los ficheros .cs del módulo Infrastructure en mi contexto —
los leyó Explore en su contexto aislado.
```

> "**Aquí está la diferencia operativa**. En la sesión anterior tenía trece ficheros del Infrastructure en mi contexto. Aquí, **cero**. Solo el resumen. Y el resumen me da **toda la información que necesito** para tomar decisiones.
>
> Si ahora quiero pivotar a tocar el frontend, **mi contexto está limpio**. La gamma 3.1a slide 4 lo decía: *'compactar es un parche, los subagentes son la solución'*. **Aquí lo veis materializado**."

**Salgo (Ctrl+C):**

**Tiempo:** ~3 minutos.

---

### Bloque 5 — `/plan` en directo: planificar antes de actuar (~3 min)

> "Segundo built-in: **Plan**. La gamma 3.1a slide 17 lo cubrió. Plan recopila contexto, razona sobre cómo abordar la tarea, y devuelve un plan paso a paso. **No actúa**.
>
> Vamos a usarlo para una decisión que tendría que tomar el equipo: refactorizar la capa de validación de los handlers."

**Arranco Claude Code:**

```powershell
claude
```

**Tecleo:**

```
> /plan Tengo que añadir validación a los CreateOrderCommand y
  UpdateOrderCommand de OrderManagement. Cada uno tiene reglas de
  negocio distintas. Quiero usar FluentValidation. Diséñame un plan
  para integrarlo en los handlers existentes sin romper la
  arquitectura actual.
```

**Aparece:**

```
● Plan subagent activated...

  [Plan agent reads relevant files: handlers, dependency injection,
  CLAUDE.md...]

● Plan subagent returned strategy:

  PLAN: Integración de FluentValidation en handlers de OrderManagement
  ──────────────────────────────────────────────────────────────────────
  
  Resumen
  ───────
  Añadir validación con FluentValidation a CreateOrderCommand y
  UpdateOrderCommand sin romper la arquitectura MediatR existente.
  
  Decisiones de diseño
  ────────────────────
  
  1. Pipeline behavior vs validación en handler
     → ELEGIDA: Pipeline behavior MediatR
     Razón: validación transversal, reutilizable, no contamina handlers.
  
  2. Validators en proyecto Application vs proyecto separado
     → ELEGIDA: en Application, junto a los Commands
     Razón: el equipo ya tiene esa convención en CLAUDE.md slide 3.
  
  3. Errores de validación: excepción vs Result
     → ELEGIDA: ValidationException tipada
     Razón: mantiene el patrón de excepciones tipadas que ya tenéis con
     CustomerNotFoundException, OrderNotFoundException, etc.
  
  Pasos
  ─────
  
  PASO 1: Añadir paquete NuGet
    - FluentValidation y FluentValidation.DependencyInjectionExtensions
    - dotnet add package en OrderManagement.Application
  
  PASO 2: Crear ValidationException
    - src/OrderManagement.Application/Exceptions/ValidationException.cs
    - Hereda de Exception, contiene IDictionary<string, string[]> Errors
    - Sigue el patrón de InvalidOrderStateException
  
  PASO 3: Crear pipeline behavior
    - src/OrderManagement.Application/Behaviors/ValidationBehavior.cs
    - Implementa IPipelineBehavior<TRequest, TResponse>
    - Resuelve validators con inject, ejecuta, lanza si hay errores
  
  PASO 4: Crear validators concretos
    - src/OrderManagement.Application/Commands/CreateOrderCommandValidator.cs
    - src/OrderManagement.Application/Commands/UpdateOrderCommandValidator.cs
    - Reglas: customerId no vacío, items > 0, total > 0, etc.
  
  PASO 5: Registrar en DI
    - DependencyInjection.cs de Application
    - services.AddValidatorsFromAssembly(...)
    - services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
  
  PASO 6: Manejar la ValidationException en la API
    - ExceptionHandlingMiddleware o ProblemDetails en API
    - Retornar 400 con detalles
  
  PASO 7: Tests
    - Tests unitarios de los validators (caminos felices y error)
    - Test de integración de un handler que recibe input inválido
  
  Riesgos
  ───────
  - Tests existentes pueden romperse si no pasan validación.
    Mitigación: revisar fixtures de tests y actualizar inputs.
  - Cambios en API contract si el formato de error cambia.
    Mitigación: documentar el formato 400 en OpenAPI/Swagger.
  
  Validación final
  ────────────────
  - Tests verdes
  - Endpoint con input inválido devuelve 400 con detalles
  - Endpoint con input válido sigue funcionando como antes
  
  ¿Apruebo este plan o ajusto algo?
```

> "**Mirad lo que ha hecho Plan.** Ha leído el contexto necesario, ha tomado **tres decisiones de diseño** justificadas, ha estructurado **siete pasos** ordenados, ha identificado **dos riesgos** con mitigación, y **pide confirmación antes de actuar**.
>
> **Y lo más importante**: Plan **no ha tocado nada**. Cero `Write`, cero `Edit`. **Solo Read y Grep**. Es deliberado — el manual línea 79 lo dijo: *'no actúa, devuelve el plan al agente principal'*.
>
> Si yo le digo *'ejecuta este plan'*, **el agente principal lo ejecuta**. Pero la **fase de pensar** la hizo Plan en su contexto aislado. Mi contexto principal **no se cargó con la lectura exploratoria**. Solo se cargó con el plan resultante, que son sesenta líneas estructuradas.
>
> Cancelo, no vamos a ejecutar nada en esta demo:"

**Tecleo:**

```
> No ejecutes el plan. Era solo demostración. Sal del modo plan.
```

```
Plan abortado. Volvemos a modo normal.
```

**Salgo (Ctrl+C):**

**Tiempo:** ~3 minutos.

---

### Bloque 6 — General-purpose: el comodín (~1 min 30 seg)

> "Tercer built-in: **general-purpose**. La gamma 3.1a slide 19 lo cubrió. Es el comodín — puede tanto explorar como modificar. Lo usa el agente principal cuando una tarea requiere ambas cosas pero quiere mantener su propio contexto limpio."

**En el editor de texto al lado:**

```
GENERAL-PURPOSE — el comodín

¿Cuándo se activa?

  Cuando una tarea:
  ├── Requiere LEER varios ficheros
  ├── Y MODIFICAR algún resultado
  └── Pero NO encaja en el flujo principal actual

Casos típicos:

  - Refactor de un módulo aislado
    "Refactoriza este servicio que ya está separado del resto."
  - Generación de tests para un componente existente
    "Genera la suite de tests para OrderRepository."
  - Limpiar código muerto
    "Elimina los TODOs que llevan más de seis meses."

¿Cuándo NO?

  - Si la tarea está en el flujo natural del trabajo principal
    → no delegues, hazlo directo.
  - Si solo lees → Explore basta.
  - Si solo planificas → Plan basta.
```

> "**No vamos a invocarlo en esta demo** porque las tareas de OrderManagement de hoy o son lectura pura (Explore) o son planificación (Plan). General-purpose va a salir más en la 3.1b cuando creemos subagentes propios — porque muchos subagentes propios son **especializaciones de general-purpose**.
>
> Tres built-in cubiertos: **Explore para leer/explorar. Plan para diseñar. General-purpose para tareas mixtas**. Son los que vienen de fábrica."

**Tiempo:** ~1 minuto 30 segundos.

---

### Bloque 7 — La auto-delegación NO es perfecta (~2 min)

> "Antes de cerrar, **un punto importante** que la gamma 3.1a slide 22 marcó. **La auto-delegación no es perfecta.**"

**En el editor de texto:**

```
LA AUTO-DELEGACIÓN: LIMITACIONES HONESTAS

A veces el agente principal:

  ❌ NO delega cuando debería
     - Lee 15 ficheros directamente
     - Carga el contexto principal
     - Y la tarea era claramente para Explore

  ❌ Delega cuando no debería
     - Tarea simple del flujo principal
     - Lanza un subagente innecesariamente
     - Ralentiza el flujo

LA REGLA PRÁCTICA:

  Si notas que tu sesión principal se carga con
  exploraciones que deberían haberse aislado
  
  → INVÓCALOS EXPLÍCITAMENTE.

  > Usa el subagente Explore para mapear...
  > Lanza Plan para diseñar...

Y si los ves activarse cuando no debían
(skill ya cubría el caso) → reformular la pregunta.
```

> "**Esto es honestidad operativa**. La gamma 3.1a slide 22 lo dijo: *'hay tareas donde merecería la pena delegar y no lo hace. Hay otras donde delega cuando podría haber resuelto solo'*.
>
> Lo que vimos en el bloque 3 — el agente leyendo 15 ficheros sin delegar a Explore — **es exactamente esto**. La pregunta era clara para Explore. **El agente decidió hacerlo en directo**. Es un caso típico de no-delegación cuando debería haber delegado.
>
> **La regla práctica**: si veis que vuestra sesión principal se carga con exploraciones, **no peleéis con la auto-delegación. Invocadlos explícitamente**. *'Usa el subagente Explore para X'*. *'Lanza Plan para Y'*. Es **una línea de prompt** y os ahorra mucha contaminación.
>
> Y al revés también — si veis que un built-in se activa cuando un skill ya cubría el caso, reformulad la pregunta para que el principal entienda que el skill es lo que toca."

**Tiempo:** ~2 minutos.

---

### Bloque 8 — Documentar hallazgos del experimento (~1 min 30 seg)

**Voy a VS Code y abro `docs/subagentes-explorados.md`. Edito la sección "Hallazgos del experimento de contaminación":**

```markdown
## Hallazgos del experimento de contaminación

### Sin delegación (sesión "antes")

- **Pregunta**: "Mapea el módulo OrderManagement.Infrastructure"
- **Ficheros leídos por el principal**: 13 (OrderManagementContext,
  3 configurations, 3 repositories, DependencyInjection, Program.cs,
  2 interfaces, 2 handlers)
- **Estado del contexto al final**: cargado con todo el módulo
  Infrastructure. Si pivoto a otra tarea, arrastro este peso.

### Con delegación a Explore (sesión "después")

- **Misma pregunta** invocando explícitamente "Usa el subagente Explore..."
- **Ficheros leídos por el principal**: 0
- **Resumen recibido del subagente Explore**: ~15 líneas estructuradas
  con: estructura del módulo, repositorios, DI, patrones, notas de cuidado.
- **Estado del contexto al final**: limpio. Solo el resumen.

### Diferencia operativa

| | Sin delegación | Con Explore |
|---|---|---|
| Ficheros en contexto principal | 13 | 0 |
| Información disponible | Detallada (todo) | Resumen útil |
| Coste de pivotar a otra tarea | Alto (arrastra ruido) | Cero (limpio) |
| Modelo usado | Sonnet/Opus principal | Haiku (más barato) |
```

**Salvo. En la terminal:**

```powershell
git add docs/subagentes-explorados.md
git commit -m "demo/3.1a: hallazgos del experimento de contaminación"
```

> "Commit. Los hallazgos quedan en la rama como referencia."

**Tiempo:** ~1 minuto 30 segundos.

---

### Bloque 9 — Recap del modelo conceptual (~1 min 30 seg)

> "Cinco ideas para llevarse al lunes."

**En el editor:**

```
LO QUE TIENES DEL MODELO CONCEPTUAL

1. EL FRAME
   agent = model + harness
   Cuando personalizas Claude Code, construyes tu propio harness.

2. EL PROBLEMA QUE RESUELVEN LOS SUBAGENTES
   Aislamiento de contexto.
   Sin delegación, exploraciones contaminan el principal.
   Con delegación, el principal recibe solo el resumen útil.

3. EL MENTAL MODEL
   ├── CLAUDE.md  → contexto persistente del proyecto
   ├── Skills     → playbooks bajo demanda en el principal
   └── Subagentes → tareas aisladas con su propio contexto

4. LOS TRES BUILT-IN
   ├── Explore       → lectura/exploración (Haiku)
   ├── Plan          → planifica antes de actuar
   └── General-purpose → comodín (lee Y modifica)

5. LA AUTO-DELEGACIÓN NO ES PERFECTA
   Cuando falle, invócalos explícitamente:
     "Usa el subagente Explore para X"
     "Lanza Plan para Y"
```

> "Cinco ideas. Si las tenéis claras, vais a tener el modelo conceptual para entender el resto del módulo 3."

**Tiempo:** ~1 minuto 30 segundos.

---

### Bloque 10 — Cliffhanger a la 3.1b (~1 min 30 seg)

> "En la siguiente demo, la **3.1b**, salimos de los built-in y empezamos a crear **subagentes propios**. La gamma 3.1b va a cubrir tres cosas:
>
> Una. **Anatomía de un subagente custom**. La estructura del fichero `.claude/agents/<nombre>.md` con su frontmatter (`name`, `description`, `tools`, `model`) y su system prompt.
>
> Dos. **El comando `/agents`** para gestionar subagentes desde dentro de Claude Code — crear, listar, editar, borrar.
>
> Tres. **Los cuatro casos típicos** que merece la pena que vuestro equipo tenga: Explorer, Reviewer, Tester y Planner. Vamos a crear el primero — un `repo-explorer` para OrderManagement — y a probarlo en directo.
>
> Y vais a ver el patrón potente que la gamma 3.1a slide 28 sembró: **un skill que invoca un subagente**. La combinación más rentable cuando empezáis a tener todas las piezas. Lo veremos en la 3.2a.
>
> Empezamos con el **tres punto uno punto B**."

**Tiempo:** ~1 minuto 30 segundos.

---

## 9. Qué resaltar verbalmente

Cinco puntos que **no pueden quedarse sin decir**:

1. **"Agent = model + harness. Estás construyendo tu propio harness."** — el frame que vertebra el módulo 3 entero. Bloque 2.

2. **"Trece ficheros cargados sin delegar. Cero ficheros con Explore."** — la diferencia operativa materializada en pantalla. Bloques 3-4.

3. **"Plan no actúa. Devuelve un plan estructurado y pide confirmación."** — la división de trabajo correcta. Bloque 5.

4. **"La auto-delegación no es perfecta. Cuando falle, invócalos explícitamente."** — la honestidad operativa que evita frustración. Bloque 7.

5. **"CLAUDE.md persistente, skills bajo demanda, subagentes aislados."** — el mental model en tres piezas. Bloque 9.

**Frase de remate al final:**

> *"Sin delegar, el contexto se contamina. Con Explore, el principal queda limpio. Esa es la diferencia operativa que justifica todo el módulo."*

---

## 10. Slide de entrada (locución HeyGen)

> **Texto del avatar antes del screencast:**

"Y arrancamos el módulo tres con la primera demo. La 3.1a. Vamos a ver dos cosas en directo. Primero, el problema que resuelven los subagentes — y lo provocamos en pantalla recreando el caso típico de la gamma. Una sesión sin delegar termina con trece ficheros del módulo Infrastructure cargados en el contexto principal. La misma pregunta delegando a Explore deja el contexto principal a cero, recibimos solo un resumen estructurado. Veréis la diferencia operativa con vuestros ojos. Segundo, los tres subagentes built-in que vienen con Claude Code: Explore para lectura y exploración con Haiku, Plan para diseñar antes de actuar invocado con `/plan`, y general-purpose como comodín. Y antes de tocar nada, el frame que vertebra el módulo 3 entero: cuando personalizáis Claude Code estáis construyendo vuestro propio harness encima del de Anthropic. Vamos al screencast."

---

## 11. Slide de salida (locución HeyGen)

> **Texto del avatar al volver del screencast:**

"Lo que acabáis de ver es el modelo conceptual del módulo 3 entero materializado en pantalla. Cinco ideas para el lunes. Una, agent igual a model más harness, y vosotros estáis construyendo el vuestro. Dos, los subagentes resuelven aislamiento de contexto, no es teoría, es operativo — trece ficheros versus cero. Tres, el mental model en tres piezas: CLAUDE.md persistente, skills bajo demanda en el principal, subagentes aislados con su propio contexto. Cuatro, los tres built-in: Explore para leer, Plan para diseñar, general-purpose para tareas mixtas. Cinco, la auto-delegación no es perfecta — cuando falle, invocadlos explícitamente. Una línea de prompt os ahorra mucha contaminación. En la siguiente demo, la 3.1b, salimos de los built-in y creamos vuestro primer subagente propio. Un `repo-explorer` para OrderManagement con su propio rol y su propio system prompt. Y veréis el comando `/agents` para gestionarlos sin salir de Claude Code. Empezamos con el tres punto uno punto B."

---

## 12. Tiempo total estimado

| Bloque | Tiempo |
|---|---|
| Slide de entrada (avatar) | ~30 seg |
| Bloque 1 — Setup y la pregunta del módulo 2 | ~1 min 30 seg |
| Bloque 2 — El frame: agent = model + harness | ~2 min |
| Bloque 3 — Experimento de contaminación: provocar el problema | ~3 min 30 seg |
| Bloque 4 — La misma pregunta delegando a Explore | ~3 min |
| Bloque 5 — `/plan` en directo | ~3 min |
| Bloque 6 — General-purpose: el comodín | ~1 min 30 seg |
| Bloque 7 — La auto-delegación NO es perfecta | ~2 min |
| Bloque 8 — Documentar hallazgos | ~1 min 30 seg |
| Bloque 9 — Recap del modelo conceptual | ~1 min 30 seg |
| Bloque 10 — Cliffhanger a la 3.1b | ~1 min 30 seg |
| Slide de salida (avatar) | ~30 seg |
| **Total screencast** | **~22-24 min** |
| **Total con avatar** | **~23-25 min** |

> Si hay preguntas durante el screencast, súmale 3-4 minutos. La demo encaja en un bloque de **30 minutos** dentro de la sesión.

**Margen de seguridad por si algo va lento:**

- **Si el agente principal SÍ delega automáticamente a Explore** en el bloque 3 (antes de que tú lo invoques explícitamente en bloque 4), no fuerces el guion. Comenta: *"a veces el agente delega automáticamente cuando detecta exploración. Aquí lo ha hecho. Mejor — eso confirma que la auto-delegación funciona en este caso. Pero atentos a la siguiente sesión donde vamos a ver que NO siempre lo hace, y la solución es invocar explícitamente"*. Y procede con el bloque 4 mostrando la invocación explícita.

- **Si los mensajes "Launching Explore subagent..." no aparecen** porque la versión de Claude Code los muestra distinto, comenta: *"el banner exacto puede variar entre versiones. Lo importante es lo que veis en herramientas: lecturas en contexto aislado, resumen devuelto, y mi contexto principal limpio"*.

- **Si `/plan` no responde como esperamos** y devuelve algo más improvisado, no te peles. Comenta: *"Plan a veces es más conservador o más detallado según la complejidad detectada. Lo importante es que devuelve un plan, no actúa, y espera confirmación. Esa es la división de trabajo"*.

- **Si en el bloque 3 el agente lee MENOS ficheros** de los previstos (5-7 en lugar de 15), sigue el guion como está. La pedagogía es **la diferencia entre N ficheros y cero ficheros**, no el número exacto. Adapta los números reales en la documentación del bloque 8.

- **Si te quedas sin tiempo y los bloques 6 y 9 te aprietan**, el bloque 6 (general-purpose) puedes recortarlo a 30 segundos: *"general-purpose es el comodín, lee y modifica, lo veremos más en la 3.1b cuando creemos custom"*. El recap del bloque 9 puedes recortarlo a las cinco ideas sin desarrollarlas.

---

# Apéndice A — Decisiones pedagógicas justificadas

> Esta sección no se graba. Es para Pedro, formador.

**¿Por qué empezar el módulo 3 con un experimento de contaminación en lugar de explicar subagentes directamente?**

Porque **la lección de la gamma 3.1a — "los subagentes resuelven aislamiento de contexto"— es abstracta hasta que se ve fallar primero**. Si construyera la sesión empezando por usar Explore, el alumno vería que funciona pero no entendería **por qué importa**. **Provocar la contaminación** primero materializa el problema. La diferencia 13 vs 0 ficheros **se ve, no se infiere**.

**¿Por qué `OrderManagement.Infrastructure` y no otro módulo?**

Por tres razones:
1. **Tiene tamaño suficiente** para que la exploración sin delegar cargue ~13 ficheros. Si fuera más pequeño, no se vería la contaminación.
2. **Es módulo desconocido para el alumno** que ha visto Domain y Application en demos anteriores pero no Infrastructure en detalle. **Es el caso típico del manual línea 9**: una zona del repo que no has tocado todavía.
3. **No tiene cambios de demos anteriores** — las demos del módulo 2 tocaron API y Application. Infrastructure es el mismo desde el módulo 1.

**¿Por qué el frame de "agent harness" se cubre como bloque 2 y no más adelante?**

Porque **es el marco mental que vertebra el módulo 3 entero**. Si lo dejo para más adelante, las primeras experiencias del alumno con subagentes serán "es como un skill pero distinto" — confuso. Con el frame al inicio, **subagentes encajan como una pieza del harness que está construyendo**. El resto de demos (3.1b, 3.2, 3.3) van añadiendo piezas al mismo harness.

**¿Por qué no creo un subagente propio en esta demo aunque el alumno tenga ya un Claude Code abierto?**

Porque la gamma 3.1a (30 slides) es **deliberadamente conceptual**. Los slides 5-12 son modelo mental. Los slides 13-19 son los built-in. Los slides 26-30 son **siembra de la 3.1b**. Si creara un subagente propio aquí, **invadiría la 3.1b**. La separación 3.1a (built-in + concepto) / 3.1b (custom) es la que la gamma marca.

**¿Por qué el bloque 4 (Explore) viene justo después del bloque 3 (contaminación) sin pausa?**

Porque la fuerza pedagógica está en **la comparación inmediata**. Si meto otro contenido entre medias, el alumno olvida los detalles del bloque 3 y la diferencia se diluye. **Cero ficheros vs trece ficheros** tiene que verse cuando ambos números están frescos. Comparativa back-to-back.

**¿Por qué `/plan` con FluentValidation y no con otro caso?**

Por tres razones:
1. **Es decisión que tendría que tomar el equipo de OrderManagement** — añadir validación es realista, no inventado.
2. **Tiene complejidad suficiente** para que Plan tenga material para razonar (3 decisiones de diseño, 7 pasos, 2 riesgos). Si fuera trivial, el plan sería trivial y no se vería el valor.
3. **No requiere conocimiento de Angular** ni dependencias raras. FluentValidation es estándar en .NET y el alumno lo reconoce.

**¿Por qué cancelo el plan al final del bloque 5 en lugar de ejecutarlo?**

Porque ejecutarlo añadiría 5-7 minutos de demo (siete pasos de implementación) y **no es el foco pedagógico**. El foco es **ver Plan generando el plan y esperando confirmación**. Ejecutar invadiría tiempo que necesito para los bloques 6-10.

**¿Por qué general-purpose es el bloque más corto (1m30s)?**

Porque **no es invocable explícitamente como Explore o Plan en la demo**. Para verlo en acción necesitaría un caso de refactor con lectura+escritura aislada que no encaja en este recorrido. **Tiene su sitio en la 3.1b cuando los subagentes custom sean variantes especializadas de general-purpose**.

**¿Por qué el bloque 7 (auto-delegación NO es perfecta) está antes del recap?**

Porque **es la honestidad operativa que el alumno necesita aceptar antes de cerrar**. Si el recap (bloque 9) viniera antes y luego saliera el bloque 7, el alumno se va con el último mensaje siendo "no es perfecto". Mejor: aceptar la limitación, dar la solución (invocar explícitamente), y luego el recap consolida lo aprendido en positivo.

**¿Por qué los hallazgos se rellenan EN VIVO durante el screencast?**

Porque los **números reales pueden variar** — el agente puede leer 11, 13 o 15 ficheros según humores y versión. Si pre-grabo "13", el alumno ve "11" y hay disonancia. **Honestidad operativa**: rellenar en vivo con los datos que se acaban de producir.

**¿Por qué el cliffhanger menciona "skill que invoca subagente" cuando es contenido de 3.2a?**

Porque la gamma 3.1a slide 28 lo sembró literalmente — *"casos clásicos de candidato a subagente"* incluye el patrón compuesto. **Sembrar el patrón en el cliffhanger** prepara mentalmente al alumno para que cuando llegue a 3.2a no le sorprenda. Es teaser deliberado.

**¿Por qué el alumno no ejecuta nada del plan generado?**

Porque la rama `demo/3.1a` debe quedar **limpia para 3.1b**. Si ejecutara el plan de FluentValidation, la rama tendría 7-10 ficheros nuevos relacionados con validación que invadirían las demos siguientes. La 3.1b va a tener su propio caso, no este. **Disciplina de scope por demo**.
