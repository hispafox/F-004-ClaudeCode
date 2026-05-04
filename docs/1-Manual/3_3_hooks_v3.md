# 3.3 Hooks, channels y automatización determinista

**Duración en clase:** 50-52 minutos · **Sesión 3, submódulo 3** · **Versión: v3**

> **Cambios v2 → v3**: nueva sección "Observabilidad: la pieza que cierra el harness fiable" (~700 palabras, +5-7 min), fórmula del harness extendida con `+ observability` y bullet correspondiente en "El harness completo", lectura complementaria al whitepaper de Anthropic en cierre.

---

## La pieza que cierra el harness

Si tuviera que destacar una sola cosa de todo el módulo 3 — incluso de toda la sesión 3 — serían los hooks. No porque sean lo más sofisticado. Por lo contrario: porque son **lo más simple que tiene el mayor impacto inmediato**.

Subagentes son potentes pero requieren cambiar tu forma de trabajar. Orquestaciones complejas son útiles pero solo cuando tu uso de Claude Code está maduro. Hooks, en cambio, los configuras una tarde y al día siguiente notas la diferencia. Por eso este apartado cierra el módulo: para que te lleves al puesto, además del modelo conceptual de los anteriores, **algo accionable que rinda inmediatamente**.

Y conceptualmente, los hooks son **la pieza determinista del agent harness** que hemos estado construyendo a lo largo del módulo. En 3.1 vimos los workers (subagentes). En 3.2 vimos cómo se orquestan en flujos con loops y context bank. Aquí cerramos con la capa que **no depende del razonamiento del agente**: lo que pasa siempre, sin opción a no pasar. Sin hooks, tu harness está incompleto — depende de que el agente recuerde hacer las cosas mecánicas. Con hooks, el harness se vuelve fiable.

La idea de fondo es esta: Claude Code es un agente que razona, decide y ejecuta. Y lo hace bastante bien. Pero hay cosas que **no deberían depender de su razonamiento**. Cuando un fichero se modifica, debe ejecutarse el formateador. Cuando se va a lanzar un comando peligroso, debe bloquearse. Cuando termina una sesión, debe quedar un log. Estas son operaciones deterministas — siempre las mismas, predecibles, mecánicas. Y depender del razonamiento del agente para que se ejecuten siempre es una garantía de que un día, cuando estés ocupado en otra cosa, no se ejecutarán.

Hooks son **scripts que se ejecutan automáticamente en eventos del ciclo de vida** de Claude Code. Sin razonamiento. Sin opción a no ejecutarse. Garantizados.

---

## El problema que resuelven: instrucción vs garantía

Para entender el valor de los hooks conviene contrastarlos con la alternativa: meter las mismas reglas en `CLAUDE.md` o en un skill.

Imagina la regla *"después de modificar un fichero .cs, ejecuta dotnet format"*. Tres formas de implementarla:

**Opción 1: en `CLAUDE.md`.** Escribes en el fichero *"Después de modificar cualquier fichero .cs, ejecuta dotnet format antes de continuar"*. ¿Funciona? A veces. El agente lo ve, intenta seguirlo, y la mayoría de las veces lo hace. Pero el `CLAUDE.md` es contexto, no garantía. En sesiones largas, la instrucción puede salirse de la ventana de atención. Cuando hay muchas cosas pasando a la vez, el agente puede priorizar otras. *"Lo tendría que haber hecho"* es una respuesta que puedes recibir.

**Opción 2: en un skill.** Defines un skill llamado "post-edit-format" cuya descripción dice *"ejecutar después de cada edición de fichero .cs"*. ¿Funciona? Mejor que la opción 1, porque la activación de skills es más explícita. Pero sigue dependiendo de que el agente reconozca el momento adecuado. Y en sesiones complejas, la activación es probabilística.

**Opción 3: en un hook.** Configuras un hook `PostToolUse` con matcher `Edit|Write|MultiEdit`. Cuando el matcher se cumple, **el hook se ejecuta sin que el agente pueda decidir si lo ejecuta o no**. Es código, no instrucción. La diferencia es absoluta.

La regla práctica:

- Lo que es **regla deterministática** (siempre la misma respuesta a un evento) → hook.
- Lo que requiere **criterio o adaptación** → skill o subagente.
- Lo que es **contexto general del proyecto** → `CLAUDE.md`.

Hooks son la pieza que cierra el círculo: aseguran que ciertas cosas pasen siempre, sin que ni tú ni el agente tengan que acordarse.

---

## Anatomía de un hook

Vamos a la parte mecánica. Un hook se configura como JSON dentro de tu `settings.json`. **No hay un fichero `hooks.json` separado** — esto es algo que confunde a quienes vienen de tutoriales antiguos. Los hooks viven dentro del fichero de configuración general.

Estructura básica:

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

Cuatro elementos:

1. **El bloque `hooks`** dentro de `settings.json`.
2. **El evento al que se engancha** — `PostToolUse` en este caso. Hay 17 eventos disponibles, los veremos.
3. **El matcher** — un patrón regex que decide cuándo se activa el hook. Aquí, después de `Write`, `Edit` o `MultiEdit`.
4. **El handler** — el comando concreto que se ejecuta. Más adelante veremos los tipos de handler disponibles.

Esto se mete en uno de los tres scopes de configuración (igual que con permisos, igual que con subagentes):

- `~/.claude/settings.json` — user. Tus hooks personales que viajan contigo.
- `.claude/settings.json` — proyecto. Va a git, lo comparte el equipo.
- `.claude/settings.local.json` — local. Gitignored, tuyos para este proyecto.

El project-level **tiene precedencia** cuando hay duplicados. Esto significa que un equipo puede definir hooks "no negociables" a nivel proyecto y los devs individuales no pueden saltárselos en su user-level.

### Ver y gestionar los hooks configurados

Dos formas:

```
> /hooks
```

Te abre una vista interactiva con todos los hooks configurados, agrupados por evento. Útil para auditar qué tienes configurado, especialmente si has acumulado varios.

```bash
# Desactivar todos los hooks temporalmente
"disableAllHooks": true
```

Útil cuando estás debuggeando algo y los hooks están haciendo ruido.

---

## Los eventos del ciclo de vida

Claude Code expone 17 eventos a los que puedes engancharte. Los más útiles, agrupados por categoría:

### Eventos de sesión

- **`SessionStart`** — al arrancar una sesión. Útil para inyectar contexto inicial (la branch actual, el último commit, estado del entorno).
- **`SessionEnd`** — al cerrar una sesión. Útil para logging, notificaciones, limpieza.
- **`Stop`** — cuando Claude termina de responder. Distinto de `SessionEnd` porque puede haber muchos `Stop` en una sesión (uno por respuesta).

### Eventos de herramientas

- **`PreToolUse`** — **antes** de que Claude ejecute una herramienta. La pieza más potente. Puede inspeccionar la acción y bloquearla. Se activa antes incluso de la comprobación de permisos.
- **`PostToolUse`** — **después** de que Claude haya ejecutado una herramienta exitosamente. Para validar resultado, formatear, registrar. **No puede deshacer la acción** (la herramienta ya se ejecutó), pero puede dar feedback al agente.
- **`PostToolUseFailure`** — cuando una herramienta falla. Útil para logging de errores o intentos de recuperación.

### Eventos de permisos

- **`PermissionRequest`** — cuando Claude pide permiso interactivamente. Puede automatizar la decisión.
- **`PermissionDenied`** — cuando se deniega un permiso. Para logging o alertas.

### Otros eventos útiles

- **`UserPromptSubmit`** — cada vez que envías un prompt. Para validar prompts antes de que se procesen.
- **`SubagentStop`** — cuando un subagente termina su tarea. Para encadenar acciones.
- **`Notification`** — cuando Claude genera una notificación. Para enrutar alertas.

### Cuáles usar primero

Para empezar, dos eventos cubren el 80% de los casos útiles:

- **`PostToolUse`** con matcher `Write|Edit|MultiEdit` — auto-formato, lint, validación.
- **`PreToolUse`** con matcher `Bash` — bloqueo de comandos peligrosos.

Con esos dos en marcha ya tienes la mayoría del valor. Los demás los añades cuando tienes casos concretos.

---

## Tipos de handler

Cuando un hook se dispara, ejecuta un handler. Hay cuatro tipos:

### `command` — el más común

Ejecuta un comando shell. El input del evento llega por stdin como JSON; el output va a stdout (donde puedes devolver más JSON para control fino).

```json
{
  "type": "command",
  "command": "npx prettier --write \"$CLAUDE_TOOL_INPUT_FILE_PATH\"",
  "timeout": 30
}
```

El parámetro `timeout` (en segundos) es importante para hooks que pueden colgarse. Si pasa el timeout, el hook se considera fallido.

### `http` — para integraciones externas

POST de un JSON a un endpoint que tú expongas. La respuesta del endpoint puede controlar el flujo.

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

Útil cuando quieres integrar Claude Code con sistemas centralizados de tu empresa: un servicio de policy enforcement, un sistema de logging corporativo, un broker de notificaciones.

### `prompt` — el handler con criterio

Cuando la decisión que hace el hook **no es deterministática** sino que requiere juicio, en vez de un comando shell puedes pedirle a un modelo (Haiku por defecto, configurable) que tome la decisión.

```json
{
  "type": "prompt",
  "prompt": "Determina si el comando '{tool_input.command}' es seguro de ejecutar en un entorno de producción. Responde solo 'allow' o 'deny'.",
  "model": "haiku"
}
```

Esto es relativamente nuevo y abre un patrón potente: **hooks inteligentes**. No bloqueas con regex sino con criterio. Útil cuando los patrones a detectar son sutiles o varían según contexto.

### Otros handlers especializados

Hay tipos más específicos para casos avanzados, pero `command`, `http` y `prompt` cubren prácticamente todo lo que vas a necesitar al principio.

---

## El sistema de exit codes: la clave del control

Esto merece atención propia porque es donde se decide qué pasa después de que el hook se ejecute.

| Exit code | Significado |
|---|---|
| **0** | Éxito. Si hay JSON en stdout, se parsea para control fino. |
| **2** | **Blocking error.** El stderr se devuelve a Claude. Para `PreToolUse`, bloquea la herramienta. Para `Stop`, fuerza a Claude a seguir trabajando. |
| Otros | Non-blocking error. stderr se muestra en modo verbose, pero la ejecución continúa. |

**Exit code 2 es la herramienta más potente**. Es lo que permite a un hook **bloquear** acciones de forma absoluta. Y es lo que distingue una verdadera política de seguridad ("este comando NO se ejecuta") de un simple aviso ("esto está raro").

Una observación crítica para policies de seguridad: **un `PreToolUse` que devuelve exit 2 bloquea la herramienta incluso en modo `--dangerously-skip-permissions`**. Esto es importante. Significa que puedes establecer reglas que el usuario no puede saltarse simplemente cambiando su modo de permisos. Si tu equipo necesita garantías reales (no recomendaciones), los hooks `PreToolUse` con exit 2 son el mecanismo.

### Output JSON estructurado

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

`permissionDecision` puede ser `"allow"`, `"deny"`, o `"ask"`. `updatedInput` permite **modificar** los argumentos de la herramienta antes de que se ejecute (transparente para Claude). `additionalContext` permite inyectar información en la conversación.

Este último, `updatedInput`, es **brutal** y poco conocido. Permite cosas como: si el agente intenta ejecutar `git push origin main`, lo modificas a `git push origin feature/branch-actual`. Si intenta usar Prettier sin la config correcta, le añades el flag. Si intenta ejecutar `dotnet test` sin filtros, le añades los filtros del módulo actual. **Modifica la acción sobre la marcha**, sin que el agente sepa que la modificaste.

Es potente y tiene su contrapartida: si lo abusas, debugging se vuelve confuso (*"¿por qué ejecutó X cuando dije Y?"*). Úsalo para correcciones obvias y obvialas; no para magia.

---

## Caso práctico guiado: auto-formato y lint

Vamos al primer caso real que vamos a montar en clase. La situación: cada vez que Claude modifica un fichero .cs, queremos que se ejecute `dotnet format`. Cada vez que modifica un fichero .ts, `npm run lint -- --fix`. Sin tener que pedirlo.

### El hook

En `.claude/settings.json` (proyecto, va a git):

```json
{
  "hooks": {
    "PostToolUse": [
      {
        "matcher": "Write|Edit|MultiEdit",
        "hooks": [
          {
            "type": "command",
            "command": "case \"$CLAUDE_TOOL_INPUT_FILE_PATH\" in *.cs) dotnet format --include \"$CLAUDE_TOOL_INPUT_FILE_PATH\" 2>&1 ;; *.ts|*.html|*.scss) cd src/Web && npm run lint -- --fix \"$CLAUDE_TOOL_INPUT_FILE_PATH\" 2>&1 ;; esac",
            "timeout": 60
          }
        ]
      }
    ]
  }
}
```

Lo que hace:

- Se dispara después de cualquier `Write`, `Edit` o `MultiEdit`.
- Usa la variable de entorno `$CLAUDE_TOOL_INPUT_FILE_PATH` (que Claude Code expone) para saber qué fichero se ha tocado.
- Según la extensión, ejecuta el formateador adecuado.
- Si no es ninguno de los conocidos, no hace nada (el `case` no encaja con ningún patrón).

### Refinamiento: usar un script externo

El comando inline funciona pero es difícil de leer y mantener. Mejor patrón: un script externo.

Crea `.claude/hooks/format-on-write.sh`:

```bash
#!/bin/bash
set -e

FILE_PATH="$CLAUDE_TOOL_INPUT_FILE_PATH"

case "$FILE_PATH" in
  *.cs)
    dotnet format --include "$FILE_PATH" 2>&1
    ;;
  *.ts|*.html|*.scss)
    if [ -d "src/Web" ]; then
      cd src/Web
      npm run lint -- --fix "$FILE_PATH" 2>&1
    fi
    ;;
  *.json|*.md)
    npx prettier --write "$FILE_PATH" 2>&1
    ;;
esac

exit 0
```

Y simplifica el `settings.json`:

```json
{
  "hooks": {
    "PostToolUse": [
      {
        "matcher": "Write|Edit|MultiEdit",
        "hooks": [
          {
            "type": "command",
            "command": "$CLAUDE_PROJECT_DIR/.claude/hooks/format-on-write.sh",
            "timeout": 60
          }
        ]
      }
    ]
  }
}
```

`$CLAUDE_PROJECT_DIR` es otra variable que Claude Code expone — apunta a la raíz del proyecto. Y el script vive en `.claude/hooks/`, que es el sitio convencional para guardarlos.

Acuérdate de hacerlo ejecutable:

```bash
chmod +x .claude/hooks/format-on-write.sh
```

### Probar el hook

Lanza una sesión nueva (los hooks se cargan al arrancar) y pide al agente que modifique un fichero .cs. Cuando termine la modificación, deberías ver el formato aplicarse automáticamente.

Si no se ejecuta, debug:

```bash
# Test manual
echo '{"tool_name":"Write","tool_input":{"file_path":"test.cs"}}' | .claude/hooks/format-on-write.sh
echo $?
```

Si el script funciona aislado pero no se dispara en la sesión, el problema suele estar en el matcher o en la configuración del `settings.json`. Usa `/hooks` para ver lo que Claude Code ha cargado.

---

## Caso práctico guiado: bloqueo de comandos peligrosos

El segundo caso. Esto es el equivalente del cinturón de seguridad: lo configuras una vez y te olvidas, pero el día que importa estás vivo gracias a él.

```json
{
  "hooks": {
    "PreToolUse": [
      {
        "matcher": "Bash",
        "hooks": [
          {
            "type": "command",
            "command": "$CLAUDE_PROJECT_DIR/.claude/hooks/block-dangerous.sh",
            "timeout": 5
          }
        ]
      }
    ]
  }
}
```

Y el script `.claude/hooks/block-dangerous.sh`:

```bash
#!/bin/bash

# Lee el JSON de stdin
INPUT=$(cat)
COMMAND=$(echo "$INPUT" | jq -r '.tool_input.command')

# Lista de patrones bloqueados
BLOCKED_PATTERNS=(
  "rm -rf /"
  "rm -rf ~"
  "git push --force"
  "git push -f"
  "git reset --hard origin"
  "DROP TABLE"
  "DROP DATABASE"
  ":(){ :|:& };:"  # fork bomb
)

for pattern in "${BLOCKED_PATTERNS[@]}"; do
  if echo "$COMMAND" | grep -qE "$pattern"; then
    echo "BLOCKED: comando contiene patrón peligroso: $pattern" >&2
    exit 2
  fi
done

exit 0
```

Lo que hace:

- Antes de cualquier ejecución de `Bash`, lee el comando que va a ejecutarse.
- Si encaja con cualquier patrón peligroso, devuelve **exit 2** — bloqueo absoluto.
- Si no, deja pasar.

Como dije antes, el exit 2 bloquea **incluso en modo autónomo**. Esto significa que aunque alguien lance Claude Code con `--dangerously-skip-permissions`, los comandos peligrosos siguen bloqueados. Es una garantía real, no una recomendación.

Mi consejo: este hook va siempre. En tu user-level (`~/.claude/settings.json`), no en el proyecto. Así viaja contigo a todos los repos. Cuando alguna vez intentes hacer algo destructivo y veas el bloqueo, te vas a alegrar de tenerlo.

### Ampliación natural: validación basada en LLM

El bloqueo basado en regex captura los patrones obvios. Pero hay comandos peligrosos que no son obvios — `dd if=/dev/zero of=/dev/sda` es destructivo y un regex tendría que ser muy elaborado para captarlo.

Aquí encaja un handler de tipo `prompt`:

```json
{
  "hooks": {
    "PreToolUse": [
      {
        "matcher": "Bash",
        "hooks": [
          {
            "type": "prompt",
            "prompt": "Analiza este comando y determina si es destructivo o peligroso: '{tool_input.command}'. Responde solo 'safe' o 'dangerous'.",
            "model": "haiku"
          }
        ]
      }
    ]
  }
}
```

Haiku evalúa cada comando antes de ejecutarse. Si dice `dangerous`, bloqueo automático. Más caro que el regex (cada Bash ahora pasa por LLM), pero captura cosas que un regex no captaría.

Combinar ambos enfoques es lo más prudente: regex para los obvios (rápido, gratis), LLM para los sutiles (más caro pero más cobertura).

---

## Channels: referencia rápida

El temario menciona channels. La pieza es real pero su uso práctico todavía es minoritario, así que lo cubrimos por encima.

Un **channel** es básicamente un MCP server que en vez de exponer herramientas hace **push hacia la sesión** de Claude Code. La inversión: en lugar de que Claude pregunte cuando necesita algo, el sistema externo notifica a Claude cuando pasa algo.

Casos típicos:

- **Alerta de CI fallido.** El sistema de CI manda un mensaje al canal de Claude Code: *"el build de la rama X ha fallado, aquí está el log"*. Claude lo recibe en mitad de tu sesión y puede analizarlo.
- **Eventos de monitorización.** Un sistema externo detecta una anomalía y la notifica.
- **Mensajes de chat.** Slack reenvía menciones del equipo al canal.

La configuración es como un MCP server normal pero con la capacidad `claude/channel`. Anthropic los está madurando pero su uso está más en el lado experimental.

**Mi recomendación honesta para este curso**: saber que existen, no es algo que vayas a configurar la primera semana. Si en unos meses identificas un caso donde un sistema externo necesita notificar a Claude Code de forma proactiva, busca documentación específica en ese momento.

---

## Anti-patrones de hooks

Los errores típicos al empezar a usar hooks:

**Hooks demasiado lentos.** Los hooks se ejecutan **síncronamente**. Cada hook que disparas suma a la latencia de la herramienta correspondiente. Si tu PostToolUse tarda 5 segundos, cada Write tarda 5 segundos más de lo que tardaría sin el hook. Mantén los hooks por debajo de 200-500ms. Si necesitas algo más lento, ejecútalo en background o muévelo a otro evento.

**Múltiples hooks modificando el mismo input.** Los hooks corren en paralelo. Si dos `PreToolUse` con matcher `Bash` intentan modificar el `tool_input.command`, el orden de ejecución es no determinista, y el último gana. Si tienes que modificar input, hazlo desde un único hook.

**Hooks que dependen del directorio actual.** Si tu hook hace `cd src/Web && ...`, el `cd` solo afecta al subshell del hook. Si Claude está trabajando en otro directorio, las rutas pueden no resolverse. Mejor usar paths absolutos basados en `$CLAUDE_PROJECT_DIR`.

**No probar los hooks aisladamente.** Un hook que solo pruebas dentro de Claude Code es difícil de debuggear cuando falla. Acostúmbrate a testar tus hooks pasando JSON por stdin desde la línea de comandos.

**Bloqueos demasiado agresivos.** Un `PreToolUse` que devuelve exit 2 en demasiados casos hace que Claude Code se sienta paralizado. Sé selectivo con los bloqueos — son la herramienta más fuerte pero también la más fácil de abusar.

**Stop hooks que crean bucles infinitos.** Un Stop hook que devuelve exit 2 fuerza a Claude a seguir trabajando. Si esa condición se mantiene siempre verdadera, Claude nunca para. La solución: comprueba el campo `stop_hook_active` en el JSON de input para detectar la segunda invocación y dejarlo pasar.

**Hardcodear paths de scripts.** Si tu hook llama a `/Users/pedro/.claude/hooks/my-hook.sh`, deja de funcionar para todo el equipo. Usa `$CLAUDE_PROJECT_DIR/.claude/hooks/...` y mete los scripts en el repo.

**Olvidar `chmod +x` en scripts nuevos.** Cuando creas un script de hook nuevo y no se ejecuta, lo primero que comprobar: `ls -l` para ver si tiene permisos de ejecución.

---

## Errores frecuentes con tus primeros hooks

Lista práctica:

- **Editar `settings.json` con sesión abierta y esperar que se aplique.** El file watcher normalmente recoge los cambios, pero si no, **reinicia la sesión**.
- **Confundir `PreToolUse` con `PostToolUse`.** Pre = antes, puede bloquear. Post = después, no puede deshacer pero puede dar feedback. Si quieres bloquear, es Pre. Si quieres reaccionar, es Post.
- **Esperar que `PermissionRequest` funcione en modo `-p` (one-shot).** No lo hace. En modo no interactivo, usa `PreToolUse` para automatizar decisiones de permiso.
- **No revisar `/hooks` para confirmar lo que está cargado.** Si cambias algo y no funciona, `/hooks` te dice qué tiene Claude Code cargado actualmente. La mitad de las veces, no es lo que crees.
- **Olvidarse de que hooks aplican a subagentes también.** Si tienes un PreToolUse que valida algo, también valida cuando un subagente lo invoca. Esto es bueno para seguridad (los subagentes no se saltan tus reglas) pero puede generar comportamiento inesperado si no lo tienes en cuenta.
- **Hooks que llaman a herramientas que no están en el PATH del entorno de Claude.** Tu `.zshrc` puede tener PATH custom, pero el subshell del hook puede no tenerlos. Usa paths absolutos o configura PATH en el script.
- **Pretender que los hooks son la solución a todo.** Hay cosas que sí merecen criterio. No metas en un hook lo que requiere razonamiento — para eso están los skills y los subagentes.

---

## Observabilidad: la pieza que cierra el harness fiable

Hay una pregunta que aparece la primera vez que algo se rompe en mitad de un workflow compuesto. Te llega un mensaje del lead diciendo que un subagente devolvió algo raro y el código pushed está mal. Vas a `/usage`, ves que la sesión consumió un montón de tokens, y te toca reconstruir qué pasó: ¿en qué subagente se torció la cosa?, ¿qué decisión tomó el orquestador?, ¿en qué iteración del loop validator se decidió aprobar?

Sin logs estructurados, la respuesta es *"vete a saber"*. Y en agentes, eso no vale.

Aquí entra la **observabilidad**. Es la pieza que faltaba para cerrar el harness.

### Por qué es distinto en agentes

Con código tradicional, debugger + stack trace + logs te alcanzan. Con agentes, no. Tres razones:

- **No determinismo.** Lanzas el mismo workflow dos veces y las decisiones del orquestador no son idénticas. No puedes "reproducir" un fallo con la misma fiabilidad que reproduces un null pointer.
- **Decisiones opacas.** El razonamiento del agente vive dentro del modelo. Cuando algo va mal, no tienes un branch del código que mirar — tienes que reconstruir qué pensó el agente, y eso solo lo sabes si lo logueaste.
- **Cadenas largas.** Skill orquestador → subagente A → subagente B → MCP server. Cada eslabón es un punto de fallo. Si no sabes qué pasó en cada uno, debugging es adivinación.

La regla práctica: **los agentes que llegan a producción tienen siempre algún sistema de observabilidad**. Los que no, no llegan.

### Qué loggear

Lo que merece la pena tener registrado para cada sesión seria:

- **Decisiones del orquestador.** Cuándo decidió invocar a qué subagente, con qué parámetros, y cuál fue la respuesta. Es el log principal del workflow.
- **Invocaciones a subagentes.** Modelo usado (haiku/sonnet/opus), tokens consumidos, latencia, si terminó con éxito o falló.
- **Iteraciones de loops.** Cuántas vueltas dio el validator → implementer antes de converger. Si un loop necesita las 3 iteraciones cada vez, el problema está en el subagente, no en el flujo.
- **Errores y bloqueos de hooks.** Qué se bloqueó y por qué. Útil para auditar políticas de seguridad y detectar falsos positivos.
- **Coste por workflow.** Tokens agregados por sesión, agrupados por subagente. Sin esto, descubrir cuál de tus subagentes engorda la factura es imposible.

### Cómo se hace con hooks

Los hooks no son solo automatización — son **el mecanismo natural de instrumentación**. Tres eventos te dan el grueso:

- **`SessionEnd`** — al cerrar una sesión, vuelca un resumen a un fichero de log: tokens, subagentes invocados, duración. Tu fila en la "base de datos" de sesiones.
- **`PostToolUse`** — tracing por tool call. Cada vez que el agente ejecuta una herramienta, un hook registra qué fue, con qué input, y cuánto tardó.
- **`SubagentStop`** — el más específico. Se dispara cuando un subagente termina. Útil para registrar su salida sin que el log se ensucie con cada operación intermedia que hizo por dentro.

### Hook concreto de logging

Lo más simple que ya rinde:

```json
{
  "hooks": {
    "SessionEnd": [
      {
        "hooks": [
          {
            "type": "command",
            "command": "$CLAUDE_PROJECT_DIR/.claude/hooks/log-session.sh"
          }
        ]
      }
    ]
  }
}
```

Y el script `.claude/hooks/log-session.sh`:

```bash
#!/bin/bash

INPUT=$(cat)
TIMESTAMP=$(date -u +"%Y-%m-%dT%H:%M:%SZ")
SESSION_ID=$(echo "$INPUT" | jq -r '.session_id')
TOTAL_TOKENS=$(echo "$INPUT" | jq -r '.usage.total_tokens // 0')
TOOL_CALLS=$(echo "$INPUT" | jq -r '.tool_calls_count // 0')

LOG_FILE="$CLAUDE_PROJECT_DIR/.claude/logs/sessions.jsonl"
mkdir -p "$(dirname "$LOG_FILE")"

echo "{\"timestamp\":\"$TIMESTAMP\",\"session_id\":\"$SESSION_ID\",\"tokens\":$TOTAL_TOKENS,\"tool_calls\":$TOOL_CALLS}" >> "$LOG_FILE"

exit 0
```

A partir de ahí, los datos del fichero `sessions.jsonl` van a Datadog, Grafana, Splunk, o tu sistema interno. La pieza importante no es el destino — es **tener los datos**.

### Conexión con el context bank

En 3.2 viste el **context bank** — los ficheros markdown bajo `.claude/workflow-state/<feature>/` que sirven de memoria compartida entre subagentes. El context bank ya es, de facto, medio log del workflow: te dice qué pensó el planner, qué exploró el explorer, qué hallazgos sacó el reviewer.

Para workflows compuestos no necesitas montar logging desde cero — el context bank ya está ahí. Lo que añaden los hooks de observabilidad es **la capa transversal**: tokens, latencia, eventos de sistema, métricas agregadas. Las dos capas juntas — context bank + hooks de logging — te dan trazabilidad real de lo que pasó en cada sesión y por qué.

Y con esto se cierra el harness.

---

## El harness completo: definición operativa

Cierre del módulo 3. Llegado a este punto tienes el harness entero. Una definición operativa que vale la pena memorizar:

> **harness = prompts + tools + context policies + hooks + feedback loops + observability**

Cada pieza la tienes ya:

- **Prompts** — `CLAUDE.md` (módulo 1), skills (módulo 2). Lo que el agente sabe sobre cómo trabaja tu equipo.
- **Tools** — Bash, Read, Edit, MCP servers (módulos 1, 2 y 4). Lo que el agente puede ejecutar.
- **Context policies** — subagentes con `tools` restringidos, scopes (user/proyecto), permisos (3.1). Lo que el agente puede tocar y cuándo.
- **Hooks** — automatización determinista (este apartado). Lo que pasa siempre.
- **Feedback loops** — validators que devuelven el flujo, context bank como memoria compartida (3.2). Lo que hace que el harness se autocorrija.
- **Observability** — hooks de logging y métricas (este apartado, sección anterior). Lo que hace que el harness sea depurable.

Skills personalizan lo que el agente sabe. Subagentes definen roles especializados. La orquestación los combina con loops y context bank. Y los hooks aseguran que las cosas mecánicas pasen siempre, sin depender de razonamiento. Eso, junto, es **tu harness sobre Claude Code**.

Cuando llegues al portal de recursos del curso, encontrarás el cheatsheet visual del patrón Agent Harness. Conviene tenerlo a mano las primeras semanas mientras decides qué pieza usar para cada cosa.

---

## Antes de seguir

En el módulo 4 cambiamos de tema completamente. Hasta aquí hemos hablado del agente y su personalización — el harness entero. Ahora entramos en cómo Claude Code se integra con **el flujo de diseño**. El módulo 4 cubre **Figma MCP y Claude Design**: cómo trabajar con diseños existentes a través del MCP de Figma, cómo usar Claude Design para creación visual conversacional, y cómo el formato emergente DESIGN.md está reconfigurando la forma en que los agentes consumen design systems.

### Lectura complementaria opcional

Si en tu rol toca decidir arquitecturas a nivel sistema — no solo usar Claude Code como dev — Anthropic publicó un whitepaper que conviene tener a mano: **"Building Effective AI Agents: Architecture Patterns and Implementation Frameworks"** (`resources.anthropic.com/building-effective-ai-agents`). Cubre los mismos patrones que has visto aquí en versión más teórica: hierarchical, collaborative, sequential, parallel, evaluator-optimizer. El vocabulario formal que aquí hemos pincelado de pasada. No es requisito del curso — es para cuando alguien arriba pida un *"diseño de sistema multi-agente"* y quieras ir con los términos que esa persona espera oír.

Antes de pasar, dos preguntas:

**Primera:** ¿qué hook concreto vas a configurar el lunes en tu repo del trabajo? Si la respuesta es *"el de auto-format"*, perfecto — es el que más rentabilidad da en la primera semana. Si es *"el de bloquear comandos peligrosos"*, también. Cualquiera de los dos cuenta.

**Segunda:** ¿hay alguien en tu equipo de diseño con quien colaboras y que ya usa Figma? Si sí, el módulo 4 va a tener nombre y apellidos para vosotros. Y si tu equipo aún no ha tocado Claude Design, ahí está el bonus — no hace falta haber hecho diseño antes para sacarle partido.
