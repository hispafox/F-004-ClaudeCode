> **Versión:** v3 | **Módulo:** 3 | **Sub:** 3.1a | **Slides:** 30 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M03-S3.1a-modelo-conceptual-subagentes-integrados-v3.md`

# Submódulo 3.1a — Modelo conceptual y subagentes integrados

## Slide 1 — Portada
**Módulo 3 · Submódulo 3.1 · Parte A**
Modelo conceptual y subagentes integrados
Qué problema resuelven, agent harness, los tres built-in

---

## Slide 2 — El problema que resuelve un subagente

Llega un punto en una sesión larga de Claude Code donde notas algo raro:

```
├── El agente repregunta cosas que sabía hace una hora.
├── Sus respuestas se vuelven más vagas.
└── Las decisiones que tomasteis al principio se difuminan.
```

> La causa es siempre la misma:
>
> la ventana de contexto se está llenando,
> y cada nueva exploración añade ruido al razonamiento principal.

---

## Slide 3 — El caso típico

Hay un caso típico que dispara este problema:

```
Alguien implementa una feature compleja.

A mitad, quiere entender cómo está hecho un módulo
del repo que no ha tocado todavía.

El agente principal:
├── Lee ficheros
├── Explora dependencias
└── Cuenta imports

Cuando termina la exploración:
└── Ha cargado al contexto principal
    30 ficheros que NO son relevantes
    para el código que está escribiendo.
```

```
La conversación queda contaminada con esa exploración.

Y los siguientes 60 minutos de trabajo
arrastran ese peso.
```

---

## Slide 4 — Compactar es un parche, los subagentes son la solución

```
/compact ayuda. Pero es un parche.
```

```
La solución estructural:

NO contaminar el contexto principal
con tareas que tienen su propia naturaleza.
```

> Y esa solución son los **subagentes**.

---

## Slide 5 — Qué es un subagente

```
Un subagente es OTRO Claude dentro de tu sesión.
```

**Con tres propiedades clave:**

```
├── Su propio contexto
├── Su propio razonamiento
└── Un scope LIMITADO a la tarea que le delegues
```

```
Hace su trabajo.
Te devuelve un resultado.
Y se va sin dejar rastro en tu sesión principal.
```

> La exploración del módulo del ejemplo de antes,
> hecha por un subagente, te devuelve:
>
> *"el módulo Foo hace X, expone Y, depende de Z"*
>
> sin que esos 30 ficheros aparezcan jamás
> en tu contexto principal.

---

## Slide 6 — Encuadre del módulo: agent harness

Antes de meternos en subagentes, conviene poner el frame que vertebra los tres submódulos del módulo 3.

Vas a escuchar el término **"agent harness"** mucho a partir de ahora — en blogs de Anthropic, en threads técnicos, en discusiones de la comunidad. Conviene saber qué significa.

> La fórmula que circula es:
>
> ## **agent = model + harness**

---

## Slide 7 — Qué es un harness

```
Un MODELO en bruto NO es un agente.
```

```
Lo es solo cuando le rodeas de:
├── Tools
├── Contexto
├── Hooks
└── Feedback loops

Todo eso, junto, es el HARNESS.
```

> Claude Code, Cursor, Codex —
> todos son harnesses construidos sobre el modelo.
>
> La performance que sientes al usar Claude Code
> viene tanto del harness como del modelo subyacente.

---

## Slide 8 — Estás construyendo tu propio harness

Aquí está la parte interesante:

> **Cuando personalizas Claude Code con skills, subagentes y hooks,
> estás construyendo TU PROPIO HARNESS encima del de Anthropic.**

```
El módulo 2 cubría una pieza:
└── SKILLS

El módulo 3 cubre las otras tres principales:

├── 3.1 (este apartado)
│   └── SUBAGENTES como workers del harness
│
├── 3.2
│   └── ORQUESTACIÓN que combina las piezas
│       en flujos coherentes
│
└── 3.3
    └── HOOKS como capa determinista del harness
```

---

## Slide 9 — La idea a recordar

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   No estás aprendiendo features sueltas.                 │
│                                                          │
│   Estás aprendiendo a montar un harness                  │
│   que conoce a tu equipo.                                │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

> En el portal de recursos del curso encontraréis
> un cheatsheet visual de este patrón
>
> al que conviene volver cuando dudéis dónde encaja una pieza.

---

## Slide 10 — Mental model: CLAUDE.md, skills, subagentes

Tres piezas con propósitos relacionados pero distintos:

| Pieza | Cuándo se carga | Para qué |
|---|---|---|
| **`CLAUDE.md`** | Siempre, al arrancar | Contexto **persistente** del proyecto |
| **Skills** | Cuando la descripción coincide | Capacidades **bajo demanda**, playbooks |
| **Subagentes** | Cuando se invocan | Tareas **aisladas** con su propio contexto |

---

## Slide 11 — La analogía de la oficina

```
Imagina que estás trabajando en una oficina.
```

```
CLAUDE.md
└── El manual del empleado
    que tienes pegado en la pared
    y consultas cada día.

SKILLS
└── Las macros y plantillas
    que tienes en tu cajón
    para tareas frecuentes.

SUBAGENTES
└── Compañeros con los que DELEGAS:
    ├── Les pides algo
    ├── Lo hacen en su mesa
    └── Te traen el resultado terminado
        sin que tú tengas que ver
        el papel desordenado de su escritorio.
```

---

## Slide 12 — La pregunta práctica que tienes que aprender a responder

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   Cuándo lo hago yo con un SKILL                         │
│                                                          │
│         vs                                               │
│                                                          │
│   Cuándo se lo paso a un SUBAGENTE                       │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

> Esa es la decisión que vamos a aprender a tomar
> en este apartado y los siguientes.

---

## Slide 13 — Los subagentes integrados

Claude Code trae **tres subagentes built-in** que están disponibles en cada sesión sin que tengas que crearlos.

```
Conviene conocerlos porque:
├── A veces el agente principal los activa solo
└── Otras veces puedes invocarlos explícitamente
```

**Los tres:**

```
1. Explore
2. Plan
3. general-purpose
```

Los vemos uno a uno.

---

## Slide 14 — Explore: lectura y exploración

```
SUBAGENTE ESPECIALIZADO EN LECTURA Y EXPLORACIÓN.

├── Solo lee
└── No modifica

Por defecto se ejecuta en HAIKU
└── Más rápido y barato
    └── Porque la exploración es una tarea
        donde la velocidad importa más
        que el razonamiento profundo.
```

---

## Slide 15 — Cuándo se activa Explore

```
Cuando una tarea principal requiere ENTENDER
una zona del repo que no es la que estás tocando.
```

**Ejemplo típico:**

> *"Implementa X en el módulo A,
> pero antes asegúrate de no romper la integración con el módulo B"*

```
El agente principal le pide a Explore que estudie el módulo B.
Explore vuelve con un resumen.
El principal procede sin haber cargado todo B en su contexto.
```

---

## Slide 16 — Casos típicos donde Explore brilla

```
ANÁLISIS DE UN REPO GRANDE QUE NO HAS TOCADO
└── Le pides al agente que entienda
    la estructura general
    └── Lo hace con Explore por debajo.

BÚSQUEDA DE PATRONES
└── "Busca todos los sitios donde se usa
     la inyección de IOrderService"
    └── Búsqueda Grep amplia
        analizada en su propio contexto.

REVISIÓN CRUZADA
└── "Antes de tocar este servicio,
     mira cómo se usa en el resto del proyecto"
```

---

## Slide 17 — Plan: planifica antes de actuar

```
SUBAGENTE QUE PLANIFICA ANTES DE ACTUAR.
```

```
Es lo que se ejecuta cuando:
├── Lanzas /plan
└── O cuando la tarea es lo suficientemente compleja
    como para que el agente principal
    decida que merece la pena planificar antes.
```

**Lo que hace Plan:**

```
1. Recopila contexto
2. Razona sobre la mejor forma de abordar la tarea
3. Presenta un plan paso a paso

NO ACTÚA.

Devuelve el plan al agente principal
(o a ti, si lo invocaste con /plan)
└── Y se queda esperando confirmación.
```

---

## Slide 18 — Cuándo Plan merece la pena

```
LA TAREA VA A TOCAR MÁS DE TRES FICHEROS

HAY DECISIONES DE DISEÑO IMPLÍCITAS

UN ERROR A MITAD SERÍA COSTOSO DE REVERTIR
```

> En cualquiera de esas tres situaciones,
> el `/plan` antes ahorra horas después.

---

## Slide 19 — General-purpose: el comodín

```
EL COMODÍN.

Subagente que puede tanto EXPLORAR como MODIFICAR.
```

```
Lo usa el agente principal cuando:
└── Una tarea requiere ambas cosas
    pero quiere mantener su propio contexto limpio.
```

**Casos:**

```
REFACTOR DE UN MÓDULO AISLADO
└── Donde el resultado vuelve al principal como "hecho".

GENERACIÓN DE UN CONJUNTO DE TESTS
└── Que el principal solo necesita saber que existen.
```

---

## Slide 20 — Cómo se invocan: automático

La mayoría del tiempo, el agente principal **decide automáticamente** cuándo usar cada uno.

```
Cuando ves en la salida un mensaje tipo:

> Launching Explore agent to investigate...
> Plan agent generating strategy...

Ahí está pasando.
```

> El agente principal está delegando
> sin que tú hayas tenido que pedirlo.

---

## Slide 21 — Cómo se invocan: explícito

Si quieres invocarlos **explícitamente**, puedes pedirlo:

```
> Usa el subagente Explore
  para mapear la estructura del módulo Orders

> Lanza Plan para diseñar el refactor
  de la capa de validación
```

```
O directamente con el comando:

> /plan
```

> Para activar planificación sin tener que pedirlo en prosa.

---

## Slide 22 — La limitación honesta de la auto-delegación

Una observación honesta:

```
La auto-delegación a subagentes integrados
NO ES PERFECTA.
```

**Dos casos típicos donde se queda corta:**

```
1. HAY TAREAS DONDE MERECERÍA LA PENA QUE EL PRINCIPAL DELEGARA
   A EXPLORE Y NO LO HACE.
   
   Resultado: el principal se carga con exploraciones
   que deberían haberse aislado.

2. HAY OTRAS DONDE DELEGA CUANDO PODRÍA HABER RESUELTO SOLO.

   Resultado: overhead innecesario.
```

---

## Slide 23 — La regla práctica

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   Si notas que tu sesión principal se está cargando      │
│   con exploraciones que deberían haberse aislado:        │
│                                                          │
│         INVÓCALOS EXPLÍCITAMENTE.                        │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

> No esperes a que el agente principal acierte siempre.
> Cuando el caso es claro, dilo.

---

## Slide 24 — Los tres juntos: cuándo cada uno

Recapitulación rápida de los tres built-in:

| Subagente | Para qué | Modelo |
|---|---|---|
| **Explore** | Lectura y exploración | Haiku |
| **Plan** | Planificación antes de actuar | Sonnet |
| **general-purpose** | Comodín: explorar + modificar | Sonnet |

---

## Slide 25 — Lo que tienes ahora con los built-in

```
✅ Identificas el problema
   (contaminación de contexto en sesiones largas)

✅ Conoces la solución estructural
   (subagentes con su propio contexto)

✅ Entiendes el frame de agent harness
   y dónde encajan los subagentes

✅ Distingues entre CLAUDE.md, skills y subagentes

✅ Conoces los tres built-in y cuándo usarlos
```

> Para muchas tareas, esto es suficiente.
>
> Los built-in cubren los casos genéricos
> de exploración y planificación.

---

## Slide 26 — Lo que falta: subagentes propios

```
Pero la potencia real aparece cuando defines
subagentes PROPIOS para tu equipo.
```

**Ejemplos de candidatos:**

```
├── Un revisor de código C# / .NET
│   con las convenciones de tu equipo
│
├── Un generador de tests
│   con tu patrón estándar (xUnit + NSubstitute)
│
├── Un explorador del repo
│   afinado a la estructura de tu proyecto
│
└── Un planificador de features
│   que conoce tu workflow
```

> Esto es lo que vamos a ver en 3.1b.

---

## Slide 27 — La pregunta antes de seguir

Antes de pasar a la creación de subagentes propios, una pregunta para tener en mente:

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   ¿Qué tarea de tu día a día requiere su propio          │
│   contexto / razonamiento independiente del trabajo      │
│   principal?                                             │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

```
Pistas:
├── ¿Tarea que carga muchos ficheros?
├── ¿Tarea con criterio propio (review, planificación)?
└── ¿Tarea que el principal preferiría delegar
     y solo recibir el resultado?
```

---

## Slide 28 — Casos clásicos de candidato a subagente

Para que las ideas aterricen:

```
"UN REVISOR DE PRs ANTES DE SUBIRLOS"
└── El caso clásico.
    
    Es el que más rentabilidad da
    en equipos que ya hacen code review humano.
    
    Un subagente reviewer NO sustituye al humano.
    Pero le ahorra el primer pase mecánico
    y le permite centrarse
    en lo que requiere criterio real.

"UN EXPLORADOR DEL REPO PARA JUNIORS QUE SE INCORPORAN"
└── También clásico.
    
    Un Explorer bien afinado
    es la diferencia entre
    ├── un junior que tarda dos semanas en ubicarse
    └── y uno que en dos días ya entiende
        la estructura general.
```

> Tener uno de estos dos en mente antes de la siguiente parte
> hace que el resto del módulo gane sentido práctico.

---

## Slide 29 — Lo que viene en 3.1b

```
SUBMÓDULO 3.1b — SUBAGENTES CUSTOM Y PATRONES
─────────────────────────────────────────────────────

Crear un subagente custom
├── Estructura del fichero
├── Anatomía: frontmatter + body
├── El comando /agents
└── Carga y refresco

Patrones de delegación
├── Cuándo SÍ delegar (5 razones)
├── Cuándo NO delegar (4 razones)
└── El número práctico: 3-4 con matiz importante
    (harness verticales son distinto)

Cuatro casos típicos con código completo
├── Explorer (haiku)
├── Reviewer (sonnet)
├── Tester (sonnet)
└── Planner (opus)

Combinación con skills
└── Skill que invoca subagente

Anti-patrones de subagentes

Errores frecuentes con tus primeros subagentes

Cierre con bridge a 3.2
```

---

## Slide 30 — Antes de pasar a 3.1b

```
Recordatorio del frame:

agent = model + harness
```

```
Estás aprendiendo a construir TU PROPIO HARNESS.

Subagentes son los workers.
En 3.2 los orquestamos.
En 3.3 los hacemos deterministas con hooks.
```

**Nos vemos en 3.1b.**
