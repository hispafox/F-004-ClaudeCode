---
name: angular-component
description: Genera un componente Angular 19 standalone con Signals para OrderManagement, siguiendo las convenciones del equipo — imports en orden estricto, estructura de clase con 8 bloques en orden, inject() en lugar de constructor, control flow nuevo (@if/@for/@switch), tokens del design system desde frontend/src/styles/_tokens.scss. Usar cuando el usuario pida crear, generar o añadir un componente Angular nuevo (con verbos como "crea", "genera", "añade", "necesito un componente").
---

# angular-component

Generador de componentes Angular 19 standalone con Signals para OrderManagement.

## Cuándo se usa

Activar este skill cuando el usuario pida añadir un componente Angular nuevo al
frontend del proyecto. Ejemplos de petición: "crea un componente para mostrar
el resumen de un pedido", "añade un filtro de pedidos", "necesito un componente
de paginación".

NO usar este skill para:

- Modificar componentes existentes (ahí va edición directa, no generación).
- Crear servicios, directivas, pipes o módulos.
- Tocar el backend (.NET) o la configuración del proyecto.

## Estructura del componente

Cada componente generado vive en su propia carpeta dentro de
`frontend/src/app/components/<kebab-name>/` con cuatro ficheros:

```
frontend/src/app/components/<kebab-name>/
├── <kebab-name>.component.ts      (clase standalone con signals)
├── <kebab-name>.component.html    (template con control flow nuevo)
├── <kebab-name>.component.scss    (estilos consumiendo tokens)
└── <kebab-name>.component.spec.ts (test mínimo de creación)
```

## Convenciones del fichero `.ts`

La clase del componente sigue **8 bloques en orden estricto**:

1. **Imports** — orden: Angular core → Angular common/forms/router → terceros
   → módulos del proyecto. Líneas en blanco entre grupos.
2. **Decorador `@Component`** — selector con prefijo `app-`, `standalone: true`,
   `imports` declarado, `templateUrl` y `styleUrl` (singular en Angular 19).
3. **Inputs** declarados con la función `input<T>()` de Angular 19 (no decorador
   `@Input()`).
4. **Outputs** con `output<T>()`.
5. **Inyecciones** con `inject(Service)` en propiedades `private readonly`.
   No usar constructor para inyectar.
6. **Estado** con `signal<T>()` para valores locales y `computed<T>()` para
   derivados. Nunca propiedades mutables sueltas.
7. **Lifecycle hooks** (`ngOnInit`, `ngOnDestroy`...) si aplica, en orden de
   ejecución.
8. **Métodos** públicos primero, privados después.

## Convenciones del template

- **Control flow nuevo** (Angular 17+): `@if`, `@for`, `@switch`. Nunca
  `*ngIf`, `*ngFor` ni `*ngSwitch`.
- **Atributos en orden**: estructurales (`@if/@for`) → bindings (`[prop]`) →
  eventos (`(event)`) → atributos estáticos.
- **Estilos inline solo para tokens del design system** (`var(--space-4)`,
  `var(--color-primary)`, etc.). El resto va al `.scss`.
- **Pipes**: `| number`, `| date`, `| currency` con argumentos explícitos.

## Convenciones del fichero `.scss`

- Importar tokens vía `@use 'styles/tokens';` si se necesita acceso a mixins;
  los custom properties (`--color-*`, `--space-*`) están globales en
  `frontend/src/styles/_tokens.scss` y se consumen con `var(--...)`.
- Selector raíz: `:host` con `display: block` por defecto.
- Sin nesting agresivo; máximo 2 niveles de indentación.

## Convenciones del fichero `.spec.ts`

- Test mínimo de creación con `TestBed.configureTestingModule` y `componentInstance`
  no nulo.
- Imports del componente como `imports: [<PascalName>Component]` (es standalone).
- Sin tests funcionales en este nivel — los E2E se cubren con Playwright (módulo 5).

## Lo que NO debe hacer el skill

- NO crear servicios (`*.service.ts`).
- NO crear directivas, pipes ni guards.
- NO modificar `app.routes.ts`, `app.config.ts` ni el componente raíz.
- NO añadir el componente a ningún `imports` externo (es standalone, se
  importa donde se use vía `loadComponent` o `imports` directo).
- NO tocar el backend (.NET) ni los `.csproj`.
- NO instalar dependencias npm nuevas.

## Pasos al generar

1. Pedir al usuario el nombre del componente en PascalCase
   (`OrderSummary`, `OrderFilter`, etc.).
2. Convertir a kebab-case (`order-summary`, `order-filter`).
3. Verificar que `frontend/src/app/components/<kebab-name>/` no existe.
   Si existe, avisar y preguntar si sobrescribir.
4. Crear los cuatro ficheros con los 8 bloques en orden.
5. Confirmar al usuario el path y los ficheros generados.
6. Recordar que el componente se importa donde se use con
   `loadComponent: () => import('./components/<kebab-name>/<kebab-name>.component').then(m => m.<PascalName>Component)`.
