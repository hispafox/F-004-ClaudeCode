> **Versión:** v2 | **Módulo:** 5 | **Sub:** 5.3b | **Slides:** 45 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M05-S5.3b-tests-estrategia-workflow-cierre-curso-v2.md`

# Submódulo 5.3b — Tests en .NET: estrategia, workflow y cierre del curso

## Slide 1 — Portada
**Módulo 5 · Submódulo 5.3 · Parte B · Cierre del curso**
Estrategia de cobertura, workflow completo y cierre del curso
XML docs útiles, cobertura inteligente, sistema de testing del equipo, despedida

---

## Slide 2 — Dónde estamos

En 5.3a vimos por qué tests es donde Claude Code rinde tanto, el CLAUDE.md como contrato del equipo, tests unitarios con xUnit + NSubstitute + FluentAssertions, el antipatrón estrella de tests que no testean nada, tests de integración con WebApplicationFactory, el caso práctico guiado, code smells, y empezamos con la regla de documentación XML.

Ahora cerramos el curso entero:

```
1. Documentación XML útil + Swagger
   (cerrar el bloque del 5.3a)

2. ESTRATEGIA DE COBERTURA
   (tests útiles vs tests para inflar)

3. WORKFLOW COMPLETO
   (sistema de testing del equipo)

4. ANTI-PATRONES de testing con IA

5. ERRORES FRECUENTES con tu primera semana

6. CIERRE DEL MÓDULO Y DEL CURSO ENTERO
```

---

## Slide 3 — XML doc: el antes

Recordatorio del problema clásico que vimos al final de 5.3a.

**Lo que Claude Code va a generar SIN regla específica:**

```csharp
/// <summary>
/// Creates the order.
/// </summary>
/// <param name="request">The request.</param>
/// <returns>The order.</returns>
public async Task<Order> CreateOrderAsync(
    CreateOrderRequest request) { ... }
```

```
Es comentario por kilo, NO por valor.
```

> Repite el nombre del método con palabras distintas.
> NO añade nada que el lector no supiera ya leyendo la firma.

---

## Slide 4 — XML doc: el después con la regla del CLAUDE.md

```csharp
/// <summary>
/// Creates a new order, validates against business rules, 
/// and queues confirmation email asynchronously. 
/// The email is fire-and-forget — failures don't roll back the order.
/// </summary>
/// <param name="request">Request payload. CustomerId must reference 
/// an active customer; orders for blocked customers throw.</param>
/// <returns>The created order with assigned Id and timestamps.</returns>
/// <exception cref="CustomerNotFoundException">Thrown when the 
/// CustomerId doesn't exist.</exception>
/// <exception cref="CustomerBlockedException">Thrown when the 
/// customer is blocked or suspended.</exception>
/// <exception cref="ValidationException">Thrown when the request 
/// fails FluentValidation rules.</exception>
public async Task<Order> CreateOrderAsync(
    CreateOrderRequest request) { ... }
```

```
DIFERENCIA: el segundo es realmente ÚTIL.
Te dice cosas que NO se ven en la firma.
```

> Comportamiento async fire-and-forget.
> Restricción del CustomerId.
> Excepciones explícitas con sus condiciones.
>
> Eso es información que un consumidor de la API
> realmente necesita.

---

## Slide 5 — Anotaciones para Swagger / OpenAPI

```
Misma idea aplicada a anotaciones de Swagger.
```

**Los atributos** `[ProducesResponseType]`, `[Produces]`, etc. **son tediosos de mantener** pero hacen que la doc generada sea muy útil para consumidores de la API.

**El prompt:**

```
"Genera anotaciones Swagger completas 
 para OrdersController. Para cada endpoint:

 ├── Anota TODOS los códigos de respuesta posibles 
 │   con su tipo de respuesta
 ├── Indica el Content-Type producido y aceptado
 ├── Incluye descripciones 
 │   (NO genéricas — algo que añada valor 
 │    al consumidor de la API)
 └── Anota requisitos de autenticación si aplican"
```

```
OUTPUT
└── anotaciones completas, código documentado de forma que
    cuando alguien abra el Swagger UI
    ve algo PROFESIONAL y ÚTIL.
```

---

## Slide 6 — Estrategia de cobertura: la pregunta inevitable

```
La pregunta que toda discusión de testing acaba en:
```

```
¿100% de cobertura?
```

```
Respuesta corta:
```

```
NO.
```

```
Respuesta larga:
└── depende del código,
    y aquí es donde la IA mal usada
    puede convertir tu codebase
    en una pesadilla.
```

---

## Slide 7 — El antipatrón clásico de cobertura

```
Equipo decide que quiere 80% de cobertura.
Pones a Claude Code a generar tests masivamente.
```

**Claude genera tests para todo:**

```
├── getters/setters triviales
├── métodos que son wrappers de otros métodos
├── controladores que solo llaman a handlers
│   (los handlers ya están testeados aparte)
└── cualquier línea de código no cubierta
```

---

## Slide 8 — El resultado del antipatrón

```
✅ La cobertura sube a 80%. Métrica conseguida.
```

```
PERO:
```

```
├── Ahora tienes 500 tests
│   que casi NINGUNO detecta bugs cuando algo se rompe.
│
├── Cuando refactorices,
│   tienes que actualizar 500 tests,
│   la mayoría de los cuales NO añaden valor.
│
└── La suite tarda 5 MINUTOS en ejecutarse en CI
    cuando antes tardaba 30 SEGUNDOS.
```

```
HAS EMPEORADO TU CODEBASE
EN NOMBRE DE UNA MÉTRICA.
```

> Esto es lo que la gente llama **test slop**:
>
> el equivalente del code slop
> pero para tests.

---

## Slide 9 — Cómo pedir cobertura inteligente: 3 principios

```
1. COBERTURA POR CAPA, NO POR NÚMERO.
2. COBERTURA DE COMPORTAMIENTO, NO DE LÍNEAS.
3. LA COBERTURA ES RESULTADO, NO OBJETIVO.
```

> Los vemos uno a uno.

---

## Slide 10 — Principio 1: cobertura por capa

Algunas capas merecen mucha cobertura, otras casi ninguna. Tabla orientativa:

| Capa | Cobertura objetivo | Razón |
|---|---|---|
| **Domain** (lógica de negocio pura) | 90%+ | Es donde más bugs duelen, y es fácil testear |
| **Application handlers** (CQRS) | 80%+ | Donde se orquesta la lógica |
| **Validators** (FluentValidation) | 100% | Cada regla debe tener su test |
| **Controllers** | 30-50% | Tests de integración cubren más que unitarios |
| **Mappers** (AutoMapper / Mapster) | Bajo | Unit tests no cubren bien — mejor integration |
| **Infraestructura** (repos EF, HttpClients) | Bajo | Tests de integración cubren mejor |
| **DTOs / contracts** | 0% | NO hay nada que testear |

---

## Slide 11 — Principio 2: cobertura de comportamiento, no de líneas

```
El número de "líneas cubiertas" es ENGAÑOSO.
```

**Ejemplo:**

```
Un método con 10 líneas
pero 5 caminos lógicos
└── necesita 5 TESTS,
    NO 1 que lo recorra entero.
```

```
La métrica útil es:

├── BRANCH COVERAGE
└── PATH COVERAGE

NO line coverage.
```

> Branch coverage te dice si tus tests
> están cubriendo los `if`/`else`/`switch`/`try/catch`.
>
> Line coverage solo te dice si la línea se ejecutó.
> Una línea con un `if` que nunca toma la rama del `else`
> está "cubierta" pero el `else` no.

---

## Slide 12 — Principio 3: la cobertura es resultado, no objetivo

```
EL OBJETIVO ES:

"tests útiles que detectan bugs
 cuando algo se rompe"
```

```
LA COBERTURA ES LA CONSECUENCIA.
```

```
Si optimizas para la consecuencia
(subir el número)
└── pierdes el sentido.
```

> Tests útiles **producen** buena cobertura.
> Buena cobertura **NO produce** tests útiles.

---

## Slide 13 — Prompt para cobertura inteligente

```
"Quiero subir la cobertura de tests de OrdersService.
La cobertura actual es 40%.

Mi objetivo NO es alcanzar un porcentaje,
es cubrir COMPORTAMIENTOS que actualmente NO están testeados
y que SÍ valen la pena.

Por favor:

1. Identifica los métodos públicos que NO tienen tests.

2. Para cada uno, lista los CAMINOS LÓGICOS (branches)
   que NO están cubiertos.

3. Para cada camino, decide si es un comportamiento
   que vale la pena testear:
   ├── SÍ si es lógica de negocio, validación, 
   │   manejo de errores específico.
   └── NO si es un wrapper trivial, getter/setter, 
       o lógica del framework.

4. Para los que SÍ valen la pena,
   genera tests siguiendo CLAUDE.md.

5. REPORTA los que decidiste NO testear y POR QUÉ.

NO optimices para subir un porcentaje.
Optimiza para que cuando algo se rompa,
el test te lo diga."
```

```
OUTPUT
└── una suite de tests que TE DICE CUÁNDO ALGO SE ROMPE,
    NO una que te dice "100%".
```

---

## Slide 14 — Qué métricas mirar en serio

Las que importan, una vez tienes tests útiles:

```
BRANCH COVERAGE > LINE COVERAGE
└── Más informativa.

TIEMPO DE EJECUCIÓN DE LA SUITE
└── Si crece linealmente con cada feature,
    HAY ALGO MAL.

TASA DE TESTS ROTOS EN CI
CUANDO ALGO SE CAMBIA
└── Si cambias UNA SOLA cosa
    y se rompen 50 tests no relacionados:
    └── los tests están acoplados a IMPLEMENTACIÓN,
        no a COMPORTAMIENTO.

BUGS DETECTADOS POR TESTS
EN PRE-PRODUCCIÓN VS EN PRODUCCIÓN
└── Métrica final de utilidad.
```

> Si la mayoría de los bugs aparecen en producción
> y nunca en pre-prod,
> tus tests están bonitos pero ciegos.

---

## Slide 15 — Workflow completo: el sistema de testing del equipo

```
Cierre operativo.
```

> Cómo se integra TODO en el día a día.

**Tres piezas:**

```
PIEZA 1
└── El subagente test-generator

PIEZA 2
└── El hook PostToolUse para test on save

PIEZA 3
└── El flujo de feature completa
    (donde encajan todas las piezas del curso)
```

---

## Slide 16 — Pieza 1: el subagente test-generator

Recordando el módulo 3, la convención de tener un subagente dedicado a tests:

```yaml
---
name: test-generator
description: Genera tests unitarios y de integración 
  para código .NET siguiendo el patrón del equipo 
  (xUnit + NSubstitute + FluentAssertions). 
  Usar cuando se necesite generar suite de tests 
  para un componente, servicio, handler, o controller existente.
tools: Read, Grep, Write, Edit, Bash(dotnet test *)
model: sonnet
---

[system prompt detallado del rol del subagente, 
 las convenciones, el loop de iteración 
 hasta que pasen los tests]
```

```
Cuando el alumno pide "genera tests para X":

├── este subagente se invoca AUTOMÁTICAMENTE
├── genera, ejecuta, ITERA
└── devuelve al principal SOLO cuando los tests pasan
    o cuando concluye que NO van a pasar.
```

---

## Slide 17 — Pieza 2: el hook PostToolUse para test on save

```json
{
  "hooks": {
    "PostToolUse": [
      {
        "matcher": "Write|Edit|MultiEdit",
        "hooks": [
          {
            "type": "command",
            "command": "$CLAUDE_PROJECT_DIR/.claude/hooks/test-affected.sh",
            "timeout": 60
          }
        ]
      }
    ]
  }
}
```

```
El script test-affected.sh
├── detecta qué tests se ven afectados por el cambio
│   (los del fichero modificado y los que dependen de él)
└── los EJECUTA.

Si fallan:
└── devuelve exit 2 con el output del test runner
    └── Claude Code recibe el feedback
        y puede ajustar.
```

---

## Slide 18 — Lo que esto cambia

```
ANTES:

"escribir código → ejecutar tests manualmente →
 ver fallos → ajustar"
```

```
DESPUÉS:

"escribir código → tests se ejecutan SOLOS →
 si fallan, el agente lo SABE inmediatamente"
```

> Es la diferencia entre programación manual
> y programación con feedback continuo.

---

## Slide 19 — Pieza 3: el flujo de feature completa

Combinando todo lo del curso, el flujo end-to-end:

```
1. PETICIÓN
   "Implementa la feature de cancelación de pedidos"

2. SKILL feature-implementer se activa (de 3.2). 
   Orquestador.

3. Subagente repo-explorer 
   mapea la zona afectada.

4. Subagente feature-planner 
   genera plan paso a paso.

5. Plan presentado al usuario, CONFIRMACIÓN.

6. IMPLEMENTACIÓN del código del feature 
   en el principal.
```

---

## Slide 20 — Continuación del flujo

```
7. Subagente test-generator INVOCADO 
   para los nuevos handlers, controllers, validators.

8. Hook PostToolUse ejecuta los tests AUTOMÁTICAMENTE 
   al modificar.

9. Si fallan, agente principal AJUSTA hasta que pasen.

10. Subagente dotnet-reviewer AUDITA 
    el resultado completo.

11. Hallazgos críticos SE APLICAN.

12. RESUMEN final al usuario:
    ├── ficheros modificados
    ├── tests añadidos
    └── hallazgos del reviewer.
```

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   Lo que era trabajo de 2-3 DÍAS                         │
│   ahora es trabajo de 2-3 HORAS,                         │
│                                                          │
│   con calidad consistente, tests, audit,                 │
│   y todo respetando las convenciones del equipo.         │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

---

## Slide 21 — Integración con CI

```
Cuando este flujo es estable internamente,
el siguiente paso es INTEGRAR CON CI.
```

```
├── Cada PR ejecuta el dotnet-reviewer AUTOMÁTICAMENTE
│   y comenta hallazgos en GitHub.
│
├── Cobertura se trackea pero NO se usa como gate
│   (recordar: cobertura es resultado, NO objetivo).
│
└── Los hooks que se aplican en local
    también se aplican en CI
    (formato, lint, validaciones).
```

> Esto cierra el círculo.
>
> El equipo entero opera bajo las mismas reglas,
> automatizadas, sin que cada dev tenga que recordarlas.

---

## Slide 22 — Anti-patrones de testing con IA (1/2)

Compilación de los principales que se ven en equipos que adoptan estas herramientas:

```
TEST SLOP SISTEMÁTICO
└── Generar tests masivamente sin criterio.
    Cobertura sube, valor real baja.
    Solución: las tres prácticas que vimos
    (tests de comportamiento, justificar cada test,
     regla en CLAUDE.md de tests prohibidos).

TESTS QUE SON LA IMPLEMENTACIÓN AL REVÉS
└── Tests que verifican la implementación interna
    en lugar del comportamiento observable.
    Cuando refactorices, se rompen
    aunque el comportamiento siga siendo correcto.
    Solución: testear contra interfaces y observables,
    NO contra detalles internos.

MOCK EVERYTHING
└── Servicios pequeños que ni siquiera necesitarían mocks
    (objetos de dominio puros, value objects)
    terminan mockeados porque "es lo que se hace en tests".
    Solución: regla en CLAUDE.md de cuándo mockear y cuándo no.

TESTS QUE REQUIEREN MANTENIMIENTO PERMANENTE
└── Cada cambio del código requiere actualizar 20 tests.
    La suite se vuelve un COSTE, no un activo.
    Solución: el problema casi siempre está en
    tests acoplados a implementación. REFACTOR.
```

---

## Slide 23 — Anti-patrones de testing con IA (2/2)

```
SUITES QUE TARDAN MINUTOS EN EJECUTARSE
└── Tests unitarios que tardan milisegundos individualmente
    pero la suite se va a 3-5 minutos.
    Solución: identificar tests lentos
    (dotnet test --logger:"console;verbosity=detailed")
    y revisar — normalmente son de integración
    disfrazados de unitarios.

NO REVISAR TESTS AUTOGENERADOS
└── Generas con Claude, comiteas, te olvidas.
    Tests con bugs entran al repo.
    Solución: code review aplica a tests
    igual que a código de producción.

PRETENDER QUE TESTS SUSTITUYEN
AL CRITERIO DEL DEV
└── Aunque tengas 95% de cobertura,
    sigues necesitando que un humano piense
    si la feature es correcta.
    
    Tests son una RED DE SEGURIDAD,
    NO la verificación principal de que algo funciona.
```

---

## Slide 24 — Errores frecuentes con tu primera semana (1/2)

```
❌ ACEPTAR EL OUTPUT SIN EJECUTARLO
   Los tests pueden COMPILAR pero NO PASAR.
   Siempre verifica que pasan antes de comitear.

❌ NO ACTUALIZAR LOS SNAPSHOTS / APPROVAL TESTS
   CUANDO EL OUTPUT LEGÍTIMO CAMBIA
   Si usas approval testing,
   los tests fallan tras cambios visibles
   incluso cuando son cambios DESEADOS.
   Actualiza los baselines conscientemente.

❌ NO TENER UN SEEDER DE DATOS DE PRUEBA
   Cada test crea sus datos desde cero.
   Lento, repetitivo, y los tests acaban desincronizados
   en cómo construyen entidades.
   Solución: TestDataSeeder o TestDataFactory reutilizable.

❌ OLVIDAR CancellationToken EN MÉTODOS ASYNC
   Tu CLAUDE.md dice que se usa siempre,
   pero si NO lo refuerzas,
   Claude lo omite a veces.
   Revisa.
```

---

## Slide 25 — Errores frecuentes con tu primera semana (2/2)

```
❌ TESTS CON TIEMPOS HARDCODEADOS
   "El timestamp del test debe ser 2024-01-15".
   Funciona hoy, falla en CI cuando los relojes
   están desincronizados o en Daylight Saving.
   Solución: inyectar IDateTimeProvider y mockearlo.

❌ TESTS QUE DEPENDEN DEL ORDEN DE EJECUCIÓN
   Si el test 5 asume que el test 4 corrió antes,
   eso es BUG.
   Tests deberían ser INDEPENDIENTES.
   xUnit los corre en paralelo por default
   — los tests dependientes fallarán intermitentemente.

❌ NO CORRER LA SUITE COMPLETA ANTES DE UN PUSH
   "Mis tests pasan, los demás también pasarán"
   — NO siempre.
   Acostúmbrate a "dotnet test" antes de cada push,
   o configúralo como hook pre-push.

❌ NO REVISAR LA COBERTURA PERIÓDICAMENTE
   Mes a mes, suelen aparecer zonas
   que han ido quedando sin tests.
   Una revisión MENSUAL de cobertura
   ayuda a no acumular deuda.
```

---

## Slide 26 — Cierre del módulo y del curso

```
Llegamos al final.
```

```
Hemos cubierto en 10 HORAS mucho material.

Conviene cerrar con la imagen entera
para que os vayáis con el MAPA MENTAL CLARO.
```

---

## Slide 27 — Los 5 módulos en una frase cada uno

```
MÓDULO 1
└── Claude Code es un AGENTE, no un asistente.
    Lo configuras con CLAUDE.md y .claude/settings.json,
    lo invocas en tres modos,
    y le das permisos según el caso.

MÓDULO 2
└── SKILLS son las capacidades modulares.
    Te enseñan a tu equipo.
    Frontmatter YAML, descripción que dispara,
    scopes user vs proyecto,
    ecosistema oficial y comunitario.

MÓDULO 3
└── Tu AGENT HARNESS.
    Subagentes (workers),
    orquestación (composición + loops + context bank),
    hooks (capa determinista).
    Las tres piezas convierten Claude Code
    en herramienta a medida.
```

---

## Slide 28 — Los 5 módulos (continuación)

```
MÓDULO 4
└── DISEÑO INTEGRADO.
    Figma MCP para diseños existentes,
    Claude Design para creación visual,
    DESIGN.md como pegamento.
    Los tres flujos típicos según tu equipo.

MÓDULO 5
└── HANDOFF y TESTING.
    Loop cerrado Claude Design → Claude Code
    para implementación.
    Suite de tests xUnit + NSubstitute + FluentAssertions
    con criterio de comportamiento,
    NO cobertura ciega.
```

---

## Slide 29 — El kit Claude Code del alumno

```
Lo que cada uno se lleva, MATERIALIZADO:
```

```
1. CLAUDE.md y .claude/settings.json
   afinados para tu stack
   — del MÓDULO 1.

2. 2-3 SKILLS PROPIOS
   (componente Angular, code review .NET)
   — del MÓDULO 2.

3. SUBAGENTE EXPLORER + 2 HOOKS
   (auto-format, bloqueo de comandos peligrosos)
   — del MÓDULO 3.

4. PROTOTIPO en CLAUDE DESIGN + DESIGN.md
   del equipo configurado
   — del MÓDULO 4.

5. SUITE DE TESTS xUnit AUTOGENERADA
   para una API .NET
   — del MÓDULO 5.
```

```
CINCO ARTEFACTOS.

Para mostrar al equipo el lunes.
```

---

## Slide 30 — Qué hacer el lunes: acción 1

Tres acciones concretas, en orden de prioridad.

```
ACCIÓN 1 (LUNES MISMO):

APLICA LOS HOOKS.

├── Auto-format en PostToolUse
└── Bloqueo de comandos peligrosos en PreToolUse
```

```
Es lo que MÁS RENTABILIDAD INMEDIATA da
y se configura en 30 minutos.
```

> Mañana, cuando llegues al ordenador.
> Antes de cualquier otra cosa.
>
> Treinta minutos de configuración.
> Beneficio el resto de tu carrera con Claude Code.

---

## Slide 31 — Qué hacer el lunes: acción 2

```
ACCIÓN 2 (PRIMERA SEMANA):

GENERA TESTS CON CLAUDE CODE
PARA EL MÓDULO DE TU CODEBASE
CON MENOS COBERTURA
Y MÁS BUGS HISTÓRICOS.
```

```
Es el caso de uso MÁS RENTABLE
de la herramienta.
```

> Si tienes una zona de código que llevaba meses
> esperando tests y nadie se ponía,
>
> esa es la candidata.
>
> Una sesión de 1-2 horas con Claude Code
> y sales con cobertura razonable
> donde antes había desierto.

---

## Slide 32 — Qué hacer el lunes: acción 3

```
ACCIÓN 3 (PRIMER MES):

MONTA EL FLUJO DE DISEÑO con DESIGN.md.
```

```
├── Si tienes Figma:
│   sincroniza vía MCP.
│
└── Si NO tienes Figma:
    genera uno con Stitch o Claude Design.

Y commitea ese fichero al repo.
```

> Tu agente y todo el ecosistema lo van a usar.

```
Una hora de setup para una pieza
que va a estar dando rentabilidad
durante meses.
```

---

## Slide 33 — Qué NO hacer

```
NO INTENTES ADOPTAR TODO EL PRIMER DÍA.
└── La curva de aprendizaje es REAL.
    Skills, subagentes y hooks son piezas
    que rinden cuando están BIEN,
    no cuando están improvisadas.

NO TOMES EL OUTPUT COMO PRODUCTION-READY
SIN REVISAR.
└── Es código de Claude Code,
    NO código auditado.
    La revisión sigue siendo TU TRABAJO.

NO ABANDONES EL OFICIO.
└── La herramienta acelera lo MECÁNICO.
    El criterio sigue siendo HUMANO.
    Diseño crítico, decisiones arquitectónicas,
    performance fina
    — siguen necesitándote.

NO SUBESTIMES EL COSTE.
└── Especialmente con Claude Design.
    Monitoriza con /usage.
    Si te quedas sin tokens a mitad de iteración,
    perder el hilo es FRUSTRANTE.
```

---

## Slide 34 — Para continuar aprendiendo

Tres recursos que conviene tener a mano:

```
DOCUMENTACIÓN OFICIAL DE ANTHROPIC
└── La fuente más actualizada,
    especialmente para features experimentales.

awesome-claude-code Y awesome-agent-skills EN GITHUB
└── Comunidad de skills, subagentes
    y configuraciones probadas.

EL BLOG TÉCNICO DE ANTHROPIC
└── Anuncios de productos, cambios en el modelo,
    mejores prácticas que van saliendo.
```

```
Y, por supuesto, PROBARLO.
```

> La herramienta cambia rápido.
>
> Lo que en este curso es estado del arte hoy,
> en 6 meses puede tener 3 features nuevas.
>
> La forma de mantenerse al día es **usándola**.

---

## Slide 35 — La pregunta final

```
Hemos terminado.
```

```
Una pregunta para llevarse:
```

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   ¿Qué pieza concreta de este curso                      │
│   vas a aplicar al equipo el lunes que viene?            │
│                                                          │
│   NO TRES, NO CINCO                                      │
│                                                          │
│   UNA.                                                   │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

---

## Slide 36 — Tres respuestas posibles

```
SI LA RESPUESTA ES "EL HOOK DE AUTO-FORMAT":
└── PERFECTO.
    Es la apuesta más segura de las 10 horas.

SI ES "MONTAR UN DESIGN.MD DESDE NUESTRA CONFIG DE TAILWIND":
└── TAMBIÉN.
    Y vas a notar el efecto en cómo Claude genera UI
    desde la primera semana.

SI ES "GENERAR TESTS PARA EL MÓDULO X 
QUE LLEVABA MESES SIN COBERTURA":
└── IGUAL DE BIEN.
    Es el caso donde más rentabilidad inmediata vas a ver.
```

```
Cualquiera de las tres es buena.
```

```
Pero UNA SOLA.
```

---

## Slide 37 — La estrategia de adopción

```
LA ADOPCIÓN REAL PASA POR
INTEGRAR BIEN UNA PIEZA
ANTES DE METER LA SIGUIENTE.
```

```
Cuando esa esté pulida en tu flujo
└── la siguiente.

Cuando esa esté
└── la tercera.
```

```
EN TRES MESES TIENES EL KIT ENTERO RODANDO.
```

> Esto NO es marketing.
>
> Es la diferencia que hemos visto
> en equipos que adoptan Claude Code:

---

## Slide 38 — Lo que separa a los equipos

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   Los equipos que prueban Claude Code                    │
│   y lo abandonan                                         │
│                                                          │
│   vs                                                     │
│                                                          │
│   los que lo adoptan en serio                            │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

```
La diferencia NO es:
└── la cantidad de features que intentan.
```

```
La diferencia ES:
└── la DISCIPLINA de adoptar UNA bien
    antes de pasar a la siguiente.
```

---

## Slide 39 — La pregunta que cierra el curso

```
Y con esa pregunta:
```

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   ¿Qué UNA pieza vas a aplicar el lunes?                 │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

```
y con la respuesta concreta TUYA en la cabeza:
```

> cerramos las 10 horas.

---

## Slide 40 — Lo que tienes ahora

```
Tienes los 5 artefactos del KIT CLAUDE CODE:

✅ CLAUDE.md y settings.json para tu stack
✅ 2-3 skills propios
✅ Subagente Explorer + 2 hooks
✅ Prototipo Claude Design + DESIGN.md
✅ Suite de tests xUnit
```

```
Tienes el mapa MENTAL de los 5 módulos:

✅ El agente y su personalización
✅ Skills modulares
✅ Subagentes, orquestación, hooks
✅ Diseño integrado (Figma + Claude Design + DESIGN.md)
✅ Handoff y testing
```

```
Y tienes UNA respuesta concreta:

✅ La pieza que vas a aplicar el lunes.
```

---

## Slide 41 — La verdad de Claude Code

```
Claude Code NO es magia.
```

```
NO escribe código por vosotros mientras dormís.
NO entiende vuestro negocio mejor que vosotros.
NO sustituye al criterio del dev senior.
```

> Lo que hace es **eliminar fricción**.

```
La fricción de escribir tests aburridos
La fricción de actualizar documentación
La fricción de configurar un nuevo proyecto
La fricción de mantener convenciones del equipo
La fricción de pasar de diseño a código
```

```
Y cuando la fricción baja:
└── el equipo PRODUCE MÁS
    con la MISMA energía mental.
```

> Esa es la promesa.
>
> Y es **real**.

---

## Slide 42 — La promesa real para devs .NET + Angular

```
Para vosotros, devs .NET + Angular:
```

```
EL CASO DE USO MÁS RENTABLE
└── tests en .NET.
    Empezad por ahí.

LA HERRAMIENTA MÁS POTENTE
└── el agent harness
    (subagentes + hooks + skills).
    Construidlo gradualmente.

LA APUESTA A LARGO PLAZO
└── el flujo integrado de diseño con DESIGN.md.
    Configuradlo una vez, beneficio por años.
```

> Tres horizontes temporales.
>
> Tres prioridades.
>
> Tres decisiones que podéis tomar el lunes.

---

## Slide 43 — Lo que viene después del curso

```
Anthropic publica features nuevas
prácticamente cada semana.
```

**Cosas que probablemente van a aparecer en los próximos meses:**

```
├── Claude Design saliendo de research preview
├── DESIGN.md llegando a versión estable
├── Más skills oficiales del ecosistema
├── Mejor integración con CI/CD
└── Capacidades multi-agente más maduras
```

```
La forma de no quedarse atrás:
```

```
USAR LA HERRAMIENTA SEMANALMENTE.

Cuando aparezca algo nuevo:
└── la primera reacción
    NO va a ser "¿qué es esto?"
    
    Va a ser "ah, claro, esto encaja aquí".
```

> Eso pasa cuando ya estás en el flujo.

---

## Slide 44 — Gracias

```
GRACIAS por vuestras 10 horas.
```

```
Espero que el lunes que viene
sea el primer día de muchos
con Claude Code rodando
en vuestro equipo.
```

```
Cuando llevéis dos meses
y vuestro DESIGN.md viva en el repo,
y vuestros hooks corran solos,
y vuestros tests se generen con un prompt corto
─

acordaos de este momento.
```

> El día que decidisteis:
>
> *"vale, voy a aplicar UNA pieza el lunes
> y construyo desde ahí"*.

---

## Slide 45 — Fin del curso

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│                                                          │
│   FIN                                                    │
│                                                          │
│   Curso Claude Code para devs .NET + Angular             │
│                                                          │
│                                                          │
│   ✅ 5 módulos                                            │
│   ✅ 10 horas                                             │
│   ✅ El kit completo                                      │
│                                                          │
│                                                          │
│   El lunes empieza la implementación.                    │
│                                                          │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

> Suerte.
>
> Y nos vemos en la siguiente versión del curso
> cuando el ecosistema haya cambiado lo suficiente
> como para que merezca la pena.
