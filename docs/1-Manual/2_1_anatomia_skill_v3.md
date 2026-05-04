# 2.1 Anatomía de un skill

**Duración en clase:** 30 minutos · **Sesión 2, submódulo 1**
**Versión:** v3 — incorpora reglas técnicas críticas y campo `compatibility` extraídos de la guía oficial de Anthropic *"The Complete Guide to Building Skills for Claude"*.

---

## La pregunta del cierre del módulo anterior

Te quedaste con una pregunta al final del módulo 1: ¿qué patrón se repite tres veces a la semana en tu equipo y al que tendrías que enseñar a un junior nuevo? *"Cuando creas un controller, va con este DTO, este validator y este test"*. *"Cuando tocas el dominio, ejecuta este check de invariantes"*. *"Cuando subes un PR, este checklist de seguridad"*.

Cada uno de esos patrones es candidato a skill. Y los skills son el mecanismo que convierte a Claude Code de un asistente genérico — que sabe programar, sabe leer tu repo, sabe ejecutar comandos — en un especialista que conoce las convenciones de tu equipo. **El que sabe *exactamente* cómo se hacen las cosas en vuestro código.**

He visto a un equipo que pasó de *"Claude Code es interesante pero no acaba de cuadrar con cómo trabajamos"* a *"no sé cómo trabajábamos sin esto"* en una semana, después de escribir tres skills. Tres. Uno para generar controllers con sus DTOs. Otro para revisar PRs con el checklist interno. Y un tercero, quizá el más útil, para escribir mensajes de commit siguiendo la convención del equipo. Skills tontos sobre el papel — el tercero literalmente es *"escribe el commit message como yo lo haría"*. Pero en agregado, estos tres skills convirtieron al agente en alguien que conocía las convenciones del equipo mejor que algunos juniors.

Antes de escribir uno completo en el siguiente apartado, conviene entender la pieza por dentro. Eso es este apartado. **El microscopio antes del telescopio.**

---

## La unidad mínima

Un skill es, en lo más básico, **un directorio con un fichero `SKILL.md` dentro**.

Eso es todo. Si solo metes un `SKILL.md` en un directorio con la estructura correcta, ya tienes un skill funcional. El resto — scripts, ficheros de referencia, plantillas — son extensiones opcionales que se cargan cuando hacen falta.

Esta simplicidad es deliberada. Un skill se piensa como una unidad pequeña y autónoma — una capacidad concreta que Claude carga *cuando le hace falta*, no cuando arranca la sesión. La diferencia con `CLAUDE.md` es importante y la cubrimos al final del apartado.

### El ciclo de vida de un skill

Para entender por qué un skill es lo que es, ayuda ver el ciclo desde el punto de vista del agente:

**Al arrancar la sesión:** Claude lee la metadata de cada skill instalado — solo el frontmatter, no el cuerpo. Esa metadata se carga en su system prompt. *Ahora sabe que existen, sabe cuándo usarlos, pero no ha leído sus instrucciones.*

**Durante la conversación:** tú escribes una petición. Claude la analiza y mira las descripciones de skills disponibles. Si alguna coincide con lo que pides, **carga el cuerpo de ese skill** y empieza a aplicarlo.

**Durante la ejecución:** si el cuerpo del skill referencia ficheros adicionales (`references/`, scripts, plantillas), Claude los lee solo si los necesita.

**Al terminar:** la información del skill se queda en el contexto de la sesión actual. Para la siguiente sesión, vuelta a empezar — solo metadata cargada.

Este ciclo es lo que hace que tener 30 skills instalados no sea un problema: solo los que activan ocupan contexto serio.

### Dónde viven

Tres scopes, igual que con `settings.json`:

- **Personal** — `~/.claude/skills/<nombre>/SKILL.md`. Tus skills personales, viajan contigo de proyecto en proyecto. Aquí van cosas como tus convenciones de naming personales o un skill de "explica este código en español de forma didáctica" si trabajas mucho explicando código.
- **Proyecto** — `.claude/skills/<nombre>/SKILL.md`. Skills del equipo, van a git, se comparten al clonar. La mayoría de skills útiles para un equipo viven aquí.
- **Plugin** — empaquetados dentro de un plugin distribuible. Lo veremos en 2.3 cuando hablemos de cómo distribuir un kit completo de skills + MCP servers.

El nombre del directorio se convierte en el nombre del skill. Así que un directorio `.claude/skills/dotnet-controller/SKILL.md` define un skill llamado `dotnet-controller` que se puede invocar con `/dotnet-controller` o, si la descripción está bien escrita, autónomamente cuando Claude detecta que se necesita.

### Nota importante: la fusión skills + commands

Si has visto tutoriales antiguos que mencionan `.claude/commands/<nombre>.md`, ese formato sigue funcionando pero está marcado como **legacy** desde finales de 2025. Anthropic ha unificado custom commands y skills en un único modelo. Lo recomendado a partir de ahora es `.claude/skills/<nombre>/SKILL.md`. Hace lo mismo y más — un skill puede ser invocado por slash command (`/nombre`) **y** activarse automáticamente por Claude según la descripción.

Si llegas a un repo con `.claude/commands/`, no hay que migrar de urgencia. Pero los skills nuevos van al formato nuevo.

---

## Estructura del directorio

La estructura estándar de un skill, con los directorios opcionales:

```
.claude/skills/dotnet-controller/
├── SKILL.md           # Núcleo del skill: instrucciones que carga Claude
├── scripts/           # Scripts ejecutables (Python, Bash) — opcional
│   └── generate.py
├── references/        # Documentación detallada cargable bajo demanda
│   └── conventions.md
└── assets/            # Plantillas, ficheros binarios, ejemplos
    └── controller.template.cs
```

Solo `SKILL.md` es obligatorio. El resto se añade cuando lo necesitas.

### Cuándo usar cada carpeta

**`scripts/`** — para tareas deterministas. La regla práctica: si una tarea se puede hacer con código en vez de razonamiento, hazla con código. Más fiable, más barato, y más rápido.

Casos típicos:
- *"Cuenta cuántos endpoints tiene el controller X"* — script Python que parsea el fichero, no razonamiento del modelo.
- *"Genera un GUID v4 para este nuevo registro"* — script Bash de una línea.
- *"Valida que este JSON cumple con el schema X"* — script de validación.

Ventaja añadida: los scripts se ejecutan vía `Bash` y solo el output llega al contexto. El código del script no consume tokens.

**`references/`** — para documentación detallada que solo se carga cuando el skill realmente necesita esa información concreta. Aquí van convenciones extensas, especificaciones largas, manuales de API.

La idea: que el `SKILL.md` se mantenga ligero (instrucciones generales) y la información gorda viva aparte. Por ejemplo, un skill de generación de controllers puede tener un `references/error-handling-patterns.md` con la guía completa de cómo se manejan errores en el equipo. El `SKILL.md` solo dice *"para errores, consulta `references/error-handling-patterns.md`"*. Claude solo lee ese fichero si la tarea concreta toca manejo de errores.

**`assets/`** — para plantillas y ficheros binarios. Plantillas `.cs` de un controller, ficheros `.html` base, imágenes que el skill necesita.

Diferencia con `references/`: `references/` se lee como contexto. `assets/` se usa como punto de partida o material de copiado. Una plantilla de controller en `assets/` la lee Claude para copiar la estructura, pero no para razonar sobre ella.

La regla práctica de Anthropic: el cuerpo del `SKILL.md` por debajo de **1.500-2.000 palabras**. Si crece más, parte el contenido en ficheros `references/` y haz que `SKILL.md` apunte ahí. Esto no es estética — es rendimiento. Un `SKILL.md` muy largo se carga entero al activarse el skill, ocupando contexto que podrías necesitar para tu código.

---

## Reglas técnicas críticas

Antes de escribir el primer skill, conviene tener claras unas reglas que **no son negociables** y que a la gente nueva les hace tropezar a menudo. La guía oficial de Anthropic las marca como reglas duras y son las que más errores triviales evitan.

### El nombre del fichero

`SKILL.md` se llama exactamente así. **Es case-sensitive**. Variaciones como `SKILL.MD`, `Skill.md`, `skill.md` o `skills.md` no funcionan — Claude no los reconoce como skill. Una `S` mayúscula equivocada y el skill simplemente no existe a ojos del agente.

Si has visto que tu skill no se activa nunca, lo primero que toca comprobar es esto. Más de una vez la causa es una `M` minúscula en `.MD`.

### El nombre del directorio

El nombre de la carpeta del skill **es el nombre del skill**. Y tiene formato obligatorio:

- **kebab-case**: minúsculas, palabras separadas por guiones.
- Sin espacios.
- Sin guiones bajos.
- Sin mayúsculas.

```
✅ dotnet-controller
✅ angular-component
✅ pr-checklist

❌ DotnetController       (capitales)
❌ dotnet_controller      (guion bajo)
❌ dotnet controller      (espacio)
❌ DotnetCONTROLLER       (mezcla)
```

Y un detalle que pilla a algunos: el `name` del frontmatter YAML debe coincidir con el nombre del directorio. Si la carpeta se llama `dotnet-controller` pero el frontmatter pone `name: DotnetController`, hay inconsistencia y comportamiento impredecible.

### Nada de `README.md` dentro del skill

El skill se documenta entero dentro de su propio `SKILL.md` y, si hace falta más, en `references/`. **No se mete un `README.md`** dentro de la carpeta del skill — Claude no lo va a leer como parte del skill y solo añade ruido.

Hay una excepción que sí tiene sentido: cuando publicas tu skill como repo en GitHub para que humanos lo encuentren y lo instalen, conviene tener un `README.md` **a nivel de repo**, fuera de la carpeta del skill. Ese `README.md` es para personas que llegan al repo, no para Claude. Pero dentro de la carpeta del skill, nunca.

### Nombres reservados

`name` no puede empezar por `claude` ni por `anthropic`. Estos prefijos están reservados para Anthropic. Si lo intentas, el skill no se carga.

```
❌ claude-helper
❌ anthropic-tools
✅ team-claude-helper       (claude no como prefijo)
✅ mi-anthropic-utils        (idem)
```

### Sin XML en frontmatter

Esto es una restricción de seguridad: el frontmatter YAML **no puede contener etiquetas XML** (`<` `>`). La razón es que el frontmatter se inyecta literal en el system prompt del modelo, y permitir XML abriría la puerta a inyecciones de instrucciones disimuladas.

```yaml
# ❌ MAL — XML tags en description
description: "Procesa <input> y devuelve <output>"

# ✅ BIEN — sin XML
description: "Procesa la entrada del usuario y devuelve la respuesta procesada"
```

Si tu skill necesita hablar de XML en sus instrucciones (porque genera HTML, por ejemplo), eso va en el **cuerpo** del `SKILL.md`, no en el frontmatter.

### Límite de la descripción

El campo `description` está limitado a **1024 caracteres**. Es bastante — caben tres o cuatro frases largas — pero hay que conocer el límite. Si te pasas, el skill no se carga.

La mayoría de descripciones bien escritas caben en 200-400 caracteres. Si la tuya pasa de 800, probablemente tienes contenido de cuerpo metido en la descripción y conviene replantearla.

### Resumen de las reglas duras

```
✅ SKILL.md (case-sensitive, exactamente así)
✅ Nombre de carpeta en kebab-case, igual que el name del frontmatter
✅ name no empieza por "claude" ni por "anthropic"
✅ Sin XML tags en el frontmatter
✅ description bajo 1024 caracteres
❌ NO meter README.md dentro de la carpeta del skill
```

Estas son del orden de "si te las saltas, el skill no funciona". Conviene tenerlas en mente desde el principio porque son la fuente número uno de *"escribí el skill, parece correcto, pero no se activa nunca"*.

---

## El frontmatter YAML

`SKILL.md` se compone de dos partes:

1. **Frontmatter YAML** entre marcadores `---` arriba del fichero. La metadata.
2. **Cuerpo Markdown** debajo. Las instrucciones que Claude sigue cuando ejecuta el skill.

El frontmatter mínimo:

```yaml
---
name: dotnet-controller
description: Genera controllers ASP.NET Core siguiendo las convenciones del equipo. Usar cuando el usuario pida crear un nuevo endpoint, controller, o se necesite scaffolding de un recurso REST.
---
```

Solo dos campos son obligatorios: `name` y `description`. Lo demás es opcional pero útil.

### Los campos que conviene conocer

| Campo | Para qué sirve |
|---|---|
| `name` | Nombre del skill. Lowercase, números y guiones. Máximo 64 caracteres. |
| `description` | Lo que hace el skill y cuándo activarlo. **El campo más importante.** Bajo 1024 caracteres. |
| `allowed-tools` | Lista de herramientas que el skill puede usar. Si la omites, hereda los permisos de la sesión. |
| `disable-model-invocation` | Si es `true`, el skill solo se invoca explícitamente por el usuario (`/nombre`). Útil para skills caros o destructivos. |
| `model` | Modelo específico para este skill. Útil si quieres que un skill pesado use Opus aunque la sesión esté en Sonnet. |
| `argument-hint` | Pista visual sobre qué argumentos espera el skill cuando se invoca por slash command. |
| `context` | `fork` para ejecutar en contexto aislado. Lo veremos en el módulo 3. |
| `license` | Licencia del skill cuando se publica como open source (MIT, Apache-2.0, etc.). Opcional. |
| `compatibility` | Indica requisitos de entorno: producto destino, paquetes necesarios, acceso a red, etc. Cadena de 1 a 500 caracteres. Útil para skills que dependen de algo concreto del sistema o de un MCP server específico. |
| `metadata` | Pares clave-valor personalizados. Lo más típico: `author`, `version`, `mcp-server`. No afecta a la activación, sirve para versionado y trazabilidad. |

### Tres ejemplos completos comparativos

**Ejemplo 1: skill simple, generador de componentes.**

```yaml
---
name: angular-component
description: Genera componentes Angular standalone con Signals siguiendo la estructura del equipo. Usar cuando el usuario pida crear un nuevo componente, haga referencia a un componente nuevo en una feature, o cuando el flujo requiera scaffolding de UI Angular.
---
```

Mínimo absoluto. Sin restricción de tools, hereda todo de la sesión. Activación automática por descripción.

**Ejemplo 2: skill de revisión, con tools restringidas.**

```yaml
---
name: dotnet-review
description: Revisa código C# / .NET buscando problemas de naming, patrones async incorrectos, manejo de errores y convenciones del equipo. Usar cuando el usuario pida revisar, auditar o validar código .NET antes de un commit o PR.
allowed-tools: Read, Grep, Glob
---
```

Solo lectura. Aunque la sesión tenga permisos de escritura, este skill no puede modificar nada — solo leer y analizar. Para code review esto es lo correcto.

**Ejemplo 3: skill destructivo, solo invocación explícita.**

```yaml
---
name: db-reset
description: Resetea la base de datos local borrando todas las tablas y reaplicando migraciones desde cero. Usar SOLO cuando el usuario pida explícitamente resetear la BBDD local.
allowed-tools: Bash(dotnet ef *), Bash(rm -rf *.db)
disable-model-invocation: true
---
```

Aquí varios mecanismos de seguridad: la descripción dice *"SOLO cuando el usuario lo pida explícitamente"*, `disable-model-invocation: true` impide que Claude active este skill por su cuenta, y `allowed-tools` está acotado al mínimo necesario. Para skills destructivos esto es lo que toca.

Tras el frontmatter va el cuerpo en Markdown — las instrucciones reales que Claude va a seguir cuando el skill se active. Lo veremos en el siguiente apartado cuando escribamos uno desde cero.

---

## La descripción es el switch

Si te tienes que llevar una sola idea de este apartado, que sea esta: **la descripción es lo que decide si tu skill se activa o no**. No el nombre. No el contenido. La descripción.

Cuando arrancas una sesión de Claude Code, el agente carga al sistema solo dos cosas de cada skill instalado: el nombre y la descripción. Eso es. Cuando tú escribes una petición, Claude mira las descripciones disponibles y decide cuál coincide con lo que pides. Si ninguna coincide, no usa skill. Si una coincide, carga el `SKILL.md` completo y ejecuta.

Por eso una descripción mal escrita puede dejar tu skill **invisible** — instalado, ahí, pero nunca se activa porque Claude no sabe cuándo usarlo. Y este es, sin duda, el problema número uno de los principiantes con skills: escriben skills funcionalmente correctos que nunca se activan, y concluyen que *"esto no funciona"*.

### Anti-patrones de descripción

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

### La fórmula para una buena descripción

Tres ingredientes:

1. **Qué hace** — el verbo de acción concreto.
2. **Cuándo usarlo** — los disparadores lingüísticos. *"Usar cuando el usuario pida X, mencione Y, o necesite Z"*.
3. **Tercera persona, no imperativo.** Anthropic recomienda *"Este skill genera..."* / *"Should be used when..."*, no *"Genera..."* / *"Use when..."*. La razón es técnica: la descripción se inyecta en el system prompt y la consistencia de punto de vista mejora la activación.

Ejemplo de la misma capacidad con descripción mala y buena:

```yaml
# MAL — vaga, sin triggers
description: Genera componentes Angular
```

```yaml
# BIEN — concreta, con casos de uso
description: Genera componentes Angular standalone con Signals siguiendo la
  estructura del equipo. Usar cuando el usuario pida crear un nuevo componente,
  haga referencia a un componente nuevo en una feature, o cuando el flujo
  requiera scaffolding de UI Angular.
```

La segunda activa fiablemente cuando el usuario dice cosas como *"crea un componente para el listado de pedidos"*, *"necesito un componente OrdersListComponent"* o *"vamos a hacer la UI del filtro"*. La primera puede activarse o no, según los humores del modelo.

### Casos donde la descripción "casi funciona"

A veces las descripciones se activan a medias — funcionan en el 70% de los casos pero fallan en el 30%. Conviene saber detectar esto:

**Caso A: trigger demasiado específico.** *"Usar cuando el usuario diga 'genera un controller'"*. Activa cuando dice esa frase exacta. No activa cuando dice *"crea un controller"* o *"añade un nuevo controller"*. La solución: variar el vocabulario en la descripción. *"Usar cuando el usuario pida generar, crear o añadir un controller, o use sinónimos como endpoint o resource handler"*.

**Caso B: trigger ambiguo entre dos skills.** Tienes un skill `dotnet-controller` y otro `dotnet-review`. Ambos hablan de *"código .NET"*. Cuando el usuario dice *"revisa este controller"*, ¿cuál se activa? Depende. Si los dos pueden, Claude probablemente activa el más específico al verbo *"revisar"*, pero no es garantía. La solución: descripciones más distintivas. El de generación menciona *"genera, crea, scaffolding, nuevo"*. El de revisión menciona *"revisa, audita, valida, antes de commit"*.

**Caso C: contexto del proyecto que se da por hecho.** *"Genera componentes siguiendo nuestra arquitectura"*. ¿Qué arquitectura? Claude no la conoce hasta que carga el cuerpo del skill. Y la decisión de cargar el skill se toma con la descripción. Si la descripción asume contexto que no está, falla. Solución: en la descripción, ser explícito sobre qué stack y qué patrón. *"Genera componentes Angular standalone con Signals para arquitectura signal-based store"*.

### Cómo iterar una descripción

La forma más práctica de afinar una descripción es usar el skill y observar:

1. Escribes el skill con tu primera descripción.
2. Lanzas peticiones que esperarías que lo activaran.
3. Después de cada petición, le preguntas explícitamente al agente: *"¿qué skill has usado?"*. El agente te dice, sin filtrar.
4. Si no se activó cuando esperabas, refinas la descripción añadiendo variaciones del vocabulario que usaste.
5. Si se activó cuando NO esperabas, restringes el alcance.

Este proceso es no determinista — la activación de skills tiene componente probabilístico. La meta no es 100%, es que sea fiable cuando importa.

---

## Progressive disclosure: la arquitectura que hace que esto funcione a escala

Esto es lo que diferencia un skill de un prompt largo que metes en `CLAUDE.md`. **Los skills se cargan en tres niveles**, no de una vez.

### Nivel 1 — Metadatos (siempre cargados)

Solo `name` + `description`. Aproximadamente 100 tokens por skill instalado. Esto vive en el system prompt de Claude desde que arrancas la sesión.

Implicación práctica: puedes tener **decenas** de skills instalados sin que se te coma el contexto. Cada skill que añadas suma ~100 tokens. Negligible.

### Nivel 2 — Instrucciones (cargadas al activarse)

Cuando Claude decide que un skill aplica a la tarea actual, lee el cuerpo de `SKILL.md`. Aquí entran las instrucciones detalladas, los workflows, las reglas. La recomendación oficial: por debajo de 5.000 tokens (~1.500-2.000 palabras).

### Nivel 3 — Recursos profundos (cargados bajo demanda)

Si el skill tiene `references/` o `assets/`, esos ficheros NO se cargan automáticamente al activarse el skill. Solo cuando el cuerpo de `SKILL.md` los referencia explícitamente y Claude los lee con la herramienta correspondiente.

Esto te permite tener manuales enteros de convenciones, especificaciones largas o catálogos detallados como parte de tu skill, sin que ocupen contexto a no ser que se necesiten para la tarea concreta.

### Por qué importa: las matemáticas

Imagina que tu equipo tiene 30 skills instalados — convenciones de .NET, convenciones de Angular, generadores de varios componentes, varios checklists de revisión, plantillas para documentación, etc.

**Sin progressive disclosure** (todos los skills cargados siempre):

- 30 skills × ~2.000 tokens cada uno (cuerpo medio) = **60.000 tokens** de overhead
- Sobre una ventana de Sonnet de 200.000 tokens, esto es el 30% comido antes de empezar.
- Resultado: contexto saturado, sesiones cortas, comportamiento pobre.

**Con progressive disclosure** (lo que tenemos):

- 30 skills × ~100 tokens metadata = **3.000 tokens** de overhead total
- Cuando uno se activa, suma sus ~2.000 tokens. Total: ~5.000 tokens de overhead efectivo.
- Sobre la misma ventana de 200.000, esto es el 2.5%.

Esa es la diferencia entre poder tener 30 skills sin penalización y tener que andar eligiendo cuáles instalar para no saturar la sesión. Esta es la razón por la que tener muchos skills instalados no penaliza, mientras que tener un `CLAUDE.md` enorme sí lo hace.

---

## Mentalidad: cómo identificar candidatos a skill

Esta sección es donde se decide si tu kit de skills va a ser útil o solo un montón de ficheros bonitos. **No todo merece ser un skill**.

### Tres preguntas para decidir si algo es candidato a skill

**1. ¿Es un patrón que se repite, o es un caso puntual?**

Si haces algo una vez al mes, no es skill — es una conversación con Claude Code cuando toque. Si lo haces tres veces a la semana, sí. La frecuencia justifica el coste de definir un skill (escribirlo, mantenerlo, documentarlo).

**2. ¿Tiene reglas no obvias que el agente no deduciría solo?**

Si tu equipo tiene una convención específica que no está en el código (porque es nuevo, porque está mezclada, porque tiene excepciones), un skill captura esas reglas. Si el agente puede hacer el trabajo igual de bien sin el skill, no lo necesitas.

Test rápido: pídele al agente la tarea sin skill. Si lo hace bien, no necesitas skill. Si comete errores que tendrías que corregir cada vez (siempre los mismos), eso es lo que va al skill.

**3. ¿El output es predecible o varía mucho?**

Skills brillan en tareas con output relativamente predecible — generar un controller, escribir un test, formatear un commit message. Tareas creativas o de criterio (diseñar arquitectura, decidir trade-offs) no son buen candidato para skill, son conversación.

### Patrones típicos que SÍ merecen skill

- Generación de boilerplate con convenciones del equipo (controllers, componentes, DTOs).
- Code review con checklist específico del equipo.
- Generación de tests siguiendo el patrón establecido (xUnit + NSubstitute, ng-test, etc.).
- Mensajes de commit con formato de equipo (semantic, con referencia a issue, con cuerpo estructurado).
- Documentación de funciones/clases con el formato del equipo (XML docs, JSDoc, estilo concreto).
- Generación de migraciones con naming convention.
- Setup inicial de features (carpeta + ficheros + tests + entrada en routing).

### Patrones que NO merecen skill

- *"Explícame qué hace este código"*. Eso es conversación.
- *"Optimiza este algoritmo"*. Eso es razonamiento de criterio.
- *"Decide qué arquitectura usar"*. Eso es discusión.
- *"Refactoriza este módulo"*. Lo es solo si tu equipo tiene un patrón muy específico de refactor; si no, es conversación normal.
- *"Resuelve este bug"*. Es debugging, conversación.

La heurística: **si la respuesta correcta depende del criterio, no es skill. Si la respuesta correcta es seguir un patrón, sí.**

---

## SKILL.md vs CLAUDE.md vs AGENTS.md

Cierre del apartado con la decisión que más confunde a la gente que llega a esto. Tres ficheros con propósitos distintos:

| Fichero | Cuándo se carga | Para qué sirve |
|---|---|---|
| `CLAUDE.md` | Siempre, al arrancar sesión | Contexto del proyecto que aplica a *todo* lo que hagas en él |
| `AGENTS.md` | Siempre, al arrancar sesión | Lo mismo que `CLAUDE.md` pero como estándar cross-tool |
| `SKILL.md` | Solo cuando el skill se activa | Capacidad puntual que aplica a *ciertas tareas* concretas |

Árbol de decisión rápido:

- **¿Es información que el agente necesita siempre que toques este repo?** (estructura, convenciones generales, comandos clave) → `CLAUDE.md`.
- **¿Es información que el agente solo necesita en ciertas tareas?** (cómo generar un controller, cómo revisar un PR, cómo escribir un test concreto) → skill.
- **¿Quieres que viaje contigo a otros proyectos?** → skill personal en `~/.claude/skills/`.
- **¿Es para tu equipo?** → skill de proyecto en `.claude/skills/`, va a git.

Y lo que **no** debes hacer: meter en `CLAUDE.md` todo lo que se te ocurra "por si acaso". Cada cosa que metas ahí pesa en cada sesión, para cualquier tarea, para cualquier persona. Si una convención solo aplica a una de cada cinco tareas, no debería estar en `CLAUDE.md` — debería ser un skill.

### Casos prácticos del árbol de decisión

**Caso 1: convención de naming de variables.** Aplica a todo el código. Va en `CLAUDE.md`.

**Caso 2: forma estándar de generar un endpoint nuevo.** Solo aplica cuando generas endpoints. Skill.

**Caso 3: comando para arrancar el dev environment.** Aplica siempre que trabajes en el repo. `CLAUDE.md`.

**Caso 4: checklist de seguridad antes de un PR.** Solo aplica antes de PRs. Skill.

**Caso 5: tu preferencia personal de comentar el código en español.** Es tuya, va contigo a otros proyectos. Skill personal.

**Caso 6: estructura de carpetas del proyecto.** Es del proyecto, aplica siempre. `CLAUDE.md`.

---

## Errores frecuentes con tus primeros skills

Lista de los anti-patrones que casi todo el mundo comete con sus primeros skills:

- **Descripción genérica.** *"Genera código"* no activa nada. Sé específico, con verbos y casos de uso.
- **Skills que duplican lo que ya hace Claude solo.** Si el agente sin skill ya genera tests xUnit decentes, un skill genérico de tests xUnit no aporta. El valor está en codificar las **particularidades de tu equipo**, no las prácticas generales.
- **Cuerpo del skill demasiado largo.** Si el `SKILL.md` pasa de 2.000 palabras, parte en `references/`. Si no, ocupa contexto innecesariamente.
- **Skills sin testar.** Escribes el skill, asumes que funciona. Lánzalo en una conversación y verifica que se activa cuando esperas y hace lo que esperas.
- **Mismo skill en proyecto y user.** Si lo tienes en `.claude/skills/` y en `~/.claude/skills/` con el mismo nombre, hay conflicto. Decide dónde vive y bórralo del otro.
- **Skills que deberían ser CLAUDE.md.** Si tu skill aplica a *todas* las tareas del repo, no es un skill — es contenido de `CLAUDE.md` que has puesto en el sitio equivocado.
- **Skills sin scope claro.** Un skill que hace tres cosas distintas activará mal. Mejor tres skills pequeños que uno gordo.
- **No iterar la descripción.** La primera descripción casi nunca es la final. Itera al menos dos veces antes de darla por buena.

---

## Antes de seguir

Ya tienes el modelo conceptual de un skill: directorio, frontmatter, descripción como switch, progressive disclosure en tres niveles, cuándo elegir skill frente a `CLAUDE.md`, y los criterios para decidir si algo merece ser skill.

En el siguiente apartado escribimos uno desde cero. El de ejemplo va a ser un skill que tu equipo va a usar a diario: un **generador de componentes Angular standalone con la estructura estándar**. Empezamos por la versión más simple — un solo `SKILL.md` — y vamos añadiendo capas (scripts, plantillas) hasta dejarlo a nivel producción.

Antes de pasar, dos preguntas que conviene tener pensadas:

**Primera:** ¿qué descripción le pondrías a ese skill? *"Cuándo activarlo, qué hace, qué espera de mí"*. Esa es la pieza que más vamos a iterar.

**Segunda:** ¿qué patrón tiene tu equipo que sería el segundo skill, después del generador de componentes? El generador de componentes vamos a hacerlo juntos como ejemplo guiado. El segundo lo elegirás tú — y será el que más rentabilidad te dé porque será específico a tu trabajo real.

Tener una idea clara de esos dos skills antes de la siguiente sesión hace que el trabajo práctico sea mucho más efectivo. No estás aprendiendo el concepto en abstracto: lo estás aplicando a algo que ya sabes que vale la pena.
