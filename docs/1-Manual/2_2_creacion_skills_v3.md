# 2.2 Creación de skills personalizados

**Duración en clase:** 60 minutos · **Sesión 2, submódulo 2**
**Versión:** v3 — incorpora consejo de iteración previa al skill extraído de la guía oficial de Anthropic *"The Complete Guide to Building Skills for Claude"*.

---

## Construir uno, no leer sobre uno

La mejor forma de entender un skill es escribirlo. Lo que viene son los próximos 60 minutos de la sesión: construimos juntos un skill real, en cuatro versiones, cada una más sofisticada que la anterior. Empezamos por el `SKILL.md` mínimo que se puede escribir en una pantalla de móvil. Terminamos con un skill que tiene scripts, plantillas y convenciones del equipo codificadas. Y entre medias, vamos viendo qué problema soluciona cada capa.

El skill de ejemplo es un **generador de componentes Angular standalone**. Lo elijo por tres razones: es un caso común que cualquier equipo Angular agradece automatizar, permite enseñar la progresión de complejidad de forma natural, y al final del apartado el alumno se lleva un skill que puede instalar literalmente tal cual en su repo del trabajo.

Si tu equipo trabaja con .NET puro y Angular no aplica, el patrón se traduce directamente: cambia "componente Angular" por "controller .NET" y los conceptos se mantienen idénticos. Los anti-patrones, las decisiones de diseño y la progresión funcionan igual.

Una nota sobre cómo leer este apartado: cada versión del skill es funcional. Si te paras en la versión 2, ya tienes un skill útil. Las versiones 3 y 4 añaden potencia pero no son obligatorias. La progresión está pensada para que veas cuándo cada capa empieza a justificarse.

---

## Antes de escribir nada: resuelve un caso primero

Hay un consejo que aparece en la guía oficial de Anthropic que vale la pena interiorizar antes de tocar `SKILL.md`:

> Los creadores de skills más efectivos **iteran sobre una única tarea desafiante hasta que Claude la resuelve bien**, y solo entonces extraen el patrón ganador a un skill.

Es contraintuitivo. La tentación natural cuando aprendes lo de los skills es pensar *"voy a hacer un skill de generación de componentes Angular"*, abrir un editor y empezar a escribir el `SKILL.md` desde cero. Y casi siempre sale mal en la primera versión. Faltan convenciones, sobran reglas, las descripciones no activan, los ejemplos no encajan con la realidad del repo.

La alternativa que funciona mejor es esta:

1. **Coge un caso concreto.** No abstracto. *"Tengo que crear el componente `OrdersListComponent` aquí, en este repo, ahora"*.
2. **Trabaja con Claude Code en interactivo**, sin skill. Sesión normal. Conversación. Iteras hasta que el componente queda como tú quieres. Le corriges lo que pifia. Le explicas tus convenciones cuando se las salta. Le indicas qué imports en qué orden. Lo que haga falta.
3. **Cuando el resultado final te gusta**, paras y miras hacia atrás. Lo que has hecho durante esos 20 o 30 minutos es **el material en bruto del skill**. Las correcciones que has tenido que hacer son las reglas que el skill debe codificar. Las convenciones que has tenido que repetir son lo que va al `SKILL.md`. Los pasos que han funcionado bien son el workflow.
4. **Extraes el patrón ganador a `SKILL.md`.** No inventas nada — copias lo que has aprendido en la sesión a un formato persistente.

### Por qué este flujo da mejor resultado

**Empezar por escribir el skill desde cero parte de una hipótesis** — *"creo que mi equipo hace los componentes así"*. Y las hipótesis de cómo trabajamos rara vez coinciden con cómo trabajamos de verdad.

**Empezar por resolver un caso primero parte de evidencia** — *"este componente concreto me ha quedado bien después de iterar X veces, y aquí están las correcciones que tuve que hacer"*. El skill que sale de eso ya sabe lidiar con casos reales porque se ha escrito a partir de uno.

Hay otra ventaja: durante la sesión iterativa estás **enseñándole a Claude tu convención** en tiempo real. Lo que aprendes durante esa sesión no es solo *"cómo se hace el componente"*; es también *"qué necesita Claude que le diga para hacerlo bien"*. Esa información — qué es obvio para Claude y qué hay que escribirle — es justo lo que un skill captura.

### Cuándo este consejo no aplica

Hay un caso donde sí merece la pena partir de la teoría: cuando ya tienes el skill mental claro porque has hecho la tarea cientos de veces a mano. Si llevas cinco años generando controllers y tienes la convención del equipo metabolizada, te puedes saltar la fase de iteración y escribir el `SKILL.md` directamente. Pero esto es menos frecuente de lo que parece — incluso en patrones que crees dominar, la sesión iterativa con Claude saca a la luz convenciones implícitas que no habías articulado nunca.

### Lo que vamos a hacer en este apartado

Aunque el consejo es "primero resuelve un caso", en este apartado vamos a saltar ese paso por economía de tiempo de clase. El generador de componentes Angular del ejemplo está **ya basado en una sesión iterativa real** que se hizo previamente — el patrón ganador ya está extraído. Lo que vas a ver son las cuatro versiones progresivas del skill que sale de ese trabajo previo.

Pero **cuando vuelvas a tu repo del trabajo y vayas a hacer tu primer skill de verdad, sigue el flujo iterativo primero**. Es la diferencia entre que tu skill funcione el día 1 o que necesite tres rondas de refinamiento antes de empezar a aportar valor.

---

## Versión 1: el skill más simple posible

Empezamos por el mínimo absoluto. Un solo fichero, frontmatter mínimo, instrucciones cortas.

### El fichero

Crea esta estructura:

```
.claude/skills/angular-component/
└── SKILL.md
```

Y en `SKILL.md`:

```markdown
---
name: angular-component
description: Genera componentes Angular standalone con Signals siguiendo la estructura del equipo. Usar cuando el usuario pida crear un nuevo componente, haga referencia a un componente nuevo en una feature, o cuando el flujo requiera scaffolding de UI Angular.
---

# Generador de componentes Angular standalone

## Cuándo se usa este skill

Cuando el usuario pida crear un componente Angular nuevo. Esto incluye peticiones como "crea un componente para X", "necesito un componente OrdersList", "vamos a hacer la UI de Y", o referencias implícitas a un componente que no existe aún.

## Qué genera

Un componente Angular standalone con la estructura estándar del equipo:

- Un fichero `.component.ts` con la clase y el decorador `@Component({ standalone: true })`
- Un fichero `.component.html` con el template
- Un fichero `.component.scss` con los estilos
- Un fichero `.component.spec.ts` con tests unitarios usando Jasmine

## Convenciones que sigue

- Componentes `standalone: true` siempre. Nada de NgModules nuevos.
- Para estado local, usa `signal()`. Para valores derivados, `computed()`.
- Los inputs y outputs van como propiedades del componente (no decoradores).
- Nombres en kebab-case para selectores (prefijo `app-`).
- Tests con configuración mínima necesaria.

## Pasos al generar

1. Pregunta al usuario el nombre del componente si no lo ha dado.
2. Decide la ubicación según la estructura del proyecto (busca `src/app/components/` o equivalente).
3. Genera los cuatro ficheros.
4. Si el componente debe enrutarse, sugiere la entrada en routing pero no la añadas sin confirmación.
```

Eso es todo. Frontmatter con `name` y `description`. Cuerpo con instrucciones claras.

### Probarlo

Lanza una sesión nueva en el repo donde has metido el skill. Pídele *"crea un componente para mostrar el listado de pedidos"*. Si todo va bien, Claude detecta la activación del skill y genera los cuatro ficheros con la estructura descrita.

Verifica que se ha activado:

```
> ¿qué skill has usado para esto?
```

Claude te dice. Si no se activó, vuelves a la descripción del frontmatter y la afinas.

### Limitaciones de la versión 1

Esta primera versión funciona, pero tiene límites:

- **El template es genérico.** Genera un componente con la estructura básica, pero no con los detalles específicos del equipo. Si tu equipo tiene un patrón de imports concreto, una estructura de constructor específica, o un orden particular de propiedades, este skill no lo conoce todavía.
- **No conoce el contexto del proyecto.** Si tu equipo usa NgRx SignalStore para estado compartido, este skill no lo aplica automáticamente. Si tienes un estilo concreto de tests, tampoco.
- **Las convenciones están en prosa, no codificadas.** Si una convención cambia en el equipo, hay que reescribirla a mano en el `SKILL.md`.

Para los casos básicos basta. Pero el siguiente paso natural es subir el detalle.

---

## Versión 2: añadiendo convenciones del equipo

La siguiente capa: meter en el skill las convenciones reales del equipo. Lo que diferencia a un componente generado por el skill de uno generado por Claude sin skill es justamente esto.

### Qué añadir

Imagina que tu equipo tiene estas convenciones específicas (ajusta a la realidad del tuyo):

- Imports en orden: Angular core → librerías externas → módulos internos → componentes hijo.
- Selector siempre con prefijo `app-` y kebab-case.
- Componentes con cinco bloques en orden estricto: imports, decorador, propiedades públicas, propiedades privadas, métodos.
- Para inyección de dependencias, `inject()` en lugar del constructor.
- Tests siempre con `TestBed` y un patrón Arrange-Act-Assert.
- Lifecycle hooks en orden de ejecución (ngOnInit antes de ngOnDestroy).

### El SKILL.md ampliado

```markdown
---
name: angular-component
description: Genera componentes Angular standalone con Signals siguiendo la estructura del equipo. Usar cuando el usuario pida crear un nuevo componente, haga referencia a un componente nuevo en una feature, o cuando el flujo requiera scaffolding de UI Angular.
---

# Generador de componentes Angular standalone

## Cuándo se usa este skill

Cuando el usuario pida crear un componente Angular nuevo. Esto incluye peticiones como "crea un componente para X", "necesito un componente OrdersList", "vamos a hacer la UI de Y", o referencias implícitas a un componente que no existe aún.

## Estructura del componente

Cada componente generado tiene cuatro ficheros:

- `<nombre>.component.ts` — clase del componente
- `<nombre>.component.html` — template
- `<nombre>.component.scss` — estilos
- `<nombre>.component.spec.ts` — tests unitarios

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

Siempre `standalone: true`. Selector con prefijo `app-` y nombre en kebab-case.

### Estructura de la clase

Bloques en este orden, separados por una línea en blanco:

1. Inputs (signals input)
2. Outputs (signals output)
3. Inyecciones con `inject()`
4. Estado local (signals)
5. Valores derivados (computed)
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
- Control flow nuevo: `@if`, `@for`, `@switch`. No usar las directivas estructurales antiguas.

## Convenciones del fichero spec.ts

- `TestBed.configureTestingModule()` con la configuración mínima necesaria.
- Estructura Arrange-Act-Assert explícita en cada test, con comentarios `// Arrange`, `// Act`, `// Assert`.
- Mocks con `jasmine.createSpyObj` para servicios inyectados.
- Tests para: creación del componente, comportamiento de inputs, eventos de outputs, lógica de signals computed.

## Ubicación del componente

Por defecto: `src/app/components/<nombre>/`. Si el usuario indica otra ubicación o si el contexto del proyecto sugiere otra estructura (por ejemplo, dentro de un módulo de feature), seguir la estructura del proyecto.

## Lo que NO debe hacer el skill

- No generar `NgModule` para el componente nuevo (son standalone).
- No usar `Subject` o `BehaviorSubject` para estado interno (usar signals).
- No inyectar `ChangeDetectorRef` para forzar refresh (innecesario con signals).
- No añadir el componente al routing sin pedir confirmación.

## Pasos al generar

1. Si el usuario no ha dado nombre, preguntar.
2. Decidir ubicación según estructura del proyecto.
3. Generar los cuatro ficheros con las convenciones de arriba.
4. Verificar que el resultado compila (`npm run build` o `ng build`).
5. Si hay routing implicado, sugerir el snippet pero no aplicarlo sin confirmación.
```

Bastante más sustancia. Sigue siendo un solo fichero, pero ahora encapsula conocimiento real del equipo.

### Cuándo parar de añadir prosa

A medida que añades convenciones, el `SKILL.md` crece. Hay dos señales que dicen *"ya basta de prosa, hay que cambiar de estrategia"*:

**Señal 1: el fichero pasa de 2.000 palabras.** Anthropic recomienda mantenerlo por debajo. Más allá, el cuerpo del skill ocupa demasiado contexto al activarse.

**Señal 2: las convenciones se vuelven repetitivas o muy detalladas.** Cuando estás escribiendo el quinto bloque de código de ejemplo en prosa, es señal de que el contenido ya no es prosa — son plantillas. Y las plantillas viven mejor en `assets/`.

Cuando llegues a una de las dos señales, toca pasar a la siguiente versión.

---

## Scripts ejecutables: el siguiente nivel

Antes de meter plantillas, conviene hablar de scripts. Es la pieza que más diferencia a un skill básico de uno potente.

### El concepto

Un skill puede ejecutar código. Cuando hay tareas deterministas — leer información, validar algo, parsear ficheros, contar elementos — es mucho mejor que el skill ejecute un script que pedirle al modelo que razone. Más fiable, más rápido, más barato.

Hay dos sintaxis para ejecutar comandos dentro de un `SKILL.md`:

### Sintaxis inline: `` !`comando` ``

Para comandos cortos cuya salida quieres incrustar directamente:

```markdown
## Estado del proyecto

Versión actual: !`cat package.json | grep version | head -1`
Branch: !`git branch --show-current`
Último commit: !`git log -1 --format=%s`
```

Cuando el skill se carga, esos comandos se ejecutan y la salida se incrusta en el contexto. Útil para inyectar contexto dinámico.

### Sintaxis en bloque: bloques ` ``` ! `

Para acciones más largas:

````markdown
## Setup inicial

Antes de generar el componente, comprobar el estado del proyecto:

```!
ng version
node --version
ls src/app/components/ 2>/dev/null || echo "carpeta components no existe"
```
````

Estos bloques se ejecutan antes de que Claude proceda con su tarea principal, dando al agente contexto fresco sobre el estado del entorno.

### Casos típicos de uso

**Verificación de prerequisitos:**

```!
which ng || (echo "Angular CLI no instalado"; exit 1)
node --version | grep -q "v22" || echo "Aviso: Node 22 LTS recomendado"
```

**Inyección de contexto del proyecto:**

```!
echo "Componentes existentes:"
ls src/app/components/ 2>/dev/null
echo ""
echo "Servicios disponibles:"
ls src/app/services/ 2>/dev/null
```

**Setup automático:**

```!
mkdir -p src/app/components
```

Esto al ejecutarse el skill pero antes de generar nada se asegura que la estructura está lista.

### Cuándo usar scripts y cuándo prosa

La regla práctica:

- **Si la tarea tiene una respuesta correcta determinista** → script. Calcular un GUID, leer la versión del package.json, contar ficheros, validar un schema.
- **Si la tarea requiere criterio o adaptación al contexto** → prosa para que el modelo razone.

No mezcles. Un skill que pone en prosa cosas que un script haría en una línea es un skill mal escrito. Y al revés también: un skill que pretende que un script tome decisiones de criterio acaba siendo frágil.

### Una advertencia sobre seguridad

Los scripts en `SKILL.md` se ejecutan con los permisos de la sesión. Si tu skill tiene un comando `Bash` que accede a algo sensible, eso queda registrado y puede ejecutarse cada vez que el skill se activa.

Buenas prácticas:

- Mantén los scripts read-only en la medida de lo posible.
- Si necesitas ejecutar comandos con efectos secundarios, deja claro en la descripción del skill que los va a ejecutar.
- Si un skill ejecuta `git push`, `rm -rf`, o cualquier acción destructiva, considera marcarlo con `disable-model-invocation: true` para que solo se invoque explícitamente.

---

## Versión 3: con plantillas en `assets/`

Cuando las convenciones se vuelven muy específicas y empezarías a meter bloques largos de código en el `SKILL.md`, toca extraer plantillas a `assets/`.

### Estructura

```
.claude/skills/angular-component/
├── SKILL.md
└── assets/
    ├── component.template.ts
    ├── component.template.html
    └── component.template.spec.ts
```

### Una plantilla concreta

`assets/component.template.ts`:

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
  // Inputs
  // {{INPUTS}}

  // Outputs
  // {{OUTPUTS}}

  // Inyecciones
  // {{INJECTIONS}}

  // Estado local
  // {{STATE}}

  // Computed
  // {{COMPUTED}}

  // Lifecycle hooks
  ngOnInit(): void {
    // {{ON_INIT}}
  }

  ngOnDestroy(): void {
    // {{ON_DESTROY}}
  }

  // Métodos públicos
  // {{PUBLIC_METHODS}}

  // Métodos privados
  // {{PRIVATE_METHODS}}
}
```

Las marcas `{{...}}` son placeholders que el skill rellena según el contexto.

### Cómo referenciar la plantilla desde `SKILL.md`

En el cuerpo del skill, una sección como:

```markdown
## Plantillas

Para generar el componente, usa las plantillas disponibles en `assets/`:

- `assets/component.template.ts` para el fichero TypeScript
- `assets/component.template.html` para el template
- `assets/component.template.spec.ts` para los tests

Cada plantilla tiene placeholders entre `{{...}}` que debes rellenar según la información del componente:

- `{{KEBAB_NAME}}` — nombre del componente en kebab-case (ej: `orders-list`)
- `{{PASCAL_NAME}}` — nombre del componente en PascalCase (ej: `OrdersList`)
- `{{IMPORTS_*}}` — imports según las dependencias
- `{{INPUTS}}` — declaraciones de inputs
- `{{OUTPUTS}}` — declaraciones de outputs
- `{{INJECTIONS}}` — inyecciones con `inject()`
- ... etc.

Lee la plantilla con `Read`, sustituye los placeholders, y escribe el resultado con `Write`.
```

### Ventajas de las plantillas en `assets/`

**Una:** el `SKILL.md` se mantiene corto. Las plantillas pesan tokens solo cuando Claude las lee, y solo lee las que necesita.

**Dos:** las plantillas se versionan como código real. Si tu equipo cambia la convención, modificas la plantilla, no la prosa del skill.

**Tres:** otros skills pueden reutilizar las mismas plantillas. Si tienes un skill `angular-page` que también genera componentes (de un tipo distinto), pueden compartir partes.

---

## Versión 4: con script ejecutable

La última capa. Cuando una parte del trabajo es estrictamente determinista, mejor lo hace un script.

### Cuándo merece la pena

No todos los skills necesitan scripts. Pero hay tareas donde sí justifican el esfuerzo:

- **Conversiones de formato.** Pasar `OrdersList` a `orders-list` para el selector. Sí, el modelo lo hace bien — pero un script lo hace siempre y ocupa cero contexto.
- **Generación con datos del proyecto.** Calcular qué imports necesitas en función de los servicios disponibles en el proyecto.
- **Validaciones.** Verificar que un nombre no colisiona con un componente existente antes de generarlo.

### Un script de ejemplo

`scripts/generate.py`:

```python
#!/usr/bin/env python3
"""
Genera la información del componente a partir de un nombre.
Devuelve un JSON con los nombres en distintos formatos y la ubicación esperada.
"""
import json
import os
import re
import sys

def main():
    if len(sys.argv) < 2:
        print(json.dumps({"error": "Falta el nombre del componente"}))
        sys.exit(1)

    name_input = sys.argv[1]

    # Conversión PascalCase → kebab-case
    kebab = re.sub(r'(?<!^)(?=[A-Z])', '-', name_input).lower()
    pascal = ''.join(word.capitalize() for word in re.split(r'[-_ ]', name_input))
    selector = f"app-{kebab}"

    # Comprobar si la carpeta ya existe
    target_dir = f"src/app/components/{kebab}"
    exists = os.path.isdir(target_dir)

    result = {
        "kebab": kebab,
        "pascal": pascal,
        "selector": selector,
        "target_dir": target_dir,
        "already_exists": exists
    }

    print(json.dumps(result, indent=2))

if __name__ == "__main__":
    main()
```

### Cómo el skill llama al script

En `SKILL.md`, una sección como:

````markdown
## Pasos al generar

### Paso 1: calcular la información del componente

Ejecuta el script para obtener los nombres en distintos formatos y verificar si el componente ya existe:

```!
python scripts/generate.py "{{NOMBRE_DADO_POR_USUARIO}}"
```

El output es un JSON con `kebab`, `pascal`, `selector`, `target_dir` y `already_exists`.

### Paso 2: si already_exists es true

Avisa al usuario y pregunta si quiere sobrescribir antes de continuar.

### Paso 3: si no existe

Procede a generar los ficheros usando las plantillas de `assets/` con los valores devueltos por el script.
````

### El skill completo, estructura final

Después de las cuatro versiones, la estructura del skill es:

```
.claude/skills/angular-component/
├── SKILL.md                    # ~1.500 palabras: convenciones + workflow
├── scripts/
│   └── generate.py             # cálculo determinista de nombres y ubicación
└── assets/
    ├── component.template.ts
    ├── component.template.html
    └── component.template.spec.ts
```

Cuatro elementos. Cada uno hace su trabajo. El `SKILL.md` orquesta, los scripts deterministan, las plantillas materializan.

Esto es lo que se considera un skill *"a nivel producción"*. Si se lo das a un compañero, puede usarlo sin saber cómo está hecho por dentro. Si tu equipo cambia una convención, modificas la pieza correspondiente. Y la próxima vez que generes un componente, el resultado es indistinguible de uno hecho a mano por un senior del equipo.

---

## Control de invocación

Aparte de la activación automática por descripción, los skills tienen mecanismos para controlar cómo se invocan.

### `disable-model-invocation`

```yaml
---
name: db-reset
description: Resetea la BBDD local borrando datos y reaplicando migraciones.
disable-model-invocation: true
---
```

Cuando esto es `true`, el skill **solo se ejecuta si el usuario lo invoca explícitamente** con `/db-reset`. Claude no lo activa por su cuenta aunque la descripción coincida con la petición.

¿Cuándo usar?

- **Skills destructivos.** Borrar BBDD, hacer deploy, push forzado, eliminar ficheros. Cosas que no quieres que pase nunca por accidente.
- **Skills caros.** Si el skill consume mucho contexto o lanza tareas largas, mejor que sea siempre intencional.
- **Skills experimentales.** Mientras pruebas un skill nuevo, puede ayudar tenerlo solo invocable explícitamente para que no se active en sitios inesperados.

### `argument-hint`

```yaml
---
name: angular-component
description: Genera un componente Angular...
argument-hint: <nombre-del-componente>
---
```

Solo afecta a la invocación por slash command. Cuando el usuario escribe `/angular-component`, le aparece la pista de que espera un argumento. Útil para skills que tienen un parámetro principal claro.

### Slash command de skill

Todo skill que esté `user-invocable` (que es el default) se puede invocar también con slash command. Si tu skill se llama `angular-component`, escribir `/angular-component crear orders-list` lo activa explícitamente con ese argumento.

¿Cuándo usar invocación por slash en lugar de dejar que el agente decida?

- **Cuando quieres ser explícito.** Sabes que necesitas ese skill concreto, no quieres jugar a la lotería de la activación automática.
- **Cuando la activación automática no es fiable.** Si tu descripción no acaba de afinar y prefieres invocar a mano mientras la iteras.
- **Cuando el skill es de uno de varios similares.** Tienes `angular-component` y `angular-page` y quieres asegurarte de invocar el correcto.

---

## Subagentes en skills (referencia rápida)

```yaml
---
name: dotnet-deep-review
description: Revisa exhaustivamente un módulo .NET buscando problemas profundos
context: fork
---
```

`context: fork` hace que el skill se ejecute en un **contexto aislado**. Tiene su propia ventana de contexto, su propio razonamiento, y devuelve el resultado al agente principal sin contaminar.

Esto es útil cuando:

- El skill necesita explorar mucho contenido (leer un módulo entero, analizar muchos ficheros) y no quieres que ese contenido pese en tu sesión principal.
- El skill hace una tarea con sus propias decisiones que prefieres que no influyan en lo que estás haciendo en paralelo.

El deep dive de subagentes es el **módulo 3** completo. Aquí solo introducimos la sintaxis para que sepas que existe. En 3.1 lo veremos con detalle.

---

## Scopes: dónde vive cada skill

Tres ubicaciones, cada una con su lógica:

### Personal: `~/.claude/skills/`

Tus skills personales. Viajan contigo de proyecto en proyecto. Aquí van:

- Convenciones que aplicas siempre, en cualquier proyecto. *"Comenta el código en español"*, *"explica los conceptos como a un junior"*.
- Skills de productividad personal. *"Escribe el commit como yo lo escribiría"*, *"resume este PR en lenguaje claro"*.
- Experimentos antes de promoverlos al equipo.

### Proyecto: `.claude/skills/`

Skills del equipo, van a git, se comparten al clonar. Aquí van:

- Convenciones específicas del proyecto. Generadores con la estructura del equipo.
- Code reviews con el checklist del equipo.
- Cualquier skill que aplica a todo el que trabaje en este repo.

### Plugin

Empaquetados dentro de un plugin distribuible. Esto lo veremos en 2.3 cuando hablemos de cómo distribuir un kit completo de skills + MCP servers como un paquete.

### Cómo decidir entre personal y proyecto

La regla:

- **¿Aplica solo a este proyecto?** → proyecto.
- **¿Aplica a varios proyectos del mismo cliente?** → proyecto pero copiado en cada uno (o plugin si son muchos).
- **¿Aplica a tu trabajo en general?** → personal.
- **¿Es algo que un compañero del equipo se beneficiaría de tener?** → proyecto, va a git.

Y un patrón típico que conviene tener en cuenta: empezar como personal, promover a proyecto cuando se valida.

Cuando descubres una nueva forma de hacer algo con Claude Code, lo natural es empezar el skill en `~/.claude/skills/` para experimentar sin afectar a nadie. Cuando ves que funciona y sería útil para el equipo, lo mueves a `.claude/skills/` y lo commiteas. Esta progresión personal → proyecto da espacio para iterar sin presionar al equipo con experimentos a medias.

---

## Errores frecuentes con tus primeros skills

Lista de los anti-patrones que casi todo el mundo comete con sus primeros skills:

- **Skill demasiado grande.** El típico *"un skill que hace todo lo de generación de componentes, páginas, módulos, services..."*. Mejor varios skills pequeños y especializados que uno gordo. Activan mejor y son más fáciles de mantener.
- **Empezar por la versión 4.** No hace falta meter scripts y plantillas el primer día. Empieza con un `SKILL.md` simple, úsalo, ve qué falla, y añade capas según se justifiquen. La sobreingeniería es el primer enemigo.
- **No iterar la descripción.** Tu primera descripción casi nunca es la final. Lánzala, ve cuándo se activa y cuándo no, ajusta. La activación es probabilística.
- **No testar después de cambios.** Tras modificar un skill, lánzalo en una sesión nueva y verifica que sigue activando como esperas. Es fácil romper la activación al refinar.
- **Convenciones que duplican lo que ya hace Claude.** Si Claude sin skill ya genera componentes Angular standalone bien, un skill que solo dice *"genera componentes Angular standalone"* no aporta. El valor está en codificar **las particularidades de tu equipo**, no las prácticas generales.
- **Mezclar skills que deberían estar en CLAUDE.md.** Si tu skill aplica a *todas* las tareas del repo (estructura de carpetas, comandos de build), no es skill — va a `CLAUDE.md`. Skills son para tareas concretas.
- **No documentar dentro del skill por qué se hacen las cosas.** Cuando otro miembro del equipo (o tú dentro de seis meses) lo lea, va a querer saber por qué hay esa convención. Un comentario corto en el `SKILL.md` justificando decisiones no obvias se agradece.
- **Mezclar lógica determinista con razonamiento del modelo.** Si un script puede hacerlo bien, no se lo pidas al modelo. Y al revés también — no metas en script lo que requiere criterio.

---

## Antes de seguir

Has construido un skill desde cero en cuatro versiones progresivas: el mínimo, con convenciones, con plantillas y con script. Has visto los mecanismos de control de invocación, dónde vive cada tipo de skill, y los anti-patrones más comunes.

En el siguiente apartado salimos del taller individual y miramos al ecosistema. Hay muchos skills ya escritos por Anthropic y por la comunidad. Hay formas de empaquetarlos y distribuirlos como plugins. Y hay consideraciones de seguridad importantes — un skill de un tercero con permisos amplios no es algo que metas en tu repo a la ligera.

Antes de pasar, una pregunta:

¿Qué skill que has hecho en los últimos 60 minutos podría dar el salto de "experimento personal" a "skill del equipo"? Si la respuesta es *"el de generación de componentes"*, perfecto — es justo el patrón que se beneficia de codificar las convenciones del equipo. Si la respuesta es *"ninguno todavía, prefiero practicar más antes"*, también está bien — la promoción a equipo se hace cuando estás seguro de que funciona, no por presión de etiquetas.

Lo importante es que ya tienes el modelo mental para construir skills de verdad. La parte de aprender se acaba aquí. La parte de practicar empieza en cuanto vuelvas a tu repo del trabajo.
