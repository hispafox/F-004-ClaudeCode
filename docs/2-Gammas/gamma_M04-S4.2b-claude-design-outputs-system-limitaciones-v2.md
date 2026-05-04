> **Versión:** v2 | **Módulo:** 4 | **Sub:** 4.2b | **Slides:** 30 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M04-S4.2b-claude-design-outputs-system-limitaciones-v2.md`

# Submódulo 4.2b — Claude Design: outputs, design system, casos y limitaciones

## Slide 1 — Portada
**Módulo 4 · Submódulo 4.2 · Parte B**
Outputs, design system, casos y limitaciones
Tipos de output, onboarding con DESIGN.md, handoff, colaboración, anti-patrones

---

## Slide 2 — Dónde estamos

En 4.2a vimos qué es Claude Design, su flujo conversacional, los datos reales de aceleración (Brilliant 20→2, Datadog), los 4 mecanismos de refinamiento (conversación, inline, edición directa, sliders), y los 4 modos de input (texto, imágenes, documentos, web capture).

Ahora completamos el modelo:

```
1. Los 6 tipos de OUTPUT
2. Onboarding del DESIGN SYSTEM (con DESIGN.md priorizado)
3. EXPORTACIÓN y handoff (vista rápida)
4. COLABORACIÓN (lo que sí, lo que NO)
5. CUÁNDO BRILLA / cuándo NO compensa
6. LIMITACIONES, anti-patrones, errores frecuentes
```

---

## Slide 3 — Tipos de output que Claude Design genera

```
NO es solo prototipos.

La gama de outputs es bastante amplia.
```

**Los seis tipos:**

```
1. Prototipos interactivos
2. Wireframes y mockups de producto
3. Pitch decks y presentaciones
4. One-pagers y documentos visuales
5. Marketing collateral
6. Frontier design
```

> Los vemos uno a uno.

---

## Slide 4 — Output 1: prototipos interactivos

```
Mockups NAVEGABLES.

├── Con clicks que hacen cosas
├── Transiciones
├── Estados
└── Formularios funcionales
```

```
Útil para presentar una idea al equipo
o probarla con usuarios
sin pasar por código de producción.
```

---

## Slide 5 — Output 2: wireframes y mockups de producto

```
Lo que un product manager dibuja
para comunicar una feature.
```

> Con Claude Design,
> **el mockup ya es el primer paso de la implementación**.

```
NO hay que rehacer nada cuando llega a desarrollo.

Basta con hacer HANDOFF.
```

---

## Slide 6 — Output 3: pitch decks y presentaciones

```
Genera decks completos ON-BRAND
desde un outline.
```

**Caso de uso obvio:**

```
Tienes que presentar trabajo a cliente
cada dos por tres
└── y crear presentaciones nuevas
    era trabajo manual de horas.
```

> Aquí lo describes y lo ajustas con sliders.

---

## Slide 7 — Output 4: one-pagers y documentos visuales

```
├── Resúmenes ejecutivos
├── Propuestas
├── Fichas de producto
└── Documentos para clientes
```

> La parte menos sexy pero la que más tiempo ahorra
> en el día a día corporativo.

---

## Slide 8 — Output 5: marketing collateral

```
├── Landing pages
├── Social media assets
└── Campaign visuals
```

```
El equipo de marketing puede generar el material RÁPIDO
└── y luego loop in al diseñador para pulir
    └── en vez de empezar siempre de cero.
```

---

## Slide 9 — Output 6: frontier design

Esto es lo que Anthropic llama **"lo que no se podía antes"**:

```
├── Prototipos con voz
├── Video
├── Shaders
├── 3D
└── IA integrada en el diseño
```

```
Si tu prototipo necesita demostrar una interacción
que requiere AI por debajo
└── Claude Design puede MATERIALIZARLA,
    no solo dibujarla.
```

---

## Slide 10 — Onboarding del design system y DESIGN.md

Esta es la pieza que convierte a Claude Design de:

```
"juguete chulo para prototipos sueltos"
```

a:

```
HERRAMIENTA DE EQUIPO COHERENTE.
```

---

## Slide 11 — Cómo funciona el onboarding

Cuando arrancas Claude Design por primera vez en un proyecto, te ofrece **construir un design system automáticamente** leyendo:

```
├── Tu codebase 
│   (a través del Claude Code/repo conectado)
├── Ficheros de Figma que le pases
├── Carpetas de fonts
├── Logo assets
├── Repositorios de GitHub
└── Y, MUY IMPORTANTE
    ├── ficheros markdown del repo
    └── incluyendo el DESIGN.md si lo tienes
```

---

## Slide 12 — Qué infiere Claude Design del onboarding

A partir de esa información, Claude infiere:

```
├── Tu paleta de colores
├── Tu jerarquía tipográfica
├── Tus componentes recurrentes 
│   (botones, cards, formularios)
└── Tu estilo visual general 
    (sobrio vs lúdico, denso vs aireado)
```

```
Y desde ese momento:

cada proyecto que crees en Claude Design
respeta automáticamente esa identidad.
```

> NO tienes que repetir tus colores en cada prompt.
> NO tienes que re-explicar tu estilo.
>
> El sistema lo aplica solo.

---

## Slide 13 — El rol específico de DESIGN.md

Recordatorio de 4.1: DESIGN.md es un fichero markdown con dos partes — frontmatter YAML (tokens) + prose (rationale).

```
Cuando Claude Design hace el onboarding 
y encuentra un DESIGN.md en el repo:

LO PRIORIZA SOBRE LA INFERENCIA AUTOMÁTICA.
```

**Por dos razones:**

```
1. LA INFERENCIA AUTOMÁTICA PUEDE EQUIVOCARSE
   Si tu codebase tiene varios estilos mezclados
   (legacy + nuevo, marca antigua + rebranding en marcha)
   └── Claude puede inferir mal cuál es el "correcto".
       Un DESIGN.md explícito le da la respuesta sin ambigüedad.

2. EL RATIONALE GUÍA LAS DECISIONES NUEVAS
   La parte de prose 
   ("la marca evoca minimalismo arquitectónico 
    con gravitas periodística")
   es lo que le da a Claude el contexto para tomar decisiones
   cuando aparezca un componente nuevo
   que el design system no cubre.
```

---

## Slide 14 — La práctica recomendada

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   Mantén un DESIGN.md en el repo                         │
│   y úsalo como FUENTE DE VERDAD.                         │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

```
Claude Design lo va a leer.
Claude Code lo va a leer.

Otras herramientas del ecosistema 
(Cursor, Codex, Gemini CLI) que estén en el equipo
también lo van a leer.
```

> Es el único formato que funciona transversalmente.

---

## Slide 15 — Múltiples design systems en paralelo

Si trabajas con varias marcas:

```
├── Agencia
├── Empresa con varias submarcas
└── Producto con personalidades visuales distintas
    para B2C y B2B
```

```
Claude Design permite mantener
VARIOS design systems EN PARALELO.

Cuando arrancas un proyecto:
└── eliges qué sistema aplicar.
```

> Pequeño detalle pero rentable
> para equipos que trabajan multi-marca.

---

## Slide 16 — Exportación y handoff: vista rápida

Esto lo cubrimos en profundidad en 5.1, pero conviene saber qué opciones hay desde ya.

```
URL ORG-SCOPED
└── link compartible dentro de tu organización.
    Ideal para review interna.

PDF
└── para clientes que esperan PDF.

PPTX
└── el formato corporativo de presentaciones.

HTML STANDALONE
└── para demos rápidas o despliegue.
```

---

## Slide 17 — Exportación: Canva, folder, handoff bundle

```
CANVA
└── los diseños van COMPLETAMENTE EDITABLES
    dentro de Canva, NO como imagen.
    
    Anthropic firmó partnership con Canva
    el 10 de abril de 2026,
    tres semanas antes del lanzamiento de Claude Design.
    
    Si tu equipo de marketing trabaja en Canva
    └── esto es valioso.

SAVE AS FOLDER
└── descargar la estructura completa.

HANDOFF BUNDLE A CLAUDE CODE
└── el más interesante para devs.
    Claude empaqueta el diseño con la INTENCIÓN
    (qué hace cada componente, 
     qué pasa al hacer click, 
     qué responsive se espera)
    y se lo pasa a Claude Code 
    con UNA SOLA INSTRUCCIÓN.
```

> El **"loop cerrado"** de Anthropic:
>
> Claude Design (prototipo) → Claude Code (implementación)
>
> sin perder en la traducción.
>
> Esto se cubre en detalle en **5.1**.

---

## Slide 18 — Colaboración: lo que ofrece y lo que no

Una nota importante porque conviene tener expectativas calibradas si vienes de Figma.

```
LO QUE SÍ OFRECE
1. Sharing org-scoped (privado / view / edit)
2. Group conversations 
   (varios miembros chateando con Claude
    simultáneamente sobre el mismo diseño)
3. Inline comments
4. Version history (rollback a versiones anteriores)
5. Permission management (en Enterprise)
```

---

## Slide 19 — Colaboración: lo que NO ofrece

```
LO QUE NO OFRECE
```

```
NO HAY COLABORACIÓN EN TIEMPO REAL ESTILO FIGMA.

├── NO hay live cursors
├── NO hay edición simultánea con dos personas
│   moviendo cosas a la vez
└── NO hay sensación de "estamos todos en el mismo lienzo"
```

```
Esto es ESTRUCTURAL, no temporal.

Claude Design es un producto CONVERSACIONAL CON UN AGENTE,
NO un editor multi-cursor.
```

> Si vuestra cultura de diseño es muy multi-jugador
> en tiempo real, este es un **límite real**.

---

## Slide 20 — Cómo se trabaja en equipo entonces

```
La forma de trabajar en equipo aquí es
SECUENCIAL O POR TURNOS.
```

```
1. Una persona itera
2. Comparte
3. Otra revisa con comentarios
4. Otra itera
```

> NO es peor.
> Es **distinto**.

```
Para equipos que ya estaban acostumbrados
a que cada uno trabajase en su rama
y se hiciera review por PR
└── el modelo encaja bien.

Para equipos hiper-colaborativos en Figma
└── el cambio es notable.
```

---

## Slide 21 — Cuándo brilla Claude Design

Sección honesta. Casos donde rinde mucho:

```
1. EXPLORACIÓN RÁPIDA DE DIRECCIONES
   Generar 5 versiones distintas de una landing
   en 20 minutos para elegir.
   Imposible en Figma a esa velocidad.

2. TRABAJO SOLO / SIN DISEÑADOR
   Founders, PMs, devs que tienen que producir 
   algo visual sin tener el equipo de diseño detrás.
   Pasa de "no puedo hacer esto" a "puedo hacerlo en una hora".

3. PITCH DECKS RECURRENTES
   Si presentas trabajo a cliente cada dos semanas
   └── generar el deck con marca aplicada automáticamente
       es ahorro brutal de horas.

4. PROTOTIPOS PARA VALIDACIÓN
   Cuando necesitas algo navegable para enseñar 
   a usuarios o stakeholders, NO para implementar.
   Una semana de mockups + clickeable se vuelve 
   una conversación.

5. LOOP CON CLAUDE CODE
   Si tu pipeline ya está en Claude Code:
   el handoff es directo.
   Diseño → implementación 
   sin pasar por traducción humana.
```

---

## Slide 22 — Cuándo NO compensa

```
1. TRABAJO DE MARCA CRÍTICO PARA CONSUMER-FACING
   Si la diferencia entre 5% y 6% de conversión 
   depende de la calidad estética
   └── sigues necesitando un diseñador con Figma.
       Claude Design es buen punto de partida,
       NO acabado fino.

2. ANIMACIONES SOFISTICADAS
   Microinteracciones complejas, animaciones cinemáticas,
   motion design serio.
   Claude Design se queda corto.

3. DISEÑO PURAMENTE ARTÍSTICO / EDITORIAL
   Diseño de portada de revista, branding desde cero
   con personalidad propia, ilustración.
   No es la herramienta.

4. CUANDO YA TIENES FIGMA VIVO
   Si tu equipo de diseño está manteniendo activamente 
   un Figma con sistema, componentes y variantes
   └── lo que necesitas es Figma MCP,
       NO Claude Design.

5. SI TU PLAN ES PRO Y VAIS A USARLO INTENSAMENTE
   El coste de tokens (58% del weekly quota en dos sesiones)
   hace inviable el uso intensivo en Pro.
   Para uso real, necesitas Max.
```

---

## Slide 23 — Limitaciones reales (1/2)

Sección honesta de cierre.

```
1. COSTE DE TOKENS ALTO
   Cada generación de Claude Design es rica
   (HTML completo, interacciones, assets)
   y eso pesa.
   En Pro vas justo. En Max va bien.

2. NO HAY COLLAB TIEMPO REAL
   Estructural, no temporal.
   Si vuestra cultura es multi-cursor, no encaja.

3. LA INFERENCIA DEL DESIGN SYSTEM PUEDE SER IMPRECISA
   Si dejas a Claude inferir el sistema 
   desde el codebase sin guía
   └── puede meter colores que no son tuyos
       o tipografías que se parecen pero no son las correctas.
   Solución: DESIGN.md explícito.
```

---

## Slide 24 — Limitaciones reales (2/2)

```
4. RESEARCH PREVIEW, NO PRODUCTO CERRADO
   Features rolling out gradualmente.
   El comportamiento puede cambiar entre semanas.
   La documentación oficial es escasa.

5. EL OUTPUT FUNCIONAL REQUIERE AUDIT ANTES DE PRODUCCIÓN
   El código que produce Claude Design es real y funcional
   pero NO es production-ready as-is.
   Hay que auditarlo en seguridad, accesibilidad,
   escalabilidad, SEO, tests.
   Es un punto de partida, no un destino.

6. ALGUNOS FORMATOS DE EXPORTACIÓN TODAVÍA EVOLUCIONAN
   PPTX y PDF están maduros.
   Canva está partnership reciente y mejorando.
   HTML standalone funciona pero la organización del código 
   generado a veces deja que desear.
```

---

## Slide 25 — Anti-patrones de uso (1/2)

```
PRETENDER QUE SUSTITUYE A UN DISEÑADOR PARA TODO
└── No lo hace.
    Para trabajos críticos de marca
    conviene seguir teniendo el ojo humano.
    Claude Design acelera lo rutinario,
    NO elimina el oficio.

EMPEZAR DE CERO TODO EL RATO SIN DESIGN SYSTEM
└── Si cada proyecto arranca con paleta y tipografía aleatorias
    porque no le has dado contexto de marca
    └── tu output es "AI slop visual"
        — funcional pero indistinguible de cualquier 
        otro AI-generated.
    Configurar el design system al principio 
    (idealmente vía DESIGN.md)
    es lo que separa "demos chulas" de "trabajo de equipo".

ITERAR 30 PROMPTS CUANDO 3 HUBIERAN BASTADO
└── Hay tendencia a entrar en bucle de "casi pero no"
    porque cada iteración cuesta segundos.
    Si llevas 10 prompts en el mismo elemento
    └── es señal de que conviene parar y reformular el problema.
```

---

## Slide 26 — Anti-patrones de uso (2/2)

```
NO USAR LOS CUATRO MECANISMOS DE REFINAMIENTO
└── La gente que viene de chat usa solo conversación.
    La que viene de editores visuales solo intenta clicar.
    
    Combina los cuatro:
    ├── conversación para lo macro
    ├── comentarios inline para lo localizado
    ├── edición directa para texto
    └── sliders para fine-tuning

COMPARTIR TRABAJOS SIN HABER VALIDADO EL CÓDIGO GENERADO
└── Sobre todo si vas a hacer handoff a Claude Code.
    El diseño puede estar visualmente bien
    y el código por debajo puede tener problemas.
    Audita antes de pasar.

NO TENER DESIGN.MD CUANDO YA HAY CODEBASE
└── Dejas a Claude inferir el design system 
    desde cero cada vez.
    Trabajo extra que puedes evitar 
    con un fichero bien hecho.
```

---

## Slide 27 — Errores frecuentes con tus primeros usos (1/2)

```
❌ SUBIR IMÁGENES DE BAJA RESOLUCIÓN
   La visión de Opus 4.7 es alta resolución 
   pero necesita input de calidad.
   Una captura borrosa rinde peor que una nítida.

❌ PROMPTS DEMASIADO VAGOS
   "Hazme una landing" genera algo, pero genérico.
   "Landing para SaaS de gestión de pedidos B2B, 
    target SME, hero con video demo, 3 features con iconos, 
    social proof, paleta corporativa"
   genera algo útil.

❌ NO MIRAR EL QUOTA ANTES DE UNA SESIÓN LARGA
   Si te quedas sin tokens a mitad de iteración
   └── perder hilo de conversación es frustrante.
   Comprueba /usage antes de meterte a fondo.
```

---

## Slide 28 — Errores frecuentes con tus primeros usos (2/2)

```
❌ OLVIDAR QUE EL OUTPUT ES CÓDIGO REAL
   Cuando exportes a Claude Code o veas lo que generó por dentro
   └── entérate de qué hizo.
   Hay decisiones (estructura HTML, organización CSS, 
   librerías usadas) que conviene saber para integrar bien.

❌ CONFUNDIR CLAUDE DESIGN CON CLAUDE.AI CHAT CON ARTIFACTS
   Son productos relacionados pero NO idénticos.
   ├── Claude.ai chat puede generar artifacts visuales
   └── Claude Design es un producto específico con
       design system, sliders, web capture, handoff bundle.
   
   Si solo quieres un mockup suelto:
   └── claude.ai chat puede bastar.
   Si quieres flujo completo:
   └── Claude Design.

❌ NO PROBAR EL HANDOFF A CLAUDE CODE ANTES DE LA PRIMERA ENTREGA
   El handoff es la parte más interesante para devs
   y la que más puede sorprender (en bueno y en malo).
   Mejor probarla con un proyecto pequeño
   antes que apostar el primer entregable a un flujo 
   que no has validado.
```

---

## Slide 29 — Lo que tienes ahora con 4.2 entero

```
✅ Sabes qué es Claude Design 
   (no es generador de imágenes, es código funcional)

✅ Acceso por plan, atención al consumo, motor Opus 4.7

✅ Flujo conversacional con datos reales
   (Brilliant 20→2, Datadog 1 semana → 1 conversación)

✅ Los 4 mecanismos de refinamiento
   (conversación, inline, edición directa, sliders)

✅ Los 4 modos de input
   (texto, imágenes, documentos, web capture)

✅ Los 6 tipos de output

✅ Onboarding del design system con DESIGN.md priorizado

✅ Exportación y handoff (vista rápida)
   con handoff bundle a Claude Code para devs

✅ Colaboración (sharing, version history)
   y lo que NO ofrece (no real-time)

✅ Cuándo brilla, cuándo no compensa,
   limitaciones, anti-patrones, errores frecuentes
```

---

## Slide 30 — Lo que viene en 4.3

```
DESIGN.md como TEMA CENTRAL.
```

Hasta aquí lo hemos mencionado dos veces:

```
├── En 4.1 como complemento al MCP
└── Aquí en 4.2 como input para Claude Design
```

```
Pero merece su propio apartado porque está cambiando 
rápido cómo los equipos materializan sus design systems
para los agentes.
```

```
SUBMÓDULO 4.3 — DESIGN.MD COMO SISTEMA
─────────────────────────────────────────────────────

ANATOMÍA DETALLADA
├── Ejemplo completo
├── Frontmatter YAML
├── Token references
├── Markdown prose
└── Variantes de componentes

LOS TRES CAMINOS PARA GENERARLO
├── Vía Stitch directamente
├── Desde Figma con MCP
└── Escritura manual (con dos atajos)

LA CLI OFICIAL @google/design.md
├── lint (validador WCAG)
├── diff (comparación entre versiones)
├── export (a Tailwind, DTCG, CSS)
└── spec (output de la especificación)

EL CASO "TRABAJO SIN FIGMA"
└── Donde DESIGN.md es la única fuente del design system

MANTENIMIENTO ITERATIVO Y VERSIONADO

CIERRE DEL MÓDULO 4
├── Los tres pilares juntos 
│   (Figma MCP + Claude Design + DESIGN.md)
└── Los tres flujos típicos
```

**Nos vemos en 4.3.**
