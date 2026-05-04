# Demo 3.3a — Hooks: anatomía, eventos y el primer hook funcionando

> **Versión:** v1 | **Módulo:** 3 | **Sub:** 3.3a | **Estado:** ✅ Versión final
> **Archivo:** `demo_M03-S3.3a-hooks-anatomia-eventos-windows-v1.md`
> **Branch before:** `demo/3.3a-before`  (preparado: hooks-explorados.md + fichero `.cs` mal formateado, sin hook configurado)
> **Branch after:**  `demo/3.3a-after`   (estado final pre-cocinado con el hook PostToolUse operativo y el fichero ya auto-formateado)
> **Branch parent:** `demo/3.2b-after`
> **Tiempo total estimado:** ~24-28 minutos
> **Tipo:** Demo de fundamentos (INFRA). **Es la primera demo del curso donde el alumno ve hooks funcionando — la pieza determinista del harness que pasa siempre, sin depender del razonamiento del agente.** Cubre el frame instrucción vs garantía, anatomía completa del hook, los 17 eventos del ciclo de vida (los más útiles), tipos de handler, y construye el primer hook real: auto-format on Write/Edit. Sienta las bases para 3.3b donde haremos el bloqueo de comandos peligrosos y la observabilidad. Sigue el patrón **before/after** definido en [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md).
> **Plataforma:** Windows 11 (PowerShell 7 + Git Bash, **no WSL**).

---

## 1. Contexto

Cerramos el 3.2b con seis skills y tres subagentes operativos en `OrderManagement`. **Pero todo lo que hicimos pasa cuando el usuario lo pide**. *"`/pre-commit-check`"*. *"`/pre-pr-check`"*. *"Usa el subagente repo-explorer"*. **Iniciativa del usuario**. La gamma 3.2b cierre lo dejó claro: *"hooks son la pieza determinista del harness — lo que hace que ciertas cosas pasen automáticamente sin que tengáis que pedirlo cada vez"*.

La gamma 3.3a (30 slides, ~30 min) cubrió cuatro piezas conceptuales:

1. **Por qué hooks son lo más simple con mayor impacto inmediato** (slides 2-4) — los configuras una tarde, al día siguiente notas la diferencia. Pieza accionable que rinde rápido.
2. **El frame "instrucción vs garantía"** (slides 5-9) — `CLAUDE.md` es contexto (a veces se sigue), skills son activación probabilística (mejor pero no garantía), hooks son **código que se ejecuta sin que el agente pueda decidir**. La diferencia es absoluta.
3. **Anatomía completa**: estructura JSON dentro de `settings.json` (no fichero separado), evento + matcher + handler, los tres scopes (user, proyecto, local), el comando `/hooks` para auditar (slides 10-13).
4. **Los 17 eventos del ciclo de vida** y los **4 tipos de handler** (`command`, `http`, `prompt`, otros), más el sistema de exit codes y el patrón `updatedInput` (slides 14-27).

Esta demo aterriza la teoría con una construcción progresiva: el primer hook real del repo — un `PostToolUse` con matcher `Write|Edit|MultiEdit` que ejecuta `dotnet format` para `.cs` y un placeholder para Angular. Y se prueba en directo modificando un fichero en una sesión nueva, viendo el formato aplicarse **sin pedirlo**.

> **Tipo de demo:** construcción del primer hook funcional. La rama `demo/3.3a-after` queda con `.claude/settings.json` ampliado con la sección `hooks`, un script `.claude/hooks/format-on-write.sh`, y este hook probado en directo. **Es la primera demo del curso donde el alumno ve algo ejecutarse sin que el agente tenga la opción de no ejecutarlo**.

---

## 2. Objetivo de la demo

Cinco cosas concretas que tienen que quedar en la cabeza del alumno cuando termine los ~24 minutos de screencast:

1. **Hooks son la pieza determinista del harness.** *"Es código, no instrucción. La diferencia es absoluta."* (gamma slide 8). El alumno tiene que sentir la diferencia al ver el hook ejecutarse **sin posibilidad de no ejecutarse**, vs un skill que puede no activarse.

2. **El frame "instrucción vs garantía"**. `CLAUDE.md` (contexto, a veces se sigue) → skill (activación probabilística) → hook (garantía absoluta). **Cuándo cada uno**: regla determinista → hook. Criterio o adaptación → skill o subagente. Contexto general → `CLAUDE.md`.

3. **Anatomía del hook**: vive **dentro de `settings.json`** (no fichero separado), tiene evento + matcher + handler, los tres scopes (user, proyecto, local), comando `/hooks` para auditar.

4. **Los dos eventos que cubren el 80%**: `PostToolUse` con matcher `Write|Edit|MultiEdit` (auto-format, lint, validación) y `PreToolUse` con matcher `Bash` (bloqueo de comandos peligrosos). **Empezar con esos dos**.

5. **El sistema de exit codes**: 0 = éxito, 2 = blocking error (la herramienta más potente), otros = non-blocking. **Exit 2 es lo que distingue una verdadera política de seguridad de una recomendación**. Y bloquea **incluso en modo `--dangerously-skip-permissions`**.

Y dos cosas que tienen que **NO quedar** en su cabeza:

- *"Un hook puede sustituir a un skill o un subagente."* — falso. **Hooks son para reglas deterministas**. Lo que requiere criterio sigue siendo skill/subagente. La gamma 3.3a slide 9 lo dijo: *"hardcodear lo que requiere razonamiento es de los anti-patrones más comunes"*.
- *"Cuantos más hooks, mejor."* — falso. **Hooks se ejecutan síncronamente** y suman latencia. La gamma 3.3a anti-patrón #1: *"mantén los hooks por debajo de 200-500ms"*. Demasiados hooks = sesión lenta.

---

## 3. Branch `demo/3.3a-before`

Punto de partida del screencast.

```
demo/3.3a-before
```

**Parte de:** `demo/3.2b-after`.

**Estado del repo:** todo lo de `demo/3.2b-after` (seis skills, tres subagentes, context bank, endpoint de búsqueda) más dos artefactos preparatorios:

1. **`docs/hooks-explorados.md`** — documento nuevo equivalente a `subagentes-explorados.md`, con la estructura inicial vacía para que el alumno lo encuentre listo.
2. **Un fichero `.cs` deliberadamente mal formateado** (espacios mal, indentación inconsistente) que durante el screencast el hook recién construido va a arreglar.

**Qué NO hay en `-before`:**
- **Sin sección `hooks` en `.claude/settings.json`** — eso es la pieza viva.
- **Sin `.claude/hooks/`** — el directorio se crea con el primer script en directo.
- **Sin marca `[x]`** en `docs/DEMOS.md` para 3.3a.

> El formador hace `git checkout demo/3.3a-before` antes de empezar a grabar. La pieza viva del screencast es construir el hook y verlo dispararse al modificar el fichero mal formateado.

---

## 4. Branch `demo/3.3a-after`

Estado final que la siguiente clase (3.3b) asume.

```
demo/3.3a-after
```

**Parte de:** `demo/3.3a-before`.

**Qué añade respecto a `-before`:**

1. **`.claude/settings.json` ampliado** con la sección `hooks` que define el primer hook (`PostToolUse` con matcher `Write|Edit|MultiEdit`).
2. **`.claude/hooks/format-on-write.sh`** — script bash que ejecuta `dotnet format` para `.cs`, `prettier` para `.json`/`.md`, y referencia futura para Angular.
3. **El fichero `.cs` ya bien formateado** (resultado de aplicar el hook).
4. **`docs/hooks-explorados.md`** rellenado con las notas del experimento.
5. **Marca `[x]`** en `docs/DEMOS.md`.

**Cómo se prepara:** ver §6b. Se materializa antes de grabar.

> Durante la grabación, el formador construye el hook en directo desde `demo/3.3a-before` y lo prueba modificando el fichero mal formateado. Al cerrar descarta los cambios reales y la siguiente clase parte de `demo/3.3a-after` ya pre-cocinada.

---

## 5. Estado del repo al hacer `git checkout demo/3.3a-before`

Casi idéntico a `demo/3.2b-after`, con los dos artefactos preparatorios añadidos:

```
ordermanagement/
├── .claude/
│   ├── settings.json                    (sin sección hooks aún)
│   ├── skills/                          (6 skills)
│   ├── agents/                          (3 subagentes)
│   └── workflow-state/                  (gitignored, del 3.2b)
├── docs/
│   ├── DEMOS.md
│   ├── skills-explorados.md
│   ├── auditoria-skills-comunidad.md
│   └── subagentes-explorados.md
├── scripts/
├── src/                                 (con endpoint de búsqueda commiteado)
├── frontend/
├── tests/
├── CLAUDE.md
├── .gitignore                           (incluye .claude/workflow-state/)
└── README.md
```

**Estado clave para esta demo:**

- **No hay sección `hooks` en `settings.json`** — la añadimos en directo.
- **No hay `.claude/hooks/`** — la creamos con el primer script.
- Para probar el hook en directo necesitamos un fichero `.cs` que esté **deliberadamente desformateado** (espacios mal, indentación inconsistente) que el `dotnet format` arregle visiblemente. Lo introducimos en preparación.

**Estado de la máquina Windows del formador:**

```
✅ Claude Code v2.1.x con hooks operativos
✅ Git for Windows + Git Bash (necesario para ejecutar el script .sh en Windows)
✅ PowerShell 7 (terminal principal)
✅ VS Code con el repo cargado en demo/3.3a-before
✅ dotnet format disponible (viene con .NET SDK)
✅ npx prettier disponible (instalado en frontend/)
```

> **Nota Windows crítica**: los scripts `.sh` en Windows nativo se ejecutan a través de **Git Bash** (instalado por defecto con Git for Windows). Cuando Claude Code ejecuta el comando del hook, lo lanza con la shell por defecto del sistema. En Windows, eso pasa por Git Bash automáticamente si el script está en formato shebang `#!/bin/bash`. **No necesitas WSL**. Verifica antes de grabar: `bash --version` en PowerShell debe responder.

**Lo que el alumno verá al final de la demo:**

- El frame "instrucción vs garantía" explicado con tres ejemplos concretos.
- La anatomía del hook con JSON real en `settings.json`.
- El comando `/hooks` mostrando hooks cargados.
- Los 17 eventos categorizados con los 2 más importantes destacados.
- Los 4 tipos de handler (`command`, `http`, `prompt`, otros) con ejemplos.
- El sistema de exit codes con énfasis en exit 2.
- El patrón `updatedInput` mencionado como adelanto.
- **El primer hook construido y probado en directo**: modifica un `.cs` deliberadamente mal formateado, el hook ejecuta `dotnet format`, el formato se aplica sin pedirlo.

---

## 6a. Prompt para Claude Code — preparar `demo/3.3a-before`

> Crea la rama de partida del screencast desde `demo/3.2b-after` con dos artefactos preparatorios: `docs/hooks-explorados.md` (estructura vacía) y un fichero `.cs` deliberadamente mal formateado que el hook recién construido formateará en directo. **No crea el hook todavía** — esa es la pieza viva.

````
Estoy preparando la demo 3.3a del curso de Claude Code (hooks: anatomía,
eventos, primer hook funcional). Sigue el patrón before/after (ver demo M0.2).

Quiero que prepares la rama `demo/3.3a-before` desde `demo/3.2b-after`
con dos artefactos preparatorios y NADA del hook real (eso es la pieza viva
del screencast).

# Lo que necesito

Tres tareas:

## Tarea 1: crear la rama

```powershell
git checkout demo/3.2b-after
git pull
git checkout -b demo/3.3a-before
```

## Tarea 2: crear docs/hooks-explorados.md (estructura vacía)

Contenido:

```markdown
# Hooks — notas de exploración

Documento equivalente a `subagentes-explorados.md` para los hooks que
vamos construyendo a lo largo del módulo 3.3.

## Hooks construidos en este repo

(Esta sección se rellena durante las demos 3.3a y 3.3b.)

## Hooks de referencia que vale la pena tener

### En user level (~/.claude/settings.json) — viajan con vosotros

- **block-dangerous** (PreToolUse / Bash) — bloquea comandos peligrosos
  (`rm -rf /`, `git push --force`, etc.). Pendiente para 3.3b.

### En project level (.claude/settings.json) — van a git con el equipo

- **format-on-write** (PostToolUse / Write|Edit|MultiEdit) — auto-format
  para `.cs`, `.ts`, `.json`, `.md`. Construido en 3.3a.

## Eventos cubiertos hasta ahora

(Se rellena durante el módulo 3.3.)

## Lecciones extraídas

(Se rellena durante el módulo 3.3.)
```

## Tarea 3: crear el handler con MAL FORMATO deliberado

Crea `src/OrderManagement.Application/Handlers/RemoveItemHandler.cs` con
formato malo (espacios sobrantes, indentación inconsistente):

```csharp
using MediatR;
using OrderManagement.Application.Interfaces;
using   OrderManagement.Application.Exceptions;
using OrderManagement.Domain;

namespace OrderManagement.Application.Handlers;

public  class RemoveItemHandler : IRequestHandler<RemoveItemCommand, Unit>
{
private readonly IOrderRepository _repository;

    public RemoveItemHandler(IOrderRepository repository)
{
        _repository = repository;
    }

    public async Task<Unit> Handle(RemoveItemCommand request,CancellationToken cancellationToken){
            var order = await _repository.GetByIdAsync(request.OrderId, cancellationToken)??throw new OrderNotFoundException(request.OrderId);
order.RemoveItem(request.ProductId);
        await _repository.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
```

Y crea `src/OrderManagement.Application/Commands/RemoveItemCommand.cs`
(correcto, sin formato malo):

```csharp
using MediatR;

namespace OrderManagement.Application.Commands;

public record RemoveItemCommand(int OrderId, int ProductId) : IRequest<Unit>;
```

Verifica con `dotnet build` (debe pasar — el mal formato no rompe build) y commitea:

```powershell
git add docs/hooks-explorados.md `
        src/OrderManagement.Application/Handlers/RemoveItemHandler.cs `
        src/OrderManagement.Application/Commands/RemoveItemCommand.cs
git commit -m "demo/3.3a-before: artefactos preparatorios (hooks-explorados + handler mal formateado)"
```

# Restricciones (importantes)

- NO crees `.claude/hooks/`. Eso es la pieza viva.
- NO añadas la sección `hooks` a `.claude/settings.json`. Eso es la pieza viva.
- NO marques `[x]` en `docs/DEMOS.md` todavía. Eso va en `-after`.
- NO modifiques skills, subagentes, CLAUDE.md ni settings.json (excepto lo
  ya prohibido).
- NO toques otros ficheros del código.

# Cuando termines, dime

1. Que la rama demo/3.3a-before está creada desde demo/3.2b-after.
2. Que docs/hooks-explorados.md está creado con la estructura.
3. Que RemoveItemHandler.cs está creado con MAL formato deliberado.
4. Que dotnet build pasa.
5. Que el commit preparatorio está hecho.
````

---

## 6b. Prompt para Claude Code — preparar `demo/3.3a-after`

> Materializa la rama final con el hook PostToolUse operativo y el fichero ya bien formateado (resultado de aplicar el hook). Equivale a lo que el formador construirá en directo desde `demo/3.3a-before`.

````
Estoy preparando la demo 3.3a del curso de Claude Code. Esta rama
-after pre-cocina el hook PostToolUse de auto-format que el formador
construirá en vivo durante el screencast.

# Contexto

Estoy en la rama `demo/3.3a-before` del repo `ordermanagement`. La rama
tiene los artefactos preparatorios (docs/hooks-explorados.md y el handler
mal formateado) pero NO tiene aún el hook configurado.

Quiero que prepares la rama `demo/3.3a-after` desde `demo/3.3a-before`
con el hook completo, el fichero ya bien formateado, y la marca [x] en
docs/DEMOS.md.

# Lo que necesito

Cuatro tareas:

## Tarea 1: crear la rama

```powershell
git checkout demo/3.3a-before
git checkout -b demo/3.3a-after
```

## Tarea 2: ampliar `.claude/settings.json` con el hook

Añade al `settings.json` existente una sección `hooks` con un hook
`PostToolUse` que reaccione al matcher `Write|Edit|MultiEdit` y ejecute
el script `.claude/hooks/format-on-write.sh`. Mantén intacto el resto
del fichero (las claves `permissions.allow` y `permissions.deny` que
viene de demos anteriores).

## Tarea 3: crear `.claude/hooks/format-on-write.sh`

Script bash con shebang `#!/bin/bash` que:
- Lee de stdin el JSON de Claude Code (formato hook input).
- Extrae el path del fichero modificado.
- Si termina en `.cs`: ejecuta `dotnet format --include <path>` desde la raíz.
- Si termina en `.json` o `.md`: ejecuta `npx prettier --write <path>` desde la raíz.
- Si es Angular (`.ts`, `.html`, `.scss`): por ahora deja un eco con TODO
  (la 3.3b lo ampliará).
- Sale con exit 0 en éxito.

## Tarea 4: aplicar el formato al handler + marcar DEMOS.md + commit

Ejecuta `dotnet format --include src/OrderManagement.Application/Handlers/RemoveItemHandler.cs`
para arreglar el formato del handler que estaba mal en `-before`. El
fichero queda formateado (equivalente a lo que el hook habría hecho).

Marca la 3.3a en `docs/DEMOS.md`:

```
- [x] **demo/3.3a** — Hooks: anatomía, eventos, y primer hook funcional
```

Verifica con `dotnet build` (0 warnings, 0 errors) y commit:

```powershell
git add .claude/settings.json `
        .claude/hooks/format-on-write.sh `
        src/OrderManagement.Application/Handlers/RemoveItemHandler.cs `
        docs/DEMOS.md
git commit -m "demo/3.3a-after: primer hook PostToolUse + auto-format aplicado"
```

NO hagas push.

# Restricciones (importantes)

- NO modifiques skills, subagentes ni CLAUDE.md.
- NO toques otros ficheros del código (solo el handler que tiene mal formato).
- El script `.sh` debe ser portable a Git Bash en Windows nativo.

# Cuando termines, dime

1. Que la rama demo/3.3a-after está creada desde demo/3.3a-before.
2. Que `.claude/settings.json` tiene la sección hooks correctamente.
3. Que `.claude/hooks/format-on-write.sh` existe y es ejecutable.
4. Que RemoveItemHandler.cs ya está bien formateado.
5. Que docs/DEMOS.md está marcado.
6. Que dotnet build pasa.
7. Que el commit está hecho.

Si tienes dudas (por ejemplo, sobre el formato exacto del JSON de hook input),
para y pregúntame.
````

---

## 7. Artefactos que Claude Code debe generar

```
✓ Rama demo/3.3a-before (parte de demo/3.2b-after) con:
  ├── docs/hooks-explorados.md (estructura)
  ├── src/OrderManagement.Application/Commands/RemoveItemCommand.cs (correcto)
  └── src/OrderManagement.Application/Handlers/RemoveItemHandler.cs (deliberadamente mal formateado)
✓ Rama demo/3.3a-after (parte de demo/3.3a-before) con:
  ├── .claude/settings.json (sección hooks añadida)
  ├── .claude/hooks/format-on-write.sh (nuevo, ejecutable)
  ├── src/OrderManagement.Application/Handlers/RemoveItemHandler.cs (ya bien formateado)
  └── docs/DEMOS.md con 3.3a marcada como [x]
✓ Verificación de build OK
✓ Commit único pre-grabación
```

**Lo que NO debe haber generado:**

- ❌ `.claude/hooks/` (creación en vivo)
- ❌ Sección `hooks` en `settings.json` (en vivo)
- ❌ Cambios en skills, subagentes, CLAUDE.md, otros ficheros

> Si Claude Code se anticipa y crea el hook, **se rechaza el output**.

**Lo que el formador commitea EN VIVO sobre `demo/3.3a-before` durante el screencast:**

```
Durante la grabación, sobre demo/3.3a-before, se hacen commits ficticios:

1. "demo/3.3a-after: hook PostToolUse con auto-format script"
   └── .claude/settings.json (MODIFICADO con sección hooks)
   └── .claude/hooks/format-on-write.sh (NUEVO)
   └── docs/hooks-explorados.md (MODIFICADO con notas del 3.3a)

2. "demo/3.3a-after: aplica auto-format a RemoveItemHandler"
   (commit producido por la prueba en vivo del hook)
   └── src/OrderManagement.Application/Handlers/RemoveItemHandler.cs (FORMATEADO)

Al cerrar el screencast: el formador descarta los commits reales.
La siguiente clase parte de demo/3.3a-after (pre-cocinada en §6b)
que es equivalente al resultado del screencast.
```

**Estado final del árbol después del screencast:**

```
ordermanagement/
├── .claude/
│   ├── settings.json                ← MODIFICADO (sección hooks añadida)
│   ├── skills/                      (sin cambios)
│   ├── agents/                      (sin cambios)
│   └── hooks/                       ← NUEVA carpeta
│       └── format-on-write.sh       ← NUEVO
├── docs/
│   ├── DEMOS.md                     ← MODIFICADO (pre-grabación)
│   └── hooks-explorados.md          ← MODIFICADO (pre + en vivo)
└── src/OrderManagement.Application/Handlers/
    └── RemoveItemHandler.cs         (formateado por el hook en vivo)
```

---

## 8. Guion del screencast

**Duración estimada del screencast: ~22-26 minutos.**

Diez bloques. La demo es **conceptual con un solo experimento práctico al final** — el primer hook construido y probado.

> **Antes de empezar a grabar**, asegúrate de:
> - Tener PowerShell 7 abierto en pantalla completa, fuente al menos 16pt.
> - Tener VS Code abierto con el repo en `demo/3.3a-before`.
> - **Verificar Git Bash**: `bash --version` debe responder en PowerShell.
> - **Verificar `dotnet format`**: `dotnet format --version` debe responder.
> - **Verificar `RemoveItemHandler.cs`** está mal formateado abriéndolo en VS Code — debe verse a simple vista.
> - Cerrar Slack, Teams, navegadores con notificaciones.

---

### Bloque 1 — Setup y la pregunta del cierre del 3.2b (~1 min 30 seg)

**Pantalla compartida.** A la izquierda VS Code con el repo en `demo/3.3a-before`. A la derecha terminal PowerShell.

**En la terminal:**

```powershell
git status
git log --oneline -3
```

```
On branch demo/3.3a-before
nothing to commit, working tree clean

abc1234 (HEAD -> demo/3.3a-before) demo/3.3a-before: artefactos preparatorios (hooks-explorados + handler mal formateado)
xyz5678 (demo/3.2b-after) demo/3.2b-after: context bank, fan-out paralelo, endpoint search
def9012 (demo/3.2a-after) demo/3.2a-after: angular-component context:fork + pre-commit-check con loop
```

**Lo que dices:**

> "Estamos en `demo/3.3a-before`. **Primera demo del módulo 3.3 — hooks**. Y la última pieza del módulo 3 entero.
>
> Cerramos el 3.2b con seis skills, tres subagentes, context bank, fan-out paralelo. **Pero todo lo que hicimos pasa cuando vosotros lo pedís**. *'/pre-commit-check'*. *'/pre-pr-check'*. *'Usa el subagente repo-explorer'*. **Iniciativa vuestra**.
>
> Hoy entramos en lo que la gamma 3.3a slide 2 llamó **la pieza que cierra el harness**. Hooks son **lo más simple del módulo 3 con el mayor impacto inmediato**. La gamma 3.3a slide 3 lo dijo: *'subagentes son potentes pero requieren cambiar tu forma de trabajar. Hooks los configuras una tarde y al día siguiente notas la diferencia'*.
>
> Y conceptualmente, **hooks son la pieza determinista del agent harness**. Lo que pasa **siempre, sin opción a no pasar**. Sin razonamiento del agente. Sin decisiones probabilísticas. **Garantizado**.
>
> Cuatro cosas en estos minutos. Una, **el frame instrucción vs garantía** — por qué hooks no son skills ni `CLAUDE.md`. Dos, **anatomía** — JSON dentro de `settings.json`, eventos, matchers, handlers. Tres, **los 17 eventos del ciclo de vida** y los dos que cubren el 80%. Cuatro, **construimos el primer hook funcional**: auto-format al modificar ficheros. Lo probamos sobre un `.cs` que está deliberadamente mal formateado.
>
> Vamos."

**Tiempo:** ~90 segundos.

---

### Bloque 2 — El frame: instrucción vs garantía (~3 min)

> "**El frame que vertebra hooks**. La gamma 3.3a slides 5-9 lo cubrió. Tres formas de implementar la misma regla — y la diferencia entre las tres."

**En el editor de texto al lado, escribo (contenido pedagógico):**

```
LA REGLA DE EJEMPLO

  "Después de modificar un fichero .cs,
   ejecuta dotnet format."

TRES FORMAS DE IMPLEMENTARLA


OPCIÓN 1 — En CLAUDE.md
─────────────────────────
  Escribes en el fichero:
    "Después de modificar cualquier .cs, ejecuta dotnet format
     antes de continuar."

  ¿Funciona?
    A veces. El agente lo ve, intenta seguirlo,
    la mayoría de las veces lo hace.

  PERO: CLAUDE.md es CONTEXTO, no GARANTÍA.
    En sesiones largas → puede salirse de la atención.
    Cuando hay muchas cosas a la vez → puede priorizar otras.
    "Lo tendría que haber hecho" → respuesta posible.


OPCIÓN 2 — En un skill
────────────────────────
  Defines un skill "post-edit-format" con descripción:
    "ejecutar después de cada edición de fichero .cs"

  ¿Funciona?
    Mejor que CLAUDE.md. Activación más explícita.
    Pero sigue dependiendo de que el agente
    reconozca el momento adecuado.

  En sesiones complejas → activación PROBABILÍSTICA.


OPCIÓN 3 — En un hook
───────────────────────
  Configuras un hook PostToolUse con matcher Edit|Write|MultiEdit.

  Cuando el matcher se cumple:
    → el hook se ejecuta
    → SIN que el agente pueda decidir si lo ejecuta o no

  ES CÓDIGO, NO INSTRUCCIÓN.
  La diferencia es ABSOLUTA.


┌─────────────────────────────────────────────────────────┐
│  REGLA PRÁCTICA                                          │
│                                                          │
│  Regla DETERMINISTA (siempre la misma respuesta a un    │
│  evento)                          → HOOK                 │
│                                                          │
│  Requiere CRITERIO o ADAPTACIÓN   → skill o subagente    │
│                                                          │
│  CONTEXTO general del proyecto    → CLAUDE.md            │
└─────────────────────────────────────────────────────────┘
```

> "**La diferencia es absoluta**. Y atención al matiz: la opción 1 (`CLAUDE.md`) y la opción 2 (skill) **dependen del razonamiento del agente**. La opción 3 (hook) **no**.
>
> Por eso la gamma 3.3a slide 5 lo dijo: *'hooks son scripts que se ejecutan automáticamente en eventos del ciclo de vida. Sin razonamiento. Sin opción a no ejecutarse. Garantizados'*.
>
> Y la regla práctica al final. **Si la regla siempre tiene la misma respuesta a un evento → hook**. Si requiere criterio → skill. Si es contexto general → `CLAUDE.md`. Esta regla os va a ayudar el lunes a decidir dónde meter cada cosa.
>
> Vamos a ver cómo se escribe."

**Tiempo:** ~3 minutos.

---

### Bloque 3 — Anatomía del hook (~3 min)

> "**Estructura básica**. La gamma 3.3a slide 11."

**En el editor:**

```
DÓNDE VIVE UN HOOK

NO hay un fichero hooks.json separado.
(Esto confunde a quienes vienen de tutoriales antiguos.)

Los hooks viven DENTRO de settings.json.


ESTRUCTURA BÁSICA

{
  "hooks": {
    "PostToolUse": [                    ← evento
      {
        "matcher": "Write|Edit|MultiEdit",   ← regex
        "hooks": [
          {
            "type": "command",          ← tipo handler
            "command": "..."            ← qué se ejecuta
          }
        ]
      }
    ]
  }
}


CUATRO ELEMENTOS

  1. El bloque "hooks" dentro de settings.json
  2. EVENTO al que se engancha (PostToolUse, PreToolUse, etc.)
  3. MATCHER — regex que decide cuándo se activa
  4. HANDLER — el comando que se ejecuta


SCOPES (igual que skills, igual que subagentes)

  ~/.claude/settings.json           USER       — viaja contigo
  .claude/settings.json             PROJECT    — va a git, equipo
  .claude/settings.local.json       LOCAL      — gitignored, tuyo

  Project tiene PRECEDENCIA cuando hay duplicados.

  ⚠️ Esto significa que un equipo puede definir hooks
     "no negociables" a nivel proyecto y los devs
     individuales NO PUEDEN saltárselos en su user-level.


COMANDO PARA AUDITAR

  > /hooks

  Vista interactiva con todos los hooks configurados,
  agrupados por evento.

  Útil para auditar qué tienes cargado, especialmente
  si has acumulado varios.
```

> "Tres puntos críticos.
>
> Una. **Los hooks viven dentro de `settings.json`**. **No** hay un fichero `hooks.json` separado. Esto confunde a gente que viene de versiones antiguas o de tutoriales desactualizados. **JSON dentro de settings**.
>
> Dos. **Los tres scopes** — user, project, local. Igual que con skills y subagentes. **Project tiene precedencia**. Un equipo puede meter hooks no negociables que los devs no pueden saltarse desde su user-level. **Pieza importante** para empresa.
>
> Tres. **El comando `/hooks`** para auditar. La gamma slide 13. Si os perdéis con qué hooks tenéis, ese comando os lo dice.
>
> Vamos a los eventos."

**Tiempo:** ~3 minutos.

---

### Bloque 4 — Los 17 eventos del ciclo de vida (~3 min)

> "**Claude Code expone diecisiete eventos a los que podéis engancharos**. La gamma 3.3a slides 14-18 los cubrió. Los más útiles, agrupados."

**En el editor:**

```
LOS 17 EVENTOS DEL CICLO DE VIDA

EVENTOS DE SESIÓN
─────────────────
  SessionStart    → al arrancar. Inyectar contexto inicial,
                    branch actual, último commit.
  SessionEnd      → al cerrar. Logging, notificaciones, limpieza.
  Stop            → cuando Claude termina de responder.
                    DISTINTO de SessionEnd — puede haber
                    muchos Stop en una sesión.

EVENTOS DE HERRAMIENTAS  ← LOS MÁS POTENTES
─────────────────────────
  PreToolUse      → ANTES de ejecutar una herramienta.
                    Puede INSPECCIONAR y BLOQUEAR.
                    Se activa antes de la comprobación de permisos.
  
  PostToolUse     → DESPUÉS de ejecutar exitosamente.
                    Para validar, formatear, registrar.
                    NO puede deshacer (la herramienta ya se ejecutó),
                    pero sí dar feedback al agente.
  
  PostToolUseFailure  → cuando una herramienta falla.
                        Logging, intentos de recuperación.

EVENTOS DE PERMISOS
───────────────────
  PermissionRequest   → cuando Claude pide permiso interactivamente.
                        Puede automatizar la decisión.
  PermissionDenied    → cuando se deniega.

OTROS ÚTILES
────────────
  UserPromptSubmit    → cada vez que envías un prompt.
                        Validar antes de procesar.
  SubagentStop        → cuando un subagente termina.
                        Para encadenar acciones.
  Notification        → enrutar alertas.


┌──────────────────────────────────────────────────────────┐
│  CUÁLES USAR PRIMERO                                     │
│                                                          │
│  Para empezar, DOS EVENTOS cubren el 80% de los casos:   │
│                                                          │
│  1. PostToolUse con matcher Write|Edit|MultiEdit         │
│     → auto-format, lint, validación                      │
│                                                          │
│  2. PreToolUse con matcher Bash                          │
│     → bloqueo de comandos peligrosos                     │
│                                                          │
│  Con esos dos en marcha ya tienes la mayoría del valor. │
│  Los demás los añades cuando tienes casos concretos.    │
└──────────────────────────────────────────────────────────┘
```

> "**Diecisiete eventos**. Pero la gamma slide 18 lo dejó claro: **dos cubren el 80%**.
>
> **`PostToolUse` con matcher `Write|Edit|MultiEdit`** — auto-format, lint, validación. Hoy construimos exactamente este.
>
> **`PreToolUse` con matcher `Bash`** — bloqueo de comandos peligrosos. Lo construiremos en la 3.3b.
>
> Con esos dos en marcha **tenéis ya la mayoría del valor que un equipo medio necesita**. Los demás se añaden cuando hay casos concretos. **No empecéis cubriendo los diecisiete**. Empezad con dos."

**Tiempo:** ~3 minutos.

---

### Bloque 5 — Tipos de handler (~2 min 30 seg)

> "**Cuando un hook se dispara, ejecuta un handler**. Hay cuatro tipos. La gamma 3.3a slides 19-22."

**En el editor:**

```
LOS 4 TIPOS DE HANDLER

1. command  ← EL MÁS COMÚN
   ────────
   Ejecuta un comando shell.
   Input del evento → llega por stdin como JSON
   Output → va a stdout (puedes devolver JSON para control fino)

   {
     "type": "command",
     "command": "npx prettier --write \"$CLAUDE_TOOL_INPUT_FILE_PATH\"",
     "timeout": 30
   }

   ⚠️ El parámetro "timeout" (en segundos) es importante
      para hooks que pueden colgarse. Pasa el timeout
      → hook se considera fallido.


2. http  ← INTEGRACIONES EXTERNAS
   ────
   POST de un JSON a un endpoint que tú expongas.
   La respuesta del endpoint puede controlar el flujo.

   {
     "type": "http",
     "url": "https://hooks.miempresa.com/claude-pre-tool",
     "timeout": 30,
     "headers": {
       "Authorization": "Bearer $MY_TOKEN"
     },
     "allowedEnvVars": ["MY_TOKEN"]
   }

   Útil cuando integras Claude Code con sistemas
   centralizados de tu empresa: policy enforcement,
   logging corporativo, broker de notificaciones.


3. prompt  ← EL HANDLER CON CRITERIO
   ──────
   Cuando la decisión NO es deterministática.
   En vez de comando shell → pides a un modelo (Haiku
   por defecto) que tome la decisión.

   {
     "type": "prompt",
     "prompt": "Determina si '{tool_input.command}' es seguro
                de ejecutar en producción. Responde solo
                'allow' o 'deny'.",
     "model": "haiku"
   }

   PATRÓN POTENTE: hooks INTELIGENTES.
   No bloqueas con regex sino con criterio.
   Útil cuando los patrones son sutiles o varían según contexto.


4. Otros handlers especializados
   ──────────────────────────────
   Casos más avanzados.
   "command", "http" y "prompt" cubren prácticamente
   todo lo que vais a necesitar al principio.
```

> "Cuatro tipos. **`command` es el que vais a usar el 90% del tiempo**. Comando shell, stdin, stdout. Simple.
>
> **`http`** para integraciones de empresa — policy enforcement centralizado, logging corporativo. **Si vuestra empresa tiene esto, encaja aquí**.
>
> **`prompt`** es el más interesante conceptualmente. La gamma slide 21 lo dijo: *'hooks inteligentes'*. **No bloqueas con regex sino con criterio**. Cuando los patrones a detectar son sutiles. Lo veremos más en 3.3b cuando hagamos el bloqueo de comandos peligrosos — ahí encaja la combinación regex + LLM.
>
> Ahora **el sistema de exit codes** — cómo el hook le dice a Claude qué pasar."

**Tiempo:** ~2 minutos 30 segundos.

---

### Bloque 6 — El sistema de exit codes (~2 min 30 seg)

> "**Esto merece atención propia** porque es donde se decide qué pasa después de que el hook se ejecute. La gamma 3.3a slides 23-25."

**En el editor:**

```
EL SISTEMA DE EXIT CODES

| Exit code | Significado |
|-----------|-------------|
| 0         | Éxito. Si hay JSON en stdout, se parsea para control fino. |
| 2         | BLOCKING ERROR. stderr se devuelve a Claude.
              Para PreToolUse → bloquea la herramienta.
              Para Stop → fuerza a Claude a seguir trabajando. |
| Otros     | Non-blocking error. stderr en modo verbose,
              pero la ejecución continúa. |


┌──────────────────────────────────────────────────────────┐
│  EXIT 2 ES LA HERRAMIENTA MÁS POTENTE                    │
│                                                          │
│  Es lo que permite a un hook BLOQUEAR acciones de        │
│  forma absoluta.                                         │
│                                                          │
│  Distingue una verdadera POLÍTICA DE SEGURIDAD           │
│  ("este comando NO se ejecuta")                          │
│  de un simple AVISO ("esto está raro").                  │
└──────────────────────────────────────────────────────────┘


🔒 OBSERVACIÓN CRÍTICA PARA POLICIES DE SEGURIDAD

  Un PreToolUse que devuelve EXIT 2
  bloquea la herramienta INCLUSO EN MODO
  --dangerously-skip-permissions.

  Esto es importante:

  Significa que puedes establecer reglas que el usuario
  NO PUEDE SALTARSE simplemente cambiando su modo de
  permisos.

  Si tu equipo necesita GARANTÍAS REALES (no recomendaciones),
  los hooks PreToolUse con exit 2 son el mecanismo.


OUTPUT JSON ESTRUCTURADO (control fino)

Para más control que solo exit codes:

  {
    "hookSpecificOutput": {
      "hookEventName": "PreToolUse",
      "permissionDecision": "allow" | "deny" | "ask",
      "permissionDecisionReason": "Operación segura",
      "updatedInput": {
        "command": "comando-modificado"
      },
      "additionalContext": "Información para Claude"
    }
  }


updatedInput  ← BRUTAL Y POCO CONOCIDO

Permite MODIFICAR los argumentos de la herramienta antes de
que se ejecute (transparente para Claude).

Casos:
  - El agente intenta `git push origin main`
    → lo modificas a `git push origin feature/branch-actual`
  - Intenta usar Prettier sin la config correcta
    → le añades el flag
  - Intenta `dotnet test` sin filtros
    → le añades los filtros del módulo actual

Modifica la acción sobre la marcha SIN QUE EL AGENTE SEPA
QUE LA MODIFICASTE.

⚠️ Contrapartida: si lo abusas, debugging se vuelve confuso.
   "¿Por qué ejecutó X cuando dije Y?"
   Úsalo para correcciones obvias, no para magia.
```

> "**Tres puntos críticos** en este bloque.
>
> Una. **Exit 2 es la herramienta más potente**. Bloqueo absoluto. Distingue política de seguridad real de aviso.
>
> Dos. **Exit 2 bloquea incluso en modo `--dangerously-skip-permissions`**. Esto es **crítico para empresa**. Si el equipo necesita garantías reales (no recomendaciones), los hooks `PreToolUse` con exit 2 son el mecanismo. **No hay forma de saltárselos cambiando flags**.
>
> Tres. **`updatedInput`**. La gamma slide 26 lo llamó *'brutal y poco conocido'*. Te permite **modificar la acción del agente sobre la marcha sin que se entere**. Si intenta `git push origin main` y la rama actual es feature, lo cambias. Si intenta `dotnet test` sin filtros, le añades los filtros. **Sin que sepa que lo cambiaste**.
>
> **La contrapartida**: si lo abusas, debugging se vuelve confuso. *'¿Por qué ejecutó X cuando dije Y?'*. **Úsalo para correcciones obvias**, no para magia.
>
> Vamos a construir el primer hook real."

**Tiempo:** ~2 minutos 30 segundos.

---

### Bloque 7 — Construir el primer hook: auto-format on Write (~5 min)

> "**Caso real**: cada vez que Claude modifica un fichero `.cs`, queremos que se ejecute `dotnet format`. Cada vez que modifica un `.json` o `.md`, `prettier`. **Sin tener que pedirlo**. Vamos."

**En PowerShell:**

```powershell
mkdir .claude\hooks
```

**En VS Code, creo `.claude/hooks/format-on-write.sh`:**

```bash
#!/bin/bash
set -e

# Lee el JSON de stdin para extraer el path del fichero modificado
INPUT=$(cat)
FILE_PATH=$(echo "$INPUT" | jq -r '.tool_input.file_path // empty')

# Si no hay file_path, salir sin hacer nada
if [ -z "$FILE_PATH" ]; then
  exit 0
fi

# Si el fichero no existe (puede haber sido borrado), salir
if [ ! -f "$FILE_PATH" ]; then
  exit 0
fi

# Aplicar formateador según extensión
case "$FILE_PATH" in
  *.cs)
    # dotnet format del fichero específico
    dotnet format --include "$FILE_PATH" --no-restore 2>&1 || true
    ;;
  *.ts|*.html|*.scss)
    # Lint Angular si existe el frontend
    if [ -d "frontend" ]; then
      cd frontend
      npx eslint --fix "$FILE_PATH" 2>&1 || true
      cd ..
    fi
    ;;
  *.json|*.md)
    # Prettier para JSON y Markdown si está disponible
    if command -v npx &> /dev/null; then
      npx prettier --write "$FILE_PATH" 2>&1 || true
    fi
    ;;
esac

exit 0
```

**Salvo. En PowerShell:**

```powershell
# En Windows con Git Bash, los .sh son ejecutables si tienen shebang.
# El permiso de ejecución no aplica igual que en Linux, pero Claude Code
# lo invoca con la shell correcta.
ls .claude\hooks\
```

```
format-on-write.sh
```

> "**Mirad las decisiones del script**:
>
> Una. **Lee `stdin` con `jq`** para sacar el `file_path`. La gamma 3.3a slide 19 lo dijo: *'el input del evento llega por stdin como JSON'*.
>
> Dos. **Salida temprana si no hay path o el fichero no existe**. Defensiva.
>
> Tres. **`case` por extensión**: `.cs` → `dotnet format`, `.ts/.html/.scss` → eslint si existe `frontend/`, `.json/.md` → prettier si npx disponible.
>
> Cuatro. **`|| true`** después de cada formateador — si falla por alguna razón (fichero raro, config mala), **el hook no rompe la sesión**. Pedagogía: hooks que rompen la sesión son los más frustrantes. **Mejor tolerantes a fallos puntuales**.
>
> Cinco. **`exit 0`** explícito al final. Éxito limpio.
>
> Ahora **enganchamos el hook al evento**. Voy a `settings.json`."

**En VS Code, abro `.claude/settings.json` y añado la sección `hooks`:**

```json
{
  "permissions": {
    "...existing": "..."
  },
  "hooks": {
    "PostToolUse": [
      {
        "matcher": "Write|Edit|MultiEdit",
        "hooks": [
          {
            "type": "command",
            "command": "bash $CLAUDE_PROJECT_DIR/.claude/hooks/format-on-write.sh",
            "timeout": 60
          }
        ]
      }
    ]
  }
}
```

> "**Cuatro decisiones del JSON**:
>
> **Evento**: `PostToolUse` — después de la modificación.
>
> **Matcher**: `Write|Edit|MultiEdit` — los tres tools de modificación de ficheros.
>
> **Tipo**: `command` — el más común.
>
> **Comando**: `bash $CLAUDE_PROJECT_DIR/.claude/hooks/format-on-write.sh`. La gamma 3.3a slide 11 mencionó `$CLAUDE_PROJECT_DIR` como variable que Claude Code expone — **apunta a la raíz del proyecto**. Y prefijo `bash` explícito porque en Windows es lo que invoca Git Bash. **Path absoluto, no relativo**.
>
> **Timeout**: 60 segundos. `dotnet format` puede tardar.
>
> Salvo el `settings.json`. **Ahora hay que reiniciar Claude Code** porque los hooks se cargan al arrancar. La gamma 3.3a slide 13 lo dijo."

**Tiempo:** ~5 minutos.

---

### Bloque 8 — Probar el hook en directo (~3 min)

**En la terminal:**

```powershell
claude
```

```
✓ 6 project skills loaded
✓ 3 project agents loaded
✓ 1 project hook loaded: PostToolUse (Write|Edit|MultiEdit)
```

> "**Mirad la última línea**: `1 project hook loaded`. **Cargado**. Verifico con `/hooks`:"

**Tecleo:**

```
> /hooks
```

**Aparece (output ejemplo):**

```
Configured hooks
────────────────

PROJECT (.claude/settings.json)
  PostToolUse:
    - matcher: "Write|Edit|MultiEdit"
      handler: command
      command: bash $CLAUDE_PROJECT_DIR/.claude/hooks/format-on-write.sh
      timeout: 60s

USER (~/.claude/settings.json)
  (none)

LOCAL (.claude/settings.local.json)
  (none)
```

> "**Cargado correctamente**. Voy a forzar una modificación de fichero para ver el hook en acción. **Recordad que `RemoveItemHandler.cs` está deliberadamente mal formateado** — vamos a verlo antes de modificar."

**Salgo (Ctrl+C). En VS Code, abro `RemoveItemHandler.cs` y muestro:**

```csharp
using MediatR;
using OrderManagement.Application.Interfaces;
using   OrderManagement.Application.Exceptions;
using OrderManagement.Domain;

namespace OrderManagement.Application.Handlers;

public  class RemoveItemHandler : IRequestHandler<RemoveItemCommand, Unit>
{
private readonly IOrderRepository _repository;

    public RemoveItemHandler(IOrderRepository repository)
{
        _repository = repository;
    }

    public async Task<Unit> Handle(RemoveItemCommand request,CancellationToken cancellationToken){
            var order = await _repository.GetByIdAsync(request.OrderId, cancellationToken)??throw new OrderNotFoundException(request.OrderId);
order.RemoveItem(request.ProductId);
        await _repository.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
```

> "**Mal formato evidente**: `using` con doble espacio, `public  class` con doble espacio, indentación mal, llave de constructor en línea aparte, llave de método junto a paréntesis, espacios faltantes alrededor de operadores. **Visualmente incómodo**.
>
> Voy a pedirle a Claude que añada un comentario al fichero — un cambio mínimo para que se dispare `Edit`."

**Vuelvo a la terminal:**

```powershell
claude
```

**Tecleo:**

```
> Añade un comentario XML doc al método Handle de RemoveItemHandler.cs
  explicando qué hace en una sola línea.
```

**Aparece:**

```
● Read(src/OrderManagement.Application/Handlers/RemoveItemHandler.cs)

● Edit(src/OrderManagement.Application/Handlers/RemoveItemHandler.cs)
  Añadiendo XML doc al método Handle...

● [Hook PostToolUse triggered]
  Running: bash $CLAUDE_PROJECT_DIR/.claude/hooks/format-on-write.sh
  
  Determining projects to restore...
  Restored OrderManagement.Application/OrderManagement.Application.csproj
  Formatted code file: 'RemoveItemHandler.cs'.
  
  ✓ Hook completed in 4.2s

He añadido el comentario XML doc al método Handle.
```

> "**Mirad la secuencia**:
>
> Una. **Edit** ejecutado.
>
> Dos. **`[Hook PostToolUse triggered]`**. **El hook se disparó solo**. Sin que yo lo pidiera. Sin que Claude decidiera. **Automáticamente porque el matcher se cumplió**.
>
> Tres. **`dotnet format` ejecutado** sobre el fichero. *'Formatted code file: RemoveItemHandler.cs'*.
>
> Cuatro. **Hook completado en 4.2 segundos**. Dentro del timeout.
>
> **Vamos a ver el resultado en el fichero**:"

**Salgo y abro `RemoveItemHandler.cs` en VS Code:**

```csharp
using MediatR;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain;

namespace OrderManagement.Application.Handlers;

public class RemoveItemHandler : IRequestHandler<RemoveItemCommand, Unit>
{
    private readonly IOrderRepository _repository;

    public RemoveItemHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    /// <summary>Removes the specified item from the order.</summary>
    public async Task<Unit> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.OrderId, cancellationToken)
            ?? throw new OrderNotFoundException(request.OrderId);
        order.RemoveItem(request.ProductId);
        await _repository.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
```

> "**Mirad el resultado**:
>
> - `using` ordenados alfabéticamente y sin espacios sobrantes
> - `public class` con espacio único
> - Indentación consistente
> - Llaves alineadas correctamente
> - Espacios alrededor del operador `??`
> - Salto de línea limpio en la expresión que continúa
>
> **Y el comentario XML que pidió Claude está ahí**. Ambos cambios ocurrieron — el del agente (añadir comentario) **y el del hook (formatear)**.
>
> **Esto es la diferencia operativa de un hook**. **No tuve que pedir el formato**. **El agente no decidió formatear**. **Pasó porque tenía que pasar**.
>
> Y ahora cualquier persona del equipo que clone este repo y ejecute Claude Code va a tener exactamente el mismo comportamiento. **Hooks en project level = comportamiento garantizado para todo el equipo**."

**Tiempo:** ~3 minutos.

---

### Bloque 9 — Documentar y commitear (~1 min 30 seg)

**En VS Code, abro `docs/hooks-explorados.md` y actualizo la sección "Hooks construidos":**

```markdown
## Hooks construidos en este repo

### format-on-write (PostToolUse, project level)

- **Ubicación**: `.claude/hooks/format-on-write.sh` + sección hooks en
  `.claude/settings.json`.
- **Matcher**: `Write|Edit|MultiEdit`.
- **Comportamiento**: ejecuta `dotnet format` para `.cs`, `eslint --fix`
  para Angular (si existe `frontend/`), `prettier --write` para
  `.json`/`.md`.
- **Caso de prueba**: `RemoveItemHandler.cs` deliberadamente mal
  formateado, modificado por Claude para añadir XML doc, hook ejecuta
  `dotnet format` automáticamente. **Sin pedirlo**.
- **Tiempo de ejecución**: ~4-5s con `dotnet format` (caché en frío
  puede ser más).

## Eventos cubiertos hasta ahora

- ✅ `PostToolUse` con matcher `Write|Edit|MultiEdit`
- ⏳ `PreToolUse` con matcher `Bash` (pendiente para 3.3b)

## Lecciones extraídas (3.3a)

1. **Hooks viven dentro de `settings.json`**, no en fichero separado.
2. **El comando `bash` debe ser explícito** en Windows porque Git Bash
   es la shell que ejecuta `.sh`. En Linux/Mac no haría falta.
3. **`$CLAUDE_PROJECT_DIR`** apunta a la raíz del repo y es la forma
   correcta de referenciar paths del proyecto en hooks.
4. **`|| true`** después de cada formateador del script evita que un
   fallo puntual rompa la sesión completa.
5. **El hook se carga al arrancar** Claude Code. Si se modifica con
   sesión abierta, hay que reiniciar.
```

**Salvo. En la terminal:**

```powershell
git add .claude/settings.json .claude/hooks/ docs/hooks-explorados.md src/OrderManagement.Application/Handlers/RemoveItemHandler.cs
git commit -m "demo/3.3a-after: hook PostToolUse auto-format + RemoveItemHandler formateado"
```

> "Commit hecho. **Cuatro cosas en la rama ahora**: la sección `hooks` en `settings.json`, el script `format-on-write.sh`, las notas, y el `RemoveItemHandler` formateado por el hook. **Todo en git para que el equipo lo tenga al hacer pull**."

**Tiempo:** ~1 minuto 30 segundos.

---

### Bloque 10 — Recap, anti-patrones y cliffhanger a 3.3b (~2 min 30 seg)

> "Cinco ideas para llevarse al lunes. Y dos anti-patrones críticos."

**En el editor:**

```
LO QUE TIENES TRAS LA 3.3a

1. EL FRAME INSTRUCCIÓN VS GARANTÍA
   CLAUDE.md   → contexto, a veces se sigue
   Skill       → activación probabilística
   Hook        → CÓDIGO, garantía absoluta

2. ANATOMÍA
   - Vive DENTRO de settings.json
   - Evento + matcher + handler
   - Tres scopes: user, project, local
   - Comando /hooks para auditar

3. LOS DOS EVENTOS DEL 80%
   - PostToolUse / Write|Edit|MultiEdit  → formato, lint
   - PreToolUse / Bash                   → bloqueo de peligrosos

4. EL SISTEMA DE EXIT CODES
   - 0 = éxito
   - 2 = BLOQUEO ABSOLUTO (incluso en --dangerously-skip-permissions)
   - otros = non-blocking

5. EL PRIMER HOOK FUNCIONA
   - format-on-write con dotnet format
   - Probado en directo: RemoveItemHandler reformateado SIN PEDIRLO


ANTI-PATRONES CRÍTICOS DE HOOKS

❌ HOOKS DEMASIADO LENTOS
   Se ejecutan SÍNCRONAMENTE.
   Cada hook suma latencia a la herramienta.
   Mantén hooks < 200-500ms.
   Si necesitas más → background o evento distinto.

❌ HARDCODEAR LO QUE REQUIERE CRITERIO
   Hooks son para reglas DETERMINISTAS.
   Lo que requiere criterio → skill o subagente.
   "Si todo lo metes en hooks, vas a tener un sistema rígido."
```

> "**Cinco ideas, dos anti-patrones críticos**.
>
> El primero es importante: **hooks se ejecutan síncronamente**. Cada hook suma latencia a la herramienta. Si vuestro `PostToolUse` tarda cinco segundos, **cada `Write` tarda cinco segundos más**. La gamma 3.3a anti-patrón #1: **mantened hooks por debajo de 200-500ms**. Nuestro `dotnet format` tarda 4 segundos — está en el límite alto, **pero compensa** porque ahorra revisiones manuales.
>
> El segundo: **no hardcodear lo que requiere criterio**. Si la decisión es deterministica → hook. Si requiere razonamiento → skill o subagente. **Mezclar lo equivocado** lleva a un sistema rígido o uno con bucles raros."

**Cliffhanger:**

```
LO QUE VIENE EN 3.3b

CONSTRUIMOS EL SEGUNDO HOOK

  block-dangerous (PreToolUse / Bash)
  
  Bloqueo de comandos peligrosos:
    - rm -rf /
    - git push --force
    - DROP TABLE
    - fork bombs
    - ...

  Y la AMPLIACIÓN INTELIGENTE:
    Combinar regex (rápido, gratis) con
    handler "prompt" (LLM que evalúa cada Bash)
  → captura cosas que un regex no captaría


CASOS PRÁCTICOS COMPLETOS

  Demo de auto-format (esta) + bloqueo + observabilidad
  = el harness completo del equipo


OBSERVABILIDAD

  Por qué en agentes el debugging es DISTINTO
  (no determinismo, decisiones opacas, cadenas largas)
  
  Logging con SessionEnd hook, PostToolUse para tracing,
  SubagentStop para subagentes.

  El context bank + observabilidad = trazabilidad real.


CIERRE DEL MÓDULO 3 ENTERO

  Definición operativa del harness:
  
  harness = prompts + tools + context policies
            + hooks + feedback loops + observability


CHANNELS: REFERENCIA RÁPIDA

  MCP que hace push hacia Claude Code en lugar de pull.
```

> "**Empezamos con el tres punto tres punto B**."

**Tiempo:** ~2 minutos 30 segundos.

---

## 9. Qué resaltar verbalmente

Cinco puntos que **no pueden quedarse sin decir**:

1. **"Hooks son código, no instrucción. La diferencia es absoluta."** — el frame que vertebra el módulo 3.3. Bloque 2.

2. **"Vive dentro de `settings.json`, no en fichero separado."** — la confusión más común. Bloque 3.

3. **"Dos eventos cubren el 80%: `PostToolUse / Write|Edit|MultiEdit` y `PreToolUse / Bash`."** — el plan de adopción. Bloque 4.

4. **"Exit 2 bloquea incluso en `--dangerously-skip-permissions`. Garantía real, no recomendación."** — la pieza crítica de seguridad. Bloque 6.

5. **"`RemoveItemHandler` reformateado sin pedirlo. El agente no decidió. Pasó porque tenía que pasar."** — el momento aha. Bloque 8.

**Frase de remate al final:**

> *"Hooks: lo más simple del módulo 3 con el mayor impacto inmediato. Configurad el primero el lunes."*

---

## 10. Slide de entrada (locución HeyGen)

> **Texto del avatar antes del screencast:**

"Y vamos a la siguiente demo. La 3.3a. Primera demo del módulo 3.3 — hooks. La pieza determinista del harness. Lo más simple con el mayor impacto inmediato. Cuatro cosas en directo. Una, el frame instrucción vs garantía: `CLAUDE.md` es contexto, skills son activación probabilística, hooks son código que se ejecuta sin que el agente pueda decidir. La diferencia es absoluta. Dos, anatomía completa: viven dentro de `settings.json` (no en fichero separado), evento + matcher + handler, tres scopes, comando `/hooks` para auditar. Tres, los 17 eventos del ciclo de vida y los dos que cubren el 80%: `PostToolUse` con matcher Write|Edit|MultiEdit para auto-format, y `PreToolUse` con matcher Bash para bloqueo de peligrosos. Más los 4 tipos de handler y el sistema de exit codes — exit 2 bloquea incluso en modo `--dangerously-skip-permissions`. Garantía real, no recomendación. Cuatro, construimos el primer hook funcional: auto-format al modificar ficheros. Lo probamos sobre un `RemoveItemHandler.cs` deliberadamente mal formateado. Veréis el formato aplicarse sin pedirlo. Vamos al screencast."

---

## 11. Slide de salida (locución HeyGen)

> **Texto del avatar al volver del screencast:**

"Lo que acabáis de ver es la pieza determinista del harness funcionando. El hook `format-on-write` ejecutó `dotnet format` sobre `RemoveItemHandler` sin que ni vosotros ni Claude lo pidierais. Pasó porque tenía que pasar. Cinco ideas para el lunes. Una, hooks son código, no instrucción — la diferencia con CLAUDE.md y skills es absoluta. Dos, viven dentro de `settings.json`, no en fichero separado. Confusión común. Tres, dos eventos cubren el 80% — `PostToolUse` con matcher `Write|Edit|MultiEdit` para formato y lint, `PreToolUse` con matcher `Bash` para bloqueo de peligrosos. Empezad con esos dos. Cuatro, exit 2 es la herramienta más potente — bloqueo absoluto que funciona incluso en modo `--dangerously-skip-permissions`. Garantía real, no recomendación. Cinco, hooks rápidos siempre — bajo 500ms. Sumados a la latencia de las herramientas, hooks lentos se notan. En la siguiente demo, la 3.3b, construimos el segundo hook crítico: bloqueo de comandos peligrosos con regex y la ampliación inteligente con handler `prompt`. Más casos prácticos completos, observabilidad, channels y el cierre del módulo 3 entero con la definición operativa del harness. Empezamos con el tres punto tres punto B."

---

## 12. Tiempo total estimado

| Bloque | Tiempo |
|---|---|
| Slide de entrada (avatar) | ~30 seg |
| Bloque 1 — Setup y la pregunta del 3.2b | ~1 min 30 seg |
| Bloque 2 — El frame: instrucción vs garantía | ~3 min |
| Bloque 3 — Anatomía del hook | ~3 min |
| Bloque 4 — Los 17 eventos del ciclo de vida | ~3 min |
| Bloque 5 — Tipos de handler | ~2 min 30 seg |
| Bloque 6 — El sistema de exit codes | ~2 min 30 seg |
| Bloque 7 — Construir el primer hook | ~5 min |
| Bloque 8 — Probar el hook en directo | ~3 min |
| Bloque 9 — Documentar y commitear | ~1 min 30 seg |
| Bloque 10 — Recap, anti-patrones, cliffhanger | ~2 min 30 seg |
| Slide de salida (avatar) | ~30 seg |
| **Total screencast** | **~26-28 min** |
| **Total con avatar** | **~27-29 min** |

> Si hay preguntas durante el screencast, súmale 4-5 minutos. La demo encaja en un bloque de **35 minutos** dentro de la sesión.

**Margen de seguridad por si algo va lento:**

- **Si el hook NO se dispara** después de la edición (problema de matcher, problema de permisos del .sh en Windows, o `bash` no encontrado en PATH), debug en directo:
  - Primero, `/hooks` para ver si está cargado.
  - Segundo, prueba el script aislado: `cat <<EOF | bash .claude/hooks/format-on-write.sh` con un JSON de prueba (`{"tool_input":{"file_path":"src/OrderManagement.Application/Handlers/RemoveItemHandler.cs"}}`).
  - Tercero, si el script funciona aislado, el problema está en el matcher o en cómo Claude Code lanza el comando. Cambia `bash` por path absoluto a Git Bash: `C:\\Program Files\\Git\\bin\\bash.exe`. Comenta: *"en Windows a veces hay que ser explícito con el path a bash"*.

- **Si `dotnet format` tarda más de los 60s del timeout**, el hook se considera fallido pero la sesión continúa. Comenta: *"timeout de hook agotado. La acción del agente se completó pero el formateo no llegó a tiempo. Soluciones: subir timeout, o mover el formateo a un hook asíncrono"*. La pedagogía no se cae.

- **Si el comando `/hooks` muestra una UI distinta** en tu versión, lee la información disponible y comenta lo que veas. La idea es solo que el hook está cargado.

- **Si los `||true` del script causan que falle silenciosamente** (formato no aplicado, sesión sigue), abre el fichero después y muestra que no cambió. Comenta: *"el `|| true` evita romper la sesión pero también puede ocultar problemas. Trade-off: tolerancia vs visibilidad. Para producción, mejor que escriba a un log"*. La pedagogía gana matiz.

- **Si te quedas sin tiempo y los bloques 5 y 6 te aprietan**, recorta el bloque 5 (tipos de handler) a 1 min 30 seg cubriendo solo `command` y mencionando rápido los otros tres. El bloque 6 (exit codes) no recortes — es contenido crítico de seguridad.

- **Si surge la pregunta sobre cómo afecta a subagentes**, responde corto: *"sí, los hooks aplican también cuando un subagente invoca herramientas. Esto es bueno para seguridad — los subagentes no pueden saltarse vuestras reglas. Pero hay que tenerlo en cuenta porque puede generar comportamiento inesperado si el hook no contempla ese caso"*.

---

# Apéndice A — Decisiones pedagógicas justificadas

> Esta sección no se graba. Es para Pedro, formador.

**¿Por qué empezar con `format-on-write` y no con `block-dangerous`?**

Porque **`format-on-write` es el caso pedagógico más limpio**:
1. **Resultado visible en pantalla**: el alumno ve el código mal formateado *antes*, ve el hook ejecutarse, ve el código bien formateado *después*. **Impacto visual directo**.
2. **Riesgo cero**: si algo va mal, el peor caso es que el formato no se aplique. **No bloquea el flujo**.
3. **Aprendizaje incremental**: en 3.3b, ya con el modelo mental de hooks claro, podemos ir al caso más serio (bloqueo, exit 2, seguridad).

Si arrancara con `block-dangerous`, la primera prueba en directo sería bloquear un comando — el alumno vería un "Bloqueado" pero no sentiría la diferencia operativa del hook ejecutándose. **Auto-format es más concreto pedagógicamente**.

**¿Por qué `RemoveItemHandler` y no otro fichero?**

Por tres razones:
1. **Es un fichero nuevo**, sin tests dependientes. No rompe nada al modificarlo.
2. **El handler es de tamaño medio** — suficiente para ver mejoras visuales evidentes pero no tan grande que el formato sea sutil.
3. **Continúa el patrón** del módulo 2/3 de usar handlers de OrderManagement como casos. **Coherencia con el hilo conductor**.

**¿Por qué el script tiene `|| true` después de cada formateador?**

Para que **un fallo puntual no rompa la sesión**. Si `dotnet format` falla porque el fichero está en un proyecto raro, o si `prettier` no encuentra config, **el hook no debe abortar el flujo** — debe seguir y pasar el siguiente formateador o terminar limpio. **Trade-off explícito** entre tolerancia y visibilidad. Lo menciono en el guion como matiz.

**¿Por qué incluir `bash` explícito en el comando del JSON?**

Porque en **Windows nativo**, los `.sh` con shebang no se ejecutan directamente como en Linux/Mac — requieren invocación explícita. **`bash $CLAUDE_PROJECT_DIR/.claude/hooks/format-on-write.sh`** garantiza que Git Bash (instalado con Git for Windows) procese el script. Sin `bash` explícito, la ejecución puede fallar dependiendo del PATH del entorno.

**¿Por qué construir el primer hook en project level y no en user level?**

Porque la pedagogía de la demo es **"hooks que el equipo comparte"**. Project level **va a git con el repo**. Cualquiera que clone el repo tiene el hook al instante. Esto es lo que más rentabilidad tiene en empresa — *"el equipo entero formatea igual"*. User level es para preferencias individuales (lo veremos en 3.3b con `block-dangerous`, que es **más natural en user level** porque viaja con vosotros).

**¿Por qué `RemoveItemHandler` se queda formateado en la rama, no se revierte?**

Porque **el formato correcto es el estado deseado**. Si revirtiera, la rama tendría código mal formateado que rompe la coherencia del proyecto. **Disciplina de scope diferente** que en demos anteriores donde el anti-patrón sí se revertía (porque el anti-patrón era pedagógico, no estado deseado). Aquí: el mal formato era pedagógico (caso de prueba), el buen formato **es lo correcto**.

**¿Por qué el bloque 6 (exit codes) viene antes del bloque 7 (construir el hook)?**

Porque **exit codes definen QUÉ hace un hook después de ejecutarse** — pieza crítica para entender el comportamiento. Si lo dejara para después, el alumno construiría el hook sin saber que **exit 2 podría usarse para bloquear**. Mejor: tener el frame mental completo antes de construir. Y como `format-on-write` usa exit 0, la teoría del 2 queda preparada para 3.3b.

**¿Por qué el cliffhanger menciona "ampliación inteligente con handler `prompt`"?**

Porque **es la siembra del patrón hooks inteligentes** que la 3.3b va a desarrollar. La gamma 3.3a slide 21 lo introdujo. Mencionarlo en el cliffhanger conecta los dos submódulos del 3.3 con un hilo concreto: *"el lunes empezáis con regex, después añadís LLM"*. **Camino de adopción incremental claro**.

**¿Por qué no creo `.claude/settings.local.json` para ningún hook?**

Porque la pedagogía del local-level llega más naturalmente cuando el alumno **ya tenga hooks personales que quiera probar antes de subir a project**. Para esta demo, los dos scopes que importan son user (siguiente demo) y project (esta). **Mantener foco**.

**¿Por qué no mostrar el hook ejecutándose dentro de un subagente?**

Por dos razones:
1. **Complejidad innecesaria** para la primera demo de hooks. La idea de "hooks aplican también en subagentes" la menciono en margen de seguridad como respuesta a pregunta, no en el guion principal.
2. **El caso es mucho más visual con un Edit del agente principal** que con la chain de un subagente — el alumno ve directamente el hook + edición.

Si surgiera la pregunta, responde según el plan de margen. Pero no lo metas proactivamente.

**¿Por qué la hoja del hook tiene timeout 60s en lugar del 30s recomendado?**

Porque **`dotnet format` puede tardar más de 30s** en proyectos con caché frío o con muchos proyectos en la solution. La gamma 3.3a slide 19 mencionó `timeout: 30` como ejemplo, pero **timeout debe asociarse al comando real**. 60s es realista para `dotnet format` sin sobrar mucho. **Decisión calibrada al caso, no copia del slide**.
