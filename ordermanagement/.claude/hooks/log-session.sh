#!/bin/bash
# log-session.sh — observabilidad básica (SessionEnd)
#
# Lee de stdin el JSON que Claude Code pasa al hook SessionEnd y anexa
# una línea JSON al log estructurado en .claude/logs/sessions.jsonl.
#
# Campos extraídos (best-effort, según versión de Claude Code):
#   - timestamp:   marca temporal ISO 8601 generada localmente
#   - sessionId:   id de la sesión que termina
#   - tokens:      tokens consumidos en la sesión (si están disponibles)
#   - exitReason:  motivo del cierre (clear, logout, prompt_input_exit, other)
#
# Sale siempre con exit 0. La observabilidad no debe bloquear nada.

set -u

LOG_DIR=".claude/logs"
LOG_FILE="$LOG_DIR/sessions.jsonl"

mkdir -p "$LOG_DIR"

# Lee el JSON completo de stdin
INPUT="$(cat)"

TIMESTAMP="$(date -u +%Y-%m-%dT%H:%M:%SZ)"

if command -v jq >/dev/null 2>&1; then
  SESSION_ID="$(printf '%s' "$INPUT" | jq -r '.session_id // empty')"
  TOKENS="$(printf '%s' "$INPUT" | jq -r '.tokens // empty')"
  EXIT_REASON="$(printf '%s' "$INPUT" | jq -r '.reason // empty')"

  jq -n \
    --arg timestamp "$TIMESTAMP" \
    --arg sessionId "$SESSION_ID" \
    --arg tokens "$TOKENS" \
    --arg exitReason "$EXIT_REASON" \
    '{timestamp: $timestamp, sessionId: $sessionId, tokens: $tokens, exitReason: $exitReason}' \
    >> "$LOG_FILE"
else
  # Fallback sin jq: línea JSON con escape mínimo
  printf '{"timestamp":"%s","sessionId":"unknown","tokens":"unknown","exitReason":"unknown"}\n' \
    "$TIMESTAMP" >> "$LOG_FILE"
fi

exit 0
