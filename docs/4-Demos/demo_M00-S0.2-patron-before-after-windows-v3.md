# Demo 0.2 — Patrón before/after en las demos del curso (convención)

> **Versión:** v3 | **Módulo:** 0 | **Sub:** 0.2 | **Estado:** ✅ Versión final
> **Archivo:** `demo_M00-S0.2-patron-before-after-windows-v3.md`
> **Tipo:** Documento de convención — sin screencast, sin rama propia. Vive en `main` como guía operativa del curso.
> **Plataforma:** Windows (PowerShell 7 + Git for Windows)

---

## 1. Motivación

Cada demo del curso enseña una herramienta automatizadora (un skill, un subagente, un hook). Esto crea una tensión pedagógica:

- **Si la herramienta ya está aplicada al arrancar**, no hay nada que hacer en pantalla y la demo pierde su valor.
- **Si la herramienta no está aplicada**, la siguiente clase arranca de un estado anterior y se rompe la cadena de ramas que conecta las 28 demos.

Lo resolvemos con el patrón `before/after` que Pluralsight usa en sus cursos, adaptado a ramas en vez de carpetas: cada sección no conceptual tiene **dos ramas hermanas** — una de partida del screencast y otra ya pre-cocinada con el resultado, que es la que la siguiente clase usa.

---

## 2. Nomenclatura

Para cada sección no conceptual `M.X.Y` del curso:

| Rama | Significado |
|---|---|
| **`demo/X.Y-before`** | Punto de **partida** del screencast. La infraestructura (skill, subagente, hook, configuración) ya está instalada, pero la **acción** que dispara aún no se ha ejecutado. Es el estado del que el formador hace `git checkout` al empezar a grabar. |
| **`demo/X.Y-after`** | Punto **final**, ya con la acción ejecutada y sus artefactos persistidos. Es el estado del que la siguiente clase arranca, **independientemente de lo que ocurra durante la grabación**. |

Para las secciones puramente conceptuales (1.1, 2.1a, 2.1b, 2.3, 3.1a, ver §8): rama única `demo/X.Y` sin sufijo.

> Pronunciación interna: «la base» y «la final». Por escrito siempre con sufijo explícito para que no haya ambigüedad al leer un `git log` o un `docs/DEMOS.md`.

---

## 3. Cuándo aplica el patrón

Cuatro tipos de demo y qué hacer con cada una:

| Tipo | Ejemplos | ¿Patrón before/after? |
|---|---|---|
| **CONCEPTUAL** — el alumno solo ve / explora; los cambios del screencast se descartan al final | 1.1, 2.1a, 2.1b, 2.3, 3.1a | **No.** Rama única `demo/X.Y`. La siguiente demo arranca de aquí. |
| **INFRA** — instala / configura un artefacto del curso (CLAUDE.md, .claude/, skill propio, hook) | 1.2a, 1.2b, 1.3a, 2.2c, 3.3a, 3.3b | **Sí.** `before` con la infraestructura instalada; `after` con la primera invocación ejecutada. |
| **CÓDIGO** — el agente / skill / hook genera código en OrderManagement | 2.2a, 2.2b, 3.1b | **Sí.** `before` con la herramienta lista; `after` con el código generado. |
| **MIXTA** — combina infra + código en la misma sección | 1.3b, 3.2a, 3.2b | **Sí.** `before` con la infra; `after` con la infra ejecutada *y* el código resultante. |

Cuando dudes, hazte una pregunta: **¿la siguiente clase necesita el resultado de esta demo en el repo?** Si la respuesta es no, basta rama única; si es sí, aplica before/after.

---

## 4. Cadena de ramas

La cadena conecta cada demo con la siguiente. La regla es simple: **cada `-before` parte del `-after` (o de la rama única) de la demo predecesora**.

Ejemplo del Módulo 1:

```
main
 │
 └── demo/0.1                       (M0.1 — setup, rama única)
       │
       └── demo/1.1                 (CONCEPTUAL — rama única, README y DEMOS.md commiteados)
             │
             └── demo/1.2a-before   (INFRA — Claude Code listo para autenticar)
                   │
                   └── demo/1.2a-after   (autenticado, .claude/ creado)
                         │
                         └── demo/1.2b-before   (CLAUDE.md plantilla pegada, sin afinar)
                               │
                               └── demo/1.2b-after   (CLAUDE.md afinado para .NET 10 + Angular)
                                     │
                                     └── demo/1.3a-before   ...
                                           │
                                           └── demo/1.3a-after   ...
                                                 │
                                                 └── ...
```

**Las CONCEPTUAL no se saltan** — siguen siendo punto de partida porque suelen contener al menos commits documentales (la marca `[x]` en `docs/DEMOS.md`, o un `docs/skills-explorados.md`). Lo que se descarta es solo lo que ocurre durante el screencast: el endpoint creado en vivo, el `cat` de un fichero, etc.

**Diagrama compacto:**

```
demo/(X.Y-1) o demo/(X.Y-1)-after
        │
        ▼
demo/X.Y-before  ── (screencast: pieza viva ejecutada y descartada)
        │
        │ (commits pre-cocinados que materializan la pieza viva)
        ▼
demo/X.Y-after
        │
        ▼
demo/(siguiente)-before
```

---

## 5. Plantilla de encabezado YAML

Copiar y pegar al crear o refactorizar una demo no conceptual:

```markdown
# Demo X.Y — <título corto>

> **Versión:** v3 | **Módulo:** N | **Sub:** X.Y | **Estado:** <estado>
> **Archivo:** `demo_M0N-SX.Y-<slug>-windows-v3.md`
> **Branch before:** `demo/X.Y-before`  (estado al hacer `git checkout` antes de grabar)
> **Branch after:**  `demo/X.Y-after`   (estado final que la siguiente clase asume)
> **Branch parent:** `demo/(X.Y-1)-after`  (o `demo/X.Y-1` si la previa es CONCEPTUAL, o `main`)
> **Tiempo total estimado:** <minutos>
> **Tipo:** <INFRA | CÓDIGO | MIXTA>
> **Plataforma:** Windows (PowerShell 7 + Git for Windows)
```

Para las CONCEPTUAL se mantiene el formato actual con campo único `Branch destino: demo/X.Y` y se añade una línea: *«Tipo: CONCEPTUAL — rama única, sin patrón before/after (ver M0.2)»*.

---

## 6. Plantilla de secciones reescritas

Las secciones que cambian respecto al formato pre-patrón. El resto (§1 Contexto, §2 Objetivo, §7 Artefactos, §9 Puntos clave, §10–11 Slides, §12 Timing, Apéndice) se mantiene igual que en las demos anteriores al refactor.

### §3 — Branch `demo/X.Y-before`

```markdown
## 3. Branch `demo/X.Y-before`

Punto de partida del screencast.

**Parte de:** `demo/(X.Y-1)-after`  (o el que corresponda)

**Qué tiene ya instalado / configurado** (la infraestructura lista para invocarse):

- [...]

**Qué NO tiene todavía** (lo que se ejecuta en vivo durante el screencast):

- [...]

> Si Pedro hace `git checkout demo/X.Y-before` antes de grabar, ese es exactamente el estado que verá. La pieza viva — la que decide la pedagogía de la demo — se ejecuta en pantalla a partir de aquí.
```

### §4 — Branch `demo/X.Y-after`

```markdown
## 4. Branch `demo/X.Y-after`

Punto final que la siguiente clase asume.

**Parte de:** `demo/X.Y-before`

**Qué añade** respecto a `-before` (los artefactos que la pieza viva produce, pre-cocinados):

- [...]

**Cómo se prepara** (independientemente del screencast):

- Ejecutar el prompt de §6b en una sesión limpia.
- Verificar con los criterios de §10.

> Importante: la rama `-after` se crea **antes de grabar**, no después. La grabación arranca de `-before` y los cambios reales del screencast se descartan al cerrar — la siguiente clase parte siempre de `-after`, así no dependemos de que la grabación salga clavada.
```

### §5 — Estado del repo al hacer `git checkout demo/X.Y-before`

```markdown
## 5. Estado del repo al hacer `git checkout demo/X.Y-before`

Árbol y estado funcional con el que el alumno (y el formador antes de grabar) se encuentra.

[árbol del proyecto]

**Estado funcional:**

- [...]

**Comandos para verificar:**

```powershell
git checkout demo/X.Y-before
dotnet build
# Esperado: 0 warnings, 0 errors
```

### §6a — Prompt para Claude Code: preparar `demo/X.Y-before`

```markdown
## 6a. Prompt para Claude Code — preparar `demo/X.Y-before`

> Lo que el formador copia y pega en una sesión limpia de Claude Code para crear `demo/X.Y-before` desde la rama predecesora.

[bloque de prompt: las tareas que instalan la infraestructura sin disparar la acción + commit + restricciones]
```

### §6b — Prompt para Claude Code: preparar `demo/X.Y-after`

```markdown
## 6b. Prompt para Claude Code — preparar `demo/X.Y-after`

> Lo que el formador copia y pega para materializar el resultado pre-cocinado, partiendo de `demo/X.Y-before`.

[bloque de prompt: las tareas que ejecutan la acción y commitean los artefactos resultantes + restricciones]
```

### §8 — Guion del screencast (ajuste explícito al patrón)

```markdown
## 8. Guion del screencast

> **Antes de grabar:** `git checkout demo/X.Y-before`. La rama `demo/X.Y-after` ya existe pre-cocinada (preparada con el prompt §6b) y NO se toca durante la grabación.

[bloques cronometrados como siempre]

---

### Bloque final — Limpieza y descarte

Al terminar la pieza viva, descarta los cambios del screencast — la rama `-after` ya tiene el resultado pre-cocinado:

```powershell
git restore .
git clean -fd
git status
# Esperado: working tree clean — la rama -before queda limpia
```

> No se commitea nada de lo que se hizo en pantalla. La siguiente clase hará `git checkout demo/X.Y-after` para arrancar.
```

---

## 7. Cómo gestionas las ramas en local

Crear el par `-before` / `-after` para una sección nueva:

```powershell
# 1. Crear -before desde la rama predecesora
git checkout demo/(X.Y-1)-after          # o la rama única si la previa es CONCEPTUAL
git checkout -b demo/X.Y-before

# 2. Lanzar Claude Code con el prompt §6a, que commitea la infraestructura
#    (Claude Code se encarga del commit; tú no haces git commit a mano)

# 3. Crear -after desde -before
git checkout -b demo/X.Y-after

# 4. Lanzar Claude Code con el prompt §6b, que commitea los artefactos resultantes

# 5. Verificar la cadena
git log --oneline --graph demo/X.Y-after
```

Validar que la cadena entera está bien encadenada (ejecutar antes de grabar cualquier demo):

```powershell
# Lista las ramas demo/* ordenadas
git branch --list "demo/*" | Sort-Object

# Confirma que cada -before parte del -after correcto
git merge-base --is-ancestor demo/(X.Y-1)-after demo/X.Y-before
# Exit code 0 = ancestor, OK. Distinto de 0 = la cadena está rota.
```

Cierre del curso (mergear toda la cadena a `main`):

```powershell
# Lo hace Pedro al cerrar el curso, no antes.
git checkout main
git merge --ff-only demo/<última-rama>
```

> Las ramas `-before` / `-after` no se borran después del merge — quedan como artefactos históricos navegables por cualquier alumno con `git log` o `git checkout`.

---

## 8. Excepciones (rama única, sin patrón)

Las siguientes secciones **no aplican** el patrón before/after y mantienen rama única:

- **`demo/0.1`** — setup del proyecto OrderManagement (M0.1). Es construcción, no se graba como screencast pedagógico, no hay distinción «antes» / «después» de un evento en pantalla.
- **`demo/1.1`** — CONCEPTUAL. El alumno ve el ciclo agentic en pantalla y al final se descartan los cambios del screencast. La rama solo persiste el README y `docs/DEMOS.md` iniciales.
- **`demo/2.1a`** — CONCEPTUAL. Exploración de skills oficiales en VS Code, sin crear nada.
- **`demo/2.1b`** — CONCEPTUAL. Lectura crítica de descripciones de skills.
- **`demo/2.3`** — CONCEPTUAL. Ecosistema y distribución; instalaciones se descartan al final.
- **`demo/3.1a`** — CONCEPTUAL. Modelo conceptual de subagentes integrados.

**Cada una de estas demos lleva en su §1 una línea:**

> *«Demo CONCEPTUAL — rama única `demo/X.Y` sin patrón before/after (ver M0.2). Los cambios que ocurren durante el screencast se descartan al final; lo que persiste son los commits documentales (marca en `docs/DEMOS.md`, notas auxiliares). La siguiente clase parte de aquí.»*

---

## 9. Notas para Pedro

**Por qué el formato de la rama es `-before` / `-after` y no `-pre` / `-post` o `-base` / `-final`:**

«Before» y «after» son los términos de Pluralsight, los reconoces visualmente al instante en `git branch --list`, y resuelven el riesgo de que un alumno avanzado confunda «base» con «la rama main».

**Por qué se prepara `-after` antes de grabar y no después:**

Si grabas y luego intentas reproducir el resultado en código para crear `-after`, dos riesgos: (a) lo que sale en directo no coincide con lo que persistes (Claude Code es no determinista) y la siguiente clase arranca de un estado distinto al que el alumno vio en pantalla; (b) si la grabación se eterniza o se rompe, la cadena se queda colgada. Pre-cocinarla es el único camino reproducible.

**Por qué las CONCEPTUAL no se eliminan de la cadena:**

Algunas conceptuales (1.1, 2.3) commitean documentación que las siguientes demos asumen (`docs/DEMOS.md`, marcas `[x]`). Mantenerlas como punto de partida directo de la siguiente `-before` evita reescribir esos artefactos en cada salto.

**Si una demo refactoriza algo que otra demo posterior asume:**

Cualquier cambio en `-after` se propaga aguas abajo: las demos siguientes parten de un punto distinto. Por eso la convención es: **antes de tocar una rama `-after` ya creada, comprobar si hay descendientes con `git branch --contains <hash>` y avisar a Pedro**. Si no hay descendientes (la cadena aún no se ha extendido más allá), el cambio es seguro.

**Scripts ejecutables (decisión tomada):**

M0.2 documenta los comandos PowerShell como referencia, **no incluye scripts ejecutables**. Los scripts son herramienta del Módulo 3 (hooks y automatización) y meterlos aquí complicaría la fundación. Si en algún momento quieres automatizar la creación de ramas, ese es contenido de un demo M3.X.

**Plantillas de §6a y §6b — qué incluir y qué no:**

- **§6a** instala la infraestructura sin disparar la acción. Por ejemplo, en una demo de skill: el SKILL.md está creado y registrado, pero NO se invoca ningún `/skill ...`. En una demo de hook: el hook está conectado en settings.json, pero NO se ejecuta el `Write` que lo dispararía.
- **§6b** parte de `-before` y dispara la acción. Pero **no escenifica**: ejecuta el resultado directamente vía Claude Code en una sesión limpia (mediante el prompt). Lo que el alumno ve en directo es la pieza viva en `-before`, no el `-after` materializado.

Si un detalle del prompt §6a o §6b no encaja con cómo te grabarías la demo, ese es el sitio para corregir antes de avanzar.
