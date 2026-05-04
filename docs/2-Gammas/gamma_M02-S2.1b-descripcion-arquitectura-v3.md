> **Versión:** v3 | **Módulo:** 2 | **Sub:** 2.1b | **Slides:** 33 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M02-S2.1b-descripcion-arquitectura-v3.md`

# Submódulo 2.1b — La descripción y la arquitectura

## Slide 1 — Portada
**Módulo 2 · Submódulo 2.1 · Parte B**
La descripción es el switch. Progressive disclosure.
Cómo decidir si algo merece ser skill.

---

## Slide 2 — Dónde estamos

En la parte A vimos la pieza por dentro: directorio, ciclo de vida, los 3 scopes, estructura, las 6 reglas técnicas críticas y el frontmatter YAML.

Ahora viene la parte de cómo el agente decide cuándo activar tu skill, cómo está diseñada la carga para que tener 30 skills no penalice, y cómo identificar si algo de tu día a día merece convertirse en skill.

```
1. La descripción es el switch
2. Progressive disclosure: 3 niveles
3. Mentalidad: candidatos a skill
4. SKILL.md vs CLAUDE.md vs AGENTS.md
5. Errores frecuentes con tus primeros skills
```

---

## Slide 3 — La descripción es el switch

Si te tienes que llevar una sola idea de este apartado, que sea esta:

> **La descripción es lo que decide si tu skill se activa o no.**
>
> No el nombre. No el contenido.
>
> **La descripción.**

```
Cuando arrancas una sesión de Claude Code:
└── El agente carga al sistema
    solo dos cosas de cada skill instalado:
    ├── el nombre
    └── la descripción

Cuando tú escribes una petición:
└── Claude mira las descripciones disponibles
    └── Y decide cuál coincide con lo que pides

Si ninguna coincide:
└── No usa skill

Si una coincide:
└── Carga el SKILL.md completo
    └── Y ejecuta
```

---

## Slide 4 — El skill invisible

Por eso una descripción mal escrita puede dejar tu skill **invisible**.

```
Instalado.
Ahí.
Pero nunca se activa
porque Claude no sabe cuándo usarlo.
```

> Este es, sin duda, el problema número uno
> de los principiantes con skills:
>
> escriben skills funcionalmente correctos que nunca se activan,
> y concluyen que *"esto no funciona"*.

---

## Slide 5 — Anti-patrones de descripción

Estos son los que más se ven en skills de principiantes:

```yaml
# Demasiado vaga — ¿cuándo se usa esto?
description: Ayuda con código

# Demasiado técnica — describe implementación, no caso de uso
description: Llama al endpoint /api/v2/sprint con auth OAuth2

# Demasiado amplia — se va a activar para todo
description: Asiste con cualquier tarea de desarrollo

# Sin disparadores — no hay verbos ni contexto
description: Estilos CSS

# Solo dice qué hace, no cuándo
description: Genera componentes Angular standalone

# Imperativo en primera persona — Anthropic recomienda tercera
description: Use this when you want to create controllers
```

---

## Slide 6 — La fórmula para una buena descripción

**Tres ingredientes:**

```
1. QUÉ HACE
   └── El verbo de acción concreto.

2. CUÁNDO USARLO
   └── Los disparadores lingüísticos.
       "Usar cuando el usuario pida X,
        mencione Y, o necesite Z"

3. TERCERA PERSONA, NO IMPERATIVO
   └── "Este skill genera..." / "Should be used when..."
       NO "Genera..." / "Use when..."
```

> La razón es técnica: la descripción se inyecta en el system prompt
> y la consistencia de punto de vista mejora la activación.

---

## Slide 7 — La misma capacidad: descripción mala vs buena

```yaml
# MAL — vaga, sin triggers
description: Genera componentes Angular
```

```yaml
# BIEN — concreta, con casos de uso
description: Genera componentes Angular standalone con Signals
  siguiendo la estructura del equipo. Usar cuando el usuario
  pida crear un nuevo componente, haga referencia a un componente
  nuevo en una feature, o cuando el flujo requiera scaffolding
  de UI Angular.
```

```
La segunda activa fiablemente cuando el usuario dice:

├── "crea un componente para el listado de pedidos"
├── "necesito un componente OrdersListComponent"
└── "vamos a hacer la UI del filtro"

La primera puede activarse o no,
según los humores del modelo.
```

---

## Slide 8 — Cuando la descripción "casi funciona": caso A

A veces las descripciones se activan a medias — funcionan en el 70% de los casos pero fallan en el 30%.

**Caso A: trigger demasiado específico.**

```
Descripción:
"Usar cuando el usuario diga 'genera un controller'"

Activa cuando dice esa frase EXACTA.

NO activa cuando dice:
├── "crea un controller"
└── "añade un nuevo controller"
```

**Solución:** variar el vocabulario en la descripción.

```
"Usar cuando el usuario pida generar, crear o añadir
un controller, o use sinónimos como endpoint o resource handler"
```

---

## Slide 9 — Caso B: trigger ambiguo entre dos skills

```
Tienes dos skills:
├── dotnet-controller
└── dotnet-review

Ambos hablan de "código .NET".

Cuando el usuario dice "revisa este controller"
└── ¿cuál se activa?

DEPENDE.
```

> Si los dos pueden, Claude probablemente activa el más específico
> al verbo *"revisar"*.
> Pero no es garantía.

**Solución:** descripciones más distintivas.

```
dotnet-controller → menciona "genera, crea, scaffolding, nuevo"

dotnet-review     → menciona "revisa, audita, valida,
                              antes de commit"
```

---

## Slide 10 — Caso C: contexto del proyecto que se da por hecho

```
Descripción:
"Genera componentes siguiendo nuestra arquitectura"

¿Qué arquitectura?
└── Claude no la conoce
    hasta que carga el cuerpo del skill.
```

**Pero la decisión de cargar el skill se toma con la descripción.**

```
Si la descripción asume contexto que no está
└── falla.
```

**Solución:** en la descripción, ser explícito sobre stack y patrón.

```
"Genera componentes Angular standalone con Signals
 para arquitectura signal-based store"
```

---

## Slide 11 — Cómo iterar una descripción

La forma más práctica de afinar una descripción es **usar el skill y observar**:

```
1. Escribes el skill con tu primera descripción.

2. Lanzas peticiones que esperarías que lo activaran.

3. Después de cada petición, le preguntas explícitamente:
   "¿qué skill has usado?"
   El agente te dice, sin filtrar.

4. Si NO se activó cuando esperabas:
   └── Refinas la descripción añadiendo
       variaciones del vocabulario que usaste.

5. Si se activó cuando NO esperabas:
   └── Restringes el alcance.
```

> Este proceso es no determinista — la activación de skills
> tiene componente probabilístico.
>
> La meta no es 100%, es que sea fiable cuando importa.

---

## Slide 12 — Progressive disclosure: la arquitectura que hace que esto funcione a escala

Esto es lo que diferencia un skill de un prompt largo que metes en `CLAUDE.md`.

> **Los skills se cargan en TRES NIVELES, no de una vez.**

```
Nivel 1 → metadata (siempre cargada)
Nivel 2 → instrucciones (al activarse)
Nivel 3 → recursos profundos (bajo demanda)
```

Los vemos uno a uno, y al final hacemos las matemáticas.

---

## Slide 13 — Nivel 1: metadata (siempre cargada)

```
Solo name + description.

≈ 100 tokens por skill instalado.

Esto vive en el system prompt de Claude
desde que arrancas la sesión.
```

**Implicación práctica:**

```
Puedes tener DECENAS de skills instalados
sin que se te coma el contexto.

Cada skill que añadas suma ~100 tokens.

Negligible.
```

---

## Slide 14 — Nivel 2: instrucciones (cargadas al activarse)

```
Cuando Claude decide que un skill aplica a la tarea actual:
└── Lee el cuerpo de SKILL.md

Aquí entran:
├── Las instrucciones detalladas
├── Los workflows
└── Las reglas
```

**La recomendación oficial:**

```
Por debajo de 5.000 tokens
└── ~1.500-2.000 palabras
```

> Esto es lo que importa para el rendimiento.
> Si tu skill se carga, esos tokens entran al contexto.

---

## Slide 15 — Nivel 3: recursos profundos (cargados bajo demanda)

```
Si el skill tiene references/ o assets/:

└── Esos ficheros NO se cargan automáticamente
    al activarse el skill.

    Solo cuando el cuerpo de SKILL.md
    los referencia explícitamente
    └── Y Claude los lee con la herramienta correspondiente.
```

**Implicación:**

```
Puedes tener manuales enteros de convenciones,
especificaciones largas o catálogos detallados
como parte de tu skill.

Sin que ocupen contexto
└── a no ser que se necesiten para la tarea concreta.
```

---

## Slide 16 — Las matemáticas: ¿por qué importa?

Imagina que tu equipo tiene **30 skills instalados** — convenciones de .NET, convenciones de Angular, generadores, checklists de revisión, plantillas, etc.

```
SIN PROGRESSIVE DISCLOSURE
(todos los skills cargados siempre)

30 skills × ~2.000 tokens cada uno = 60.000 tokens
                                     de overhead

Sobre una ventana de Sonnet de 200.000 tokens:
└── 30% comido antes de empezar

Resultado:
├── Contexto saturado
├── Sesiones cortas
└── Comportamiento pobre
```

---

## Slide 17 — Las matemáticas: con progressive disclosure

```
CON PROGRESSIVE DISCLOSURE
(lo que tenemos)

30 skills × ~100 tokens metadata = 3.000 tokens
                                   de overhead total

Cuando uno se activa, suma sus ~2.000 tokens.

Total: ~5.000 tokens de overhead efectivo.

Sobre la misma ventana de 200.000:
└── 2,5%
```

> Esa es la diferencia entre poder tener 30 skills sin penalización
> y tener que andar eligiendo cuáles instalar para no saturar la sesión.

```
Esta es la razón por la que tener muchos skills instalados
no penaliza, mientras que tener un CLAUDE.md enorme sí lo hace.
```

---

## Slide 18 — Mentalidad: cómo identificar candidatos a skill

Esta sección es donde se decide si tu kit de skills va a ser útil o solo un montón de ficheros bonitos.

> **No todo merece ser un skill.**

Tres preguntas para decidir si algo es candidato. Las vemos.

---

## Slide 19 — Pregunta 1: ¿es un patrón que se repite?

```
¿Es un patrón que se repite, o es un caso puntual?
```

```
Si haces algo UNA VEZ AL MES
└── No es skill
    └── Es una conversación con Claude Code cuando toque.

Si lo haces TRES VECES A LA SEMANA
└── Sí.
```

> La frecuencia justifica el coste de definir un skill:
>
> escribirlo, mantenerlo, documentarlo.

---

## Slide 20 — Pregunta 2: ¿tiene reglas no obvias?

```
¿Tiene reglas no obvias que el agente no deduciría solo?
```

```
Si tu equipo tiene una convención específica
que no está en el código
├── porque es nueva
├── porque está mezclada
└── porque tiene excepciones

└── Un skill captura esas reglas.

Si el agente puede hacer el trabajo igual de bien sin el skill
└── No lo necesitas.
```

**Test rápido:**

```
Pídele al agente la tarea SIN skill.

Si lo hace bien
└── No necesitas skill.

Si comete errores que tendrías que corregir cada vez
(siempre los mismos)
└── ESO es lo que va al skill.
```

---

## Slide 21 — Pregunta 3: ¿el output es predecible?

```
¿El output es predecible o varía mucho?
```

**Skills brillan en tareas con output relativamente predecible:**

```
✅ Generar un controller
✅ Escribir un test
✅ Formatear un commit message
```

**Tareas creativas o de criterio NO son buen candidato para skill:**

```
❌ Diseñar arquitectura
❌ Decidir trade-offs

Eso es conversación.
```

---

## Slide 22 — Patrones que SÍ merecen skill

```
✅ Generación de boilerplate con convenciones del equipo
   (controllers, componentes, DTOs)

✅ Code review con checklist específico del equipo

✅ Generación de tests siguiendo el patrón establecido
   (xUnit + NSubstitute, ng-test, etc.)

✅ Mensajes de commit con formato de equipo
   (semantic, con referencia a issue, con cuerpo estructurado)

✅ Documentación de funciones/clases con el formato del equipo
   (XML docs, JSDoc, estilo concreto)

✅ Generación de migraciones con naming convention

✅ Setup inicial de features
   (carpeta + ficheros + tests + entrada en routing)
```

---

## Slide 23 — Patrones que NO merecen skill

```
❌ "Explícame qué hace este código"
   └── Eso es conversación.

❌ "Optimiza este algoritmo"
   └── Eso es razonamiento de criterio.

❌ "Decide qué arquitectura usar"
   └── Eso es discusión.

❌ "Refactoriza este módulo"
   └── Lo es solo si tu equipo tiene
       un patrón muy específico de refactor;
       si no, es conversación normal.

❌ "Resuelve este bug"
   └── Es debugging, conversación.
```

---

## Slide 24 — La heurística

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   Si la respuesta correcta depende del CRITERIO          │
│   → no es skill.                                         │
│                                                          │
│   Si la respuesta correcta es SEGUIR UN PATRÓN           │
│   → sí.                                                  │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

---

## Slide 25 — SKILL.md vs CLAUDE.md vs AGENTS.md

Cierre del apartado con la decisión que más confunde a la gente que llega a esto. Tres ficheros con propósitos distintos:

| Fichero | Cuándo se carga | Para qué sirve |
|---|---|---|
| `CLAUDE.md` | Siempre, al arrancar sesión | Contexto del proyecto que aplica a *todo* lo que hagas en él |
| `AGENTS.md` | Siempre, al arrancar sesión | Lo mismo que `CLAUDE.md` pero como estándar cross-tool |
| `SKILL.md` | Solo cuando el skill se activa | Capacidad puntual que aplica a *ciertas tareas* concretas |

---

## Slide 26 — Árbol de decisión rápido

```
¿Es información que el agente necesita SIEMPRE
 que toques este repo?
 (estructura, convenciones generales, comandos clave)

 → CLAUDE.md

¿Es información que el agente solo necesita
 en CIERTAS tareas?
 (cómo generar un controller, cómo revisar un PR,
  cómo escribir un test concreto)

 → skill

¿Quieres que viaje contigo a otros proyectos?

 → skill personal en ~/.claude/skills/

¿Es para tu equipo?

 → skill de proyecto en .claude/skills/, va a git
```

---

## Slide 27 — Lo que NO hay que hacer

> Meter en `CLAUDE.md` todo lo que se te ocurra "por si acaso".

```
Cada cosa que metas en CLAUDE.md:
├── Pesa en cada sesión
├── Para cualquier tarea
└── Para cualquier persona

Si una convención solo aplica
a una de cada cinco tareas:
└── No debería estar en CLAUDE.md
    └── Debería ser un skill
```

---

## Slide 28 — Casos prácticos del árbol de decisión

```
CASO 1: Convención de naming de variables
└── Aplica a TODO el código
    └── Va en CLAUDE.md

CASO 2: Forma estándar de generar un endpoint nuevo
└── Solo aplica cuando generas endpoints
    └── SKILL

CASO 3: Comando para arrancar el dev environment
└── Aplica siempre que trabajes en el repo
    └── CLAUDE.md

CASO 4: Checklist de seguridad antes de un PR
└── Solo aplica antes de PRs
    └── SKILL

CASO 5: Tu preferencia personal de comentar
        el código en español
└── Es tuya, va contigo a otros proyectos
    └── SKILL personal

CASO 6: Estructura de carpetas del proyecto
└── Es del proyecto, aplica siempre
    └── CLAUDE.md
```

---

## Slide 29 — Errores frecuentes con tus primeros skills (1/2)

```
❌ DESCRIPCIÓN GENÉRICA
   "Genera código" no activa nada.
   └── Sé específico, con verbos y casos de uso.

❌ SKILLS QUE DUPLICAN LO QUE YA HACE CLAUDE SOLO
   Si el agente sin skill ya genera tests xUnit decentes,
   un skill genérico de tests xUnit no aporta.
   └── El valor está en codificar las PARTICULARIDADES
       de tu equipo, no las prácticas generales.

❌ CUERPO DEL SKILL DEMASIADO LARGO
   Si el SKILL.md pasa de 2.000 palabras,
   parte en references/.
   └── Si no, ocupa contexto innecesariamente.

❌ SKILLS SIN TESTAR
   Escribes el skill, asumes que funciona.
   └── Lánzalo en una conversación
       y verifica que se activa cuando esperas
       y hace lo que esperas.
```

---

## Slide 30 — Errores frecuentes con tus primeros skills (2/2)

```
❌ MISMO SKILL EN PROYECTO Y USER
   Si lo tienes en .claude/skills/
   y en ~/.claude/skills/ con el mismo nombre,
   hay conflicto.
   └── Decide dónde vive y bórralo del otro.

❌ SKILLS QUE DEBERÍAN SER CLAUDE.md
   Si tu skill aplica a TODAS las tareas del repo,
   no es un skill.
   └── Es contenido de CLAUDE.md
       que has puesto en el sitio equivocado.

❌ SKILLS SIN SCOPE CLARO
   Un skill que hace tres cosas distintas
   activará mal.
   └── Mejor tres skills pequeños que uno gordo.

❌ NO ITERAR LA DESCRIPCIÓN
   La primera descripción casi nunca es la final.
   └── Itera al menos dos veces antes de darla por buena.
```

---

## Slide 31 — Antes de seguir: lo que tienes ahora

Ya tienes el modelo conceptual completo de un skill:

```
✅ Directorio + SKILL.md = unidad mínima
✅ Ciclo de vida de carga (4 fases)
✅ 3 scopes (personal, proyecto, plugin)
✅ Estructura (scripts/, references/, assets/)
✅ Las 6 reglas técnicas críticas
✅ El frontmatter YAML con sus campos
✅ La descripción como SWITCH de activación
✅ Progressive disclosure en 3 niveles
✅ Los criterios para decidir si algo merece skill
✅ La diferencia con CLAUDE.md y AGENTS.md
```

> En el siguiente apartado escribimos uno desde cero.

---

## Slide 32 — Preguntas para llegar al 2.2

Antes de pasar, dos preguntas que conviene tener pensadas:

```
PRIMERA
¿Qué descripción le pondrías al skill que hagamos juntos
en 2.2 (un generador de componentes Angular standalone)?

"Cuándo activarlo, qué hace, qué espera de mí"

Esa es la pieza que más vamos a iterar.
```

```
SEGUNDA
¿Qué patrón tiene tu equipo que sería el SEGUNDO skill,
después del generador de componentes?

El generador lo hacemos juntos como ejemplo guiado.
El segundo lo elegirás tú
└── Y será el que más rentabilidad te dé
    porque será específico a tu trabajo real.
```

---

## Slide 33 — Lo que viene en 2.2

```
SUBMÓDULO 2.2 — CREACIÓN DE SKILLS PERSONALIZADOS
─────────────────────────────────────────────────────

Antes de escribir nada: resuelve un caso primero
└── El consejo del PDF oficial de Anthropic

Construimos juntos un skill en CUATRO versiones progresivas:

  Versión 1 — El skill más simple posible
              SKILL.md mínimo, frontmatter, instrucciones cortas

  Versión 2 — Añadiendo convenciones del equipo
              El detalle que diferencia un skill bueno

  Versión 3 — Con plantillas en assets/
              Cuando la prosa empieza a quedarse corta

  Versión 4 — Con script ejecutable
              La capa determinista para tareas mecánicas

Y al final:
├── Control de invocación
├── Subagentes en skills (referencia rápida)
├── Scopes: dónde vive cada skill
└── Errores frecuentes con tus primeros skills
```

**Nos vemos en 2.2.**
