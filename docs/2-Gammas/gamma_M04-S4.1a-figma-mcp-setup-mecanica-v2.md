> **Versión:** v2 | **Módulo:** 4 | **Sub:** 4.1a | **Slides:** 30 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M04-S4.1a-figma-mcp-setup-mecanica-v2.md`

# Submódulo 4.1a — Figma MCP: setup y mecánica

## Slide 1 — Portada
**Módulo 4 · Submódulo 4.1 · Parte A**
Figma MCP: setup y mecánica
El problema que resuelve, dos versiones, instalación, formas de pasar contexto, herramientas principales

---

## Slide 2 — Cambio de tema: del agente al flujo de diseño

```
Hasta aquí, el curso ha ido sobre Claude Code en sí mismo:

├── cómo se instala
├── cómo se personaliza con skills y subagentes
└── cómo se automatiza con hooks
```

> Todo lo que hemos visto era el **agente y su comportamiento**.

```
A partir de este módulo cambiamos de eje.

Ahora hablamos de cómo Claude Code se INTEGRA
con el FLUJO DE DISEÑO.
```

```
Concretamente:
├── cómo trabaja con FIGMA
└── cómo se usa CLAUDE DESIGN para creación visual conversacional
```

---

## Slide 3 — Por qué esto merece dos sesiones del curso

Porque es donde un porcentaje muy alto de equipos pierde más horas de las que cree.

```
Cuando hablas con devs frontend sobre dónde se les va el día,
una cosa que aparece sistemáticamente:

"intentando entender el Figma del diseñador
 y traducirlo a código"
```

**Lo que aparece a diario:**

```
├── Píxeles que no cuadran
├── Tokens de color que no están claros
├── Spacings que el diseñador puso a ojo
│   y nadie sabe de dónde salen
└── Componentes en Figma que en el código no existen
    y hay que decidir si crearlos o reutilizar otros similares
```

> Con el MCP server oficial de Figma,
> mucho de ese trabajo lo hace ahora la máquina.

---

## Slide 4 — El problema que resuelve, antes del MCP

Imagina el escenario clásico **antes** del MCP:

```
1. El diseñador termina una pantalla en Figma.
2. Te pasa el link.
3. Tú abres el modo Dev Mode.
4. Haces click en cada elemento para ver su CSS.
5. Copias colores hex, anotas paddings y márgenes.
6. Intuyes el grid.
7. Abres VS Code y empiezas a escribir el componente.
8. A media pantalla te das cuenta de que un padding
   que pensabas que era 16px en realidad es 14px.
9. Vuelves a Figma a verificar, sigues.
```

```
Cuando terminas, el resultado se parece al diseño en un 80%.

Un buen 80%.
Pero el diseñador va a abrir un ticket
porque hay un radio de borde mal y una sombra que falta.
```

> Si tu equipo entrega una feature por sprint
> que toca 8-12 componentes nuevos,
>
> hablamos de **HORAS A LA SEMANA**
> que se van en traducción mecánica.

---

## Slide 5 — El problema que resuelve, con el MCP

```
El Figma MCP server cambia las reglas.
```

**El nuevo flujo:**

```
1. Conectas Claude Code a tu cuenta de Figma.
2. Le das un link a un frame.
3. Le pides que genere el componente.
```

```
La máquina lee directamente la ESTRUCTURA del Figma:

├── No la imagen
├── No su renderizado
└── La ESTRUCTURA

Y produce código que está alineado
con lo que el diseñador hizo.
```

> ¿Es perfecto? **No**.
>
> Pero el cambio en el flujo es real y notable.

---

## Slide 6 — Qué es exactamente Figma MCP

```
Es un servidor MCP — Model Context Protocol —
oficial de Figma
```

```
Expone tus diseños como herramientas
que un agente como Claude Code puede consultar.
```

> La idea es la misma que cualquier MCP:
>
> en vez de copy-paste manual de información,
> el agente lo pide cuando lo necesita.

---

## Slide 7 — Dos versiones del Figma MCP server

```
1. REMOTE (recomendada)
   ├── Endpoint hosted por Figma
   │   └── https://mcp.figma.com/mcp
   ├── NO requiere tener Figma desktop abierto
   └── Tiene el conjunto MÁS AMPLIO de herramientas
       └── incluyendo capacidades de escritura al canvas
           (crear y modificar contenido nativo desde Claude Code)

2. DESKTOP (local)
   ├── Corre dentro del Figma desktop app
   │   └── http://127.0.0.1:3845/mcp
   ├── Requiere tener Figma desktop instalado y abierto
   │   en Dev Mode con el server activado
   └── Pensado para casos específicos:
       └── empresas con políticas de seguridad
           que requieren que los datos no salgan
           a un endpoint remoto
```

> En este apartado vamos con la REMOTA.
>
> Es lo que el **95% de los equipos** va a usar.

---

## Slide 8 — Plan requirements (importante)

```
El Figma MCP requiere PLAN DE PAGO de Figma.
```

| Plan | Acceso |
|---|---|
| **Dev seat o Full seat** en Professional, Organization o Enterprise | **Acceso completo**. Rate limits por minuto similares a Tier 1 de la API REST. |
| **Starter plan** o **View/Collab seats** en planes pagos | Solo **6 tool calls al MES**. Suficiente para evaluar, no para uso real. |

> Si tu equipo NO tiene Dev seats ni plan profesional:
>
> **el MCP no es viable como herramienta diaria**.
>
> Conviene aclararlo con tu jefe antes de meterte en el setup.

---

## Slide 9 — Setup paso a paso: dos opciones

La instalación tiene dos opciones.

```
OPCIÓN 1
└── vía PLUGIN OFICIAL (recomendada)

OPCIÓN 2
└── vía CLI directo
```

> Las cubrimos las dos. La recomendada es la primera.

---

## Slide 10 — Opción 1: vía plugin oficial

Anthropic mantiene un plugin oficial de Figma que incluye no solo el MCP server sino también **agent skills preconfigurados** para los flujos más comunes.

```
Es el camino más rápido
y el que cubre más casos
sin que tengas que configurar tú nada.
```

**Instalación:**

```bash
claude plugin install figma@claude-plugins-official
```

> Tras la instalación, **reinicia Claude Code**.

---

## Slide 11 — Opción 1: autenticación y verificación

Dentro de la sesión:

```
> /plugin
```

```
Te abre el menú de plugins.

├── Navega a la pestaña Installed
├── Selecciona el plugin de Figma
└── Pulsa Enter para iniciar la autenticación

Se abrirá una página externa donde Figma te pide permisos.
└── Click "Allow access" y vuelve a la terminal.
```

**Para verificar:**

```
> /mcp
```

> Debería listar `figma` como **connected**.

---

## Slide 12 — Opción 2: vía CLI directo

Si prefieres no instalar el plugin completo y solo quieres el MCP server:

```bash
# Instalación a nivel proyecto (solo este repo)
claude mcp add --transport http figma https://mcp.figma.com/mcp

# O instalación a nivel user (todos tus proyectos)
claude mcp add --scope user --transport http figma https://mcp.figma.com/mcp
```

---

## Slide 13 — Opción 2: recomendación de scope

```
Mi recomendación: SCOPE USER.
```

```
El Figma MCP es algo que vas a querer disponible
en cualquier proyecto donde toques diseño.

NO solo en uno concreto.
```

> Si te ves repitiendo `claude mcp add` en cada repo,
> es señal de que debería ser **user**.

---

## Slide 14 — Opción 2: autenticación

Tras añadirlo, dentro de Claude Code:

```
> /mcp
```

```
Te muestra la lista de MCP servers.

├── Selecciona figma
├── Elige Authenticate
├── Haz el flujo OAuth en el navegador
└── Vuelve a la terminal
```

> Verás el mensaje:
>
> *"Authentication successful. Connected to figma"*

---

## Slide 15 — Verificación práctica

Con la instalación lista, antes de pasar a flujos reales conviene verificar que las herramientas están disponibles:

```
> /mcp
```

```
Debería mostrar figma como connected.

Y al expandirlo deberías ver las herramientas que expone.
```

**Las principales:**

```
├── get_design_context
│   └── extrae la representación estructurada
│       de una selección o un frame.
│
├── get_variable_defs
│   └── extrae las variables (tokens)
│       usadas en una selección.
│
└── Otras varias para
    └── Code Connect, FigJam, escritura al canvas
```

---

## Slide 16 — Si las herramientas no aparecen

```
Si alguna de estas NO aparece,
el setup tiene un problema.
```

**Las causas más comunes:**

```
1. AUTENTICACIÓN INCOMPLETA
   └── Vuelve a /mcp y reauténtica.

2. SEAT PLAN INSUFICIENTE
   └── Verifica con tu admin de Figma
       que tienes Dev/Full seat.
```

---

## Slide 17 — Setup del lado Figma

Para que el MCP funcione bien, hay un par de cosas que conviene comprobar **en Figma**.

```
1. EL FICHERO TIENE QUE ESTAR ACCESIBLE PARA TU CUENTA
   ├── Edit o view-only basta.
   └── Si es un fichero del equipo de diseño:
       pídeles que te lo compartan.

2. EL EQUIPO DE DISEÑO ESTÁ USANDO BIEN
   LA ESTRUCTURA DE FIGMA
   ├── Variables (no colores hex sueltos)
   ├── Componentes (no grupos)
   └── Auto layout (no posicionamiento manual)
```

> Esto NO es un detalle estético.
>
> El MCP lee mucho mejor un diseño hecho con disciplina.
>
> Lo veremos en la sección "buenas prácticas en el lado Figma" en 4.1b.

---

## Slide 18 — Dos formas de pasar contexto al MCP

Una vez instalado, hay **dos maneras** de decirle a Claude Code qué frame quieres que mire.

```
1. SELECTION-BASED
2. LINK-BASED
```

> Ambas son útiles y conviene conocer las dos.

---

## Slide 19 — Selection-based

```
Tienes Figma abierto (web o desktop).
Seleccionas el frame o el componente que te interesa.
Vuelves a Claude Code y pides:
```

```
Genera un componente Angular standalone
basado en mi selección actual de Figma.
```

```
Claude Code llama al MCP server,
que detecta tu selección activa,
y devuelve la información estructurada.
```

```
✅ VENTAJA
   No tienes que pasar URLs entre herramientas.
   Más fluido.

⚠️ LIMITACIÓN
   Solo puedes tener UNA selección activa a la vez.
   Si quieres trabajar con varios frames distintos
   en paralelo, tienes que ir cambiando.
```

---

## Slide 20 — Link-based

Más útil cuando estás trabajando en algo concreto y quieres ser **explícito** sobre qué frame analizar.

```
En Figma:
└── click derecho sobre el frame
    └── Copy link to selection
```

Te copia una URL con el formato:

```
https://www.figma.com/file/ABC123/Mi-Diseno?node-id=12%3A345
```

> El `node-id` al final es lo que el MCP necesita
> para identificar exactamente qué objeto del fichero te interesa.

---

## Slide 21 — Link-based: cómo se usa

En Claude Code:

```
Genera un componente Angular para esta tarjeta de pedido:
https://www.figma.com/file/ABC123/Mi-Diseno?node-id=12%3A345
```

```
✅ VENTAJA
   Explícito y trazable.
   Puedes documentar qué link va con qué componente.

⚠️ LIMITACIÓN
   Más fricción para flujos rápidos.
```

---

## Slide 22 — Cuál usar cuándo

La regla práctica que la gente experimentada termina aplicando:

```
EXPLORACIÓN RÁPIDA Y EXPERIMENTACIÓN
└── selection-based.
    Vas tocando frames en Figma,
    generando código, iterando.

TRABAJO SERIO SOBRE UN FRAME CONCRETO
└── link-based.
    El link queda en la conversación
    y siempre puedes volver a él.

DOCUMENTACIÓN O HANDOFF A OTRO MIEMBRO
└── SIEMPRE link-based.
    "El componente de tarjeta es esto: <link>"
    Es accionable por cualquiera.
```

---

## Slide 23 — Las herramientas principales del MCP

El Figma MCP expone varias herramientas. Las **dos** que más vas a usar:

```
1. get_design_context
2. get_variable_defs
```

> Las cubrimos las dos. Y al final, las "otras" en una nota.

---

## Slide 24 — get_design_context

**La herramienta principal.**

```
Le pasas una selección o un node-id.

Te devuelve una REPRESENTACIÓN ESTRUCTURADA
del diseño.
```

```
Por defecto, esa representación viene en formato
React + Tailwind.

Eso es lo que Figma considera "lengua franca"
de cara al modelo.

Pero el agente la traduce al framework que tú le pidas.
```

---

## Slide 25 — get_design_context: lo que devuelve

```
El MCP NO devuelve una imagen.

Devuelve una ESTRUCTURA:

├── jerarquía de capas
├── dimensiones
├── colores
├── tipografías
├── spacings
├── componentes referenciados
└── variables aplicadas

El modelo razona sobre esa estructura
para producir tu código.
```

---

## Slide 26 — La implicación de devolver estructura

```
Esto tiene una IMPLICACIÓN:

el resultado depende mucho
de cómo esté hecho el Figma.
```

```
SI EL DISEÑADOR USÓ AUTO LAYOUT
└── el MCP entiende el comportamiento responsive.

SI LO POSICIONÓ TODO A MANO
└── el MCP solo ve coordenadas absolutas
    └── el código generado va a ser FRÁGIL.
```

---

## Slide 27 — get_variable_defs

```
Extrae los TOKENS DE DISEÑO
usados en la selección.
```

```
Si el equipo de diseño tiene definidas variables de Figma
para colores, tipografía, spacing, radius, sombras
└── esta herramienta te las devuelve
    POR SU NOMBRE (no su valor).
```

> Esto es **clave** para que el código generado sea mantenible.

```
SIN esta herramienta:
└── el código generado tendría
    hex codes y px hardcodeados.

CON esta herramienta:
└── el código referencia los tokens del design system.
    Si el equipo de diseño cambia un color,
    tu código no se rompe.
    El token mantiene su nombre.
```

---

## Slide 28 — get_variable_defs: cuándo pedirlo explícitamente

Si te encuentras con que Claude está generando código con valores raw en vez de tokens, **pídelo explícitamente**:

```
Genera el componente, pero antes extrae
los nombres y valores de las variables usadas en este frame
con get_variable_defs

y úsalas como referencias en el código generado.
```

> Sin esto, el código no es mantenible.
>
> Con esto, está alineado con tu design system.

---

## Slide 29 — Otras herramientas

El MCP expone más cosas:

```
├── Integración con Code Connect
│   (mapea componentes Figma con tu código real)
│
├── Escritura al canvas
│   (crear/modificar Figma desde Claude Code)
│
├── Acceso a FigJam
│
└── Recursos de Figma Make
```

> Pero las dos de antes son las que vas a usar
> el **80% del tiempo**.
>
> Las demás aparecen en flujos más avanzados.

---

## Slide 30 — Lo que viene en 4.1b

```
SUBMÓDULO 4.1b — CASOS PRÁCTICOS Y DESIGN.MD COMO COMPLEMENTO
─────────────────────────────────────────────────────────────

Caso práctico guiado: EXTRACCIÓN DE TOKENS DEL DESIGN SYSTEM
├── El flujo completo
├── Resultado: _tokens.scss
└── Mantenimiento (skill sync-design-tokens)

Caso práctico guiado: GENERACIÓN DE UN COMPONENTE ANGULAR
├── El flujo (frame estructurado, prompt detallado)
├── Lo que obtienes (layout, estilos, tokens)
└── Lo que NO obtienes automáticamente
    (lógica, estados ocultos, animaciones)

5 BUENAS PRÁCTICAS EN EL LADO FIGMA
├── Componentes para todo lo que se reutiliza
├── Variables para spacing, color, radius, tipografía
├── Auto layout, no posicionamiento absoluto
├── Nombres semánticos en las capas
└── Code Connect para mapear componentes con código real

7 LIMITACIONES REALES DEL MCP

ANTI-PATRONES + ERRORES FRECUENTES

DESIGN.MD COMO COMPLEMENTO NATURAL AL FIGMA MCP
├── Qué es
├── Por qué importa
├── Cómo se genera desde Figma
└── Bridge a 4.3 donde se cubre como tema central
```

**Nos vemos en 4.1b.**
