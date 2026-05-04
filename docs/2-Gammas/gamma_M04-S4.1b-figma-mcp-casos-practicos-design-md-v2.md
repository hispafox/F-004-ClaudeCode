> **Versión:** v2 | **Módulo:** 4 | **Sub:** 4.1b | **Slides:** 32 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M04-S4.1b-figma-mcp-casos-practicos-design-md-v2.md`

# Submódulo 4.1b — Figma MCP: casos prácticos y DESIGN.md como complemento

## Slide 1 — Portada
**Módulo 4 · Submódulo 4.1 · Parte B**
Casos prácticos, buenas prácticas y DESIGN.md como complemento
Extracción de tokens, generación de componente Angular, anti-patrones, DESIGN.md

---

## Slide 2 — Dónde estamos

En 4.1a vimos qué problema resuelve Figma MCP, sus dos versiones (remote y desktop), el setup paso a paso, las dos formas de pasar contexto (selection / link), y las dos herramientas principales (`get_design_context` + `get_variable_defs`).

Ahora vamos a aplicarlo:

```
1. CASO PRÁCTICO: extracción de tokens del design system
2. CASO PRÁCTICO: generación de un componente Angular
3. BUENAS PRÁCTICAS en el lado Figma (5)
4. LIMITACIONES reales del MCP (7)
5. ANTI-PATRONES + errores frecuentes
6. DESIGN.MD como complemento natural al Figma MCP
```

---

## Slide 3 — Caso práctico 1: extracción de tokens

```
Tu equipo de diseño tiene definidos en Figma
los tokens del design system:

├── colores primarios y secundarios
├── escala de tipografía
├── escala de spacing
└── radius
```

```
Quieres que esos mismos tokens estén en tu código Angular
como variables CSS o en un fichero TypeScript.
```

**Sin MCP:**

> trabajo de copy-paste manual de cada token,
> esperando no equivocarte.

**Con MCP:**

> lo haces en un par de prompts.

---

## Slide 4 — El flujo

```
1. En Figma, ve al frame del DESIGN SYSTEM.
   La página o frame donde están todos los tokens documentados.
   Suele llamarse "Tokens", "Design system" o similar.

2. Selecciónalo entero (o copia el link).

3. En Claude Code, lanza el prompt.
```

---

## Slide 5 — El prompt completo

```
Tengo seleccionado el frame de tokens del design system 
de mi proyecto en Figma.

Quiero extraer todos los tokens definidos: 
colores, tipografía, spacing, radius y sombras.

Genera un fichero `src/styles/_tokens.scss` 
con todas las variables SCSS correspondientes, 
manteniendo los nombres exactos que tienen en Figma 
(con conversión a kebab-case).

Antes de generar el fichero, llama a get_variable_defs 
para obtener todas las variables. 

Si hay categorías (color/typography/spacing), 
agrúpalas en secciones en el fichero generado.
```

> Claude Code ejecuta el flujo:
> ├── llama al MCP
> ├── extrae las variables
> ├── las agrupa
> └── genera el fichero

---

## Slide 6 — Un resultado típico

```scss
// src/styles/_tokens.scss
// Generado desde Figma — no editar manualmente

// === Colores ===
$color-primary-500: #3B82F6;
$color-primary-600: #2563EB;
$color-primary-700: #1D4ED8;

$color-neutral-50: #F9FAFB;
$color-neutral-100: #F3F4F6;
$color-neutral-900: #111827;

$color-semantic-success: #10B981;
$color-semantic-error: #EF4444;
$color-semantic-warning: #F59E0B;

// === Tipografía ===
$font-family-sans: "Inter", system-ui, sans-serif;
$font-size-xs: 0.75rem;   // 12px
$font-size-sm: 0.875rem;  // 14px
$font-size-base: 1rem;    // 16px
$font-size-lg: 1.125rem;  // 18px
$font-size-xl: 1.25rem;   // 20px
```

> Limpio, agrupado por categorías, marcado como autogenerado.

---

## Slide 7 — Mantenimiento del fichero de tokens

La pregunta natural:

```
"¿qué pasa cuando el diseño cambie?"
```

**La respuesta:**

```
Cuando el equipo de diseño actualice un token en Figma:
└── vuelves a ejecutar el mismo prompt
    └── y regeneras el fichero.
```

```
Por eso conviene marcarlo como AUTOGENERADO al principio:
└── para que nadie lo edite a mano
    y se pierdan los cambios al regenerar.
```

> Una práctica que algunos equipos adoptan:
>
> **convertir esto en un skill**.
>
> Skill `sync-design-tokens` que dice
> *"actualiza el fichero `_tokens.scss` desde el frame de Design System en Figma"*.
>
> Cinco segundos para mantener sincronizados código y diseño.

---

## Slide 8 — Caso práctico 2: generación de un componente Angular

Segundo caso, más visual.

```
El equipo de diseño te pasa el frame de una TARJETA DE PEDIDO:

├── avatar
├── título
├── fecha
├── importe
└── botones

Quieres convertirla en un componente Angular standalone.
```

---

## Slide 9 — El flujo (preparación)

```
1. Asegúrate de que el frame está bien estructurado en Figma.
   
   Si NO usa Auto layout y tiene capas con nombres tipo "Group 47":
   └── PARA.
       Habla con el diseñador antes.
       El MCP va a generar código frágil.

2. Selecciona el frame. Copia el link.

3. En Claude Code, lanza el prompt.
```

---

## Slide 10 — El prompt: cabecera

```
Genera un componente Angular standalone llamado 
OrderCardComponent basado en este frame de Figma:

<link al frame>
```

---

## Slide 11 — El prompt: requisitos

```
Requisitos:

├── Componente standalone con Signals 
│   (no NgModules, no Subjects)
├── Imports en orden: 
│   Angular core → externos → internos → componentes hijo
├── Usa los tokens del design system 
│   (importa _tokens.scss desde src/styles/)
├── Inyecciones con inject(), no constructor
├── Props con signal input para: 
│   pedido (Order), variante ('compact' | 'detailed')
├── Output con signal output para: 
│   cancelClicked, viewDetailsClicked
├── Usa el control flow nuevo (@if, @for) en el template
└── Genera también los tests unitarios siguiendo 
    el patrón Arrange-Act-Assert
```

---

## Slide 12 — El prompt: instrucciones previas al MCP

```
Antes de generar:

1. Llama a get_variable_defs 
   para conocer los tokens usados

2. Llama a get_design_context 
   para entender la estructura
```

> Estas dos instrucciones son lo que asegura
> que el código salga alineado con el design system
> y respete la estructura del Figma.

---

## Slide 13 — Lo que Claude Code ejecuta

```
1. Pide al MCP los tokens usados.

2. Pide al MCP la estructura completa del frame.

3. Razona sobre la estructura,
   identifica jerarquía y comportamientos.

4. Genera los cuatro ficheros del componente.

5. Te enseña el resultado.
```

---

## Slide 14 — Lo que obtienes

```
Un componente que en términos de LAYOUT y ESTILOS
está bastante alineado con el Figma:

├── Spacings correctos
├── Colores correctos referenciando tokens
├── Tipografía correcta
└── Jerarquía visual respetada
```

---

## Slide 15 — Lo que NO obtienes automáticamente

```
LÓGICA DE NEGOCIO
└── El método cancelClicked es un emit vacío.
    Tú tienes que conectar la lógica real.

ESTADOS INTERACTIVOS SUTILES
└── Si el Figma tiene tres estados 
    (default, hover, active) 
    pero solo el default está dibujado:
    └── el MCP solo conoce el default.
        Los demás son intuición del modelo.

COMPORTAMIENTO RESPONSIVE COMPLEJO
└── Si el responsive no está claro en Figma 
    (Auto layout incompleto, sin variantes para móvil):
    └── el MCP genera lo que ve 
        y deja vacíos en lo demás.

ANIMACIONES Y TRANSICIONES
└── Si el diseño los tiene, hay que añadirlos a mano
    o reflejarlos en Figma de forma que el MCP los detecte
    (con annotations).
```

> **El MCP no te da un componente listo para producción.**
>
> Te da un punto de partida sustancialmente mejor que partir de cero.
>
> La parte de criterio sigue siendo tuya.

---

## Slide 16 — Buenas prácticas en el lado Figma

```
El factor que más afecta a la calidad del código generado
NO es el modelo de Claude Code.

Es CÓMO ESTÁ HECHO EL FIGMA.
```

```
Un diseño hecho con disciplina produce código bueno.

Un diseño hecho a mano y a ojo produce código frágil
aunque uses el mejor modelo.
```

> Si tu equipo de diseño no aplica buenas prácticas,
> el MCP va a decepcionar y la culpa la van a echar a la herramienta.

```
Cinco recomendaciones para compartir con tu equipo de diseño.
```

---

## Slide 17 — Buena práctica 1: componentes para todo lo que se reutiliza

```
Botones, cards, inputs, badges
└── cualquier elemento que aparezca más de una vez
    debería ser un Component de Figma
    NO un grupo copiado.
```

```
El MCP detecta componentes
└── y los traduce a componentes en código.

Si todo son grupos:
└── el código generado es DUPLICADO.
```

---

## Slide 18 — Buena práctica 2: variables de Figma

```
Variables de Figma para
└── spacing, color, radius, tipografía.

NO "color hex específico"
└── sino "Color/Primary/500".
```

```
Las variables se traducen a tokens en código.
Los hex sueltos se quedan hardcodeados.
```

> La diferencia entre código mantenible y código frágil.

---

## Slide 19 — Buenas prácticas 3, 4 y 5

```
3. AUTO LAYOUT, NO POSICIONAMIENTO ABSOLUTO
   Auto layout comunica intent:
   "este card tiene un padding de 16px 
    y los hijos están separados por 8px"
   
   Posicionamiento absoluto comunica solo coordenadas
   └── y el código generado replica esas coordenadas
       sin entender por qué.

4. NOMBRES SEMÁNTICOS EN LAS CAPAS
   "OrderCard / Header / Title"
   en vez de
   "Group 23"
   
   El nombre se usa por el MCP para inferir estructura
   y a veces para nombrar elementos en el código generado.

5. CODE CONNECT PARA MAPEAR COMPONENTES CON CÓDIGO REAL
   Esto es más avanzado:
   Figma permite que cada Component esté asociado a un 
   componente real de tu código (Angular, React, etc.).
   
   Cuando el MCP encuentra ese Component en un frame:
   └── en vez de regenerar el código del componente
       REFERENCIA el componente que ya tienes.
```

> Si tu equipo de diseño no está acostumbrado a estas prácticas,
> este es el momento de tener la conversación.

---

## Slide 20 — Limitaciones reales del MCP (1/3)

Sección honesta. Cosas que el MCP **no** hace bien y conviene saber antes de invertir tiempo.

```
1. NO ACTUALIZA CÓDIGO EXISTENTE BIEN
   El MCP es genial para generar componentes desde cero.
   Cuando el diseño evoluciona y quieres aplicar cambios
   al código existente, NO es tan limpio.
   
   Lo que hace en la práctica:
   └── regenera el componente entero,
       perdiendo cualquier modificación que tú habías hecho a mano
       (lógica de negocio, integraciones, edge cases).
   
   Tienes que hacer merge manual de cambios.

2. MULTI-FRAME FLOWS REQUIEREN COORDINACIÓN MANUAL
   Si tu diseño tiene un carrusel cuyas tres tarjetas
   están en tres frames separados:
   └── NO puedes pedirle al MCP "genera el carrusel completo".
       Tienes que generar cada frame, y luego pedirle a 
       Claude Code que combine los tres en un componente
       con navegación, transiciones y estado.

3. NO HAY REFINAMIENTO VISUAL LOOP
   Una vez generado el código, si el resultado tiene
   una sombra mal o un padding off-by-2:
   └── el MCP NO se entera.
       NO tiene forma de comparar el render real con el Figma.
       Tú lo ves, tú lo arreglas.
```

---

## Slide 21 — Limitaciones reales del MCP (2/3)

```
4. EL FIGMA DEBE ESTAR BIEN PARA QUE ESTO FUNCIONE
   La calidad del output depende mucho del input.
   Diseños "rápidos" mal estructurados producen código malo.

5. RATE LIMITS
   Aunque tengas Dev seat, hay rate limits por minuto.
   En sesiones intensas (regenerando muchas veces, iterando):
   └── puedes encontrarte el cap.
   
   Usa get_design_context con cabeza,
   no llames innecesariamente.
```

---

## Slide 22 — Limitaciones reales del MCP (3/3)

```
6. LA GENERACIÓN POR DEFECTO VIENE EN REACT + TAILWIND
   Eso es lo que el MCP devuelve internamente.
   
   Para Angular, el modelo lo traduce
   └── y la traducción casi siempre es buena.
   
   Pero ocasionalmente se filtran patrones React
   (useEffect, useState) que tienes que pedirle que reemplace.
   
   Si trabajas con Angular:
   └── en el prompt deja claro EL STACK desde el principio.

7. BETA PAGA EVENTUALMENTE
   Anthropic y Figma han indicado que esto será una
   "feature paga basada en uso" eventualmente.
   
   Ahora mismo está en beta y el coste se absorbe
   en el plan de Figma + Claude Code,
   pero es algo que vigilar.
```

---

## Slide 23 — Anti-patrones de uso del Figma MCP (1/2)

```
PRETENDER QUE EL MCP GENERA "EL CÓDIGO FINAL"
└── El MCP da un punto de partida.
    La parte de lógica, integraciones, accessibility avanzada,
    animaciones, edge cases — sigue siendo trabajo del dev.
    Si tu equipo espera que el MCP haga todo, va a decepcionar.

SALTARSE get_variable_defs
└── Si NO lo pides explícitamente, a veces el agente genera
    código con valores hardcodeados en vez de tokens.
    Pídelo siempre que el equipo de diseño tenga variables definidas.

TRABAJAR CON FRAMES MAL ESTRUCTURADOS
└── Si el Figma no está hecho con disciplina:
    el código generado va a ser frágil.
    La solución NO es pelear con el MCP
    └── es mejorar el Figma.
```

---

## Slide 24 — Anti-patrones de uso del Figma MCP (2/2)

```
NO DOCUMENTAR QUÉ FRAME HA GENERADO QUÉ COMPONENTE
└── Cuando el diseño cambie, vas a querer saber
    qué link de Figma corresponde a qué componente del código
    para regenerar limpio.
    Mete un comentario al inicio de cada componente generado:
    "Generado desde [link al frame] el [fecha]".

CONFIAR EN LA INFERENCIA DE ESTADOS OCULTOS
└── Si el Figma solo dibuja el estado default,
    el modelo va a inventar los estados hover/active/disabled.
    A veces bien, a veces no.
    Si los estados importan:
    └── pide al diseñador que los dibuje en variantes del componente.

USAR EL MCP PARA DISEÑOS PURAMENTE EXPLORATORIOS
└── Si el diseñador está iterando en un wireframe sin estructura clara,
    NO malgastes tokens generando código de algo que va a cambiar.
    Espera a que el diseño esté estabilizado.
```

---

## Slide 25 — Errores frecuentes con tus primeros usos

```
❌ OLVIDAR REINICIAR CLAUDE CODE TRAS INSTALAR EL MCP
   Las conexiones MCP se inicializan al arrancar.
   Si añades un MCP server con sesión abierta,
   no se conecta hasta el siguiente arranque.

❌ PASAR UN LINK A UNA PÁGINA ENTERA EN VEZ DE UN FRAME CONCRETO
   Te devuelve mucho más contexto del necesario,
   satura la conversación, genera código menos enfocado.
   Selecciona el frame específico antes de copiar el link.

❌ NO VERIFICAR EL SEAT PLAN DE FIGMA
   Si te encuentras con que después de 6 calls el MCP te bloquea:
   estás con seat View/Collab.
   Habla con tu admin de Figma.

❌ PRETENDER PIXEL-PERFECT SIEMPRE
   Habrá detalles que no cuadrarán.
   Si tu KPI es pixel-perfect, vas a frustrarte.
   Si tu KPI es "código alineado con el diseño en lo principal":
   eres feliz.
```

---

## Slide 26 — DESIGN.md: el complemento natural al Figma MCP

Una pieza nueva en el ecosistema que conviene mencionar antes de cerrar este apartado, porque cambia cómo conviene plantear el trabajo con tokens.

```
DESIGN.md
└── formato emergente abierto por
    Google Labs (Stitch) en abril de 2026
    bajo Apache 2.0
    └── github.com/google-labs-code/design.md
```

```
Originalmente desarrollado para el tool de diseño UI con Gemini.

Lo abrieron con la idea de que se convirtiera 
en un estándar cross-tool.
```

> Anthropic lo está empujando también.
> Claude Design lo soporta nativamente.
> Cursor, Antigravity, Codex CLI también lo están adoptando.

---

## Slide 27 — Qué es DESIGN.md

Un fichero markdown con dos partes que coexisten:

```markdown
---
name: Heritage
colors:
  primary: "#1A1C1E"
  secondary: "#6C7278"
  tertiary: "#B8422E"
typography:
  h1:
    fontFamily: Public Sans
    fontSize: 3rem
spacing:
  sm: 8px
  md: 16px
rounded:
  sm: 4px
---

## Overview

Architectural Minimalism meets Journalistic Gravitas.

## Colors

The palette is rooted in high-contrast neutrals 
with a single accent...
```

---

## Slide 28 — Las dos partes de DESIGN.md

```
FRONTMATTER YAML
└── los TOKENS machine-readable.
    Lo que el agente lee literal
    y aplica al generar código.
    
    Equivalente conceptual a un fichero de tokens JSON,
    pero en formato más compacto y legible.

MARKDOWN PROSE
└── el RATIONALE.
    Brand personality, decisiones,
    casos no cubiertos por tokens.
    
    Lo que el agente consulta cuando un caso requiere CRITERIO
    (un componente nuevo, una situación que los tokens no cubren).
```

> La frase que se ha vuelto la cita oficiosa del formato:
>
> *"el `what` (tokens) es para máquinas,*
> *el `why` (prose) es para criterio"*.

---

## Slide 29 — Por qué importa para Figma MCP

Hasta ahora el flujo que hemos visto era:

```
Cada vez que necesitas un componente
└── llamas a Figma MCP
    └── extrae tokens y estructura
        └── genera código.
```

```
Esto funciona pero tiene fricción:

├── Cada generación pasa por Figma
├── Cada vez tienes que pegar links o seleccionar frames
├── El equipo de diseño tiene que estar disponible
│   y los Figmas vivos
└── Los rate limits del MCP están ahí.
```

---

## Slide 30 — El patrón híbrido óptimo

El **patrón híbrido óptimo** con DESIGN.md cambia el flujo:

```
1. UNA VEZ
   usas Figma MCP para extraer todos los tokens
   del design system.

2. Generas un fichero DESIGN.md en la raíz del repo
   con esos tokens + la prose que da contexto.

3. A PARTIR DE AHÍ
   Claude Code (y Claude Design, y cualquier otro 
   agente compatible) lee ese DESIGN.md como 
   FUENTE DE VERDAD.

4. Cuando el design system cambia
   regeneras el DESIGN.md (otra vuelta al MCP)
   y commiteas el cambio.
```

**Ventajas concretas:**

```
├── CERO FRICCIÓN en el día a día.
│   El agente tiene los tokens siempre cargados al arrancar.
│
├── VERSIONADO EN GIT.
│   Los cambios del design system son cambios versionados,
│   con review y diff.
│
├── FUNCIONA OFFLINE.
│   No dependes del Figma MCP para cada operación.
│
├── CROSS-TOOL.
│   El mismo DESIGN.md sirve para Claude Code,
│   Claude Design, Cursor, etc.
│
└── EL EQUIPO DE DISEÑO PUEDE TRABAJAR SIN ESTAR DISPONIBLE.
    Tu DESIGN.md es estable hasta que decidas regenerarlo.
```

---

## Slide 31 — Cómo se genera DESIGN.md desde Figma

Tras el caso práctico de extracción de tokens que hicimos antes, el siguiente paso natural es transformar esos tokens al formato DESIGN.md:

```
Tengo seleccionado el frame de design system. 
Genera un fichero `DESIGN.md` en la raíz del proyecto 
con el formato de Google Stitch 
(YAML frontmatter + prose). 

Para los tokens, llama a get_variable_defs. 

Para la prose, infiere una breve descripción del estilo 
visual basada en la paleta y la tipografía extraídas. 

El fichero tiene que validar contra:
└── npx @google/design.md spec
```

```
Claude Code:
├── llama al MCP
├── transforma los tokens al frontmatter YAML correcto
├── escribe una sección de prose razonable
└── deja el DESIGN.md listo
```

**Si tienes la CLI instalada, lo validas:**

```bash
npx @google/design.md lint DESIGN.md
```

---

## Slide 32 — Lo que viene en 4.2 y 4.3

```
✅ Tienes el primer pilar del módulo 4:

├── Setup del Figma MCP
├── Dos formas de pasar contexto
├── Dos herramientas principales 
│   (get_design_context, get_variable_defs)
├── Dos casos prácticos completos
├── Buenas prácticas en el lado Figma
├── Limitaciones reales y honestas
└── DESIGN.md como complemento
```

```
SUBMÓDULO 4.2 — CLAUDE DESIGN
─────────────────────────────────────────────────────

Producto distinto del MCP de Figma.

├── El MCP conecta Claude Code con un Figma EXISTENTE
└── Claude Design es la herramienta de Anthropic
    para CREAR diseños de cero por conversación.

Es la otra mitad del flujo:
├── el MCP es para cuando ya hay diseño
└── Claude Design es para cuando lo estás creando.

Y verás que también soporta DESIGN.md de forma nativa.
```

```
SUBMÓDULO 4.3 — DESIGN.MD COMO TEMA CENTRAL
─────────────────────────────────────────────────────

├── Los tres caminos para generarlo
├── La CLI oficial con su validador WCAG
└── El caso "trabajo sin Figma" donde DESIGN.md
    es la única fuente del design system.
```

> Tener los tres en la mochila completa el ciclo:
>
> ideación → creación visual → handoff → código,
>
> con DESIGN.md como pegamento estructural.

**Nos vemos en 4.2.**
