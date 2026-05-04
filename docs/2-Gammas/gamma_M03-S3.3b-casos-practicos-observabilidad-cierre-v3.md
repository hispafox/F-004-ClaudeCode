> **Versión:** v3 | **Módulo:** 3 | **Sub:** 3.3b | **Slides:** 35 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M03-S3.3b-casos-practicos-observabilidad-cierre-v3.md`

# Submódulo 3.3b — Casos prácticos, observabilidad, cierre módulo 3

## Slide 1 — Portada
**Módulo 3 · Submódulo 3.3 · Parte B**
Casos prácticos, observabilidad y cierre del módulo 3
Auto-formato, bloqueo de comandos peligrosos, observabilidad

---

## Slide 2 — Dónde estamos

En 3.3a vimos la teoría: por qué los hooks cierran el harness, instrucción vs garantía, anatomía, los 17 eventos, los handlers, el sistema de exit codes con `updatedInput`.

Ahora vamos a aplicarlo:

```
1. CASO PRÁCTICO 1: auto-formato y lint
2. CASO PRÁCTICO 2: bloqueo de comandos peligrosos
3. CHANNELS (referencia rápida)
4. ANTI-PATRONES + ERRORES FRECUENTES
5. OBSERVABILIDAD (sección nueva v3)
6. EL HARNESS COMPLETO: definición operativa
7. CIERRE DEL MÓDULO 3
```

---

## Slide 3 — Caso práctico 1: auto-formato y lint

**La situación:**

```
Cada vez que Claude modifica un fichero .cs
└── queremos que se ejecute dotnet format.

Cada vez que modifica un fichero .ts
└── npm run lint -- --fix.

SIN tener que pedirlo.
```

> El caso más rentable de los hooks.
>
> Lo configuras una tarde
> y al día siguiente notas la diferencia.

---

## Slide 4 — El hook inline

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

---

## Slide 5 — Lo que hace el hook inline

```
SE DISPARA después de cualquier
└── Write
└── Edit
└── MultiEdit

USA la variable de entorno
└── $CLAUDE_TOOL_INPUT_FILE_PATH
    (que Claude Code expone)
    para saber qué fichero se ha tocado.

SEGÚN LA EXTENSIÓN
ejecuta el formateador adecuado.

SI NO ES NINGUNO DE LOS CONOCIDOS
└── no hace nada
    (el case no encaja con ningún patrón).
```

> Funciona.
>
> Pero es difícil de leer y mantener.

---

## Slide 6 — Refinamiento: usar un script externo

**Mejor patrón: un script externo.**

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

---

## Slide 7 — settings.json simplificado

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

```
$CLAUDE_PROJECT_DIR
└── otra variable que Claude Code expone
    apunta a la raíz del proyecto.

El script vive en .claude/hooks/
└── el sitio convencional para guardarlos.
```

> Acuérdate de hacerlo ejecutable:
>
> ```bash
> chmod +x .claude/hooks/format-on-write.sh
> ```

---

## Slide 8 — Probar el hook

```
Lanza una sesión nueva
└── (los hooks se cargan al arrancar)
    y pide al agente que modifique un fichero .cs.

Cuando termine la modificación:
└── deberías ver el formato aplicarse automáticamente.
```

**Si no se ejecuta, debug:**

```bash
# Test manual
echo '{"tool_name":"Write","tool_input":{"file_path":"test.cs"}}' | .claude/hooks/format-on-write.sh
echo $?
```

```
Si el script funciona AISLADO
pero NO se dispara en la sesión:
└── el problema suele estar
    en el matcher o en la configuración.

USA /hooks
└── para ver lo que Claude Code ha cargado.
```

---

## Slide 9 — Caso práctico 2: bloqueo de comandos peligrosos

```
El equivalente del cinturón de seguridad.
```

```
Lo configuras UNA VEZ y te olvidas.

Pero el día que importa
└── estás vivo gracias a él.
```

---

## Slide 10 — El hook de bloqueo

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

> Matcher `Bash` con `PreToolUse`.
>
> Antes de cualquier ejecución de Bash, pasa por el script.

---

## Slide 11 — El script: lectura del comando

`.claude/hooks/block-dangerous.sh`:

```bash
#!/bin/bash

# Lee el JSON de stdin
INPUT=$(cat)
COMMAND=$(echo "$INPUT" | jq -r '.tool_input.command')
```

> El input del evento llega por stdin como JSON.
>
> Con `jq` extraes el campo del comando que va a ejecutarse.

---

## Slide 12 — El script: lista de patrones bloqueados

```bash
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

---

## Slide 13 — Lo que hace el bloqueo

```
1. ANTES de cualquier ejecución de Bash
   └── lee el comando que va a ejecutarse.

2. SI ENCAJA con cualquier patrón peligroso
   └── devuelve exit 2 → BLOQUEO ABSOLUTO.

3. SI NO ENCAJA
   └── deja pasar.
```

```
El exit 2 BLOQUEA INCLUSO EN MODO AUTÓNOMO.
```

> Aunque alguien lance Claude Code con
> `--dangerously-skip-permissions`
>
> los comandos peligrosos siguen bloqueados.
>
> Es una **garantía real**, no una recomendación.

---

## Slide 14 — Recomendación: este hook va siempre

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   Este hook va SIEMPRE.                                  │
│                                                          │
│   En tu user-level (~/.claude/settings.json)             │
│   no en el proyecto.                                     │
│                                                          │
│   Así viaja contigo a todos los repos.                   │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

```
Cuando alguna vez intentes hacer algo destructivo
y veas el bloqueo:

└── te vas a alegrar de tenerlo.
```

---

## Slide 15 — Ampliación: validación basada en LLM

El bloqueo basado en regex captura los patrones obvios. **Pero hay comandos peligrosos que no son obvios.**

```
dd if=/dev/zero of=/dev/sda
└── es destructivo
    y un regex tendría que ser muy elaborado
    para captarlo.
```

**Aquí encaja un handler de tipo `prompt`:**

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

---

## Slide 16 — Combinar ambos enfoques

```
Haiku evalúa cada comando antes de ejecutarse.

Si dice "dangerous":
└── bloqueo automático.

Más caro que el regex
(cada Bash ahora pasa por LLM)
└── pero captura cosas que un regex no captaría.
```

```
Combinar ambos enfoques es lo más prudente:

├── REGEX para los obvios
│   (rápido, gratis)
│
└── LLM para los sutiles
    (más caro pero más cobertura)
```

---

## Slide 17 — Channels: referencia rápida

El temario menciona **channels**. La pieza es real pero su uso práctico todavía es minoritario, así que lo cubrimos por encima.

```
Un CHANNEL es básicamente un MCP server
que en vez de exponer herramientas
hace PUSH HACIA LA SESIÓN de Claude Code.
```

**La inversión:**

```
EN LUGAR DE QUE CLAUDE PREGUNTE
cuando necesita algo
└── el sistema externo NOTIFICA a Claude
    cuando pasa algo.
```

---

## Slide 18 — Channels: casos típicos

```
ALERTA DE CI FALLIDO
└── El sistema de CI manda un mensaje
    al canal de Claude Code:
    "el build de la rama X ha fallado,
     aquí está el log"
    Claude lo recibe en mitad de tu sesión
    y puede analizarlo.

EVENTOS DE MONITORIZACIÓN
└── Un sistema externo detecta una anomalía
    y la notifica.

MENSAJES DE CHAT
└── Slack reenvía menciones del equipo al canal.
```

```
Configuración: como un MCP server normal
pero con la capability claude/channel.
```

> Mi recomendación honesta para este curso:
> **saber que existen, no es algo que vayas a configurar**
> **la primera semana.**

---

## Slide 19 — Anti-patrones de hooks (1/2)

```
HOOKS DEMASIADO LENTOS
└── Los hooks se ejecutan SÍNCRONAMENTE.
    Si tu PostToolUse tarda 5 segundos
    └── cada Write tarda 5 segundos más
        de lo que tardaría sin el hook.
    Mantén los hooks por debajo de 200-500ms.

MÚLTIPLES HOOKS MODIFICANDO EL MISMO INPUT
└── Los hooks corren en paralelo.
    Si dos PreToolUse intentan modificar
    el mismo tool_input.command:
    └── el orden de ejecución es no determinista
        y el último gana.
    Si tienes que modificar input
    └── hazlo desde un único hook.

HOOKS QUE DEPENDEN DEL DIRECTORIO ACTUAL
└── Si tu hook hace cd src/Web && ...
    el cd solo afecta al subshell del hook.
    Mejor usar paths absolutos
    basados en $CLAUDE_PROJECT_DIR.

NO PROBAR LOS HOOKS AISLADAMENTE
└── Acostúmbrate a testar tus hooks
    pasando JSON por stdin desde la línea de comandos.
```

---

## Slide 20 — Anti-patrones de hooks (2/2)

```
BLOQUEOS DEMASIADO AGRESIVOS
└── Un PreToolUse que devuelve exit 2 en demasiados casos
    hace que Claude Code se sienta paralizado.
    Sé selectivo con los bloqueos.

STOP HOOKS QUE CREAN BUCLES INFINITOS
└── Un Stop hook que devuelve exit 2
    fuerza a Claude a seguir trabajando.
    Si esa condición se mantiene siempre verdadera
    └── Claude nunca para.
    La solución:
    comprueba el campo stop_hook_active en el JSON
    para detectar la segunda invocación
    y dejarlo pasar.

HARDCODEAR PATHS DE SCRIPTS
└── Si tu hook llama a /Users/pedro/.claude/hooks/...
    deja de funcionar para todo el equipo.
    Usa $CLAUDE_PROJECT_DIR/.claude/hooks/...

OLVIDAR chmod +x
└── Cuando creas un script de hook nuevo
    y no se ejecuta:
    lo primero que comprobar
    └── ls -l para ver si tiene permisos.
```

---

## Slide 21 — Errores frecuentes con tus primeros hooks

```
❌ EDITAR settings.json CON SESIÓN ABIERTA
   El file watcher normalmente recoge los cambios.
   Pero si no: REINICIA LA SESIÓN.

❌ CONFUNDIR PreToolUse CON PostToolUse
   Pre = antes, puede bloquear.
   Post = después, no puede deshacer pero puede dar feedback.
   Si quieres bloquear: Pre.
   Si quieres reaccionar: Post.

❌ ESPERAR QUE PermissionRequest FUNCIONE EN MODO -p
   En modo no interactivo
   usa PreToolUse para automatizar decisiones de permiso.

❌ NO REVISAR /hooks PARA CONFIRMAR LO QUE ESTÁ CARGADO
   Si cambias algo y no funciona:
   /hooks te dice qué tiene Claude Code cargado.
   La mitad de las veces, no es lo que crees.

❌ OLVIDARSE DE QUE HOOKS APLICAN A SUBAGENTES TAMBIÉN
   Si tienes un PreToolUse que valida algo
   también valida cuando un subagente lo invoca.

❌ HOOKS QUE LLAMAN A HERRAMIENTAS QUE NO ESTÁN EN EL PATH
   Tu .zshrc puede tener PATH custom
   pero el subshell del hook puede no tenerlos.
   Usa paths absolutos o configura PATH en el script.
```

---

## Slide 22 — Observabilidad: la pieza que cierra el harness fiable

Hay una pregunta que aparece la primera vez que algo se rompe en mitad de un workflow compuesto:

```
Te llega un mensaje del lead diciendo
que un subagente devolvió algo raro
y el código pushed está mal.

Vas a /usage:
└── la sesión consumió un montón de tokens.

Y te toca reconstruir qué pasó:
├── ¿en qué subagente se torció la cosa?
├── ¿qué decisión tomó el orquestador?
└── ¿en qué iteración del loop validator
    se decidió aprobar?
```

```
Sin logs estructurados
└── la respuesta es "vete a saber".

Y en agentes, eso NO VALE.
```

> Aquí entra la **OBSERVABILIDAD**.
>
> Es la pieza que faltaba para cerrar el harness.

---

## Slide 23 — Por qué es distinto en agentes

Con código tradicional, debugger más stack trace más logs te alcanzan.

```
Con AGENTES, NO.
```

**Tres razones:**

```
1. NO DETERMINISMO
   Lanzas el mismo workflow dos veces
   y las decisiones del orquestador no son idénticas.
   No puedes "reproducir" un fallo
   con la misma fiabilidad que reproduces un null pointer.

2. DECISIONES OPACAS
   El razonamiento del agente vive dentro del modelo.
   Cuando algo va mal:
   └── no tienes un branch del código que mirar
       tienes que reconstruir qué pensó el agente
       y eso solo lo sabes si lo logueaste.

3. CADENAS LARGAS
   Skill orquestador → subagente A → subagente B → MCP server.
   Cada eslabón es un punto de fallo.
   Si no sabes qué pasó en cada uno
   └── debugging es ADIVINACIÓN.
```

---

## Slide 24 — La regla práctica de observabilidad

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   Los agentes que llegan a producción                    │
│   tienen siempre algún sistema de observabilidad.        │
│                                                          │
│   Los que no, no llegan.                                 │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

---

## Slide 25 — Qué loggear

Lo que merece la pena tener registrado para cada sesión seria:

```
DECISIONES DEL ORQUESTADOR
└── Cuándo decidió invocar a qué subagente
    con qué parámetros
    y cuál fue la respuesta.
    └── Es el LOG PRINCIPAL del workflow.

INVOCACIONES A SUBAGENTES
├── Modelo usado (haiku/sonnet/opus)
├── Tokens consumidos
├── Latencia
└── Si terminó con éxito o falló.

ITERACIONES DE LOOPS
└── Cuántas vueltas dio el validator → implementer
    antes de converger.
    Si un loop necesita las 3 iteraciones cada vez
    └── el problema está en el subagente
        no en el flujo.

ERRORES Y BLOQUEOS DE HOOKS
└── Qué se bloqueó y por qué.
    Útil para auditar políticas de seguridad
    y detectar falsos positivos.

COSTE POR WORKFLOW
└── Tokens agregados por sesión
    agrupados por subagente.
    Sin esto: descubrir cuál de tus subagentes
    engorda la factura es imposible.
```

---

## Slide 26 — Cómo se hace con hooks

```
Los hooks NO son solo automatización.

Son EL MECANISMO NATURAL de instrumentación.
```

**Tres eventos te dan el grueso:**

```
SessionEnd
└── Al cerrar una sesión
    vuelca un resumen a un fichero de log:
    ├── tokens
    ├── subagentes invocados
    └── duración.
    Tu fila en la "base de datos" de sesiones.

PostToolUse
└── Tracing por tool call.
    Cada vez que el agente ejecuta una herramienta
    un hook registra:
    ├── qué fue
    ├── con qué input
    └── cuánto tardó.

SubagentStop
└── El más específico.
    Se dispara cuando un subagente termina.
    Útil para registrar su salida
    sin que el log se ensucie con cada operación intermedia
    que hizo por dentro.
```

---

## Slide 27 — Hook concreto de logging

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

> A partir de ahí, los datos del fichero `sessions.jsonl`
> van a Datadog, Grafana, Splunk, o tu sistema interno.
>
> La pieza importante no es el destino:
> es **tener los datos**.

---

## Slide 28 — Conexión con el context bank

En 3.2b viste el **context bank** — los ficheros markdown bajo `.claude/workflow-state/<feature>/` que sirven de memoria compartida entre subagentes.

```
El context bank ya es, de facto,
medio LOG del workflow:

├── Te dice qué pensó el planner
├── Qué exploró el explorer
└── Qué hallazgos sacó el reviewer
```

```
Para workflows compuestos
NO necesitas montar logging desde cero
└── el context bank ya está ahí.

Lo que añaden los HOOKS DE OBSERVABILIDAD
es LA CAPA TRANSVERSAL:
├── tokens
├── latencia
├── eventos de sistema
└── métricas agregadas.
```

> Las dos capas juntas
> — context bank + hooks de logging —
>
> te dan **trazabilidad real**
> de lo que pasó en cada sesión y por qué.
>
> Y con esto se cierra el harness.

---

## Slide 29 — El harness completo: definición operativa

Cierre del módulo 3. Llegado a este punto tienes el harness entero.

> **Una definición operativa que vale la pena memorizar:**
>
> ## **harness = prompts + tools + context policies + hooks + feedback loops + observability**

---

## Slide 30 — Cada pieza la tienes ya

```
PROMPTS
└── CLAUDE.md (módulo 1), skills (módulo 2).
    Lo que el agente sabe sobre cómo trabaja tu equipo.

TOOLS
└── Bash, Read, Edit, MCP servers (módulos 1, 2 y 4).
    Lo que el agente puede ejecutar.

CONTEXT POLICIES
└── Subagentes con tools restringidos
    scopes (user/proyecto)
    permisos (3.1).
    Lo que el agente puede tocar y cuándo.

HOOKS
└── Automatización determinista.
    Lo que pasa siempre.

FEEDBACK LOOPS
└── Validators que devuelven el flujo
    context bank como memoria compartida (3.2).
    Lo que hace que el harness se autocorrija.

OBSERVABILITY
└── Hooks de logging y métricas.
    Lo que hace que el harness sea depurable.
```

---

## Slide 31 — La idea final

```
SKILLS personalizan lo que el agente sabe.

SUBAGENTES definen roles especializados.

LA ORQUESTACIÓN los combina con loops y context bank.

LOS HOOKS aseguran que las cosas mecánicas pasen siempre
sin depender de razonamiento.
```

```
Eso, junto, es:

TU HARNESS SOBRE CLAUDE CODE.
```

> En el portal de recursos del curso encontrarás
> el cheatsheet visual del patrón Agent Harness.
>
> Conviene tenerlo a mano las primeras semanas
> mientras decides qué pieza usar para cada cosa.

---

## Slide 32 — Lo que viene en el módulo 4

En el módulo 4 cambiamos de tema completamente:

```
Hasta aquí hemos hablado del agente y su personalización.
└── El harness entero.

Ahora entramos en cómo Claude Code se integra
con EL FLUJO DE DISEÑO.
```

```
MÓDULO 4 — FIGMA MCP Y CLAUDE DESIGN

├── Cómo trabajar con diseños existentes
│   a través del MCP de Figma.
│
├── Cómo usar Claude Design
│   para creación visual conversacional.
│
└── Cómo el formato emergente DESIGN.md
    está reconfigurando la forma en que los agentes
    consumen design systems.
```

---

## Slide 33 — Lectura complementaria opcional

Si en tu rol toca decidir arquitecturas a nivel sistema — no solo usar Claude Code como dev — Anthropic publicó un whitepaper que conviene tener a mano:

```
"Building Effective AI Agents:
 Architecture Patterns and Implementation Frameworks"

resources.anthropic.com/building-effective-ai-agents
```

```
Cubre los mismos patrones que has visto aquí
en versión más teórica:

├── hierarchical
├── collaborative
├── sequential
├── parallel
└── evaluator-optimizer
```

> El vocabulario formal que aquí hemos pincelado de pasada.
>
> No es requisito del curso.
> Es para cuando alguien arriba pida un
> *"diseño de sistema multi-agente"*
> y quieras ir con los términos que esa persona espera oír.

---

## Slide 34 — Antes de pasar: dos preguntas

**Primera:**

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   ¿Qué hook concreto vas a configurar el lunes           │
│   en tu repo del trabajo?                                │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

```
Si la respuesta es "el de auto-format":
└── perfecto. Es el que más rentabilidad da
    en la primera semana.

Si es "el de bloquear comandos peligrosos":
└── también.

CUALQUIERA DE LOS DOS CUENTA.
```

**Segunda:**

```
¿Hay alguien en tu equipo de diseño
con quien colaboras y que ya usa Figma?
```

```
Si sí:
└── el módulo 4 va a tener nombre y apellidos
    para vosotros.

Y si tu equipo aún no ha tocado Claude Design:
└── ahí está el bonus.
    No hace falta haber hecho diseño antes
    para sacarle partido.
```

---

## Slide 35 — Cierre del módulo 3

```
✅ MÓDULO 3 COMPLETO

3.1 — SUBAGENTES
├── Built-in: Explore, Plan, general-purpose
└── Custom: cuatro casos típicos

3.2 — ORQUESTACIÓN
├── context: fork
├── Composición de capas
├── Loops con techo
├── Context bank
├── Paralelo vs serial
├── Claude Code como MCP
└── Agent Teams

3.3 — HOOKS
├── Anatomía y eventos
├── Casos prácticos: format + bloqueo
├── Channels (referencia)
└── Observabilidad
```

> Con esto tienes el harness completo:
>
> **prompts + tools + context policies + hooks + feedback loops + observability**

**Nos vemos en el módulo 4.**
