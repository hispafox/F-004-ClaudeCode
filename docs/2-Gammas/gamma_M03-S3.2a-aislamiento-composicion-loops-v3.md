> **Versión:** v3 | **Módulo:** 3 | **Sub:** 3.2a | **Slides:** 32 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M03-S3.2a-aislamiento-composicion-loops-v3.md`

# Submódulo 3.2a — Aislamiento, composición y loops

## Slide 1 — Portada
**Módulo 3 · Submódulo 3.2 · Parte A**
Aislamiento, composición y loops
context: fork, skills que orquestan subagentes, loops con techo

---

## Slide 2 — El frame: agent harness

Recoges el hilo donde lo dejamos en 3.1.

```
agent = model + harness
```

```
Cuando personalizas Claude Code con
skills, subagentes y hooks
└── Estás construyendo TU PROPIO HARNESS
    encima del de Anthropic.
```

> Este apartado es **donde el harness empieza a parecer un harness**.
>
> Hasta aquí cada pieza vivía aislada.
> Skills por su lado, subagentes por el suyo.
>
> Aquí los UNIMOS en flujos coherentes.

---

## Slide 3 — La pregunta natural

Cuando empiezas a tener varios skills, varios subagentes y algún MCP server conectado:

```
"¿Cómo se combinan en algo más grande
 que la suma de sus partes?"
```

**La respuesta:**

```
1. Composición de capas
2. Loops de retroalimentación
3. Memoria compartida (context bank)
4. Y, en casos extremos, varias sesiones coordinándose
```

> Lo que viene es más conceptual que técnico.
> No vamos a escribir código nuevo.
> Vamos a entender cómo encajan los elementos
> que ya tenemos.

---

## Slide 4 — context: fork en skills

Lo más simple para empezar a orquestar:

```
Un skill que se ejecuta en SU PROPIO CONTEXTO
sin contaminar el de la sesión principal.
```

**Recordatorio del módulo 2:**

```
Un skill se ejecuta DENTRO del contexto
de la sesión principal.

├── Sus instrucciones
├── Sus razonamientos
└── Lo que decide hacer

Todo eso vive en la misma ventana de contexto
que tu conversación con el agente.
```

---

## Slide 5 — Cuando un skill se vuelve grande

```
Cuando un skill ejecuta tareas
que requieren mucha exploración
└── esto puede ser problemático.
```

**La solución:**

```yaml
---
name: deep-architecture-analysis
description: Analiza la arquitectura del repo en profundidad
  y devuelve un informe estructurado. Usar cuando se necesite
  entender el diseño general antes de cambios grandes.
context: fork
allowed-tools: Read, Grep, Glob
---

[instrucciones extensas de análisis...]
```

> Una sola línea: `context: fork` en el frontmatter.

---

## Slide 6 — Cómo funciona context: fork

Cuando el skill se activa:

```
1. Claude principal lanza el skill
   en un CONTEXTO AISLADO.

2. El skill ejecuta sus instrucciones
   ├── leer ficheros
   ├── analizar
   └── razonar

   sin que nada de eso aparezca
   en el contexto principal.

3. Cuando el skill termina:
   └── devuelve SOLO su resultado final
       al principal.
```

> Es lo mismo que hace un subagente,
> pero envuelto en la abstracción de un skill.

---

## Slide 7 — Cuándo usar context: fork

```
SKILL QUE LEE MUCHO
└── (decenas de ficheros como parte de su trabajo)
    → context: fork
       Si no, satura el principal.

SKILL QUE PRODUCE OUTPUT CORTO Y CONCISO
A PARTIR DE MUCHO INPUT
→ context: fork
   La idea de aislar lo gordo
   y devolver lo destilado.

SKILL QUE PUEDE ACTIVARSE EN SESIONES LARGAS
→ context: fork
   Donde no quieres que añada peso.
```

---

## Slide 8 — Cuándo NO usar context: fork

```
SKILL RÁPIDO Y SIMPLE
└── (genera un componente, formatea un commit)
    → NO fork
       El overhead no compensa.

SKILL CUYO OUTPUT VA A SER INMEDIATAMENTE
MODIFICADO POR EL PRINCIPAL
└── → NO fork
       Mantén la integración tightly acoplada.
```

---

## Slide 9 — Diferencia con subagente, filosóficamente

```
SKILL CON context: fork
└── "Haz esto en otro lado y dame el resultado"
    El skill define UNA TAREA.

SUBAGENTE
└── "Encárgate de cosas como esta"
    El subagente define UN ROL.
```

```
En la práctica son cercanos.

La elección depende de cómo prefieras
modelar tu kit.
```

> Mi recomendación:
> ├── **Subagentes** → roles recurrentes (Reviewer, Tester, Explorer)
> └── **Skills con `context: fork`** → tareas concretas con aislamiento

---

## Slide 10 — Composición de capas: el patrón base

Aquí está el patrón que más rentabilidad da.

```
Un skill ORQUESTA (es el initiator)
Los subagentes EJECUTAN en paralelo o serial
Los MCP servers PROVEEN datos externos
Los artefactos durables sirven de MEMORIA compartida

Cada capa tiene su responsabilidad clara.
```

---

## Slide 11 — El patrón base, visualizado

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

> Cada flecha es un punto donde
> el contexto se aísla o se transfiere.

```
La clave:
el agente principal SOLO ve los resultados destilados.
NO las exploraciones intermedias.
```

---

## Slide 12 — Una nota de vocabulario

```
HIERARCHICAL / SUPERVISORY PATTERN
```

> Este patrón — un orquestador que delega a especialistas
> y sintetiza los resultados — tiene este nombre formal
> en literatura de arquitectura agentic.

```
En esa terminología:
├── el SKILL ORQUESTADOR es el supervisor
└── los SUBAGENTES son los specialists
```

> Si lees whitepapers o presentaciones más teóricas,
> los vas a encontrar así nombrados.

---

## Slide 13 — Caso real: implementar un endpoint de cancelación

Imagina la siguiente petición:

> *"Implementa el endpoint para cancelar pedidos.
> Asegúrate de que respeta nuestras convenciones,
> tiene tests con cobertura razonable,
> y no rompe nada existente."*

**Sin orquestación:**

```
Claude Code haría todo en serie en su contexto principal:
├── leer el OrdersController
├── leer Order.cs
├── leer los tests existentes
├── generar la modificación
├── generar los tests
├── ejecutar...

Y todo eso pesando en la ventana.
```

---

## Slide 14 — El mismo caso, con orquestación

Skill `feature-implementer` que ejecuta 8 pasos:

```
1. Invoca al subagente repo-explorer
   para que mapee la zona del código relevante.
   Vuelve con un resumen.

2. Invoca al subagente feature-planner
   con el resumen del Explorer.
   Devuelve un plan paso a paso.

3. El skill confirma el plan con el usuario
   (esto sí en el principal).

4. IMPLEMENTA LOS CAMBIOS — esta parte sí en el principal
   porque queremos que vea lo que se está modificando.

5. Invoca al subagente test-generator sobre el código nuevo.
   Devuelve los tests generados.

6. Invoca al subagente dotnet-reviewer
   para que valide el conjunto.
   Devuelve hallazgos.

7. Si hay hallazgos críticos:
   el skill los presenta al usuario y propone fixes.

8. Devuelve al principal un resumen de la feature
   con el estado de los tests.
```

---

## Slide 15 — Lo que el contexto principal HA visto

```
✅ La petición inicial
✅ El resumen del Explorer (corto)
✅ El plan del Planner
✅ La modificación real del código
✅ El resumen de tests generados
✅ Los hallazgos del Reviewer
```

---

## Slide 16 — Lo que el contexto principal NO ha visto

```
❌ Las decenas de ficheros que el Explorer leyó

❌ Las hipótesis que el Planner consideró
   y descartó

❌ Las iteraciones internas del Tester

❌ Las exploraciones del Reviewer

Todo eso vivió en sus propios contextos aislados.
```

> **Resultado:** una sesión que ha hecho una feature completa
> con tests y review en el contexto que antes te llevaba
> implementar el endpoint nada más.

---

## Slide 17 — El skill orquestador: cómo se escribe (1/2)

```markdown
---
name: feature-implementer
description: Implementa features completas siguiendo el flujo del equipo:
  explorar, planificar, codificar, testear, revisar. Usar cuando el usuario
  pida implementar una feature de tamaño medio o grande.
allowed-tools: Read, Edit, Write, Bash(dotnet *), Bash(git *)
---

# Implementador de features

Cuando seas invocado para implementar una feature:

## Paso 1: Exploración

Invoca al subagente `repo-explorer` con un objetivo concreto:
"Explora la zona afectada por <feature> y devuelve un resumen
de cómo está organizada."

Espera el resumen.

## Paso 2: Planificación

Invoca al subagente `feature-planner` pasándole el resumen del Explorer:
"Dado este contexto, planifica la implementación de <feature>."

Espera el plan. Preséntalo al usuario y pide confirmación.
```

---

## Slide 18 — El skill orquestador: cómo se escribe (2/2)

```markdown
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
```

> Es bastante prosa, pero el resultado es un workflow
> **estandarizado y reproducible**.

---

## Slide 19 — Loops de retroalimentación

El flujo del ejemplo anterior es **lineal**: paso 1 → paso 2 → ... → paso 6 → fin.

```
Pero la realidad casi nunca es lineal.
```

```
├── El reviewer puede encontrar un problema crítico
│   que invalida la implementación.
├── Los tests pueden fallar y no quedar claro
│   si es bug del código o de los tests.
└── El validator puede pedir reescribir parte del trabajo.
```

> Un harness sin loops es **frágil**.
> Un harness con loops bien diseñados se autocorrige.

---

## Slide 20 — Patrón 1: validator que devuelve al implementer

El más común. El más rentable.

```
Después de implementar:
└── un validator (subagente reviewer) examina el resultado.

Si encuentra gaps:
└── DEVUELVE el flujo al paso de implementación
    con instrucciones específicas de qué arreglar.
```

```
El loop se cierra cuando:
├── el validator aprueba
└── o se alcanza un máximo de iteraciones.
```

---

## Slide 21 — Una nota de vocabulario

```
EVALUATOR-OPTIMIZER
```

> Este patrón — generador que produce, evaluador que critica,
> generador que reescribe basándose en la crítica —
> en literatura formal se llama así.

```
Lo vas a ver así nombrado en cualquier discusión
sobre patrones agentic más allá de Claude Code.
```

```
Aquí lo aplicamos en su versión más concreta:
└── validator → implementer con techo.
```

---

## Slide 22 — Validator → implementer, visualizado

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

> Y cuando el validator aprueba:
> el flujo continúa.
>
> Cuando llega al techo de iteraciones sin aprobación:
> devuelve al usuario para que decida.

---

## Slide 23 — Cómo se escribe en el skill orquestador

```markdown
## Paso 4: Validación con loop

Invoca al subagente `dotnet-reviewer`. Si devuelve hallazgos CRÍTICOS:

1. Aplica los fixes propuestos.
2. Vuelve a invocar al `dotnet-reviewer`.
3. Repite hasta que no haya hallazgos críticos
   o se alcancen 3 iteraciones.
4. Si tras 3 iteraciones siguen apareciendo hallazgos críticos,
   devuelve al usuario con un resumen del problema.
   No sigas iterando ciegamente.
```

> El **límite de iteraciones** es importante.

```
Sin límite:
└── un loop puede entrar en bucles infinitos
    donde el validator y el implementer
    no llegan a acuerdo.

PON SIEMPRE UN TECHO.
```

---

## Slide 24 — Patrón 2: tester que retiene control

Patrón parecido pero específico para tests:

```
El subagente test-generator:
├── corre los tests
├── si fallan, analiza si es problema del código o de los tests
├── ajusta lo que toque
└── reintenta.

SOLO devuelve al principal cuando:
├── los tests pasan
└── o concluye que no van a pasar.
```

> Esto ya lo vimos en 3.1 en el ejemplo del subagente test-generator:
>
> *"Máximo 3 iteraciones antes de devolver al principal"*.
>
> Esa es la pieza del loop.

---

## Slide 25 — Patrón 3: plan re-validation

El más overkill, pero brutal cuando se justifica:

```
Cuando un planner produce un plan,
ANTES de ejecutarlo:

└── otro subagente lo VALIDA.
    ├── Busca riesgos no contemplados
    ├── Dependencias olvidadas
    └── Decisiones implícitas que no están justificadas.
```

```
Si el validator lo rechaza:
└── el planner reformula.

Solo cuando el validator lo aprueba:
└── se procede.
```

> Esto es overkill para features pequeñas.
>
> Pero **brutal para features grandes**
> donde una mala decisión inicial cuesta horas o días.

---

## Slide 26 — Loops a coste cero: cómo medirlos

Cada vuelta de un loop es:

```
├── Una invocación más a un subagente
├── Más tokens
└── Más tiempo
```

```
Conviene tener idea del coste
ANTES de poner loops alegremente.
```

---

## Slide 27 — Heurística práctica para loops

```
LOOPS CORTOS (max 2-3 iteraciones)
├── Son baratos
└── Suelen rentar SIEMPRE.
    ├── El coste extra es pequeño
    └── La fiabilidad mejora mucho.

LOOPS LARGOS (5+ iteraciones)
└── Rara vez compensan.
    
    Si necesitas tantas vueltas:
    └── normalmente el problema es la calidad
        del subagente o del plan inicial,
        NO la cantidad de iteraciones.
```

> La regla:
> **pon loops, pero siempre con techo.**
>
> Y si te encuentras subiendo el techo
> porque el loop no converge:
> el problema está en otra parte.

---

## Slide 28 — Lo que tienes ahora

```
✅ Aislamiento ligero con context: fork en skills
✅ Composición de capas (skill orquestador + subagentes)
✅ El patrón hierarchical / supervisory
✅ Caso real desarrollado: feature-implementer
✅ Loops de retroalimentación (3 patrones)
✅ Loops con techo siempre
✅ Heurística para medir el coste de los loops
```

> Tienes la mitad del repertorio de orquestación.

---

## Slide 29 — Lo que falta para completar el harness orquestado

```
1. CONTEXT BANK
   Cómo varios subagentes comparten información
   sin pasarse prompts gigantescos.

2. PARALELO vs SERIAL
   Cuándo lanzar subagentes en paralelo
   y cuándo en serie.

3. CLAUDE CODE COMO MCP SERVER
   Para integraciones con sistemas externos.

4. AGENT TEAMS
   Múltiples sesiones de Claude Code coordinándose.
   Lo que la comunidad llamaba "Swarm".

5. CUÁNDO USAR QUÉ — el árbol de decisión final.
```

---

## Slide 30 — Antes de seguir: una idea importante

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   La orquestación es una herramienta,                    │
│   NO un objetivo.                                        │
│                                                          │
│   Si tu tarea es simple:                                 │
│   no la compliques con un workflow de cinco capas.       │
│                                                          │
│   Empieza simple.                                        │
│   Escala cuando el caso lo justifique.                   │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

---

## Slide 31 — La pregunta que cierra esta parte

```
¿Tienes ya un caso en mente para tu equipo
donde un skill orquestador con 2-3 subagentes
sumaría valor real?
```

```
Pistas:
├── Workflows que se ejecutan varias veces a la semana
├── Procesos con varios pasos donde la calidad importa
└── Tareas donde hoy hay mucho ir y venir
    entre el dev y la herramienta
```

> Si tienes uno claro, lo de 3.2b va a aterrizar mejor.
> Especialmente la parte del **context bank**.

---

## Slide 32 — Lo que viene en 3.2b

```
SUBMÓDULO 3.2b — MEMORIA, PARALELO, MCP, AGENT TEAMS
─────────────────────────────────────────────────────

CONTEXT BANK
├── El problema (compartir info entre subagentes)
├── Las dos formas (mala vs buena)
├── Estructura típica
├── Ventajas (trazabilidad, recuperación, loops baratos, auditoría)
├── Limpieza
└── Diferencia con CLAUDE.md

PARALELO vs SERIAL (sección nueva v3)
├── Fan-out / fan-in
├── Decisión rápida
└── El error típico

CLAUDE CODE COMO MCP SERVER
├── 3 casos de uso
├── Cómo se activa
└── Cuándo NO usarlo

AGENT TEAMS
├── Aclaración terminológica (vs "Swarm")
├── Cómo funciona
├── Cuándo merece la pena
├── La cifra: 10-15x más tokens
└── Estado experimental

CIERRE
├── Árbol de decisión final
├── Anti-patrones de orquestación
├── Errores frecuentes con flujos compuestos
└── Bridge a 3.3 (hooks)
```

**Nos vemos en 3.2b.**
