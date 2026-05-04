> **Versión:** v3 | **Módulo:** 3 | **Sub:** 3.3a | **Slides:** 30 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M03-S3.3a-hooks-anatomia-eventos-v3.md`

# Submódulo 3.3a — Hooks: anatomía, eventos, handlers, exit codes

## Slide 1 — Portada
**Módulo 3 · Submódulo 3.3 · Parte A**
Hooks: la pieza determinista del harness
Anatomía, eventos del ciclo de vida, handlers, exit codes

---

## Slide 2 — La pieza que cierra el harness

Si tuviera que destacar una sola cosa de todo el módulo 3 — incluso de toda la sesión 3 — serían **los hooks**.

```
NO porque sean lo más sofisticado.

Por lo CONTRARIO:
└── porque son lo más SIMPLE
    que tiene el mayor IMPACTO INMEDIATO.
```

> Subagentes son potentes pero requieren cambiar tu forma de trabajar.
>
> Orquestaciones complejas son útiles
> pero solo cuando tu uso de Claude Code está maduro.
>
> Hooks, en cambio, los configuras una tarde
> y al día siguiente notas la diferencia.

---

## Slide 3 — Por qué este apartado cierra el módulo

```
Para que te lleves al puesto:

├── el modelo conceptual de los anteriores
└── ALGO ACCIONABLE QUE RINDA INMEDIATAMENTE
```

**Y conceptualmente:**

```
HOOKS = la pieza DETERMINISTA del agent harness.
```

```
En 3.1 vimos los WORKERS (subagentes).
En 3.2 vimos cómo se ORQUESTAN en flujos con loops y context bank.
Aquí cerramos con la capa que NO depende del razonamiento del agente.
```

> Lo que pasa **siempre**, sin opción a no pasar.
>
> Sin hooks, tu harness está incompleto:
> depende de que el agente recuerde
> hacer las cosas mecánicas.
>
> Con hooks: el harness se vuelve **fiable**.

---

## Slide 4 — La idea de fondo

```
Claude Code es un agente que:
├── razona
├── decide
└── ejecuta

Y lo hace bastante bien.
```

```
Pero hay cosas que NO deberían depender
de su razonamiento.
```

**Ejemplos:**

```
├── Cuando un fichero se modifica
│   → debe ejecutarse el formateador.
│
├── Cuando se va a lanzar un comando peligroso
│   → debe bloquearse.
│
└── Cuando termina una sesión
    → debe quedar un log.
```

> Estas son operaciones **deterministas**.
> Siempre las mismas. Predecibles. Mecánicas.

---

## Slide 5 — Qué es un hook

```
Hooks son SCRIPTS QUE SE EJECUTAN AUTOMÁTICAMENTE
en eventos del ciclo de vida de Claude Code.
```

```
├── Sin razonamiento
├── Sin opción a no ejecutarse
└── Garantizados
```

> Esto es lo que distingue depender del razonamiento del agente
> (un día, ocupado en otra cosa, no se ejecutará)
> de tener una **garantía**.

---

## Slide 6 — Instrucción vs garantía: la regla en CLAUDE.md

Para entender el valor de los hooks, conviene contrastarlos con la alternativa.

**La regla:** *"después de modificar un fichero .cs, ejecuta dotnet format"*.

**Opción 1: en `CLAUDE.md`.**

```
Escribes en el fichero:
"Después de modificar cualquier fichero .cs,
 ejecuta dotnet format antes de continuar"

¿Funciona? A VECES.
```

```
El agente lo ve, intenta seguirlo,
y la mayoría de las veces lo hace.

Pero CLAUDE.md es CONTEXTO, no GARANTÍA.

├── En sesiones largas, la instrucción puede salirse
│   de la ventana de atención.
└── Cuando hay muchas cosas pasando,
    el agente puede priorizar otras.
```

> *"Lo tendría que haber hecho"* es una respuesta que puedes recibir.

---

## Slide 7 — Instrucción vs garantía: la regla en un skill

**Opción 2: en un skill.**

```
Defines un skill llamado "post-edit-format"
cuya descripción dice:

"ejecutar después de cada edición de fichero .cs"

¿Funciona? MEJOR que la opción 1.
```

```
Porque la activación de skills
es más explícita.

PERO sigue dependiendo de que el agente
reconozca el momento adecuado.
```

> Y en sesiones complejas, la activación es **probabilística**.
>
> Mejor que CLAUDE.md, pero todavía no es garantía.

---

## Slide 8 — Instrucción vs garantía: la regla en un hook

**Opción 3: en un hook.**

```
Configuras un hook PostToolUse
con matcher Edit|Write|MultiEdit.

Cuando el matcher se cumple:
└── el hook se ejecuta SIN QUE EL AGENTE
    pueda decidir si lo ejecuta o no.
```

```
Es CÓDIGO, no instrucción.

La diferencia es ABSOLUTA.
```

---

## Slide 9 — La regla práctica de cuándo cada uno

```
LO QUE ES REGLA DETERMINISTA
(siempre la misma respuesta a un evento)
└── HOOK

LO QUE REQUIERE CRITERIO O ADAPTACIÓN
└── SKILL O SUBAGENTE

LO QUE ES CONTEXTO GENERAL DEL PROYECTO
└── CLAUDE.md
```

> Hooks son la pieza que cierra el círculo:
>
> aseguran que ciertas cosas pasen siempre,
> sin que ni tú ni el agente tengan que acordarse.

---

## Slide 10 — Anatomía de un hook

```
Un hook se configura como JSON
dentro de tu settings.json.
```

```
NO HAY un fichero hooks.json separado.

Esto es algo que confunde a quienes vienen
de tutoriales antiguos.

Los hooks viven dentro del fichero de configuración general.
```

---

## Slide 11 — Estructura básica de un hook

```json
{
  "hooks": {
    "PostToolUse": [
      {
        "matcher": "Write|Edit|MultiEdit",
        "hooks": [
          {
            "type": "command",
            "command": "npx prettier --write \"$CLAUDE_TOOL_INPUT_FILE_PATH\""
          }
        ]
      }
    ]
  }
}
```

**Cuatro elementos:**

```
1. El bloque "hooks" dentro de settings.json.

2. El EVENTO al que se engancha
   └── PostToolUse en este caso.
       Hay 17 eventos disponibles.

3. El MATCHER
   └── un patrón regex que decide cuándo se activa.
       Aquí: después de Write, Edit o MultiEdit.

4. El HANDLER
   └── el comando concreto que se ejecuta.
```

---

## Slide 12 — Scopes de configuración

Esto se mete en uno de los tres scopes (igual que con permisos, igual que con subagentes):

```
~/.claude/settings.json
└── USER
    Tus hooks personales que viajan contigo.

.claude/settings.json
└── PROYECTO
    Va a git, lo comparte el equipo.

.claude/settings.local.json
└── LOCAL
    Gitignored, tuyos para este proyecto.
```

```
El project-level TIENE PRECEDENCIA
cuando hay duplicados.
```

> Esto significa que un equipo puede definir hooks
> "no negociables" a nivel proyecto
> y los devs individuales NO pueden saltárselos
> en su user-level.

---

## Slide 13 — Ver y gestionar los hooks configurados

**Dos formas:**

```
> /hooks
```

```
Te abre una vista interactiva
con todos los hooks configurados
agrupados por evento.

Útil para auditar qué tienes configurado
especialmente si has acumulado varios.
```

**Para desactivar todos temporalmente:**

```json
"disableAllHooks": true
```

> Útil cuando estás debuggeando algo
> y los hooks están haciendo ruido.

---

## Slide 14 — Los eventos del ciclo de vida

Claude Code expone **17 eventos** a los que puedes engancharte. Los más útiles, agrupados por categoría:

```
1. EVENTOS DE SESIÓN
2. EVENTOS DE HERRAMIENTAS
3. EVENTOS DE PERMISOS
4. OTROS EVENTOS ÚTILES
```

Los vemos.

---

## Slide 15 — Eventos de sesión

```
SessionStart
└── Al ARRANCAR una sesión.
    Útil para inyectar contexto inicial:
    ├── la branch actual
    ├── el último commit
    └── estado del entorno

SessionEnd
└── Al CERRAR una sesión.
    Útil para:
    ├── logging
    ├── notificaciones
    └── limpieza

Stop
└── Cuando Claude TERMINA DE RESPONDER.
    Distinto de SessionEnd
    └── puede haber muchos Stop en una sesión
        (uno por respuesta).
```

---

## Slide 16 — Eventos de herramientas

```
PreToolUse
└── ANTES de que Claude ejecute una herramienta.
    LA PIEZA MÁS POTENTE.
    Puede inspeccionar la acción y BLOQUEARLA.
    Se activa antes incluso
    de la comprobación de permisos.

PostToolUse
└── DESPUÉS de que Claude haya ejecutado
    una herramienta exitosamente.
    Para validar resultado, formatear, registrar.
    NO puede deshacer la acción
    (la herramienta ya se ejecutó)
    pero puede dar feedback al agente.

PostToolUseFailure
└── Cuando una herramienta FALLA.
    Útil para logging de errores
    o intentos de recuperación.
```

---

## Slide 17 — Eventos de permisos y otros útiles

```
EVENTOS DE PERMISOS

PermissionRequest
└── Cuando Claude pide permiso interactivamente.
    Puede automatizar la decisión.

PermissionDenied
└── Cuando se DENIEGA un permiso.
    Para logging o alertas.
```

```
OTROS EVENTOS ÚTILES

UserPromptSubmit
└── Cada vez que envías un prompt.
    Para validar prompts antes de que se procesen.

SubagentStop
└── Cuando un subagente termina su tarea.
    Para encadenar acciones.

Notification
└── Cuando Claude genera una notificación.
    Para enrutar alertas.
```

---

## Slide 18 — Cuáles usar primero

Para empezar, **dos eventos cubren el 80% de los casos útiles**:

```
PostToolUse con matcher Write|Edit|MultiEdit
└── auto-formato, lint, validación.

PreToolUse con matcher Bash
└── bloqueo de comandos peligrosos.
```

> Con esos dos en marcha
> ya tienes la mayoría del valor.
>
> Los demás los añades cuando tienes casos concretos.

```
Esos dos casos son los que vamos a montar
en clase en 3.3b.
```

---

## Slide 19 — Tipos de handler: el más común, "command"

Cuando un hook se dispara, ejecuta un handler. **Hay cuatro tipos.** Empezamos por el más común:

```json
{
  "type": "command",
  "command": "npx prettier --write \"$CLAUDE_TOOL_INPUT_FILE_PATH\"",
  "timeout": 30
}
```

**Características:**

```
├── Ejecuta un comando shell
├── El input del evento llega por stdin como JSON
└── El output va a stdout
    (donde puedes devolver más JSON para control fino)
```

> El parámetro `timeout` (en segundos) es importante
> para hooks que pueden colgarse.
>
> Si pasa el timeout: el hook se considera **fallido**.

---

## Slide 20 — Handler "http" — para integraciones externas

```json
{
  "type": "http",
  "url": "https://hooks.miempresa.com/claude-pre-tool",
  "timeout": 30,
  "headers": {
    "Authorization": "Bearer $MY_TOKEN"
  },
  "allowedEnvVars": ["MY_TOKEN"]
}
```

```
POST de un JSON a un endpoint que tú expongas.
La respuesta del endpoint puede controlar el flujo.
```

> Útil cuando quieres integrar Claude Code
> con sistemas centralizados de tu empresa:
>
> ├── un servicio de policy enforcement
> ├── un sistema de logging corporativo
> └── un broker de notificaciones

---

## Slide 21 — Handler "prompt" — el handler con criterio

Cuando la decisión que hace el hook **no es determinista** sino que requiere juicio:

```json
{
  "type": "prompt",
  "prompt": "Determina si el comando '{tool_input.command}' es seguro de ejecutar en un entorno de producción. Responde solo 'allow' o 'deny'.",
  "model": "haiku"
}
```

```
En vez de un comando shell:
└── le pides a un modelo (Haiku por defecto, configurable)
    que tome la decisión.
```

> Esto es relativamente nuevo y abre un patrón potente:
>
> **HOOKS INTELIGENTES.**
>
> No bloqueas con regex sino con criterio.
>
> Útil cuando los patrones a detectar
> son sutiles o varían según contexto.

---

## Slide 22 — Otros handlers especializados

```
Hay tipos más específicos para casos avanzados.

Pero los tres anteriores
└── command, http, prompt

cubren prácticamente todo lo que vas a necesitar
al principio.
```

---

## Slide 23 — El sistema de exit codes: la clave del control

Esto merece atención propia porque es **donde se decide qué pasa después de que el hook se ejecute**.

| Exit code | Significado |
|---|---|
| **0** | Éxito. Si hay JSON en stdout, se parsea para control fino. |
| **2** | **Blocking error.** El stderr se devuelve a Claude. Para `PreToolUse`, bloquea la herramienta. Para `Stop`, fuerza a Claude a seguir trabajando. |
| Otros | Non-blocking error. stderr se muestra en modo verbose, pero la ejecución continúa. |

> **Exit code 2 es la herramienta más potente.**

---

## Slide 24 — Por qué exit 2 es absoluto

```
Es lo que permite a un hook BLOQUEAR acciones
de forma absoluta.
```

```
Es lo que distingue:

├── una verdadera política de seguridad
│   "este comando NO se ejecuta"
│
└── un simple aviso
    "esto está raro"
```

**Una observación crítica para policies de seguridad:**

```
Un PreToolUse que devuelve exit 2
BLOQUEA LA HERRAMIENTA INCLUSO EN MODO

  --dangerously-skip-permissions
```

> Esto es importante.
>
> Significa que puedes establecer reglas
> que el usuario NO PUEDE saltarse
> simplemente cambiando su modo de permisos.

---

## Slide 25 — Output JSON estructurado

Para control más fino, en vez de solo exit codes puedes devolver JSON estructurado por stdout (con exit 0):

```json
{
  "hookSpecificOutput": {
    "hookEventName": "PreToolUse",
    "permissionDecision": "allow",
    "permissionDecisionReason": "Operación segura",
    "updatedInput": {
      "command": "comando-modificado"
    },
    "additionalContext": "Información para Claude"
  }
}
```

**Tres campos clave:**

```
permissionDecision
└── "allow", "deny" o "ask"

updatedInput
└── permite MODIFICAR los argumentos de la herramienta
    antes de que se ejecute.
    Transparente para Claude.

additionalContext
└── permite inyectar información en la conversación.
```

---

## Slide 26 — updatedInput: brutal y poco conocido

```
updatedInput es BRUTAL y poco conocido.
```

**Permite cosas como:**

```
SI EL AGENTE INTENTA EJECUTAR
git push origin main
└── Lo modificas a:
    git push origin feature/branch-actual

SI INTENTA USAR PRETTIER SIN LA CONFIG CORRECTA
└── Le añades el flag.

SI INTENTA EJECUTAR dotnet test SIN FILTROS
└── Le añades los filtros del módulo actual.
```

```
MODIFICA LA ACCIÓN SOBRE LA MARCHA
sin que el agente sepa que la modificaste.
```

---

## Slide 27 — La contrapartida de updatedInput

```
Es potente. Y tiene su contrapartida.
```

```
Si lo abusas:
└── debugging se vuelve confuso.

"¿Por qué ejecutó X cuando dije Y?"
```

> Úsalo para correcciones obvias y obvias.
> No para magia.

```
Buen uso:
└── corregir flags estándar olvidados,
    rutas relativas mal calculadas,
    convenciones del equipo automáticas.

Mal uso:
└── lógica de negocio,
    decisiones que el dev debería ver.
```

---

## Slide 28 — Lo que tienes ahora

```
✅ Por qué los hooks cierran el harness
   (lo determinista que no depende del modelo)

✅ Diferencia entre instrucción y garantía
   (CLAUDE.md vs skill vs hook)

✅ Anatomía de un hook
   (4 elementos en el JSON)

✅ Los scopes y la precedencia project>user>local

✅ Los 17 eventos disponibles
   (con los 2 más útiles destacados)

✅ Los tipos de handler
   (command, http, prompt + otros)

✅ El sistema de exit codes
   (con exit 2 = bloqueo absoluto)

✅ Output JSON estructurado
   (con updatedInput como pieza brutal)
```

> Tienes el modelo conceptual.
> Falta verlo aplicado a casos reales.

---

## Slide 29 — La pregunta antes de seguir

```
Antes de pasar a 3.3b, una pregunta:
```

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   ¿Qué cosa mecánica de tu día a día con Claude Code     │
│   te gustaría que pasara SIEMPRE,                        │
│   sin tener que acordarte de pedirla?                    │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

```
Pistas:
├── ¿Cada vez que tocas un fichero .cs
│   te toca recordar correr dotnet format?
├── ¿Has visto al agente proponer git push --force
│   y has tenido que pararlo?
└── ¿Te gustaría tener un log automático
    de qué hizo cada sesión?
```

---

## Slide 30 — Lo que viene en 3.3b

```
SUBMÓDULO 3.3b — CASOS PRÁCTICOS, OBSERVABILIDAD, CIERRE
─────────────────────────────────────────────────────────

Caso práctico guiado: AUTO-FORMATO Y LINT
├── El hook (PostToolUse + matcher Write|Edit|MultiEdit)
├── Refinamiento: usar un script externo
└── Probar el hook

Caso práctico guiado: BLOQUEO DE COMANDOS PELIGROSOS
├── El hook (PreToolUse + matcher Bash)
├── El script con la lista de patrones
├── Por qué exit 2 vale incluso en modo autónomo
└── Ampliación: validación basada en LLM

CHANNELS (referencia rápida)

ANTI-PATRONES + ERRORES FRECUENTES

OBSERVABILIDAD (sección nueva v3)
├── Por qué es distinto en agentes
├── Qué loggear
├── Cómo se hace con hooks
├── Hook concreto de logging
└── Conexión con el context bank

EL HARNESS COMPLETO: definición operativa
└── prompts + tools + context policies
    + hooks + feedback loops + observability

CIERRE DEL MÓDULO 3
```

**Nos vemos en 3.3b.**
