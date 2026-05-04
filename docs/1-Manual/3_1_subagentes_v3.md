# 3.1 Subagentes integrados y personalizados

**Duración en clase:** 45 minutos · **Sesión 3, submódulo 1** · **Versión: v3**

> **Cambios v2 → v3**: añadida nota de vocabulario formal (specialist agents) al final de "Cuatro casos típicos". Sin cambios en estructura ni duración.

---

## El problema que resuelve un subagente

Llega un punto en una sesión larga de Claude Code donde notas algo raro. El agente repregunta cosas que sabía hace una hora. Sus respuestas se vuelven más vagas. Las decisiones que tomasteis al principio se difuminan. La causa es siempre la misma: la ventana de contexto se está llenando, y cada nueva exploración añade ruido al razonamiento principal.

Hay un caso típico que dispara este problema: alguien está implementando una feature compleja y, a mitad, quiere entender cómo está hecho un módulo del repo que no ha tocado todavía. El agente principal se pone a leer ficheros, explorar dependencias, contar imports. Cuando termina la exploración, ha cargado al contexto principal **30 ficheros que no son relevantes para el código que está escribiendo**. La conversación queda contaminada con esa exploración, y los siguientes 60 minutos de trabajo arrastran ese peso.

Compactar con `/compact` ayuda, pero es un parche. La solución estructural es no contaminar el contexto principal con tareas que tienen su propia naturaleza. Y esa solución son los subagentes.

Un subagente es **otro Claude** dentro de tu sesión, con su propio contexto, su propio razonamiento, y un scope limitado a la tarea que le delegues. Hace su trabajo, te devuelve un resultado, y se va sin dejar rastro en tu sesión principal. La exploración del módulo del ejemplo de antes, hecha por un subagente, te devuelve *"el módulo Foo hace X, expone Y, depende de Z"* sin que esos 30 ficheros aparezcan jamás en tu contexto principal.

Este es el problema que resuelven, y lo importante de esta sesión es entender **cuándo merece la pena delegar y cuándo no**.

---

## Encuadre del módulo: estamos construyendo un agent harness

Antes de meternos en subagentes, conviene poner el frame que vertebra los tres submódulos del módulo 3. Vas a escuchar el término **"agent harness"** mucho a partir de ahora — en blogs de Anthropic, en threads técnicos, en discusiones de la comunidad. Conviene que sepas qué significa.

La fórmula que circula es esta:

> **agent = model + harness**

Un modelo en bruto no es un agente. Lo es solo cuando le rodeas de tools, contexto, hooks y feedback loops. Todo eso, junto, es **el harness**. Claude Code, Cursor, Codex — todos son harnesses construidos sobre el modelo. La performance que sientes al usar Claude Code viene tanto del harness como del modelo subyacente.

Y aquí está la parte interesante: **cuando personalizas Claude Code con skills, subagentes y hooks, estás construyendo tu propio harness encima del de Anthropic**. El módulo 2 cubría una pieza (skills). El módulo 3 cubre las otras tres principales:

- **3.1 (este apartado)** — subagentes como *workers* del harness.
- **3.2** — la *orquestación* que combina las piezas en flujos coherentes.
- **3.3** — los *hooks* como capa determinista del harness.

Esta es la idea que conviene tener en mente: no estás aprendiendo features sueltas; estás aprendiendo a montar un harness que conoce a tu equipo. Cuando llegues al portal de recursos del curso encontrarás un cheatsheet visual de este patrón al que conviene volver cuando dudes dónde encaja una pieza.

---

## El mental model: CLAUDE.md, skills, subagentes

Las tres piezas tienen propósitos relacionados pero distintos:

- **`CLAUDE.md`** — contexto **persistente** del proyecto. Lo que el agente necesita saber siempre que toques este repo. Cargado al arrancar, presente en cada interacción.
- **Skills** — capacidades **bajo demanda**. Playbooks reutilizables para tareas concretas. Cargados cuando la descripción coincide con lo que pides; ejecutados dentro del contexto principal.
- **Subagentes** — tareas **aisladas**. Otro agente con su propio contexto que ejecuta una tarea y te devuelve solo el resultado. La conversación interna del subagente no contamina la tuya.

Una analogía que se entiende bien: imagina que estás trabajando en una oficina. `CLAUDE.md` es el manual del empleado que tienes pegado en la pared y consultas cada día. Los skills son las macros y plantillas que tienes en tu cajón para tareas frecuentes. Los subagentes son **compañeros con los que delegas** — les pides algo, lo hacen en su mesa, y te traen el resultado terminado, sin que tú tengas que ver el papel desordenado de su escritorio.

La pregunta práctica que tienes que aprender a responder en este apartado: cuándo **lo hago yo con un skill** y cuándo **se lo paso a un subagente**.

---

## Los subagentes integrados

Claude Code trae tres subagentes built-in que están disponibles en cada sesión sin que tengas que crearlos. Conviene conocerlos porque a veces el agente principal los activa solo, y otras veces puedes invocarlos explícitamente.

### Explore

Subagente especializado en **lectura y exploración**. Solo lee, no modifica. Por defecto se ejecuta en Haiku — más rápido y barato — porque la exploración es una tarea donde la velocidad importa más que el razonamiento profundo.

¿Cuándo se activa? Cuando una tarea principal requiere entender una zona del repo que no es la que estás tocando. *"Implementa X en el módulo A, pero antes asegúrate de no romper la integración con el módulo B"*. El agente principal le pide a Explore que estudie el módulo B, vuelve con un resumen, y el principal procede sin haber cargado todo B en su contexto.

Casos típicos donde Explore brilla:

- **Análisis de un repo grande que no has tocado.** Le pides al agente que entienda la estructura general; lo hace con Explore por debajo.
- **Búsqueda de patrones.** *"Busca todos los sitios donde se usa la inyección de IOrderService"*. Una búsqueda Grep amplia, pero analizada en su propio contexto.
- **Revisión cruzada.** *"Antes de tocar este servicio, mira cómo se usa en el resto del proyecto"*.

### Plan

Subagente que **planifica antes de actuar**. Es lo que se ejecuta cuando lanzas `/plan` o cuando la tarea es lo suficientemente compleja como para que el agente principal decida que merece la pena planificar antes.

Plan recopila contexto, razona sobre la mejor forma de abordar la tarea, y presenta un plan paso a paso. **No actúa**. Devuelve el plan al agente principal (o a ti, si lo invocaste con `/plan`) y se queda esperando confirmación.

¿Cuándo merece la pena? Cuando la tarea va a tocar más de tres ficheros, cuando hay decisiones de diseño implícitas, o cuando un error a mitad sería costoso de revertir.

### General-purpose

El comodín. Subagente que puede tanto explorar como modificar. Lo usa el agente principal cuando una tarea requiere ambas cosas pero quiere mantener su propio contexto limpio.

Casos: refactor de un módulo aislado donde el resultado vuelve al principal como *"hecho"*, generación de un conjunto de tests que el principal solo necesita saber que existen, etc.

### Cómo se invocan

La mayoría del tiempo, el agente principal **decide automáticamente** cuándo usar cada uno. Cuando ves en la salida un mensaje tipo *"Launching Explore agent to investigate..."* o *"Plan agent generating strategy..."*, ahí está pasando.

Si quieres invocarlos explícitamente, puedes pedirlo:

```
> Usa el subagente Explore para mapear la estructura del módulo Orders
> Lanza Plan para diseñar el refactor de la capa de validación
```

O con el comando `/plan` directamente para activar planificación.

### Limitación de la auto-delegación

Una observación honesta: la auto-delegación a subagentes integrados **no es perfecta**. Hay tareas donde merecería la pena que el agente principal delegara a Explore y no lo hace. Hay otras donde delega cuando podría haber resuelto solo. Si notas que tu sesión principal se está cargando con exploraciones que deberían haberse aislado, **invócalos explícitamente**.

---

## Crear un subagente custom

Aquí empieza la parte interesante. Más allá de los integrados, puedes definir subagentes propios para tareas recurrentes en tu equipo.

### Estructura

Un subagente es un fichero Markdown con frontmatter YAML, igual que un skill. Pero vive en una carpeta distinta:

- **Proyecto** — `.claude/agents/<nombre>.md`. Va a git, lo comparte el equipo.
- **Personal** — `~/.claude/agents/<nombre>.md`. Tuyo, viaja contigo.
- **Plugin** — empaquetado dentro de un plugin distribuible.

Cuando hay nombres duplicados entre scopes, gana el de más prioridad: project > user > plugin. Igual que con skills.

### Anatomía de un subagente

```markdown
---
name: code-reviewer
description: Revisa código recién modificado buscando bugs, problemas de seguridad y violaciones de convenciones del equipo. Usar inmediatamente después de cambios significativos en código.
tools: Read, Grep, Glob, Bash(git diff *)
model: sonnet
---

Eres un revisor de código senior con experiencia en C# / .NET y Angular. 

Cuando seas invocado:

1. Ejecuta `git diff` para identificar los ficheros modificados.
2. Para cada fichero modificado, examina los cambios con foco en:
   - Patrones async/await incorrectos
   - Naming que no respeta la convención del equipo
   - Manejo de errores ausente o débil
   - Duplicación con código existente
   - Test coverage insuficiente
3. Devuelve los hallazgos como una lista priorizada (alta / media / baja) con:
   - Fichero y línea afectada
   - Naturaleza del problema
   - Sugerencia concreta de fix

Sé directo y técnico. No edulcores los problemas. Si no hay problemas significativos, dilo.
```

Tres bloques importantes:

**Frontmatter (entre `---`):**
- `name` — nombre del subagente (kebab-case).
- `description` — qué hace y cuándo activarlo. Igual que con skills, este campo es crítico para la auto-delegación.
- `tools` — herramientas permitidas. Si lo omites, hereda todas las de la sesión principal. **Es buena práctica restringir.**
- `model` — modelo concreto. `sonnet`, `opus`, `haiku`, o `inherit` para usar el mismo que el principal.

**Body (después del frontmatter):**
El system prompt del subagente. Define su rol, su personalidad, cómo aborda las tareas, qué formato de salida usar. Aquí es donde se diferencia de un skill — un skill da instrucciones de cómo hacer una tarea concreta; un subagente define un rol completo con criterio.

### El comando `/agents`

La forma más cómoda de crear y gestionar subagentes es la UI integrada:

```
> /agents
```

Te abre un menú interactivo:

```
/agents

❯ Create new agent
  List existing agents
  Edit agent
  Delete agent
  ...

Built-in (always available):
  - Explore
  - Plan
  - general-purpose
```

Al crear uno nuevo, te pregunta primero el scope (proyecto vs personal), luego si quieres generarlo con ayuda de Claude o escribirlo manualmente. Mi recomendación: **deja que Claude lo genere primero y después lo ajustas**. Genera un draft decente con un par de frases tuyas, y editar es mucho más rápido que partir de cero.

Para listar todos los subagentes disponibles desde fuera de una sesión:

```bash
claude agents
```

Te muestra los agentes agrupados por scope, indicando cuáles están sobreescritos por niveles de mayor prioridad.

### Carga y refresco

Los subagentes se **cargan al arrancar la sesión**. Si creas o modificas uno mientras tienes una sesión abierta, **reinicia la sesión** o usa `/agents` para forzar la recarga. Si no, los cambios no surten efecto.

Esto coge a alguno por sorpresa al principio: editas el fichero, lanzas la tarea, y la conducta no cambia. Es porque la sesión sigue con la versión anterior cargada.

---

## Patrones de delegación: cuándo subagente vs principal

La pregunta más importante de este apartado. La regla práctica resumida:

**Delega a subagente cuando la tarea cumpla al menos una de estas:**

1. **Requiere su propio contexto significativo.** La exploración de un módulo grande que no quieres que pese en tu sesión principal. La revisión de un PR completo que va a leer muchos ficheros.
2. **Tiene su propio razonamiento que no quieres que influya en el principal.** Un debugging exhaustivo de una hipótesis donde el principal sigue trabajando en otra cosa. Un análisis de seguridad que produce su propio juicio sin contaminar las decisiones de implementación.
3. **Va a ejecutarse en paralelo o en background.** Tareas que no necesitan tu atención inmediata pero quieres que se completen.
4. **Necesita un modelo distinto del principal.** Una tarea simple que merece Haiku mientras el principal está en Opus, o al revés.
5. **Tiene su propio rol/personalidad.** Un revisor crítico, un planificador estratégico, un debugger sistemático. Roles que se benefician de tener su propio system prompt.

**No delegues cuando:**

1. **La tarea está en el flujo natural del trabajo actual.** Si estás implementando algo y necesitas leer un fichero adyacente, hazlo directo. No abras un subagente para algo que es una operación simple.
2. **Una sola tarea pequeña.** El overhead de invocar un subagente no compensa para algo de cinco minutos.
3. **Necesitas que el resultado se integre tightly con el contexto principal.** Si lo que el subagente produce va a ser inmediatamente modificado por el principal, los pasos extra (devolver, integrar, modificar) son fricción innecesaria.
4. **No tienes claro el scope de la delegación.** Si no puedes describir en una frase qué quieres que el subagente haga y devuelva, mejor que no delegues — vas a estar haciendo de orquestador y el resultado va a ser peor.

### El número práctico: 3-4 subagentes (con matiz importante)

Algo que la gente experimentada suele decir: **el límite práctico para uso general está en 3-4 subagentes activos**. Más allá, la productividad **baja**.

¿Por qué? Porque tener muchos subagentes especializados implica:

- Decidir cada vez cuál delegar (coste mental).
- Mantener el sistema de delegación coherente (todos sus prompts, sus tools, sus modelos).
- Recordar qué hace cada uno.

La curva de utilidad sube hasta un punto y baja. La gente que rinde más con subagentes para tareas de desarrollo cotidiano tiene **pocos pero bien afinados**. Un Explorer, un Reviewer, un Tester, y quizá un Planner. Cuatro como mucho. Si te ves teniendo siete subagentes diferentes para el día a día, probablemente algunos deberían ser skills y otros deberían fusionarse.

**El matiz importante: harness especializados pueden tener muchos más**.

La regla de 3-4 aplica a **uso general** — el dev que cada día implementa features, hace bugfix, revisa PRs. Pero hay otro tipo de uso, los **harness verticales especializados**, donde la cuenta es completamente distinta.

Hay equipos que tienen workflows con 10-15 subagentes para dominios muy concretos: pipelines de research académico (un subagente que genera ideas, otro que rastrea literatura, otro que escribe pruebas matemáticas, otro que implementa, otro que valida experimentos, dos reviewers, un revisor de referencias, un generador de slides...), pipelines de auditoría de seguridad, pipelines de análisis legal de contratos. Y funcionan bien.

¿Por qué? Porque están **estructurados como un harness con flujo definido**. No son 15 subagentes sueltos a los que el principal decide cuándo invocar — son una pipeline donde un orquestador sabe el orden, cada subagente tiene su entrada y salida claras, y hay loops de validación entre ellos. La complejidad la absorbe la estructura, no el dev.

Para el módulo 3.2 esta distinción importa: **subagentes sueltos = 3-4 como mucho. Harness estructurado = los que hagan falta para cubrir el flujo**. Lo veremos en detalle en el siguiente apartado.

---

## Cuatro casos típicos

Los subagentes que más equipos terminan teniendo. No los necesitas todos — usa los que aplican a tu flujo.

### El Explorer

Subagente especializado en analizar zonas desconocidas del repo. Read-only, modelo rápido.

```markdown
---
name: repo-explorer
description: Explora y mapea zonas del repositorio para devolver una vista resumida. Usar cuando se necesita entender un módulo, una carpeta, o una funcionalidad sin haberla tocado antes.
tools: Read, Grep, Glob
model: haiku
---

Eres un explorador del repositorio. Tu trabajo es entender una zona del código y devolver un resumen estructurado.

Cuando seas invocado con un objetivo de exploración:

1. Identifica los ficheros principales del área (entry points, definiciones públicas).
2. Mapea las dependencias y exports.
3. Identifica los patrones recurrentes (cómo se estructura la zona).
4. Encuentra los puntos de extensión o variación.

Devuelve un resumen en formato markdown con cuatro secciones:

- **Estructura general** — carpetas, ficheros principales.
- **Puntos de entrada** — qué se expone, cómo se usa desde fuera.
- **Patrones internos** — cómo está organizado por dentro.
- **Notas de cuidado** — código que parece frágil, comentarios TODO/FIXME, deuda visible.

Sé conciso. El destinatario es el agente principal, que necesita información estructurada, no narrativa.
```

Lo invocas explícitamente cuando llegas a un repo nuevo o cuando vas a tocar una zona donde no has trabajado.

### El Reviewer

Code review aislado. Lee, juzga, devuelve hallazgos. No modifica.

```markdown
---
name: dotnet-reviewer
description: Revisa código C# / .NET buscando problemas de naming, async patterns, manejo de errores y violaciones de convenciones del equipo. Usar después de cambios significativos en código antes de un commit.
tools: Read, Grep, Glob, Bash(git diff *), Bash(git log *)
model: sonnet
---

Eres un revisor senior de código .NET. Tu trabajo es identificar problemas en código recién escrito y proponer fixes concretos.

Foco específico:
- Async/await: nunca .Result ni .Wait(), nunca async void salvo event handlers
- Naming: PascalCase clases/métodos, _camelCase campos privados
- Manejo de errores: Result<T> en dominio, ProblemDetails en API
- Convenciones del equipo recogidas en CLAUDE.md

Cuando te invoquen:

1. Identifica los ficheros modificados con `git diff`.
2. Examina los cambios línea a línea.
3. Devuelve hallazgos clasificados como CRÍTICO / IMPORTANTE / SUGERENCIA.
4. Para cada hallazgo: fichero, línea, problema, fix concreto.

Sé directo. No suavices los problemas críticos. Si todo está limpio, dilo en una línea y termina.
```

Útil antes de commit. Encadena bien con un hook pre-commit (lo veremos en 3.3).

### El Tester

Genera tests sin contaminar el contexto del principal.

```markdown
---
name: test-generator
description: Genera tests unitarios y de integración para código .NET siguiendo el patrón del equipo (xUnit + NSubstitute). Usar cuando se necesite generar suite de tests para un componente, servicio o controller existente.
tools: Read, Grep, Write, Edit, Bash(dotnet test *)
model: sonnet
---

Eres un especialista en testing .NET. Tu trabajo es producir tests de calidad para código existente.

Patrón estándar del equipo:
- Framework: xUnit
- Mocking: NSubstitute (nunca Moq)
- Estructura: Arrange-Act-Assert con comentarios explícitos
- Naming de tests: MétodoBajoTest_Escenario_ResultadoEsperado

Cuando te invoquen:

1. Lee el código a testear.
2. Identifica las dependencias que necesitan mock.
3. Identifica los casos a cubrir (camino feliz, errores, edge cases).
4. Genera los tests siguiendo el patrón.
5. Ejecuta `dotnet test` para verificar que pasan.
6. Si fallan, analiza, ajusta y reintenta. Máximo 3 iteraciones antes de devolver al principal.

Cobertura objetivo: caminos críticos cubiertos, no obsesión con porcentajes. Devuelve resumen con: tests creados, cobertura conceptual, casos no cubiertos y por qué.
```

### El Planner

Para tareas grandes donde merece la pena planificar antes.

```markdown
---
name: feature-planner
description: Planifica la implementación de features grandes desglosándolas en pasos concretos antes de empezar a codificar. Usar cuando una feature toca más de tres ficheros o hay decisiones de diseño implícitas.
tools: Read, Grep, Glob
model: opus
---

Eres un planificador estratégico de features. Tu trabajo es desglosar una feature compleja en un plan paso a paso antes de cualquier escritura.

Cuando te invoquen con un objetivo de feature:

1. Estudia el código relevante y entiende el estado actual.
2. Identifica los ficheros que van a tocarse.
3. Detecta decisiones de diseño implícitas (¿hay varias formas de hacer esto?).
4. Devuelve un plan estructurado:

   - **Resumen** — una frase del objetivo
   - **Decisiones de diseño** — alternativas consideradas y justificación de la elegida
   - **Pasos** — ordenados, cada uno con: qué tocas, qué creas, qué tests hacen falta
   - **Riesgos** — qué puede salir mal o complicarse
   - **Validación** — cómo sabremos que está bien al final

Termina pidiendo confirmación al agente principal antes de pasar a ejecución.
```

Notar que aquí uso `model: opus` — para planificación de features no triviales, el coste extra de Opus se justifica.

### Una nota de vocabulario

Si te encuentras leyendo whitepapers de arquitectura agentic — o algún post técnico de Anthropic más teórico — vas a ver este tipo de roles con otro nombre: **specialist agents**. Es lo mismo que estás viendo aquí: un agente con un dominio acotado dentro de un sistema mayor. La etiqueta cambia según dónde lo leas, el concepto no.

---

## Combinación con skills: cuándo cada uno

La pregunta natural cuando empiezas a tener subagentes y skills: ¿cuándo uso uno y cuándo el otro?

### Skills

Para **tareas con playbook fijo** que aplica a casos similares. Generación de un componente, code review con checklist, documentación con formato concreto. El skill define **cómo** se hace la tarea; la ejecución la hace el agente principal en su contexto.

### Subagentes

Para **tareas que se benefician de aislamiento**. Exploración de zonas del repo, revisiones que requieren juicio independiente, análisis pesados que el principal no quiere cargar. El subagente tiene su **propio contexto** y devuelve solo el resultado.

### La combinación: skill que invoca subagente

Hay un patrón potente que aparece cuando empiezas a tenerlo todo: un skill cuyo cuerpo le pide al agente que invoque a un subagente concreto.

Ejemplo: skill `pre-commit-check` cuyo `SKILL.md` dice:

> *Para validar el estado del repo antes de commit, invoca al subagente `dotnet-reviewer` y al subagente `test-runner` en paralelo. Combina sus resultados y devuelve un veredicto unificado.*

Aquí el skill orquesta, los subagentes ejecutan en aislamiento. Lo mejor de los dos mundos. Y este patrón es la base de los harness compuestos que veremos en 3.2.

### Decisión rápida

| Situación | Solución |
|---|---|
| Tarea con instrucciones reutilizables, encaja en el flujo principal | Skill |
| Tarea que requiere contexto propio o juicio independiente | Subagente |
| Tarea con instrucciones reutilizables que requiere aislamiento | Skill que invoca subagente |
| Convención que aplica a todo el repo | Va en `CLAUDE.md`, no es skill ni subagente |

---

## Anti-patrones de subagentes

Los errores más comunes al crear subagentes:

**Naming genérico.** `frontend-engineer`, `backend-helper`, `dev-assistant`. Suenan bien pero la auto-delegación falla porque la descripción es difusa. Mejor: nombres orientados a job — `repo-explorer`, `test-runner`, `pr-reviewer`, `docs-researcher`. Activan mejor.

**Description que es persona, no workflow.** *"Eres un experto desarrollador full-stack..."*. Eso describe al agente, no la tarea. La descripción es para que el principal decida si delegarle algo. Mejor: *"Genera tests xUnit para servicios .NET. Usar después de crear o modificar servicios"*.

**`tools` sin restringir.** Si omites `tools`, el subagente hereda todo de la sesión. Para subagentes que solo deberían leer, esto es un agujero. Buenas prácticas: explicita las herramientas mínimas necesarias (least privilege).

**Subagente que debería ser skill.** Si la tarea está perfectamente bien dentro del flujo principal, no necesita aislamiento. Skill > subagente para esto.

**Demasiados subagentes para uso general.** Más allá de 3-4 activos para tu día a día, la productividad baja. Si te ves teniendo 8 subagentes y no es un harness vertical estructurado, probablemente algunos sobran o deberían fusionarse.

**No iterar la descripción.** Igual que con skills, la primera descripción casi nunca es la final. Lánzala, ve si se activa cuando esperas, ajusta.

**Modelo mal elegido.** Subagente de exploración corriendo en Opus = caro y sin necesidad. Subagente de planificación en Haiku = falta de profundidad. Asocia modelo a tipo de tarea: Haiku para tareas mecánicas, Sonnet para la mayoría, Opus para razonamiento complejo.

**Subagente que pide aprobaciones constantes.** Si el subagente está pidiendo permisos cada dos por tres, su `tools` está mal acotado o su rol no está bien definido. Revisa.

---

## Errores frecuentes con tus primeros subagentes

Lista de problemas típicos al empezar:

- **Crear el subagente y no reiniciar la sesión.** Los subagentes se cargan al arrancar. Modificación = reinicio o `/agents` para refrescar.
- **Empezar con descripción difusa.** *"Ayuda con código"* no activa nada. Sé específico, con verbos y casos.
- **No probar la auto-delegación.** Después de crearlo, lánzale tareas que esperarías que activaran al subagente. Si no se activa, el problema está en la descripción casi siempre.
- **Subagentes que escriben en sitios donde no esperas.** Si das `Write` o `Edit` sin restricción, el subagente puede modificar ficheros fuera de su scope. Restringe.
- **Encadenar varios subagentes sin orquestación clara.** Subagente A llama a B, B llama a C. Si la cadena se rompe, debugging difícil. Empieza simple — uno o dos subagentes — antes de orquestaciones complejas.
- **Olvidar que cada subagente arranca con contexto vacío.** El subagente no sabe lo que tú y el principal lleváis dos horas hablando. Si le pides que haga algo que requiere ese contexto, dáselo en la invocación.
- **No usar `/agents` para mantener orden.** El comando es la forma fácil de ver, listar, editar y borrar subagentes. Acuérdate de él.
- **Pretender que un subagente recuerda interacciones anteriores entre sesiones.** Cada invocación es nueva. La memoria persistente vive en `CLAUDE.md` (proyecto) o en artefactos durables que el subagente produce.

---

## Antes de seguir

Tienes el modelo conceptual de subagentes: qué problema resuelven (aislamiento de contexto), cómo se diferencian de skills (rol con criterio propio vs playbook reutilizable), los tres built-in (Explore, Plan, general-purpose), cómo crear los tuyos (`.claude/agents/<nombre>.md`), y cuándo merece la pena delegar frente a hacerlo en el principal.

Y tienes el frame: **estamos construyendo un agent harness**. Subagentes son los workers. Skills son los playbooks. Faltan dos piezas más, que vienen ahora.

En **3.2** vamos a ver cómo se orquestan las piezas. Skills que invocan subagentes. Loops de retroalimentación entre validators y workers. Context bank — artefactos durables que sirven de memoria compartida entre subagentes. El propio Claude Code expuesto como MCP a otros agentes. Y un vistazo a Agent Teams (lo que la comunidad llama "Swarm") para sesiones multi-agente coordinadas.

En **3.3** entramos en la pieza determinista del harness: **hooks y channels**. Lo que hace que ciertas cosas pasen siempre, sin que tengas que pedirlo cada vez. *"Después de cada commit, lanza el reviewer"*. *"Antes de cada PR, ejecuta el checklist"*. Esto cierra el harness.

Antes de pasar, una pregunta:

¿Qué tarea de tu día a día sería el primer candidato a subagente para tu equipo? Pensando en las heurísticas: ¿requiere su propio contexto?, ¿tiene un rol distinto del trabajo principal?, ¿se beneficia de tener su propio razonamiento?

Si la respuesta es *"un revisor de PRs antes de subirlos"*, tienes el caso clásico — y es el que más rentabilidad da en equipos que ya hacen code review humano. Un subagente reviewer no sustituye al humano, pero le ahorra el primer pase mecánico y le permite centrarse en lo que requiere criterio real.

Si la respuesta es *"un explorador del repo para juniors que se incorporan"*, también clásico. Un Explorer bien afinado es la diferencia entre un junior que tarda dos semanas en ubicarse y uno que en dos días ya entiende la estructura general.

Tener uno de estos dos en mente antes de la siguiente sesión hace que el módulo 3 entero gane sentido práctico.
