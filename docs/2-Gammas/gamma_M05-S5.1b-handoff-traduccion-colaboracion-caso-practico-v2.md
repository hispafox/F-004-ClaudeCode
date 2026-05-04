> **Versión:** v2 | **Módulo:** 5 | **Sub:** 5.1b | **Slides:** 32 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M05-S5.1b-handoff-traduccion-colaboracion-caso-practico-v2.md`

# Submódulo 5.1b — Traducción, colaboración y caso práctico del handoff

## Slide 1 — Portada
**Módulo 5 · Submódulo 5.1 · Parte B**
Traducción, colaboración y caso práctico
Qué traduce bien y qué no, permisos, group conversations, handoff completo, anti-patrones

---

## Slide 2 — Dónde estamos

En 5.1a vimos por qué el handoff ha sido un problema histórico, el panorama de formatos de exportación, cómo funciona técnicamente el handoff a Claude Code, qué contiene el bundle (7 piezas), por qué es distinto del handoff tradicional, el loop cerrado, y las 5 prácticas para preparar un handoff que rinda.

Ahora completamos el modelo:

```
1. QUÉ TRADUCE BIEN al código y QUÉ NO
2. PERMISOS Y COLABORACIÓN
   (los 3 niveles + group conversations + lo que NO hay)
3. CASO PRÁCTICO GUIADO
   (handoff completo, 5 pasos)
4. ANTI-PATRONES + errores frecuentes
5. CIERRE Y BRIDGE A 5.2
```

---

## Slide 3 — Qué traduce bien al código

```
Sección honesta, igual que la que metimos
para el Figma MCP en 4.1.
```

**Lo que traduce bien al código.** Seis cosas:

```
LAYOUT Y ESPACIADO
└── Si el diseño respeta la grid
    y los tokens de spacing
    └── el código generado es PRECISO.

TOKENS (colores, tipografía, radius)
└── Si vienen de un DESIGN.md
    o de un design system bien configurado
    └── el código los REFERENCIA correctamente.

ESTRUCTURA DE COMPONENTES
└── Jerarquía y composición se transfieren bien.
    Un Card con Header + Body + Footer en el diseño
    └── llega al código con la misma estructura.
```

---

## Slide 4 — Más cosas que traducen bien

```
ESTADOS BÁSICOS
└── (loading, error, empty)
    si los marcaste antes en el diseño.

REUTILIZACIÓN DE COMPONENTES EXISTENTES
└── Del codebase, si está linkado.

RESPONSIVE BÁSICO
└── (breakpoints estándar,
     layouts que cambian de columnas a filas)
```

> Estas 6 cosas son **el grueso del trabajo**
> que tradicionalmente se hacía a mano.

---

## Slide 5 — Lo que NO traduce o requiere ajuste manual

```
LÓGICA DE NEGOCIO
└── El handler "onCancelClick" es un emit vacío.
    TÚ implementas la lógica.

LLAMADAS A API Y CONEXIÓN A BACKEND
└── Claude Code NO inventa endpoints.
    Espera que los conectes tú.

ANIMACIONES COMPLEJAS
└── Microinteracciones,
    transiciones cinemáticas,
    motion design avanzado.
    Claude Code generará código razonable
    pero NO cinemáticamente acabado.

VALIDACIÓN DE FORMULARIOS SOFISTICADA
└── Reglas de validación complejas,
    error handling específico del dominio.
    TÚ las pones.
```

---

## Slide 6 — Más cosas que requieren ajuste

```
ESTADOS TRANSITORIOS SUTILES
└── Hover effects sutiles,
    focus states elaborados,
    animaciones de loading personalizadas
    └── requieren AJUSTE post-implementación.

INTEGRACIONES CON SISTEMAS EXTERNOS
└── Auth, billing, analytics
    └── todo eso lo conectas TÚ
        al código generado.

EDGE CASES QUE NO MARCASTE
└── Si NO flagueaste el estado
    de "100 ítems en la tabla":
    └── Claude Code va a implementar
        para los 5 que ve en el diseño.
```

---

## Slide 7 — La regla práctica

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   El handoff te da:                                      │
│                                                          │
│   "carcasa visual + estructura + estados básicos"        │
│                                                          │
│   en mucho menos tiempo que partir de cero.              │
│                                                          │
│   La LÓGICA, las INTEGRACIONES y el PULIDO FINO          │
│   siguen siendo TU TRABAJO.                              │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

> Si tu equipo lo entiende así, el handoff rinde.
>
> Si lo entiende como *"aquí tengo el código terminado"*,
> va a frustrarse.

---

## Slide 8 — Permisos y colaboración

Vimos esto a alto nivel en 4.2. Aquí lo recogemos con detalle operativo.

```
Cuando compartes un proyecto vía URL org-scoped,
tienes TRES niveles de permiso.
```

```
1. VIEW
2. COMMENT
3. EDIT
```

> Los vemos.

---

## Slide 9 — Los 3 niveles de permiso

```
VIEW
└── Read-only.
    La persona puede VER el diseño
    pero NO comentarlo ni modificarlo.

COMMENT
└── Puede dejar comentarios sobre elementos del canvas.
    NO puede modificar el diseño
    NI interactuar con Claude.
    
    Útil para STAKEHOLDERS
    que tienen que dar feedback
    pero NO son los que iteran.

EDIT
└── Puede MODIFICAR el diseño
    Y participar en el chat con Claude
    junto con el resto del equipo con edit.
```

---

## Slide 10 — Group conversations: la pieza más interesante

```
Esta es la parte que conviene entender bien
porque es lo que más sorprende.
```

```
Cuando varios miembros del equipo tienen permisos de EDIT
en el mismo proyecto:

└── pueden chatear con Claude SIMULTÁNEAMENTE
    sobre el mismo canvas.
```

---

## Slide 11 — Cómo funcionan las group conversations

```
ESTO SIGNIFICA QUE:

├── Dos PMs y un diseñador
│   pueden estar mandando inputs a Claude
│   AL MISMO TIEMPO.
│
├── Claude INTEGRA todos los inputs
│   y razona sobre lo que está construyendo
│   a partir de TODOS ELLOS.
│
└── La conversación es VISIBLE para los tres.
```

> Es una forma DISTINTA de colaborar.
>
> NO es *"yo trabajo y luego tú revisas"*,
> sino *"trabajamos juntos con Claude en el mismo lienzo"*.

```
Útil en sesiones de exploración
donde varios cerebros enriquecen el resultado.
```

---

## Slide 12 — Lo que NO hay (todavía)

```
Conviene calibrar expectativas
si vienes de Figma.
```

**Cuatro cosas que NO existen aún:**

```
NO HAY LIVE CURSORS
└── NO ves dónde está mirando o trabajando
    otra persona del equipo en tiempo real.

NO HAY EDICIÓN SIMULTÁNEA
└── Con dos personas moviendo cosas a la vez.
    La colaboración es vía CHAT con Claude,
    NO vía manipulación directa del canvas.

NO HAY ACTIVITY FEED EN TIEMPO REAL
└── El sistema de notificaciones de cambios
    en el proyecto está en pañales.

NO HAY AUDIT LOGS NI USAGE TRACKING todavía.
└── Esto es relevante para enterprise:
    Claude Design es research preview
    y aún NO tiene las características de gobernanza
    que un Figma Enterprise sí tiene.
```

---

## Slide 13 — Estas cosas vendrán

```
Anthropic ha sido bastante claro
en que estas cosas vendrán.
```

```
Pero a día de hoy:

si tu cultura de equipo depende fuertemente
de la colaboración multi-cursor en tiempo real
└── este es un LÍMITE ESTRUCTURAL en este momento.
```

---

## Slide 14 — Inline comments: una nota práctica

Pequeña nota práctica que importa:

```
Los INLINE COMMENTS en Claude Design
funcionan DISTINTO que en Figma.
```

**Aquí, cuando dejas un comentario sobre un elemento:**

```
Es para que CLAUDE LO PROCESE
└── NO necesariamente para que otro humano
    lo vea y lo responda.

Claude lee el comentario,
infiere qué cambio quieres,
y modifica el diseño.
```

```
Si quieres dejar un comentario
para que otro HUMANO lo discuta:
└── eso lo haces en el CHAT COMPARTIDO
    (en group conversation)
    o externamente.
```

> El sistema de comments-as-discussion-thread
> NO está al nivel de Figma todavía.

---

## Slide 15 — Caso práctico guiado: el escenario

Vamos a ver un flujo completo end-to-end. Lo hacemos en clase.

```
Un PM ha creado en Claude Design un prototipo de una
PÁGINA DE NOTIFICACIONES PARA UNA APP DE GESTIÓN DE PEDIDOS
— la siguiente feature que el equipo va a implementar.
```

**Tiene:**

```
├── Lista de notificaciones
│   con título, tiempo, tipo, estado leído/no-leído
├── Filtros por tipo
│   (alertas críticas, info, marketing)
├── Acciones bulk
│   (marcar todo como leído, eliminar seleccionadas)
├── Estados:
│   ├── empty
│   ├── loading
│   ├── error
│   └── con muchas notificaciones (paginación)
└── Layout responsive
    (sidebar con filtros en desktop,
     drawer en mobile)
```

---

## Slide 16 — Paso 1: review interna antes del handoff

```
El PM ha terminado de iterar con Claude.
Antes de hacer el handoff,
comparte el proyecto vía URL
con permiso COMMENT
al diseñador y al tech lead.
```

**Lo que pasa:**

```
EL DISEÑADOR
└── deja inline comments en tres elementos:
    ├── jerarquía del header
    ├── sutil ajuste del color del badge crítico
    └── ajuste del spacing en la lista

EL TECH LEAD
└── deja un comment en el chat:
    "el endpoint de notificaciones devuelve paginated,
     NO all-at-once.
     Confirma que el diseño asume paginación,
     NO scroll infinito"
```

```
El PM toma los inline comments del diseñador
y los aplica con Claude Design
(cada comment se vuelve una iteración corta).

Confirma con el tech lead que SÍ:
└── el diseño asume paginación
    con un botón "Cargar más" al final.
```

---

## Slide 17 — Paso 2: auditoría antes del export

Antes del handoff, le pide a Claude Design que se audite:

```
"Audita este diseño antes de que lo exporte:

├── Contraste WCAG AA en todos los pares texto/fondo
├── Touch targets mínimos en mobile (44x44px)
├── Jerarquía visual: ¿se distingue claramente
│   lo crítico de lo informativo?
├── Consistencia con DESIGN.md del repo
└── Edge cases: muéstrame estado empty, error,
    loading, y con 100+ notificaciones"
```

---

## Slide 18 — Lo que detecta Claude Design

```
Claude Design genera un report.

Detecta tres cosas:
```

```
1. EL BADGE "ALERTA CRÍTICA"
   tiene contraste 3.8:1
   (NO pasa WCAG AA, requiere 4.5:1).
   Sugiere ajustar el color.

2. EL TOUCH TARGET DEL BOTÓN "MARCAR COMO LEÍDO"
   en mobile es 36x36
   — debajo del mínimo.
   Sugiere ampliar.

3. NO ESTABA DISEÑADO EL ESTADO "LOADING INICIAL".
   Lo añade.
```

```
El PM acepta los fixes propuestos.
```

---

## Slide 19 — Paso 3: el export

```
Click EXPORT → "Hand off to Claude Code".
```

```
Claude Design empaqueta el bundle.
Te aparece un mensaje:
```

```
✓ Bundle ready. Copy this command 
  to import into Claude Code:

claude design import 
  https://api.anthropic.com/design/bundles/[bundle-id]
```

```
El PM copia el comando.
```

---

## Slide 20 — Paso 4: implementación con Claude Code

El PM (o el dev al que le ha pasado el comando) abre Claude Code en el repo del proyecto y pega el comando:

```
> claude design import 
  https://api.anthropic.com/design/bundles/abc123

Loading design bundle...
✓ Bundle loaded
✓ Design tokens reconciled with project DESIGN.md
✓ Component structure analyzed
✓ Codebase context detected (Angular 19 + standalone + Signals)

Ready to implement. The bundle includes:
- 1 page: NotificationsPage
- 5 components: NotificationItem, NotificationFilter, 
  BulkActionsBar, EmptyState, ErrorState
- Design system: aligned with project DESIGN.md
- States: empty, loading, error, paginated

How would you like to proceed?
- Full implementation (all components and page)
- Review plan first
- Implement one component at a time
```

```
El dev pide "review plan first".
Claude Code presenta un plan paso a paso.
El dev lo aprueba.
Claude Code procede.
```

---

## Slide 21 — Lo que se genera, a los pocos minutos

```
✓ Página NotificationsPage creada
  como standalone component con Signals.

✓ Cinco subcomponentes en su carpeta correspondiente,
  cada uno con su lógica de estados básicos.

✓ Servicios mock para los datos
  (Claude Code NO inventa endpoints reales:
   crea interfaces y mocks).

✓ Tests básicos para cada componente.

✓ El layout respeta los tokens del DESIGN.md
  del proyecto.

✓ El diseño responsive funciona
  en desktop y mobile.
```

---

## Slide 22 — Lo que el dev tiene que añadir

```
├── Conectar los servicios mock al endpoint real
│   de notificaciones.
│
├── Implementar la lógica de las acciones bulk
│   (marcar como leído, borrar
│    — los mocks están, la integración no).
│
├── Ajustar transiciones sutiles
│   que el dev ve que se sienten bruscas.
│
└── Tests más exhaustivos para casos extremos.
```

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   Lo que tradicionalmente le habría llevado 2-3 DÍAS     │
│   (interpretar mockups, montar la estructura,            │
│    decidir nombres de componentes,                       │
│    generar boilerplate, hacer el responsive)             │
│                                                          │
│   lo tiene en 30-40 MINUTOS,                             │
│   listo para meterse en lo que requiere su criterio.     │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

---

## Slide 23 — Paso 5: feedback al diseño (loop cerrado)

```
Mientras implementa, el dev nota
que la animación de "marcar como leído"
se siente brusca en mobile.
```

```
Vuelve a Claude Design
(sigue accediendo al mismo proyecto),
deja un comment:

"la transición al marcar como leído
 necesita un fade-out de 200ms más sutil,
 especialmente en mobile"
```

```
Claude Design ajusta.
Genera nuevo bundle (incremental, solo los cambios).
El dev hace import del delta:

> claude design import 
  https://api.anthropic.com/design/bundles/abc123 
  --update

Claude Code aplica solo los cambios afectados.
La transición se actualiza.
El loop sigue.
```

---

## Slide 24 — Anti-patrones del handoff (1/2)

```
HACER HANDOFF ANTES DE ITERAR EL DISEÑO
└── El primer output de Claude Design suele ser "rough".
    Si haces handoff antes de pulir
    └── el código generado va a heredar lo rough.
        Itera primero hasta que el diseño esté donde lo querrías.

NO DOCUMENTAR DECISIONES EN CHAT
└── Pasas a Claude Design instrucciones cortas todo el rato,
    SIN razones.
    El bundle pierde el porqué de las decisiones,
    y el dev tiene que inferir igual que en el handoff tradicional.
    Defeating el propósito.

OLVIDAR MARCAR EDGE CASES
└── Diseñas solo el feliz path.
    El dev recibe un bundle con la página feliz
    y vacíos en los estados secundarios.
    Los estados loading/error/empty los acaba implementando
    el dev a ojo, lo que iba a ahorrarse.

PRETENDER QUE EL CÓDIGO ES PRODUCTION-READY
└── El output funciona y respeta el diseño,
    pero NO es production-ready.
    Falta auditarlo: seguridad, accesibilidad real
    (no solo contraste WCAG), performance,
    integración con tests existentes.
    Es un punto de partida muy fuerte, NO un destino.
```

---

## Slide 25 — Anti-patrones del handoff (2/2)

```
HACER HANDOFF A UN REPO SIN DESIGN.MD
└── Claude Design genera el bundle con tokens,
    pero si el repo destino NO tiene un DESIGN.md
    └── Claude Code NO tiene cómo confirmar
        que los tokens del bundle son los correctos del proyecto.
    Resultado: drift entre lo que Claude Design pensó
    que era tu marca y lo que tu marca es realmente.

MÚLTIPLES HANDOFFS SIN SINCRONIZAR
└── Si haces handoff,
    el dev implementa,
    alguien sigue iterando en Claude Design sin saber
    que ya hay implementación,
    y luego se hace otro handoff completo
    en lugar de incremental
    └── el resultado es que se reemplazan trozos de código
        que el dev ya había ajustado a mano.
    
    Coordina el flujo:
    └── cuando empieza la implementación,
        los cambios visuales pasan a ser
        REFINAMIENTOS PUNTUALES,
        no rediseños desde cero.

HACER HANDOFF CUANDO EL CASO ERA PARA OTRA HERRAMIENTA
└── Para una landing simple a presentar mañana,
    exportas a HTML y la subes
    — NO hagas handoff a Claude Code
    para algo que NO va a vivir como código mantenido.
    El handoff es para FEATURES REALES
    que van a implementarse y mantenerse.
```

---

## Slide 26 — Errores frecuentes con tus primeros handoffs (1/2)

```
❌ NO TENER EL CODEBASE LINKADO AL PROYECTO CLAUDE DESIGN
   Sin codebase linkado, Claude Design genera componentes
   genéricos que NO usan los tuyos.
   Linkarlo es UN CLICK y cambia el resultado completamente.

❌ EL COMANDO DEL HANDOFF FALLA EN CLAUDE CODE
   PORQUE LA URL DEL BUNDLE HA EXPIRADO
   Los bundles tienen TTL.
   Si haces import horas después del export,
   puede que el bundle ya NO esté disponible.
   Usa el comando inmediatamente
   o regenera el bundle si necesitas.

❌ ASUMIR QUE LA VERSIÓN DEL CÓDIGO QUE SE GENERA ES LA FINAL
   Es la PRIMERA versión.
   Va a necesitar ajustes
   — sobre todo en lógica, integraciones y pulido sutil.

❌ NO VERIFICAR QUE LOS TOKENS DEL BUNDLE
   COINCIDEN CON LOS DEL DESIGN.MD DEL PROYECTO
   A veces hay drift.
   Mejor un check rápido al recibir el bundle.
```

---

## Slide 27 — Errores frecuentes con tus primeros handoffs (2/2)

```
❌ PASARLE EL BUNDLE A CLAUDE CODE SIN CONTEXTO ADICIONAL
   El bundle trae el diseño y las decisiones,
   pero el dev puede AÑADIR CONTEXTO EXTRA al hacer la importación:
   
   "este código tiene que integrarse con el servicio
    NotificationService existente en src/app/services/"
   
   Más contexto = mejor implementación.

❌ HACER HANDOFF DESDE UN PROYECTO CLAUDE DESIGN
   QUE NO TENÍA EL DESIGN SYSTEM BIEN CONFIGURADO
   Resultado: bundle con tokens medio-correctos,
   código generado medio-coherente.
   Configura el design system PRIMERO,
   después diseña, después haz handoff.

❌ OLVIDARSE DE QUE EL CÓDIGO GENERADO TIENE QUE PASAR
   EL FLUJO NORMAL DEL EQUIPO
   Code review, tests, CI, despliegue.
   El handoff te ahorra el primer pase
   pero NO salta los procesos de calidad.
   Sigue siendo código que va a producción.
```

---

## Slide 28 — Lo que tienes ahora con 5.1 entero

```
✅ Modelo conceptual del handoff:
   un BUNDLE ESTRUCTURADO que viaja entre Claude Design
   y Claude Code en el mismo ecosistema,
   sin pérdida de intent.

✅ Component structure, design tokens efectivos,
   layout hierarchy, chat history con decisiones,
   README de instrucciones, y contexto del codebase.

✅ Las 5 prácticas para preparar el handoff
   (documenta decisiones, nombres de componentes,
    flag edge cases, DESIGN.md, autoauditoría).

✅ Qué traduce bien y qué no.

✅ Permisos, group conversations, lo que NO hay todavía.

✅ Caso práctico end-to-end (review → audit → export →
   import → feedback al diseño).

✅ Anti-patrones y errores frecuentes.
```

---

## Slide 29 — La pieza importante que llevarse

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   Lo bueno que sea el resultado depende de               │
│   CÓMO PREPARES EL DISEÑO.                               │
│                                                          │
│   ├── Documentar decisiones en el chat                   │
│   ├── Referirse a componentes por su nombre real         │
│   ├── Marcar edge cases antes del handoff                │
│   ├── Mantener un DESIGN.md vivo                         │
│   └── Auditar antes de exportar                          │
│                                                          │
│   Estas cinco prácticas son la diferencia entre          │
│   un handoff que ahorra HORAS                            │
│   y uno que ahorra solo MINUTOS.                         │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

---

## Slide 30 — La pregunta antes de pasar a 5.2

```
¿Qué feature de tu backlog actual
sería el mejor candidato para probar
el handoff Claude Design → Claude Code
la primera semana?
```

**Las apuestas seguras** — tres atributos que conviene buscar:

```
1. UNA PÁGINA NUEVA con varios componentes
   NO algo trivial, pero tampoco
   la página más compleja del producto.

2. UNA FEATURE donde el diseño YA SERÍA
   trabajo de mockup en Figma de todas formas
   Así NO te ahorras nada
   generando trabajo extra.

3. UNA FEATURE QUE EL EQUIPO ENTIENDE BIEN
   Para que la falta de contexto
   NO te haga inventar requisitos.
```

```
Si tienes una en mente:
└── el lunes siguiente del curso es el día perfecto.

Y si no, mira tu backlog buscando una feature pendiente
con esos tres atributos.
└── Ahí está tu candidata.
```

---

## Slide 31 — Lo que viene en 5.2

```
SUBMÓDULO 5.2 — CASOS AVANZADOS Y FLUJO INTEGRADO
─────────────────────────────────────────────────────

Cierre conceptual del bloque de diseño con tres cosas:

PRIMERO: CASOS AVANZADOS QUE NO HEMOS CUBIERTO
├── Prototipos interactivos navegables
├── Pitch decks completos on-brand
├── One-pagers
├── Design explorations rápidas
└── Frontier design (mención breve)

SEGUNDO: LA DECISIÓN FINAL
└── Después de ver Figma MCP, Claude Design y DESIGN.md
    └── ¿cuándo uso cada uno?
        Tabla decisional concreta basada en criterios reales,
        NO en preferencias.

TERCERO: EL FLUJO COMBINADO
└── La realidad de un equipo serio
    NO es "uso solo una herramienta"
    └── es COMBINARLAS en un flujo coherente.
        
        Cómo encajan las tres en un equipo .NET + Angular
        y dejar al alumno con una imagen mental clara
        de cuándo cada pieza entra.
```

---

## Slide 32 — Y después de 5.2

```
PIVOTE TOTAL.
```

```
En 5.3 cambiamos de tema completamente:

TESTS EN .NET CON CLAUDE CODE.
60 minutos.
```

> La parte más concreta y la que casi todos los equipos
> que prueban acaban quedándose con la mecánica.

```
Vamos a cubrir:

├── Generación de tests unitarios
│   con xUnit + NSubstitute + FluentAssertions
├── Tests de integración con WebApplicationFactory
├── Detección de code smells
├── Generación de documentación XML y OpenAPI/Swagger
└── Estrategia de cobertura
    — cómo pedirle a Claude Code que apunte a cobertura
    sin generar tests inútiles
```

**Nos vemos en 5.2.**
