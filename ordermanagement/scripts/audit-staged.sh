#!/bin/bash
#
# audit-staged.sh
# Ejemplo de uso de Claude Code en modo one-shot dentro de un hook
# de pre-commit. NO está instalado por defecto en .git/hooks/.
#
# Para activarlo como hook de pre-commit:
#   cp ordermanagement/scripts/audit-staged.sh .git/hooks/pre-commit
#   chmod +x .git/hooks/pre-commit
#
# En Windows con Git for Windows, los hooks shell funcionan
# directamente. No requiere PowerShell.

set -e

# Capturamos el diff de lo staged
DIFF=$(git diff --cached --diff-filter=AM)

if [ -z "$DIFF" ]; then
    echo "No hay cambios staged. Saltando audit."
    exit 0
fi

# Pipeamos el diff a Claude Code en modo one-shot
echo "Auditando cambios staged con Claude Code..."

RESULT=$(echo "$DIFF" | claude -p "Audita este diff staged buscando: bugs evidentes, problemas de seguridad (credenciales, inyección, XSS), violaciones de las convenciones del CLAUDE.md, y código sin tests cuando aplique.

Si no detectas problemas críticos, responde solo: OK
Si detectas algo, lista cada problema con: fichero, línea aproximada, severidad (BAJA/MEDIA/ALTA/CRÍTICA), descripción breve.")

# Si la respuesta es OK, dejamos pasar
if [ "$RESULT" = "OK" ]; then
    echo "✓ Audit OK. Commiteando."
    exit 0
fi

# Si hay hallazgos, los mostramos y dejamos que el dev decida
echo ""
echo "=========================================="
echo "Hallazgos del audit de Claude Code:"
echo "=========================================="
echo "$RESULT"
echo "=========================================="
echo ""

# Pedimos confirmación al dev
read -p "¿Quieres commitear de todas formas? (s/N): " -n 1 -r CONFIRM
echo ""

if [[ "$CONFIRM" =~ ^[SsYy]$ ]]; then
    echo "Commiteando bajo tu responsabilidad."
    exit 0
else
    echo "Commit abortado por hallazgos del audit."
    exit 1
fi
