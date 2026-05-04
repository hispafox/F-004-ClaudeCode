> **Versión:** v2 | **Módulo:** 4 | **Sub:** 4.3a | **Slides:** 32 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M04-S4.3a-design-md-anatomia-generacion-onboarding-v2.md`

# Submódulo 4.3a — DESIGN.md: anatomía, generación y onboarding

## Slide 1 — Portada
**Módulo 4 · Submódulo 4.3 · Parte A**
DESIGN.md: anatomía, generación y onboarding
Por qué importa, anatomía, tres caminos para generarlo, onboarding en Claude Design

---

## Slide 2 — Recogiendo el hilo

```
Llevamos dos apartados mencionando DESIGN.md
sin entrar a fondo.
```

```
EN 4.1
└── lo presentamos como complemento al Figma MCP.
    El patrón híbrido de extraer tokens una vez
    y consolidarlos en un fichero del repo.

EN 4.2
└── vimos cómo Claude Design lo lee durante el onboarding
    y lo prioriza sobre la inferencia automática del codebase.
```

> Aquí cerramos el módulo poniéndolo donde merece estar:
>
> **EN EL CENTRO**.

---

## Slide 3 — Por qué en el centro

```
A fecha de este curso, DESIGN.md está cambiando rápido
cómo los equipos materializan sus design systems
para los agentes.
```

**Y no hablamos de algo experimental con poca tracción:**

```
├── 5.000+ estrellas en GitHub
│   en sus primeras 72 HORAS de vida pública
│
└── 64.000+ estrellas en la colección comunitaria
    awesome-design-md
    (Stripe, Spotify, Apple, Figma...)
```

> Es un caso raro de adopción tan rápida
> que sugiere que la idea estaba esperando ser publicada.

---

## Slide 4 — El caso que no hemos cubierto todavía

```
Hay un caso bastante común que no hemos cubierto bien:

¿QUÉ PASA SI NO TIENES FIGMA?
```

**Pueden ser muchos casos:**

```
├── Eres un equipo pequeño
├── Eres un developer trabajando solo
└── Una empresa que nunca invirtió 
    en herramientas de diseño
```

```
Ahí Figma MCP NO aplica.

Y Claude Design por sí solo te deja un design system IMPLÍCITO
└── el que infiere de tu codebase.
```

> DESIGN.md es la pieza que te permite tener un sistema
> **EXPLÍCITO Y VERSIONADO**
> sin pasar por una herramienta de diseño tradicional.

---

## Slide 5 — Las tres preguntas que vamos a responder

```
1. ¿Qué es exactamente DESIGN.md
   y cómo se escribe uno?

2. ¿Cómo lo genero
   — desde Figma, desde Stitch, a mano?

3. ¿Cómo encaja en el flujo del equipo
   y se mantiene vivo?
```

> Las primeras dos en esta parte A.
> La tercera en 4.3b.

---

## Slide 6 — Origen y contexto

```
Google Labs publicó la especificación de DESIGN.md
como open source el 21 de abril de 2026
bajo licencia Apache 2.0.

Repositorio oficial:
└── github.com/google-labs-code/design.md
```

```
Originalmente desarrollado para STITCH
└── su tool de diseño de UI con Gemini.

Lo abrieron al público con la idea de que
se convirtiera en un ESTÁNDAR CROSS-TOOL.
```

---

## Slide 7 — La cita del autor

David East, de Google Labs, lo describió en el vídeo de anuncio así:

```
"Una capa de contexto persistente
 — algo que el agente lee una vez
 al arrancar la sesión,
 NO algo que el developer tiene que re-explicar
 con cada prompt."
```

> Ese es el mental model que conviene fijar:

```
DESIGN.md no es un fichero de tokens más.

Es un CONTRATO VERSIONABLE
entre tu marca y los agentes
que escriben código contra ella.
```

---

## Slide 8 — Adopción inmediata

```
Anthropic está empujándolo
└── hay una issue activa en el repo anthropics/skills
    proponiendo que el skill oficial frontend-design
    consuma y produzca DESIGN.md.

Cursor, Codex CLI y Antigravity
└── lo soportan.

La colección comunitaria awesome-design-md
└── ofrece DESIGN.md ya hechos para marcas conocidas
    que puedes usar como punto de partida
    o como referencia estilística.
```

---

## Slide 9 — La analogía que conviene tener clara

La mejor forma de entenderlo es por analogía:

> ## **DESIGN.md es a diseño lo que `CLAUDE.md` o `AGENTS.md` es a código.**

```
Los tres ficheros comparten la misma idea fundamental:

├── Plain text en el repo, versionado en git
├── Lo leen los agentes al arrancar como contexto persistente
├── Sobreviven entre sesiones porque están en el repo,
│   no en la conversación
└── Markdown, formato que los LLM leen mejor
    que casi cualquier otro
```

---

## Slide 10 — Mismo patrón, dominio distinto

```
CLAUDE.md
└── te ahorra explicar tus convenciones de código
    en cada conversación.

DESIGN.md
└── te ahorra explicar tu sistema visual
    en cada generación.
```

> Mismo patrón, dominio distinto.

---

## Slide 11 — La filosofía: what + why

Aquí está lo que hace al formato distinto de un simple fichero de tokens en JSON:

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   El WHAT (tokens) es para máquinas.                     │
│                                                          │
│   El WHY (prose) es para criterio.                       │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

---

## Slide 12 — Qué hacen el what y el why

```
LOS TOKENS son los valores exactos.

El agente los lee LITERAL
y los aplica al generar código.

"primary: #1A1C1E"
└── significa que el botón primario va con ese hex,
    NO con uno parecido.
```

```
LA PROSE es lo que el agente consulta
cuando aparece un caso que los tokens no cubren.
```

> Si tienes un componente nuevo
> (una alerta de seguridad, un wizard de onboarding)
> que NO estaba en tus tokens,
>
> el agente necesita CRITERIO para decidir
> qué colores y qué tipografía usar.

---

## Slide 13 — Por qué la prose es crítica

```
SIN LA PROSE
└── cada componente nuevo es un parto:
    ├── el agente adivina
    ├── tú corriges
    └── el agente vuelve a adivinar.

CON LA PROSE
└── el agente toma decisiones razonables a la primera
    porque entiende el sistema.
```

**La prose le da al agente cosas como:**

```
├── "la marca es minimalismo arquitectónico
│   con gravitas periodística"
│
├── "el accent solo se usa para CTAs primarias"
│
└── "evita display fonts, mantén una sola familia tipográfica"
```

> Un fichero JSON de tokens **NO PUEDE** capturar la prose.
>
> Por eso el formato escogió Markdown.

---

## Slide 14 — Anatomía: ejemplo completo

Vamos al detalle del fichero. Lo cubrimos con un ejemplo completo y luego desglosamos cada pieza.

```yaml
---
version: alpha
name: Heritage
description: Editorial design system for a digital news publication
colors:
  primary: "#1A1C1E"
  secondary: "#6C7278"
  tertiary: "#B8422E"
  neutral: "#F7F5F2"
  accent: "#B8422E"
typography:
  h1:
    fontFamily: Public Sans
    fontSize: 3rem
    fontWeight: 600
    lineHeight: 1.1
    letterSpacing: -0.02em
  h2:
    fontFamily: Public Sans
    fontSize: 2rem
    fontWeight: 600
  body-md:
    fontFamily: Public Sans
    fontSize: 1rem
    fontWeight: 400
    lineHeight: 1.5
  label-caps:
    fontFamily: Space Grotesk
    fontSize: 0.75rem
    letterSpacing: 0.1em
```

---

## Slide 15 — Anatomía: tokens estructurales y componentes

Sigue el frontmatter:

```yaml
rounded:
  sm: 4px
  md: 8px
  lg: 16px
spacing:
  xs: 4px
  sm: 8px
  md: 16px
  lg: 24px
  xl: 48px
components:
  button-primary:
    backgroundColor: "{colors.primary}"
    textColor: "{colors.neutral}"
    typography: "{typography.label-caps}"
    rounded: "{rounded.sm}"
    padding: "{spacing.sm} {spacing.md}"
  button-secondary:
    backgroundColor: transparent
    textColor: "{colors.primary}"
    typography: "{typography.label-caps}"
    rounded: "{rounded.sm}"
    padding: "{spacing.sm} {spacing.md}"
---
```

> Fíjate en las **token references**: `{colors.primary}`.
>
> Lo veremos a continuación.

---

## Slide 16 — Anatomía: la prose

Y debajo del frontmatter, la prose en Markdown:

```markdown
## Overview

Architectural Minimalism meets Journalistic Gravitas. 
The UI evokes a premium matte finish — a high-end broadsheet 
or contemporary gallery. Restraint over ornamentation; 
typography over decoration.

## Colors

The palette is rooted in high-contrast neutrals 
and a single accent color.

- **Primary (#1A1C1E):** Deep ink for headlines and core text.
- **Secondary (#6C7278):** Sophisticated slate for borders, 
  captions, metadata.
- **Tertiary (#B8422E):** Boston Clay — used exclusively 
  for primary CTAs and editorial highlights. 
  Never for body text.
- **Neutral (#F7F5F2):** Warm limestone background, 
  off-white cards.
```

---

## Slide 17 — Anatomía: prose con guidelines

Más del cuerpo Markdown:

```markdown
## Typography

Public Sans for editorial gravitas. Avoid display fonts. 
The hierarchy is achieved through size and weight, 
not through fontFamily switching.

## Components

### Buttons

Primary buttons use the `button-primary` token. 
Reserved for the single most important action on a page. 
NEVER use more than one primary CTA per view.

Secondary buttons use `button-secondary`. Outlined variant. 
Used for supporting actions.

There is no tertiary button by design — if you need three 
levels of emphasis, you have too many options on the page.
```

> Esa última frase es el ejemplo perfecto de **rationale**.
>
> No es un valor de token. Es una **decisión de diseño**.

---

## Slide 18 — Desglose del frontmatter

Los campos del schema (versión alpha actual):

```
version
└── opcional, valor actual "alpha".
    Marca la versión de la spec contra la que validar.

name
└── nombre del design system. OBLIGATORIO.

description
└── descripción breve. Opcional pero recomendado.

colors
└── diccionario de tokens de color.
    Cada uno con <nombre>: <hex>.

typography
└── diccionario de tokens tipográficos.
    Cada token define fontFamily, fontSize,
    opcionalmente fontWeight, lineHeight, letterSpacing.

rounded
└── diccionario de border-radius por nivel de escala
    (sm, md, lg, etc.).

spacing
└── diccionario de spacings por nivel de escala.
    Acepta dimensiones (8px) o números (interpretados en px).

components
└── diccionario de tokens de componente.
    Propiedades válidas: 
    backgroundColor, textColor, typography, rounded,
    padding, size, height, width.
```

---

## Slide 19 — Token references

Una pieza importante. Dentro de los componentes puedes **referenciar otros tokens** con la sintaxis `{path.to.token}`:

```yaml
components:
  button-primary:
    backgroundColor: "{colors.primary}"
    textColor: "{colors.neutral}"
```

```
Esto:
├── EVITA DUPLICAR valores
└── MANTIENE el sistema coherente.
```

> Si cambias `colors.primary` en un sitio,
> todos los componentes que lo referencian
> **se actualizan solos**.

---

## Slide 20 — La parte de prose: convención emergente

El cuerpo Markdown está organizado en secciones `##`. **No hay secciones obligatorias**, pero la convención emergente es:

```
## Overview
└── la frase que captura el espíritu del sistema.
    "Architectural Minimalism meets Journalistic Gravitas".
    Una o dos líneas. Es lo primero que el agente lee.

## Colors
└── descripción de la paleta.
    Por qué cada color, cuándo usarlo.

## Typography
└── la jerarquía y el porqué.
    Reglas explícitas:
    "avoid display fonts",
    "hierarchy through size, not family".

## Layout
└── estrategia de spacing y grid.

## Components
└── guidelines a nivel de componente.
    Cuándo usar primary vs secondary, qué evitar.

## Elevation & Depth (opcional)
└── si el sistema usa sombras, cómo se aplican.
```

> Estos títulos NO son obligatorios pero dan
> **cobertura del 80% de los casos**
> que el agente va a encontrar.

---

## Slide 21 — Variantes de componentes

Las variantes (hover, active, pressed) se expresan como **componentes separados con un nombre relacionado**:

```yaml
components:
  button-primary:
    backgroundColor: "{colors.primary}"
    # ...
  button-primary-hover:
    backgroundColor: "{colors.primary-dark}"
    # ...
```

```
NO hay (todavía, en versión alpha)
un mecanismo nativo para anidar estados.
```

> Esto es una de las **gaps reconocidas**
> del formato actual.

---

## Slide 22 — Tres caminos para generar tu DESIGN.md

```
NO tienes que escribirlo de cero.
Ni mucho menos.
```

```
Hay tres rutas, cada una para un punto de partida distinto.
```

```
1. Vía Stitch directamente
2. Desde Figma con MCP
3. Escritura manual (con dos atajos)
```

> Los vemos.

---

## Slide 23 — Camino 1: vía Stitch directamente

La ruta más fácil si no tienes nada montado.

```
1. Vas a stitch.withgoogle.com
2. Describes tu marca o subes referencias visuales
3. Stitch te genera el DESIGN.md
```

```
La integración nativa con su propio formato
significa que el output sale LIMPIO.
```

**Stitch tiene además una capacidad muy útil:**

```
EXTRAE UN DESIGN.MD DESDE UNA URL PÚBLICA.

├── Apuntas a un sitio web 
│   (puede ser el tuyo en producción
│    o uno de referencia que te guste estilísticamente)
└── Stitch deriva un DESIGN.md de los estilos que detecta.
```

> *"Vibe extraction"*, lo llaman.

---

## Slide 24 — Camino 2: desde Figma con MCP

Lo introdujimos en 4.1 y conviene desarrollarlo ahora.

```
Si tienes Figma vivo con variables y componentes
└── el flujo óptimo es:

1. Configura Figma MCP (lo viste en 4.1).
2. Abre el frame del Design System en Figma.
3. En Claude Code, lanza el prompt.
```

---

## Slide 25 — El prompt completo desde Figma

```
Tengo seleccionado el frame de Design System en Figma. 
Genera un fichero DESIGN.md en la raíz del proyecto 
siguiendo la spec de Google Labs 
(YAML frontmatter + Markdown prose).

Para los tokens:
- Llama a get_variable_defs y mapea a colors, typography, 
  rounded, spacing.
- Para componentes, llama a get_design_context sobre los 
  componentes principales (Button, Card, Input) y extrae 
  sus propiedades válidas: backgroundColor, textColor, 
  typography, rounded, padding.
- Usa token references {colors.primary} en lugar de 
  duplicar hex.

Para la prose:
- Genera secciones Overview, Colors, Typography, Layout, 
  Components.
- Para Overview, infiere una descripción breve del estilo 
  basada en la paleta y la tipografía extraídas.
- Para Components, incluye reglas de uso 
  (cuándo primary vs secondary, etc.).

Valida que el resultado pase `npx @google/design.md lint`.
```

> Claude Code llama al MCP, transforma los tokens al formato correcto, escribe la prose razonable, y valida con la CLI.
>
> Si la validación falla, **itera hasta que pase**.

---

## Slide 26 — Camino 3: escritura manual

```
Tampoco es complicado escribir uno a mano.
```

**Para empezar, dos atajos que funcionan bien:**

```
ATAJO A: COPIAR UNO PARECIDO Y ADAPTAR
└── La colección awesome-design-md tiene 400+ DESIGN.md
    de marcas conocidas.
    
    Buscas uno cuya estética se parezca a la tuya:
    ├── Stripe si tu marca es corporate-tech
    ├── Vercel si es minimalista
    └── Mailchimp si es lúdica
    
    Lo copias, ajustas valores y prose a tu marca.
    └── 70% del trabajo ya hecho.

ATAJO B: REVERSE EXPORT DESDE TAILWIND
└── Si tu proyecto ya usa Tailwind:
    la CLI de DESIGN.md tiene un comando inverso
    que convierte tu tailwind.config.js
    en un DESIGN.md de partida.
```

---

## Slide 27 — Reverse export desde Tailwind

```bash
# (verifica el comando exacto en la versión actual de la CLI)
npx @google/design.md import --from tailwind tailwind.config.js > DESIGN.md
```

```
El output NO incluye prose
└── eso lo añades tú.

Pero los tokens están sacados de tu CONFIGURACIÓN REAL.

└── Lo que evita el problema de la página en blanco.
```

> A partir de ahí escribes la prose y queda listo.

---

## Slide 28 — Bonus: Stitch para web capture

Vale la pena destacarlo aparte porque es una capacidad **poco conocida**.

```
Si trabajas en una empresa que tiene un producto en producción
pero NUNCA consolidó un design system

└── STITCH PUEDE EXTRAERLO DEL SITIO EN VIVO.
```

```
1. Apuntas a tu URL de producción
2. Stitch analiza los estilos aplicados
3. Genera un DESIGN.md que recoge el estado actual
```

> Esto convierte el problema:
>
> *"tengo que escribir un design system desde cero,*
> *¿por dónde empiezo?"*
>
> en:
>
> *"tengo un punto de partida con todos mis valores reales,*
> *ahora ajusto"*.
>
> La diferencia entre esos dos problemas es **enorme**.

---

## Slide 29 — Onboarding del design system en Claude Design

Ya lo cubrimos en 4.2 a alto nivel. Aquí los detalles operativos.

**Qué lee Claude Design (en orden de importancia):**

```
1. El DESIGN.md del repo si existe
   └── PRIORIDAD MÁXIMA.

2. Ficheros de tokens si los detecta
   ├── tokens.json
   ├── theme.json
   └── configuración de Tailwind.

3. CSS variables / Sass variables del codebase.

4. Componentes recurrentes y sus estilos.

5. Logo assets y carpetas de fuentes.
```

```
A partir de ahí, INFIERE el design system y lo aplica
automáticamente a cualquier proyecto subsequente
que crees en Claude Design para esa empresa.
```

---

## Slide 30 — La jerarquía de la inferencia

Si hay conflicto entre fuentes, el orden de precedencia es:

```
1. DESIGN.md explícito → MANDA
2. Ficheros de tokens estructurados 
   (tokens.json, configuración de Tailwind)
3. Variables del codebase (CSS / Sass)
4. Inferencia desde componentes y estilos aplicados
```

```
Esto es exactamente lo que querrías que pasara:

├── lo más EXPLÍCITO gana
└── lo INFERIDO es fallback
```

> Si quieres garantías de que tu marca se aplica correctamente:
> **asegúrate de tener un DESIGN.md**.

---

## Slide 31 — Qué hace si NO encuentra design system

```
Si NO encuentra nada
├── ni DESIGN.md
├── ni tokens estructurados
└── ni codebase con coherencia visible
```

**Claude Design te ofrece CONSTRUIR UNO desde cero:**

```
1. Te hace preguntas
   ├── color base
   ├── tono visual
   └── referencias de marcas que te gustan

2. Genera una propuesta inicial.

3. Iteras con la conversación.

4. Al final te EXPORTA un DESIGN.md
   que puedes commit al repo
   para que el siguiente proyecto ya lo tenga.
```

> Es una forma cómoda de dar el primer paso
> si nunca formalizaste el sistema.

---

## Slide 32 — Lo que viene en 4.3b

```
SUBMÓDULO 4.3b — CLI, "SIN FIGMA", MANTENIMIENTO, CIERRE MÓDULO 4
─────────────────────────────────────────────────────────────────

LA CLI OFICIAL @google/design.md
├── lint (validador WCAG)
├── diff (comparación entre versiones)
├── export (a Tailwind, DTCG, CSS)
├── spec (output de la especificación)
└── Integración recomendada en CI

EL CASO "TRABAJO SIN FIGMA"
├── Cuándo encaja
└── Cómo se monta sin Figma

MANTENIMIENTO ITERATIVO
├── Trátalo como código
├── Quién lo edita
└── Versionado semántico (informal)

LIMITACIONES HONESTAS (estado alpha)

ANTI-PATRONES DE DESIGN.MD

ERRORES FRECUENTES

CIERRE DEL MÓDULO 4
├── Los tres pilares juntos
│   (Figma MCP + Claude Design + DESIGN.md)
└── Los tres flujos típicos
    (con Figma vivo, sin Figma, prototipado rápido)

BRIDGE A SESIÓN 5
```

**Nos vemos en 4.3b.**
