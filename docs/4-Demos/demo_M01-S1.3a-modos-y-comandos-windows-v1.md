# Demo 1.3a — Tres modos de uso, slash commands y `/compact` en OrderManagement

> **Versión:** v1 | **Módulo:** 1 | **Sub:** 1.3a | **Estado:** ✅ Versión final
> **Archivo:** `demo_M01-S1.3a-modos-y-comandos-windows-v1.md`
> **Branch before:** `demo/1.3a-before`  (estado al hacer `git checkout` antes de grabar)
> **Branch after:**  `demo/1.3a-after`   (estado final que la siguiente clase asume)
> **Branch parent:** `demo/1.2b-after`
> **Tiempo total estimado:** ~22-26 minutos
> **Tipo:** Demo de mecánica operativa (INFRA). **Aquí el alumno ve los tres modos de uso (interactivo, one-shot, pipe) en escenarios reales sobre OrderManagement, y los slash commands más útiles del día a día — con foco especial en `/compact` que la gamma marcó como "el más rentable en producción".** Sigue el patrón **before/after** definido en [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md).
> **Plataforma:** Windows 11 (PowerShell 7 + Git Bash disponibles).

---

## 1. Contexto

Llegamos al "día 2" del curso. Ya tenéis Claude Code instalado, autenticado y con un `CLAUDE.md` decente para OrderManagement. La rama `demo/1.2b` queda como base estructural — todas las demos de aquí en adelante parten de tener configuración real del proyecto.

La gamma 1.3a (33 slides, ~30-35 min de teoría) cubre lo que el manual llama "las otras marchas" del coche. La analogía es del slide 3 y conviene tenerla presente: la mayoría de gente al llegar al día 2 abre `claude`, escribe lo que se le ocurre, y navega a base de prueba y error. Llegan a destino. Pero están haciendo cuestas a 20 km/h. La gente que cambia de marchas — modo one-shot para automatizar, modo pipe para procesar, slash commands para gestionar la sesión — llega antes y disfruta más el coche.

Esta demo aterriza la gamma sobre OrderManagement. **No es la demo más espectacular del módulo 1 — esa fue la 1.2b** — pero es la que más cambia los hábitos del alumno la primera semana. Si la 1.2b decide si el agente entiende el repo, la 1.3a decide si el alumno usa la herramienta o pelea contra ella.

> **Tipo de demo:** mecánica operativa pura. El alumno ve los tres modos en acción y los diez slash commands más usados, con énfasis especial en `/compact` y `/plan`.

---

## 2. Objetivo de la demo

Cinco cosas concretas que tienen que quedar en la cabeza del alumno cuando termine los ~22 minutos de screencast:

1. **Los tres modos no son alternativas — son herramientas distintas.** Interactivo para trabajar. One-shot para automatizar. Pipe para procesar datos que ya tienes. Cada uno tiene su sitio. El que usa solo el interactivo está dejando un tercio del valor de la herramienta fuera.

2. **`/compact` es el comando más rentable en sesiones largas.** No es una optimización fina — es la diferencia entre una sesión de tres horas que rinde y una sesión de tres horas donde el agente "se ha vuelto tonto" en la última hora. Cada veinte o treinta minutos, `/compact`. Sin excusas.

3. **`/plan` para tareas que tocan más de tres ficheros.** Sin esto, el agente a veces toma direcciones equivocadas y te das cuenta diez minutos después. Con esto, revisas el plan en treinta segundos antes de que toque nada.

4. **El antipatrón estrella: usar `/clear` cuando deberías usar `/compact`.** Es el error número uno del primer día. `/clear` borra todo el contexto. `/compact` lo conserva resumido. Si la tarea no ha terminado, **siempre `/compact`**.

5. **`/usage` cada veinte o treinta minutos.** No es opcional si trabajas en plan Pro y haces sesiones largas. La gamma 1.3a slide 18 lo dijo: tocar el límite con una tarea a medias es desagradable. La prevención cuesta dos segundos.

Y dos cosas que tienen que **NO quedar** en su cabeza:

- *"Tengo que aprender los 60+ slash commands."* — no, los **diez** que cubre la gamma 1.3a son el 90% del uso real. Los demás los descubrís cuando los necesitéis.
- *"El modo pipe es solo para devops."* — no, es la mejor forma de hacer code review asistido antes de un PR. Cualquier dev se beneficia.

---

## 3. Branch `demo/1.3a-before`

Punto de partida del screencast.

```
demo/1.3a-before
```

**Parte de:** `demo/1.2b-after`.

**Estado del repo:** `CLAUDE.md` en raíz con los cinco bloques (visión, estructura, comandos, convenciones .NET, convenciones Angular, reglas duras). `.claude/settings.json` con `allow` y `deny` configurados. `.gitignore` excluye `.claude/settings.local.json`. Esto importa porque **los permisos del bloque 1.2b son los que vamos a ver activarse durante esta demo** — `dotnet build` pasa sin pedir aprobación porque está en `allow`, etc.

**Qué NO hay todavía en `-before`:** ni `scripts/audit-staged.sh`, ni la marca `[x]` de la 1.3a en `docs/DEMOS.md`. Esos son los artefactos que la demo materializa.

> El formador hace `git checkout demo/1.3a-before` antes de empezar a grabar.

---

## 4. Branch `demo/1.3a-after`

Estado final que la siguiente clase (1.3b) asume.

```
demo/1.3a-after
```

**Parte de:** `demo/1.3a-before`.

**Qué añade respecto a `-before`:** dos cosas pequeñas al repo — un fichero `scripts/audit-staged.sh` (script bash de ejemplo para hook de pre-commit, no instalado activamente) y la marca `[x]` en `docs/DEMOS.md`. **Lo importante de la demo no está en el repo — está en los hábitos que el alumno se lleva**. Como la 1.2a, esta demo añade poco material al repo y mucho al alumno.

**Cómo se prepara:** ver §6b. Se materializa antes de grabar.

> Durante la grabación, el formador parte de `demo/1.3a-before`, ejecuta los tres modos + los slash commands en directo, y al cerrar descarta los cambios reales. La siguiente clase parte de `demo/1.3a-after` ya pre-cocinada.

---

## 5. Estado del repo al empezar

Idéntico a la rama `demo/1.2b`. La estructura sigue siendo:

```
ordermanagement/
├── .claude/
│   └── settings.json                   (con allow/deny configurados)
├── docs/
│   └── DEMOS.md                        (con 1.1, 1.2a, 1.2b marcadas)
├── src/                                (sin cambios)
├── frontend/                           (sin cambios)
├── tests/                              (sin cambios)
├── .gitignore                          (excluye settings.local.json)
├── CLAUDE.md                           (147 líneas, 5 bloques)
└── README.md                           (descripción del proyecto)
```

**Estado de la máquina Windows del formador:**

```
✅ Claude Code instalado y autenticado
✅ Git for Windows
✅ PowerShell 7
✅ Git Bash disponible (viene con Git for Windows)
✅ VS Code con el repo cargado en demo/1.2b
✅ CLAUDE.md y .claude/settings.json operativos
```

**Lo que el alumno verá al final de la demo:**

- Los tres modos de uso (interactivo, one-shot, pipe) en acción sobre OrderManagement con escenarios reales.
- Los diez slash commands esenciales — `/help`, `/init`, `/clear`, `/compact`, `/usage`, `/model`, `/permissions`, `/mcp`, `/agents`, `/plan` — usados cuando aplican.
- Una sesión interactiva de ~quince minutos simulada/comprimida donde se lanza `/compact` y se ve el efecto.
- `/plan` aplicado a una tarea de cuatro ficheros para mostrar la diferencia entre tener plan y no tenerlo.
- Un ejemplo concreto de modo pipe con `git diff` para code review pre-PR.
- Un fichero `scripts/audit-staged.sh` que **se enseña pero no se instala** como hook de pre-commit (ejemplo, no se ejecuta en CI todavía).

---

## 6. Prompt para Claude Code

> Lo que tú, formador, copias y pegas en Claude Code para preparar la rama `demo/1.3a` antes de grabar.
>
> **Importante:** este prompt prepara la rama con el script de ejemplo y la actualización del `docs/DEMOS.md`. **El script de pre-commit NO se instala activamente en `.git/hooks/`** — solo queda en `scripts/` como ejemplo que el alumno puede llevarse.

````
Estoy preparando la demo 1.3a del curso de Claude Code para devs .NET +
Angular. Esta demo cubre los tres modos de uso (interactivo, one-shot,
pipe), los diez slash commands esenciales, y /compact en profundidad
sobre el proyecto OrderManagement.

# Contexto

Estoy en la rama `demo/1.2b-after` del repo del curso `F-004-ClaudeCode`.
El proyecto demo en `ordermanagement/` ya tiene `ordermanagement/CLAUDE.md`,
`ordermanagement/.claude/settings.json` con allow/deny, y `.gitignore` raíz
actualizado. La estructura del código no cambia desde la 1.2b.

Quiero que prepares la rama `demo/1.3a-after` con un cambio mínimo:
añadir un script de ejemplo en `ordermanagement/scripts/` que muestre cómo se
usaría Claude Code en modo one-shot dentro de un hook de pre-commit. NO se
instala activamente, solo queda como ejemplo.

# Lo que necesito

Cuatro tareas:

## Tarea 1: crear las ramas

```powershell
git checkout demo/1.2b-after
git pull
git checkout -b demo/1.3a-before
git checkout -b demo/1.3a-after
```

(La rama `demo/1.3a-before` queda idéntica a `demo/1.2b-after`; el screencast
arranca de ahí. Todo el cambio del repo va en `-after`.)

## Tarea 2: crear `ordermanagement/scripts/audit-staged.sh`

Si la carpeta `ordermanagement/scripts/` no existe, créala.

Contenido del fichero `ordermanagement/scripts/audit-staged.sh`:

```bash
#!/bin/bash
#
# audit-staged.sh
# Ejemplo de uso de Claude Code en modo one-shot dentro de un hook
# de pre-commit. NO está instalado por defecto en .git/hooks/.
#
# Para activarlo como hook de pre-commit:
#   cp scripts/audit-staged.sh .git/hooks/pre-commit
#   chmod +x .git/hooks/pre-commit
#
# En Windows con Git for Windows, los hooks shell funcionan
# directamente. No requiere PowerShell.

set -e

# Capturamos el diff de lo staged
DIFF=$(git diff --cached --diff-filter=AM)

if [ -z "$DIFF" ]; then
    echo "No hay cambios staged. Saltando audit."
    exit 0
fi

# Pipeamos el diff a Claude Code en modo one-shot
echo "Auditando cambios staged con Claude Code..."

RESULT=$(echo "$DIFF" | claude -p "Audita este diff staged buscando: bugs evidentes, problemas de seguridad (credenciales, inyección, XSS), violaciones de las convenciones del CLAUDE.md, y código sin tests cuando aplique.

Si no detectas problemas críticos, responde solo: OK
Si detectas algo, lista cada problema con: fichero, línea aproximada, severidad (BAJA/MEDIA/ALTA/CRÍTICA), descripción breve.")

# Si la respuesta es OK, dejamos pasar
if [ "$RESULT" = "OK" ]; then
    echo "✓ Audit OK. Commiteando."
    exit 0
fi

# Si hay hallazgos, los mostramos y dejamos que el dev decida
echo ""
echo "=========================================="
echo "Hallazgos del audit de Claude Code:"
echo "=========================================="
echo "$RESULT"
echo "=========================================="
echo ""

# Pedimos confirmación al dev
read -p "¿Quieres commitear de todas formas? (s/N): " -n 1 -r CONFIRM
echo ""

if [[ "$CONFIRM" =~ ^[SsYy]$ ]]; then
    echo "Commiteando bajo tu responsabilidad."
    exit 0
else
    echo "Commit abortado por hallazgos del audit."
    exit 1
fi
```

## Tarea 3: actualizar docs/DEMOS.md

Localiza la línea:

```
- [ ] demo/1.3a-before / demo/1.3a-after — Tres modos de uso, slash commands, /compact
```

Y cámbiala por:

```
- [x] **demo/1.3a-before / demo/1.3a-after** — Tres modos de uso, slash commands, /compact
```

## Tarea 4: verificar y commitear

```powershell
Set-Location c:\w\repos\F-004-ClaudeCode\ordermanagement
dotnet build
```

Esperado: 0 warnings, 0 errors. (El script no afecta al build pero
verificamos que no hemos roto nada accidentalmente.)

```powershell
Set-Location c:\w\repos\F-004-ClaudeCode
git add ordermanagement/scripts/audit-staged.sh docs/DEMOS.md
git commit -m "demo/1.3a-after: script de ejemplo para audit pre-commit"
```

NO hagas push.

# Restricciones

- NO instales el script activamente en .git/hooks/. Solo queda en
  `ordermanagement/scripts/` como ejemplo. Los hooks reales se cubren
  en el módulo 3 (3.3a).
- NO añadas skills, subagentes ni hooks de Claude Code (los del módulo 3).
- NO toques el código de la app, ni los .csproj, ni Program.cs.
- NO modifiques `ordermanagement/CLAUDE.md` ni `ordermanagement/.claude/settings.json` (vienen de la 1.2b).
- NO modifiques README.md (ni el del curso ni el de `ordermanagement/`).
- El script debe ser bash compatible con Git for Windows.

# Cuando termines, dime

1. Que las ramas demo/1.3a-before y demo/1.3a-after están creadas desde demo/1.2b-after.
2. Que `ordermanagement/scripts/audit-staged.sh` está creado en demo/1.3a-after.
3. Que docs/DEMOS.md tiene la 1.3a marcada.
4. Que el build pasa.
5. Que el commit está hecho en demo/1.3a-after.

Si tienes dudas, para y pregúntame.
````

---

## 7. Artefactos que Claude Code debe generar

```
✓ Rama nueva: demo/1.3a (parte de demo/1.2b)
✓ scripts/audit-staged.sh creado (script bash de ejemplo)
✓ docs/DEMOS.md con 1.3a marcada como [x]
✓ Verificación de build OK: dotnet build limpio
✓ Commit único: "demo/1.3a: script de ejemplo para audit pre-commit"
```

**Lo que NO debe haber generado:**

- ❌ Hook activo en `.git/hooks/pre-commit` (el script solo queda en `scripts/`)
- ❌ Cambios en `CLAUDE.md` o `.claude/settings.json` (vienen de 1.2b)
- ❌ Skills, subagentes, hooks de Claude Code (módulo 3)
- ❌ Cambios en código de la app
- ❌ Cambios en README.md

> Si Claude Code instala activamente el hook en `.git/hooks/`, **se rechaza el output**. Los hooks de git son del módulo 3 cuando se cubre la observabilidad operativa. Aquí solo queda como ejemplo en `scripts/`.

**Estado final del árbol después del prompt:**

```
ordermanagement/
├── .claude/
│   └── settings.json               (sin cambios desde 1.2b)
├── docs/
│   └── DEMOS.md                    ← MODIFICADO (1 línea)
├── scripts/
│   └── audit-staged.sh             ← NUEVO
├── src/                            (sin cambios)
├── frontend/                       (sin cambios)
├── tests/                          (sin cambios)
├── .gitignore                      (sin cambios)
├── CLAUDE.md                       (sin cambios)
└── README.md                       (sin cambios)
```

---

## 8. Guion del screencast

**Duración estimada del screencast: ~20-23 minutos.**

Diez bloques. Esta demo es densa pero no tan pesada como la 1.2b porque ya no hay material conceptual nuevo — solo demostración operativa.

> **Antes de empezar a grabar**, asegúrate de:
> - Tener PowerShell 7 abierto en pantalla completa, fuente al menos 16pt.
> - Tener VS Code abierto al lado con el repo `ordermanagement` cargado en `demo/1.3a`.
> - **Importante:** asegúrate de que **Git Bash está accesible** desde PowerShell (basta tener Git for Windows instalado — viene incluido).
> - Cerrar Slack, Teams, navegadores con notificaciones.
> - Tener **dos terminales** abiertas en paralelo: una para PowerShell con Claude Code interactivo, otra para PowerShell o Git Bash para ejecutar one-shot y pipe. Esto facilita el flujo entre bloques.

---

### Bloque 1 — Setup y la analogía del coche manual (~1 min 30 seg)

**Pantalla compartida.** A la izquierda, VS Code con el repo en `demo/1.3a`. A la derecha, dos ventanas de PowerShell limpias.

**Antes de teclear nada,** abres una de las dos terminales y muestras donde estás:

```powershell
git status
```

```
On branch demo/1.3a
nothing to commit, working tree clean
```

**Lo que dices:**

> "Estamos en la rama `demo/1.3a`. Toda la configuración de la 1.2b está vigente — `CLAUDE.md` con los cinco bloques, `settings.json` con los permisos, `.gitignore` con `settings.local.json` excluido. Si miráis el árbol, veréis que la única diferencia con la 1.2b es un script de ejemplo en `scripts/` que veremos al final.
>
> La gamma 1.3a empezó con una analogía que conviene retomar antes de empezar. La del coche manual. La gente que llega al día 2 con Claude Code suele lanzar `claude`, abrir sesión interactiva, y trabajar desde ahí. Punto. Y así llega a destino. **Pero está haciendo cuestas a veinte kilómetros por hora**, desgastando el embrague.
>
> Esta demo son las otras marchas. Vamos a ver:
>
> Uno, los tres modos de uso — interactivo, one-shot, pipe — y cuándo usar cada uno. Dos, los diez slash commands que de verdad cambian el día a día. Tres, `/compact` en profundidad porque es el que más rentabilidad da en sesiones largas. Y cuatro, `/plan` aplicado a tareas reales.
>
> Vamos a ello."

**Tiempo:** ~90 segundos.

---

### Bloque 2 — Modo interactivo: el que ya conocéis (~1 min 30 seg)

> "Empezamos por lo conocido — modo interactivo. Es el que hemos usado en las demos 1.1 y 1.2b. Lo recogemos rápido para que quede en el mapa mental."

**Tecleas en la terminal izquierda:**

```powershell
claude
```

Aparece el banner:

```
 Welcome to Claude Code v2.1.x

 cwd: C:\Users\pedro\projects\ordermanagement
 model: claude-opus-4.7
 ✓ CLAUDE.md loaded (147 lines)
 ✓ Project settings loaded
 Type / for commands, ? for help

>
```

> "Mirad las dos líneas con check verde: **`CLAUDE.md loaded`** y **`Project settings loaded`**. La primera carga el contexto del proyecto. La segunda carga los permisos del `settings.json`. Ambas vienen de lo que configuramos en la 1.2b. Sin esa configuración, no veríais estas líneas.
>
> Modo interactivo. Sesión continua. Lo que dijisteis hace veinte minutos sigue presente. Las decisiones que tomasteis siguen vigentes. **Es el modo que más rentabilidad da en tareas medianas y grandes.** Si una tarea va a tomar más de cinco minutos, modo interactivo.
>
> Vamos a usarlo más adelante para ver `/plan` en acción. De momento lo dejamos abierto y vamos a ver los otros dos modos en la otra terminal."

> Dejas la terminal izquierda con el prompt de Claude Code abierto. Vas a la terminal derecha.

**Tiempo:** ~90 segundos.

---

### Bloque 3 — Modo one-shot: pregunta puntual (~2 min)

> "Modo one-shot. Una instrucción, ejecuta, devuelve resultado, se cierra. **Sin sesión, sin memoria, sin nada que recordar.** Es como mandar un email — preguntas algo, respondes, cierras."

**Tecleas en la terminal derecha:**

```powershell
claude -p "¿Cuántos endpoints REST tiene el OrdersController? Lista las rutas exactas."
```

> "El flag `-p` es de 'print' — pídele algo, imprime la respuesta, sal. **No abre sesión.** Eso es lo importante."

**Pulsas Enter. Aparece la respuesta directamente:**

```
El OrdersController tiene 5 endpoints REST:

- GET    /api/orders
- GET    /api/orders/{id}
- POST   /api/orders
- PUT    /api/orders/{id}
- DELETE /api/orders/{id}

Todos siguen el patrón CQRS con MediatR (según CLAUDE.md).
```

> "Y ya está. Diez segundos. Sin abrir sesión. **Si yo abriera Claude Code en modo interactivo solo para preguntar esto, sería matar moscas a cañonazos.** El one-shot es la herramienta correcta.
>
> Casos donde rinde:
>
> Una pregunta puntual mientras estás trabajando en otra cosa. *'¿Cuántos métodos tiene esta clase?'*. *'¿Qué imports tiene este fichero?'*. **One-shot.**
>
> Cuando quieres meter Claude Code dentro de un script o pipeline. La gamma slide 7 al 9 vio tres ejemplos: pre-commit, CI, batch. Vamos a ver el primero al final de la demo en `scripts/audit-staged.sh`.
>
> Y cuando estás encadenando comandos. Eso es el modo pipe. Vamos a verlo ahora."

**Tiempo:** ~2 minutos.

---

### Bloque 4 — Modo pipe: code review pre-PR (~3 min)

> "Modo pipe. Cuando el input ya está en algún lado y solo queréis analizarlo. La gamma slide 10. **Cualquier comando que produzca texto se puede pipear a Claude Code.**
>
> El caso de uso favorito de la mayoría de devs que adoptan esto: **code review asistido antes de un PR**. Vamos a verlo en vivo."

> Para que el ejemplo tenga material que revisar, **simulamos primero un cambio en el código** que será revisado.

**Cambias temporalmente a la rama y haces un cambio en vivo:**

```powershell
# Creamos una rama temporal para simular trabajo
git checkout -b temp/feature-pre-review
```

**Abres `src/OrderManagement.Application/Handlers/CreateOrderHandler.cs` en VS Code y le metes un cambio que tenga un par de problemas detectables. Algo como:**

```csharp
// Añade en mitad del handler una línea sin sentido:
public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
{
    // Validamos el customer
    var customer = _customerRepository.GetByIdAsync(request.CustomerId).Result; // ← .Result deliberado
    
    if (customer == null)
        throw new Exception("Customer not found"); // ← excepción genérica deliberada
    
    // ... resto del handler intacto ...
}
```

**Lo señalas brevemente con el cursor:**

> "He metido dos cosas a propósito que rompen las reglas del CLAUDE.md. Primero: `.Result` en una llamada async — la regla del CLAUDE.md dice **nunca `.Result`, nunca `.Wait()`**. Segundo: una excepción genérica `throw new Exception` en lugar de la tipada — el CLAUDE.md dice **nada de catch genérico, excepciones tipadas**. Vamos a ver si el modo pipe lo detecta sin que yo le diga nada."

**Vuelves a la terminal. Stageas el cambio:**

```powershell
git add src/OrderManagement.Application/Handlers/CreateOrderHandler.cs
```

> "Ahora el comando del modo pipe. Pasamos `git diff --cached` a Claude Code y le pedimos code review."

**Tecleas:**

```powershell
git diff --cached | claude -p "Audita este diff staged buscando: bugs evidentes, violaciones de las convenciones del CLAUDE.md, y problemas de seguridad. Lista cada problema con fichero, línea aproximada, severidad y descripción breve."
```

> "Punto a destacar: **`git diff --cached`** es el diff de lo que está staged. **`|`** es el pipe — cualquiera que haya usado Linux o PowerShell lo conoce. Y `claude -p` ahora recibe el diff por stdin. **Cero ficheros explícitos. Cero argumentos extra.**"

**Pulsas Enter. Aparece el análisis:**

```
Auditando diff staged...

Hallazgos:

1. src/OrderManagement.Application/Handlers/CreateOrderHandler.cs:25
   Severidad: ALTA
   Uso de `.Result` en una llamada async. Esto viola la regla
   del CLAUDE.md: "Async/await siempre. Nunca .Result ni .Wait()."
   Además puede causar deadlocks bloqueando el thread del context.
   Sugerencia: cambiar a `await _customerRepository.GetByIdAsync(
   request.CustomerId, cancellationToken)`.

2. src/OrderManagement.Application/Handlers/CreateOrderHandler.cs:28
   Severidad: ALTA
   Uso de excepción genérica `throw new Exception(...)`. Esto viola
   la regla del CLAUDE.md: "Manejo de errores: excepciones tipadas
   en Application... Nada de catch genérico con `Exception ex`."
   Sugerencia: usar `CustomerNotFoundException(request.CustomerId)`
   que ya está definida en src/OrderManagement.Application/Exceptions/.

3. src/OrderManagement.Application/Handlers/CreateOrderHandler.cs:25
   Severidad: MEDIA (relacionada con el #1)
   No se propaga el CancellationToken a la llamada del repositorio.
   El CLAUDE.md indica: "CancellationToken propagado desde la firma
   del controller hasta el repositorio."

Total: 3 hallazgos, 2 ALTA, 1 MEDIA.
```

> "Mirad esto. Detecta los dos problemas que metí intencionalmente, los marca con la severidad correcta, **y referencia las reglas del CLAUDE.md por su contenido**. Y de paso me detecta un tercero — la falta de propagación del `CancellationToken` — que ni siquiera había planeado pero que también viola las reglas. **Code review en treinta segundos. Antes de pedírselo a un humano.**
>
> Esto en plan Pro consume tokens, sí. Pero ahorrarse pedirle a un compañero que dedique veinte minutos a una primera pasada de code review compensa con creces.
>
> Limpio el cambio antes de seguir:"

**Descartas el cambio:**

```powershell
git reset HEAD src/OrderManagement.Application/Handlers/CreateOrderHandler.cs
git checkout -- src/OrderManagement.Application/Handlers/CreateOrderHandler.cs
git checkout demo/1.3a
git branch -D temp/feature-pre-review
```

> "Vuelvo a la rama de la demo. La rama temporal se descarta. Los tres modos vistos. **Interactivo para sesiones largas. One-shot para preguntas puntuales y automatización. Pipe para procesar lo que ya tienes.** Vamos a los slash commands."

**Tiempo:** ~3 minutos.

---

### Bloque 5 — Slash commands: panorama y los más usados (~3 min 30 seg)

> "Hay más de sesenta slash commands integrados en Claude Code. **No los vamos a ver todos.** La gamma 1.3a marcó los diez que cambian el día a día. Vamos a por ellos en orden, tirando del modo interactivo que dejamos abierto."

> Vuelves a la terminal izquierda donde Claude Code está esperando.

**En el prompt de Claude Code, tecleas:**

```
/help
```

Aparece la lista de comandos disponibles:

```
Available slash commands:

  /help       Show this help message
  /init       Generate a CLAUDE.md from the current repo
  /clear      Clear the current conversation context
  /compact    Compact the conversation history
  /usage      Show context usage and quota
  /cost       Alias for /usage
  /model      Change the active model
  /permissions  Manage runtime permissions
  /mcp        Manage MCP servers
  /agents     List available subagents
  /plan       Switch to planning mode
  /review     Review staged changes
  /test       Run project tests
  ... (54 more)

Type /<partial> + Tab for autocompletion.
```

> "Esto es **`/help`**. La primera parada cuando no recuerdas algo. Y un truco que casi nadie usa: si tecleo `/h` y pulso Tab, autocompleta y filtra solo los que empiezan por h. **Filtrado en vivo.**"

**Demuestras el filtrado:**

```
/u<Tab>
```

Aparece:

```
/usage
```

> "Ahí está. **`/usage`** — ya lo tengo. Lo lanzamos."

**Pulsas Enter:**

```
/usage
```

Aparece:

```
Current session usage:

  Context:        12,450 / 200,000 tokens (6.2%)
  Today:          1,200,000 tokens used
  Plan: Claude Max
  Quota:          50,000,000 tokens/month
  Used this month: 8,400,000 (16.8%)
  
  Status: ✓ Plenty of room
```

> "**`/usage`**. Estoy al seis por ciento del contexto de la sesión actual. He gastado un millón doscientos mil tokens hoy. Me quedan ochenta y tres por ciento del cupo mensual.
>
> La gamma 1.3a slide 18 lo dijo: **lanzar `/usage` cada veinte o treinta minutos**. Especialmente en plan Pro y sesiones largas. Si veis que el contexto sube al setenta por ciento, ya estáis en territorio donde toca lanzar `/compact`. Lo veremos en el bloque siguiente.
>
> Vamos a recorrer los demás:"

**Tecleas comandos para mostrarlos brevemente:**

```
/model
```

```
Current model: claude-opus-4.7

Available:
  claude-opus-4.7    [current]
  claude-sonnet-4.6
  claude-haiku-4.5

Use: /model <name> to switch.
```

> "**`/model`**. Cambio en caliente. Si la sesión está tirando de Sonnet y la siguiente tarea es complicada, paso a Opus para esa tarea concreta y luego vuelvo. **Sin reiniciar sesión.** La gamma slide 19 lo recogió. Ojo: cambiar a cada cinco minutos cuesta más de lo que parece, no os volváis locos."

**Tecleas:**

```
/permissions
```

```
Current session permissions:

[Project allow]
  Read, Write, Edit, Glob, Grep
  Bash(dotnet build), Bash(dotnet test*), ...
  (28 patterns from .claude/settings.json)

[Project deny]
  Bash(rm -rf*), Bash(Remove-Item -Recurse*), ...
  (14 patterns from .claude/settings.json)

[Session-only allow]
  (none added this session)

To add: /permissions add <pattern>
To remove: /permissions remove <pattern>
```

> "**`/permissions`**. Os muestra los permisos vigentes. Veis los del `settings.json` de la 1.2b cargados — los veintiocho de allow y los catorce de deny. Si en mitad de sesión queréis añadir un permiso temporal, **`/permissions add <patrón>`**. Solo aplica a esa sesión, no se commitea. Útil para cosas puntuales sin tener que abrir el `settings.json`.
>
> **`/mcp`** y **`/agents`** los dejo sin tocar — la gamma 1.3a slide 21 los mencionó pero son del módulo 4 (MCP de Figma) y módulo 3 (subagentes).
>
> El último de la lista es el más importante: **`/plan`**. Lo veremos en su propio bloque porque merece la pena."

**Tiempo:** ~3 minutos 30 segundos.

---

### Bloque 6 — `/plan` en acción: una tarea de 4 ficheros (~3 min)

> "**`/plan`**. La gamma slide 22 lo dijo: **si la tarea va a tocar más de tres ficheros, `/plan`**. La pequeña fricción de revisar el plan vale el ahorro de no descubrir a los diez minutos que el agente está reescribiendo algo que querías que dejara como estaba. Vamos a verlo en una tarea real."

**En el prompt de Claude Code:**

```
/plan
```

Aparece:

```
✓ Planning mode enabled.
The agent will propose a plan before executing any changes.
Use /plan again to disable.
```

> "Activado. Ahora cualquier tarea que le pida, primero me propondrá un plan. Le doy una tarea que toque cuatro ficheros."

**Tecleas:**

```
> Quiero que añadas documentación XML completa a los 5 endpoints
  del OrdersController. Cada endpoint debe tener:
  - <summary> con descripción breve
  - <param> para cada parámetro
  - <returns> con los códigos HTTP que puede devolver
  - <response code="..."> para los status codes principales
  
  Esto es para que Swagger genere documentación útil. Sigue las
  convenciones del CLAUDE.md.
```

**Aparece el plan (sin tocar nada):**

```
Plan para añadir documentación XML al OrdersController:

He examinado el código y aquí está mi plan, paso a paso.

1. **Habilitar generación de XML doc en el csproj**
   src/OrderManagement.Api/OrderManagement.Api.csproj
   - Añadir dentro del PropertyGroup principal:
     <GenerateDocumentationFile>true</GenerateDocumentationFile>
     <NoWarn>$(NoWarn);CS1591</NoWarn>
   - El NoWarn evita que cada método sin doc genere warning durante
     la transición.

2. **Documentar los 5 endpoints del OrdersController**
   src/OrderManagement.Api/Controllers/OrdersController.cs
   - GetAll: summary, returns, response 200.
   - GetById: summary, param id, returns, response 200/404.
   - Create: summary, param request, returns, response 201/400/422.
   - Update: summary, param id+request, returns, response 200/404/422.
   - Delete: summary, param id, returns, response 204/404.
   - Documentación basada en el comportamiento real del código.

3. **Configurar Swagger para usar XML doc**
   src/OrderManagement.Api/Program.cs
   - En la llamada AddSwaggerGen(), añadir IncludeXmlComments con
     la ruta del fichero XML generado.

4. **Verificar Swagger arranca correctamente**
   - dotnet build (debe compilar limpio).
   - Mencionar que se puede comprobar en /swagger arrancando la API,
     pero no la voy a arrancar.

¿Procedo, o quieres que ajuste algo del plan?
```

> "Aquí está el plan. **Cuatro ficheros tocados.** Sin `/plan`, esto se ejecutaría directo y a mí me daría miedo. Con `/plan`, lo veo y puedo decidir.
>
> Mirad el detalle del paso uno: **propone añadir `<NoWarn>$(NoWarn);CS1591</NoWarn>` al csproj**. Esto evita que durante la transición — mientras documentamos los métodos uno a uno — el compilador tire warnings por cada método aún sin doc. **Es una decisión técnica de calidad** que un junior podría no haber pensado.
>
> Y mirad el paso tres: configurar Swagger para que use el XML. Sin esto, el csproj genera el fichero pero Swagger no lo lee. Es **un cuarto fichero tocado** que yo no había mencionado en mi prompt — el agente lo identifica como necesario para que el resultado sea coherente.
>
> Si no me convence el plan, lo digo y lo ajusta. Si sí me convence, procedo. **Hoy no lo voy a procesar para no inflar la rama, pero quería que vierais el flujo.** Salgo del modo plan:"

**Tecleas:**

```
/plan
```

```
✓ Planning mode disabled.
```

**Tiempo:** ~3 minutos.

---

### Bloque 7 — `/compact` en profundidad (~4 min)

> "Y vamos al rey de los slash commands en sesiones largas: **`/compact`**. La gamma le dedicó ocho slides (del 23 al 30) y lo presentó como **el comando que más rentabilidad da en producción**. Justificadamente."

> "Vamos a ver primero el problema que resuelve."

**En el prompt de Claude Code, lanzas:**

```
/usage
```

```
Current session usage:

  Context:        18,200 / 200,000 tokens (9.1%)
  ...
```

> "Estamos al nueve por ciento. Sesión corta. Pero imaginad que llevamos hora y media trabajando en una feature de cancelación de pedidos. Hemos tocado siete ficheros. Hemos discutido decisiones de arquitectura. Hemos descartado dos enfoques. El contexto está al setenta por ciento.
>
> Lo que pasa entonces: **el agente empieza a olvidar**. Le pides un test para el endpoint que acabas de modificar y te devuelve un test contra una signatura que no es la que existe. La gamma slide 25 contó el caso real: el dev se queja de que *'el agente se ha vuelto tonto'*. **No se ha vuelto tonto. Se ha quedado sin contexto.**
>
> `/compact` resuelve esto. Comprime la conversación: resume lo tratado, conserva las decisiones, descarta las idas y vueltas innecesarias. **El resultado ocupa una fracción del contexto pero contiene la información relevante.**
>
> Vamos a verlo. Voy a simular una sesión cargada haciéndole pensar en algo:"

**Tecleas (dos o tres prompts encadenados que generen contexto):**

```
> Léete CLAUDE.md, OrdersController.cs, CreateOrderHandler.cs y 
  CancelOrderHandler.cs. Identifica los puntos comunes del estilo
  del proyecto.
```

(esperas a que responda — leerá esos ficheros, generará respuesta)

```
> Ahora explícame qué decisión arquitectónica consideras más
  cuestionable en lo que has visto, y por qué.
```

(esperas — generará otra respuesta)

```
> Si tuviera que añadir GET /api/orders/by-customer/{customerId},
  qué pasos seguirías. Solo el plan, no actúes.
```

(esperas — más output)

**Lanzas `/usage`:**

```
/usage
```

```
Current session usage:

  Context:        47,800 / 200,000 tokens (23.9%)
```

> "Ahora estoy al veinticuatro por ciento. He generado contexto a propósito para mostrar el efecto. Voy a lanzar `/compact` con instrucciones específicas — la gamma slide 29 dijo que **se le pueden pasar instrucciones para guiar la compactación**."

**Tecleas:**

```
/compact "conserva las decisiones arquitectónicas que detectaste y los nombres concretos de las clases que has visto"
```

**Aparece el output del compact:**

```
Compacting conversation...

Original context: 47,800 tokens
Compacted to:    4,200 tokens (8.8% of original)

Summary preserved:

The session has analyzed the OrderManagement project codebase, focusing
on:
- CLAUDE.md conventions: CQRS with MediatR, async/await strict, typed
  exceptions (CustomerNotFoundException, OrderNotFoundException), tests
  with xUnit + NSubstitute + FluentAssertions, no Moq.
- Files reviewed: OrdersController.cs (5 REST endpoints, MediatR
  dispatch pattern), CreateOrderHandler.cs (validation flow with
  FluentValidation), CancelOrderHandler.cs (uses InvalidOperationException
  for state validation, inconsistent with the typed exceptions pattern
  elsewhere).
- Key architectural observation: the CancelOrderHandler uses generic
  InvalidOperationException where the rest of the project uses typed
  domain exceptions. This is a candidate for refactoring.
- Pending discussion: a plan to add GET /api/orders/by-customer/{customerId}
  was outlined but not executed.

Files explicitly mentioned: OrdersController.cs, CreateOrderHandler.cs,
CancelOrderHandler.cs, OrderNotFoundException, CustomerNotFoundException.

Context cleared and replaced with the summary above. The session
continues from here.
```

> "Mirad. Cuarenta y siete mil ochocientos tokens reducidos a cuatro mil doscientos. **El nueve por ciento del original.** Y atentos a qué se ha conservado:
>
> Las decisiones arquitectónicas que detecté — la inconsistencia de `InvalidOperationException`. **Conservada porque se la pedí explícitamente.**
>
> Los nombres concretos de las clases — `OrdersController`, `CreateOrderHandler`, `CancelOrderHandler`, las excepciones tipadas. **Conservadas.**
>
> El plan de añadir el endpoint by-customer que estaba pendiente. **Marcado como pendiente.**
>
> Y todo lo demás — las idas y vueltas de cómo expliqué cada paso, las repreguntas, el ruido — **descartado.**"

**Lanzas `/usage` para confirmar:**

```
/usage
```

```
Current session usage:

  Context:        4,200 / 200,000 tokens (2.1%)
```

> "Del veinticuatro al dos por ciento. Y la sesión sigue siendo útil. Si ahora le pido *'continúa con el plan de añadir el endpoint by-customer'*, sabe exactamente de qué le hablo.
>
> La regla práctica: **cada veinte o treinta minutos de trabajo activo, lanza `/compact`**. No esperes a que se vuelva tonto. Para entonces ya es tarde."

**Tiempo:** ~4 minutos.

---

### Bloque 8 — El antipatrón estrella: `/clear` cuando deberías `/compact` (~1 min 30 seg)

> "Y antes de cerrar, **el antipatrón más típico del primer día**. La gamma 1.3a lo marcó en el slide 16 y la 1.3b lo recogió en el slide 20 como el primer error frecuente."

> "**`/clear` borra todo el contexto. `/compact` lo conserva resumido.**
>
> El error: usar `/clear` cuando debería ser `/compact`. *'Voy a limpiar la sesión porque está cargada'*. **Pero la tarea no ha terminado.** Y al limpiar, pierdes todo lo que estabas haciendo. Tienes que reexplicar al agente desde cero qué ficheros había, qué decisiones tomasteis, dónde estabas.
>
> La regla mnemotécnica:
>
> - **`/clear` → cambio de tarea completo.** Vas a empezar algo distinto. Quieres tabula rasa.
> - **`/compact` → seguir con la misma tarea.** El contexto pesa pero el trabajo no ha terminado.
>
> Si vuestra duda en cualquier momento es 'cuál de los dos', **casi siempre es `/compact`**. Porque lo más común es estar a mitad de algo y querer aligerar el peso, no querer borrar y empezar de cero."

**Tiempo:** ~1 minuto 30 segundos.

---

### Bloque 9 — Mostrar el script `audit-staged.sh` (~2 min)

> "Y para cerrar, el script que dejé en la rama. Es el ejemplo del modo one-shot integrado en un hook de pre-commit. **No está instalado** — solo queda en `scripts/` como referencia. Pero merece la pena verlo."

> Cierras Claude Code con Ctrl+C. Abres el fichero en VS Code.

**Abres `scripts/audit-staged.sh` en VS Code:**

```bash
#!/bin/bash
# audit-staged.sh
# Ejemplo de uso de Claude Code en modo one-shot dentro de un hook
# de pre-commit.

set -e

DIFF=$(git diff --cached --diff-filter=AM)

if [ -z "$DIFF" ]; then
    echo "No hay cambios staged. Saltando audit."
    exit 0
fi

RESULT=$(echo "$DIFF" | claude -p "Audita este diff staged...")

if [ "$RESULT" = "OK" ]; then
    echo "✓ Audit OK. Commiteando."
    exit 0
fi

# Si hay hallazgos, mostrar y pedir confirmación
echo "$RESULT"
read -p "¿Quieres commitear de todas formas? (s/N): " -n 1 -r CONFIRM

if [[ "$CONFIRM" =~ ^[SsYy]$ ]]; then
    exit 0
else
    exit 1
fi
```

> "Esto es lo mismo que vimos en el bloque 4 — `git diff --cached | claude -p "..."` — pero envuelto en un script que se puede instalar como hook de pre-commit. **Si lo activáis** copiándolo a `.git/hooks/pre-commit` y dándole permisos ejecutables, **antes de cada commit**, el script lanza Claude Code para auditar lo staged. Si encuentra problemas críticos, te muestra los hallazgos y te pregunta si commitear de todas formas.
>
> En Windows, esto funciona porque Git for Windows trae bash. **No requiere PowerShell.** Cuando hacéis `git commit`, Git ejecuta el hook usando bash, que viene con Git for Windows. Compatible.
>
> No lo activamos hoy porque los hooks reales son del módulo 3, donde veremos el sistema de hooks nativo de Claude Code — más potente que git hooks. **Pero queda aquí como referencia para que veáis cómo se conecta el modo one-shot con el flujo de trabajo real.**"

**Tiempo:** ~2 minutos.

---

### Bloque 10 — Recap y cliffhanger (~2 min)

> "Y eso es la 1.3a. Recap de hábitos a llevarse al lunes. Cinco puntos."

**En la terminal lanzas:**

```powershell
git status
```

```
On branch demo/1.3a
nothing to commit, working tree clean
```

> "Uno. **Tres modos, tres herramientas distintas.**
>
> - Interactivo cuando vais a trabajar más de cinco minutos en algo.
> - One-shot para preguntas puntuales y para automatización.
> - Pipe para procesar lo que ya tenéis — el caso de oro es `git diff | claude -p` para code review pre-PR.
>
> Dos. **`/usage` cada veinte o treinta minutos.** No es opcional. La fricción de teclear seis caracteres ahorra una sorpresa al final de la sesión.
>
> Tres. **`/compact` para sesiones largas.** Cuando el contexto pasa del cincuenta por ciento, lanzadlo. Le podéis pasar instrucciones — *'conserva las decisiones de arquitectura'* — para guiar qué se queda.
>
> Cuatro. **`/plan` para tareas que tocan más de tres ficheros.** Treinta segundos de revisión os ahorran diez minutos de retrocesos.
>
> Cinco. **`/clear` solo cuando cambiáis de tarea por completo. Si la tarea sigue, `/compact` es la opción.** Es el antipatrón estrella del primer día.
>
> En la siguiente demo, la 1.3b, cerramos el módulo 1 con permisos en runtime — qué hacer cuando el agente os pide aprobación a mitad de sesión, los cuatro patrones sanos — y los cuatro workflows típicos del día a día. **Vamos a ver el patrón 1, implementación de feature de cancelación de pedidos, encadenado entero**. Es la culminación del módulo 1: con todo lo que tenemos hasta aquí — `CLAUDE.md`, settings, modos, slash commands, permisos — vais a ver una sesión real de trabajo de principio a fin."

**Tiempo:** ~2 minutos.

---

## 9. Qué resaltar verbalmente

Cinco puntos que **no pueden quedarse sin decir**:

1. **"Los tres modos son distintos."** — interactivo para trabajar, one-shot para automatizar, pipe para procesar. Si el alumno solo retiene esto, ya hemos ganado. Mencionarlo en el bloque 1 (analogía coche manual), recoger en el bloque 10 (recap).

2. **"`/compact` cada veinte o treinta minutos."** — la regla mnemotécnica más rentable. Aparece en el bloque 7 (`/compact` en profundidad) y en el bloque 10 (recap). El alumno tiene que poder repetirla de memoria.

3. **"`/clear` borra. `/compact` resume."** — el antipatrón estrella. Bloque 8. Conviene reforzarlo porque es donde más se equivoca el alumno la primera semana.

4. **"`/plan` para tareas que tocan más de tres ficheros."** — la regla de dedo. Bloque 6. Sin esto, el agente toma direcciones equivocadas y el alumno se frustra.

5. **"En Windows, los pipes funcionan idénticos en PowerShell."** — el alumno con dudas puede pensar que `|` es Linux-only. Recordarle en el bloque 4 que **PowerShell soporta pipes Unix-style sin nada extra**.

**Frase de remate al final, que conviene memorizar:**

> *"Tres modos, diez slash commands, dos hábitos: `/usage` cada veinte minutos y `/compact` cada treinta. Esos hábitos son la diferencia entre quien usa Claude Code dos semanas y se queda, y quien lo usa un mes y abandona porque 'no es para tanto'."*

---

## 10. Slide de entrada (locución HeyGen)

> **Texto del avatar antes del screencast:**

"Y vamos a la siguiente demo. La 1.3a. Aquí cambian los hábitos. La gamma os contó la analogía del coche manual: la mayoría de gente al llegar al día 2 con Claude Code abre sesión interactiva, escribe lo que se le ocurre, y trabaja desde ahí. Llega a destino. Pero está haciendo cuestas a veinte por hora desgastando el embrague. Esta demo son las otras marchas. Vais a ver tres cosas. Primero, los tres modos de uso — interactivo, one-shot, pipe — en escenarios reales sobre OrderManagement, con foco especial en el modo pipe haciendo code review pre-PR de un diff con dos errores deliberados. Segundo, los diez slash commands más útiles del día a día, con énfasis en `/plan` aplicado a una tarea de cuatro ficheros y `/compact` con instrucciones para guiar qué se conserva. Y tercero, el antipatrón estrella del primer día: cuándo `/clear` y cuándo `/compact`. Vamos al screencast."

---

## 11. Slide de salida (locución HeyGen)

> **Texto del avatar al volver del screencast:**

"Lo que acabáis de ver son hábitos. No es contenido espectacular como la 1.2b. Pero son los hábitos que separan a quien usa Claude Code dos semanas y se queda, de quien lo usa un mes y abandona porque *'no es para tanto'*. Tres modos, tres herramientas. Diez slash commands con `/compact` y `/plan` a la cabeza. Y dos reglas mnemotécnicas: `/usage` cada veinte minutos para no llevarse sorpresas, `/compact` cada treinta minutos en sesiones largas para que el agente no se vuelva tonto. Y el antipatrón estrella: si la tarea no ha terminado, **siempre `/compact`, nunca `/clear`**. Queda una demo para cerrar el módulo 1: la 1.3b, donde vamos a ver permisos en runtime, los cuatro patrones sanos, y un workflow real de implementación de feature encadenado entero. Es la culminación del módulo. Empezamos con el cinco punto uno punto tres B."

---

## 12. Tiempo total estimado

| Bloque | Tiempo |
|---|---|
| Slide de entrada (avatar) | ~30 seg |
| Bloque 1 — Setup y analogía coche manual | ~1 min 30 seg |
| Bloque 2 — Modo interactivo | ~1 min 30 seg |
| Bloque 3 — Modo one-shot | ~2 min |
| Bloque 4 — Modo pipe: code review pre-PR | ~3 min |
| Bloque 5 — Slash commands: panorama | ~3 min 30 seg |
| Bloque 6 — `/plan` en acción | ~3 min |
| Bloque 7 — `/compact` en profundidad | ~4 min |
| Bloque 8 — Antipatrón `/clear` vs `/compact` | ~1 min 30 seg |
| Bloque 9 — Script `audit-staged.sh` | ~2 min |
| Bloque 10 — Recap y cliffhanger | ~2 min |
| Slide de salida (avatar) | ~30 seg |
| **Total screencast** | **~24-26 min** |
| **Total con avatar** | **~25-27 min** |

> Si hay preguntas durante el screencast, súmale 3-4 minutos. La demo encaja en un bloque de **30 minutos** dentro de la sesión.

**Margen de seguridad por si algo va lento:**

- **Si el bloque 4 (modo pipe code review) no detecta los dos problemas que metiste deliberadamente**, **no fuerces el guion**. Comenta: *"a veces el modo pipe no detecta todo a la primera. La instrucción del prompt importa. Voy a refinarla"*. Y vuelves a lanzar con un prompt más específico. La pedagogía es que el modo pipe sirve para code review, no que detecte exactamente lo que tú esperas.

- **Si `/compact` produce un resumen poco útil**, **úsalo como aprendizaje**. *"Mirad, el resumen ha conservado las clases pero no la decisión arquitectónica. Eso es porque mi instrucción al `/compact` no fue lo suficientemente específica. Vamos a ver con más detalle"*. Y muestras cómo afinar la instrucción.

- **Si `/plan` propone un plan de menos de 3 ficheros**, no improvises. *"Esta tarea ha resultado más simple de lo que pensaba. El criterio de '3 ficheros' es de dedo, no estricto. Lo importante es que el plan está. Vamos a verlo"*. Y comentas el plan tal cual aparece.

- **Si el antipatrón `/clear` vs `/compact` te parece muy abstracto sin demo en vivo**, lánzalo: tras el `/compact` del bloque 7, **lanza `/clear` en otra sesión paralela** y muestra que el contexto desaparece a cero. Es la demo más dramática del antipatrón.

- **Si el script `audit-staged.sh` os parece denso para mostrar al final**, recorta el bloque 9 a 1 minuto: solo enseñas el fichero, dices que es ejemplo del one-shot integrado, y lo dejas para que el alumno lo lea por su cuenta. El bloque 9 es opcional, no esencial.

---

# Apéndice A — Decisiones pedagógicas justificadas

> Esta sección no se graba. Es para Pedro, formador.

**¿Por qué arrancar la demo retomando la analogía del coche manual?**

Porque la gamma 1.3a la abrió con esa analogía (slide 3) y es **el marco mental de toda la sesión**. Si la demo no la retoma, el alumno la oye en abstracto pero no la ata. Retomarla en el bloque 1 fija el "por qué" de aprender los modos y comandos.

**¿Por qué meter dos errores deliberados en el código del bloque 4?**

Porque el modo pipe **necesita un caso real** donde detectar problemas sea palpable. Si pasas un diff limpio, el modo pipe responde "OK" y el alumno no ve la utilidad. Con dos violaciones de las reglas del CLAUDE.md, el modo pipe demuestra que **referencia las reglas por su contenido** — y eso conecta con la pieza pedagógica de la 1.2b.

**¿Por qué mostrar `/usage` antes y después de `/compact`?**

Porque la diferencia es **cuantitativa y verificable**. La gamma habló de la rentabilidad de `/compact` en abstracto. Mostrar 47.800 tokens reducidos a 4.200 (8.8% del original) en pantalla **es la única forma honesta** de aterrizarlo. Sin eso, el alumno cree el principio pero no lo internaliza.

**¿Por qué `/compact` con instrucciones explícitas?**

Porque la gamma slide 29 dijo que es **lo más útil que casi nadie sabe**. Si la demo solo enseña `/compact` solo, el alumno lo usa solo y pierde el ochenta por ciento del valor. Con instrucciones (*"conserva las decisiones arquitectónicas"*), el resumen es útil para seguir trabajando. Sin instrucciones, es genérico.

**¿Por qué `/plan` aplicado a documentación XML y no a una feature?**

Porque la implementación de feature completa es la **demo 1.3b**. Si aquí ya muestro una feature entera, la 1.3b pierde fuerza. La documentación XML es:
- Una tarea **realista** que un dev .NET haría.
- Toca **cuatro ficheros** (csproj, controller, Program.cs, build verification).
- Tiene **una decisión técnica no obvia** (`<NoWarn>CS1591</NoWarn>` durante la transición) que demuestra el valor del `/plan`.
- No invade contenido de la 1.3b.

**¿Por qué el script `audit-staged.sh` solo se muestra y no se instala?**

Porque los hooks reales son del **módulo 3 (3.3a)** donde se cubre el sistema de hooks nativo de Claude Code — más potente que git hooks. Si aquí instalo un hook de git, invado el módulo 3. Y si lo instalo y luego en el módulo 3 enseño otro distinto, el alumno se confunde. Mejor dejar el script como **referencia** y explicar la conexión con el módulo 3.

**¿Por qué la frase de remate menciona "dos semanas vs un mes"?**

Porque la gamma 1.3a slide 2 lo dijo literal: *"son los hábitos que separan a quien usa Claude Code dos semanas y se queda, de quien lo usa un mes y abandona"*. La frase es **del propio material del curso**. Repetirla refuerza el mensaje sin sonar a invento.

**¿Por qué Windows-specific en el bloque 4 (mención a PowerShell pipes)?**

Porque el alumno con menos experiencia podría asumir que `|` es Linux-only y pensar que necesita Git Bash o WSL. **Una frase corta** — "los pipes funcionan idénticos en PowerShell" — desbloquea esa duda y evita que el alumno se complique innecesariamente la primera semana.

**¿Por qué el bloque 9 es marcado como "opcional, recortable a 1 min"?**

Porque la demo es densa (10 bloques) y el bloque 9 es el menos crítico pedagógicamente — el alumno puede leer el script por su cuenta. Si el formador va con prisas o el alumno está cansado, recortar el bloque 9 sin pérdida de calidad es preferible a apurar otros bloques esenciales. **Margen de flexibilidad incorporado.**
