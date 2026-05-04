> **Versión:** v2 | **Módulo:** 5 | **Sub:** 5.2 | **Slides:** 50 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M05-S5.2-casos-avanzados-flujo-integrado-v2.md`

# Submódulo 5.2 — Casos avanzados y flujo integrado

## Slide 1 — Portada
**Módulo 5 · Submódulo 5.2**
Casos avanzados y flujo integrado
5 categorías de output, la decisión final entre las tres herramientas, los 3 flujos típicos

---

## Slide 2 — Cierre conceptual del bloque de diseño

```
En 5.1 cubrimos el handoff.

La PIEZA ESTRELLA, pero solo UNA
de las muchas cosas que Claude Design sabe hacer.
```

**Aquí cerramos el bloque de diseño con tres cosas:**

```
PRIMERO
└── Los CASOS AVANZADOS que NO hemos cubierto:
    ├── prototipos interactivos navegables
    ├── pitch decks completos on-brand
    ├── one-pagers
    └── design explorations rápidas

SEGUNDO
└── La DECISIÓN FINAL.
    Después de ver Figma MCP, Claude Design y DESIGN.md
    └── ¿cuándo uso cada uno?

TERCERO
└── El FLUJO COMBINADO.
    Cómo encajan las tres en un equipo .NET + Angular.
```

> Antes de pivotar a tests .NET en 5.3,
> este apartado deja el cierre del bloque de diseño
> con la imagen completa.

---

## Slide 3 — Más allá de los mockups: la gama de outputs

```
La forma fácil de pensar en Claude Design
— y la trampa en la que cae el 80% de la gente
que lo prueba —
es como "un generador de mockups de UI".
```

```
SÍ lo hace, y lo hace bien.

Pero la gama es bastante más AMPLIA.
```

**Cinco tipos de output que merece la pena conocer:**

```
1. PROTOTIPOS INTERACTIVOS
   — mockups que se navegan, no solo se miran.

2. PITCH DECKS Y PRESENTACIONES
   — decks completos generados desde un outline.

3. ONE-PAGERS Y DOCUMENTOS VISUALES
   — resúmenes ejecutivos, propuestas, fichas.

4. DESIGN EXPLORATIONS
   — varias direcciones visuales del mismo concepto, en minutos.

5. FRONTIER DESIGN
   — prototipos con voz, video, shaders, 3D, IA integrada.
```

> Vamos uno por uno con foco en el **flujo práctico**,
> no en la lista de features.

---

## Slide 4 — Prototipos interactivos: la diferencia

```
La diferencia entre un mockup y un prototipo:
```

```
EL MOCKUP SE MIRA.
EL PROTOTIPO SE NAVEGA.
```

```
MOCKUP
└── Estático.
    Una imagen de cómo se ve la pantalla.

PROTOTIPO
└── Funcional.
    Clicas, cambia el estado,
    navegas a la siguiente pantalla,
    los formularios responden,
    los hovers reaccionan.
```

---

## Slide 5 — Qué genera Claude Design en este modo

Cuando le pides *"hazme un prototipo navegable de [feature]"* — o cuando empiezas con un mockup y le pides después *"hazlo interactivo"* — el output incluye:

```
├── CLICK HANDLERS REALES
│   Los botones funcionan.
│   Los enlaces navegan.
│   Las acciones disparan estados.
│
├── ESTADOS QUE CAMBIAN
│   Cuando el usuario interactúa, el prototipo responde.
│   Loading aparece, errors aparecen,
│   contenido se actualiza.
│
├── NAVEGACIÓN ENTRE PANTALLAS
│   Si tu prototipo tiene varias pantallas,
│   las transiciones funcionan.
│
├── FORMULARIOS FUNCIONALES con validación básica
│   No conectados a backend real,
│   pero validan localmente.
│
└── ANIMACIONES Y TRANSICIONES SIMPLES
    Los cambios de estado tienen su micro-animación.
```

---

## Slide 6 — Lo que NO genera (sin pedirlo explícitamente)

```
├── Conexión a APIs reales
│   — los datos son MOCK.
│
├── Lógica de negocio compleja
│   — solo la simulación que pediste.
│
└── Persistencia
    — refresh y vuelves al estado inicial.
```

---

## Slide 7 — Cuándo merece la pena un prototipo interactivo

```
Tres casos donde rinden mucho:
```

**1. USER TESTING ANTES DE IMPLEMENTAR.**

```
Quieres validar un flujo con usuarios reales
(o stakeholders) antes de meter horas de desarrollo.

Un prototipo navegable les permite "USAR" la feature,
NO solo verla.

El feedback que recoges es cualitativamente
mejor que el de un mockup estático.
```

**2. STAKEHOLDER REVIEW DIFÍCIL.**

```
Estás presentando una feature a alguien
que tiene problemas para visualizar
de un mockup cómo va a funcionar el producto.

Con un prototipo navegable lo entiende inmediatamente.

"Ah, vale, el botón hace eso
 y entonces aparece esta pantalla
 — ahora lo veo"
```

**3. VALIDACIÓN INTERNA DE UX.**

```
Cuando dudas si un flujo funciona,
navegarlo tú mismo te dice cosas
que mirando el mockup NO se ven.

Atajos visuales que parecían claros
se vuelven confusos.

Estados que pensabas obvios
resultan ambiguos.

Mejor descubrirlo en el prototipo
que en producción.
```

---

## Slide 8 — Cuándo NO merece la pena

```
CAMBIOS MÍNIMOS SOBRE FLUJOS YA VALIDADOS
└── Si la feature es simple y el flujo está claro,
    ir a prototipo es OVERENGINEERING.
    Mockup estático basta.

CUANDO VAS DIRECTO A IMPLEMENTAR
└── Si el handoff a Claude Code es inmediato,
    el prototipo intermedio puede saltar
    └── Claude Code va a generar la versión
        interactiva igualmente.
    
    El prototipo solo añade valor
    si va a ser revisado por alguien
    ANTES de tocar código.
```

---

## Slide 9 — Caso típico de prototipo

Un PM diseña un onboarding flow de 5 pantallas. Pide que sea interactivo:

```
"Convierte este mockup de onboarding en un prototipo navegable.

Funcionalidades:
├── Los botones 'Siguiente' navegan a la pantalla siguiente.
├── El formulario de la pantalla 3 valida campo email (formato).
├── Si el usuario clica 'Atrás', vuelve a la pantalla previa
│   con su input preservado.
├── En la última pantalla, el botón 'Empezar'
│   muestra una pantalla de confirmación.
└── Indicador de progreso visual (barra)
    que avanza con cada pantalla."
```

```
Claude Design materializa el prototipo.

El PM lo comparte vía URL org-scoped
con permiso comment al equipo de UX y a usability test.

5 usuarios prueban el flujo, dejan inline comments.

El PM itera.

Cuando el flujo está validado, hace handoff a Claude Code.
```

> Lo que se ahorra:
>
> **dos semanas de "implementarlo, probarlo con usuarios,
> descubrir que el flujo no funciona bien, rehacerlo"**.
>
> Ahora el ciclo es de DÍAS, no de semanas.

---

## Slide 10 — Pitch decks y presentaciones

```
Caso menos sexy pero MUY RENTABLE
para muchos roles.
```

```
Si presentas trabajo a cliente, board o investors
con regularidad
└── generar decks a mano es trabajo manual de horas
    que se puede colapsar a minutos.
```

---

## Slide 11 — Cómo funciona

```
Le das un OUTLINE
(puede ser tan simple como una lista de bullets
 o tan rico como un brief completo)
y Claude Design GENERA EL DECK ENTERO.

Aplicando tu marca automáticamente
si tienes design system configurado.
```

**Ejemplo de prompt:**

```
"Genera un pitch deck de 12 slides para presentar a inversores
con esta estructura:

1. Title slide: empresa, tagline, fecha
2. El problema: B2B SaaS pierden 30% del revenue 
   por descontento de clientes sin saberlo.
   Datos: encuesta a 200 empresas.
3. La solución: nuestro producto detecta señales tempranas
   usando NLP sobre tickets de soporte.
4. Cómo funciona: 3 pasos visuales
   — capturar, analizar, alertar.
5. Demo del producto: 2-3 screenshots clave.
[...]
12. Ask: cuánto levantamos y para qué.

Tono: profesional pero no aburrido.
Marca: la del DESIGN.md del repo."
```

```
OUTPUT
└── deck completo, on-brand,
    con slides estructuradas,
    tipografía coherente,
    datos visualizados (gráficos, tablas),
    pequeñas animaciones donde tienen sentido.
    
    LISTO PARA REVISAR Y AJUSTAR.
```

---

## Slide 12 — Lo que ahorra y lo que no

```
LO QUE AHORRA
└── el 70-80% del trabajo MECÁNICO.
    
    ├── La estructura
    ├── La maquetación
    ├── La consistencia visual
    └── La primera versión textual de cada slide
    
    Eso son las HORAS DE NOCHE
    antes de la presentación
    que históricamente se hacían a mano.
```

```
LO QUE NO AHORRA
└── el CRITERIO EDITORIAL.
    
    Si los datos son flojos,
    si los argumentos no encajan,
    si la narrativa no convence
    └── Claude Design te genera un deck
        visualmente decente
        CON ESOS PROBLEMAS.
```

> Tienes que revisar, ajustar, reescribir trozos.
>
> Es un PUNTO DE PARTIDA muy fuerte,
> NO un sustituto del PENSAMIENTO ESTRATÉGICO.

---

## Slide 13 — Workflow recomendado para pitch decks

```
1. OUTLINE EN UNA CONVERSACIÓN RÁPIDA
   con Claude (chat normal, NO Design).
   Refina los puntos que vas a presentar,
   identifica el ángulo,
   decide la narrativa.

2. PASA EL OUTLINE A CLAUDE DESIGN
   con instrucciones de formato y marca.

3. ITERA 2-3 VUELTAS:
   ├── ajustar tono
   ├── jerarquía
   └── qué slides ampliar y cuáles condensar.

4. EXPORTA A PPTX O CANVA
   según con qué herramienta vas a presentar / refinar.

5. PASA EL ÚLTIMO 20% A MANO:
   ├── detalles que requieren tu criterio
   └── frases clave que tienen que sonar como tú.
```

> Ese **20% final** es donde diferencias tu deck
> de un deck genérico hecho con IA.
>
> NO te lo saltes.

---

## Slide 14 — One-pagers y documentos visuales

```
Aquí entra el caso menos visible
pero más COTIDIANO.
```

**Resúmenes ejecutivos. Fichas de producto. Propuestas comerciales. Briefings. Documentos visuales para clientes.**

```
Todo el material que tradicionalmente
se hace en Word + diseñador
y termina como
├── PDFs feos, o
└── documentos bonitos pero que costaron horas.
```

```
Claude Design los hace BIEN.

Y aquí la rentabilidad es ALTA porque son documentos
que se hacen MUCHOS
(no uno cada tanto como un pitch deck).
```

---

## Slide 15 — Casos típicos de one-pagers

```
FICHA DE PRODUCTO / ONE-PAGER COMERCIAL
└── Un PDF de una página con:
    propuesta de valor, features, pricing, CTA.
    Para enviar en frío
    o como follow-up de reunión.

RESUMEN EJECUTIVO DE UN PROYECTO
└── Para mandar a board o steering committee.
    Estructura clara, datos visuales, CTA.

PROPUESTA COMERCIAL VISUAL
└── La propuesta que tradicionalmente sería
    5 páginas de Word
    se vuelve un documento de 1-2 páginas
    visualmente diseñadas.

BRIEFING INTERNO
└── Para alinear equipos sobre una iniciativa.
    Estructura visual + texto.

REPORTES DE PROYECTO
└── Estado, progreso, métricas, próximos pasos.
    Mensual o trimestral.
```

---

## Slide 16 — El flujo de one-pagers

Similar al de los pitch decks pero **más rápido** porque el documento es más corto:

```
"Genera un one-pager comercial para [producto].

Estructura:
├── Header con logo + tagline
├── 3-4 puntos de valor con iconos
├── Sección de 'Cómo funciona' en 3 pasos
├── Mini-tabla de pricing (3 planes)
├── CTA grande
└── Footer con contacto

Tono: directo, B2B, no demasiado formal.
Marca: aplicar DESIGN.md del repo.
Formato: PDF de una página A4."
```

```
Sale en menos de un minuto.
Lo ajustas (texto, jerarquía, datos),
lo exportas a PDF, lo mandas.
```

> Lo que era trabajo de **3-4 horas**
> con InDesign + diseñador
>
> se vuelve **15 minutos** con tu prompt
> + 5 minutos de ajustes.

---

## Slide 17 — Design explorations: múltiples direcciones rápido

```
Esta es la categoría que los DISEÑADORES EXPERIMENTADOS
descubren con sorpresa.
```

```
Hasta ahora, explorar varias direcciones visuales
de un mismo concepto era COSTOSO
└── cada exploración requería sentarse a maquetar.

Por tiempo, los diseñadores se limitaban a 2-3 direcciones,
las "obvias", antes de elegir una y refinarla.
```

```
Con Claude Design, la barrera BAJA.
```

```
Puedes generar 5, 7, 10 DIRECCIONES
del mismo concepto
en una hora.
```

> La pregunta deja de ser
> *"¿cuál de las dos opciones que tengo es mejor?"*
>
> y se vuelve
> *"¿cuál de estas siete me convence?"*

---

## Slide 18 — Cómo se hace una exploration

Una conversación tipo:

```
"Quiero explorar direcciones visuales para una landing page
de un SaaS B2B de gestión de pedidos.
Genera 5 versiones que sean DIFERENTES entre sí,
NO variaciones menores.
Cada una con un ángulo distinto:

1. MINIMALISMO EXTREMO:
   blanco, mucho whitespace, una tipografía sobria.
2. EDITORIAL:
   como una revista de negocio,
   jerarquía tipográfica fuerte.
3. TECH-FORWARD:
   tonos oscuros, grids visibles, tipografía mono.
4. PLAYFUL:
   colores cálidos, formas orgánicas, ilustraciones.
5. DASHBOARD-STYLE:
   como si fueras dentro del producto, datos visibles.

Para cada una, mantén el mismo CONTENIDO
— los puntos de valor son los mismos.
Lo que cambia es el ÁNGULO VISUAL."
```

```
Claude Design genera las 5.
Puedes navegar entre ellas, capturarlas, compartirlas.
```

---

## Slide 19 — Lo que esto cambia

Para diseñadores que vienen de Figma, esto **rompe una restricción** que estaba tan integrada que casi no se veía:

```
"explorar es caro,
 así que limita las opciones".
```

**Cuando explorar es barato, las decisiones cambian:**

```
├── Llegas a la dirección elegida con MÁS CONFIANZA
│   porque has visto más alternativas.
│
├── Encuentras direcciones inesperadas
│   — la cuarta o la quinta a veces es la buena,
│   y nunca habrías llegado ahí
│   con tiempo de hacer dos.
│
└── Puedes presentar al cliente
    EL RANGO DE POSIBILIDADES,
    NO solo tu opinión final.
```

**El precio: los tokens.**

```
Como dijimos en 4.2, Claude Design es token-heavy.

Generar 5 direcciones sustanciales puede consumir
buena parte de tu weekly quota.

Por eso el caso de uso es genuino para Pro/Max,
NO para Pro de uso casual.
```

---

## Slide 20 — Frontier design: lo que no se podía antes

```
Mención BREVE porque está en el filo
y NO es uso típico todavía.
```

**Frontier design** es lo que Anthropic llama a los prototipos que incluyen capacidades imposibles en herramientas tradicionales:

```
├── VOZ: prototipos donde el usuario habla
│   y el sistema responde.
│
├── VÍDEO: prototipos con vídeo embebido
│   que reacciona al usuario.
│
├── SHADERS Y 3D: efectos visuales avanzados,
│   escenas tridimensionales.
│
└── IA INTEGRADA: prototipos que NO simulan
    respuestas de IA, las generan en tiempo real.
```

---

## Slide 21 — Por qué importa frontier design

```
¿Por qué importa?
```

```
Porque el handoff a Claude Code PRESERVA esto.
```

```
Si tu prototipo tiene una capacidad de IA por debajo:
└── el código generado la implementa.

NO es un mockup que dice "aquí iría una IA":
└── es un prototipo donde la IA FUNCIONA.
```

> Para casos cotidianos de un equipo .NET + Angular,
> esto está más en el plano de "ya veremos"
> que de uso inmediato.
>
> Pero conviene saber que existe
> — abre opciones de prototipado
> que hace dos años eran ciencia ficción.

---

## Slide 22 — La decisión: Figma MCP vs Claude Design vs DESIGN.md

```
Llegamos a la pregunta que el alumno trae desde 4.1.

Después de ver tres herramientas:
└── ¿cuándo uso cada una?
```

```
La respuesta corta:
```

```
NO son herramientas COMPETIDORAS.
Son herramientas COMPLEMENTARIAS.

Cada una resuelve un problema distinto
y la decisión correcta es casi siempre
alguna COMBINACIÓN, no escoger una.
```

> Vamos a la tabla decisional explícita.

---

## Slide 23 — Tabla comparativa (1/2)

| Criterio | Figma MCP | Claude Design | DESIGN.md |
|---|---|---|---|
| **Problema que resuelve** | Conectar Claude Code con un Figma existente | Crear diseño visual desde la conversación | Materializar el design system para los agentes |
| **Punto de partida** | Hay un Figma vivo, mantenido | No hay Figma o se quiere crear de cero | Hay un design system explícito o se quiere consolidar |
| **Output principal** | Componentes de código generados desde frames | Prototipos visuales (UI, decks, docs) | Fichero markdown con tokens + prose |
| **Vive en** | Servidor MCP conectado al Figma | App web (`claude.ai/design`) | Repo del proyecto, en git |
| **Audiencia primaria** | Devs que reciben diseños de un equipo de diseño | Founders, PMs, devs sin diseñador | Toda herramienta que respeta tokens |

---

## Slide 24 — Tabla comparativa (2/2)

| Criterio | Figma MCP | Claude Design | DESIGN.md |
|---|---|---|---|
| **Output traduce a código** | Sí (vía Claude Code) | Sí (vía handoff bundle) | Indirectamente (se usa por otras tools) |
| **Coste de tokens** | Bajo (lecturas puntuales) | Alto (generación rica) | **Cero** (es solo un fichero) |
| **Curva de aprendizaje** | Media (depende del Figma) | Baja (es conversacional) | Media (formato nuevo) |
| **Estado de madurez** | Estable | Research preview | Alpha |
| **Funciona offline** | No (conexión con Figma) | No (web app) | **Sí** |
| **Versionado en git** | No (vive fuera del repo) | No (vive en claude.ai) | **Sí** |

---

## Slide 25 — Cuándo usar Figma MCP

```
USA FIGMA MCP CUANDO:
```

```
├── Tu equipo de diseño mantiene un Figma vivo
│   con variables, componentes y Auto Layout.
│
├── Necesitas extraer información concreta
│   de un frame específico
│   para generar código fiel.
│
├── Hay flujo establecido entre diseño y desarrollo
│   y solo quieres ACELERAR la traducción.
│
└── El Figma es la FUENTE DE VERDAD
    y quieres respetarla.
```

---

## Slide 26 — Cuándo usar Claude Design

```
USA CLAUDE DESIGN CUANDO:
```

```
├── NO tienes Figma o tu equipo de diseño
│   no lo mantiene activamente.
│
├── Necesitas EXPLORAR DIRECCIONES rápido
│   antes de comprometerte con una.
│
├── Estás creando MATERIAL VISUAL NO-UI:
│   pitch decks, one-pagers, marketing collateral.
│
├── Quieres prototipos navegables
│   para validar flujos antes de implementar.
│
└── Eres founder, PM o dev sin diseñador a mano.
```

---

## Slide 27 — DESIGN.md siempre

```
USA DESIGN.MD SIEMPRE.
```

> Esto es importante:
>
> **DESIGN.md NO es alternativa a las otras dos**,
> **es complemento**.

```
Va con cualquier flujo.
```

```
SI USAS FIGMA MCP
└── conviene tener DESIGN.md sincronizado con tu Figma
    para que el código generado
    tenga referencia directa de tokens.

SI USAS CLAUDE DESIGN
└── conviene tener DESIGN.md
    para que la generación respete tu marca.

SI TRABAJAS SOLO
└── DESIGN.md es lo que MATERIALIZA
    tu sistema visual de forma versionada.
```

---

## Slide 28 — La regla práctica

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   DESIGN.MD SIEMPRE.                                     │
│                                                          │
│   Las otras dos según el caso.                           │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

---

## Slide 29 — El flujo combinado completo

```
La realidad de un equipo serio
NO es "uso solo una".

Es COMBINAR las tres en un flujo coherente.
```

> Vamos a ver el flujo completo en una organización seria,
> paso a paso.

---

## Slide 30 — Diagrama mental del ciclo de vida de una feature

```
1. IDEA / BRIEF
   Aparece la necesidad
   — un PM tiene una feature en mente,
   un diseñador tiene una hipótesis,
   un dev quiere probar algo.

2. EXPLORACIÓN VISUAL
   Antes de comprometerse,
   ¿cómo podría verse esto?
   Aquí entra CLAUDE DESIGN
   para generar direcciones, prototipos rápidos.

3. DECISIÓN Y REFINAMIENTO
   El equipo elige la dirección.
   Si hay diseñador y la feature lo justifica:
   └── refinamiento detallado puede ir a FIGMA.
   Si no, sigue en Claude Design.

4. VALIDACIÓN
   Si vale la pena, prototipo navegable
   en CLAUDE DESIGN para user testing
   o stakeholder review.
```

---

## Slide 31 — Continuación del ciclo de vida

```
5. CONSOLIDACIÓN DEL DESIGN SYSTEM
   Cualquier token nuevo o componente nuevo
   se consolida en DESIGN.MD.

6. HANDOFF A DESARROLLO
   ├── Si vino de Figma:
   │   FIGMA MCP extrae lo necesario.
   ├── Si vino de Claude Design:
   │   HANDOFF BUNDLE a Claude Code.
   └── Ambos referencian el DESIGN.MD
       como fuente de verdad.

7. IMPLEMENTACIÓN
   CLAUDE CODE escribe el código
   respetando el DESIGN.md
   y los inputs específicos de la feature.

8. ITERACIÓN
   Si surgen cambios visuales,
   vuelven a la herramienta donde se diseñó
   (Figma o Claude Design).
   DESIGN.md se actualiza si hay cambios al sistema.
```

> Cada herramienta tiene su lugar.
> Las tres respetan el mismo DESIGN.md.
>
> **Esto es lo que diferencia un flujo coherente
> de un caos de herramientas que NO se hablan.**

---

## Slide 32 — Tres flujos típicos según el equipo

Las combinaciones más habituales en empresas reales:

```
FLUJO A
└── Equipo con cultura de diseño establecida.

FLUJO B
└── Equipo pequeño o startup sin diseñador.

FLUJO C
└── Agencia o consultora multi-cliente.
```

> Los vemos uno a uno.

---

## Slide 33 — Flujo A: equipo con cultura de diseño establecida

```
Brief
  ↓
Exploración en Claude Design
  ↓
Refinamiento detallado en Figma
  ↓
DESIGN.md sincronizado periódicamente
  ↓
Figma MCP para extracciones puntuales
  ↓
Claude Code para implementación
```

```
Para este equipo:

├── CLAUDE DESIGN
│   es para exploración y diseño liviano
│   (presentaciones, internal docs).
│
├── FIGMA
│   es para el trabajo de marca
│   y producto serio.
│
└── DESIGN.MD
    es snapshot reproducible
    del estado de Figma.
```

---

## Slide 34 — Flujo B: equipo pequeño o startup sin diseñador

```
Brief
  ↓
Claude Design (todo)
  ↓
DESIGN.md generado y mantenido
  ↓
Handoff bundle a Claude Code
  ↓
Implementación
```

```
Aquí NO hay Figma.

├── CLAUDE DESIGN hace la creación
├── DESIGN.MD materializa el sistema
└── CLAUDE CODE implementa.
```

> Flujo más simple porque hay menos piezas,
> pero requiere DISCIPLINA
> para mantener DESIGN.md vivo.

---

## Slide 35 — Flujo C: agencia o consultora multi-cliente

```
Cada cliente: su DESIGN.md propio
  ↓
Claude Design por cliente
con su sistema cargado
  ↓
Implementación con Claude Code
apuntando al cliente correcto
```

```
La pieza clave aquí es la SEPARACIÓN
de design systems.

├── Cada cliente tiene su DESIGN.md
├── Cada proyecto referencia el correcto
└── NO hay drift entre marcas.
```

---

## Slide 36 — Qué falla cuando NO se combinan bien

Un par de antipatrones que se ven en empresas:

```
ANTI-PATRÓN 1: IGNORAR DESIGN.MD
└── El equipo usa Figma MCP para extraer
    y Claude Design para crear,
    pero NUNCA consolida nada en DESIGN.md.
    
    Resultado:
    ├── cada generación reinventa los tokens
    ├── los componentes drift entre proyectos
    └── la marca pierde coherencia.
```

```
ANTI-PATRÓN 2: METER TODO EN CLAUDE DESIGN
└── El equipo abandona Figma porque
    "Claude Design es más rápido".
    
    Bien para exploración,
    mal para refinamiento de marca crítica.
    
    Termina con material decente
    pero SIN el pulido fino
    que un Figma maduro permite.
```

---

## Slide 37 — Tercer anti-patrón

```
ANTI-PATRÓN 3: USAR FIGMA MCP PARA TODO,
IGNORAR CLAUDE DESIGN
└── El equipo ignora Claude Design
    porque "ya tenemos Figma".
    
    Pierde:
    ├── la velocidad de exploración
    └── la creación rápida de pitch decks / one-pagers.
    
    Termina haciendo a mano
    lo que se podría haber automatizado.
```

```
La salida común:
```

```
ADOPTA LAS TRES CON CABEZA.
USA CADA UNA PARA LO SUYO.
```

---

## Slide 38 — Anti-patrones generales del bloque de diseño

Algunos errores que aparecen con frecuencia cuando se adopta este stack:

```
SALTARSE EL DESIGN.MD
└── El error MÁS COMÚN.
    Empiezas con Figma MCP o Claude Design
    sin consolidar nada en DESIGN.md,
    y al cabo de un mes
    tu sistema visual está disperso entre
    tres herramientas que NO se hablan.
    
    Mejor invertir media hora al inicio
    en un DESIGN.md decente
    que vivir con drift los siguientes meses.

PRETENDER QUE LAS HERRAMIENTAS
ELIMINAN EL OFICIO DEL DISEÑO
└── NO lo hacen.
    Aceleran lo mecánico,
    NO eliminan el criterio.
    
    Una empresa que sustituye a su diseñador
    por Claude Design termina con UI MEDIOCRE
    — funcional pero indistinguible
    de cualquier otra cosa hecha con IA.
    
    La diferencia entre un diseño OK y uno bueno
    sigue siendo HUMANA.
```

---

## Slide 39 — Más anti-patrones generales

```
DEMASIADA EXPLORACIÓN, POCO COMPROMISO
└── Generar 10 direcciones en Claude Design es BARATO.
    Eso te puede llevar a explorar perpetuamente
    SIN ELEGIR.
    
    Pon LÍMITE DE TIEMPO al exploration phase.
    "Tres direcciones máximo, decisión en una hora".

CONFUNDIR PROTOTIPO CON PRODUCTO
└── El prototipo de Claude Design
    es navegable y funcional,
    pero NO es código de producción.
    
    Al hacer handoff hay que AUDITAR
    lo que llega a Claude Code:
    ├── tests
    ├── accesibilidad real
    ├── performance
    └── integración con sistemas existentes.

NO TRATAR DESIGN.MD COMO CÓDIGO
└── Es markdown, vive en git,
    debería pasar por PR como cualquier cambio importante.
    
    Si el DESIGN.md lo modifica cualquiera SIN review,
    tu sistema visual está en riesgo de DRIFT CONSTANTE.

MEZCLAR VERSIONES / BRANCHES SIN DISCIPLINA
└── Si tu equipo trabaja con feature branches,
    los cambios de DESIGN.md
    también deberían pasar por esas branches.
    
    Si NO, hay riesgo de que un cambio en DESIGN.md
    afecte features no relacionadas.
```

---

## Slide 40 — Errores frecuentes con el flujo combinado (1/2)

```
❌ OLVIDAR REGENERAR EL DESIGN.MD
   CUANDO CAMBIA EL DESIGN SYSTEM EN FIGMA
   El equipo de diseño introduce un nuevo color en Figma.
   Nadie regenera DESIGN.md.
   El código sigue usando los colores antiguos.
   
   Decisión:
   regenerar DESIGN.md como parte del workflow
   cada vez que el design system cambia,
   y como mínimo en ciclos regulares
   (quincenal, mensual).

❌ USAR CLAUDE DESIGN EN SESIONES DESCONECTADAS DEL CODEBASE
   Sin codebase linkado,
   los componentes generados son GENÉRICOS.
   Linka el repo siempre que sea relevante.

❌ PASAR A CLAUDE CODE UN HANDOFF SIN DESIGN.MD
   PRESENTE EN EL REPO
   Resultado: drift entre tokens del bundle
   y tokens reales.
   Asegura DESIGN.md en el repo destino
   ANTES del handoff.
```

---

## Slide 41 — Errores frecuentes con el flujo combinado (2/2)

```
❌ NO DOCUMENTAR QUÉ HERRAMIENTA USAR PARA QUÉ
   EN TU EQUIPO
   Cada miembro improvisa.
   Resultado: caos.
   
   Documenta en el CLAUDE.md del repo
   o en algún sitio visible:
   "para componentes UI nuevos, X herramienta;
    para presentaciones internas, Y;
    para mantenimiento del sistema, Z".

❌ PRETENDER QUE LA PRIMERA VERSIÓN
   DE CUALQUIER OUTPUT ES LA FINAL
   Todas estas herramientas generan un buen primer paso.
   La iteración sigue siendo necesaria.
   Si entregas el primer output sin pulir,
   la calidad SE NOTA.

❌ NO MEDIR EL COSTE
   Especialmente con Claude Design (token-heavy),
   la cuenta puede subir.
   Monitoriza usage regularmente con /usage.
   Si tu equipo está en Pro y consume >50% en una semana,
   es señal de pasar a Max.
```

---

## Slide 42 — Lo que tienes ahora con 5.2

```
✅ Las 5 categorías de output
   (UI, prototipos, decks, one-pagers, exploraciones,
    + frontier design)

✅ Los flujos prácticos para prototipos interactivos,
   pitch decks y one-pagers

✅ Design explorations
   (la categoría que rompe la restricción
    de "explorar es caro")

✅ La decisión clara entre las herramientas
   con tabla comparativa

✅ El flujo combinado completo
   (8 pasos del ciclo de vida de una feature)

✅ Los 3 flujos típicos según el equipo

✅ Anti-patrones generales del bloque
   y errores frecuentes
```

---

## Slide 43 — Lo que conviene llevarse al lunes

```
1. DESIGN.MD SIEMPRE
   No importa qué herramienta uses,
   ten un DESIGN.md vivo en el repo.
   Es la pieza más rentable del bloque entero.

2. COMBINA, NO ESCOJAS
   Las tres herramientas son complementarias.
   La decisión rara vez es "cuál de las tres"
   — es "qué COMBINACIÓN tiene sentido para esta tarea".

3. EL HANDOFF ES EL FEATURE
   El loop cerrado Claude Design → Claude Code
   es lo que diferencia este stack de cualquier otro.
   Pruébalo con una feature pequeña
   para entender cómo se siente.
   Después escálalo.
```

---

## Slide 44 — Pivote total a 5.3

```
En 5.3 PIVOTAMOS COMPLETAMENTE.
```

```
Hasta aquí hemos hablado de:

├── PERSONALIZACIÓN DEL AGENTE (módulos 1-3)
└── INTEGRACIÓN CON DISEÑO (módulos 4-5 hasta este punto)
```

```
Ahora cambiamos de tema en 60 minutos:
```

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   TESTS EN .NET CON CLAUDE CODE                          │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

> Es la parte MÁS CONCRETA y la que casi todos los equipos
> que prueban acaban quedándose con la mecánica.

---

## Slide 45 — Lo que vamos a cubrir en 5.3

```
SESIÓN 5 · SUBMÓDULO 5.3 — TESTS EN .NET (60 min)
─────────────────────────────────────────────────────

PARTE A: MECÁNICA DE TESTING
├── Por qué tests es donde Claude Code rinde tanto
├── CLAUDE.md como contrato del equipo
├── Tests unitarios con xUnit + NSubstitute + FluentAssertions
├── El antipatrón estrella: tests que NO testean nada
├── Tests de integración con WebApplicationFactory
├── Caso práctico guiado: API .NET completa
├── Detección de code smells y refactoring asistido
└── Documentación XML y OpenAPI/Swagger

PARTE B: ESTRATEGIA, WORKFLOW Y CIERRE DEL CURSO
├── Estrategia de cobertura
│   (tests útiles vs tests para inflar)
├── Workflow completo
│   (subagente test-generator + hook PostToolUse + 
│    flujo feature completa + integración CI)
├── Anti-patrones de testing con IA
├── Errores frecuentes con tu primera semana
└── CIERRE DEL MÓDULO Y DEL CURSO ENTERO
    ├── Los 5 módulos en una frase cada uno
    ├── El kit Claude Code del alumno
    ├── Qué hacer el lunes
    └── La pregunta final
```

---

## Slide 46 — La pregunta antes de pasar a 5.3

```
¿Cuál es el área de tu codebase actual
con MENOS TESTS y MÁS BUGS HISTÓRICOS?
```

```
Esa es probablemente tu mejor candidata para el lunes.
```

```
NO el módulo nuevo que vas a empezar
└── el VIEJO que carga deuda.
```

> Es donde Claude Code RINDE MÁS,
> porque es donde MÁS DOLOR estás aguantando.

---

## Slide 47 — Si tienes uno en mente

```
Si tienes uno en mente:
└── el siguiente apartado va a ser
    el MÁS DIRECTAMENTE ACCIONABLE
    del curso entero.
```

```
Pieza concreta.
Caso de uso concreto.
Resultado medible la primera semana.
```

> Esa es la promesa del 5.3.

---

## Slide 48 — Antes de pasar: una observación honesta

Hemos terminado el bloque de diseño completo. Vamos a parar un momento para reflexionar.

```
EL BLOQUE DE DISEÑO (módulos 4 + 5.1 + 5.2)
es el más NUEVO del curso entero.
```

**Tres cosas a tener en mente al adoptarlo:**

```
LAS HERRAMIENTAS ESTÁN EVOLUCIONANDO RÁPIDO
└── Claude Design es research preview.
    DESIGN.md es alpha.
    Lo que veis aquí es el estado a fecha del curso
    — features pueden aparecer y cambiar.

EL DOMINIO ES MENOS MADURO
└── que el de Claude Code puro
    (que lleva más tiempo y tiene más casos probados).
    Hay menos ejemplos de empresas grandes
    operando con esto en producción.

PERO EL POTENCIAL ES REAL
└── El loop cerrado Claude Design → Claude Code
    es genuinamente nuevo y resuelve un problema
    que llevaba años abierto.
    El que adopte esto bien tiene ventaja
    sobre quien siga con el flujo tradicional.
```

---

## Slide 49 — La estrategia de adopción del bloque de diseño

```
NO INTENTES ADOPTAR LAS TRES HERRAMIENTAS
A LA VEZ.
```

```
Mi recomendación de orden:

1. EMPIEZA POR DESIGN.MD
   Es lo más estable, lo menos costoso
   (cero tokens), y lo que más rentabilidad da.
   Una hora de setup, beneficio inmediato.

2. SIGUE POR FIGMA MCP
   Si tu equipo tiene Figma vivo.
   Setup de 30 minutos, ahorra horas
   en cada generación de componente.

3. CLAUDE DESIGN AL FINAL
   Cuando tengas claros los dos anteriores
   y un caso de uso concreto donde encaje
   (typically: prototipos rápidos
    o material visual sin diseñador).
```

> Tres meses para adoptar las tres bien
> es un plan razonable.
>
> Una semana para intentar las tres
> es un plan que va a fallar.

---

## Slide 50 — Nos vemos en 5.3

```
Bloque de diseño cerrado.
```

```
Submódulos cubiertos:

├── 4.1 — Figma MCP
├── 4.2 — Claude Design (qué es y cómo se usa)
├── 4.3 — DESIGN.md como sistema
├── 5.1 — Handoff Claude Design → Claude Code
└── 5.2 — Casos avanzados y flujo integrado
```

```
Ahora cambiamos de marcha completamente.
```

> Los siguientes 60 minutos
> son lo más concreto y accionable del curso.

**Nos vemos en 5.3.**
