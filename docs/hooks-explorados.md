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

## Eventos cubiertos hasta ahora

(Se rellena durante el módulo 3.3.)

## Lecciones extraídas

(Se rellena durante el módulo 3.3.)
