> **Versión:** v3 | **Módulo:** 1 | **Sub:** 1.2a | **Slides:** 22 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M01-S1.2a-instalacion-autenticacion-v3.md`

# Submódulo 1.2a — Instalación, verificación y autenticación

## Slide 1 — Portada
**Módulo 1 · Submódulo 1.2 · Parte A**
Instalación, verificación y autenticación
De terminal vacía a Claude Code respondiendo

---

## Slide 2 — La parte mecánica donde tropieza la gente

Ya tenemos el modelo conceptual del 1.1. Ahora toca dejar Claude Code en marcha.

Esta es la parte más mecánica del módulo. Pero también donde más gente tropieza. Y curiosamente, no por la instalación en sí — que es razonablemente simple — sino por tres o cuatro decisiones que parecen menores y condicionan todo lo que viene después.

He visto a más de un equipo terminar esta sesión con Claude Code instalado y... poco más. *"Sí, ya lo tengo"*. A la semana siguiente, abandonado.

> Sin un `CLAUDE.md` decente, Claude Code en tu repo es como un cocinero recién contratado
> al que no le has dicho ni el menú del día. Sabe cocinar.
> Pero en *tu* cocina, va perdido.

El `CLAUDE.md` lo vemos en 1.2b. Aquí lo dejamos instalado, autenticado y verificando que responde.

---

## Slide 3 — Sistema operativo: estado en 2026

```
macOS 13 (Ventura) o superior
└── Sin sorpresas. Instalación directa, todo nativo.

Linux Ubuntu 20.04+, Debian 10+ o equivalente moderno
└── Cualquier distro reciente con bash o zsh va bien.

Windows 10/11
├── Hasta principios de 2026 era WSL2 obligatorio.
├── Ahora hay instalador nativo (PowerShell o CMD).
└── Si Git Bash está instalado, Claude Code lo usa por debajo.
```

Si venís de tutoriales antiguos que mencionan WSL2 como obligatorio para Windows: **ignoradlos**. Es una limitación que ya no aplica.

> Nota: si vuestro equipo ya tiene WSL configurado para otros proyectos
> (Node, Python, Docker), seguid usándolo si os va bien.
> El instalador nativo es "menos fricción para empezar", no "la única vía válida".

---

## Slide 4 — Cuenta de pago: la sorpresa típica

**La capa gratuita de Claude.ai NO incluye Claude Code.**

Esto coge a algunos por sorpresa. Mejor saberlo antes de la sesión 1 que descubrirlo a mitad.

| Plan | Precio | Para quién |
|---|---|---|
| **Pro** | 20 $/mes | Devs que usan Claude Code unas horas al día. Entry point razonable. |
| **Max** | 100-200 $/mes | Quien tira de Claude Code todos los días varias horas |
| **Teams / Enterprise** | A medida | Equipos con gestión centralizada y controles administrativos |
| **API por consumo** | Pago por tokens | CI/CD, scripts, integraciones, uso intermitente |

**Recomendación práctica:**

```
Empieza con Pro.

En una semana sabrás si te quedas corto.

Saltar Pro → Max es trivial.
Pagar Max desde el día uno y no aprovecharlo es tirar dinero.
```

---

## Slide 5 — Node.js: solo si vas por la vía npm

Node 18 o superior. Node 22 LTS es la recomendación práctica.

Si tu Node está viejo, la forma limpia es `nvm`:

```bash
# Instalar nvm
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.40.0/install.sh | bash

# Recargar shell
source ~/.bashrc   # o ~/.zshrc según tu shell

# Instalar y activar Node 22 LTS
nvm install 22
nvm use 22

# Verificar
node --version    # v22.x.x
npm --version     # cualquier 8+ vale
```

**Y aquí viene un detalle importante:**

```
Si vas con el instalador nativo (la vía recomendada ahora)
└── NO necesitas Node para nada.
    Es un binario autosuficiente.
```

> Esto te ahorra mantener Node solo para Claude Code en una máquina
> donde quizá no programas en JavaScript.

---

## Slide 6 — Dos vías oficiales de instalación

```
┌─────────────────────────────────────────────────────────┐
│                                                         │
│   OPCIÓN A — Native installer    ✅ RECOMENDADO          │
│   ─────────────────────────                             │
│   ├── Cero dependencias                                 │
│   ├── Auto-update en background                         │
│   └── Vía primaria que Anthropic prueba y soporta       │
│       desde principios de 2026                          │
│                                                         │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│                                                         │
│   OPCIÓN B — npm                  Funciona, no es la primaria│
│   ─────────────                                         │
│   ├── Requiere Node 18+                                 │
│   ├── Tú te ocupas de actualizarla                      │
│   └── Solo si tienes razón concreta para usarla         │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

Conviene elegir bien al principio. Cambiar después es engorroso — no imposible, pero un poco rollo.

---

## Slide 7 — Native installer: comandos por sistema

**macOS y Linux:**

```bash
curl -fsSL https://claude.ai/install.sh | bash
```

**Windows (PowerShell o CMD):**

```
Sigue el script desde:
docs.claude.com/en/docs/claude-code/setup
```

> En Windows, conviene tener Git para Windows instalado.
> Claude Code lo usa por debajo cuando ejecuta comandos.

**Qué hace el instalador:**

```
1. Descarga el binario
2. Lo coloca en el PATH
3. Configura los auto-updates
```

Toma menos de un minuto. Ningún paso que tengas que hacer manualmente.

---

## Slide 8 — Vía npm: cuándo merece la pena

```bash
npm install -g @anthropic-ai/claude-code
```

**Tres casos donde npm sí tiene sentido:**

```
1. Necesitas pinear una versión concreta
   └── En CI/CD quieres reproducibilidad — la versión que probaste
       ayer es la que quieres en el pipeline hoy

2. Tu equipo estandariza todo en npm
   └── Si tienes scripts que asumen `npm install`, ser consistente vale

3. Quieres incluir Claude Code como dependencia local de un proyecto
   └── Útil cuando lo usas en hooks de Husky o tareas de package.json
```

Para el día a día normal, el native installer da menos problemas.

> La vía npm es legítima — pero requiere que TÚ te ocupes
> de actualizarla con `npm update -g`.

---

## Slide 9 — Aviso importante: nunca uses sudo

Esto es la fuente número uno de errores entre devs nuevos con npm.

```
Si te da error EACCES con npm install -g

❌ NO es señal de que necesites sudo
✅ Es señal de que tu npm está mal configurado
```

`sudo` no es la solución correcta. Abre agujeros de seguridad, instala con permisos de root cosas que no deberían tenerlos, y crea problemas que arrastras durante meses.

---

## Slide 10 — Cómo arreglar EACCES sin sudo

```bash
# La forma limpia: configurar el prefix de npm en tu home
npm config set prefix '~/.npm-global'

# Añadir el bin de tu home al PATH
export PATH=~/.npm-global/bin:$PATH

# Y guardar esa línea de export en tu ~/.zshrc o ~/.bashrc
echo 'export PATH=~/.npm-global/bin:$PATH' >> ~/.zshrc
source ~/.zshrc
```

**Alternativa todavía más limpia:** usa **nvm**.

```
Cuando Node se instala vía nvm:
├── Vive en tu home
└── npm install -g no necesita permisos de root nunca
```

---

## Slide 11 — La decisión, en una línea

```
┌─────────────────────────────────────────────────────────┐
│                                                         │
│   ¿Es tu primera instalación?                           │
│                                                         │
│   → Native installer                                    │
│                                                         │
│   ¿Tienes razón concreta para npm?                      │
│   (versión específica, CI, ecosistema npm consolidado)  │
│                                                         │
│   → npm                                                 │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

---

## Slide 12 — Verificación: claude --version

Después de instalar, independientemente de la vía:

```bash
claude --version
```

Debería devolver el número de versión (a fecha de hoy va por la 2.1.x).

**Si te dice `command not found`:**

```
Lo más habitual
├── El PATH no tiene el directorio de instalación
├── Cierra el terminal y abre uno nuevo
└── La mayoría de veces se resuelve solo

Si persiste
├── Revisa que el directorio de instalación está en tu PATH
├── nvm: ~/.nvm/versions/node/v22.x.x/bin
└── Native installer: ~/.local/bin típicamente
```

---

## Slide 13 — claude doctor: la herramienta más útil para diagnosticar problemas

```bash
claude doctor
```

Este comando ejecuta diagnósticos del entorno:

```
├── Estado de auth
├── PATH
├── Configuración
└── MCP servers conectados
```

Es la primera parada cuando algo no va bien.

> Antes de buscar en Stack Overflow, antes de preguntar al canal de Slack,
> antes de reinstalar.
>
> La mitad de las veces te dice exactamente qué falla.

---

## Slide 14 — El primer arranque

La primera vez que lances `claude` en una terminal, te pide login.

Hay dos formas de autenticarse:

```
1. Login interactivo  → lo normal
2. API key            → para CI, scripts, automatización
```

Las vemos.

---

## Slide 15 — Login interactivo (lo normal)

```bash
claude
```

```
1. Abre el navegador
2. Te lleva a OAuth
3. Autorizas la aplicación
4. Vuelves al terminal con la sesión iniciada
```

Funciona con Pro, Max, Teams y Enterprise.

**Una nota práctica:** si trabajas con varias cuentas (personal y de empresa, por ejemplo), la cuenta queda asociada al sistema. Cambiar es fácil pero hay que saberlo:

```bash
claude /logout
claude /login
```

---

## Slide 16 — API key: para CI, scripts y automatización

Cuando no hay un humano para abrir un navegador:

```bash
export ANTHROPIC_API_KEY=sk-ant-...
```

Configúralo en variables de entorno del sistema o en el secret manager de tu CI.

**Las dos reglas:**

```
❌ Nunca en código que vaya a git
❌ Nunca en CLAUDE.md
```

Cuando lo digo en clase parece evidente. Lo digo igualmente porque el día menos pensado alguien hace `git push` con una API key dentro.

---

## Slide 17 — Errores típicos del primer arranque

```
❌ "Permission denied" al ejecutar claude
   El binario no es ejecutable.
   └── chmod +x sobre el binario suele resolverlo.

❌ "Could not find Claude Code" en Windows tras instalar
   Cierra todos los terminales y abre uno nuevo.
   └── Windows a veces tarda en recoger cambios de PATH.

❌ OAuth se queda colgado
   Desactiva bloqueadores de pop-ups y ad-blockers
   └── En el navegador donde se abrió.

❌ La API key no funciona
   ├── Verifica que la variable está exportada
   │   (echo $ANTHROPIC_API_KEY debería mostrarla)
   └── Y que tu cuenta tiene saldo
```

---

## Slide 18 — Test rápido de instalación

Para confirmar que todo está bien antes de pasar a la parte B:

```bash
# 1. Versión instalada
claude --version
# → 2.1.x o superior

# 2. Diagnóstico del entorno
claude doctor
# → todos los checks en verde

# 3. Que responde a una petición
claude
> ¿estás operativo?
# → respuesta razonable
```

Si los tres están en verde, **base operativa lista**.

---

## Slide 19 — Errores frecuentes en esta fase

```
❌ USAR sudo CON npm
   Ataja el problema a corto plazo. Lo multiplica a medio.
   └── Configura el prefix bien, o usa nvm.

❌ NO USAR claude doctor CUANDO ALGO FALLA
   Antes de buscar en internet, lánzalo.
   └── Casi siempre te ahorra el viaje.
```

---

## Slide 20 — Lo que tienes ahora

```
✅ Claude Code instalado en tu sistema
   ├── Vía native installer (recomendado)
   └── O vía npm (si tienes razón para ello)

✅ Cuenta autenticada
   ├── Login OAuth (uso normal)
   └── O API key (CI, scripts, automatización)

✅ claude --version responde
✅ claude doctor en verde
✅ El agente responde a peticiones simples
```

Lo que te falta para que sea **operativo en tu repo**:

```
⏳ Crear el CLAUDE.md del proyecto
⏳ Configurar .claude/settings.json
⏳ Definir permisos (allow / deny)
```

Eso es la parte B de este submódulo.

---

## Slide 21 — Lo que viene en 1.2b

```
SUBMÓDULO 1.2b — CONFIGURACIÓN DEL PROYECTO
─────────────────────────────────────────────────────

CLAUDE.md
├── Qué es y dónde vive
├── Qué meter (y qué NO meter — esto es donde más se falla)
├── Anatomía de un buen CLAUDE.md para .NET + Angular
└── Tres patrones según el tipo de proyecto

AGENTS.md
└── Estándar cross-tool: cuándo usarlo (y cuándo no compensa)

settings.json y los tres scopes
├── User (~/.claude/settings.json)
├── Project (.claude/settings.json en repo)
└── Local (.claude/settings.local.json)

Permisos: el modelo de seguridad
├── Cómo funciona por defecto
├── allow y deny con ejemplos para .NET + Angular
├── Patrones por tipo de proyecto
└── Modo --dangerously-skip-permissions: cuándo sí, cuándo nunca

Plantilla CLAUDE.md lista para llevarte al puesto
```

Esto es el verdadero entregable de la sesión. La instalación es prerequisito.

---

## Slide 22 — Cierre y siguiente

Lo que tienes ahora:

```
✅ Claude Code instalado y verificado
✅ Cuenta autenticada
✅ claude doctor como primera parada cuando algo falla
```

**El agente responde.** La base mecánica está lista. Ahora vamos a darle algo con lo que trabajar.

```
SIGUIENTE → 1.2b Configuración del proyecto
```

**Nos vemos en 1.2b.**
