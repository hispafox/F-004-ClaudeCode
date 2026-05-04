# Demo 2.2b — Plantillas y scripts: `angular-component` versiones 3 y 4

> **Versión:** v1 | **Módulo:** 2 | **Sub:** 2.2b | **Estado:** ✅ Versión final
> **Archivo:** `demo_M02-S2.2b-plantillas-scripts-windows-v1.md`
> **Branch before:** `demo/2.2b-before`  (skill v2 intacto, sin assets/ ni scripts/)
> **Branch after:**  `demo/2.2b-after`   (estado final pre-cocinado con angular-component v4 commiteado)
> **Branch parent:** `demo/2.2a-after`
> **Tiempo total estimado:** ~26-30 minutos
> **Tipo:** Demo de evolución a nivel producción (CÓDIGO). **Subimos el skill `angular-component` que construimos en 2.2a a sus dos siguientes capas: v3 con plantillas en `assets/` (cuando las convenciones se vuelven plantillas) y v4 con script ejecutable en `scripts/` (cuando hay tareas deterministas como conversión de nombres y validación de duplicados).** Es donde el alumno ve cómo un skill sube a nivel producción real. Sigue el patrón **before/after** definido en [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md).
> **Plataforma:** Windows 11 (PowerShell 7 + Python 3 disponible).

---

## 1. Contexto

En la 2.2a construimos `angular-component` v1 (skill mínimo, ~30 líneas) y v2 (con convenciones del equipo, ~150 líneas). Cerramos con las dos señales para parar de añadir prosa: pasar de 2.000 palabras o que las convenciones se vuelvan plantillas.

La gamma 2.2b (32 slides, ~30 min) cubrió las dos capas siguientes:

- **Sintaxis para ejecutar comandos en `SKILL.md`** — inline con `` !`comando` `` y bloques con ` ```! ` (slides 4-9).
- **Versión 3: plantillas en `assets/`** — cuando los bloques de código de ejemplo se vuelven repetitivos (slides 12-19).
- **Versión 4: script ejecutable en `scripts/`** — cuando hay tareas deterministas (slides 20-26).
- **Cuándo NO subir de versión** — la regla de oro (slides 29-30).

Esta demo aterriza la gamma. Aplica la primera señal de parada (las convenciones del fichero `.ts` ya son plantillas en la v2 — extraerlas a `assets/`) y luego añade un script Python para conversiones deterministas y validaciones.

> **Tipo de demo:** evolución estructural. La rama `demo/2.2b-after` queda con el skill **completo a nivel producción**, listo para ser distribuido al equipo. Es la primera demo del curso donde el alumno ve un script ejecutable funcionando dentro de un skill.

---

## 2. Objetivo de la demo

Cinco cosas concretas que tienen que quedar en la cabeza del alumno cuando termine los ~26 minutos de screencast:

1. **Las plantillas extraídas a `assets/` no son una optimización: son una decisión de mantenibilidad.** Cuando el equipo cambia la convención, modificas la plantilla, no la prosa del skill. Ejemplo concreto: si Angular 20 cambia algo en el patrón de componentes, **modificas un fichero `.template.ts`, no relees todo el `SKILL.md`**.

2. **Los placeholders `{{...}}` son el contrato entre la plantilla y el skill.** El alumno tiene que poder escribir su propia plantilla con placeholders. Demostramos `{{KEBAB_NAME}}`, `{{PASCAL_NAME}}`, `{{INPUTS}}`, `{{INJECTIONS}}`. La gamma 2.2b slide 14-15 los introdujo. Aquí los aplicamos.

3. **Los scripts son para tareas deterministas. La regla de oro: si la tarea tiene una respuesta correcta, script. Si requiere criterio, prosa.** El script de ejemplo hace tres cosas: convertir `OrdersList` a `orders-list`, comprobar si la carpeta existe ya, y generar el JSON con la información necesaria.

4. **La sintaxis inline `` !`comando` `` y la sintaxis en bloque ` ```! ` son herramientas distintas.** Inline para incrustar un valor en una frase. Bloque para ejecutar varios comandos antes de actuar. Ambas se ejecutan al activarse el skill.

5. **No todos los skills necesitan v3 ni v4.** La regla de oro de la gamma slide 29: solo subes de versión cuando aparece una señal concreta. Subir antes de tiempo es **complicar el skill sin razón**. La demo cierra con esta advertencia.

Y dos cosas que tienen que **NO quedar** en su cabeza:

- *"Cuanto más completo el skill, mejor."* — al revés. **Lo simple es la opción por defecto.** Subir de versión es respuesta a una necesidad concreta.
- *"El script puede tomar decisiones de criterio."* — no, **el script hace lo determinista**. Las decisiones siguen siendo del modelo razonando.

---

## 3. Branch `demo/2.2b-before`

Punto de partida del screencast.

```
demo/2.2b-before
```

**Parte de:** `demo/2.2a-after`.

**Estado del repo:** el skill `angular-component` v2 instalado en `.claude/skills/angular-component/SKILL.md` (~150 líneas). **No hay aún `assets/` ni `scripts/`** dentro del skill — esa es la pieza viva. (Nota: la rama `demo/2.2a-after` no contiene los componentes `OrderSummary` que se generaron en la grabación de 2.2a — esos eran ejemplos volátiles que se descartaron al cerrar.)

> El formador hace `git checkout demo/2.2b-before` antes de empezar a grabar. Sube el skill v2 → v3 (assets) → v4 (scripts) en directo.

---

## 4. Branch `demo/2.2b-after`

Estado final que la siguiente clase (2.2c) asume.

```
demo/2.2b-after
```

**Parte de:** `demo/2.2b-before`.

**Qué añade respecto a `-before`:** tres cosas estructurales al repo — la carpeta `.claude/skills/angular-component/assets/` con tres plantillas (component, template, spec), la carpeta `.claude/skills/angular-component/scripts/` con `generate.py`, y el `SKILL.md` actualizado a v4 (más corto que el v2 porque la prosa de plantillas se sustituyó por una mención a `assets/`). Más la marca `[x]` en `docs/DEMOS.md`. **Es la primera rama del curso con un skill con la estructura de tres carpetas funcionando junta**.

**Cómo se prepara:** ver §6b. Se materializa antes de grabar.

> Durante la grabación, el formador construye assets/ y scripts/ en directo, prueba v3 generando `OrderFilter`, prueba v4 con colisión sobre `OrderSummary`. Al cerrar descarta los componentes de prueba y la siguiente clase parte de `demo/2.2b-after` ya pre-cocinada.

---

## 5. Estado del repo al hacer `git checkout demo/2.2b-before`

Idéntico a `demo/2.2a-after`:

```
ordermanagement/
├── .claude/
│   ├── settings.json
│   └── skills/
│       └── angular-component/
│           └── SKILL.md                    (v2, ~150 líneas)
├── docs/
│   ├── DEMOS.md                            (hasta 2.2a marcada)
│   └── skills-explorados.md
├── scripts/
├── src/                                    (sin cambios .NET)
├── frontend/
│   └── src/app/
│       ├── components/
│       │   └── order-summary/              (componente generado en 2.2a)
│       └── orders/
├── tests/
├── .gitignore
├── CLAUDE.md
└── README.md
```

**Estado clave para la demo:**

- El skill `angular-component` v2 está activo y funcional.
- **Python 3 disponible** en la máquina del formador. Lo verificamos en el bloque 1. Si no está, hay que instalarlo antes (la gamma 2.2b slides 22-23 muestra el script en Python).
- El componente `OrderSummary` generado en 2.2a está en `frontend/src/app/components/order-summary/`. Lo usamos como evidencia de cómo se ve un componente generado por v2 antes de subir a v3/v4.

**Estado de la máquina Windows del formador:**

```
✅ Claude Code v2.1.x instalado y autenticado
✅ Git for Windows
✅ PowerShell 7
✅ Python 3.11+ disponible (verificar con: python --version)
✅ VS Code con el repo cargado en demo/2.2b-before
✅ CLAUDE.md y .claude/settings.json operativos
✅ Skill angular-component v2 cargable desde .claude/skills/
```

> **Importante para Pedro antes de grabar:** verifica `python --version` en PowerShell. Si Python no está, instálalo desde `python.org` o `winget install Python.Python.3.12`. Sin Python, el bloque del script v4 se cae al ejecutar.

**Lo que el alumno verá al final de la demo:**

- Demostración de las dos sintaxis inline y bloque para ejecutar comandos en `SKILL.md`.
- Tres plantillas creadas en `.claude/skills/angular-component/assets/` con placeholders documentados.
- El `SKILL.md` reducido en líneas (al sustituir prosa por referencias a `assets/`).
- Prueba del v3 generando otro componente (`OrderFilter`) que respeta las plantillas.
- Script `generate.py` creado en `.claude/skills/angular-component/scripts/`.
- Demostración del script en aislado: ejecutado con `python generate.py OrdersList` desde la terminal.
- El `SKILL.md` actualizado a v4 que llama al script al activarse.
- Prueba del v4 generando un componente con nombre que **ya existe** (`OrderSummary`) — el script detecta la colisión y el skill avisa.
- Mención clara: *"no todos los skills necesitan v3 ni v4"*.

---

## 6a. Prompt para Claude Code — preparar `demo/2.2b-before`

> Crea la rama de partida del screencast desde `demo/2.2a-after`. **No crea plantillas ni scripts ni modifica el SKILL.md** — la pieza viva es la evolución v2 → v3 → v4 en pantalla. La rama `-before` queda idéntica a `demo/2.2a-after` (skill v2 intacto).

````
Estoy preparando la demo 2.2b del curso de Claude Code (evolución del
skill angular-component v2 → v3 con assets/ → v4 con scripts/). Sigue
el patrón before/after (ver demo M0.2).

Quiero que prepares la rama `demo/2.2b-before` desde `demo/2.2a-after`.
Esta rama es el punto de partida del screencast: el skill sigue en v2,
sin assets/ ni scripts/.

## Tarea única

```powershell
git checkout demo/2.2a-after
git pull
git checkout -b demo/2.2b-before
```

NO toques el SKILL.md, NO crees `.claude/skills/angular-component/assets/`
ni `.claude/skills/angular-component/scripts/`, NO marques nada en
docs/DEMOS.md. Esos artefactos van en `demo/2.2b-after` (ver §6b).

NO hagas commit. La rama `demo/2.2b-before` es exactamente igual a
`demo/2.2a-after` excepto en el nombre.

# Cuando termines, dime

1. Que la rama demo/2.2b-before está creada.
2. Que `git diff demo/2.2a-after demo/2.2b-before` no muestra cambios.
````

---

## 6b. Prompt para Claude Code — preparar `demo/2.2b-after`

> Materializa la rama final con el skill `angular-component` v4 pre-cocinado: plantillas en `assets/`, script en `scripts/`, SKILL.md actualizado.

````
Estoy preparando la demo 2.2b del curso de Claude Code. Esta rama
-after pre-cocina la evolución del skill angular-component v2 → v3
(plantillas en assets/) → v4 (script ejecutable) que el formador
construirá en vivo durante el screencast.

# Contexto

Estoy en la rama `demo/2.2b-before` del repo `ordermanagement`. La rama
parte de `demo/2.2a-after` y tiene el skill `angular-component` v2
en `.claude/skills/angular-component/SKILL.md` pero NO tiene aún
`assets/` ni `scripts/`.

Quiero que prepares la rama `demo/2.2b-after` desde `demo/2.2b-before`
con el skill completo a nivel v4 y la marca [x] en docs/DEMOS.md.

# Lo que necesito

Cuatro tareas:

## Tarea 1: crear la rama

```powershell
git checkout demo/2.2b-before
git checkout -b demo/2.2b-after
```

## Tarea 2: crear plantillas en `.claude/skills/angular-component/assets/`

Tres ficheros plantilla con placeholders `{{...}}`:

- `component.template.ts` — esqueleto del componente Angular standalone con
  Signals, con placeholders `{{KEBAB_NAME}}`, `{{PASCAL_NAME}}`, `{{INPUTS}}`,
  `{{INJECTIONS}}`. Respeta los 8 bloques en orden estricto del v2.
- `component.template.html` — esqueleto de template con control flow nuevo
  (`@if`/`@for`/`@switch`) y placeholder `{{TEMPLATE_BODY}}`.
- `component.template.spec.ts` — esqueleto del fichero spec con xUnit-style
  para Angular y placeholders `{{KEBAB_NAME}}`, `{{PASCAL_NAME}}`.

## Tarea 3: crear `.claude/skills/angular-component/scripts/generate.py`

Script Python 3 que:
- Toma como argumento un nombre de componente en cualquier convención
  (ej. `OrdersList`, `orders-list`).
- Convierte a PascalCase y kebab-case.
- Comprueba si `frontend/src/app/components/<kebab-name>/` ya existe
  (si sí, sale con stderr y exit 1).
- Si no existe, imprime un JSON con `{ pascalName, kebabName, targetDir }`
  que el skill consume.
- Manejo de errores claro y portable a Windows (no usa rutas POSIX hardcoded).

## Tarea 4: actualizar SKILL.md a v4 + marcar DEMOS.md + commit

Reescribe `.claude/skills/angular-component/SKILL.md` reduciéndolo respecto
a la v2: la prosa de plantillas se sustituye por referencias al `assets/`,
y se añade un bloque `` ```! `` que ejecuta el script `scripts/generate.py`
con el nombre del componente al activarse el skill. Mantén el frontmatter
con `name`, `description` (actualizada para indicar que el skill ahora usa
plantillas y script), y respeta las 5 reglas técnicas críticas.

Marca la 2.2b en `docs/DEMOS.md`:

```
- [x] **demo/2.2b** — angular-component v3 (assets) y v4 (scripts)
```

Verifica con `dotnet build` (0 warnings, 0 errors) y commit:

```powershell
git add .claude/skills/angular-component docs/DEMOS.md
git commit -m "demo/2.2b-after: angular-component v4 con assets/ y scripts/"
```

NO hagas push.

# Restricciones (importantes)

- NO toques el código .NET ni Angular (los componentes generados son
  volátiles y no entran al repo).
- NO modifiques CLAUDE.md ni .claude/settings.json.
- NO modifiques README.md ni .gitignore.
- El script Python debe funcionar invocado desde Git Bash o PowerShell
  en Windows, y debe usar rutas portables.

# Cuando termines, dime

1. Que la rama demo/2.2b-after está creada desde demo/2.2b-before.
2. Que existen las tres plantillas en `assets/` con sus placeholders.
3. Que `scripts/generate.py` existe y se ejecuta con `python scripts/generate.py OrdersList`.
4. Que SKILL.md está reducido y referencia las plantillas + script.
5. Que docs/DEMOS.md está marcado.
6. Que dotnet build pasa.
7. Que el commit está hecho.

Si tienes dudas, para y pregúntame.
````

---

## 7. Artefactos que Claude Code debe generar

```
✓ Rama demo/2.2b-before (parte de demo/2.2a-after) — sin cambios respecto al parent
✓ Rama demo/2.2b-after (parte de demo/2.2b-before) con:
  ├── .claude/skills/angular-component/SKILL.md (v4, reducido + bloque ```!)
  ├── .claude/skills/angular-component/assets/ (3 plantillas con placeholders)
  ├── .claude/skills/angular-component/scripts/generate.py (script ejecutable)
  └── docs/DEMOS.md con 2.2b marcada como [x]
✓ Verificación de build OK: dotnet build limpio
✓ Commit en demo/2.2b-after: "demo/2.2b-after: angular-component v4 con assets/ y scripts/"
```

**Lo que NO debe haber generado:**

- ❌ Modificaciones al `SKILL.md` (eso se hace EN VIVO)
- ❌ Carpeta `assets/` (creada en vivo)
- ❌ Carpeta `scripts/` dentro del skill (creada en vivo)
- ❌ Cambios en CLAUDE.md o `.claude/settings.json`
- ❌ Cambios en código .NET ni Angular

> Si Claude Code se anticipa y crea las carpetas `assets/` o `scripts/`, **se rechaza el output**. La construcción de v3 y v4 es el corazón pedagógico de esta demo.

**Lo que el formador commitea EN VIVO sobre `demo/2.2b-before` durante el screencast:**

```
Durante la grabación, sobre demo/2.2b-before, se hace un commit ficticio:
- "demo/2.2b-after: skill angular-component v3 (plantillas) y v4 (script)"
  └── .claude/skills/angular-component/SKILL.md (MODIFICADO, ahora más corto)
  └── .claude/skills/angular-component/assets/component.template.ts (NUEVO)
  └── .claude/skills/angular-component/assets/component.template.html (NUEVO)
  └── .claude/skills/angular-component/assets/component.template.spec.ts (NUEVO)
  └── .claude/skills/angular-component/scripts/generate.py (NUEVO)
  └── frontend/src/app/components/order-filter/ (NUEVO componente como prueba v3 — VOLÁTIL)
  └── frontend/src/app/components/order-filter/order-filter.component.ts
  └── frontend/src/app/components/order-filter/order-filter.component.html
  └── frontend/src/app/components/order-filter/order-filter.component.scss
  └── frontend/src/app/components/order-filter/order-filter.component.spec.ts

Al cerrar el screencast: el formador descarta el commit real (incluidos
los componentes generados de prueba). La siguiente clase parte de
demo/2.2b-after (pre-cocinada en §6b) que tiene solo el skill v4
estructural, sin componentes de prueba.
```

**Estado final del árbol después del screencast (no del prompt):**

```
ordermanagement/
├── .claude/
│   ├── settings.json
│   └── skills/
│       └── angular-component/
│           ├── SKILL.md                            ← MODIFICADO (más corto)
│           ├── assets/                             ← NUEVO (carpeta)
│           │   ├── component.template.ts
│           │   ├── component.template.html
│           │   └── component.template.spec.ts
│           └── scripts/                            ← NUEVO (carpeta)
│               └── generate.py
├── docs/
│   └── DEMOS.md                                    ← MODIFICADO (pre-grabación)
├── frontend/
│   └── src/app/
│       ├── components/
│       │   ├── order-summary/                      (de 2.2a, sin cambios)
│       │   └── order-filter/                       ← NUEVO (en vivo, prueba v3)
│       │       ├── order-filter.component.ts
│       │       ├── order-filter.component.html
│       │       ├── order-filter.component.scss
│       │       └── order-filter.component.spec.ts
│       └── orders/
└── ... (resto sin cambios)
```

---

## 8. Guion del screencast

**Duración estimada del screencast: ~24-28 minutos.**

Diez bloques. Es la demo más densa técnicamente del módulo 2 — incluye plantillas con placeholders, sintaxis de comandos en `SKILL.md`, y un script Python con argumentos.

> **Antes de empezar a grabar**, asegúrate de:
> - Tener PowerShell 7 abierto en pantalla completa, fuente al menos 16pt.
> - Tener VS Code abierto con el repo en `demo/2.2b-before`.
> - **Verificar Python**: `python --version` debe responder con 3.11+ idealmente. Si no, instalar antes.
> - Verificar que el skill v2 existe: `Get-Content .claude\skills\angular-component\SKILL.md | Select-Object -First 5` debe mostrar el frontmatter.
> - Tener el componente `OrderSummary` accesible en VS Code para mostrarlo si hace falta como referencia.
> - Cerrar Slack, Teams, navegadores con notificaciones.

---

### Bloque 1 — Setup y verificación de entorno (~1 min 30 seg)

**Pantalla compartida.** A la izquierda VS Code con el repo en `demo/2.2b-before`. A la derecha terminal PowerShell.

**En la terminal:**

```powershell
git status
git log --oneline -3
python --version
```

```
On branch demo/2.2b
nothing to commit, working tree clean

abc1234 (HEAD -> demo/2.2b-before, demo/2.2a-after) demo/2.2a-after: skill angular-component v2 con convenciones del equipo
xyz9876 (demo/2.2a) demo/2.2a: skill angular-component v2 funcional + componente generado de prueba
def5678 (demo/2.1b) demo/2.1b: hallazgos del experimento de las 4 versiones

Python 3.12.1
```

**Lo que dices:**

> "Estamos en la rama `demo/2.2b-before`. Recap rápido: partimos de la `demo/2.2a-after` que tiene el skill `angular-component` v2 con ciento cincuenta líneas de convenciones del equipo. (Los componentes generados como prueba en la 2.2a fueron volátiles y no entraron al repo — solo tenemos aquí el skill.)
>
> Y hoy subimos el skill a sus dos siguientes capas. La gamma 2.2b lo cubrió:
>
> **Versión 3** — plantillas en `assets/`. Sustituimos la prosa larga del SKILL.md por plantillas con placeholders. El cuerpo del skill se reduce. Las plantillas se versionan como código.
>
> **Versión 4** — script ejecutable en `scripts/`. Para tareas deterministas: conversión de `OrdersList` a `orders-list`, comprobar si la carpeta existe, generar JSON con la información necesaria. Lo determinista lo hace el script. Lo de criterio sigue siendo del modelo.
>
> Antes de empezar, verifico Python. Si en mi máquina no estuviera, este bloque se cae. **Versión 3.12 instalado.** Bien.
>
> Vamos por orden. Empezamos por algo que la gamma 2.2b cubrió en sus primeros slides — **las dos sintaxis para ejecutar comandos dentro del SKILL.md**."

**Tiempo:** ~90 segundos.

---

### Bloque 2 — Las dos sintaxis para ejecutar comandos en `SKILL.md` (~3 min)

> "Antes de meter plantillas, vamos a hablar de algo que la gamma slides 4-9 cubrió y que conviene tener en la cabeza para entender la v3 y la v4: **se pueden ejecutar comandos directamente desde dentro del SKILL.md**. Hay dos sintaxis."

**En VS Code, abro un editor de texto al lado para mostrar las sintaxis (no toco el SKILL.md todavía):**

```markdown
SINTAXIS INLINE — comandos cortos cuya salida se incrusta en frase

Versión actual: !`Get-Content package.json | Select-String "version"`
Branch: !`git branch --show-current`
Último commit: !`git log -1 --format=%s`
```

> "**Sintaxis inline.** Backtick más exclamación más comando entre backticks. Se ejecuta cuando el skill se carga y la salida se incrusta en el contexto. Útil para inyectar contexto dinámico — *'estás trabajando en branch X, último commit Y'*. La gamma slide 5 lo introdujo."

```markdown
SINTAXIS EN BLOQUE — para acciones más largas

```!
Get-ChildItem src/app/components/ -ErrorAction SilentlyContinue
node --version
ng version
```
```

> "**Sintaxis en bloque.** Bloque triple-backtick con exclamación al inicio. Sirve para ejecutar varios comandos antes de que el agente proceda. La gamma slide 6 lo introdujo.
>
> **Casos típicos** que la gamma 2.2b slides 7-9 marcó:
>
> Verificación de prerequisitos. Comprobar que Angular CLI está instalado.
>
> Inyección de contexto del proyecto. Listar componentes existentes antes de generar uno nuevo.
>
> Setup automático. Crear carpetas si no existen.
>
> **Para la v3 que vamos a hacer ahora, no necesitamos comandos.** Pero esto está aquí para que sepáis que existen. **Os lo encontraréis en skills oficiales** — si abrís el `SKILL.md` de algún skill avanzado, las verán.
>
> Vamos a la v3."

**Tiempo:** ~3 minutos.

---

### Bloque 3 — Identificar la primera señal de parada en la v2 (~2 min)

> "La gamma 2.2a cerró con dos señales para parar de añadir prosa. **Vamos a aplicar la primera ahora**."

**En VS Code, abro `.claude/skills/angular-component/SKILL.md` (la v2 actual):**

> "Mirad la v2. Cuando subimos de v1 a v2, metimos esto en el SKILL.md."

**Hago scroll hasta la sección 'Convenciones del fichero .ts → Decorador':**

```markdown
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

Siempre `standalone: true`. Selector con prefijo `app-` y nombre en
kebab-case. Template y estilos SIEMPRE en ficheros separados — nunca
inline.
```

> "**Esto es la señal 2.** La gamma 2.2a slide 25 lo dijo: *'cuando estás escribiendo el quinto bloque de código de ejemplo en prosa, eso ya no es prosa — son plantillas'*. Y mirad lo que tenemos: un bloque de código en TypeScript que es **literalmente** una plantilla con `<kebab-case>` y `<nombre>` como marcadores.
>
> Si el equipo cambiara mañana la estructura del decorador — pongamos que Angular 20 introduce algo nuevo o `standalone: true` se vuelve por defecto y se quita explícito — **tendría que reescribir esta prosa a mano**. La plantilla está aquí codificada como texto, sin estructura para mantenerla.
>
> **Eso lo arregla la versión 3.** Sacamos el bloque a `assets/component.template.ts`. La plantilla es código real. Si Angular cambia, tocamos un fichero, no la prosa del skill."

**Tiempo:** ~2 minutos.

---

### Bloque 4 — Crear las plantillas en `assets/` (~5 min)

> "Vamos a crear las tres plantillas. Una para el `.ts`, otra para el `.html`, otra para el `.spec.ts`. El `.scss` lo dejamos al modelo — los estilos cambian mucho de componente a componente y no tiene sentido plantillarlos."

**En PowerShell:**

```powershell
mkdir .claude\skills\angular-component\assets
```

**En VS Code, creo `.claude/skills/angular-component/assets/component.template.ts`:**

```typescript
import { Component, inject, signal, computed, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
// {{IMPORTS_EXTERNAL}}
// {{IMPORTS_INTERNAL}}
// {{IMPORTS_CHILDREN}}

@Component({
  selector: 'app-{{KEBAB_NAME}}',
  standalone: true,
  imports: [CommonModule, /* {{IMPORTS_LIST}} */],
  templateUrl: './{{KEBAB_NAME}}.component.html',
  styleUrl: './{{KEBAB_NAME}}.component.scss'
})
export class {{PASCAL_NAME}}Component implements OnInit, OnDestroy {
  // 1. Inputs
  // {{INPUTS}}

  // 2. Outputs
  // {{OUTPUTS}}

  // 3. Inyecciones
  // {{INJECTIONS}}

  // 4. Estado local
  // {{STATE}}

  // 5. Computed
  // {{COMPUTED}}

  // 6. Lifecycle hooks
  ngOnInit(): void {
    // {{ON_INIT}}
  }

  ngOnDestroy(): void {
    // {{ON_DESTROY}}
  }

  // 7. Métodos públicos
  // {{PUBLIC_METHODS}}

  // 8. Métodos privados
  // {{PRIVATE_METHODS}}
}
```

> "Mirad esta plantilla. **Los placeholders entre `{{...}}`** son el contrato. La gamma 2.2b slide 14-15 los enumeró. Tenemos:
>
> **`{{KEBAB_NAME}}`** — el nombre en kebab-case, para selector y filenames.
>
> **`{{PASCAL_NAME}}`** — el nombre en PascalCase, para la clase.
>
> **`{{IMPORTS_*}}`** — tres slots para imports según las convenciones del equipo.
>
> **Los ocho bloques numerados** — la estructura estricta del .ts. **Ya no en prosa, en plantilla**. Si el equipo cambia mañana el orden, modificamos esta plantilla.
>
> Y mirad un detalle importante: `import { Component, inject, signal, computed, OnInit, OnDestroy } from '@angular/core';` ya está aquí incluido. **Aunque el componente concreto no use signal o computed, el import está**. Hay dos opciones aquí: que la plantilla incluya todo y el componente final tenga imports muertos, o que el modelo elimine los no usados. **La gamma slide 14 dice: dejarlos**. El linter de Angular se quejará y forzará a quitarlos. **Mejor que falten que sobren.**
>
> Vamos con la plantilla del `.html`:"

**Creo `.claude/skills/angular-component/assets/component.template.html`:**

```html
<div class="{{KEBAB_NAME}}">
  <!-- {{TEMPLATE_CONTENT}} -->
</div>
```

> "Mucho más corta. Solo el wrapper con la clase CSS y un placeholder para el contenido. **El template de cada componente es muy distinto** — la plantilla solo fija lo que es estructural (clase CSS con el nombre del componente).
>
> Y la plantilla del `.spec.ts`:"

**Creo `.claude/skills/angular-component/assets/component.template.spec.ts`:**

```typescript
import { ComponentFixture, TestBed } from '@angular/core/testing';

import { {{PASCAL_NAME}}Component } from './{{KEBAB_NAME}}.component';

describe('{{PASCAL_NAME}}Component', () => {
  let component: {{PASCAL_NAME}}Component;
  let fixture: ComponentFixture<{{PASCAL_NAME}}Component>;

  beforeEach(async () => {
    // Arrange
    await TestBed.configureTestingModule({
      imports: [{{PASCAL_NAME}}Component]
    }).compileComponents();

    fixture = TestBed.createComponent({{PASCAL_NAME}}Component);
    component = fixture.componentInstance;
    // {{SET_INPUTS}}
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  // {{ADDITIONAL_TESTS}}
});
```

> "Plantilla de test con la estructura **Arrange-Act-Assert** ya marcada. Los placeholders dejan dos huecos importantes: `{{SET_INPUTS}}` para que el componente reciba sus inputs antes del `detectChanges`, y `{{ADDITIONAL_TESTS}}` para los tests específicos del componente concreto.
>
> **Tres plantillas creadas.** Ahora actualizo el SKILL.md."

**Tiempo:** ~5 minutos.

---

### Bloque 5 — Actualizar el `SKILL.md` a v3 (~3 min)

**En VS Code, abro el `SKILL.md` y reemplazo las secciones de prosa con bloques de código por una nueva sección 'Plantillas':**

> "Lo que voy a hacer es **eliminar las dos secciones de la v2 que tenían bloques de código de ejemplo** y sustituirlas por una sección que apunta a `assets/`. La gamma 2.2b slide 16 lo enseñó."

**En el `SKILL.md`, sustituyo las secciones 'Convenciones del fichero .ts → Decorador', 'Convenciones del fichero .ts → Estructura de la clase', y 'Convenciones del fichero spec.ts' por:**

```markdown
## Plantillas

Para generar el componente, usa las plantillas disponibles en `assets/`:

- `assets/component.template.ts` para el fichero TypeScript
- `assets/component.template.html` para el template HTML
- `assets/component.template.spec.ts` para los tests

Cada plantilla tiene placeholders entre `{{...}}` que debes rellenar
según la información del componente:

- `{{KEBAB_NAME}}` — nombre del componente en kebab-case (ej: `order-summary`)
- `{{PASCAL_NAME}}` — nombre del componente en PascalCase (ej: `OrderSummary`)
- `{{IMPORTS_EXTERNAL}}`, `{{IMPORTS_INTERNAL}}`, `{{IMPORTS_CHILDREN}}` —
  imports según las dependencias detectadas
- `{{IMPORTS_LIST}}` — lista de imports en el array del decorador
- `{{INPUTS}}`, `{{OUTPUTS}}` — declaraciones con `input.required()` y `output()`
- `{{INJECTIONS}}` — inyecciones con `inject()`
- `{{STATE}}`, `{{COMPUTED}}` — signals locales y derivados
- `{{ON_INIT}}`, `{{ON_DESTROY}}` — cuerpo de los lifecycle hooks
- `{{PUBLIC_METHODS}}`, `{{PRIVATE_METHODS}}` — métodos del componente
- `{{TEMPLATE_CONTENT}}` — contenido del HTML
- `{{SET_INPUTS}}` — set de inputs en el beforeEach del test
- `{{ADDITIONAL_TESTS}}` — tests adicionales del componente

Lee la plantilla con `Read`, sustituye los placeholders, y escribe el
resultado con `Write`. Si un placeholder no aplica al componente
concreto, reemplázalo por una línea vacía.
```

**Salvo. Hago scroll mostrando que el SKILL.md es ahora más corto:**

> "Mirad. Las secciones del .ts y del spec.ts que tenían bloques de código se han ido. **El SKILL.md ahora tiene unas 100 líneas** en lugar de 150. Más ligero al cargarse. Y los detalles de estructura **viven en las plantillas reales** que se cargan solo cuando hace falta.
>
> **Esa es la ventaja 1 de la gamma 2.2b slide 17:** el SKILL.md se mantiene corto. Las plantillas pesan tokens solo cuando Claude las lee. Y solo lee las que necesita.
>
> **Ventaja 2 (slide 18):** las plantillas se versionan como código real. Si el equipo cambia la convención, modificamos `component.template.ts`, no la prosa.
>
> **Ventaja 3 (slide 19):** otras skills pueden reutilizar las mismas plantillas. Si tuviéramos un skill `angular-page` que genera componentes de tipo página (con su routing), podría compartir la base.
>
> Vamos a probarlo."

**Tiempo:** ~3 minutos.

---

### Bloque 6 — Probar el skill v3: generar `OrderFilter` (~3 min)

> "Voy a generar un componente nuevo — `OrderFilter` — para confirmar que el skill v3 funciona usando las plantillas. **Componente distinto al OrderSummary** que ya tenemos, para que sea evidencia limpia."

**Arranco Claude Code:**

```powershell
claude
```

```
✓ 1 project skill loaded: angular-component
```

**Tecleo:**

```
> Crea un componente Angular llamado OrderFilter que reciba como
  inputs un array de pedidos y un texto de búsqueda. Debe filtrar los
  pedidos en computed por el texto y emitir un output cuando el
  usuario haga click en uno. Componente standalone con Signals.
```

**Aparece:**

```
● [Skill activated: angular-component]
● Read(.claude/skills/angular-component/SKILL.md)
● Read(.claude/skills/angular-component/assets/component.template.ts)
● Read(.claude/skills/angular-component/assets/component.template.html)
● Read(.claude/skills/angular-component/assets/component.template.spec.ts)
● Glob(frontend/src/app/components/*)
● Read(frontend/src/app/components/order-summary/order-summary.component.ts)
● Write(frontend/src/app/components/order-filter/order-filter.component.ts)
● Write(frontend/src/app/components/order-filter/order-filter.component.html)
● Write(frontend/src/app/components/order-filter/order-filter.component.scss)
● Write(frontend/src/app/components/order-filter/order-filter.component.spec.ts)
● Bash(cd frontend && npm run build)
```

> "Mirad las herramientas. **Lee primero el SKILL.md**, luego **las tres plantillas en assets/**. Después mira el componente OrderSummary que ya tenemos como referencia. Luego escribe los cuatro ficheros. Y al final lanza el build por su cuenta — paso 4 del SKILL.md.
>
> El build se ejecuta. Esperamos."

```
> ng build
✔ Browser application bundle generation complete.
```

**Abro el `order-filter.component.ts`:**

```typescript
import { Component, inject, signal, computed, OnInit, OnDestroy, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';

interface Order {
  id: number;
  total: number;
  items: { id: number; price: number }[];
  status: 'Pending' | 'Confirmed' | 'Shipped' | 'Cancelled';
}

@Component({
  selector: 'app-order-filter',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-filter.component.html',
  styleUrl: './order-filter.component.scss'
})
export class OrderFilterComponent implements OnInit, OnDestroy {
  // 1. Inputs
  orders = input.required<Order[]>();
  searchText = input.required<string>();

  // 2. Outputs
  orderClicked = output<Order>();

  // 3. Inyecciones

  // 4. Estado local

  // 5. Computed
  filteredOrders = computed(() => {
    const search = this.searchText().toLowerCase();
    return this.orders().filter(o => o.status.toLowerCase().includes(search));
  });

  // 6. Lifecycle hooks
  ngOnInit(): void {
  }

  ngOnDestroy(): void {
  }

  // 7. Métodos públicos
  onOrderClick(order: Order): void {
    this.orderClicked.emit(order);
  }

  // 8. Métodos privados
}
```

> "**Mirad la estructura.** Los ocho bloques numerados con sus comentarios. **Plantilla aplicada al pie de la letra.** Donde no había nada — bloque 3 inyecciones, bloque 4 estado local, bloque 8 métodos privados — el modelo dejó el comentario y el espacio vacío. La estructura se mantiene aunque algunos bloques no se usen. **La consistencia entre componentes la garantiza la plantilla.**
>
> Y mirad el bloque 5: `filteredOrders = computed(() => ...)`. Ahí sí hay lógica concreta del componente, el modelo la rellenó. **La plantilla pone el esqueleto. El modelo pone la carne.** Esa es la división de trabajo.
>
> Versión 3 funcional. Vamos a la versión 4."

**Salgo (Ctrl+C).**

**Tiempo:** ~3 minutos.

---

### Bloque 7 — La regla de los scripts: cuándo sí, cuándo no (~2 min)

> "Antes de meter el script, **vamos a parar a entender cuándo merece la pena**. La gamma 2.2b slide 21 lo cubrió y conviene tenerlo claro."

**En el editor de texto al lado, escribo (texto explicativo):**

```
REGLA DE ORO:

Si la tarea tiene una respuesta correcta determinista
   → script. Más fiable, más rápido, más barato.

Si la tarea requiere criterio o adaptación al contexto
   → prosa para que el modelo razone.

NO MEZCLES.

Tareas que SÍ son scripts:
✓ Conversión de formato — OrdersList → orders-list
✓ Comprobación de duplicados — ¿ya existe esta carpeta?
✓ Generación de GUID — un GUID v4
✓ Validación de schema — JSON cumple X o no
✓ Cálculos sobre el filesystem — contar componentes existentes

Tareas que NO son scripts:
✗ Decidir qué imports necesita el componente
✗ Decidir el contenido del template HTML
✗ Decidir cuántos tests escribir
✗ Decidir cómo nombrar las variables internas
```

> "**Lo que hace el script en nuestro caso**: dada una entrada *'OrdersList'*, devolver un JSON con: kebab-case (`orders-list`), pascal-case (`OrdersList`), selector (`app-orders-list`), ruta esperada (`frontend/src/app/components/orders-list`), y si la carpeta ya existe (boolean).
>
> Esto es **determinista**. Una entrada → una salida correcta. **No requiere criterio.** Y lo más importante: **el modelo, sin script, lo hace bien la mayoría de las veces, pero a veces se equivoca**. *'OrdersList'* podría convertirse en `orders-list` o `Orders-list` o `orders_list` según humores. **El script lo hace siempre igual.**
>
> Vamos a escribirlo."

**Tiempo:** ~2 minutos.

---

### Bloque 8 — Crear el script `generate.py` (~3 min)

**En PowerShell:**

```powershell
mkdir .claude\skills\angular-component\scripts
```

**En VS Code, creo `.claude/skills/angular-component/scripts/generate.py`:**

```python
#!/usr/bin/env python3
"""
Genera información del componente a partir de un nombre.
Devuelve un JSON con los nombres en distintos formatos y la ubicación
esperada, además de comprobar si la carpeta ya existe.

Uso: python generate.py <NombreComponente>
Ej:  python generate.py OrdersList
"""
import json
import os
import re
import sys


def to_kebab(name: str) -> str:
    """Convierte PascalCase, camelCase o palabras separadas en kebab-case."""
    name = re.sub(r'[_ ]', '-', name)
    name = re.sub(r'(?<!^)(?<!-)(?=[A-Z])', '-', name)
    return name.lower()


def to_pascal(name: str) -> str:
    """Convierte cualquier formato a PascalCase."""
    parts = re.split(r'[-_ ]', name)
    return ''.join(word.capitalize() for word in parts if word)


def main():
    if len(sys.argv) < 2:
        print(json.dumps({"error": "Falta el nombre del componente"}))
        sys.exit(1)

    name_input = sys.argv[1]

    kebab = to_kebab(name_input)
    pascal = to_pascal(name_input)
    selector = f"app-{kebab}"

    target_dir = f"frontend/src/app/components/{kebab}"
    exists = os.path.isdir(target_dir)

    result = {
        "kebab": kebab,
        "pascal": pascal,
        "selector": selector,
        "target_dir": target_dir,
        "already_exists": exists,
        "files": [
            f"{target_dir}/{kebab}.component.ts",
            f"{target_dir}/{kebab}.component.html",
            f"{target_dir}/{kebab}.component.scss",
            f"{target_dir}/{kebab}.component.spec.ts",
        ]
    }

    print(json.dumps(result, indent=2))


if __name__ == "__main__":
    main()
```

**Salvo. Lo pruebo en aislado en la terminal:**

```powershell
python .claude\skills\angular-component\scripts\generate.py OrdersList
```

```json
{
  "kebab": "orders-list",
  "pascal": "OrdersList",
  "selector": "app-orders-list",
  "target_dir": "frontend/src/app/components/orders-list",
  "already_exists": false,
  "files": [
    "frontend/src/app/components/orders-list/orders-list.component.ts",
    "frontend/src/app/components/orders-list/orders-list.component.html",
    "frontend/src/app/components/orders-list/orders-list.component.scss",
    "frontend/src/app/components/orders-list/orders-list.component.spec.ts"
  ]
}
```

> "Funciona. JSON limpio. **Lo importante**: este script es **independiente del modelo**. Lo puedo ejecutar yo desde la terminal, lo puede ejecutar el skill, lo puede ejecutar un pipeline. **Determinista.**
>
> Y mirad qué hace en el caso del componente que ya existe:"

**Pruebo con un nombre que sí existe (`OrderSummary`):**

```powershell
python .claude\skills\angular-component\scripts\generate.py OrderSummary
```

```json
{
  "kebab": "order-summary",
  "pascal": "OrderSummary",
  "selector": "app-order-summary",
  "target_dir": "frontend/src/app/components/order-summary",
  "already_exists": true,
  "files": [
    "frontend/src/app/components/order-summary/order-summary.component.ts",
    "frontend/src/app/components/order-summary/order-summary.component.html",
    "frontend/src/app/components/order-summary/order-summary.component.scss",
    "frontend/src/app/components/order-summary/order-summary.component.spec.ts"
  ]
}
```

> "**`already_exists: true`**. El script detecta la colisión. **Esto es lo que el modelo no puede saber sin leer el sistema de ficheros, y lo que el script comprueba en cinco milisegundos.** Vamos a integrarlo en el SKILL.md."

**Tiempo:** ~3 minutos.

---

### Bloque 9 — Actualizar `SKILL.md` a v4 e integrar el script (~3 min)

**En VS Code, edito el SKILL.md y añado al inicio (después del frontmatter, antes de 'Cuándo se usa'):**

```markdown
## Setup inicial

Antes de generar el componente, ejecutar el script de generación de
metadatos. Pasa al script el nombre que el usuario haya pedido
(o el que has pedido al usuario si no lo dio):

```!
python .claude/skills/angular-component/scripts/generate.py <NOMBRE>
```

El script devuelve un JSON con:

- `kebab` — nombre en kebab-case
- `pascal` — nombre en PascalCase
- `selector` — selector con prefijo `app-`
- `target_dir` — ruta donde generar
- `already_exists` — boolean de colisión
- `files` — lista de los cuatro ficheros a generar

**Si `already_exists` es `true`, NO generes nada. Avisa al usuario y
pregúntale si quiere sobrescribir, usar otro nombre, o cancelar.**

Usa los valores del JSON para rellenar los placeholders de las plantillas
(en particular `{{KEBAB_NAME}}` y `{{PASCAL_NAME}}`).
```

**Salvo el fichero.**

> "Mirad lo que añadí. Una sección nueva al inicio del SKILL.md: **'Setup inicial'**. Con un bloque ` ```! ` que ejecuta el script. **El skill ahora hace tres cosas en orden**:
>
> Una. Lee el SKILL.md.
>
> Dos. Ejecuta el script con el nombre del componente. Recibe el JSON.
>
> Tres. Si la carpeta no existe, lee las plantillas y genera. Si existe, avisa al usuario.
>
> **El paso 3 — la decisión de qué hacer si existe — sigue siendo del modelo.** El script solo informa. Es exactamente la división que la gamma 2.2b slide 10 marcó: **lo determinista lo hace el script, lo de criterio lo hace el modelo.**
>
> Vamos a probarlo. Le pido un componente con un nombre que **ya existe**:"

**Arranco Claude Code:**

```powershell
claude
```

**Tecleo:**

```
> Crea un componente OrderSummary para mostrar el resumen del pedido.
```

**Aparece:**

```
● [Skill activated: angular-component]
● Read(.claude/skills/angular-component/SKILL.md)
● Bash(python .claude/skills/angular-component/scripts/generate.py OrderSummary)

{
  "kebab": "order-summary",
  "pascal": "OrderSummary",
  "selector": "app-order-summary",
  "target_dir": "frontend/src/app/components/order-summary",
  "already_exists": true,
  ...
}

He ejecutado el script de generación y detecto que el componente
OrderSummary ya existe en frontend/src/app/components/order-summary/
(de la demo 2.2a).

¿Quieres que:
(a) sobrescriba el componente existente
(b) lo cree con otro nombre (ej: OrderSummaryV2)
(c) cancele la operación

Por defecto NO voy a sobrescribir hasta que me confirmes.
```

> "**Mirad esto.** El script detectó la colisión, devolvió `already_exists: true`, y el modelo **paró**. Me da tres opciones. **No ha generado nada.** Eso es la división correcta: el script informa, el modelo decide qué preguntar.
>
> Le digo que cancele:"

```
> Cancela. No quiero sobrescribir.
```

```
Operación cancelada. No se generó ningún componente.
```

> "Perfecto. **Esto en la v3 sin script no habría pasado.** El modelo habría sobrescrito sin avisar, o habría intentado adivinar. El script garantiza la verificación."

**Salgo (Ctrl+C).**

**Tiempo:** ~3 minutos.

---

### Bloque 10 — Recap, regla de oro, y cliffhanger (~2 min)

> "Y eso es la 2.2b. Recap rápido."

**En el editor de texto:**

```
LO QUE HEMOS HECHO:

VERSIÓN 3 — Plantillas en assets/
─────────────────────────────────
- Tres plantillas: component.template.ts, .html, .spec.ts
- Placeholders {{KEBAB_NAME}}, {{PASCAL_NAME}}, etc.
- SKILL.md más corto (eliminamos prosa con bloques de código)
- Plantillas se versionan como código

VERSIÓN 4 — Script ejecutable en scripts/
──────────────────────────────────────────
- generate.py convierte nombres y comprueba colisiones
- Ejecutado al activarse el skill (sintaxis ```! del SKILL.md)
- Garantiza determinismo donde el modelo improvisaría
- Lo de criterio sigue siendo del modelo


REGLA DE ORO (gamma slide 29):

NO subas de versión por curiosidad.
Sube cuando aparezca una señal concreta:

  Señal 1: el SKILL.md pasa de 2.000 palabras
           → toca v3 (extraer plantillas).

  Señal 2: hay tareas deterministas que el modelo
           a veces hace mal o tarda en hacer
           → toca v4 (script).

Si nunca aparecen, te quedas en v2. Y bien.
```

> "**La regla de oro** es la frase que la gamma 2.2b slide 29 puso al final: *'cuándo NO subir de versión'*. **Subir antes de tiempo es complicar el skill sin razón.**
>
> Para el caso de `angular-component`, **subir tenía sentido** — la v2 ya estaba al borde con bloques de código que iban a crecer (señal 1), y había una tarea determinista clara (la conversión de nombres y comprobación de duplicados, señal 2). **Pero para muchos otros skills, la v2 es suficiente.** No subáis si no es necesario.
>
> En la siguiente demo, la **2.2c**, vamos a ver dos cosas que cierran el módulo de creación de skills. Primero, **scopes user vs proyecto** — qué skills van al `~/.claude/skills/` personal y cuáles al `.claude/skills/` del repo. Y segundo, **descripciones decentes de skills** — la pieza que decide si tu equipo los usa o los ignora cuando se los des. Empezamos con el dos punto dos punto C."

**Tiempo:** ~2 minutos.

---

## 9. Qué resaltar verbalmente

Cinco puntos que **no pueden quedarse sin decir**:

1. **"La señal 2 ya está aquí: bloques de código en prosa = plantillas."** — la justificación del salto v2 → v3. Bloque 3.

2. **"El SKILL.md se reduce. Las plantillas pesan tokens solo cuando Claude las lee."** — la rentabilidad del progressive disclosure aplicado. Bloque 5 cuando se muestra el SKILL.md más corto.

3. **"Lo determinista lo hace el script. Lo de criterio lo hace el modelo. NO mezcles."** — la regla más importante de la gamma 2.2b. Bloque 7, repetida en bloque 9.

4. **"El script detecta la colisión. El modelo decide qué preguntar."** — la división de trabajo materializada. Bloque 9 cuando aparece `already_exists: true`.

5. **"No subas de versión por curiosidad. Sube cuando aparezca una señal concreta."** — la regla de oro de cierre. Bloque 10.

**Frase de remate al final:**

> *"Las plantillas se versionan como código. El script garantiza determinismo. El modelo razona sobre el contexto. Cada uno hace lo que mejor hace. Esa es la versión 4."*

---

## 10. Slide de entrada (locución HeyGen)

> **Texto del avatar antes del screencast:**

"Y vamos a la siguiente demo. La 2.2b. Hoy subimos el skill `angular-component` que construimos en la 2.2a a sus dos siguientes capas. La versión 3 saca las plantillas a `assets/` cuando los bloques de código en el SKILL.md se vuelven literalmente plantillas. Es la primera de las dos señales de parada que vimos. La versión 4 añade un script ejecutable en `scripts/` para tareas deterministas — conversión de `OrdersList` a `orders-list`, comprobación de si la carpeta ya existe, generación del JSON con la información necesaria. Y al final el skill probado en directo: pedimos un componente con un nombre que ya existe, el script detecta la colisión, el modelo para y pregunta. La división de trabajo correcta — lo determinista lo hace el script, lo de criterio lo hace el modelo. Vamos al screencast."

---

## 11. Slide de salida (locución HeyGen)

> **Texto del avatar al volver del screencast:**

"Lo que acabáis de ver es un skill subido a nivel producción. Tres carpetas: el SKILL.md como cerebro, `assets/` con las plantillas que se versionan como código real, y `scripts/` con la lógica determinista. Y la regla de oro: no subáis de versión por curiosidad. Subir solo cuando aparece una de las dos señales — pasar de dos mil palabras o tareas deterministas que el modelo a veces hace mal. Para el caso de `angular-component` tenía sentido. Para muchos otros skills, la versión dos es suficiente. En la siguiente demo, la 2.2c, cerramos el módulo de creación. Vamos a ver dos cosas. Primero, scopes user vs proyecto — qué skills viajan contigo de proyecto en proyecto y cuáles van a git con el equipo. Y segundo, descripciones decentes de skills — la pieza que decide si tu equipo los usa o los ignora cuando se los das. Empezamos con el dos punto dos punto C."

---

## 12. Tiempo total estimado

| Bloque | Tiempo |
|---|---|
| Slide de entrada (avatar) | ~30 seg |
| Bloque 1 — Setup y verificación de entorno | ~1 min 30 seg |
| Bloque 2 — Las dos sintaxis para ejecutar comandos | ~3 min |
| Bloque 3 — Identificar primera señal de parada | ~2 min |
| Bloque 4 — Crear las plantillas en `assets/` | ~5 min |
| Bloque 5 — Actualizar SKILL.md a v3 | ~3 min |
| Bloque 6 — Probar v3 generando OrderFilter | ~3 min |
| Bloque 7 — Regla de los scripts | ~2 min |
| Bloque 8 — Crear el script generate.py | ~3 min |
| Bloque 9 — SKILL.md a v4 e integrar script | ~3 min |
| Bloque 10 — Recap, regla de oro y cliffhanger | ~2 min |
| Slide de salida (avatar) | ~30 seg |
| **Total screencast** | **~27-29 min** |
| **Total con avatar** | **~28-30 min** |

> Si hay preguntas durante el screencast, súmale 3-5 minutos. La demo encaja en un bloque de **35 minutos** dentro de la sesión.

**Margen de seguridad por si algo va lento:**

- **Si Python NO está instalado** y el bloque 1 lo detecta, ofrece dos opciones: instalar `winget install Python.Python.3.12` y reiniciar PowerShell, o **rehacer el script en Bash** que viene con Git for Windows. Si optas por Bash, el script `generate.sh` en lugar de `generate.py` con sed para conversiones — pero pierde portabilidad. **Mejor instalar Python en preparación**.

- **Si el skill v3 no respeta las plantillas** (porque el modelo decide improvisar), comenta: *"a veces el modelo decide ignorar la plantilla. Vamos a ser más explícitos en el SKILL.md"* y añade una frase fuerte como *"DEBES leer las plantillas con Read antes de generar. NO improvises la estructura"*. Repites la prueba.

- **Si el script da error de sintaxis o ejecución**, ten una versión funcional del script en una nota aparte para pegar. **No improvises Python en directo si no es tu lenguaje fuerte**. Mejor pegar una versión validada y comentar paso a paso.

- **Si el modelo NO ejecuta el script con `Bash` después de leer SKILL.md**, comenta: *"a veces el modelo decide saltarse el script. Hay que ser más explícito en la instrucción"*. Y refuerza el SKILL.md poniendo *"OBLIGATORIO: ejecutar el script ANTES de leer las plantillas"*. Repites.

- **Si el bloque 4 (crear plantillas) se hace pesado por el volumen**, puedes pegarlas desde una nota aparte en lugar de teclear. La pedagogía es **el patrón de placeholders**, no que el formador escriba TypeScript en directo.

- **Si te quedas sin tiempo**, recorta el bloque 6 (prueba de v3) a 1 minuto: solo enseñas el SKILL.md actualizado y dices *"el v3 funciona, lo verificaríamos pidiendo otro componente — saltamos directo a la v4 que es donde está la diferencia conceptual"*. La 2.2b se enfoca más en v4 (script).

---

# Apéndice A — Decisiones pedagógicas justificadas

> Esta sección no se graba. Es para Pedro, formador.

**¿Por qué empezar con las dos sintaxis (inline y bloque) si no las usamos hasta el bloque 9?**

Porque la gamma 2.2b dedicó cuatro slides (4-9) a las dos sintaxis y son **un concepto fundacional** para entender el SKILL.md a partir de ahora. Si las introduces en el bloque 9 cuando ya las usas, parece un detalle técnico que viene de la nada. Introducirlas al inicio establece el marco conceptual y luego el bloque 9 las aplica con confianza.

**¿Por qué v3 = plantillas y v4 = script, en ese orden?**

Porque **es el orden del manual y la gamma 2.2b**. La gamma slides 12-19 cubren plantillas (v3) y luego slides 20-26 cubren scripts (v4). Cambiar el orden confundiría al alumno que viene de la teoría. **La progresión está justificada conceptualmente**: las plantillas resuelven la señal 1 de parada (prosa que se vuelve plantillas). Los scripts resuelven necesidades de determinismo, que aparecen incluso después.

**¿Por qué `OrderFilter` y no otro componente para probar v3?**

Porque tiene tres características útiles:
1. **Distinto del OrderSummary** — evidencia limpia de que el skill v3 funciona consistentemente.
2. **Tiene inputs Y outputs** — prueba más bloques de la plantilla (en OrderSummary solo había inputs).
3. **Tiene computed con lógica real** — el filtrado por texto demuestra que la plantilla deja espacio para la lógica del componente sin obstaculizar.

**¿Por qué el script comprueba `already_exists` en lugar de hacerlo el modelo?**

Porque la gamma 2.2b slide 21 lo dijo: *"Validaciones — verificar que un nombre no colisiona con un componente existente antes de generarlo"*. Esta es **la tarea estrella de un script en este skill**. El modelo podría comprobarlo, pero requeriría una tool call (`Glob` o `Read`) y procesamiento. El script lo hace en cinco milisegundos sin tokens. **Caso de oro para script**.

**¿Por qué en el bloque 9 pruebo con un nombre que YA existe (`OrderSummary`)?**

Porque la pieza pedagógica del v4 es **demostrar que el script aporta algo que el modelo solo no puede**. Si pruebo con un nombre que no existe, la prueba se ve igual que la v3 (genera el componente). **Pruebar con colisión** es donde el script brilla — el modelo se para, pregunta opciones, no sobrescribe. **Esa es la división de trabajo correcta** materializada.

**¿Por qué cancelar al final y no sobrescribir o crear nuevo?**

Porque **sobrescribir destruiría el componente que creamos en la 2.2a** y rompería el repo. **Crear nuevo `OrderSummaryV2`** dejaría un fichero raro en el repo. **Cancelar mantiene el repo limpio** y demuestra el flujo más relevante: el alumno aprende que el agente respeta la decisión del usuario.

**¿Por qué el SKILL.md no se reduce a su mínimo absoluto?**

Porque hay **convenciones del equipo que siguen siendo prosa** — orden de imports, control flow nuevo, lo que el skill NO debe hacer. Esto **no son plantillas** — son reglas de criterio que el modelo aplica al rellenar las plantillas. **Mantenerlas en prosa es correcto**. Solo extraemos a `assets/` lo que es literal plantilla de código.

**¿Por qué el script imprime JSON y no texto plano?**

Porque JSON es **el formato más fácil de parsear para el modelo** — es estructura, no prosa. Si el script imprimiera texto plano, el modelo tendría que parsear con razonamiento ("¿dónde está el kebab? ¿es la palabra después de 'kebab:'?"). Con JSON, **el modelo lo parsea como dato**. Robustez.

**¿Por qué el script Python y no PowerShell?**

Por dos razones:
1. **Python es más portable** — funciona en Windows, Mac, Linux idéntico. PowerShell tiene sintaxis distinta entre Windows PowerShell 5 y PowerShell 7.
2. **El manual lo usó en Python** — la gamma 2.2b slides 22-23 dan el ejemplo en Python. Mantener consistencia con el material del curso evita confusión.

Si tu audiencia es 100% .NET y **no tiene Python disponible**, una alternativa válida es PowerShell con un `.ps1`. Pero requiere advertencia previa al alumno.

**¿Por qué el bloque 7 (regla de los scripts) está justo antes del bloque 8 (crear el script)?**

Porque **enmarca el script como decisión técnica, no como capricho**. Si meto el script directamente sin la regla, el alumno puede pensar "vale, los skills llevan scripts". Con la regla **antes**, el alumno entiende **por qué este caso justifica el script** y va a saber decidir si su skill propio lo necesita o no.

**¿Por qué el cliffhanger menciona "scopes user vs proyecto" y "descripciones decentes" para 2.2c?**

Porque la gamma 2.2c (que cierra el bloque de creación) cubre exactamente esos dos temas. Anticipar los títulos de las dos partes principales de la 2.2c **ata el contenido entre demos**. El alumno llega a la 2.2c sabiendo qué viene, no sorprendido.
