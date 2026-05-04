> **Versión:** v3 | **Módulo:** 3 | **Sub:** 3.2b | **Slides:** 35 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M03-S3.2b-memoria-paralelo-agent-teams-v3.md`

# Submódulo 3.2b — Memoria, paralelo, MCP, Agent Teams

## Slide 1 — Portada
**Módulo 3 · Submódulo 3.2 · Parte B**
Memoria compartida, paralelización, integraciones, Agent Teams
Context bank, fan-out/fan-in, Claude Code como MCP server, multi-sesión

---

## Slide 2 — Dónde estamos

En 3.2a vimos `context: fork`, composición de capas con un caso real desarrollado, y los tres patrones de loops con techo.

Ahora vienen las piezas que escalan el harness:

```
1. CONTEXT BANK
   Memoria compartida entre subagentes.

2. PARALELO vs SERIAL
   Cuándo lanzar varias cosas a la vez.

3. CLAUDE CODE COMO MCP SERVER
   Cuando otros sistemas hablan con tu Claude.

4. AGENT TEAMS
   Múltiples sesiones coordinándose.

5. CIERRE
   Árbol de decisión, anti-patrones, errores frecuentes.
```

---

## Slide 3 — Context bank: el problema que resuelve

Cuando tienes varios subagentes trabajando en un workflow compuesto, **hay información que necesitan compartir**:

```
├── El plan que generó el planner
├── Los hallazgos del reviewer
├── La lista de ficheros que el tester ha tocado
└── La decisión de diseño que el feature-implementer tomó
```

```
Hay DOS FORMAS de compartir esta información.
```

> Una funciona mal.
> Otra funciona bien.

---

## Slide 4 — Forma 1 (mala): pasarla por prompts

```
Cuando invocas al siguiente subagente
le pasas todo el contexto previo en el prompt:

"Aquí está el plan.
 Aquí están los hallazgos del reviewer anterior.
 Aquí los ficheros tocados..."
```

**El problema:**

```
├── Cada subagente recibe un prompt cada vez más grande
└── Cada vuelta se duplica trabajo
    └── el subagente parsea de nuevo
        lo que ya estaba parseado antes
```

> Y si tienes 5 subagentes en un workflow,
> el último recibe un prompt enorme.
>
> Tokens consumidos sin aportar valor.

---

## Slide 5 — Forma 2 (buena): artefactos durables

```
Cada subagente lee y escribe a ficheros markdown del repo
que persisten durante el workflow.
```

**Ficheros típicos:**

```
├── PLAN.md
├── REVIEW.md
└── CHANGES.md
```

```
Los subagentes:
├── leen lo que necesitan
├── escriben lo que producen
└── la información sobrevive entre invocaciones
```

> Esto es el **context bank**:
>
> un conjunto de ficheros que actúan
> como **memoria compartida**
> entre subagentes y el orquestador.

---

## Slide 6 — Estructura típica del context bank

Para el ejemplo del feature-implementer del 3.2a:

```
.claude/workflow-state/<feature-name>/
├── PLAN.md              # producido por feature-planner
│                          leído por implementer
├── EXPLORATION.md       # producido por repo-explorer
├── CHANGES.md           # registrado por implementer
│                          al modificar ficheros
├── TESTS.md             # producido por test-generator
└── REVIEW.md            # producido por dotnet-reviewer
                          en cada iteración
```

---

## Slide 7 — Cada subagente con entrada/salida claras

```
repo-explorer
└── escribe a EXPLORATION.md

feature-planner
└── lee EXPLORATION.md
    escribe a PLAN.md

agente principal
└── lee PLAN.md
    ejecuta
    registra en CHANGES.md

test-generator
└── lee CHANGES.md
    escribe TESTS.md

dotnet-reviewer
└── lee CHANGES.md + TESTS.md
    escribe REVIEW.md
```

> Cada pieza tiene su contrato.
> Cada fichero es la entrada de uno
> y la salida de otro.

---

## Slide 8 — Las 4 ventajas del context bank

```
1. TRAZABILIDAD
   Si algo falla a mitad del workflow:
   los ficheros del context bank te dicen
   qué pasó hasta ese punto.
   └── Es tu LOG.

2. RECUPERACIÓN
   Si la sesión muere a la mitad:
   puedes retomar el workflow
   porque el estado está persistido.

3. LOOPS BARATOS
   Cuando un validator devuelve al implementer:
   el implementer NO necesita re-explicar todo.
   Simplemente lee REVIEW.md y aplica los fixes.

4. AUDITORÍA
   En equipos grandes:
   los ficheros son evidencia
   de qué se hizo, cómo, y con qué criterio.
```

---

## Slide 9 — Limpieza del context bank

```
Los ficheros del context bank son TEMPORALES del workflow.
NO son parte del repo permanente.
```

**Convención típica:**

```
├── Vivir bajo .claude/workflow-state/
└── Añadirlo a .gitignore

Cuando el workflow termina con éxito:
└── el orquestador limpia
    (opcionalmente, lo deja por si quieres revisarlo)
```

**Algunos equipos prefieren mantenerlo como historial:**

> *"El PLAN.md de la feature de cancelación de pedidos
> vivirá en el repo bajo `docs/features/cancellation-orders/PLAN.md`"*

> Útil para documentación. No es obligatorio.

---

## Slide 10 — Diferencia con CLAUDE.md

Importante:

```
EL CONTEXT BANK
└── NO ES CLAUDE.md
```

| | `CLAUDE.md` | Context bank |
|---|---|---|
| **Qué contiene** | Contexto persistente del proyecto | Estado del workflow concreto en curso |
| **Vida** | Persiste siempre | Solo durante el workflow |
| **Quién lo usa** | Cargado en cada sesión | Subagentes lo leen/escriben activamente |
| **Quién lo edita** | Tú (a mano) o el equipo (vía PR) | El orquestador y los subagentes |

---

## Slide 11 — Paralelo vs serial: cuándo elegir cada patrón

Hasta aquí los flujos que hemos visto son **lineales**:

```
El skill orquestador invoca a A
→ espera
→ invoca a B con el resultado de A
→ espera
→ invoca a C
→ ...

Cada paso depende del anterior.
```

```
Pero NO todas las tareas son así.
```

> A veces tienes varias subtareas **independientes** entre sí.
> Y ejecutarlas en serie es desperdiciar tiempo.

---

## Slide 12 — El ejemplo más claro

```
Validar un PR antes de subirlo.
```

**Quieres:**

```
├── que un reviewer mire el código
├── que un tester corra los tests
└── que otro subagente verifique
    las convenciones de naming
```

```
Ninguno de los tres depende
de la salida de los demás.

Pueden ejecutarse a la vez
y reducir A UN TERCIO el tiempo total.
```

---

## Slide 13 — Fan-out / fan-in

Cuando tienes subtareas independientes, el orquestador hace:

```
FAN-OUT
└── lanza N invocaciones a subagentes en paralelo.
    Cada uno trabaja en su contexto aislado.

FAN-IN
└── cuando todos terminan,
    el orquestador recoge los N resultados
    y los combina en una respuesta unificada.
```

> Esto ya lo viste en miniatura en 3.1b
> con el ejemplo del skill `pre-commit-check`:
>
> *"Para validar el estado del repo antes de commit,
> invoca al subagente `dotnet-reviewer`
> y al subagente `test-runner` en paralelo.
> Combina sus resultados y devuelve un veredicto unificado."*

```
Es fan-out / fan-in con dos subagentes.
```

---

## Slide 14 — Hasta cuántos en paralelo

```
El patrón escala bien hasta 4-5 subagentes en paralelo.
```

```
Más allá:
└── el coste de coordinación
    se come el ahorro de tiempo.
```

> No es un límite duro, es una zona de rendimiento.
> A partir de 5 paralelos, cada uno extra rinde menos.

---

## Slide 15 — La decisión rápida

| Situación | Patrón |
|---|---|
| Subtarea B necesita el output de A | **Serial** |
| Subtareas independientes que pueden hacerse a la vez | **Paralelo** |
| Validación con varios ángulos (seguridad + estilo + tests) | **Paralelo** |
| Pipeline de transformación (explorar → planificar → ejecutar → validar) | **Serial** |
| Varios subagentes opinando sobre el mismo input | **Paralelo** (luego votación o síntesis) |

---

## Slide 16 — El error típico con paralelo

Lanzar paralelo cuando hay **dependencias ocultas**:

> *"Quiero que el tester y el reviewer corran en paralelo
> para ahorrar tiempo"*

```
Pero el reviewer NECESITA ver los tests también.

Si no:
└── sus hallazgos van a estar incompletos.

Resultado:
├── ahorras los segundos del paralelo
└── te los gastas en una ronda extra
    cuando el reviewer reporta cosas
    que no podía saber sin el output del tester.
```

> La regla rápida:
>
> ├── Si la salida de A condiciona cómo B trabaja → **serial**
> └── Si A y B se combinan al final pero no se influyen → **paralelo**

---

## Slide 17 — Una nota de vocabulario

```
PARALLEL WORKFLOW
```

> En literatura formal este patrón aparece así.

```
Es uno de los más rentables
cuando las dependencias están claras.

Pero también
└── uno de los más fáciles de aplicar mal.
```

---

## Slide 18 — Claude Code como MCP server

Una capa más arriba:

```
¿Y si quieres que OTROS AGENTES
hablen con tu Claude Code?
```

```
Esto es lo que permite
el modo "Claude Code como MCP server".
```

```
EN VEZ DE
└── ser solo un cliente
    que consume MCP servers (Figma, GitHub, etc.)

CLAUDE CODE TAMBIÉN
└── se expone como un MCP server
    al que otros pueden conectarse.
```

---

## Slide 19 — Casos de uso (1/2)

```
1. OTRO CLAUDE CODE QUE DELEGA

Tienes una sesión "principal" en tu portátil de trabajo
y otra "auxiliar" para tareas paralelas:
├── research
├── exploración
└── generación de docs

La auxiliar se conecta a la principal vía MCP
cuando necesita contexto del repo activo.
```

```
2. INTEGRACIÓN CON SISTEMAS INTERNOS

Tu plataforma interna del equipo
necesita capacidades de Claude Code
para procesar tareas asíncronas:
├── generar documentación
├── validar PRs en bulk
└── etc.

Lanza llamadas MCP
a una instancia de Claude Code
corriendo en un servidor.
```

---

## Slide 20 — Casos de uso (2/2)

```
3. OTRO TIPO DE CLIENTE MCP

Hay clientes MCP más allá de Claude Code:
├── Cursor
├── Codex CLI
└── alguna otra herramienta

Si tienes un Claude Code configurado con tu kit
(skills propios, subagentes):
└── puedes exponerlo como MCP a esos otros clientes
    y aprovechar el setup.
```

---

## Slide 21 — Cómo se activa

```
En la configuración de Claude Code:

├── Indicas que el modo MCP server está activo
└── Y en qué puerto/socket.

El otro lado se conecta
└── como cliente MCP normal.
```

> Detalles concretos varían según versión y plataforma.

```
Lo importante para nosotros:

├── SABER QUE EXISTE
└── RECONOCER cuándo merece la pena plantearlo
    └── integraciones serias entre sistemas
        no para uso personal del día a día.
```

---

## Slide 22 — Cuándo NO usar este patrón

```
SI LO ÚNICO QUE QUIERES ES DELEGAR TAREAS
DENTRO DE UNA MISMA SESIÓN
└── los SUBAGENTES son el camino correcto.

SI QUIERES QUE DOS SESIONES DE CLAUDE CODE
COLABOREN MÁS DE CERCA
└── los AGENT TEAMS están pensados para eso.

CLAUDE CODE COMO MCP SERVER ES PARA
└── INTEGRACIÓN CON UN TERCERO:
    ├── otro sistema
    ├── otra herramienta
    └── otro agente que NO es Claude Code mismo.
```

> Para todo lo demás, los mecanismos internos son más sencillos.

---

## Slide 23 — Agent Teams: cuando los subagentes no bastan

```
Aquí entramos en territorio EXPERIMENTAL.
```

```
Hasta ahora, todo lo que hemos visto
pasa DENTRO de una sesión de Claude Code:

├── el agente principal
├── sus subagentes
├── los skills que invoca
└── los MCP que consulta

Una sesión es la unidad.
```

```
Agent Teams ROMPE esa unidad.
```

> Permite que **múltiples sesiones de Claude Code**
> se comuniquen entre sí,
> con un lead que las orquesta
> y mensajes directos entre ellas.

---

## Slide 24 — Aclaración terminológica: "Swarm" vs Agent Teams

```
Mucha gente lo llama "Swarm" o "Swarm Mode"
porque la comunidad estuvo construyendo esto
ANTES de que fuera oficial.
```

```
Herramientas como:
├── claude-flow
└── oh-my-claude

Ofrecían orquestación de varios agentes con persistencia.
```

```
Cuando Anthropic lo lanzó nativamente
a principios de 2026:
└── lo llamó oficialmente AGENT TEAMS.
```

> Los dos términos se usan.
> Pero **"Agent Teams" es lo correcto**
> en la documentación oficial.

```
Y un último apunte de vocabulario:
en literatura más formal sobre arquitecturas agentic
└── "collaborative" o "swarm architecture"
    Si lees discusiones de arquitectura distribuida de agentes
    └── esos son los términos.
```

---

## Slide 25 — Cómo funciona Agent Teams

```
TEAM LEAD
└── Sesión que recibe la petición inicial del usuario
    y la orquesta.

TEAMMATES
└── Sesiones independientes de Claude Code
    que reciben sub-tareas del Lead.
    └── Cada teammate tiene su propio contexto
        y su propio terminal.
        ├── NO es un subagente dentro de la sesión del Lead
        └── es OTRA sesión.

COMUNICACIÓN DIRECTA
└── Los teammates pueden mandarse mensajes entre ellos
    no solo reportar al Lead.

BACKEND TMUX
└── Para visualizar lo que pasa
    los teammates corren en panes separados de tmux.
```

**Activación:**

```bash
export CLAUDE_CODE_EXPERIMENTAL_AGENT_TEAMS=1
```

> O en el `settings.json` de tu user/proyecto.

---

## Slide 26 — La progresión de delegación

De menor a mayor autonomía:

```
1. SOLO SESSION
   Tú hablando con Claude Code.
   Control total.

2. SKILLS
   Encapsulas tareas reutilizables.

3. SUBAGENTES
   Delegas tareas con su propio contexto.

4. AGENT TEAMS
   Múltiples sesiones colaborando.
```

> Cada paso te da:
> ├── **más** capacidad de cómputo paralelo
> └── A cambio de **menos** control y **más** coste de tokens

```
La pregunta NO es "¿cuánto puedo delegar?"

Es "¿cuánto debo delegar para esta tarea concreta?"
```

---

## Slide 27 — Cuándo Agent Teams aporta

```
FEATURES MUY GRANDES DIVISIBLES EN TRACKS PARALELOS
└── Backend + frontend + infraestructura
    cada uno con su teammate.

QA SWARMS
└── Varios teammates probando la misma feature
    desde perspectivas distintas:
    ├── funcional
    ├── performance
    ├── seguridad
    └── accesibilidad

HIPÓTESIS COMPETITIVAS EN DEBUGGING
└── Cada teammate explora una hipótesis distinta
    debaten, convergen.
```

---

## Slide 28 — Cuándo NO es necesario

```
EL 95% DE LAS TAREAS DEL DÍA A DÍA
├── Implementar un endpoint
├── Escribir tests
└── Refactorizar un módulo
    └── Subagentes bastan.

TAREAS DONDE EL CONTROL HUMANO IMPORTA
└── Si vas a tener que revisar cada paso
    └── mejor sesión solo o con subagentes.
        Agent Teams asume mucha autonomía.

TU PRIMERA SEMANA CON CLAUDE CODE
└── Antes de Agent Teams
    hay mucho recorrido en subagentes.
```

---

## Slide 29 — El coste real de Agent Teams

Algo que se menciona poco pero importa:

```
Agent Teams cuesta MÁS tokens, no menos.
```

```
Cada teammate
└── es una sesión de Claude Code
    con su propio contexto
    sus propias decisiones
    su propio razonamiento.

Tres teammates
└── ≈ 3x más tokens que una sesión sola.

Y eso suponiendo que la coordinación funciona bien.
```

---

## Slide 30 — La cifra concreta: 10-15x

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   Cifra del whitepaper de Anthropic                      │
│   sobre arquitecturas agentic:                           │
│                                                          │
│   Los sistemas multi-agente consumen aproximadamente     │
│   10-15x MÁS TOKENS que un agente solo.                  │
│                                                          │
│   Incluye desde subagentes hasta Agent Teams.            │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

> No es un detalle menor.
>
> Es lo que diferencia *"voy a montar un harness con tres subagentes"*
> de *"voy a montar un sistema multi-agente porque mola"*.

```
Si la tarea no justifica el incremento de orden de magnitud:
└── NO compensa.

Empieza con uno o dos subagentes.
Mide tu factura con /usage.
Y escala solo cuando los números cuadren.
```

---

## Slide 31 — El estado actual de Agent Teams

```
Agent Teams sigue siendo EXPERIMENTAL
a fecha de este curso.
```

**Eso significa:**

```
├── API y comportamiento pueden cambiar entre versiones
├── Algunas integraciones (con MCP servers, con plugins)
│   no están del todo pulidas
├── La documentación oficial es más escasa
│   que la de subagentes o skills
└── La fiabilidad varía con la complejidad del workflow
```

> Mi recomendación honesta para este curso:
>
> **basta con saber que existe**
> **y entender cuándo plantearlo.**
>
> No es algo que tu equipo vaya a poner en producción
> la semana que viene.
>
> Pero sí es donde va la herramienta a medio plazo.

---

## Slide 32 — Cuándo usar qué: árbol de decisión

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

> La regla práctica:
> **empieza simple.**
>
> La mayoría de necesidades se cubren con skills + subagentes.

---

## Slide 33 — Anti-patrones de orquestación

```
SOBREINGENIERÍA DESDE EL DÍA UNO
└── Skill que orquesta cinco subagentes
    y consulta tres MCP servers
    para una tarea que un agente principal
    con un buen CLAUDE.md resolvería sola.

CADENAS DEMASIADO LARGAS
└── Skill llama a subagente que llama a otro skill
    que llama a otro subagente que consulta un MCP.
    Mantén las cadenas cortas
    └── máximo 2-3 niveles de profundidad.

LOOPS SIN TECHO
└── Validator que devuelve al implementer infinitamente.
    Loops SIEMPRE con máximo de iteraciones.

FALTA DE OBSERVABILIDAD
└── Cuando algo falla en un workflow compuesto:
    ¿cómo sabes en qué eslabón?
    Aquí es donde el context bank ayuda
    └── los ficheros del workflow son tu LOG.

PASAR CONTEXTO POR PROMPT EN VEZ DE POR CONTEXT BANK
└── Cuando varios subagentes comparten información
    usa ficheros markdown
    no prompts gigantescos.
```

---

## Slide 34 — Más anti-patrones

```
SUBAGENTES QUE SE SOLAPAN
└── Reviewer + Code Quality Checker + Security Auditor
    que hacen cosas similares.
    La auto-delegación va a fallar
    porque las descripciones colisionan.
    Mejor UNO bien definido que TRES con scope difuso.

PRETENDER AGENT TEAMS CUANDO SUBAGENTES BASTA
└── "Quiero que tres agentes trabajen en paralelo"
    ¿De verdad necesitas tres sesiones independientes
    que se comunican entre sí?
    La mayoría de las veces
    tres subagentes en una sesión te dan lo mismo
    por una fracción del coste.

NO ITERAR LAS ORQUESTACIONES
└── Igual que con skills y subagentes individuales:
    los workflows compuestos no salen perfectos a la primera.
    Pruébalos en casos reales, observa qué falla, ajusta.
```

---

## Slide 35 — Cierre y bridge a 3.3

```
✅ context: fork en skills
✅ Composición de capas con skill orquestador
✅ Loops con techo (3 patrones)
✅ Context bank como memoria compartida
✅ Paralelo vs serial (fan-out / fan-in)
✅ Claude Code como MCP server
✅ Agent Teams (con la cifra del 10-15x en mente)
✅ Árbol de decisión completo
```

```
Y el frame:

estás construyendo un agent harness.

├── El skill orquestador es el INITIATOR
├── Los subagentes son los WORKERS
├── El context bank es la MEMORIA compartida
└── Los loops son lo que hace
    que el harness se autocorrija
```

> Falta una pieza.

```
SUBMÓDULO 3.3 — HOOKS, CHANNELS Y AUTOMATIZACIÓN DETERMINISTA
─────────────────────────────────────────────────────────────

La capa DETERMINISTA del harness.

Lo que hace que ciertas cosas pasen SIEMPRE
sin que tengas que pedirlo cada vez.

"Después de cada commit, lanza el reviewer"
"Antes de cada PR, ejecuta el checklist"

Esto cierra el harness:
└── convierte la herramienta en algo
    que TRABAJA CONTIGO en background.
```

**Nos vemos en 3.3.**
