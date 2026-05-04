> **Versión:** v3 | **Módulo:** 1 | **Sub:** 1.2b | **Slides:** 47 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M01-S1.2b-configuracion-proyecto-v3.md`

# Submódulo 1.2b — Configuración del proyecto

## Slide 1 — Portada
**Módulo 1 · Submódulo 1.2 · Parte B**
Configuración del proyecto
CLAUDE.md, settings y permisos

---

## Slide 2 — Dónde estamos

En la parte A dejamos Claude Code instalado, autenticado, respondiendo. Eso es prerequisito.

Ahora viene la parte que de verdad marca la diferencia entre un equipo que se queda con Claude Code y uno que lo abandona a la semana: **darle al agente un repo bien presentado**.

Tres piezas:

```
1. CLAUDE.md          → contexto persistente del proyecto
2. settings.json      → configuración personal y de equipo
3. Permisos           → qué puede hacer y qué no, sin pedir
```

> El entregable de la sesión es un `CLAUDE.md` decente para tu repo
> y unos permisos sensatos. Con eso te llevas a tu puesto algo útil.

---

## Slide 3 — CLAUDE.md: el fichero más importante del módulo

Si tuviera que quedarme con una sola pieza de configuración, sería esta.

`CLAUDE.md` es **contexto persistente** — lo que el agente lee al empezar cada sesión para entender tu proyecto. Y cambia drásticamente la calidad del output.

> La diferencia entre un dev que se frustra con Claude Code
> y uno que cierra triple de tickets que antes
> **NO está en haber aprendido más comandos.**
> Está, casi siempre, en haber escrito un `CLAUDE.md` decente.

---

## Slide 4 — Qué es y dónde vive

```
Es un fichero Markdown en la raíz del repo.
├── Va a git
├── Lo comparte todo el equipo
└── Cuando lanzas `claude` desde ese directorio,
    se carga automáticamente al contexto desde el segundo cero
```

**Y en monorepo, jerárquico:**

```
En proyectos con monorepo o estructura por módulos:
├── CLAUDE.md adicionales en subdirectorios
├── El de la raíz aplica siempre
└── El de un subdirectorio se añade cuando trabajas dentro

Útil cuando frontend Angular y backend .NET tienen
convenciones distintas y quieres especificarlas por separado.
```

---

## Slide 5 — Qué meter en CLAUDE.md

Lo que tu equipo ya sabe pero un junior nuevo necesitaría saber para no meter la pata. Cinco cosas:

```
1. Visión general del proyecto       (3-5 líneas)
2. Estructura de carpetas             (solo lo no obvio)
3. Comandos clave                     ← esto es ORO
4. Convenciones de código
5. Reglas duras                       ← prohibiciones explícitas
```

Las vemos una a una.

---

## Slide 6 — Qué meter (1): visión general

**3-5 líneas. Qué es, qué hace, qué stack. No te enrolles.**

```
"Aplicación full-stack para gestión de pedidos B2B.
 Backend en ASP.NET Core 10, frontend Angular 19,
 base PostgreSQL. Autenticación con Auth0."
```

> Cuatro líneas. Stack. Función. Punto.
> No le cuentes la historia del proyecto. Al agente le da igual.

---

## Slide 7 — Qué meter (2): estructura de carpetas

**Para qué sirve cada carpeta principal. Solo lo no obvio.**

```
Si tu carpeta /utils se llama /utils
└── el agente ya lo deduce.
    No hace falta listarla.

Si tienes una carpeta /legacy-orders/
que es código vivo pero no se toca
└── ESO sí hay que decirlo.
    No lo va a deducir solo.
```

> Lista solo lo que el agente no entendería leyendo el árbol del repo.
> Todo lo demás es ruido que dispersa su atención.

---

## Slide 8 — Qué meter (3): comandos clave — esto es oro

**Cómo se buildea, se testea, se levanta el dev environment, se corre el linter.**

```
Sin estos comandos:
└── Claude inventa los que cree que tienes.
    Y casi nunca acierta.
```

**Pon todo lo que orquesta tu día:**

```
Si tu equipo tiene un make dev que orquesta todo
└── Ponlo.

Si usáis un script ./scripts/start-local.sh
└── Ponlo.
```

---

## Slide 9 — Qué meter (4): convenciones de código

**Patrones de naming, async/await, manejo de errores, estructura de DTOs, dónde van las inyecciones, cómo se escriben los tests.**

```
Aquí puedes meterte un poco más en detalle.
Sin pasarte.
```

Ejemplos del tipo de cosas que van aquí:

```
├── Naming: PascalCase clases/métodos, _camelCase campos privados
├── Async/await siempre. Nunca .Result ni .Wait()
├── Errores: Result<T> en dominio, ProblemDetails en API
├── Tests: xUnit + NSubstitute
└── DTOs: terminados en Dto, en /src/Api/Contracts/
```

---

## Slide 10 — Qué meter (5): reglas duras

**Las cosas que un junior haría mal y que tú tendrías que corregir cada vez.**

```
"No toques los ficheros generados en /generated/"
"Este DTO siempre va con esta convención"
"Los tests usan xUnit + NSubstitute, NUNCA Moq"
```

**Aquí también van las prohibiciones:**

```
"Nunca crees migraciones nuevas sin avisar"
"No toques appsettings.Production.json"
```

---

## Slide 11 — Qué NO meter en CLAUDE.md

Aquí es donde más fallan los equipos.

> **Un CLAUDE.md mal calibrado es peor que no tenerlo.**

Cinco trampas. Las vemos.

---

## Slide 12 — Trampa 1: documentación que cambia mucho

Si metes la lista de endpoints o el esquema de la base de datos completo, en dos meses está desactualizado y el agente sigue creyéndoselo.

**Resultado:** genera código contra una realidad que ya no existe.

```
Caso real:

Equipo cuyo CLAUDE.md documentaba en detalle 40 endpoints.

Tres meses después:
├── 12 de esos endpoints habían cambiado
├── 5 ya no existían
└── 8 eran nuevos

Claude Code seguía creyendo que la API era la del CLAUDE.md.

Cada generación de cliente HTTP era inservible.
```

---

## Slide 13 — Trampa 2: datos sensibles

```
El fichero está en git.
├── Va a producción
├── Va al portátil de cada dev
└── Va a quedarse en los logs de tu CI
```

**No metas:**

```
❌ Claves
❌ URLs internas que no deban filtrar
❌ Credenciales de bases de datos de prueba
❌ Nada que un commit accidental pueda exponer
```

> Lo digo dos veces porque el día menos pensado alguien lo hace.

---

## Slide 14 — Trampa 3: cosas obvias

**No le digas al agente cosas que ya sabe.**

```
❌ "C# es un lenguaje tipado"
❌ "Angular usa TypeScript"
```

> Estás gastando contexto y dispersando la atención del agente
> sobre lo que sí importa.

El contexto del modelo es finito. Cada línea que ocupes con algo obvio es una línea menos para algo útil.

---

## Slide 15 — Trampa 4: listas exhaustivas de todo

```
Cuanto más largo el CLAUDE.md
└── peor calibrado.
```

**La regla práctica:**

```
Si el fichero pasa de 200 líneas
└── replantéatelo.
```

Lo que está dentro tiene que ganarse el sitio. Si no está aportando valor cada línea, fuera.

---

## Slide 16 — Trampa 5: documentación pensada para humanos

```
CLAUDE.md NO es el README.

├── No tiene que ser pedagógico
├── No tiene que tener introducción
└── No tiene que explicar la motivación del proyecto
```

**Qué SÍ tiene que hacer:**

```
└── Dar al agente la información mínima útil
    para trabajar bien en el repo
```

**Frase típica de README que NO va en CLAUDE.md:**

```
❌ "Este proyecto fue iniciado en 2023 con el objetivo
    de digitalizar los procesos comerciales..."
```

> Al agente le da igual la motivación del proyecto.
> Solo le importa qué tiene que hacer.

---

## Slide 17 — Anatomía de un CLAUDE.md para .NET + Angular

Una estructura que funciona bien. Las siguientes seis slides son secciones de un mismo fichero.

```markdown
# Proyecto: Pedidos B2B

Aplicación full-stack para gestión de pedidos en cliente mayorista.
Backend ASP.NET Core 10, frontend Angular 19, BBDD PostgreSQL.
```

---

## Slide 18 — Anatomía: estructura

```markdown
## Estructura

- /src/Api — proyecto ASP.NET Core con los endpoints REST.
- /src/Domain — entidades y lógica de dominio. Sin dependencias externas.
- /src/Infrastructure — acceso a datos (EF Core), integraciones externas.
- /src/Web — aplicación Angular standalone.
- /tests — tests unitarios e integración (xUnit + NSubstitute).
- /legacy-orders — código vivo pero no se modifica. Tocar solo si se pide explícitamente.
```

---

## Slide 19 — Anatomía: comandos

```markdown
## Comandos

- dotnet build — compilar la solución.
- dotnet test — ejecutar todos los tests.
- dotnet ef migrations add <Nombre> — crear nueva migración.
- cd src/Web && npm run dev — levantar el frontend en :4200.
- cd src/Web && npm run lint — linter Angular.
- ./scripts/seed-db.sh — poblar BBDD local con datos de prueba.
```

> Esto es lo que más rentabilidad da. Sin los comandos exactos
> el agente inventa los que cree que tienes.

---

## Slide 20 — Anatomía: convenciones .NET

```markdown
## Convenciones .NET

- Naming: PascalCase para clases y métodos, _camelCase para campos privados.
- Async/await en todo el stack; nunca .Result ni .Wait().
- Manejo de errores: Result<T> en capa de dominio, ProblemDetails en API.
- Tests: xUnit + NSubstitute. **Nunca Moq.**
- DTOs: terminados en Dto, en /src/Api/Contracts/.
```

---

## Slide 21 — Anatomía: convenciones Angular

```markdown
## Convenciones Angular

- Componentes standalone siempre. Nada de NgModules nuevos.
- Signals para estado local; SignalStore para estado compartido.
- Reactive Forms con tipado estricto.
- Tests: Karma + Jasmine para unit, Playwright para E2E.
```

---

## Slide 22 — Anatomía: reglas duras

```markdown
## Reglas duras

- No tocar /src/Api/Generated/ — es código autogenerado desde OpenAPI.
- DTOs públicos siempre en /src/Api/Contracts/ con nombres terminados en Dto.
- Migraciones: nunca editar una migración ya aplicada en main.
- Nunca crear branches con prefijo release/* (las gestiona el pipeline).
```

> Esto es punto de partida, no destino.
> Tu equipo va a refinarlo conforme veas qué pifia el agente.
>
> **El CLAUDE.md es un fichero vivo.**

---

## Slide 23 — Tres patrones según tipo de proyecto

No todos los proyectos necesitan el mismo CLAUDE.md. Tres casos típicos.

```
1. Greenfield      → proyecto nuevo, equipo pequeño
2. Legacy          → proyecto antiguo con convenciones implícitas
3. Monorepo        → varios módulos con convenciones distintas
```

Las vemos.

---

## Slide 24 — Patrón 1: greenfield

**Proyecto nuevo, equipo pequeño.**

```
Empieza minimalista.
├── 30-50 líneas
├── Solo estructura
├── Comandos
└── Tres o cuatro reglas duras críticas
```

**Lo vas engordando** según el agente vaya cometiendo errores que repites en cada sesión.

---

## Slide 25 — Patrón 2: legacy

**Proyecto antiguo con muchas convenciones implícitas.**

Aquí el CLAUDE.md cobra valor porque el agente no puede deducir las convenciones del código (a menudo el código es inconsistente).

```
Más documentación de "qué hacer y qué no".

Las reglas duras son CRÍTICAS aquí.
└── Lista las trampas históricas.
```

**Ejemplo de trampa histórica:**

```
"El módulo X parece dead code pero NO se borra,
 lo usa un cliente concreto vía endpoint legacy"
```

---

## Slide 26 — Patrón 3: monorepo

```
CLAUDE.md raíz cortito
├── Visión global
└── Comandos comunes

+ CLAUDE.md específicos por módulo
└── Cada uno documenta cómo se hacen las cosas
    en ese servicio concreto
```

**Ejemplo:**

```
/services/orders/CLAUDE.md
└── Documenta convenciones específicas de orders
```

Claude carga la combinación cuando trabajas en ese subdirectorio. El de la raíz aplica siempre, el del subdirectorio se añade.

---

## Slide 27 — AGENTS.md: el estándar cross-tool

Aquí entramos en una pieza que confunde a bastante gente.

```
AGENTS.md es un estándar abierto adoptado por:
├── Codex
├── Copilot CLI
├── Gemini CLI
└── Claude Code
    (entre otras)
```

**La idea es la misma que CLAUDE.md:** contexto persistente para el agente.

**La diferencia:** es **portable entre herramientas**.

---

## Slide 28 — Cuándo usar AGENTS.md

```
Tu equipo solo usa Claude Code
└── Quédate con CLAUDE.md. No te compliques.

Tu equipo usa varias herramientas de IA
(unos Claude Code, otros Copilot CLI, otros Gemini)
└── AGENTS.md tiene sentido como fuente común.

Quieres que tu repo sea agnóstico para el futuro
└── AGENTS.md para el contexto compartido
    + CLAUDE.md muy corto solo para lo específico
    de Claude Code (que en la práctica es poco)
```

---

## Slide 29 — El error que hay que evitar

**Tener ambos ficheros con el mismo contenido.**

```
Claude Code lee los dos.
Encuentra la información duplicada.
Empieza a comportarse raro:
├── A veces aplica una versión
├── A veces otra
└── A veces mezcla
```

**Si decides usar los dos**, que cubran cosas distintas:

```
AGENTS.md  → lo cross-tool (común a todas)
CLAUDE.md  → solo lo específico de Claude Code
             (que es muy poco — slash commands de skills propios, etc.)
```

---

## Slide 30 — Recomendación práctica para .NET + Angular

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   Empieza con CLAUDE.md solo.                            │
│                                                          │
│   Cuando el equipo esté usando Claude Code               │
│   de forma consistente, evalúa si compensa migrar        │
│   a AGENTS.md para cubrir otras herramientas.            │
│                                                          │
│   La inmensa mayoría de equipos no llegan a necesitarlo. │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

---

## Slide 31 — settings.json y los tres scopes

Última pieza grande de configuración. Esta es de las que más tropiezos genera porque hay tres niveles y la gente mezcla.

```
1. User      → ~/.claude/settings.json
2. Project   → .claude/settings.json (en repo, va a git)
3. Local     → .claude/settings.local.json (en repo, gitignored)
```

Los vemos uno a uno.

---

## Slide 32 — Scope 1: User

**`~/.claude/settings.json`**

```
Tus preferencias personales.
└── Viajan contigo de proyecto en proyecto.

Aquí van:
├── Tu modelo preferido por defecto
└── Flags de comportamiento general que aplicas siempre
```

---

## Slide 33 — Scope 2: Project

**`.claude/settings.json` en el repo**

```
Configuración del equipo.
├── Va a git
└── Se aplica a todo el que clone el repo

Aquí van:
├── Permisos del equipo
├── Hooks
└── Configuración de MCP servers compartidos
```

---

## Slide 34 — Scope 3: Local

**`.claude/settings.local.json` en el repo**

```
Configuración del proyecto, pero solo para ti.
├── Va a .gitignore
└── No se commitea

Aquí van:
├── Overrides personales sobre el proyecto
├── Tu cuenta de API si es distinta a la del equipo
└── Flags personales solo cuando tú trabajas en este repo
```

---

## Slide 35 — Casos típicos de tropiezo (1/2)

**Configurar permisos a nivel user que rompen al pasar a otro proyecto.**

```
Te diste permisos amplios para un proyecto sandbox.
Se te quedaron activos al cambiar al repo del cliente.
└── Estás dando permisos al agente que no querías.

Solución:
└── Los permisos amplios casi siempre van a nivel proyecto, no user.
```

**Configurar permisos a nivel project sin avisar al equipo.**

```
Tú metes en .claude/settings.json un permiso que necesitas.
Lo commiteas.
Al resto del equipo le aparece como cambio sin entender de dónde viene.
└── Genera fricción.

Solución:
└── Si modificas algo que va a git, avisa.
```

---

## Slide 36 — Casos típicos de tropiezo (2/2)

**Esperar que las preferencias locales viajen con el repo.**

```
.claude/settings.local.json está gitignored.
├── Si te lo borras, se pierde
└── Si cambias de máquina, se pierde
```

**Mezclar configuración entre niveles sin saber qué prevalece.**

```
Hay un orden de precedencia:
├── project > user
└── local override de tu copia local

Si te encuentras con comportamientos raros:
└── Revisa qué nivel tiene qué.
```

---

## Slide 37 — Permisos: el modelo de seguridad

Esto merece su propia sección porque Claude Code, a diferencia de Copilot, **ejecuta cosas**.

> El modelo de permisos es real, no decorativo.

---

## Slide 38 — Cómo funciona por defecto

Cuando Claude va a usar una herramienta — leer un fichero, escribir uno, ejecutar un comando — **te pide aprobación**.

```
Ves qué va a hacer.
Decides:
├── Permitir UNA vez
├── Permitir SIEMPRE para ese tipo de operación
└── Bloquear
```

**Al principio es agotador.** Te puede pasar a hacer veinte aprobaciones mientras instala una librería.

> Pero es el modelo correcto cuando estás aprendiendo
> o cuando trabajas en un repo sensible.
> Te obliga a saber qué está haciendo el agente.

---

## Slide 39 — Cómo aflojarlo: allow y deny

En `.claude/settings.json` (proyecto) o `~/.claude/settings.json` (user) puedes definir qué herramientas el agente puede usar sin pedir permiso.

```json
{
  "permissions": {
    "allow": [
      "Read",
      "Write",
      "Edit",
      "Bash(npm run *)",
      "Bash(npm test)",
      "Bash(npm install *)",
      "Bash(dotnet build)",
      "Bash(dotnet test *)",
      "Bash(dotnet ef *)",
      "Bash(git status)",
      "Bash(git diff *)",
      "Bash(git add *)",
      "Bash(git commit -m *)"
    ],
    "deny": [
      "Bash(rm -rf *)",
      "Bash(git push --force *)",
      "Bash(git push origin main)",
      "Bash(git reset --hard *)",
      "Read(./.env)",
      "Read(./.env.*)",
      "Read(./secrets/*)",
      "Write(./production.config.*)"
    ]
  }
}
```

---

## Slide 40 — allow vs deny

```
allow
└── Esas operaciones pasan SIN aprobación.

deny
└── Esas operaciones se BLOQUEAN aunque el agente las pida.
    Útil para comandos destructivos que nunca quieres
    ejecutar accidentalmente.
```

**La granularidad es por patrón:**

```
Bash(dotnet *)
└── Permite cualquier subcomando de dotnet sin preguntar

Bash(dotnet build) + Bash(dotnet test)
└── Más restrictivo: solo build y test, lo demás pregunta
```

---

## Slide 41 — Patrones de permisos por tipo de proyecto (1/2)

**Proyecto sandbox / aprendizaje**

```
allow → permisivo (Read, Write, Edit, Bash(*) o casi)
deny  → mínimo

Idea: no estar aprobando cada cosa.
```

**Proyecto de cliente / producción**

```
allow → restrictivo
        (solo Read, Edit, y comandos build/test concretos)
deny  → con la lista de comandos destructivos típicos
```

> Un día va a intentar hacer algo que no quieres
> y te alegrarás de tener el deny.

---

## Slide 42 — Patrones de permisos por tipo de proyecto (2/2)

**CI / pipeline automatizado**

```
allow → específico al pipeline
        (qué comandos necesita ejecutar y nada más)
modo autónomo → activado SOLO en este contexto
```

> Lo veremos con más detalle en 1.3.

**Tu portátil con repo de cliente**

```
allow → heredado del proyecto
deny  → heredado del proyecto

Modo autónomo → NUNCA aquí.
```

---

## Slide 43 — El modo autónomo

```bash
claude --dangerously-skip-permissions
```

Lo que pone el flag en su nombre. Salta todo el sistema de aprobaciones.

**Útil en dos contextos muy concretos:**

```
1. Sandbox aislado
   └── VM sin acceso a producción ni a información sensible.
       El peor escenario es resetearla.

2. CI/CD
   └── Pipeline donde el agente ejecuta una tarea acotada
       (generar tests, ejecutar lint, formatear código)
       y el entorno está controlado.
```

---

## Slide 44 — El modo autónomo: cuándo NUNCA

```
❌ Tu portátil de trabajo conectado a producción
❌ Cualquier máquina con credenciales de cliente cargadas
❌ Sesiones donde el agente puede tener acceso a:
   ├── .env
   ├── secrets/
   └── Configuración sensible
```

**Anécdota real:**

```
He visto a gente lanzar --dangerously-skip-permissions
en su portátil "para que vaya más rápido".

El día que el agente decide que la mejor forma de resolver
un conflicto de merge es:
└── git push --force origin main

...te acuerdas del flag.

Y la conversación con operaciones esa tarde no es agradable.
```

---

## Slide 45 — Plantilla CLAUDE.md lista para usar (entregable de la sesión)

Cópiala. Pégala en la raíz de tu repo de prácticas. Ajusta a la realidad de tu equipo.

```markdown
# Proyecto: <NOMBRE>

<Descripción de 2-3 líneas: qué es, para qué sirve, stack principal>

## Estructura

- /src/Api — endpoints REST con ASP.NET Core.
- /src/Domain — entidades y lógica de dominio.
- /src/Infrastructure — EF Core, integraciones externas.
- /src/Web — frontend Angular standalone.
- /tests — tests unitarios y de integración.

## Comandos

- dotnet build — compilar la solución.
- dotnet test — ejecutar tests .NET.
- dotnet ef migrations add <Nombre> — crear migración.
- cd src/Web && npm install — instalar dependencias frontend.
- cd src/Web && npm run dev — levantar Angular en :4200.
- cd src/Web && npm run lint — linter Angular.

## Convenciones .NET

- Naming: PascalCase clases/métodos, _camelCase campos privados.
- Async/await siempre. Nunca .Result ni .Wait().
- Errores: Result<T> en dominio, ProblemDetails en API.
- Tests: xUnit + NSubstitute. <ajustar a tu stack real>

## Convenciones Angular

- Componentes standalone. Nada de NgModules nuevos.
- Signals para estado local, SignalStore para estado compartido.
- Reactive Forms con tipado estricto.
- Tests: Karma + Jasmine. <ajustar>

## Reglas duras

- <Lista las cosas que el agente NUNCA debe hacer en tu repo>
- <Por ejemplo: no tocar carpetas autogeneradas>
- <O: nunca modificar migraciones ya aplicadas>
```

> **Esto es el entregable de la sesión.**
> Salir con esta plantilla rellenada para tu repo real.

---

## Slide 46 — Errores frecuentes del primer día

```
❌ SALTARSE EL CLAUDE.md
   "Ya lo escribiré luego" — y nunca llega ese luego.
   └── Hazlo el día 1, aunque sea minimalista. Lo refinas después.

❌ USAR sudo CON npm
   Ataja problemas a corto plazo y los multiplica a medio.
   └── Configura el prefix bien o usa nvm.

❌ CONFIGURAR PERMISOS A NIVEL USER EN VEZ DE PROYECTO
   Te abre permisos en proyectos donde no quieres.
   └── Los permisos del equipo van en .claude/settings.json,
       no en ~/.claude/settings.json.

❌ ACTIVAR MODO AUTÓNOMO EN MÁQUINA CON REPO DE CLIENTE
   Ya lo dije. Lo digo otra vez porque es de los pocos
   que pueden joder un día entero.
```

---

## Slide 47 — Errores frecuentes (2/2) y la prueba rápida

```
❌ METER COSAS QUE CAMBIAN MUCHO EN CLAUDE.md
   Lista de endpoints, esquema BBDD detallado, versiones.
   └── Se desactualiza y mete ruido.

❌ CONFUNDIR CLAUDE.md CON README.md
   Audiencias distintas. Tonos distintos. Contenidos distintos.

❌ NO USAR claude doctor CUANDO ALGO FALLA
   Antes de buscar en internet, lánzalo. Casi siempre te ahorra el viaje.
```

**Y antes de pasar a 1.3, una prueba rápida que merece la pena:**

```bash
claude
> explícame qué hace este proyecto basándote en
  el código y el CLAUDE.md
```

```
Si la respuesta refleja lo que metiste en CLAUDE.md
└── (los comandos, las convenciones, las reglas duras)
    Vas bien.

Si parece que se lo está inventando
└── O responde de forma genérica
    ("este es un proyecto .NET...")
    Sin tocar las particularidades de tu repo
    └── Revisa el fichero. Le falta señal o tiene ruido.
        Iteras y vuelves a probar.
```

> Esa iteración del CLAUDE.md es trabajo útil.
> Se lleva al equipo igual que se lleva la herramienta.
>
> **Nos vemos en 1.3.**
