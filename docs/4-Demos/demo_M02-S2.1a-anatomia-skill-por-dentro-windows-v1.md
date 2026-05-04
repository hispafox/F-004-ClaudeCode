# Demo 2.1a — Anatomía de un skill por dentro: leyendo `frontend-design` oficial

> **Versión:** v1 | **Módulo:** 2 | **Sub:** 2.1a | **Estado:** ✅ Versión final
> **Archivo:** `demo_M02-S2.1a-anatomia-skill-por-dentro-windows-v1.md`
> **Branch destino:** `demo/2.1a`
> **Branch de partida:** `demo/1.3b`
> **Tiempo total estimado:** ~16-19 minutos
> **Tipo:** Demo de exploración. **Aún no se crea ningún skill — se diseccionan los oficiales para que el alumno entienda qué tiene un skill por dentro antes de escribir el suyo en la 2.2a.** Es deliberadamente "ver, no tocar". Las demos de creación empiezan en 2.2a.
> **Plataforma:** Windows 11 (PowerShell 7).

---

## 1. Contexto

Cerramos el módulo 1 con una pregunta abierta: *"¿qué patrón se repite tres veces a la semana en vuestro equipo y al que tendríais que enseñarle a un junior nuevo?"*. El alumno llegó al módulo 2 con (idealmente) una idea o dos en mente. Y en la gamma 2.1a (30 slides, ~30 min) ha visto qué es un skill: la unidad mínima, el ciclo de vida (metadata cargada al inicio, cuerpo cargado bajo demanda, ficheros adicionales cargados solo si hacen falta), los tres scopes, la estructura del directorio, las seis reglas técnicas críticas, y la anatomía del frontmatter YAML con tres ejemplos.

Lo que la teoría no transmite del todo: **un skill real, escrito por gente que sabe lo que hace, leído en pantalla**. Por eso esta demo no construye nada. **Diseccionamos**.

Y diseccionamos sobre material **oficial** — los skills que vienen instalados con Claude Code o que Anthropic publica como referencia. Es la mejor escuela para ver qué hace un buen skill por dentro: cómo se escribe el frontmatter, cómo se estructura el cuerpo, cómo se distribuyen los `references/` y `scripts/`, qué entra en el `SKILL.md` y qué se externaliza. La 2.1b va a desmenuzar la **descripción como switch** y la 2.2a empieza la creación. Esta es la base.

> **Tipo de demo:** lectura crítica. El alumno ve, no toca. La rama `demo/2.1a` solo añade la marca en `docs/DEMOS.md` y un fichero `docs/skills-explorados.md` con notas para repaso del alumno. **No instala skills, no crea ninguno**.

---

## 2. Objetivo de la demo

Cinco cosas concretas que tienen que quedar en la cabeza del alumno cuando termine los ~17 minutos de screencast:

1. **Un skill es un directorio con `SKILL.md` dentro. Punto.** Lo demás (`scripts/`, `references/`, `assets/`) es opcional. Tras esta demo, el alumno tiene que haber visto eso con sus ojos en al menos dos ejemplos reales.

2. **El frontmatter YAML es la pieza que activa o no activa el skill.** Dos campos obligatorios — `name` y `description`. Y la descripción **se escribe pensando en cuándo activarlo**, no en qué hace. Aquí ya empieza a sembrarse lo que la 2.1b explica con detalle.

3. **El cuerpo del `SKILL.md` debe quedar entre 1.500 y 2.000 palabras como tope.** Lo gordo va a `references/`. Es por progressive disclosure — la diferencia entre que tener 30 skills se note o no se note en el contexto.

4. **Los ficheros `references/` se cargan solo si hacen falta.** Eso lo veremos en pantalla — abrimos un skill que tiene `references/`, vemos cómo el `SKILL.md` apunta a ellos sin meterlos directamente.

5. **`SKILL.md` no es lo mismo que `CLAUDE.md`.** El árbol de decisión de la gamma slide 8 y el manual línea 344. **`CLAUDE.md` siempre. Skill bajo demanda.** Esta diferencia es la que más confunde al alumno y conviene reforzarla con casos concretos.

Y dos cosas que tienen que **NO quedar** en su cabeza:

- *"Tengo que saber escribir skills antes de poder usarlos."* — no, en la 2.3 vamos a ver el ecosistema y muchos vienen ya listos.
- *"Cuanto más completo el skill, mejor."* — al revés. **El cuerpo ligero, lo gordo en `references/`**. Un skill de 5.000 palabras es señal de que algo está mal estructurado.

---

## 3. Branch de partida

```
demo/1.3b
```

> Estado actual de la rama: el módulo 1 cerrado entero. `CLAUDE.md` con sus 5 bloques. `.claude/settings.json` con permisos. La feature de cancelación de pedidos implementada con `InvalidOrderStateException`, refactor del handler y endpoint nuevo. Todo commiteado.

---

## 4. Branch destino

```
demo/2.1a
```

> Tras la demo, la rama `demo/2.1a` añade dos cosas mínimas: la marca `[x]` en `docs/DEMOS.md` y un fichero `docs/skills-explorados.md` con notas que el alumno se puede llevar como repaso de los skills explorados. **No se instalan skills, no se crea ninguno propio**. Es deliberado: la creación empieza en 2.2a.

---

## 5. Estado del repo al empezar

Idéntico a `demo/1.3b`. La estructura del proyecto sigue intacta:

```
ordermanagement/
├── .claude/
│   └── settings.json                   (allow/deny configurados)
├── docs/
│   └── DEMOS.md                        (1.1, 1.2a, 1.2b, 1.3a, 1.3b marcadas)
├── scripts/
│   └── audit-staged.sh                 (de demo/1.3a)
├── src/
│   ├── OrderManagement.Api/
│   │   └── Controllers/
│   │       └── OrdersController.cs     (con endpoint cancel desde 1.3b)
│   ├── OrderManagement.Application/
│   │   ├── Exceptions/
│   │   │   ├── CustomerNotFoundException.cs
│   │   │   ├── OrderNotFoundException.cs
│   │   │   └── InvalidOrderStateException.cs  (de 1.3b)
│   │   └── Handlers/
│   │       ├── CreateOrderHandler.cs
│   │       ├── UpdateOrderHandler.cs
│   │       └── CancelOrderHandler.cs   (refactorizado en 1.3b)
│   ├── OrderManagement.Domain/
│   └── OrderManagement.Infrastructure/
├── frontend/
├── tests/
├── .gitignore
├── CLAUDE.md
└── README.md
```

**Estado clave para esta demo:**

- **No hay `.claude/skills/`** todavía en el proyecto. La carpeta no existe. Eso lo veremos en el bloque 1.
- En el `~/.claude/skills/` del usuario — el scope personal — **tampoco hay skills propios todavía**. Pero en la instalación de Claude Code de Pedro **sí hay skills oficiales del bundle**. Esos son los que vamos a explorar.
- Los skills oficiales viven en `~/.claude/skills/` o en una carpeta del paquete de Claude Code en Windows: `C:\Users\pedro\.local\claude-code\skills\` o equivalente según versión instalada. **Esto lo aclaramos en el bloque 2 con `find`/`Get-ChildItem`**.

**Estado de la máquina Windows del formador:**

```
✅ Claude Code v2.1.x instalado y autenticado
✅ Git for Windows
✅ PowerShell 7
✅ VS Code con el repo cargado en demo/2.1a
✅ Tener al menos UN skill oficial accesible para leer en VS Code
   (típicamente: frontend-design, simplify, docx, pdf, pptx, xlsx)
```

> **Importante para Pedro antes de grabar:** verifica con `Get-ChildItem` la ruta exacta donde Claude Code tiene los skills oficiales en tu máquina. Puede ser `~/.claude/skills/` (carpeta del usuario), o dentro del propio binario en `~/.local/claude-code/...`. **Ajusta los comandos del screencast a la ruta real**. Las rutas que aparecen en el guion son orientativas.

**Lo que el alumno verá al final de la demo:**

- La estructura de un skill oficial real desplegada en VS Code: `SKILL.md` + `references/` + (a veces) `scripts/`.
- El frontmatter de **dos skills oficiales** (`frontend-design` y `simplify`) leído línea a línea con observaciones críticas.
- El árbol completo de un skill mediano explorado en pantalla.
- La diferencia entre lo que va en `SKILL.md` (instrucciones generales) y lo que se externaliza a `references/` (detalles densos).
- Una nota sobre dónde están los skills oficiales en Windows (depende de la versión de Claude Code).

---

## 6. Prompt para Claude Code

> Lo que tú, formador, copias y pegas en Claude Code para preparar la rama `demo/2.1a` antes de grabar.

````
Estoy preparando la demo 2.1a del curso de Claude Code para devs .NET +
Angular. Esta demo es la anatomía de un skill por dentro — exploramos
skills oficiales que vienen con Claude Code para entender la estructura
real antes de escribir el primer skill propio en la 2.2a.

# Contexto

Estoy en la rama `demo/1.3b` del repo `ordermanagement`. La rama tiene
todo el módulo 1 completo: CLAUDE.md, .claude/settings.json, scripts/,
y la feature de cancelación de pedidos implementada.

Quiero que prepares la rama demo/2.1a con un cambio mínimo: marcar la
demo en docs/DEMOS.md y añadir un docs/skills-explorados.md con notas
de los skills que se van a explorar durante el screencast. NO instales
ningún skill ni crees ningún SKILL.md.

# Lo que necesito

Tres tareas:

## Tarea 1: crear la rama

```powershell
git checkout demo/1.3b
git pull
git checkout -b demo/2.1a
```

## Tarea 2: actualizar docs/DEMOS.md

Localiza la línea:

```
- [ ] demo/2.1a — Primer skill leído por dentro
```

Cámbiala por:

```
- [x] **demo/2.1a** — Anatomía de un skill leyendo los oficiales
```

## Tarea 3: crear docs/skills-explorados.md

Contenido:

```markdown
# Skills explorados en la demo 2.1a

Notas de los skills oficiales que se diseccionaron durante la demo
2.1a para entender la anatomía. Sirven como referencia para escribir
skills propios a partir de la demo 2.2a.

## Skills explorados

### 1. `frontend-design` (oficial Anthropic)

**Para qué sirve:** generación de componentes y páginas web siguiendo
prácticas modernas de diseño.

**Por qué lo elegimos para la demo:** es uno de los más completos del
bundle oficial. Tiene `SKILL.md`, `references/` con varias guías
tematizadas, y `scripts/` con utilidades. Buen ejemplo de skill
estructurado a nivel producción.

**Lecciones que enseña:**
- Frontmatter conciso, descripción enfocada en cuándo activarlo.
- SKILL.md ligero (~1.500 palabras), apunta a `references/` para detalle.
- Separación clara: principios en SKILL.md, recetas concretas en
  references/, scripts utilitarios en scripts/.

### 2. `simplify` (oficial Anthropic)

**Para qué sirve:** simplificar código manteniendo el comportamiento.

**Por qué lo elegimos para la demo:** es el opuesto en complejidad
al frontend-design. Es un skill mínimo — un solo SKILL.md, sin
references/ ni scripts/. Buen ejemplo de skill pequeño que justifica
existir.

**Lecciones que enseña:**
- Un skill puede ser una sola página de instrucciones.
- La descripción es la pieza más importante: marca cuándo se activa.
- No siempre hace falta references/ y scripts/.

## Patrón emergente

Tras explorar dos skills oficiales con escalas distintas:

1. **El cuerpo del SKILL.md es siempre ligero** (1.500-2.000 palabras).
2. **El frontmatter es declarativo** — describe cuándo activar, no
   re-explica qué hace el cuerpo.
3. **Los `references/` se mencionan, no se inlinean.** El SKILL.md
   apunta a ellos: "para X, consulta `references/x.md`".
4. **Los `scripts/` se ejecutan vía Bash y solo el output llega al
   contexto.** Permite mantener trabajo determinista fuera del
   razonamiento del modelo.

## Próximo paso

En la demo 2.1b vamos a profundizar en la **descripción como switch**
— la pieza que decide si un skill se activa o no cuando el usuario
hace una petición. Y en la 2.2a empezamos a crear nuestro primer skill
propio: un generador de componentes Angular standalone para OrderManagement.
```

## Tarea 4: verificar y commitear

```powershell
dotnet build
```

Esperado: 0 warnings, 0 errors. (No tocamos código, solo doc, pero
verificamos.)

```powershell
git add docs/DEMOS.md docs/skills-explorados.md
git commit -m "demo/2.1a: notas de skills oficiales explorados"
```

NO hagas push.

# Restricciones (importantes)

- NO crees ningún `.claude/skills/` en el proyecto. Esta demo es
  exploración, no creación. Los skills propios empiezan en 2.2a.
- NO modifiques CLAUDE.md ni .claude/settings.json.
- NO toques el código de la app.
- NO modifiques README.md.
- El único cambio adicional al docs/DEMOS.md debe ser docs/skills-explorados.md.

# Cuando termines, dime

1. Que la rama demo/2.1a está creada desde demo/1.3b.
2. Que docs/DEMOS.md tiene 2.1a marcada.
3. Que docs/skills-explorados.md está creado.
4. Que el build pasa.
5. Que el commit está hecho.

Si tienes dudas, para y pregúntame.
````

---

## 7. Artefactos que Claude Code debe generar

```
✓ Rama nueva: demo/2.1a (parte de demo/1.3b)
✓ docs/DEMOS.md con 2.1a marcada como [x]
✓ docs/skills-explorados.md con notas de los skills explorados
✓ Verificación de build OK: dotnet build limpio
✓ Commit único: "demo/2.1a: notas de skills oficiales explorados"
```

**Lo que NO debe haber generado:**

- ❌ Ningún `.claude/skills/` en el proyecto
- ❌ Ningún `SKILL.md` propio
- ❌ Cambios en código de la app
- ❌ Cambios en CLAUDE.md o `.claude/settings.json`
- ❌ Cambios en README.md o `.gitignore`

> Si Claude Code se anticipa y crea un `.claude/skills/` aunque sea vacío, **se rechaza el output**. La creación empieza estrictamente en 2.2a.

**Estado final del árbol después del prompt:**

```
ordermanagement/
├── .claude/
│   └── settings.json
├── docs/
│   ├── DEMOS.md                    ← MODIFICADO (1 línea)
│   └── skills-explorados.md        ← NUEVO
├── scripts/
├── src/                            (sin cambios)
├── frontend/                       (sin cambios)
├── tests/                          (sin cambios)
├── .gitignore                      (sin cambios)
├── CLAUDE.md                       (sin cambios)
└── README.md                       (sin cambios)
```

---

## 8. Guion del screencast

**Duración estimada del screencast: ~14-16 minutos.**

Siete bloques. Es una demo de exploración — más visual que técnica. El alumno mira VS Code más que la terminal.

> **Antes de empezar a grabar**, asegúrate de:
> - Tener PowerShell 7 abierto en pantalla completa, fuente al menos 16pt.
> - Tener VS Code abierto al lado con el repo `ordermanagement` cargado en `demo/2.1a`.
> - **Crítico:** **localizar previamente** la ruta donde están los skills oficiales en tu instalación. Puede ser:
>   - `C:\Users\pedro\.claude\skills\` (skills personales, normalmente vacío en limpio)
>   - O dentro del binario de Claude Code: ruta exacta varía según versión
>   - O en una carpeta sistema: `C:\Program Files\Claude Code\skills\`
>   Lánzalo con: `Get-ChildItem -Recurse -Filter "SKILL.md" -ErrorAction SilentlyContinue C:\Users\pedro\` y similar. **Confirma cuáles ves antes de grabar.**
> - **Asegurarse** de que `frontend-design` (o un skill similar con `references/`) está accesible para abrirlo en VS Code. Si no lo está, cámbialo por otro skill oficial que sí lo esté.
> - Cerrar Slack, Teams, navegadores con notificaciones.

---

### Bloque 1 — Setup y la pregunta del cierre del módulo 1 (~1 min 30 seg)

**Pantalla compartida.** A la izquierda, VS Code con el repo en `demo/2.1a`. A la derecha, una terminal PowerShell.

**En la terminal:**

```powershell
git status
git log --oneline -3
```

```
On branch demo/2.1a
nothing to commit, working tree clean

abc1234 (HEAD -> demo/2.1a) demo/2.1a: notas de skills oficiales explorados
xyz9876 (demo/1.3b) demo/1.3b: implementa endpoint POST /api/orders/{id}/cancel...
def5678 demo/1.3b: marca demo de workflow completo (pre-grabación)
```

**Lo que dices:**

> "Estamos en la rama `demo/2.1a`. Cerramos el módulo 1 con la pregunta de la gamma 1.3b slide 24: *'¿qué patrón se repite tres veces a la semana en vuestro equipo y al que tendríais que enseñarle a un junior nuevo?'*. Si vosotros traéis una respuesta a esa pregunta, vamos a llegar al final del módulo 2 con vuestro primer skill funcional. **No el ejemplo del curso. Vuestro de verdad.**
>
> Pero antes de escribir un skill, conviene **leer skills bien escritos por otros**. Es la mejor escuela. Esta demo es eso. **Diseccionamos skills oficiales** — los que vienen instalados de fábrica con Claude Code o que Anthropic publica como referencia.
>
> No vamos a crear nada. **Vamos a leer.** Vais a ver dos cosas. Primero, dónde viven los skills en una instalación Windows real. Segundo, dos skills reales — uno completo con `references/` y `scripts/`, y uno minimalista con solo `SKILL.md`. Veréis las diferencias y los patrones que comparten."

**Tiempo:** ~90 segundos.

---

### Bloque 2 — Dónde viven los skills en Windows (~2 min)

> "Primera cosa práctica: ¿dónde están los skills en mi máquina? Esto es importante para poder explorarlos."

**Tecleas:**

```powershell
Get-ChildItem -Recurse -Filter "SKILL.md" -ErrorAction SilentlyContinue $env:USERPROFILE\.claude\
```

**Aparece algo como (depende de la instalación):**

```
    Directorio: C:\Users\pedro\.claude\skills\frontend-design

LastWriteTime    Length Name
-------------    ------ ----
...                1234 SKILL.md

    Directorio: C:\Users\pedro\.claude\skills\simplify

LastWriteTime    Length Name
-------------    ------ ----
...                 678 SKILL.md

    Directorio: C:\Users\pedro\.claude\skills\docx

LastWriteTime    Length Name
-------------    ------ ----
...                2456 SKILL.md
...
```

> "Ahí los tenéis. Los skills oficiales que vienen con Claude Code viven en `C:\Users\<usuario>\.claude\skills\`. Esto es el **scope user** que la gamma 2.1a slide 8 mencionó. **Cada skill es un directorio**. **Dentro de cada directorio hay un `SKILL.md`**. Y posiblemente otras carpetas que vamos a ver enseguida.
>
> Los que veo aquí son los que Anthropic publica como skills base. `frontend-design` para componentes web. `simplify` para refactor. `docx`, `pdf`, `pptx`, `xlsx` para generación de documentos.
>
> Si vuestro proyecto tiene skills propios — los que el equipo escribe — viven en `.claude/skills/` **del repo**. **Scope project**. Ese es el que va a git y se comparte. **El nuestro está vacío** ahora mismo. Vamos a verlo:"

**Tecleas:**

```powershell
Get-ChildItem .claude\
```

```
    Directorio: C:\Users\pedro\projects\ordermanagement\.claude

LastWriteTime    Length Name
-------------    ------ ----
...                3456 settings.json
```

> "**Solo el `settings.json` que pusimos en la 1.2b**. **Ningún `skills/`**. Eso lo crearemos en la 2.2a cuando escribamos nuestro primer skill propio.
>
> Vamos a abrir el primer skill oficial. **`frontend-design`**. Es uno de los más completos."

**Tiempo:** ~2 minutos.

---

### Bloque 3 — Disección 1: `frontend-design` (skill completo) (~5 min)

**En VS Code, abres la carpeta `C:\Users\pedro\.claude\skills\frontend-design\` directamente desde el explorador (no desde el repo).**

> "Mirad la estructura. Si os recordáis de la gamma 2.1a slide 11, **el árbol estándar de un skill** era: `SKILL.md`, `scripts/`, `references/`, `assets/`."

**El árbol que aparece en VS Code (real, depende de versión):**

```
frontend-design/
├── SKILL.md
├── references/
│   ├── animations.md
│   ├── design-philosophy.md
│   ├── design-systems.md
│   └── interactivity.md
└── scripts/
    └── (algunas utilidades)
```

> "Ahí lo tenéis. **`SKILL.md` + carpeta `references/` con cuatro ficheros + carpeta `scripts/`**. Un skill mediano-grande. **Vamos a entrar primero al `SKILL.md`**."

**Click en `SKILL.md`. Se abre en el editor.**

**Lees el frontmatter en pantalla (ejemplo, el contenido real puede variar):**

```yaml
---
name: frontend-design
description: Create distinctive, production-grade frontend interfaces with high design quality. Use this skill when the user asks to build web components, pages, artifacts, posters, or applications (examples include websites, landing pages, dashboards, React components, HTML/CSS layouts, or when styling/beautifying any web UI). Generates creative, polished code and UI design that avoids generic AI aesthetics.
---
```

**Lo destacas con el cursor:**

> "Mirad bien. **Solo dos campos**. `name` y `description`. Lo que la gamma 2.1a slide 25 marcó como obligatorios. Pero **mirad el contenido de `description`**.
>
> Empieza con un verbo claro: **'Create distinctive, production-grade frontend interfaces...'**. Le sigue una indicación explícita de cuándo activarlo: **'Use this skill when the user asks to build web components, pages, artifacts, posters, or applications'**. Y después da **ejemplos concretos**: 'websites, landing pages, dashboards, React components, HTML/CSS layouts, when styling/beautifying any web UI'.
>
> Esto es **la descripción como switch**, lo que la 2.1b va a desmenuzar. **Si yo escribo a Claude *'créame una landing page para X'*, esta descripción coincide con mi petición y el skill se activa**. Si escribo *'analízame este SQL'*, no coincide, no se activa. **La descripción es lo que decide.**"

**Bajas en el `SKILL.md`. Lees el cuerpo:**

> "Y debajo del frontmatter, el cuerpo. Voy a hacer scroll rápido para que veáis la longitud. Mirad."

**Haces scroll del SKILL.md hasta el final.**

> "El cuerpo es de unas mil quinientas palabras. **Dentro del rango que la gamma 2.1a slide 15 marcó**: 1.500-2.000 palabras como tope. Esto es importante porque **la regla no es estética — es de rendimiento**. Un `SKILL.md` que pasa de las dos mil palabras carga ese peso entero cada vez que el skill se activa.
>
> ¿Cómo cabe todo el conocimiento de diseño frontend en mil quinientas palabras? **Mirad esto.**"

**Buscas en el SKILL.md una mención a `references/`:**

```markdown
## Design philosophy

For a deeper exploration of design principles applied here, consult
`references/design-philosophy.md`.

## Animations

For animation patterns, see `references/animations.md`.

## Interactivity

For state management and interaction patterns, see
`references/interactivity.md`.
```

> "**Aquí está el patrón.** El `SKILL.md` da las instrucciones generales y **apunta a los `references/`** cuando hace falta entrar en detalle. *'For a deeper exploration of design principles, consult `references/design-philosophy.md`'*. **Claude solo va a leer ese fichero si la tarea concreta lo requiere.** Eso es **progressive disclosure**.
>
> Vamos a abrir uno de los `references/` para que veáis qué hay dentro."

**Click en `references/design-philosophy.md`. Se abre.**

**Haces scroll por el contenido:**

> "Mirad. Este fichero solo es **mucho más largo que el `SKILL.md`** — fácil tres mil, cuatro mil palabras. Tiene principios de diseño detallados, ejemplos de buenas y malas prácticas, comentarios sobre cuándo aplicar cada cosa.
>
> **Esto no se carga al activar el skill.** Solo se carga si **dentro de la conversación** Claude decide que necesita esa información. Por ejemplo, si pido *'créame una landing'*, se activa el skill y el `SKILL.md` entra al contexto. Si después pregunto *'¿qué decisión tomarías sobre la jerarquía visual aquí?'*, ahí Claude va a `design-philosophy.md` y lo lee.
>
> **Rentabilidad pura del modelo de carga bajo demanda.**"

**Vuelves al árbol del skill. Abres `scripts/`:**

> "Y la última pieza: `scripts/`. **Para tareas deterministas que es mejor hacer con código que con razonamiento**. La gamma 2.1a slide 12 lo dijo claro: si una tarea se puede hacer con un script de Python, mejor hacerla con script. Es más fiable, más barato, y más rápido. El script se ejecuta vía Bash y **solo el output llega al contexto, no el código del script**.
>
> En `frontend-design` los scripts hacen utilidades como **generar paletas de colores con valores hexadecimales correctos** o **calcular ratios de contraste WCAG**. Cosas que un modelo de lenguaje haría más lento y a veces con errores. Un script Python lo hace en cien milisegundos sin equivocarse."

**Tiempo:** ~5 minutos.

---

### Bloque 4 — Disección 2: `simplify` (skill mínimo) (~3 min)

> "Ahora vamos al opuesto. **Un skill mínimo**. Solo `SKILL.md`. Para que veáis que **un skill puede ser una sola página de instrucciones bien escritas**."

**En VS Code, abres `C:\Users\pedro\.claude\skills\simplify\`:**

```
simplify/
└── SKILL.md
```

> "Eso es. **Un solo fichero.** Sin `references/`, sin `scripts/`, sin `assets/`. ¿Es un skill? Sí. ¿Es válido? Sí. ¿Es útil? Vamos a ver."

**Click en `SKILL.md`. Se abre.**

**Lees el frontmatter:**

```yaml
---
name: simplify
description: Simplify code while preserving its behavior. Use when the user asks to refactor for clarity, reduce complexity, eliminate redundancy, or make code more readable. Applies to functions, classes, or modules where the structure is overly complex but the logic is correct.
---
```

> "Mirad la descripción. **Misma estructura que `frontend-design`**: verbo claro al inicio (*'Simplify code while preserving its behavior'*), trigger explícito (*'Use when the user asks to refactor for clarity, reduce complexity, eliminate redundancy, or make code more readable'*), y matiz importante al final (*'Applies to functions, classes, or modules where the structure is overly complex but the logic is correct'*).
>
> Ese matiz es interesante. **Marca cuándo NO se debe activar**. Si el código tiene un bug y le pido a Claude que lo arregle, este skill **no debería activarse** — es un caso donde la lógica está mal, no donde la estructura es compleja. **Esa precisión en la descripción evita activaciones equivocadas.**"

**Bajas al cuerpo:**

> "Y el cuerpo. Mirad lo corto que es."

**Haces scroll. El SKILL.md cabe en una pantalla, máximo dos.**

> "Quizás unas trescientas, cuatrocientas palabras. **Más corto incluso que el límite recomendado**. ¿Y qué dice? Cuatro o cinco principios de cómo simplificar código sin romperlo. *'Conserva el comportamiento observable'*. *'No hagas cambios que el test no detectaría'*. *'Si dudas, pregunta antes de cambiar'*.
>
> **Es suficiente.** Para algo que no requiere conocimiento del dominio — solo aplicar buenas prácticas universales — esto basta. **Skills cortos son válidos cuando el dominio es estrecho.**"

> "Comparativa rápida de los dos."

**Tiempo:** ~3 minutos.

---

### Bloque 5 — Comparativa de los dos y patrón emergente (~2 min)

**Vuelves a la terminal o abres un editor de texto al lado:**

> "Lo que hemos visto en pantalla, en una tabla mental:"

**Escribes en pantalla (texto de referencia, no comando):**

```
                  frontend-design       simplify
─────────────────────────────────────────────────────
Estructura        SKILL.md +            SKILL.md
                  references/           solo
                  scripts/

Tamaño SKILL.md   ~1.500 palabras       ~400 palabras

References        4 ficheros            0

Scripts           Sí                    No

Cuándo activar    Construir frontend    Refactorizar código
                  desde cero            ya correcto

Conocimiento      Profundo, con         Universal, no
                  matices de diseño     requiere matices
```

> "Mirad las diferencias. Pero mirad también lo que **comparten**:
>
> Uno. **El frontmatter tiene la misma estructura.** `name` y `description`. La descripción **enfocada en cuándo activar**, con verbos claros y casos de uso concretos.
>
> Dos. **El cuerpo del `SKILL.md` está dentro del límite** que la gamma 2.1a slide 15 marcó. El de `frontend-design` está cerca del tope. El de `simplify` está muy por debajo. Pero **ninguno se pasa**.
>
> Tres. **Los `references/` se mencionan, no se inlinean.** En `frontend-design` el SKILL.md dice *'consulta `references/design-philosophy.md`'*. **No copia el contenido del fichero ahí**. La gamma slide 13 lo dijo: el SKILL.md mantiene las instrucciones generales, los detalles densos van a `references/`.
>
> Estos tres son **el patrón** que cualquier skill bien escrito sigue. Cuando vosotros escribáis el primero en la 2.2a, vais a aplicar exactamente esto."

**Tiempo:** ~2 minutos.

---

### Bloque 6 — `SKILL.md` vs `CLAUDE.md` con casos concretos (~2 min)

> "Antes de cerrar, un punto que la gamma 2.1a slide 8 cubrió pero que conviene **anclar con casos del nuestro proyecto OrderManagement**. La diferencia entre `CLAUDE.md` y un skill."

> "**Recordad la regla que la gamma marcó:**
>
> - **`CLAUDE.md` siempre.** Información que el agente necesita en cada sesión, para cualquier tarea.
> - **Skill bajo demanda.** Capacidad puntual que aplica a ciertas tareas concretas.
>
> Vamos a aplicarlo a OrderManagement, con casos del manual línea 355."

**Abres el `CLAUDE.md` del repo en VS Code y lo enseñas brevemente:**

> "Mirad nuestro `CLAUDE.md`. Tiene cinco bloques: visión general, estructura, comandos, convenciones, reglas duras. Toda esta información **aplica siempre que el agente toque este repo**. Por eso está en `CLAUDE.md`.
>
> ¿Qué pondría en `CLAUDE.md`?
>
> **Caso 1 del manual:** convención de naming de variables. Aplica a todo el código. **Va en `CLAUDE.md`**. Y de hecho está en nuestro CLAUDE.md, en la sección 'Convenciones .NET'.
>
> **Caso 3 del manual:** comando para arrancar el dev environment. Aplica siempre. **`CLAUDE.md`**. Está en nuestra sección 'Comandos'.
>
> **Caso 6 del manual:** estructura de carpetas. Aplica siempre. **`CLAUDE.md`**. Sección 'Estructura'.
>
> ¿Qué pondría en un skill?
>
> **Caso 2 del manual:** forma estándar de generar un endpoint nuevo. **Solo aplica cuando generas endpoints**. Si lo metiera en `CLAUDE.md`, esta información cargaría en cada sesión, para cada tarea, aunque la sesión sea solo para preguntar qué hace una clase. **Es un buen candidato a skill**.
>
> **Caso 4 del manual:** checklist de seguridad antes de un PR. **Solo aplica antes de PRs**. Skill.
>
> Y la regla que la gamma cierra con: **si una convención solo aplica a una de cada cinco tareas, no debería estar en `CLAUDE.md` — debería ser un skill.**"

> "Esto es lo que vais a tener que decidir cada vez que penséis 'esto no es obvio para el agente, ¿lo apunto?'. **Pregunta clave: ¿aplica siempre o solo a ciertas tareas?**"

**Tiempo:** ~2 minutos.

---

### Bloque 7 — Recap, notas guardadas y cliffhanger (~1 min 30 seg)

> "Y eso es la 2.1a. Repaso rápido en cuatro puntos."

**En la terminal:**

```powershell
cat docs/skills-explorados.md
```

**Aparece el contenido del fichero que se commiteó:**

```
# Skills explorados en la demo 2.1a

Notas de los skills oficiales que se diseccionaron durante la demo
2.1a para entender la anatomía...
...
```

> "Las notas que hicimos están commiteadas en `docs/skills-explorados.md`. Os queda como referencia.
>
> **Recap.**
>
> Uno. **Un skill es un directorio con `SKILL.md` dentro.** Lo demás es opcional. Hemos visto un skill mínimo (`simplify`) y uno completo (`frontend-design`). Los dos son válidos.
>
> Dos. **El frontmatter es la pieza que activa el skill.** Dos campos obligatorios — `name` y `description`. La **descripción** se escribe pensando en **cuándo activarlo**, no solo en qué hace. Con verbos claros y ejemplos.
>
> Tres. **El cuerpo del `SKILL.md` debe quedar entre 1.500 y 2.000 palabras.** Lo gordo va a `references/`. **Progressive disclosure**.
>
> Cuatro. **`SKILL.md` ≠ `CLAUDE.md`.** `CLAUDE.md` siempre. Skill bajo demanda. Si la información solo aplica a ciertas tareas, es skill.
>
> En la siguiente demo, **2.1b**, vamos a profundizar en **la descripción como switch** — qué hace que un skill se active o no se active cuando el usuario pide algo. Es la pieza más sutil de la anatomía y donde más se equivoca la gente al escribir su primer skill. La 2.2a empezamos a crear el primero propio: un generador de componentes Angular standalone para OrderManagement."

**Tiempo:** ~1 minuto 30 segundos.

---

## 9. Qué resaltar verbalmente

Cinco puntos que **no pueden quedarse sin decir**:

1. **"Un skill es un directorio con `SKILL.md` dentro. Lo demás es opcional."** — el alumno tiene que poder repetir esta frase. Bloque 3, recap en bloque 7.

2. **"El frontmatter tiene dos campos obligatorios: `name` y `description`. La descripción es el switch."** — siembra de la 2.1b. Bloque 3.

3. **"El cuerpo del `SKILL.md` entre 1.500 y 2.000 palabras como tope. Lo gordo va a `references/`."** — la regla de oro del progressive disclosure. Bloque 3, recap en bloque 7.

4. **"Los scripts son para tareas deterministas. Solo el output llega al contexto, no el código."** — clave del rendimiento. Bloque 3, parte de scripts.

5. **"`CLAUDE.md` siempre. Skill bajo demanda. Si solo aplica a ciertas tareas, es skill."** — la pieza que más confunde al alumno. Bloque 6.

**Frase de remate al final:**

> *"Hemos diseccionado lo escrito por gente que sabe. En la 2.2a vamos a escribir el nuestro siguiendo exactamente los patrones que acabáis de ver."*

---

## 10. Slide de entrada (locución HeyGen)

> **Texto del avatar antes del screencast:**

"Y arrancamos el módulo 2 con la primera demo. La 2.1a. Antes de escribir el primer skill propio, vamos a leer skills bien escritos por otros. Es la mejor escuela. Vais a ver dos cosas. Primero, dónde viven los skills en una instalación Windows real — `C:\Users\pedro\.claude\skills\`. Segundo, dos skills oficiales en pantalla con escalas distintas: `frontend-design` con su `SKILL.md` más `references/` más `scripts/`, y `simplify` que es solo `SKILL.md` y nada más. Diseccionamos los dos. Veréis las diferencias y los patrones que comparten — el frontmatter con `name` y `description`, el cuerpo dentro del límite de las dos mil palabras, los `references/` que se mencionan pero no se inlinean. Es la base para escribir vuestro primero en la 2.2a. Vamos al screencast."

---

## 11. Slide de salida (locución HeyGen)

> **Texto del avatar al volver del screencast:**

"Lo que acabáis de ver son dos skills oficiales en sus dos extremos. Uno completo con `references/` y `scripts/`. Otro minimalista con solo el `SKILL.md`. Y los dos siguiendo el mismo patrón: frontmatter con dos campos obligatorios, cuerpo ligero, lo denso externalizado. Esa es la anatomía. Y la decisión clave: **`CLAUDE.md` siempre, skill bajo demanda**. Si la información que tenéis en mente solo aplica a ciertas tareas, es skill, no `CLAUDE.md`. En la siguiente demo, la 2.1b, vamos a profundizar en la pieza más sutil — la descripción como switch. Qué hace que un skill se active o no cuando el usuario pide algo. Es donde más se equivoca la gente al escribir su primer skill, así que merece la pena pararse un momento ahí antes de pasar a la creación. Empezamos con el dos punto uno punto B."

---

## 12. Tiempo total estimado

| Bloque | Tiempo |
|---|---|
| Slide de entrada (avatar) | ~30 seg |
| Bloque 1 — Setup y la pregunta del cierre | ~1 min 30 seg |
| Bloque 2 — Dónde viven los skills en Windows | ~2 min |
| Bloque 3 — Disección 1: `frontend-design` | ~5 min |
| Bloque 4 — Disección 2: `simplify` | ~3 min |
| Bloque 5 — Comparativa y patrón emergente | ~2 min |
| Bloque 6 — `SKILL.md` vs `CLAUDE.md` con casos | ~2 min |
| Bloque 7 — Recap y cliffhanger | ~1 min 30 seg |
| Slide de salida (avatar) | ~30 seg |
| **Total screencast** | **~17-18 min** |
| **Total con avatar** | **~18-19 min** |

> Si hay preguntas durante el screencast, súmale 2-3 minutos. La demo encaja en un bloque de **20 minutos** dentro de la sesión.

**Margen de seguridad por si algo va lento:**

- **Si los skills oficiales NO están en `~/.claude/skills/` en tu instalación**, tienes dos opciones. La fácil: **clona el repo público de skills de Anthropic** (`anthropic/skills` en GitHub) en una carpeta local antes de grabar y muestra esos. Comenta: *"normalmente vienen instalados con Claude Code, pero por compactarse el contenido los hemos clonado del repo oficial"*. La pedagogía no se ve afectada — el skill que muestras es el mismo. La menos fácil: usar dos skills personalizados que escribas tú a mano antes de grabar (aunque entonces "oficial" pierde sentido). Recomiendo la primera.

- **Si el `SKILL.md` que abres es muy distinto de los ejemplos del manual** (porque las versiones cambian), **adapta el guion al contenido real**. La pedagogía es **el patrón** (frontmatter conciso, cuerpo ligero, references externos), no las palabras exactas. Lee lo que ves y comenta los principios.

- **Si el alumno pregunta cómo crear el primer skill propio**, responde brevemente: *"esa es la 2.2a. Aquí estamos viendo la anatomía. Crear empezamos en el siguiente bloque del módulo. Hoy es lectura crítica."* No te metas a crear nada en directo — invade la 2.2a.

- **Si el bloque 3 (disección de `frontend-design`) se hace pesado por el volumen del fichero**, recorta la lectura del cuerpo a un escaneo rápido. Lo crítico es el frontmatter y la mención a `references/`. Si te quedas sin tiempo, salta el `scripts/` y dilo: *"hay scripts también, los veréis cuando exploréis vuestro propio cuando llegue el momento"*.

- **Si la diferencia entre `references/` y `assets/` no queda clara visualmente** (porque `frontend-design` puede no tener `assets/`), comenta: *"`assets/` es para plantillas y ficheros binarios. Aquí no lo vemos pero lo tenemos en otros skills. La diferencia: `references/` se lee como contexto, `assets/` se usa como punto de partida para copiar"*. Y sigue.

---

# Apéndice A — Decisiones pedagógicas justificadas

> Esta sección no se graba. Es para Pedro, formador.

**¿Por qué empezar el módulo 2 con una demo de exploración y no de creación?**

Porque **escribir un skill sin haber leído skills bien escritos es escribir adivinando**. La gamma 2.1a (30 slides de teoría) explica la anatomía pero no la materializa. Esta demo materializa la anatomía con **dos skills reales** antes de que el alumno escriba el suyo. Es la diferencia entre saber qué es un libro y haber leído algunos antes de escribir uno.

**¿Por qué `frontend-design` y `simplify` específicamente?**

Porque cubren **los dos extremos del rango**. `frontend-design` es uno de los skills más completos del bundle oficial — tiene la estructura completa (SKILL + references + scripts), un cuerpo cerca del límite, y es **fácil de entender** lo que hace (componentes web). `simplify` es lo opuesto — solo SKILL.md, cuerpo corto, dominio universal. **Mostrando los dos extremos en el mismo screencast, el alumno ve que ambos son válidos**.

**¿Por qué no usamos un skill de un MCP o de un proyecto interno como ejemplo?**

Porque dependeríamos de tener un skill custom listo para grabar, y eso aumenta la fricción. Los skills oficiales **están en cualquier instalación de Claude Code** — Pedro los tiene, el alumno los tendrá. **Pedagogía sin fricción**.

**¿Por qué la rama 2.1a deja huella mínima en el repo?**

Porque la 2.1a **no instala ni crea nada**. Lo que el alumno se lleva está en su cabeza, no en el código. La marca en `docs/DEMOS.md` y el `docs/skills-explorados.md` con notas son para que pueda revisar lo visto sin tener que rebobinar el vídeo. **No invadir el repo es deliberado** — la 2.2a será la primera demo que añada `.claude/skills/` al proyecto.

**¿Por qué se guarda el fichero `docs/skills-explorados.md` con notas?**

Porque es la primera demo del módulo 2 donde **no hay artefacto visible para el alumno**. Si el alumno cierra el screencast pensando *"vi cosas pero no me llevé nada"*, perdemos. Las notas son **el artefacto físico** que se queda en su rama y le permite revisar lo visto.

**¿Por qué la comparativa del bloque 5 está en formato tabla?**

Porque el cerebro humano procesa contraste mejor en tabla que en prosa. Tras ver dos skills en directo, **la tabla cierra el modelo mental**. El alumno se va con un mapa visual de las dos escalas — completo vs minimalista — y los puntos que comparten.

**¿Por qué dedicar un bloque entero a `SKILL.md` vs `CLAUDE.md`?**

Porque **es la pieza que más confunde a la gente que llega a esto** (manual línea 336). Si la demo no la cierra con casos del proyecto OrderManagement aplicados, el alumno se va con *"vale, vi skills, pero ¿cuándo paso algo al `CLAUDE.md` y cuándo lo hago skill?"*. Los seis casos del manual aplicados a OrderManagement (controllers, comandos, naming, security checklist, etc.) **anclan la decisión con material que el alumno reconoce**.

**¿Por qué la advertencia de "ajusta los comandos a la ruta real"?**

Porque las rutas exactas de los skills oficiales **dependen de la versión de Claude Code y de cómo se instaló** (native installer vs npm vs personal vs sistema). El guion no puede asumir una ruta concreta — Pedro tiene que verificar la suya antes de grabar. **Si la ruta del guion no funciona, la demo se cae al instante**. Mejor advertir explícitamente.

**¿Por qué Pedro debe localizar previamente el repo de skills de Anthropic como plan B?**

Porque hay versiones de Claude Code que **no traen skills oficiales preinstalados** — los descargas a demanda o los clonas del repo público. Si el formador llega al bloque 2 y `Get-ChildItem` devuelve vacío, **el plan B (clonar `anthropic/skills` antes) salva la demo**. Es trabajo de 5 minutos previo al screencast pero blinda la grabación.
