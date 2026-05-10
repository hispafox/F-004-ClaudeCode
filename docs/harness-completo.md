# El harness completo — definición operativa del módulo 3

## La fórmula

> **harness = prompts + tools + context policies + hooks + feedback loops + observability**

## Cada pieza, dónde la tenéis

### Prompts

- **CLAUDE.md** del proyecto (módulo 1) con las convenciones del equipo.
- **6 skills** en `ordermanagement/.claude/skills/` (módulo 2):
  - `angular-component` (con `context: fork`)
  - `commit-style`
  - `db-reset` (con `disable-model-invocation`)
  - `frontend-design` (oficial Anthropic)
  - `pre-commit-check` (orquestador con loop + context bank)
  - `pre-pr-check` (fan-out paralelo)

### Tools

- Tools del agente principal: Read, Write, Edit, Bash, Grep, Glob…
- MCP servers (módulos 1 y 4).

### Context policies

- **3 subagentes** en `ordermanagement/.claude/agents/` (módulo 3.1 + 3.2b):
  - `repo-explorer` (Haiku, read-only)
  - `dotnet-reviewer` (Sonnet, read + git diff)
  - `convention-checker` (Haiku, read + git diff)
- Tools restringidos por rol.
- Scopes: user / project / local.

### Hooks

- **format-on-write** (project level, `PostToolUse` / `Write|Edit|MultiEdit`)
  — añadido en 3.3a.
- **block-dangerous** (user level, `PreToolUse` / `Bash`)
  — añadido en 3.3b. Vive en `~/.claude/settings.json`, no en este repo.
- **log-session** (project level, `SessionEnd`) — añadido en 3.3b.

### Feedback loops

- Loop validator → implementer en `pre-commit-check` (techo 3 iteraciones).
- Loop fan-out → fan-in en `pre-pr-check`.
- Context bank en `ordermanagement/.claude/workflow-state/<session>/`.

### Observability

- Hook `SessionEnd` que vuelca a `ordermanagement/.claude/logs/sessions.jsonl`.
- Context bank ya provee trazabilidad de workflows.

## La idea final

Cuando personalizáis Claude Code con todas estas piezas, no estáis
configurando una herramienta. **Estáis construyendo vuestro propio
harness encima del de Anthropic**. Vuestro harness sabe a vuestro
equipo. Sabe vuestras convenciones. Sabe delegar. Sabe corregir. Sabe
garantizar. Sabe loggear.

## Las dos preguntas antes del módulo 4

1. ¿Qué hook concreto vais a configurar el lunes en vuestro repo del
   trabajo? Si la respuesta es *"el de auto-format"* o *"el de bloquear
   peligrosos"*, perfecto.
2. ¿Hay alguien en vuestro equipo de diseño con quien colaboréis y que
   ya use Figma? Si sí, el módulo 4 va a tener nombre y apellidos.

## Lectura complementaria opcional

Para roles que decidan arquitecturas a nivel sistema:
**"Building Effective AI Agents: Architecture Patterns and
Implementation Frameworks"** — Anthropic. Los patrones que vimos aquí
(hierarchical, collaborative, sequential, parallel, evaluator-optimizer)
en versión formal.
