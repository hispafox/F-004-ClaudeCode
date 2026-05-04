> **Versión:** v3 | **Módulo:** 1 | **Sub:** 1.3b | **Slides:** 25 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M01-S1.3b-permisos-workflows-v3.md`

# Submódulo 1.3b — Permisos en runtime y workflows

## Slide 1 — Portada
**Módulo 1 · Submódulo 1.3 · Parte B**
Permisos en runtime y workflows del día a día
Cómo trabajar con Claude Code en sesión

---

## Slide 2 — Dónde estamos

En la parte A vimos los modos de uso, los slash commands esenciales, `/compact` en profundidad y tool search.

Ahora viene la parte de cómo decides en sesión:

```
1. Permisos en runtime
   → Qué hacer cuando el agente te pide aprobación

2. Workflows del día a día
   → Cómo combinar modos + comandos en flujos reales

3. Errores frecuentes con todo esto

4. Cierre del módulo 1
```

---

## Slide 3 — Permisos en runtime

En el apartado anterior — el 1.2 — cubrimos cómo configurar permisos en `settings.json`. El setup persistente, el del equipo, el que va a git.

Aquí vemos lo otro:

> **Qué hacer cuando estás en sesión y el agente te pide aprobación.**

---

## Slide 4 — El flujo de aprobación

Cuando Claude va a usar una herramienta que no está en tu `allow`, te pregunta. Lo que ves en pantalla:

```
The agent wants to run:
  Bash: dotnet ef migrations add AddCancelOrderColumn

[A]llow once  [Y]es, allow always  [N]o, deny  [E]dit
```

Cuatro opciones. Las vemos.

---

## Slide 5 — Las cuatro opciones del prompt

```
[A]llow once
└── Permite esta vez. No cambia tu configuración.
    La opción más segura cuando dudas.

[Y]es, allow always
└── Permite y añade el patrón a tu allow.
    La sesión y las futuras lo permiten sin preguntar.
    Útil para comandos que sabes que vas a usar mucho.

[N]o, deny
└── Bloquea esta vez.
    El agente busca alternativas o te dice que no puede continuar.

[E]dit
└── Modificas la propuesta.
    Útil cuando "sí, lánzalo, pero con un parámetro distinto".
```

---

## Slide 6 — El patrón que más cuesta acertar

La mayoría de la gente, en su primera semana, hace una de dos cosas.

```
PATRÓN A — Aprobar todo a ciegas
└── "Sí, sí, sí, sí..."

PATRÓN B — Aprobar todo individualmente sin promover a "always"
└── "Mejor lo controlo cada vez"
```

Los vemos.

---

## Slide 7 — Patrón A: aprobar todo a ciegas

```
"Sí, sí, sí, sí..."

La fricción de aprobar les molesta
y le dan a "Yes always" a todo lo que se ponga delante.
```

**Resultado:** se cargan el modelo de seguridad.

```
Un día el agente decide que la mejor forma
de resolver un problema es algo que no querían

└── Y resulta que ya tienen permiso
    para hacerlo.
```

---

## Slide 8 — Patrón B: aprobar todo individualmente

```
"Mejor lo controlo cada vez"

Aprueban "Allow once" siempre, nunca promueven a "always".
```

**Resultado:** aprueban 200 veces el mismo `Bash(dotnet test)` durante una sesión.

```
Y al final acaban frustrados con la herramienta.
```

---

## Slide 9 — El patrón sano: el del medio

```
┌─────────────────────────────────────────────────────────┐
│                                                         │
│   PROMOVER A "ALWAYS" lo que es seguro y repetitivo:    │
│                                                         │
│   ├── Bash(dotnet test)                                 │
│   ├── Bash(npm run *)                                   │
│   ├── Read                                              │
│   └── Edit                                              │
│                                                         │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│                                                         │
│   MANTENER "ONCE" para lo que tiene riesgo:             │
│                                                         │
│   ├── Bash(rm *)                                        │
│   ├── Bash(git push *)                                  │
│   └── Escrituras en ficheros sensibles                  │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

---

## Slide 10 — Cuándo bloquear con "no" (1/3)

Tres casos donde "no" es la respuesta correcta. Los vemos uno a uno.

**Caso 1: el agente propone algo que no entiendes.**

```
"Voy a ejecutar este comando que parece raro"

Si no puedes razonar lo que hace, NO LO APRUEBES.

Cuestiónaselo:
└── "¿por qué necesitas ejecutar eso?"
```

> La mayoría de las veces, cuando lo justifica,
> ves si tiene sentido o no.

---

## Slide 11 — Cuándo bloquear con "no" (2/3)

**Caso 2: el agente quiere modificar algo crítico que no has dicho que toque.**

```
"Voy a editar appsettings.Production.json para..."

Aunque la justificación parezca razonable
└── Si tú no lo pediste, "no".
    Y discútelo después.
```

---

## Slide 12 — Cuándo bloquear con "no" (3/3)

**Caso 3: te das cuenta de que el agente va por mal camino.**

```
A veces ves la propuesta
y entiendes que el plan global no tiene sentido.

→ Bloquear.
→ Replantear la tarea.
```

**Una buena heurística:**

> Cuando dudes, di **"no"** y discútelo.
>
> El coste de un "no" es bajo (puedes volver a aprobar después).
> El coste de un "yes" mal dado puede ser alto.

---

## Slide 13 — El modo autónomo, en runtime

```bash
claude --dangerously-skip-permissions
```

Visto en 1.2 con detalle. Aquí solo el recordatorio:

> En el contexto de sesiones interactivas en tu portátil de trabajo, **nunca**.
>
> Solo en sandbox aislado o en CI controlado.

---

## Slide 14 — Workflows típicos del día a día

Hasta aquí los componentes. Ahora cómo se combinan en los flujos que más rinden.

**Cuatro patrones que vas a usar a diario:**

```
1. Implementación de feature
2. Refactor mediano
3. Code review asistido
4. Debugging
```

Y al final, el patrón anti-eficiente. Lo que **NO** rinde.

---

## Slide 15 — Patrón 1: implementación de feature

Cualquier feature de tamaño medio. Endpoint nuevo, componente Angular nuevo, módulo entero.

```
1. claude                                  # interactivo

2. /plan "implementa cancelación de pedidos en API y UI"

3. [revisas el plan, ajustas]

4. [le dices que proceda]

5. [trabaja, aprueba acciones según patrón sano]

6. /compact                                # cuando termina la primera fase

7. [le pides los tests]

8. /usage                                  # check antes de la siguiente sesión
```

---

## Slide 16 — Patrón 2: refactor mediano

Tocar 3-5 ficheros para cambiar una convención, simplificar un patrón, eliminar duplicación.

```
1. claude

2. /plan "refactoriza el manejo de errores
          para usar Result<T> en vez de excepciones"

3. [muy importante revisar el plan aquí —
    el agente puede proponer cambios más amplios
    de lo que querías]

4. [aprobar el plan o ajustarlo]

5. [trabaja]

6. dotnet test                              # validación al final
```

---

## Slide 17 — Patrón 3: code review asistido

Antes de subir un PR, una segunda mirada del agente.

```
1. git diff main...HEAD | claude -p "revisa estos cambios
                                     buscando bugs, problemas
                                     de seguridad y mala calidad.
                                     Sé concreto."
```

**Modo pipe + one-shot.** No abres sesión interactiva.

> En 30 segundos tienes una segunda opinión decente sobre tu PR
> antes de pedirla a un humano.

---

## Slide 18 — Patrón 4: debugging

El bug raro que te tiene atascado. Aquí tool search y `/compact` brillan especialmente.

```
1. claude

2. [le explicas el bug, le pasas el log relevante]

3. [el agente propone hipótesis,
    tú validas, descartas, pruebas]

4. /compact "conserva las hipótesis descartadas y por qué"

5. [seguís investigando]

6. /model opus                              # si Sonnet se atasca

7. [conseguir resolver]
```

---

## Slide 19 — El patrón anti-eficiente

Para contraste, el patrón que **NO** rinde:

```
1. claude

2. [le pides cosa pequeña sin contexto]

3. [te molesta que tarde]

4. /clear cuando deberías /compact

5. [repites el mismo trabajo]

6. [no usas /plan en una tarea que toca 8 ficheros]

7. [acabas frustrado]
```

> Si te ves en este flujo durante la primera semana, sí, es normal.
> Si sigues viéndote ahí en la tercera semana, hay algo de hábito que ajustar.

---

## Slide 20 — Errores frecuentes (1/2)

Lista de los anti-patrones más típicos que se ven en alumnos durante la primera semana:

```
❌ /clear CUANDO DEBERÍAS /compact
   El más común. Pierdes el contexto de la tarea actual.
   └── Aprende a distinguir:
       /clear  → para cambiar de tarea
       /compact → para seguir en la misma

❌ NO USAR /plan EN TAREAS GRANDES
   Si la tarea va a tocar 5+ ficheros, /plan te ahorra retrocesos.
   └── La fricción de revisar el plan compensa.

❌ IGNORAR /usage HASTA LLEGAR AL LÍMITE
   Lánzalo cada 20-30 min.
   └── No es opcional si trabajas sesiones largas.

❌ APROBAR TODO CON "Yes, allow always" SIN PENSAR
   Te cargas el modelo de seguridad.
   └── Sé selectivo.
```

---

## Slide 21 — Errores frecuentes (2/2)

```
❌ APROBAR TODO CON "Allow once" SIEMPRE
   Acabas con fatiga de aprobaciones.
   └── Promueve a "always" lo seguro y repetitivo.

❌ NO USAR EL MODO ONE-SHOT PARA AUTOMATIZAR
   Mucha gente solo conoce el interactivo.
   └── Cuando empieces a meter Claude Code en hooks de pre-commit,
       en CI, en scripts, descubrirás otro nivel de utilidad.

❌ PELEARSE CON UN MODELO QUE SE ATASCA
   Si Sonnet falla dos veces en lo mismo, /model opus y sigue.
   └── No te empeñes.

❌ ACTIVAR --dangerously-skip-permissions EN MÁQUINA DE CLIENTE
   Recordatorio.
   └── Está aquí porque sigue pasando.
```

---

## Slide 22 — Cierre del módulo 1

Llegado a este punto tienes:

```
✅ Claude Code instalado y autenticado
✅ Un CLAUDE.md que da contexto razonable de tu proyecto
✅ .claude/settings.json configurado al nivel correcto
   con tus permisos
✅ Los tres modos de uso en la mochila
   (interactivo, one-shot, pipe)
✅ Los diez slash commands esenciales
✅ /compact como hábito de los 20 minutos
✅ Criterio para gestionar permisos en runtime
✅ El cheatsheet completo de los 26 comandos como referencia
```

> Esto es **base operativa**.
> Con esto ya puedes empezar a usar Claude Code para tareas reales.

---

## Slide 23 — Lo que viene en el módulo 2

Lo que viene en el módulo 2 (Skills) es lo que convierte esta base genérica en una herramienta especializada para tu equipo.

```
Si Claude Code así de serie ya es útil
└── Con skills propios pasa a ser otra cosa.
```

Pero antes de saltar al módulo 2, una pregunta importante.

---

## Slide 24 — La pregunta antes del módulo 2

```
┌──────────────────────────────────────────────────────────────┐
│                                                              │
│   ¿Qué patrón se repite tres veces a la semana                │
│   en tu equipo y al que tendrías que enseñarle               │
│   a un junior nuevo?                                         │
│                                                              │
└──────────────────────────────────────────────────────────────┘
```

**Ejemplos de patrones reales:**

```
"Cuando creas un controller,
 siempre va con este DTO,
 este validator y este test"

"Cuando tocas el dominio,
 hay que ejecutar este check de invariantes"

"Cuando subes un PR,
 este checklist de seguridad"
```

---

## Slide 25 — Por qué importa esa pregunta

```
Cada uno de esos patrones es candidato a SKILL.
```

**Tener uno o dos identificados antes de la siguiente sesión** hace que el módulo 2 vaya mucho más rápido.

```
No estarás aprendiendo el concepto de skill en abstracto.
└── Lo estarás aprendiendo aplicándolo
    a algo que ya sabes que hace falta.
```
