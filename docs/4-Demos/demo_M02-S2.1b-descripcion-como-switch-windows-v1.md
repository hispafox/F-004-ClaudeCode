# Demo 2.1b — La descripción como switch: cuándo se activa un skill

> **Versión:** v1 | **Módulo:** 2 | **Sub:** 2.1b | **Estado:** ✅ Versión final
> **Archivo:** `demo_M02-S2.1b-descripcion-como-switch-windows-v1.md`
> **Branch destino:** `demo/2.1b`
> **Branch de partida:** `demo/2.1a`
> **Tiempo total estimado:** ~18-22 minutos
> **Tipo:** Demo de experimento controlado. **Es la pieza más sutil del módulo 2: el alumno ve por qué un skill funcionalmente correcto puede no activarse jamás si la descripción está mal.** Construimos un mismo skill con cuatro descripciones distintas y observamos cuándo se activa y cuándo no, sobre el proyecto OrderManagement. **Aún no creamos un skill propio "de verdad" — eso es 2.2a.** Aquí solo experimentamos con la descripción.
> **Plataforma:** Windows 11 (PowerShell 7).

---

## 1. Contexto

En la 2.1a vimos la anatomía: un skill es un directorio con `SKILL.md`, frontmatter con `name` y `description`, cuerpo ligero, lo denso a `references/`. Diseccionamos `frontend-design` y `simplify` como referencia.

Ya en aquella demo se sembró algo: **la descripción es el switch**. Ahora en la 2.1b lo desmenuzamos. La gamma 2.1b (33 slides, ~30 min) lo trabajó en profundidad: el problema del skill invisible, los seis anti-patrones de descripción, la fórmula de tres ingredientes, los tres casos donde "casi funciona" (trigger demasiado específico, ambigüedad entre dos skills, contexto del proyecto que se da por hecho), y cómo iterar una descripción.

Lo que falta es **verlo en pantalla**. La gente que llega aquí piensa *"vale, lo entiendo, descripción más concreta"*. Pero hasta que no ven con sus ojos cómo el mismo skill se activa o no según cómo está escrita la descripción, la lección no cala. Esta demo es ese momento.

> **Tipo de demo:** experimento controlado. Construimos un skill mínimo experimental, le cambiamos la descripción cuatro veces, y vemos en directo qué pasa cuando lanzamos peticiones distintas. **El skill experimental NO se queda en el repo** — al final lo borramos. La rama solo añade `docs/skills-explorados.md` actualizado y la marca en `DEMOS.md`. **El primer skill propio "de verdad" llega en 2.2a**.

---

## 2. Objetivo de la demo

Cinco cosas concretas que tienen que quedar en la cabeza del alumno cuando termine los ~18 minutos de screencast:

1. **Una descripción mal escrita = skill invisible.** Aunque el skill sea funcionalmente correcto, si la descripción no coincide con cómo el usuario formula su petición, no se activa nunca. **Lo verán en pantalla con cuatro versiones distintas de la misma descripción**.

2. **La fórmula de tres ingredientes.** Verbo claro. Disparadores lingüísticos con variantes. Tercera persona, no imperativo. La gamma slide 6 lo dijo. La demo lo materializa con una descripción "v4" que sí funciona consistentemente.

3. **El truco para iterar: preguntarle directamente al agente "¿qué skill has usado?".** Es la única forma fiable de saber si un skill se activó. Lo demostramos en directo.

4. **Los tres casos donde la descripción "casi funciona".** Trigger demasiado específico. Ambigüedad entre dos skills. Contexto del proyecto que se da por hecho. Vemos el primero y el tercero materializados en el experimento.

5. **La activación es probabilística, no determinista.** El alumno tiene que aceptar que **el 100% no es objetivo**. La meta es **fiable cuando importa**. Sin esta aceptación, va a pelear con la herramienta.

Y dos cosas que tienen que **NO quedar** en su cabeza:

- *"Si escribo una descripción larga, mejor."* — no, ni mucho menos. La descripción debe ser **concreta**, no larga. Hay un límite de ~1024 caracteres y pasarse genera ruido.
- *"Si el skill no se activa, es bug del modelo."* — no, **es la descripción**. La carga está en quien escribe el skill, no en el modelo.

---

## 3. Branch de partida

```
demo/2.1a
```

> Estado actual: igual que `demo/1.3b-after` excepto por la marca de la 2.1a en `docs/DEMOS.md` y el fichero `docs/skills-explorados.md`. **No hay ningún `.claude/skills/` en el repo todavía**. Eso es importante porque vamos a crear uno experimental durante la demo y veremos cómo Claude Code lo detecta automáticamente.

---

## 4. Branch destino

```
demo/2.1b
```

> Tras la demo, la rama `demo/2.1b` añade dos cosas mínimas: la marca `[x]` en `docs/DEMOS.md` y la actualización de `docs/skills-explorados.md` con las cuatro versiones experimentales de la descripción y los hallazgos del experimento. **El skill experimental NO se queda en el repo** — se borra al final del screencast. **El primer skill propio "de verdad" se crea en 2.2a**.

---

## 5. Estado del repo al empezar

Idéntico a `demo/2.1a`:

```
ordermanagement/
├── .claude/
│   └── settings.json
├── docs/
│   ├── DEMOS.md                        (1.1, 1.2a, 1.2b, 1.3a, 1.3b, 2.1a marcadas)
│   └── skills-explorados.md            (notas de la 2.1a)
├── scripts/
├── src/                                (sin cambios)
├── frontend/                           (sin cambios)
├── tests/                              (sin cambios)
├── .gitignore
├── CLAUDE.md
└── README.md
```

**Estado clave para esta demo:**

- **No hay `.claude/skills/`** todavía. Lo crearemos en vivo y lo borraremos al final.
- **El skill experimental se llama `find-handler`** — un skill mínimo que localiza el handler de un comando dado en el proyecto. Lo elegimos porque es **fácil de probar y la activación es ambigua**: si pides *"busca el handler de X"*, *"localiza el handler"*, *"dónde está el handler"*, etc., una descripción mal escrita va a fallar en algunas variantes.

**Estado de la máquina Windows del formador:**

```
✅ Claude Code v2.1.x instalado y autenticado
✅ Git for Windows
✅ PowerShell 7
✅ VS Code con el repo cargado en demo/2.1b
✅ CLAUDE.md y .claude/settings.json operativos
```

**Lo que el alumno verá al final de la demo:**

- Un skill experimental `find-handler` creado en `.claude/skills/find-handler/SKILL.md` con cuatro versiones de la descripción.
- Cuatro experimentos de activación con peticiones que incluyen variantes de vocabulario.
- Demostración del truco *"¿qué skill has usado?"*.
- El skill borrado al final con `git clean` para no contaminar la rama.
- `docs/skills-explorados.md` actualizado con las cuatro descripciones y resultados del experimento.

---

## 6. Prompt para Claude Code

> Lo que tú, formador, copias y pegas en Claude Code para preparar la rama `demo/2.1b` antes de grabar.

````
Estoy preparando la demo 2.1b del curso de Claude Code para devs .NET +
Angular. Esta demo es un experimento controlado sobre cómo la descripción
de un skill decide si se activa o no. Construimos un skill experimental
con cuatro versiones distintas de la descripción y observamos qué pasa.

# Contexto

Estoy en la rama `demo/2.1a` del repo `ordermanagement`. La rama tiene
todo lo de demos anteriores incluyendo el docs/skills-explorados.md
con notas de la 2.1a.

Quiero que prepares la rama demo/2.1b con un cambio mínimo: marcar la
demo en docs/DEMOS.md y dejar la actualización de docs/skills-explorados.md
preparada para que durante el screencast yo añada los hallazgos del
experimento.

NO crees ningún `.claude/skills/`. Eso lo hago en vivo durante el
screencast como parte del experimento, y lo borro al final.

# Lo que necesito

Tres tareas:

## Tarea 1: crear la rama

```powershell
git checkout demo/2.1a
git pull
git checkout -b demo/2.1b
```

## Tarea 2: actualizar docs/DEMOS.md

Localiza la línea:

```
- [ ] demo/2.1b — Skill propio diseccionado
```

Cámbiala por:

```
- [x] **demo/2.1b** — La descripción como switch: experimento con 4 versiones
```

## Tarea 3: ampliar docs/skills-explorados.md

Añade al final del fichero existente esta sección nueva:

```markdown

---

# Demo 2.1b — Experimento de la descripción como switch

## Contexto

En esta demo se construyó un skill experimental `find-handler` para
ilustrar cómo la descripción decide la activación. Se probaron cuatro
versiones de la descripción con peticiones de vocabulario variado.

## Las cuatro versiones probadas

### Versión 1 (mala — anti-patrón "demasiado vaga")

```yaml
description: Ayuda con código
```

Resultado esperado: el skill no se activa NUNCA porque la descripción
no coincide con ningún caso de uso específico.

### Versión 2 (mala — anti-patrón "solo dice qué hace, no cuándo")

```yaml
description: Localiza el handler de un comando MediatR
```

Resultado esperado: activa solo cuando la petición incluye literalmente
"handler de un comando MediatR". Falla cuando se pregunta "dónde está
el handler" o variantes naturales.

### Versión 3 (mejor — añade trigger pero específico)

```yaml
description: Localiza handlers MediatR del proyecto. Usar cuando el
  usuario diga "busca el handler de X".
```

Resultado esperado: activa con "busca el handler". Falla con "encuentra
el handler", "dónde está el handler", "muestra el handler". Es el caso
A del manual línea 222: trigger demasiado específico.

### Versión 4 (buena — fórmula completa)

```yaml
description: Localiza handlers MediatR (clases que implementan
  IRequestHandler) en el proyecto OrderManagement. Usar cuando el usuario
  pida buscar, localizar, encontrar o mostrar el handler de un comando
  o query, o use sinónimos como "dónde está", "muéstrame" o "busca"
  referidos a handlers.
---
```

Resultado esperado: activa con la mayoría de las variantes naturales.
Triggers explícitos en abanico ("buscar, localizar, encontrar, mostrar",
"dónde está, muéstrame, busca"), contexto del proyecto explícito
("MediatR, OrderManagement"), y referencia al patrón concreto
(IRequestHandler).

## Hallazgos del experimento

(esta sección la rellena Pedro durante el screencast con los resultados
reales que obtenga)

- Versión 1: …
- Versión 2: …
- Versión 3: …
- Versión 4: …

## Lecciones extraídas

1. **La descripción es el switch.** Sin descripción concreta, el skill
   es invisible aunque el cuerpo sea perfecto.
2. **La fórmula de tres ingredientes funciona:** verbo claro,
   disparadores en abanico, tercera persona.
3. **El truco para iterar:** preguntarle al agente "¿qué skill has
   usado?" tras cada petición. Es la única forma fiable de saber si
   el skill se activó.
4. **La activación es probabilística.** El 100% no es objetivo. La meta
   es ser fiable cuando importa.
5. **Caso A del manual aplicado en directo:** trigger demasiado
   específico. La V3 lo materializa.
```

## Tarea 4: verificar y commitear

```powershell
Set-Location c:\w\repos\F-004-ClaudeCode\ordermanagement
dotnet build
```

Esperado: 0 warnings, 0 errors.

```powershell
Set-Location c:\w\repos\F-004-ClaudeCode
git add docs/DEMOS.md docs/skills-explorados.md
git commit -m "demo/2.1b: experimento descripción como switch (pre-grabación)"
```

NO hagas push.

# Restricciones (importantes)

- NO crees `.claude/skills/` ni ningún SKILL.md en el repo. Esto se
  hace EN VIVO durante el screencast.
- NO modifiques CLAUDE.md ni .claude/settings.json.
- NO toques el código de la app.
- NO modifiques README.md.

# Cuando termines, dime

1. Que la rama demo/2.1b está creada desde demo/2.1a.
2. Que docs/DEMOS.md tiene 2.1b marcada.
3. Que docs/skills-explorados.md tiene la sección nueva añadida con
   las cuatro versiones.
4. Que el build pasa.
5. Que el commit está hecho.

Si tienes dudas, para y pregúntame.
````

---

## 7. Artefactos que Claude Code debe generar

```
✓ Rama nueva: demo/2.1b (parte de demo/2.1a)
✓ docs/DEMOS.md con 2.1b marcada como [x]
✓ docs/skills-explorados.md ampliado con la sección "Demo 2.1b — Experimento"
   y las cuatro versiones documentadas (los hallazgos los rellena Pedro
   durante el screencast)
✓ Verificación de build OK: dotnet build limpio
✓ Commit único: "demo/2.1b: experimento descripción como switch (pre-grabación)"
```

**Lo que NO debe haber generado:**

- ❌ Ningún `.claude/skills/` (eso se crea EN VIVO durante el screencast)
- ❌ Ningún `SKILL.md`
- ❌ Cambios en código de la app
- ❌ Cambios en CLAUDE.md o settings.json

> Si Claude Code se anticipa y crea `.claude/skills/find-handler/`, **se rechaza el output**. La creación es parte del experimento del screencast.

**Estado final del árbol después del prompt:**

```
ordermanagement/
├── .claude/
│   └── settings.json                       (sin cambios)
├── docs/
│   ├── DEMOS.md                            ← MODIFICADO (1 línea)
│   └── skills-explorados.md                ← MODIFICADO (sección nueva)
├── scripts/                                (sin cambios)
├── src/                                    (sin cambios)
├── frontend/                               (sin cambios)
├── tests/                                  (sin cambios)
├── .gitignore                              (sin cambios)
├── CLAUDE.md                               (sin cambios)
└── README.md                               (sin cambios)
```

---

## 8. Guion del screencast

**Duración estimada del screencast: ~16-19 minutos.**

Ocho bloques. Es la demo más visual del módulo 2 — el alumno ve activaciones y no-activaciones en directo.

> **Antes de empezar a grabar**, asegúrate de:
> - Tener PowerShell 7 abierto en pantalla completa, fuente al menos 16pt.
> - Tener VS Code abierto al lado con el repo `ordermanagement` cargado en `demo/2.1b`.
> - Verificar que **NO existe** `.claude/skills/` en el repo. Si existe de pruebas anteriores, bórralo.
> - Cerrar Slack, Teams, navegadores con notificaciones.
> - Tener **el prompt del experimento copiado** en una nota aparte para no equivocarte al teclearlo en cada iteración. El experimento requiere lanzar la **misma petición** cuatro veces (cambiando solo la descripción del skill), así que la consistencia importa.

---

### Bloque 1 — Setup y planteamiento del experimento (~1 min 30 seg)

**Pantalla compartida.** A la izquierda VS Code con el repo en `demo/2.1b`. A la derecha terminal PowerShell.

**En terminal:**

```powershell
git status
ls .claude\
```

```
On branch demo/2.1b
nothing to commit, working tree clean

    Directorio: C:\Users\pedro\projects\ordermanagement\.claude

LastWriteTime    Length Name
-------------    ------ ----
...                3456 settings.json
```

**Lo que dices:**

> "Estamos en la rama `demo/2.1b`. El repo no tiene ningún `.claude/skills/` todavía. Lo confirmamos: solo hay `settings.json` ahí dentro. Ningún skill instalado en el proyecto.
>
> En la 2.1a vimos la anatomía. Vimos que el frontmatter tiene dos campos obligatorios — `name` y `description` — y os adelanté que la descripción es el switch. Esta demo es ese tema en profundidad. **Vamos a hacer un experimento controlado.**
>
> Construyo un skill mínimo experimental aquí mismo, en `.claude/skills/find-handler/`. Su trabajo es localizar handlers MediatR del proyecto. **El cuerpo del skill no va a cambiar — siempre el mismo.** Lo que va a cambiar es **la descripción**, cuatro veces. Y observamos qué pasa cuando lanzo la misma petición con cada versión.
>
> Si la gamma 2.1b tenía razón en lo que dijo en el slide 4 — **'el skill invisible'** — vais a ver versiones donde el skill **existe físicamente, está instalado, está perfectamente escrito en su cuerpo, y aún así no se activa nunca**. Solo por culpa de la descripción.
>
> Y al final del experimento, **borro el skill**. La rama queda limpia. El skill experimental no contamina el proyecto. La 2.2a empieza con un skill propio de verdad."

**Tiempo:** ~90 segundos.

---

### Bloque 2 — Construir el skill experimental con descripción V1 (la peor) (~2 min)

> "Empezamos. Creo la carpeta y el `SKILL.md`."

**En terminal:**

```powershell
mkdir .claude\skills\find-handler
```

**En VS Code, creas el fichero `.claude/skills/find-handler/SKILL.md` y escribes la versión 1:**

```markdown
---
name: find-handler
description: Ayuda con código
---

# Find handler

Cuando el usuario pida localizar un handler MediatR, sigue estos pasos:

1. Identifica el comando o query que mencionó el usuario.
2. Busca con Glob el patrón `*Handler.cs` en `src/OrderManagement.Application/Handlers/`.
3. Lee el handler correspondiente.
4. Devuelve al usuario:
   - La ruta completa del fichero.
   - La signatura del método Handle.
   - Una descripción breve de qué hace el handler.

Si no encuentras el handler exacto, lista todos los handlers disponibles
y pregúntale al usuario cuál quería.
```

**Salvas. Vuelves a la terminal.**

> "Ahí tenéis. Skill mínimo. **El cuerpo está perfectamente claro** — pasos numerados, qué hacer, qué devolver. Si Claude Code activara este skill, sabría exactamente qué hacer.
>
> **Pero la descripción es 'Ayuda con código'**. La gamma 2.1b slide 5 marcó esto como el primer anti-patrón: **demasiado vaga**. *'¿Cuándo se usa esto?'*. No hay verbo concreto, no hay disparadores. **Vamos a ver qué pasa cuando lanzo una petición que claramente debería activar este skill.**"

**Tiempo:** ~2 minutos.

---

### Bloque 3 — Test V1: el skill invisible en directo (~2 min 30 seg)

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
 ✓ 1 project skill loaded: find-handler
 Type / for commands, ? for help

>
```

> "Mirad. **Ha cargado el skill.** Línea verde: *'1 project skill loaded: find-handler'*. **El skill existe.** Está cargado. Si la teoría de la descripción como switch es real, el siguiente paso lo va a demostrar.
>
> Le hago la petición que **debería** activar el skill:"

**Tecleas:**

```
> Localiza el handler del CancelOrderCommand en el proyecto.
```

**Pulsas Enter. Aparecen las herramientas:**

```
● Glob(**/CancelOrderHandler*)
● Read(src/OrderManagement.Application/Handlers/CancelOrderHandler.cs)
```

**El agente responde:**

```
He encontrado el handler. Vive en:
src/OrderManagement.Application/Handlers/CancelOrderHandler.cs

Implementa IRequestHandler<CancelOrderCommand, Unit> y tiene un método
Handle que valida el estado del pedido y, si es cancelable, lanza el
proceso de cancelación. Si el estado no permite cancelar, lanza
InvalidOrderStateException (la excepción tipada que añadimos en la
demo 1.3b).
```

> "**El agente ha respondido.** Y bien. Pero la pregunta clave: **¿ha usado mi skill?** Vamos a comprobarlo con el truco que la gamma slide 11 enseñó."

**Tecleas:**

```
> ¿Qué skill has usado para resolver esa petición?
```

**El agente responde:**

```
No he usado ningún skill específico. He resuelto la petición
directamente con las herramientas estándar (Glob, Read) basándome en
las convenciones del CLAUDE.md (handlers MediatR en
src/OrderManagement.Application/Handlers/).
```

> "Mirad esto. **'No he usado ningún skill específico'**. ¡El skill estaba cargado! Estaba ahí, instalado, su cuerpo era perfecto. Pero la descripción **'Ayuda con código'** no le dijo a Claude *cuándo activarlo*, así que Claude lo ignoró completamente. Resolvió la petición con sus herramientas básicas.
>
> Esto es **el skill invisible** del slide 4 de la gamma. Skill funcionalmente correcto que **nunca se activa**. Y este es el problema número uno de la gente que escribe sus primeros skills. La gamma lo dijo: escriben skills perfectos que nadie usa, y concluyen que *'esto no funciona'*.
>
> Vamos a probar la versión 2."

**Salgo (Ctrl+C):**

**Tiempo:** ~2 minutos 30 segundos.

---

### Bloque 4 — Iteración V2: "solo dice qué hace, no cuándo" (~2 min)

**En VS Code, edito el `SKILL.md` y cambio solo la descripción a la V2:**

```markdown
---
name: find-handler
description: Localiza el handler de un comando MediatR
---
```

(El cuerpo no cambia — siempre el mismo.)

> "Versión 2. **'Localiza el handler de un comando MediatR'**. Mejora respecto a V1: ya hay verbo, ya hay objeto. Pero **solo dice qué hace, no cuándo activarlo**. Es el anti-patrón del slide 5 línea 6 de la gamma. Vamos a ver."

**Vuelvo a Claude Code:**

```powershell
claude
```

**Misma petición exacta:**

```
> Localiza el handler del CancelOrderCommand en el proyecto.
```

**Aparece — atención al output:**

```
● [Skill activated: find-handler]
● Glob(*Handler.cs en src/OrderManagement.Application/Handlers/)
● Read(src/OrderManagement.Application/Handlers/CancelOrderHandler.cs)

He localizado el handler siguiendo los pasos del skill find-handler:

- Ruta: src/OrderManagement.Application/Handlers/CancelOrderHandler.cs
- Signatura: public async Task<Unit> Handle(CancelOrderCommand request, 
  CancellationToken cancellationToken)
- Qué hace: valida que el pedido existe, valida que su estado permite
  cancelación (sólo Pending o Confirmed), si no lanza
  InvalidOrderStateException, si sí ejecuta la cancelación.
```

> "Mirad la primera línea: **'[Skill activated: find-handler]'**. **Esta vez sí se ha activado.** Y mirad la respuesta — ha seguido los pasos del skill: ruta, signatura, descripción. **El cuerpo del SKILL.md ha entrado en juego.**
>
> Pero esto era **una petición clara**. El usuario usó el verbo *'localiza'* y mencionó *'handler'*. Vamos a probar **una variante natural** que un dev real diría:"

**Tecleas:**

```
> ¿Dónde está el handler que cancela los pedidos?
```

**Aparece:**

```
● Glob(**/CancelOrderHandler*)
● Read(src/OrderManagement.Application/Handlers/CancelOrderHandler.cs)

El handler vive en src/OrderManagement.Application/Handlers/CancelOrderHandler.cs...
```

**Pregunto:**

```
> ¿Has usado el skill find-handler en esta última petición?
```

```
No, en esta no. He resuelto directamente con Glob y Read porque la
descripción del skill find-handler es "Localiza el handler de un
comando MediatR" y la palabra "Localiza" no coincide con "dónde está"
suficientemente. He preferido ir directo.
```

> "**Atentos a esto.** El propio agente me dice **por qué no usó el skill**: *'la palabra Localiza no coincide con dónde está suficientemente'*. **Esto es exactamente el caso A del slide 8 de la gamma**: trigger demasiado específico. La descripción dice *'localiza'* y el usuario dice *'dónde está'*. Para el agente, esa diferencia basta para no activarlo.
>
> Pasamos a la V3."

**Salgo (Ctrl+C):**

**Tiempo:** ~2 minutos.

---

### Bloque 5 — Iteración V3: trigger explícito pero aún demasiado específico (~2 min)

**En VS Code, V3:**

```markdown
---
name: find-handler
description: Localiza handlers MediatR del proyecto. Usar cuando el usuario diga "busca el handler de X".
---
```

> "Versión 3. **Ya añadimos el ingrediente que faltaba: el trigger explícito.** *'Usar cuando el usuario diga busca el handler de X'*. Pero atentos al detalle — pongo *'busca el handler de X'* entre comillas, **literal**. Esto va a ser instructivo."

**Claude Code:**

```powershell
claude
```

**Petición exacta con el verbo del trigger:**

```
> Busca el handler de CancelOrderCommand.
```

**Aparece:**

```
● [Skill activated: find-handler]
● Glob, Read...

[respuesta usando el skill]
```

> "Activado. Como esperábamos, *'busca el handler'* coincide con el trigger explícito de la descripción.
>
> Ahora voy a probar **otra variante natural**:"

**Tecleas:**

```
> Encuéntrame el handler que cancela pedidos.
```

**Aparece:**

```
● Glob, Read...

[respuesta SIN activación del skill]
```

```
> ¿Has activado el skill?
```

```
No, no lo activé. La descripción dice "Usar cuando el usuario diga
busca el handler de X" y tú dijiste "encuéntrame el handler". Las
palabras "encuéntrame" y "busca" no son lo suficientemente cercanas
para activarlo.
```

> "**Otra vez el mismo problema.** Aunque mejoré la V2, sigue siendo demasiado específica. **El verbo concreto en la descripción se convierte en una restricción**. Si el usuario usa otro verbo, no activa.
>
> Esto es lo que la gamma 2.1b slide 8 llamó **'casi funciona'**. La V3 funciona en el setenta por ciento de los casos donde el usuario usa la palabra exacta. Falla en el treinta por ciento que usa sinónimos naturales — *'encuentra'*, *'localiza'*, *'muéstrame'*, *'dónde está'*.
>
> La V4 va a aplicar la fórmula completa de los tres ingredientes."

**Salgo (Ctrl+C):**

**Tiempo:** ~2 minutos.

---

### Bloque 6 — V4: la fórmula completa, descripción que sí funciona (~3 min 30 seg)

**En VS Code, V4 — la buena:**

```markdown
---
name: find-handler
description: Localiza handlers MediatR (clases que implementan IRequestHandler) en el proyecto OrderManagement. Usar cuando el usuario pida buscar, localizar, encontrar o mostrar el handler de un comando o query, o use sinónimos como "dónde está", "muéstrame" o "busca" referidos a handlers.
---
```

> "Versión 4. La buena. Vamos a desmenuzarla aplicando la fórmula del slide 6 de la gamma:
>
> **Ingrediente uno: qué hace.** *'Localiza handlers MediatR (clases que implementan IRequestHandler) en el proyecto OrderManagement'*. **Verbo claro al inicio**. Y mirad el detalle: *'(clases que implementan IRequestHandler)'* y *'en el proyecto OrderManagement'*. **Resuelve el caso C del slide 10**: contexto del proyecto que se da por hecho. Aquí no se da por hecho, se explicita. El agente sabe inmediatamente de qué patrón hablo y de qué proyecto.
>
> **Ingrediente dos: cuándo usarlo, con disparadores en abanico.** *'Usar cuando el usuario pida buscar, localizar, encontrar o mostrar el handler...'*. **Cuatro verbos distintos**. Y luego: *'o use sinónimos como dónde está, muéstrame o busca referidos a handlers'*. **Resuelve el caso A del slide 8**: trigger demasiado específico. Ya no es un verbo, son varios. Ya no es una expresión literal, son sinónimos.
>
> **Ingrediente tres: tercera persona, no imperativo.** Mirad cómo está escrito: *'Localiza...'* (descriptivo en tercera persona, no *'Localiza esto'* en imperativo). *'Usar cuando...'* (no *'Usa cuando...'* en imperativo). La gamma slide 6 tercer punto.
>
> Vamos a someterla a una batería de pruebas. Cuatro variantes naturales que un dev real diría:"

**Claude Code:**

```powershell
claude
```

**Test 1 — verbo "localiza":**

```
> Localiza el handler del CreateOrderCommand.
```

```
● [Skill activated: find-handler]
[respuesta correcta usando el skill]
```

> "Activado. Verbo del trigger principal."

**Test 2 — verbo "encuentra":**

```
> Encuentra el handler que crea pedidos.
```

```
● [Skill activated: find-handler]
[respuesta correcta]
```

> "Activado. *'Encuentra'* está en el abanico."

**Test 3 — pregunta natural:**

```
> ¿Dónde está el handler que cancela pedidos?
```

```
● [Skill activated: find-handler]
[respuesta correcta]
```

> "Activado. Y ojo: en V2 esta pregunta exacta NO activó el skill. Aquí sí. **El abanico de sinónimos cubre 'dónde está'.**"

**Test 4 — variante coloquial:**

```
> Muéstrame el handler de update.
```

```
● [Skill activated: find-handler]
[respuesta correcta]
```

> "Activado. *'Muéstrame'* también está en el abanico.
>
> **Cuatro de cuatro.** La V4 activa fiablemente con vocabulario natural. ¿Va a activar siempre? **No.** La gamma fue clara: la activación es probabilística. Pero **es fiable cuando importa**. Y eso es la meta.
>
> Para confirmar el control: pruebo una petición que **NO debería** activar el skill, para asegurarme que la V4 no se va de madre."

**Test 5 — petición fuera de scope:**

```
> Explícame qué hace el OrdersController.
```

```
● Read(src/OrderManagement.Api/Controllers/OrdersController.cs)
[respuesta sin activar el skill]
```

```
> ¿Has activado el skill find-handler?
```

```
No, el usuario pidió "explica qué hace OrdersController" — no es una
petición de localización de handler. La descripción del skill habla
de buscar/localizar/mostrar handlers, no de explicar otros tipos de
ficheros.
```

> "Perfecto. **No se ha activado** porque la petición no encaja con la descripción. **Eso es lo que queremos.** Un skill que activa demasiado es tan malo como un skill invisible — interrumpe el flujo natural del agente.
>
> La V4 funciona. Activa cuando debe, no activa cuando no debe."

**Tiempo:** ~3 minutos 30 segundos.

---

### Bloque 7 — Documentar hallazgos y limpiar (~2 min)

**Salgo (Ctrl+C). Voy a VS Code y abro `docs/skills-explorados.md`:**

> "Voy a rellenar la sección de 'Hallazgos del experimento' que dejamos preparada en el commit pre-grabación."

**Edito el fichero, sección 'Hallazgos del experimento':**

```markdown
## Hallazgos del experimento

- **Versión 1** ("Ayuda con código"): NUNCA se activó. El skill cargado
  pero invisible. Anti-patrón "demasiado vaga" confirmado.
  
- **Versión 2** ("Localiza el handler de un comando MediatR"): Se activó
  con la frase exacta "Localiza el handler de X". NO se activó con
  variantes naturales como "¿dónde está el handler?". Anti-patrón
  "solo dice qué hace, no cuándo".
  
- **Versión 3** (con trigger "busca el handler de X"): Mejoró pero
  sigue limitada. Activa solo con el verbo "busca". Falla con
  "encuentra", "localiza", "muéstrame". Caso A del slide 8: trigger
  demasiado específico.

- **Versión 4** (fórmula completa: verbo, abanico de sinónimos, tercera
  persona, contexto del proyecto): activó en cuatro de cuatro pruebas
  con variantes naturales. Y NO activó cuando la petición fue fuera
  de scope (explicar OrdersController). Funciona fiablemente.
```

**Salvo. Cierro VS Code. En la terminal:**

```powershell
git status
```

```
Changes not staged for commit:
        modified:   docs/skills-explorados.md

Untracked files:
        .claude/skills/
```

> "Mirad. El skill experimental está como `untracked`. **Lo borro ahora** para que la rama quede limpia. La 2.2a empezará con la creación del primer skill **propio de verdad** — no este experimental."

**Borro la carpeta del skill:**

```powershell
Remove-Item -Recurse -Force .claude\skills\
```

**Verifico:**

```powershell
git status
```

```
Changes not staged for commit:
        modified:   docs/skills-explorados.md
```

> "Limpio. Ya solo el `docs/skills-explorados.md` con los hallazgos.
>
> Lo commiteo:"

```powershell
git add docs/skills-explorados.md
git commit -m "demo/2.1b: hallazgos del experimento de las 4 versiones"
```

**Tiempo:** ~2 minutos.

---

### Bloque 8 — Recap, las cuatro lecciones y cliffhanger (~2 min)

> "Y eso es la 2.1b. Recap en cuatro lecciones que el alumno se lleva al lunes."

**En el editor o en pantalla:**

```
LECCIÓN 1: La descripción es el switch.
─────────────────────────────────────
Sin descripción concreta, el skill es invisible — instalado, ahí,
pero nunca se activa. Lo habéis visto en V1 y partes de V2.

LECCIÓN 2: La fórmula de los tres ingredientes.
─────────────────────────────────────────────
1) Verbo claro al inicio.
2) Disparadores en abanico (no UN trigger, varios).
3) Tercera persona descriptiva, no imperativo.
La V4 los aplica los tres. La V3 solo los dos primeros parcialmente.

LECCIÓN 3: El truco para iterar.
─────────────────────────────────
Preguntarle al agente "¿qué skill has usado?" tras cada petición.
Es la única forma fiable de saber si el skill se activó. Lo hemos
hecho cuatro veces. Es así de simple.

LECCIÓN 4: La activación es probabilística, no determinista.
─────────────────────────────────────────────────────────
El 100% no es objetivo. La meta es fiable cuando importa. Aceptarlo
es lo que separa al dev que pelea contra la herramienta del que la
usa con criterio.
```

> "Cuatro lecciones. Si las tenéis claras, vais a escribir descripciones decentes desde el primer skill propio.
>
> En la siguiente demo, **2.2a**, empezamos a crear el primer skill propio de verdad. Y no es uno cualquiera: vamos a crear un **generador de componentes Angular standalone** para OrderManagement. Va a ser un skill que vuestro equipo podría usar a diario. Y vamos a aplicar todo lo que hemos visto: anatomía completa, descripción que activa fiablemente, cuerpo ligero, y referencia a `references/` y `assets/` cuando lo necesitemos.
>
> El módulo 2 entra en su parte productiva. Hasta aquí teoría aplicada. A partir de la 2.2a, **construcción real**."

**Tiempo:** ~2 minutos.

---

## 9. Qué resaltar verbalmente

Cinco puntos que **no pueden quedarse sin decir**:

1. **"El skill invisible: instalado, cargado, perfectamente escrito en su cuerpo, y aún así nunca se activa porque la descripción no lo dice cuándo."** — el momento culminante de la demo. Bloque 3 con la V1. Sin esto, el alumno no internaliza la importancia.

2. **"La fórmula de los tres ingredientes: verbo, abanico de disparadores, tercera persona."** — la regla que el alumno repetirá en su cabeza. Bloque 6, recap en bloque 8.

3. **"`¿Qué skill has usado?` — es la única forma fiable de saber si se activó."** — el truco operativo. Bloque 3, repetido en bloques 4-6.

4. **"Caso A del manual: trigger demasiado específico."** — el patrón que más se repite. Bloque 5 con la V3.

5. **"La activación es probabilística. El 100% no es objetivo. La meta es fiable cuando importa."** — la aceptación que evita la frustración. Bloque 6 al final, recap en bloque 8.

**Frase de remate al final:**

> *"Sin descripción decente, el skill es invisible. Con la fórmula de los tres ingredientes, activa fiablemente. La 2.2a vamos a aplicarla en un skill que vuestro equipo podría usar el lunes."*

---

## 10. Slide de entrada (locución HeyGen)

> **Texto del avatar antes del screencast:**

"Y vamos a la siguiente demo. La 2.1b. La pieza más sutil del módulo 2 y donde más se equivoca la gente al escribir su primer skill. Vais a ver un experimento controlado: construyo un skill mínimo experimental sobre OrderManagement con un cuerpo perfecto y le cambio la descripción cuatro veces. Misma petición las cuatro veces. Observamos cuándo se activa y cuándo no. La V1 demuestra el skill invisible — instalado, perfectamente escrito en su cuerpo, y nunca se activa. Las V2 y V3 demuestran los anti-patrones del 'casi funciona'. La V4 aplica la fórmula completa de los tres ingredientes y activa fiablemente con cuatro variantes naturales del vocabulario. Y al final del experimento, borro el skill — la creación de skills propios de verdad empieza en la 2.2a. Vamos al screencast."

---

## 11. Slide de salida (locución HeyGen)

> **Texto del avatar al volver del screencast:**

"Lo que acabáis de ver es la diferencia entre un skill invisible y un skill que activa fiablemente. La descripción es el switch. Cuatro lecciones para llevarse: una, sin descripción concreta el skill es invisible aunque el cuerpo sea perfecto. Dos, la fórmula de los tres ingredientes funciona — verbo claro, disparadores en abanico, tercera persona. Tres, el truco para iterar es preguntar al agente *'¿qué skill has usado?'* tras cada petición. Y cuatro, la activación es probabilística, el 100% no es objetivo, la meta es ser fiable cuando importa. En la siguiente demo, la 2.2a, empezamos a construir el primer skill propio de verdad: un generador de componentes Angular standalone para OrderManagement. Aplicaremos todo lo que habéis visto. Empezamos con el dos punto dos punto A."

---

## 12. Tiempo total estimado

| Bloque | Tiempo |
|---|---|
| Slide de entrada (avatar) | ~30 seg |
| Bloque 1 — Setup y planteamiento | ~1 min 30 seg |
| Bloque 2 — Construir skill V1 | ~2 min |
| Bloque 3 — Test V1: el skill invisible | ~2 min 30 seg |
| Bloque 4 — Iteración V2 | ~2 min |
| Bloque 5 — Iteración V3 | ~2 min |
| Bloque 6 — V4: la fórmula completa | ~3 min 30 seg |
| Bloque 7 — Documentar hallazgos y limpiar | ~2 min |
| Bloque 8 — Recap y cliffhanger | ~2 min |
| Slide de salida (avatar) | ~30 seg |
| **Total screencast** | **~17-18 min** |
| **Total con avatar** | **~18-19 min** |

> Si hay preguntas durante el screencast, súmale 2-3 minutos. La demo encaja en un bloque de **20 minutos** dentro de la sesión.

**Margen de seguridad por si algo va lento:**

- **Si en el bloque 3 (V1) el skill SÍ se activa** (porque el agente decide arriesgar a pesar de la descripción genérica), no fuerces el guion. Comenta: *"a veces el agente arriesga incluso con descripción mala. Pero esto es probabilístico — si lanzo la misma petición tres veces, en algunas no se activará. La descripción genérica es una apuesta, no una garantía"*. Y lanzas dos veces más para mostrar la inconsistencia.

- **Si la V4 falla en alguna prueba** (porque el agente decide no activarla pese a la descripción rica), comenta: *"esto es lo que decía sobre probabilístico. La V4 activa fiablemente, no determinísticamente. Es aceptable. Lo importante es que cuando importa, activa"*. Y lanza otra variante para mostrar que la mayoría sí activa.

- **Si el agente NO te dice qué skill usó** cuando preguntas (responde algo evasivo), reformula: *"sé exhaustivo: dime exactamente qué skills cargó esta petición y cuáles no, y por qué"*. Ese prompt más explícito suele dar respuesta clara.

- **Si crear `.claude/skills/find-handler/` da problemas de path en PowerShell** por el `\`, usa `mkdir .claude/skills/find-handler` con barras normales. PowerShell acepta los dos separadores en Windows moderno.

- **Si el bloque 6 (V4) se hace pesado por las cinco pruebas**, recorta a tres: el verbo principal del trigger ("localiza"), una variante natural ("dónde está"), y la prueba de no-activación ("explica OrdersController"). El bloque queda en 2 min 30 seg sin perder pedagogía.

---

# Apéndice A — Decisiones pedagógicas justificadas

> Esta sección no se graba. Es para Pedro, formador.

**¿Por qué un experimento controlado y no construir un skill bien hecho directamente?**

Porque la lección de la gamma 2.1b — *"la descripción es el switch"* — es **abstracta** hasta que se ve fallar. Si construimos un skill bien hecho desde la primera versión, el alumno ve que funciona pero no entiende **por qué la descripción importa tanto**. El experimento con cuatro versiones materializa el principio: V1 invisible → V2 limitada → V3 casi funciona → V4 fiable. **La progresión enseña**.

**¿Por qué `find-handler` y no otro skill experimental?**

Porque cumple cuatro requisitos pedagógicos:

1. Es **fácil de probar** — basta preguntar por handlers que ya existen.
2. La activación es **lingüísticamente ambigua** — hay muchas formas naturales de pedir "busca el handler" que pueden o no coincidir con el trigger.
3. **No requiere código nuevo** — el cuerpo del skill no implementa nada, solo orquesta `Glob` + `Read`.
4. **Conecta con OrderManagement** — los handlers `CreateOrderHandler`, `CancelOrderHandler`, `UpdateOrderHandler` ya están en el repo desde demos anteriores.

**¿Por qué borrar el skill al final?**

Porque la 2.2a empieza con la **creación del primer skill propio de verdad** — un generador de componentes Angular. Si dejamos `find-handler` en el repo, contaminamos la base. La rama `demo/2.1b` debe quedar **igual que `demo/2.1a` excepto por las notas del experimento**. **Limpieza es disciplina**.

**¿Por qué cuatro versiones y no tres o cinco?**

Tres es muy poco — no se ve la progresión completa. Cinco se hace pesado y aporta poco. Cuatro cubre las tres fases pedagógicas críticas: el extremo malo (V1, anti-patrón puro), las dos zonas grises (V2 y V3, "casi funciona"), y el bueno (V4, fórmula completa). **Cuatro es el número que mantiene tensión narrativa**.

**¿Por qué la prueba de no-activación en el bloque 6 (test 5)?**

Porque sin esta prueba, el alumno podría irse pensando *"vale, la V4 es buena, activa siempre"* — y eso es **otro problema**. Un skill que activa demasiado es tan malo como uno invisible: interrumpe el flujo normal del agente. Mostrar que la V4 **NO** se activa con peticiones fuera de scope demuestra que la descripción tiene **precisión**, no solo amplitud. Equilibrio.

**¿Por qué insistir en el truco "¿qué skill has usado?" cuatro veces?**

Porque es el **único proxy operativo** que el alumno tiene para saber si su skill funciona. La gamma 2.1b lo mencionó pero sin demostración. Repetirlo en cada bloque (V1, V2, V3, V4) hace que se grabe como **hábito de verificación**. El alumno tiene que poder reproducirlo de memoria al lunes.

**¿Por qué la frase "la activación es probabilística"?**

Porque es la **expectativa que más causa frustración** al alumno principiante. Si espera 100% y obtiene 90%, lo vive como fallo. Si acepta que el 90% es la realidad y la meta, lo vive como éxito. La gamma 2.1b lo dijo explícitamente al final del slide 11. Recogerlo en el recap del bloque 8 **gestiona expectativas**.

**¿Por qué los hallazgos del experimento se rellenan en VIVO durante el screencast?**

Porque el commit pre-grabación deja el esqueleto, pero los **resultados reales pueden variar** en cada grabación (la activación es probabilística). Si pre-grabo los hallazgos y luego en la grabación la V3 sí se activa con "encuentra" por casualidad, hay disonancia entre lo escrito y lo visto. **Rellenar en vivo es honesto** — el alumno ve los hallazgos reales que se acaban de producir.

**¿Por qué la descripción de la V4 es tan larga?**

Porque la fórmula completa exige los tres ingredientes con disparadores en abanico, y eso no cabe en pocas palabras. La V4 son ~50 palabras (~350 caracteres). El límite oficial es ~1024 caracteres. **Sigue dentro del rango sano**. Una descripción de tres palabras como la V1 es **imposible** que cubra la fórmula. La concreción tiene un coste mínimo de palabras.

**¿Por qué la V3 menciona el caso A del manual literalmente en el guion?**

Porque la gamma 2.1b dedicó tres slides (8-10) a los tres casos donde "casi funciona". Si la demo no los referencia explícitamente cuando aparecen, el alumno **no conecta** la teoría con lo que ve. Llamar "caso A" al fenómeno que está en pantalla **ata el contenido del curso entre módulos**.
