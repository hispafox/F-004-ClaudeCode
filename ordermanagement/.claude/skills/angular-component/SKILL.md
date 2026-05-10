---
name: angular-component
description: Genera un componente Angular 19 standalone con Signals para OrderManagement. Usa plantillas en `assets/` y un script `scripts/generate.py` que normaliza el nombre y valida colisiones de carpeta. Activar cuando el usuario pida crear, generar o añadir un componente Angular nuevo (verbos "crea", "genera", "añade", "necesito un componente").
context: fork
---

# angular-component

Generador de componentes Angular 19 standalone con Signals para OrderManagement.

## Cuándo se usa

Activar cuando el usuario pida añadir un componente Angular nuevo al frontend.
NO usar para modificar componentes existentes, crear servicios/directivas/pipes,
ni tocar el backend.

## Pasos al generar

1. **Pedir al usuario el nombre del componente** (PascalCase o kebab-case).

2. **Ejecutar el script de preparación** para normalizar el nombre y validar
   la colisión de carpeta:

   ```!
   python ordermanagement/.claude/skills/angular-component/scripts/generate.py {{NOMBRE}}
   ```

   El script imprime un JSON con `{ pascalName, kebabName, targetDir, exists }`.
   Si `exists` es `true`, avisar al usuario y preguntar si sobrescribir.

3. **Generar los cuatro ficheros del componente** desde las plantillas
   de `assets/`, sustituyendo los placeholders `{{KEBAB_NAME}}`,
   `{{PASCAL_NAME}}`, `{{INPUTS}}`, `{{OUTPUTS}}`, `{{INJECTIONS}}`,
   `{{INJECTIONS_IMPORTS}}`, `{{TEMPLATE_BODY}}` con los valores que
   correspondan al caso del usuario:

   - `assets/component.template.ts` → `<targetDir>/<kebabName>.component.ts`
   - `assets/component.template.html` → `<targetDir>/<kebabName>.component.html`
   - `assets/component.template.spec.ts` → `<targetDir>/<kebabName>.component.spec.ts`
   - Crear también un `<kebabName>.component.scss` mínimo con `:host { display: block; }`.

4. **Confirmar al usuario** los ficheros generados y recordar que el
   componente se importa donde se use con `loadComponent`.

## Convenciones que respetan las plantillas

- **8 bloques en orden estricto** en el `.ts`: imports → @Component → inputs
  → outputs → injections → estado (signals/computed) → lifecycle → métodos.
- **Control flow nuevo** en el template: `@if`, `@for`, `@switch`. Nunca
  `*ngIf`/`*ngFor`/`*ngSwitch`.
- **`inject()` en lugar de constructor** para dependencias.
- **Tokens del design system** (`var(--color-*)`, `var(--space-*)`) consumidos
  desde `frontend/src/styles/_tokens.scss` cargados globalmente.
- **Selector con prefijo `app-`** y kebab-case del nombre.

## Lo que NO debe hacer

- NO crear servicios, directivas, pipes ni guards.
- NO modificar `app.routes.ts`, `app.config.ts` ni el componente raíz.
- NO añadir el componente a ningún `imports` externo (es standalone).
- NO tocar el backend (.NET) ni los `.csproj`.
- NO instalar dependencias npm nuevas.

## Cuándo NO subir más versiones

Este skill ya está en v4 (plantillas + script). Subir más solo si aparece
una señal concreta del manual: `SKILL.md` cruzando 2.000 palabras o tareas
deterministas que se vuelven recetas repetitivas. Hasta entonces, esta v4
es la forma estable.
