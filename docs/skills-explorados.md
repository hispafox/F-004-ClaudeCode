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

---

### Decisiones operativas (2.2c)

- **Cuándo usar `disable-model-invocation: true`:** solo en skills que sean (1)
  destructivos (borran datos), (2) caros (consumen mucho tokens o llaman a
  APIs externas con coste), o (3) experimentales (todavía no validados). El
  ejemplo del repo es `db-reset`. Aplicarlo a un skill útil lo anula —
  el modelo no lo va a invocar nunca por sí mismo, así que el alumno tiene
  que invocarlo siempre con `/<name>` explícito.

- **Regla personal → proyecto:** un skill nuevo se cocina primero en
  `~/.claude/skills/` (scope personal del dev), se valida durante 1-2 semanas
  en el flujo real, y solo cuando aporta valor reproducible se promueve a
  `.claude/skills/` (scope proyecto, va a git con el equipo). Es lo que se
  hizo con `commit-style` en esta demo. Promover antes de validar contamina
  el repo del equipo con experimentos a medio cocer.

- **Las 5 reglas técnicas críticas** que NO son negociables:
  1. El fichero se llama `SKILL.md` (case-sensitive).
  2. El nombre de la carpeta del skill va en kebab-case y coincide con el
     campo `name` del frontmatter.
  3. No empezar el `name` por `claude` ni `anthropic` (prefijos reservados).
  4. Sin XML en el frontmatter (es YAML estricto).
  5. La `description` no pasa de 1024 caracteres.
