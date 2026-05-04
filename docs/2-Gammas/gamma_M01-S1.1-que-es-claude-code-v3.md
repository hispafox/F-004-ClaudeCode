> **Versión:** v3 | **Módulo:** 1 | **Sub:** 1.1 | **Slides:** 48 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M01-S1.1-que-es-claude-code-v3.md`

# Submódulo 1.1 — ¿Qué es Claude Code? Arquitectura y filosofía

## Slide 1 — Portada
**Módulo 1 · Submódulo 1.1**
¿Qué es Claude Code?
Arquitectura y filosofía

---

## Slide 2 — El malentendido del primer día

Hay una escena que se repite en casi todos los equipos que se acercan a Claude Code:

```
Día 1
├── Alguien lo instala
├── Le pide algo trivial: "añade un test al método X"
├── Claude Code se pone a leer ficheros
├── Mira el contexto. Lee otro fichero. Tarda
└── Cuando responde, ha tocado un fichero auxiliar
    y sugerido cambiar otro

Reacción típica:
"Esto es Copilot pero más lento y más metomentodo"

Día 3 → desinstalado
```

Y luego está el caso opuesto. Menos común, pero igual de real. Esa misma persona, dos semanas más tarde, llega al equipo con una métrica: **18 issues cerrados en un sprint donde antes cerraba 6**. La diferencia, según ella, no es que escriba más rápido. Es que ha dejado de hacer las partes que más tiempo le quitaban — los tests, los DTOs, la documentación, los refactors pequeños — y se centra solo en lo que pide su criterio.

*"He aprendido a delegar"*, lo resume.

---

## Slide 3 — La misma persona, dos momentos

Estos dos casos no son personas distintas con habilidades distintas. **Son la misma persona en dos momentos**.

Lo que cambia entre el día 3 y la semana 2 es **haber entendido qué tipo de herramienta es esto**. Cuando esa pieza encaja, todo lo demás se asienta solo. Cuando no encaja, da igual que hagas un máster en skills y subagentes — el resultado va a ser frustración.

Por eso este apartado, antes que nada técnico, es conceptual. Te ahorra las dos semanas de fricción.

> Lo que viene en los próximos 30 minutos no es cómo instalarlo.
> Es entender qué es y cómo se piensa con ello.

---

## Slide 4 — La frase corta

> **Un asistente sugiere. Un agente actúa.**

Esa es la diferencia entera. Todo lo demás se deriva.

Cada herramienta de IA para programadores opera bajo un contrato implícito sobre quién lleva la iniciativa. Y ese contrato — aunque casi nunca lo verbalicen los productos — condiciona cómo se usa la herramienta y qué puedes esperar de ella.

Los próximos slides bajan esto a tierra con un ejemplo concreto: la misma tarea, tres herramientas distintas.

---

## Slide 5 — La misma tarea con Copilot

*"Añade un endpoint GET para devolver un pedido por ID."*

```
1. Abres OrdersController.cs
2. Empiezas a escribir [HttpGet("...")]
3. Copilot sugiere el resto de la línea
4. Sigues con public async Task<...>
5. Copilot intenta completar el tipo de retorno
6. Tú tecleas. Copilot adivina lo que viene
7. Si dejas de teclear → no pasa nada
```

> La iniciativa la llevas tú.
> Copilot reacciona a lo que escribes.

---

## Slide 6 — La misma tarea con Cursor

*"Añade un endpoint GET para obtener un pedido por ID en este controller."*

```
1. Abres el chat
2. Le pides el endpoint en una frase
3. Cursor te propone un bloque de código
4. Tú revisas → aplicas / modificas / descartas
5. Hay conversación. Hay propuesta.
6. Pero el agente espera tu siguiente instrucción
   después de cada paso
```

> Iniciativa compartida.
> Tú orquestas, él propone.

---

## Slide 7 — La misma tarea con Claude Code

Misma frase, en el terminal, en la raíz del repo.

```
Claude Code se pone a trabajar:
├── Lee OrdersController.cs           → ve la estructura
├── Ojea Order.cs                     → entiende la entidad
├── Mira los DTOs existentes          → ¿hay un OrderDto?
├── Comprueba endpoints similares     → sigue el patrón del equipo
├── Identifica que necesita el repo   → lo busca
├── Genera el método del controller
├── Genera el DTO si no existe
├── Genera el test de integración con WebApplicationFactory
├── Ejecuta los tests
└── Si fallan: analiza, corrige, reintenta

Al final te dice:
"endpoint añadido, dos ficheros modificados,
 un fichero nuevo, todos los tests pasan"
```

**Tú no has escrito nada.** Has descrito una intención y has validado un resultado.

---

## Slide 8 — Iniciativa delegada

A esto le llamamos **iniciativa delegada**. Tú defines la tarea, el agente decide cómo abordarla.

```
Antes (Copilot, Cursor, IDE clásico)            Con Claude Code
───────────────────────────────────             ───────────────

Qué fichero abrir primero          ─── tu decisión ──►  scope del agente
Qué patrón seguir                  ─── tu decisión ──►  scope del agente
Qué tests escribir                 ─── tu decisión ──►  scope del agente
En qué orden hacerlo               ─── tu decisión ──►  scope del agente
Qué consecuencias tiene en otros   ─── tu decisión ──►  scope del agente
ficheros del repo

Tú te quedas con:
├── Definir la tarea
├── Aportar el contexto del dominio
└── Validar el resultado final
```

Esto tiene **tres consecuencias prácticas** que conviene asimilar antes de tocar nada. Las tres en los siguientes slides.

---

## Slide 9 — Consecuencia 1: tocará más de lo que crees

Si le pides un test, puede retocar el método que se testea para hacerlo testeable. Si le pides un endpoint, puede modificar el DTO, el mapper, el controller y los tests. Y de paso añadir una entrada en el `Program.cs` o en la documentación.

```
Día 1-3      → Asusta
              "Pero yo solo te pedí un endpoint,
               ¿por qué has tocado el OrderMapper?"

Semana 1-2   → Se valora
              "Está haciendo trabajo de verdad,
               no autocompletando líneas"

Semana 2+    → Lo agradeces
              "Esos toques laterales son los que
               mantienen mi codebase sano"
```

> Es el mismo cambio que cuando contratas un junior bueno y dejas de microgestionar.
> Al principio te da vértigo. Después, no querrías volver atrás.

---

## Slide 10 — Consecuencia 2: la conversación cambia

Con Copilot tú escribes y el agente acompaña. Con Claude Code tú piensas y delegas.

| Con Copilot | Con Claude Code |
|---|---|
| Tú escribes, el agente completa | Tú piensas, el agente trabaja |
| Conversación = teclear con autocompletado | Conversación = pedir un favor a un junior preparado |
| *"Implementa esto: `if (id == null) return NotFound();`"* | *"Oye, ¿puedes meterle un endpoint para devolver pedidos por ID?"* |
| Lenguaje técnico paso a paso | Lenguaje narrativo orientado al objetivo |

> La forma de redactar las peticiones cambia.
> Más narrativa, menos técnica, más enfocada al objetivo de negocio.

---

## Slide 11 — Consecuencia 3: vas a estar menos delante del teclado

Esto es lo que más cuesta a algunos perfiles. Si tu identidad como dev está construida sobre *"yo escribo el código"*, Claude Code te va a pinchar en algún punto.

```
Antes                              Ahora
─────                              ─────
Tecleando                  ───►    Decidiendo qué se hace
Implementando línea a línea ───►   Validando que está bien hecho
Recordando convenciones    ───►    Codificando convenciones (CLAUDE.md)
Haciendo el trabajo        ───►    Resolviendo lo que el agente
mecánico                            no sabe resolver
```

> Que también es código, eh. Solo que menos.
> Y, normalmente, más interesante.

---

## Slide 12 — El anti-patrón "voice-to-code"

En la primera semana, mucha gente intenta usar Claude Code como si fuera Cursor.

```
"Abre OrdersController.cs"
"Busca el método Get"
"Ahora añade después de él un nuevo método llamado..."
"Haz que reciba un parámetro id de tipo int"
"Ahora añade un if que compruebe si es null"
"Ahora..."
```

Le dictan paso a paso. Lo tratan como una versión voice-to-code de su editor.

---

## Slide 13 — La analogía del cocinero

Lo de dictarle paso a paso a Claude Code tiene una traducción culinaria muy clara:

```
Tienes contratado a un cocinero profesional.

Y le dictas:
├── "Pon un huevo en la sartén"
├── "Ahora otro"
└── "Ahora una pizca de sal"
```

Has cogido la herramienta más cara del mercado para usarla como una más barata.

> Si el cocinero no decide nada, ¿para qué pagas a un cocinero?

---

## Slide 14 — Recetas vs objetivos

La señal de que estás usando bien Claude Code es que tus peticiones describen **resultados**, no **pasos**.

```
PETICIONES MAL FORMULADAS               PETICIONES BIEN FORMULADAS
(recetas paso a paso)                   (objetivos)
──────────────────────────              ─────────────────────────────────

"Añade un if que compruebe si           "Que el endpoint devuelva 404
 es null"                                si no existe el pedido"

"Mete un try-catch alrededor             "El sistema no debe romper si la
 de la llamada"                          BD falla; el llamante recibe un
                                         error tipado para reintentarlo"

"Cambia esta clase para que               "Quiero que este servicio sea
 reciba la dependencia por                testeable sin levantar la BD"
 constructor"

"Pon esto en una variable                "Extrae la lógica de validación
 separada"                                a un método reutilizable"
```

> Cuando llegues al punto de dar **objetivos en lugar de recetas**,
> sentirás el cambio de productividad.

---

## Slide 15 — Anatomía: las dos piezas grandes

Por dentro, Claude Code tiene dos piezas grandes y un puñado de mecanismos auxiliares.

```
┌────────────────────────────────────────────────┐
│   1. EL MOTOR                                  │
│      Un modelo de lenguaje grande              │
│      (Opus o Sonnet, según plan)               │
│                                                │
│      Razona. Decide. Genera código.            │
│                                                │
│      Sin el motor, no hay agente.              │
│      Solo un script que ejecuta órdenes ciegas.│
└────────────────────────────────────────────────┘

┌────────────────────────────────────────────────┐
│   2. LAS HERRAMIENTAS INTEGRADAS               │
│      Las acciones que Claude puede ejecutar    │
│      dentro de tu sistema.                     │
│                                                │
│      (Las vemos en el siguiente slide.)        │
└────────────────────────────────────────────────┘
```

---

## Slide 16 — Las herramientas integradas

| Herramienta | Qué hace |
|---|---|
| **`Read`** | Leer un fichero |
| **`Write`** | Escribir un fichero (creación o sobreescritura completa) |
| **`Edit`** | Modificar un trozo concreto sin reescribir el fichero entero |
| **`Bash`** | Ejecutar comandos en la terminal |
| **`Glob`** | Buscar ficheros por patrón |
| **`Grep`** | Buscar texto dentro de los ficheros |

Y algunas auxiliares — `WebFetch`, `WebSearch`, `Task`, gestión de procesos largos — que entran en juego en casos concretos.

> Seis primitivas. El agente las combina según lo que necesita.
> Y aquí está la clave: **no le digas tú qué herramientas usar**. Déjale decidir.

---

## Slide 17 — Traza real: "explícame qué hace este controller"

Abstracto suena raro. Veámoslo con una traza concreta:

```
1. Read OrdersController.cs              → lee el fichero
2. Grep "IOrderRepository" en /src/Domain → busca dónde se define el repo
3. Read OrderRepository.cs               → lee la implementación
4. Grep "Order" en /src/Domain/Entities  → busca la entidad
5. Read Order.cs                         → lee la entidad

[síntesis y respuesta al usuario]
```

> Lo importante no es memorizar la traza.
> Es ver cómo el agente **descubre el contexto que necesita** antes de escribir nada.
>
> No genera código contra una idea preconcebida. Primero entiende. Después actúa.

---

## Slide 18 — Traza real: "genera tests unitarios para CreateOrder"

```
1. Read OrdersController.cs               → entiende el método a testear
2. Glob /tests/**/*OrdersControllerTests* → busca tests existentes
3. Read existing test file                → ve el patrón usado
4. Read referenced dependencies           → entiende mocks necesarios
5. Write OrdersControllerTests.cs         → genera los tests
6. Bash dotnet test --filter ...          → los ejecuta

Si fallan:
├── Analiza el error
├── Corrige el test (o el código si toca)
└── Vuelve a ejecutar

[itera hasta que verde]
```

Fíjate en el **paso 3**: lee tests existentes para ver el patrón del equipo.

> Si tu equipo usa NSubstitute, los genera con NSubstitute.
> Si usáis FluentAssertions, lo respeta. Sin que se lo digas.

---

## Slide 19 — Traza real: "crea un endpoint para cancelar un pedido"

```
1.  Glob OrdersController.cs              → localiza el fichero
2.  Read OrdersController.cs              → ve patrones existentes
3.  Read Order.cs                         → ¿hay método CancelOrder?
4.  Grep "CancelOrder" en /src            → ¿ya existe la lógica?
5.  Read OrderService.cs                  → cómo se gestiona la lógica
6.  Edit Order.cs                         → añade método CancelOrder
7.  Edit OrderService.cs                  → añade orquestación
8.  Edit OrdersController.cs              → añade endpoint
9.  Write OrdersControllerCancelTests.cs  → genera tests
10. Bash dotnet build                     → comprueba que compila
11. Bash dotnet test                      → ejecuta tests

[itera hasta que todo verde]
```

11 acciones. Ningún paso te lo dictó nadie. Solo dijiste *"crea un endpoint para cancelar un pedido"*.

> Eso es **iniciativa delegada** materializada en una traza concreta.

---

## Slide 20 — La regla de oro

Cuando le pidas algo a Claude Code:

```
┌─────────────────────────────────────────────────────────┐
│                                                         │
│   No le digas qué HERRAMIENTAS usar.                    │
│                                                         │
│   Dile qué quieres CONSEGUIR.                           │
│                                                         │
│   El agente decide la combinación.                      │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

**El error en directo:**

```
Petición forzada (mala):
> "Abre OrdersController.cs y modifica el método Get
   para que devuelva 404 si no encuentra el pedido"

Lo que hace Claude Code:
└── Read + Edit (lo que le has dictado)

Lo que habría hecho por su cuenta:
├── Comprobar primero el patrón del equipo para 404
│   (¿Result<T>? ¿Excepción tipada? ¿NotFound() directo?)
├── Buscar cómo lo hacen otros endpoints
└── Detectar — quizá — que el equipo usa ProblemDetails,
    no un NotFound() pelado
```

> Cuando dictas las herramientas, te llevas la mitad del valor del agente.
> Lo trataste como un asistente, no como un agente.

---

## Slide 21 — El ciclo agentic: cuatro fases

Cuando le pides algo a Claude Code, no ejecuta una acción y devuelve. Entra en un ciclo de cuatro fases que se repiten hasta terminar.

```
                ┌─────────────────────────────────┐
                │                                 │
                ▼                                 │
        ┌──────────────┐                          │
        │  1. LECTURA  │                          │
        │  DE CONTEXTO │                          │
        └──────┬───────┘                          │
               │                                  │
               ▼                                  │
        ┌────────────────┐                        │
        │ 2. RAZONAMIENTO│                        │
        │   (interno)    │                        │
        └──────┬─────────┘                        │
               │                                  │
               ▼                                  │
        ┌──────────────┐                          │
        │  3. ACCIÓN   │                          │
        └──────┬───────┘                          │
               │                                  │
               ▼                                  │
        ┌──────────────┐                          │
        │ 4. VERIFICAR │                          │
        └──────┬───────┘                          │
               │                                  │
        ¿OK? ──┴──── NO ───────────────────────►──┘
               │
               SÍ
               ▼
            TERMINADO
```

Las cuatro, una por una, en los siguientes slides.

---

## Slide 22 — Fase 1: lectura de contexto

Claude lee lo que necesita para entender la tarea. **No lee todo el repo** — sería ineficiente y probablemente imposible por contexto. Lee lo relevante.

¿Cómo decide qué es relevante? Combina varias señales:

```
├── El CLAUDE.md del proyecto (lo veremos en 1.2)
│   → Le da una visión general del repo
│
├── La estructura de carpetas que ve con Glob
│
├── Las palabras clave de tu petición
│   → Busca con Grep los términos que has mencionado
│
└── Las dependencias que va descubriendo
    → Si lee el controller, ve que importa IOrderRepository
    → Y eso le lleva a leer la interfaz
```

En tareas pequeñas, la fase dura segundos. En grandes — un refactor cross-fichero — puede llevar varios minutos de exploración.

> Y aquí es donde algunos devs se ponen nerviosos.
> *"¿Pero por qué está abriendo tantos ficheros?"*
>
> Está construyendo el contexto. Sin ese contexto, lo que escriba después
> será paja inventada.

---

## Slide 23 — Fase 2: razonamiento

Decide cómo abordar la tarea. Qué ficheros tocar, en qué orden, qué comandos ejecutar, qué riesgos hay.

Esta fase **no se ve en pantalla**. Ocurre dentro del modelo. Pero deja huella en lo que hace después.

A veces el razonamiento se verbaliza:

```
> "Voy a empezar por modificar el Order.cs,
   después actualizo el servicio,
   y al final añado el endpoint en el controller"
```

A veces no se verbaliza, y simplemente actúa.

El comando **`/plan`** fuerza esta fase de forma explícita y te pide aprobación antes de actuar:

```
> /plan implementar cancelación de pedidos
[el agente te muestra el plan completo]
[tú apruebas, ajustas o rechazas]
[solo después actúa]
```

> Para tareas grandes, `/plan` es la diferencia entre un buen resultado
> y un *"no, espera, no por ahí"*. Lo veremos en 1.3.

---

## Slide 24 — Fase 3: acción

Ejecuta. Modifica ficheros, lanza comandos, escribe código.

```
Aquí SÍ se ve actividad:
├── Los ficheros que toca
├── Los outputs de los tests
├── Los logs
└── Los comandos lanzados
```

Un matiz importante: el agente **no actúa "ciegamente"** según un plan rígido. Si en mitad de la acción descubre algo inesperado:

```
> "El OrderRepository no tiene método GetById,
   hay que crearlo"

→ ADAPTA el plan sin pedirte permiso.
```

Es lo que tiene la iniciativa delegada. El agente ajusta su rumbo en tiempo real.

> Esto sorprende la primera vez. Después se agradece.
> Es lo que haría un dev senior bueno trabajando solo.

---

## Slide 25 — Fase 4: verificación

Esto es lo que más diferencia a Claude Code de un autocompletado:

> **Comprueba si lo que ha hecho funciona.**

```
Si ha lanzado tests:
├── Lee los resultados
├── Si fallan: analiza por qué
├── Si encuentra el error: modifica el código
├── Si no lo encuentra a la primera:
│   ├── Sigue investigando
│   ├── Busca en el código
│   ├── Busca en los logs
│   └── Prueba hipótesis
└── Vuelve a ejecutar

El ciclo se repite hasta que:
├── El agente considera que la tarea está terminada
└── O hasta que se queda atascado y te pide ayuda
```

**Iteraciones típicas:**

```
Tarea pequeña    → 2 vueltas
Tarea media      → 4-5 vueltas
Tarea grande     → 8-10 vueltas
```

---

## Slide 26 — Por qué parece "lento" al principio

La queja más común de la primera semana. Y la más malinterpretada.

```
Le pides un cambio sencillo:
> "Renombra esta variable"

Lo ves tardar 30 segundos
donde Copilot habría tardado 1.

Tu primera reacción: lento.
```

**Pero no está siendo lento. Está iterando.** En esos 30 segundos:

```
├── Ha leído los ficheros donde aparece la variable
├── Ha verificado dónde se usa
├── Ha hecho el cambio
├── Ha comprobado que el código sigue compilando
└── Ha asegurado que no se ha roto nada en cascada
```

---

## Slide 27 — La métrica honesta del rename

La métrica honesta no es *"cuánto tarda en responder"*. Es *"cuánto tarda el cambio en estar terminado y validado"*.

```
Copilot
├── 1 segundo en sugerir el rename
└── 5 minutos tú revisando manualmente
    los 5 ficheros donde aparece
    
Total: 5 minutos y 1 segundo


Claude Code
└── 30 segundos en hacer todo verificado

Total: 30 segundos
```

Lo que pasa es que esos 5 minutos tuyos no los cuentas. Te los cobras como tiempo de calidad. La impresión de "Copilot fue rápido" se sostiene porque la verificación la has hecho tú, mentalmente, y no la metes en la ecuación.

> Claude Code mete la verificación en la ecuación de fábrica.
> Por eso parece lento. Por eso es más rápido.

---

## Slide 28 — Comparativa con Copilot y Cursor

En tu equipo, alguien usa una de las otras dos. La trampa: vender Claude Code como *"lo mismo pero mejor"*. **No lo es. Es otra categoría.**

| | Copilot | Cursor | Claude Code |
|---|---|---|---|
| **Modo de uso** | Autocompletado mientras escribes | Conversación dentro del editor | Agente en terminal |
| **Iniciativa** | Reactiva | Compartida | Delegada |
| **Alcance típico** | Una línea o un bloque | Una función o un fichero | Cross-fichero, posiblemente cross-repo |
| **Cuándo brilla** | Código que ya sabes cómo escribir, más rápido | Modificar y explicar código existente | Tareas que requieren planificación |
| **Cuándo no toca** | Tareas con contexto amplio | Tareas que tocan muchos ficheros | Cuando solo quieres autocompletado |
| **Curva** | Plana | Suave | Más vertical — cambia tu forma de trabajar |
| **Donde vive** | En tu IDE | En tu IDE (editor propio) | Terminal (opcionalmente integrado en VS Code) |
| **Coste mental** | Bajo | Medio | Alto al principio, bajo después |

---

## Slide 29 — Escenarios concretos (1/2)

**Escribiendo un método nuevo, sabes exactamente lo que vas a poner.**

```
└── Copilot.
    Te ahorra el tecleo.
    Claude Code aquí es overkill.
```

**Tienes un fichero feo que quieres limpiar y entender.**

```
└── Cursor.
    Lo abres, le pides explicaciones, refactorizas tramo a tramo.
    Claude Code también lo hace, pero es como abrir un coche
    con una llave inglesa.
```

**Implementar una feature que toca cuatro ficheros y necesita tests.**

```
└── Claude Code, sin duda.
    Aquí Copilot te deja a medias.
    Cursor te haría ir paso a paso pidiéndote validación.
```

---

## Slide 30 — Escenarios concretos (2/2)

**Generar 30 tests para un servicio existente.**

```
└── Claude Code.
    Las dos primeras tienen lo justo para tareas mecánicas grandes.
    Claude Code las despacha en una sesión.
```

**Entender un repo que no es tuyo.**

```
├── Sesión rápida de exploración    → Cursor
└── Te quedas trabajando en él       → Claude Code
   (saca más partido al contexto persistente con CLAUDE.md)
```

**Prototipado rápido en un script Python suelto.**

```
└── Copilot.
    La fricción de Claude Code no compensa para 50 líneas.
```

---

## Slide 31 — La estrategia híbrida que más rinde

> **La gente que rinde más con Claude Code NO abandona Copilot.**
> Mantienen las dos.

```
Copilot
└── Activado en el editor para el día a día

    Cuando escribes un método y sabes lo que quieres:
    → Te ahorra tecleo

Claude Code
└── Cuando una tarea pasa de cierto tamaño:
    ├── Implementar algo nuevo
    ├── Refactorizar varios ficheros
    ├── Generar una suite de tests
    └── Hacer un code review serio

    → Saltas a la terminal y lanzas Claude Code
```

Piénsalo así: Copilot es la herramienta que te ayuda a escribir código más rápido. Claude Code es la herramienta a la que le delegas que se escriba el código solo.

> Son cosas distintas. Cabe perfectamente que en tu día convivan las dos.

---

## Slide 32 — Cuándo NO usar Claude Code

```
❌ EDITS TRIVIALES EN UN ÚNICO FICHERO
   ├── Renombrar una variable local
   ├── Cambiar un literal de string
   ├── Ajustar un margen de CSS
   └── Para eso, ve directo. No lances el agente.

❌ TRABAJO CREATIVO DONDE TÚ LLEVAS LA INSPIRACIÓN
   ├── Diseñas una arquitectura nueva desde cero
   ├── La idea está en tu cabeza
   └── Claude Code puede acelerarte la implementación,
       pero no le pidas que lleve la dirección
       — la va a llevar a un lugar genérico

❌ CÓDIGO QUE REQUIERE ACCESO A SISTEMAS QUE NO PUEDE VER
   ├── Lógica que depende de un sistema externo sin documentación
   ├── Claude Code va a inventar (o pedirte detalles continuamente)
   └── Mejor escríbelo tú con la doc delante

❌ TAREAS DE COSTE DE ERROR ALTO Y SIN FORMA DE TESTEARLAS
   ├── Migración de datos en producción sin entorno de prueba
   ├── Configuración crítica de infraestructura sin staging
   └── Aquí la verificación del agente no compensa el riesgo.
       Si lo usas, mínimo en --plan sin permitir acción
```

---

## Slide 33 — Los modelos disponibles

Tres modelos. Los relevantes ahora mismo:

```
┌──────────────────────────────────────────────────────────────┐
│   Claude Opus    — el más capaz                              │
│   ─────────────                                              │
│   ├── Mejor para tareas complejas:                           │
│   │   refactors grandes, debugging no trivial,               │
│   │   análisis de arquitectura                               │
│   ├── Más lento. Más caro.                                   │
│   └── Si llevas dos horas dándole vueltas a un problema      │
│       y no sale → es a Opus a quien lo tiras                 │
├──────────────────────────────────────────────────────────────┤
│   Claude Sonnet  — el caballo de batalla                     │
│   ─────────────                                              │
│   ├── Suficientemente bueno para el 90% de lo que pides      │
│   ├── Mucho más rápido y barato que Opus                     │
│   └── La mayoría de tu día a día va a estar aquí.            │
│       Y está bien que así sea.                               │
├──────────────────────────────────────────────────────────────┤
│   Claude Haiku   — el rápido                                 │
│   ─────────────                                              │
│   ├── Para tareas pequeñas y bien acotadas                   │
│   ├── No lo recomiendo como default en Claude Code           │
│   │   para el día a día                                      │
│   └── El ahorro no compensa la pérdida de calidad            │
│       cuando hay razonamiento de por medio                   │
└──────────────────────────────────────────────────────────────┘
```

---

## Slide 34 — Los planes

| Plan | Precio | Para quién |
|---|---|---|
| **Pro** | 20 $/mes | Devs que usan Claude Code unas horas al día. La entrada razonable. |
| **Max** | 100 a 200 $/mes | Quien tira de Claude Code todos los días varias horas |
| **Teams / Enterprise** | A medida | Equipos. Gestión centralizada, controles administrativos |
| **API por consumo** | Pago por tokens | Integrarlo en CI, scripts, automatizaciones |

```
¿Cuándo subir de Pro a Max?

Si llevas un mes con Pro y te encuentras topando los límites
con frecuencia → es la señal para subir.
```

> Para uso profesional intensivo individual, **Max es lo razonable**.
> Pro se queda corto rápido si lo usas a diario.

---

## Slide 35 — Decisión práctica de modelo

```
┌─────────────────────────────────────────────────────────────────┐
│  POR DEFECTO → SONNET                                           │
│  ────────────────────                                           │
│  Hace bien casi todo. Tarda la mitad.                           │
│  La inmensa mayoría de tareas se resuelven con Sonnet           │
│  sin notar diferencia.                                          │
└─────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────┐
│  CAMBIA A OPUS cuando se cumpla AL MENOS UNO de estos:          │
│  ─────────────────────────────────────────────────────          │
│                                                                 │
│  1. Sonnet se ha atascado dos veces seguidas en la misma tarea  │
│                                                                 │
│  2. La tarea de entrada ya parece compleja:                     │
│     ├── Refactor que toca cinco ficheros                        │
│     ├── Debugging de algo raro                                  │
│     └── Análisis de un repo grande que no conoces               │
│                                                                 │
│  3. Es una tarea de criterio:                                   │
│     ├── Revisar arquitectura                                    │
│     └── Evaluar trade-offs                                      │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

---

## Slide 36 — La regla más importante de modelo

> **No cambies de modelo cada cinco minutos.**

El switch tiene un coste mental. Vas a estar pensando *"¿debería estar usando Opus?"* en vez de en la tarea.

**Decide al empezar la sesión y mantén el modelo** — a no ser que haya razón fuerte para cambiar.

```
Si vas a alternar mucho:
└── Suele significar que estás trabajando en una sesión
    que mezcla tareas de naturalezas distintas.

    → Quizá merece partirla en sesiones separadas.
```

---

## Slide 37 — Ventana de contexto y costes

Cada modelo tiene una **ventana máxima de tokens**. En Sonnet ronda los 200k actualmente.

```
Cuando la sesión se acerca al límite:
├── El agente empieza a comportarse raro
├── Puede olvidar cosas que dijiste al principio
└── Generar respuestas más superficiales
```

El comando **`/compact`** comprime la conversación cuando lo notas. Lo veremos en 1.3 y es de los comandos más rentables que tiene la herramienta.

Para cuotas y límites:

```bash
/usage   # alias /cost
# Te muestra dónde estás
```

> Hábito útil: lánzalo cada 20-30 minutos.
> Te ahorra sorpresas a mitad de tarea.

---

## Slide 38 — Magnitud del coste

```
TAREA TÍPICA DE TAMAÑO MEDIO
└── Implementar un endpoint con tests

CONSUMO
└── Del orden de 30-50 mil tokens
    entre lectura, razonamiento y output

COSTE
├── Con Sonnet  → céntimos
└── Con Opus    → varias veces más
```

**Pongamos contexto:**

```
Si la tarea hubiera tardado dos horas a un humano:

   Coste del humano (50 €/h)     →    100 €
   Coste de Claude Code           →    céntimos / euros
```

> Los céntimos siguen siendo gangas. El cálculo no es difícil.

---

## Slide 39 — Mentalidad: lo que tienes que cambiar

> Esto puede sonar a charla motivacional, pero tiene cinco minutos de utilidad real, así que aguanta.

Adoptar Claude Code te obliga a tres cambios pequeños pero reales:

```
┌──────────────────────────────────────────────────────────────┐
│                                                              │
│  1. De "qué tengo que escribir" a "qué quiero conseguir"     │
│                                                              │
│  2. De "controlar cada acción" a "validar el resultado"      │
│                                                              │
│  3. De "lo hago yo" a "delego con criterio"                  │
│                                                              │
└──────────────────────────────────────────────────────────────┘
```

Uno a uno.

---

## Slide 40 — Cambio 1: de "qué escribir" a "qué conseguir"

La mayoría de devs llevamos años entrenándonos en pensar en código:

```
> "Necesito un método que reciba esto y devuelva aquello,
   con un try-catch alrededor por si falla la base de datos"
```

Esa es la mentalidad correcta cuando **tú** vas a escribir el método. Con Claude Code, da un paso atrás:

```
> "Necesito que el sistema no rompa cuando la base de datos falla,
   y que el llamante reciba un error tipado para reintentarlo"
```

La diferencia parece estética. No lo es.

```
Petición de implementación              Petición de objetivo
─────────────────────────              ────────────────────
Te encadena a una                       Deja al agente decidir entre:
implementación concreta                 ├── un try-catch
                                        ├── un Result
                                        ├── una política de retry
                                        └── un middleware
```

---

## Slide 41 — El "por qué" del cambio 1

Aquí viene lo interesante. **La decisión que tome el agente puede ser mejor que la tuya.**

No porque el agente sea más listo que tú — no lo es. Sino porque tiene contexto que tú no estás usando en ese momento:

```
└── Sabe que tu equipo tiene un middleware
    de manejo de errores en Program.cs

└── Sabe que en otros endpoints similares
    no usáis try-catch sino Result

└── Mientras tú piensas en la línea concreta
    que ibas a escribir,
    él lee el repo entero
```

> Cuando le das el objetivo en vez de la implementación,
> le dejas usar todo ese contexto.

---

## Slide 42 — Cambio 2: de "controlar" a "validar el resultado"

Esto es lo que más cuesta a los seniors con muchos años de oficio.

```
Mentalidad de control:
└── "Quiero saber exactamente qué está pasando en cada momento"

Es la que más fricción genera con Claude Code.
```

**El cambio de chip:**

```
1. Deja al agente trabajar
2. Cuando termina → VALIDA
   ├── Mira el diff
   ├── Ejecuta los tests
   └── Comprueba que la solución te encaja
3. Si no te encaja → díselo
   > "No, prefiero que esto se haga con un decorator"
   Y deja que reintente.
```

> Pero NO le dictes paso a paso mientras trabaja.
> Eso anula la mitad de su valor.

---

## Slide 43 — Cambio 3: de "lo hago yo" a "delego con criterio"

La frontera entre seniors junior y seniors senior.

```
TAREAS QUE CLARAMENTE DEBES DELEGAR
├── Tests
├── DTOs
├── Migraciones triviales
├── Documentación
└── Refactors mecánicos

TAREAS QUE NO DEBES DELEGAR
├── Decisiones de arquitectura
├── Código crítico para negocio
├── Performance tuning fino
└── Diseño de APIs públicas
```

> La curva real de aprendizaje con Claude Code no es técnica.
> Es saber dónde está esa frontera para tu caso.

---

## Slide 44 — Y esa frontera no se enseña

Esa frontera no se enseña en un curso, ni la encuentras leyendo un blog. **Se descubre.**

```
La primera vez que delegas algo
que deberías haber hecho tú
y se rompe en producción
└── aprendes.

La primera vez que haces a mano
algo que el agente habría hecho mejor
en la mitad de tiempo
└── también aprendes.
```

> Las dos lecciones cuentan.
> Las dos vienen de usar la herramienta el suficiente tiempo
> como para haberte equivocado en ambos sentidos.

---

## Slide 45 — Errores frecuentes del primer día (1/2)

```
❌ TRATAR EL AGENTE COMO UN AUTOCOMPLETADO
   Le hablas en pasos pequeños.
   ├── Resultado: te frustras porque "es lento"
   └── Cambio: dale OBJETIVOS, no recetas

❌ APROBAR TODAS LAS ACCIONES A CIEGAS
   El modelo de permisos existe por algo.
   ├── En tareas de cliente, párate a leer qué va a hacer antes de aprobar
   ├── Sí, es más lento
   └── Pero un día va a hacer algo que no quieres

❌ NO MANTENER CLAUDE.md ACTUALIZADO
   Y luego quejarse de que el agente
   "no sabe nuestras convenciones".
   └── Lo veremos en 1.2.

❌ LANZAR --dangerously-skip-permissions EN EL PORTÁTIL DE TRABAJO
   "Porque va más rápido."
   └── Es la receta para un día con mala suerte
       y un git push --force no deseado.
```

---

## Slide 46 — Errores frecuentes del primer día (2/2)

```
❌ PEDIR TAREAS DEMASIADO GRANDES EN UNA SOLA SESIÓN
   Si la sesión necesita tocar 20 ficheros
   y tomar varias decisiones de arquitectura
   ├── Parte la tarea
   └── Una sesión que se hace muy larga acaba
       con contexto saturado y resultados mediocres

❌ NO USAR /plan PARA TAREAS GRANDES
   Para algo que va a tocar más de 3-4 ficheros
   ├── Lanzar /plan antes te ahorra muchos retrocesos
   └── Lo veremos en 1.3

❌ COMPARAR CONTRA COPILOT EN TAREAS PEQUEÑAS
   "Pero esto Copilot lo hace en un segundo"
   ├── Sí. Y para esas tareas, sigue usando Copilot.
   └── Claude Code no es para todo.
```

> Si te ves cometiendo varios de estos durante la primera semana
> — completamente normal. Casi todo el mundo pasa por ahí.

---

## Slide 47 — La pregunta del primer test honesto

Antes de pasar a la instalación, una pregunta que conviene hacerse:

```
┌──────────────────────────────────────────────────────────────┐
│                                                              │
│   ¿Qué tarea concreta de tu día a día                        │
│   sería el primer test honesto para Claude Code?             │
│                                                              │
└──────────────────────────────────────────────────────────────┘
```

**No esto:**

```
❌ "Algo que ya sabes hacer y quieres ver
    si la herramienta lo hace igual"
   └── Eso es comparar peras con manzanas.
```

**Esto:**

```
✅ Algo que te lleva más tiempo del que debería:
   ├── Un refactor pendiente
   ├── Una suite de tests que no acabas de escribir
   └── Un módulo que entiendes a medias y querrías documentar
```

> Tener esa tarea en mente mientras configuras Claude Code
> en el siguiente apartado convierte el ejercicio de
> "instalar herramienta" en "preparar un experimento real".
>
> Y ahí es donde se ve si la cosa funciona o no.

---

## Slide 48 — Cierre

```
SIGUIENTE → 1.2 Instalación y configuración (45 min)
            ├── De terminal vacía a Claude Code operativo
            └── Primer CLAUDE.md real para tu stack
```

**Nos vemos en 1.2.**
