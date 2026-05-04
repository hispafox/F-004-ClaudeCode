> **Versión:** v2 | **Módulo:** 4 | **Sub:** 4.2a | **Slides:** 32 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M04-S4.2a-claude-design-producto-flujo-mecanismos-v2.md`

# Submódulo 4.2a — Claude Design: producto, flujo, mecanismos e inputs

## Slide 1 — Portada
**Módulo 4 · Submódulo 4.2 · Parte A**
Claude Design: producto, flujo, mecanismos e inputs
Cambio de polo, motor Opus 4.7, conversación, refinamiento, modos de input

---

## Slide 2 — Cambio de polo: del Figma existente al diseño que se crea por conversación

```
Hasta aquí, todo el módulo 4 iba sobre un escenario muy concreto:

YA HAY UN FIGMA.
```

```
Tu equipo de diseño hizo el trabajo.
Los frames están ahí.
Los componentes están ahí.
Las variables están ahí.

Tu trabajo es traducirlos a código sin perder nada.
```

```
Para ese escenario:
├── Figma MCP es la herramienta
└── DESIGN.md es la pieza que hace persistente lo extraído.
```

> Ahora cambiamos de polo.

---

## Slide 3 — Las preguntas que aparecen

```
¿Qué pasa cuando NO HAY FIGMA todavía?
```

```
¿O cuando hay un Figma pero quieres explorar diez direcciones
sin que el equipo de diseño se ponga a hacer mockups?
```

```
¿O cuando eres TÚ MISMO el que tiene que producir
un prototipo, un pitch deck o un one-pager
y NO tienes a un diseñador a mano?
```

> Para ese escenario, Anthropic ha lanzado **Claude Design**.

---

## Slide 4 — La otra mitad del flujo

```
Es la otra mitad del flujo:

├── el MCP es para cuando YA HAY diseño
└── Claude Design es para cuando lo ESTÁS CREANDO.
```

```
Y juntos, con DESIGN.md como pegamento estructural,
completan el ciclo:

ideación → creación visual → handoff → código.
```

> Este apartado es la introducción al producto y al flujo.
>
> En 5.1 cubriremos en detalle el handoff a Claude Code.
> En 5.2 la decisión de cuándo usar cada herramienta de las tres.
>
> Aquí el foco es entender:
> ├── qué es Claude Design
> ├── cómo se siente trabajar con él
> └── cuándo merece la pena.

---

## Slide 5 — Qué es Claude Design

```
Anthropic lanzó Claude Design el 17 de abril de 2026
como producto de Anthropic Labs.
```

```
La división de R&D que saca experimentos
antes de integrarlos a productos principales.
```

```
Ahora mismo está en RESEARCH PREVIEW.

En la jerga de Anthropic:
└── "está en producción pero no es un producto
     totalmente cerrado todavía
     y va a ir evolucionando rápido".
```

> La idea del producto, en una frase:
>
> **producir trabajo visual completo a través de una conversación**,
>
> sin necesidad de saber Figma, Sketch, Photoshop ni nada parecido.

---

## Slide 6 — Lo que NO es Claude Design

Conviene aclararlo desde el principio:

```
NO es un generador de imágenes.
```

```
Cuando le pides algo, NO te devuelve un PNG.

Te devuelve un artefacto FUNCIONAL:

├── HTML real
├── CSS real
├── Componentes React reales
└── Prototipos interactivos navegables
```

```
Eso significa que lo que produce se puede:
├── inspeccionar
├── editar
├── exportar
└── pasar a Claude Code para implementación
```

> Es **código disfrazado de diseño**.
>
> NO diseño disfrazado de imagen.

---

## Slide 7 — Acceso y planes

Disponible en `claude.ai/design`. Aparece como icono de paleta en la barra lateral izquierda de claude.ai si tu plan lo incluye.

| Plan | Estado |
|---|---|
| **Pro, Max, Team** | Activado por defecto |
| **Enterprise** | **Desactivado** por defecto. Hay que activarlo en organization settings |

> Los admins de Enterprise lo encuentran
> en la consola de Enterprise.

---

## Slide 8 — Atención al uso de tokens

```
Claude Design consume un WEEKLY QUOTA SEPARADO
del chat estándar y de Claude Code.
```

**Y la cantidad que consume es seria:**

```
Hay reviews documentados donde:

dos sesiones de diseño se llevaron
EL 58% DEL QUOTA SEMANAL DEL PLAN PRO.
```

```
Esto es importante saberlo antes de probarlo en clase:

Si tu equipo va a usarlo regularmente
└── Pro NO basta.
```

> Anthropic empuja claramente hacia **Max** (€100-200/mes)
> para uso continuo.
>
> Para Enterprise hay además un crédito inicial
> de "20 prompts típicos" que expira en julio.

```
Para el curso, asumimos que cada alumno tiene al menos plan Pro.
Para uso real en empresa, conviene proyectar el coste
antes de adoptarlo masivamente.
```

---

## Slide 9 — El motor: Claude Opus 4.7

```
Claude Design corre sobre CLAUDE OPUS 4.7
└── el modelo con visión más capaz de Anthropic
    en este momento.

Lanzado el mismo día que Claude Design 
└── 17 abril 2026.
```

**La capacidad clave para este caso de uso:**

```
VISIÓN DE ALTA RESOLUCIÓN
└── Opus 4.7 puede analizar imágenes hasta 2.576 px.
```

```
Esto significa que puede entender:
├── mockups complejos
├── capturas de pantalla densas
└── layouts con muchos elementos
└── sin perder detalle.
```

---

## Slide 10 — Por qué la visión importa

Esto importa porque **uno de los modos de input** de Claude Design es subir imágenes de referencia.

```
Si subes la captura de un dashboard de un competidor 
pidiendo:

"hazme algo en este estilo pero adaptado a mi marca"
```

```
El modelo tiene que ser capaz de LEER esa captura.

NO genéricamente:
└── con DETALLE suficiente para extraer la estructura.
```

> Opus 4.7 lo hace.

---

## Slide 11 — El flujo conversacional

La forma de trabajar con Claude Design **no se parece a Figma**.

```
NO tienes lienzo
NO tienes paleta de herramientas
NO tienes layers panel.
```

```
Tienes UNA CONVERSACIÓN con el agente.

Y el resultado del trabajo se va materializando
en un panel a la derecha
como un ARTEFACTO VIVO.
```

---

## Slide 12 — El loop básico

```
1. DESCRIBES LO QUE QUIERES
   "Prototipa una app móvil de meditación 
    con tipografía calmada, paleta de colores 
    inspirada en la naturaleza, layout limpio."

2. CLAUDE GENERA UNA PRIMERA VERSIÓN
   En segundos.
   Es funcional, no es un mockup estático.

3. REFINAS
   "Hazme la tipografía un poco más grande. 
    Añade dark mode. 
    Mete una pestaña de respiraciones guiadas."

4. CLAUDE ACTUALIZA EL ARTEFACTO
   Inmediatamente, en el mismo panel.

5. REPITES HASTA QUE ESTÉ.
```

---

## Slide 13 — Lo que la gente que viene de Figma encuentra extraño

```
NO estás moviendo elementos a píxel.
```

```
Estás describiendo el resultado
y Claude lo materializa.
```

```
SI NO TE GUSTA EL RESULTADO
└── le dices qué cambiar.

SI QUIERES EXPLORAR UNA DIRECCIÓN DISTINTA
└── le pides una variante.
```

> La granularidad del control es por **INTENCIÓN**,
> no por geometría.

---

## Slide 14 — Datos reales de cuánto acelera

Esto suena como marketing pero hay datos verificables que conviene tener en mente:

```
BRILLIANT (educación interactiva)
├── Páginas que requerían 20+ prompts en herramientas competidoras
└── Necesitaron solo 2 prompts en Claude Design
    para llegar a un resultado equivalente.

DATADOG
├── Comprimió un ciclo de UNA SEMANA
│   (briefs + mockups + rondas de review)
└── En UNA SOLA CONVERSACIÓN con Claude Design.

CASOS DE COMUNIDAD
└── Prototipos de landing completos en 12 prompts.
    Videos cortos en cantidades parecidas.
```

---

## Slide 15 — Qué significan estos datos

```
Esto NO significa que Claude Design haga magia.
```

```
Significa que para un determinado tipo de tarea
└── la EXPLORACIÓN RÁPIDA y la PRIMERA ITERACIÓN

el flujo conversacional ELIMINA LA FRICCIÓN
del handoff entre brief, diseño y review.
```

```
Lo que tradicionalmente se hacía en cadena:

PM escribe brief
→ diseñador interpreta
→ hace mockups
→ presenta
→ recibe feedback
→ itera

Se colapsa en UNA SOLA conversación.
```

> Para tareas distintas (acabado de marca pixel-perfect,
> work artístico complejo, animaciones sofisticadas):
>
> Claude Design **NO** compite con un buen diseñador con Figma.

---

## Slide 16 — Los cuatro mecanismos de refinamiento

Aquí está la pieza que **diferencia a Claude Design de un generador de imágenes** y, honestamente, lo que hace que funcione.

```
NO tienes una sola forma de pedir cambios.

Tienes CUATRO.

Y cada una para un tipo de cambio.
```

```
1. CONVERSACIÓN
2. COMENTARIOS INLINE
3. EDICIONES DIRECTAS DE TEXTO
4. SLIDERS PERSONALIZADOS
```

> Los vemos uno a uno.

---

## Slide 17 — Mecanismo 1: conversación

La forma más obvia. Le hablas en lenguaje natural sobre lo que quieres cambiar.

```
> El header se siente muy cargado. 
  Reduce el padding y haz el logo más pequeño.

> Cambia toda la paleta a tonos cálidos.

> Añade una sección de testimonios entre el hero 
  y los precios.
```

```
✅ Funciona BIEN para
   cambios estructurales o de tono.

⚠️ Funciona MAL para
   cambios pixel-perfect.
```

> *"Sube el botón 8 píxeles"* es algo que el agente
> puede no acertar a la primera porque está razonando
> sobre el resultado, no sobre coordenadas.

---

## Slide 18 — Mecanismo 2: comentarios inline en elementos concretos

Esta es la pieza que sorprende a los que vienen de chat.

```
Puedes hacer click sobre un elemento concreto del artefacto
y dejar un comentario directamente sobre él.
```

```
Claude entiende A QUÉ TE REFIERES
sin que tengas que describirlo.
```

```
[click sobre el botón "Get Started"]
> Hazlo más prominente, con sombra
```

> Más rápido que conversación cuando el cambio es localizado.
>
> Y reduce ambigüedad — no hay forma de que Claude se confunda
> y modifique el botón equivocado.

---

## Slide 19 — Mecanismo 3: ediciones directas de texto

Para cambios de copy, **NO le hablas a Claude**.

```
EDITAS EL TEXTO DIRECTAMENTE.

Como en un Google Doc.
```

```
El artefacto se actualiza en sitio.
```

**Útil cuando estás puliendo:**

```
├── Copy de un landing
└── Texto de slides
```

> La conversación NO es el camino más rápido para cambiar
> *"Welcome to our product"* por *"Built for product teams"*.
>
> Abrir el texto y editarlo es mejor.

---

## Slide 20 — Mecanismo 4: sliders personalizados (lo único de su tipo)

Aquí está la pieza más interesante y la que más sorprende.

```
Cuando trabajas en un elemento
└── Claude GENERA SLIDERS DINÁMICOS
    └── controles deslizantes contextuales
        para ajustar parámetros concretos del diseño 
        EN TIEMPO REAL.
```

**Ejemplos:**

```
Si Claude detecta que estás iterando sobre un BOTÓN:
└── te genera sliders de
    "Padding", "Border radius", "Color saturation".

Si estás iterando sobre una SECCIÓN:
└── te aparecen sliders de
    "Spacing", "Density", "Width".
```

---

## Slide 21 — Lo que hace especiales a los sliders

```
NO son sliders fijos.
```

```
Claude DECIDE qué sliders son relevantes
para lo que estás haciendo en ese momento.
```

> Esto es lo que diferencia a Claude Design
> de un generador de imágenes
> y, francamente, también de la mayoría
> de IDE-based design tools.

```
Los sliders dan FINE-GRAINED CONTROL
sin necesidad de meter números a mano.

Y el resultado se actualiza al mover el slider,
EN TIEMPO REAL.
```

---

## Slide 22 — Combinando los cuatro mecanismos

```
Cuando combines los cuatro mecanismos
ves por qué este producto preocupó a Figma.
```

```
CONVERSACIÓN
└── para lo macro

INLINE COMMENTS
└── para lo localizado

EDICIÓN DIRECTA
└── para texto

SLIDERS
└── para fine-tuning
```

> Cubre el espectro entero.

---

## Slide 23 — Los cuatro modos de input

Conviene saber desde dónde puedes arrancar un proyecto, porque **no todo arranca de cero**.

```
1. TEXT PROMPT
2. IMÁGENES / SKETCHES
3. DOCUMENTOS (DOCX, PPTX, XLSX)
4. WEB CAPTURE
```

> Los vemos.

---

## Slide 24 — Modo 1: text prompt

El más común. Describes lo que quieres en lenguaje natural.

**Mejor cuanto más específico:**

```
❌ MAL
   "hazme un dashboard"

✅ MEJOR
   "hazme un dashboard de analytics para SaaS B2B 
    con 4 KPI cards arriba, 
    gráfico de revenue mensual, 
    tabla de top customers, 
    sidebar para navegación. 
    Estilo limpio, paleta sobria."
```

> La especificidad del prompt es lo que separa
> "demo chula" de "diseño útil".

---

## Slide 25 — Modo 2: imágenes / sketches

```
Subes una imagen — captura de pantalla, sketch en papel 
fotografiado, mockup en otra herramienta —

y Claude la usa como REFERENCIA.
```

> Aquí es donde la visión de Opus 4.7 se nota.

**Casos típicos:**

```
"Hazme algo en este estilo"
└── + captura de un sitio que te gusta.

"Implementa este sketch que dibujé en una servilleta"
└── + foto del sketch.

"Mejora este mockup feo que hice yo"
└── + tu primera versión.
```

> ⚠️ **Ojo con el copyright**:
>
> Claude no copia diseños existentes literales,
> pero sí los usa como inspiración estilística.

---

## Slide 26 — Modo 3: documentos (DOCX, PPTX, XLSX)

```
Subes un documento
y Claude lo transforma en algo visual.
```

**Casos:**

```
UN BRIEF EN WORD
└── primer mockup que refleja lo que pide el brief.

UN PPT CON TEXTO SUELTO
└── versión visualmente mejorada.

UN EXCEL CON DATOS
└── dashboard que los visualiza.
```

> Especialmente útil para **PMs que tienen un documento de spec**
> y quieren saltar al prototipo
> sin pasar por Figma.

---

## Slide 27 — Modo 4: web capture

Una herramienta nativa de Claude Design para **capturar elementos directamente desde un sitio web vivo**.

```
1. Apuntas a una URL
2. Seleccionas qué partes quieres
3. Claude las trae como referencia para tu proyecto
```

**Caso típico de uso:**

```
Para que los prototipos parezcan más al producto real.
```

```
Si estás iterando en una nueva feature de tu app:
└── capturas elementos de la app real en producción
    └── y Claude los usa para que el prototipo
        tenga la misma sensación visual.
```

---

## Slide 28 — Recap: los inputs

```
TEXT PROMPT
└── arrancas con la idea, sin nada más.

IMÁGENES
└── arrancas con una referencia estilística.

DOCUMENTOS
└── arrancas con contenido estructurado a transformar.

WEB CAPTURE
└── arrancas con un producto real como base.
```

> Cuatro puntos de partida distintos.
>
> Para cuatro tipos de proyecto distintos.

---

## Slide 29 — Lo que tienes ahora

```
✅ Sabes qué es Claude Design
   y qué NO es (no es un generador de imágenes)

✅ Conoces el acceso por plan
   y la atención al consumo de tokens

✅ Sabes que corre sobre Opus 4.7
   con visión hasta 2.576 px

✅ Entiendes el flujo conversacional
   y los datos reales de aceleración

✅ Conoces los 4 mecanismos de refinamiento
   (conversación, inline, edición directa, sliders)

✅ Conoces los 4 modos de input
   (texto, imágenes, documentos, web capture)
```

> Tienes la mitad del modelo conceptual de Claude Design.

---

## Slide 30 — Lo que falta para completar el modelo

```
1. TIPOS DE OUTPUT
   Qué genera Claude Design exactamente.
   No es solo prototipos.

2. ONBOARDING DEL DESIGN SYSTEM
   La pieza que convierte el producto en
   herramienta de equipo coherente.
   Y el rol específico de DESIGN.md.

3. EXPORTACIÓN Y HANDOFF
   Las opciones (URL, PDF, PPTX, HTML, Canva, folder,
   handoff bundle a Claude Code).

4. COLABORACIÓN
   Lo que sí ofrece y lo que NO ofrece
   (no hay real-time estilo Figma).

5. CASOS DONDE BRILLA / DONDE NO COMPENSA

6. LIMITACIONES, ANTI-PATRONES, ERRORES FRECUENTES
```

---

## Slide 31 — La pregunta antes de seguir

```
¿Qué proyecto visual recurrente de tu equipo
sería el primer candidato para Claude Design?
```

**Pistas (apuestas seguras):**

```
EL DECK QUE PRESENTAS A CLIENTE CADA DOS SEMANAS
└── Deja de hacerlo a mano.
    Dale el outline a Claude Design con tu marca aplicada.

EL PROTOTIPO NAVEGABLE PARA VALIDAR FEATURES
└── Deja de pintar mockups estáticos
    cuando puedes tener algo navegable
    en una conversación.

EL ONE-PAGER CORPORATIVO
└── Que sale del mismo template aburrido cada vez.
```

> Si tienes uno de estos en mente,
> el lunes siguiente del curso es el día perfecto
> para probarlo en serio.

---

## Slide 32 — Lo que viene en 4.2b

```
SUBMÓDULO 4.2b — OUTPUTS, DESIGN SYSTEM, CASOS, LIMITACIONES
─────────────────────────────────────────────────────────────

LOS 6 TIPOS DE OUTPUT
├── Prototipos interactivos
├── Wireframes y mockups de producto
├── Pitch decks y presentaciones
├── One-pagers y documentos visuales
├── Marketing collateral
└── Frontier design

ONBOARDING DEL DESIGN SYSTEM
├── Cómo funciona el onboarding (qué lee Claude Design)
├── El rol específico de DESIGN.md
└── Múltiples design systems en paralelo

EXPORTACIÓN Y HANDOFF (vista rápida)
├── URL org-scoped, PDF, PPTX
├── HTML standalone, Canva, save as folder
└── Handoff bundle a Claude Code (cubierto a fondo en 5.1)

COLABORACIÓN
├── Lo que SÍ ofrece (sharing, group conversations,
│   inline comments, version history, permissions)
└── Lo que NO ofrece (no real-time estilo Figma)

CUÁNDO BRILLA / CUÁNDO NO COMPENSA

LIMITACIONES REALES + ANTI-PATRONES + ERRORES FRECUENTES

BRIDGE A 4.3 (DESIGN.md como tema central)
```

**Nos vemos en 4.2b.**
