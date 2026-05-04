> **Versión:** v3 | **Módulo:** 1 | **Sub:** 1.3a | **Slides:** 33 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M01-S1.3a-modos-y-comandos-v3.md`

# Submódulo 1.3a — Modos de uso y comandos esenciales

## Slide 1 — Portada
**Módulo 1 · Submódulo 1.3 · Parte A**
Modos de uso y comandos esenciales
Las marchas que casi nadie usa

---

## Slide 2 — El día 2

Ya tienes Claude Code en marcha, autenticado, con un `CLAUDE.md` decente.

La mayoría de gente que llega aquí lanza `claude`, escribe lo que se le ocurre, y a partir de ahí navega a base de prueba y error. Funciona. Pero te estás dejando una parte buena del valor de la herramienta fuera del flujo.

> No es contenido espectacular. Son hábitos.
> Pero son los hábitos que separan a quien usa Claude Code dos semanas y se queda,
> de quien lo usa un mes y abandona porque *"no es para tanto"*.

---

## Slide 3 — La analogía del coche manual

```
Imagina que te compras un coche manual
y solo usas la primera y la marcha atrás.

├── Llegas a los sitios
├── El coche funciona
├── Pero estás haciendo cuestas a 20 km/h
└── Y desgastando el embrague

La gente que cambia de marchas
├── Llega antes
├── Gasta menos gasolina
└── Disfruta el coche
```

Con Claude Code en modo interactivo nada más, eres ese conductor de primera y marcha atrás. Este apartado son **las otras marchas**.

Los modos en que puedes lanzar Claude Code (no solo conversación), los comandos que ahorran tiempo en sesiones largas, y cómo decidir qué permisos das al agente sin tener que aprobar cada acción individualmente.

---

## Slide 4 — Los tres modos de uso

Lo que casi todo el mundo conoce de Claude Code es el modo conversación. Pero hay dos más, y cambian bastante lo que puedes hacer con la herramienta.

```
1. Modo interactivo   → claude
2. Modo one-shot      → claude -p "..."
3. Modo pipe          → cat fichero | claude
```

Los vemos uno a uno.

---

## Slide 5 — Modo 1: interactivo

```bash
claude
```

Es el modo por defecto. Abres una sesión, escribes, el agente responde, vuelves a escribir. **Conversación continua.**

```
Lo usas para trabajar en algo que requiere ida y vuelta:
├── Implementar una feature
├── Debuggear
└── Refactorizar

La sesión mantiene contexto:
├── Lo que dijisteis hace 20 minutos sigue presente
├── Las decisiones que tomasteis siguen vigentes
└── El agente recuerda dónde estabas en una tarea
    cuando la retomas
```

> Es el modo que más rentabilidad da en tareas medianas y grandes.
> Si tu tarea va a tomar más de cinco minutos de trabajo,
> lo más probable es que la quieras en modo interactivo.

---

## Slide 6 — Modo 2: one-shot

```bash
claude -p "Lista las clases públicas en src/Domain"
```

Le das una instrucción, ejecuta, devuelve resultado, y se cierra. **Sin sesión. Sin memoria.**

**Ideal cuando:**

```
├── Quieres meter Claude Code en un script o pipeline
│   El agente como una pieza más del flujo

├── Tienes una pregunta puntual y no quieres abrir sesión
│   "¿qué hace este fichero?"
│   → claude -p "..." OrdersController.cs

└── Estás encadenando comandos
    Y quieres que Claude Code sea uno más en una tubería
```

Donde más rinde es en automatizaciones. Tres ejemplos reales en los siguientes slides.

---

## Slide 7 — One-shot en un hook de pre-commit

```bash
#!/bin/bash
# .git/hooks/pre-commit

claude -p "audita el diff staged y devuelve solo los riesgos críticos.
           Si no hay, di OK."
```

Cada commit dispara una revisión automática. Sin abrir conversación. Sin pulsar nada.

> El agente como guardián que revisa antes de que algo entre al repo.

---

## Slide 8 — One-shot en CI

```yaml
# .github/workflows/review.yml

- name: AI review
  run: |
    git diff origin/main...HEAD | claude -p "revisa estos cambios
    buscando bugs, cuestiones de seguridad o malas prácticas"
```

```
En el pipeline, cada PR
├── Se revisa automáticamente
├── Sin que un humano abra Claude Code
└── El resultado queda en los logs del workflow
```

---

## Slide 9 — One-shot en un script batch

```bash
# Generar tests para todos los servicios que aún no tienen

for service in src/Application/Services/*.cs; do
  if [ ! -f "tests/Unit/$(basename $service .cs)Tests.cs" ]; then
    claude -p "genera tests xUnit + NSubstitute para $service
               siguiendo las convenciones del proyecto"
  fi
done
```

Un bucle de bash recorre todos los servicios, comprueba si tienen test asociado, y si no lo tienen, genera el test con Claude Code.

> El modo one-shot es donde Claude Code **deja de sentirse como una herramienta interactiva**
> y empieza a sentirse como una pieza de tu infraestructura.

---

## Slide 10 — Modo 3: pipe

```bash
cat error.log | claude
git diff | claude -p "explica los cambios"
ps aux | claude -p "qué procesos parecen sospechosos"
```

Le pasas algo por stdin. Claude lo lee como contexto y procede.

```
Combina muy bien con flujos de Unix:
├── grep
├── find
├── git log
├── journalctl
└── lo que tengas
```

> Aquí es donde la herramienta empieza a sentirse parte del shell,
> no algo aparte.

---

## Slide 11 — Casos típicos del modo pipe

```
ANÁLISIS DE LOGS
├── "Aquí tienes 200 líneas de error.log,
│    dime qué está pasando"
└── Mucho más rápido que leerlas tú

EXPLICACIÓN DE DIFFS
├── Antes de un PR:
│   git diff main...HEAD | claude -p "resume los cambios
│                                      para la descripción del PR"
└── 30 segundos y tienes la descripción

PROCESAMIENTO DE SALIDAS
├── Combinas comandos Unix con razonamiento del agente
└── "Aquí está la salida de top, dime si hay algo raro"
```

---

## Slide 12 — Combinar los tres modos

Los tres modos no son excluyentes. La gente que rinde más con Claude Code los combina según necesidad:

```
PARA TRABAJAR
└── Modo interactivo. Sesión larga.

PARA AUTOMATIZAR
└── Modo one-shot. En scripts, hooks, CI.

PARA PROCESAR DATOS
└── Modo pipe. Cuando el input ya está en algún lado
    y solo lo quieres analizar.
```

> Conocer los tres y elegir el adecuado para cada tarea
> es la diferencia entre usar Claude Code como un chatbot de programación
> y usarlo como una herramienta integrada en tu flujo.

---

## Slide 13 — Slash commands: el panorama

Claude Code trae **más de 60 slash commands integrados**.

Verlos todos no tiene sentido. La mayoría son útiles en momentos puntuales y se descubren con el tiempo. En esta sección nos centramos en los **diez que de verdad cambian el día a día**.

> Al final del módulo tienes el cheatsheet completo de los 26 más comunes
> como referencia.

Los agrupo por para qué sirven, no por orden alfabético — la idea es que en cada situación sepas a cuál tirar.

---

## Slide 14 — Para arrancar y entender lo que hay: /help

```
/help
```

**Lista todos los comandos disponibles**, incluyendo los tuyos personalizados. Es la primera parada cuando no recuerdas algo.

```
Pista que casi nadie usa:
└── Si pones /h y autocompleta,
    te muestra solo los que empiezan por h.

    Filtrado en vivo.
```

---

## Slide 15 — Para arrancar y entender lo que hay: /init

```
/init
```

**Genera un `CLAUDE.md` base analizando el repo donde estás.**

No es tan bueno como uno escrito a mano por alguien que conoce el proyecto, pero es un buen punto de partida cuando llegas a un repo y no hay nada.

```
Mi consejo:
├── Lánzalo
├── Mira lo que ha generado
└── Úsalo como esqueleto sobre el que escribes el de verdad
```

> Es un 70% de un buen `CLAUDE.md` en 30 segundos.

---

## Slide 16 — Para gestionar la sesión: /clear

```
/clear
```

**Reinicia la conversación.** Ojo: borra todo el contexto.

Útil cuando cambias de tarea por completo y no quieres que el agente arrastre información de la anterior.

**Anti-patrón típico**: usar `/clear` cuando deberías usar `/compact`.

```
Si estás en mitad de una tarea grande y notas que el contexto pesa:
├── /compact te conserva lo importante
└── /clear te tira todo

Mucha gente, especialmente al principio, usa /clear por hábito
y pierde contexto valioso.
```

> Si la tarea actual no ha terminado, `/compact` casi siempre es la opción correcta.

---

## Slide 17 — Para gestionar la sesión: /compact

```
/compact
```

**Compacta el historial conservando lo importante.**

Esto es de lejos el comando más útil en sesiones largas. Cuando lleves hora y media trabajando y notes que el agente empieza a comportarse raro, lánzalo.

> Le dedico un apartado entero más abajo porque merece la profundidad.

---

## Slide 18 — Para gestionar la sesión: /usage

```
/usage    (alias rápido: /cost)
```

**Muestra cuánto contexto y cuántos tokens has consumido** en la sesión actual y dónde estás respecto a tus límites de plan.

```
Hábito útil:
└── Lánzalo cada 20-30 minutos
    para no llevarte sorpresas.
```

> Especialmente importante si estás en plan Pro y trabajas sesiones largas.
> Tocar el límite con una tarea a medias es desagradable.

---

## Slide 19 — Para configurar el comportamiento: /model

```
/model
```

**Cambia el modelo en caliente.**

Útil cuando la sesión está tirando de Sonnet y la siguiente tarea es complicada — pasas a Opus para esa tarea concreta y luego vuelves a Sonnet.

```
Vale la pena saberlo, aunque, como dije en 1.1,
lo recomendable es no andar cambiando cada cinco minutos.
└── El cambio mental cuesta más de lo que parece.
```

---

## Slide 20 — Para configurar el comportamiento: /permissions

```
/permissions
```

**Revisa y modifica los permisos de la sesión actual.**

```
Útil cuando te das cuenta a mitad de sesión:
├── El agente está pidiendo aprobación para cosas que sí quieres permitir
└── O al revés, está ejecutando algo que querrías controlar más
```

> Ajustes en caliente, sin tener que abrir el `settings.json` y reiniciar.

---

## Slide 21 — Para integraciones y razonamiento: /mcp y /agents

```
/mcp
```

**Gestiona los MCP servers conectados.**

Lo usaremos a fondo en los módulos 4 y 5 cuando integremos Figma. De momento, basta con saber que existe y que es donde compruebas si tus integraciones están vivas. *"¿El MCP de Figma está conectado?"* — `/mcp` te lo dice.

```
/agents
```

**Lista y gestiona subagentes.**

Lo vemos en profundidad en el módulo 3. Aquí basta con saber que con `/agents` ves cuáles tienes definidos y puedes invocar uno explícitamente.

---

## Slide 22 — Para tareas grandes: /plan

```
/plan
```

**Activa el modo planificación.**

En vez de actuar directamente, Claude propone un plan paso a paso y te pide confirmación antes de tocar nada.

> Para tareas grandes, **es la diferencia entre que el agente vaya por el camino correcto desde el principio y que tengas que pararle a mitad** porque ha tomado una dirección equivocada.

**Mi regla práctica:**

```
Si la tarea va a tocar más de tres ficheros
└── lanza /plan.

La pequeña fricción de revisar un plan
vale el ahorro de no descubrir a los diez minutos
que el agente está reescribiendo algo que querías que dejara como estaba.
```

---

## Slide 23 — /compact en profundidad

Este merece su propio apartado porque es **el comando que más rentabilidad da en producción**.

Vamos a ver:
- El problema que resuelve.
- Qué hace exactamente.
- Cuándo lanzarlo.
- Cómo guiarlo.
- Lo que pierdes inevitablemente.

---

## Slide 24 — /compact: el problema que resuelve

Cada modelo tiene una ventana de contexto máxima. Cuando una sesión se acerca al límite, pasan tres cosas:

```
1. EL AGENTE EMPIEZA A "OLVIDAR"
   Decisiones que se tomaron al principio de la sesión.

2. LAS RESPUESTAS SE VUELVEN MÁS SUPERFICIALES
   Menos contexto disponible
   = menos sustancia en cada respuesta.

3. LA LATENCIA SUBE
   Mover ventanas de contexto enormes cuesta cómputo y tiempo.
```

**Y todo esto pasa de forma gradual.** No te avisa una alarma.

```
Lo que notas:
├── El agente repregunta cosas que ya sabía
├── Propone soluciones que ya descartasteis hace media hora
└── Las respuestas se sienten más "genéricas" que al principio
```

---

## Slide 25 — Caso real

```
Dev en sesión de implementación de feature compleja.

90 minutos.
7 ficheros tocados.
Varias decisiones de arquitectura discutidas con el agente.

Pide:
"ahora añade un test para el caso de error
 en el endpoint que acabamos de modificar"

El agente devuelve:
└── Un test genérico contra una signatura que NO es la que existe.
    No recuerda los cambios del principio.

La conversación se ha vuelto inservible.
"El agente se ha vuelto tonto."
```

> No se ha vuelto tonto. Se ha quedado sin contexto.

---

## Slide 26 — Qué hace /compact

Le pide a Claude que comprima la conversación:

```
├── Resume lo que se ha tratado
├── Conserva las decisiones tomadas y los puntos abiertos
└── Descarta las idas y vueltas innecesarias
```

El resultado es una sesión que sigue teniendo la información relevante pero ocupa una fracción del contexto.

---

## Slide 27 — Ejemplo de compactación

Imagina que la conversación de 90 minutos se compactara así:

```
> Resumen de los últimos 90 minutos:
>
> Estás implementando la feature de cancelación de pedidos.
>
> Has modificado:
>   - OrdersController.cs (añadido endpoint POST cancel)
>   - OrderService.cs (lógica de cancelación con validaciones)
>   - Order.cs (método CancelOrder)
>   - los tests correspondientes
>
> Decisiones clave:
>   - Se valida que el pedido no esté ya enviado
>     (lanza InvalidOperationException tipada)
>   - Se publica evento OrderCancelled al bus
>   - Se actualiza estado en BBDD en transacción
>
> Pendiente: añadir tests para el caso de error.
```

> Ese resumen ocupa un 5% de lo que ocupaba la conversación entera,
> pero contiene toda la información necesaria para seguir trabajando bien.

---

## Slide 28 — Cuándo lanzar /compact

La regla práctica de la gente que lleva tiempo con Claude Code:

> **Cada 20-30 minutos de trabajo activo.**

No esperes a que el agente empiece a comportarse raro — para entonces ya es tarde y has perdido tiempo.

**También conviene lanzarlo:**

```
CUANDO VAS A CAMBIAR DE TAREA DENTRO DE LA MISMA SESIÓN
├── Terminaste un endpoint y ahora vas a hacer los tests
└── /compact limpia el contexto antes de empezar la siguiente fase

ANTES DE PEDIRLE ALGO GRANDE
├── Llevas un rato charloteando con el agente
├── Y ahora le vas a pedir que implemente una feature compleja
└── /compact deja la sesión "limpia" para esa tarea sin arrastrar ruido

CUANDO NOTAS SÍNTOMAS RAROS
├── El agente empieza a olvidar
├── Repreguntar
└── Dar respuestas vagas
```

---

## Slide 29 — Cómo guiar la compactación

Lo más útil que casi nadie sabe: **puedes pasarle instrucciones a `/compact`** para guiar qué se conserva.

```
/compact "conserva las decisiones de arquitectura
          y los nombres de las clases creadas en esta sesión"
```

```
/compact "conserva los nombres de las hipótesis del bug
          que estamos investigando, especialmente las descartadas"
```

```
/compact "conserva la estructura del esquema
          que hemos diseñado para la BBDD"
```

> Esto evita que el resumen pierda información que sí necesitas.
> Si estás trabajando en un refactor donde los nombres importan, dile que los conserve.
> Si estás haciendo debugging y la causa raíz aún no está clara,
> dile que conserve las hipótesis descartadas — para no probar dos veces lo mismo.

---

## Slide 30 — /compact: lo que pierdes inevitablemente

Para ser honesto, `/compact` no es magia. Pierdes:

```
EL "COLOR" DE LA CONVERSACIÓN
├── Las pequeñas explicaciones
├── Los matices que diste sobre por qué algo
└── El resumen se queda con los hechos, no con la forma

DETALLES SECUNDARIOS
└── Que en su momento no parecían importantes
    pero que el modelo descartó al compactar

TU HISTORIAL DE PROMPTS
├── Después de un /compact,
│   "haz lo mismo que antes con la otra clase"
│   puede no funcionar
└── Si el resumen no conservó cuál era ese "lo mismo"
```

> La compensación: vas a poder seguir trabajando 20-30 minutos más
> sin que el agente colapse.
>
> **Casi siempre vale la pena.**

---

## Slide 31 — Tool search: la pieza que reduce contexto sin que la veas

Una de las novedades de Claude Code respecto a otras herramientas agenticas es **tool search**.

**La idea:** en vez de cargar todas las herramientas disponibles (Read, Write, Bash, MCP servers conectados, skills instalados, etc.) en el contexto desde el principio, **Claude las descubre bajo demanda** según lo que va a necesitar.

```
¿Por qué importa?

Las descripciones de herramientas, especialmente cuando tienes
varios MCP servers conectados, ocupan mucho contexto.

Si todas se cargan al arrancar la sesión,
te quedas sin sitio para tu código.
```

**Tool search invierte la lógica:**

```
El agente busca herramientas cuando las necesita
├── Las carga
├── Las usa
└── Libera el espacio
```

> La reducción de contexto es notable en sesiones que combinan varias integraciones.

---

## Slide 32 — Tool search: lo que necesitas saber y cuándo se nota

**Lo que necesitas saber como usuario:**

```
├── Va activado por defecto en versiones recientes
├── No tienes que hacer nada para que funcione
├── Si quieres comprobar qué herramientas tiene disponibles tu sesión
│   /help o /mcp te lo enseñan
└── Si el agente "no encuentra" una herramienta que sí debería tener
    suele ser tema de descripción del skill o del MCP server
    No del tool search
```

**Cuándo se nota:**

```
EN SESIONES CORTAS
└── No se nota nada.

CUANDO COMBINAS VARIOS ELEMENTOS
├── Skills instalados (>10)
│   Las descripciones de cada skill ocupan tokens.
│   Tool search hace que solo se carguen al activarse.
│
├── Varios MCP servers conectados
│   MCP de Figma, otro de GitHub, otro interno.
│   Las descripciones se acumulan.
│   Tool search las trae bajo demanda.
│
└── Sesiones largas
    Ya tienes el contexto cargado de tu trabajo.
    Tool search asegura que las herramientas no compiten por ese espacio.
```

> Si tu setup es minimalista (Claude Code "pelado", sin skills propios ni MCP), no se nota.
> Cuando te acerques al setup que vamos a tener al final del curso
> (skills propios + Figma MCP + posiblemente más), entonces sí.

---

## Slide 33 — Lo que viene en 1.3b

```
SUBMÓDULO 1.3b — PERMISOS EN RUNTIME Y WORKFLOWS
─────────────────────────────────────────────────────

Permisos en runtime
├── El flujo de aprobación (las 4 opciones del prompt)
├── El patrón A (aprobar todo a ciegas) — peligroso
├── El patrón B (aprobar todo individualmente) — frustrante
├── El patrón sano (promover a "always" lo seguro y repetitivo)
├── Cuándo bloquear con "no"
└── Recordatorio del modo autónomo

Workflows del día a día
├── Patrón 1: implementación de feature
├── Patrón 2: refactor mediano
├── Patrón 3: code review asistido
└── Patrón 4: debugging

El patrón anti-eficiente (lo que NO rinde)

Errores frecuentes con estos comandos y modos

Cierre del módulo 1
└── Y la pregunta de cara al módulo 2
```

**Nos vemos en 1.3b.**
