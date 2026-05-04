> **Versión:** v3 | **Módulo:** 2 | **Sub:** 2.2c | **Slides:** 26 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M02-S2.2c-control-scopes-cierre-v3.md`

# Submódulo 2.2c — Control, scopes y cierre

## Slide 1 — Portada
**Módulo 2 · Submódulo 2.2 · Parte C**
Control de invocación, scopes y cierre del 2.2
Decisiones operativas sobre cómo se usa cada skill

---

## Slide 2 — Dónde estamos

En las partes A y B del 2.2 has construido un skill desde cero en cuatro versiones progresivas, has visto cómo extraer plantillas a `assets/` y lógica determinista a scripts.

Ahora pasamos a **decisiones operativas** que ya no son sobre el contenido del skill:

```
1. Control de invocación
   → Cómo y cuándo se activa

2. Subagentes en skills (referencia rápida)
   → Para skills que necesitan contexto aislado

3. Scopes
   → Dónde vive cada skill

4. Errores frecuentes con tus primeros skills

5. Cierre del módulo 2.2
```

---

## Slide 3 — Control de invocación

Aparte de la activación automática por descripción, los skills tienen mecanismos para controlar cómo se invocan.

**Tres piezas:**

```
1. disable-model-invocation
   └── Impide que Claude active el skill por su cuenta

2. argument-hint
   └── Pista visual sobre qué argumentos espera

3. Slash command de skill
   └── Invocación explícita: /nombre-del-skill
```

Las vemos.

---

## Slide 4 — disable-model-invocation: la regla

```yaml
---
name: db-reset
description: Resetea la BBDD local borrando datos
  y reaplicando migraciones.
disable-model-invocation: true
---
```

```
Cuando esto es true:
└── El skill SOLO se ejecuta si el usuario lo invoca
    explícitamente con /db-reset.

Claude NO lo activa por su cuenta
aunque la descripción coincida con la petición.
```

---

## Slide 5 — disable-model-invocation: cuándo usar

**Tres casos donde esto tiene sentido:**

```
SKILLS DESTRUCTIVOS
├── Borrar BBDD
├── Hacer deploy
├── Push forzado
├── Eliminar ficheros
└── Cosas que no quieres que pase nunca
    por accidente.

SKILLS CAROS
└── Si el skill consume mucho contexto
    o lanza tareas largas
    └── Mejor que sea siempre intencional.

SKILLS EXPERIMENTALES
└── Mientras pruebas un skill nuevo:
    └── Puede ayudar tenerlo solo invocable
        explícitamente.
        Para que no se active en sitios inesperados.
```

---

## Slide 6 — argument-hint

```yaml
---
name: angular-component
description: Genera un componente Angular...
argument-hint: <nombre-del-componente>
---
```

```
Solo afecta a la invocación por slash command.
```

```
Cuando el usuario escribe /angular-component
└── Le aparece la pista
    de que espera un argumento.

Útil para skills que tienen
un parámetro principal claro.
```

> No es funcional — el skill funciona igual con o sin él.
> Es ayuda visual.

---

## Slide 7 — Slash command de skill

```
Todo skill que esté user-invocable
(que es el default)
└── Se puede invocar también con slash command.
```

```
Si tu skill se llama angular-component:

  /angular-component crear orders-list

Lo activa explícitamente con ese argumento.
```

---

## Slide 8 — Cuándo usar invocación por slash en lugar de dejar que el agente decida

```
CUANDO QUIERES SER EXPLÍCITO
└── Sabes que necesitas ese skill concreto.
    No quieres jugar a la lotería
    de la activación automática.

CUANDO LA ACTIVACIÓN AUTOMÁTICA NO ES FIABLE
└── Si tu descripción no acaba de afinar
    └── Y prefieres invocar a mano
        mientras la iteras.

CUANDO EL SKILL ES UNO DE VARIOS SIMILARES
└── Tienes angular-component y angular-page
    └── Y quieres asegurarte
        de invocar el correcto.
```

---

## Slide 9 — Subagentes en skills (referencia rápida)

```yaml
---
name: dotnet-deep-review
description: Revisa exhaustivamente un módulo .NET
  buscando problemas profundos
context: fork
---
```

```
context: fork
└── Hace que el skill se ejecute en un CONTEXTO AISLADO.

    ├── Tiene su propia ventana de contexto
    ├── Su propio razonamiento
    └── Devuelve el resultado al agente principal
        sin contaminar.
```

---

## Slide 10 — Subagentes: cuándo es útil

```
EL SKILL NECESITA EXPLORAR MUCHO CONTENIDO
├── Leer un módulo entero
├── Analizar muchos ficheros
└── Y NO quieres que ese contenido pese
    en tu sesión principal.

EL SKILL HACE UNA TAREA CON SUS PROPIAS DECISIONES
└── Que prefieres que NO influyan
    en lo que estás haciendo en paralelo.
```

> El deep dive de subagentes es el módulo 3 completo.
> Aquí solo introducimos la sintaxis para que sepas que existe.
>
> En 3.1 lo veremos con detalle.

---

## Slide 11 — Scopes: dónde vive cada skill

Tres ubicaciones, cada una con su lógica:

```
1. Personal      → ~/.claude/skills/
2. Proyecto      → .claude/skills/
3. Plugin        → empaquetados, distribuibles
```

Las vemos.

---

## Slide 12 — Personal: ~/.claude/skills/

**Tus skills personales. Viajan contigo de proyecto en proyecto.**

```
Aquí van:

CONVENCIONES QUE APLICAS SIEMPRE
EN CUALQUIER PROYECTO
├── "Comenta el código en español"
└── "Explica los conceptos como a un junior"

SKILLS DE PRODUCTIVIDAD PERSONAL
├── "Escribe el commit como yo lo escribiría"
└── "Resume este PR en lenguaje claro"

EXPERIMENTOS
└── Antes de promoverlos al equipo.
```

---

## Slide 13 — Proyecto: .claude/skills/

**Skills del equipo. Van a git. Se comparten al clonar.**

```
Aquí van:

CONVENCIONES ESPECÍFICAS DEL PROYECTO
└── Generadores con la estructura del equipo.

CODE REVIEWS CON EL CHECKLIST DEL EQUIPO

CUALQUIER SKILL QUE APLICA A TODO EL QUE TRABAJE
EN ESTE REPO
```

> La mayoría de skills útiles para un equipo
> viven aquí.

---

## Slide 14 — Plugin

**Empaquetados dentro de un plugin distribuible.**

```
Esto lo veremos en 2.3
└── Cuando hablemos de cómo distribuir
    un kit completo de skills + MCP servers
    como un paquete.
```

> Por ahora basta con saber que existe esta opción
> para distribución avanzada.

---

## Slide 15 — Cómo decidir entre personal y proyecto

```
¿APLICA SOLO A ESTE PROYECTO?
└── Proyecto.

¿APLICA A VARIOS PROYECTOS DEL MISMO CLIENTE?
└── Proyecto pero copiado en cada uno
    └── O plugin si son muchos.

¿APLICA A TU TRABAJO EN GENERAL?
└── Personal.

¿ES ALGO QUE UN COMPAÑERO DEL EQUIPO
SE BENEFICIARÍA DE TENER?
└── Proyecto.
    Va a git.
```

---

## Slide 16 — Patrón típico: empezar personal, promover a proyecto

```
Cuando descubres una nueva forma de hacer algo
con Claude Code

└── Lo natural es empezar el skill
    en ~/.claude/skills/
    └── Para experimentar sin afectar a nadie.

Cuando ves que funciona
y sería útil para el equipo

└── Lo mueves a .claude/skills/
    └── Y lo commiteas.
```

> Esta progresión personal → proyecto da espacio
> para iterar sin presionar al equipo
> con experimentos a medias.

---

## Slide 17 — Errores frecuentes con tus primeros skills (1/3)

Lista de los anti-patrones que casi todo el mundo comete con sus primeros skills:

```
❌ SKILL DEMASIADO GRANDE
   El típico "un skill que hace todo lo de generación
   de componentes, páginas, módulos, services..."
   └── Mejor varios skills pequeños y especializados
       que uno gordo.
       Activan mejor y son más fáciles de mantener.

❌ EMPEZAR POR LA VERSIÓN 4
   No hace falta meter scripts y plantillas el primer día.
   └── Empieza con un SKILL.md simple
       Úsalo, ve qué falla
       Y añade capas según se justifiquen.
       └── La sobreingeniería es el primer enemigo.

❌ NO ITERAR LA DESCRIPCIÓN
   Tu primera descripción casi nunca es la final.
   └── Lánzala, ve cuándo se activa y cuándo no
       Ajusta.
       La activación es probabilística.
```

---

## Slide 18 — Errores frecuentes (2/3)

```
❌ NO TESTAR DESPUÉS DE CAMBIOS
   Tras modificar un skill, lánzalo en una sesión nueva
   y verifica que sigue activando como esperas.
   └── Es fácil romper la activación al refinar.

❌ CONVENCIONES QUE DUPLICAN LO QUE YA HACE CLAUDE
   Si Claude sin skill ya genera componentes Angular
   standalone bien, un skill que solo dice
   "genera componentes Angular standalone" no aporta.
   └── El valor está en codificar
       las PARTICULARIDADES de tu equipo
       no las prácticas generales.

❌ MEZCLAR SKILLS QUE DEBERÍAN ESTAR EN CLAUDE.md
   Si tu skill aplica a todas las tareas del repo
   (estructura de carpetas, comandos de build)
   no es skill — va a CLAUDE.md.
   └── Skills son para tareas concretas.
```

---

## Slide 19 — Errores frecuentes (3/3)

```
❌ NO DOCUMENTAR DENTRO DEL SKILL POR QUÉ SE HACEN LAS COSAS
   Cuando otro miembro del equipo
   (o tú dentro de seis meses)
   lo lea, va a querer saber
   por qué hay esa convención.
   └── Un comentario corto en el SKILL.md
       justificando decisiones no obvias
       se agradece.

❌ MEZCLAR LÓGICA DETERMINISTA CON RAZONAMIENTO DEL MODELO
   Si un script puede hacerlo bien, no se lo pidas al modelo.
   Y al revés también:
   no metas en script lo que requiere criterio.
```

---

## Slide 20 — Antes de seguir: lo que tienes ahora

Has construido un skill desde cero en cuatro versiones progresivas:

```
✅ El mínimo (versión 1)
✅ Con convenciones del equipo (versión 2)
✅ Con plantillas en assets/ (versión 3)
✅ Con script ejecutable (versión 4)
```

**Has visto:**

```
✅ Los mecanismos de control de invocación
✅ Dónde vive cada tipo de skill (3 scopes)
✅ El patrón típico personal → proyecto
✅ Los anti-patrones más comunes
```

---

## Slide 21 — Lo que viene en 2.3

En el siguiente apartado salimos del taller individual y miramos al **ecosistema**.

```
Hay muchos skills ya escritos
├── Por Anthropic
└── Por la comunidad

Hay formas de empaquetarlos y distribuirlos
└── Como plugins.

Y hay consideraciones de seguridad importantes:
└── Un skill de un tercero con permisos amplios
    no es algo que metas en tu repo a la ligera.
```

---

## Slide 22 — Lo que cubre 2.3

```
SUBMÓDULO 2.3 — ECOSISTEMA Y DISTRIBUCIÓN
─────────────────────────────────────────────────────

Lo que viene de serie: bundled skills

Skills oficiales de Anthropic
├── frontend-design
├── simplify
└── docx, pdf, pptx, xlsx

El comando npx skills add

Skills de la comunidad
├── Antigravity Awesome Skills
├── Vercel Labs agent-skills
├── Superpowers
├── awesome-agent-skills
└── aitmpl.com

Plugins y bundling

Seguridad: el lado oscuro del ecosistema
├── El estudio Snyk: ToxicSkills
├── Tipos de problemas
├── Principio de mínimo privilegio
└── Caso real
```

---

## Slide 23 — Una pregunta antes de cerrar el 2.2

Antes de pasar, una pregunta:

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   ¿Qué skill que has hecho en los últimos 60 minutos     │
│   podría dar el salto                                    │
│   de "experimento personal"                              │
│   a "skill del equipo"?                                  │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

```
Si la respuesta es "el de generación de componentes":
└── Perfecto.
    Es justo el patrón que se beneficia
    de codificar las convenciones del equipo.

Si la respuesta es "ninguno todavía,
prefiero practicar más antes":
└── También está bien.
    La promoción a equipo se hace cuando estás seguro
    de que funciona, no por presión de etiquetas.
```

---

## Slide 24 — La parte de aprender se acaba aquí

```
Lo importante es que ya tienes el modelo mental
para construir skills de verdad.
```

```
La parte de APRENDER se acaba aquí.
La parte de PRACTICAR empieza en cuanto vuelvas
a tu repo del trabajo.
```

> En el 2.3 vemos qué hay disponible
> en el ecosistema para no escribir todo tú,
> y cómo distinguir lo que merece la pena instalar
> de lo que mejor dejar fuera.

---

## Slide 25 — Recapitulación del 2.2

Antes de cerrar, repaso de lo que cubrió el submódulo entero:

```
2.2a — BASES Y PRIMER SKILL FUNCIONAL
├── El consejo del PDF: resuelve un caso primero
├── Versión 1: el SKILL.md mínimo
└── Versión 2: añadiendo convenciones del equipo

2.2b — SCRIPTS Y PLANTILLAS
├── Scripts ejecutables (2 sintaxis, 3 casos típicos)
├── Versión 3: plantillas en assets/
└── Versión 4: script ejecutable

2.2c — CONTROL, SCOPES Y CIERRE   ← Aquí
├── Control de invocación
├── Subagentes (referencia rápida)
├── Scopes y cómo decidir
└── Errores frecuentes
```

---

## Slide 26 — Cierre del 2.2

```
✅ Submódulo 2.2 completo.
✅ Tienes el taller para construir skills propios.
✅ Sabes cuándo subir de versión y cuándo no.
✅ Sabes dónde vive cada skill.
✅ Conoces los anti-patrones típicos.
```

**Nos vemos en 2.3 — Ecosistema y distribución.**
