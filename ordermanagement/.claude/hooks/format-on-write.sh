#!/bin/bash
# format-on-write.sh — auto-format hook (PostToolUse / Write|Edit|MultiEdit)
#
# Lee de stdin el JSON de input que Claude Code pasa al hook, extrae el
# path del fichero modificado, y aplica el formateador correspondiente
# según la extensión:
#   .cs           → dotnet format --include <path>
#   .json | .md   → npx prettier --write <path>
#   .ts | .html | .scss → TODO en 3.3b (placeholder por ahora)
#
# Sale con exit 0 siempre — el formato es best-effort, no debe bloquear
# la operación. Para bloqueo usaremos exit 2 en hooks PreToolUse en 3.3b.

set -u

# Lee el JSON completo de stdin
INPUT="$(cat)"

# Extrae el path del fichero. Claude Code expone el campo en
# tool_input.file_path para Write/Edit/MultiEdit. Usamos jq si está
# disponible, si no un grep/sed defensivo.
if command -v jq >/dev/null 2>&1; then
  FILE_PATH="$(printf '%s' "$INPUT" | jq -r '.tool_input.file_path // empty')"
else
  FILE_PATH="$(printf '%s' "$INPUT" \
    | grep -o '"file_path"[[:space:]]*:[[:space:]]*"[^"]*"' \
    | head -1 \
    | sed -E 's/.*"file_path"[[:space:]]*:[[:space:]]*"([^"]*)"/\1/')"
fi

if [ -z "${FILE_PATH:-}" ]; then
  exit 0
fi

# Normaliza separador de paths para Git Bash en Windows
FILE_PATH="${FILE_PATH//\\//}"

# Si el fichero no existe (ya lo borraron, p. ej.), salimos limpios
[ -f "$FILE_PATH" ] || exit 0

case "$FILE_PATH" in
  *.cs)
    if command -v dotnet >/dev/null 2>&1; then
      dotnet format --include "$FILE_PATH" >/dev/null 2>&1 || true
    fi
    ;;
  *.json|*.md)
    if command -v npx >/dev/null 2>&1; then
      npx --yes prettier --write "$FILE_PATH" >/dev/null 2>&1 || true
    fi
    ;;
  *.ts|*.html|*.scss)
    # TODO 3.3b: enganchar prettier + ESLint para frontend Angular.
    echo "format-on-write: pendiente para Angular ($FILE_PATH)" >&2
    ;;
esac

exit 0
