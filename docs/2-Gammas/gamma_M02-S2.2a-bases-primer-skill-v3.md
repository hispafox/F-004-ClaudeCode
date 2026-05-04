> **Versión:** v3 | **Módulo:** 2 | **Sub:** 2.2a | **Slides:** 30 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M02-S2.2a-bases-primer-skill-v3.md`

# Submódulo 2.2a — Bases y primer skill funcional

## Slide 1 — Portada
**Módulo 2 · Submódulo 2.2 · Parte A**
Bases y primer skill funcional
Versión 1 y versión 2 del generador de componentes Angular

---

## Slide 2 — Construir uno, no leer sobre uno

```
La mejor forma de entender un skill
└── es escribirlo.
```

Lo que viene son los próximos 60 minutos de la sesión:

```
Construimos juntos un skill REAL.
En CUATRO versiones.
Cada una más sofisticada que la anterior.

Empezamos por:
└── el SKILL.md mínimo que se puede escribir
    en una pantalla de móvil

Terminamos con:
└── un skill que tiene scripts, plantillas
    y convenciones del equipo codificadas
```

> Y entre medias, vamos viendo qué problema soluciona cada capa.

---

## Slide 3 — El skill de ejemplo

El skill de ejemplo es un **generador de componentes Angular standalone**.

**Por qué este:**

```
1. Es un caso común
   └── Cualquier equipo Angular agradece automatizar.

2. Permite enseñar la progresión de complejidad
   de forma natural.

3. Al final del apartado el alumno se lleva un skill
   └── Que puede instalar literalmente tal cual
       en su repo del trabajo.
```

> Si tu equipo trabaja con .NET puro y Angular no aplica:
> el patrón se traduce directamente.
> Cambia "componente Angular" por "controller .NET"
> y los conceptos se mantienen idénticos.

---

## Slide 4 — Cómo leer este apartado

```
Cada versión del skill es FUNCIONAL.

Si te paras en la versión 2:
└── Ya tienes un skill útil.

Las versiones 3 y 4 añaden potencia
└── Pero no son obligatorias.
```

> La progresión está pensada para que veas
> cuándo cada capa empieza a justificarse.

---

## Slide 5 — Antes de escribir nada: resuelve un caso primero

Hay un consejo que aparece en la guía oficial de Anthropic que vale la pena interiorizar antes de tocar `SKILL.md`:

> Los creadores de skills más efectivos **iteran sobre una única tarea desafiante hasta que Claude la resuelve bien**, y solo entonces extraen el patrón ganador a un skill.

```
Es contraintuitivo.

La tentación natural cuando aprendes lo de los skills
es pensar:

"voy a hacer un skill de generación de componentes Angular"

Abrir un editor.
Empezar a escribir el SKILL.md desde cero.

Y casi siempre sale mal en la primera versión.
```

---

## Slide 6 — Por qué falla empezar escribiendo el skill

```
Cuando partes de un editor en blanco:

├── Faltan convenciones que sí usabas en práctica
├── Sobran reglas que en realidad no aplicas
├── Las descripciones no activan
└── Los ejemplos no encajan con la realidad del repo
```

> La razón es sencilla:
> empezar por escribir el skill desde cero
> parte de una **hipótesis**.
>
> *"Creo que mi equipo hace los componentes así."*
>
> Y las hipótesis de cómo trabajamos
> rara vez coinciden con cómo trabajamos de verdad.

---

## Slide 7 — La alternativa que funciona mejor

Cuatro pasos:

```
1. COGE UN CASO CONCRETO
   No abstracto.
   "Tengo que crear el componente OrdersListComponent
    aquí, en este repo, ahora."

2. TRABAJA CON CLAUDE CODE EN INTERACTIVO, SIN SKILL
   Sesión normal. Conversación.
   Iteras hasta que el componente queda como tú quieres.
   ├── Le corriges lo que pifia
   ├── Le explicas tus convenciones cuando se las salta
   └── Le indicas qué imports en qué orden

3. CUANDO EL RESULTADO FINAL TE GUSTA, PARAS Y MIRAS
   Lo que has hecho durante 20-30 minutos
   └── es el material en bruto del skill.

4. EXTRAES EL PATRÓN GANADOR A SKILL.md
   No inventas nada
   └── Copias lo que has aprendido en la sesión
       a un formato persistente.
```

---

## Slide 8 — Por qué este flujo da mejor resultado

```
Empezar resolviendo un caso primero
└── parte de EVIDENCIA.

"Este componente concreto me ha quedado bien
 después de iterar X veces, y aquí están las
 correcciones que tuve que hacer."

El skill que sale de eso
└── ya sabe lidiar con casos reales
    porque se ha escrito a partir de uno.
```

**Ventaja añadida:**

```
Durante la sesión iterativa estás enseñándole a Claude
tu convención EN TIEMPO REAL.

Lo que aprendes en esa sesión no es solo
"cómo se hace el componente"
└── Es también
    "qué necesita Claude que le diga para hacerlo bien"

Esa información — qué es obvio para Claude
y qué hay que escribirle —
es justo lo que un skill captura.
```

---

## Slide 9 — Cuándo este consejo NO aplica

Hay un caso donde sí merece la pena partir de la teoría:

```
Cuando ya tienes el skill mental claro
porque has hecho la tarea cientos de veces a mano.
```

```
Si llevas cinco años generando controllers
y tienes la convención del equipo metabolizada
└── Te puedes saltar la fase de iteración
    └── Y escribir el SKILL.md directamente.
```

> Pero esto es menos frecuente de lo que parece.
> Incluso en patrones que crees dominar,
> la sesión iterativa con Claude saca a la luz
> convenciones implícitas que no habías articulado nunca.

---

## Slide 10 — Lo que vamos a hacer en este apartado

Aunque el consejo es "primero resuelve un caso", en este apartado vamos a saltar ese paso por economía de tiempo de clase.

```
El generador de componentes del ejemplo
está YA basado en una sesión iterativa real
que se hizo previamente.
└── El patrón ganador ya está extraído.

Lo que vas a ver son las cuatro versiones progresivas
del skill que sale de ese trabajo previo.
```

> Cuando vuelvas a tu repo del trabajo y vayas a hacer tu primer skill de verdad:
> **sigue el flujo iterativo primero.**
>
> Es la diferencia entre que tu skill funcione el día 1
> o que necesite tres rondas de refinamiento
> antes de empezar a aportar valor.

---

## Slide 11 — Versión 1: el skill más simple posible

Empezamos por el mínimo absoluto.

```
Un solo fichero.
Frontmatter mínimo.
Instrucciones cortas.
```

**La estructura:**

```
.claude/skills/angular-component/
└── SKILL.md
```

> Eso es todo.

---

## Slide 12 — Versión 1: el SKILL.md (1/2)

```markdown
---
name: angular-component
description: Genera componentes Angular standalone con Signals
  siguiendo la estructura del equipo. Usar cuando el usuario
  pida crear un nuevo componente, haga referencia a un componente
  nuevo en una feature, o cuando el flujo requiera scaffolding
  de UI Angular.
---

# Generador de componentes Angular standalone

## Cuándo se usa este skill

Cuando el usuario pida crear un componente Angular nuevo.
Esto incluye peticiones como "crea un componente para X",
"necesito un componente OrdersList", "vamos a hacer la UI de Y",
o referencias implícitas a un componente que no existe aún.
```

---

## Slide 13 — Versión 1: el SKILL.md (2/2)

```markdown
## Qué genera

Un componente Angular standalone con la estructura estándar:

- Un fichero `.component.ts` con la clase
  y el decorador `@Component({ standalone: true })`
- Un fichero `.component.html` con el template
- Un fichero `.component.scss` con los estilos
- Un fichero `.component.spec.ts` con tests con Jasmine

## Convenciones que sigue

- Componentes `standalone: true` siempre. Nada de NgModules nuevos.
- Para estado local, `signal()`. Para valores derivados, `computed()`.
- Los inputs y outputs van como propiedades del componente.
- Nombres en kebab-case para selectores (prefijo `app-`).
- Tests con configuración mínima necesaria.

## Pasos al generar

1. Pregunta al usuario el nombre del componente si no lo ha dado.
2. Decide la ubicación según la estructura del proyecto.
3. Genera los cuatro ficheros.
4. Si el componente debe enrutarse, sugiere la entrada en routing
   pero no la añadas sin confirmación.
```

> Frontmatter con `name` y `description`.
> Cuerpo con instrucciones claras.

---

## Slide 14 — Versión 1: probarlo

Lanza una sesión nueva en el repo donde has metido el skill.

```
Pídele:
"crea un componente para mostrar el listado de pedidos"

Si todo va bien:
└── Claude detecta la activación del skill
    └── Y genera los cuatro ficheros con la estructura descrita.
```

**Verifica que se ha activado:**

```
> ¿qué skill has usado para esto?
```

```
Claude te dice.

Si no se activó:
└── vuelves a la descripción del frontmatter
    y la afinas.
```

---

## Slide 15 — Versión 1: limitaciones

Esta primera versión funciona, pero tiene límites:

```
EL TEMPLATE ES GENÉRICO
├── Genera un componente con la estructura básica
└── Pero no con los detalles específicos del equipo

NO CONOCE EL CONTEXTO DEL PROYECTO
├── Si tu equipo usa NgRx SignalStore para estado compartido
│   └── Este skill no lo aplica automáticamente
└── Si tienes un estilo concreto de tests, tampoco

LAS CONVENCIONES ESTÁN EN PROSA, NO CODIFICADAS
└── Si una convención cambia en el equipo
    └── Hay que reescribirla a mano en el SKILL.md
```

> Para los casos básicos basta.
> Pero el siguiente paso natural es subir el detalle.

---

## Slide 16 — Versión 2: añadiendo convenciones del equipo

La siguiente capa: meter en el skill las convenciones reales del equipo.

> Lo que diferencia a un componente generado por el skill
> de uno generado por Claude sin skill
> **es justamente esto.**

---

## Slide 17 — Las convenciones que vamos a meter

Imagina que tu equipo tiene estas convenciones específicas:

```
ORDEN DE IMPORTS
├── Angular core
├── Librerías externas
├── Módulos internos
└── Componentes hijo

SELECTOR
└── Siempre con prefijo "app-" y kebab-case

ESTRUCTURA DE LA CLASE
└── 5 bloques en orden estricto:
    imports, decorador, propiedades públicas,
    propiedades privadas, métodos

INYECCIÓN DE DEPENDENCIAS
└── inject() en lugar del constructor

TESTS
├── TestBed siempre
└── Patrón Arrange-Act-Assert

LIFECYCLE HOOKS
└── ngOnInit antes de ngOnDestroy
```

> Ajusta a la realidad de tu equipo. Esto es ejemplo.

---

## Slide 18 — Versión 2: SKILL.md ampliado (1/4)

**Frontmatter** (no cambia respecto a v1):

```markdown
---
name: angular-component
description: Genera componentes Angular standalone con Signals
  siguiendo la estructura del equipo. Usar cuando el usuario
  pida crear un nuevo componente...
---

# Generador de componentes Angular standalone

## Cuándo se usa este skill

[mismo que v1]

## Estructura del componente

Cada componente generado tiene cuatro ficheros:

- `<nombre>.component.ts` — clase del componente
- `<nombre>.component.html` — template
- `<nombre>.component.scss` — estilos
- `<nombre>.component.spec.ts` — tests unitarios
```

---

## Slide 19 — Versión 2: SKILL.md ampliado (2/4) — convenciones .ts

```markdown
## Convenciones del fichero .ts

### Orden de imports (estricto)

1. Angular core (`@angular/core`, `@angular/common`, etc.)
2. Librerías externas (RxJS, librerías de terceros)
3. Módulos internos del proyecto
4. Componentes hijo

Una línea en blanco entre cada bloque.

### Decorador

```typescript
@Component({
  selector: 'app-<kebab-case>',
  standalone: true,
  imports: [...],
  templateUrl: './<nombre>.component.html',
  styleUrl: './<nombre>.component.scss'
})
```

Siempre `standalone: true`. Selector con prefijo `app-`
y nombre en kebab-case.
```

---

## Slide 20 — Versión 2: SKILL.md ampliado (3/4) — estructura clase

```markdown
### Estructura de la clase

Bloques en este orden, separados por una línea en blanco:

1. Inputs (signals input)
2. Outputs (signals output)
3. Inyecciones con `inject()`
4. Estado local (signals)
5. Valores derivados (computed)
6. Lifecycle hooks (ngOnInit, ngOnDestroy)
7. Métodos públicos
8. Métodos privados (prefijo `_`)

### Inyección de dependencias

Siempre con `inject()`, nunca por constructor:

```typescript
private readonly orderService = inject(OrderService);
```
```

---

## Slide 21 — Versión 2: SKILL.md ampliado (4/4) — template, tests, qué NO

```markdown
## Convenciones del template

- Indentación de 2 espacios.
- Atributos largos en líneas separadas, alineados verticalmente.
- Control flow nuevo: `@if`, `@for`, `@switch`.
  No usar las directivas estructurales antiguas.

## Convenciones del fichero spec.ts

- TestBed con configuración mínima necesaria.
- Estructura Arrange-Act-Assert con comentarios.
- Mocks con `jasmine.createSpyObj`.

## Lo que NO debe hacer el skill

- No generar `NgModule` (son standalone).
- No usar `Subject` o `BehaviorSubject` para estado interno.
- No inyectar `ChangeDetectorRef` para forzar refresh.
- No añadir el componente al routing sin pedir confirmación.

## Pasos al generar

1. Si el usuario no ha dado nombre, preguntar.
2. Decidir ubicación según estructura del proyecto.
3. Generar los cuatro ficheros con las convenciones de arriba.
4. Verificar que el resultado compila.
5. Si hay routing implicado, sugerir snippet sin aplicar.
```

---

## Slide 22 — Versión 2: lo que has conseguido

```
Bastante más sustancia.
Sigue siendo un solo fichero.
Pero ahora encapsula CONOCIMIENTO REAL del equipo.
```

```
Diferencia respecto a v1:
├── Las convenciones están explícitas
├── Las prohibiciones están claras
├── El orden de bloques está fijado
└── El estilo de tests está documentado
```

> Un componente generado con esta v2
> es prácticamente indistinguible de uno hecho a mano
> por un senior que conoce las convenciones del equipo.

---

## Slide 23 — Cuándo parar de añadir prosa

A medida que añades convenciones, el `SKILL.md` crece. Hay **dos señales** que dicen *"ya basta de prosa, hay que cambiar de estrategia"*.

---

## Slide 24 — Señal 1: el fichero pasa de 2.000 palabras

```
Anthropic recomienda mantenerlo por debajo.

Más allá:
└── el cuerpo del skill ocupa demasiado contexto al activarse.
```

> Recordad que el cuerpo se carga ENTERO
> cuando el skill se activa.
>
> Más palabras = más contexto consumido
> = menos espacio para tu código en la sesión.

---

## Slide 25 — Señal 2: las convenciones se vuelven repetitivas o muy detalladas

```
Cuando estás escribiendo el quinto bloque
de código de ejemplo en prosa
└── es señal de que el contenido
    ya no es prosa
    └── son plantillas.

Y las plantillas viven mejor en assets/.
```

---

## Slide 26 — Cuando llegues a una de las dos señales

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   Toca pasar a la siguiente versión.                     │
│                                                          │
│   ├── Si el problema es ESPECIFICIDAD                    │
│   │   (necesitas plantillas concretas con placeholders)  │
│   │   → Versión 3 (assets/)                              │
│   │                                                      │
│   └── Si el problema es DETERMINISMO                     │
│       (necesitas cálculos, validaciones, datos)          │
│       → Versión 4 (scripts/)                             │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

---

## Slide 27 — Lo que tienes ahora con las versiones 1 y 2

```
✅ Un skill funcional con frontmatter mínimo
✅ Una descripción que activa fiablemente
✅ Convenciones del equipo codificadas en prosa
✅ Cinco bloques en orden estricto
✅ Reglas duras de "qué NO hacer" explícitas
✅ Pasos claros al generar
```

> Para muchos casos, esto basta.
>
> El generador de componentes con la versión 2
> ya genera código que pasa code review.

---

## Slide 28 — Para qué casos sí necesitarás más

**No todos los skills llegan hasta v3 o v4.**

```
NECESITAS V3 (plantillas)
└── Cuando tienes bloques de código repetitivos
    con estructura fija que solo cambia en ciertos placeholders.

NECESITAS V4 (scripts)
└── Cuando hay tareas deterministas:
    ├── Convertir formatos de nombres (PascalCase → kebab-case)
    ├── Validar que algo no colisiona
    ├── Leer datos del proyecto para tomar decisiones
    └── Cualquier cosa que un script hace siempre igual
```

> El siguiente apartado (2.2b) cubre las dos.

---

## Slide 29 — Hábito sano: parte por la versión más simple

Lo que NO hay que hacer:

```
❌ Empezar por la versión 4
   Meter scripts y plantillas el primer día
   sin haber probado siquiera la versión 1.
```

> La sobreingeniería es el primer enemigo.

**Lo que sí funciona:**

```
1. Empieza con un SKILL.md simple
2. Úsalo
3. Ve qué falla
4. Añade capas SOLO cuando se justifiquen
```

> En la siguiente parte vemos cuándo y cómo
> se justifican esas capas.

---

## Slide 30 — Lo que viene en 2.2b

```
SUBMÓDULO 2.2b — SCRIPTS Y PLANTILLAS
─────────────────────────────────────────────────────

Scripts ejecutables: el siguiente nivel
├── El concepto: tareas deterministas
├── Sintaxis inline:    !`comando`
├── Sintaxis en bloque: ```!
├── Casos típicos
├── Cuándo usar scripts y cuándo prosa
└── Una advertencia sobre seguridad

Versión 3: con plantillas en assets/
├── Estructura
├── Una plantilla concreta con placeholders
├── Cómo referenciar desde SKILL.md
└── Ventajas

Versión 4: con script ejecutable
├── Cuándo merece la pena
├── Un script Python de ejemplo
├── Cómo el skill llama al script
└── La estructura final del skill nivel producción
```

**Nos vemos en 2.2b.**
