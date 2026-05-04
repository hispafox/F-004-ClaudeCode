> **Versión:** v3 | **Módulo:** 2 | **Sub:** 2.1a | **Slides:** 30 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M02-S2.1a-anatomia-pieza-por-dentro-v3.md`

# Submódulo 2.1a — La pieza por dentro

## Slide 1 — Portada
**Módulo 2 · Submódulo 2.1 · Parte A**
Anatomía de un skill: la pieza por dentro
Directorio, ciclo de vida, estructura y reglas técnicas

---

## Slide 2 — La pregunta del cierre del módulo anterior

Te quedaste con una pregunta al final del módulo 1:

> ¿Qué patrón se repite tres veces a la semana en tu equipo
> y al que tendrías que enseñar a un junior nuevo?

```
"Cuando creas un controller, va con este DTO,
 este validator y este test"

"Cuando tocas el dominio,
 ejecuta este check de invariantes"

"Cuando subes un PR,
 este checklist de seguridad"
```

Cada uno de esos patrones es candidato a skill.

---

## Slide 3 — De asistente genérico a especialista del equipo

Los skills son el mecanismo que convierte a Claude Code:

```
DE
└── un asistente genérico
    ├── que sabe programar
    ├── sabe leer tu repo
    └── sabe ejecutar comandos

A
└── un especialista
    └── que conoce las convenciones de tu equipo

    El que sabe EXACTAMENTE
    cómo se hacen las cosas en vuestro código.
```

---

## Slide 4 — Caso real: tres skills, una semana

He visto a un equipo pasar de:

> *"Claude Code es interesante pero no acaba de cuadrar con cómo trabajamos"*

A:

> *"No sé cómo trabajábamos sin esto"*

```
En una semana.
Después de escribir TRES skills:

1. Generar controllers con sus DTOs
2. Revisar PRs con el checklist interno
3. Escribir mensajes de commit siguiendo
   la convención del equipo
```

> Skills tontos sobre el papel — el tercero literalmente es
> "escribe el commit como yo lo haría".
>
> En agregado, convirtieron al agente en alguien que conocía
> las convenciones del equipo mejor que algunos juniors.

---

## Slide 5 — La unidad mínima

Un skill es, en lo más básico, **un directorio con un fichero `SKILL.md` dentro**.

```
.claude/skills/dotnet-controller/
└── SKILL.md
```

Eso es todo.

```
Si solo metes un SKILL.md en un directorio
con la estructura correcta
└── ya tienes un skill funcional.

El resto:
├── scripts/
├── references/
└── assets/

Son extensiones opcionales que se cargan
cuando hacen falta.
```

> Esta simplicidad es deliberada.

---

## Slide 6 — El ciclo de vida de un skill (1/2)

Para entender por qué un skill es lo que es, ayuda ver el ciclo desde el punto de vista del agente.

```
AL ARRANCAR LA SESIÓN
├── Claude lee la metadata de cada skill instalado
├── Solo el frontmatter, NO el cuerpo
├── Esa metadata se carga en su system prompt
│
└── Ahora sabe que existen, sabe cuándo usarlos
    pero no ha leído sus instrucciones

DURANTE LA CONVERSACIÓN
├── Tú escribes una petición
├── Claude la analiza y mira las descripciones
│
└── Si alguna coincide con lo que pides
    └── Carga el cuerpo de ese skill
        └── Y empieza a aplicarlo
```

---

## Slide 7 — El ciclo de vida de un skill (2/2)

```
DURANTE LA EJECUCIÓN
├── Si el cuerpo del skill referencia ficheros adicionales
│   (references/, scripts, plantillas)
└── Claude los lee solo si los necesita

AL TERMINAR
├── La información del skill se queda en el contexto
│   de la sesión actual
│
└── Para la siguiente sesión, vuelta a empezar
    └── Solo metadata cargada
```

> Este ciclo es lo que hace que tener 30 skills instalados
> no sea un problema:
>
> **solo los que se activan ocupan contexto serio.**

---

## Slide 8 — Dónde viven: tres scopes

Tres scopes, igual que con `settings.json`:

```
PERSONAL
└── ~/.claude/skills/<nombre>/SKILL.md
    ├── Tus skills personales
    ├── Viajan contigo de proyecto en proyecto
    └── Convenciones de naming personales,
        skill de "explica este código en español", etc.

PROYECTO
└── .claude/skills/<nombre>/SKILL.md
    ├── Skills del equipo
    ├── Van a git, se comparten al clonar
    └── La mayoría de skills útiles para un equipo
        viven aquí.

PLUGIN
└── Empaquetados dentro de un plugin distribuible
    └── Lo veremos en 2.3 cuando hablemos
        de cómo distribuir un kit completo.
```

---

## Slide 9 — El nombre del directorio = nombre del skill

El nombre del directorio se convierte en el nombre del skill.

```
.claude/skills/dotnet-controller/SKILL.md
                    ↓
          Skill llamado "dotnet-controller"
                    ↓
       Se invoca con /dotnet-controller
       o autónomamente cuando Claude detecta
       que se necesita
```

> La descripción del frontmatter es lo que decide
> si la activación automática funciona o no.
>
> Lo veremos a fondo en 2.1b.

---

## Slide 10 — Nota importante: la fusión skills + commands

Si has visto tutoriales antiguos que mencionan `.claude/commands/<nombre>.md`:

```
Ese formato sigue funcionando
└── Pero está marcado como LEGACY desde finales de 2025

Anthropic ha unificado custom commands y skills
en un único modelo:
└── .claude/skills/<nombre>/SKILL.md
```

**Hace lo mismo y más:**

```
Un skill puede ser invocado por slash command (/nombre)
└── Y también activarse automáticamente por Claude
    según la descripción.
```

> Si llegas a un repo con .claude/commands/, no hay que migrar de urgencia.
> Pero los skills nuevos van al formato nuevo.

---

## Slide 11 — Estructura del directorio: el árbol completo

La estructura estándar de un skill, con todos los directorios opcionales:

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

> Solo `SKILL.md` es obligatorio.
> El resto se añade cuando lo necesitas.

---

## Slide 12 — Cuándo usar `scripts/`

Para tareas deterministas. **La regla práctica:**

```
Si una tarea se puede hacer con código
en vez de razonamiento
└── HAZLA CON CÓDIGO

Más fiable. Más barato. Más rápido.
```

**Casos típicos:**

```
"Cuenta cuántos endpoints tiene el controller X"
└── Script Python que parsea el fichero
    └── No razonamiento del modelo

"Genera un GUID v4 para este nuevo registro"
└── Script Bash de una línea

"Valida que este JSON cumple con el schema X"
└── Script de validación
```

> Ventaja añadida: los scripts se ejecutan vía Bash
> y solo el output llega al contexto.
>
> El código del script NO consume tokens.

---

## Slide 13 — Cuándo usar `references/`

Para documentación detallada que solo se carga cuando el skill realmente necesita esa información concreta.

```
Aquí van:
├── Convenciones extensas
├── Especificaciones largas
└── Manuales de API

La idea:
├── Que el SKILL.md se mantenga ligero
│   (instrucciones generales)
└── Y la información gorda viva aparte
```

**Ejemplo concreto:**

```
Un skill de generación de controllers puede tener:
└── references/error-handling-patterns.md
    ├── Guía completa de cómo se manejan errores
    └── En el equipo

El SKILL.md solo dice:
└── "Para errores, consulta references/error-handling-patterns.md"

Claude solo lee ese fichero
si la tarea concreta toca manejo de errores.
```

---

## Slide 14 — Cuándo usar `assets/`

Para plantillas y ficheros binarios.

```
Aquí van:
├── Plantillas .cs de un controller
├── Ficheros .html base
└── Imágenes que el skill necesita
```

**Diferencia con `references/`:**

```
references/    → se LEE como contexto

assets/        → se USA como punto de partida
                 o material de copiado

Una plantilla de controller en assets/:
├── La lee Claude para copiar la estructura
└── Pero no para razonar sobre ella
```

---

## Slide 15 — La regla de las 1.500-2.000 palabras

```
La regla práctica de Anthropic:

El cuerpo del SKILL.md
└── por debajo de 1.500-2.000 palabras

Si crece más:
├── Parte el contenido en ficheros references/
└── Haz que SKILL.md apunte ahí
```

> Esto NO es estética. Es rendimiento.
>
> Un SKILL.md muy largo se carga ENTERO al activarse el skill,
> ocupando contexto que podrías necesitar para tu código.

---

## Slide 16 — Reglas técnicas críticas

Antes de escribir el primer skill, conviene tener claras unas reglas que **no son negociables** y que a la gente nueva les hace tropezar a menudo.

```
La guía oficial de Anthropic las marca como reglas duras
└── Son las que más errores triviales evitan
```

**Seis reglas que vamos a ver:**

```
1. El nombre del fichero (case-sensitive)
2. El nombre del directorio (kebab-case)
3. Coherencia carpeta ↔ frontmatter
4. Nada de README.md dentro del skill
5. Nombres reservados (claude, anthropic)
6. Sin XML en el frontmatter
+ Límite de la descripción
```

---

## Slide 17 — Regla 1: el nombre del fichero

```
SKILL.md
└── Se llama EXACTAMENTE así
    └── Es CASE-SENSITIVE
```

**Variaciones que NO funcionan:**

```
❌ SKILL.MD       (extensión en mayúsculas)
❌ Skill.md       (S minúscula en algún sitio)
❌ skill.md       (todo minúsculas)
❌ skills.md      (en plural)
```

> Una `S` mayúscula equivocada y el skill simplemente
> no existe a ojos del agente.

```
Si tu skill no se activa nunca:
└── lo PRIMERO que toca comprobar es esto.

Más de una vez la causa es una `M` minúscula en `.MD`.
```

---

## Slide 18 — Regla 2: el nombre del directorio

El nombre de la carpeta del skill **es el nombre del skill**. Y tiene formato obligatorio:

```
✅ kebab-case: minúsculas, palabras separadas por guiones
✅ Sin espacios
✅ Sin guiones bajos
✅ Sin mayúsculas
```

**Ejemplos:**

```
✅ dotnet-controller
✅ angular-component
✅ pr-checklist

❌ DotnetController       (capitales)
❌ dotnet_controller      (guion bajo)
❌ dotnet controller      (espacio)
❌ DotnetCONTROLLER       (mezcla)
```

---

## Slide 19 — Regla 3: coherencia carpeta ↔ frontmatter

Y un detalle que pilla a algunos:

```
El name del frontmatter YAML
└── debe COINCIDIR con el nombre del directorio
```

**Ejemplo de inconsistencia que falla:**

```
Carpeta:    .claude/skills/dotnet-controller/
Frontmatter: name: DotnetController
                       ↑
                       MAL

Comportamiento impredecible.
```

**Lo correcto:**

```
Carpeta:    .claude/skills/dotnet-controller/
Frontmatter: name: dotnet-controller
                       ↑
                       Coinciden
```

---

## Slide 20 — Regla 4: nada de README.md dentro del skill

```
El skill se documenta entero dentro de su propio SKILL.md
y, si hace falta más, en references/.

NO se mete un README.md dentro de la carpeta del skill.
```

**¿Por qué?**

```
Claude no lo va a leer como parte del skill
└── Solo añade ruido
```

**Excepción que sí tiene sentido:**

```
Cuando publicas tu skill como repo en GitHub
para que humanos lo encuentren y lo instalen
└── Conviene tener un README.md A NIVEL DE REPO
    └── FUERA de la carpeta del skill

Ese README.md es para personas que llegan al repo,
no para Claude.

Pero DENTRO de la carpeta del skill: NUNCA.
```

---

## Slide 21 — Regla 5: nombres reservados

```
name no puede empezar por "claude" ni por "anthropic"
└── Estos prefijos están reservados para Anthropic
```

**Si lo intentas, el skill no se carga:**

```
❌ claude-helper
❌ anthropic-tools

✅ team-claude-helper       (claude no como prefijo)
✅ mi-anthropic-utils        (idem)
```

> Reservados como prefijos. En medio del nombre, sí valen.

---

## Slide 22 — Regla 6: sin XML en frontmatter

Esto es una **restricción de seguridad**:

```
El frontmatter YAML
└── NO puede contener etiquetas XML (< >)
```

**¿Por qué?**

```
El frontmatter se inyecta literal en el system prompt del modelo.

Permitir XML abriría la puerta a inyecciones
de instrucciones disimuladas.
```

**Ejemplo:**

```yaml
# ❌ MAL — XML tags en description
description: "Procesa <input> y devuelve <output>"

# ✅ BIEN — sin XML
description: "Procesa la entrada del usuario
              y devuelve la respuesta procesada"
```

> Si tu skill necesita hablar de XML en sus instrucciones
> (porque genera HTML, por ejemplo)
>
> eso va en el CUERPO del SKILL.md, no en el frontmatter.

---

## Slide 23 — Límite de la descripción

```
El campo description está limitado a 1024 CARACTERES.
```

```
Es bastante:
├── Caben tres o cuatro frases largas
└── Pero hay que conocer el límite

Si te pasas: el skill no se carga.
```

**Calibración práctica:**

```
La mayoría de descripciones bien escritas
└── Caben en 200-400 caracteres

Si la tuya pasa de 800:
└── Probablemente tienes contenido de cuerpo
    metido en la descripción
    └── Conviene replantearla.
```

---

## Slide 24 — Resumen de las reglas duras

```
✅ SKILL.md (case-sensitive, exactamente así)

✅ Nombre de carpeta en kebab-case,
   igual que el name del frontmatter

✅ name no empieza por "claude" ni por "anthropic"

✅ Sin XML tags en el frontmatter

✅ description bajo 1024 caracteres

❌ NO meter README.md dentro de la carpeta del skill
```

> Estas son del orden de
> **"si te las saltas, el skill no funciona"**.
>
> Conviene tenerlas en mente desde el principio porque son
> la fuente número uno de:
>
> *"escribí el skill, parece correcto, pero no se activa nunca"*.

---

## Slide 25 — El frontmatter YAML: estructura

`SKILL.md` se compone de dos partes:

```
1. FRONTMATTER YAML
   └── Entre marcadores --- arriba del fichero
       └── La metadata

2. CUERPO MARKDOWN
   └── Debajo
       └── Las instrucciones que Claude sigue
           cuando ejecuta el skill
```

**El frontmatter mínimo:**

```yaml
---
name: dotnet-controller
description: Genera controllers ASP.NET Core siguiendo las
  convenciones del equipo. Usar cuando el usuario pida crear
  un nuevo endpoint, controller, o se necesite scaffolding
  de un recurso REST.
---
```

> Solo dos campos son obligatorios: `name` y `description`.
> Lo demás es opcional pero útil.

---

## Slide 26 — Los campos del frontmatter

| Campo | Para qué sirve |
|---|---|
| `name` | Nombre del skill. Lowercase, números y guiones. Máx 64 chars. |
| `description` | Lo que hace y cuándo activarlo. **El más importante.** Bajo 1024 chars. |
| `allowed-tools` | Herramientas que el skill puede usar. Si la omites, hereda. |
| `disable-model-invocation` | Si `true`, solo se invoca explícitamente. Skills caros o destructivos. |
| `model` | Modelo específico para este skill. Útil para skills pesados. |
| `argument-hint` | Pista visual sobre qué argumentos espera con slash command. |
| `context` | `fork` para ejecutar en contexto aislado. Lo veremos en módulo 3. |
| `license` | Licencia cuando se publica como open source. Opcional. |
| `compatibility` | Requisitos de entorno. 1-500 caracteres. |
| `metadata` | Pares clave-valor: `author`, `version`, `mcp-server`. |

---

## Slide 27 — Ejemplo 1: skill simple

```yaml
---
name: angular-component
description: Genera componentes Angular standalone con Signals
  siguiendo la estructura del equipo. Usar cuando el usuario
  pida crear un nuevo componente, haga referencia a un
  componente nuevo en una feature, o cuando el flujo requiera
  scaffolding de UI Angular.
---
```

```
Mínimo absoluto.

├── Sin restricción de tools
│   └── Hereda todo de la sesión
│
└── Activación automática por descripción
```

---

## Slide 28 — Ejemplo 2: skill de revisión, con tools restringidas

```yaml
---
name: dotnet-review
description: Revisa código C# / .NET buscando problemas de
  naming, patrones async incorrectos, manejo de errores y
  convenciones del equipo. Usar cuando el usuario pida revisar,
  auditar o validar código .NET antes de un commit o PR.
allowed-tools: Read, Grep, Glob
---
```

```
Solo lectura.

Aunque la sesión tenga permisos de escritura
└── Este skill no puede modificar nada
    └── Solo leer y analizar
```

> Para code review, esto es lo correcto.

---

## Slide 29 — Ejemplo 3: skill destructivo, solo invocación explícita

```yaml
---
name: db-reset
description: Resetea la base de datos local borrando todas las
  tablas y reaplicando migraciones desde cero. Usar SOLO cuando
  el usuario pida explícitamente resetear la BBDD local.
allowed-tools: Bash(dotnet ef *), Bash(rm -rf *.db)
disable-model-invocation: true
---
```

**Aquí varios mecanismos de seguridad:**

```
├── Descripción dice "SOLO cuando el usuario lo pida explícitamente"
├── disable-model-invocation: true
│   └── Impide que Claude active este skill por su cuenta
└── allowed-tools acotado al mínimo necesario
```

> Para skills destructivos, esto es lo que toca.

---

## Slide 30 — Lo que viene en 2.1b

```
SUBMÓDULO 2.1b — LA DESCRIPCIÓN Y LA ARQUITECTURA
─────────────────────────────────────────────────────

La descripción es el switch
├── Anti-patrones de descripción
├── La fórmula de una buena descripción
├── Casos donde "casi funciona"
└── Cómo iterar una descripción

Progressive disclosure: 3 niveles
├── Nivel 1: metadata (siempre cargado)
├── Nivel 2: instrucciones (al activarse)
├── Nivel 3: recursos profundos (bajo demanda)
└── Las matemáticas: 30 skills sin penalización

Mentalidad: cómo identificar candidatos a skill
├── Las 3 preguntas
├── Patrones que SÍ merecen skill
└── Patrones que NO merecen skill

SKILL.md vs CLAUDE.md vs AGENTS.md
└── Árbol de decisión + casos prácticos

Errores frecuentes con tus primeros skills
```

**Nos vemos en 2.1b.**
