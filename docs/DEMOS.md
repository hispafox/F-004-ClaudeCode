# Registro de demos del curso

Cada gamma del curso tiene una demo asociada que avanza el proyecto
OrderManagement de alguna forma concreta. La mayoría de demos siguen el
patrón **before/after** (ver `docs/4-Demos/demo_M00-S0.2-...`): cada
sección no conceptual tiene dos ramas hermanas — `demo/X.Y-before`
(estado de partida del screencast) y `demo/X.Y-after` (estado final que
la siguiente clase asume). Las demos puramente conceptuales mantienen
rama única `demo/X.Y` (los cambios del screencast se descartan al final).

## Módulo 0 — Setup

- [x] **demo/0.1** — Setup del proyecto OrderManagement (rama única, sin screencast)

## Módulo 1 — Claude Code básico

- [x] **demo/1.1** — Hello Claude Code: el ciclo agentic en acción (CONCEPTUAL, rama única)
- [x] **demo/1.2a-before / demo/1.2a-after** — Instalación, autenticación y primer arranque
- [x] **demo/1.2b-before / demo/1.2b-after** — CLAUDE.md y settings.json para .NET 10 + Angular 19
- [x] **demo/1.3a-before / demo/1.3a-after** — Tres modos de uso, slash commands, /compact
- [x] **demo/1.3b-before / demo/1.3b-after** — Workflow completo: feature de cancelación end-to-end

## Módulo 2 — Skills

- [x] **demo/2.1a** — Anatomía de un skill leyendo los oficiales (CONCEPTUAL, rama única)
- [x] **demo/2.1b** — La descripción como switch: experimento con 4 versiones (CONCEPTUAL, rama única)
- [x] **demo/2.2a-before / demo/2.2a-after** — Primer skill creado: angular-component v1 y v2
- [x] **demo/2.2b-before / demo/2.2b-after** — angular-component v3 (assets) y v4 (scripts)
- [x] **demo/2.2c-before / demo/2.2c-after** — Control, scopes y cierre del bloque de creación
- [x] **demo/2.3** — Ecosistema y distribución (cierre módulo 2) (CONCEPTUAL, rama única)

## Módulo 3 — Agent harness

- [x] **demo/3.1a** — Subagentes integrados: Explore, Plan, general-purpose (CONCEPTUAL, rama única)
- [ ] demo/3.1b-before / demo/3.1b-after — Subagente custom: dotnet-reviewer
- [ ] demo/3.2a-before / demo/3.2a-after — Orquestación: aislamiento, composición, loops
- [ ] demo/3.2b-before / demo/3.2b-after — Memoria, paralelo, agent teams
- [ ] demo/3.3a-before / demo/3.3a-after — Primer hook PostToolUse
- [ ] demo/3.3b-before / demo/3.3b-after — Hooks completos

## Módulo 4 — Diseño integrado

- [ ] demo/4.1a-before / demo/4.1a-after — Figma MCP conectado
- [ ] demo/4.1b-before / demo/4.1b-after — Tokens extraídos a _tokens.scss
- [ ] demo/4.2a-before / demo/4.2a-after — Claude Design creando notificaciones
- [ ] demo/4.2b-before / demo/4.2b-after — Onboarding del design system
- [ ] demo/4.3a-before / demo/4.3a-after — DESIGN.md anatomía completa
- [ ] demo/4.3b-before / demo/4.3b-after — CLI design.md en CI

## Módulo 5 — Handoff y testing

- [ ] demo/5.1a-before / demo/5.1a-after — Handoff bundle generado
- [ ] demo/5.1b-before / demo/5.1b-after — Handoff completo importado
- [ ] demo/5.2-before / demo/5.2-after — Flujo combinado en acción
- [ ] demo/5.3a-before / demo/5.3a-after — Tests xUnit autogenerados
- [ ] demo/5.3b-before / demo/5.3b-after — Workflow completo: feature de cancelación
