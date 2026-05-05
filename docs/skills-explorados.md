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
