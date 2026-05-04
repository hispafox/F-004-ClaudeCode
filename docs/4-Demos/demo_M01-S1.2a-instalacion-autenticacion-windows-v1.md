# Demo 1.2a — Instalación, verificación y autenticación de Claude Code en Windows

> **Versión:** v1 | **Módulo:** 1 | **Sub:** 1.2a | **Estado:** ✅ Versión final
> **Archivo:** `demo_M01-S1.2a-instalacion-autenticacion-windows-v1.md`
> **Branch before:** `demo/1.2a-before`  (estado al hacer `git checkout` antes de grabar)
> **Branch after:**  `demo/1.2a-after`   (estado final que la siguiente clase asume)
> **Branch parent:** `demo/1.1`  (CONCEPTUAL — rama única de la demo predecesora)
> **Tiempo total estimado:** ~14-16 minutos
> **Tipo:** Demo de instalación en vivo (INFRA). **Es la primera demo donde el alumno ve cómo se enciende Claude Code de cero.** La 1.1 mostró qué hace; ésta muestra cómo llegar a tener el agente operativo en una máquina Windows virgen. Sigue el patrón **before/after** definido en [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md).
> **Plataforma:** Windows 11 (PowerShell 7).

---

## 1. Contexto

En la gamma 1.1 vimos el modelo conceptual. En la demo 1.1 vimos las cuatro fases del ciclo agentic en pantalla. El alumno llegó al final pensando *"vale, esto va más allá de Copilot, ¿cómo lo enciendo?"*.

La gamma 1.2a (22 slides, ~15 min de teoría) responde esa pregunta con todo el detalle: requisitos del sistema, cuenta de pago, las dos vías de instalación (native installer recomendado vs npm legacy), por qué nunca usar `sudo`, `claude --version`, `claude doctor`, login OAuth, API key, errores típicos.

Esta demo aterriza la gamma. **Es la primera demo donde el alumno ve, con sus ojos, una máquina Windows en estado virgen pasar a tener Claude Code operativo en menos de cinco minutos.** Sin trucos, sin atajos. El proceso real, paso a paso, en PowerShell.

Hay una decisión pedagógica importante aquí: la demo no es el formador instalando en su máquina personal (donde ya tiene Claude Code, Git, todo). **Es una sesión de PowerShell limpia** — preferiblemente en una VM Windows 11 fresca, o como mínimo desinstalando Claude Code antes de grabar. Si el alumno ve `claude --version` funcionando antes de tiempo, pierde la fuerza de la demo.

> **Tipo de demo:** instalación en vivo. El alumno ve **el camino real** de virgen a operativo.

---

## 2. Objetivo de la demo

Tres cosas concretas que tienen que quedar en la cabeza del alumno cuando termine:

1. **Instalar Claude Code en Windows es un comando.** Un solo comando en PowerShell — `irm https://claude.ai/install.ps1 | iex` — y queda instalado. **No requiere WSL, ni Docker, ni Node.** Es la novedad de 2026 y el alumno tiene que tenerla clara para no perder tiempo con tutoriales antiguos que mencionan WSL como obligatorio.

2. **La autenticación es OAuth en navegador.** No hay que generar API keys ni configurar nada complicado. Se abre el navegador, autorizas con tu cuenta Pro/Max, vuelves al terminal, listo.

3. **`claude doctor` es la primera parada cuando algo falla.** No Stack Overflow, no foros, no `claude --version` — `claude doctor`. La mitad de los problemas se resuelven leyendo lo que dice el doctor.

Y dos cosas que tienen que **NO quedar** en su cabeza:

- *"Esto solo funciona si tengo Git, WSL, Docker..."* — no, en 2026 ya no.
- *"Si me da problemas, tengo que reinstalar todo"* — no, el doctor te dice exactamente qué falla.

---

## 3. Branch `demo/1.2a-before`

Punto de partida del screencast.

```
demo/1.2a-before
```

**Parte de:** `demo/1.1` (CONCEPTUAL, rama única).

**Estado del repo:** idéntico a `demo/1.1`. Proyecto OrderManagement limpio (README + `docs/DEMOS.md`), sin nada de Claude Code. Los cambios del screencast de la 1.1 (el endpoint de cancelación) se descartaron, así que la rama está en estado virgen para esta demo.

**Estado de la máquina:** Claude Code **NO instalado**. La instalación es la pieza viva del screencast.

> El formador hace `git checkout demo/1.2a-before` antes de empezar a grabar.

---

## 4. Branch `demo/1.2a-after`

Estado final que la siguiente clase (1.2b) asume.

```
demo/1.2a-after
```

**Parte de:** `demo/1.2a-before`.

**Qué añade respecto a `-before`:** una entrada en `docs/DEMOS.md` marcando esta demo como hecha (`[x]`). **Nada más en el repo.** No se toca el código de la app, no se añade `CLAUDE.md` (eso es 1.2b). La diferencia está en la máquina del alumno, no en el repo: ahora tiene Claude Code instalado y autenticado, listo para configurarlo en la 1.2b.

**Cómo se prepara:** ver §6b. Se materializa antes de grabar — no depende del resultado del screencast.

> Durante la grabación, el formador instala Claude Code en directo desde `demo/1.2a-before`. Al cerrar, descarta los cambios reales (la instalación queda en su máquina, no en el repo). La siguiente clase parte de `demo/1.2a-after` ya pre-cocinada.

---

## 5. Estado del repo al empezar

Idéntico a la rama `demo/1.1`. La estructura del proyecto no cambia.

```
ordermanagement/
├── docs/
│   └── DEMOS.md                        (registro de demos, primera marcada)
├── src/                                (sin cambios desde demo/1.1)
├── frontend/                           (sin cambios)
├── tests/                              (sin cambios)
├── .gitignore
└── README.md                           (descripción del proyecto)
```

**Estado de la máquina Windows del formador (importante para la demo):**

```
✅ Windows 11 (idealmente VM fresca, o máquina con Claude Code desinstalado)
✅ PowerShell 7 instalado
✅ Git for Windows instalado
✅ Cuenta Claude Pro o Max (autenticada en el navegador por defecto)
❌ Claude Code NO instalado (importante)
❌ Node.js opcional (no necesario para la vía nativa, pero suele estar en máquinas .NET)
```

**Cómo desinstalar Claude Code antes de grabar (si la máquina ya lo tiene):**

```powershell
# Si está instalado vía native installer:
Remove-Item -Recurse -Force $env:USERPROFILE\.local\bin\claude.exe -ErrorAction SilentlyContinue

# Si está instalado vía npm:
npm uninstall -g @anthropic-ai/claude-code

# Limpia config (opcional, pero recomendado para grabar de cero)
Remove-Item -Recurse -Force $env:USERPROFILE\.claude -ErrorAction SilentlyContinue

# Verificar que ya no está
claude --version
# Esperado: 'claude' is not recognized as the name of a cmdlet...
```

**Lo que el alumno verá en su máquina al final de la demo:**

- Claude Code instalado en `C:\Users\<su_usuario>\.local\bin\claude.exe`.
- Autenticado vía OAuth con su cuenta.
- `claude --version` devuelve un número de versión (ej. `2.1.x`).
- `claude doctor` con todos los checks en verde.
- Una primera pregunta tonta a Claude Code respondiendo correctamente.

---

## 6. Prompt para Claude Code

> **Atención: este prompt es distinto a los anteriores.**
>
> En las demos 1.1 y siguientes, el prompt era para **Claude Code preparara la rama**. Aquí, **el alumno todavía no tiene Claude Code instalado** durante el screencast — la demo es precisamente la instalación. Por eso, el prompt para Claude Code se ejecuta **después** de la grabación, una vez Pedro tiene todo instalado, **para preparar el commit del `docs/DEMOS.md`** con la marca de demo hecha.
>
> Pedro lo ejecuta tras grabar, no antes.

````
Estoy preparando la demo 1.2a del curso de Claude Code para devs .NET + 
Angular. Esta demo es la instalación en vivo de Claude Code en Windows.

# Contexto

Acabo de grabar la demo. Durante el screencast he instalado Claude Code 
en una máquina Windows virgen, lo he autenticado con OAuth, he ejecutado 
claude --version y claude doctor para verificar, y he hecho una primera 
pregunta tonta para confirmar que responde.

Ahora necesito preparar la rama demo/1.2a con un cambio mínimo: marcar 
en docs/DEMOS.md que esta demo ya está hecha. La rama debe partir de 
demo/1.1 y añadir SOLO ese cambio. Nada más.

# Lo que necesito

Tres tareas:

## Tarea 1: crear la rama demo/1.2a

```powershell
git checkout demo/1.1
git pull
git checkout -b demo/1.2a
```

## Tarea 2: actualizar docs/DEMOS.md

Localiza la línea que dice:

```
- [ ] demo/1.2a — Instalación, autenticación y primer arranque
```

Y cámbiala por:

```
- [x] **demo/1.2a** — Instalación, autenticación y primer arranque
```

(El símbolo `[ ]` pasa a `[x]`, y se añaden los asteriscos para destacar.)

NO toques el resto del fichero. Las demás demos siguen pendientes.

## Tarea 3: verificar y commitear

Verifica que el cambio es solo el de la línea de la demo 1.2a:

```powershell
git diff docs/DEMOS.md
```

Esperado: una línea cambiada, ninguna más.

Luego:

```powershell
git add docs/DEMOS.md
git commit -m "demo/1.2a: marca demo de instalación como completada"
```

NO hagas push. Yo lo hago manualmente cuando lo revise.

# Restricciones

- NO añadas CLAUDE.md ni .claude/settings.json. Eso es la demo 1.2b.
- NO toques el código de la app, ni los .csproj, ni Program.cs.
- NO toques el README.md.
- El cambio total tiene que ser de una sola línea en docs/DEMOS.md.

# Cuando termines, dime

1. Que la rama demo/1.2a está creada desde demo/1.1.
2. Que docs/DEMOS.md tiene la demo 1.2a marcada como hecha.
3. Que git diff muestra solo ese cambio (una línea).
4. Que el commit está hecho.
````

---

## 7. Artefactos que Claude Code debe generar

```
✓ Rama nueva: demo/1.2a (parte de demo/1.1)
✓ docs/DEMOS.md modificado: línea de demo 1.2a marcada con [x]
✓ Commit único: "demo/1.2a: marca demo de instalación como completada"
```

**Lo que NO debe haber generado:**

- ❌ `CLAUDE.md` o `.claude/` (eso es la demo 1.2b)
- ❌ Cambios en código de la app
- ❌ Cambios en README.md
- ❌ Más de una línea modificada en `docs/DEMOS.md`

> Si Claude Code se anticipa y mete `CLAUDE.md` o `.claude/settings.json` "para que la demo sea más completa", se rechaza. La demo 1.2a es solo instalación; la configuración del proyecto es la 1.2b.

**Estado final del árbol después del prompt:**

```
ordermanagement/
├── docs/
│   └── DEMOS.md                    ← MODIFICADO (1 línea)
├── src/                            ← sin cambios
├── frontend/                       ← sin cambios
├── tests/                          ← sin cambios
├── .gitignore                      ← sin cambios
└── README.md                       ← sin cambios
```

---

## 8. Guion del screencast

**Duración estimada del screencast: ~12-13 minutos.**

Siete bloques. Esta demo es la única donde el formador **muestra una máquina virgen y la lleva a operativa**. Los bloques siguen el flujo natural de la instalación.

> **Antes de empezar a grabar**, asegúrate de:
> - Tener PowerShell 7 abierto en pantalla completa, fuente al menos 16pt.
> - Tener un navegador (Edge o Chrome) abierto en una segunda pantalla o ventana, **ya logueado en `claude.ai` con tu cuenta Pro/Max**. Esto evita que tengas que poner credenciales en pantalla durante el OAuth.
> - **Claude Code NO instalado** en la máquina (verifica con `claude --version` que no existe — si responde, desinstala antes).
> - **No tener configurado `~/.claude/`** — borra la carpeta si existe.
> - VS Code abierto al lado con el repo `ordermanagement` cargado en `demo/1.1`.
> - Cerrar Slack, Teams, navegadores con notificaciones.
> - Tener una pestaña con `https://docs.claude.com/en/docs/claude-code/setup` abierta por si surge alguna duda durante la grabación.

---

### Bloque 1 — Setup visible y orientación al alumno (~1 min 30 seg)

**Pantalla compartida.** A la izquierda, VS Code con el repo en `demo/1.1`. A la derecha, una terminal **PowerShell 7 limpia** en la raíz del proyecto.

**Antes de teclear nada,** muestras visualmente que **Claude Code NO está instalado** en la máquina. Esto es importante para la fuerza pedagógica de la demo.

**Tecleas:**

```powershell
claude --version
```

**Lo que aparece:**

```
claude : The term 'claude' is not recognized as the name of a cmdlet, 
function, script file, or operable program. Check the spelling of the 
name, or if a path was included, verify that the path is correct and 
try again.
```

**Lo que dices, mientras señalas el output rojo:**

> "Quiero que veáis exactamente esto antes de empezar. Esta es una máquina Windows en estado virgen para Claude Code. No tengo nada instalado. Lo verificamos: `claude --version` me devuelve el error rojo que conocéis bien — *'el término claude no se reconoce como cmdlet'*. Cero. No hay nada.
>
> Y este es el punto de partida más realista. Vuestra máquina del lunes va a estar exactamente así. La pregunta es: ¿cuántos pasos hay desde aquí — desde una máquina sin nada — hasta tener Claude Code respondiendo en este mismo terminal?
>
> Os adelanto la respuesta: tres comandos. Tres. Y lo importante es que **en 2026 ya no necesitáis WSL**. Ya no necesitáis Docker. Ya no necesitáis Node si vais por la vía recomendada. Si habéis visto tutoriales de hace seis meses que os mandaban montar WSL2 antes de instalar Claude Code, **olvidaos de ellos**. Esa restricción ya no aplica desde principios de 2026.
>
> Vamos paso a paso."

**Tiempo:** ~90 segundos.

---

### Bloque 2 — Verificación de prerequisitos mínimos (~1 min)

Antes de instalar, comprobamos lo único que sí necesita estar antes: **Git for Windows**. La gamma lo mencionó (slide 7). Aquí lo materializamos.

**Tecleas:**

```powershell
git --version
```

**Aparece algo como:**

```
git version 2.45.1.windows.1
```

> "Esto sí lo necesito. Git for Windows. ¿Por qué? Porque Claude Code en Windows usa Git Bash por debajo cuando ejecuta comandos shell. Es la única dependencia real que tenéis que tener instalada antes. Si no lo tenéis, lo instaláis con `winget install --id Git.Git` o desde `git-scm.com`. Yo ya lo tengo. Versión 2.45 — sirve cualquier reciente.
>
> Lo que **no** necesito comprobar — y conviene que veáis qué no hace falta — es Node.js. Algún tutorial todavía os pide `node --version` antes de instalar Claude Code. Eso era de la vía npm, que sigue funcionando pero ya no es la recomendada. Para la vía nativa que vamos a usar, **no necesito Node para nada**. Si no programáis JavaScript, no tenéis que mantener una versión de Node solo para Claude Code.
>
> Tampoco necesito ser administrador. Ya veréis al instalar — ni siquiera me pide elevación. Esto importa porque en muchos entornos corporativos no tenemos derechos de admin, y aún así Claude Code se instala sin problema."

**Tiempo:** ~60 segundos.

---

### Bloque 3 — Instalación con el comando único (~2 min)

La pieza clave de la demo. **Un solo comando.** Si la conexión es lenta, este bloque puede tardar más — está bien, la demo se beneficia de ver el progreso real.

**Tecleas:**

```powershell
irm https://claude.ai/install.ps1 | iex
```

> "Aquí está el comando. Lo voy despacio para que los que estéis tomando notas podáis copiarlo. **`irm`** — `Invoke-RestMethod` — descarga el script de instalación oficial desde `claude.ai/install.ps1`. **`iex`** — `Invoke-Expression` — lo ejecuta. Pipe en medio.
>
> Atentos: esto es PowerShell, no CMD. Si ejecutáis `irm` en CMD os va a dar error. La gamma lo dijo, pero conviene recordarlo aquí: **PowerShell, no CMD**. La diferencia visual es el `PS` al principio del prompt. Si veis `PS C:\...` estáis en PowerShell. Si veis `C:\...` sin el PS, estáis en CMD."

**Pulsas Enter. Aparece el progreso del instalador.**

```
Setting up Claude Code...

Detecting platform: Windows x64
Downloading Claude Code v2.1.x...
[████████████████████████████████] 100%

Verifying signature...
✓ Signature valid (Anthropic, PBC)

Installing to C:\Users\pedro\.local\bin\claude.exe...
✓ Binary installed
✓ Auto-update enabled

Updating PATH...
✓ Added C:\Users\pedro\.local\bin to user PATH

✓ Claude Code successfully installed!
   Version: 2.1.x
   Location: C:\Users\pedro\.local\bin\claude.exe

Next: Run 'claude --help' to get started
```

> "Mientras descarga, comento. Veis lo que va saliendo. Detecta la plataforma, descarga el binario, **verifica la firma** — esto es importante, no instala cualquier cosa, comprueba que está firmado por Anthropic PBC — y lo coloca en `C:\Users\pedro\.local\bin\claude.exe`. Esa ruta es la estándar en Windows.
>
> Atentos a la línea: *'Auto-update enabled'*. Esto es una de las ventajas del native installer sobre la vía npm. **Las actualizaciones llegan solas en background**. No tenéis que ejecutar `npm update -g` cada dos semanas. Yo, sinceramente, no he ejecutado una actualización manual en seis meses.
>
> Y al final: *'Claude Code successfully installed'*. Versión y ubicación. Listo. Veintiocho segundos. Esto que parece simple, hace año y medio era WSL más Docker más configuración. Ahora es un comando."

**Tiempo:** ~2 minutos.

---

### Bloque 4 — La trampa típica del PATH (~1 min 30 seg)

Aquí surge el problema más común en Windows según la documentación oficial: **el PATH cacheado en la sesión actual de PowerShell**. El instalador añade el path al PATH del usuario, pero la sesión actual no lo recoge automáticamente.

**Tecleas:**

```powershell
claude --version
```

**Posiblemente aparece (depende de la versión de PowerShell):**

```
claude : The term 'claude' is not recognized as the name of a cmdlet, 
function, script file, or operable program.
```

> "Y aquí viene una de las trampas más comunes en Windows. Acabo de instalarlo. El instalador me ha dicho 'éxito'. Pero si lanzo `claude --version` en **esta misma sesión** de PowerShell, me da error.
>
> ¿Qué pasa? El instalador ha añadido `C:\Users\pedro\.local\bin` al PATH del usuario. Pero PowerShell **cacheó el PATH cuando abrió esta sesión**, antes de la instalación. Esta sesión no se ha enterado del cambio.
>
> La solución es trivial pero conviene saberla. Dos opciones."

**Opción A — Cerrar y abrir terminal nuevo. Tecleas:**

```powershell
exit
```

Cierras PowerShell. Abres una nueva ventana de PowerShell.

**Tecleas:**

```powershell
claude --version
```

**Aparece:**

```
2.1.x
```

> "Y ahora sí. Versión 2.1.x. La forma 'limpia' es cerrar y abrir un terminal nuevo. Pero a veces no quieres perder lo que tenías abierto. La opción B es refrescar el PATH en la misma sesión sin cerrar."

**Opción B — Refrescar PATH en la misma sesión (lo enseñas como alternativa):**

```powershell
$env:PATH = "$env:PATH;$env:USERPROFILE\.local\bin"
claude --version
```

> "Esto añade el path a la variable PATH de esta sesión. Es temporal — solo aplica a esta ventana. Pero si la sesión que tenías abierta era importante y no querías cerrarla, te saca del problema sin perder contexto.
>
> En la documentación oficial veréis los dos enfoques. Yo, en producción, prefiero cerrar y abrir un terminal nuevo. Es más limpio. Pero la opción B existe para casos donde no quieras."

**Tiempo:** ~90 segundos.

---

### Bloque 5 — `claude doctor`: la primera parada cuando algo falla (~2 min)

> "Y aquí viene el comando que la gamma marcó como **el más útil para diagnosticar problemas**. Si os recordáis, dije que antes de buscar en Stack Overflow o reinstalar todo, lanzáis `claude doctor`. Vamos a ver qué muestra."

**Tecleas:**

```powershell
claude doctor
```

**Aparece algo como:**

```
Claude Code Doctor v2.1.x
=========================

[Installation]
✓ Binary location: C:\Users\pedro\.local\bin\claude.exe
✓ Version: 2.1.x (latest)
✓ Auto-update: enabled
✓ Code signature: valid (Anthropic, PBC)

[Environment]
✓ Platform: Windows 11 (10.0.22631)
✓ Shell: PowerShell 7.4.0
✓ Git for Windows: 2.45.1 (will be used as Bash tool)
✓ PATH: properly configured

[Authentication]
✗ Not authenticated. Run 'claude' to log in.

[Configuration]
- User config: not found (~/.claude/settings.json)
- Project config: not found (.claude/settings.json)
- CLAUDE.md: not found in current directory

[MCP Servers]
- No MCP servers configured

[Network]
✓ Can reach api.anthropic.com
✓ TLS: OK

Run 'claude' to start authenticating.
```

**Vas señalando con el cursor las secciones a medida que lees:**

> "Mirad lo que me dice el doctor. Lo voy desglosando.
>
> **Installation** — todo en verde. El binario está donde toca, la versión es la última, auto-update enabled, la firma del código es válida. Todo lo del bloque anterior, confirmado.
>
> **Environment** — la plataforma, el shell, **Git for Windows detectado** y va a usarse como Bash tool. Esa línea es importante: confirma que Claude Code va a poder ejecutar comandos shell vía Git Bash.
>
> **Authentication** — y aquí está la primera **X roja** legítima de la demo. *'Not authenticated. Run claude to log in.'* Es el siguiente paso. El doctor me dice exactamente qué tengo que hacer.
>
> **Configuration** — todo 'not found'. Normal: no he configurado nada. El `CLAUDE.md` lo veremos en la 1.2b. Aquí solo es información.
>
> **MCP Servers** — vacío. Normal. Los MCP los veremos en el módulo 4 con Figma.
>
> **Network** — verde. Puedo llegar a `api.anthropic.com`, el TLS funciona. Si estuviera detrás de un firewall corporativo restrictivo, esto me lo diría aquí.
>
> Esta es la primera parada cuando algo falla. **`claude doctor`**. La mitad de los problemas se resuelven leyendo lo que dice el doctor."

**Tiempo:** ~2 minutos.

---

### Bloque 6 — Autenticación OAuth en navegador (~3 min)

> "Y vamos al paso que el doctor me ha dicho que falta. Autenticación. Lanzamos `claude` por primera vez."

**Tecleas:**

```powershell
claude
```

**Aparece en pantalla:**

```
Welcome to Claude Code!

To get started, you need to log in. We'll open your browser for OAuth.

[Press ENTER to continue, or type 'api-key' to use an API key instead]
```

> "Dos opciones. Login OAuth en navegador, o API key. La gamma cubrió las dos.
>
> Para uso interactivo en vuestra máquina del día a día, **siempre OAuth**. Más seguro, más cómodo, queda asociado a vuestra cuenta Pro o Max. La API key es para CI, scripts, automatización — la veremos cuando montemos hooks de pre-commit en el módulo 3. En ese contexto sí tiene sentido.
>
> Para esta máquina, OAuth. Pulsamos Enter."

**Pulsas Enter. Se abre el navegador. Aparece la página de OAuth de Claude.**

**El navegador muestra:**

```
Authorize Claude Code

Claude Code is requesting access to your Anthropic account.

This will allow Claude Code to:
- Access Claude API on your behalf
- Use your Pro/Max subscription quotas

Account: pedro@example.com (Claude Max)

[Authorize]    [Cancel]
```

> "Mirad. Se ha abierto el navegador. Veo que estoy logueado con mi cuenta — `pedro@example.com`, Claude Max. Si vosotros no estáis logueados en la cuenta correcta, el navegador os llevará a login primero. Asegúrate de loguearte con la cuenta del trabajo, no la personal, si tu equipo te la ha dado.
>
> Y atentos a los permisos que pide. Acceso a la API en mi nombre, uso de las cuotas de Max. Eso es. No pide acceso a otras cosas. **Le doy autorizar.**"

**Click en `[Authorize]`. El navegador muestra:**

```
✓ Authorization successful!
You can close this window and return to your terminal.
```

> "Y esto es lo que el navegador me dice: éxito. Cierro la ventana y vuelvo al terminal."

**Vuelves al terminal PowerShell. Ahora muestra:**

```
✓ Authentication successful!
   Account: pedro@example.com (Claude Max)
   Plan: Claude Max
   Quota: 50,000,000 tokens/month

Welcome to Claude Code v2.1.x

cwd: C:\Users\pedro\projects\ordermanagement
model: claude-opus-4.7
Type / for commands, ? for help

>
```

> "Y aquí está el agente. Autenticado con mi cuenta Max. El cwd es la raíz del proyecto OrderManagement, igual que vimos en la demo 1.1. El modelo, Opus 4.7. Y el prompt esperando.
>
> Lo más importante de este momento: **acabo de pasar de una máquina virgen a Claude Code operativo en menos de cinco minutos**. Tres comandos. `irm install.ps1 | iex`. `claude doctor`. `claude` y autorizar en navegador. Eso es todo."

**Tiempo:** ~3 minutos.

---

### Bloque 7 — Primera pregunta verificadora y cierre (~2 min)

> "Y para cerrar, una pregunta tonta para verificar que el agente está realmente operativo. No el ciclo agentic — eso ya lo vimos en la demo 1.1. Solo confirmar que responde."

**Tecleas en el prompt de Claude Code:**

```
> ¿Qué directorio crees que tienes como cwd? Responde con la ruta exacta.
```

> "Le pregunto algo trivial pero que solo puede responder bien si **realmente está operativo en el contexto correcto**. La ruta del directorio actual."

**Claude Code responde, sin necesidad de leer ficheros:**

```
Mi cwd actual es: C:\Users\pedro\projects\ordermanagement
```

> "Perfecto. Sabe dónde está. Eso confirma que el agente está realmente conectado, autenticado, con acceso al directorio correcto.
>
> Salgo:"

**Tecleas (o usas Ctrl+C):**

```
> Ctrl+C
```

> "Y ya está. Repaso de lo que hemos hecho, en cinco puntos.
>
> Uno. Verificamos que la máquina **NO** tenía Claude Code. Importante para que veáis el camino completo.
>
> Dos. Verificamos que Git for Windows estaba presente. **Es la única dependencia real**. Si vuestro equipo trabaja con .NET y no usa Git... bueno, problemas más graves que Claude Code.
>
> Tres. Un comando: `irm https://claude.ai/install.ps1 | iex`. Treinta segundos. Binario instalado en `~/.local/bin`, auto-update enabled, firma verificada.
>
> Cuatro. La trampa del PATH cacheado. **Cerrar y abrir un terminal nuevo** o refrescar `$env:PATH` en la sesión actual. Recordadlo, es el cinco por ciento de los reportes en GitHub Issues de Claude Code.
>
> Cinco. `claude doctor` para diagnosticar. `claude` para autenticar OAuth en navegador. Ya está operativo.
>
> En la siguiente demo, la 1.2b, vamos a darle contexto al agente sobre **vuestro proyecto** — `CLAUDE.md`, `settings.json`, permisos. Y vais a notar el contraste enorme entre Claude Code 'en frío' como en la 1.1 y Claude Code con un `CLAUDE.md` decente. Esa es la pieza pedagógica estrella del módulo."

**Tiempo:** ~2 minutos.

---

## 9. Qué resaltar verbalmente

Cinco puntos que **no pueden quedarse sin decir**:

1. **"En 2026, ya no se necesita WSL en Windows."** — error histórico. Hay tutoriales antiguos que siguen pidiendo WSL. **Olvidaos de esos tutoriales.** El comando `irm install.ps1 | iex` instala nativo.

2. **"PowerShell, no CMD."** — diferencia visual: `PS C:\...` (PowerShell) vs `C:\...` (CMD). Si el alumno se equivoca, va a tener errores raros.

3. **"La trampa del PATH cacheado."** — el quinto por ciento de los issues. Tras instalar, **abrir terminal nuevo** o `$env:PATH = "$env:PATH;$env:USERPROFILE\.local\bin"`.

4. **"`claude doctor` es la primera parada."** — antes de Stack Overflow, antes de reinstalar, antes de cualquier cosa. La mitad de los problemas los diagnostica.

5. **"OAuth para máquina personal, API key solo para CI."** — el alumno tiene que saber cuándo cada uno. Si configura API key en su portátil de trabajo, está abriendo agujero de seguridad.

**Frase de remate al final, que conviene memorizar:**

> *"Tres comandos. `irm install.ps1 | iex`. `claude doctor`. `claude` y autorizar. De máquina virgen a operativo en menos de cinco minutos."*

---

## 10. Slide de entrada (locución HeyGen)

> **Texto del avatar antes del screencast:**

"Y vamos a la siguiente demo. En la 1.1 vimos qué hace Claude Code. Toca instalarlo. Esta demo es la mecánica pura: cómo se enciende Claude Code en una máquina Windows virgen. Tres comandos en PowerShell. Sin WSL, sin Docker, sin Node. Es la novedad de 2026 y conviene que la veáis con vuestros ojos para que no perdáis tiempo con tutoriales antiguos. Vais a ver tres cosas. Una: el comando único de instalación con `irm` y `iex`. Dos: la trampa más típica en Windows, el PATH cacheado, y cómo se resuelve. Tres: `claude doctor` como primera parada cuando algo falla, y el login OAuth en navegador. Vamos al screencast."

---

## 11. Slide de salida (locución HeyGen)

> **Texto del avatar al volver del screencast:**

"Ya tenéis Claude Code instalado, autenticado y respondiendo en PowerShell. Tres comandos: `irm` con `iex`, `claude doctor`, y `claude` para autorizar. Punto. Si os habéis quedado con la sensación de *'pues más fácil de lo que esperaba'*, ese era el objetivo. La instalación no es donde el módulo gana valor — es solo el prerequisito. Donde el módulo gana valor es en darle a Claude Code contexto del proyecto en el que vais a trabajar. Y eso es lo siguiente. Vamos a construir un `CLAUDE.md` decente para OrderManagement, configurar `settings.json` con permisos sanos, y ver con vuestros ojos el contraste entre Claude Code 'en frío' y Claude Code con configuración propia. Esta es la pieza pedagógica estrella del módulo. Empezamos con el cinco punto uno punto dos B."

---

## 12. Tiempo total estimado

| Bloque | Tiempo |
|---|---|
| Slide de entrada (avatar) | ~30 seg |
| Bloque 1 — Setup visible | ~1 min 30 seg |
| Bloque 2 — Verificación de prerequisitos | ~1 min |
| Bloque 3 — Instalación con `irm` | ~2 min |
| Bloque 4 — La trampa del PATH | ~1 min 30 seg |
| Bloque 5 — `claude doctor` | ~2 min |
| Bloque 6 — Autenticación OAuth | ~3 min |
| Bloque 7 — Primera pregunta verificadora y cierre | ~2 min |
| Slide de salida (avatar) | ~30 seg |
| **Total screencast** | **~12-13 min** |
| **Total con avatar** | **~13-14 min** |

> Si hay preguntas del alumno durante el screencast, súmale 2-3 minutos. La demo está pensada para encajar en un bloque de **15 minutos** dentro de la sesión.

**Margen de seguridad por si algo va lento:**

- **Si la descarga del instalador tarda más de 30 segundos**, comenta lo que está pasando: *"está bajando el binario desde los servidores de Anthropic. Verifica la firma con la clave pública. En conexiones rápidas son segundos, en redes corporativas a veces tarda."*. Eso es contenido, no relleno.

- **Si `claude doctor` muestra alguna X roja inesperada** (por ejemplo, falla la red por proxy corporativo), **úsalo como ejemplo pedagógico**. *"Mirad, esto es exactamente lo que va a pasaros en algunos entornos. El doctor os dice qué falla. En este caso, la red. Tenéis que configurar el proxy en `~/.claude/settings.json` con un bloque `network`. Lo dejaremos para más adelante."* Y sigues con la demo.

- **Si OAuth se queda colgado** (bloqueador de pop-ups, ad-blocker), **es el caso típico de la gamma slide 17**. Comenta: *"Esto que veis aquí es el caso del slide 17. Bloqueador de pop-ups o ad-blocker en el navegador. Lo desactivo, recargo, y autorizo."*. Y lo arreglas en vivo.

- **Si la primera pregunta verificadora del bloque 7 falla** porque el agente no responde, no improvises silencio. *"A veces la primera petición tarda. El agente está conectándose, autenticando con sus servidores. Es normal en el primer arranque."*

- **Si la máquina ya tiene Claude Code instalado** y has olvidado desinstalar antes de grabar, **NO grabes la demo**. Páralo y desinstala primero. La fuerza pedagógica de empezar con `claude --version` dando error rojo es el bloque 1 entero. Sin eso, la demo pierde sentido.

---

# Apéndice A — Decisiones pedagógicas justificadas

> Esta sección no se graba. Es para Pedro, formador.

**¿Por qué empezar mostrando que Claude Code NO está instalado?**

Porque es la única forma honesta de mostrar el camino real. Si arrancas la demo con `claude --version` ya devolviendo `2.1.x`, el alumno no ve la transformación de virgen a operativo. **El bloque 1 con el error rojo es la base de toda la demo.**

**¿Por qué dedicar bloque entero a la trampa del PATH cacheado?**

Porque es el problema **número uno** de Windows según la documentación oficial y los issues de GitHub. Si la demo no lo cubre, el alumno se va a topar con él el lunes en su máquina y va a pensar que la herramienta está rota. Mejor verlo en clase y aprender la solución (cerrar terminal o `$env:PATH = ...`).

**¿Por qué OAuth y no API key en la demo?**

Porque OAuth es el flujo del 95% de los devs. La API key es solo para CI, hooks, scripts. Si la demo enseña API key, el alumno puede pensar que es la opción "buena" y configurar API key en su portátil — abriendo agujero de seguridad. **OAuth para humano, API key solo para máquina.**

**¿Por qué el commit final (sección 6) solo cambia una línea?**

Porque la rama `demo/1.2a` no aporta nada al **código** del proyecto. Lo que aporta está en la **máquina del alumno**. La rama solo refleja "esta demo se hizo" para el roadmap. Si añadiéramos `CLAUDE.md` aquí, invadiríamos terreno de la 1.2b.

**¿Por qué `irm | iex` y no `winget install`?**

Porque es el comando oficial recomendado por Anthropic en 2026. `winget install Anthropic.ClaudeCode` también funciona, pero es alternativa. La documentación oficial recomienda `irm | iex` y eso es lo que el alumno se va a encontrar al googlear. Coherencia con la doc.

**¿Por qué no demuestro la vía npm?**

Porque la gamma 1.2a la cubrió en 2 minutos (slide 8) y la presentó como **alternativa legacy**. Demostrarla en vivo daría señal de que es comparable, cuando no lo es. La doc oficial recomienda el native installer; demostremos el native installer.

**¿Por qué la primera pregunta verificadora es "¿qué directorio tienes como cwd?"?**

Porque es la pregunta más simple posible que **solo se puede responder bien si el agente está realmente operativo en el contexto correcto**. Si respondiera mal o genérico, sabríamos que algo falla. Es prueba de vida sin invadir el contenido de demos posteriores (no usa el ciclo agentic completo, no genera código, no toca el repo).
