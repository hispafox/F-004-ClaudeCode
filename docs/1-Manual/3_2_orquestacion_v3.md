# 3.2 Orquestación y flujos compuestos

**Duración en clase:** 35-37 minutos · **Sesión 3, submódulo 2** · **Versión: v3**

> **Cambios v2 → v3**: notas de vocabulario formal (hierarchical/supervisory pattern, evaluator-optimizer, collaborative/swarm), nueva sección "Paralelo vs serial: cuándo elegir cada patrón", caja con cifra de coste real de multi-agente (10-15x). +5 min de clase respecto a v2 (30 → 35-37 min) por la sección nueva de paralelo vs serial.

---

## El frame: agent harness

Recoges el hilo donde lo dejamos en 3.1. Allí mencionamos el concepto: **agent = model + harness**. Un modelo en bruto no es un agente; lo es solo cuando le rodeas de tools, contexto, hooks y feedback loops. Y cuando personalizas Claude Code con skills, subagentes y hooks, estás construyendo tu propio harness encima del de Anthropic.

Este apartado es **donde el harness empieza a parecer un harness**. Hasta aquí cada pieza vivía aislada. Skills por su lado, subagentes por el suyo. Aquí los unimos en flujos coherentes. Y aparece la pregunta natural cuando empiezas a tener varios skills, varios subagentes y algún MCP server conectado: *"¿cómo se combinan en algo más grande que la suma de sus partes?"*

Aquí está la respuesta. Composición de capas. Un skill que orquesta varios subagentes. Un subagente que tira de MCP servers. Loops de retroalimentación entre un validator y un implementer. Artefactos durables en el repo que actúan como memoria compartida entre subagentes — lo que la comunidad llama **context bank**. Y, en los casos más complejos, varias sesiones de Claude Code coordinándose entre sí — Agent Teams.

Esto es más conceptual que el resto. No vamos a escribir código nuevo — vamos a entender cómo encajan los elementos que ya tenemos y cuándo merece la pena montar orquestaciones más complejas. Que normalmente es **menos veces de las que la gente cree**.

---

## `context: fork` en skills: el primer mecanismo de aislamiento

Lo más simple para empezar a orquestar: un skill que se ejecuta en su propio contexto sin contaminar el de la sesión principal.

### Cómo funciona

Recordatorio del módulo 2: un skill se ejecuta dentro del contexto de la sesión principal. Sus instrucciones, sus razonamientos, lo que decide hacer — todo eso vive en la misma ventana de contexto que tu conversación con el agente.

Cuando un skill se vuelve grande o ejecuta tareas que requieren mucha exploración, esto puede ser problemático. La solución: añadir `context: fork` al frontmatter del skill.

```yaml
---
name: deep-architecture-analysis
description: Analiza la arquitectura del repo en profundidad y devuelve un informe estructurado. Usar cuando se necesite entender el diseño general antes de cambios grandes.
context: fork
allowed-tools: Read, Grep, Glob
---

[instrucciones extensas de análisis...]
```

Con `context: fork`, cuando el skill se activa:

1. Claude principal lanza el skill **en un contexto aislado**.
2. El skill ejecuta sus instrucciones — leer ficheros, analizar, razonar — sin que nada de eso aparezca en el contexto principal.
3. Cuando el skill termina, devuelve **solo su resultado final** al principal.

Es lo mismo que hace un subagente, pero envuelto en la abstracción de un skill. La diferencia es de framing — un skill se invoca para "hacer algo", un subagente se invoca para que "alguien con criterio se encargue de algo".

### Cuándo usar `context: fork`

La regla práctica:

- **Skill que lee mucho** (decenas de ficheros como parte de su trabajo) → `context: fork`. Si no, satura el principal.
- **Skill que produce output corto y conciso** a partir de mucho input → `context: fork`. La idea de aislar lo gordo y devolver lo destilado.
- **Skill que puede activarse en sesiones largas** y donde no quieres que añada peso → `context: fork`.

- **Skill rápido y simple** (genera un componente, formatea un commit) → no `fork`. El overhead no compensa.
- **Skill cuyo output va a ser inmediatamente modificado por el principal** → no `fork`. Mantén la integración tightly acoplada.

### Diferencia con subagente

Filosóficamente:

- **Skill con `context: fork`** = "haz esto en otro lado y dame el resultado". El skill define **una tarea**.
- **Subagente** = "encárgate de cosas como esta". El subagente define **un rol**.

En la práctica son cercanos, y la elección de cuál usar depende de cómo prefieras modelar tu kit. Mi recomendación: subagentes para roles recurrentes (Reviewer, Tester, Explorer) y skills con `context: fork` para tareas concretas que necesitan aislamiento sin merecer un rol entero.

---

## Composición de capas: el flujo end-to-end

Aquí está el patrón que más rentabilidad da. Un skill **orquesta** (es el initiator), los subagentes ejecutan en paralelo o serial, los MCP servers proveen datos externos, y artefactos durables sirven de memoria compartida. Cada capa tiene su responsabilidad clara.

### El patrón base

```
[Usuario]
    ↓
[Agente principal]
    ↓
[Skill: orquestador / initiator]
    ↓
[Subagente A]   [Subagente B]   [MCP Server]
    ↓               ↓                ↓
   resultado A   resultado B    datos externos
    ↓               ↓                ↓
    [Skill recoge resultados / context bank]
            ↓
    [Devuelve al principal]
```

Cada flecha es un punto donde el contexto se aísla o se transfiere. La clave es que el agente principal **solo ve los resultados destilados**, no las exploraciones intermedias.

### Una nota de vocabulario

Este patrón — un orquestador que delega a especialistas y sintetiza los resultados — tiene un nombre formal en literatura de arquitectura agentic: **hierarchical** o **supervisory pattern**. En esa terminología, el skill orquestador es el supervisor y los subagentes son los specialists. Si lees whitepapers o presentaciones de arquitectura más teóricos, los vas a encontrar así nombrados. Aquí lo aplicamos en su versión concreta dentro de Claude Code.

### Caso real desarrollado: feature completa con orquestación

Imagina la siguiente petición:

> *"Implementa el endpoint para cancelar pedidos. Asegúrate de que respeta nuestras convenciones, tiene tests con cobertura razonable, y no rompe nada existente."*

Sin orquestación, Claude Code haría todo en serie en su contexto principal: leer el OrdersController, leer Order.cs, leer los tests existentes, generar la modificación, generar los tests, ejecutar... y todo eso pesando en la ventana.

Con orquestación, podríamos tener un skill `feature-implementer` que:

1. **Invoca al subagente `repo-explorer`** para que mapee la zona del código relevante. Vuelve con un resumen.
2. **Invoca al subagente `feature-planner`** con el resumen del Explorer. Devuelve un plan paso a paso.
3. El skill confirma el plan con el usuario (esto sí en el principal).
4. **Implementa los cambios** — esta parte sí en el principal porque queremos que vea lo que se está modificando.
5. **Invoca al subagente `test-generator`** sobre el código nuevo. Devuelve los tests generados.
6. **Invoca al subagente `dotnet-reviewer`** para que valide el conjunto. Devuelve hallazgos.
7. Si hay hallazgos críticos, el skill los presenta al usuario y propone fixes.
8. **Devuelve al principal** un resumen de la feature implementada con el estado de los tests.

¿Qué hemos ganado? El contexto principal solo ha visto:

- La petición inicial.
- El resumen del Explorer (corto).
- El plan del Planner.
- La modificación real del código.
- El resumen de tests generados.
- Los hallazgos del Reviewer.

**No** ha visto: las decenas de ficheros que el Explorer leyó, las hipótesis que el Planner consideró y descartó, las iteraciones internas del Tester, las exploraciones del Reviewer. Todo eso vivió en sus propios contextos aislados.

Resultado: una sesión que ha hecho una feature completa con tests y review en el contexto que antes te llevaba implementar el endpoint nada más.

### Cómo se escribe esto

El skill orquestador tiene un cuerpo más o menos así:

````markdown
---
name: feature-implementer
description: Implementa features completas siguiendo el flujo del equipo: explorar, planificar, codificar, testear, revisar. Usar cuando el usuario pida implementar una feature de tamaño medio o grande.
allowed-tools: Read, Edit, Write, Bash(dotnet *), Bash(git *)
---

# Implementador de features

Cuando seas invocado para implementar una feature:

## Paso 1: Exploración

Invoca al subagente `repo-explorer` con un objetivo concreto:
"Explora la zona afectada por <feature> y devuelve un resumen de cómo está organizada."

Espera el resumen.

## Paso 2: Planificación

Invoca al subagente `feature-planner` pasándole el resumen del Explorer:
"Dado este contexto, planifica la implementación de <feature>."

Espera el plan. Preséntalo al usuario y pide confirmación.

## Paso 3: Implementación

Una vez confirmado el plan, ejecuta los cambios en el contexto principal.
Asegúrate de que cada cambio respeta las convenciones del CLAUDE.md.

## Paso 4: Tests

Invoca al subagente `test-generator` con el código modificado:
"Genera tests para los cambios introducidos en esta sesión."

Espera los tests. Si el subagente reporta fallos, pídele que itere.

## Paso 5: Review

Invoca al subagente `dotnet-reviewer`:
"Revisa los cambios introducidos buscando problemas críticos."

Espera el reporte. Presenta los hallazgos al usuario priorizados.

## Paso 6: Cierre

Devuelve un resumen final con:
- Ficheros modificados
- Tests añadidos
- Hallazgos del reviewer (con su severidad)
- Próximos pasos sugeridos
````

Es bastante prosa, pero el resultado es un workflow **estandarizado y reproducible** del cómo el equipo entrega features. Una vez escrito, cada feature pasa por el mismo flujo sin que nadie tenga que recordarlo.

---

## Loops de retroalimentación: lo que diferencia un harness frágil de uno fiable

El flujo del ejemplo anterior es **lineal**: paso 1 → paso 2 → paso 3 → ... → paso 6 → fin. Pero la realidad casi nunca es lineal. El reviewer puede encontrar un problema crítico que invalida la implementación. Los tests pueden fallar y no quedar claro si es bug del código o de los tests. El validator puede pedir reescribir parte del trabajo.

Un harness sin **loops de retroalimentación** es frágil. La primera vez que algo falla, todo el flujo se cae y depende de ti retomarlo. Un harness con loops bien diseñados se autocorrige sin tu intervención hasta cierto punto.

### Patrones típicos de loops

**1. Validator que devuelve al implementer.**

El más común. Después de implementar, un validator (subagente reviewer) examina el resultado. Si encuentra gaps, **devuelve el flujo al paso de implementación con instrucciones específicas de qué arreglar**. El loop se cierra cuando el validator aprueba o cuando se alcanza un máximo de iteraciones.

**Una nota de vocabulario**: este patrón — generador que produce, evaluador que critica, generador que reescribe basándose en la crítica — en literatura formal se llama **evaluator-optimizer**. Lo vas a ver así nombrado en cualquier discusión sobre patrones agentic más allá de Claude Code. Aquí lo aplicamos en su versión más concreta: validator → implementer con techo.

```
implementer → validator
                ↓ (gap encontrado)
              [instrucciones de fix]
                ↓
              implementer (segunda vuelta)
                ↓
              validator
                ↓ (ok)
              siguiente paso
```

Cómo se escribe en el skill orquestador:

```markdown
## Paso 4: Validación con loop

Invoca al subagente `dotnet-reviewer`. Si devuelve hallazgos CRÍTICOS:
1. Aplica los fixes propuestos.
2. Vuelve a invocar al `dotnet-reviewer`.
3. Repite hasta que no haya hallazgos críticos o se alcancen 3 iteraciones.
4. Si tras 3 iteraciones siguen apareciendo hallazgos críticos, devuelve 
   al usuario con un resumen del problema. No sigas iterando ciegamente.
```

El límite de iteraciones es **importante**. Sin límite, un loop puede entrar en bucles infinitos donde el validator y el implementer no llegan a acuerdo. Pon siempre un techo.

**2. Tester que retiene control hasta que los tests pasan.**

Patrón parecido pero específico para tests. El subagente `test-generator` corre los tests, si fallan analiza si es problema del código o de los tests, ajusta lo que toque, y reintenta. Solo devuelve al principal cuando los tests pasan o cuando concluye que no van a pasar.

Esto ya lo vimos en 3.1 en el ejemplo del subagente `test-generator`: *"Máximo 3 iteraciones antes de devolver al principal"*. Esa es la pieza del loop.

**3. Plan re-validation.**

Cuando un planner produce un plan, antes de ejecutarlo, otro subagente lo valida (busca riesgos no contemplados, dependencias olvidadas, decisiones implícitas que no están justificadas). Si el validator lo rechaza, el planner reformula. Solo cuando el validator lo aprueba, se procede.

Esto es overkill para features pequeñas pero **brutal para features grandes** donde una mala decisión inicial cuesta horas o días.

### Loops a coste cero: cómo medirlos

Cada vuelta de un loop es una invocación más a un subagente, lo que significa más tokens y más tiempo. Conviene tener idea del coste antes de poner loops alegremente.

Heurística práctica:

- **Loops cortos (max 2-3 iteraciones)** son baratos y suelen rentar siempre. El coste extra es pequeño y la fiabilidad mejora mucho.
- **Loops largos (5+ iteraciones)** rara vez compensan. Si necesitas tantas vueltas, normalmente el problema es la calidad del subagente o del plan inicial, no la cantidad de iteraciones.

La regla: pon loops, pero **siempre con techo**. Y si te encuentras subiendo el techo porque el loop no converge, el problema está en otra parte.

---

## Context bank: artefactos durables como memoria compartida

Aquí entra una pieza que la documentación oficial menciona menos pero que la comunidad ha bautizado claramente: el **context bank**.

### El problema que resuelve

Cuando tienes varios subagentes trabajando en un workflow compuesto, hay información que necesitan compartir. El plan que generó el `planner`. Los hallazgos del `reviewer`. La lista de ficheros que el `tester` ha tocado. La decisión de diseño que el `feature-implementer` tomó al arrancar.

Hay dos formas de compartir esta información:

**Forma 1 (mala): pasarla por prompts.** Cuando invocas al siguiente subagente, le pasas todo el contexto previo en el prompt: *"Aquí está el plan. Aquí están los hallazgos del reviewer anterior. Aquí los ficheros tocados..."*. El problema: cada subagente recibe un prompt cada vez más grande, y cada vuelta se duplica trabajo (el subagente parsea de nuevo lo que ya estaba parseado antes).

**Forma 2 (buena): artefactos durables en el repo.** Cada subagente lee y escribe a ficheros markdown del repo que persisten durante el workflow. Un `PLAN.md`, un `REVIEW.md`, un `CHANGES.md`. Los subagentes leen lo que necesitan, escriben lo que producen, y la información sobrevive entre invocaciones.

Esto es el **context bank**: un conjunto de ficheros que actúan como memoria compartida entre subagentes y el orquestador.

### Estructura típica

Para el ejemplo del feature-implementer, el context bank vivirá en una carpeta temporal del workflow:

```
.claude/workflow-state/<feature-name>/
├── PLAN.md              # producido por feature-planner, leído por implementer
├── EXPLORATION.md       # producido por repo-explorer
├── CHANGES.md           # registrado por implementer al modificar ficheros
├── TESTS.md             # producido por test-generator
└── REVIEW.md            # producido por dotnet-reviewer en cada iteración
```

Cada subagente conoce su entrada y salida claras:

- `repo-explorer` → escribe a `EXPLORATION.md`
- `feature-planner` → lee `EXPLORATION.md`, escribe a `PLAN.md`
- agente principal → lee `PLAN.md`, ejecuta, registra en `CHANGES.md`
- `test-generator` → lee `CHANGES.md`, escribe `TESTS.md`
- `dotnet-reviewer` → lee `CHANGES.md` + `TESTS.md`, escribe `REVIEW.md`

Ventajas concretas:

- **Trazabilidad.** Si algo falla a mitad del workflow, los ficheros del context bank te dicen qué pasó hasta ese punto. Es tu log.
- **Recuperación.** Si la sesión muere a la mitad, puedes retomar el workflow porque el estado está persistido.
- **Loops baratos.** Cuando un validator devuelve al implementer, el implementer no necesita re-explicar todo: simplemente lee `REVIEW.md` y aplica los fixes.
- **Auditoría.** En equipos grandes, los ficheros del context bank son evidencia de qué se hizo, cómo, y con qué criterio.

### Limpieza

Una nota práctica: los ficheros del context bank son temporales del workflow, no parte del repo permanente. Convención típica: meterlos bajo `.claude/workflow-state/` y añadirlo a `.gitignore`. Cuando el workflow termina con éxito, el orquestador limpia (opcionalmente, lo deja por si quieres revisarlo).

Algunos equipos prefieren mantener los ficheros como **historial** — *"el PLAN.md de la feature de cancelación de pedidos vivirá en el repo bajo `docs/features/cancellation-orders/PLAN.md`"*. Esto es útil para documentación, no es obligatorio.

### Diferencia con `CLAUDE.md`

Importante: el context bank **no es** `CLAUDE.md`. `CLAUDE.md` es contexto persistente que aplica a cada sesión del repo. El context bank es contexto **del workflow concreto en curso**, vive solo durante el workflow, y los subagentes lo leen/escriben activamente.

---

## Paralelo vs serial: cuándo elegir cada patrón

Hasta aquí los flujos que hemos visto son **lineales**. El skill orquestador invoca al subagente A, espera, invoca al subagente B con el resultado de A, espera, invoca al C... cada paso depende del anterior.

Pero no todas las tareas son así. A veces tienes varias subtareas **independientes** entre sí, y ejecutarlas en serie es desperdiciar tiempo. El ejemplo más claro: validar un PR antes de subirlo. Quieres que un reviewer mire el código, que un tester corra los tests, y que otro subagente verifique las convenciones de naming. Ninguno de los tres depende de la salida de los demás — pueden ejecutarse a la vez y reducir a un tercio el tiempo total.

### Fan-out / fan-in

Cuando tienes subtareas independientes, el orquestador hace **fan-out**: lanza N invocaciones a subagentes en paralelo. Cada subagente trabaja en su contexto aislado. Cuando todos terminan, el orquestador hace **fan-in**: recoge los N resultados y los combina en una respuesta unificada.

Esto ya lo has visto en miniatura en 3.1 con el ejemplo del skill `pre-commit-check`:

> *Para validar el estado del repo antes de commit, invoca al subagente `dotnet-reviewer` y al subagente `test-runner` en paralelo. Combina sus resultados y devuelve un veredicto unificado.*

Es fan-out / fan-in con dos subagentes. Aplicado a workflows mayores, el patrón escala bien hasta 4-5 subagentes en paralelo. Más allá, el coste de coordinación se come el ahorro de tiempo.

### La decisión rápida

| Situación | Patrón |
|---|---|
| Subtarea B necesita el output de A | Serial |
| Subtareas independientes que pueden hacerse a la vez | Paralelo |
| Validación con varios ángulos (seguridad + estilo + tests) | Paralelo |
| Pipeline de transformación (explorar → planificar → ejecutar → validar) | Serial |
| Varios subagentes opinando sobre el mismo input | Paralelo (luego votación o síntesis) |

### El error típico

Lanzar paralelo cuando hay dependencias ocultas. *"Quiero que el tester y el reviewer corran en paralelo para ahorrar tiempo"*. Pero el reviewer necesita ver los tests también — o sus hallazgos van a estar incompletos. Resultado: ahorras los segundos del paralelo y te los gastas en una ronda extra cuando el reviewer reporta cosas que no podía saber sin el output del tester.

La regla rápida: si la salida de A condiciona cómo B trabaja, **es serial**. Si A y B hacen cosas que se combinan al final pero no se influyen entre sí, **es paralelo**.

En literatura formal este patrón aparece como **parallel workflow** y es uno de los más rentables cuando las dependencias están claras. Pero es también uno de los más fáciles de aplicar mal.

---

## Claude Code como MCP server

Una capa más arriba: ¿y si quieres que **otros agentes hablen con tu Claude Code**?

Esto es lo que permite el modo "Claude Code como MCP server". En vez de ser solo un cliente que consume MCP servers (Figma, GitHub, etc.), Claude Code se expone también como un MCP server al que otros pueden conectarse.

### Casos de uso

**1. Otro Claude Code que delega.** Tienes una sesión "principal" en tu portátil de trabajo y otra "auxiliar" para tareas paralelas (research, exploración, generación de docs). La auxiliar se conecta a la principal vía MCP cuando necesita contexto del repo activo.

**2. Integración con sistemas internos.** Tu plataforma interna del equipo necesita capacidades de Claude Code para procesar tareas asíncronas (generar documentación, validar PRs en bulk, etc.). Lanza llamadas MCP a una instancia de Claude Code corriendo en un servidor.

**3. Otro tipo de cliente MCP.** Hay clientes MCP más allá de Claude Code (Cursor, Codex CLI, alguna otra herramienta). Si tienes un Claude Code configurado con tu kit (skills propios, subagentes), puedes exponerlo como MCP a esos otros clientes y aprovechar el setup.

### Cómo se activa

En la configuración de Claude Code, indicas que el modo MCP server está activo y en qué puerto/socket. El otro lado se conecta como cliente MCP normal.

Detalles concretos varían según versión y plataforma. Lo importante para nosotros aquí es **saber que existe** y reconocer cuándo merece la pena plantearlo: integraciones serias entre sistemas, no para uso personal del día a día.

### Cuándo NO usar este patrón

Si lo único que quieres es delegar tareas dentro de una misma sesión, los **subagentes** son el camino correcto. Si quieres que dos sesiones de Claude Code colaboren más de cerca, los **Agent Teams** (siguiente sección) están pensados para eso.

Claude Code como MCP server es para cuando hay **integración con un tercero** — otro sistema, otra herramienta, otro agente que no es Claude Code mismo. Para todo lo demás, los mecanismos internos son más sencillos.

---

## Agent Teams: cuando los subagentes no bastan

Aquí entramos en territorio experimental. Hasta ahora, todo lo que hemos visto pasa **dentro de una sesión** de Claude Code: el agente principal, sus subagentes, los skills que invoca, los MCP que consulta. Una sesión es la unidad.

**Agent Teams** rompe esa unidad. Permite que **múltiples sesiones de Claude Code se comuniquen entre sí**, con un lead que las orquesta y mensajes directos entre ellas.

### Aclaración terminológica

Mucha gente lo llama "Swarm" o "Swarm Mode" porque la comunidad estuvo construyendo esto antes de que fuera oficial — herramientas como `claude-flow` u `oh-my-claude` ofrecían orquestación de varios agentes con persistencia. Cuando Anthropic lo lanzó nativamente a principios de 2026, lo llamó oficialmente **Agent Teams**. Los dos términos se usan, pero "Agent Teams" es lo correcto en la documentación oficial.

Si en otro material ves referencias a un "Swarm SDK", lo más probable es que se refiera a esto, o a alguna implementación de la comunidad que predató al feature oficial.

Y un último apunte de vocabulario: en literatura más formal sobre arquitecturas agentic — papers, whitepapers, blogs de research — este tipo de patrones (varias instancias coordinándose como pares, sin un supervisor único) se etiqueta como **collaborative** o **swarm architecture**. Si en algún momento lees discusiones sobre arquitectura distribuida de agentes, esos son los términos que verás.

### Cómo funciona Agent Teams

A grandes rasgos:

- **Un Team Lead** — sesión que recibe la petición inicial del usuario y la orquesta.
- **Teammates** — sesiones independientes de Claude Code que reciben sub-tareas del Lead.
- **Cada teammate tiene su propio contexto y su propio terminal.** No es un subagente dentro de la sesión del Lead; es otra sesión.
- **Comunicación directa** — los teammates pueden mandarse mensajes entre ellos, no solo reportar al Lead.
- **Backend tmux** — para visualizar lo que pasa, los teammates corren en panes separados de tmux.

Variables de entorno relevantes: `CLAUDE_CODE_TEAM_NAME`, `CLAUDE_CODE_AGENT_ID`, `CLAUDE_CODE_AGENT_TYPE`. Estado en `~/.claude/teams/<team>/`.

Activación:

```bash
export CLAUDE_CODE_EXPERIMENTAL_AGENT_TEAMS=1
```

O en el `settings.json` de tu user/proyecto.

### Cuándo merece la pena

La progresión de delegación en Claude Code, de menor a mayor autonomía:

1. **Solo session** — tú hablando con Claude Code. Control total.
2. **Skills** — encapsulas tareas reutilizables.
3. **Subagentes** — delegas tareas con su propio contexto.
4. **Agent Teams** — múltiples sesiones colaborando.

Cada paso te da más capacidad de cómputo paralelo a cambio de **menos control** y **más coste de tokens**. La pregunta no es *"¿cuánto puedo delegar?"* — es *"¿cuánto debo delegar para esta tarea concreta?"*.

Casos donde Agent Teams puede aportar:

- **Features muy grandes** divisibles en tracks paralelos. Backend + frontend + infraestructura, cada uno con su teammate.
- **QA swarms** — varios teammates probando la misma feature desde perspectivas distintas (funcional, performance, seguridad, accesibilidad).
- **Hipótesis competitivas en debugging** — cada teammate explora una hipótesis distinta, debaten, convergen.

Casos donde **NO** es necesario:

- **El 95% de las tareas del día a día.** Implementar un endpoint, escribir tests, refactorizar un módulo. Subagentes bastan.
- **Tareas donde el control humano importa.** Si vas a tener que revisar cada paso, mejor sesión solo o con subagentes — Agent Teams asume mucha autonomía.
- **Tu primera semana con Claude Code.** Antes de Agent Teams hay mucho recorrido en subagentes.

### El coste real

Algo que se menciona poco pero importa: **Agent Teams cuesta más tokens, no menos**. Cada teammate es una sesión de Claude Code con su propio contexto, sus propias decisiones, su propio razonamiento. Tres teammates = aproximadamente tres veces más tokens que una sesión sola.

Y eso suponiendo que la coordinación entre teammates funciona bien. Cuando no, hay tokens gastados en mensajes entre teammates que no llevan a ninguna parte, o en redoing trabajo que ya hizo otro.

La métrica honesta: para que Agent Teams compense, la tarea debe ser **suficientemente paralelizable como para que el ahorro de tiempo justifique el coste extra**. Si no, una sesión bien orquestada con subagentes da igual de buen resultado por una fracción del coste.

Y aquí va una cifra concreta para tener en mente, sacada del whitepaper de Anthropic sobre arquitecturas agentic: **los sistemas multi-agente consumen aproximadamente 10-15x más tokens que un agente solo**. La cifra incluye desde subagentes hasta Agent Teams. No es un detalle menor — es lo que diferencia *"voy a montar un harness con tres subagentes"* de *"voy a montar un sistema multi-agente porque mola"*. Si la tarea no justifica el incremento de orden de magnitud, no compensa. Empieza con uno o dos subagentes, mide tu factura con `/usage`, y escala solo cuando los números cuadren.

### El estado actual: experimental

Agent Teams sigue siendo **experimental** a fecha de este curso. Eso significa:

- API y comportamiento pueden cambiar entre versiones.
- Algunas integraciones (con MCP servers, con plugins) no están del todo pulidas.
- La documentación oficial es más escasa que la de subagentes o skills.
- La fiabilidad varía con la complejidad del workflow.

Mi recomendación honesta: para este curso, **basta con saber que existe y entender cuándo plantearlo**. No es algo que tu equipo vaya a poner en producción la semana que viene. Pero sí es donde va la herramienta a medio plazo.

---

## Cuándo usar qué: árbol de decisión

Resumiendo todas las capas en una decisión rápida:

| Situación | Solución |
|---|---|
| Tarea simple, dentro del flujo actual | Agente principal sin más |
| Tarea reutilizable con instrucciones fijas | Skill |
| Tarea reutilizable que requiere aislamiento de contexto | Skill con `context: fork` |
| Tarea con criterio propio o exploración pesada | Subagente |
| Workflow estandarizado que combina varias subtareas | Skill orquestador (initiator) que invoca subagentes |
| Workflow con validación que puede fallar | Skill orquestador + loop validator → implementer |
| Workflow donde varios subagentes comparten información | Skill orquestador + context bank en `.claude/workflow-state/` |
| Integración con sistemas externos | MCP server |
| Exposición de Claude Code a terceros | Claude Code como MCP server |
| Tarea muy grande con tracks paralelos genuinos | Agent Teams (si el coste compensa) |

La regla práctica que vale la pena memorizar: **empieza simple**. La mayoría de necesidades se cubren con skills + subagentes. El resto son casos especiales que se justifican explícitamente.

---

## Anti-patrones de orquestación

Los errores típicos cuando empiezas a montar workflows compuestos:

**Sobreingeniería desde el día uno.** Empezar con un skill que orquesta cinco subagentes y consulta tres MCP servers. Para una tarea que un agente principal con un buen `CLAUDE.md` resolvería sola. Si la tarea es simple, no la compliques. La orquestación es una herramienta, no un objetivo.

**Cadenas demasiado largas.** Skill que llama a subagente que llama a otro skill que llama a otro subagente que consulta un MCP. Cada eslabón es un punto donde puede romperse algo. Mantén las cadenas cortas — máximo 2-3 niveles de profundidad.

**Loops sin techo.** Un validator que devuelve al implementer infinitamente. Loops siempre con máximo de iteraciones. Si el loop no converge en 3 vueltas, el problema está en otra parte.

**Falta de observabilidad.** Cuando algo falla en un workflow compuesto, ¿cómo sabes en qué eslabón? Si no tienes forma de inspeccionar qué hizo cada subagente y qué devolvió, debugging es una pesadilla. Aquí es donde el context bank ayuda — los ficheros del workflow son tu log.

**Pasar contexto por prompt en vez de por context bank.** Cuando varios subagentes tienen que compartir información, usa ficheros markdown, no prompts gigantescos.

**Subagentes que se solapan.** Tienes un Reviewer y un Code Quality Checker y un Security Auditor que hacen cosas similares. La auto-delegación va a fallar porque sus descripciones colisionan. Mejor uno bien definido que tres con scope difuso.

**Pretender Agent Teams cuando subagentes basta.** *"Quiero que tres agentes trabajen en paralelo"*. ¿De verdad necesitas tres sesiones independientes que se comunican entre sí? La mayoría de las veces tres subagentes en una sesión te dan lo mismo por una fracción del coste.

**No iterar las orquestaciones.** Igual que con skills y subagentes individuales, los workflows compuestos no salen perfectos a la primera. Pruébalos en casos reales, observa qué falla, ajusta. La primera versión casi nunca es la final.

---

## Errores frecuentes con flujos compuestos

Lista práctica:

- **Lanzar Agent Teams sin haber dominado subagentes.** Es como pasar de bicicleta a moto sin pasar por scooter. La curva de aprendizaje es brutal.
- **Mezclar subagentes con `context: fork` skills sin criterio claro.** Decide para qué va a usar tu equipo cada mecanismo. Mantenerlo coherente facilita el mantenimiento.
- **Olvidar el coste extra.** Cada subagente o teammate es una factura adicional de tokens. Si tu equipo tiene presupuesto limitado, monitorízalo con `/usage` con frecuencia.
- **No documentar cómo está orquestado.** Tu workflow vive en la cabeza del que lo escribió. Documenta qué hace cada pieza y cómo se relacionan. Tres meses después tu yo del futuro lo agradece.
- **Pretender que la auto-delegación entre subagentes es perfecta.** No lo es. Si quieres garantizar que cierto subagente se invoca en cierto momento, **invócalo explícitamente** desde el skill orquestador.
- **No tener plan B cuando algo del workflow falla.** *"Si el subagente Tester falla, ¿qué pasa?"*. Si la respuesta es *"se rompe el workflow"*, eso es frágil. Diseña los workflows con tolerancia a fallos parciales y loops con techo.

---

## Antes de seguir

Tienes el modelo conceptual de la orquestación: el espectro completo va de un agente principal solitario, pasando por skills con `context: fork`, subagentes especializados, composiciones de skill+subagentes+MCP con loops y context bank, hasta Agent Teams para casos extremos.

Y tienes el frame que vertebra todo: **estás construyendo un agent harness**. El skill orquestador es el initiator. Los subagentes son los workers. El context bank es la memoria compartida. Los loops son lo que hace que el harness se autocorrija sin tu intervención. Falta una pieza.

La regla que vale la pena llevarse: **empieza simple, escala solo cuando el caso lo justifique**. El 80% de las necesidades de un equipo .NET / Angular se cubren con un buen `CLAUDE.md`, tres o cuatro skills y dos o tres subagentes. El resto es para casos especiales.

En **3.3** vamos a ver la última pieza del módulo: **hooks y channels**. La capa determinista del harness. Los mecanismos que hacen que todo lo anterior se ejecute **automáticamente, sin que tengas que pedirlo**. *"Después de cada commit, lanza el reviewer"*. *"Antes de cada PR, ejecuta el checklist"*. Esto es lo que cierra el harness — convierte la herramienta en algo que **trabaja contigo en background**, no solo cuando se lo pides.

Antes de pasar, una pregunta:

¿Qué parte de tu flujo actual sería bueno que se automatizara? *"Cada vez que hago commit, ejecutar el linter"*. *"Antes de subir un PR, validar que los tests pasan"*. *"Cuando un build falla, recibir notificación con análisis de la causa"*.

Si tienes uno de estos en mente, el módulo 3.3 va a tener nombre y apellidos para ti.
