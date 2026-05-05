---
name: commit-style
description: Genera mensajes de commit en estilo Conventional Commits en español a partir del diff staged, siguiendo las convenciones del equipo OrderManagement (tipo, scope por capa, imperativo presente sin punto final). Activar cuando el usuario pida redactar, escribir o sugerir un mensaje de commit, o use verbos como "haz un commit", "commitea", "redacta el commit".
---

# commit-style

Genera mensajes de commit Conventional Commits para OrderManagement.

## Origen

Este skill nació en `~/.claude/skills/` (scope personal) cuando el formador
descubrió que escribía manualmente el mismo formato de commit varias veces
al día. Tras dos semanas validándolo en su flujo personal, se promovió a
`.claude/skills/` (scope project) en la demo 2.2c para que todo el equipo
lo herede al clonar el repo.

## Cuándo se usa

Activar cuando el usuario quiera generar un mensaje de commit a partir del
diff staged. Ejemplos: "haz un commit con esto", "redáctame el commit",
"sugiéreme un mensaje", "commitea".

NO usar para:

- Hacer el commit en sí (eso es decisión del usuario tras revisar el mensaje).
- Modificar mensajes ya commiteados (`git commit --amend` pertenece al usuario).
- Generar tags ni release notes.

## Formato del mensaje

```
<tipo>(<scope>): <resumen en imperativo presente>
```

Ejemplo: `feat(api): añade endpoint POST /api/orders/{id}/cancel`

Reglas:

- **En español**, imperativo presente (`añade`, `corrige`, `refactoriza`).
- **Sin punto final** en el resumen.
- **Resumen ≤ 72 caracteres**.
- **Cuerpo opcional**, separado por línea en blanco, explicando *por qué*
  más que *qué* (el qué se ve en el diff).

## Tipos permitidos

- `feat` — feature nueva.
- `fix` — corrección de bug.
- `refactor` — cambio de código sin alterar comportamiento.
- `docs` — solo documentación.
- `test` — añadir o ajustar tests.
- `chore` — cambios de mantenimiento (deps, gitignore, etc.).
- `style` — formato/estilo (no afecta a lógica).

## Scopes permitidos

Reflejan las capas del proyecto:

- `api` — `src/OrderManagement.Api/`
- `application` — `src/OrderManagement.Application/`
- `domain` — `src/OrderManagement.Domain/`
- `infrastructure` — `src/OrderManagement.Infrastructure/`
- `frontend` — `frontend/`
- `tests` — `tests/`
- `docs` — `docs/`
- `init` — andamiaje, configuración inicial del repo.

Si el cambio toca varias capas, elegir la **principal** (la que justifica el
commit), no enumerarlas todas.

## Pasos al generar

1. **Leer el diff staged**:

   ```!
   git diff --cached --stat
   ```

   ```!
   git diff --cached
   ```

2. **Identificar el tipo** (feat/fix/refactor/...) según el cambio dominante.

3. **Identificar el scope** (api/application/domain/...) según la capa
   dominante.

4. **Redactar el resumen** en imperativo presente, ≤ 72 caracteres, sin
   punto final.

5. **Si el cambio es ambiguo** o toca varias capas, redactar un cuerpo de
   2-4 líneas explicando el porqué.

6. **Presentar el mensaje al usuario** sin ejecutar `git commit`. Pedirle
   confirmación o ajuste antes de cualquier acción.

## Ejemplo de salida coherente con el repo

```
feat(api): añade endpoint POST /api/orders/{id}/cancel

Expone el handler CancelOrderHandler ya existente como endpoint
dedicado. Reemplaza la excepción genérica por InvalidOrderStateException
para que el controller pueda traducir a 422 UnprocessableEntity.
```
