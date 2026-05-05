# Demo 3.1b — Subagentes custom: construir `repo-explorer` y `dotnet-reviewer` sobre OrderManagement

> **Versión:** v1 | **Módulo:** 3 | **Sub:** 3.1b | **Estado:** ✅ Versión final
> **Archivo:** `demo_M03-S3.1b-subagentes-custom-patrones-windows-v1.md`
> **Branch before:** `demo/3.1b-before`  (con `.Result` bloqueante deliberado en CancelOrderHandler para que el reviewer lo cace)
> **Branch after:**  `demo/3.1b-after`   (estado final pre-cocinado: 2 subagentes + `.Result` revertido)
> **Branch parent:** `demo/3.1a`
> **Tiempo total estimado:** ~28-32 minutos
> **Tipo:** Demo de construcción y aplicación (CÓDIGO). **Es la primera demo del curso donde el alumno ve construir subagentes propios — el `repo-explorer` (especialización del built-in Explore con rol y formato concretos para OrderManagement) y el `dotnet-reviewer` (revisor crítico con tools restringidas).** Ambos se prueban en directo. Se cubren los 4 casos típicos (Explorer, Reviewer, Tester, Planner) — los dos primeros se construyen, los otros dos se muestran como referencia. Y se aplican los anti-patrones del manual. Sigue el patrón **before/after** definido en [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md).
> **Plataforma:** Windows 11 (PowerShell 7).

---

## 1. Contexto

En la 3.1a vimos los tres built-in (Explore, Plan, general-purpose) y el modelo conceptual del agent harness. Cerramos con una pregunta sembrada por la gamma 3.1a slide 26: *"¿qué tarea de vuestro día a día sería el primer candidato a subagente para vuestro equipo?"*. Y la gamma 3.1b va a contestarla materializando los **cuatro casos típicos** que más equipos terminan teniendo.

La gamma 3.1b (32 slides, ~30 min) cubrió:

1. **Anatomía completa del subagente custom** — `.claude/agents/<nombre>.md`, frontmatter (`name`, `description`, `tools`, `model`), body como system prompt.
2. **El comando `/agents`** — la UI integrada para crear, listar, editar y borrar.
3. **Patrones de delegación** — las 5 razones para delegar y las 4 para no delegar (slides 10-12).
4. **El número práctico: 3-4 subagentes para uso general** + matiz de harness verticales.
5. **Cuatro casos típicos**: Explorer, Reviewer, Tester, Planner — frontmatters y bodies completos.
6. **Skill vs subagente: la decisión rápida** y el patrón compuesto "skill que invoca subagente".
7. **Anti-patrones**: naming genérico, description que es persona en vez de workflow, tools sin restringir, subagente que debería ser skill, modelo mal elegido, etc.

Esta demo aterriza la gamma con tres construcciones: el `repo-explorer` y el `dotnet-reviewer` que el alumno ve construir paso a paso, y el `test-generator` mostrado como frontmatter sin construir (referencia). Cada uno justifica una decisión de modelo distinta (Haiku, Sonnet, Sonnet) y tools distintas (read-only vs read+git diff vs read+write+test).

> **Tipo de demo:** construcción guiada de dos subagentes con prueba en directo. La rama `demo/3.1b-after` queda con dos subagentes propios funcionales en `.claude/agents/`. **Es la primera demo del curso donde el alumno ve subagentes propios escritos desde cero y probados.**

---

## 2. Objetivo de la demo

Cinco cosas concretas que tienen que quedar en la cabeza del alumno cuando termine los ~28 minutos de screencast:

1. **La estructura de un subagente custom**: `.claude/agents/<nombre>.md` con frontmatter (`name`, `description`, `tools`, `model`) y body que es el system prompt del subagente. **El alumno debe poder repetirla de memoria**.

2. **La diferencia operativa entre skill y subagente**: skill = playbook de cómo hacer una tarea, ejecutado en el contexto principal. Subagente = rol con criterio, ejecutado en contexto aislado. La decisión rápida del manual línea 410: *"si la tarea requiere contexto propio o juicio independiente → subagente. Si es playbook reutilizable que encaja en el flujo → skill"*.

3. **El comando `/agents`** para crear, listar y editar subagentes desde dentro de Claude Code. **El alumno lo ve en directo creando el `repo-explorer` con la UI integrada**.

4. **Asociar modelo a tipo de tarea**: Haiku para tareas mecánicas (exploración), Sonnet para la mayoría (review, tests), Opus para razonamiento complejo (planner). La gamma 3.1b slide 30 lo marcó como anti-patrón: *"subagente de exploración corriendo en Opus = caro y sin necesidad"*.

5. **El patrón compuesto "skill que invoca subagente"**. La gamma 3.1b slide 28 lo sembró como **el patrón potente**. La 3.2a lo va a desarrollar. Aquí solo se anuncia con un caso concreto: el skill `pre-commit-check` que invocaría al `dotnet-reviewer`.

Y dos cosas que tienen que **NO quedar** en su cabeza:

- *"Cuantos más subagentes, mejor."* — al revés. **3-4 para uso general**. Más allá, la productividad baja. La regla de oro de la gamma 3.1b slide 13.
- *"El subagente recuerda interacciones anteriores."* — no. **Cada invocación arranca con contexto vacío**. La memoria persistente vive en `CLAUDE.md` (proyecto) o en artefactos durables (lo veremos en 3.2b).

---

## 3. Branch `demo/3.1b-before`

Punto de partida del screencast.

```
demo/3.1b-before
```

**Parte de:** `demo/3.1a`.

**Estado del repo:** todo lo de `demo/3.1a` (cuatro skills, `CLAUDE.md`, los hallazgos del experimento de contaminación en `docs/subagentes-explorados.md`) más un único cambio commiteado: en `src/OrderManagement.Application/Handlers/CancelOrderHandler.cs` la primera llamada async se ha convertido a `.Result` bloqueante. **Anti-patrón deliberado** — está commiteado para que el `dotnet-reviewer` lo cace cuando se ejecute en el screencast con `git diff HEAD~1 HEAD`. **No hay `.claude/agents/`** todavía: la pieza viva es construirla.

> El formador hace `git checkout demo/3.1b-before` antes de empezar a grabar.

---

## 4. Branch `demo/3.1b-after`

Estado final que la siguiente clase (3.2a) asume.

```
demo/3.1b-after
```

**Parte de:** `demo/3.1b-before`.

**Qué añade respecto a `-before`:**

1. **`repo-explorer`** en `.claude/agents/repo-explorer.md` — especialización del built-in Explore con rol y formato concretos para OrderManagement. Modelo Haiku, tools read-only (`Read`, `Grep`, `Glob`).
2. **`dotnet-reviewer`** en `.claude/agents/dotnet-reviewer.md` — revisor crítico de código C#/.NET. Modelo Sonnet, tools read + `git diff`.
3. **Revert del `.Result`** — el handler vuelve a la llamada async original (lo que el `dotnet-reviewer` propone arreglar; queda como precedente de "el reviewer cazó el bug y se aplica la corrección").
4. **Marca `[x]`** en `docs/DEMOS.md` y `docs/subagentes-explorados.md` actualizado con los dos subagentes nuevos.

**Cómo se prepara:** ver §6b. Se materializa antes de grabar.

> Durante la grabación, el formador construye los dos subagentes en directo desde `demo/3.1b-before`, los prueba (el `dotnet-reviewer` caza el `.Result`), y al cerrar descarta los cambios reales. La siguiente clase parte de `demo/3.1b-after` ya pre-cocinada con los subagentes y el `.Result` revertido.

---

## 5. Estado del repo al hacer `git checkout demo/3.1b-before`

Idéntico a `demo/3.1a`, con un único cambio: el `.Result` deliberadamente introducido en `CancelOrderHandler.cs`:

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
│   ├── auditoria-skills-comunidad.md
│   └── subagentes-explorados.md            (de 3.1a)
├── scripts/
├── src/
├── frontend/
├── tests/
├── .gitignore
├── CLAUDE.md
└── README.md
```

**Estado clave para esta demo:**

- **No hay `.claude/agents/`** — la creamos en vivo.
- Los componentes `OrderSummary` y `OrderFilter` generados en módulo 2 sirven como caso para probar el `dotnet-reviewer`.
- Para probar el `dotnet-reviewer` con `git diff`, vamos a hacer un cambio de prueba en `CancelOrderHandler.cs` introduciendo deliberadamente un anti-patrón (un `.Result` bloqueante) que el reviewer tiene que cazar.

**Estado de la máquina Windows del formador:**

```
✅ Claude Code v2.1.x con comando /agents disponible
✅ Git for Windows
✅ PowerShell 7
✅ VS Code con el repo cargado en demo/3.1b-before
✅ CLAUDE.md y settings.json operativos
✅ Cuatro skills cargables
```

**Lo que el alumno verá al final de la demo:**

- El comando `/agents` invocado en directo, con el menú interactivo.
- La construcción del `repo-explorer` paso a paso: frontmatter primero (con la decisión razonada de tools y modelo), body después (con el rol y el formato de salida estructurado).
- Prueba del `repo-explorer` mapeando el módulo `Application` — devuelve resumen estructurado en su contexto aislado.
- La construcción del `dotnet-reviewer` con tools restringidas (read + `git diff`) y modelo Sonnet.
- Prueba del `dotnet-reviewer` sobre un cambio deliberadamente malo (`.Result` bloqueante en `CancelOrderHandler.cs`) — el reviewer lo caza.
- Frontmatters de Tester y Planner mostrados como referencia (sin construir).
- Anti-patrones repasados con ejemplos de qué SÍ y qué NO.
- La siembra del patrón compuesto "skill que invoca subagente" para 3.2a.

---

## 6a. Prompt para Claude Code — preparar `demo/3.1b-before`

> Crea la rama de partida del screencast desde `demo/3.1a` con UN cambio commiteado: el `.Result` bloqueante deliberado en `CancelOrderHandler` para que el `dotnet-reviewer` lo cace con `git diff HEAD~1 HEAD`. **No crea `.claude/agents/` ni los subagentes** — esa es la pieza viva.

````
Estoy preparando la demo 3.1b del curso de Claude Code (subagentes custom:
repo-explorer y dotnet-reviewer). Sigue el patrón before/after (ver demo M0.2).

Quiero que prepares la rama `demo/3.1b-before` desde `demo/3.1a`
con un único cambio commiteado: el .Result bloqueante deliberado en
CancelOrderHandler para que durante el screencast el dotnet-reviewer
recién construido lo cace con `git diff HEAD~1 HEAD`.

# Lo que necesito

Tres tareas:

## Tarea 1: crear la rama

```powershell
git checkout demo/3.1a
git pull
git checkout -b demo/3.1b-before
```

## Tarea 2: introducir el .Result bloqueante en CancelOrderHandler

Localiza el método Handle de
`ordermanagement/src/OrderManagement.Application/Handlers/CancelOrderHandler.cs`.
Encuentra la primera llamada async y conviértela en `.Result` bloqueante.
Por ejemplo, si hay:

```csharp
var order = await _orders.GetByIdAsync(request.OrderId, ct);
```

Cámbialo a:

```csharp
var order = _orders.GetByIdAsync(request.OrderId, ct).Result;
```

Solo UNA llamada. El resto del handler intacto.

## Tarea 3: verificar build y commitear

```powershell
Set-Location c:\w\repos\F-004-ClaudeCode\ordermanagement
dotnet build
```

Esperado: 0 warnings, 0 errors. (El .Result compila aunque sea anti-patrón.)

```powershell
Set-Location c:\w\repos\F-004-ClaudeCode
git add ordermanagement/src/OrderManagement.Application/Handlers/CancelOrderHandler.cs
git commit -m "demo/3.1b-before: anti-patrón .Result bloqueante en CancelOrderHandler (deliberado para el screencast)"
```

NO hagas push.

# Restricciones (importantes)

- NO crees `.claude/agents/`. Esa creación es la pieza viva del screencast.
- NO marques `[x]` en docs/DEMOS.md todavía. Eso va en `-after`.
- NO modifiques skills, CLAUDE.md, settings.json.
- NO toques otros ficheros aparte del CancelOrderHandler.

# Cuando termines, dime

1. Que la rama demo/3.1b-before está creada desde demo/3.1a.
2. Que el .Result está introducido y commiteado.
3. Que `git log --oneline -1` muestra el commit del anti-patrón.
4. Que dotnet build pasa.
````

---

## 6b. Prompt para Claude Code — preparar `demo/3.1b-after`

> Materializa la rama final con los dos subagentes pre-cocinados, el `.Result` revertido (como si el `dotnet-reviewer` hubiera cazado el bug y se hubiera aplicado el fix), y `docs/DEMOS.md` marcado. Equivalente al resultado del screencast.

````
Estoy preparando la demo 3.1b del curso de Claude Code. Esta rama
-after pre-cocina dos subagentes custom (repo-explorer y dotnet-reviewer)
y el revert del .Result que el formador construirá / aplicará en vivo.

# Contexto

Estoy en la rama `demo/3.1b-before` del repo `ordermanagement`. La rama
parte de `demo/3.1a` y tiene UN único cambio commiteado: el
.Result bloqueante en CancelOrderHandler. NO tiene `.claude/agents/`.

Quiero que prepares la rama `demo/3.1b-after` desde `demo/3.1b-before`
con los dos subagentes, el revert del .Result, DEMOS.md marcado y
subagentes-explorados.md ampliado.

# Lo que necesito

Cinco tareas:

## Tarea 1: crear la rama

```powershell
git checkout demo/3.1b-before
git checkout -b demo/3.1b-after
```

## Tarea 2: crear `ordermanagement/.claude/agents/repo-explorer.md`

Subagente especializado en exploración estructural del proyecto OrderManagement.

- Frontmatter:
  - `name: repo-explorer`
  - `description: Explora la estructura de OrderManagement con foco en capas y producir un resumen accionable. Úsalo cuando necesites entender una zona del repo sin contaminar el contexto principal.`
  - `tools: Read, Grep, Glob`
  - `model: haiku`
- Body: system prompt que define el rol, el formato de salida estructurado (5 secciones: estructura, dependencias, patrones detectados, anti-patrones emergentes, hallazgos accionables), y la restricción de NUNCA escribir.

## Tarea 3: crear `ordermanagement/.claude/agents/dotnet-reviewer.md`

Subagente revisor crítico de código C#/.NET.

- Frontmatter:
  - `name: dotnet-reviewer`
  - `description: Revisa cambios staged o un diff dado y reporta CRÍTICO / ALTA / MEDIA con el formato file:line:problema:fix. Úsalo antes de commitear o antes de un PR.`
  - `tools: Read, Grep, Glob, Bash(git diff:*)`
  - `model: sonnet`
- Body: system prompt que define el rol, los criterios de severidad (CRÍTICO bloquea PR, ALTA revisar antes de mergear, MEDIA mejora propuesta), el formato verbatim `file:line:problema:fix`, y la restricción de NUNCA modificar código.

## Tarea 4: revertir el `.Result` (el dotnet-reviewer lo habría cazado)

Vuelve a poner el `await` original en `CancelOrderHandler.cs`. Equivalente al fix que el formador aplicaría en directo tras la observación del reviewer.

## Tarea 5: actualizar DEMOS.md, subagentes-explorados.md, build y commit

Marca la 3.1b en `docs/DEMOS.md`:

```
- [x] **demo/3.1b-before / demo/3.1b-after** — Subagentes custom: repo-explorer y dotnet-reviewer
```

Añade al final de `docs/subagentes-explorados.md` una sección «### Subagentes propios construidos en 3.1b» con dos bullets que resuman cada uno (modelo, tools, rol).

Verifica con `dotnet build` desde `ordermanagement/` (0 warnings, 0 errors) y commit desde la raíz del curso:

```powershell
Set-Location c:\w\repos\F-004-ClaudeCode
git add ordermanagement/.claude/agents `
        ordermanagement/src/OrderManagement.Application/Handlers/CancelOrderHandler.cs `
        docs/DEMOS.md docs/subagentes-explorados.md
git commit -m "demo/3.1b-after: subagentes repo-explorer y dotnet-reviewer + revert .Result"
```

NO hagas push.

# Restricciones (importantes)

- NO modifiques skills, CLAUDE.md, settings.json.
- NO toques otros ficheros del código aparte del revert del .Result.
- Respeta los anti-patrones del manual: nombres descriptivos (no `helper`),
  description en tercera persona empezando con verbo, tools restringidos.

# Cuando termines, dime

1. Que la rama demo/3.1b-after está creada desde demo/3.1b-before.
2. Que existen los dos subagentes con frontmatter correcto.
3. Que CancelOrderHandler ya no tiene `.Result` (vuelve a `await`).
4. Que docs/DEMOS.md está marcado.
5. Que docs/subagentes-explorados.md está ampliado.
6. Que dotnet build pasa.
7. Que el commit está hecho.

Si tienes dudas, para y pregúntame.
````

---

## 7. Artefactos que Claude Code debe generar

```
✓ Rama demo/3.1b-before (parte de demo/3.1a) con UN commit:
  └── src/OrderManagement.Application/Handlers/CancelOrderHandler.cs
      (anti-patrón .Result deliberado, commiteado para que el reviewer
       lo cace con git diff HEAD~1 HEAD)
✓ Rama demo/3.1b-after (parte de demo/3.1b-before) con UN commit:
  ├── .claude/agents/repo-explorer.md (Haiku, read-only)
  ├── .claude/agents/dotnet-reviewer.md (Sonnet, read + git diff)
  ├── src/OrderManagement.Application/Handlers/CancelOrderHandler.cs (revert .Result)
  ├── docs/DEMOS.md con 3.1b marcada como [x]
  └── docs/subagentes-explorados.md ampliado
✓ Verificación de build OK
```

**Lo que NO debe haber generado:**

- ❌ Ningún `.claude/agents/` (creación en vivo)
- ❌ Ningún subagente custom
- ❌ El `.Result` commiteado (debe quedar como cambio sin stagear)
- ❌ Cambios en skills, CLAUDE.md o settings.json
- ❌ Cambios en otros ficheros .NET o Angular

> Si Claude Code se anticipa y crea los subagentes, **se rechaza el output**. La construcción en vivo es el corazón pedagógico de esta demo.

**Lo que el formador commitea EN VIVO sobre `demo/3.1b-before` durante el screencast:**

```
Durante la grabación, sobre demo/3.1b-before, se hacen commits ficticios:

1. "demo/3.1b-after: subagente repo-explorer creado"
   └── .claude/agents/repo-explorer.md (NUEVO)

2. "demo/3.1b-after: subagente dotnet-reviewer creado"
   └── .claude/agents/dotnet-reviewer.md (NUEVO)
   └── docs/subagentes-explorados.md (MODIFICADO con notas)

3. "demo/3.1b-after: revierte anti-patrón .Result (lo cazó el dotnet-reviewer)"
   └── src/OrderManagement.Application/Handlers/CancelOrderHandler.cs
       (revertido al estado original)

Al cerrar el screencast: el formador descarta los commits reales.
La siguiente clase parte de demo/3.1b-after (pre-cocinada en §6b)
que tiene los DOS subagentes y el revert del .Result aplicados.
```

**Estado final del árbol después del screencast (no del prompt):**

```
ordermanagement/
├── .claude/
│   ├── settings.json
│   ├── skills/
│   │   └── (sin cambios)
│   └── agents/                                  ← NUEVA carpeta
│       ├── repo-explorer.md                     ← NUEVO (en vivo)
│       └── dotnet-reviewer.md                   ← NUEVO (en vivo)
├── docs/
│   ├── DEMOS.md                                 ← MODIFICADO (pre-grabación)
│   └── subagentes-explorados.md                 ← MODIFICADO (en vivo)
└── src/OrderManagement.Application/Handlers/
    └── CancelOrderHandler.cs                    (revertido al estado de 3.1a)
```

---

## 8. Guion del screencast

**Duración estimada del screencast: ~26-30 minutos.**

Once bloques. Es la demo más larga del módulo 3 hasta este punto, alineado con que la gamma 3.1b cubre 32 slides densos.

> **Antes de empezar a grabar**, asegúrate de:
> - Tener PowerShell 7 abierto en pantalla completa, fuente al menos 16pt.
> - Tener VS Code abierto con el repo en `demo/3.1b-before`.
> - Verificar que el cambio `.Result` está sin stagear: `git status` debe mostrar `modified: src/OrderManagement.Application/Handlers/CancelOrderHandler.cs`.
> - Verificar que `git diff` enseña claramente el cambio (la línea `await ...` cambiada a `.Result`).
> - Cerrar Slack, Teams, navegadores con notificaciones.

---

### Bloque 1 — Setup y la pregunta del cierre del 3.1a (~1 min 30 seg)

**Pantalla compartida.** A la izquierda VS Code con el repo en `demo/3.1b-before`. A la derecha terminal PowerShell.

**En la terminal:**

```powershell
git status
ls .claude\
```

```
On branch demo/3.1b
Changes not staged for commit:
        modified:   src/OrderManagement.Application/Handlers/CancelOrderHandler.cs

    Directorio: C:\Users\pedro\projects\ordermanagement\.claude

Mode    LastWriteTime    Length Name
----    -------------    ------ ----
d----   ...                     skills
-a---   ...                3456 settings.json
```

**Lo que dices:**

> "Estamos en la rama `demo/3.1b-before`. Mirad lo que hay en `.claude/`: solo `skills/` y `settings.json`. **Ningún `agents/` todavía**. Eso es lo que vamos a cambiar.
>
> En la 3.1a vimos los tres built-in y el modelo conceptual. Cerramos con una pregunta: *'¿qué tarea de vuestro día a día sería el primer candidato a subagente para vuestro equipo?'*. Hoy lo respondemos materializando dos.
>
> Vamos a construir **dos subagentes propios**:
>
> Uno. **`repo-explorer`** — especialización del built-in Explore con un rol concreto y un formato de salida estructurado para OrderManagement. Modelo Haiku — exploración mecánica.
>
> Dos. **`dotnet-reviewer`** — revisor crítico de código C# .NET. Modelo Sonnet, tools restringidas. Y vamos a probarlo con un caso real: **mirad el `git status`, hay un cambio sin commitear en `CancelOrderHandler.cs`**. Es un anti-patrón deliberado — un `.Result` bloqueante. Quiero que el `dotnet-reviewer` lo cace antes de commitear.
>
> Tres. **Tester y Planner** — los otros dos casos típicos del manual. No los construimos todos, pero **veréis sus frontmatters** como referencia para que sepáis cómo se ven.
>
> Empezamos."

**Tiempo:** ~90 segundos.

---

### Bloque 2 — Anatomía del subagente custom (~2 min)

> "Antes de tocar nada, **la anatomía**. La gamma 3.1b slide 4 lo cubrió. Un subagente vive en `.claude/agents/<nombre>.md` y tiene dos partes."

**En el editor de texto al lado, escribo (contenido pedagógico):**

```
ANATOMÍA DE UN SUBAGENTE CUSTOM

Ubicación:
  .claude/agents/<nombre>.md       (proyecto, va a git)
  ~/.claude/agents/<nombre>.md     (personal, viaja contigo)

Estructura del fichero:

  ---                                ← Frontmatter YAML (igual que skill)
  name: nombre-del-subagente
  description: Qué hace y cuándo
                activarlo. Crítico.
  tools: Read, Grep, Glob,
         Bash(git diff *)
  model: sonnet                      ← haiku, sonnet, opus, inherit
  ---
  
  ← Body (system prompt del subagente)
  
  Eres un [rol específico]...
  
  Cuando seas invocado:
  1. ...
  2. ...
  
  Devuelve [formato estructurado]...
```

> "Mirad las cuatro piezas del frontmatter:
>
> Una. **`name`** — kebab-case, igual que con skills. *`repo-explorer`*, no *`RepoExplorer`*.
>
> Dos. **`description`** — qué hace y cuándo activarlo. **Crítica para la auto-delegación**, igual que con skills (recordad la 2.1b — la descripción es el switch).
>
> Tres. **`tools`** — herramientas permitidas. **Si lo omites, hereda todas las de la sesión**. Mala práctica. La gamma 3.1b slide 29 lo marcó como anti-patrón.
>
> Cuatro. **`model`** — `haiku`, `sonnet`, `opus`, o `inherit`. Asociar modelo a tipo de tarea: Haiku para mecánicas, Sonnet para la mayoría, Opus para razonamiento complejo.
>
> Y el body **es el system prompt del subagente**. Aquí es donde se diferencia de un skill. Un skill da instrucciones de cómo hacer una tarea. Un subagente **define un rol completo con criterio**. *'Eres un revisor senior de código .NET'*. *'Eres un explorador del repositorio'*. **Persona + workflow + formato de salida**.
>
> Vamos a construir el primero."

**Tiempo:** ~2 minutos.

---

### Bloque 3 — `/agents`: el comando de gestión (~2 min)

> "La forma más cómoda de crear y gestionar subagentes es la **UI integrada con `/agents`**. La gamma 3.1b slide 7 lo cubrió. Lanzo Claude Code y lo veo."

**En la terminal:**

```powershell
claude
```

```
✓ 4 project skills loaded: angular-component, commit-style, db-reset, frontend-design
```

**Tecleo:**

```
> /agents
```

**Aparece (output ejemplo según versión):**

```
Subagent management
─────────────────────

Built-in (always available):
  ◆ Explore         (lectura/exploración, haiku)
  ◆ Plan            (planificación, sonnet)
  ◆ general-purpose (comodín, sonnet)

Project agents (.claude/agents/):
  (none yet)

User agents (~/.claude/agents/):
  (none yet)

Actions:
  ❯ Create new agent
    List existing agents
    Edit agent
    Delete agent
    Exit
```

> "Mirad. La UI me muestra los **tres built-in** que ya conocemos. Y dos secciones vacías: **Project agents** y **User agents**. Ahí van los nuestros.
>
> Le doy a 'Create new agent'."

**Selecciono "Create new agent". Aparece:**

```
Where do you want to create the agent?
  ❯ Project (.claude/agents/) — goes to git, shared with team
    User (~/.claude/agents/) — personal, follows you across projects
```

> "Me pregunta scope. **Recordad la lección de la 2.2c**: para skills hay 3 scopes. Para agents lo mismo — proyecto, personal, plugin. **Project** porque queremos que vaya a git con el equipo. Selecciono."

**Selecciono "Project".**

```
How do you want to create the agent?
  ❯ Generate with Claude (recommended for first time)
    Write manually
```

> "**La recomendación de la gamma 3.1b slide 8**: deja que Claude lo genere primero, después lo ajustas. Es más rápido editar un draft decente que partir de cero. Pero **para esta demo voy a escribirlo manualmente** — para que veáis exactamente cada decisión. Selecciono 'Write manually'."

**Selecciono "Write manually".**

```
Agent file will be created at: .claude/agents/<name>.md
Press Enter to open it in your editor.
```

> "Me abre el editor con la plantilla. Vamos a rellenarla con el `repo-explorer`."

**Tiempo:** ~2 minutos.

---

### Bloque 4 — Construir `repo-explorer`: frontmatter y body (~5 min)

**Salgo del menú de `/agents` y voy a VS Code para construir el fichero con calma. En VS Code, creo `.claude/agents/repo-explorer.md`:**

> "Vamos por el frontmatter primero. Cuatro decisiones."

**Escribo el frontmatter:**

```markdown
---
name: repo-explorer
description: Explora y mapea zonas del repositorio OrderManagement para devolver una vista resumida estructurada. Usar cuando el usuario necesite entender un módulo, una carpeta o una funcionalidad sin haberla tocado antes, especialmente Domain, Application, Infrastructure o frontend Angular.
tools: Read, Grep, Glob
model: haiku
---
```

> "Decisión uno: **`name: repo-explorer`**. Kebab-case. Específico, no genérico. La gamma 3.1b slide 29 marcó como anti-patrón los nombres genéricos como `frontend-engineer`. *`repo-explorer`* dice exactamente qué hace.
>
> Decisión dos: **`description`**. Aplica la fórmula de los tres ingredientes que aprendimos en la 2.1b: verbo claro al inicio (*'Explora y mapea'*), abanico de triggers (*'entender un módulo, una carpeta o una funcionalidad'*), contexto del proyecto explícito (*'OrderManagement... Domain, Application, Infrastructure o frontend Angular'*). Triggers en abanico, no un solo verbo.
>
> Decisión tres: **`tools: Read, Grep, Glob`**. Solo lectura. **No tiene Write, no tiene Edit**. Aunque la sesión principal tenga permisos amplios, este subagente **físicamente no puede modificar nada**. La gamma 3.1b slide 29 lo marcó: *'subagentes que solo deberían leer, restringe explícitamente'*. Principio de mínimo privilegio.
>
> Decisión cuatro: **`model: haiku`**. La exploración es tarea mecánica — leer ficheros, contar imports, identificar patrones. **No requiere razonamiento profundo**. Haiku es más rápido y más barato. Si pongo Opus, gasto dinero sin ganancia. La gamma slide 30 lo dijo: *'subagente de exploración corriendo en Opus = caro y sin necesidad'*."

**Ahora añado el body — el system prompt del subagente:**

```markdown
# Repo Explorer — system prompt

Eres un explorador del repositorio OrderManagement. Tu trabajo es entender
una zona del código y devolver un resumen estructurado al agente principal.

## Cuando seas invocado

Recibirás un objetivo de exploración del agente principal. Ejemplos:
- *"Mapea el módulo OrderManagement.Application"*
- *"Entiende cómo está estructurado el frontend Angular"*
- *"Investiga cómo se usan los handlers MediatR en el proyecto"*

Procede así:

1. **Identifica los ficheros principales** del área (entry points,
   definiciones públicas, exports).
2. **Mapea las dependencias y conexiones** entre ficheros con Grep y Glob.
3. **Identifica los patrones recurrentes** (cómo se estructura la zona).
4. **Encuentra los puntos de extensión o variación** (interfaces, factories,
   strategies).
5. **Detecta señales de cuidado**: TODOs, FIXMEs, código que parezca frágil,
   convenciones inconsistentes.

## Formato de salida

Devuelve un resumen en markdown con cinco secciones, en este orden:

### Estructura general
Carpetas, ficheros principales, organización lógica del área.

### Puntos de entrada
Qué se expone hacia fuera, cómo otras zonas del proyecto consumen esto.

### Patrones internos
Cómo está organizado por dentro. Patrones recurrentes (Repository, Handler,
Strategy, etc.). Convenciones que se repiten.

### Dependencias
Qué consume esta zona del resto del proyecto. Qué proyectos del solution
depende.

### Notas de cuidado
- TODOs y FIXMEs explícitos
- Código duplicado entre ficheros
- Convenciones inconsistentes con el resto del proyecto
- Tests ausentes o débiles

## Restricciones

- **Solo lectura**. No modificas ficheros.
- **Conciso**. El destinatario es el agente principal — necesita
  información estructurada, no narrativa.
- **No improvises información** que no veas en los ficheros. Si no lo ves,
  dilo: *"sin información sobre X en los ficheros explorados"*.
- **Respeta las convenciones del proyecto** que están en `CLAUDE.md` —
  si encuentras desviaciones, márcalas en *Notas de cuidado*.
```

**Salvo el fichero.**

> "Mirad lo que tiene el body:
>
> Empieza con **'Eres un explorador del repositorio OrderManagement'** — define el rol con criterio. **No 'eres un desarrollador full-stack'** — eso describe a la persona, no al workflow. Es **persona orientada al job**. La gamma 3.1b slide 29 lo marcó como anti-patrón.
>
> Cinco pasos numerados de **cómo abordar la tarea**. Estructurado. El subagente sabe exactamente qué hacer.
>
> **Formato de salida estructurado** con cinco secciones fijas. Estructura general, puntos de entrada, patrones internos, dependencias, notas de cuidado. **El agente principal puede confiar en que el subagente devuelve siempre el mismo formato** — eso facilita orquestaciones futuras (3.2a).
>
> **Restricciones explícitas**: solo lectura, conciso, no improvises, respeta `CLAUDE.md`. Estas son las **guard rails** del subagente.
>
> Vamos a probarlo."

**Tiempo:** ~5 minutos.

---

### Bloque 5 — Probar `repo-explorer` en directo (~3 min)

**En la terminal, salgo y vuelvo a entrar a Claude Code para refrescar (los subagentes se cargan al arrancar — recordemos del manual línea 130):**

```powershell
exit
claude
```

```
✓ 4 project skills loaded: angular-component, commit-style, db-reset, frontend-design
✓ 1 project agent loaded: repo-explorer
```

> "Mirad la línea: **`1 project agent loaded: repo-explorer`**. Cargado. Listo para invocar.
>
> Vamos a probarlo. Le pido que mapee el módulo `Application` que no hemos tocado mucho en demos:"

**Tecleo:**

```
> Usa el subagente repo-explorer para mapear el módulo
  OrderManagement.Application. Quiero saber qué handlers, qué
  comandos, qué interfaces y cómo se interconectan.
```

**Aparece:**

```
● Launching repo-explorer subagent...

  [Subagent runs in isolated context with Haiku model]
  
  [Tools used: Glob, Read, Grep — all read-only]

● repo-explorer subagent returned:

  ESTRUCTURA GENERAL
  ──────────────────
  src/OrderManagement.Application/
  ├── Commands/         (6 commands MediatR)
  ├── Queries/          (3 queries MediatR)  
  ├── Handlers/         (9 handlers, 1 por command/query)
  ├── Interfaces/       (3 interfaces de repositorio)
  ├── Exceptions/       (4 excepciones tipadas)
  └── DependencyInjection.cs
  
  PUNTOS DE ENTRADA
  ─────────────────
  - Comandos públicos: CreateOrderCommand, UpdateOrderCommand, 
    CancelOrderCommand, AddItemCommand, RemoveItemCommand, 
    SetCustomerCommand
  - Queries: GetOrderByIdQuery, ListOrdersQuery, GetOrderItemsQuery
  - Consumidos desde OrderManagement.Api/Controllers/OrdersController.cs
    vía IMediator.Send()
  
  PATRONES INTERNOS
  ─────────────────
  - CQRS con MediatR: una clase Command/Query por intent, un Handler por
    cada uno
  - Handlers async retornan Task<Result> o Task<Unit>
  - Excepciones tipadas (CustomerNotFoundException, OrderNotFoundException,
    InvalidOrderStateException)
  - Inyección con inject() vía constructor
  
  DEPENDENCIAS
  ────────────
  - OrderManagement.Domain (entidades, agregados)
  - MediatR (pipeline)
  - No depende de Infrastructure — usa interfaces que Infrastructure
    implementa
  
  NOTAS DE CUIDADO
  ────────────────
  - 0 tests para Handlers todavía (tests/ está vacío)
  - CancelOrderHandler.cs MODIFICADO actualmente con un .Result
    bloqueante — anti-patrón en código async
  - SetCustomerCommand sin handler todavía (TODO en línea 12)
```

> "**Mirad lo que ha devuelto.** Cinco secciones, exactamente como el body del subagente decía. Estructura, puntos de entrada, patrones, dependencias, notas de cuidado.
>
> Y atención a la última nota: **'CancelOrderHandler.cs MODIFICADO actualmente con un .Result bloqueante'**. **El subagente lo ha detectado** porque vio el `git status` o el contenido del fichero. **Sin que se lo pidiera**. Lo marcó como anti-patrón en notas de cuidado.
>
> Esto es exactamente lo que un Explorer bien afinado hace — **alerta de cosas raras que ve mientras explora**. La gamma 3.1b slide 19 lo marcó: *'detecta puntos de cuidado'*.
>
> Y lo más importante: **mi contexto principal está limpio**. No tengo los handlers cargados. Solo el resumen. Igual que con el built-in Explore en la 3.1a, pero ahora con un **rol concreto y un formato concreto** que el equipo puede confiar.
>
> Salgo y construyo el segundo."

**Salgo (Ctrl+C):**

**Tiempo:** ~3 minutos.

---

### Bloque 6 — Construir `dotnet-reviewer`: tools y modelo distintos (~4 min)

> "Segundo subagente: **`dotnet-reviewer`**. Revisor crítico de código C# .NET. Las decisiones de frontmatter cambian respecto al `repo-explorer` — **distinto rol, distintas tools, distinto modelo**."

**En VS Code, creo `.claude/agents/dotnet-reviewer.md`:**

```markdown
---
name: dotnet-reviewer
description: Revisa código C# / .NET buscando problemas de naming, patrones async incorrectos, manejo de errores deficiente y violaciones de las convenciones del equipo OrderManagement. Usar después de cambios significativos en código antes de un commit o un PR.
tools: Read, Grep, Glob, Bash(git diff *), Bash(git log *)
model: sonnet
---

# .NET Reviewer — system prompt

Eres un revisor senior de código C# / .NET. Tu trabajo es identificar
problemas en código recién escrito y proponer fixes concretos.

## Foco específico (basado en CLAUDE.md del proyecto)

### Async / await
- **NUNCA** `.Result` ni `.Wait()` en código async — bloquea el thread.
- **NUNCA** `async void` salvo en event handlers reales.
- Propaga `CancellationToken` siempre que esté disponible.

### Naming
- PascalCase para clases, métodos públicos, propiedades.
- _camelCase para campos privados.
- camelCase para variables locales y parámetros.

### Manejo de errores
- `Result<T>` en capa de dominio.
- Excepciones tipadas en Application (`CustomerNotFoundException`, etc.).
- `ProblemDetails` en API.
- **NUNCA** `catch (Exception)` genérico sin re-throw o log explícito.

### Convenciones del equipo
- CQRS con MediatR: 1 Command por intent, 1 Handler por Command.
- Inyección con `inject()` o constructor — nunca service locator.
- Tests con xUnit + NSubstitute. Estructura Arrange-Act-Assert explícita.

## Cuando seas invocado

1. Ejecuta `git diff` (o `git diff --cached` si te indican que es para commit)
   para identificar los ficheros modificados.
2. Para cada fichero modificado, examina los cambios línea a línea.
3. Aplica los criterios del foco específico.
4. Devuelve los hallazgos clasificados por severidad.

## Formato de salida

```
HALLAZGOS DE LA REVISIÓN
────────────────────────

[CRÍTICO] (problemas que bloquean el commit)
  - Fichero:línea
    Problema: descripción concisa
    Fix sugerido: cambio concreto

[IMPORTANTE] (problemas que merece la pena arreglar)
  - ...

[SUGERENCIA] (mejoras opcionales)
  - ...
```

Si no hay problemas, devuelve una sola línea:
**"Revisión limpia. Sin hallazgos."**

## Restricciones

- **Solo lectura**. Lees el diff, identificas problemas, devuelves hallazgos.
  **NO modificas ficheros.**
- **Sé directo**. No suavices los problemas críticos.
- **Sé técnico**. El destinatario es un dev senior — no expliques
  conceptos básicos.
- **No inventes problemas**. Si todo está bien, dilo en una línea.
```

**Salvo.**

> "Mirad las decisiones distintas al `repo-explorer`:
>
> **`tools`**: ahora incluye `Bash(git diff *)` y `Bash(git log *)`. **Patterns acotados** — solo `git diff` y `git log`, no `Bash` a secas. Recordad la regla de la 2.3 sobre seguridad: *'nunca permitas Bash sin patrón'*. El reviewer **necesita ver los diffs**, pero **solo eso de Bash**.
>
> **`model: sonnet`**. Aquí no Haiku porque la revisión **requiere razonamiento**. Hay que entender el código, identificar anti-patrones sutiles, sugerir fixes. No es mecánico. Pero tampoco es Opus — Sonnet es el balance correcto para este nivel. La regla del manual línea 470: *'asocia modelo a tipo de tarea'*.
>
> El body **define las cuatro áreas de foco** del CLAUDE.md del proyecto. Async/await. Naming. Manejo de errores. Convenciones del equipo. **El subagente sabe lo que el equipo espera**. No es una review genérica de C# — es una review que conoce a este equipo.
>
> El **formato de salida** es estricto: tres severidades (CRÍTICO / IMPORTANTE / SUGERENCIA), cada hallazgo con fichero, línea, problema, y fix sugerido. Y si no hay problemas, **una línea**.
>
> Vamos a probarlo con el `.Result` que dejé sin commitear."

**Tiempo:** ~4 minutos.

---

### Bloque 7 — Probar `dotnet-reviewer` con el caso real (~3 min)

**En la terminal:**

```powershell
git status
```

```
On branch demo/3.1b
Changes not staged for commit:
        modified:   src/OrderManagement.Application/Handlers/CancelOrderHandler.cs

Untracked files:
        .claude/agents/
```

> "El cambio del `.Result` está sin stagear. Y el `.claude/agents/` con los dos subagentes está untracked. **Vamos a stagear el cambio del handler para probar el reviewer**:"

```powershell
git add src/OrderManagement.Application/Handlers/CancelOrderHandler.cs
```

**Arranco Claude Code:**

```powershell
claude
```

```
✓ 4 project skills loaded: angular-component, commit-style, db-reset, frontend-design
✓ 2 project agents loaded: repo-explorer, dotnet-reviewer
```

> "**Dos project agents cargados ahora**: `repo-explorer` y `dotnet-reviewer`. Le pido al reviewer que revise lo staged:"

**Tecleo:**

```
> Usa el subagente dotnet-reviewer para revisar los cambios staged
  antes de que los commitee.
```

**Aparece:**

```
● Launching dotnet-reviewer subagent...

  [Subagent runs in isolated context with Sonnet model]
  
  [Tools used: Bash(git diff --cached), Read]

● dotnet-reviewer subagent returned:

  HALLAZGOS DE LA REVISIÓN
  ────────────────────────

  [CRÍTICO]
    - src/OrderManagement.Application/Handlers/CancelOrderHandler.cs:23
      Problema: uso de .Result en código async — bloquea el thread y
                puede causar deadlocks.
      Fix sugerido: cambiar
        var order = _repository.GetByIdAsync(request.OrderId, 
                       cancellationToken).Result;
      por
        var order = await _repository.GetByIdAsync(request.OrderId, 
                       cancellationToken);
      
      El método Handle ya es async Task<Unit>, así que await es lo
      correcto. .Result fuerza sincronía y rompe el patrón async/await
      del proyecto.
```

> "**Cazado.** Mirad el output. Severidad **CRÍTICO**. Fichero, línea, problema, fix concreto. **Y la justificación**: *'el método Handle ya es async Task<Unit>, así que await es lo correcto'*. **No solo dice qué está mal — explica por qué y propone el fix exacto**.
>
> Esto es un reviewer útil. Si lo encadeno con un hook pre-commit (lo veremos en 3.3), **no se va a commitear código con `.Result` nunca**. La gamma 3.1b slide 21 lo dijo: *'útil antes de commit, encadena bien con hook pre-commit'*.
>
> Vamos a aplicar el fix y seguir."

**Salgo (Ctrl+C). Aplico el fix manualmente:**

**En VS Code, edito `CancelOrderHandler.cs` y revierto el `.Result` a `await`. Salvo.**

```powershell
git add src/OrderManagement.Application/Handlers/CancelOrderHandler.cs
git diff --cached
```

```
(diff vacío — el fichero está como en demo/3.1a)
```

> "Diff vacío. El handler está como debía estar. **El reviewer me ahorró un commit malo**. Y ahora vamos a guardar la lección.

**Tiempo:** ~3 minutos.

---

### Bloque 8 — Tester y Planner: referencia rápida (~2 min 30 seg)

> "Antes de cerrar, **los otros dos casos típicos** del manual línea 311 que no construimos pero conviene conocer. La gamma 3.1b slides 22-25 los cubrió."

**En el editor de texto al lado, escribo (contenido de referencia):**

```
TESTER — frontmatter de referencia
───────────────────────────────────

---
name: test-generator
description: Genera tests unitarios xUnit + NSubstitute para código .NET
  siguiendo el patrón Arrange-Act-Assert del equipo OrderManagement.
  Usar cuando se necesite generar suite de tests para un componente,
  servicio o handler existente.
tools: Read, Grep, Write, Edit, Bash(dotnet test *)
model: sonnet
---

Cambios respecto al reviewer:
  - tools incluye Write y Edit (genera tests, no solo revisa)
  - tools incluye Bash(dotnet test *) (ejecuta lo generado para validar)
  - model: sonnet (igual — razonamiento moderado)


PLANNER — frontmatter de referencia
────────────────────────────────────

---
name: feature-planner
description: Planifica la implementación de features grandes desglosándolas
  en pasos concretos antes de empezar a codificar. Usar cuando una feature
  toca más de tres ficheros o hay decisiones de diseño implícitas.
tools: Read, Grep, Glob
model: opus
---

Cambios respecto al reviewer y al tester:
  - tools: solo lectura (igual que explorer) — Plan no escribe.
  - model: OPUS — razonamiento complejo, decisiones de diseño.
    Justifica el coste extra para tareas no triviales.
```

> "Los **cuatro casos típicos** que la gamma 3.1b cubre — Explorer, Reviewer, Tester, Planner — **cubren el 80% de las necesidades** de un equipo de devs medio.
>
> Mirad cómo cambian las decisiones según el rol:
>
> - **Explorer**: solo lee, modelo Haiku (mecánico).
> - **Reviewer**: lee + git diff, modelo Sonnet (razonamiento).
> - **Tester**: lee + escribe + ejecuta tests, modelo Sonnet (genera código).
> - **Planner**: solo lee, modelo Opus (decisiones complejas).
>
> **Asocia modelo a tipo de tarea**. Asocia tools al scope mínimo. Cada subagente es **diferente porque su trabajo es diferente**.
>
> Y la regla de oro de la gamma 3.1b slide 13: **3-4 subagentes para uso general**. Más allá, la productividad baja. **Pocos pero bien afinados**."

**Tiempo:** ~2 minutos 30 segundos.

---

### Bloque 9 — Anti-patrones repasados (~2 min)

> "Y los **anti-patrones** que la gamma 3.1b slides 29-31 cubrió. Checklist."

**En el editor:**

```
LOS 8 ANTI-PATRONES DE SUBAGENTES

1. ❌ NAMING GENÉRICO
   "frontend-engineer", "backend-helper", "dev-assistant"
   → La auto-delegación falla porque la descripción es difusa.
   ✅ Mejor: "repo-explorer", "test-runner", "pr-reviewer".

2. ❌ DESCRIPTION QUE ES PERSONA, NO WORKFLOW
   "Eres un experto desarrollador full-stack..."
   → Eso describe al agente, no la tarea.
   ✅ Mejor: "Genera tests xUnit para servicios .NET. Usar después
            de crear o modificar servicios."

3. ❌ TOOLS SIN RESTRINGIR
   Si omites tools, hereda todo de la sesión.
   → Para subagente que solo debería leer, agujero.
   ✅ Mejor: explicita least privilege.

4. ❌ SUBAGENTE QUE DEBERÍA SER SKILL
   Si la tarea está en el flujo principal, no necesita aislamiento.
   ✅ Mejor: skill > subagente para esto.

5. ❌ DEMASIADOS SUBAGENTES PARA USO GENERAL
   Más de 3-4 activos = productividad baja.
   ✅ Excepción: harness verticales estructurados.

6. ❌ NO ITERAR LA DESCRIPCIÓN
   Igual que con skills, la primera no es la final.
   ✅ Lánzala, ve si activa, ajusta.

7. ❌ MODELO MAL ELEGIDO
   - Exploración en Opus = caro y sin necesidad
   - Planificación en Haiku = falta de profundidad
   ✅ Asocia modelo a tipo de tarea.

8. ❌ SUBAGENTE QUE PIDE APROBACIONES CONSTANTES
   Si pide permisos cada dos por tres, su tools está mal acotado
   o su rol no está bien definido.
   ✅ Revisa.
```

> "**Ocho**. Si vuestros subagentes evitan estos ocho, vais por encima de la media. **Los más comunes que veo**: el primero (naming genérico) y el séptimo (modelo mal elegido). El sexto también pasa mucho — la gente escribe la descripción una vez y no la itera nunca, igual que con skills.
>
> **Mnemotécnica para el segundo**: *'persona vs workflow'*. Si la descripción dice *'eres un X'*, es persona. Si dice *'genera Y cuando Z'*, es workflow. **La descripción es para que el principal decida cuándo delegar — no es para presentar al subagente**."

**Tiempo:** ~2 minutos.

---

### Bloque 10 — Commit, notas y siembra del patrón compuesto (~2 min)

> "Vamos a commitear todo. **Dos subagentes funcionales en el repo, un fix aplicado al handler, notas en `docs/subagentes-explorados.md`**."

**En VS Code, abro `docs/subagentes-explorados.md` y añado al final:**

```markdown

---

# Demo 3.1b — Subagentes custom creados

## Subagentes nuevos

### repo-explorer

- **Ubicación**: `.claude/agents/repo-explorer.md`
- **Modelo**: Haiku (exploración mecánica, rápido y barato)
- **Tools**: Read, Grep, Glob (read-only)
- **Caso de prueba**: mapeo del módulo OrderManagement.Application
  → devolvió 5 secciones estructuradas (estructura, puntos de entrada,
  patrones, dependencias, notas de cuidado)
- **Pieza notable**: detectó por su cuenta el `.Result` modificado en
  CancelOrderHandler como anti-patrón en notas de cuidado.

### dotnet-reviewer

- **Ubicación**: `.claude/agents/dotnet-reviewer.md`
- **Modelo**: Sonnet (razonamiento moderado)
- **Tools**: Read, Grep, Glob, Bash(git diff *), Bash(git log *)
- **Caso de prueba**: revisión del `.Result` deliberadamente introducido
  en CancelOrderHandler.cs antes de commit.
- **Resultado**: severidad CRÍTICO con fichero:línea, problema, y fix
  concreto sugerido.
- **Aplicación**: el `.Result` revertido a `await` antes de commit.

## Patrón sembrado para 3.2a

El skill `commit-style` (módulo 2.2c) podría invocar al
`dotnet-reviewer` antes de generar el mensaje de commit. Ese es **el
patrón compuesto "skill que invoca subagente"** que la gamma 3.1b
slide 28 sembró y la 3.2a desarrollará.

## Estado actual de subagentes en el repo

```
.claude/agents/
├── repo-explorer.md       (Haiku, read-only)
└── dotnet-reviewer.md     (Sonnet, read + git diff)
```

Dos subagentes, dentro del rango sano de 3-4 para uso general.
Próximos candidatos posibles: test-generator, feature-planner.
```

**Salvo. En la terminal:**

```powershell
git add .claude/agents/ docs/subagentes-explorados.md src/OrderManagement.Application/Handlers/CancelOrderHandler.cs
git commit -m "demo/3.1b-after: subagentes repo-explorer y dotnet-reviewer + fix .Result"
```

```
[demo/3.1b-before xyz9876] demo/3.1b-after: subagentes repo-explorer y dotnet-reviewer + fix .Result
 4 files changed, 187 insertions(+), 1 deletion(-)
 create mode 100644 .claude/agents/repo-explorer.md
 create mode 100644 .claude/agents/dotnet-reviewer.md
```

> "Commit. **Cuatro ficheros tocados**: dos subagentes nuevos, las notas, y el handler revertido.
>
> Y **el patrón sembrado para 3.2a**: el skill `commit-style` que ya tenemos podría **invocar al `dotnet-reviewer` antes de generar el mensaje de commit**. *'Antes de proponer el commit message, pasa los cambios por el reviewer y solo procede si está limpio'*. **Skill que invoca subagente** — la gamma 3.1b slide 28. La 3.2a lo va a desarrollar."

**Tiempo:** ~2 minutos.

---

### Bloque 11 — Cliffhanger a 3.2a (~1 min 30 seg)

> "En la siguiente demo, la **3.2a**, entramos en **orquestación**. La gamma 3.2a va a cubrir tres cosas:
>
> Una. **Aislamiento de contexto en profundidad** — qué se comparte y qué no entre el principal y el subagente.
>
> Dos. **Composición skill + subagente** — el patrón que sembré hace un momento. Skills que invocan subagentes. La rentabilidad cuando combinas las piezas.
>
> Tres. **Loops de retroalimentación** — un workflow donde un subagente valida lo que produce otro. Generator + reviewer + critic.
>
> Y vais a empezar a ver el harness completo. Hasta aquí piezas sueltas: skills (módulo 2), subagentes (3.1). A partir de 3.2 las **combinamos en flujos coherentes**.
>
> Empezamos con el **tres punto dos punto A**."

**Tiempo:** ~1 minuto 30 segundos.

---

## 9. Qué resaltar verbalmente

Cinco puntos que **no pueden quedarse sin decir**:

1. **"`name`, `description`, `tools`, `model`. Cuatro decisiones del frontmatter."** — la anatomía que el alumno debe poder repetir. Bloques 2 y 4.

2. **"Asocia modelo a tipo de tarea. Haiku para mecánicas, Sonnet para la mayoría, Opus para razonamiento complejo."** — la regla de oro de los modelos. Bloques 4 y 6.

3. **"Tools restringidas siempre. Especialmente `Bash` con patrón."** — el principio de mínimo privilegio aplicado a subagentes. Bloque 6.

4. **"3-4 subagentes para uso general. Más allá, productividad baja."** — la regla práctica del manual. Bloque 8.

5. **"Skill que invoca subagente. El patrón compuesto."** — siembra para 3.2a. Bloques 10 y 11.

**Frase de remate al final:**

> *"Dos subagentes con roles concretos, tools restringidas, modelos asociados al trabajo. Y el patrón sembrado: en la 3.2a los combinamos con skills."*

---

## 10. Slide de entrada (locución HeyGen)

> **Texto del avatar antes del screencast:**

"Y vamos a la siguiente demo. La 3.1b. La primera donde construimos subagentes propios desde cero. Vais a ver dos. El `repo-explorer` — especialización del built-in Explore con un rol concreto y un formato de salida estructurado de cinco secciones para OrderManagement, modelo Haiku, tools read-only. Lo probaremos mapeando el módulo Application. Y el `dotnet-reviewer` — revisor crítico de código C# .NET, modelo Sonnet, tools que incluyen `git diff` con patrón restringido. Lo probaremos con un caso real: hay un `.Result` bloqueante deliberado en `CancelOrderHandler.cs` esperando ser cazado antes del commit. Veréis el reviewer detectarlo, clasificarlo como CRÍTICO, y proponer el fix exacto. Y veréis los frontmatters del Tester y Planner como referencia, los anti-patrones del primer día, y la siembra del patrón compuesto skill que invoca subagente para la 3.2a. Vamos al screencast."

---

## 11. Slide de salida (locución HeyGen)

> **Texto del avatar al volver del screencast:**

"Lo que acabáis de ver son vuestros primeros subagentes propios funcionando sobre OrderManagement. Cinco ideas para llevarse al lunes. Una, el frontmatter tiene cuatro decisiones: `name`, `description`, `tools`, `model`. Cada una con criterio. Dos, asocia modelo a tipo de tarea — Haiku para mecánicas, Sonnet para la mayoría, Opus para razonamiento complejo. Tres, tools restringidas siempre. Especialmente `Bash` con patrón acotado. Cuatro, tres a cuatro subagentes para uso general. Más allá la productividad baja. Excepción: harness verticales estructurados. Cinco, los cuatro casos típicos cubren el ochenta por ciento — Explorer, Reviewer, Tester, Planner. Y el patrón sembrado para la siguiente demo: skill que invoca subagente. El skill `commit-style` que ya tenéis podría invocar al `dotnet-reviewer` antes de generar el mensaje. Combinar las piezas. Eso es la 3.2a — orquestación. Empezamos con el tres punto dos punto A."

---

## 12. Tiempo total estimado

| Bloque | Tiempo |
|---|---|
| Slide de entrada (avatar) | ~30 seg |
| Bloque 1 — Setup y la pregunta del cierre 3.1a | ~1 min 30 seg |
| Bloque 2 — Anatomía del subagente custom | ~2 min |
| Bloque 3 — `/agents`: el comando de gestión | ~2 min |
| Bloque 4 — Construir `repo-explorer` | ~5 min |
| Bloque 5 — Probar `repo-explorer` en directo | ~3 min |
| Bloque 6 — Construir `dotnet-reviewer` | ~4 min |
| Bloque 7 — Probar `dotnet-reviewer` con caso real | ~3 min |
| Bloque 8 — Tester y Planner: referencia rápida | ~2 min 30 seg |
| Bloque 9 — Anti-patrones repasados | ~2 min |
| Bloque 10 — Commit, notas y siembra de 3.2a | ~2 min |
| Bloque 11 — Cliffhanger a 3.2a | ~1 min 30 seg |
| Slide de salida (avatar) | ~30 seg |
| **Total screencast** | **~28-30 min** |
| **Total con avatar** | **~29-31 min** |

> Si hay preguntas durante el screencast, súmale 4-5 minutos. La demo encaja en un bloque de **35 minutos** dentro de la sesión.

**Margen de seguridad por si algo va lento:**

- **Si `/agents` tiene una UI distinta** a la del guion en tu versión de Claude Code, no fuerces el menú exacto. Comenta: *"la UI exacta puede variar entre versiones — lo importante es que tienes un menú interactivo para crear, listar y editar subagentes sin salir de Claude Code"*. Y construye los subagentes manualmente en VS Code (que es lo que vas a hacer de todas formas).

- **Si el `repo-explorer` en el bloque 5 NO detecta el `.Result` modificado** (porque tu versión no le pasa el git status como contexto), no insistas. Comenta: *"a veces el subagente no llega a ver los cambios sin commit — lo importante es que el resumen estructurado funciona, lo del git status es bonus que no siempre sale"*. Y procede.

- **Si el `dotnet-reviewer` en el bloque 7 NO caza el `.Result`** (porque la descripción no fue suficientemente específica), iterá la descripción en directo. Comenta: *"esto es exactamente lo que la 2.1b nos enseñó — la descripción es probabilística. Voy a ser más explícito en el frontmatter"*. Añade *"Detecta especialmente .Result, .Wait() y otros patrones bloqueantes"* a la description y reinicias. **Esto es pedagógicamente potente** — el alumno ve la iteración real.

- **Si te quedas sin tiempo y los bloques 8 y 9 te aprietan**, recorta el bloque 8 a 1 minuto (solo los frontmatters de Tester y Planner sin desarrollar). El 9 puedes recortarlo a 1 min 30 seg enumerando los 8 anti-patrones sin desarrollar cada uno.

- **Si surgen preguntas sobre Subagent vs Specialist Agent**, responde corto: *"es la misma cosa, vocabulario distinto en distintos blogs. Subagente es como Anthropic lo llama oficialmente. Specialist Agent es como aparece en whitepapers más teóricos"*. Y sigue.

---

# Apéndice A — Decisiones pedagógicas justificadas

> Esta sección no se graba. Es para Pedro, formador.

**¿Por qué construir 2 subagentes y no los 4 casos típicos?**

Porque construir los 4 sería **una demo de 50 minutos** y la gamma 3.1b ya cubre todos los frontmatters en sus slides 18-25. La pedagogía está en **construir 2 con decisiones distintas y mostrar las otras 2 como referencia**. El alumno ve **dos perfiles operativos completos** (read-only Haiku vs read+git Sonnet) y reconoce los otros dos cuando los ve. **Profundidad sobre 2, anchura sobre 4**.

**¿Por qué `repo-explorer` y `dotnet-reviewer` y no Tester y Planner?**

Por tres razones:
1. **Tester requiere generar tests reales** — añade complejidad técnica que distrae del foco pedagógico (qué es un subagente).
2. **Planner usa Opus** — explicar la decisión de modelo es importante pero el alumno ya la ve en el contraste Haiku-Sonnet del Explorer y Reviewer. No añade aprendizaje extra.
3. **Reviewer permite el caso real con `.Result`** — pieza pedagógica estrella. El alumno ve un fallo cazado antes del commit. **Tester y Planner no tienen este "momento aha" tan claro**.

**¿Por qué introducir el `.Result` deliberado en CancelOrderHandler antes de la grabación?**

Para que el `dotnet-reviewer` **tenga algo concreto que cazar**. Si lo probara contra código limpio, devolvería *"revisión limpia, sin hallazgos"* — pedagógicamente flojo. Con el anti-patrón deliberado, el alumno ve el reviewer en su mejor momento: detección + severidad + fix concreto. **Caso de prueba diseñado para showcasear el subagente**.

**¿Por qué revertir el `.Result` al final de la demo?**

Por dos razones:
1. **Disciplina de scope**: si la rama `demo/3.1b-after` quedara con el `.Result`, las demos siguientes arrastrarían un anti-patrón en el código. Las demos del módulo 4 y 5 podrían fallar test runs por culpa de eso.
2. **Pedagogía completa**: el ciclo es ver el problema, cazarlo con el reviewer, **aplicar el fix**. Si dejara el `.Result`, el ciclo queda incompleto.

**¿Por qué construir el frontmatter primero y luego el body, en lugar de juntos?**

Porque **el frontmatter es donde están las decisiones técnicas críticas** (tools, model). Si lo escribo todo de una sola vez, el alumno se pierde en el body y no asimila las decisiones del frontmatter. **Separarlos en dos pasos** hace que cada decisión tenga su momento. Bloque 4 dedica 2 minutos al frontmatter y 3 minutos al body.

**¿Por qué el body del `repo-explorer` define un formato de salida estructurado de 5 secciones?**

Porque **el destinatario del output es otro agente**, no un humano. El agente principal **necesita información estructurada** para tomar decisiones. Si el subagente devolviera narrativa libre, el principal tendría que parsearlo con razonamiento. **Formato fijo = parsing fácil = orquestaciones futuras más fiables** (3.2a). Esto justifica el peso del bloque "Formato de salida" en el body.

**¿Por qué el `dotnet-reviewer` tiene `Bash(git diff *)` y `Bash(git log *)` y no solo `Bash`?**

Porque **el principio de mínimo privilegio aplicado a subagentes** que la gamma 3.1b slide 29 marcó. `Bash` sin patrón = el subagente puede ejecutar cualquier cosa. Con patrón acotado a `git diff *` y `git log *`, el subagente **físicamente no puede ejecutar** otros comandos. Y son los que necesita para revisar — nada más.

**¿Por qué Tester y Planner se muestran como frontmatter solo, sin body?**

Porque **el frontmatter es donde están las decisiones que el alumno debe asimilar** (tools por rol, modelo por tipo de tarea). El body de cada uno es similar al patrón ya visto. Mostrar 4 bodies completos sería redundante. Frontmatter como **señal compacta de la decisión técnica**.

**¿Por qué los 8 anti-patrones se cubren como checklist sin desarrollar cada uno?**

Porque la gamma 3.1b slides 29-31 los desarrolló. **Repetirlos en directo sería redundancia**. Como checklist visual sirven de recordatorio rápido. **Densidad de contenido apropiada** para el bloque.

**¿Por qué el cliffhanger a 3.2a menciona específicamente "skill que invoca subagente"?**

Porque la gamma 3.1b slide 28 lo sembró literalmente como **el patrón potente** y la 3.2a lo va a desarrollar. **Sembrar un patrón concreto en el cliffhanger** prepara mentalmente al alumno. Y en el bloque 10 lo aterricé con un caso real: el skill `commit-style` invocando al `dotnet-reviewer` antes de proponer el mensaje. **Caso plausible que el alumno reconoce**.

**¿Por qué el `repo-explorer` ve por su cuenta el `.Result` aunque la pregunta es sobre el módulo Application?**

Porque la sección "Notas de cuidado" del body le pide **detectar señales de cuidado**: TODOs, FIXMEs, código frágil. Y `.Result` en código async **es código frágil que el subagente reconoce con su training**. Esto es **emergencia útil** — el subagente hace cosas que no le pediste explícitamente porque su rol está bien definido. **Pieza pedagógica bonus** que no estaba en la gamma pero refuerza el valor del subagente bien escrito.

**¿Por qué Pedro escribe los subagentes manualmente en VS Code y no usa "Generate with Claude" del menú `/agents`?**

Por dos razones pedagógicas:
1. **Mostrar las decisiones explícitamente**: si Claude lo genera, las decisiones de tools y modelo aparecen sin justificar. Manualmente, cada decisión se argumenta en directo.
2. **El alumno reproducirá manualmente**: cuando vuelva a su trabajo, va a abrir el editor y escribir. Mejor que vea el formato manual completo.

> La recomendación de la gamma 3.1b slide 8 (*"deja que Claude lo genere primero"*) **es válida para el lunes del alumno**, no para esta demo. La demo es **didáctica**, el lunes es **operativo**.
