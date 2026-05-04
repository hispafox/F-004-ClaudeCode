# Demo 2.2a — Primer skill propio: `angular-component` versiones 1 y 2 sobre OrderManagement

> **Versión:** v1 | **Módulo:** 2 | **Sub:** 2.2a | **Estado:** ✅ Versión final
> **Archivo:** `demo_M02-S2.2a-bases-primer-skill-windows-v1.md`
> **Branch before:** `demo/2.2a-before`  (estado al hacer `git checkout` antes de grabar — sin skill propio)
> **Branch after:**  `demo/2.2a-after`   (estado final pre-cocinado con `angular-component` v2 commiteado)
> **Branch parent:** `demo/2.1b`  (CONCEPTUAL — rama única de la demo predecesora)
> **Tiempo total estimado:** ~24-28 minutos
> **Tipo:** Demo de construcción incremental (CÓDIGO). **Es la primera demo del curso donde el alumno ve construir un skill propio "de verdad" desde cero**, con dos iteraciones (v1 mínima → v2 con convenciones del equipo) y prueba real de cada versión sobre OrderManagement. **Las versiones 3 y 4 (scripts y plantillas) son la demo 2.2b**. Sigue el patrón **before/after** definido en [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md).
> **Plataforma:** Windows 11 (PowerShell 7).

---

## 1. Contexto

Llegamos al momento productivo del módulo 2. Ya hemos visto la anatomía (2.1a). Ya hemos entendido por qué la descripción es el switch (2.1b). Ahora **construimos el primero**.

La gamma 2.2a (30 slides, ~30 min) sembró cuatro mensajes clave que esta demo materializa:
- **Construir uno, no leer sobre uno** — el aprendizaje real está en la construcción, no en la teoría.
- **Resuelve un caso primero, después escribe el skill** — el flujo que la gamma slide 5-9 marcó como hábito sano.
- **Empezar por lo más simple posible y subir el detalle solo cuando se justifique** — la progresión v1 → v2 → v3 → v4.
- **Hay dos señales claras para parar de añadir prosa**: pasar de 2.000 palabras o que las convenciones se vuelvan plantillas. Cuando una de las dos llega, toca cambiar de estrategia.

El skill que construimos es **`angular-component`** — un generador de componentes Angular standalone con Signals para OrderManagement. Lo elegimos por tres razones (gamma slide 3): es un caso común que cualquier equipo Angular agradece automatizar, permite enseñar la progresión natural de complejidad, y al final del módulo el alumno se lleva un skill que puede instalar literalmente en su repo.

> **Tipo de demo:** construcción guiada paso a paso. Empezamos por el `SKILL.md` mínimo (~30 líneas), lo probamos, identificamos limitaciones, y subimos a la v2 con convenciones reales del equipo (~150 líneas). **A partir de aquí la rama queda con un skill propio funcional permanente** — todas las demos siguientes lo tendrán disponible.

---

## 2. Objetivo de la demo

Cinco cosas concretas que tienen que quedar en la cabeza del alumno cuando termine los ~24 minutos de screencast:

1. **El hábito sano: resolver un caso primero, después escribir el skill.** La gamma slide 5-9 lo dijo. La demo lo ejecuta literalmente — antes de crear el skill, se le pide a Claude Code que genere un componente Angular y se observa qué hace. **Eso es la base sobre la que se construye el skill**, no la imaginación del formador.

2. **El skill v1 es funcional — un solo `SKILL.md` con frontmatter y cuerpo de ~30 líneas basta para empezar.** El alumno ve que **no hay que escribir un skill perfecto al primer intento**. Lo simple funciona y se mejora luego.

3. **La v2 codifica convenciones reales del equipo en bloques claros.** Imports en orden estricto, estructura de la clase con 8 bloques en orden, `inject()` en lugar de constructor, control flow nuevo. Cada bloque es **una decisión que un junior nuevo no haría sin que se la enseñes**.

4. **Las dos señales para parar de añadir prosa.** Pasar de 2.000 palabras o que las convenciones se vuelvan plantillas. **Cuando una llega, toca pasar a la v3 con scripts o assets** (la 2.2b).

5. **La rentabilidad del flujo "resuelve un caso → escribe el skill" se ve operativamente.** El componente generado por la v2 del skill cumple con las convenciones del equipo desde el primer intento. **Antes del skill**, el componente generado tenía 4-5 desviaciones del patrón del equipo. **Después del skill v2**, cero.

Y dos cosas que tienen que **NO quedar** en su cabeza:

- *"Tengo que clavar el skill perfecto a la primera."* — no, **se itera**. La v1 es deliberadamente mínima. La v2 sube el listón. Las v3-v4 (en 2.2b) añaden potencia.
- *"Si la v2 funciona, la v3 con scripts es opcional."* — sí y no. **Para muchos casos la v2 es suficiente**. Las v3-v4 se justifican solo cuando aparecen las dos señales de parada.

---

## 3. Branch `demo/2.2a-before`

Punto de partida del screencast.

```
demo/2.2a-before
```

**Parte de:** `demo/2.1b` (CONCEPTUAL, rama única).

**Estado del repo:** misma estructura que `demo/2.1a` excepto por el `docs/skills-explorados.md` ampliado con los hallazgos del experimento de las 4 versiones de descripción. **No hay ningún `.claude/skills/` en el repo todavía** — el experimental `find-handler` se borró al final de la 2.1b. La pieza viva del screencast es construir `angular-component` v1 → v2 desde cero.

> El formador hace `git checkout demo/2.2a-before` antes de empezar a grabar.

---

## 4. Branch `demo/2.2a-after`

Estado final que la siguiente clase (2.2b) asume.

```
demo/2.2a-after
```

**Parte de:** `demo/2.2a-before`.

**Qué añade respecto a `-before`:** una pieza permanente y de valor real al repo — el skill `angular-component` versión 2 instalado en `.claude/skills/angular-component/SKILL.md`, más la marca `[x]` en `docs/DEMOS.md`. **A partir de aquí, todas las demos siguientes del módulo 2 (2.2b, 2.2c, 2.3) y del resto del curso tendrán este skill disponible**. Es la primera contribución estructural permanente del módulo 2.

**Cómo se prepara:** ver §6b. Se materializa antes de grabar — Claude Code en una sesión limpia genera el skill v2 equivalente al que el formador construirá en directo (v1 → v2).

> Durante la grabación, el formador construye v1 mínima en pantalla, la prueba, identifica limitaciones, sube a v2 con convenciones, prueba v2. Al cerrar descarta los cambios reales y la siguiente clase parte de `demo/2.2a-after` ya pre-cocinada con la v2.

---

## 5. Estado del repo al hacer `git checkout demo/2.2a-before`

Idéntico a `demo/2.1b`:

```
ordermanagement/
├── .claude/
│   └── settings.json                       (sin cambios)
├── docs/
│   ├── DEMOS.md                            (1.1, 1.2a, 1.2b, 1.3a, 1.3b, 2.1a, 2.1b marcadas)
│   └── skills-explorados.md                (notas de 2.1a y 2.1b)
├── scripts/
├── src/                                    (sin cambios)
├── frontend/                               (Angular 19 vanilla)
├── tests/                                  (sin cambios)
├── .gitignore
├── CLAUDE.md
└── README.md
```

**Estado del frontend Angular (importante para esta demo):**

```
frontend/
├── src/
│   ├── app/
│   │   ├── orders/
│   │   │   ├── orders-list.component.ts        (componente standalone existente)
│   │   │   └── order-detail.component.ts
│   │   ├── app.routes.ts
│   │   └── app.config.ts
│   └── styles/
│       └── _tokens.scss
├── package.json                                (Angular 19)
└── angular.json
```

**Estado clave para la demo:**

- **NO hay `.claude/skills/`** todavía en el repo.
- **NO hay carpeta `frontend/src/app/components/`** — los componentes de OrderManagement viven en `orders/`. La demo va a aprovechar esto: el primer componente del experimento (orden-summary) lo crearemos en `frontend/src/app/components/order-summary/` que **no existía**, y el skill v2 reflejará esta convención del equipo.

**Estado de la máquina Windows del formador:**

```
✅ Claude Code v2.1.x instalado y autenticado
✅ Git for Windows
✅ PowerShell 7
✅ VS Code con el repo cargado en demo/2.2a
✅ CLAUDE.md y .claude/settings.json operativos
✅ Frontend Angular compilando: cd frontend && npm install && npm run build
```

**Lo que el alumno verá al final de la demo:**

- Un experimento previo: pedir a Claude Code generar un componente Angular **sin skill** y observar 4-5 desviaciones del patrón del equipo (no usa `inject()`, falta orden de imports, etc.).
- El **skill v1** mínimo creado en `.claude/skills/angular-component/SKILL.md` (~30 líneas, 5 minutos de escritura).
- Prueba de la v1: genera un componente que mejora el experimento previo pero aún tiene 2-3 desviaciones del equipo.
- Identificación de las limitaciones de la v1 (gamma slide 15).
- El skill subido a **v2** en el mismo `SKILL.md` con convenciones reales (~150 líneas).
- Prueba de la v2: genera un componente que cumple **todas** las convenciones del equipo desde el primer intento.
- Las dos señales de parada anunciadas: pasar de 2.000 palabras o las convenciones convertidas en plantillas. **Sembrado para la 2.2b**.

---

## 6a. Prompt para Claude Code — preparar `demo/2.2a-before`

> Crea la rama de partida del screencast desde `demo/2.1b` (CONCEPTUAL, rama única). **No crea skill alguno** — la pieza viva es construir `angular-component` v1 → v2 en pantalla. La rama `-before` queda idéntica a `demo/2.1b`.

````
Estoy preparando la demo 2.2a del curso de Claude Code (primer skill
propio: angular-component v1 → v2). Sigue el patrón before/after
(ver demo M0.2).

Quiero que prepares la rama `demo/2.2a-before` desde `demo/2.1b`
(CONCEPTUAL, rama única). Esta rama es el punto de partida del
screencast: el repo NO debe tener `.claude/skills/`. El skill es la
pieza viva.

## Tarea única

```powershell
git checkout demo/2.1b
git pull
git checkout -b demo/2.2a-before
```

NO crees `.claude/skills/`, NO toques el frontend, NO marques nada en
docs/DEMOS.md. Esos artefactos van en `demo/2.2a-after` (ver §6b).

NO hagas commit. La rama `demo/2.2a-before` es exactamente igual a
`demo/2.1b` excepto en el nombre.

# Cuando termines, dime

1. Que la rama demo/2.2a-before está creada.
2. Que `git diff demo/2.1b demo/2.2a-before` no muestra cambios.
````

---

## 6b. Prompt para Claude Code — preparar `demo/2.2a-after`

> Materializa la rama final con el skill `angular-component` versión 2 pre-cocinado — equivalente al que el formador construirá en directo (v1 → v2). Pre-cocinar `-after` garantiza que la siguiente clase parte de un estado conocido aunque el directo se desvíe.

````
Estoy preparando la demo 2.2a del curso de Claude Code. Esta rama
-after pre-cocina el skill angular-component v2 que el formador
construirá en vivo durante el screencast (v1 mínima → v2 con
convenciones).

# Contexto

Estoy en la rama `demo/2.2a-before` del repo `ordermanagement`. La rama
parte de `demo/2.1b` (CONCEPTUAL) y NO tiene aún `.claude/skills/`.

Quiero que prepares la rama `demo/2.2a-after` desde `demo/2.2a-before`
con el skill `angular-component` versión 2 instalado y la marca [x]
en docs/DEMOS.md.

# Lo que necesito

Tres tareas:

## Tarea 1: crear la rama

```powershell
git checkout demo/2.2a-before
git checkout -b demo/2.2a-after
```

## Tarea 2: crear `.claude/skills/angular-component/SKILL.md` v2

Crea el SKILL.md con frontmatter y cuerpo que codifique las convenciones
reales del equipo OrderManagement para componentes Angular standalone con
Signals (~150 líneas):

- **Frontmatter:**
  - `name: angular-component`
  - `description: Genera un componente Angular 19 standalone con Signals para OrderManagement, siguiendo las convenciones del equipo: imports en orden estricto, estructura de clase con 8 bloques en orden, inject() en lugar de constructor, control flow nuevo (@if/@for/@switch), tokens del design system desde frontend/src/styles/_tokens.scss.`
- **Cuerpo:** secciones «Cuándo se usa», «Estructura del componente» (8 bloques en orden estricto), «Convenciones del fichero .ts» (orden de imports, signals para estado, inject() para deps, control flow nuevo en template), «Convenciones del template» (control flow `@if`/`@for`/`@switch`, tokens del design system), «Convenciones del fichero spec.ts», «Lo que NO debe hacer el skill» (no crear servicios, no modificar app.routes, no tocar el backend), «Pasos al generar».

Sigue el patrón de los slides 23-25 de la gamma 2.2a y respeta las 5 reglas
técnicas críticas (kebab-case, sin XML, sin prefijos `claude`/`anthropic`,
description bajo 1024 chars, sin `README.md` dentro del skill).

## Tarea 3: marcar DEMOS.md + commit

Marca la 2.2a en `docs/DEMOS.md`:

```
- [x] **demo/2.2a** — Primer skill propio: angular-component v1 y v2
```

Verifica con `dotnet build` (0 warnings, 0 errors) y commit:

```powershell
git add .claude/skills/angular-component docs/DEMOS.md
git commit -m "demo/2.2a-after: skill angular-component v2 con convenciones del equipo"
```

NO hagas push.

# Restricciones (importantes)

- NO toques el frontend Angular ni añadas componentes nuevos. Los
  componentes que se generan en el screencast son ejemplos volátiles
  que se descartan al cerrar.
- NO modifiques CLAUDE.md ni .claude/settings.json.
- NO toques el código .NET.
- NO modifiques README.md ni .gitignore.

# Cuando termines, dime

1. Que la rama demo/2.2a-after está creada desde demo/2.2a-before.
2. Que `.claude/skills/angular-component/SKILL.md` existe con frontmatter
   y los bloques de la v2.
3. Que docs/DEMOS.md está marcado.
4. Que dotnet build pasa.
5. Que el commit está hecho.

Si tienes dudas (por ejemplo, sobre las convenciones exactas de imports
o el orden de los 8 bloques), para y pregúntame.
````

---

## 7. Artefactos que Claude Code debe generar

```
✓ Rama demo/2.2a-before (parte de demo/2.1b) — sin cambios respecto al parent
✓ Rama demo/2.2a-after (parte de demo/2.2a-before) con:
  ├── .claude/skills/angular-component/SKILL.md (v2, ~150 líneas)
  └── docs/DEMOS.md con 2.2a marcada como [x]
✓ Verificación de build OK: dotnet build limpio
✓ Commit en demo/2.2a-after: "demo/2.2a-after: skill angular-component v2 con convenciones del equipo"
```

**Lo que NO debe haber generado:**

- ❌ El skill `.claude/skills/angular-component/` (eso se crea EN VIVO)
- ❌ Componentes Angular nuevos (los genera el skill durante el screencast)
- ❌ Cambios en CLAUDE.md o `.claude/settings.json`
- ❌ Cambios en README.md o `.gitignore`
- ❌ Cambios en código .NET

> Si Claude Code se anticipa y crea el skill, **se rechaza el output**. La construcción del skill es el corazón pedagógico de esta demo.

**Lo que el formador commitea EN VIVO sobre `demo/2.2a-before` durante el screencast:**

```
Durante la grabación, sobre demo/2.2a-before, se hace un commit ficticio:
- "demo/2.2a-after: skill angular-component v2 funcional"
  └── .claude/skills/angular-component/SKILL.md (NUEVO, ~150 líneas)
  └── frontend/src/app/components/order-summary/order-summary.component.ts (ejemplo generado)
  └── frontend/src/app/components/order-summary/order-summary.component.html
  └── frontend/src/app/components/order-summary/order-summary.component.scss
  └── frontend/src/app/components/order-summary/order-summary.component.spec.ts

Al cerrar el screencast: el formador descarta el commit real y los
componentes generados. La siguiente clase parte de demo/2.2a-after
(pre-cocinada en §6b) que tiene solo el SKILL.md (los componentes
generados como prueba se descartan — son volátiles).
```

**Estado final del árbol después del screencast (no del prompt):**

```
ordermanagement/
├── .claude/
│   ├── settings.json
│   └── skills/
│       └── angular-component/
│           └── SKILL.md                        ← NUEVO (en vivo)
├── docs/
│   └── DEMOS.md                                ← MODIFICADO (pre-grabación)
├── scripts/
├── src/                                        (sin cambios .NET)
├── frontend/
│   └── src/app/
│       ├── components/
│       │   └── order-summary/                  ← NUEVO carpeta (en vivo)
│       │       ├── order-summary.component.ts
│       │       ├── order-summary.component.html
│       │       ├── order-summary.component.scss
│       │       └── order-summary.component.spec.ts
│       └── orders/                             (sin cambios)
├── tests/
├── .gitignore
├── CLAUDE.md
└── README.md
```

---

## 8. Guion del screencast

**Duración estimada del screencast: ~22-26 minutos.**

Diez bloques. Es la demo más larga del módulo 2 hasta este punto, alineado con que la gamma 2.2a cubre 30 minutos de teoría densa y la construcción merece tiempo.

> **Antes de empezar a grabar**, asegúrate de:
> - Tener PowerShell 7 abierto en pantalla completa, fuente al menos 16pt.
> - Tener VS Code abierto con el repo en `demo/2.2a-before`.
> - **Verificar** que NO existe `.claude/skills/` en el repo. Si existe, bórralo.
> - **Verificar** que el frontend Angular compila: `cd frontend && npm install && npm run build`. Si falla, soluciónalo antes.
> - Tener al menos un componente existente del proyecto (orders-list.component.ts) abierto en VS Code para mostrar como referencia del patrón del equipo.
> - Cerrar Slack, Teams, navegadores con notificaciones.

---

### Bloque 1 — Setup y planteamiento (~1 min 30 seg)

**Pantalla compartida.** A la izquierda VS Code con el repo en `demo/2.2a-before`. A la derecha terminal PowerShell.

**En la terminal:**

```powershell
git status
ls .claude\
```

```
On branch demo/2.2a
nothing to commit, working tree clean

    Directorio: C:\Users\pedro\projects\ordermanagement\.claude

LastWriteTime    Length Name
-------------    ------ ----
...                3456 settings.json
```

**Lo que dices:**

> "Estamos en la rama `demo/2.2a-before`. La 2.1b dejó el repo limpio — el skill experimental `find-handler` que usamos para el experimento de descripciones se borró. Aquí ahora **no hay ningún skill instalado en el proyecto**. Solo `settings.json`.
>
> Y vamos a cambiar eso. Esta es **la primera demo del curso donde construimos un skill propio de verdad**. La gamma 2.2a lo dijo desde el slide 2: *'construir uno, no leer sobre uno'*. Hasta aquí hemos analizado anatomía, hemos diseccionado oficiales, hemos experimentado con descripciones. Pero **construir, ninguno todavía**.
>
> El skill que construimos es **`angular-component`** — un generador de componentes Angular standalone con Signals para OrderManagement. La elección está justificada en el slide 3: es común, se traduce fácil a otros stacks, y al final podéis instalarlo tal cual en vuestro repo del trabajo si vuestro equipo usa Angular.
>
> Y vamos a seguir un hábito que la gamma 2.2a marcó como **el patrón sano** para escribir cualquier skill: **resolver un caso primero, después escribir el skill**. Eso es lo siguiente."

**Tiempo:** ~90 segundos.

---

### Bloque 2 — El hábito sano: resolver un caso primero (~3 min)

> "Antes de tocar `.claude/skills/` siquiera, **vamos a pedirle a Claude Code que genere un componente Angular sin ningún skill**. La gamma slide 5 dijo: *'antes de escribir nada, resuelve un caso primero'*. **Vamos a verlo.**"

**Arrancas Claude Code:**

```powershell
claude
```

```
 Welcome to Claude Code v2.1.x

 cwd: C:\Users\pedro\projects\ordermanagement
 model: claude-opus-4.7
 ✓ CLAUDE.md loaded (147 lines)
 ✓ Project settings loaded
 Type / for commands, ? for help

>
```

> "Sin skills cargados — la línea 'X project skills loaded' no aparece. Solo CLAUDE.md y settings. Le pido el componente:"

**Tecleas:**

```
> Crea un componente Angular para mostrar un resumen del pedido —
  el OrderSummary. Recibe el pedido como input y muestra el total,
  el número de items y el estado. Componente standalone con Signals.
```

**Aparecen las herramientas:**

```
● Read(frontend/src/app/orders/orders-list.component.ts)
● Read(frontend/src/app/orders/order-detail.component.ts)
● Glob(frontend/src/app/**/*.component.ts)
● Read(CLAUDE.md)
```

> "Mirad. **Lee los componentes existentes** para entender el patrón del equipo. Lee el CLAUDE.md. Construye contexto antes de generar."

**Aparece el componente generado por Claude Code (sin skill):**

```typescript
// frontend/src/app/components/order-summary/order-summary.component.ts
import { Component, Input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

interface Order {
  id: number;
  total: number;
  items: any[];
  status: string;
}

@Component({
  selector: 'order-summary',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="order-summary">
      <h3>Order #{{ order.id }}</h3>
      <p>Total: {{ order.total | currency }}</p>
      <p>Items: {{ order.items.length }}</p>
      <p>Status: {{ order.status }}</p>
    </div>
  `,
  styles: [`
    .order-summary { padding: 1rem; border: 1px solid #ccc; }
  `]
})
export class OrderSummaryComponent {
  @Input() order!: Order;
}
```

> "Funciona. Compila. Hace lo que pedí. **Pero mirad las desviaciones del patrón del equipo si lo comparamos con lo que tenemos en `orders-list.component.ts`**:"

**Abres `frontend/src/app/orders/orders-list.component.ts` en VS Code y comparas brevemente.**

> "**Desviaciones que veo**:
>
> Uno. **Selector sin prefijo `app-`**. Pone `selector: 'order-summary'`. El equipo usa siempre `app-` (lo veis en `app-orders-list`). Mal.
>
> Dos. **Usa `@Input` decorador**. El equipo está migrando a la **API de inputs como signals** (`input()`). Esto es un patrón viejo.
>
> Tres. **Template inline en lugar de `templateUrl`**. El equipo separa template y estilos en ficheros aparte. La gamma 2.2a slide 17 lo recordó.
>
> Cuatro. **No genera ficheros separados** — un solo `.ts` con todo dentro. El equipo tiene la convención de cuatro ficheros: `.ts`, `.html`, `.scss`, `.spec.ts`.
>
> Cinco. **No genera test**. El `.spec.ts` no aparece.
>
> **Cinco desviaciones del patrón del equipo en un componente trivial**. El agente no lo ha hecho mal — lo ha hecho **genérico**. Y eso es el problema. **Aquí es donde un skill aporta valor.**
>
> Voy a borrar lo que ha generado para empezar de cero con el skill. Y construyo el skill."

**Salgo (Ctrl+C). En otra terminal:**

```powershell
Remove-Item -Recurse -Force frontend\src\app\components\
```

**Tiempo:** ~3 minutos.

---

### Bloque 3 — Construir el skill v1: el más simple posible (~3 min)

> "Voy a construir la **versión 1 del skill** — la más simple posible. Un solo `SKILL.md`. Frontmatter, cuerpo de unas 30 líneas. Lo que la gamma 2.2a slide 11 marcó como punto de partida."

**En PowerShell:**

```powershell
mkdir .claude\skills\angular-component
```

**En VS Code, creo `.claude/skills/angular-component/SKILL.md` y escribo:**

```markdown
---
name: angular-component
description: Genera componentes Angular standalone con Signals siguiendo la estructura del equipo OrderManagement. Usar cuando el usuario pida crear un nuevo componente Angular, haga referencia a un componente que aún no existe en una feature, o cuando el flujo requiera scaffolding de UI Angular.
---

# Generador de componentes Angular standalone

## Cuándo se usa este skill

Cuando el usuario pida crear un componente Angular nuevo. Esto incluye
peticiones como "crea un componente para X", "necesito un componente
OrderSummary", "vamos a hacer la UI de Y", o referencias implícitas a
un componente que no existe aún.

## Qué genera

Un componente Angular standalone con la estructura estándar del equipo:

- Un fichero `<nombre>.component.ts` con la clase y el decorador
  `@Component({ standalone: true })`
- Un fichero `<nombre>.component.html` con el template
- Un fichero `<nombre>.component.scss` con los estilos
- Un fichero `<nombre>.component.spec.ts` con tests unitarios

## Convenciones que sigue

- Componentes `standalone: true` siempre. Nada de NgModules nuevos.
- Para estado local, usa `signal()`. Para valores derivados, `computed()`.
- Los inputs y outputs van como propiedades del componente con la API
  de signals (`input()`, `output()`).
- Selectores en kebab-case con prefijo `app-`.
- Ubicación por defecto: `frontend/src/app/components/<nombre>/`.

## Pasos al generar

1. Pregunta al usuario el nombre del componente si no lo ha dado.
2. Decide la ubicación según la estructura del proyecto.
3. Genera los cuatro ficheros con la estructura descrita.
4. Si el componente debe enrutarse, sugiere la entrada en routing pero
   no la añadas sin confirmación.
```

**Salvo el fichero.**

> "**Eso es la versión 1.** Treinta y pico líneas. Frontmatter con descripción aplicando la fórmula de los tres ingredientes que vimos en la 2.1b — verbo claro, abanico de triggers, contexto del proyecto. Cuerpo con cuatro secciones: cuándo se usa, qué genera, convenciones, pasos.
>
> **Mirad la descripción.** Aplica todo lo de la 2.1b: *'crear un nuevo componente Angular'*, *'necesito un componente OrderSummary'*, *'vamos a hacer la UI'*. Cuatro variantes de cómo un dev puede pedir esto. Y referencia al proyecto OrderManagement explícita.
>
> Vamos a probarlo."

**Tiempo:** ~3 minutos.

---

### Bloque 4 — Probar el skill v1 (~3 min)

**Arranco Claude Code:**

```powershell
claude
```

```
 Welcome to Claude Code v2.1.x

 cwd: C:\Users\pedro\projects\ordermanagement
 model: claude-opus-4.7
 ✓ CLAUDE.md loaded (147 lines)
 ✓ Project settings loaded
 ✓ 1 project skill loaded: angular-component
 Type / for commands, ? for help

>
```

> "**Mirad la línea: '1 project skill loaded: angular-component'**. Cargado. El frontmatter en el system prompt. Vamos a la prueba."

**Tecleas exactamente la misma petición de antes:**

```
> Crea un componente Angular para mostrar un resumen del pedido —
  el OrderSummary. Recibe el pedido como input y muestra el total,
  el número de items y el estado. Componente standalone con Signals.
```

**Aparece:**

```
● [Skill activated: angular-component]
● Glob(frontend/src/app/**/*.component.ts)
● Read(frontend/src/app/orders/orders-list.component.ts)
● Write(frontend/src/app/components/order-summary/order-summary.component.ts)
● Write(frontend/src/app/components/order-summary/order-summary.component.html)
● Write(frontend/src/app/components/order-summary/order-summary.component.scss)
● Write(frontend/src/app/components/order-summary/order-summary.component.spec.ts)
```

> "**Skill activado.** Y mirad las herramientas: **cuatro Write**, uno por cada fichero del componente. La estructura que pide el skill. Vamos a verlos en VS Code."

**Abres `order-summary.component.ts`:**

```typescript
import { Component, input, computed } from '@angular/core';
import { CommonModule } from '@angular/common';

interface Order {
  id: number;
  total: number;
  items: { id: number; price: number }[];
  status: string;
}

@Component({
  selector: 'app-order-summary',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-summary.component.html',
  styleUrl: './order-summary.component.scss'
})
export class OrderSummaryComponent {
  order = input.required<Order>();
  itemCount = computed(() => this.order().items.length);
}
```

> "Mejora respecto al de antes. **Selector con `app-` ya bien.** **`input.required<Order>()` con la API nueva**. **`templateUrl` y `styleUrl` separados** en lugar de inline. **`computed()` para el itemCount**. Y los cuatro ficheros generados.
>
> Pero atentos a las **limitaciones de la v1** que la gamma 2.2a slide 15 marcó:"

**Abres el `order-summary.component.html`:**

```html
<div class="order-summary">
  <h3>Order #{{ order().id }}</h3>
  <p>Total: {{ order().total | currency }}</p>
  <p>Items: {{ itemCount() }}</p>
  <p>Status: {{ order().status }}</p>
</div>
```

> "**Limitación 1**: El template usa interpolación clásica, está bien. Pero **no usa el control flow nuevo**. Si tuviera condicionales, usaría `*ngIf` antiguo en lugar del nuevo `@if`. El equipo está migrando al control flow de Angular 17+. **El skill v1 no lo dice**.
>
> **Limitación 2**: El `.ts` no tiene **orden estricto de imports**. Solo dos imports aquí pero en componentes reales con muchos imports, el orden importa para el equipo: Angular core → librerías externas → módulos internos → componentes hijo.
>
> **Limitación 3**: Si el componente necesitara inyectar un servicio, **el skill no especifica usar `inject()` en lugar de constructor**. La v1 no lo cubre.
>
> Y mirad el spec:"

**Abres `order-summary.component.spec.ts`:**

```typescript
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { OrderSummaryComponent } from './order-summary.component';

describe('OrderSummaryComponent', () => {
  let component: OrderSummaryComponent;
  let fixture: ComponentFixture<OrderSummaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderSummaryComponent]
    }).compileComponents();
    
    fixture = TestBed.createComponent(OrderSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
```

> "**Limitación 4**: el test es **el genérico de Angular** — solo `should create`. **No tiene Arrange-Act-Assert** explícito. **No prueba inputs**. **No prueba computed**. El equipo tiene un patrón concreto para tests que la v1 no codifica.
>
> Cuatro limitaciones. Algunas pequeñas, otras importantes. **La v1 funciona pero deja que el agente improvise** en zonas donde el equipo tiene reglas concretas. Vamos a la v2."

**Tiempo:** ~3 minutos.

---

### Bloque 5 — Subir a v2: codificar las convenciones del equipo (~5 min)

**Salgo de Claude Code (Ctrl+C). En VS Code, abro el `SKILL.md` y lo amplío:**

> "Voy a meter las convenciones reales del equipo en bloques claros. La gamma 2.2a slides 17 al 21 lo cubrió. Vamos por partes."

**Edito el `SKILL.md` y lo amplío. Primero subo el bloque de estructura del fichero `.ts`:**

```markdown
---
name: angular-component
description: Genera componentes Angular standalone con Signals siguiendo las convenciones estrictas del equipo OrderManagement. Usar cuando el usuario pida crear un nuevo componente Angular, haga referencia a un componente que aún no existe en una feature, o cuando el flujo requiera scaffolding de UI Angular.
---

# Generador de componentes Angular standalone

## Cuándo se usa este skill

Cuando el usuario pida crear un componente Angular nuevo. Esto incluye
peticiones como "crea un componente para X", "necesito un componente
OrderSummary", "vamos a hacer la UI de Y", o referencias implícitas a
un componente que no existe aún.

## Estructura del componente

Cada componente generado tiene cuatro ficheros:

- `<nombre>.component.ts` — clase del componente
- `<nombre>.component.html` — template
- `<nombre>.component.scss` — estilos
- `<nombre>.component.spec.ts` — tests unitarios

Ubicación por defecto: `frontend/src/app/components/<nombre>/`. Si el
contexto del proyecto sugiere otra estructura (por ejemplo, dentro de
una feature como `orders/`), seguir la estructura del proyecto.

## Convenciones del fichero .ts

### Orden de imports (estricto)

1. Angular core (`@angular/core`, `@angular/common`, etc.)
2. Librerías externas (RxJS, librerías de terceros)
3. Módulos internos del proyecto (servicios, modelos, utilities)
4. Componentes hijo

Una línea en blanco entre cada bloque.

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

### Estructura de la clase (orden estricto)

Bloques en este orden, separados por una línea en blanco:

1. Inputs (`input.required()` o `input()`)
2. Outputs (`output()`)
3. Inyecciones con `inject()`
4. Estado local (`signal()`)
5. Valores derivados (`computed()`)
6. Lifecycle hooks (ngOnInit, ngOnDestroy)
7. Métodos públicos
8. Métodos privados (prefijo `_`)

### Inyección de dependencias

Siempre con `inject()`, nunca por constructor:

```typescript
private readonly orderService = inject(OrderService);
```

## Convenciones del template

- Indentación de 2 espacios.
- Atributos largos en líneas separadas, alineados verticalmente.
- Control flow nuevo: `@if`, `@for`, `@switch`. **No usar las
  directivas estructurales antiguas** (`*ngIf`, `*ngFor`).

## Convenciones del fichero spec.ts

- `TestBed.configureTestingModule()` con la configuración mínima.
- Estructura Arrange-Act-Assert explícita en cada test, con comentarios
  `// Arrange`, `// Act`, `// Assert`.
- Mocks con `jasmine.createSpyObj` para servicios inyectados.
- Tests para:
  - creación del componente
  - comportamiento de inputs (que reflejan el valor)
  - eventos de outputs (cuando aplique)
  - lógica de signals computed

## Lo que NO debe hacer el skill

- No generar `NgModule` para el componente nuevo (son standalone).
- No usar `Subject` o `BehaviorSubject` para estado interno (usar signals).
- No inyectar `ChangeDetectorRef` para forzar refresh (innecesario con signals).
- No usar `@Input()` decorador (usar `input.required()` o `input()`).
- No añadir el componente al routing sin pedir confirmación.

## Pasos al generar

1. Si el usuario no ha dado nombre, preguntar.
2. Decidir ubicación según estructura del proyecto.
3. Generar los cuatro ficheros con TODAS las convenciones de arriba
   aplicadas.
4. Verificar que el resultado compila (`cd frontend; npm run build`).
5. Si hay routing implicado, sugerir el snippet pero no aplicarlo sin
   confirmación.
```

**Salvo el fichero.**

> "Mirad lo que he metido en la v2. Vamos por bloques.
>
> **Bloque 'Convenciones del fichero `.ts`'.** Tres subsecciones:
>
> Una. **Orden estricto de imports**. Cuatro categorías. Una línea en blanco entre cada bloque. Esto un junior nuevo no lo sabe. Esto cuesta un PR review explicárselo. **Aquí está codificado.**
>
> Dos. **Decorador con templateUrl y styleUrl en ficheros separados**. *'Template y estilos SIEMPRE en ficheros separados — nunca inline'*. **Énfasis en mayúsculas**. Si el agente tiene la tentación de meter template inline (que la versión 1 a veces hacía), **el skill v2 lo prohíbe explícitamente**.
>
> Tres. **Estructura de la clase con 8 bloques en orden estricto**. Inputs, outputs, inyecciones, estado, derivados, lifecycle, públicos, privados. **Esto es la convención del equipo.** Sin esto, el agente pondría las cosas en el orden que viera en cualquier ejemplo aleatorio.
>
> **Bloque 'Convenciones del template'.** Lo importante: **'Control flow nuevo: `@if`, `@for`, `@switch`. NO usar las directivas estructurales antiguas'**. Esto resuelve la limitación 1 que vimos en la v1.
>
> **Bloque 'Convenciones del fichero spec.ts'.** Resuelve la limitación 4: ya no es 'should create' nada más. Pide tests para creación, inputs, outputs, computed. Con Arrange-Act-Assert explícito. Mocks con `jasmine.createSpyObj`.
>
> **Bloque 'Lo que NO debe hacer el skill'.** Cinco prohibiciones. Esto es **oro puro**. Lo que la gamma 2.1a slide 16 dijo de las 'reglas duras' del CLAUDE.md, aplicado al skill. *'No usar `@Input()` decorador'* es la línea que evita la regresión que vimos en el experimento sin skill.
>
> Vamos a probarlo."

**Tiempo:** ~5 minutos.

---

### Bloque 6 — Probar el skill v2 (~3 min)

**Borro lo que generó la v1:**

```powershell
Remove-Item -Recurse -Force frontend\src\app\components\
```

**Arranco Claude Code:**

```powershell
claude
```

```
✓ 1 project skill loaded: angular-component
```

**Misma petición exacta:**

```
> Crea un componente Angular para mostrar un resumen del pedido —
  el OrderSummary. Recibe el pedido como input y muestra el total,
  el número de items y el estado. Componente standalone con Signals.
```

**Aparece:**

```
● [Skill activated: angular-component]
● Read(frontend/src/app/orders/orders-list.component.ts)
● Read(frontend/src/app/orders/order-detail.component.ts)
● Write(frontend/src/app/components/order-summary/order-summary.component.ts)
● Write(frontend/src/app/components/order-summary/order-summary.component.html)
● Write(frontend/src/app/components/order-summary/order-summary.component.scss)
● Write(frontend/src/app/components/order-summary/order-summary.component.spec.ts)
● Bash(cd frontend && npm run build)
```

> "Mirad. **Read primero** — sigue mirando los componentes existentes para entender el patrón concreto. **Cuatro Write**. Y al final, **`Bash(cd frontend && npm run build)`** — ha leído el paso 4 de los 'Pasos al generar' y verifica el build. **Por su cuenta.**"

**Esperamos al output del build:**

```
> ng build
✔ Browser application bundle generation complete.
```

**El agente responde:**

```
He generado el componente OrderSummary siguiendo las convenciones del
skill angular-component. Cuatro ficheros creados. Build verificado:
0 errores, 0 warnings.

Estructura:
- frontend/src/app/components/order-summary/
  ├── order-summary.component.ts (con orden de imports, decorador con
    template/style separados, clase con 8 bloques en orden)
  ├── order-summary.component.html (con control flow nuevo @if)
  ├── order-summary.component.scss (vacío con comentario placeholder)
  └── order-summary.component.spec.ts (4 tests con Arrange-Act-Assert)

¿Quieres que añada el componente a alguna ruta?
```

> "Build verde. Y mirad lo que cumple. Voy a abrir los ficheros."

**Abro `order-summary.component.ts`:**

```typescript
import { Component, input, computed } from '@angular/core';

import { CommonModule } from '@angular/common';

import { OrderService } from '../../services/order.service';

interface Order {
  id: number;
  total: number;
  items: { id: number; price: number }[];
  status: 'Pending' | 'Confirmed' | 'Shipped' | 'Cancelled';
}

@Component({
  selector: 'app-order-summary',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-summary.component.html',
  styleUrl: './order-summary.component.scss'
})
export class OrderSummaryComponent {
  // 1. Inputs
  order = input.required<Order>();

  // 5. Valores derivados
  itemCount = computed(() => this.order().items.length);
}
```

> "Mirad las convenciones aplicadas:
>
> - **Orden de imports correcto**: Angular core, línea en blanco, librería externa (CommonModule), línea en blanco, módulo interno (OrderService aunque no lo use, lo dejó indicado), línea en blanco.
> - **Selector con `app-`**.
> - **`templateUrl` y `styleUrl` separados** ya no inline.
> - **Estructura de clase con comentarios numerados** — bloque 1 (Inputs), bloque 5 (Valores derivados). Si tuviera servicios, los pondría en el bloque 3.
> - **`input.required<Order>()`** con la API nueva — no `@Input` decorador.
>
> Y el template:"

**Abro `order-summary.component.html`:**

```html
<div class="order-summary">
  <h3>Order #{{ order().id }}</h3>

  @if (order().items.length > 0) {
    <p>Total: {{ order().total | currency }}</p>
    <p>Items: {{ itemCount() }}</p>
  } @else {
    <p>No items</p>
  }

  <p>Status: {{ order().status }}</p>
</div>
```

> "**Control flow nuevo**: `@if` y `@else`. La regla del skill aplicada. **Indentación de 2 espacios.**"

**Abro `order-summary.component.spec.ts`:**

```typescript
import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderSummaryComponent } from './order-summary.component';

describe('OrderSummaryComponent', () => {
  let component: OrderSummaryComponent;
  let fixture: ComponentFixture<OrderSummaryComponent>;

  const mockOrder = {
    id: 1,
    total: 100,
    items: [{ id: 1, price: 50 }, { id: 2, price: 50 }],
    status: 'Pending' as const
  };

  beforeEach(async () => {
    // Arrange
    await TestBed.configureTestingModule({
      imports: [OrderSummaryComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(OrderSummaryComponent);
    fixture.componentRef.setInput('order', mockOrder);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should reflect input order', () => {
    // Arrange
    const expectedId = mockOrder.id;
    
    // Act
    const actualId = component.order().id;
    
    // Assert
    expect(actualId).toBe(expectedId);
  });

  it('should compute itemCount correctly', () => {
    // Arrange
    const expectedCount = 2;
    
    // Act
    const actualCount = component.itemCount();
    
    // Assert
    expect(actualCount).toBe(expectedCount);
  });
});
```

> "**Tres tests** ahora. *Should create* (genérico). *Should reflect input order* (verifica el input). *Should compute itemCount correctly* (verifica el computed). **Cada uno con Arrange-Act-Assert explícito** con comentarios. **Mock del order al inicio.**
>
> **Cero desviaciones** del patrón del equipo. **Cero**. Las cuatro limitaciones que vimos en la v1 — control flow viejo, orden de imports, no usar `inject()`, tests sin AAA — **resueltas**. **Esta es la diferencia entre un skill v1 y un skill v2.**"

**Tiempo:** ~3 minutos.

---

### Bloque 7 — Las dos señales para parar de añadir prosa (~2 min)

**Salgo de Claude Code (Ctrl+C). Vuelvo a VS Code y abro el `SKILL.md` v2:**

> "Antes de cerrar, **un punto importante** que la gamma 2.2a marcó en los slides 23 al 26: **¿hasta dónde subir el listón de la prosa?**"

**Hago scroll en el `SKILL.md` mostrando su longitud actual:**

> "El `SKILL.md` v2 tiene unas 150 líneas. Aproximadamente 700-800 palabras. **Bien dentro del límite seguro** que vimos en la 2.1a — el rango sano es 1.500 a 2.000 palabras. Tenemos margen.
>
> Pero la pregunta es: ¿hasta dónde seguir añadiendo? La gamma marcó **dos señales claras** para parar de añadir prosa:
>
> **Señal 1**: el fichero pasa de 2.000 palabras. Cuando llegáis ahí, parad. El cuerpo del SKILL.md se carga entero al activarse el skill, y pasarse del rango sano os va a ocupar contexto que necesitáis para el código.
>
> **Señal 2**: las convenciones se vuelven repetitivas o muy detalladas. Cuando estáis escribiendo el quinto bloque de código de ejemplo en prosa, **eso ya no es prosa — son plantillas**. Y las plantillas viven mejor fuera del SKILL.md, en `assets/`.
>
> En la siguiente demo, **2.2b**, vamos a ver qué pasar cuando llega una de estas señales. **Vamos a refactorizar el skill subiéndolo a v3 con scripts** — para tareas deterministas como validar nombres o contar componentes — **y a v4 con plantillas** en `assets/`. Esa es la siguiente capa.
>
> **Para el caso del componente Angular básico, la v2 que tenéis aquí es suficiente.** No todo skill necesita v3 ni v4. La gamma slide 28 lo dijo: solo cuando aparezca una de las dos señales, toca subir."

**Tiempo:** ~2 minutos.

---

### Bloque 8 — Commitear el skill y los componentes generados (~2 min)

> "Vamos a commitear lo que tenemos. **El skill `angular-component` v2 funcional**. Y dejamos el componente `OrderSummary` generado como ejemplo en el repo — es la primera prueba de que el skill funciona."

**En la terminal:**

```powershell
git status
```

```
On branch demo/2.2a
Untracked files:
        .claude/skills/
        frontend/src/app/components/
```

**Stageo y commit:**

```powershell
git add .claude/skills/ frontend/src/app/components/
git commit -m "demo/2.2a-after: skill angular-component v2 funcional + componente generado de prueba"
```

```
[demo/2.2a-before abc1234] demo/2.2a-after: skill angular-component v2 funcional + componente generado de prueba
 5 files changed, 178 insertions(+)
 create mode 100644 .claude/skills/angular-component/SKILL.md
 create mode 100644 frontend/src/app/components/order-summary/order-summary.component.ts
 create mode 100644 frontend/src/app/components/order-summary/order-summary.component.html
 create mode 100644 frontend/src/app/components/order-summary/order-summary.component.scss
 create mode 100644 frontend/src/app/components/order-summary/order-summary.component.spec.ts
```

> "Cinco ficheros nuevos commiteados. **El skill queda en la rama**. Todas las demos siguientes — la 2.2b, la 2.2c, la 2.3, todo el resto del curso — van a tener este skill disponible.
>
> Y eso es la 2.2a."

**Tiempo:** ~2 minutos.

---

### Bloque 9 — Recap de hábitos del primer skill (~1 min 30 seg)

> "Tres hábitos clave que el alumno se lleva al lunes."

**En el editor de texto al lado:**

```
HÁBITO 1: Resuelve un caso primero, después escribe el skill.
─────────────────────────────────────────────────────────────
Empezar escribiendo el skill es como diseñar la API de una librería
sin haber implementado primero. Casi siempre falla.

Lo que vimos en el bloque 2: pedir el componente sin skill, observar
desviaciones, y de ahí salieron las convenciones que codifiqué en
el SKILL.md. Esa es la dirección correcta.

HÁBITO 2: Empieza por la versión más simple posible.
────────────────────────────────────────────────────
La v1 tenía 30 líneas. Funcionó. Tenía limitaciones, sí. Pero
funcionó como punto de partida. **Si la v1 es suficiente para tu
caso, no hagas v2**. Solo subes cuando la v1 te queda corta.

HÁBITO 3: Para de añadir prosa cuando aparezca una señal.
─────────────────────────────────────────────────────────
Señal 1: 2.000 palabras. Señal 2: las convenciones se vuelven
plantillas. Cuando llegues a una, toca v3 (scripts) o v4 (assets).
No antes.
```

> "Tres hábitos. Si los aplicáis en el lunes, vais a evitar los errores que la gamma 2.2a slide 6 marcó como típicos del primer día — escribir un skill abstracto, sin probarlo, demasiado largo desde el principio."

**Tiempo:** ~1 minuto 30 segundos.

---

### Bloque 10 — Cliffhanger a la 2.2b (~1 min)

> "En la siguiente demo, **2.2b**, vamos a llevar el skill a las versiones 3 y 4. Vais a ver:
>
> **v3 con scripts** — un script Python o Bash que el skill ejecuta para tareas deterministas. Por ejemplo, validar que el nombre del componente nuevo no choca con uno existente, o contar cuántos componentes Angular hay en el proyecto. La gamma 2.2b slide 5 lo cubrió en teoría. La 2.2b lo materializa.
>
> **v4 con plantillas en `assets/`** — cuando las convenciones se convierten en bloques de código repetitivos, sacamos esos bloques a `assets/templates/` y el `SKILL.md` solo dice 'usa la plantilla'. Esto resuelve la señal 2 de parada de prosa que mencionamos hace un momento.
>
> Al final de la 2.2b, **el skill `angular-component` queda completo a nivel producción**. Y lo más importante: vais a tener el modelo mental para escribir vuestros propios skills aplicando los mismos principios.
>
> Empezamos con el dos punto dos punto B."

**Tiempo:** ~1 minuto.

---

## 9. Qué resaltar verbalmente

Cinco puntos que **no pueden quedarse sin decir**:

1. **"Resuelve un caso primero, después escribe el skill."** — el hábito sano que evita el 80% de los errores. Bloque 2 (cuando hacemos el experimento sin skill), recap en bloque 9.

2. **"La v1 tiene 30 líneas. Funciona."** — el alumno tiene que aceptar que **lo simple es válido**. Bloque 3 cuando construimos la v1.

3. **"Las cuatro limitaciones de la v1 cubiertas por la v2."** — la progresión justificada. Bloque 5 al subir a v2.

4. **"Dos señales para parar de añadir prosa: 2.000 palabras o convenciones convertidas en plantillas."** — la regla operativa para decidir cuándo subir a v3/v4. Bloque 7.

5. **"El skill queda en la rama. Todas las demos siguientes lo tienen."** — el alumno entiende que esto es una contribución estructural permanente, no un experimento desechable. Bloque 8.

**Frase de remate al final:**

> *"Hemos construido el primero. Treinta líneas funcionaron como punto de partida. Ciento cincuenta lo dejaron a nivel equipo. Mismo flujo, vuestros casos."*

---

## 10. Slide de entrada (locución HeyGen)

> **Texto del avatar antes del screencast:**

"Y vamos a la siguiente demo. La 2.2a. La primera del curso donde construimos un skill propio de verdad. Vais a ver dos cosas. Primero, el hábito sano que la gamma marcó: resolver un caso primero, después escribir el skill. Antes de tocar `.claude/skills/`, le pedimos a Claude Code que genere un componente Angular sin skill. Observamos las cuatro o cinco desviaciones del patrón del equipo. **Y esa observación es la base sobre la que construimos el skill** — no nuestra imaginación. Segundo, la progresión incremental: arrancamos por la versión 1 mínima — un solo `SKILL.md` de treinta líneas con la fórmula de la 2.1b aplicada — y subimos a la versión 2 con las convenciones reales del equipo codificadas. Cada versión se prueba en vivo sobre el mismo componente OrderSummary. Veréis las limitaciones de la v1 y cómo la v2 las resuelve. Vamos al screencast."

---

## 11. Slide de salida (locución HeyGen)

> **Texto del avatar al volver del screencast:**

"Lo que acabáis de ver es vuestro primer skill propio funcional. Ciento cincuenta líneas codifican lo que un junior nuevo necesitaría que le explicarais durante semanas — orden de imports, estructura de la clase con ocho bloques en orden, control flow nuevo, tests con Arrange-Act-Assert, las cinco cosas que el skill prohíbe explícitamente. Tres hábitos para llevarse al lunes. Uno: resolved un caso primero, después escribid el skill. Empezar por escribir el skill casi siempre falla. Dos: empezad por la versión más simple posible. Si la v1 funciona para vuestro caso, no hagáis v2 hasta que se quede corta. Tres: parad de añadir prosa cuando aparezca una de las dos señales — pasar de dos mil palabras o que las convenciones se conviertan en plantillas. En la siguiente demo, la 2.2b, vamos a ver qué hacer cuando aparece una de estas señales: subimos el skill a versión 3 con scripts ejecutables y a versión 4 con plantillas en `assets/`. Empezamos con el dos punto dos punto B."

---

## 12. Tiempo total estimado

| Bloque | Tiempo |
|---|---|
| Slide de entrada (avatar) | ~30 seg |
| Bloque 1 — Setup y planteamiento | ~1 min 30 seg |
| Bloque 2 — Hábito sano: resolver caso primero | ~3 min |
| Bloque 3 — Construir skill v1 | ~3 min |
| Bloque 4 — Probar el skill v1 | ~3 min |
| Bloque 5 — Subir a v2: codificar convenciones | ~5 min |
| Bloque 6 — Probar el skill v2 | ~3 min |
| Bloque 7 — Las dos señales para parar de añadir prosa | ~2 min |
| Bloque 8 — Commitear skill y componentes | ~2 min |
| Bloque 9 — Recap de hábitos | ~1 min 30 seg |
| Bloque 10 — Cliffhanger a la 2.2b | ~1 min |
| Slide de salida (avatar) | ~30 seg |
| **Total screencast** | **~25-26 min** |
| **Total con avatar** | **~26-27 min** |

> Si hay preguntas durante el screencast, súmale 3-4 minutos. La demo encaja en un bloque de **30 minutos** dentro de la sesión.

**Margen de seguridad por si algo va lento:**

- **Si el componente que Claude genera SIN skill (bloque 2) no tiene las desviaciones esperadas** (porque el CLAUDE.md ya cubre suficiente), comenta: *"a veces el CLAUDE.md cubre tanto que el agente sin skill ya genera bien. Pero veréis que cuando el equipo tiene reglas más finas, el skill aporta. Vamos a buscar lo que falta"*. Y exagera levemente las desviaciones que sí veas.

- **Si la v1 produce un componente igual de bueno que la v2** (porque Claude es muy bueno), reconócelo: *"a veces la v1 funciona mejor de lo esperado. Pero atentos a esto..."* — y muestra que la v1 tiene **el riesgo de no ser determinista**: en una nueva sesión el agente podría improvisar de forma distinta. La v2 garantiza el patrón. **La diferencia no es siempre visible en un solo intento, es la consistencia entre intentos.**

- **Si el `npm run build` del bloque 6 falla** por algo del entorno, no improvises silencio. *"Esto pasa en el flujo real. El agente lo va a ver, ajustar y repetir"*. Deja que Claude Code lo arregle. Es la fase 4 del ciclo agentic en directo.

- **Si el skill v2 no se activa** la segunda vez (probabilístico), recuerda al alumno la 2.1b: *"la activación es probabilística, no determinista. Vamos a relanzar"*. Y pide otra vez con vocabulario distinto.

- **Si te quedas sin tiempo y los bloques 9 y 10 te aprietan**, recorta el bloque 9 (recap de hábitos) a 1 minuto. Los hábitos están en el slide de salida del avatar.

---

# Apéndice A — Decisiones pedagógicas justificadas

> Esta sección no se graba. Es para Pedro, formador.

**¿Por qué `angular-component` y no un skill .NET?**

Porque el manual 2.2 explícitamente lo eligió y el cuerpo entero del manual está construido alrededor de este ejemplo. Mantener consistencia con el manual evita que el alumno se confunda al cruzar referencias. **Si tu audiencia es solo .NET y Angular no aplica**, comenta brevemente al inicio: *"el patrón se traduce a controllers .NET con DTO/validator/test — los conceptos son idénticos"* (es lo que el propio manual dice en la sección de introducción).

**¿Por qué empezar pidiéndole un componente SIN skill (bloque 2)?**

Porque la gamma 2.2a slides 5-9 marcaron este flujo como **el patrón sano** para escribir cualquier skill. Si la demo escribiera el skill directamente, estaría enseñando el anti-patrón. **Tienes que vivir en pantalla el momento de "esto está mal porque no usa `app-`"** para que el alumno entienda **de dónde salen las reglas del skill**.

**¿Por qué la v1 tiene exactamente 30-35 líneas?**

Porque es **el mínimo que cubre la fórmula de descripción del 2.1b** y aplica el principio del 2.1a (cuerpo ligero). Más corto sería un skill incompleto. Más largo invadiría la pedagogía de la v2. **Es el punto que demuestra que "lo mínimo absoluto" funciona como punto de partida**.

**¿Por qué el `OrderSummary` y no `OrderForm`?**

Porque `OrderSummary` es **simple de razonar** (un display de datos sin lógica compleja) y **se puede generar rápido**. Un formulario tendría más complejidad técnica (validación, eventos, formularios reactivos) que distraería del foco pedagógico de la demo. **Mantenemos el dominio de UI Angular pero la complejidad técnica baja para que el foco esté en la progresión del skill, no en Angular**.

**¿Por qué el bloque 5 (v2) muestra el SKILL.md entero?**

Porque el alumno tiene que **ver todas las convenciones que se codifican** para apreciar el alcance del salto v1→v2. Si solo enseño los bloques nuevos, no se ve cómo se integran con el cuerpo. Mostrar el `SKILL.md` v2 entero es **el momento donde el alumno comprende qué tamaño tiene un skill v2 real** (~150 líneas, ~800 palabras) y por qué cabe dentro del rango sano de la 2.1a.

**¿Por qué dejar el `OrderSummary` generado como prueba en la rama?**

Por dos razones. Una: es **evidencia material** de que el skill v2 funciona. El alumno puede mirar la rama y verificar que la convención se cumple. Dos: la 2.2b va a aprovechar este componente — cuando construyamos un script de validación, lo probaremos contra el componente existente. **Continuidad entre demos**.

**¿Por qué las dos señales de parada se mencionan AQUÍ y no en la 2.2b?**

Porque la gamma 2.2a las cubrió en sus slides 23-26 — son **el cierre conceptual de la 2.2a**, el puente a la 2.2b. Si las dejara para la 2.2b, perdería esa conexión. **Mejor cerrar la 2.2a respondiendo "y entonces ¿hasta dónde subimos?"** — y la 2.2b empieza con "ya, vamos a aplicar lo que decíamos sobre las señales".

**¿Por qué el bloque 9 (recap de hábitos) y no incluir esto en el bloque 10 (cliffhanger)?**

Porque los **hábitos son el take-away principal** de esta demo y merecen un bloque dedicado. El cliffhanger es promoción de la 2.2b. Mezclarlos diluye los hábitos. **Estructura limpia: take-away primero, promoción después**.

**¿Por qué los componentes generados van a `frontend/src/app/components/` y no a `frontend/src/app/orders/`?**

Porque el equipo de OrderManagement tiene la convención de **componentes reutilizables en `components/`** y **componentes de feature en `<feature>/`**. El `OrderSummary` es **reutilizable** — se podría usar en orders-list, en order-detail, en un dashboard. Por eso va a `components/`. La v2 del skill explicita esto en la sección 'Estructura del componente'. **Decisión arquitectónica del equipo materializada**.

**¿Por qué el commit es uno solo en lugar de dos (skill + componente)?**

Porque conceptualmente son **una sola pieza** — el skill v2 funcional probado con un componente real. Separarlos en dos commits implicaría que se pueden tener uno sin el otro. **No es así**. El componente es la **prueba** del skill, no una contribución independiente.

**¿Por qué Pedro no usa `/skills` slash command para gestionar el skill?**

Porque `/skills` o equivalentes son **del módulo 2.2c** (el skill de scopes y gestión). Aquí estamos creando, no gestionando. Si introduzco `/skills`, invado el contenido siguiente. La creación se hace **escribiendo directamente el fichero en VS Code**, que es como cualquier dev empezaría.
