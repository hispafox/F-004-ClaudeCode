> **Versión:** v3 | **Módulo:** 2 | **Sub:** 2.2b | **Slides:** 32 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M02-S2.2b-scripts-plantillas-v3.md`

# Submódulo 2.2b — Scripts y plantillas

## Slide 1 — Portada
**Módulo 2 · Submódulo 2.2 · Parte B**
Scripts y plantillas
Versión 3 (assets/) y versión 4 (scripts/)

---

## Slide 2 — Dónde estamos

En la parte A construimos el skill `angular-component` en sus dos primeras versiones. Ya tienes un skill funcional con convenciones del equipo en prosa.

```
Y vimos que cuando el SKILL.md crece demasiado
(más de 2.000 palabras o con bloques de código repetitivos)
└── toca cambiar de estrategia.
```

Ahora vemos las dos capas que diferencian un skill básico de uno potente:

```
1. Scripts ejecutables
   └── Para tareas deterministas

2. Plantillas en assets/
   └── Para output con estructura predecible
```

Y al final, las versiones 3 y 4 del skill `angular-component` aplicando ambas.

---

## Slide 3 — Scripts ejecutables: el siguiente nivel

Antes de meter plantillas, conviene hablar de scripts. **Es la pieza que más diferencia a un skill básico de uno potente.**

```
Un skill puede ejecutar código.
```

```
Cuando hay tareas deterministas:
├── leer información
├── validar algo
├── parsear ficheros
└── contar elementos

Es mucho mejor que el skill ejecute un script
└── Que pedirle al modelo que razone.

├── Más fiable
├── Más rápido
└── Más barato
```

---

## Slide 4 — Dos sintaxis para ejecutar comandos

Hay dos formas de ejecutar comandos dentro de un `SKILL.md`:

```
1. SINTAXIS INLINE
   !`comando`

   Para comandos cortos cuya salida quieres
   incrustar directamente.

2. SINTAXIS EN BLOQUE
   ```!
   comando largo
   ```

   Para acciones más largas o con varias líneas.
```

Las vemos.

---

## Slide 5 — Sintaxis inline: !`comando`

Para comandos cortos cuya salida quieres incrustar directamente:

````markdown
## Estado del proyecto

Versión actual: !`cat package.json | grep version | head -1`
Branch: !`git branch --show-current`
Último commit: !`git log -1 --format=%s`
````

```
Cuando el skill se carga:
└── Esos comandos se ejecutan
    └── Y la salida se incrusta en el contexto.
```

> Útil para inyectar contexto dinámico:
> qué versión hay, en qué rama estás, cuál fue el último commit.

---

## Slide 6 — Sintaxis en bloque: bloques ```!

Para acciones más largas:

````markdown
## Setup inicial

Antes de generar el componente,
comprobar el estado del proyecto:

```!
ng version
node --version
ls src/app/components/ 2>/dev/null || echo "carpeta components no existe"
```
````

```
Estos bloques se ejecutan
ANTES de que Claude proceda con su tarea principal.

Dándole al agente contexto fresco
sobre el estado del entorno.
```

---

## Slide 7 — Caso típico 1: verificación de prerequisitos

```!
which ng || (echo "Angular CLI no instalado"; exit 1)
node --version | grep -q "v22" || echo "Aviso: Node 22 LTS recomendado"
```

```
El skill comprueba ANTES de hacer nada
que el entorno tiene lo que necesita.

Si falla:
├── O aborta
└── O avisa al usuario
```

> Patrón clásico:
> primero verificar, luego generar.

---

## Slide 8 — Caso típico 2: inyección de contexto del proyecto

```!
echo "Componentes existentes:"
ls src/app/components/ 2>/dev/null

echo ""
echo "Servicios disponibles:"
ls src/app/services/ 2>/dev/null
```

```
Inyecta en el contexto del agente
información sobre el estado actual del proyecto.

Antes de pedirle "crea el componente OrdersList"
└── el agente sabe qué componentes ya existen
    y qué servicios tiene disponibles.
```

> Reduce probabilidad de colisiones de naming
> y permite reutilizar lo que ya hay.

---

## Slide 9 — Caso típico 3: setup automático

```!
mkdir -p src/app/components
```

```
Antes de generar nada:
└── el skill se asegura de que la estructura está lista.

Si la carpeta /components no existe, la crea.
Si ya existe, mkdir -p no hace nada.

Idempotente, seguro.
```

> Ejecuciones repetidas dan el mismo resultado.
> No estropea nada si ya está hecho.

---

## Slide 10 — Cuándo usar scripts y cuándo prosa

La regla práctica:

```
SI LA TAREA TIENE UNA RESPUESTA CORRECTA DETERMINISTA
└── SCRIPT
    ├── Calcular un GUID
    ├── Leer la versión del package.json
    ├── Contar ficheros
    └── Validar un schema

SI LA TAREA REQUIERE CRITERIO O ADAPTACIÓN AL CONTEXTO
└── PROSA
    └── Para que el modelo razone
```

**Y atención a esto:**

```
NO mezcles.

Un skill que pone en prosa cosas que un script haría en una línea
└── es un skill mal escrito.

Y al revés también:
un skill que pretende que un script tome decisiones de criterio
└── acaba siendo frágil.
```

---

## Slide 11 — Una advertencia sobre seguridad

```
Los scripts en SKILL.md se ejecutan
con los permisos de la sesión.

Si tu skill tiene un comando Bash
que accede a algo sensible
└── eso queda registrado
    └── y puede ejecutarse
        cada vez que el skill se activa.
```

**Buenas prácticas:**

```
✅ Mantén los scripts read-only en la medida de lo posible.

✅ Si necesitas ejecutar comandos con efectos secundarios:
   └── Deja claro en la descripción del skill
       que los va a ejecutar.

✅ Si un skill ejecuta git push, rm -rf,
   o cualquier acción destructiva:
   └── Considera marcarlo con
       disable-model-invocation: true
       └── Para que solo se invoque explícitamente.
```

---

## Slide 12 — Versión 3: con plantillas en assets/

Cuando las convenciones se vuelven muy específicas y empezarías a meter bloques largos de código en el `SKILL.md`, **toca extraer plantillas a `assets/`**.

```
La idea:
├── El SKILL.md mantiene las INSTRUCCIONES
│   (qué hacer, cuándo, qué reglas)
│
└── assets/ guarda las PLANTILLAS
    (la estructura que se rellena)
```

> Separación clara entre lógica de qué hacer
> y material concreto que se usa para hacerlo.

---

## Slide 13 — Versión 3: la estructura

```
.claude/skills/angular-component/
├── SKILL.md
└── assets/
    ├── component.template.ts
    ├── component.template.html
    └── component.template.spec.ts
```

```
Tres plantillas, una por fichero generado:
├── La clase TypeScript
├── El template HTML
└── Los tests
```

---

## Slide 14 — Una plantilla concreta: component.template.ts (1/2)

`assets/component.template.ts`:

```typescript
import { Component, inject, signal, computed,
         OnInit, OnDestroy } from '@angular/core';
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
```

Las marcas `{{...}}` son **placeholders** que el skill rellena según el contexto.

---

## Slide 15 — Una plantilla concreta: component.template.ts (2/2)

```typescript
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

> Estructura completa de los 8 bloques en orden estricto.
> Cada placeholder se rellena con lo que toque.

---

## Slide 16 — Cómo referenciar la plantilla desde SKILL.md

En el cuerpo del skill, una sección como:

````markdown
## Plantillas

Para generar el componente, usa las plantillas
disponibles en `assets/`:

- `assets/component.template.ts` para el fichero TypeScript
- `assets/component.template.html` para el template
- `assets/component.template.spec.ts` para los tests

Cada plantilla tiene placeholders entre {{...}}
que debes rellenar según la información del componente:

- {{KEBAB_NAME}} — nombre en kebab-case (ej: `orders-list`)
- {{PASCAL_NAME}} — nombre en PascalCase (ej: `OrdersList`)
- {{IMPORTS_*}} — imports según las dependencias
- {{INPUTS}} — declaraciones de inputs
- {{OUTPUTS}} — declaraciones de outputs
- {{INJECTIONS}} — inyecciones con inject()
- ... etc.

Lee la plantilla con Read,
sustituye los placeholders,
y escribe el resultado con Write.
````

---

## Slide 17 — Versión 3: ventaja 1 — el SKILL.md se mantiene corto

```
Las plantillas pesan tokens
solo cuando Claude las lee.

Y solo lee las que necesita.
```

```
Si la tarea es generar un componente:
├── Lee component.template.ts → sí
├── Lee component.template.html → sí
└── Lee component.template.spec.ts → sí

Si la tarea es solo el componente sin tests
└── No lee la plantilla de tests.
```

> Carga bajo demanda, dentro del propio skill.

---

## Slide 18 — Versión 3: ventaja 2 — las plantillas se versionan como código real

```
Si tu equipo cambia la convención:
└── Modificas la plantilla
    └── No la prosa del skill
```

```
La plantilla es código.
Está en git.
Se le hacen PRs.
Se revisa.
```

> Cuando el equipo decide *"a partir de ahora `inject()` va arriba del todo"*,
> cambias el template.
> El SKILL.md no se toca.

---

## Slide 19 — Versión 3: ventaja 3 — reutilizables entre skills

```
Otros skills pueden reutilizar
las mismas plantillas.
```

```
Si tienes un skill angular-page
que también genera componentes (de un tipo distinto):
└── puede compartir partes
    con angular-component.
```

> No tienes que duplicar plantillas
> entre dos skills similares.

---

## Slide 20 — Versión 4: con script ejecutable

La última capa.

> Cuando una parte del trabajo es estrictamente determinista,
> **mejor lo hace un script.**

---

## Slide 21 — Cuándo merece la pena un script

No todos los skills necesitan scripts. Pero hay tareas donde sí justifican el esfuerzo:

```
CONVERSIONES DE FORMATO
├── Pasar OrdersList a orders-list para el selector.
└── Sí, el modelo lo hace bien
    └── Pero un script lo hace SIEMPRE
        Y ocupa CERO contexto.

GENERACIÓN CON DATOS DEL PROYECTO
└── Calcular qué imports necesitas
    en función de los servicios disponibles
    en el proyecto.

VALIDACIONES
└── Verificar que un nombre no colisiona
    con un componente existente
    antes de generarlo.
```

---

## Slide 22 — Un script de ejemplo: generate.py (1/2)

`scripts/generate.py`:

```python
#!/usr/bin/env python3
"""
Genera la información del componente a partir de un nombre.
Devuelve un JSON con los nombres en distintos formatos
y la ubicación esperada.
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
    pascal = ''.join(word.capitalize()
                     for word in re.split(r'[-_ ]', name_input))
    selector = f"app-{kebab}"
```

---

## Slide 23 — Un script de ejemplo: generate.py (2/2)

```python
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

```
Hace tres cosas, todas deterministas:

1. Calcula nombres en kebab-case y PascalCase
2. Calcula el selector con prefijo "app-"
3. Comprueba si la carpeta del componente ya existe
```

---

## Slide 24 — Cómo el skill llama al script (1/2)

En `SKILL.md`, una sección como:

````markdown
## Pasos al generar

### Paso 1: calcular la información del componente

Ejecuta el script para obtener los nombres
en distintos formatos y verificar
si el componente ya existe:

```!
python scripts/generate.py "{{NOMBRE_DADO_POR_USUARIO}}"
```

El output es un JSON con `kebab`, `pascal`,
`selector`, `target_dir` y `already_exists`.
````

---

## Slide 25 — Cómo el skill llama al script (2/2)

````markdown
### Paso 2: si already_exists es true

Avisa al usuario y pregunta si quiere sobrescribir
antes de continuar.

### Paso 3: si no existe

Procede a generar los ficheros usando las plantillas
de assets/ con los valores devueltos por el script.
````

```
Tres pasos.

El script hace el cálculo determinista.
El skill orquesta basándose en el output.
Las plantillas materializan el resultado.
```

---

## Slide 26 — El skill completo: estructura final

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

```
Cuatro elementos. Cada uno hace su trabajo.

├── El SKILL.md       → orquesta
├── Los scripts       → deterministan
└── Las plantillas    → materializan
```

---

## Slide 27 — Lo que se considera "skill nivel producción"

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   Si se lo das a un compañero:                           │
│   └── Puede usarlo sin saber cómo está hecho por dentro. │
│                                                          │
│   Si tu equipo cambia una convención:                    │
│   └── Modificas la pieza correspondiente.                │
│                                                          │
│   Y la próxima vez que generes un componente:            │
│   └── El resultado es indistinguible                     │
│       de uno hecho a mano por un senior del equipo.      │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

> Esto es la diferencia entre un experimento
> y una herramienta de equipo.

---

## Slide 28 — Recapitulación de las cuatro versiones

```
VERSIÓN 1 — el skill más simple posible
└── Un solo SKILL.md con frontmatter mínimo
    e instrucciones cortas.

VERSIÓN 2 — añadiendo convenciones del equipo
└── El SKILL.md crece para encapsular
    el conocimiento real del equipo.

VERSIÓN 3 — con plantillas en assets/
└── La estructura repetitiva sale del SKILL.md
    y vive como plantillas con placeholders.

VERSIÓN 4 — con script ejecutable
└── La lógica determinista se delega a un script.
    El skill orquesta. El código materializa.
```

> Cada versión es funcional por sí sola.
> Subes de capa cuando se justifica, no porque sí.

---

## Slide 29 — Cuándo NO subir de versión

Recordatorio importante:

```
NO TODOS LOS SKILLS NECESITAN LLEGAR A VERSIÓN 4.
```

```
Muchos skills útiles del día a día
└── viven perfectamente en versión 1 o 2.

Un skill de "escribe el commit como yo lo haría":
└── Probablemente versión 1.

Un skill de "revisa este PR contra checklist del equipo":
└── Probablemente versión 1 o 2.

Un skill de generación con muchos placeholders:
└── Versión 3.

Un skill que necesita validar nombres,
calcular paths, comprobar colisiones:
└── Versión 4.
```

---

## Slide 30 — Cómo decidir cuándo subir

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   ¿Mi SKILL.md tiene bloques de código repetitivos       │
│   con estructura fija que solo cambia en placeholders?   │
│                                                          │
│   → Versión 3 (plantillas en assets/)                    │
│                                                          │
│   ¿Hay tareas en mi skill que un script haría siempre    │
│   igual y mejor que el modelo razonando?                 │
│                                                          │
│   → Versión 4 (scripts/)                                 │
│                                                          │
│   ¿Mi SKILL.md sigue siendo manejable                    │
│   y funciona bien?                                       │
│                                                          │
│   → No subas. Estás bien donde estás.                    │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

---

## Slide 31 — Lo que tienes ahora

Después de las partes A y B del 2.2:

```
✅ Has construido un skill desde cero
✅ Lo has visto evolucionar en cuatro versiones
✅ Sabes cuándo extraer prosa a plantillas
✅ Sabes cuándo extraer lógica a scripts
✅ Conoces las dos sintaxis de scripts ejecutables
✅ Conoces los casos típicos donde brillan
✅ Sabes la regla scripts vs prosa
✅ Conoces la advertencia de seguridad
```

> Has cubierto la parte más práctica del módulo 2.

---

## Slide 32 — Lo que viene en 2.2c

```
SUBMÓDULO 2.2c — CONTROL, SCOPES Y CIERRE
─────────────────────────────────────────────────────

Control de invocación
├── disable-model-invocation
├── argument-hint
├── Slash command de skill
└── Cuándo usar invocación por slash

Subagentes en skills (referencia rápida)
└── context: fork

Scopes: dónde vive cada skill
├── Personal: ~/.claude/skills/
├── Proyecto: .claude/skills/
├── Plugin
└── Cómo decidir entre personal y proyecto
    Patrón típico: empezar personal, promover a proyecto

Errores frecuentes con tus primeros skills

Cierre del módulo 2.2 y bridge a 2.3
```

**Nos vemos en 2.2c.**
