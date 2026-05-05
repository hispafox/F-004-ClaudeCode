# Auditoría rápida de skills de la comunidad

Plantilla para evaluar cualquier skill de la comunidad antes de instalarlo
con `npx skills add`. Basada en el manual 2.3 del curso de Claude Code y
en el principio de mínimo privilegio aplicado a skills (manual 2.3 línea 168).

> **Por qué esto importa**: Snyk publicó a principios de 2026 un estudio
> sobre el ecosistema de skills donde encontró **prompt injection en el
> 36% de skills de terceros analizados** y **más de 1.400 payloads
> maliciosos** distribuidos. Los skills ejecutan código en tu entorno
> con los permisos que les das. Cinco minutos de auditoría te pueden
> ahorrar horas o algo peor.

## Datos del skill

- **Nombre del skill**:
- **Repo o URL**:
- **Autor**:
- **Última actualización**:
- **Stars / Forks**:

## Los 5 pasos

### 1. Mirar el repo en GitHub

- [ ] ¿Stars del repo? (orientativo, no decisivo)
- [ ] ¿Forks? (señal de uso real por otros equipos)
- [ ] ¿Fecha del último commit? (más de 14 meses sin commits = señal de abandono)
- [ ] ¿Issues abiertas vs cerradas? (mantenimiento activo)
- [ ] ¿Pull requests recientes mergeados? (comunidad activa)

### 2. Leer el SKILL.md

- [ ] ¿Qué herramientas pide en `allowed-tools`?
  - Si pide `Bash` sin restricciones → ⚠️ ROJO
  - Si pide `Write` y `Edit` sin caso justificado → ⚠️ AMARILLO
  - Si pide solo `Read`, `Grep`, `Glob` → ✅ VERDE
- [ ] ¿La descripción está bien escrita y es honesta sobre qué hace?
- [ ] ¿Hay instrucciones que parezcan disimuladas o ambiguas en el cuerpo?

### 3. Mirar los scripts (si los tiene)

Para cada script en `scripts/`:

- [ ] ¿Hace llamadas a internet con `curl`, `wget`, `requests`, `fetch`?
  - Si sí → revisar a qué URL y por qué
- [ ] ¿Lee variables de entorno sospechosas?
  - `AWS_*`, `GITHUB_TOKEN`, `*_SECRET`, `*_KEY`, `*_TOKEN` → ⚠️ ROJO
- [ ] ¿Escribe a paths fuera de la carpeta del skill?
  - Especialmente `~/`, `/etc`, `/usr` → ⚠️ ROJO
- [ ] ¿Instala dependencias con `pip install`, `npm install`, etc.?

### 4. Comprobar la descripción

- [ ] ¿Está la descripción bien escrita aplicando la fórmula de los tres
      ingredientes (verbo, abanico de triggers, contexto)?
- [ ] ¿Bajo 1024 caracteres? (regla técnica crítica)
- [ ] Una descripción mediocre suele acompañar a un skill mediocre.

### 5. Buscar reviews

- [ ] Buscar en Google: `<nombre del skill> claude code review`
- [ ] Buscar en GitHub Issues del propio repo: ¿hay quejas de seguridad?
- [ ] La comunidad suele señalar los problemáticos.

## Decisión

- [ ] **Verde**: instalar a nivel personal y probar.
- [ ] **Amarillo**: instalar con `disable-model-invocation: true` para invocación explícita solamente.
- [ ] **Rojo**: NO instalar. Buscar alternativa.

## Tras instalar (si decides instalar)

- [ ] **Restringe `Bash`** — nunca `Bash` a secas, siempre con patrón:
      `Bash(ng *)`, `Bash(npm test)`, `Bash(git status)`, etc.
- [ ] **Sandbox**: la primera vez que lo pruebas, en un repo sin
      información sensible.
- [ ] **Revisa allowed-tools** y restringe lo que pueda restringirse.
- [ ] Si dudas, marca con `disable-model-invocation: true` durante el
      período de prueba.

## Skills evaluados

(Aquí se va llenando a medida que se evalúan skills)

| Skill | Fecha | Decisión | Nota |
|-------|-------|----------|------|
|       |       |          |      |
