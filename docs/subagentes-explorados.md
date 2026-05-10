# Subagentes — notas de exploración

Notas de los experimentos con los subagentes integrados que vienen con
Claude Code. Sirven como referencia para escribir subagentes propios a
partir de la demo 3.1b.

## Los tres built-in

### Explore

- **Para qué**: lectura y exploración. Solo lee, no modifica.
- **Modelo**: Haiku por defecto (rápido y barato).
- **Cuándo activa**: cuando una tarea principal requiere entender una
  zona del repo que no es la que estás tocando.

### Plan

- **Para qué**: planifica antes de actuar. No escribe código.
- **Cuándo se usa**: tareas que tocan más de tres ficheros, decisiones
  de diseño implícitas, o donde un error a mitad sería costoso.
- **Invocación**: `/plan` o automáticamente cuando el agente principal
  detecta complejidad.

### General-purpose

- **Para qué**: el comodín. Puede explorar Y modificar.
- **Cuándo se usa**: cuando una tarea requiere ambas pero el principal
  quiere mantener su contexto limpio.

## Hallazgos del experimento de contaminación

(Esta sección la rellena Pedro durante el screencast con los datos
reales que obtenga.)

### Sin delegación (sesión "antes")

- Pregunta: …
- Ficheros leídos por el principal: …
- Estado del contexto al final: …

### Con delegación a Explore (sesión "después")

- Misma pregunta: …
- Ficheros leídos por el principal: …
- Resumen recibido del subagente Explore: …
- Estado del contexto al final: …

## Lecciones extraídas

1. **El problema que resuelve el subagente es operativo, no teórico**:
   sin delegación, X ficheros pesan en el contexto. Con delegación,
   solo el resumen.
2. **Auto-delegación no es perfecta**: a veces el principal no delega
   cuando debería. Solución: invocar explícitamente.
3. **Cada built-in tiene su nicho**: Explore para entender, Plan para
   diseñar, general-purpose para tareas mixtas.

## Próximo paso

En la demo 3.1b vamos a crear nuestro primer subagente custom: un
`repo-explorer` para OrderManagement con su propio rol y su propio
system prompt.

---

### Subagentes propios construidos en 3.1b

- **`repo-explorer`** — modelo Haiku, tools `Read, Grep, Glob` (read-only).
  Rol: explorador estructural del proyecto OrderManagement con foco en
  capas. Devuelve resumen en cinco secciones (estructura, dependencias,
  patrones detectados, anti-patrones emergentes, hallazgos accionables)
  bajo 400 palabras. NUNCA escribe.

- **`dotnet-reviewer`** — modelo Sonnet, tools `Read, Grep, Glob, Bash(git diff:*)`.
  Rol: revisor crítico de código C#/.NET. Lee `git diff --cached` o
  `git diff HEAD~1 HEAD`, clasifica hallazgos por severidad
  (CRÍTICO / ALTA / MEDIA) con formato verbatim
  `<severidad>: <fichero>:<línea>:<problema>:<fix>`. Cierra con
  línea de resumen y recomendación (BLOQUEAR_PR / REVISAR / OK_CON_NOTAS).
  NUNCA modifica código.

Caso pedagógico de la 3.1b: en `demo/3.1b-before` se introdujo
deliberadamente un `.Result` bloqueante en `CancelOrderHandler.cs`. El
formador construye los dos subagentes en directo, ejecuta el
`dotnet-reviewer` sobre el diff, lo caza como CRÍTICO, y aplica el fix
revertiendo a `await`.

### Composición en 3.2a

La 3.2a no añade subagentes nuevos, pero compone los de 3.1b en dos
patrones distintos:

- **Aislamiento por `context: fork`**: el skill `angular-component`
  pasa de v4 sin aislamiento a v4 con `context: fork` en el frontmatter.
  El skill lee plantillas y ejecuta un script Python; sin fork, todo
  ese material pesa sobre el principal. Con fork, solo el resumen final
  llega al principal. Es la misma motivación que con un subagente, pero
  aplicada a un skill.

- **Composición skill+subagente con loop techo=3**: el skill orquestador
  `pre-commit-check` (nuevo en 3.2a) implementa un patrón
  *validator → implementer*. Captura `git diff --cached`, invoca al
  subagente `dotnet-reviewer` (Sonnet), lee la severidad clasificada,
  aplica fixes con `Edit` si hay CRÍTICO, hace `git add` del fichero
  arreglado y reitera. Techo de 3 iteraciones para evitar bucles
  infinitos. Si tras 3 vueltas siguen los CRÍTICOS, devuelve al usuario.
  Si solo hay ALTA/MEDIA, sale con éxito y avisa.

Caso pedagógico de la 3.2a: en `demo/3.2a-before` se introdujo
deliberadamente un `try { ... } catch (Exception ex) { Console.WriteLine }`
en `CreateOrderHandler.cs` (prohibido por CLAUDE.md). El formador
añade `context: fork` al skill `angular-component`, crea el skill
`pre-commit-check`, lo ejecuta sobre el diff staged, y el loop con el
`dotnet-reviewer` caza el catch genérico como CRÍTICO y propone
revertir a excepción tipada. Tras aplicar el fix con `Edit`, la segunda
iteración termina en `OK_CON_NOTAS`.
