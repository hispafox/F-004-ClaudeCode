# Demo 1.3b — Permisos en runtime y workflow completo: feature de cancelación de pedidos

> **Versión:** v1 | **Módulo:** 1 | **Sub:** 1.3b | **Estado:** ✅ Versión final
> **Archivo:** `demo_M01-S1.3b-permisos-workflow-completo-windows-v1.md`
> **Branch before:** `demo/1.3b-before`  (estado al hacer `git checkout` antes de grabar — sin endpoint cancel)
> **Branch after:**  `demo/1.3b-after`   (estado final pre-cocinado con la feature de cancelación commiteada)
> **Branch parent:** `demo/1.3a-after`
> **Tiempo total estimado:** ~30-35 minutos
> **Tipo:** Demo culminación del módulo 1 (MIXTA: INFRA + CÓDIGO). **Aquí no se introduce material nuevo — se encadena todo lo aprendido en un workflow real end-to-end.** El alumno ve permisos en runtime con sus 4 opciones, el patrón sano vs los antipatrones A y B, y el patrón 1 (implementación de feature) ejecutado entero: `/plan` al inicio, trabajo con permisos sanos en runtime, `/compact` cuando toca, code review final en modo pipe. Sigue el patrón **before/after** definido en [M0.2](demo_M00-S0.2-patron-before-after-windows-v3.md): durante el screencast el formador implementa la feature de cancelación en vivo desde `demo/1.3b-before`; al cerrar descarta los cambios reales y la siguiente clase parte de `demo/1.3b-after` que tiene la feature pre-cocinada (commit equivalente al que se hizo en vivo).
> **Plataforma:** Windows 11 (PowerShell 7).

---

## 1. Contexto

Llegamos al final del módulo 1. La 1.1 mostró qué hace el agente. La 1.2a lo instaló. La 1.2b le dio contexto del proyecto. La 1.3a enseñó las marchas — modos, slash commands, `/compact`. **Esta demo no añade pieza nueva**. Es la culminación: con todo lo anterior en marcha, ¿cómo se siente trabajar de verdad con Claude Code?

La gamma 1.3b (25 slides, ~25-30 min de teoría) cubre dos bloques claros. Primero, **permisos en runtime** — qué hacer cuando el agente te pide aprobación a mitad de sesión, las 4 opciones del prompt, los dos antipatrones (aprobar todo a ciegas, aprobar todo individualmente), el patrón sano del medio, y los tres casos donde decir **"no"** explícito. Segundo, **workflows del día a día** — los 4 patrones que rinden (implementación de feature, refactor mediano, code review asistido, debugging) y el patrón anti-eficiente que **NO** rinde.

Esta demo aterriza la gamma sobre OrderManagement con un escenario real: **implementación end-to-end de la feature de cancelación de pedidos** — la misma que aparece en el manual 1.3 línea 215 como ejemplo, la misma que el alumno vio asomarse en la demo 1.1 cuando el agente detectó la inconsistencia de `InvalidOperationException`. Aquí la implementamos de verdad.

> **Tipo de demo:** workflow completo end-to-end. **Es la demo más larga del módulo 1** porque es la única que muestra el flujo entero. No hay forma honesta de comprimirla más sin perder la sensación de "esto es trabajo real".

---

## 2. Objetivo de la demo

Cinco cosas concretas que tienen que quedar en la cabeza del alumno cuando termine los ~30 minutos de screencast:

1. **Las 4 opciones del prompt de aprobación.** Allow once, Yes always, No deny, Edit. **Cuándo cada una.** El alumno tiene que poder explicarlo a un colega.

2. **El patrón sano: promover a "always" lo seguro y repetitivo, mantener "once" para lo de riesgo.** No los dos antipatrones — ni Patrón A (todo a ciegas) ni Patrón B (todo individualmente). El del medio.

3. **Los 3 casos donde "no" es la respuesta correcta.** Cuando no entiendes lo que propone, cuando toca algo crítico que no pediste, cuando el plan global no tiene sentido.

4. **El patrón 1 de implementación de feature, encadenado entero.** `claude` interactivo + `/plan` + revisar + aprobar + trabajar con permisos sanos + `/compact` cuando toca + tests + `/usage` antes de cerrar. Es el flujo real de un dev productivo.

5. **El cierre del módulo 1.** El alumno se va con base operativa real: instalado, configurado, con CLAUDE.md, con permisos, con modos y comandos en la mochila. **Listo para usar Claude Code en su trabajo el lunes.**

Y dos cosas que tienen que **NO quedar** en su cabeza:

- *"Tengo que aprobar todo individualmente para mantener el control."* — no, eso es el Patrón B y lleva al abandono. **Promover a always lo seguro y repetitivo es el patrón sano.**
- *"Cuando dude, mejor digo sí."* — al revés. **Cuando dude, "no" y discútelo.** El coste de un "no" es bajo (puedes volver a aprobar). El coste de un "yes" mal dado puede ser alto.

---

## 3. Branch `demo/1.3b-before`

Punto de partida del screencast.

```
demo/1.3b-before
```

**Parte de:** `demo/1.3a-after`.

**Estado del repo:** `CLAUDE.md` con las 5 secciones, `.claude/settings.json` con allow/deny configurados (28 patrones permitidos, 14 denegados), `scripts/audit-staged.sh` como ejemplo del modo one-shot, todas las demos anteriores marcadas en `docs/DEMOS.md`. **El proyecto sigue sin tener endpoint dedicado de cancelación** — esa carencia que el agente detectó en la demo 1.1 sigue ahí. **En esta demo la implementamos en vivo.**

> El formador hace `git checkout demo/1.3b-before` antes de empezar a grabar.

---

## 4. Branch `demo/1.3b-after`

Estado final que la siguiente clase (Módulo 2) asume.

```
demo/1.3b-after
```

**Parte de:** `demo/1.3b-before`.

**Qué añade respecto a `-before`:** la **feature de cancelación de pedidos completa** — nuevo `InvalidOrderStateException` tipado, `CancelOrderHandler` refactorizado para lanzar la excepción tipada en vez de la genérica, nuevo endpoint `POST /api/orders/{id}/cancel` en `OrdersController`, todo respetando las convenciones del CLAUDE.md, y la marca `[x]` en `docs/DEMOS.md`. **Es la primera implementación real de feature del curso** y queda como precedente para todas las demos siguientes.

**Cómo se prepara:** ver §6b. Se materializa antes de grabar — Claude Code en una sesión limpia genera la feature equivalente a la que el formador construirá en directo.

> Durante la grabación, el formador implementa la feature en directo desde `demo/1.3b-before`, hace commit en pantalla (la pieza pedagógica del workflow end-to-end requiere ver el commit). **Al cerrar descarta los commits reales del screencast** y la siguiente clase parte de `demo/1.3b-after` ya pre-cocinada — equivalente a lo que se vio en pantalla, pero reproducible aunque el directo se desvíe.

---

## 5. Estado del repo al hacer `git checkout demo/1.3b-before`

Idéntico a `demo/1.3a-after`:

```
ordermanagement/
├── .claude/
│   └── settings.json                   (allow/deny configurados)
├── docs/
│   └── DEMOS.md                        (1.1, 1.2a, 1.2b, 1.3a marcadas)
├── scripts/
│   └── audit-staged.sh                 (ejemplo one-shot)
├── src/                                (sin cambios — sin endpoint de cancel todavía)
├── frontend/                           (sin cambios)
├── tests/                              (sin cambios — vacío)
├── .gitignore                          (excluye settings.local.json)
├── CLAUDE.md                           (147 líneas, 5 bloques)
└── README.md
```

**El estado clave para esta demo:**

- `OrdersController.cs` tiene 5 endpoints REST estándar **pero NO tiene el endpoint dedicado de cancelación**. La inconsistencia que el agente detectó en la demo 1.1 sigue ahí.
- `CancelOrderHandler.cs` existe en `Application/Handlers/` y tiene la lógica, pero lanza `InvalidOperationException` genérica donde el resto del proyecto usa excepciones tipadas.
- No hay `InvalidOrderStateException` en `Application/Exceptions/`.

**Estado de la máquina Windows del formador:**

```
✅ Claude Code instalado y autenticado
✅ Git for Windows
✅ PowerShell 7
✅ VS Code con el repo cargado en demo/1.3b-before
✅ CLAUDE.md y .claude/settings.json operativos
✅ Sesión limpia (cierra cualquier sesión previa con Ctrl+C)
```

**Lo que el alumno verá al final de la demo:**

- 4 opciones del prompt de aprobación demostradas con casos reales: Allow once, Yes always, No deny, Edit.
- Los 3 casos donde "no" es la respuesta correcta, ejemplificados con propuestas reales del agente.
- El patrón 1 de implementación de feature ejecutado completo: `/plan` al inicio, trabajo con permisos sanos, `/compact` a mitad, tests al final, `/usage` antes de cerrar.
- La feature de cancelación de pedidos implementada en el repo, commiteada.
- Code review final en modo pipe del diff completo antes del PR ficticio.
- El cierre del módulo 1 con recap de la base operativa.

---

## 6a. Prompt para Claude Code — preparar `demo/1.3b-before`

> Crea la rama de partida del screencast desde `demo/1.3a-after`. **No implementa la feature** — la pieza viva es exactamente eso: implementarla en pantalla. La rama `-before` queda idéntica a `demo/1.3a-after` (sin endpoint cancel, sin `InvalidOrderStateException`).

````
Estoy preparando la demo 1.3b del curso de Claude Code (workflow completo
end-to-end). Sigue el patrón before/after (ver demo M0.2).

Quiero que prepares la rama `demo/1.3b-before` desde `demo/1.3a-after`.
Esta rama es el punto de partida del screencast: el repo NO debe tener
el endpoint de cancelación ni la excepción tipada. Esa es la pieza viva
de la demo, la implementaré en directo.

## Tarea única

```powershell
git checkout demo/1.3a-after
git pull
git checkout -b demo/1.3b-before
```

NO crees InvalidOrderStateException, NO toques OrdersController.cs ni
CancelOrderHandler.cs, NO modifiques CLAUDE.md ni docs/DEMOS.md.
Esos artefactos van en `demo/1.3b-after` (ver §6b).

NO hagas commit. La rama `demo/1.3b-before` es exactamente igual a
`demo/1.3a-after` excepto en el nombre.

# Cuando termines, dime

1. Que la rama demo/1.3b-before está creada.
2. Que `git diff demo/1.3a-after demo/1.3b-before` no muestra cambios.
````

---

## 6b. Prompt para Claude Code — preparar `demo/1.3b-after`

> Materializa la rama final con la feature de cancelación pre-cocinada — equivalente a lo que el formador construirá en vivo durante el screencast. Pre-cocinar `-after` garantiza que la siguiente clase parte de un estado conocido y reproducible aunque el directo se desvíe.

````
Estoy preparando la demo 1.3b del curso de Claude Code (workflow
completo end-to-end). Esta rama -after pre-cocina la feature de
cancelación de pedidos que el formador implementará en vivo durante
el screencast desde demo/1.3b-before.

# Contexto

Estoy en la rama `demo/1.3b-before` del repo `ordermanagement`. La rama
parte de `demo/1.3a-after` y tiene todo lo de las demos anteriores
(CLAUDE.md, .claude/settings.json con allow/deny, scripts/audit-staged.sh)
pero NO tiene aún la feature de cancelación.

Quiero que prepares la rama `demo/1.3b-after` desde `demo/1.3b-before`
con la feature de cancelación COMPLETA, respetando las convenciones
del CLAUDE.md y el patrón del resto del proyecto (excepciones tipadas,
PascalCase, MediatR, etc.).

# Lo que necesito

Cuatro tareas:

## Tarea 1: crear la rama

```powershell
git checkout demo/1.3b-before
git checkout -b demo/1.3b-after
```

## Tarea 2: crear la excepción tipada

Crea `src/OrderManagement.Application/Exceptions/InvalidOrderStateException.cs`
siguiendo el patrón de `CustomerNotFoundException` y `OrderNotFoundException`:
hereda de Exception, recibe `orderId` y `currentState` (de tipo
`OrderManagement.Domain.Enums.OrderStatus`) en el constructor, mensaje
formateado tipo "Order {id} is in state {state} and cannot be cancelled.",
ambas propiedades expuestas como `public {get;}`.

## Tarea 3: refactorizar CancelOrderHandler

Modifica `src/OrderManagement.Application/Handlers/CancelOrderHandler.cs`:
donde lanza `InvalidOperationException(...)`, sustitúyelo por
`InvalidOrderStateException(order.Id, order.Status)`. El resto del handler
(la condición `OrderStatus.Pending or OrderStatus.Confirmed`, el
`order.Status = OrderStatus.Cancelled` y el `_orders.UpdateAsync(...)`)
se mantiene igual.

## Tarea 4: añadir endpoint cancel + actualizar DEMOS.md + commit

Añade al final de `OrdersController.cs` un método nuevo:
- `[HttpPost("{id:int}/cancel")]`
- Firma: `public async Task<IActionResult> Cancel(int id, CancellationToken ct)`
- Despacha `new CancelOrderCommand(id)` con `_mediator.Send(...)`.
- try/catch `OrderNotFoundException` → `return NotFound(new { message = ex.Message });`
- try/catch `InvalidOrderStateException` → `return UnprocessableEntity(new { message = ex.Message });`
- Éxito → `return NoContent();` (204)

Actualiza `docs/DEMOS.md`:
```
- [x] **demo/1.3b** — Workflow completo: feature de cancelación end-to-end
```

Verifica con `dotnet build` (0 warnings, 0 errors) y commit:

```powershell
git add src/OrderManagement.Application/Exceptions/InvalidOrderStateException.cs `
        src/OrderManagement.Application/Handlers/CancelOrderHandler.cs `
        src/OrderManagement.Api/Controllers/OrdersController.cs `
        docs/DEMOS.md
git commit -m "demo/1.3b-after: feature de cancelación de pedidos end-to-end"
```

NO hagas push.

# Restricciones (importantes)

- NO añadas tests (los cubre el módulo 5).
- NO modifiques CLAUDE.md, .claude/settings.json, README.md ni .gitignore.
- NO modifiques otros endpoints de OrdersController. Solo añades uno nuevo.
- NO toques los demás handlers ni queries.

# Cuando termines, dime

1. Que la rama demo/1.3b-after está creada desde demo/1.3b-before.
2. Que existe InvalidOrderStateException con el patrón correcto.
3. Que CancelOrderHandler ya lanza la excepción tipada (no la genérica).
4. Que OrdersController tiene el endpoint POST /api/orders/{id}/cancel.
5. Que docs/DEMOS.md está marcado.
6. Que dotnet build pasa limpio.
7. Que el commit está hecho.

Si tienes dudas (por ejemplo, si el namespace exacto difiere de lo que asumo), para y pregúntame.
````

---

## 7. Artefactos que Claude Code debe generar

```
✓ Rama demo/1.3b-before (parte de demo/1.3a-after) — sin cambios respecto al parent
✓ Rama demo/1.3b-after (parte de demo/1.3b-before) con:
  ├── src/OrderManagement.Application/Exceptions/InvalidOrderStateException.cs (nuevo)
  ├── src/OrderManagement.Application/Handlers/CancelOrderHandler.cs (refactorizado: lanza InvalidOrderStateException)
  ├── src/OrderManagement.Api/Controllers/OrdersController.cs (nuevo endpoint POST /api/orders/{id}/cancel)
  └── docs/DEMOS.md con 1.3b marcada como [x]
✓ Verificación de build OK: dotnet build limpio
✓ Commit en demo/1.3b-after: "demo/1.3b-after: feature de cancelación de pedidos end-to-end"
```

**Lo que NO debe haber generado:**

- ❌ La implementación de la feature de cancelación (eso se hace en vivo)
- ❌ Cambios en `OrdersController.cs`, `CancelOrderHandler.cs`
- ❌ La excepción `InvalidOrderStateException.cs`
- ❌ Tests
- ❌ Cambios en `CLAUDE.md` o `.claude/settings.json`
- ❌ Cambios en `README.md` o `.gitignore`

> **Esto es lo más importante de toda la preparación de esta demo.** Si Claude Code se anticipa e implementa la feature, la demo pierde su razón de ser. **Tarea estricta y única: marcar `docs/DEMOS.md`.**

**Lo que el formador commitea EN VIVO sobre `demo/1.3b-before` durante el screencast:**

```
Durante la grabación, sobre demo/1.3b-before, se hace un commit ficticio:
- "demo/1.3b-after: implementa endpoint POST /api/orders/{id}/cancel"
  └── src/OrderManagement.Application/Exceptions/InvalidOrderStateException.cs (NUEVO)
  └── src/OrderManagement.Application/Handlers/CancelOrderHandler.cs (MODIFICADO)
  └── src/OrderManagement.Api/Controllers/OrdersController.cs (MODIFICADO)

Al cerrar el screencast: el formador DESCARTA estos cambios
(`git reset --hard demo/1.3a-after` o equivalente). La siguiente
clase parte de demo/1.3b-after (pre-cocinada en §6b) que es
equivalente al resultado del screencast.
```

**Estado final del árbol después del screencast (no del prompt):**

```
ordermanagement/
├── .claude/
│   └── settings.json
├── docs/
│   └── DEMOS.md                              ← MODIFICADO (pre-grabación)
├── scripts/
│   └── audit-staged.sh
├── src/
│   ├── OrderManagement.Api/
│   │   └── Controllers/
│   │       └── OrdersController.cs           ← MODIFICADO (en vivo)
│   ├── OrderManagement.Application/
│   │   ├── Exceptions/
│   │   │   ├── CustomerNotFoundException.cs
│   │   │   ├── OrderNotFoundException.cs
│   │   │   └── InvalidOrderStateException.cs ← NUEVO (en vivo)
│   │   └── Handlers/
│   │       └── CancelOrderHandler.cs         ← MODIFICADO (en vivo)
│   └── OrderManagement.Domain/               ← sin cambios
├── frontend/
├── tests/
├── .gitignore
├── CLAUDE.md
└── README.md
```

---

## 8. Guion del screencast

**Duración estimada del screencast: ~28-30 minutos.**

Once bloques. Esta es la demo más densa del curso. Conviene grabar con buena energía y dejarla como pieza ordenada.

> **Antes de empezar a grabar**, asegúrate de:
> - Tener PowerShell 7 abierto en pantalla completa, fuente al menos 16pt.
> - Tener VS Code abierto al lado con el repo `ordermanagement` cargado en `demo/1.3b-before` (la rama de partida del screencast, **no en `demo/1.3b-after`** que ya tiene la feature pre-cocinada).
> - **Importante:** la rama `demo/1.3b-before` debe estar idéntica a `demo/1.3a-after` — sin la feature, sin la marca `[x]` en `docs/DEMOS.md`. Verifica antes:
>   ```powershell
>   git checkout demo/1.3b-before
>   ls src/OrderManagement.Application/Exceptions/
>   # Debe mostrar SOLO CustomerNotFoundException.cs y OrderNotFoundException.cs
>   # NO InvalidOrderStateException.cs
>   ```
> - Cerrar Slack, Teams, navegadores con notificaciones.
> - Tener **dos terminales** PowerShell abiertas: una para Claude Code interactivo, otra para git/dotnet en paralelo.

---

### Bloque 1 — Setup, recap del módulo, y planteamiento (~2 min)

**Pantalla compartida.** A la izquierda, VS Code con el repo en `demo/1.3b-before`. A la derecha, dos terminales PowerShell limpias.

**En la primera terminal:**

```powershell
git status
git log --oneline -10
```

**Aparece algo como:**

```
On branch demo/1.3b-before
nothing to commit, working tree clean

def5678 (HEAD -> demo/1.3b-before, demo/1.3a-after) demo/1.3a-after: script de ejemplo para audit pre-commit
ghi9012 (demo/1.2b-after) demo/1.2b-after: CLAUDE.md, settings.json y permisos para OrderManagement
jkl3456 (demo/1.2a) demo/1.2a: marca demo de instalación como completada
mno7890 (demo/1.1) demo/1.1: README actualizado y registro de demos creado
```

**Lo que dices:**

> "Llegamos al final del módulo 1. Repaso rápido del recorrido — mirad el `git log`. Cinco demos. Cinco ramas encadenadas.
>
> La 1.1 nos enseñó qué hace Claude Code — el ciclo agentic en acción. La 1.2a lo instaló. La 1.2b le dio contexto del proyecto, el `CLAUDE.md` y los permisos del `settings.json`. La 1.3a nos enseñó las marchas — modos, slash commands, `/compact`.
>
> Esta demo, la 1.3b, es la culminación. **No introducimos material nuevo.** Encadenamos todo lo aprendido en un workflow real. Vais a ver dos cosas:
>
> Primero, **permisos en runtime**. Las 4 opciones del prompt cuando el agente os pide aprobación. Allow once, Yes always, No deny, Edit. Cuándo cada una. Y los tres casos donde 'no' es la respuesta correcta.
>
> Segundo — y es la pieza fuerte — **el patrón 1 de implementación de feature ejecutado entero**. La gamma 1.3b lo cubrió en el slide 15. Aquí lo vais a ver de verdad. ¿Qué feature? La que el agente detectó como 'falta' en la demo 1.1: el endpoint dedicado de cancelación de pedidos. La implementamos hoy. **Y los cambios se commitean** — esta es la primera demo del curso donde la rama queda con código de feature real.
>
> Vamos."

**Tiempo:** ~2 minutos.

---

### Bloque 2 — Las 4 opciones del prompt: vista general (~2 min)

> "Antes de meternos en el workflow, repaso de las 4 opciones del prompt. La gamma 1.3b lo cubrió en el slide 5. Cuando Claude Code va a usar una herramienta o lanzar un comando que **no está en allow ni en deny**, te muestra esto:"

**Abro un editor de texto al lado y escribo el ejemplo (no es del agente, es para ilustración):**

```
The agent wants to run:
  Bash: dotnet ef migrations add AddCancelOrderColumn

[A]llow once  [Y]es, allow always  [N]o, deny  [E]dit
>
```

**Recorrer las 4 opciones con el cursor mientras hablas:**

> "Las 4 opciones, una a una.
>
> **`A` — Allow once.** Permite esta vez. **No cambia tu configuración.** El comando se ejecuta una vez y la próxima te volverá a preguntar. **Es la opción más segura cuando dudas.** Si el comando es algo que no haces todos los días — `dotnet ef migrations`, por ejemplo — esta es la elección sensata.
>
> **`Y` — Yes, allow always.** Permite y **añade el patrón a tu allow**. La sesión y las futuras lo permiten sin preguntar. Útil para **comandos que sabes que vas a usar mucho**. La gamma slide 9 dijo: 'promover a always lo seguro y repetitivo'.
>
> **`N` — No, deny.** Bloquea esta vez. El agente busca alternativas o te dice que no puede continuar. **Importante: el `N` aquí no añade al `deny` permanente** — eso requiere editar el `settings.json`. El `N` aquí es 'no esta vez'.
>
> **`E` — Edit.** Modificas la propuesta antes de aprobarla. Útil cuando *'sí, lánzalo, pero con un parámetro distinto'*. Por ejemplo, el agente propone `dotnet test --filter Category=Integration` y tú quieres `dotnet test --filter Category=Unit`. Editas y apruebas.
>
> **El error más común del primer día** lo cubrió la gamma en los slides 6, 7 y 8. Vamos a verlo."

**Tiempo:** ~2 minutos.

---

### Bloque 3 — Los dos antipatrones y el patrón sano (~2 min 30 seg)

> "Dos antipatrones que se ven **muchísimo** en la primera semana. Y el patrón del medio que es el sano. Os los recuerdo de la gamma."

**En el editor de texto, escribes (es contenido pedagógico, no comando):**

```
PATRÓN A — Aprobar todo a ciegas
└── "Sí, sí, sí, sí..."
   Yes always a todo lo que se pone delante.
   La fricción de aprobar les molesta.

   Resultado: se cargan el modelo de seguridad.
   Un día el agente decide hacer algo que no querían
   y resulta que ya tienen permiso.
```

> "Patrón A. La fricción de aprobar les molesta y le dan a 'Yes always' a todo. **Se cargan el modelo de seguridad.** El día menos pensado, el agente decide que la mejor forma de resolver un conflicto es algo que no querían — y ya tiene permiso. La gamma slide 7."

**Continúas escribiendo:**

```
PATRÓN B — Aprobar todo individualmente sin promover a "always"
└── "Mejor lo controlo cada vez"
   Allow once siempre, nunca promueven a always.

   Resultado: aprueban 200 veces el mismo
   Bash(dotnet test) en una sesión.
   Y al final acaban frustrados con la herramienta.
```

> "Patrón B. *'Mejor lo controlo cada vez'*. **Aprueban doscientas veces el mismo `dotnet test`** en una sesión. Acaban frustrados, no por la herramienta sino por su elección. La gamma slide 8."

```
PATRÓN SANO (slide 9) — el del medio

PROMOVER A "ALWAYS" lo que es seguro y repetitivo:
   Bash(dotnet test)
   Bash(npm run *)
   Read, Edit

MANTENER "ONCE" para lo que tiene riesgo:
   Bash(rm *)
   Bash(git push *)
   Escrituras en ficheros sensibles
```

> "El patrón sano. **El del medio.**
>
> Promover a 'always' lo seguro y repetitivo. `dotnet test`. `npm run *`. `Read`, `Edit`. Las cosas que vais a aprobar mil veces si las pongo en `once` y nunca van a hacer daño.
>
> Mantener 'once' para lo que tiene riesgo. `rm`. `git push`. Escrituras en ficheros sensibles. Cosas que **conviene parar a pensar cada vez**.
>
> La regla mnemotécnica: **frecuente y seguro → always. Raro o de riesgo → once.**
>
> Y los 3 casos donde la respuesta correcta es **`N` — Deny**:"

**Continúas:**

```
3 CASOS DONDE "NO" ES LA RESPUESTA CORRECTA (slides 10-12):

1. El agente propone algo que NO entiendes
   "Voy a ejecutar este comando que parece raro..."
   → Si no puedes razonar lo que hace, NO LO APRUEBES.
   → Cuestiónaselo: "¿por qué necesitas eso?"

2. El agente quiere modificar algo CRÍTICO que no pediste
   "Voy a editar appsettings.Production.json..."
   → Aunque la justificación parezca razonable.
   → Si tú no lo pediste, "no". Y discútelo después.

3. Te das cuenta de que el agente va POR MAL CAMINO
   El plan global no tiene sentido.
   → Bloquear y replantear la tarea.

HEURÍSTICA: cuando dudes, di "no" y discútelo.
El coste de un "no" es bajo (puedes volver a aprobar).
El coste de un "yes" mal dado puede ser alto.
```

> "Tres casos. **Cuando dudes, 'no' y discútelo.** Esa es la heurística. **El coste de un 'no' es bajo. Puedes volver a aprobar después si te convence. El coste de un 'yes' mal dado puede ser alto.**
>
> Vamos a ver esto en acción. Implementamos la feature."

**Tiempo:** ~2 minutos 30 segundos.

---

### Bloque 4 — Patrón 1: arranque con `/plan` (~3 min)

> "Patrón 1: implementación de feature. La gamma slide 15. Vamos a implementar el endpoint dedicado de cancelación de pedidos. Es la inconsistencia que el agente detectó en la demo 1.1.
>
> Recordáis lo que veíamos en el código: existe `CancelOrderHandler` con la lógica, pero **no hay endpoint dedicado**. Y el handler lanza `InvalidOperationException` genérica donde el resto del proyecto usa excepciones tipadas. Lo arreglamos hoy."

**Arranco Claude Code en la primera terminal:**

```powershell
claude
```

```
 Welcome to Claude Code v2.1.x

 cwd: C:\Users\pedro\projects\ordermanagement
 model: claude-opus-4.7
 ✓ CLAUDE.md loaded (147 lines)
 ✓ Project settings loaded (28 allow patterns, 14 deny patterns)
 Type / for commands, ? for help

>
```

> "Banner conocido. CLAUDE.md cargado, settings cargados. Veintiocho permisos en allow, catorce en deny. Vamos."

**Activo `/plan` primero:**

```
/plan
```

```
✓ Planning mode enabled.
The agent will propose a plan before executing any changes.
```

> "**`/plan` activo.** La gamma slide 22 lo dijo: si la tarea va a tocar más de 3 ficheros, `/plan`. Esta va a tocar mínimo tres — el handler, el controller, una excepción nueva. Activo el modo plan antes de pedir nada."

**Hago la petición:**

```
> Quiero implementar la feature de cancelación de pedidos completa.
  Tres cosas:
  
  1. Crear una excepción tipada InvalidOrderStateException en
     src/OrderManagement.Application/Exceptions/. Debe seguir el
     mismo patrón que CustomerNotFoundException y OrderNotFoundException.
  
  2. Refactorizar CancelOrderHandler para que lance esa excepción
     nueva en lugar de la InvalidOperationException genérica que usa
     ahora.
  
  3. Añadir el endpoint POST /api/orders/{id}/cancel al OrdersController.
     Debe llamar al handler refactorizado. 404 si el pedido no existe,
     422 si el estado no permite cancelar, 204 NoContent en éxito.
  
  Sigue las convenciones del CLAUDE.md.
```

**Aparecen los `Read` (fase de lectura del agente):**

```
● Read(src/OrderManagement.Application/Handlers/CancelOrderHandler.cs)
● Read(src/OrderManagement.Application/Exceptions/CustomerNotFoundException.cs)
● Read(src/OrderManagement.Application/Exceptions/OrderNotFoundException.cs)
● Read(src/OrderManagement.Api/Controllers/OrdersController.cs)
● Read(src/OrderManagement.Domain/Entities/Order.cs)
● Glob(src/**/CancelOrderCommand.cs)
● Read(src/OrderManagement.Application/Commands/CancelOrderCommand.cs)
```

> "Lee los ficheros que necesita. Las excepciones existentes para ver el patrón. El handler para entender el estado actual. El controller para ver cómo se construyen los endpoints. La entidad Order para conocer los estados. Y busca con Glob si existe el comando — sí existe, lo lee."

**Aparece el plan:**

```
Plan para implementar la feature de cancelación de pedidos:

He examinado el código y aquí está mi plan, paso a paso.

1. **Crear InvalidOrderStateException**
   src/OrderManagement.Application/Exceptions/InvalidOrderStateException.cs
   - Hereda de Exception (consistente con CustomerNotFoundException y
     OrderNotFoundException).
   - Constructor recibe orderId (int) y currentState (OrderStatus).
   - Message formateado: "Order {orderId} is in state {currentState} 
     and cannot be cancelled."
   - Propiedades públicas OrderId y CurrentState para introspección.

2. **Refactorizar CancelOrderHandler**
   src/OrderManagement.Application/Handlers/CancelOrderHandler.cs
   - Línea actual: throw new InvalidOperationException(...)
   - Cambio: throw new InvalidOrderStateException(orderId, order.Status)
   - Sin cambios en la lógica de validación de estados.

3. **Añadir endpoint POST /api/orders/{id}/cancel**
   src/OrderManagement.Api/Controllers/OrdersController.cs
   - [HttpPost("{id:int}/cancel")]
   - public async Task<IActionResult> Cancel(int id, CancellationToken ct)
   - Llama _mediator.Send(new CancelOrderCommand(id), ct)
   - try/catch OrderNotFoundException → return NotFound(new { message })
   - try/catch InvalidOrderStateException → return UnprocessableEntity(...)
   - Éxito: return NoContent() (204)

4. **Verificar el build**
   - dotnet build (debe compilar sin warnings)

¿Procedo con todo el plan?
```

> "Aquí está el plan. **Cuatro pasos limpios.** Mirad el detalle.
>
> Paso uno: la excepción nueva. **Hereda de `Exception`**, **constructor con `orderId` y `currentState`**, mensaje formateado claro. **Esto sigue el patrón de las dos excepciones que ya hay** — el agente las ha leído y replica.
>
> Paso dos: refactor del handler. Solo cambia la línea del `throw`. **No toca la lógica de validación.** Bien.
>
> Paso tres: el endpoint. `[HttpPost(\"{id:int}/cancel\")]` con `:int` constraint. Captura las dos excepciones específicas, devuelve 404 y 422. Éxito devuelve **204 NoContent — el código HTTP correcto** para una operación que no devuelve cuerpo. **Decisión técnica del agente, no se la dije yo.**
>
> Paso cuatro: el build de verificación. Lo hace por iniciativa propia.
>
> El plan está bien. Le digo que proceda."

**Tiempo:** ~3 minutos.

---

### Bloque 5 — Permisos en runtime durante la ejecución (~5 min)

**Tecleo:**

```
> Procede con el plan completo.
```

**Claude Code empieza a ejecutar. Aparecen las herramientas:**

```
● Write(src/OrderManagement.Application/Exceptions/InvalidOrderStateException.cs)
```

> "Write. Crea la excepción nueva. **No me ha pedido permiso** porque `Write` está en `allow` del `settings.json` que configuramos en la 1.2b."

```
● Edit(src/OrderManagement.Application/Handlers/CancelOrderHandler.cs)
```

> "Edit. Modifica el handler. **Tampoco me pide permiso** — `Edit` también en allow. Va fluido."

```
● Edit(src/OrderManagement.Api/Controllers/OrdersController.cs)
```

> "Edit el controller para añadir el endpoint."

**Y ahora viene el primer prompt de aprobación:**

```
The agent wants to run:
  Bash: dotnet build

[A]llow once  [Y]es, allow always  [N]o, deny  [E]dit
>
```

> "Para. **Aquí me pide permiso.** Pero esto es interesante — `Bash(dotnet build)` **sí está en mi allow**. ¿Por qué me pregunta?"

**Examino con `/permissions` en otra terminal o miro el `settings.json`:**

> "Espera, lo miro:"

**En otra terminal:**

```powershell
cat .claude/settings.json | findstr "dotnet build"
```

```
"Bash(dotnet build)",
```

> "Sí está. Ah, espera. **Me pregunta porque la versión actual del agente todavía pide confirmación la primera vez aunque esté en allow** — depende de cómo Claude Code interprete las settings. **Lo apruebo con `Y` — Yes always**. Si vuelve a pedirlo en esta sesión, no me preguntará. **Comando frecuente y seguro: always.**"

**Tecleo:**

```
y
```

```
✓ Promoted to allow list for this session.

● Bash(dotnet build)

Microsoft (R) Build Engine version 17.10.0...
Restore complete (0.5s)
  OrderManagement.Domain succeeded (0.4s)
  OrderManagement.Application succeeded (0.6s)
  OrderManagement.Infrastructure succeeded (0.8s)
  OrderManagement.Api succeeded (1.2s)

Build succeeded.
    0 Warning(s)
    0 Error(s)
```

**Aparece el resumen del agente:**

```
He implementado la feature de cancelación de pedidos siguiendo el plan
acordado. Resumen.

Nuevos ficheros:
- src/OrderManagement.Application/Exceptions/InvalidOrderStateException.cs

Ficheros modificados:
- src/OrderManagement.Application/Handlers/CancelOrderHandler.cs
- src/OrderManagement.Api/Controllers/OrdersController.cs

Build: 0 warnings, 0 errors.

¿Quieres que probemos el endpoint con un curl, o pasamos a otra cosa?
```

> "Listo. Tres ficheros tocados, build verde. Vamos a hacer una cosa: vamos a simular **uno de los tres casos del 'no' deliberado** que vimos en la gamma slide 11. Le voy a pedir algo crítico que **no le pedí**, a ver qué pasa."

**Tecleo:**

```
> Aprovecha que estás dentro del controller y añade autenticación
  con [Authorize] al método de cancelación. Y mientras estás ahí,
  añade [Authorize(Roles = "Admin")] a TODOS los endpoints del
  controller, no solo al cancel.
```

**Esto es deliberadamente abierto, fuera del scope. El agente va a proponerlo. Y yo voy a decir "no".**

**El agente responde con un plan:**

```
Plan para añadir autenticación al OrdersController:

Antes de actuar, quiero confirmarte algo. Tu petición tiene dos partes
y la segunda es de impacto amplio.

1. Añadir [Authorize] al método Cancel (alcance: 1 endpoint).
2. Añadir [Authorize(Roles = "Admin")] a los OTROS 5 endpoints
   del controller (alcance: cambio de seguridad en endpoints existentes
   que actualmente son públicos).

La parte 2 es un cambio de seguridad significativo en endpoints que
hasta ahora no requerían autenticación. Si esto es producción, podría
romper integraciones existentes que llaman GET /api/orders sin token.

¿Confirmas que quieres aplicar Authorize Admin a los 5 endpoints
existentes, o solo querías al endpoint nuevo de cancelación?
```

> "Mirad. **El propio agente me está cuestionando la petición.** Bien. Esto es lo que la gamma slide 11 pidió que el alumno haga: **el agente quiere modificar algo crítico que no pediste explícitamente** — la autenticación de los endpoints existentes — **y aunque la justificación parezca razonable, si tú no lo pediste, 'no'**.
>
> Y esto es un buen ejemplo del **caso 2 del slide 11**: 'el agente quiere modificar algo crítico que no has dicho que toque'. Aunque mi petición original era ambigua, **lo correcto es decir 'no' y discutirlo**."

**Tecleo:**

```
> No. Solo quería el endpoint de cancel. Olvida el cambio a los
  otros 5 endpoints. Y para el de cancel, retiremos también el
  Authorize por ahora — la gestión de auth la abordamos en otra 
  sesión. Vamos a quedarnos como está.
```

```
Entendido. No toco ningún endpoint. La feature de cancelación queda
implementada sin Authorize, igual que el resto del controller. Si
quieres añadir auth en el futuro, lo abordamos como tarea separada.
```

> "Perfecto. **Caso 2 del slide 11 ejecutado en directo.** El agente propuso algo amplio, dudé, dije 'no'. **No he perdido nada.** La feature de cancelación está implementada. La autenticación queda fuera de scope, como debía ser desde el principio."

**Tiempo:** ~5 minutos.

---

### Bloque 6 — `/compact` cuando toca (~3 min)

> "Antes de seguir, vamos a comprobar el `usage`. La sesión ya lleva unos cuantos prompts y reads."

**Tecleo:**

```
/usage
```

```
Current session usage:

  Context:        38,400 / 200,000 tokens (19.2%)
```

> "Diecinueve por ciento. No estoy crítico todavía. Pero la regla mnemotécnica de la 1.3a era: **`/compact` cada veinte o treinta minutos de trabajo activo, no esperar a que se vuelva tonto**. Llevo unos veinte minutos. Toca."

**Tecleo:**

```
/compact "conserva las decisiones de implementación de la feature de cancelación de pedidos: la nueva excepción InvalidOrderStateException, el refactor del handler, y el endpoint POST /api/orders/{id}/cancel con sus códigos HTTP. Y conserva que decidí NO añadir Authorize ni a Cancel ni a los demás endpoints."
```

```
Compacting conversation...

Original context: 38,400 tokens
Compacted to:    3,800 tokens (9.9% of original)

Summary preserved:

The session implemented the cancel order feature in OrderManagement
following the CLAUDE.md conventions. Three changes:

- New InvalidOrderStateException in Application/Exceptions/, mirroring
  the pattern of CustomerNotFoundException (constructor with orderId
  and currentState, formatted message, public properties for
  introspection).
- CancelOrderHandler refactored: throw InvalidOrderStateException
  instead of generic InvalidOperationException. Validation logic
  unchanged.
- New endpoint POST /api/orders/{id:int}/cancel in OrdersController,
  catches both OrderNotFoundException (404) and InvalidOrderStateException
  (422), success returns 204 NoContent.

Build verified clean (0 warnings, 0 errors).

Decision: Authorize attribute NOT added to the cancel endpoint nor to
the other 5 endpoints. Auth management is deferred to a separate session.
The original ambiguous request to "add auth to all 5 existing endpoints"
was rejected explicitly by the user as out of scope.

Files in current state:
- src/OrderManagement.Application/Exceptions/InvalidOrderStateException.cs (NEW)
- src/OrderManagement.Application/Handlers/CancelOrderHandler.cs (MODIFIED)
- src/OrderManagement.Api/Controllers/OrdersController.cs (MODIFIED with new endpoint)

Pending: commit the changes, run a sanity check, optional code review
in pipe mode before pretending to push the branch.
```

> "Mirad la compactación. **De 38.400 a 3.800 tokens.** Casi el diez por ciento del original. Y el resumen es **utilísimo**:
>
> Conserva las tres decisiones de implementación con detalle.
> Conserva que **rechacé añadir Authorize**. Eso lo añadí explícitamente al `/compact` por si en próximos prompts el agente intentara reproponérmelo.
> Lista los ficheros en el estado actual.
> Marca lo pendiente: commitear, sanity check, code review en pipe.
>
> **El siguiente prompt va a partir de este resumen, no de toda la conversación previa.** El agente sigue sabiendo exactamente dónde está."

**Verifico con `/usage`:**

```
/usage
```

```
Context: 3,800 / 200,000 tokens (1.9%)
```

> "Del diecinueve al dos por ciento. Listo para seguir."

**Tiempo:** ~3 minutos.

---

### Bloque 7 — Tests rápidos y commit (~3 min)

> "Vamos a verificar que el código realmente funciona como dice. La gamma 1.3b slide 15 incluyó los tests en el patrón 1, paso 7. **Los tests reales del módulo 5**, así que aquí solo hago una verificación rápida de smoke testing — que arranca la API y el endpoint responde."

**Pido al agente que verifique con un comando concreto:**

```
> Lanza la API y haz una llamada de smoke test al endpoint de cancel
  con un orderId inválido para ver que devuelve 404. Usa curl o
  Invoke-RestMethod, lo que prefieras.
```

**El agente piensa unos segundos y responde:**

```
Para hacer el smoke test necesito arrancar la API en background y
hacer la llamada. Te propongo dos opciones:

OPCIÓN A: Verificación en frío (sin arrancar API).
- Reviso que el código del endpoint compila y que la lógica de error
  está correcta. Inspección estática.

OPCIÓN B: Arranco dotnet run en background y hago la petición HTTP.
- Implica que yo controle un proceso en background y haga peticiones
  externas. Tu settings.json tiene Bash(dotnet run*) en allow así
  que es viable, pero conviene confirmar.

¿Cuál prefieres?
```

> "Mirad qué bien. **El agente me está dando las dos opciones y me pregunta.** Por respeto al modelo de seguridad. Para esta demo, opción A está bien — no necesitamos arrancar la API real."

**Tecleo:**

```
> Opción A. Inspección estática.
```

**El agente responde:**

```
Inspección estática del endpoint Cancel en OrdersController.cs:

✓ Atributo [HttpPost("{id:int}/cancel")] - constraint :int correcto.
✓ Firma: public async Task<IActionResult> Cancel(int id, CancellationToken ct)
  - parámetro de ruta correctamente bindeado, CancellationToken propagado.
✓ Llamada al handler: await _mediator.Send(new CancelOrderCommand(id), ct)
  - CancelOrderCommand existente, sin tocar.
✓ Manejo de excepciones:
  - catch (OrderNotFoundException) → return NotFound(new { message = ex.Message })
    Devuelve 404. Si lanzo cancel con orderId=99999 (inexistente), el handler
    lanzará OrderNotFoundException, el controller lo captura, devuelve 404.
  - catch (InvalidOrderStateException) → return UnprocessableEntity(...)
    Devuelve 422. Si el pedido existe pero está en estado Shipped o Cancelled,
    InvalidOrderStateException será lanzada por el handler refactorizado,
    capturada aquí, devuelve 422.
✓ Caso éxito: return NoContent() (204) - código correcto para operación
  sin cuerpo de respuesta.

Conclusión: el endpoint está implementado correctamente. La lógica de
errores cubre los dos casos (no encontrado y estado inválido) con códigos
HTTP correctos. El flujo entero compila y respeta las convenciones del
CLAUDE.md.

Tests reales (xUnit + NSubstitute + FluentAssertions) los abordaremos
en el módulo 5 según el plan del curso.
```

> "Excelente. Inspección estática completa. **El agente recuerda incluso que los tests reales son del módulo 5** — eso lo recoge del CLAUDE.md, sección de 'Estado actual'.
>
> Vamos a commitear los cambios. Esto es la primera demo del curso donde **commiteamos código de feature real**. Salgo del agente."

**Salgo (Ctrl+C) y voy a la otra terminal:**

```powershell
git status
```

```
On branch demo/1.3b
Changes not staged for commit:
        modified:   src/OrderManagement.Api/Controllers/OrdersController.cs
        modified:   src/OrderManagement.Application/Handlers/CancelOrderHandler.cs

Untracked files:
        src/OrderManagement.Application/Exceptions/InvalidOrderStateException.cs
```

> "Tres ficheros. Los stageé y commit:"

```powershell
git add src/
git commit -m "demo/1.3b-after: implementa endpoint POST /api/orders/{id}/cancel con excepción tipada"
```

```
[demo/1.3b-before xyz1234] demo/1.3b-after: implementa endpoint POST /api/orders/{id}/cancel con excepción tipada
 3 files changed, 64 insertions(+), 1 deletion(-)
 create mode 100644 src/OrderManagement.Application/Exceptions/InvalidOrderStateException.cs
```

> "Commit hecho. Igual que en la demo 1.1, este commit lo voy a descartar al cerrar — el código que veis acaba de quedar en `demo/1.3b-before` solo durante la grabación. La rama `demo/1.3b-after`, ya pre-cocinada, tiene exactamente esta misma feature commiteada de forma reproducible. **Es la primera implementación real de feature del curso, y todas las demos siguientes la tendrán a través de `demo/1.3b-after`.**"

**Tiempo:** ~3 minutos.

---

### Bloque 8 — Code review final en modo pipe antes del PR (~3 min)

> "Y para cerrar el patrón 1 con todas las marchas, **code review pre-PR en modo pipe**. La gamma 1.3a lo enseñó, la gamma 1.3b lo recoge en el patrón 3 del slide 17. **Antes de subir el PR, una segunda mirada del agente.**"

**En la terminal:**

```powershell
git diff HEAD~1 HEAD | claude -p "Audita estos cambios pre-PR. Busca: bugs, problemas de seguridad, violaciones de las convenciones del CLAUDE.md, código que falta (tests, validación), y cualquier cosa que un revisor humano pediría en code review. Lista cada hallazgo con fichero, severidad, descripción y sugerencia."
```

> "**`git diff HEAD~1 HEAD`** es el diff del último commit. **Lo pipeo a `claude -p`** con instrucciones específicas. Y veamos qué encuentra."

**Aparece el análisis:**

```
Auditoría del diff del commit demo/1.3b-after (cancelación de pedidos):

✓ HALLAZGOS POSITIVOS:

1. InvalidOrderStateException sigue el patrón de las excepciones existentes:
   constructor con orderId + currentState, mensaje formateado, propiedades
   públicas. Coherente con CustomerNotFoundException.

2. El refactor del CancelOrderHandler es minimalmente invasivo: solo cambia
   la línea del throw, no toca la lógica de validación. Bien.

3. El endpoint POST /{id:int}/cancel usa el constraint :int y devuelve los
   códigos HTTP correctos (204 NoContent en éxito).

⚠ HALLAZGOS A CONSIDERAR:

1. SEVERIDAD MEDIA: El endpoint Cancel no tiene documentación XML.
   Igual que el resto del controller, pero conviene anotar para cuando
   se aborde la documentación de Swagger en una feature futura.

2. SEVERIDAD MEDIA: No hay tests para el endpoint nuevo. Lo entiendo en
   el contexto del curso (módulo 5), pero en un PR real este sería un
   bloqueante. Mínimo conviene un test que verifique:
   - 404 cuando orderId no existe.
   - 422 cuando el estado del pedido no permite cancelar.
   - 204 cuando se cancela correctamente.

3. SEVERIDAD BAJA: La InvalidOrderStateException no override
   ToString() ni implementa ISerializable. Si la excepción cruza el
   límite de proceso (mensajería, microservicios), conviene serializarla
   correctamente. En contexto monolito como ahora, no es bloqueante.

✗ HALLAZGOS CRÍTICOS:

   Ninguno.

CONCLUSIÓN: El cambio está bien implementado y respeta las convenciones
del CLAUDE.md. Los hallazgos de severidad media son recordatorios
(documentación XML y tests) que conviene abordar en otra iteración.
Code review aprobado para merge si los hallazgos medios se aceptan
como deuda técnica conocida.
```

> "Mirad esto. Esto es **una segunda mirada profesional del PR en treinta segundos**.
>
> Tres tipos de hallazgos. Positivos, a considerar, críticos. **Cero críticos.** Dos medios — falta documentación XML y faltan tests. Y una nota baja sobre serialización de la excepción.
>
> En un PR real, este output **se pegaría como comentario al PR** antes de pedirle review a un humano. El humano va con el contexto ya. **Code review en treinta segundos. Antes de molestar a un compañero.**
>
> Esta es la última pieza del patrón 1. La gamma 1.3a slide 11 lo dijo: *'el modo pipe es donde Claude Code deja de sentirse como una herramienta interactiva y empieza a sentirse como una pieza de tu infraestructura'*. Acabáis de verlo."

**Tiempo:** ~3 minutos.

---

### Bloque 9 — Mención al modo autónomo (~1 min 30 seg)

> "Antes de cerrar, **un recordatorio que la gamma 1.3b slide 13 marcó explícitamente**. El modo autónomo. Ya lo vimos en la 1.2b en detalle, pero conviene volver a tocarlo aquí porque es la regla que más daño puede causar si se ignora."

**En el editor de texto, escribo (no lo ejecuto):**

```powershell
claude --dangerously-skip-permissions
```

> "Esta flag se salta TODO el modelo de permisos. Allow, deny, todo. El agente puede hacer cualquier cosa sin preguntar.
>
> En el contexto de **sesiones interactivas en vuestro portátil de trabajo: NUNCA**.
>
> Solo en sandbox aislado o CI controlado. La gamma slide 13 lo dijo. Lo repito aquí porque sé que en algún momento algún dev vais a ver la flag y a pensar *'esto va más rápido sin permisos'*. Y va más rápido, sí. Hasta el día que el agente decide hacer `git push --force origin main` para arreglar un conflicto. Y la conversación con operaciones esa tarde no es agradable.
>
> **Frecuente y seguro → always en `settings.json`. Raro o de riesgo → once en runtime. NUNCA `--dangerously-skip-permissions` en máquina de trabajo.**"

**Tiempo:** ~1 minuto 30 segundos.

---

### Bloque 10 — Recap y cierre del módulo 1 (~3 min)

> "Y eso es la 1.3b. Pero también es **el cierre del módulo 1 entero**. Vamos a recapitular qué se lleva el alumno tras estas cinco demos."

**Vuelvo al VS Code y lanzo `git log --oneline` para mostrar el progreso:**

```powershell
git log --oneline -10
```

```
zyx9876 (HEAD -> demo/1.3b-before) demo/1.3b-after: implementa endpoint POST /api/orders/{id}/cancel con excepción tipada
def5678 (demo/1.3a) demo/1.3a: script de ejemplo para audit pre-commit
ghi9012 (demo/1.2b) demo/1.2b: CLAUDE.md, settings.json y permisos para OrderManagement
jkl3456 (demo/1.2a) demo/1.2a: marca demo de instalación como completada
mno7890 (demo/1.1) demo/1.1: README actualizado y registro de demos creado
```

> "Cinco ramas. Cinco demos. Recap del módulo 1, en cinco puntos.
>
> **Uno.** Tenéis Claude Code instalado y autenticado en Windows. Tres comandos: `irm install.ps1 | iex`, `claude doctor`, `claude` y autorizar OAuth. La 1.2a.
>
> **Dos.** Tenéis un `CLAUDE.md` decente para vuestro proyecto. Cinco bloques: visión, estructura, comandos, convenciones, reglas duras. Y vais a notar la diferencia desde el segundo cero. La 1.2b.
>
> **Tres.** Tenéis `.claude/settings.json` configurado con allow y deny apropiados para vuestro equipo. Permisos por patrón con criterio. Y sabéis que `--dangerously-skip-permissions` **nunca en máquina de trabajo**. La 1.2b también.
>
> **Cuatro.** Conocéis los tres modos — interactivo, one-shot, pipe — y los **diez slash commands esenciales** del día a día. Y dos hábitos: `/usage` cada veinte minutos, `/compact` cada treinta. La 1.3a.
>
> **Cinco.** Sabéis trabajar el patrón 1 — implementación de feature — encadenado entero. `/plan` al inicio, permisos sanos en runtime, `/compact` cuando toca, code review final en modo pipe. La 1.3b. Y conocéis los tres casos donde 'no' es la respuesta correcta.
>
> **Esto es base operativa real.** Con esto, podéis usar Claude Code en vuestro trabajo el lunes. **Ya.** Sin esperar al módulo 2."

**Lanzo `dotnet build` por última vez en la terminal:**

```powershell
dotnet build
```

```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

> "Verde. La feature de cancelación implementada y compilando. Repositorio listo para que los alumnos la cojan y trabajen encima.
>
> **Cierre del módulo 1.** En el módulo 2 vamos a llevar Claude Code a otro nivel — **skills**. Conjuntos de instrucciones reutilizables que vuestro equipo va a poder usar como herramientas modulares. Si Claude Code así de serie ya rinde, con skills propios pasa a ser otra cosa. Lo veremos en la sesión 2 del curso."

**Tiempo:** ~3 minutos.

---

### Bloque 11 — La pregunta antes del módulo 2 (~1 min 30 seg)

> "Y antes de cerrar, una pregunta importante que la gamma 1.3b slide 24 dejó como deberes para casa. Una pregunta que conviene **traer respondida al módulo 2** porque va a hacer que vaya muchísimo más rápido."

**Lo señalo en pantalla:**

```
¿QUÉ PATRÓN SE REPITE TRES VECES A LA SEMANA EN VUESTRO EQUIPO
Y AL QUE TENDRÍAIS QUE ENSEÑARLE A UN JUNIOR NUEVO?

Ejemplos de patrones reales:
- "Cuando creas un controller, siempre va con este DTO,
   este validator y este test."
- "Cuando tocas el dominio, hay que ejecutar este check
   de invariantes."
- "Cuando subes un PR, este checklist de seguridad."
```

> "Cada uno de esos patrones es candidato a **skill**. Tener uno o dos identificados antes de la siguiente sesión hace que el módulo 2 vaya rápido — no estaréis aprendiendo el concepto de skill en abstracto, lo aprenderéis aplicándolo a algo que ya sabéis que hace falta.
>
> Pensadlo durante la pausa entre módulos. Identificad **uno** que repita en vuestro equipo. Esa es la base sobre la que vais a construir el primer skill propio en la sesión 2.
>
> Hasta aquí el módulo 1. Nos vemos en el módulo 2."

**Tiempo:** ~1 minuto 30 segundos.

---

## 9. Qué resaltar verbalmente

Cinco puntos que **no pueden quedarse sin decir**:

1. **"Las 4 opciones del prompt: Allow once, Yes always, No deny, Edit."** — el alumno tiene que poder explicarlas a un colega. Bloque 2.

2. **"Frecuente y seguro → always. Raro o de riesgo → once."** — la regla mnemotécnica del patrón sano. Bloque 3, repetida en bloque 9.

3. **"Cuando dudes, di 'no' y discútelo. El coste de un 'no' es bajo. El coste de un 'yes' mal dado puede ser alto."** — heurística de los 3 casos. Bloque 3, repetida en bloque 5.

4. **"`--dangerously-skip-permissions` nunca en máquina de trabajo."** — la regla que más daño puede causar si se ignora. Bloque 9, recoge la 1.2b.

5. **"El patrón 1 entero: `/plan` + permisos sanos + `/compact` + tests + code review en pipe. Es el flujo real."** — la culminación. Bloque 10.

**Frase de remate al cierre del módulo:**

> *"Cinco demos. Cinco ramas. Base operativa real. Con esto podéis usar Claude Code en vuestro trabajo el lunes. Ya. Sin esperar al módulo 2."*

---

## 10. Slide de entrada (locución HeyGen)

> **Texto del avatar antes del screencast:**

"Y vamos a la última demo del módulo 1. La 1.3b. Aquí no se introduce material nuevo — encadenamos todo lo aprendido en un workflow real end-to-end. Vais a ver dos cosas. Primero, permisos en runtime: las 4 opciones del prompt cuando el agente os pide aprobación, los dos antipatrones del primer día (aprobar todo a ciegas, aprobar todo individualmente) y el patrón sano del medio. Segundo, el patrón 1 de implementación de feature ejecutado entero, sobre la cancelación de pedidos. La misma feature que el agente detectó como falta en la demo 1.1 — hoy la implementamos. `/plan` al inicio. Permisos sanos en runtime. `/compact` cuando toca. Tests rápidos. Y code review final en modo pipe del diff completo antes del PR. **A diferencia de la demo 1.1, los cambios se commitean** — esta es la primera implementación real de feature del curso. Es la culminación del módulo 1. Vamos al screencast."

---

## 11. Slide de salida (locución HeyGen)

> **Texto del avatar al volver del screencast:**

"Lo que acabáis de ver es la culminación del módulo 1. Cinco demos, cinco ramas, una base operativa real. Tenéis Claude Code instalado en Windows. Tenéis CLAUDE.md y settings.json configurados con criterio. Conocéis los tres modos, los diez slash commands esenciales, los dos hábitos clave (`/usage` cada veinte, `/compact` cada treinta). Sabéis las 4 opciones del prompt de aprobación y la regla del patrón sano: frecuente y seguro va a always, raro o de riesgo se queda en once. Conocéis los tres casos donde 'no' es la respuesta correcta. Y habéis visto el patrón 1 de implementación de feature encadenado entero. Con esto podéis usar Claude Code en vuestro trabajo el lunes. Ya. Sin esperar al módulo 2. Pero el módulo 2 va a llevar Claude Code a otro nivel: skills, conjuntos de instrucciones reutilizables que vuestro equipo va a poder usar como herramientas modulares. Antes de la pausa, una pregunta para casa: ¿qué patrón se repite tres veces a la semana en vuestro equipo y al que tendríais que enseñarle a un junior nuevo? Pensadlo. Esa es la base del primer skill propio. Nos vemos en la sesión 2."

---

## 12. Tiempo total estimado

| Bloque | Tiempo |
|---|---|
| Slide de entrada (avatar) | ~30 seg |
| Bloque 1 — Setup, recap del módulo, planteamiento | ~2 min |
| Bloque 2 — Las 4 opciones del prompt | ~2 min |
| Bloque 3 — Antipatrones A, B y el patrón sano + 3 casos del "no" | ~2 min 30 seg |
| Bloque 4 — Patrón 1: arranque con `/plan` | ~3 min |
| Bloque 5 — Permisos en runtime durante la ejecución | ~5 min |
| Bloque 6 — `/compact` cuando toca | ~3 min |
| Bloque 7 — Tests rápidos y commit | ~3 min |
| Bloque 8 — Code review final en modo pipe | ~3 min |
| Bloque 9 — Mención al modo autónomo | ~1 min 30 seg |
| Bloque 10 — Recap y cierre del módulo 1 | ~3 min |
| Bloque 11 — La pregunta antes del módulo 2 | ~1 min 30 seg |
| Slide de salida (avatar) | ~30 seg |
| **Total screencast** | **~30-32 min** |
| **Total con avatar** | **~31-33 min** |

> Si hay preguntas durante el screencast, súmale 4-5 minutos. La demo encaja en un bloque de **35-40 minutos** dentro de la sesión.

**Margen de seguridad por si algo va lento:**

- **Si el agente NO pide aprobación en el bloque 5 cuando lanza `dotnet build`** (porque ya está en allow y la sesión lo recuerda), **adapta el guion**. Comenta: *"esta vez no me pide permiso porque ya está en allow del settings.json. Vamos a forzar un comando que sí pida — uno que no esté en allow"*. Y le pides algo como `dotnet --info` (igual que en la 1.3a). El objetivo pedagógico (mostrar el prompt) se cumple igual.

- **Si el agente acepta la petición de Authorize sin cuestionarla** (en el bloque 5), no fuerces el "no" deliberado. Comenta: *"a veces el agente va directo. En este caso me ha hecho lo que pedí. Pero la lección sigue siendo válida — si yo veo que está modificando endpoints existentes que no quería tocar, debería decir 'no' aquí. Vamos a hacer rollback"*. Y deshaces el cambio con `git checkout -- src/`. La pedagogía se mantiene.

- **Si `/compact` produce un resumen poco útil**, sé honesto: *"el resumen no ha conservado todo lo que esperaba. Se nota que tengo que ser más específico en las instrucciones. Pero la sesión sigue siendo viable, vamos"*. No exageres la calidad si no se ve.

- **Si el code review en modo pipe del bloque 8 no detecta los hallazgos esperados**, comenta: *"a veces el modo pipe da hallazgos distintos según el contexto del diff. Lo importante es que tengas la segunda opinión antes del PR — los hallazgos exactos pueden variar"*. Y comentas los que sí aparezcan.

- **Si `dotnet build` falla en algún momento** por algo del entorno, **úsalo como aprendizaje del workflow real**. *"Esto es lo que pasa de verdad. El agente lo va a ver, ajustar y repetir. Eso es la fase 4 del ciclo agentic — verificación que itera hasta verde"*. Y dejas que Claude Code lo arregle solo.

---

# Apéndice A — Decisiones pedagógicas justificadas

> Esta sección no se graba. Es para Pedro, formador.

**¿Por qué esta es la demo más larga del módulo (~30 min)?**

Porque es la **única que muestra un workflow completo end-to-end**. Las demás demos del módulo 1 muestran piezas individuales (instalación, configuración, modos). Esta encadena todo. **No hay forma honesta de comprimirla más sin perder la sensación de "esto es trabajo real"**. El alumno necesita ver el flujo entero desde `/plan` hasta `code review en pipe` de manera continua, no fragmentada.

**¿Por qué la feature implementada es la cancelación de pedidos y no otra?**

Porque hay **tres conexiones pedagógicas fuertes** con el resto del curso:

1. La gamma 1.3a y manual 1.3 línea 215 ya la mencionan como ejemplo del patrón 1.
2. El agente la detectó como "falta" en la demo 1.1 (la observación de `InvalidOperationException`). El alumno **se acuerda** y conecta cabos.
3. La feature requiere **exactamente tres ficheros** — handler refactor, excepción nueva, endpoint nuevo — el escenario perfecto para `/plan` (la regla "más de tres ficheros, `/plan`").

**¿Por qué el código se commitea aquí y no en la demo 1.1?**

Porque la demo 1.1 era **conceptual** — se mostraba el ciclo agentic, no se hacía trabajo permanente. La 1.3b es la culminación operativa, así que la feature queda como **precedente para todas las demos siguientes**. El módulo 5 (testing) va a generar tests para esta feature concreta. La continuidad del repo lo requiere.

**¿Por qué simulo la pregunta de Authorize en el bloque 5?**

Porque la gamma 1.3b slide 11 enseñó "los tres casos donde 'no' es la respuesta correcta", pero **sin demostración en vivo el alumno no internaliza cuándo aplicarlo**. Forzando el escenario donde el agente propone algo amplio fuera de scope (caso 2 del slide 11), el alumno **ve cuándo decir "no" y cómo se siente**. Es un escenario didáctico construido a propósito.

**¿Por qué code review en pipe al final y no antes de commitear?**

Porque la gamma 1.3a slide 11 dijo: *"antes de subir un PR, una segunda mirada del agente"*. **El code review es PRE-PR, no pre-commit**. Pre-commit son los hooks (módulo 3). Pre-PR es el modo pipe sobre el diff entre rama y main. Mantener esa distinción evita confusión.

**¿Por qué simulo `/compact` con instrucciones tan específicas en el bloque 6?**

Porque la gamma 1.3a slide 29 dijo que las instrucciones explícitas a `/compact` son "lo más útil que casi nadie sabe". Si la demo solo enseña `/compact` solo, el alumno pierde el 80% del valor. Con instrucciones del tipo *"conserva las decisiones de implementación... Y conserva que decidí NO añadir Authorize"*, el resumen es **utilísimo** — y el alumno ve el patrón de cómo guiarlo.

**¿Por qué el cierre del módulo 1 incluye la pregunta del slide 24 de la gamma?**

Porque la gamma terminaba con esa pregunta como **deberes para casa**. Si la demo no la recoge, el alumno la oye en abstracto y la olvida. Recogerla aquí, **al cierre del módulo**, le da peso. Y conecta directamente con el módulo 2 (skills) — la pregunta es literalmente la base sobre la que se construye el primer skill propio.

**¿Por qué la rama `demo/1.3b-after` queda como base permanente?**

Porque a partir de aquí, **todas las demos siguientes asumen que la feature de cancelación existe**. La 5.3a (tests xUnit) va a generar tests para `CancelOrderHandler` y para el endpoint nuevo. La 4.1b (Figma MCP) puede mostrar tokens del frontend que acompañen la UI de cancelación. Esta rama es **base estructural** para el resto del curso. Aunque durante la grabación los cambios se commitearon sobre `demo/1.3b-before` y se descartaron al cerrar, la rama `demo/1.3b-after` (pre-cocinada en §6b) queda con la feature reproducible para las demos siguientes.

**¿Por qué tres terminales no, y dos sí?**

Porque tres terminales saturan la pantalla y el alumno se pierde en cuál estamos. Con dos terminales (Claude Code interactivo + git/dotnet) cubrimos todo el flujo, mantenemos foco visual, y el alumno sigue mejor. **Decisión de claridad sobre completitud.**

**¿Por qué el bloque 7 hace inspección estática y no arranca la API?**

Porque arrancar `dotnet run` durante la grabación **introduce latencia y posibles errores externos** (puerto ocupado, configuración local, base de datos in-memory que necesita seed). La inspección estática **demuestra el valor** sin riesgos del entorno. Y lo más importante: **el agente mismo me da las dos opciones y me pregunta**. Eso es lección pedagógica. Si yo decidiera por mi cuenta, perdería esa pieza.
