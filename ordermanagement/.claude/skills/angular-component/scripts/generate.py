#!/usr/bin/env python3
"""generate.py — preparación determinista para el skill angular-component.

Toma un nombre de componente en cualquier convención (PascalCase o
kebab-case), lo normaliza, comprueba si la carpeta destino ya existe, y
emite por stdout un JSON con la información que el skill consume para
generar los ficheros desde las plantillas de assets/.

Uso:
    python generate.py OrdersList
    python generate.py orders-list

Salida (stdout):
    {"pascalName": "OrdersList", "kebabName": "orders-list",
     "targetDir": "frontend/src/app/components/orders-list", "exists": false}

Errores:
    - Argumento ausente o vacío -> exit 2.
    - Si la carpeta destino existe -> exits 0 con `"exists": true`,
      el skill decide si sobrescribir.
"""

from __future__ import annotations

import json
import re
import sys
from pathlib import Path


def to_pascal_case(name: str) -> str:
    """Convierte cualquier nombre razonable a PascalCase."""
    # Separa por guiones, espacios o transiciones camel.
    parts = re.split(r"[-_\s]+|(?=[A-Z])", name)
    return "".join(p[:1].upper() + p[1:] for p in parts if p)


def to_kebab_case(name: str) -> str:
    """Convierte cualquier nombre razonable a kebab-case."""
    s = re.sub(r"(?<!^)(?=[A-Z])", "-", name)
    s = re.sub(r"[\s_]+", "-", s)
    return s.lower().strip("-")


def main(argv: list[str]) -> int:
    if len(argv) < 2 or not argv[1].strip():
        print("ERROR: nombre del componente requerido.", file=sys.stderr)
        print("Uso: python generate.py <NombreComponente>", file=sys.stderr)
        return 2

    raw = argv[1].strip()
    pascal_name = to_pascal_case(raw)
    kebab_name = to_kebab_case(raw)

    target_dir = Path("frontend") / "src" / "app" / "components" / kebab_name
    exists = target_dir.exists()

    payload = {
        "pascalName": pascal_name,
        "kebabName": kebab_name,
        "targetDir": str(target_dir).replace("\\", "/"),
        "exists": exists,
    }

    print(json.dumps(payload, ensure_ascii=False))
    return 0


if __name__ == "__main__":
    sys.exit(main(sys.argv))
