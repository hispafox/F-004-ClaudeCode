# Hooks — notas de exploración

Documento equivalente a `subagentes-explorados.md` para los hooks que
vamos construyendo a lo largo del módulo 3.3.

## Hooks construidos en este repo

### `format-on-write` (3.3a)

- **Evento**: `PostToolUse`.
- **Matcher**: `Write|Edit|MultiEdit`.
- **Handler**: `command` → `bash .claude/hooks/format-on-write.sh`.
- **Scope**: project (`.claude/settings.json`, va a git con el equipo).
- **Qué hace**: lee de stdin el JSON de input que Claude Code pasa al
  hook, extrae `tool_input.file_path`, y aplica el formateador
  según extensión:
  - `.cs` → `dotnet format --include <path>`.
  - `.json` / `.md` → `npx prettier --write <path>`.
  - `.ts` / `.html` / `.scss` → placeholder con `TODO 3.3b`.
- **Exit code**: siempre 0. El formato es best-effort, no debe bloquear
  la operación. Para bloqueo (rollback de un comando) usaremos exit 2
  en hooks `PreToolUse` en 3.3b.
- **Portabilidad Windows**: el script usa shebang `#!/bin/bash` y se
  ejecuta a través de Git Bash en Windows nativo. No requiere WSL.

**Caso pedagógico (3.3a)**: en `demo/3.3a-before` se commitea
`RemoveItemHandler.cs` con formato malo (espacios sobrantes,
indentación inconsistente, llaves pegadas). El formador construye
el hook en directo, luego edita el handler — el hook se dispara
**sin pedirlo**, ejecuta `dotnet format`, y el fichero queda
formateado de forma idéntica al estado pre-cocinado en
`demo/3.3a-after`.

## Hooks de referencia que vale la pena tener

### En user level (~/.claude/settings.json) — viajan con vosotros

- **block-dangerous** (PreToolUse / Bash) — bloquea comandos peligrosos
  (`rm -rf /`, `git push --force`, etc.). Pendiente para 3.3b.

### En project level (.claude/settings.json) — van a git con el equipo

- **format-on-write** (PostToolUse / Write|Edit|MultiEdit) — auto-format
  para `.cs`, `.ts`, `.json`, `.md`. Construido en 3.3a.

## Eventos cubiertos al cerrar el módulo 3

- ✅ `PostToolUse` con matcher `Write|Edit|MultiEdit` (3.3a) → format-on-write
- ✅ `PreToolUse` con matcher `Bash` (3.3b) → block-dangerous
- ✅ `SessionEnd` (3.3b) → log-session

## Hooks por scope al cerrar el módulo 3

### User level (~/.claude/settings.json)

- **block-dangerous** — viaja con vosotros a todos los repos. Bloquea
  `rm -rf /`, `git push --force`, `DROP TABLE`, fork bombs, etc. con
  `exit 2` (incluso en modo `--dangerously-skip-permissions`). No
  entra a este repo: vive en la máquina del alumno.

### Project level (`ordermanagement/.claude/settings.json`) — van a git con el repo

- **format-on-write** (3.3a) — auto-formato al modificar ficheros
  (`dotnet format` para `.cs`, `prettier` para `.json`/`.md`).
- **log-session** (3.3b) — observabilidad básica. Anexa una línea
  JSON por sesión a `ordermanagement/.claude/logs/sessions.jsonl`
  (gitignored).

## Lecciones extraídas (módulo 3.3 entero)

1. **Hooks son código, no instrucción**. La diferencia con `CLAUDE.md`
   y skills es absoluta — el agente no decide si pasan o no.
2. **Empezad con dos hooks**: `format-on-write` (project) y
   `block-dangerous` (user). Cubren el 80% del valor.
3. **Exit 2 bloquea incluso en `--dangerously-skip-permissions`**.
   Garantía real, no recomendación.
4. **Mantened hooks bajo 500ms** salvo formateadores que tarden por
   naturaleza (como `dotnet format`).
5. **Observabilidad NO es opcional** para flujos serios. Sin logs
   estructurados, debugging es adivinación.
6. **`bash` explícito y `$CLAUDE_PROJECT_DIR`** son las dos claves
   para hooks portables en Windows.
