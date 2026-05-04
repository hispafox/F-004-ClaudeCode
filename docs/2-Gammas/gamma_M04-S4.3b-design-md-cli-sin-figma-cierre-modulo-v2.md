> **Versión:** v2 | **Módulo:** 4 | **Sub:** 4.3b | **Slides:** 33 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M04-S4.3b-design-md-cli-sin-figma-cierre-modulo-v2.md`

# Submódulo 4.3b — DESIGN.md: CLI, sin Figma, mantenimiento y cierre módulo 4

## Slide 1 — Portada
**Módulo 4 · Submódulo 4.3 · Parte B**
CLI, "trabajo sin Figma", mantenimiento y cierre del módulo 4
@google/design.md, integración CI, los tres pilares juntos

---

## Slide 2 — Dónde estamos

En 4.3a vimos por qué DESIGN.md importa, la analogía con `CLAUDE.md`, la filosofía **what + why**, la anatomía completa (frontmatter + token references + prose + variantes), los tres caminos para generarlo, y el onboarding en Claude Design.

Ahora cerramos el módulo 4:

```
1. La CLI oficial @google/design.md
   (lint, diff, export, spec)

2. El caso "trabajo sin Figma"

3. Mantenimiento iterativo y versionado

4. Limitaciones honestas del estado alpha

5. Anti-patrones y errores frecuentes

6. Cierre del módulo 4
   (los tres pilares y los tres flujos típicos)
```

---

## Slide 3 — La CLI oficial: @google/design.md

```
La spec viene con una CLI oficial que se invoca con:

npx @google/design.md
```

```
O instalación global:

npm install -g @google/design.md
```

**Cuatro comandos relevantes:**

```
1. lint     → el validador
2. diff     → comparación entre versiones
3. export   → conversión a otros formatos
4. spec     → output de la especificación
```

> Los vemos uno a uno.

---

## Slide 4 — Comando 1: lint, el validador

```bash
npx @google/design.md lint DESIGN.md
```

**Comprueba dos cosas:**

```
1. INTEGRIDAD ESTRUCTURAL
   ├── el frontmatter respeta la spec
   ├── los token references no apuntan a tokens inexistentes
   └── los campos de componentes son válidos

2. CONTRASTE WCAG
   Para cada par de colores que aparece junto en un componente
   (por ejemplo, backgroundColor y textColor de button-primary)
   └── comprueba el ratio de contraste 
       contra las reglas WCAG AA
```

---

## Slide 5 — Output del lint

Output JSON estructurado con findings por severity:

```json
{
  "findings": [
    {
      "severity": "warning",
      "path": "components.button-primary",
      "message": "textColor (#ffffff) on backgroundColor (#1A1C1E) has contrast ratio 15.42:1 — passes WCAG AA."
    }
  ],
  "summary": { "errors": 0, "warnings": 1, "info": 1 }
}
```

```
Esto es útil para INTEGRAR EN CI:
└── si introduces un cambio que rompe contraste WCAG
    └── el pipeline FALLA.
```

> Es la forma de evitar que un PR introduzca regresiones
> de accesibilidad antes de que lleguen al diseño implementado.

---

## Slide 6 — Comando 2: diff, comparación entre versiones

```bash
npx @google/design.md diff DESIGN.md.old DESIGN.md.new
```

Compara dos versiones del fichero y devuelve los cambios estructurados:

```json
{
  "tokens": {
    "colors": {
      "added": ["accent"],
      "removed": [],
      "modified": ["tertiary"]
    },
    "typography": { ... }
  },
  "regression": false
}
```

```
Útil para REVIEW DE PRs que tocan el design system.

En vez de leer un diff de YAML a mano:
└── ves de un vistazo
    qué tokens se añaden, cuáles se quitan, cuáles se modifican.
```

---

## Slide 7 — Comando 3: export, conversión a otros formatos

Esto cierra el círculo con build pipelines existentes:

```bash
# A Tailwind config
npx @google/design.md export --format tailwind DESIGN.md > tailwind.config.js

# A W3C Design Tokens Community Group JSON
npx @google/design.md export --format dtcg DESIGN.md > tokens.json

# A CSS custom properties
npx @google/design.md export --format css DESIGN.md > tokens.css
```

> Esto resuelve el problema de:
>
> *"vale, los agentes leen DESIGN.md*
> *pero mi build pipeline necesita tokens.json"*

```
La CLI te genera ambos formatos
a partir del MISMO fichero fuente.
```

> ## **Single source of truth, múltiples representaciones.**

---

## Slide 8 — Comando 4: spec, output de la especificación

```bash
npx @google/design.md spec
```

```
Imprime la spec completa.
```

**Útil cuando:**

```
Necesitas pasársela a un agente como contexto:

"sigue esta spec exactamente al generar el DESIGN.md"
```

> La CLI te da el texto autoritativo a inyectar.

---

## Slide 9 — Integración recomendada en CI

Mi recomendación práctica para un equipo serio:

```
1. PRE-COMMIT HOOK que ejecute lint
   └── Bloquea commits si hay errors de estructura o contraste.

2. PIPELINE DE PR que ejecute diff contra main
   └── Y publique un comentario con los cambios.

3. PIPELINE DE RELEASE que ejecute export
   └── A los formatos que tu build necesita.
```

> Esto convierte DESIGN.md de:
>
> *"un fichero más en el repo"*
>
> a:
>
> *"el contrato vivo del design system, validado en cada cambio"*.

---

## Slide 10 — El caso "trabajo sin Figma"

Aquí está el caso que muchos developers no tienen claro y que conviene explicitar.

```
NO todo el mundo tiene Figma.

Y NO todo el mundo lo necesita.
```

---

## Slide 11 — Cuándo encaja este caso

```
FOUNDER O EQUIPO PEQUEÑO sin diseñador dedicado
└── Tú haces producto y tú haces front.
    Nadie va a abrir Figma nunca.

EMPRESA INTERNA SIN CULTURA DE DISEÑO FORMALIZADA
└── El producto existe, funciona,
    pero nunca se invirtió en un design system explícito.

PROYECTOS B2B DONDE LA MARCA ES SECUNDARIA
└── El producto se vende por funcionalidad, no por estética.
    Nadie va a hacer pruebas A/B de paletas.

SIDE PROJECTS Y MVPs
└── La velocidad importa más que la pulida visual.
```

---

## Slide 12 — Por qué DESIGN.md es la pieza clave aquí

```
En estos casos:

├── FIGMA MCP no aplica
│   (no hay Figma)
│
└── CLAUDE DESIGN por sí solo te deja un sistema IMPLÍCITO
    que cambia entre prompts.
```

```
La pieza que falta es DESIGN.md
como CONTRATO EXPLÍCITO.
```

---

## Slide 13 — Cómo se monta sin Figma

El flujo en este escenario:

```
1. GENERAS UN DESIGN.MD INICIAL
   Tres opciones:
   ├── Vía Stitch describiendo tu marca en lenguaje natural.
   ├── Reverse export desde tu Tailwind config 
   │   si ya usas Tailwind.
   └── Copiando uno de awesome-design-md que se parezca 
       a tu vibe y adaptándolo.

2. LO COMMIT AL REPO
   En la raíz, junto al CLAUDE.md o AGENTS.md.

3. CONFIGURAS LA CLI EN CI
   Para validación automática.

4. A PARTIR DE AQUÍ
   Claude Code y Claude Design lo leen automáticamente.
   Cuando generes componentes o cuando crees prototipos
   └── ambos respetan los tokens.

5. ITERAS EL DESIGN.MD según el sistema evoluciona.
   Igual que iteras el CLAUDE.md.
   Es un fichero vivo.
```

---

## Slide 14 — El resultado

```
Tienes un design system:

├── EXPLÍCITO
├── VERSIONADO
├── VALIDADO
└── CONSUMIDO POR AGENTES

sin haber abierto Figma jamás.
```

> Y si en el futuro contratas un diseñador,
> le pasas el DESIGN.md como punto de partida.
>
> Verá el sistema actual y podrá refinarlo
> en lugar de empezar de cero.

---

## Slide 15 — Mantenimiento iterativo

```
DESIGN.md NO es un fichero que se escribe una vez
y se olvida.
```

```
Es un fichero VIVO que evoluciona con el producto.
```

**Algunas prácticas:**

---

## Slide 16 — Trátalo como código

```
Vive en git, igual que el código.

├── Los cambios pasan por PR
├── Hay review
├── Hay diff antes de mergear
└── La CLI valida en CI
```

```
Esto puede sonar burocrático para equipos pequeños.

Y para un proyecto solo, no necesitas tanto.
```

> Pero para equipos donde más de una persona
> toca el sistema visual,
> la disciplina **previene el caos**.

---

## Slide 17 — Quién lo edita

La pregunta política: ¿quién puede modificar el DESIGN.md? **Tres modelos típicos:**

```
OWNER ÚNICO
└── Una persona del equipo (o el lead de diseño si lo hay)
    es responsable.
    Cambios se proponen vía PR pero solo merge ella.
    └── Funciona para equipos pequeños.

DESIGNER-LED CON REVIEW
└── Diseñadores proponen.
    Devs revisan implicaciones técnicas.
    └── Funciona para equipos con departamento de diseño.

OPEN CONTRIBUTION CON CI
└── Cualquiera puede proponer cambios.
    Pero la CLI tiene que pasar
    (no se pueden romper contrastes WCAG,
     no se pueden añadir tokens que rompan referencias).
    └── Funciona si tu equipo tiene madurez técnica suficiente.
```

---

## Slide 18 — Versionado semántico (informal)

A medida que el sistema evoluciona, conviene tener idea de **cuánto** ha cambiado.

Convención que algunos equipos están adoptando:

```
PATCH
└── Corrección de error obvio,
    ajuste menor 
    (cambio de un valor por un picante visual).

MINOR
└── Añadir tokens nuevos, añadir componentes,
    refinamientos NO breaking.

MAJOR
└── Cambio de paleta, cambio de tipografía,
    refactor estructural.
    Requiere coordinación porque rompe la coherencia
    con código existente.
```

```
NO hay enforcement,
pero documentarlo en commits o changelogs
ayuda al equipo a saber cuándo un cambio
va a requerir actualizaciones extensas en código.
```

---

## Slide 19 — Limitaciones honestas (estado alpha)

La spec está en **versión alpha** y conviene saber qué hay y qué no.

```
1. LOS NOMBRES DE CAMPOS PUEDEN CAMBIAR
   Mientras la spec esté en alpha,
   los maintainers han avisado de que field names 
   pueden cambiar antes de la versión estable.
   Tooling que construyas hoy puede necesitar updates
   cuando la spec madure.

2. NO HAY ENFORCEMENT DURANTE LA GENERACIÓN
   El lint detecta problemas DESPUÉS 
   de que se han generado.
   No hay forma de impedir que un agente genere
   un componente con un hex que no está en los tokens.
   Para enforcement real necesitas component libraries 
   en código que solo acepten tokens válidos.

3. NO HAY TOKENS DE ANIMACIÓN NI INTERACCIÓN
   El formato cubre estática
   (colores, tipografía, spacing, formas)
   pero NO transiciones, easings, durations, microinteracciones.
   Si tu sistema visual depende de animaciones,
   las tienes que documentar fuera.
```

---

## Slide 20 — Más limitaciones honestas

```
4. LA SCHEMA DE COMPONENTES ES DELIBERADAMENTE FLEXIBLE
   Las propiedades válidas (backgroundColor, textColor, etc.)
   son un set fijo, pero la spec admite que los equipos
   definan tipos de componente más allá de los comunes.
   Esa flexibilidad es buena pero implica que cada equipo
   termine con convenciones ligeramente distintas
   para componentes domain-specific.

5. NO ES UN REEMPLAZO DE DESIGN TOKENS EN PIPELINES EXISTENTES
   Si tu equipo ya tiene un Style Dictionary
   o un pipeline de tokens.json maduro
   └── DESIGN.md NO lo sustituye.
   La CLI exporta a tokens.json y a tailwind.config
   para que coexistan.

6. COMO CUALQUIER FORMATO EMERGENTE, LA DOCUMENTACIÓN ES ESCASA
   La spec oficial es buena pero corta.
   Las preguntas raras se responden mirando ejemplos
   del repo o issues de GitHub.
   La curva de aprendizaje es manejable
   pero no esperes Stack Overflow lleno de respuestas.
```

---

## Slide 21 — Anti-patrones de DESIGN.md (1/2)

```
FRONTMATTER SIN PROSE
└── Generas el YAML, lo commiteas y te olvidas de la prose.
    Resultado: el agente tiene los valores
    pero NO el criterio.
    Cualquier componente nuevo es una aproximación.
    La prose NO es opcional desde un punto de vista práctico.

PROSE GENÉRICA
└── "Modern, clean, professional"
    Eso NO le dice nada al agente.
    
    La prose ÚTIL es ESPECÍFICA y RESTRICTIVA:
    ├── "avoid display fonts"
    ├── "primary color only for the single 
    │   most important CTA"
    └── "shadows minimal — depth through tonal layers, 
        not blur"
    
    Las restricciones son lo que da CARÁCTER.

TOKENS SIN TOKEN REFERENCES
└── Duplicar hex en cada componente
    en vez de referenciarlos.
    Cuando cambias un color,
    tienes que cambiarlo en seis sitios.
    Usa {colors.primary} siempre que puedas.

DEMASIADOS TOKENS
└── Un sistema con 47 colores nombrados
    es difícil de aplicar coherentemente.
    Sistema simple, paleta de 5-7 colores 
    con sus variaciones.
```

---

## Slide 22 — Anti-patrones de DESIGN.md (2/2)

```
NO VALIDAR EN CI
└── Sin el lint, los problemas
    (errores de estructura, contrastes que no pasan WCAG)
    se descubren cuando alguien los ve.
    Demasiado tarde.
    La CLI corre en menos de un segundo.
    NO hay excusa para no tenerla en pre-commit o CI.

NO ACTUALIZAR EL DESIGN.MD CUANDO CAMBIA EL CÓDIGO
└── Tu CSS evoluciona, alguien mete un color nuevo
    en un componente, y nadie lo añade al DESIGN.md.
    Tres meses después, el fichero ya no refleja la realidad.
    Solución: incluir actualización del DESIGN.md
    como parte del flujo cuando se introduce 
    algo visualmente nuevo.

PRETENDER QUE EL DESIGN.MD SUSTITUYE A UN COMPONENT LIBRARY
└── El DESIGN.md DESCRIBE el sistema.
    Los componentes vivos en código (React, Angular)
    son los que APLICAN el sistema.
    Sin ese código, el DESIGN.md es teoría.

MANTENER DESIGN.MD FUERA DEL REPO
└── Si lo guardas en Notion, Confluence o un Google Doc
    └── los agentes NO lo van a leer.
    
    Tiene que vivir EN EL REPO, como código.
```

---

## Slide 23 — Errores frecuentes con tus primeros DESIGN.md

```
❌ OLVIDAR LAS COMILLAS EN HEX CODES
   YAML interpreta #1A1C1E como comentario
   si NO está en comillas.
   Siempre "#1A1C1E".

❌ TOKEN REFERENCES ROTAS
   Pones {colors.primary-dark} en un componente
   pero no has definido primary-dark en colors.
   La CLI lo detecta — corre lint.

❌ MEZCLAR UNIDADES
   8px en spacing y 1rem en fontSize coexisten bien
   pero 8 (sin unidad) en spacing puede interpretarse
   distinto según el toolchain.
   Sé consistente — px o rem, pero no medio.

❌ COMPONENTES CON PROPIEDADES INVÁLIDAS
   Las válidas: backgroundColor, textColor, typography,
   rounded, padding, size, height, width.
   Otras propiedades (hoverColor, activeColor) NO son válidas
   └── exprésalas como componentes separados
       (button-primary-hover).

❌ OLVIDAR LA SECCIÓN COMPONENTS EN LA PROSE
   Definitions de componentes en YAML está bien
   pero sin la prose explicando CUÁNDO usar cada uno
   └── el agente no sabe si el botón primary va 
       con cualquier acción o solo con la principal.

❌ NO ITERAR
   El primer DESIGN.md NO es el final.
   Lo escribes, generas algunos componentes con él,
   ves dónde el agente se equivoca, refinas la prose.

❌ COMPARAR TU SISTEMA CON LOS DE MARCAS GRANDES Y DESANIMARSE
   Los DESIGN.md de Stripe o Spotify están extremadamente pulidos.
   El tuyo no necesita estar a ese nivel
   └── necesita estar a TU nivel.
   Empieza simple.
```

---

## Slide 24 — Cierre del módulo 4: los tres pilares juntos

Llegamos al final del módulo 4.

```
Hemos visto TRES HERRAMIENTAS
que cubren el ciclo de diseño completo.
```

> Y conviene tenerlas mapeadas con claridad
> para no confundirse.

---

## Slide 25 — Pilar 1: Figma MCP

```
Para cuando YA HAY un Figma vivo
y quieres que Claude Code lo entienda.
```

> La conexión más rica entre el lado humano del diseño (Figma)
> y el lado del código (Claude Code).

```
Lo que cubrimos en 4.1:

├── Setup remote o desktop, plan requirements
├── Dos formas de pasar contexto (selection / link)
├── get_design_context y get_variable_defs
├── Casos prácticos (tokens + componente Angular)
├── 5 buenas prácticas en lado Figma
└── Limitaciones reales (7) y anti-patrones
```

---

## Slide 26 — Pilar 2: Claude Design

```
Para cuando estás CREANDO diseño desde la conversación.
```

> Especialmente útil para:
> ├── exploración rápida
> ├── prototipos navegables
> ├── pitch decks
> └── trabajo sin diseñador dedicado

```
Lo que cubrimos en 4.2:

├── Acceso por plan, motor Opus 4.7
├── Datos de aceleración (Brilliant 20→2, Datadog)
├── 4 mecanismos de refinamiento
├── 4 modos de input
├── 6 tipos de output
├── Onboarding del design system con DESIGN.md
├── Exportación y handoff
└── Casos donde brilla / donde no compensa
```

---

## Slide 27 — Pilar 3: DESIGN.md

```
EL PEGAMENTO ESTRUCTURAL.
```

```
├── Vive en el repo
├── Lo leen los agentes 
│   (Claude Code, Claude Design, Cursor, Codex CLI)
└── Mantiene la coherencia visual a través del ciclo
```

> Especialmente importante en el caso "no tengo Figma"
> donde es la **única fuente de verdad**.

```
Lo que cubrimos en 4.3:

├── Origen y filosofía what + why
├── Anatomía completa
├── 3 caminos para generarlo
├── CLI oficial @google/design.md (lint, diff, export, spec)
├── Caso "trabajo sin Figma"
└── Mantenimiento iterativo
```

---

## Slide 28 — Los tres flujos típicos

Tres patrones de uso que cubren la mayoría de equipos.

```
FLUJO A: equipo con Figma vivo
FLUJO B: equipo sin Figma o con Figma esporádico
FLUJO C: prototipado rápido para validación
```

> Los vemos uno a uno.

---

## Slide 29 — Flujo A: equipo con Figma vivo

```
Figma (diseñadores)
    ↓
Figma MCP (extracción)
    ↓
DESIGN.md (consolidación periódica)
    ↓
Claude Code (implementación)
```

```
Aquí:
├── Figma es la fuente
├── DESIGN.md es snapshot reproducible
└── Claude Code lee DESIGN.md
    y ocasionalmente vuelve al Figma vía MCP
    cuando necesita más detalle de un componente concreto.
```

---

## Slide 30 — Flujo B: equipo sin Figma

```
Claude Design (creación visual)
    ↓
DESIGN.md (export/refinamiento manual)
    ↓
Claude Code (implementación)
```

```
Aquí NO hay Figma o se usa muy puntualmente.

La fuente de verdad es DESIGN.md
└── que se mantiene a mano
    o se itera con Claude Design.
```

---

## Slide 31 — Flujo C: prototipado rápido

```
Claude Design (prototipo)
    ↓
handoff bundle
    ↓
Claude Code (implementación rápida)
```

```
Para casos donde la velocidad de validación importa más
que la integración con un sistema preexistente.

El DESIGN.md viaja en el handoff.

Es el flujo de los founders y los equipos de innovation.
```

---

## Slide 32 — Antes de seguir: la pregunta que cierra el módulo 4

```
¿Tu equipo tiene un design system EXPLÍCITO ahora mismo?
```

> NO me refiero a *"tenemos colores que usamos"*
> o *"hay una guía de marca en algún PowerPoint"*.
>
> Me refiero a un fichero o tool del que se pueda decir
> *"aquí está el sistema, esta es la fuente de verdad"*.

```
SI LA RESPUESTA ES "SÍ, FIGMA CON VARIABLES Y COMPONENTES"
└── El primer paso el lunes:
    generar un DESIGN.md desde ese Figma vía MCP,
    commit al repo, empezar a usarlo como fuente.

SI ES "TENEMOS ALGO PERO ESTÁ DESACTUALIZADO"
└── El lunes generas un DESIGN.md a partir 
    del estado ACTUAL del código
    (reverse export desde Tailwind, 
     o Stitch sobre la URL de producción).
    Es más útil un sistema imperfecto pero alineado
    que uno perfecto pero teórico.

SI ES "NO TENEMOS NADA FORMALIZADO"
└── Este es probablemente el mejor punto de partida posible.
    Una hora con Stitch o con Claude Design 
    en modo construcción de design system,
    y tienes un DESIGN.md inicial.
    A partir de ahí, todo el equipo hereda esa coherencia
    automáticamente.
```

---

## Slide 33 — Lo que viene en sesión 5

```
SESIÓN 5 — HANDOFF, CASOS AVANZADOS, TESTS .NET
─────────────────────────────────────────────────────

5.1 — EL HANDOFF A CLAUDE CODE EN DETALLE
├── Cómo se empaqueta un diseño
├── Cómo lo recoge Claude Code
└── Qué traduce bien y qué no

5.2 — CASOS AVANZADOS
├── Prototipos interactivos
├── Pitch decks
└── Decision tree completo entre las tres herramientas

5.3 — PIVOTE A TESTS EN .NET CON CLAUDE CODE (60 min)
├── Donde el agente brilla
└── Donde casi todo equipo que prueba se queda 
    con la mecánica
```

> Cualquiera de los tres caminos del módulo 4
> te da algo concreto que enseñar al equipo el lunes.

```
Y eso es lo que mide
si el módulo 4 ha funcionado.
```

**Nos vemos en la sesión 5.**
