# Demo 3.3b — Bloqueo de comandos peligrosos, observabilidad y cierre del módulo 3 entero

> **Versión:** v1 | **Módulo:** 3 | **Sub:** 3.3b | **Estado:** ✅ Versión final
> **Archivo:** `demo_M03-S3.3b-casos-practicos-observabilidad-cierre-windows-v1.md`
> **Branch before:** `demo/3.3b-before`  (preparado: harness-completo.md + .gitignore + hooks-explorados.md ampliado, sin hook de observabilidad ni bloqueo)
> **Branch after:**  `demo/3.3b-after`   (estado final pre-cocinado con el hook SessionEnd commiteado y los logs ignorados)
> **Branch parent:** `demo/3.3a-after`
> **Tiempo total estimado:** ~28-32 minutos
> **Tipo:** Demo de cierre del módulo 3 entero (INFRA). **Construye el segundo hook crítico (bloqueo de comandos peligrosos con `PreToolUse` y `exit 2`), añade la ampliación inteligente con handler `prompt`, monta observabilidad básica con hook `SessionEnd`, y cierra el módulo 3 con la definición operativa del harness completo: `harness = prompts + tools + context policies + hooks + feedback loops + observability`.** Sigue el patrón **before/after** definido en [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md).
>
> **Nota sobre `block-dangerous`:** vive en `~/.claude/settings.json` (user level, máquina del alumno) y por tanto **no entra al repo**. Se demuestra durante el screencast pero no forma parte de `demo/3.3b-after`. Lo que sí entra al repo: el hook `SessionEnd` (project level) + `.claude/logs/` ignorada + documentos.
> **Plataforma:** Windows 11 (PowerShell 7 + Git Bash, **no WSL**).

---

## 1. Contexto

Cerramos la 3.3a con el primer hook funcional (`format-on-write` en project level) y el frame instrucción vs garantía claro. La 3.3b cierra el módulo 3 entero. **Cinco temas grandes** según la gamma 3.3b (35 slides, ~30 min):

1. **Bloqueo de comandos peligrosos** (slides 9-14) — el segundo caso práctico, **el equivalente del cinturón de seguridad**: lo configuras una vez, te olvidas, y el día que importa estás vivo gracias a él. Hook `PreToolUse / Bash` con `exit 2`.
2. **Ampliación inteligente con handler `prompt`** (slides 15-16) — combinar regex (rápido, gratis, captura obvios) con LLM (más caro, captura sutiles). El patrón potente.
3. **Channels como referencia rápida** (slides 17-18) — MCP que hace push hacia Claude Code en lugar de pull. Conceptual.
4. **Observabilidad: la pieza que cierra el harness fiable** (slides 22-28) — por qué en agentes el debugging es distinto, qué loggear, cómo se hace con hooks (`SessionEnd`, `PostToolUse`, `SubagentStop`), conexión con context bank.
5. **El harness completo: definición operativa** (slides 29-31) — `harness = prompts + tools + context policies + hooks + feedback loops + observability`. **Cada pieza el alumno la tiene ya**.

Esta demo aterriza la teoría con tres construcciones progresivas:

- **Showcase del bloqueo de comandos peligrosos**: hook en **user level** (no project) con `PreToolUse` + `Bash` + `exit 2`. Probado con `rm -rf /` y `git push --force` que el hook bloquea **incluso si activáramos `--dangerously-skip-permissions`**.
- **Ampliación inteligente con handler `prompt`**: añadir Haiku como segundo capa que evalúa comandos sutiles con criterio.
- **Hook de observabilidad**: `SessionEnd` que vuelca un log estructurado a `.claude/logs/sessions.jsonl`. Trazabilidad real para debugging futuro.
- **Cierre conceptual** del módulo 3 con la definición operativa del harness y las dos preguntas para casa antes del módulo 4.

> **Tipo de demo:** cierre integrador del módulo 3. La rama `demo/3.3b-after` queda con el hook de observabilidad en project level (`SessionEnd` + `log-session.sh`), notas finales en `docs/hooks-explorados.md`, y el módulo 3 cerrado a nivel de repo. El hook `block-dangerous` (user level) se demuestra en directo pero vive en la máquina del alumno, fuera del repo. **Es la última demo del módulo 3 entero — el harness completo del curso queda funcionando**.

---

## 2. Objetivo de la demo

Cinco cosas concretas que tienen que quedar en la cabeza del alumno cuando termine los ~28 minutos de screencast:

1. **El hook `block-dangerous` va en user level, no en project**. Viaja con vosotros a todos los repos. Y **bloquea incluso en modo `--dangerously-skip-permissions`**. *"Cuando alguna vez intentéis algo destructivo y veáis el bloqueo, os vais a alegrar de tenerlo"*. La gamma 3.3b slide 14.

2. **Combinar regex + handler `prompt`**: regex para los obvios (rápido, gratis), LLM para los sutiles (más caro pero más cobertura). **El patrón potente** sin ser dogmático.

3. **En agentes, observabilidad es distinta**. No-determinismo + decisiones opacas + cadenas largas. **Sin logs estructurados, debugging es adivinación**. La gamma 3.3b slide 23: *"los agentes que llegan a producción tienen siempre algún sistema de observabilidad. Los que no, no llegan"*.

4. **El harness completo en una fórmula**: `harness = prompts + tools + context policies + hooks + feedback loops + observability`. **Cada pieza el alumno la tiene ya** después de los módulos 1-3. **Frame de cierre del módulo entero**.

5. **El lunes empezáis con DOS hooks**: el `block-dangerous` en user (transversal a todos los repos) y el `format-on-write` en project (que ya construimos en 3.3a). **Esos dos cubren el 80% del valor**.

Y dos cosas que tienen que **NO quedar** en su cabeza:

- *"Hooks resuelven todo."* — falso. La gamma 3.3b slide 21 anti-patrón #6: *"hardcodear lo que requiere razonamiento. Hay cosas que sí merecen criterio. Para eso están skills y subagentes"*.
- *"Logging es opcional para empezar."* — depende. Para uso personal, no es crítico. Para uso en equipo o si vais a poner Claude Code en flujos serios, es **no opcional**. Sin logs estructurados, los problemas que aparezcan serán imposibles de debuggear.

---

## 3. Branch `demo/3.3b-before`

Punto de partida del screencast.

```
demo/3.3b-before
```

**Parte de:** `demo/3.3a-after`.

**Estado del repo:** todo lo de `demo/3.3a-after` (6 skills, 3 subagentes, 1 hook `format-on-write` en project level, context bank, `RemoveItemHandler.cs` formateado) más tres artefactos preparatorios:

1. **`docs/harness-completo.md`** — documento de cierre del módulo 3 con la fórmula operativa y las dos preguntas para el módulo 4 (estructura completa, listo para que el formador lo lea en pantalla durante el bloque de cierre).
2. **`docs/hooks-explorados.md`** ampliado con las secciones finales del módulo 3.3 (eventos cubiertos, hooks por scope, lecciones extraídas).
3. **`.gitignore`** ya actualizado para excluir `.claude/logs/`.

**Qué NO hay en `-before`:**
- **Sin sección `SessionEnd` en `.claude/settings.json`** — eso es la pieza viva.
- **Sin `.claude/hooks/log-session.sh`** — se crea en directo.
- **Sin marca `[x]`** en `docs/DEMOS.md` para 3.3b.
- **Sin `~/.claude/settings.json` con `block-dangerous`** — eso vive en la máquina del alumno (user level), no en el repo, y se demuestra en directo sin que entre a ninguna rama.

> El formador hace `git checkout demo/3.3b-before` antes de empezar a grabar.

---

## 4. Branch `demo/3.3b-after`

Estado final que cierra el módulo 3 (los Módulos 4 y 5 parten de aquí).

```
demo/3.3b-after
```

**Parte de:** `demo/3.3b-before`.

**Qué añade respecto a `-before`:**

1. **Hook de observabilidad** en `.claude/settings.json` — `SessionEnd` con script `.claude/hooks/log-session.sh` que vuelca a `.claude/logs/sessions.jsonl`.
2. **Marca `[x]`** en `docs/DEMOS.md` para 3.3b.

**Lo que NO entra al repo (intencional):**
- El hook `block-dangerous` vive en `~/.claude/settings.json` del usuario. Es transversal a todos los repos del alumno y no se commitea a `demo/3.3b-after`. Se documenta en `hooks-explorados.md` como instrucción de instalación.

**Cómo se prepara:** ver §6b. Se materializa antes de grabar.

> Durante la grabación, el formador construye `block-dangerous` en `~/.claude/settings.json` (su máquina) y `log-session` en `.claude/settings.json` (el repo). Al cerrar descarta los cambios reales del repo (los del user level se quedan en su máquina como demostración) y la siguiente fase del curso parte de `demo/3.3b-after` ya pre-cocinada con el hook de observabilidad.

---

## 5. Estado del repo al hacer `git checkout demo/3.3b-before`

Casi idéntico a `demo/3.3a-after`, con los tres artefactos preparatorios añadidos:

```
ordermanagement/
├── .claude/
│   ├── settings.json                    (con hooks/PostToolUse de 3.3a)
│   ├── skills/                          (6 skills)
│   ├── agents/                          (3 subagentes)
│   └── hooks/                           (con format-on-write.sh de 3.3a)
├── docs/
│   ├── DEMOS.md
│   └── hooks-explorados.md              (con notas del 3.3a)
├── src/                                  (con RemoveItemHandler.cs formateado)
├── frontend/
├── tests/
├── CLAUDE.md
├── .gitignore                           (incluye workflow-state)
└── README.md
```

**Estado clave para esta demo:**

- **`~/.claude/settings.json` no tiene la sección hooks** todavía. La añadimos en directo para `block-dangerous`.
- **`~/.claude/hooks/`** no existe. Se crea en directo.
- Para probar el bloqueo end-to-end vamos a intentar ejecutar **un comando deliberadamente peligroso** desde Claude Code. La idea: que el alumno **vea el bloqueo en pantalla**.

**Estado de la máquina Windows del formador:**

```
✅ Claude Code v2.1.x con hooks operativos
✅ Git for Windows + Git Bash
✅ PowerShell 7
✅ VS Code con el repo en demo/3.3b-before
✅ Hook format-on-write de 3.3a operativo
✅ jq disponible (necesario para parsear JSON en los scripts)
```

> **Nota Windows crítica para Pedro**: necesitamos `jq` para parsear el JSON que llega por stdin a los hooks. Verifica antes de grabar: `jq --version` en PowerShell debe responder. Si no está, instálalo con `winget install jqlang.jq` o `choco install jq`. Sin `jq`, los scripts pueden funcionar con `grep`/`awk` pero el guion los usa por simplicidad.

**Lo que el alumno verá al final de la demo:**

- Construcción del hook `block-dangerous` en user level con explicación de **por qué user y no project**.
- El script con la lista de patrones bloqueados (`rm -rf /`, `git push --force`, `DROP TABLE`, fork bombs).
- **Prueba en directo**: pedir a Claude que ejecute `git push --force` → hook bloquea con `exit 2` → mensaje claro al agente.
- Ampliación con handler `prompt` mostrada como ejemplo conceptual (sin construir, por coste de tokens).
- Channels mencionados como referencia rápida.
- Hook de observabilidad con `SessionEnd` construido y visto generar entrada en `sessions.jsonl`.
- Conexión visualizada entre context bank + observabilidad = trazabilidad completa.
- Cierre del módulo 3 entero con la fórmula del harness completo y las dos preguntas para el módulo 4.

---

## 6a. Prompt para Claude Code — preparar `demo/3.3b-before`

> Crea la rama de partida del screencast desde `demo/3.3a-after` con tres artefactos preparatorios: `docs/harness-completo.md` (documento de cierre del módulo 3), `.gitignore` actualizado para excluir `.claude/logs/`, y `docs/hooks-explorados.md` ampliado con las secciones finales del módulo 3.3. **No crea hook alguno** — esa es la pieza viva.

````
Estoy preparando la demo 3.3b del curso de Claude Code (cierre del
módulo 3 con bloqueo de peligrosos, observabilidad y harness completo).
Sigue el patrón before/after (ver demo M0.2).

Quiero que prepares la rama `demo/3.3b-before` desde `demo/3.3a-after`
con TRES artefactos preparatorios y NADA del hook real.

# Lo que necesito

Cuatro tareas:

## Tarea 1: crear la rama

```powershell
git checkout demo/3.3a-after
git pull
git checkout -b demo/3.3b-before
```

## Tarea 2: actualizar .gitignore

Añade al final, después de la entrada de workflow-state:

```
# Logs de observabilidad (locales por dev, no van a git)
.claude/logs/
```

## Tarea 3: crear docs/harness-completo.md

Contenido:

```markdown
# El harness completo — definición operativa del módulo 3

## La fórmula

> **harness = prompts + tools + context policies + hooks + feedback loops + observability**

## Cada pieza, dónde la tenéis

### Prompts
- **CLAUDE.md** del proyecto (módulo 1) con las convenciones del equipo
- **6 skills** en `.claude/skills/` (módulo 2):
  - `angular-component` (con `context: fork`)
  - `commit-style`
  - `db-reset` (con `disable-model-invocation`)
  - `frontend-design` (oficial Anthropic)
  - `pre-commit-check` (orquestador con loop + context bank)
  - `pre-pr-check` (fan-out paralelo)

### Tools
- Tools del agente principal: Read, Write, Edit, Bash, Grep, Glob...
- MCP servers (módulos 1 y 4)

### Context policies
- **3 subagentes** en `.claude/agents/` (módulo 3.1):
  - `repo-explorer` (Haiku, read-only)
  - `dotnet-reviewer` (Sonnet, read + git diff)
  - `convention-checker` (Haiku, read + git diff)
- Tools restringidos por rol
- Scopes: user / project / local

### Hooks
- **format-on-write** (project level, PostToolUse / Write|Edit|MultiEdit)
- **block-dangerous** (user level, PreToolUse / Bash) — añadido en 3.3b
- **log-session** (project level, SessionEnd) — añadido en 3.3b

### Feedback loops
- Loop validator → implementer en `pre-commit-check` (techo 3 iteraciones)
- Loop fan-out → fan-in en `pre-pr-check`
- Context bank en `.claude/workflow-state/<session>/`

### Observability
- Hook `SessionEnd` que vuelca a `.claude/logs/sessions.jsonl`
- Context bank ya provee trazabilidad de workflows

## La idea final

Cuando personalizáis Claude Code con todas estas piezas, no estáis
configurando una herramienta. **Estáis construyendo vuestro propio
harness encima del de Anthropic**. Vuestro harness sabe a vuestro
equipo. Sabe vuestras convenciones. Sabe delegar. Sabe corregir.
Sabe garantizar. Sabe loggear.

## Las dos preguntas antes del módulo 4

1. ¿Qué hook concreto vais a configurar el lunes en vuestro repo del
   trabajo? Si la respuesta es "el de auto-format" o "el de bloquear
   peligrosos", perfecto.

2. ¿Hay alguien en vuestro equipo de diseño con quien colaboréis y que
   ya use Figma? Si sí, el módulo 4 va a tener nombre y apellidos.

## Lectura complementaria opcional

Para roles que decidan arquitecturas a nivel sistema:
**"Building Effective AI Agents: Architecture Patterns and Implementation
Frameworks"** — Anthropic. Los patrones que vimos aquí (hierarchical,
collaborative, sequential, parallel, evaluator-optimizer) en versión
formal.
```

## Tarea 4: completar docs/hooks-explorados.md

Localiza al final del documento la sección "Eventos cubiertos hasta
ahora" y reemplázala por:

```markdown
## Eventos cubiertos al cerrar el módulo 3

- ✅ `PostToolUse` con matcher `Write|Edit|MultiEdit` (3.3a) → format-on-write
- ✅ `PreToolUse` con matcher `Bash` (3.3b) → block-dangerous
- ✅ `SessionEnd` (3.3b) → log-session

## Hooks por scope al cerrar el módulo 3

### User level (~/.claude/settings.json)
- **block-dangerous** — viaja con vosotros a todos los repos.
  Bloquea `rm -rf /`, `git push --force`, `DROP TABLE`, etc.
  con exit 2 (incluso en modo `--dangerously-skip-permissions`).

### Project level (.claude/settings.json) — van a git con el repo
- **format-on-write** (3.3a) — auto-formato al modificar ficheros.
- **log-session** (3.3b) — observabilidad básica.

## Lecciones extraídas (módulo 3.3 entero)

1. **Hooks son código, no instrucción**. La diferencia con CLAUDE.md
   y skills es absoluta.
2. **Empezad con dos hooks**: format-on-write (project) y block-dangerous
   (user). Cubren el 80% del valor.
3. **Exit 2 bloquea incluso en `--dangerously-skip-permissions`**.
   Garantía real, no recomendación.
4. **Mantened hooks bajo 500ms** salvo formateadores que tarden por
   naturaleza (como `dotnet format`).
5. **Observabilidad NO es opcional** para flujos serios. Sin logs
   estructurados, debugging es adivinación.
6. **`bash` explícito y `$CLAUDE_PROJECT_DIR`** son las dos claves
   para hooks portables en Windows.
```

## Tarea 5: verificar build y commitear

```powershell
dotnet build
```

Esperado: 0 warnings, 0 errors.

```powershell
git add docs/harness-completo.md docs/hooks-explorados.md .gitignore
git commit -m "demo/3.3b-before: artefactos preparatorios (harness-completo + notas finales 3.3 + gitignore)"
```

NO hagas push.

# Restricciones (importantes)

- NO marques `[x]` en `docs/DEMOS.md` todavía. Eso va en `-after`.
- NO crees `.claude/hooks/log-session.sh`. Es la pieza viva.
- NO modifiques `.claude/settings.json`. La sección hooks de SessionEnd va en `-after`.
- NO modifiques `~/.claude/settings.json` (user level) — eso lo hace el formador en directo.
- NO crees `.claude/logs/`. La crea el hook en runtime.
- NO modifiques skills, subagentes, CLAUDE.md, código.

# Cuando termines, dime

1. Que la rama demo/3.3b-before está creada desde demo/3.3a-after.
2. Que .gitignore tiene .claude/logs/ excluido.
3. Que docs/harness-completo.md está creado con la fórmula.
4. Que docs/hooks-explorados.md está completado.
5. Que el build pasa.
````

---

## 6b. Prompt para Claude Code — preparar `demo/3.3b-after`

> Materializa la rama final con el hook `SessionEnd` operativo (project level) y la marca `[x]` en `docs/DEMOS.md`. El hook `block-dangerous` que el formador construye en directo en `~/.claude/settings.json` **no entra al repo** — vive en la máquina del alumno y se documenta en `hooks-explorados.md`.

````
Estoy preparando la demo 3.3b del curso de Claude Code (cierre del
módulo 3). Esta rama -after pre-cocina el hook SessionEnd
(observabilidad project level) que el formador construirá en vivo.

# Contexto

Estoy en la rama `demo/3.3b-before` del repo `ordermanagement`. La rama
parte de `demo/3.3a-after` y tiene los artefactos preparatorios
(harness-completo.md, hooks-explorados.md ampliado, .gitignore con
.claude/logs/) pero NO tiene aún el hook de observabilidad ni la marca
[x] en DEMOS.md.

Quiero que prepares la rama `demo/3.3b-after` desde `demo/3.3b-before`
con el hook SessionEnd y la marca de DEMOS.md. El hook block-dangerous
NO va al repo (vive en user level, en la máquina del alumno).

# Lo que necesito

Tres tareas:

## Tarea 1: crear la rama

```powershell
git checkout demo/3.3b-before
git checkout -b demo/3.3b-after
```

## Tarea 2: ampliar `.claude/settings.json` con el hook SessionEnd

Mantén intacta la sección `hooks` que ya viene de la 3.3a (PostToolUse
format-on-write) y AÑADE un hook `SessionEnd` con handler `command` que
ejecute `bash $CLAUDE_PROJECT_DIR/.claude/hooks/log-session.sh`.

## Tarea 3: crear `.claude/hooks/log-session.sh` + marcar DEMOS.md + commit

Script bash con shebang `#!/bin/bash` que:
- Crea `.claude/logs/` si no existe (`mkdir -p`).
- Lee de stdin el JSON con metadata de la sesión.
- Extrae timestamp, sessionId, tokens consumed (si están en el JSON) usando `jq`.
- Anexa una línea JSON al fichero `.claude/logs/sessions.jsonl` con
  `{ timestamp, sessionId, tokens, exitReason }`.
- Sale con exit 0.

Marca la 3.3b en `docs/DEMOS.md`:

```
- [x] **demo/3.3b** — Bloqueo de peligrosos, observabilidad, cierre módulo 3
```

Verifica con `dotnet build` (0 warnings, 0 errors) y commit:

```powershell
git add .claude/settings.json .claude/hooks/log-session.sh docs/DEMOS.md
git commit -m "demo/3.3b-after: hook SessionEnd con log-session + cierre módulo 3"
```

NO hagas push.

# Restricciones (importantes)

- NO modifiques `~/.claude/settings.json` (user level). El hook
  block-dangerous se demuestra en vivo y vive en la máquina del alumno.
- NO crees `.claude/hooks/block-dangerous.sh` en el repo. No va al repo.
- NO modifiques skills, subagentes, CLAUDE.md, código.
- NO toques los hooks ya existentes (format-on-write de 3.3a se mantiene).

# Cuando termines, dime

1. Que la rama demo/3.3b-after está creada desde demo/3.3b-before.
2. Que `.claude/settings.json` tiene los DOS hooks (PostToolUse de 3.3a + SessionEnd nuevo).
3. Que `.claude/hooks/log-session.sh` existe.
4. Que docs/DEMOS.md está marcado con 3.3b.
5. Que dotnet build pasa.
6. Que el commit está hecho.

Si tienes dudas, para y pregúntame.
````

---

## 7. Artefactos que Claude Code debe generar

```
✓ Rama demo/3.3b-before (parte de demo/3.3a-after) con:
  ├── .gitignore con .claude/logs/ excluido
  ├── docs/harness-completo.md (cierre del módulo 3)
  └── docs/hooks-explorados.md completado
✓ Rama demo/3.3b-after (parte de demo/3.3b-before) con:
  ├── .claude/settings.json ampliado con SessionEnd
  ├── .claude/hooks/log-session.sh (nuevo)
  └── docs/DEMOS.md con 3.3b marcada [x]
✓ Build OK
```

**Lo que NO debe haber generado:**

- ❌ `.claude/hooks/block-dangerous.sh` (en vivo)
- ❌ `.claude/hooks/log-session.sh` (en vivo)
- ❌ Modificación a `.claude/settings.json` (en vivo)
- ❌ Modificación a `~/.claude/settings.json` (en vivo)
- ❌ Cambios en skills, subagentes, código

> Si Claude Code se anticipa, **se rechaza el output**.

**Lo que el formador commitea EN VIVO sobre `demo/3.3b-before` durante el screencast:**

```
Durante la grabación, sobre demo/3.3b-before, se hace un commit ficticio:

1. "demo/3.3b-after: hook log-session para observabilidad"
   └── .claude/settings.json (MODIFICADO con SessionEnd)
   └── .claude/hooks/log-session.sh (NUEVO)

(El hook block-dangerous se hace en user level (~/.claude/settings.json)
 — NO va al repo del proyecto, vive en la máquina del alumno.)

Al cerrar el screencast: el formador descarta el commit real.
La siguiente fase del curso parte de demo/3.3b-after (pre-cocinada
en §6b), que es equivalente al resultado del screencast.
```

> **Importante**: el `block-dangerous` se hace en `~/.claude/settings.json` y `~/.claude/hooks/` — **fuera del repo del proyecto**. No se commitea. La pedagogía: este hook **es personal, viaja con vosotros**. La rama del repo solo recibe el `log-session` y el `harness-completo.md`.

**Estado final del árbol después del screencast:**

```
ordermanagement/                         (proyecto)
├── .claude/
│   ├── settings.json                    ← MODIFICADO (sección hooks ampliada)
│   ├── skills/                          (sin cambios)
│   ├── agents/                          (sin cambios)
│   ├── hooks/
│   │   ├── format-on-write.sh           (de 3.3a)
│   │   └── log-session.sh               ← NUEVO
│   └── logs/                            ← NUEVA (gitignored)
│       └── sessions.jsonl               ← generado por el hook en vivo
├── docs/
│   ├── DEMOS.md                         (modificado)
│   ├── hooks-explorados.md              (completado)
│   └── harness-completo.md              ← NUEVO (cierre módulo 3)
└── ...

~/.claude/                                (HOME del usuario, fuera del repo)
├── settings.json                        ← MODIFICADO en vivo
└── hooks/
    └── block-dangerous.sh               ← NUEVO en vivo
```

---

## 8. Guion del screencast

**Duración estimada del screencast: ~26-30 minutos.**

Once bloques. La demo más larga del módulo 3 — **cierra el módulo entero** y cubre cinco temas grandes con dos showcases prácticos (bloqueo + observabilidad).

> **Antes de empezar a grabar**, asegúrate de:
> - Tener PowerShell 7 abierto en pantalla completa, fuente al menos 16pt.
> - Tener VS Code abierto con el repo en `demo/3.3b-before`.
> - **Verificar Git Bash y `jq`**: `bash --version` y `jq --version` deben responder.
> - **Verificar que `~/.claude/settings.json` existe** (lo creó la demo 1.2b o equivalente). Si no existe, crearlo vacío con `{}` antes de empezar.
> - **Importante**: prepara mentalmente que vas a ejecutar comandos peligrosos en demo. **Trabaja en una VM, snapshot, o repo desechable** si tienes dudas, aunque el `git push --force` que probaremos no debería ejecutarse porque el hook lo bloquea.
> - Cerrar Slack, Teams, navegadores con notificaciones.

---

### Bloque 1 — Setup y la pregunta del 3.3a (~1 min 30 seg)

**Pantalla compartida.** A la izquierda VS Code con el repo en `demo/3.3b-before`. A la derecha terminal PowerShell.

**En la terminal:**

```powershell
git status
ls .claude\hooks\
```

```
On branch demo/3.3b
nothing to commit, working tree clean

Mode    LastWriteTime    Length Name
----    -------------    ------ ----
-a---   ...                     format-on-write.sh
```

**Lo que dices:**

> "Estamos en `demo/3.3b-before`. **Última demo del módulo 3 entero**. Cerramos la 3.3a con el primer hook funcional — `format-on-write` en project level. Hoy:
>
> Una. **Construimos el segundo hook crítico**: bloqueo de comandos peligrosos. Pero **en user level, no en project**. Os explico por qué. Y veréis el bloqueo en directo intentando ejecutar `git push --force`.
>
> Dos. **La ampliación inteligente**: combinar regex con handler `prompt` para capturar comandos sutiles que un regex no captaría. La gamma 3.3b slide 15 lo cubrió.
>
> Tres. **Channels** — referencia rápida.
>
> Cuatro. **Observabilidad**. La pieza que cierra el harness fiable. La gamma 3.3b slide 22 lo dijo: *'los agentes que llegan a producción tienen siempre algún sistema de observabilidad. Los que no, no llegan'*. Construimos un hook `SessionEnd` con logging estructurado.
>
> Y cinco. **Cierre del módulo 3 entero** con la fórmula operativa del harness completo y las dos preguntas para el módulo 4.
>
> Empezamos."

**Tiempo:** ~90 segundos.

---

### Bloque 2 — Por qué `block-dangerous` va en user level (~2 min)

> "**Decisión clave antes de construir**: ¿user level o project level? La gamma 3.3b slide 14 lo respondió claramente."

**En el editor de texto al lado, escribo (contenido pedagógico):**

```
PROJECT LEVEL vs USER LEVEL — para block-dangerous

PROJECT (.claude/settings.json)
─────────────────────────────────
  ✓ Va a git, lo comparte el equipo
  ✗ Solo aplica DENTRO de este repo
  ✗ Si saltas a otro repo, no te protege


USER LEVEL (~/.claude/settings.json)
──────────────────────────────────────
  ✓ Viaja CONTIGO a todos los repos
  ✓ Te protege en CUALQUIER trabajo que hagas con Claude Code
  ✓ rm -rf en repo de cliente importante = bloqueado igual


┌──────────────────────────────────────────────────────────┐
│  RECOMENDACIÓN DE LA GAMMA 3.3b SLIDE 14                 │
│                                                          │
│  "Este hook va siempre. En tu user-level (~/.claude/    │
│   settings.json), no en el proyecto. Así viaja contigo  │
│   a todos los repos."                                    │
│                                                          │
│  "Cuando alguna vez intentes hacer algo destructivo     │
│   y veas el bloqueo, te vas a alegrar de tenerlo."      │
└──────────────────────────────────────────────────────────┘


CONTRASTE CON format-on-write

  format-on-write → PROJECT
    Convención del equipo OrderManagement
    Cada equipo formatea distinto
    Va a git con el repo

  block-dangerous → USER
    Política de seguridad personal
    Aplica universalmente
    Tu cinturón de seguridad personal
```

> "**Decisión razonada**:
>
> `format-on-write` es **convención del equipo OrderManagement**. Cada equipo formatea distinto, así que va a project. Compañero clona, tiene el formato.
>
> `block-dangerous` es **política de seguridad personal**. Aplica universalmente. **Si activáis ese hook en user level, os protege en cualquier repo en el que abráis Claude Code**. Repo del cliente actual, repo del cliente del año que viene, side project del fin de semana. **Mismo cinturón de seguridad**.
>
> Y la frase de la gamma 3.3b slide 14 — *'cuando alguna vez intentéis algo destructivo y veáis el bloqueo, os vais a alegrar de tenerlo'*. **Cinturón de seguridad**. **Lo configuras una vez. Te olvidas. El día que importa estás vivo gracias a él**."

**Tiempo:** ~2 minutos.

---

### Bloque 3 — Construir `block-dangerous` en user level (~5 min)

**En PowerShell:**

```powershell
# Crear la carpeta de hooks personal si no existe
mkdir $HOME\.claude\hooks -ErrorAction SilentlyContinue

# Verificar la ruta
ls $HOME\.claude\
```

```
Mode    LastWriteTime    Length Name
----    -------------    ------ ----
d----   ...                     hooks
-a---   ...                     settings.json
```

> "Mi `~/.claude/` ya existe — lo creé al instalar Claude Code en la 1.2b. **Carpeta `hooks/` recién creada**. Voy a escribir el script."

**En VS Code, abro `~/.claude/hooks/block-dangerous.sh`:**

```bash
#!/bin/bash
# block-dangerous — hook PreToolUse en user level
# Bloquea comandos peligrosos antes de que Bash los ejecute.
# Devuelve exit 2 para bloqueo absoluto (incluso en --dangerously-skip-permissions).

# Lee el JSON del evento
INPUT=$(cat)
COMMAND=$(echo "$INPUT" | jq -r '.tool_input.command // empty')

# Si no hay comando, salir limpio (no aplicable)
if [ -z "$COMMAND" ]; then
  exit 0
fi

# Lista de patrones peligrosos
BLOCKED_PATTERNS=(
  'rm[[:space:]]+-rf[[:space:]]+/'           # rm -rf /
  'rm[[:space:]]+-rf[[:space:]]+~'           # rm -rf ~
  'rm[[:space:]]+-rf[[:space:]]+\$HOME'      # rm -rf $HOME
  'git[[:space:]]+push[[:space:]]+.*--force' # git push --force / -f
  'git[[:space:]]+push[[:space:]]+.*-f[[:space:]]'
  'git[[:space:]]+reset[[:space:]]+--hard[[:space:]]+origin'
  'DROP[[:space:]]+TABLE'                    # SQL destructivo
  'DROP[[:space:]]+DATABASE'
  'TRUNCATE[[:space:]]+TABLE'
  ':\(\)\{[[:space:]]*:\|:&[[:space:]]*\};:' # fork bomb
  'mkfs\.'                                   # formateo de discos
  'dd[[:space:]]+if=.*of=/dev/'              # dd a dispositivos
)

for pattern in "${BLOCKED_PATTERNS[@]}"; do
  if echo "$COMMAND" | grep -qE "$pattern"; then
    echo "BLOCKED: comando contiene patrón peligroso." >&2
    echo "Patrón detectado: $pattern" >&2
    echo "Comando: $COMMAND" >&2
    echo "" >&2
    echo "Si necesitas ejecutar este comando, hazlo manualmente fuera de Claude Code." >&2
    exit 2
  fi
done

# Comando aprobado
exit 0
```

**Salvo.**

> "Mirad las decisiones del script:
>
> Una. **Lee `stdin` con `jq`** y extrae `tool_input.command`. Igual que el `format-on-write` en 3.3a.
>
> Dos. **Lista de patrones bloqueados** — extendida respecto al ejemplo del manual. La gamma 3.3b slide 12 mostró los 8 más comunes. Yo añado `mkfs.` (formateo) y `dd if=...of=/dev/` (sobreescritura de dispositivos) por buena medida.
>
> Tres. **`grep -qE`** con regex extendida. **Patrones con `[[:space:]]+`** en lugar de espacios literales para capturar tabs y múltiples espacios.
>
> Cuatro. **`exit 2`** en caso de bloqueo. La gamma 3.3b slide 13: *'exit 2 bloquea la herramienta incluso en modo `--dangerously-skip-permissions`'*. **Garantía real**.
>
> Cinco. **Mensaje claro al stderr** — Claude Code lo recibe y se lo pasa al modelo. *'BLOCKED... patrón detectado... si necesitas ejecutarlo manualmente'*. **El agente sabe por qué se bloqueó**.
>
> Ahora **enganchamos el hook**. Voy al `~/.claude/settings.json`."

**En VS Code, abro `~/.claude/settings.json` y añado la sección `hooks`. Si ya existe alguna otra sección, la respetamos:**

```json
{
  "permissions": {
    "...si existe...": "..."
  },
  "hooks": {
    "PreToolUse": [
      {
        "matcher": "Bash",
        "hooks": [
          {
            "type": "command",
            "command": "bash $HOME/.claude/hooks/block-dangerous.sh",
            "timeout": 5
          }
        ]
      }
    ]
  }
}
```

> "**Tres decisiones del JSON**:
>
> **Evento**: `PreToolUse` — antes de la ejecución. **Crítico**: si fuera `PostToolUse`, ya se habría ejecutado el comando peligroso.
>
> **Matcher**: `Bash`. Solo nos interesa interceptar comandos shell.
>
> **Timeout**: 5 segundos. Es un hook **rápido** — solo lee stdin, hace grep, devuelve. Más de 5s sería raro. La gamma 3.3b anti-patrón #1 nos recordó: *'mantened hooks bajo 200-500ms'*. Aquí el bloqueo debe ser instantáneo.
>
> **Y `$HOME` en lugar de `$CLAUDE_PROJECT_DIR`** porque este hook no es del proyecto — es de mi carpeta personal. La diferencia importante.
>
> Salvo. Los hooks de user level **se cargan al arrancar**. Reinicio Claude Code en seguida."

**Tiempo:** ~5 minutos.

---

### Bloque 4 — Probar `block-dangerous` en directo (~3 min)

**En la terminal:**

```powershell
claude
```

```
✓ 6 project skills loaded
✓ 3 project agents loaded
✓ 1 user hook loaded: PreToolUse (Bash)
✓ 1 project hook loaded: PostToolUse (Write|Edit|MultiEdit)
```

> "**Mirad la línea**: `1 user hook loaded`. Cargado desde `~/.claude/settings.json`. Y el `1 project hook` del 3.3a también sigue. **Conviven**.
>
> Voy a forzar el bloqueo. Le pido a Claude que ejecute `git push --force`."

**Tecleo:**

```
> Ejecuta el comando: git push --force origin demo/3.3b
```

**Aparece (output ejemplo):**

```
● [Hook PreToolUse triggered]
  Running: bash $HOME/.claude/hooks/block-dangerous.sh
  
  STDERR:
  BLOCKED: comando contiene patrón peligroso.
  Patrón detectado: git[[:space:]]+push[[:space:]]+.*--force
  Comando: git push --force origin demo/3.3b
  
  Si necesitas ejecutar este comando, hazlo manualmente fuera de Claude Code.

  Hook returned exit code 2 — tool execution BLOCKED.

No puedo ejecutar ese comando — un hook de seguridad lo ha bloqueado
porque contiene un patrón peligroso (`git push --force`). El force
push puede sobreescribir el historial remoto y causar pérdida de
trabajo a otros del equipo.

Si realmente necesitas hacer un force push, hazlo manualmente desde tu
terminal fuera de Claude Code. Si la intención era otra (por ejemplo,
pushar después de un rebase legítimo), sugiero `git push --force-with-lease`
que es más seguro porque verifica que el remoto no haya cambiado desde
tu último fetch.
```

> "**Mirad la secuencia**:
>
> Una. **Hook PreToolUse triggered** — antes de ejecutar.
>
> Dos. **STDERR del script** mostrado al modelo. *'BLOCKED, patrón detectado, comando, sugerencia'*. Toda la información.
>
> Tres. **Hook returned exit code 2 — tool execution BLOCKED**. **Bloqueado**.
>
> Cuatro. **El agente entiende lo que pasó** y le dice al usuario. *'Un hook de seguridad lo ha bloqueado'*. Y **propone una alternativa más segura**: `git push --force-with-lease`.
>
> **Esto es la diferencia**. Sin el hook, el comando se habría ejecutado. Con el hook, **se ha bloqueado de forma absoluta**. **Y el modelo recibe contexto suficiente para sugerir alternativas razonables**.
>
> Y atención al matiz importante: **esto bloquea incluso en modo `--dangerously-skip-permissions`**. La gamma 3.3b slide 13. **Garantía real**, no recomendación. Si vuestro equipo tiene un dev nuevo y le obligan a usar el modo permisivo por velocidad, el `block-dangerous` sigue funcionando.
>
> Salgo."

**Salgo (Ctrl+C):**

**Tiempo:** ~3 minutos.

---

### Bloque 5 — Ampliación inteligente: handler `prompt` (~2 min 30 seg)

> "**El bloqueo basado en regex captura los obvios**. Pero hay comandos peligrosos **que no son obvios**. La gamma 3.3b slide 15."

**En el editor:**

```
EL LÍMITE DEL REGEX

Comandos PELIGROSOS pero SUTILES:

  dd if=/dev/zero of=/dev/sda bs=1M     ← destructivo total
  > /dev/sda                              ← redirección a disco
  find / -name "*.cs" -delete            ← borrado masivo
  chmod -R 000 /var                       ← permisos masivos
  
Capturarlos con regex es VIABLE pero requiere
patrones cada vez más elaborados.

A partir de cierto punto:
  REGEX FRÁGIL + falsos positivos abundantes


LA AMPLIACIÓN: handler "prompt"

Combinar regex con LLM:

{
  "hooks": {
    "PreToolUse": [
      {
        "matcher": "Bash",
        "hooks": [
          {
            "type": "prompt",
            "prompt": "Analiza este comando y determina si es destructivo
                      o peligroso: '{tool_input.command}'.
                      Responde solo 'safe' o 'dangerous'.",
            "model": "haiku"
          }
        ]
      }
    ]
  }
}


CÓMO FUNCIONA

  1. Cada comando Bash pasa por Haiku
  2. Haiku evalúa con CRITERIO (no regex)
  3. Si responde "dangerous" → bloqueo automático
  4. Si responde "safe" → permite continuar


COSTE Y BENEFICIO

  ✓ Captura cosas que un regex no captaría
  ✗ Más caro: cada Bash ahora pasa por LLM
    (token cost real, no trivial en sesiones largas)
  ✗ Latencia: añade ~500ms a cada Bash


┌──────────────────────────────────────────────────────────┐
│  COMBINAR AMBOS ENFOQUES                                 │
│                                                          │
│  Regex para los obvios (rápido, gratis):                │
│    rm -rf /, git push --force, fork bombs               │
│                                                          │
│  LLM para los sutiles (más caro pero más cobertura):    │
│    dd peligroso, redirecciones a disco, find -delete    │
│                                                          │
│  Es lo más prudente.                                    │
└──────────────────────────────────────────────────────────┘
```

> "**No vamos a montarlo en directo** porque añadir Haiku a cada `Bash` cuesta tokens reales en sesiones largas. **Pero conviene saber que existe**.
>
> La gamma 3.3b slide 16 lo dijo: *'combinar ambos enfoques es lo más prudente'*. Regex para los obvios — rápido, gratis. LLM para los sutiles — más caro, más cobertura. **Capas defensivas**.
>
> Si vuestro equipo trabaja con sistemas críticos (DB de producción, infra que importa), **vale la pena la inversión** de añadir el handler `prompt`. Para uso normal, el regex suele bastar."

**Tiempo:** ~2 minutos 30 segundos.

---

### Bloque 6 — Channels: referencia rápida (~1 min 30 seg)

> "Antes de la observabilidad, **channels**. La gamma 3.3b slides 17-18. Pieza real pero **uso práctico todavía minoritario**."

**En el editor:**

```
CHANNELS — referencia rápida

¿QUÉ ES?

  Un channel es BÁSICAMENTE un MCP server que en vez
  de exponer herramientas hace PUSH hacia la sesión
  de Claude Code.

  La INVERSIÓN:
    MCP normal:  Claude pregunta cuando necesita algo
    Channel:     El sistema externo notifica a Claude
                cuando pasa algo


CASOS TÍPICOS

  1. Alerta de CI fallido
     Sistema CI manda mensaje al canal:
       "build de rama X ha fallado, aquí está el log"
     Claude lo recibe en mitad de tu sesión y puede
     analizarlo.

  2. Eventos de monitorización
     Sistema externo detecta anomalía → notifica.

  3. Mensajes de chat
     Slack reenvía menciones del equipo al canal.


CONFIGURACIÓN

  Como un MCP server normal pero con la capacidad
  `claude/channel`. Anthropic los está madurando.


┌──────────────────────────────────────────────────────────┐
│  RECOMENDACIÓN HONESTA PARA EL CURSO                     │
│                                                          │
│  Saber que existen.                                      │
│                                                          │
│  No es algo que vayáis a configurar la primera semana.   │
│                                                          │
│  Si en unos meses identificáis un caso donde un sistema  │
│  externo necesita notificar a Claude Code de forma       │
│  proactiva, buscad documentación específica entonces.    │
└──────────────────────────────────────────────────────────┘
```

> "**Brevemente**: existen, son MCP que hace push, casos típicos son alertas de CI, eventos de monitorización, mensajes de chat. La gamma 3.3b slide 18 lo cubrió. **Saber que están y volver cuando haga falta**."

**Tiempo:** ~1 minuto 30 segundos.

---

### Bloque 7 — Observabilidad: por qué es distinta en agentes (~3 min)

> "**El último gran tema del módulo 3**: observabilidad. La gamma 3.3b slides 22-28. **Pieza que cierra el harness fiable**."

**En el editor:**

```
OBSERVABILIDAD — la pieza que cierra el harness fiable

EL ESCENARIO REAL

  Te llega mensaje del lead:
    "Un subagente devolvió algo raro y el código pushed está mal."
  
  Vas a /usage. La sesión consumió 200K tokens.
  
  Pregunta: ¿en qué eslabón se torció?
    - skill orquestador?
    - subagente A?
    - subagente B?
    - decisión del loop validator?
    - hallazgo del reviewer?
  
  Sin LOGS ESTRUCTURADOS → "vete a saber"


POR QUÉ EN AGENTES ES DISTINTO

  1. NO DETERMINISMO
     Lanzas el mismo workflow 2 veces.
     Las decisiones del orquestador NO son idénticas.
     No puedes "reproducir" un fallo
     como reproduces un null pointer.

  2. DECISIONES OPACAS
     El razonamiento del agente vive dentro del modelo.
     Cuando algo va mal, no tienes branch del código.
     Tienes que reconstruir qué pensó el agente.
     Y eso solo lo sabes si lo logueaste.

  3. CADENAS LARGAS
     Skill → subagente A → subagente B → MCP server.
     Cada eslabón = punto de fallo.
     Sin saber qué pasó en cada uno, debugging = adivinación.


┌──────────────────────────────────────────────────────────┐
│  REGLA PRÁCTICA                                          │
│                                                          │
│  "Los agentes que llegan a producción tienen siempre     │
│   algún sistema de observabilidad.                       │
│   Los que no, no llegan."                                │
│                                                          │
│  Gamma 3.3b slide 23                                     │
└──────────────────────────────────────────────────────────┘


QUÉ LOGGEAR

  ✓ DECISIONES DEL ORQUESTADOR
    Cuándo invocó a qué subagente, con qué parámetros,
    cuál fue la respuesta.

  ✓ INVOCACIONES A SUBAGENTES
    Modelo usado, tokens consumidos, latencia,
    éxito o fallo.

  ✓ ITERACIONES DE LOOPS
    Cuántas vueltas dio el validator → implementer.
    Si necesita las 3 cada vez → problema en el subagente.

  ✓ ERRORES Y BLOQUEOS DE HOOKS
    Qué se bloqueó y por qué.
    Auditar policies de seguridad.

  ✓ COSTE POR WORKFLOW
    Tokens agregados por sesión, agrupados por subagente.


CÓMO SE HACE CON HOOKS

  Tres eventos te dan el grueso:

  • SessionEnd      → resumen de sesión: tokens, duración,
                       subagentes invocados, tools usadas
  
  • PostToolUse     → tracing por tool call individual

  • SubagentStop    → registro específico cuando un
                       subagente termina
```

> "**El escenario real importa**. La gamma 3.3b slide 22 lo describió: te llega un fallo, sesión consumió 200K tokens, y tú **no tienes ni idea de en qué eslabón se torció**. Sin logs, **debugging es adivinación**.
>
> **Tres razones** por las que en agentes es distinto al código tradicional. **No determinismo**, **decisiones opacas**, **cadenas largas**. Estas tres no las tiene tu código de C# normal. **Necesitan otro enfoque**.
>
> **Cinco cosas que merece la pena loggear**. Decisiones del orquestador, invocaciones a subagentes, iteraciones de loops, errores de hooks, coste por workflow.
>
> Y **tres eventos cubren el grueso**: `SessionEnd` para resumen, `PostToolUse` para tracing, `SubagentStop` para subagentes. **Vamos a montar el más simple — el SessionEnd**."

**Tiempo:** ~3 minutos.

---

### Bloque 8 — Construir hook `log-session` (~3 min)

> "Vuelvo a project level porque **la observabilidad es del proyecto** — el equipo entero quiere los logs."

**En PowerShell:**

```powershell
# El .claude/hooks/ ya existe del 3.3a — solo añadimos un script más
# Verifico
ls .claude\hooks\
```

```
format-on-write.sh
```

**En VS Code, creo `.claude/hooks/log-session.sh`:**

```bash
#!/bin/bash
# log-session — hook SessionEnd
# Vuelca un resumen estructurado de la sesión a sessions.jsonl

INPUT=$(cat)

TIMESTAMP=$(date -u +"%Y-%m-%dT%H:%M:%SZ")
SESSION_ID=$(echo "$INPUT" | jq -r '.session_id // "unknown"')
TOTAL_TOKENS=$(echo "$INPUT" | jq -r '.usage.total_tokens // 0')
INPUT_TOKENS=$(echo "$INPUT" | jq -r '.usage.input_tokens // 0')
OUTPUT_TOKENS=$(echo "$INPUT" | jq -r '.usage.output_tokens // 0')
TOOL_CALLS=$(echo "$INPUT" | jq -r '.tool_calls_count // 0')
DURATION=$(echo "$INPUT" | jq -r '.duration_seconds // 0')

LOG_DIR="$CLAUDE_PROJECT_DIR/.claude/logs"
mkdir -p "$LOG_DIR"

LOG_FILE="$LOG_DIR/sessions.jsonl"

# Línea JSON estructurada — un fichero JSONL por convención
cat <<EOF >> "$LOG_FILE"
{"timestamp":"$TIMESTAMP","session_id":"$SESSION_ID","total_tokens":$TOTAL_TOKENS,"input_tokens":$INPUT_TOKENS,"output_tokens":$OUTPUT_TOKENS,"tool_calls":$TOOL_CALLS,"duration_seconds":$DURATION}
EOF

exit 0
```

**Salvo. En VS Code, modifico `.claude/settings.json` para añadir el `SessionEnd`:**

```json
{
  "permissions": {
    "...": "..."
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
    ],
    "SessionEnd": [
      {
        "hooks": [
          {
            "type": "command",
            "command": "bash $CLAUDE_PROJECT_DIR/.claude/hooks/log-session.sh",
            "timeout": 5
          }
        ]
      }
    ]
  }
}
```

> "**Decisiones**:
>
> Una. **Evento `SessionEnd`** — al cerrar la sesión. **No `Stop`** que se dispara muchas veces — `SessionEnd` solo al final.
>
> Dos. **Sin matcher** porque `SessionEnd` no aplica a una herramienta específica.
>
> Tres. **Timeout 5s** — debe ser rápido, solo escribe una línea.
>
> Cuatro. **Append a `sessions.jsonl`** en formato JSON Lines — un objeto por línea. **Estándar para logs procesables**.
>
> Cinco. **`mkdir -p` defensivo** — si la carpeta no existe, se crea. Y la carpeta `.claude/logs/` está **gitignored** (lo añadimos en preparación).
>
> Vamos a probarlo."

**En la terminal:**

```powershell
claude
```

**Hago una sesión rápida — un par de comandos:**

```
> ¿Qué versión de .NET usa este proyecto?
```

**Aparece la respuesta.**

```
> /exit
```

**Verifico que se generó el log:**

```powershell
cat .claude\logs\sessions.jsonl
```

```json
{"timestamp":"2026-05-04T15:42:18Z","session_id":"sess-abc123","total_tokens":1245,"input_tokens":890,"output_tokens":355,"tool_calls":2,"duration_seconds":18}
```

> "**Una línea por sesión**. Timestamp ISO 8601, session_id, tokens, tool calls, duración. **Esto va a Datadog, Grafana, Splunk, lo que vuestra empresa use**. La gamma 3.3b slide 27 lo dijo: *'la pieza importante no es el destino — es tener los datos'*.
>
> Y la conexión con context bank de la 3.2b: **el context bank ya es de facto medio log del workflow**. Te dice qué pensó el planner, qué exploró el explorer, qué hallazgos sacó el reviewer. **Lo que añaden los hooks de observabilidad es la capa transversal**: tokens, latencia, eventos de sistema, métricas agregadas. **Las dos capas juntas** — context bank + hooks de logging — **te dan trazabilidad real**.
>
> **Y con esto se cierra el harness**."

**Tiempo:** ~3 minutos.

---

### Bloque 9 — La definición operativa del harness completo (~2 min 30 seg)

> "**Cierre del módulo 3 entero**. La gamma 3.3b slide 29. La fórmula que vale la pena memorizar."

**En el editor:**

```
HARNESS = PROMPTS + TOOLS + CONTEXT POLICIES + HOOKS + FEEDBACK LOOPS + OBSERVABILITY


CADA PIEZA DEL HARNESS, DÓNDE LA TENÉIS

PROMPTS — lo que el agente sabe sobre cómo trabaja vuestro equipo
─────────────────────────────────────────────────────────────────
  CLAUDE.md (módulo 1)
  6 skills (módulo 2 + 3.2):
    angular-component (con context: fork)
    commit-style
    db-reset
    frontend-design
    pre-commit-check (con loop + context bank)
    pre-pr-check (fan-out paralelo)


TOOLS — lo que el agente PUEDE ejecutar
────────────────────────────────────────
  Read, Write, Edit, Bash, Grep, Glob...
  MCP servers (módulos 1 y 4 — el 4 viene ahora)


CONTEXT POLICIES — lo que el agente PUEDE TOCAR Y CUÁNDO
─────────────────────────────────────────────────────────
  3 subagentes (módulo 3.1 + 3.2b):
    repo-explorer (Haiku, read-only)
    dotnet-reviewer (Sonnet, read + git diff)
    convention-checker (Haiku, read + git diff)
  Tools restringidos por rol
  Scopes: user / project / local


HOOKS — lo que pasa SIEMPRE
────────────────────────────
  format-on-write (project, PostToolUse)
  block-dangerous (USER, PreToolUse) ← viaja contigo
  log-session (project, SessionEnd)


FEEDBACK LOOPS — lo que hace que el harness se AUTOCORRIJA
───────────────────────────────────────────────────────────
  Loop validator → implementer en pre-commit-check
  Fan-out / fan-in en pre-pr-check
  Context bank en .claude/workflow-state/<session>/


OBSERVABILITY — lo que hace que el harness sea DEPURABLE
─────────────────────────────────────────────────────────
  Hook SessionEnd → sessions.jsonl
  Context bank ya provee trazabilidad de workflows
```

> "**Esto es vuestro harness al final del módulo 3**. Cada pieza la habéis construido vosotros sobre el repo OrderManagement.
>
> **Prompts**: `CLAUDE.md` + 6 skills.
>
> **Tools**: las del agente + MCP que viene en el módulo 4.
>
> **Context policies**: 3 subagentes con roles, tools, modelos asociados.
>
> **Hooks**: 3 funcionando, 2 en project + 1 en user que viaja con vosotros.
>
> **Feedback loops**: loops en orquestadores + context bank.
>
> **Observability**: hooks de logging + context bank como log de workflow.
>
> **Skills personalizan lo que el agente sabe. Subagentes definen roles especializados. La orquestación los combina con loops y context bank. Los hooks aseguran que las cosas mecánicas pasen siempre. La observabilidad cierra el círculo. Eso, junto, es vuestro harness sobre Claude Code**."

**Tiempo:** ~2 minutos 30 segundos.

---

### Bloque 10 — Commit y `harness-completo.md` (~1 min 30 seg)

> "Commit final del módulo 3."

**En la terminal:**

```powershell
# El block-dangerous está en ~/.claude/, NO se commitea al repo.
# Solo commiteamos los del project + el log generado por la sesión de prueba.

# Verificamos que .claude/logs/ está gitignored
cat .gitignore | grep logs
```

```
.claude/logs/
```

```powershell
git add .claude/settings.json .claude/hooks/log-session.sh
git status
```

```
Changes to be committed:
  modified:   .claude/settings.json
  new file:   .claude/hooks/log-session.sh
```

```powershell
git commit -m "demo/3.3b-after: hook log-session para observabilidad"
```

> "**Commit hecho**. **Y el `block-dangerous` queda en mi `~/.claude/`, fuera del repo**. Cualquier compañero del equipo que clone OrderManagement tendrá `format-on-write` y `log-session` automáticamente. **Pero `block-dangerous` cada uno lo configura en su user level**, según sus preferencias personales.
>
> Y abro `docs/harness-completo.md` para verlo:"

**Abro `docs/harness-completo.md` en VS Code y muestro el contenido entero al alumno:**

> "**Documento de cierre del módulo 3**. La fórmula del harness, cada pieza dónde la tenéis, las dos preguntas para casa antes del módulo 4, y la lectura complementaria opcional para roles que decidan arquitecturas a nivel sistema. **Llevadlo al puesto del lunes**."

**Tiempo:** ~1 minuto 30 segundos.

---

### Bloque 11 — Cierre del módulo 3 entero y bridge al módulo 4 (~3 min)

> "**Recap del módulo 3 entero**. Seis demos. **Lo que vosotros tenéis ahora**."

**En el editor:**

```
MÓDULO 3 — RECAP COMPLETO

3.1a — Modelo conceptual y subagentes integrados
       Frame: agent = model + harness
       Tres built-in: Explore, Plan, general-purpose
       Aislamiento de contexto materializado

3.1b — Subagentes custom y patrones
       repo-explorer + dotnet-reviewer construidos
       Asociar modelo a tipo de tarea
       3-4 subagentes para uso general

3.2a — Aislamiento, composición y loops
       context: fork en skills
       Patrón compuesto skill que invoca subagente
       Loop validator → implementer con techo

3.2b — Memoria, paralelo, MCP y Agent Teams
       Context bank en .claude/workflow-state/
       Fan-out / fan-in con pre-pr-check
       Agent Teams: 10-15x más tokens

3.3a — Hooks: anatomía y eventos
       Frame instrucción vs garantía
       Primer hook funcional: format-on-write

3.3b — Bloqueo, observabilidad y cierre  ← Esta
       block-dangerous en user level
       Observabilidad con SessionEnd
       Definición operativa del harness


VOSOTROS TENÉIS AHORA

  ✅ 6 skills (módulos 2 + 3.2)
  ✅ 3 subagentes (módulo 3.1 + 3.2b)
  ✅ 3 hooks (módulo 3.3)
  ✅ Context bank operativo
  ✅ Observabilidad básica
  ✅ Modelo mental completo del agent harness

  EL HARNESS COMPLETO. FUNCIONANDO. SOBRE ORDERMANAGEMENT.
```

> "**Esto es lo que tenéis al cerrar el módulo 3**. Y la idea final que vale la pena llevarse del módulo entero:"

**En el editor:**

```
LA IDEA FINAL

Cuando personalizáis Claude Code con todas estas piezas,
NO estáis configurando una herramienta.

Estáis CONSTRUYENDO VUESTRO PROPIO HARNESS encima del de Anthropic.

Vuestro harness:
  • SABE A VUESTRO EQUIPO
  • SABE VUESTRAS CONVENCIONES
  • SABE DELEGAR
  • SABE CORREGIR
  • SABE GARANTIZAR
  • SABE LOGGEAR


LAS DOS PREGUNTAS ANTES DEL MÓDULO 4

  1. ¿Qué hook concreto vais a configurar el lunes
     en vuestro repo del trabajo?

     Si la respuesta es "el de auto-format" → perfecto.
     Si es "el de bloquear peligrosos" → también.

     Cualquiera de los dos cuenta.

  2. ¿Hay alguien en vuestro equipo de diseño con
     quien colaboréis y que ya use Figma?

     Si sí, el módulo 4 va a tener nombre y apellidos.


LECTURA COMPLEMENTARIA OPCIONAL

  Para roles que decidan arquitecturas a nivel sistema:

  "Building Effective AI Agents: Architecture Patterns and
  Implementation Frameworks" — Anthropic

  Cubre los patrones que vimos aquí en versión más teórica:
    hierarchical, collaborative, sequential, parallel,
    evaluator-optimizer

  El vocabulario formal que aquí pincelamos de pasada.
  Para cuando alguien arriba pida un "diseño de sistema
  multi-agente" y queráis ir con los términos que esa
  persona espera oír.
```

**Cliffhanger:**

> "**Módulo 4**. Cambiamos de tema completamente.
>
> Hasta aquí hemos hablado del agente y su personalización — el harness entero. Ahora entramos en cómo Claude Code se integra con **el flujo de diseño**.
>
> El módulo 4 cubre **Figma MCP y Claude Design**:
>
> - Cómo trabajar con diseños existentes a través del MCP de Figma.
> - Cómo usar Claude Design para creación visual conversacional.
> - Y cómo el formato emergente **`DESIGN.md`** está reconfigurando la forma en que los agentes consumen design systems.
>
> Para devs .NET / Angular que colaboran con equipos de diseño, **es donde más rápida rentabilidad vais a ver de Claude Code en el día a día**.
>
> Empezamos con el **cuatro punto uno punto A**."

**Tiempo:** ~3 minutos.

---

## 9. Qué resaltar verbalmente

Cinco puntos que **no pueden quedarse sin decir**:

1. **"`block-dangerous` va en user level. Viaja con vosotros a todos los repos. Bloquea incluso en `--dangerously-skip-permissions`. Cinturón de seguridad."** — la decisión clave del primer showcase. Bloque 2 y 4.

2. **"Regex para los obvios. LLM para los sutiles. Combinar es lo más prudente."** — el patrón con handler `prompt`. Bloque 5.

3. **"En agentes, debugging sin logs es adivinación. No determinismo, decisiones opacas, cadenas largas."** — el frame de observabilidad. Bloque 7.

4. **"`harness = prompts + tools + context policies + hooks + feedback loops + observability`. Cada pieza la tenéis ya."** — la definición operativa del módulo entero. Bloque 9.

5. **"Estáis construyendo vuestro propio harness encima del de Anthropic. Sabe a vuestro equipo."** — la idea final del módulo 3. Bloque 11.

**Frase de remate al final:**

> *"Seis demos. Tres hooks. Tres subagentes. Seis skills. El harness completo. El módulo 3 cerrado. Nos vemos en el módulo 4 con Figma y Claude Design."*

---

## 10. Slide de entrada (locución HeyGen)

> **Texto del avatar antes del screencast:**

"Y vamos a la última demo del módulo 3. La 3.3b. Cierra el módulo 3 entero. Cinco temas en directo. Uno, construimos el segundo hook crítico: bloqueo de comandos peligrosos. Pero atención, en user level no project — viaja con vosotros a todos los repos como vuestro cinturón de seguridad personal. Y veréis el bloqueo en directo intentando ejecutar `git push --force` con exit 2 — bloqueo absoluto que funciona incluso en modo `--dangerously-skip-permissions`. Dos, la ampliación inteligente con handler `prompt`: combinar regex con LLM para capturar comandos sutiles. Tres, channels como referencia rápida. Cuatro, observabilidad — la pieza que cierra el harness fiable. Por qué en agentes el debugging es distinto: no determinismo, decisiones opacas, cadenas largas. Construimos un hook `SessionEnd` con logging estructurado a `sessions.jsonl`. Y cinco, cierre del módulo 3 entero con la definición operativa del harness completo: `harness = prompts + tools + context policies + hooks + feedback loops + observability`. Cada pieza ya la tenéis. Vamos al screencast."

---

## 11. Slide de salida (locución HeyGen)

> **Texto del avatar al volver del screencast:**

"Lo que acabáis de ver cierra el módulo 3 entero. Cinco ideas para llevarse al lunes. Una, el hook `block-dangerous` va en user level — viaja con vosotros, bloquea incluso en modo `--dangerously-skip-permissions`. Configuradlo el lunes. Dos, regex para obvios y LLM para sutiles — combinar ambos enfoques es lo más prudente para sistemas críticos. Tres, en agentes la observabilidad NO es opcional para flujos serios. No determinismo, decisiones opacas, cadenas largas — sin logs estructurados, debugging es adivinación. Cuatro, la definición operativa del harness: prompts, tools, context policies, hooks, feedback loops, observability. Cada pieza ya la tenéis. Y cinco, no estáis configurando una herramienta — estáis construyendo vuestro propio harness encima del de Anthropic. Vuestro harness sabe a vuestro equipo. Sabe vuestras convenciones. Sabe delegar. Sabe corregir. Sabe garantizar. Sabe loggear. Las dos preguntas para casa: qué hook configuráis el lunes en vuestro repo del trabajo, y si tenéis equipo de diseño con quien colaborar en Figma. Si la respuesta a la segunda es sí, el módulo 4 va a tener nombre y apellidos para vosotros. Empezamos con el cuatro punto uno punto A."

---

## 12. Tiempo total estimado

| Bloque | Tiempo |
|---|---|
| Slide de entrada (avatar) | ~30 seg |
| Bloque 1 — Setup y la pregunta del 3.3a | ~1 min 30 seg |
| Bloque 2 — Por qué `block-dangerous` va en user level | ~2 min |
| Bloque 3 — Construir `block-dangerous` en user level | ~5 min |
| Bloque 4 — Probar `block-dangerous` en directo | ~3 min |
| Bloque 5 — Ampliación inteligente: handler `prompt` | ~2 min 30 seg |
| Bloque 6 — Channels: referencia rápida | ~1 min 30 seg |
| Bloque 7 — Observabilidad: por qué es distinta | ~3 min |
| Bloque 8 — Construir hook `log-session` | ~3 min |
| Bloque 9 — La definición operativa del harness | ~2 min 30 seg |
| Bloque 10 — Commit y `harness-completo.md` | ~1 min 30 seg |
| Bloque 11 — Cierre del módulo 3 y bridge al 4 | ~3 min |
| Slide de salida (avatar) | ~30 seg |
| **Total screencast** | **~28-30 min** |
| **Total con avatar** | **~29-31 min** |

> Si hay preguntas durante el screencast, súmale 4-5 minutos. La demo encaja en un bloque de **35 minutos** dentro de la sesión.

**Margen de seguridad por si algo va lento:**

- **Si `block-dangerous` NO bloquea `git push --force`** (problema en el regex `[[:space:]]+`, problema con la versión de grep en Git Bash, etc.), debug en directo:
  - Probar el script aislado: `echo '{"tool_input":{"command":"git push --force origin demo/3.3b-after"}}' | bash ~/.claude/hooks/block-dangerous.sh; echo "exit: $?"` — debe imprimir `BLOCKED...` y `exit: 2`.
  - Si el script funciona aislado pero el hook no, el problema está en el matcher o en cómo Claude Code lanza el comando. Comenta: *"esto pasa a veces — la pedagogía sigue: el hook ESTÁ ahí, el bloqueo es real cuando funciona, y `exit 2` es el mecanismo correcto"*. La pedagogía sobrevive.

- **Si Claude Code NO ejecuta el `git push --force` en el bloque 4** (porque su seguridad interna ya lo rechaza), comenta: *"a veces Claude tiene seguridad interna que rechaza ciertos comandos antes incluso de llegar al hook. Vamos a ver el hook con un comando menos obvio"*. Y prueba con `rm -rf ~/test-folder` que el hook debe bloquear porque captura `rm -rf ~`.

- **Si `jq` no está instalado en la máquina del formador** (y lo descubrís a mitad de demo), reemplaza el `jq -r` por `grep -oP '"command"\s*:\s*"\K[^"]+'` o equivalente. Comenta: *"jq es lo más limpio pero hay alternativas. Para vosotros el lunes, recomiendo instalarlo con `winget install jqlang.jq`"*.

- **Si el log `sessions.jsonl` queda vacío** después de la sesión de prueba (problema con los campos JSON que Claude Code envía), no peles la pedagogía. Comenta: *"a veces los campos exactos del JSON varían entre versiones — lo importante es que el hook se disparó y escribió algo. Para producción, ajustaríamos los `jq` exactos según la versión"*. Y muestra que **al menos el fichero se creó**, aunque las líneas tengan ceros.

- **Si te quedas sin tiempo y los bloques 5 y 6 te aprietan**, recorta el bloque 5 (handler `prompt`) a 1 min: solo enuncias que existe, das el ejemplo del JSON, dices *"combinar regex + LLM"*. Bloque 6 (channels) puedes recortarlo a 30 segundos: *"existen, son MCP push, casos típicos: alertas CI, eventos de monitorización"*.

- **Si surge la pregunta "¿y si me bloqueo a mí mismo en mi propio repo?"**, responde corto: *"justa pregunta. Para los casos en que sabéis lo que hacéis, podéis editar `~/.claude/settings.json` y comentar el hook temporalmente, o ejecutar el comando manualmente fuera de Claude Code. **El hook NO impide que ejecutéis el comando por vosotros mismos** — solo bloquea que el agente lo ejecute por vosotros. Importante distinción"*.

---

# Apéndice A — Decisiones pedagógicas justificadas

> Esta sección no se graba. Es para Pedro, formador.

**¿Por qué `block-dangerous` en user level y no project level?**

Por dos razones:
1. **Es política de seguridad personal, no convención del equipo**. Cada dev decide si quiere ese bloqueo (la mayoría sí), pero **no es algo que el equipo deba imponer al individuo en su entorno personal**. La gamma 3.3b slide 14 lo dijo explícitamente.
2. **Viaja con vosotros**. Si lo metiera en project, solo aplicaría en ese repo. **Mañana abrís otro repo del cliente importante y no estáis protegidos**. User level resuelve el problema **una vez, para siempre**.

Y crear un contraste con `format-on-write` (que SÍ va en project) **enseña la decisión al alumno**: project = convención del equipo, user = preferencia personal.

**¿Por qué probar el bloqueo con `git push --force` y no con `rm -rf /`?**

Por tres razones:
1. **`rm -rf /` es muy destructivo** — si el hook fallase, las consecuencias en una máquina real serían catastróficas. **`git push --force` es destructivo pero limitado** al historial de un repo específico.
2. **`git push --force` es más realista** — `rm -rf /` casi nadie lo intenta sin querer. `git push --force` la gente lo usa por costumbre y por error.
3. **Permite la sugerencia educativa** — el agente puede sugerir `--force-with-lease` como alternativa segura. **Pedagogía bonus**: el alumno aprende un patrón mejor.

**¿Por qué la lista de patrones bloqueados extiende la del manual con `mkfs.` y `dd if=...of=/dev/`?**

Porque **la gamma menciona los más comunes como ejemplos** pero la lista real debería ajustarse al contexto. Para devs .NET / Angular en Windows, los patrones de Linux destructivo (`mkfs`, `dd`) son menos comunes, pero **incluirlos no cuesta nada y protege en caso de WSL futuro o herramientas nuevas**. Decisión defensiva razonable.

**¿Por qué NO se construye el handler `prompt` en directo?**

Por dos razones:
1. **Coste de tokens real**: añadir Haiku a cada `Bash` tiene factura medible en sesiones largas. Activarlo en una rama del curso lo dejaría activo durante demos siguientes.
2. **Pedagogía limitada**: el alumno **ya entendió la idea** con la explicación visual. Construirlo en vivo añade 5-7 minutos sin aportar más insight.

**Mejor**: explicación clara + JSON visible + cuándo merece la pena. **El alumno puede activarlo el lunes si su caso lo justifica**.

**¿Por qué el bloque 7 (observabilidad) es solo conceptual antes de construir en bloque 8?**

Porque **observabilidad sin el frame es jerga vacía**. Si saltara directo a *"creamos un hook SessionEnd"*, el alumno no entendería **por qué importa**. La gamma 3.3b dedica 7 slides al *por qué* antes de mostrar el cómo. Respeto esa estructura: **frame primero, construcción después**.

**¿Por qué `log-session` en project level (cuando `block-dangerous` está en user)?**

Porque **observabilidad es del equipo, no del individuo**. Si el equipo quiere medir uso de Claude Code, costes, patrones de invocación de subagentes, **necesitan datos de todos los devs juntos**. Project level garantiza que cualquier compañero tiene el logging activado al clonar.

**Y crítico**: los logs en sí (`sessions.jsonl`) están **gitignored** — no van a git. Solo el script y la configuración. Los datos quedan locales hasta que el dev los exporte a su sistema (Datadog, etc.).

**¿Por qué el cierre del módulo 3 incluye la idea final "vuestro harness sabe a vuestro equipo"?**

Porque **es el frame mental que tiene que llevarse el alumno**. Sin esa frase, el alumno se va con una colección de features. Con esa frase, el alumno se va con **una visión de sistema**. La diferencia entre *"aprendí Claude Code"* y *"aprendí a construir mi propio harness"*. **Magnitud del logro materializada**.

**¿Por qué las dos preguntas del cierre apuntan a hooks Y a Figma?**

Porque **tienden un puente entre el módulo 3 y el módulo 4**. La pregunta 1 es del módulo que cierra (acción concreta del lunes). La pregunta 2 es del módulo que abre (preparación mental para Figma). **Continuidad pedagógica**. La pausa entre módulos se convierte en **tiempo de incubación útil**.

**¿Por qué la lectura complementaria es opcional y solo "para roles que decidan arquitecturas"?**

Porque **el whitepaper de Anthropic es denso y formal**. Para un dev .NET / Angular que va a usar Claude Code el lunes, **no es necesario**. Para alguien que tenga que defender una arquitectura multi-agente delante de un comité, **es valioso porque da el vocabulario formal**. **Honestidad sobre quién lo necesita**: no todos los alumnos.

**¿Por qué `harness-completo.md` es nuevo (no se mete en `hooks-explorados.md`)?**

Porque es un **documento de cierre del módulo entero**, no solo de hooks. Mezclarlo con notas de hooks confundiría el scope. **Un doc por propósito** mantiene la estructura del proyecto limpia. Y el alumno puede llevárselo aparte para consultarlo el lunes sin tener que abrir el otro.

**¿Por qué la fórmula del harness está en `harness-completo.md` Y se muestra en el bloque 9?**

Porque **redundancia deliberada en pedagogía**: el alumno la ve durante la demo (auditiva + visual), y la tiene escrita para volver a ella el lunes. **Doble canal de retención**. La gamma 3.3b la marca como pieza memorable — la repetición está justificada.

**¿Por qué el guion contempla preguntar al alumno sobre auto-bloqueo accidental?**

Porque **es la pregunta más común** que aparece cuando el alumno entiende el alcance del bloqueo. Tener la respuesta preparada y clara (*"podéis editar el settings.json o ejecutar fuera de Claude Code, el hook NO impide que vosotros ejecutéis el comando"*) **evita pánico** y refuerza el modelo mental: **el hook bloquea al agente, no al usuario**.
