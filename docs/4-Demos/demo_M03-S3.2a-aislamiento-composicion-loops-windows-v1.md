# Demo 3.2a — Aislamiento (`context: fork`), composición skill+subagente y loops de retroalimentación

> **Versión:** v1 | **Módulo:** 3 | **Sub:** 3.2a | **Estado:** ✅ Versión final
> **Archivo:** `demo_M03-S3.2a-aislamiento-composicion-loops-windows-v1.md`
> **Branch before:** `demo/3.2a-before`  (con `catch(Exception)` deliberado en CreateOrderHandler para que el loop lo cace)
> **Branch after:**  `demo/3.2a-after`   (estado final pre-cocinado: angular-component con `context: fork`, skill `pre-commit-check`, catch revertido)
> **Branch parent:** `demo/3.1b-after`
> **Tiempo total estimado:** ~26-30 minutos
> **Tipo:** Demo de composición (MIXTA: INFRA + CÓDIGO). **Es la demo que cierra el ciclo del harness desde dentro de una sesión: skills, subagentes y MCPs combinados en flujos coherentes.** Tres piezas: `context: fork` en skills, el patrón "skill que invoca subagente" materializado con un nuevo skill `pre-commit-check` que invoca al `dotnet-reviewer`, y un loop validator → implementer en un caso real con techo de iteraciones. Sigue el patrón **before/after** definido en [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md).
> **Plataforma:** Windows 11 (PowerShell 7).

---

## 1. Contexto

Cerramos la 3.1b con dos subagentes propios (`repo-explorer` y `dotnet-reviewer`) y la siembra del **patrón compuesto**: el skill `commit-style` que ya teníamos podría invocar al `dotnet-reviewer` antes de proponer el mensaje de commit. La gamma 3.1b slide 28 lo dijo explícitamente. La 3.2a desarrolla esa idea.

La gamma 3.2a (32 slides, ~30 min) cubrió cuatro piezas conceptuales:

1. **`context: fork` en skills** — el primer mecanismo de aislamiento: un skill se ejecuta en su propio contexto sin contaminar el principal. Filosóficamente cercano a un subagente pero envuelto en abstracción de skill (slides 4-9).
2. **Composición de capas: el patrón base** — el flujo end-to-end donde un skill **orquesta** subagentes y MCPs, cada capa con su responsabilidad. Esto es el **hierarchical/supervisory pattern** en literatura formal (slides 10-12).
3. **Caso real desarrollado: feature completa con orquestación** — un skill `feature-implementer` que invoca explorer → planner → tester → reviewer en serie (slides 13-18).
4. **Loops de retroalimentación** — el patrón validator → implementer con **techo de iteraciones** (vocabulario formal: **evaluator-optimizer**) más dos variantes (tester con autocorrección, plan re-validation) (slides 19-27).

Esta demo aterriza la teoría con tres construcciones progresivas:

- Convertir un skill existente (`angular-component`) a `context: fork` y mostrar la diferencia operativa.
- Crear un nuevo skill `pre-commit-check` que **invoca al `dotnet-reviewer`** — el patrón compuesto sembrado en 3.1b, ahora hecho.
- Añadir al mismo skill un **loop validator → implementer con techo de 3 iteraciones**.

> **Tipo de demo:** composición de piezas existentes. La rama `demo/3.2a-after` queda con `angular-component` actualizado a `context: fork` y un nuevo skill `pre-commit-check` que orquesta al `dotnet-reviewer` con loop de validación. **Es la primera demo del curso donde el alumno ve el harness compuesto funcionando**.

---

## 2. Objetivo de la demo

Cinco cosas concretas que tienen que quedar en la cabeza del alumno cuando termine los ~26 minutos de screencast:

1. **`context: fork` en skills es aislamiento de contexto sin crear subagente.** Para skills que leen mucho o producen output corto a partir de input grande. **No es lo mismo que subagente filosóficamente** — skill = tarea, subagente = rol. Decisión de framing.

2. **El patrón compuesto "skill que invoca subagente" es donde está la rentabilidad.** El skill orquesta, los subagentes ejecutan en aislamiento. **Lo mejor de los dos mundos**. Materializado en `pre-commit-check`.

3. **El skill orquestador define un workflow estandarizado y reproducible.** Una vez escrito, cada commit pasa por el mismo flujo sin que nadie tenga que recordarlo. **Equipo entero alineado**.

4. **Los loops siempre con techo.** *"Si tras 3 iteraciones siguen apareciendo hallazgos críticos, devuelve al usuario. No sigas iterando ciegamente."* Sin techo, bucles infinitos. Con techo, harness fiable.

5. **El frame del harness completo emerge en esta demo.** El alumno ve por primera vez **skill + subagente + loop trabajando juntos** sobre OrderManagement. No piezas sueltas — un sistema.

Y dos cosas que tienen que **NO quedar** en su cabeza:

- *"Cuanto más complejo el workflow, mejor."* — al revés. **La regla del manual línea 480**: *"empieza simple, escala solo cuando el caso lo justifique"*. La sobreingeniería es el primer enemigo.
- *"Si tengo `context: fork`, no necesito subagentes."* — falso. Son herramientas distintas para casos distintos. Skills con `context: fork` para tareas concretas que necesitan aislamiento. Subagentes para roles recurrentes.

---

## 3. Branch `demo/3.2a-before`

Punto de partida del screencast.

```
demo/3.2a-before
```

**Parte de:** `demo/3.1b-after`.

**Estado del repo:** todo lo de `demo/3.1b-after` (cuatro skills, dos subagentes, los componentes generados, `subagentes-explorados.md`) más un único cambio commiteado: en `src/OrderManagement.Application/Handlers/CreateOrderHandler.cs` se ha introducido un `try/catch(Exception)` genérico sin re-throw que silencia errores con `Console.WriteLine` y devuelve `0`. **Anti-patrón deliberado** — el loop `pre-commit-check` recién construido lo cazará en el screencast.

> El formador hace `git checkout demo/3.2a-before` antes de empezar a grabar.

---

## 4. Branch `demo/3.2a-after`

Estado final que la siguiente clase (3.2b) asume.

```
demo/3.2a-after
```

**Parte de:** `demo/3.2a-before`.

**Qué añade respecto a `-before`:**

1. **`angular-component` actualizado a `context: fork`** — el skill que más lee del repo ahora aísla.
2. **Skill nuevo `pre-commit-check`** en `.claude/skills/pre-commit-check/SKILL.md` — orquestador que invoca al `dotnet-reviewer`, con loop validator → implementer y techo de 3 iteraciones.
3. **Revert del `catch(Exception)`** — el handler vuelve a su forma idiomática (lo que el loop del `pre-commit-check` aplicaría al cazarlo).
4. **Marca `[x]`** en `docs/DEMOS.md` y `docs/subagentes-explorados.md` ampliado con notas de la composición.

**Cómo se prepara:** ver §6b. Se materializa antes de grabar.

> Durante la grabación, el formador construye el skill `pre-commit-check` y la actualización de `angular-component` en directo desde `demo/3.2a-before`. Ejecuta el loop sobre el `catch(Exception)` y aplica el fix. Al cerrar descarta los cambios reales y la siguiente clase parte de `demo/3.2a-after` ya pre-cocinada.

---

## 5. Estado del repo al hacer `git checkout demo/3.2a-before`

Idéntico a `demo/3.1b-after`, con un único cambio: el `catch(Exception)` deliberado en `CreateOrderHandler.cs`:

```
ordermanagement/
├── .claude/
│   ├── settings.json
│   ├── skills/
│   │   ├── angular-component/      (v4 con assets/ y scripts/)
│   │   ├── commit-style/
│   │   ├── db-reset/
│   │   └── frontend-design/
│   └── agents/
│       ├── repo-explorer.md
│       └── dotnet-reviewer.md
├── docs/
│   ├── DEMOS.md
│   ├── skills-explorados.md
│   ├── auditoria-skills-comunidad.md
│   └── subagentes-explorados.md
├── scripts/
├── src/
├── frontend/
├── tests/
├── CLAUDE.md
├── .gitignore
└── README.md
```

**Estado clave para esta demo:**

- **Cuatro skills + dos subagentes funcionales** — primera demo donde tenemos volumen suficiente para componer.
- Para probar el loop de `pre-commit-check` con un caso real, vamos a introducir **deliberadamente un anti-patrón** en `CreateOrderHandler.cs` (un `catch (Exception)` genérico sin re-throw) que el `dotnet-reviewer` cazará. **El loop debe iterar — primer hallazgo, fix, segundo pase limpio**.

**Estado de la máquina Windows del formador:**

```
✅ Claude Code v2.1.x con subagentes operativos
✅ Git for Windows
✅ PowerShell 7
✅ VS Code con el repo cargado en demo/3.2a-before
✅ Los dos subagentes de la 3.1b funcionando
```

**Lo que el alumno verá al final de la demo:**

- `angular-component` con la línea `context: fork` añadida y la justificación.
- El skill nuevo `pre-commit-check` escrito desde cero con orquestación explícita.
- La diferencia operativa entre `context: fork` y subagente explicada con la regla del manual.
- El loop validator → implementer en directo: anti-patrón cazado → fix aplicado → segundo pase limpio.
- El techo de iteraciones documentado y respetado.
- La siembra del paralelo (fan-out / fan-in) y context bank para 3.2b.

---

## 6a. Prompt para Claude Code — preparar `demo/3.2a-before`

> Crea la rama de partida del screencast desde `demo/3.1b-after` con un único commit: el `catch(Exception)` deliberado en `CreateOrderHandler` para que el loop del `pre-commit-check` recién construido lo cace. **No crea el skill `pre-commit-check` ni toca `angular-component`** — esa es la pieza viva.

````
Estoy preparando la demo 3.2a del curso de Claude Code (composición
skill+subagente y loops). Sigue el patrón before/after (ver demo M0.2).

Quiero que prepares la rama `demo/3.2a-before` desde `demo/3.1b-after`
con un único cambio commiteado: un catch(Exception) genérico sin re-throw
en CreateOrderHandler para que el loop del pre-commit-check recién
construido lo cace durante el screencast.

# Lo que necesito

Tres tareas:

## Tarea 1: crear la rama

```powershell
git checkout demo/3.1b-after
git pull
git checkout -b demo/3.2a-before
```

## Tarea 2: introducir el `catch(Exception)` en CreateOrderHandler

Localiza el método Handle de `ordermanagement/src/OrderManagement.Application/Handlers/CreateOrderHandler.cs`
y envuelve el cuerpo del método en un try/catch genérico que silencia
errores con Console.WriteLine y devuelve 0. Esquema esperado:

```csharp
public async Task<int> Handle(CreateOrderCommand request, CancellationToken ct)
{
    try
    {
        // [... cuerpo original ...]
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error creating order: {ex.Message}");
        return 0;
    }
}
```

## Tarea 3: verificar build y commitear

```powershell
dotnet build
```

Esperado: 0 warnings, 0 errors.

```powershell
Set-Location c:\w\repos\F-004-ClaudeCode
git add ordermanagement/src/OrderManagement.Application/Handlers/CreateOrderHandler.cs
git commit -m "demo/3.2a-before: anti-patrón catch(Exception) silencioso (deliberado para el loop)"
```

NO hagas push.

# Restricciones (importantes)

- NO crees `.claude/skills/pre-commit-check/`. Esa creación es la pieza viva.
- NO modifiques `angular-component` (la actualización a context: fork va en `-after`).
- NO marques `[x]` en docs/DEMOS.md todavía. Eso va en `-after`.
- NO modifiques los subagentes existentes ni CLAUDE.md ni settings.json.
- NO toques otros ficheros aparte de CreateOrderHandler.

# Cuando termines, dime

1. Que la rama demo/3.2a-before está creada desde demo/3.1b-after.
2. Que el catch(Exception) está commiteado.
3. Que `git log --oneline -1` muestra el commit del anti-patrón.
4. Que dotnet build pasa.
````

---

## 6b. Prompt para Claude Code — preparar `demo/3.2a-after`

> Materializa la rama final con el skill `pre-commit-check` pre-cocinado, `angular-component` actualizado a `context: fork`, y el `catch(Exception)` revertido. Equivalente a lo que el formador construirá / aplicará en vivo.

````
Estoy preparando la demo 3.2a del curso de Claude Code. Esta rama
-after pre-cocina la composición skill+subagente y loops que el
formador construirá en vivo durante el screencast.

# Contexto

Estoy en la rama `demo/3.2a-before` del repo `ordermanagement`. La rama
parte de `demo/3.1b-after` y tiene UN único cambio commiteado: el
catch(Exception) en CreateOrderHandler. NO tiene pre-commit-check.

Quiero que prepares la rama `demo/3.2a-after` desde `demo/3.2a-before`
con la actualización de angular-component, el skill pre-commit-check
nuevo, el revert del catch, DEMOS.md marcado y subagentes-explorados.md
ampliado.

# Lo que necesito

Cinco tareas:

## Tarea 1: crear la rama

```powershell
git checkout demo/3.2a-before
git checkout -b demo/3.2a-after
```

## Tarea 2: actualizar `ordermanagement/.claude/skills/angular-component/SKILL.md` con `context: fork`

Añade `context: fork` al frontmatter del SKILL.md existente. Mantén el
resto del frontmatter (`name`, `description`) y el body intactos.

## Tarea 3: crear `ordermanagement/.claude/skills/pre-commit-check/SKILL.md`

Skill orquestador que invoca al subagente `dotnet-reviewer` antes de cada
commit y aplica un loop validator → implementer con techo de 3 iteraciones:

- **Frontmatter:**
  - `name: pre-commit-check`
  - `description: Orquesta una revisión de los cambios staged invocando al subagente dotnet-reviewer, aplicando los fixes propuestos, y reiterando hasta que el reviewer no encuentre hallazgos CRÍTICOS o se llegue al techo de 3 iteraciones. Úsalo antes de cada commit en código C#/.NET.`
  - `context: fork`
- **Cuerpo:** define el workflow:
  1. Leer `git diff --cached`.
  2. Invocar al subagente `dotnet-reviewer` con el diff.
  3. Si reporta CRÍTICO: aplicar fix automáticamente y volver al paso 1 (incrementar contador).
  4. Si contador > 3: parar, reportar al usuario y NO commitear.
  5. Si reviewer no reporta CRÍTICO: ok, proceder al commit.

## Tarea 4: revertir el `catch(Exception)` en CreateOrderHandler

Vuelve a poner el cuerpo del método sin try/catch genérico. Equivalente
al fix que el loop aplicaría al cazarlo.

## Tarea 5: marcar DEMOS.md, ampliar subagentes-explorados.md, build y commit

Marca la 3.2a en `docs/DEMOS.md`:

```
- [x] **demo/3.2a-before / demo/3.2a-after** — Composición skill+subagente y loops con techo
```

Añade a `docs/subagentes-explorados.md` una sección «### Composición en 3.2a» que documente:
- `angular-component` ahora con `context: fork`.
- Skill nuevo `pre-commit-check` que orquesta `dotnet-reviewer` con loop techo=3.

Verifica con `dotnet build` desde `ordermanagement/` (0 warnings, 0 errors) y commit desde la raíz del curso:

```powershell
Set-Location c:\w\repos\F-004-ClaudeCode
git add ordermanagement/.claude/skills `
        ordermanagement/src/OrderManagement.Application/Handlers/CreateOrderHandler.cs `
        docs/DEMOS.md docs/subagentes-explorados.md
git commit -m "demo/3.2a-after: angular-component context:fork + pre-commit-check con loop"
```

NO hagas push.

# Restricciones (importantes)

- NO modifiques los subagentes existentes ni CLAUDE.md ni settings.json.
- NO toques otros ficheros del código aparte del revert del catch.
- El skill pre-commit-check debe respetar las 5 reglas técnicas críticas
  (kebab-case, sin XML, description bajo 1024 chars, etc.).

# Cuando termines, dime

1. Que la rama demo/3.2a-after está creada desde demo/3.2a-before.
2. Que `angular-component/SKILL.md` tiene `context: fork` en el frontmatter.
3. Que `.claude/skills/pre-commit-check/SKILL.md` existe con el workflow.
4. Que CreateOrderHandler ya no tiene `catch(Exception)`.
5. Que docs/DEMOS.md está marcado.
6. Que docs/subagentes-explorados.md está ampliado.
7. Que dotnet build pasa.
8. Que el commit está hecho.

Si tienes dudas, para y pregúntame.
````

---

## 7. Artefactos que Claude Code debe generar

```
✓ Rama demo/3.2a-before (parte de demo/3.1b-after) con UN commit:
  └── src/OrderManagement.Application/Handlers/CreateOrderHandler.cs
      (catch(Exception) silencioso deliberado, commiteado)
✓ Rama demo/3.2a-after (parte de demo/3.2a-before) con UN commit:
  ├── .claude/skills/angular-component/SKILL.md (context: fork añadido)
  ├── .claude/skills/pre-commit-check/SKILL.md (nuevo orquestador con loop)
  ├── src/OrderManagement.Application/Handlers/CreateOrderHandler.cs (revert catch)
  ├── docs/DEMOS.md con 3.2a marcada como [x]
  └── docs/subagentes-explorados.md ampliado
✓ Verificación de build OK
```

**Lo que NO debe haber generado:**

- ❌ `.claude/skills/pre-commit-check/` (creación en vivo)
- ❌ Modificación a `angular-component` (en vivo)
- ❌ El catch commiteado (debe quedar sin stage)
- ❌ Cambios en otros ficheros .NET, skills, agents, CLAUDE.md, settings

> Si Claude Code se anticipa, **se rechaza el output**.

**Lo que el formador commitea EN VIVO sobre `demo/3.2a-before` durante el screencast:**

```
Durante la grabación, sobre demo/3.2a-before, se hacen commits ficticios:

1. "demo/3.2a-after: angular-component con context: fork"
   └── .claude/skills/angular-component/SKILL.md (MODIFICADO, +1 línea)

2. "demo/3.2a-after: skill pre-commit-check orquestador con loop"
   └── .claude/skills/pre-commit-check/SKILL.md (NUEVO)
   └── docs/subagentes-explorados.md (MODIFICADO con notas)

3. "demo/3.2a-after: revierte catch(Exception) en CreateOrderHandler (lo cazó el loop)"
   └── src/OrderManagement.Application/Handlers/CreateOrderHandler.cs
       (revertido al estado de demo/3.1b-after)

Al cerrar el screencast: el formador descarta los commits reales.
La siguiente clase parte de demo/3.2a-after (pre-cocinada en §6b)
que es equivalente al resultado del screencast.
```

**Estado final del árbol después del screencast:**

```
ordermanagement/
├── .claude/
│   ├── settings.json
│   ├── skills/
│   │   ├── angular-component/                  ← MODIFICADO (context: fork)
│   │   │   └── SKILL.md
│   │   ├── commit-style/
│   │   ├── db-reset/
│   │   ├── frontend-design/
│   │   └── pre-commit-check/                   ← NUEVO
│   │       └── SKILL.md
│   └── agents/
│       ├── repo-explorer.md
│       └── dotnet-reviewer.md
├── docs/
│   ├── DEMOS.md
│   └── subagentes-explorados.md                ← MODIFICADO
└── src/OrderManagement.Application/Handlers/
    └── CreateOrderHandler.cs                   (revertido)
```

---

## 8. Guion del screencast

**Duración estimada del screencast: ~24-28 minutos.**

Diez bloques. La demo combina conceptos de las dos demos anteriores en un solo workflow.

> **Antes de empezar a grabar**, asegúrate de:
> - Tener PowerShell 7 abierto en pantalla completa, fuente al menos 16pt.
> - Tener VS Code abierto con el repo en `demo/3.2a-before`.
> - **Verificar** que el `catch(Exception)` está sin stagear: `git status` debe mostrar `modified: src/OrderManagement.Application/Handlers/CreateOrderHandler.cs`.
> - **Verificar** que `git diff` muestra el cambio claramente.
> - Cerrar Slack, Teams, navegadores con notificaciones.

---

### Bloque 1 — Setup, recap del frame del harness y objetivo (~2 min)

**Pantalla compartida.** A la izquierda VS Code con el repo en `demo/3.2a-before`. A la derecha terminal PowerShell.

**En la terminal:**

```powershell
git status
ls .claude\skills\
ls .claude\agents\
```

```
On branch demo/3.2a
Changes not staged for commit:
        modified:   src/OrderManagement.Application/Handlers/CreateOrderHandler.cs

# .claude/skills/
angular-component, commit-style, db-reset, frontend-design

# .claude/agents/
repo-explorer.md, dotnet-reviewer.md
```

**Lo que dices:**

> "Estamos en la rama `demo/3.2a-before`. **Recuento rápido**: cuatro skills, dos subagentes, y un commit reciente con un anti-patrón deliberado en `CreateOrderHandler.cs`. Hoy vamos a componerlos.
>
> Recordad el frame del módulo 3 — **agent = model + harness**. Las primeras dos demos del módulo 3 nos dejaron piezas sueltas: subagentes (3.1a, 3.1b). El módulo 2 nos dejó skills. **Hasta aquí cada pieza vivía aislada**. Esta demo es **donde el harness empieza a parecer un harness**.
>
> Tres cosas en estos minutos:
>
> Una. **`context: fork` en skills** — el primer mecanismo de aislamiento. Convertimos el `angular-component` a `context: fork` y vemos qué cambia.
>
> Dos. **El patrón compuesto skill + subagente** — el que sembré al cierre de la 3.1b. Voy a crear un skill nuevo `pre-commit-check` que **invoca al `dotnet-reviewer`** antes de cada commit. Workflow estandarizado, reproducible, todo el equipo alineado.
>
> Tres. **Loop validator → implementer con techo**. El skill `pre-commit-check` no se queda en *'el reviewer dijo que mal, te lo cuento'*. Aplica el fix automáticamente y vuelve a invocar al reviewer. Si tras tres iteraciones sigue mal, para. **Techo de iteraciones, sin loops infinitos**.
>
> Y mirad lo que tenemos sin commitear — `CreateOrderHandler.cs`. Hay un anti-patrón deliberado que el `pre-commit-check` va a cazar y arreglar end-to-end. Vamos a verlo en vivo."

**Tiempo:** ~2 minutos.

---

### Bloque 2 — `context: fork` en `angular-component` (~3 min)

> "Empezamos por lo más simple. **`context: fork` en un skill existente**. La gamma 3.2a slide 4 lo cubrió."

**En el editor de texto al lado, escribo (contenido pedagógico):**

```
context: fork EN SKILLS

¿Qué hace?
─────────
  Skill se ejecuta en CONTEXTO AISLADO.
  ├── Sus instrucciones, lecturas, razonamiento → fuera del principal
  └── Solo el RESULTADO FINAL vuelve al principal

¿Cuándo usarlo?
───────────────
  ✓ Skill que LEE MUCHO (decenas de ficheros)
  ✓ Skill que produce OUTPUT CORTO desde INPUT GRANDE
  ✓ Skill activable en sesiones largas

¿Cuándo NO?
───────────
  ✗ Skill rápido y simple (genera componente, formatea commit)
    → overhead no compensa
  ✗ Skill cuyo output va a ser modificado en seguida
    → mantén integración tightly acoplada

DIFERENCIA CON SUBAGENTE
────────────────────────
  Skill con context: fork = "haz esto en otro lado y dame el resultado"
                            (define una TAREA)
  Subagente               = "encárgate de cosas como esta"
                            (define un ROL)

  Cercanos en la práctica.
  Decisión de framing.
```

> "**Aquí la decisión clave**: el `angular-component` lee bastante para generar componentes. Mira plantillas, mira componentes existentes para imitar el estilo, mira CLAUDE.md. Carga peso al contexto principal. **Es candidato a `context: fork`**."

**Voy a VS Code. Abro `.claude/skills/angular-component/SKILL.md` y modifico SOLO el frontmatter:**

**ANTES:**

```yaml
---
name: angular-component
description: Genera componentes Angular standalone con Signals siguiendo las convenciones estrictas del equipo OrderManagement. Usar cuando el usuario pida crear un nuevo componente Angular...
---
```

**DESPUÉS:**

```yaml
---
name: angular-component
description: Genera componentes Angular standalone con Signals siguiendo las convenciones estrictas del equipo OrderManagement. Usar cuando el usuario pida crear un nuevo componente Angular...
context: fork
---
```

**Salvo. Cambio mínimo — una línea.**

> "**Una línea.** `context: fork`. Eso es todo. La próxima vez que el `angular-component` se active, va a leer las plantillas de `assets/`, mirar componentes existentes, ejecutar el script `generate.py`... **todo en un contexto aislado**. Y solo me devuelve el resultado: los cuatro ficheros generados.
>
> ¿Por qué hago esto al `angular-component` y no al `commit-style`? Porque el `commit-style` lee dos cosas (`git diff --cached` y `git status`) y produce una línea. **Es rápido y simple**. La gamma slide 8 lo dijo: *'overhead no compensa'*. **Mantenedlo sin fork**.
>
> Y `db-reset` tampoco lo necesita — tiene `disable-model-invocation: true`, ejecuta dos comandos. No hay exploración pesada. **Cada decisión, un caso**.
>
> Vamos al patrón compuesto."

**Tiempo:** ~3 minutos.

---

### Bloque 3 — El patrón base: skill que orquesta subagentes (~3 min)

> "Antes de construir el `pre-commit-check`, **el patrón base** que la gamma 3.2a slide 10 introdujo."

**En el editor:**

```
COMPOSICIÓN DE CAPAS — el patrón base

[Usuario]
    ↓
[Agente principal]
    ↓
[Skill: orquestador / initiator]
    ↓
[Subagente A]   [Subagente B]   [MCP Server]
    ↓               ↓                ↓
   resultado A   resultado B    datos externos
    ↓               ↓                ↓
    [Skill recoge resultados]
            ↓
    [Devuelve al principal]


Cada flecha = punto donde el contexto se aísla o se transfiere.

El agente principal solo ve los RESULTADOS DESTILADOS,
no las exploraciones intermedias.


VOCABULARIO FORMAL
──────────────────
En literatura de arquitectura agentic se llama
HIERARCHICAL o SUPERVISORY pattern.

  Skill orquestador  =  supervisor
  Subagentes         =  specialists


LO QUE GANAMOS
──────────────
  ✓ Workflow estandarizado y reproducible
  ✓ Cada feature pasa por el mismo flujo
  ✓ Sin que nadie tenga que recordarlo
  ✓ Equipo entero alineado
```

> "**Esto es lo que vamos a construir**: un skill que actúa como **supervisor**. Invoca al `dotnet-reviewer` (specialist), recoge sus hallazgos, decide si aplicar fixes o devolver al usuario, y vuelve a invocar al reviewer en una segunda pasada. **Loop con techo**. Lo orquesta el skill.
>
> Y la gamma 3.2a slide 12 le pone nombre: **hierarchical/supervisory pattern**. Vais a verlo así en whitepapers de arquitectura agentic. Aquí lo aplicamos en su versión concreta dentro de Claude Code.
>
> Vamos a escribirlo."

**Tiempo:** ~3 minutos.

---

### Bloque 4 — Construir `pre-commit-check`: orquestador básico (~5 min)

**En PowerShell:**

```powershell
mkdir .claude\skills\pre-commit-check
```

**En VS Code, creo `.claude/skills/pre-commit-check/SKILL.md`:**

```markdown
---
name: pre-commit-check
description: Orquesta una validación completa de los cambios staged antes de un commit. Invoca al subagente dotnet-reviewer, aplica fixes automáticos para hallazgos críticos, y solo aprueba el commit cuando la revisión queda limpia. Usar antes de cada commit, especialmente en branches que vayan a PR.
allowed-tools: Read, Edit, Write, Bash(git diff *), Bash(git status), Bash(git add *), Bash(git log *)
---

# Pre-commit check — orquestador con loop validator → implementer

Este skill estandariza la validación pre-commit del equipo OrderManagement.
Invoca al subagente `dotnet-reviewer`, aplica fixes para hallazgos
críticos, y solo aprueba cuando la revisión queda limpia.

## Cuándo se usa

Antes de cada commit en branches importantes (no en commits triviales
de docs ni de README). El usuario puede pedirlo explícitamente o
configurar este skill para que se active automáticamente vía hook
(módulo 3.3).

## Workflow

### Paso 1: Verificar que hay cambios staged

```!
git status
git diff --cached --stat
```

Si no hay cambios staged, devuelve: *"No hay cambios staged. Stagea los
cambios primero con `git add` antes de invocar este skill."* y termina.

### Paso 2: Primera invocación del reviewer

Invoca al subagente `dotnet-reviewer`:

> *"Revisa los cambios staged actuales. Devuelve los hallazgos
> clasificados por severidad."*

Recibirás un reporte con hallazgos clasificados como CRÍTICO, IMPORTANTE
o SUGERENCIA.

### Paso 3: Decisión sobre los hallazgos

Tres casos posibles:

**Caso A — Sin hallazgos críticos:**
Procede al paso 5 (cierre).

**Caso B — Hay hallazgos críticos:**
Aplica los fixes propuestos por el reviewer en los ficheros afectados.
Para cada fix:
1. Lee el fichero.
2. Aplica el cambio sugerido (Edit).
3. Re-stagea el fichero (`git add <fichero>`).

Vuelve al paso 2 (segunda invocación).

**Caso C — Hay hallazgos importantes pero no críticos:**
Pregunta al usuario si quiere aplicar los fixes o ignorarlos.
Procede según su decisión. No iteres automáticamente para
hallazgos importantes — el criterio es del usuario.

### Paso 4: Loop con techo

El loop entre paso 2 y paso 3 (caso B) tiene **techo de 3 iteraciones**.

Si tras 3 iteraciones siguen apareciendo hallazgos CRÍTICOS:
1. Para el loop.
2. Devuelve al usuario un resumen del problema:
   - Qué hallazgos persisten
   - Qué fixes ya se intentaron
   - Por qué probablemente no convergen
3. NO procedas al commit. El usuario decide qué hacer.

### Paso 5: Cierre

Cuando la revisión queda limpia (sin hallazgos críticos):

1. Confirma al usuario:
   *"Revisión limpia tras N iteraciones. Cambios listos para commit."*
2. NO ejecutes `git commit` por tu cuenta.
3. Sugiere el comando exacto:
   *"Cuando estés listo, lanza: `git commit -m \"<tu mensaje>\"`."*

## Restricciones

- **No commitees automáticamente.** El commit es decisión del usuario.
- **Respeta el techo de iteraciones.** Si no converge en 3 vueltas, para.
- **No modifiques ficheros fuera del scope de los hallazgos.** Solo
  aplicas los fixes que el reviewer propuso.
- **Si el reviewer falla** (error de invocación, output corrupto),
  devuelve al usuario sin asumir nada.

## Ejemplo de flujo exitoso

```
Usuario: > /pre-commit-check
Skill:   Verificando cambios staged... 1 fichero modificado.
         Invocando dotnet-reviewer (iteración 1)...
         Reviewer encontró 1 hallazgo CRÍTICO en CreateOrderHandler.cs:8
         Aplicando fix sugerido...
         Re-stageando CreateOrderHandler.cs...
         Invocando dotnet-reviewer (iteración 2)...
         Reviewer: "Revisión limpia. Sin hallazgos."
         ✅ Cambios listos para commit. Lanza: git commit -m "<mensaje>"
```
```

**Salvo el fichero.**

> "Mirad la estructura. **Cinco pasos**.
>
> Paso 1: verificar staged. Si no hay nada, sale.
>
> Paso 2: invocar `dotnet-reviewer`. Recibe hallazgos.
>
> Paso 3: tres casos según severidad — sin críticos (procede), con críticos (aplica fixes y vuelve al 2), con importantes (pregunta al usuario, sin loop automático).
>
> Paso 4: **el techo**. *'Tres iteraciones máximo'*. La gamma 3.2a slide 26 lo subrayó: *'pon loops, pero siempre con techo'*.
>
> Paso 5: cierre. **No commitea automáticamente**. La gamma slide 30 lo marcó como anti-patrón: *'pretender que la auto-delegación entre subagentes es perfecta'*. El skill **propone**, el usuario **decide**.
>
> Y mirad las restricciones al final. Cuatro guard rails. **No commitee, respeta techo, no modifique fuera de scope, falle bien si el reviewer rompe**.
>
> Vamos a probarlo con el caso real que dejé sin stagear."

**Tiempo:** ~5 minutos.

---

### Bloque 5 — Probar el orquestador end-to-end (~5 min)

**En la terminal:**

```powershell
git status
git diff
```

```
modified:   src/OrderManagement.Application/Handlers/CreateOrderHandler.cs

@@ -10,12 +10,18 @@
 public async Task<int> Handle(CreateOrderCommand request,
                               CancellationToken cancellationToken)
 {
+    try
+    {
         var order = new Order(request.CustomerId, request.Items);
         await _repository.AddAsync(order, cancellationToken);
         await _repository.SaveChangesAsync(cancellationToken);
         return order.Id;
+    }
+    catch (Exception ex)
+    {
+        Console.WriteLine($"Error creating order: {ex.Message}");
+        return 0;
+    }
 }
```

> "Mirad el diff. **Anti-patrón clásico**: `catch (Exception)` genérico que **traga la excepción**, hace `Console.WriteLine` (no log estructurado) y **devuelve 0** como si el order tuviera id 0. **Triple problema**: catch genérico, log silencioso, valor de retorno mentiroso.
>
> El `dotnet-reviewer` debería cazarlo. Vamos a stagear y a invocar el orquestador:"

```powershell
git add src/OrderManagement.Application/Handlers/CreateOrderHandler.cs
claude
```

```
✓ 5 project skills loaded: angular-component, commit-style, db-reset,
                          frontend-design, pre-commit-check
✓ 2 project agents loaded: repo-explorer, dotnet-reviewer
```

> "Cinco skills, dos subagentes. **Todo cargado**. Invoco:"

**Tecleo:**

```
> /pre-commit-check
```

**Aparece secuencia (output ejemplo):**

```
● [Skill activated explicitly: pre-commit-check]

Verificando cambios staged...
● Bash(git status)
● Bash(git diff --cached --stat)

1 fichero modificado:
  src/OrderManagement.Application/Handlers/CreateOrderHandler.cs (+8, -2)

Invocando dotnet-reviewer (iteración 1)...
● Launching dotnet-reviewer subagent...

  [Subagent in isolated context]
  
● dotnet-reviewer returned:

  HALLAZGOS DE LA REVISIÓN
  ────────────────────────
  
  [CRÍTICO]
    - CreateOrderHandler.cs:14
      Problema: catch (Exception) genérico que traga la excepción.
                Viola convención del equipo (CLAUDE.md):
                "NUNCA catch (Exception) genérico sin re-throw o log explícito"
      Fix sugerido: o bien re-throw después de log estructurado,
                    o bien catch específico (DbException, ValidationException)
                    según la causa esperada. Para este caso, re-throw es
                    lo correcto:
                    
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error creating order {CustomerId}", 
                                        request.CustomerId);
                        throw;
                    }
      
    - CreateOrderHandler.cs:16
      Problema: Console.WriteLine en código de producción.
                CLAUDE.md sección Logging: "Usar ILogger<T> inyectado"
      Fix sugerido: inyectar ILogger<CreateOrderHandler> en el constructor
                   y usar _logger.LogError o _logger.LogWarning según severidad.
    
    - CreateOrderHandler.cs:17
      Problema: return 0 como valor de error oculta el fallo real.
                El llamador no distingue id=0 (error) de id real.
      Fix sugerido: re-throw permite al middleware de errores decidir
                   el mapeo (ProblemDetails 500 o lo que aplique).

Aplicando fixes (iteración 1)...
● Edit(CreateOrderHandler.cs)
  - Cambiando catch genérico a re-throw con log estructurado
  - Reemplazando Console.WriteLine por _logger.LogError
  - Eliminando return 0 (queda solo return order.Id)

● Bash(git add src/OrderManagement.Application/Handlers/CreateOrderHandler.cs)

Re-invocando dotnet-reviewer (iteración 2)...
● Launching dotnet-reviewer subagent...

● dotnet-reviewer returned:
  Revisión limpia. Sin hallazgos.

✅ Cambios listos para commit tras 2 iteraciones.
   Cuando estés listo, lanza: git commit -m "<tu mensaje>"
```

> "**Mirad lo que ha pasado.**
>
> Iteración 1: el reviewer encontró tres hallazgos críticos. Los tres relacionados con el anti-patrón. **El skill aplicó los tres fixes** automáticamente y re-stageó.
>
> Iteración 2: el reviewer dijo *'revisión limpia'*.
>
> El skill cerró con *'cambios listos para commit'* y **NO commiteó por su cuenta**. Me dejó la decisión.
>
> **Esto es el patrón compuesto funcionando**. Skill orquesta, subagente ejecuta en aislamiento, loop con techo, decisión final del usuario. **El alumno ve por primera vez el harness compuesto en directo**.
>
> Voy a verificar el estado final del fichero:"

**Salgo (Ctrl+C). En la terminal:**

```powershell
git diff --cached
```

**Aparece el diff con los fixes aplicados** — re-throw con `_logger.LogError`, sin `Console.WriteLine`, sin `return 0`.

> "**Diff limpio.** Los fixes están aplicados. El reviewer está contento. Si yo lanzo `git commit` ahora, el commit lleva código bien. **Sin que yo haya tenido que arreglarlo a mano.**"

**Tiempo:** ~5 minutos.

---

### Bloque 6 — El loop a coste cero: heurística práctica (~2 min)

> "Antes de seguir, **una nota importante** que la gamma 3.2a slide 26-27 marcó. **Los loops cuestan tokens**. Cada iteración es otra invocación al subagente."

**En el editor:**

```
LOOPS A COSTE CERO — la heurística práctica

Cada vuelta de un loop:
  → 1 invocación más al subagente
  → más tokens
  → más tiempo

REGLA PRÁCTICA:

  Loops cortos (max 2-3 iteraciones)
    ✓ Baratos
    ✓ Casi siempre rentan
    ✓ Coste extra pequeño, fiabilidad mucho mejor

  Loops largos (5+ iteraciones)
    ✗ Rara vez compensan
    ✗ Si necesitas tantas vueltas:
      → el problema es la CALIDAD del subagente
      → o la CALIDAD del plan inicial
      NO la cantidad de iteraciones

  ⚠️ Siempre con techo.

  Si te encuentras subiendo el techo
  porque el loop no converge
  → el problema está en otra parte.
```

> "**Tres en `pre-commit-check` es razonable**. Si el reviewer y los fixes no convergen en tres vueltas, **el problema está en otro lado** — quizás el reviewer está mal afinado, quizás el código tiene un problema estructural que no se arregla con fix puntual, quizás el plan inicial estaba mal.
>
> **Subir el techo a cinco no soluciona el problema. Subir el techo a diez tampoco**. La regla del manual línea 240: *'si te encuentras subiendo el techo, el problema está en otra parte'*. Y casi siempre es verdad."

**Tiempo:** ~2 minutos.

---

### Bloque 7 — Skill con context:fork vs subagente: la decisión (~2 min)

> "Una pregunta que el alumno se hace ahora: **¿cuándo skill con `context: fork` y cuándo subagente?**. La gamma 3.2a slide 9 lo cubrió y conviene tenerlo claro."

**En el editor:**

```
SKILL con context: fork  vs  SUBAGENTE

Filosóficamente:

  Skill con fork  = "haz ESTO en otro lado y dame el resultado"
                    Define una TAREA.

  Subagente       = "encárgate de COSAS COMO ESTA"
                    Define un ROL.


En la práctica son cercanos. Decisión de framing.

RECOMENDACIÓN:

  Subagentes para:
    • Roles recurrentes (Reviewer, Tester, Explorer, Planner)
    • Tareas que se invocan en múltiples contextos
    • Decisiones con criterio (no solo procesamiento)

  Skills con context: fork para:
    • Tareas concretas que se invocan una vez
    • Procesamiento de input grande → output corto
    • Cuando la abstracción "skill" es la natural


EJEMPLOS DE NUESTRO REPO:

  angular-component (con fork)  → genera componente Angular
                                   tarea concreta, output específico
  
  dotnet-reviewer (subagente)   → revisa código C#/.NET
                                   rol recurrente, criterio propio
  
  pre-commit-check (skill)      → orquesta el reviewer
                                   ¿pero NO con fork?
```

> "Atención al último ejemplo. **`pre-commit-check` es un skill orquestador, no tiene `context: fork`**. ¿Por qué? Porque **el skill orquestador necesita ver lo que hace** — necesita aplicar fixes, modificar ficheros, re-stagear. Eso pasa **en el contexto principal porque tú quieres ver los cambios**. La gamma 3.2a slide 8 lo dijo: *'skill cuyo output va a ser modificado en seguida → no fork'*.
>
> El reviewer **sí está aislado** — es subagente con su propio contexto. Pero el orquestador, no. **Cada decisión, su justificación**."

**Tiempo:** ~2 minutos.

---

### Bloque 8 — Commit y notas en `subagentes-explorados.md` (~1 min 30 seg)

**En VS Code, abro `docs/subagentes-explorados.md` y añado al final:**

```markdown

---

# Demo 3.2a — Composición y loops

## Cambios introducidos

### `angular-component` actualizado a `context: fork`

Una línea añadida al frontmatter. Justificación: el skill lee plantillas
en `assets/`, mira componentes existentes, ejecuta `generate.py`. Lectura
pesada. Output: cuatro ficheros generados. Aislar el contexto evita que
ese peso aparezca en el principal.

### Skill nuevo: `pre-commit-check`

Orquestador con loop validator → implementer.

- **Workflow**: cinco pasos (verificar staged → invocar reviewer →
  aplicar fixes / pregunta usuario / proceder según severidad → loop
  con techo → cierre).
- **Subagente invocado**: `dotnet-reviewer` (creado en 3.1b).
- **Techo**: 3 iteraciones máximo.
- **Decisión final**: del usuario, no del skill (no commitea
  automáticamente).

## Caso real probado

Anti-patrón introducido deliberadamente en `CreateOrderHandler.cs`:

- `catch (Exception)` genérico
- `Console.WriteLine` en código de producción
- `return 0` como valor de error

Resultado del orquestador end-to-end:

1. Iteración 1: reviewer cazó 3 hallazgos CRÍTICO.
2. Skill aplicó los 3 fixes automáticamente (re-throw con log
   estructurado, ILogger en lugar de Console, sin return 0).
3. Iteración 2: reviewer "revisión limpia, sin hallazgos".
4. Skill cerró con "cambios listos para commit", sin commitear.

## Lecciones extraídas

1. **El patrón hierarchical/supervisory** (skill orquesta, subagentes
   ejecutan) estandariza workflows del equipo.
2. **El loop con techo** es la diferencia entre un harness frágil
   (cualquier fallo lo rompe) y uno fiable.
3. **El skill orquestador NO se aísla** con `context: fork` — el
   usuario necesita ver lo que hace para confirmar.
4. **`context: fork` es para skills que leen mucho y devuelven poco**;
   `angular-component` califica, `commit-style` no.

## Próximo paso

En la 3.2b vamos a ver:
- Paralelo (fan-out / fan-in) vs serial.
- Context bank: artefactos durables como memoria compartida entre
  subagentes.
- Claude Code como MCP server.
- Agent Teams (referencia experimental).
```

**Salvo. En la terminal:**

```powershell
git add .claude/skills/ docs/subagentes-explorados.md
git status
```

```
On branch demo/3.2a
Changes to be committed:
        modified:   .claude/skills/angular-component/SKILL.md
        new file:   .claude/skills/pre-commit-check/SKILL.md
        modified:   docs/subagentes-explorados.md
        modified:   src/OrderManagement.Application/Handlers/CreateOrderHandler.cs
```

> "El handler también está staged porque lo editamos durante el screencast aplicando los fixes. **Está bien — el código quedó arreglado**. Pero quiero que la rama final quede como demo/3.1b (sin los cambios al handler) para que no contamine demos siguientes. **Reverteo** ese cambio:"

```powershell
git restore --staged src/OrderManagement.Application/Handlers/CreateOrderHandler.cs
git restore src/OrderManagement.Application/Handlers/CreateOrderHandler.cs
git status
```

```
Changes to be committed:
        modified:   .claude/skills/angular-component/SKILL.md
        new file:   .claude/skills/pre-commit-check/SKILL.md
        modified:   docs/subagentes-explorados.md
```

```powershell
git commit -m "demo/3.2a-after: angular-component context:fork + pre-commit-check con loop"
```

> "Commit hecho. **Tres cambios**: `angular-component` con fork, `pre-commit-check` nuevo, notas actualizadas. **El handler vuelve a su estado de demo/3.1b** — la pedagogía del loop se vio, pero la rama queda limpia."

**Tiempo:** ~1 minuto 30 segundos.

---

### Bloque 9 — Recap y los anti-patrones de orquestación (~2 min)

> "Cinco ideas para llevarse al lunes. Y dos anti-patrones para evitar."

**En el editor:**

```
LO QUE TIENES TRAS LA 3.2a

1. context: fork EN SKILLS
   Aislamiento de contexto sin crear subagente.
   Para skills que leen mucho y devuelven poco.

2. EL PATRÓN COMPUESTO
   Skill que invoca subagente.
   Skill orquesta, subagente ejecuta en aislamiento.
   Lo mejor de los dos mundos.

3. WORKFLOW ESTANDARIZADO
   Una vez escrito el orquestador,
   cada commit pasa por el mismo flujo.
   Sin que nadie tenga que recordarlo.

4. LOOPS CON TECHO
   Validator → implementer → validator.
   Máximo 3 iteraciones.
   Si no converge, el problema está en otra parte.

5. EL SKILL ORQUESTADOR NO SE AÍSLA
   Necesita ver lo que hace.
   El subagente que invoca SÍ está aislado.


ANTI-PATRONES DE ORQUESTACIÓN (gamma 3.2a slide 30)

❌ SOBREINGENIERÍA DESDE EL DÍA UNO
   Skill que orquesta 5 subagentes para tarea simple.
   La orquestación es una herramienta, no un objetivo.

❌ CADENAS DEMASIADO LARGAS
   Skill → subagente → skill → subagente → MCP.
   Cada eslabón = punto de rotura.
   Máximo 2-3 niveles de profundidad.

❌ LOOPS SIN TECHO
   Bucle infinito esperando.
   Siempre con máximo de iteraciones.

❌ FALTA DE OBSERVABILIDAD
   ¿Cómo sabes en qué eslabón falló?
   Ahí entra el context bank (3.2b).

❌ SUBAGENTES QUE SE SOLAPAN
   Reviewer + CodeQualityChecker + SecurityAuditor
   con descripciones similares.
   Auto-delegación falla.
```

> "**Cinco ideas, cinco anti-patrones**. La regla de oro: **empieza simple, escala solo cuando el caso lo justifique**. La sobreingeniería es el primer enemigo. La gamma 3.2a slide 30 lo dijo claramente."

**Tiempo:** ~2 minutos.

---

### Bloque 10 — Cliffhanger a 3.2b (~1 min 30 seg)

> "En la siguiente demo, la **3.2b**, vamos a cerrar la orquestación con cuatro temas que faltan:
>
> Una. **Paralelo vs serial: fan-out / fan-in**. Hoy hicimos un workflow lineal — paso 1 → 2 → 3 → 4 → 5. Pero hay tareas que se pueden ejecutar en paralelo y reducir el tiempo total. La regla rápida: si la salida de A condiciona cómo B trabaja, es serial. Si A y B se combinan al final pero no se influyen, paralelo. Veremos un caso concreto sobre OrderManagement.
>
> Dos. **Context bank**. Los workflows compuestos necesitan compartir información entre subagentes — el plan, los hallazgos, los ficheros tocados. Pasarlo por prompts es ineficiente. La alternativa: **artefactos durables en `.claude/workflow-state/`** que actúan como memoria compartida. Trazabilidad, recuperación si la sesión muere a la mitad, loops baratos.
>
> Tres. **Claude Code como MCP server**. La capa más arriba — exponer Claude Code para que otros agentes hablen con él.
>
> Cuatro. **Agent Teams** — el patrón experimental de Anthropic donde múltiples sesiones de Claude Code se coordinan entre sí. Y el dato honesto: **multi-agente cuesta 10-15 veces más tokens** según el whitepaper de Anthropic. Cuándo merece la pena y cuándo no.
>
> Empezamos con el **tres punto dos punto B**."

**Tiempo:** ~1 minuto 30 segundos.

---

## 9. Qué resaltar verbalmente

Cinco puntos que **no pueden quedarse sin decir**:

1. **"`context: fork` = una línea. Aísla un skill sin crear subagente."** — la pieza más simple. Bloque 2.

2. **"El skill orquesta. El subagente ejecuta en aislamiento. Lo mejor de los dos mundos."** — el patrón compuesto. Bloques 3 y 4.

3. **"Loops siempre con techo. Si no converge en 3 vueltas, el problema está en otra parte."** — la regla operativa de los loops. Bloques 5 y 6.

4. **"El orquestador propone, el usuario decide. No commitee automáticamente."** — el respeto al criterio del usuario. Bloques 4 y 5.

5. **"Empieza simple. Escala solo cuando el caso lo justifique."** — la regla de oro contra sobreingeniería. Bloque 9.

**Frase de remate al final:**

> *"Skill que orquesta. Subagente que ejecuta. Loop con techo. Tres piezas, un harness fiable."*

---

## 10. Slide de entrada (locución HeyGen)

> **Texto del avatar antes del screencast:**

"Y vamos a la siguiente demo. La 3.2a. La que cierra el ciclo del harness desde dentro de una sesión. Vamos a componer. Tres cosas en directo. Una, `context: fork` en skills — el primer mecanismo de aislamiento. Convertimos `angular-component` añadiendo una línea al frontmatter. Dos, el patrón compuesto sembrado en la 3.1b: un skill que invoca un subagente. Construimos `pre-commit-check` que orquesta al `dotnet-reviewer` antes de cada commit, con workflow de cinco pasos estandarizado. Tres, el loop validator → implementer con techo de tres iteraciones. Lo probamos en directo: hay un anti-patrón deliberado en `CreateOrderHandler` — un `catch(Exception)` genérico con `Console.WriteLine` y `return 0`. El reviewer lo caza, el orquestador aplica los fixes automáticamente, vuelve a invocar al reviewer, segunda pasada limpia. Veréis el harness compuesto funcionando por primera vez. Vamos al screencast."

---

## 11. Slide de salida (locución HeyGen)

> **Texto del avatar al volver del screencast:**

"Lo que acabáis de ver es el harness empezando a parecer un harness. Skill que orquesta, subagente que ejecuta en aislamiento, loop con techo de tres iteraciones, decisión final del usuario. Cinco ideas para llevarse al lunes. Una, `context: fork` aísla un skill sin crear subagente — para skills que leen mucho y devuelven poco. Dos, el patrón compuesto skill que invoca subagente es donde está la rentabilidad. Tres, el orquestador estandariza el workflow del equipo — una vez escrito, cada commit pasa por el mismo flujo. Cuatro, los loops siempre con techo: si no converge en tres vueltas, el problema está en otra parte, no en la cantidad de iteraciones. Cinco, el skill orquestador NO se aísla con `context: fork` — necesita ver lo que hace para confirmar al usuario. La regla de oro contra la sobreingeniería: empieza simple, escala solo cuando el caso lo justifique. En la siguiente demo, la 3.2b, cerramos la orquestación con paralelo versus serial, context bank como memoria compartida, Claude Code como MCP server, y Agent Teams con el dato honesto del coste — diez a quince veces más tokens según el whitepaper de Anthropic. Empezamos con el tres punto dos punto B."

---

## 12. Tiempo total estimado

| Bloque | Tiempo |
|---|---|
| Slide de entrada (avatar) | ~30 seg |
| Bloque 1 — Setup, recap del frame, objetivo | ~2 min |
| Bloque 2 — `context: fork` en `angular-component` | ~3 min |
| Bloque 3 — El patrón base: skill orquesta subagentes | ~3 min |
| Bloque 4 — Construir `pre-commit-check` | ~5 min |
| Bloque 5 — Probar el orquestador end-to-end | ~5 min |
| Bloque 6 — Loops a coste cero: heurística | ~2 min |
| Bloque 7 — Skill con fork vs subagente: decisión | ~2 min |
| Bloque 8 — Commit y notas | ~1 min 30 seg |
| Bloque 9 — Recap y anti-patrones | ~2 min |
| Bloque 10 — Cliffhanger a 3.2b | ~1 min 30 seg |
| Slide de salida (avatar) | ~30 seg |
| **Total screencast** | **~28-30 min** |
| **Total con avatar** | **~29-31 min** |

> Si hay preguntas durante el screencast, súmale 4-5 minutos. La demo encaja en un bloque de **35 minutos** dentro de la sesión.

**Margen de seguridad por si algo va lento:**

- **Si `context: fork` no se reconoce** en tu versión de Claude Code (algunas versiones antiguas no lo soportan), comenta: *"esta sintaxis es de Claude Code 2.1+. Si vuestra versión no lo soporta, la alternativa es crear un subagente — la diferencia es de framing, no de funcionalidad. La pedagogía sigue intacta"*. Y procede con el bloque 3 mostrando que `pre-commit-check` invoca al subagente igual que mostraríamos.

- **Si el loop entra en bucle infinito o no converge en 3 iteraciones** (porque el reviewer detecta algo que el skill no sabe arreglar), ponle un caso concreto al alumno: *"esto es exactamente lo que la heurística predijo: tres vueltas y no converge significa que el problema está en otra parte. En este caso podría ser que el reviewer pide una refactorización mayor que el fix puntual no resuelve"*. Y muestras cómo el techo protege al usuario.

- **Si el `dotnet-reviewer` NO caza los 3 hallazgos** (caza solo 1 o 2), no fuerces la pedagogía. Comenta: *"a veces el reviewer prioriza algunos hallazgos sobre otros. Lo importante es ver el ciclo completo: detección → fix → re-validación. Aquí lo vemos con uno o dos hallazgos en lugar de tres, pero el flujo es idéntico"*.

- **Si los fixes que aplica el orquestador rompen el código** (porque el modelo decide modificar más de lo necesario), reverte y comenta: *"esto es un riesgo real de los orquestadores con autonomía. Por eso el techo es de tres iteraciones y por eso el orquestador NO commitea automáticamente — el usuario verifica"*. Y procedes con el bloque 8 mostrando manualmente el resultado.

- **Si te quedas sin tiempo y los bloques 6 y 7 te aprietan**, recorta el bloque 6 a 1 min (solo la regla "loops cortos rentan, largos no"). El bloque 7 puedes recortarlo a 1 min 30 seg (solo el ejemplo de los tres skills/subagentes del repo sin desarrollar).

- **Si surge la pregunta sobre paralelizar la invocación del reviewer** (por ejemplo "¿podríamos lanzar reviewer y tester en paralelo?"), responde corto: *"sí, pero eso es la 3.2b — fan-out / fan-in. Aquí mantenemos serial porque cada iteración del reviewer depende del fix que aplicó el orquestador. Es serial por dependencia"*. Y sigue.

---

# Apéndice A — Decisiones pedagógicas justificadas

> Esta sección no se graba. Es para Pedro, formador.

**¿Por qué `context: fork` en `angular-component` y no crear un nuevo skill solo para mostrar la sintaxis?**

Porque **trabajar sobre un skill existente** muestra la decisión real: cuándo añadir `context: fork` a algo que ya tienes vs cuándo dejarlo como está. La pedagogía está en **qué skills SÍ y qué skills NO** (gamma slide 7-8). Si creara uno nuevo solo para mostrar la sintaxis, perdería la decisión.

**¿Por qué el `commit-style` se queda sin `context: fork`?**

Porque **es ejemplo del "no" de la regla**. Lee dos cosas (`git diff --cached`, `git status`) y produce una línea. La gamma slide 8 lo dijo: *"skill rápido y simple → no fork, el overhead no compensa"*. Mostrar **un sí y un no en el mismo repo** ancla la decisión.

**¿Por qué el `db-reset` tampoco se cambia?**

Misma lógica que `commit-style`. Y además tiene `disable-model-invocation: true` — el flujo del usuario es siempre `/db-reset` explícito. **Aislar un skill que se invoca explícitamente es overhead sin ganancia**.

**¿Por qué construir `pre-commit-check` y no `feature-implementer` (que es el ejemplo del manual)?**

Porque `feature-implementer` invoca **cuatro subagentes en serie** (explorer, planner, tester, reviewer) y tres de ellos no existen aún (planner, tester) o requerirían crearlos para esta demo. **`pre-commit-check` solo necesita el `dotnet-reviewer`** que ya existe de la 3.1b. Pedagógicamente más limpio: **una pieza nueva, conexión con dos existentes**.

**¿Por qué introducir el `catch(Exception)` y no otro anti-patrón?**

Por tres razones:
1. **Es violación clara del CLAUDE.md** que el `dotnet-reviewer` ya tiene en su contexto: *"NUNCA `catch (Exception)` genérico sin re-throw o log explícito"*. El reviewer está alineado.
2. **Es triple problema** (catch genérico + Console.WriteLine + return 0 mentiroso) — da material para tres hallazgos en un solo cambio. Pedagogía densa.
3. **Compila bien** — no rompe el build. La demo no se cae por errores de compilación tangenciales.

**¿Por qué el orquestador NO commitea automáticamente?**

Por dos razones pedagógicas:
1. **Respeto al criterio del usuario** — el commit es decisión humana. La gamma 3.2a slide 30 marcó como anti-patrón asumir que la auto-delegación es perfecta.
2. **Pieza didáctica clara** — el skill **propone**, el alumno ve que el cierre es manual. Si commiteara automáticamente, el alumno podría pensar *"entonces los hooks ya están aquí"* — y eso es 3.3.

**¿Por qué revertir el handler al final de la demo?**

Por **disciplina de scope**. Si la rama queda con los fixes aplicados, las demos siguientes (3.2b, 3.3, módulo 4, módulo 5) parten de un código distinto al que esperaban. Mejor: ver el ciclo completo durante la demo, mostrar el resultado, y revertir para mantener la rama coherente con el módulo 3.1b.

**¿Por qué el bloque 7 (skill con fork vs subagente) viene después del bloque 6 (loops a coste cero)?**

Porque la decisión skill-vs-subagente es **filosófica**, mientras que los loops son **operativos**. Mejor cubrir lo operativo primero (cuando el alumno está atento al caso real que acaba de ver) y dejar la reflexión filosófica para cuando el ritmo baja. Si invierto el orden, el alumno está pensando en filosofía mientras debería estar absorbiendo el resultado del experimento.

**¿Por qué el cliffhanger a 3.2b menciona el dato concreto de "10-15x más tokens"?**

Porque la gamma 3.2a slide 32 lo sembró como **dato fundacional** para entender la 3.2b. Y porque **es el dato que más cambia decisiones** — saber que multi-agente cuesta orden de magnitud más. Si lo callo, el alumno llega a 3.2b sin la mochila preparada.

**¿Por qué no introduzco context bank en esta demo aunque conceptualmente encajaría?**

Porque **es el contenido específico de 3.2b** y la separación entre demos es estructural. La 3.2a cubre composición + loops. La 3.2b cubre paralelo + context bank + MCP server + Agent Teams. **Disciplina de scope por demo**.
