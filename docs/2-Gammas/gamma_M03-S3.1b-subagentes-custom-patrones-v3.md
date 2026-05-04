> **Versión:** v3 | **Módulo:** 3 | **Sub:** 3.1b | **Slides:** 32 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M03-S3.1b-subagentes-custom-patrones-v3.md`

# Submódulo 3.1b — Subagentes custom y patrones

## Slide 1 — Portada
**Módulo 3 · Submódulo 3.1 · Parte B**
Subagentes custom y patrones de delegación
Cómo crearlos, cuándo usarlos, los cuatro casos típicos

---

## Slide 2 — Dónde estamos

En 3.1a vimos qué problema resuelven los subagentes, el frame de **agent harness**, la diferencia con CLAUDE.md y skills, y los tres built-in (Explore, Plan, general-purpose).

Ahora viene la parte donde construyes los tuyos:

```
1. Crear un subagente custom
2. Patrones de delegación (cuándo SÍ, cuándo NO)
3. Cuatro casos típicos con código
4. Combinación con skills
5. Anti-patrones y errores frecuentes
```

---

## Slide 3 — Estructura del subagente custom

Un subagente es **un fichero Markdown con frontmatter YAML**, igual que un skill. Pero vive en una carpeta distinta:

```
PROYECTO
└── .claude/agents/<nombre>.md
    ├── Va a git
    └── Lo comparte el equipo

PERSONAL
└── ~/.claude/agents/<nombre>.md
    ├── Tuyo
    └── Viaja contigo

PLUGIN
└── Empaquetado dentro de un plugin distribuible
```

> Cuando hay nombres duplicados entre scopes:
> gana el de más prioridad → **project > user > plugin**.
>
> Igual que con skills.

---

## Slide 4 — Anatomía de un subagente: frontmatter

```markdown
---
name: code-reviewer
description: Revisa código recién modificado buscando bugs, problemas
  de seguridad y violaciones de convenciones del equipo. Usar inmediatamente
  después de cambios significativos en código.
tools: Read, Grep, Glob, Bash(git diff *)
model: sonnet
---
```

**Cuatro campos:**

```
name
└── Nombre del subagente. Kebab-case.

description
└── Qué hace y cuándo activarlo.
    El campo CRÍTICO para auto-delegación.
    Igual que con skills.

tools
└── Herramientas permitidas.
    Si lo omites, hereda todas las de la sesión principal.
    BUENA PRÁCTICA: restringir.

model
└── Modelo concreto: sonnet, opus, haiku, o "inherit"
    para usar el mismo que el principal.
```

---

## Slide 5 — Anatomía de un subagente: body

```markdown
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

Sé directo y técnico. No edulcores los problemas.
Si no hay problemas significativos, dilo.
```

> El **body** es el system prompt del subagente.
>
> Define su rol, su personalidad,
> cómo aborda las tareas,
> qué formato de salida usar.

---

## Slide 6 — Skill vs subagente en una frase

```
Un SKILL
└── da instrucciones de cómo hacer
    una tarea concreta.

Un SUBAGENTE
└── define un ROL COMPLETO con criterio.
```

> Esa es la diferencia conceptual clave.
>
> Lo que decides al crear uno u otro
> es si quieres "una tarea hecha"
> o "alguien que se encargue de cosas como esta".

---

## Slide 7 — El comando /agents

La forma más cómoda de crear y gestionar subagentes:

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

> Al crear uno nuevo, te pregunta primero el scope
> (proyecto vs personal),
> luego si quieres generarlo con ayuda de Claude
> o escribirlo manualmente.

---

## Slide 8 — Recomendación: deja que Claude lo genere primero

```
Mi recomendación:

deja que Claude lo genere primero
y después lo ajustas.
```

```
Genera un draft decente
con un par de frases tuyas.

Y editar es mucho más rápido
que partir de cero.
```

**Para listar todos los subagentes desde fuera de una sesión:**

```bash
claude agents
```

> Te muestra los agentes agrupados por scope,
> indicando cuáles están sobreescritos
> por niveles de mayor prioridad.

---

## Slide 9 — Carga y refresco

```
Los subagentes se CARGAN AL ARRANCAR LA SESIÓN.
```

```
Si creas o modificas uno
mientras tienes una sesión abierta:
└── REINICIA LA SESIÓN
    o usa /agents para forzar la recarga.
```

> Esto coge a alguno por sorpresa al principio.
>
> Editas el fichero, lanzas la tarea,
> y la conducta no cambia.
>
> Es porque la sesión sigue
> con la versión anterior cargada.

---

## Slide 10 — Patrones de delegación

```
La pregunta más importante de este apartado:

  ¿cuándo delego a un subagente
   y cuándo lo hago en el principal?
```

**La regla práctica resumida en cinco razones SÍ y cuatro razones NO. Las vemos.**

---

## Slide 11 — Delega a subagente cuando...

**Cumpla al menos UNA de estas:**

```
1. REQUIERE SU PROPIO CONTEXTO SIGNIFICATIVO
   La exploración de un módulo grande que no quieres
   que pese en tu sesión principal.
   La revisión de un PR completo que va a leer muchos ficheros.

2. TIENE SU PROPIO RAZONAMIENTO
   QUE NO QUIERES QUE INFLUYA EN EL PRINCIPAL
   Un debugging exhaustivo de una hipótesis donde el principal
   sigue trabajando en otra cosa.
   Un análisis de seguridad que produce su propio juicio
   sin contaminar las decisiones de implementación.

3. VA A EJECUTARSE EN PARALELO O EN BACKGROUND
   Tareas que no necesitan tu atención inmediata
   pero quieres que se completen.

4. NECESITA UN MODELO DISTINTO DEL PRINCIPAL
   Una tarea simple que merece Haiku mientras
   el principal está en Opus, o al revés.

5. TIENE SU PROPIO ROL/PERSONALIDAD
   Un revisor crítico, un planificador estratégico,
   un debugger sistemático.
```

---

## Slide 12 — NO delegues cuando...

```
1. LA TAREA ESTÁ EN EL FLUJO NATURAL DEL TRABAJO ACTUAL
   Si estás implementando algo y necesitas leer un fichero adyacente,
   hazlo directo. NO abras un subagente para algo
   que es una operación simple.

2. UNA SOLA TAREA PEQUEÑA
   El overhead de invocar un subagente
   no compensa para algo de cinco minutos.

3. NECESITAS QUE EL RESULTADO SE INTEGRE TIGHTLY
   CON EL CONTEXTO PRINCIPAL
   Si lo que el subagente produce va a ser
   inmediatamente modificado por el principal,
   los pasos extra (devolver, integrar, modificar)
   son fricción innecesaria.

4. NO TIENES CLARO EL SCOPE DE LA DELEGACIÓN
   Si no puedes describir en una frase
   qué quieres que el subagente haga y devuelva
   └── mejor que NO delegues.
       Vas a estar haciendo de orquestador
       y el resultado va a ser peor.
```

---

## Slide 13 — El número práctico: 3-4 subagentes

```
Algo que la gente experimentada suele decir:

el límite práctico para uso general
está en 3-4 subagentes activos.

Más allá: la productividad BAJA.
```

**¿Por qué?** Porque tener muchos subagentes especializados implica:

```
├── Decidir cada vez cuál delegar (coste mental)
├── Mantener el sistema de delegación coherente
│   (todos sus prompts, sus tools, sus modelos)
└── Recordar qué hace cada uno
```

> La curva de utilidad sube hasta un punto y baja.
>
> La gente que rinde más con subagentes para tareas cotidianas
> tiene **pocos pero bien afinados**.

---

## Slide 14 — Pocos pero bien afinados

```
Un Explorer.
Un Reviewer.
Un Tester.
Y quizá un Planner.
```

```
Cuatro como mucho.
```

```
Si te ves teniendo SIETE subagentes diferentes
para el día a día:

└── Probablemente algunos deberían ser SKILLS
    y otros deberían FUSIONARSE.
```

---

## Slide 15 — El matiz importante: harness verticales

```
La regla de 3-4 aplica a USO GENERAL.

El dev que cada día implementa features,
hace bugfix, revisa PRs.
```

> Pero hay otro tipo de uso:
> **harness verticales especializados**,
> donde la cuenta es completamente distinta.

```
Hay equipos con workflows con 10-15 subagentes
para dominios muy concretos:

├── Pipelines de research académico
├── Pipelines de auditoría de seguridad
└── Pipelines de análisis legal de contratos

Y funcionan bien.
```

---

## Slide 16 — Por qué los harness verticales sí escalan

```
Porque están ESTRUCTURADOS COMO UN HARNESS
con flujo definido.
```

```
NO son 15 subagentes sueltos a los que el principal
decide cuándo invocar.

Son una pipeline donde:
├── Un orquestador sabe el orden
├── Cada subagente tiene su entrada y salida claras
└── Hay loops de validación entre ellos.

La complejidad la absorbe la ESTRUCTURA,
no el dev.
```

> Para 3.2 esta distinción importa:
>
> **subagentes sueltos = 3-4 como mucho.**
> **harness estructurado = los que hagan falta.**

---

## Slide 17 — Cuatro casos típicos

Los subagentes que más equipos terminan teniendo. **No los necesitas todos** — usa los que aplican a tu flujo.

```
1. EL EXPLORER (haiku)
2. EL REVIEWER (sonnet)
3. EL TESTER (sonnet)
4. EL PLANNER (opus)
```

Los vemos uno a uno con el código completo.

---

## Slide 18 — El Explorer: frontmatter

```markdown
---
name: repo-explorer
description: Explora y mapea zonas del repositorio para devolver
  una vista resumida. Usar cuando se necesita entender un módulo,
  una carpeta, o una funcionalidad sin haberla tocado antes.
tools: Read, Grep, Glob
model: haiku
---
```

```
Read-only. Modelo rápido (haiku).
```

> Su trabajo es entender una zona del código
> y devolver un resumen estructurado,
> sin contaminar el contexto principal.

---

## Slide 19 — El Explorer: body

```markdown
Eres un explorador del repositorio. Tu trabajo es entender una zona
del código y devolver un resumen estructurado.

Cuando seas invocado con un objetivo de exploración:

1. Identifica los ficheros principales del área (entry points,
   definiciones públicas).
2. Mapea las dependencias y exports.
3. Identifica los patrones recurrentes (cómo se estructura la zona).
4. Encuentra los puntos de extensión o variación.

Devuelve un resumen en formato markdown con cuatro secciones:

- Estructura general — carpetas, ficheros principales.
- Puntos de entrada — qué se expone, cómo se usa desde fuera.
- Patrones internos — cómo está organizado por dentro.
- Notas de cuidado — código frágil, comentarios TODO/FIXME, deuda visible.

Sé conciso. El destinatario es el agente principal.
```

> Lo invocas explícitamente cuando llegas a un repo nuevo
> o cuando vas a tocar una zona donde no has trabajado.

---

## Slide 20 — El Reviewer: frontmatter

```markdown
---
name: dotnet-reviewer
description: Revisa código C# / .NET buscando problemas de naming,
  async patterns, manejo de errores y violaciones de convenciones
  del equipo. Usar después de cambios significativos en código antes
  de un commit.
tools: Read, Grep, Glob, Bash(git diff *), Bash(git log *)
model: sonnet
---
```

```
Code review aislado.
Lee, juzga, devuelve hallazgos.
NO modifica.
```

> Útil antes de commit.
> Encadena bien con un hook pre-commit (lo veremos en 3.3).

---

## Slide 21 — El Reviewer: body

```markdown
Eres un revisor senior de código .NET. Tu trabajo es identificar
problemas en código recién escrito y proponer fixes concretos.

Foco específico:
- Async/await: nunca .Result ni .Wait(), nunca async void
  salvo event handlers
- Naming: PascalCase clases/métodos, _camelCase campos privados
- Manejo de errores: Result<T> en dominio, ProblemDetails en API
- Convenciones del equipo recogidas en CLAUDE.md

Cuando te invoquen:

1. Identifica los ficheros modificados con `git diff`.
2. Examina los cambios línea a línea.
3. Devuelve hallazgos clasificados como CRÍTICO / IMPORTANTE / SUGERENCIA.
4. Para cada hallazgo: fichero, línea, problema, fix concreto.

Sé directo. No suavices los problemas críticos.
Si todo está limpio, dilo en una línea y termina.
```

---

## Slide 22 — El Tester: frontmatter

```markdown
---
name: test-generator
description: Genera tests unitarios y de integración para código .NET
  siguiendo el patrón del equipo (xUnit + NSubstitute). Usar cuando se
  necesite generar suite de tests para un componente, servicio o
  controller existente.
tools: Read, Grep, Write, Edit, Bash(dotnet test *)
model: sonnet
---
```

```
Genera tests SIN contaminar el contexto del principal.
Puede leer y escribir.
Y ejecutar `dotnet test` para verificar.
```

---

## Slide 23 — El Tester: body

```markdown
Eres un especialista en testing .NET.

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
6. Si fallan, analiza, ajusta y reintenta.
   Máximo 3 iteraciones antes de devolver al principal.

Cobertura objetivo: caminos críticos cubiertos, no obsesión con porcentajes.
Devuelve resumen con: tests creados, cobertura conceptual,
casos no cubiertos y por qué.
```

> El "máximo 3 iteraciones" es la clave.
>
> En 3.2 esto se llama loop con techo y es lo que evita
> bucles infinitos en sesiones de generación de tests.

---

## Slide 24 — El Planner: frontmatter

```markdown
---
name: feature-planner
description: Planifica la implementación de features grandes
  desglosándolas en pasos concretos antes de empezar a codificar.
  Usar cuando una feature toca más de tres ficheros o hay decisiones
  de diseño implícitas.
tools: Read, Grep, Glob
model: opus
---
```

```
Para tareas grandes donde merece la pena planificar antes.
Modelo Opus.
```

> Para planificación de features no triviales,
> el coste extra de Opus se justifica.
>
> Una mala decisión inicial cuesta horas o días.
> Un buen plan, minutos.

---

## Slide 25 — El Planner: body

```markdown
Eres un planificador estratégico de features.

Cuando te invoquen con un objetivo de feature:

1. Estudia el código relevante y entiende el estado actual.
2. Identifica los ficheros que van a tocarse.
3. Detecta decisiones de diseño implícitas
   (¿hay varias formas de hacer esto?).
4. Devuelve un plan estructurado:

   - Resumen — una frase del objetivo
   - Decisiones de diseño — alternativas consideradas
     y justificación de la elegida
   - Pasos — ordenados, cada uno con: qué tocas, qué creas,
     qué tests hacen falta
   - Riesgos — qué puede salir mal o complicarse
   - Validación — cómo sabremos que está bien al final

Termina pidiendo confirmación al agente principal antes de pasar
a ejecución.
```

---

## Slide 26 — Una nota de vocabulario

Si te encuentras leyendo whitepapers de arquitectura agentic — o algún post técnico de Anthropic más teórico — vas a ver este tipo de roles con otro nombre:

```
SPECIALIST AGENTS
```

```
Es lo mismo que estás viendo aquí:

un agente con un dominio acotado
dentro de un sistema mayor.
```

> La etiqueta cambia según dónde lo leas.
> El concepto NO cambia.

---

## Slide 27 — Combinación con skills: la decisión rápida

```
La pregunta natural cuando empiezas a tener subagentes y skills:

¿cuándo uso uno y cuándo el otro?
```

| Situación | Solución |
|---|---|
| Tarea con instrucciones reutilizables, encaja en el flujo principal | **Skill** |
| Tarea que requiere contexto propio o juicio independiente | **Subagente** |
| Tarea con instrucciones reutilizables que requiere aislamiento | **Skill que invoca subagente** |
| Convención que aplica a todo el repo | Va en `CLAUDE.md`, no es skill ni subagente |

---

## Slide 28 — Skill que invoca subagente: el patrón potente

Hay un patrón potente que aparece cuando empiezas a tenerlo todo:

```
Un skill cuyo cuerpo le pide al agente
que invoque a un subagente concreto.
```

**Ejemplo:** skill `pre-commit-check`

> *"Para validar el estado del repo antes de commit,
> invoca al subagente `dotnet-reviewer`
> y al subagente `test-runner` en paralelo.
> Combina sus resultados y devuelve un veredicto unificado."*

```
Aquí:
├── el SKILL orquesta
└── los SUBAGENTES ejecutan en aislamiento

Lo mejor de los dos mundos.
```

> Y este patrón es la base de los harness compuestos
> que veremos en 3.2.

---

## Slide 29 — Anti-patrones de subagentes

```
NAMING GENÉRICO
└── frontend-engineer, backend-helper, dev-assistant
    Suenan bien pero la auto-delegación falla
    porque la descripción es difusa.
    Mejor: nombres orientados a job
    └── repo-explorer, test-runner, pr-reviewer.

DESCRIPTION QUE ES PERSONA, NO WORKFLOW
└── "Eres un experto desarrollador full-stack..."
    Eso describe al agente, no la tarea.
    Mejor: "Genera tests xUnit para servicios .NET.
    Usar después de crear o modificar servicios."

TOOLS SIN RESTRINGIR
└── Si omites tools, hereda todo de la sesión.
    Para subagentes que solo deberían leer
    └── es un agujero.
        Aplica least privilege.

SUBAGENTE QUE DEBERÍA SER SKILL
└── Si la tarea está perfectamente bien
    dentro del flujo principal
    └── no necesita aislamiento.
        Skill > subagente.

DEMASIADOS SUBAGENTES PARA USO GENERAL
└── Más allá de 3-4 activos para tu día a día
    └── la productividad baja.
```

---

## Slide 30 — Más anti-patrones

```
NO ITERAR LA DESCRIPCIÓN
└── Igual que con skills,
    la primera descripción casi nunca es la final.
    Lánzala, ve si se activa cuando esperas, ajusta.

MODELO MAL ELEGIDO
├── Subagente de exploración corriendo en Opus
│   └── caro y sin necesidad.
├── Subagente de planificación en Haiku
│   └── falta de profundidad.
└── Asocia modelo a tipo de tarea:
    ├── Haiku → tareas mecánicas
    ├── Sonnet → la mayoría
    └── Opus → razonamiento complejo

SUBAGENTE QUE PIDE APROBACIONES CONSTANTES
└── Si está pidiendo permisos cada dos por tres:
    su `tools` está mal acotado
    o su rol no está bien definido.
    REVISA.
```

---

## Slide 31 — Errores frecuentes con tus primeros subagentes

```
❌ CREAR EL SUBAGENTE Y NO REINICIAR LA SESIÓN
   Modificación = reinicio o /agents para refrescar.

❌ EMPEZAR CON DESCRIPCIÓN DIFUSA
   "Ayuda con código" no activa nada.
   Sé específico, con verbos y casos.

❌ NO PROBAR LA AUTO-DELEGACIÓN
   Después de crearlo, lánzale tareas que esperarías
   que activaran al subagente.
   Si no se activa, el problema está en la descripción
   casi siempre.

❌ SUBAGENTES QUE ESCRIBEN EN SITIOS DONDE NO ESPERAS
   Si das Write o Edit sin restricción,
   puede modificar ficheros fuera de su scope.
   RESTRINGE.

❌ ENCADENAR VARIOS SUBAGENTES SIN ORQUESTACIÓN CLARA
   A llama a B, B llama a C.
   Si la cadena se rompe, debugging difícil.
   Empieza simple — uno o dos — antes de orquestaciones complejas.

❌ OLVIDAR QUE CADA SUBAGENTE ARRANCA CON CONTEXTO VACÍO
   No sabe lo que tú y el principal lleváis dos horas hablando.
   Si le pides algo que requiere ese contexto,
   dáselo en la invocación.

❌ NO USAR /agents PARA MANTENER ORDEN
   Es la forma fácil de ver, listar, editar y borrar subagentes.

❌ PRETENDER QUE UN SUBAGENTE RECUERDA ENTRE SESIONES
   Cada invocación es nueva.
   La memoria persistente vive en CLAUDE.md (proyecto)
   o en artefactos durables que produzca.
```

---

## Slide 32 — Lo que viene en 3.2

```
✅ Tienes el modelo conceptual de subagentes
✅ Sabes diferenciarlos de skills (rol vs playbook)
✅ Conoces los tres built-in y los cuatro custom típicos
✅ Sabes cuándo merece la pena delegar y cuándo no
```

> Y tienes el frame:
> **estás construyendo un agent harness.**
>
> Subagentes son los workers.
> Skills son los playbooks.
>
> Faltan dos piezas más, que vienen ahora.

```
SUBMÓDULO 3.2 — ORQUESTACIÓN Y FLUJOS COMPUESTOS
─────────────────────────────────────────────────────

Cómo se orquestan las piezas:
├── context: fork en skills (aislamiento ligero)
├── Composición de capas (skill orquesta subagentes)
├── Loops de retroalimentación (validator → implementer)
├── Context bank (memoria compartida en ficheros)
├── Paralelo vs serial (fan-out / fan-in)
├── Claude Code como MCP server
└── Agent Teams (lo que la comunidad llama "Swarm")
```

**Nos vemos en 3.2.**
