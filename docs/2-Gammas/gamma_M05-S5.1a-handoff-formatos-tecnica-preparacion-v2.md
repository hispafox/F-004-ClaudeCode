> **Versión:** v2 | **Módulo:** 5 | **Sub:** 5.1a | **Slides:** 32 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M05-S5.1a-handoff-formatos-tecnica-preparacion-v2.md`

# Submódulo 5.1a — Formatos, handoff técnico y preparación

## Slide 1 — Portada
**Módulo 5 · Submódulo 5.1 · Parte A**
Formatos, handoff técnico y preparación
Cambio de fase, panorama de exportación, el handoff a Claude Code, cómo prepararlo

---

## Slide 2 — Cambio de fase: del trabajo creado al trabajo implementado

```
Cerrábamos el módulo 4 con los tres pilares
(Figma MCP, Claude Design, DESIGN.md)
y los tres flujos típicos.
```

```
Ya sabéis CREAR trabajo visual:
├── extraer tokens de un Figma
├── prototipar conversacionalmente en Claude Design
└── mantener un sistema vivo en DESIGN.md
```

> Aquí pasamos a la siguiente fase:
>
> **llevar ese trabajo a donde tiene que estar**.
>
> ├── A veces es una presentación al cliente
> ├── A veces un PDF para review
> ├── A veces un PPTX para una reunión interna
> └── A veces — la parte interesante para nosotros como devs —
>     **código real que Claude Code va a implementar**.

---

## Slide 3 — La pieza que destaca del módulo 5 entero

```
Si tuviera que destacar UNA pieza
del módulo 5 entero,
sería esta:
```

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   EL HANDOFF DE CLAUDE DESIGN A CLAUDE CODE              │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

> Es lo que Anthropic ha llamado *"loop cerrado"*:
>
> diseño y código en el mismo ecosistema,
> sin traducción, sin pérdida.

```
Es donde el producto deja de competir con Figma
y empieza a competir con algo más grande:

la cadena entera
└── idea → diseño → review → implementación

que tradicionalmente se ha repartido entre
4 herramientas y 3 equipos.
```

---

## Slide 4 — Estructura de esta parte A

```
1. Por qué el handoff ha sido un problema histórico
2. Panorama de formatos de exportación
   (rápido)
3. El handoff a Claude Code: cómo funciona técnicamente
   (la parte interesante)
4. Qué contiene el bundle
5. Por qué es distinto del handoff tradicional
6. El loop cerrado
7. Cómo preparar un handoff que RINDA
   (5 prácticas concretas)
```

> En 5.1b veremos qué traduce bien y qué no,
> permisos y colaboración honesta,
> el caso práctico completo,
> y los anti-patrones.

---

## Slide 5 — Por qué el handoff de diseño ha sido un problema

Antes de las herramientas, el problema. Cualquiera que haya estado en un equipo donde diseñadores y devs trabajan separados conoce este patrón:

```
1. El diseñador pasa horas en Figma haciendo un mockup pulido.
2. Se exporta a PDF / Zeplin / link compartible / capturas.
3. El dev recibe el material y empieza a interpretar.
   "¿Este botón es primary o secondary?
    ¿Cuál es el spacing aquí, parece 14 o 16?
    ¿Qué pasa cuando esto está en estado loading?"
4. El dev implementa lo que entiende. Lo enseña.
5. El diseñador revisa y ve cosas que se han perdido.
   "No, este border-radius era distinto.
    Y olvidaste el estado vacío."
6. Iteran. Una semana después, está bastante alineado
   pero no del todo.
```

---

## Slide 6 — Lo que se pierde en este flujo

```
Lo que se pierde en este flujo tiene un nombre:
```

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   DESIGN INTENT                                          │
│                                                          │
│   La INTENCIÓN del diseño.                               │
│   No solo CÓMO se ve, sino POR QUÉ se ve así.            │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

**Las decisiones que el diseñador tomó pero no escribió:**

```
├── Los estados que pensó pero no pintó
├── La jerarquía visual que tenía clara
│   pero no documentó
└── Las decisiones de diseño que NO se ven
    en el mockup final
```

> Las herramientas de handoff existentes
> (Zeplin, Avocode, plugins de Figma a código)
> intentan capturar esto pero **siempre con pérdida**.
>
> Lo que hacen es **traducir píxeles a especificaciones**.
>
> Y la traducción es **lossy**.

---

## Slide 7 — Lo que Claude Design propone es distinto

```
NO traducir, sino PRESERVAR.
```

```
Como Claude Design genera CÓDIGO REAL desde el principio
(no imágenes)
└── lo que se pasa al dev no es una traducción del diseño
    sino el DISEÑO MISMO.

La intención se preserva
porque vive en el mismo ecosistema.
```

> Esto es lo que cambia las reglas.

---

## Slide 8 — Los formatos de exportación: panorama

Cuando tienes un diseño listo en Claude Design, **click en Export** en la esquina superior derecha. Las opciones se agrupan en cinco categorías. Repaso rápido antes de la pieza estrella.

```
1. Para REVIEW INTERNA y feedback
2. Para DISTRIBUCIÓN a cliente externo
3. Para DEMO o despliegue
4. Para ACABADO PROFESIONAL
5. Para IMPLEMENTACIÓN
```

---

## Slide 9 — Para review interna y feedback

```
URL ORG-SCOPED
└── Link compartible dentro de tu organización.
    Tres niveles de permiso:
    ├── view
    ├── comment
    └── edit
```

> **Mi recomendación**: para feedback de stakeholders,
> este es el formato.
>
> NO exportes PDFs para una review.
>
> Usa el URL con permisos de comment
> y deja que la gente comente
> directamente sobre los elementos.

---

## Slide 10 — Para distribución a cliente externo

```
PDF
└── para clientes que esperan PDF.
    Sin sorpresa.

PPTX
└── para presentaciones formales a cliente o board.
    El output está bastante decente:
    NO es un mockup como imagen,
    es un PPTX EDITABLE.
```

---

## Slide 11 — Para demo o despliegue

```
HTML STANDALONE
└── exporta el diseño como un HTML AUTOCONTENIDO.
    Útil cuando quieres alojar el prototipo tú mismo
    (Vercel, Netlify, servidor interno).
    
    El prototipo conserva la interactividad:
    ├── clicks
    ├── navegación
    └── transiciones simples

.ZIP (Save as folder)
└── descarga la estructura completa de assets.
    Para casos donde necesitas el código fuente
    y los recursos asociados,
    SIN pasar por handoff a Claude Code.
```

---

## Slide 12 — Para acabado profesional: Canva

```
CANVA
└── los diseños van COMPLETAMENTE EDITABLES
    dentro de Canva, NO como imagen estática.
```

**Anthropic firmó una partnership oficial con Canva el 10 de abril de 2026**, una semana antes del lanzamiento de Claude Design.

```
Esto NO es export-to-image.
Es una integración real
└── el equipo de marketing puede tomar lo que generaste
    y refinarlo en su herramienta natural.
```

**Casos de uso:**

```
├── Marketing collateral
├── Social posts
└── Materiales de campaña
```

---

## Slide 13 — Tabla decisional rápida

| Necesidad | Formato |
|---|---|
| Feedback rápido de stakeholders | URL org-scoped (comment) |
| Presentación a cliente formal | PDF o PPTX |
| Demo navegable que quiero alojar | HTML standalone |
| Refinamiento por equipo de marketing | Canva |
| Implementación real en código | **Handoff a Claude Code** |
| Backup / archivo offline | `.zip` (folder) |

> La regla práctica: **un formato por audiencia**.
>
> NO exportes 5 formatos del mismo diseño "por si acaso".
>
> Cada formato es para un caso de uso.
> Genera el que necesitas, cuando lo necesitas.

---

## Slide 14 — El handoff a Claude Code: cómo funciona

```
Aquí está lo que diferencia a Claude Design
de cualquier otra herramienta de diseño.

Y donde, francamente, hay más valor para devs
en este curso.
```

**Desde el lado del usuario, el proceso es sencillo:**

```
1. Click en EXPORT → "Hand off to Claude Code"

2. Claude Design empaqueta el diseño en un HANDOFF BUNDLE
   que se sube a un API endpoint.

3. Te muestra un comando concreto que copias al portapapeles.

4. Pegas ese comando en tu Claude Code local (terminal).
   El comando incluye la URL del bundle.

5. Claude Code fetch el bundle desde el API endpoint,
   lo carga en su contexto, y empieza a implementar.
```

---

## Slide 15 — Variantes del handoff

```
HANDOFF A CLAUDE CODE WEB
└── Si NO estás en terminal, puedes usar la versión web
    de Claude Code.
    
    El flujo es idéntico:
    └── solo que en lugar de pegar el comando en tu terminal,
        lo pegas en una sesión web de Claude Code.
```

```
HANDOFF EXPORTADO COMO .ZIP
└── Puedes exportar el bundle como .zip
    y pasárselo a otros coding agents
    (Cursor, Codex CLI, lo que tengas).
    
    El formato del bundle es ABIERTO.
    
    Pero la integración mejor afinada
    es la NATIVA con Claude Code.
```

---

## Slide 16 — Qué contiene el bundle

```
Esto es lo que hace al handoff NO-LOSSY.
```

**El bundle NO es un export de imágenes — es un paquete estructurado que incluye 7 piezas:**

```
1. COMPONENT STRUCTURE como spec machine-readable
   No píxeles, no SVG.
   Una representación estructurada
   de los componentes y su jerarquía.

2. DESIGN TOKENS efectivamente usados en el canvas
   NO los tokens del DESIGN.md entero —
   solo los que aparecen en el diseño exportado.

3. LAYOUT HIERARCHY
   La estructura del documento,
   parent-child relationships, secciones.

4. REFERENCED ASSETS
   Imágenes, iconos, ficheros usados.
```

---

## Slide 17 — Las otras 3 piezas del bundle

```
5. EL HISTORIAL DEL CHAT
   Las decisiones que tomaste durante la conversación
   viajan con el bundle.
   
   Si dijiste:
   "vamos con tabs en lugar de sidebar 
    porque los usuarios necesitan ver 
    todas las secciones a la vez"
   
   esa razón está ahí.

6. UN README
   Le dice al modelo CÓMO INTERPRETAR
   los designs del bundle.
   
   Es el "manual de instrucciones" del paquete.

7. EL CONTEXTO DEL CODEBASE
   Si está linkado al proyecto.
   
   Las convenciones de tu código real.
```

---

## Slide 18 — Por qué es distinto del handoff tradicional

```
La frase que me parece más útil
para entender la diferencia:
```

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   "no necesita inferir intent desde píxeles"             │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

**Cuando un dev recibe un mockup tradicional:**

```
Su agente (sea él mismo o un agente de IA)
tiene que MIRAR LA IMAGEN y deducir qué es cada cosa.

"Esto parece un botón.
 Esto parece un input.
 Este espaciado parece de 16px"

Inferencia desde representación visual.
```

---

## Slide 19 — Con el handoff de Claude Design

```
Claude Code recibe DIRECTAMENTE:

"Esto es un button-primary con estos tokens.
 Este es un input-field-md.
 El spacing aquí es spacing.md (16px).
 El layout es flex column con gap spacing.lg"
```

```
NO HAY INFERENCIA. HAY ESPECIFICACIÓN.
```

> Esto pasa porque ambos lados —
> Claude Design y Claude Code —
> son del mismo *model family*.
>
> Claude Design escribe el bundle específicamente
> para que Claude Code lo consuma.
>
> NO es una traducción entre formatos:
> es un protocolo entre dos sistemas que se entienden.

---

## Slide 20 — El loop cerrado

Esta es la frase que Anthropic usa y que conviene tener clara:

```
prompt → design → handoff → code → feedback → prompt again
```

```
Mismo modelo, misma conversación,
sin saltos entre rooms.
```

**En la práctica esto significa tres cosas:**

---

## Slide 21 — Las tres consecuencias del loop cerrado

```
1. ITERAR ES BARATO
   Si después del primer pase de Claude Code
   algo no encaja con el diseño:
   ├── vuelves a Claude Design
   ├── ajustas
   ├── generas nuevo handoff
   └── le pasas el delta a Claude Code
   
   La transferencia es casi INSTANTÁNEA.

2. EL FEEDBACK VA EN AMBAS DIRECCIONES
   Si Claude Code ve algo que técnicamente NO es viable
   (un layout que rompe en mobile,
    un componente que no encaja con la librería existente)
   
   eso vuelve al diseño.
   
   NO es solo design → code:
   es DESIGN ↔ CODE.

3. EL CODEBASE REAL ES PARTE DEL CONTEXTO
   Si el repo está conectado:
   ├── Claude Design ya generó el prototipo
   │   respetando los componentes y patrones del codebase
   └── Claude Code los reconoce
       porque también los ha leído
   
   NO HAY RE-APRENDIZAJE entre las dos herramientas.
```

---

## Slide 22 — Cómo preparar un handoff que rinda

```
El bundle se genera SIEMPRE.

Pero LO BUENO QUE SEA EL RESULTADO
depende mucho de cómo prepares el diseño
ANTES del handoff.
```

> Cinco prácticas concretas que distinguen:
>
> ├── handoffs que producen código BUENO
> └── handoffs que producen código MEDIOCRE.

```
1. Documenta DECISIONES en el chat
2. Refiérete a componentes por su NOMBRE
3. FLAG edge cases antes del handoff
4. Mantén el design system explícito (DESIGN.md)
5. Pídele a Claude Design que se AUDITE antes de exportar
```

---

## Slide 23 — Práctica 1: documenta decisiones en el chat

Cuando tomas una decisión durante la conversación con Claude Design, **dila explícitamente**.

```
"Vamos con tabs en lugar de sidebar
 porque los usuarios necesitan
 ver todas las secciones a la vez"

"Cards en grid 3-cols en desktop,
 lista vertical en mobile"

"El estado vacío de esta tabla
 muestra una ilustración + CTA,
 NO un mensaje de texto"
```

```
Esto puede sonar a obviedad pero
la mayoría de la gente itera
con instrucciones cortas
("haz esto más grande", "cambia el color")
SIN documentar el porqué.
```

> El bundle viaja con el chat history.
>
> **Cada decisión documentada en chat
> es contexto que tu Claude Code va a recibir.**

---

## Slide 24 — Práctica 2: refiérete a componentes por su nombre

```
Si el codebase está conectado
y tienes un componente OrderCardComponent:
```

```
Refiérete a él POR SU NOMBRE
durante la conversación con Claude Design.
```

```
"para esta sección usa una variante 
 del OrderCardComponent en modo compact"
```

```
Los nombres SE PRESERVAN en el handoff.

Claude Code va a RECONOCER el componente
y REUTILIZARLO,
NO crear uno nuevo paralelo.
```

---

## Slide 25 — Práctica 3: flag edge cases antes del handoff

> Esta es la práctica que más diferencia
> los handoffs profesionales.

**Antes de hacer el handoff**, pídele a Claude Design que muestre cómo el diseño maneja los estados:

```
EMPTY STATE
└── "Muéstrame cómo se ve esta tabla cuando NO hay datos"

ERROR STATE
└── "Y cuando hay un error de carga"

LOADING STATE
└── "Y mientras los datos se están cargando"

EDGE DATA
└── "Cómo se ve cuando un usuario tiene 100 ítems
     vs cuando tiene 3"
```

---

## Slide 26 — Por qué importa

```
Cada uno de estos estados
SE MATERIALIZA en el canvas.

TODOS viajan en el bundle.
```

```
Cuando Claude Code recibe el handoff:
└── NO tiene que adivinar el estado vacío
    └── ya está diseñado y especificado.
```

> Sin esto, el resultado típico es:
>
> ├── el feliz path implementado
> └── los demás estados ausentes o medio-implementados.
>
> Y como devs sabemos:
> los **estados secundarios son donde más se nota la calidad**.

---

## Slide 27 — Práctica 4: mantén el design system explícito (DESIGN.md)

Como vimos en 4.3:

```
Si tienes un DESIGN.md en el repo:
└── Claude Design lo LEE al inicio.
```

```
Eso significa que el bundle generado
va a usar LOS TOKENS CORRECTOS de tu sistema,
NO aproximaciones.
```

**Si no tienes DESIGN.md:**

```
El bundle puede meter colores
que se parecen pero NO son los tuyos.
```

> La diferencia entre `#3B82F6` y `#3A82F5`
> es tu agente generando UI inconsistente.

---

## Slide 28 — Práctica 5: pídele a Claude Design que se audite antes de exportar

Una práctica que se está volviendo recurrente:

```
Antes del handoff, pídele a Claude Design
que REVISE SU PROPIO OUTPUT.
```

**Prompt:**

```
"Audita este diseño en:
 ├── accesibilidad
 ├── contraste
 ├── jerarquía de información
 └── consistencia con el design system

 Si encuentras problemas,
 corrígelos antes de que exporte"
```

---

## Slide 29 — Lo que captura la autoauditoría

```
La autoauditoría NO captura todo
pero sí coge los GAPS OBVIOS:
```

```
├── Contrastes que NO pasan WCAG
├── Tamaños de touch target inadecuados
└── Jerarquías visuales débiles
```

> Y eso ahorra **una vuelta entera del loop**.

---

## Slide 30 — Recap de las 5 prácticas

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   1. DOCUMENTA decisiones en el chat                     │
│   2. REFIÉRETE a componentes por su nombre               │
│   3. FLAG edge cases antes del handoff                   │
│   4. MANTÉN DESIGN.md vivo                               │
│   5. PIDE auditoría antes de exportar                    │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

```
Estas 5 prácticas son la diferencia entre:

├── un handoff que ahorra HORAS
└── un handoff que ahorra solo MINUTOS.
```

> NO son opcionales si quieres que el handoff rinda.

---

## Slide 31 — Lo que tienes ahora

```
✅ Por qué el handoff ha sido un problema histórico
✅ Panorama completo de formatos
   (con tabla decisional)
✅ Cómo funciona técnicamente el handoff a Claude Code
✅ Qué contiene el bundle (7 piezas)
✅ Por qué es distinto del handoff tradicional
✅ El loop cerrado y sus 3 consecuencias
✅ Las 5 prácticas para preparar un handoff que rinda
```

> Tienes el modelo conceptual del handoff.
>
> Falta cubrir lo que traduce bien y lo que no,
> los detalles operativos de colaboración,
> y verlo aplicado a un caso real.

---

## Slide 32 — Lo que viene en 5.1b

```
SUBMÓDULO 5.1b — TRADUCCIÓN, COLABORACIÓN, CASO PRÁCTICO, CIERRE
─────────────────────────────────────────────────────────────────

QUÉ TRADUCE BIEN AL CÓDIGO Y QUÉ NO
├── Lo que traduce bien (6 cosas)
└── Lo que requiere ajuste manual (7 cosas)

PERMISOS Y COLABORACIÓN
├── Los 3 niveles de permiso
├── Group conversations (la pieza interesante)
├── Lo que NO hay todavía (live cursors, audit logs)
└── Inline comments (cómo funcionan distinto que en Figma)

CASO PRÁCTICO GUIADO
└── Handoff completo de un prototipo de notificaciones,
    paso a paso (review → auditoría → export → import →
    feedback al diseño)

ANTI-PATRONES + ERRORES FRECUENTES

CIERRE Y BRIDGE A 5.2
```

**Nos vemos en 5.1b.**
