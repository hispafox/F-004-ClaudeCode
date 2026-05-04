# Demo 2.2c — Control de invocación, scopes y cierre del bloque de creación

> **Versión:** v1 | **Módulo:** 2 | **Sub:** 2.2c | **Estado:** ✅ Versión final
> **Archivo:** `demo_M02-S2.2c-control-scopes-cierre-windows-v1.md`
> **Branch before:** `demo/2.2c-before`  (estado al hacer `git checkout` antes de grabar — sin db-reset ni commit-style)
> **Branch after:**  `demo/2.2c-after`   (estado final pre-cocinado con los dos skills nuevos commiteados)
> **Branch parent:** `demo/2.2b-after`
> **Tiempo total estimado:** ~22-25 minutos
> **Tipo:** Demo de decisiones operativas (INFRA). **Cierra el bloque de creación de skills (2.2) cubriendo lo que falta: `disable-model-invocation`, `argument-hint`, slash commands explícitos, scopes user vs project, y las reglas técnicas críticas que no son negociables.** El alumno termina con el modelo operativo completo para construir y desplegar skills propios. Sigue el patrón **before/after** definido en [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md).
> **Plataforma:** Windows 11 (PowerShell 7).

---

## 1. Contexto

En la 2.2a construimos `angular-component` v1 y v2. En la 2.2b lo subimos a v3 con plantillas en `assets/` y v4 con script en `scripts/`. **El skill está completo a nivel producción**.

Pero faltan **decisiones operativas** que no son sobre el contenido del skill — son sobre cómo se usa, dónde vive, y cuándo NO debe activarse. La gamma 2.2c (26 slides, ~25 min) las agrupa en cinco bloques:

1. **Control de invocación** — `disable-model-invocation`, `argument-hint`, slash commands explícitos.
2. **Subagentes en skills** (referencia rápida del `context: fork` que veremos a fondo en módulo 3).
3. **Scopes** — personal (`~/.claude/skills/`), proyecto (`.claude/skills/`), plugin.
4. **Errores frecuentes con primeros skills** — los anti-patrones que casi todo el mundo comete.
5. **Reglas técnicas críticas** — kebab-case obligatorio, prefijos reservados, sin XML en frontmatter, límite 1024 chars en description, no `README.md` dentro del skill.

Esta demo aterriza la gamma con **dos casos prácticos sobre OrderManagement**:

- Crear un **segundo skill** (`db-reset`) que es **destructivo** — solo invocable explícitamente con `/db-reset` (showcase de `disable-model-invocation`).
- **Promover** un skill personal a proyecto — escribimos un skill personal (`commit-style`) en `~/.claude/skills/`, lo probamos, y luego lo movemos a `.claude/skills/` cuando vemos que aporta al equipo.

> **Tipo de demo:** decisiones operativas con dos showcases. La rama `demo/2.2c-after` queda con dos skills nuevos en el repo: `db-reset` (destructivo, solo `/db-reset`) y `commit-style` (promovido de personal a proyecto). **Es la primera vez que el alumno ve un skill destructivo y la promoción personal → proyecto en directo.**

---

## 2. Objetivo de la demo

Cinco cosas concretas que tienen que quedar en la cabeza del alumno cuando termine los ~22 minutos de screencast:

1. **`disable-model-invocation: true` para skills destructivos.** El alumno debe poder identificar cuándo un skill cae en una de las tres categorías (destructivo, caro, experimental) y aplicar la flag. **Lo verá en directo con `db-reset`**.

2. **Los tres scopes y la regla de decisión.** Personal `~/.claude/skills/` para lo que viaja contigo. Proyecto `.claude/skills/` para lo del equipo. Plugin para distribución (módulo 2.3). **Y el patrón sano: empezar personal, promover a proyecto cuando se valida**.

3. **Las 5 reglas técnicas críticas que no son negociables.** Nombre `SKILL.md` case-sensitive. Carpeta en kebab-case que coincide con `name`. No empezar por `claude` ni `anthropic`. Sin XML en frontmatter. `description` bajo 1024 chars. **Si te saltas una, el skill no funciona**.

4. **Los 8 errores frecuentes del primer día** que la gamma 2.2c slides 17-19 enumeró. Skill demasiado grande, empezar por v4, no iterar la descripción, etc. **El alumno los repasa en pantalla como checklist**.

5. **Cierre operativo del módulo 2.2.** El alumno se va con un repo que tiene **3 skills funcionales** (`angular-component` v4, `db-reset` con disable-model-invocation, `commit-style` promovido de personal). Y con la siembra clara de la 2.3: ecosistema y distribución.

Y dos cosas que tienen que **NO quedar** en su cabeza:

- *"Tengo que poner `disable-model-invocation` en todos mis skills por seguridad."* — no, **solo en los destructivos, caros o experimentales**. Aplicarlo en skills útiles los anula.
- *"Los skills personales son para escapar del equipo."* — no, **son la fase de experimentación**. La promoción a proyecto cuando funciona es **la culminación**, no una traición.

---

## 3. Branch `demo/2.2c-before`

Punto de partida del screencast.

```
demo/2.2c-before
```

**Parte de:** `demo/2.2b-after`.

**Estado del repo:** el skill `angular-component` v4 con `SKILL.md`, `assets/` (3 plantillas) y `scripts/generate.py`. Componentes `OrderSummary` y `OrderFilter` generados como prueba en `frontend/src/app/components/`. Todo commiteado de la 2.2b. **NO hay aún `db-reset` ni `commit-style`** — esos dos skills son la pieza viva de la demo.

> El formador hace `git checkout demo/2.2c-before` antes de empezar a grabar.

---

## 4. Branch `demo/2.2c-after`

Estado final que la siguiente clase (2.3) asume.

```
demo/2.2c-after
```

**Parte de:** `demo/2.2c-before`.

**Qué añade respecto a `-before`:** **dos skills nuevos** al proyecto:

1. **`db-reset`** en `.claude/skills/db-reset/SKILL.md` — skill destructivo con `disable-model-invocation: true`. Resetea la BBDD local. **Showcase de control de invocación**.
2. **`commit-style`** en `.claude/skills/commit-style/SKILL.md` — skill que se crea primero en personal (`~/.claude/skills/`) y se promueve a proyecto cuando se valida. **Showcase de scopes y promoción**.

Más la marca `[x]` en `docs/DEMOS.md` y `docs/skills-explorados.md` actualizado con las decisiones operativas tomadas. **Es la primera rama del curso donde el repo tiene 3 skills funcionales conviviendo**.

**Cómo se prepara:** ver §6b. Se materializa antes de grabar — Claude Code en una sesión limpia genera los dos skills equivalentes a los que el formador construirá en directo.

> Durante la grabación, el formador construye los dos skills en directo desde `demo/2.2c-before`. Al cerrar descarta los cambios reales y la siguiente clase parte de `demo/2.2c-after` ya pre-cocinada.

---

## 5. Estado del repo al hacer `git checkout demo/2.2c-before`

Idéntico a `demo/2.2b-after`:

```
ordermanagement/
├── .claude/
│   ├── settings.json
│   └── skills/
│       └── angular-component/                  (v4 completo desde 2.2b)
│           ├── SKILL.md
│           ├── assets/
│           │   ├── component.template.ts
│           │   ├── component.template.html
│           │   └── component.template.spec.ts
│           └── scripts/
│               └── generate.py
├── docs/
│   ├── DEMOS.md                                (hasta 2.2b marcada)
│   └── skills-explorados.md
├── scripts/
├── src/                                        (sin cambios .NET)
├── frontend/
│   └── src/app/components/
│       ├── order-summary/                      (de 2.2a)
│       └── order-filter/                       (de 2.2b)
├── tests/
├── .gitignore
├── CLAUDE.md
└── README.md
```

**Estado clave para esta demo:**

- **`~/.claude/skills/`** del usuario (scope personal): inicialmente sin skills propios, solo los oficiales (`frontend-design`, `simplify`, etc. que vimos en la 2.1a).
- El skill `commit-style` lo crearemos primero **fuera del repo**, en `~/.claude/skills/commit-style/`. Y luego lo moveremos al repo en el bloque de promoción.

**Estado de la máquina Windows del formador:**

```
✅ Claude Code v2.1.x instalado y autenticado
✅ Git for Windows
✅ PowerShell 7
✅ VS Code con el repo cargado en demo/2.2c-before
✅ CLAUDE.md y .claude/settings.json operativos
✅ angular-component v4 funcional en .claude/skills/
✅ Carpeta ~/.claude/skills/ accesible para crear skill personal
```

**Lo que el alumno verá al final de la demo:**

- Skill `db-reset` creado con `disable-model-invocation: true` y prueba en directo de que **NO se activa** aunque la petición coincida con la descripción — solo con `/db-reset`.
- Skill `commit-style` creado en `~/.claude/skills/` (scope personal), probado, y luego movido a `.claude/skills/` (scope proyecto) demostrando la promoción.
- Las 5 reglas técnicas críticas repasadas con ejemplos de qué pasa si las violas.
- Los 8 errores frecuentes del primer día enumerados como checklist.
- Repaso final de los 3 skills coexistiendo en el repo.

---

## 6. Prompt para Claude Code

## 6a. Prompt para Claude Code — preparar `demo/2.2c-before`

> Crea la rama de partida del screencast desde `demo/2.2b-after`. **No crea ningún skill nuevo** — la pieza viva es construir `db-reset` y `commit-style` en pantalla. La rama `-before` queda idéntica a `demo/2.2b-after`.

````
Estoy preparando la demo 2.2c del curso de Claude Code (control de
invocación, scopes y cierre del bloque de creación de skills). Sigue
el patrón before/after (ver demo M0.2).

Quiero que prepares la rama `demo/2.2c-before` desde `demo/2.2b-after`.
Esta rama es el punto de partida del screencast: el repo NO debe tener
db-reset ni commit-style. Esos dos skills son la pieza viva.

## Tarea única

```powershell
git checkout demo/2.2b-after
git pull
git checkout -b demo/2.2c-before
```

NO crees ningún skill nuevo, NO toques ~/.claude/skills/ (eso se hace en vivo),
NO modifiques angular-component, NO marques nada en docs/DEMOS.md.
Esos artefactos van en `demo/2.2c-after` (ver §6b).

NO hagas commit. La rama `demo/2.2c-before` es exactamente igual a
`demo/2.2b-after` excepto en el nombre.

# Cuando termines, dime

1. Que la rama demo/2.2c-before está creada.
2. Que `git diff demo/2.2b-after demo/2.2c-before` no muestra cambios.
````

---

## 6b. Prompt para Claude Code — preparar `demo/2.2c-after`

> Materializa la rama final con los dos skills nuevos pre-cocinados — equivalentes a los que el formador construirá en directo. Pre-cocinar `-after` garantiza que la siguiente clase parte de un estado conocido aunque el directo se desvíe.

````
Estoy preparando la demo 2.2c del curso de Claude Code. Esta rama
-after pre-cocina los dos skills (db-reset y commit-style) que el
formador construirá en vivo durante el screencast.

# Contexto

Estoy en la rama `demo/2.2c-before` del repo `ordermanagement`. La rama
parte de `demo/2.2b-after` y tiene el skill angular-component v4
(SKILL.md + assets/ + scripts/generate.py) más OrderSummary y OrderFilter.
NO tiene db-reset ni commit-style.

Quiero que prepares la rama `demo/2.2c-after` desde `demo/2.2c-before`
con dos skills nuevos en `.claude/skills/`, respetando las reglas
técnicas críticas (kebab-case, sin XML en frontmatter, description bajo
1024 chars, sin prefijos `claude` ni `anthropic`, sin README.md dentro
del skill).

# Lo que necesito

Cuatro tareas:

## Tarea 1: crear la rama

```powershell
git checkout demo/2.2c-before
git checkout -b demo/2.2c-after
```

## Tarea 2: crear el skill destructivo `db-reset`

Crea `.claude/skills/db-reset/SKILL.md` con:

- Frontmatter:
  - `name: db-reset`
  - `description: Resetea la base de datos local de OrderManagement borrando todos los pedidos y clientes y dejando solo los datos seed.`
  - `disable-model-invocation: true` (este skill NO se autoinvoca; solo con `/db-reset` explícito)
- Cuerpo: instrucciones precisas para ejecutar `dotnet ef database drop --force` y luego `dotnet ef database update` desde `src/OrderManagement.Api`. Incluir aviso destacado de que es destructivo.

## Tarea 3: crear el skill `commit-style` (showcase de promoción personal → proyecto)

Crea `.claude/skills/commit-style/SKILL.md` con:

- Frontmatter:
  - `name: commit-style`
  - `description: Genera mensajes de commit Conventional Commits en español a partir del diff staged, siguiendo las convenciones del equipo OrderManagement.`
- Cuerpo: instrucciones para leer `git diff --cached`, decidir el tipo (feat/fix/refactor/docs/test/chore), elegir scope (api/application/domain/infrastructure/frontend/tests/docs), formatear el mensaje en español usando imperativo presente y sin punto final. Ejemplo de salida coherente con los commits del repo.

## Tarea 4: actualizar docs/DEMOS.md y skills-explorados.md + commit

Marca la 2.2c en `docs/DEMOS.md`:

```
- [x] **demo/2.2c** — Control, scopes y cierre del bloque de creación
```

Añade al final de `docs/skills-explorados.md` una sección «### Decisiones operativas (2.2c)» con tres bullets que resuman: (1) cuándo usar `disable-model-invocation`, (2) la regla personal→proyecto para promover skills, (3) las 5 reglas técnicas críticas.

Verifica con `dotnet build` (0 warnings, 0 errors) y commit:

```powershell
git add .claude/skills/db-reset .claude/skills/commit-style docs/DEMOS.md docs/skills-explorados.md
git commit -m "demo/2.2c-after: skills db-reset (disable-model-invocation) y commit-style (promovido a proyecto)"
```

NO hagas push.

# Restricciones (importantes)

- NO modifiques angular-component (viene intacto de 2.2b-after).
- NO modifiques CLAUDE.md ni .claude/settings.json.
- NO toques el código de la app.
- NO modifiques README.md ni .gitignore.
- Respeta las 5 reglas técnicas críticas. Si alguna te chocara, para y pregunta.

# Cuando termines, dime

1. Que la rama demo/2.2c-after está creada desde demo/2.2c-before.
2. Que existen los dos skills con sus SKILL.md y frontmatter correctos.
3. Que docs/DEMOS.md está marcado y skills-explorados.md ampliado.
4. Que dotnet build pasa.
5. Que el commit está hecho.

Si tienes dudas, para y pregúntame.
````

---

## 7. Artefactos que Claude Code debe generar

```
✓ Rama demo/2.2c-before (parte de demo/2.2b-after) — sin cambios respecto al parent
✓ Rama demo/2.2c-after (parte de demo/2.2c-before) con:
  ├── .claude/skills/db-reset/SKILL.md (nuevo, disable-model-invocation: true)
  ├── .claude/skills/commit-style/SKILL.md (nuevo, promovido a project)
  ├── docs/DEMOS.md con 2.2c marcada como [x]
  └── docs/skills-explorados.md ampliado con decisiones operativas
✓ Verificación de build OK: dotnet build limpio
✓ Commit en demo/2.2c-after: "demo/2.2c-after: skills db-reset y commit-style + notas operativas"
```

**Lo que NO debe haber generado:**

- ❌ Skills nuevos (`db-reset`, `commit-style`) — se crean EN VIVO
- ❌ Modificaciones al skill `angular-component` (viene de 2.2b)
- ❌ Cambios en código de la app
- ❌ Cambios en CLAUDE.md o `.claude/settings.json`

> Si Claude Code se anticipa y crea los skills, **se rechaza el output**. La construcción de los dos skills en vivo es el corazón pedagógico de esta demo.

**Lo que el formador commitea EN VIVO sobre `demo/2.2c-before` durante el screencast:**

```
Durante la grabación, sobre demo/2.2c-before, se hacen commits ficticios:
- "demo/2.2c-after: skill db-reset con disable-model-invocation"
  └── .claude/skills/db-reset/SKILL.md (NUEVO, ~30 líneas)
- "demo/2.2c-after: skill commit-style promovido de personal a proyecto"
  └── .claude/skills/commit-style/SKILL.md (NUEVO, ~50 líneas)
  └── docs/skills-explorados.md (actualizado con notas de la 2.2c)
```

**Estado final del árbol después del screencast (no del prompt):**

```
ordermanagement/
├── .claude/
│   ├── settings.json
│   └── skills/
│       ├── angular-component/                  (de 2.2b)
│       │   ├── SKILL.md
│       │   ├── assets/...
│       │   └── scripts/...
│       ├── db-reset/                           ← NUEVO (en vivo)
│       │   └── SKILL.md
│       └── commit-style/                       ← NUEVO (en vivo, promovido de personal)
│           └── SKILL.md
├── docs/
│   ├── DEMOS.md                                ← MODIFICADO (pre-grabación)
│   └── skills-explorados.md                    ← MODIFICADO (en vivo)
├── frontend/...                                (sin cambios)
└── ... (resto sin cambios)
```

---

## 8. Guion del screencast

**Duración estimada del screencast: ~21-23 minutos.**

Diez bloques. Esta demo cierra el bloque de creación con dos showcases concretos y un repaso operativo.

> **Antes de empezar a grabar**, asegúrate de:
> - Tener PowerShell 7 abierto en pantalla completa, fuente al menos 16pt.
> - Tener VS Code abierto con el repo en `demo/2.2c-before`.
> - Verificar que `~/.claude/skills/` no tiene un skill `commit-style` previo (si lo tienes de pruebas, bórralo).
> - Tener Claude Code listo para arrancar.
> - Cerrar Slack, Teams, navegadores con notificaciones.

---

### Bloque 1 — Setup y planteamiento del cierre (~1 min 30 seg)

**Pantalla compartida.** A la izquierda VS Code con el repo en `demo/2.2c-before`. A la derecha terminal PowerShell.

**En la terminal:**

```powershell
git status
ls .claude\skills\
```

```
On branch demo/2.2c
nothing to commit, working tree clean

    Directorio: C:\Users\pedro\projects\ordermanagement\.claude\skills

Mode    LastWriteTime    Length Name
----    -------------    ------ ----
d----   ...                     angular-component
```

**Lo que dices:**

> "Estamos en la rama `demo/2.2c-before`. Tenemos un solo skill en el repo: `angular-component` v4, completo, con `assets/` y `scripts/`. Lo construimos en la 2.2a y 2.2b.
>
> Esta demo **cierra el bloque de creación de skills**. No metemos pieza nueva grande — repasamos las **decisiones operativas** que la gamma 2.2c marcó como cierre. Cinco bloques rápidos:
>
> Una. **Control de invocación** — cuándo el modelo NO debería activar el skill solo. Vamos a hacerlo en directo creando un skill destructivo, `db-reset`, con la flag `disable-model-invocation: true`.
>
> Dos. **Scopes** — qué skills viajan contigo (`~/.claude/skills/`) y cuáles van con el equipo (`.claude/skills/`). Lo demostramos con un skill `commit-style` que arranca en personal y se promueve a proyecto.
>
> Tres. **Reglas técnicas críticas** — las cinco cosas que si te las saltas, el skill no funciona. Repaso rápido.
>
> Cuatro. **Errores frecuentes del primer día** — los ocho anti-patrones que casi todo el mundo comete. Checklist.
>
> Cinco. **Cierre del 2.2** — repaso de lo construido.
>
> Vamos."

**Tiempo:** ~90 segundos.

---

### Bloque 2 — Crear skill destructivo `db-reset` con `disable-model-invocation` (~3 min)

> "Empezamos con el caso de control de invocación más claro: **un skill destructivo**. La gamma 2.2c slide 5 lo marcó: skills que borran, hacen deploy, push forzado. Cosas que **no quieres que pasen nunca por accidente**."

**En PowerShell:**

```powershell
mkdir .claude\skills\db-reset
```

**En VS Code, creo `.claude/skills/db-reset/SKILL.md`:**

```markdown
---
name: db-reset
description: Resetea la base de datos local de OrderManagement borrando todos los registros y reaplicando migraciones desde cero. Usar SOLO cuando el usuario pida explícitamente resetear o limpiar la BBDD local.
disable-model-invocation: true
allowed-tools: Bash(dotnet ef *), Bash(dotnet run *)
---

# Reset de la BBDD local de OrderManagement

## Cuándo se usa este skill

**SOLO se invoca explícitamente con `/db-reset`.** Tiene
`disable-model-invocation: true` precisamente para que no se active
por accidente.

Casos legítimos de uso:

- El estado in-memory está corrupto y hay que empezar de cero.
- Hay un cambio de schema y las migraciones nuevas requieren BBDD limpia.
- Antes de una demo, queremos partir de cero conocido.

## Pasos al ejecutar

1. Confirma con el usuario antes de proceder. Pregunta:
   *"Voy a resetear la BBDD local. ¿Confirmas? (sí/no)"*
   Si no responde sí explícito, abortar.

2. Drop de la BBDD existente:

   ```!
   dotnet ef database drop --force --project src/OrderManagement.Infrastructure
   ```

3. Aplica todas las migraciones:

   ```!
   dotnet ef database update --project src/OrderManagement.Infrastructure
   ```

4. Reporta al usuario el resultado.

## Lo que NO hace este skill

- No toca BBDD de producción ni staging. Si el usuario lo pide, abortar
  con mensaje claro.
- No genera datos de prueba. Para eso, usar el skill `seed-data` (no
  existe todavía, pero es candidato).
```

**Salvo el fichero. Vuelvo al guion:**

> "Mirad lo que metí.
>
> **`disable-model-invocation: true`**. La flag de la gamma 2.2c slide 4. Esto le dice a Claude: *'aunque la descripción coincida con lo que pide el usuario, NO actives este skill por tu cuenta'*. **Solo se ejecuta si el usuario escribe `/db-reset` explícitamente**.
>
> **`allowed-tools` restringido**. Solo permite `Bash(dotnet ef *)` y `Bash(dotnet run *)`. **No puede borrar ficheros ni ejecutar otros comandos**. Aunque la sesión tenga permisos amplios, este skill está más limitado.
>
> **Descripción con SOLO en mayúsculas**. *'Usar SOLO cuando el usuario pida explícitamente'*. Refuerzo verbal aunque la flag ya hace el trabajo técnico.
>
> **Vamos a probar que la flag funciona**."

**Tiempo:** ~3 minutos.

---

### Bloque 3 — Probar que `disable-model-invocation` funciona (~2 min 30 seg)

**Arranco Claude Code:**

```powershell
claude
```

```
✓ 2 project skills loaded: angular-component, db-reset
```

> "Mirad. **Dos skills cargados ahora**: `angular-component` y `db-reset`. La flag `disable-model-invocation` no impide que se cargue — impide que se **active automáticamente**.
>
> Le hago una petición que **debería activar** el skill `db-reset` si no tuviera la flag:"

**Tecleo:**

```
> Resetea la base de datos local de OrderManagement.
```

**Aparece la respuesta — atención:**

```
Para resetear la BBDD local de OrderManagement, los pasos serían:

1. dotnet ef database drop --force --project src/OrderManagement.Infrastructure
2. dotnet ef database update --project src/OrderManagement.Infrastructure

¿Quieres que ejecute estos comandos? Veo que tienes un skill `db-reset`
con disable-model-invocation: true que cubre exactamente este caso, pero
está marcado para invocación explícita solamente. Si quieres usarlo, 
escribe /db-reset y se ejecutará con confirmación.
```

> "**Mirad esto.** El agente **no ha activado el skill** aunque la petición coincide perfectamente con su descripción. Y mejor aún: **me dice que existe el skill** y me sugiere invocarlo explícitamente con `/db-reset`. Eso es exactamente el comportamiento que queríamos.
>
> **Sin la flag**, el agente habría ejecutado los `dotnet ef` directamente — y si me equivoco al pedirlo, BBDD borrada. **Con la flag**, hay un paso de confirmación obligatorio.
>
> Vamos a invocarlo explícitamente:"

**Tecleo:**

```
> /db-reset
```

**Aparece:**

```
● [Skill activated explicitly: db-reset]

Voy a resetear la BBDD local. ¿Confirmas? (sí/no)
```

> "Y aquí está. **Solo cuando lo invoco con slash, se activa**. Y el primer paso del SKILL.md me hace confirmar antes de actuar. **Doble seguridad**: la flag impide activación automática, y la primera línea del workflow exige confirmación humana.
>
> Le digo no para no resetear nada en esta demo:"

**Tecleo:**

```
> no
```

```
Operación cancelada. La BBDD local no se ha tocado.
```

> "Limpio. **`disable-model-invocation` validado**. Salgo."

**Salgo (Ctrl+C):**

**Tiempo:** ~2 minutos 30 segundos.

---

### Bloque 4 — Crear skill `commit-style` en scope personal (~3 min)

> "Segundo showcase: **scopes**. Voy a crear un skill `commit-style` que escribe mensajes de commit en el estilo del equipo. **Este skill empieza en mi `~/.claude/skills/`** — scope personal — porque inicialmente quiero experimentar sin afectar al equipo."

**En PowerShell:**

```powershell
mkdir $env:USERPROFILE\.claude\skills\commit-style
```

> "Mirad la ruta: `$env:USERPROFILE\.claude\skills\` que en mi máquina es `C:\Users\pedro\.claude\skills\`. **Esto NO está en el repo**. Es **mi carpeta personal**. Va conmigo de proyecto en proyecto."

**En VS Code, abro `C:\Users\pedro\.claude\skills\commit-style\` directamente (no desde el repo) y creo `SKILL.md`:**

```markdown
---
name: commit-style
description: Genera mensajes de commit en el estilo del equipo OrderManagement. Usar cuando el usuario pida escribir, generar o sugerir un mensaje de commit para los cambios actuales del repositorio.
---

# Estilo de mensajes de commit

## Cuándo se usa este skill

Cuando el usuario pida escribir, generar o sugerir un mensaje de commit
para los cambios actuales. Esto incluye peticiones como:

- "escribe el mensaje de commit"
- "qué pondrías de commit"
- "genera el commit message"
- "sugiéreme un commit"

## Formato del mensaje

Una sola línea, en imperativo, en español, sin punto final, máximo 72
caracteres.

Estructura: `<tipo>: <descripción concisa>`

Tipos válidos:

- `feat` — nueva funcionalidad
- `fix` — corrección de bug
- `refactor` — cambio que no añade ni quita funcionalidad
- `docs` — solo documentación
- `test` — añadir o modificar tests
- `chore` — cambios de mantenimiento, dependencias, build

## Ejemplos

```
feat: añade endpoint POST /api/orders/{id}/cancel
fix: corrige cálculo de itemCount en OrderSummaryComponent
refactor: extrae handler de cancelación a su propia clase
docs: actualiza README con instrucciones de Windows
test: añade tests para CancelOrderHandler
chore: actualiza Angular 19.0.0 a 19.1.2
```

## Pasos al generar

1. Lee el `git status` y `git diff --cached` para entender los cambios.
2. Identifica el tipo principal del cambio (feat, fix, refactor, etc.).
3. Genera la descripción concisa en imperativo.
4. Verifica que la línea entera está bajo 72 caracteres.
5. Devuelve el mensaje al usuario para que lo use o lo ajuste.

NO hagas commit por tu cuenta. Solo sugiere el mensaje.
```

**Salvo. Vuelvo al guion:**

> "Mirad lo que tiene. **Frontmatter mínimo** — `name` y `description`. Sin `disable-model-invocation` (no es destructivo). Cuerpo con el formato del equipo y ejemplos.
>
> **Pero atentos a la ubicación**: `~/.claude/skills/commit-style/`. **No está en el repo.** Si compañeros del equipo clonaran el repo, **no verían este skill**. Es solo mío.
>
> ¿Por qué empezar aquí? Porque la gamma slide 16 lo dice: **personal → proyecto cuando se valida**. **Quiero probar el skill un par de veces antes de decidir si vale la pena meterlo en el repo del equipo**.
>
> Vamos a probarlo."

**Tiempo:** ~3 minutos.

---

### Bloque 5 — Probar el skill personal y decidir promocionarlo (~2 min 30 seg)

**Hago un cambio menor en el repo para tener algo que commitear:**

**En VS Code, edito `docs/skills-explorados.md` y añado al final:**

```markdown

---

# Demo 2.2c — Notas

(Pendiente de rellenar tras el screencast.)
```

**Lo stageo:**

```powershell
git add docs/skills-explorados.md
```

**Arranco Claude Code:**

```powershell
claude
```

```
✓ 2 project skills loaded: angular-component, db-reset
✓ 1 user skill loaded: commit-style
```

> "Mirad. **Dos project skills + un user skill**. El user skill es el `commit-style` que vive en mi carpeta personal. **Aunque NO está en el repo, está cargado** porque es mío.
>
> Le pido el commit:"

**Tecleo:**

```
> Escribe el mensaje de commit para los cambios staged.
```

**Aparece:**

```
● [Skill activated: commit-style]
● Bash(git status)
● Bash(git diff --cached)

Cambios detectados: docs/skills-explorados.md modificado, añadiendo
sección "Demo 2.2c — Notas" pendiente de rellenar.

Tipo: docs (solo documentación)

Mensaje propuesto (62 caracteres, bajo el límite de 72):

  docs: añade placeholder para notas de la demo 2.2c

¿Quieres que ejecute git commit con este mensaje, o lo ajustas?
```

> "**Skill activado**. Ha leído `git status` y `git diff --cached`. Ha identificado el tipo `docs`. Ha generado el mensaje en imperativo, 62 caracteres, bajo el límite. **Y respeta el último paso del SKILL.md: NO commitea por su cuenta, me pregunta**.
>
> Vale, **el skill funciona bien**. Lo he probado. Es útil. **¿Sería útil para el equipo?** Sí — todo el equipo se beneficia de mensajes de commit consistentes. **Toca promoverlo.**"

**Salgo (Ctrl+C):**

**Tiempo:** ~2 minutos 30 segundos.

---

### Bloque 6 — Promocionar el skill personal a scope proyecto (~2 min)

> "Promoción es **literalmente mover el directorio** del scope personal al scope proyecto. Una operación de filesystem."

**En PowerShell:**

```powershell
# Copiar el SKILL.md desde personal a proyecto
mkdir .claude\skills\commit-style
Copy-Item $env:USERPROFILE\.claude\skills\commit-style\SKILL.md .claude\skills\commit-style\SKILL.md
```

**Verifico:**

```powershell
ls .claude\skills\
```

```
Mode    LastWriteTime    Length Name
----    -------------    ------ ----
d----   ...                     angular-component
d----   ...                     commit-style          ← nuevo
d----   ...                     db-reset
```

> "Tres skills en el repo. El skill `commit-style` ya está en `.claude/skills/`. **Va a ir a git con el siguiente commit**. Cualquier compañero que clone, lo tendrá.
>
> ¿Y el de mi carpeta personal? **Lo borro**. Si lo dejara, tendría dos skills con el mismo nombre — uno en personal, uno en proyecto. La gamma 2.2c slide 19 lo marcó como anti-patrón: *'mismo skill en proyecto y user, hay conflicto'*."

**Borro el personal:**

```powershell
Remove-Item -Recurse -Force $env:USERPROFILE\.claude\skills\commit-style
```

**Verifico que el skill sigue funcionando, ahora desde scope proyecto:**

```powershell
claude
```

```
✓ 3 project skills loaded: angular-component, commit-style, db-reset
```

> "Mirad. **Tres project skills cargados ahora**. `commit-style` ya no aparece como user skill — aparece como project. **La promoción está completa**.
>
> Y ahora **cualquier dev del equipo que clone el repo** tendrá los tres skills disponibles desde el segundo cero. **Eso es lo que queríamos**."

**Salgo (Ctrl+C):**

**Tiempo:** ~2 minutos.

---

### Bloque 7 — Las 5 reglas técnicas críticas (~3 min)

> "Antes de cerrar el bloque de creación, **un repaso obligatorio**: las reglas técnicas críticas. La gamma 2.2c y el manual 2.1 v3 las marcan como **no negociables**. Si te las saltas, el skill **no funciona**.
>
> Voy a abrir el `SKILL.md` que acabamos de crear y enseñarlas en pantalla aplicadas."

**Abro `.claude/skills/commit-style/SKILL.md` en VS Code:**

> "Vamos por las cinco."

**En el editor o terminal al lado, escribo (es contenido pedagógico):**

```
REGLA 1 — SKILL.md case-sensitive
─────────────────────────────────
✅ SKILL.md
❌ skill.md
❌ Skill.md
❌ SKILL.MD
❌ skills.md

→ Si escribes "Skill.md", Claude no lo reconoce. Skill invisible.
```

> "**Regla 1**. El nombre del fichero **es exactamente `SKILL.md`**. Mayúsculas las cuatro letras del nombre. La extensión `.md` minúscula. **Nada más sirve**. Más de un dev pasa una hora 'depurando' por qué el skill no se activa cuando el problema es una `s` minúscula.

```
REGLA 2 — Carpeta en kebab-case = name del frontmatter
──────────────────────────────────────────────────────
✅ angular-component  (carpeta)
   name: angular-component  (frontmatter)

❌ AngularComponent  (carpeta)  — capitales
❌ angular_component  (carpeta) — guion bajo
❌ angular component  (carpeta) — espacio
❌ Carpeta: angular-component, name: AngularComponent — inconsistencia
```

> "**Regla 2**. Carpeta en kebab-case. Minúsculas, palabras separadas por guiones. **Sin guiones bajos. Sin espacios. Sin mayúsculas**. Y **el `name` del frontmatter coincide con la carpeta**. Si la carpeta es `angular-component`, el name también. Si discrepan, comportamiento impredecible."

```
REGLA 3 — Nombres reservados: nada de "claude" ni "anthropic"
─────────────────────────────────────────────────────────────
❌ claude-helper   (no se carga)
❌ anthropic-tools (no se carga)
✅ team-claude-helper   (claude no como prefijo)
✅ mi-anthropic-utils   (idem)
```

> "**Regla 3**. **Nada de empezar el name con `claude` ni con `anthropic`**. Reservados. Si lo intentas, el skill **directamente no se carga**. Puedes mencionarlos en medio del nombre, pero no como prefijo."

```
REGLA 4 — Sin XML en el frontmatter
─────────────────────────────────────
❌ description: "Procesa <input> y devuelve <output>"
✅ description: "Procesa la entrada y devuelve la respuesta"
```

> "**Regla 4**. **Sin etiquetas XML en el frontmatter**. Razón de seguridad: el frontmatter se inyecta literal en el system prompt del modelo, y permitir XML abriría inyecciones de instrucciones disimuladas. Si tu skill habla de XML, **eso va al cuerpo del SKILL.md, no al frontmatter**."

```
REGLA 5 — description bajo 1024 caracteres
──────────────────────────────────────────
✅ Descripción típica bien escrita: 200-400 caracteres
✅ Descripción rica con varios triggers: 500-800 caracteres
❌ Si pasa de 1024: el skill no se carga

Si tu descripción pasa de 800, probablemente
tienes contenido del cuerpo metido en la descripción.
```

> "**Regla 5**. **Description bajo 1024 caracteres**. La mayoría caben en 200-400. Si la tuya pasa de 800, casi seguro tienes contenido del cuerpo metido ahí. **Refactoriza**."

> "Y una **bonus**: **NO metas `README.md` dentro de la carpeta del skill**. Si publicas el skill como repo en GitHub, el README va **fuera** de la carpeta del skill, a nivel de repo. Dentro de la carpeta del skill, **solo `SKILL.md` y opcionalmente `assets/`, `references/`, `scripts/`**.
>
> **Estas reglas son la fuente número uno** de *'escribí el skill, parece correcto, pero no se activa nunca'*. Conviene tenerlas en la cabeza desde el primer skill."

**Tiempo:** ~3 minutos.

---

### Bloque 8 — Los 8 errores frecuentes del primer día (~2 min 30 seg)

> "Y los **errores frecuentes** que la gamma 2.2c slides 17-19 enumeró. **Checklist** para repasar antes de dar por bueno tu primer skill."

**En el editor, escribo (contenido pedagógico):**

```
LOS 8 ANTI-PATRONES DEL PRIMER DÍA

1. ❌ Skill demasiado grande
   "Un skill que hace todo lo de generación de componentes,
    páginas, módulos, services..."
   → Mejor varios skills pequeños y especializados.

2. ❌ Empezar por la versión 4
   No hace falta meter scripts y plantillas el primer día.
   → Empieza simple. Añade capas según se justifiquen.

3. ❌ No iterar la descripción
   La primera descripción casi nunca es la final.
   → Lánzala, ve cuándo se activa y cuándo no, ajusta.

4. ❌ No testar después de cambios
   Tras modificar un skill, prueba en sesión nueva.
   → Es fácil romper la activación al refinar.

5. ❌ Convenciones que duplican lo que ya hace Claude
   Si Claude sin skill ya genera bien, un skill genérico no aporta.
   → El valor está en codificar las particularidades del equipo.

6. ❌ Mezclar skills con CLAUDE.md
   Si aplica a TODAS las tareas del repo → CLAUDE.md.
   → Skills son para tareas concretas.

7. ❌ No documentar por qué se hacen las cosas
   Otro dev (o tú dentro de seis meses) querrá saber el motivo.
   → Comentario corto justificando decisiones no obvias.

8. ❌ Mezclar lógica determinista con razonamiento del modelo
   Lo que un script hace bien, no lo razone el modelo.
   Y al revés.
```

> "Ocho. **Si vuestros primeros skills evitan estos ocho, ya estáis por encima de la media**. Los más comunes son el primero (skill que intenta hacer demasiado) y el sexto (poner en skill cosas que son de CLAUDE.md). Los dos van juntos: si pones todo en un skill 'porque es del equipo', terminas teniendo un skill enorme que es realmente CLAUDE.md mal colocado.
>
> La regla mnemotécnica para el sexto: **¿aplica a todas las tareas del repo? CLAUDE.md. ¿Solo a tareas concretas? Skill.**"

**Tiempo:** ~2 minutos 30 segundos.

---

### Bloque 9 — Commit y notas en `docs/skills-explorados.md` (~1 min 30 seg)

> "Vamos a commitear lo de hoy. **Tres skills coexistiendo en el repo**."

**En VS Code, completo la sección "Demo 2.2c — Notas" en `docs/skills-explorados.md`:**

```markdown
# Demo 2.2c — Notas

## Skills añadidos en esta demo

### `db-reset` (destructivo, solo invocable explícitamente)

- Frontmatter con `disable-model-invocation: true`.
- Solo se activa con `/db-reset`, nunca por activación automática.
- Demostrado: con la flag, el agente NO ejecuta los `dotnet ef database
  drop` aunque la petición ("resetea la BBDD local") coincida con la
  descripción. Sin la flag, lo habría hecho.

### `commit-style` (promovido de personal a proyecto)

- Creado primero en `~/.claude/skills/commit-style/` (scope personal).
- Probado generando un commit message para un cambio en
  docs/skills-explorados.md.
- Validado que funciona: imperativo, español, bajo 72 caracteres.
- Movido a `.claude/skills/commit-style/` (scope proyecto) para que
  vaya a git y lo use todo el equipo.
- El de personal se borró para evitar conflicto.

## Reglas técnicas críticas (recordatorio)

1. SKILL.md case-sensitive (no skill.md, no Skill.md).
2. Carpeta en kebab-case = `name` del frontmatter.
3. `name` no empieza por `claude` ni `anthropic`.
4. Sin XML en el frontmatter.
5. `description` bajo 1024 caracteres.
6. Bonus: no `README.md` dentro de la carpeta del skill.

## Estado de los skills del repo

```
.claude/skills/
├── angular-component/    (v4 con assets/ y scripts/)
├── commit-style/         (mensajes de commit del equipo)
└── db-reset/             (destructivo, solo /db-reset)
```

Los tres skills coexisten sin conflicto. `angular-component` y
`commit-style` activan automáticamente. `db-reset` solo con slash.
```

**Salvo. En la terminal:**

```powershell
git add .claude/skills/ docs/skills-explorados.md
git commit -m "demo/2.2c-after: skills db-reset y commit-style + notas operativas"
```

```
[demo/2.2c-before xyz9876] demo/2.2c-after: skills db-reset y commit-style + notas operativas
 3 files changed, 110 insertions(+)
 create mode 100644 .claude/skills/commit-style/SKILL.md
 create mode 100644 .claude/skills/db-reset/SKILL.md
```

> "Commit hecho. **Tres ficheros nuevos**. La rama queda con tres skills funcionales.

**Tiempo:** ~1 minuto 30 segundos.

---

### Bloque 10 — Cierre del 2.2 y cliffhanger a 2.3 (~1 min 30 seg)

> "Y eso es la 2.2c. **El bloque de creación de skills (todo el 2.2) cerrado**. Tres pasos en lo que se llevan los alumnos:"

**En el editor de texto al lado:**

```
LO QUE TIENES TRAS EL 2.2

✅ Tres skills funcionales en el repo:
   - angular-component (v4: SKILL + assets + scripts)
   - commit-style (mensajes del equipo, promovido de personal)
   - db-reset (destructivo, solo /db-reset)

✅ Modelo mental completo:
   - Cuándo activación automática y cuándo /slash explícito
   - Cuándo personal y cuándo proyecto
   - 5 reglas técnicas críticas
   - 8 anti-patrones del primer día

✅ Pregunta para casa:
   ¿Qué patrón de tu equipo va a ser tu primer skill propio
   cuando vuelvas el lunes?
```

> "**Pregunta importante** que la gamma 2.2c slide 23 dejó: ¿qué patrón de vuestro equipo va a ser **vuestro primer skill propio** cuando volváis el lunes? Pensadlo durante la pausa.
>
> En la siguiente demo, **2.3**, salimos del taller individual y miramos al **ecosistema**. Hay muchos skills ya escritos por Anthropic y por la comunidad. Vamos a ver `npx skills add` para instalar skills oficiales en un click. Vamos a ver dónde encontrar skills de la comunidad y **cómo distinguir los que merecen la pena instalar de los que mejor dejar fuera**. Y vamos a ver el lado oscuro: el estudio de Snyk sobre **ToxicSkills** — los riesgos reales de instalar skills de terceros sin auditar.
>
> Empezamos con el **dos punto tres**."

**Tiempo:** ~1 minuto 30 segundos.

---

## 9. Qué resaltar verbalmente

Cinco puntos que **no pueden quedarse sin decir**:

1. **"`disable-model-invocation: true` para destructivos, caros, experimentales. Punto."** — la regla de cuándo aplicar la flag. Bloque 2, repetida en bloque 3.

2. **"Personal `~/.claude/skills/` para experimentar. Proyecto `.claude/skills/` cuando funciona y vale para el equipo."** — la regla de promoción. Bloque 4-6.

3. **"Las 5 reglas técnicas críticas: case-sensitive, kebab-case, no `claude`/`anthropic`, no XML, bajo 1024 chars."** — el alumno debe poder repetirlas. Bloque 7.

4. **"Si tu skill aplica a TODAS las tareas del repo, no es skill — es CLAUDE.md."** — el anti-patrón más común. Bloque 8 (anti-patrón #6).

5. **"Tres skills coexisten en el repo. Modelo operativo completo."** — el take-away final. Bloque 10.

**Frase de remate al final:**

> *"Cinco demos del módulo 2 detrás. Tenéis el taller. Lo que viene es el ecosistema."*

---

## 10. Slide de entrada (locución HeyGen)

> **Texto del avatar antes del screencast:**

"Y vamos a la siguiente demo. La 2.2c. Cierra el bloque de creación de skills cubriendo lo que falta: control de invocación, scopes, y reglas técnicas críticas. Vais a ver dos showcases en directo. Primero, un skill destructivo `db-reset` con la flag `disable-model-invocation` que impide la activación automática — solo se ejecuta con `/db-reset` explícito. Y demostraremos en vivo que la flag funciona: aunque le pida 'resetea la BBDD', el agente no actúa, me sugiere usar el slash. Segundo, la promoción de un skill personal a proyecto. Crearemos `commit-style` en `~/.claude/skills/` primero, lo probaremos, y al ver que funciona lo moveremos a `.claude/skills/` del repo para todo el equipo. Y al final, repaso de las cinco reglas técnicas críticas que no son negociables y los ocho anti-patrones del primer día. Vamos al screencast."

---

## 11. Slide de salida (locución HeyGen)

> **Texto del avatar al volver del screencast:**

"Lo que acabáis de ver cierra el bloque de creación. Tres skills coexisten en el repo: `angular-component` con sus assets y scripts, `commit-style` promovido de personal a proyecto, y `db-reset` con `disable-model-invocation` que solo se ejecuta con slash explícito. Tres tipos de skill, tres patrones de uso. Cinco reglas técnicas que no podéis saltaros: case-sensitive de SKILL.md, kebab-case en la carpeta, prohibido `claude` y `anthropic` como prefijos, sin XML en frontmatter, descripción bajo mil veinticuatro caracteres. Ocho anti-patrones del primer día como checklist antes de dar por bueno cualquier skill nuevo. La parte de aprender se acaba aquí. La parte de practicar empieza el lunes. Pero antes, en la siguiente demo, la 2.3, salimos del taller individual y miramos al ecosistema. `npx skills add` para skills oficiales. Skills de la comunidad: dónde encontrarlos y cómo distinguir los buenos. Y el lado oscuro — el estudio Snyk sobre ToxicSkills, los riesgos reales de instalar skills de terceros sin auditar. Empezamos con el dos punto tres."

---

## 12. Tiempo total estimado

| Bloque | Tiempo |
|---|---|
| Slide de entrada (avatar) | ~30 seg |
| Bloque 1 — Setup y planteamiento | ~1 min 30 seg |
| Bloque 2 — Crear `db-reset` con `disable-model-invocation` | ~3 min |
| Bloque 3 — Probar que la flag funciona | ~2 min 30 seg |
| Bloque 4 — Crear `commit-style` en personal | ~3 min |
| Bloque 5 — Probar y decidir promocionarlo | ~2 min 30 seg |
| Bloque 6 — Promocionar a scope proyecto | ~2 min |
| Bloque 7 — Las 5 reglas técnicas críticas | ~3 min |
| Bloque 8 — Los 8 errores frecuentes | ~2 min 30 seg |
| Bloque 9 — Commit y notas | ~1 min 30 seg |
| Bloque 10 — Cierre del 2.2 y cliffhanger | ~1 min 30 seg |
| Slide de salida (avatar) | ~30 seg |
| **Total screencast** | **~22-24 min** |
| **Total con avatar** | **~23-25 min** |

> Si hay preguntas durante el screencast, súmale 3-4 minutos. La demo encaja en un bloque de **30 minutos** dentro de la sesión.

**Margen de seguridad por si algo va lento:**

- **Si la flag `disable-model-invocation` no funciona como esperamos** (porque alguna versión de Claude Code la trata distinto), comenta: *"a veces la flag tarda un par de intentos en manifestarse claramente. Lo importante es el comportamiento de fondo: skill cargado pero no activado por el modelo, solo por slash explícito"*. Y muestra el slash funcionando, que es lo crítico.

- **Si `~/.claude/skills/` tiene problemas de permisos en Windows** al crear el skill personal, prueba ejecutar PowerShell como admin la primera vez. Después no hace falta. Si surge el problema, comenta: *"a veces Windows pide permisos elevados la primera vez. Una vez creada la carpeta, no vuelve a pedirlos"*.

- **Si Claude Code no reporta el skill como "user skill loaded"** sino como "project skill loaded" (porque hay configuración rara), no te pares en distinguirlo en pantalla. Comenta el comportamiento general: *"el skill carga, está disponible, eso es lo importante. La distinción interna user vs project es de Claude Code, lo importante es la ubicación física del fichero"*.

- **Si te quedas sin tiempo**, recorta el bloque 8 (anti-patrones) a 1 minuto: enuncias los 8 rápidamente sin desarrollar cada uno. La gamma ya los cubrió en detalle. La demo es recordatorio.

- **Si surgen preguntas sobre el plugin scope** del bloque 1, contesta corto: *"plugin es para distribución empaquetada, lo veremos en la 2.3 con `npx skills add` y bundling"*. No te metas en detalles, invade la 2.3.

---

# Apéndice A — Decisiones pedagógicas justificadas

> Esta sección no se graba. Es para Pedro, formador.

**¿Por qué dos showcases (`db-reset` y `commit-style`) y no uno?**

Porque cubren **dos conceptos distintos** que la gamma 2.2c trata por separado:
- `db-reset` demuestra **control de invocación** (flag, slash explícito).
- `commit-style` demuestra **scopes y promoción** (personal → proyecto).

Forzarlos en un solo skill diluiría ambos conceptos. Dos showcases pequeños son más claros que uno grande con dos preocupaciones.

**¿Por qué `db-reset` y no otro skill destructivo?**

Por dos razones. Una: la gamma 2.2c slide 4 lo usa **literalmente** como ejemplo de `disable-model-invocation`. Mantener consistencia con el material del curso reduce confusión. Dos: es **fácil de razonar** — todos los devs entienden inmediatamente por qué resetear una BBDD es algo que no quieres que pase por accidente.

**¿Por qué probar la flag con una petición que SÍ debería activar el skill?**

Porque demostrar que la flag funciona requiere un caso donde **sin la flag** sí se activaría. Si pidiera algo no relacionado, el skill no se activaría con o sin flag, y no se vería la diferencia. **El test crítico es: petición que coincide → ¿activa? → no, gracias a la flag**.

**¿Por qué el agente "menciona" el skill aunque no lo activa?**

Porque es lo que Claude Code hace en la práctica con `disable-model-invocation: true`. **No oculta** la existencia del skill — informa al usuario de que existe y de cómo invocarlo. Esto es **muy útil pedagógicamente** porque el alumno ve no solo que la flag funciona, sino **el comportamiento exacto del agente**: respeto a la flag + transparencia con el usuario.

**¿Por qué `commit-style` y no otro skill personal?**

Por tres razones:
1. **Es universal**: cualquier dev escribe commits. No requiere conocimiento de Angular ni .NET.
2. **Tiene valor real para el equipo**: justifica la promoción a proyecto.
3. **El manual lo menciona literalmente** (manual 2.1 línea 13): *"un tercer skill, quizá el más útil, para escribir mensajes de commit siguiendo la convención del equipo"*. Coherencia con el material.

**¿Por qué empezar `commit-style` en personal y promoverlo, en lugar de crearlo directo en proyecto?**

Porque la gamma 2.2c slide 16 marca **personal → proyecto** como **el patrón sano**. Si lo creo directo en proyecto, el alumno no ve el flujo. **Crear en personal, probar, decidir, mover** es la pedagogía completa.

**¿Por qué borrar el de personal después de mover a proyecto?**

Porque la gamma 2.2c y el manual marcan como **anti-patrón** tener el mismo skill en ambos scopes. *"Mismo skill en proyecto y user, hay conflicto"*. Si lo dejara, estaría enseñando el anti-patrón **inmediatamente después de mostrar la promoción correcta**. Disonancia. **Borrar el personal** es parte de la promoción bien hecha.

**¿Por qué las 5 reglas técnicas críticas en formato de bloques de texto, no en código YAML?**

Porque **son reglas de naming, no de sintaxis**. Mostrarlas con ejemplos de qué SÍ y qué NO en bloques `✅` y `❌` es más claro visualmente que insertarlas dentro de un YAML. Y permite cubrirlas todas en pocos minutos sin teclear código.

**¿Por qué el bloque 8 (anti-patrones) es de solo lectura, no acción?**

Porque la gamma 2.2c ya los enumeró en sus slides 17-19. **Repetirlos como acción en directo** sería pedante — el alumno los ha visto ya. **Como checklist visual rápido** sirven de recordatorio sin extender la demo. Es **densidad de contenido apropiada**.

**¿Por qué la pregunta para casa al final?**

Porque la gamma 2.2c slide 23 la dejó como deberes para casa. Recogerla en el cierre **conecta el módulo entero con el lunes del alumno**. Sin esta conexión, el alumno termina pensando *"interesante, pero no sé qué hacer con esto"*. Con la pregunta, se va con una **tarea concreta** identificada.

**¿Por qué el cliffhanger menciona Snyk y ToxicSkills?**

Porque la gamma 2.3 los cubre en sus últimos slides como **el lado oscuro del ecosistema**. Anticipar el contenido de la 2.3 con un nombre concreto (Snyk, ToxicSkills) genera curiosidad. **Es un teaser deliberado** para que el alumno llegue a la 2.3 con expectativa.

**¿Por qué no introducir `npx skills add` aquí ya que vamos a mencionarlo?**

Porque es contenido específico de la 2.3. Tocarlo aquí **invadiría la siguiente demo**. La regla del proyecto: **una demo, una pieza pedagógica**. Mencionarlo en el cliffhanger **anuncia, no enseña**.
