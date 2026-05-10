---
name: pre-pr-check
description: Antes de abrir un PR, lanza tres subagentes EN PARALELO (dotnet-reviewer para código, convention-checker para estructura, repo-explorer para análisis de impacto) sobre los cambios y combina los tres reportes en un veredicto único. Más exhaustivo que pre-commit-check (3 perspectivas vs 1). Verbos: "antes del PR", "valida para PR", "pre-pr", "fan-out review".
context: fork
---

# pre-pr-check

Skill orquestador con patrón **fan-out / fan-in**. Lanza tres subagentes
independientes EN PARALELO sobre el diff que va al PR, recoge sus tres
reportes desde el context bank, y combina los hallazgos en un veredicto
unificado.

## Cuándo se usa

Activar antes de abrir o actualizar un PR. Verbos: *"valida para el
PR"*, *"haz un pre-pr-check"*, *"revisa antes del push"*.

NO usar para:

- Validación previa al commit local (ese es `pre-commit-check`).
- Aplicar fixes automáticamente (este skill solo informa, no edita).
- Subir el PR (la decisión y el `gh pr create` son del usuario).

## Por qué tres subagentes en paralelo

Cada uno mira el mismo diff con un foco distinto:

- **`dotnet-reviewer`** → calidad de código (async, naming, manejo de
  errores, violaciones de CLAUDE.md). Modelo Sonnet.
- **`convention-checker`** → estructura, naming MediatR, ubicación de
  ficheros, capas. Modelo Haiku.
- **`repo-explorer`** → análisis de impacto (qué consume lo modificado,
  qué módulos se ven afectados indirectamente). Modelo Haiku.

Las tres revisiones son **ortogonales**: ninguna depende del output de
otra. Por eso van en paralelo. Tiempo total ≈ tiempo del más lento, no
la suma. Si fueran dependientes (ej: tester antes que reviewer), iría
en serie — el error típico de la gamma 3.2b es paralelizar con
dependencias ocultas.

## Workflow

```
┌──────────────────────────────────────────────────────────┐
│  Setup: crear .claude/workflow-state/<sessionId>/        │
│         volcar diff a INPUT.md                           │
│  ↓                                                       │
│  FAN-OUT: invocar los 3 subagentes A LA VEZ              │
│    ├─ dotnet-reviewer    → REVIEW.md                     │
│    ├─ convention-checker → CONVENTIONS.md                │
│    └─ repo-explorer      → IMPACT.md                     │
│  ↓                                                       │
│  Esperar a que los 3 terminen                            │
│  ↓                                                       │
│  FAN-IN: leer los 3 ficheros, combinar en VERDICT.md     │
│  ↓                                                       │
│  Reportar VERDICT.md al usuario                          │
└──────────────────────────────────────────────────────────┘
```

## Pasos al ejecutar

1. **Setup del context bank**:

   ```!
   mkdir -p .claude/workflow-state
   SESSION="$(date +%Y%m%d-%H%M%S)-pr-check"
   mkdir -p ".claude/workflow-state/$SESSION"
   ```

2. **Capturar el diff** (por defecto el último commit; si el usuario
   indica otro rango, respétalo):

   ```!
   git diff HEAD~1 HEAD > ".claude/workflow-state/$SESSION/INPUT.md"
   git diff --stat HEAD~1 HEAD >> ".claude/workflow-state/$SESSION/INPUT.md"
   ```

   Si `INPUT.md` está vacío, abortar.

3. **Fan-out — invocar los tres subagentes en paralelo** (UNA sola
   ronda con tres tool calls al sistema de subagentes, no tres rondas
   secuenciales). Cada invocación incluye:

   - La ruta a `INPUT.md` para que el subagente lea el diff.
   - La ruta de salida concreta donde debe volcar su reporte:
     - `dotnet-reviewer` → `<sessionId>/REVIEW.md`
     - `convention-checker` → `<sessionId>/CONVENTIONS.md`
     - `repo-explorer` → `<sessionId>/IMPACT.md`

4. **Fan-in — combinar los tres reportes**. Lee los tres ficheros y
   construye `<sessionId>/VERDICT.md` con esta plantilla:

   ```markdown
   # Pre-PR Check — Veredicto

   ## Resumen ejecutivo
   <1-2 líneas: ¿el PR está listo?>

   ## Hallazgos por severidad

   ### CRÍTICO (bloquea el PR)
   - <fichero:línea:problema:fix>  (origen: REVIEW | CONVENTIONS | IMPACT)

   ### IMPORTANTE (recomendado arreglar)
   - ...

   ### SUGERENCIA (opcional)
   - ...

   ## Análisis de impacto
   <síntesis de IMPACT.md>

   ## Convenciones
   <síntesis de CONVENTIONS.md>

   ## Recomendación final
   ✅ PR LISTO PARA SUBIR | ⚠️ ARREGLAR ANTES | 🚫 NO SUBIR
   ```

   La regla de la recomendación final:
   - Si **alguno** de los tres reporta CRÍTICO → `🚫 NO SUBIR`.
   - Si los tres están en `OK_CON_NOTAS` y solo hay IMPORTANTE/SUGERENCIA
     → `⚠️ ARREGLAR ANTES`.
   - Si los tres están limpios → `✅ PR LISTO PARA SUBIR`.

5. **Reportar al usuario** el contenido de `VERDICT.md` y la ruta del
   context bank para que pueda inspeccionar los tres reportes
   originales si quiere afinar.

6. **NO ejecutar `git push` ni `gh pr create`**. La decisión de subir
   el PR es del usuario.

## Por qué context: fork

Cada subagente lee el diff y escribe su reporte; el orquestador lee
los tres ficheros y compone el veredicto. Sin `context: fork`, todo
ese material (diff multi-fichero + tres reportes) contaminaría el
contexto principal. Con `fork`, solo el `VERDICT.md` final llega al
principal.

## Diferencias con pre-commit-check

| Aspecto              | pre-commit-check        | pre-pr-check                     |
|----------------------|-------------------------|----------------------------------|
| Subagentes           | 1 (dotnet-reviewer)     | 3 (reviewer + convention + explorer) |
| Patrón               | Validator → Implementer | Fan-out / Fan-in                 |
| Aplica fixes         | Sí, con loop techo=3    | No, solo informa                 |
| Diff por defecto     | `git diff --cached`     | `git diff HEAD~1 HEAD`           |
| Cuándo               | Antes de commit local   | Antes de abrir/actualizar PR     |

## Lo que NO debe hacer

- NO invocar los 3 subagentes en serie. La paralelización es el
  núcleo del skill.
- NO aplicar fixes. Eso lo hace `pre-commit-check` o el usuario.
- NO hacer `git push` ni `gh pr create`.
- NO iterar — fan-out/fan-in es de una sola ronda. Si hay CRÍTICOS,
  el usuario aplica los fixes (o pasa por `pre-commit-check`) y
  vuelve a lanzar `pre-pr-check`.
- NO escalar a más de 4-5 subagentes en paralelo (la regla de la
  gamma 3.2b: el coste de coordinación se come el ahorro).
