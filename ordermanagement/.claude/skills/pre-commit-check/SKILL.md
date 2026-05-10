---
name: pre-commit-check
description: Orquesta una revisión de los cambios staged invocando al subagente dotnet-reviewer, aplicando los fixes propuestos, y reiterando hasta que el reviewer no encuentre hallazgos CRÍTICOS o se llegue al techo de 3 iteraciones. Úsalo antes de cada commit en código C#/.NET — verbos "antes de commitear", "verifica el commit", "comprueba este diff", "pre-commit".
context: fork
---

# pre-commit-check

Skill orquestador que aplica un loop **validator → implementer** sobre los
cambios staged antes de cada commit, usando el subagente `dotnet-reviewer`
como validador y el agente principal (con tools `Edit`) como implementador
de fixes. Techo de 3 iteraciones para evitar bucles infinitos.

## Cuándo se usa

Activar cuando el usuario pida verificar los cambios staged antes de
commitear. Ejemplos: "antes de commitear, comprueba esto", "valida el
diff", "pre-commit", "auditame los cambios staged".

NO usar para:

- Hacer el commit en sí (ese sigue siendo decisión del usuario tras
  revisar el resultado del loop).
- Auditar diff que no esté staged (`pre-pr-check` se cubrirá en 3.2b).
- Revisar código histórico (`git log` o exploración manual).

## Workflow

```
┌────────────────────────────────────────────────────────┐
│  iteración = 1                                         │
│  loop:                                                 │
│    1. git diff --cached                                │
│    2. invoca subagente dotnet-reviewer con el diff     │
│    3. lee la salida (CRÍTICO / ALTA / MEDIA + recom.) │
│    4. si Recomendación = OK_CON_NOTAS:                 │
│         ✓ salir del loop, devolver al usuario          │
│    5. si tiene CRÍTICO:                                │
│         a. aplicar el fix sugerido con Edit            │
│         b. git add <fichero arreglado>                 │
│         c. iteración += 1                              │
│         d. si iteración > 3:                           │
│              ✗ parar, reportar y NO commitear          │
│         e. volver a 1                                  │
│    6. si solo tiene ALTA o MEDIA (sin CRÍTICO):        │
│         ✓ salir del loop, advertir al usuario          │
│           (decide él si commitea o repasa)             │
└────────────────────────────────────────────────────────┘
```

## Pasos al ejecutar

0. **Setup del context bank**. Genera un `<sessionId>` con timestamp y
   crea la carpeta `.claude/workflow-state/<sessionId>/`. Todos los
   artefactos del workflow viven ahí — el `.gitignore` ya excluye
   `.claude/workflow-state/` para que no contamine commits.

   ```!
   mkdir -p .claude/workflow-state
   SESSION="$(date +%Y%m%d-%H%M%S)-pre-commit"
   mkdir -p ".claude/workflow-state/$SESSION"
   echo "$SESSION" > .claude/workflow-state/.last-session
   ```

1. **Capturar el diff staged** y volcarlo a `INPUT.md`:

   ```!
   git diff --cached > ".claude/workflow-state/$SESSION/INPUT.md"
   ```

   Si `INPUT.md` está vacío, abortar con un aviso al usuario.

2. **Invocar al subagente `dotnet-reviewer`** indicándole que lea el
   diff de `<sessionId>/INPUT.md` y escriba sus hallazgos en
   `<sessionId>/REVIEW-N.md` (donde `N` es el contador de iteración,
   empezando en 1). El reviewer mantiene el formato verbatim
   `<severidad>: <fichero>:<línea>:<problema>:<fix>` y cierra con la
   línea `Recomendación: ...`.

3. **Decidir según la recomendación** (leyendo `REVIEW-N.md`):

   - `OK_CON_NOTAS` → loop termina con éxito. Mostrar al usuario los
     hallazgos MEDIA/ALTA si los hay y la ruta al context bank.
   - Hay `CRÍTICO` → aplicar fixes con `Edit`, hacer `git add` del
     fichero arreglado, registrar lo aplicado en
     `<sessionId>/FIXES-N.md`, incrementar `N` y repetir desde el
     paso 1 (el nuevo diff staged se vuelca a `INPUT.md` otra vez).
   - Si `N > 3` sin que desaparezcan los CRÍTICOS, parar y referenciar
     `REVIEW-1..3.md` y `FIXES-1..N.md` para que el usuario decida.

4. **Reportar al usuario** un resumen del loop:
   - Iteraciones realizadas.
   - Hallazgos resueltos.
   - Hallazgos pendientes (si los hay).
   - Ruta del context bank: `.claude/workflow-state/<sessionId>/`.
   - Recomendación final.

5. **NO ejecutar `git commit`**. Eso queda en manos del usuario tras leer
   el resumen.

## Limpieza del context bank

El context bank **no se limpia automáticamente**. El usuario decide:

- Inspeccionarlo después del workflow para entender qué pasó.
- Borrarlo cuando ya no lo necesita (`rm -rf .claude/workflow-state/<sessionId>/`).
- Confiar en que `.gitignore` lo excluye de git.

## Por qué context: fork

Este skill lee el diff y los ficheros tocados, además de invocar a un
subagente que también lee. Sin `context: fork`, todo eso contaminaría la
sesión principal con material de revisión. Con `fork`, el skill aísla su
contexto y solo el resumen final llega al principal.

## Lo que NO debe hacer

- NO hacer `git commit` — la decisión es del usuario.
- NO modificar ficheros que no estén staged.
- NO iterar más de 3 veces aunque queden hallazgos CRÍTICOS — devolver
  al usuario para que decida cómo proceder manualmente.
- NO inventar fixes que el `dotnet-reviewer` no haya sugerido. Si el
  fix no está claro, parar.
- NO ejecutar contra ramas remotas ni hacer `git fetch`/`git pull`.

## Composición típica

```
usuario → "antes de commitear, valida"
          ↓
      pre-commit-check (skill, context: fork)
          ↓
      dotnet-reviewer (subagente, modelo Sonnet)
          ↓
      hallazgos clasificados
          ↓
      principal aplica fixes con Edit
          ↓
      loop hasta OK o techo=3
```

Esta composición es la que la gamma 3.2a llama el patrón **hierarchical /
supervisory**: skill orquesta, subagente ejecuta, principal aplica.
