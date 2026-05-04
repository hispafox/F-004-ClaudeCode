> **Versión:** v2 | **Módulo:** 5 | **Sub:** 5.3a | **Slides:** 42 | **Estado:** ✅ Versión final
> **Archivo:** `gamma_M05-S5.3a-tests-dotnet-mecanica-v2.md`

# Submódulo 5.3a — Tests en .NET con Claude Code: la mecánica

## Slide 1 — Portada
**Módulo 5 · Submódulo 5.3 · Parte A**
Tests en .NET con Claude Code: la mecánica
Pivote total, CLAUDE.md como contrato, tests unitarios, integración, code smells, documentación

---

## Slide 2 — Pivote total: del diseño al testing

```
Cambiamos de tema COMPLETAMENTE.
```

**Hasta aquí el curso ha cubierto:**

```
├── EL AGENTE Y SU PERSONALIZACIÓN (módulos 1-3)
└── INTEGRACIÓN CON DISEÑO (módulos 4 y la primera mitad del 5)
```

**Hemos visto:**

```
├── skills, subagentes, hooks, agent harness
├── Figma MCP, Claude Design, DESIGN.md
└── handoff bundle
```

```
Mucho material, mucho conceptual, mucha teoría aplicada.
```

> Ahora cambio de marcha.
>
> **UNA HORA muy concreta sobre UNA pieza específica
> donde Claude Code es BRUTALMENTE BUENO**:
>
> tests en .NET.

---

## Slide 3 — Por qué este es el caso de uso más adoptado

```
Si tuviera que elegir el caso de uso
que más equipos adoptan
después de probar Claude Code dos semanas:
```

```
SERÍA ESTE.
```

```
NO el más sofisticado.
NO el más sexy.
NO el más nuevo.
```

```
EL MÁS RENTABLE.
```

> Y por una razón clara que vamos a cubrir primero:
>
> **el match entre la herramienta y el problema
> es casi perfecto**.

---

## Slide 4 — Estructura de la parte A

```
1. Por qué tests es donde Claude Code rinde tanto
   — el match herramienta/problema

2. CLAUDE.md como CONTRATO del equipo
   — la pieza que más diferencia los outputs buenos
     de los mediocres

3. Tests unitarios con xUnit + NSubstitute + FluentAssertions
   — la pila moderna

4. El antipatrón estrella: tests que NO testean nada

5. Tests de integración con WebApplicationFactory
   — el patrón estándar de ASP.NET Core

6. Caso práctico guiado: tests para una API .NET de ejemplo

7. Detección de code smells y refactoring asistido

8. Documentación XML y OpenAPI/Swagger asistida
```

> En 5.3b veremos estrategia de cobertura,
> workflow completo y el cierre del curso entero.

---

## Slide 5 — El match herramienta/problema

```
Antes de meterse en mecánica,
conviene entender el porqué.
```

> Hay un MATCH casi perfecto
> entre lo que Claude Code hace bien
> y lo que el testing requiere.

**Tres razones por las que tests son terreno especialmente fértil:**

```
1. La tarea es REPETITIVA pero con CRITERIO.
2. El CONTRATO es claro.
3. La VERIFICACIÓN es objetiva.
```

> Las vemos.

---

## Slide 6 — Razón 1: la tarea es repetitiva pero con criterio

```
Escribir tests es:

├── 70% MECÁNICO
│   ├── estructura Arrange-Act-Assert
│   ├── mocks
│   └── asserts
│
└── 30% CRITERIO
    ├── qué casos cubrir
    └── qué edge cases importan
```

```
Claude Code automatiza el 70%
sin perderse en el 30%

— siempre que se lo expliques bien.
```

---

## Slide 7 — Razón 2: el contrato es claro

```
Para un servicio o un endpoint:

EL CONTRATO está en el código.

├── qué entra
├── qué sale
└── qué excepciones lanza
```

```
Claude Code puede LEERLO
y razonar contra él.
```

> NO tiene que **inventar** comportamiento.
>
> Tiene que **verificarlo**.

---

## Slide 8 — Razón 3: la verificación es objetiva

```
Un test PASA o FALLA.

NO hay opinión.
```

```
Eso significa que si Claude Code:

├── genera tests
└── los EJECUTA

SABE inmediatamente si lo hizo bien.
```

```
Es un LOOP CERRADO de validación.
```

> En tareas más subjetivas
> (escribir un texto, diseñar UI),
> Claude NO tiene esa señal automática.
>
> Aquí sí.

---

## Slide 9 — Las tres condiciones juntas

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│   Trabajo MECÁNICO                                       │
│   con CRITERIO ACOTADO                                   │
│   y VERIFICACIÓN INMEDIATA.                              │
│                                                          │
│   IDEAL para una herramienta agentic.                    │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

> Es donde más rinde.

---

## Slide 10 — Los datos: qué se ahorra realmente

Equipos que han adoptado Claude Code para tests reportan:

```
COBERTURA INICIAL DE UN SERVICIO NUEVO
└── Lo que tradicionalmente eran 2-3 HORAS de tedio
    (escribir 15-20 tests con sus mocks y sus asserts)
    └── baja a 20-30 MINUTOS
        de generación + revisión.

COBERTURA DE CÓDIGO LEGACY SIN TESTS
└── Un módulo viejo que llevaba MESES esperando
    que alguien le metiera tests
    └── ahora se puede atacar
        con una sesión de 1-2 HORAS
        y salir con cobertura razonable.

MANTENIMIENTO DE TESTS CUANDO EL CÓDIGO CAMBIA
└── Cuando refactorizas un servicio,
    los tests que se rompen
    se actualizan automáticamente
    con un PROMPT CORTO.
```

```
Lo que NO se ahorra: el CRITERIO de qué testear y qué no.
Eso sigue siendo tuyo.
Pero el trabajo de ESCRIBIR los tests, sí.
```

---

## Slide 11 — Por qué no es solo "generación de código"

```
Distinción importante.
```

**Hay herramientas que generan tests a partir del código existente:**

```
Escribes una función,
el plugin te genera tests para esa función.

Funcionan bien para casos básicos.
```

**Lo que Claude Code aporta encima:**

```
├── ENTENDER TU CODEBASE ENTERO,
│   no solo la función.
│   Conoce las convenciones, los servicios relacionados,
│   los patrones del equipo.
│
├── GENERAR TESTS QUE RESPETAN TUS REGLAS
│   del CLAUDE.md.
│   NSubstitute en lugar de Moq,
│   FluentAssertions en lugar de Xunit asserts,
│   naming convention concreta.
│
├── ITERAR cuando los tests fallan.
│   Si el test que generó NO pasa,
│   analiza por qué (¿bug en el test?, ¿bug en el código?),
│   ajusta y reintenta.
│
└── HACER SUITES COMPLETAS, no tests sueltos.
    Cubre el camino feliz, los errores, los edge cases,
    todo en una sola pasada.
```

> Esto es lo que diferencia
> *"generación de tests"*
> de
> *"tener un colega que escribe tests por ti respetando tus reglas"*.

---

## Slide 12 — Configuración previa: el CLAUDE.md como contrato del equipo

```
Antes de cualquier prompt de generación de tests:
```

```
Asegúrate de que tu CLAUDE.md tiene
un BLOQUE DEDICADO a testing.
```

```
ESTO ES LO QUE MÁS DIFERENCIA
LOS OUTPUTS BUENOS DE LOS MEDIOCRES.
```

> Sin convenciones explícitas,
> Claude Code va a defaultear a "lo que se hace en general en .NET"
>
> — que normalmente es **Moq + asserts de xUnit**.
>
> Si tu equipo usa NSubstitute + FluentAssertions,
> vas a estar corrigiéndolo cada vez.
>
> Mejor decirlo **una vez** en el CLAUDE.md.

---

## Slide 13 — Bloque CLAUDE.md: frameworks y librerías

Algo así en el `CLAUDE.md`:

```markdown
## Testing conventions

### Frameworks y librerías

- **Test framework**: xUnit 
  (no MSTest, no NUnit)
- **Mocking**: NSubstitute
  (no Moq — preferimos NSubstitute por su API
   más limpia y por la polémica reciente de telemetría en Moq)
- **Assertions**: FluentAssertions 
  (no Assert.* de xUnit)
- **Test data**: AutoFixture cuando aplica,
  manual cuando el dato es significativo para el test
```

---

## Slide 14 — Bloque CLAUDE.md: naming convention

```markdown
### Naming convention

Tests llevan el patrón:
MétodoBajoTest_Escenario_ResultadoEsperado

Ejemplos:
- CreateOrder_WhenCustomerExists_ReturnsCreatedOrder
- CreateOrder_WhenCustomerNotFound_ThrowsCustomerNotFoundException
- Validate_WithEmptyEmail_ReturnsValidationError
```

> Esa convención (`Método_Cuando_Resultado`)
> se ha vuelto el estándar de facto en .NET.
>
> Si el equipo usa otra, también vale.
>
> Pero **dilo en el CLAUDE.md**.

---

## Slide 15 — Bloque CLAUDE.md: estructura del test

```csharp
[Fact]
public async Task CreateOrder_WhenCustomerExists_ReturnsCreatedOrder()
{
    // Arrange
    var customerId = 42;
    var customer = new Customer { Id = customerId, Name = "Test" };
    _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>())
        .Returns(customer);
    
    // Act
    var result = await _sut.CreateOrderAsync(
        new CreateOrderRequest { CustomerId = customerId },
        CancellationToken.None);
    
    // Assert
    result.Should().NotBeNull();
    result.CustomerId.Should().Be(customerId);
    result.Status.Should().Be(OrderStatus.Created);
}
```

> Arrange-Act-Assert con comentarios explícitos.
>
> El comentario es para el humano que lee, no para el compilador.
> Pero ayuda muchísimo a la legibilidad.

---

## Slide 16 — Bloque CLAUDE.md: reglas del equipo

```markdown
### Reglas del equipo

- Nunca testear comportamiento que NO esté en el contrato público
- Nunca testear directamente métodos privados 
  (testear vía el público que los usa)
- Mocks SOLO para dependencias externas 
  (servicios, repositorios, HttpClients)
  — para dominio puro, usar instancias reales
- CancellationToken.None en tests 
  salvo que el comportamiento bajo test 
  involucre cancelación
- Una assert PRINCIPAL por test 
  (con FluentAssertions chains permitidos)

### Lo que NO testear

- Implementación interna de frameworks 
  (no testeamos que xUnit ejecute,
   que EF Core haga queries,
   que ASP.NET Core route)
- Getters/setters triviales sin lógica
- Método que es solo un wrapper 
  de otro método ya testeado
```

> Esto NO es opcional.
>
> Es la diferencia entre **5 minutos generando tests con tu pila**
> y **30 minutos corrigiendo tests que vinieron con Moq y `Assert.Equal`**.

---

## Slide 17 — Tests unitarios con xUnit + NSubstitute + FluentAssertions

```
Vamos a la pila moderna.
```

**Tres librerías que se han vuelto el estándar de facto en equipos serios de .NET:**

```
xUnit
└── el test framework MÁS POPULAR en .NET hoy.
    Reemplaza el modelo de [TestInitialize] / [TestCleanup] de MSTest
    por CONSTRUCTORES e IDisposable.
    Más limpio.

NSubstitute
└── librería de MOCKING.
    API más simple que Moq.
    Sintaxis más legible:
    ├── Returns()
    └── Received()
    en vez de It.IsAny<>() y Verify().

FluentAssertions
└── assertions con sintaxis FLUIDA:
    result.Should().Be(...)
    
    Produce mensajes de error
    mucho más claros que Assert.Equal(...).
```

---

## Slide 18 — ¿Por qué este stack y no Moq?

```
Hace unos meses hubo polémica con Moq
porque introdujeron una librería de telemetría
sin avisar adecuadamente.
```

```
Muchos equipos migraron a NSubstitute como respuesta.

Hoy NSubstitute es la opción mainstream
para nuevos proyectos.
```

```
Si tu equipo sigue con Moq:

NO PASA NADA — Claude Code los soporta ambos.

Pero díselo en el CLAUDE.md.
```

---

## Slide 19 — Cómo pedirle tests a Claude Code

El prompt básico:

```
Genera tests unitarios para 
src/OrderManagement.Application/Handlers/CreateOrderHandler.cs.

Cubre:
├── Camino feliz: customer existe, orden se crea correctamente
├── Validaciones: customerId inválido, items vacíos, importe negativo
├── Errores: customer no existe, customer está bloqueado, repositorio falla
└── Edge cases: orden con un único item, orden con muchos items, 
    items duplicados

Sigue las convenciones de testing del CLAUDE.md.
```

---

## Slide 20 — Lo que pasa cuando lo lanzas

```
1. Claude Code lee el CLAUDE.md
   (la primera vez de la sesión).

2. Lee CreateOrderHandler.cs
   para entender el contrato real.

3. Lee los servicios y dependencias
   que el handler usa.

4. Razona sobre los casos a cubrir
   basándose en lo que pediste
   + lo que ve en el código.

5. Genera el fichero de tests
   con su estructura completa.

6. EJECUTA "dotnet test"
   sobre el fichero generado
   para verificar que todo compila y pasa.

7. Si algo falla, ANALIZA, AJUSTA y REINTENTA
   hasta que esté.
```

> Este loop final — generar, ejecutar, ajustar —
> es lo que diferencia *"generación con plantilla"*
> de *"Claude Code haciendo el trabajo en serio"*.
>
> El test que termina llegándote **ya pasa**.

---

## Slide 21 — Lo que funciona bien

Casos donde Claude Code genera tests útiles a la primera o casi:

```
SERVICIOS CON DEPENDENCIAS CLARAS
└── Si tu servicio recibe ICustomerRepository,
    ILogger<T>, IDateTimeProvider,
    Claude los mockea correctamente con NSubstitute.

VALIDATORS
└── La estructura de "input → resultado de validación"
    es muy estándar.
    Claude cubre el feliz path y los inválidos sin fricción.

MAPPERS / TRANSFORMERS
└── Funciones que toman un objeto A
    y devuelven un objeto B.
    Trivial para Claude.

HANDLERS DE MEDIATR / CQRS
└── Patrón muy uniforme, Claude lo conoce bien.
    Dale el handler,
    te genera tests cubriendo el flujo entero.

MÉTODOS CON LÓGICA DE CONTROL DE FLUJO
└── If/else, switch, condiciones
    — Claude identifica los caminos y cubre cada uno.
```

---

## Slide 22 — Lo que requiere atención

Casos donde Claude Code genera tests aceptables pero conviene revisar con cuidado:

```
TESTS CON MUCHOS MOCKS ENCADENADOS
└── Si tu servicio depende de 6-7 cosas,
    los tests pueden volverse difíciles de leer.
    Vale la pena revisar si el problema NO está en el diseño
    (servicios con demasiadas dependencias).

TESTS DE COMPORTAMIENTO ASYNC SUTIL
└── Si el método tiene cancellation tokens,
    paralelismo,
    o manejo cuidadoso de excepciones async,
    los tests pueden cubrir el flujo principal
    pero perderse comportamiento sutil.

TESTS SOBRE COMPORTAMIENTO QUE DEPENDE DEL ESTADO
DE LA APLICACIÓN
└── Si tu método se comporta distinto según
    fase del día, día de la semana, o feature flags activas
    └── Claude puede NO cubrir todas las combinaciones.
```

---

## Slide 23 — El antipatrón estrella: tests que NO testean nada

```
Esto merece sección propia
porque es el RIESGO MÁS REAL
cuando se usa IA para generar tests.
```

**El antipatrón:**

```
Claude genera 30 tests para tu servicio.
TODOS PASAN.
La cobertura SUBE.
```

```
Pero la mitad de los tests
son TAUTOLOGÍAS.
```

> Tests que **siempre pasan porque NO testean comportamiento real**.

---

## Slide 24 — Ejemplos de tests que no testean nada

```csharp
[Fact]
public void Service_WhenInstantiated_IsNotNull()
{
    var service = new MyService(_mockDep);
    service.Should().NotBeNull();  // Esto NO testea nada útil
}

[Fact]
public async Task GetData_WhenRepositoryReturnsData_ReturnsData()
{
    var data = new List<Item> { new() };
    _repository.GetAllAsync().Returns(data);
    
    var result = await _sut.GetData();
    
    result.Should().BeEquivalentTo(data);  
    // Estás verificando que el método 
    // devuelve lo que el mock dijo que devolvería.
    // Eso SIEMPRE pasa.
}
```

```
Suben la cobertura.
Pero NO detectan ningún bug.

Cuando algún día algo se rompa,
estos tests NO te van a salvar.
```

---

## Slide 25 — Cómo evitar que Claude los genere

Tres prácticas que reducen este riesgo:

```
1. PÍDELE TESTS DE COMPORTAMIENTO,
   NO TESTS DE IMPLEMENTACIÓN.
```

En el prompt:

```
"Para cada test, articula explícitamente 
 qué COMPORTAMIENTO se está verificando.
 Si NO puedes describir el comportamiento 
 en una frase clara,
 NO generes el test."
```

---

## Slide 26 — Práctica 2: pídele que justifique cada test

```
2. PÍDELE QUE JUSTIFIQUE CADA TEST.
```

Tras generarlos:

```
"Para cada test que generaste,
 escribe una frase que explique
 QUÉ SE ROMPERÍA si el comportamiento
 bajo test cambiara.
 
 Si la respuesta es 'nada importante',
 ELIMINA el test."
```

> Esta auto-revisión por parte de Claude
> es sorprendentemente efectiva.

---

## Slide 27 — Práctica 3: regla explícita en CLAUDE.md

```
3. METE UNA REGLA EXPLÍCITA EN EL CLAUDE.MD.
```

```markdown
### Tests prohibidos

NO generamos tests que:
├── solo verifiquen que un método devuelve 
│   lo que su mock dijo que devuelve
├── solo verifiquen que un objeto se instancia 
│   sin lanzar excepciones
├── solo verifiquen el tipo de retorno 
│   (esto lo verifica el compilador)
└── tengan más líneas de Arrange 
    que de Act + Assert juntos

Si una clase NO tiene comportamiento testeable,
NO la testeamos.

Documentar esto explícitamente vale más
que tests vacíos.
```

```
Con estas tres prácticas en marcha,
Claude genera tests con ratio señal/ruido
mucho más alto.
```

---

## Slide 28 — Tests de integración con WebApplicationFactory

```
Cambiamos de capa.
```

```
TESTS UNITARIOS
└── viven en aislamiento.
    Mockeamos todo lo externo.

TESTS DE INTEGRACIÓN
└── prueban el sistema FUNCIONANDO,
    normalmente con todo levantado:
    ├── base de datos (en memoria o en Docker)
    ├── middleware activo
    └── autenticación real (con tokens de prueba)
```

```
En ASP.NET Core el patrón estándar es:

WebApplicationFactory<TEntryPoint>

— una clase del propio framework
que te permite levantar tu aplicación entera
en memoria para tests.
```

---

## Slide 29 — Qué resuelve

```
EL TEST UNITARIO
└── te dice si tu servicio funciona en aislamiento.

EL TEST DE INTEGRACIÓN
└── te dice si TODO EL PIPELINE funciona:
    ├── routing
    ├── middleware
    ├── autenticación
    ├── autorización
    ├── deserialización del request
    ├── validación
    ├── ejecución del handler
    ├── serialización del response
    └── manejo de errores.
```

> Cosas que pueden fallar en producción
> aunque cada componente individual funcione bien.

---

## Slide 30 — Estructura básica de un test de integración

```csharp
public class OrdersControllerTests : 
    IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    
    public OrdersControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Reemplazar dependencias reales por test doubles
                // EJ: in-memory database, mocked external services
            });
        });
        _client = _factory.CreateClient();
    }
    
    [Fact]
    public async Task GetOrders_WhenAuthenticated_ReturnsOrders()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", TestTokens.ValidUser);
        
        // Act
        var response = await _client.GetAsync("/api/orders");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var orders = await response.Content
            .ReadFromJsonAsync<List<OrderDto>>();
        orders.Should().NotBeNull();
    }
}
```

---

## Slide 31 — Cómo pedirle a Claude Code una suite de integración

El prompt típico:

```
Genera tests de integración para OrdersController
usando WebApplicationFactory<Program>.

Cubre:
├── GET /api/orders: lista vacía, lista con datos,
│   sin autenticación (401), 
│   con autenticación pero sin permisos (403)
├── POST /api/orders: creación correcta (201), 
│   validación (400),
│   conflicto si la orden ya existe (409)
└── GET /api/orders/{id}: existe (200), no existe (404),
    id inválido (400)

Configuración del fixture:
├── Reemplaza la base de datos real 
│   por una InMemory de EF Core
├── Para autenticación, usa un esquema de test 
│   con tokens predefinidos
│   (TestTokens.ValidUser, TestTokens.NoPermissions)
└── Servicios externos (servicio de email, Stripe)
    deben ser mockeados a través del DI

Sigue las convenciones del CLAUDE.md
y genera el TestFixture en una clase base reutilizable
si tiene sentido.
```

---

## Slide 32 — Lo que Claude Code va a hacer

```
1. Leer tu Program.cs para entender
   qué servicios tiene tu app.

2. Leer OrdersController para entender
   los endpoints y sus contratos.

3. Detectar tu capa de autenticación
   y proponer un esquema de test compatible.

4. Generar la WebApplicationFactory configurada
   o una CLASE BASE
   si la complejidad lo amerita.

5. Generar la suite de tests
   cubriendo los casos pedidos.

6. Ejecutar "dotnet test".
   Iterar si algo falla.
```

---

## Slide 33 — Casos donde brilla / donde necesita más guía

```
DONDE BRILLA
├── APIs REST estándar.
│   CRUD endpoints, validaciones, autenticación.
│   Patrón uniforme, Claude lo cubre bien.
│
├── Endpoints con comportamiento de error claros.
│   400, 401, 403, 404, 409, 422.
│   Claude cubre cada caso.
│
└── Validación con FluentValidation.
    Tests de integración para validaciones son mecánicos.
    Claude los hace bien.
```

```
DONDE NECESITA MÁS GUÍA
├── Endpoints con autenticación compleja.
│   OAuth con providers reales, multi-tenant, scopes.
│   Claude puede generar la base,
│   pero la configuración exacta del test fixture
│   la tienes que ajustar tú.
│
├── Tests que requieren estado de DB precargado.
│   Si tu test asume "hay 5 orders pre-existentes",
│   tienes que decirle cómo se siembra ese estado.
│   Patrón típico: clase TestDataSeeder que Claude usa.
│
└── Tests con interacciones a sistemas externos.
    Webhooks entrantes, polling de servicios externos,
    integraciones con message brokers.
    Aquí entra TESTCONTAINERS (Docker para tests).
```

---

## Slide 34 — Caso práctico guiado: el escenario

En clase, hacemos esto en pantalla.

```
El alumno parte de una API ya hecha
(preparada por el formador):

UN SISTEMA SIMPLE DE GESTIÓN DE PEDIDOS
```

**Componentes:**

```
├── OrdersController con 5 endpoints
│   (GET list, GET by id, POST, PUT, DELETE)
│
├── CreateOrderHandler (MediatR)
│   con validación, lógica de negocio, persistencia
│
├── OrderRepository
│   que abstrae EF Core
│
└── Servicios auxiliares:
    ├── IDateTimeProvider
    ├── IEmailService
    └── IPaymentService
```

> Tres pasos los vamos a hacer:
> ├── tests unitarios del handler
> ├── tests de integración del controller
> └── revisión y ajuste por el alumno

---

## Slide 35 — Paso 1: tests unitarios del handler

```
"Genera tests unitarios para CreateOrderHandler.
Cubre el camino feliz, las validaciones
(3 reglas explícitas en CreateOrderValidator)
y los errores
(customer no existe, payment service rechaza, 
 repositorio falla).

Sigue CLAUDE.md."
```

A los pocos minutos:

```
✓ Fichero CreateOrderHandlerTests.cs
  con 12 tests cubriendo cada caso.

✓ xUnit + NSubstitute + FluentAssertions,
  naming convention correcto.

✓ Mocks configurados con NSubstitute para:
  ├── ICustomerRepository
  ├── IPaymentService
  ├── IOrderRepository
  └── IEmailService

✓ "dotnet test" pasa con verde en los 12.
```

```
REVISIÓN RÁPIDA DEL ALUMNO:

├── Los tests están bien estructurados.
├── Cubren los casos pedidos.
└── Hay un par de cosas que cambiaría
    (renombrar dos tests para mayor claridad),
    pero el output es INMEDIATAMENTE ÚTIL.
```

---

## Slide 36 — Paso 2: tests de integración del controller

```
"Genera tests de integración para OrdersController
usando WebApplicationFactory<Program>.

Configuración:
├── DB en memoria 
│   (Microsoft.EntityFrameworkCore.InMemory)
├── Auth con esquema de test 
│   (un middleware custom que acepta header X-Test-User)
└── Servicios externos (IEmailService, IPaymentService) 
    mockeados vía DI

Cubre los 5 endpoints
con sus casos felices, validaciones y errores."
```

**Output:**

```
✓ OrdersControllerIntegrationTests.cs con ~25 tests.

✓ Una clase base IntegrationTestBase
  que configura la WebApplicationFactory.

✓ Helpers para crear órdenes precargadas
  (SeedDatabaseAsync).

✓ Tests cubriendo todos los códigos HTTP esperados:
  200, 201, 204, 400, 401, 404.
```

---

## Slide 37 — Paso 3: el alumno revisa y ajusta

Aquí está la parte importante:

```
NO SE COMMITEA EL OUTPUT SIN REVISAR.
```

**El alumno:**

```
├── Lee los tests generados.
│
├── Ejecuta el set entero localmente.
│
├── Identifica un caso que Claude NO cubrió:
│   "¿qué pasa si dos requests intentan
│    crear la misma orden simultáneamente?"
│   └── Le pide a Claude que añada tests de concurrencia.
│
├── Revisa que los nombres y la estructura
│   encajen con el resto del proyecto.
│
└── Comitea.
```

```
TIEMPO TOTAL:

30 MINUTOS para una suite de tests
que tradicionalmente serían 4-5 HORAS.
```

> La suite es de calidad razonable
> — NO perfecta, pero el punto de partida
> está mucho más arriba que cero.

---

## Slide 38 — Detección de code smells y refactoring asistido

Conectamos con algo que vimos en el módulo 3: el **subagente reviewer**.

```
Aplicado a .NET, es BRUTAL en detectar
code smells comunes.
```

**Qué detecta bien:**

```
├── async/await mal usado
│   .Result o .Wait() que pueden causar deadlocks.
│   async void que NO debería ser.
│   Falta de ConfigureAwait(false) en código de librería.
│   CancellationToken no propagado.
│
├── Naming inconsistente
│   Convenciones del equipo violadas.
│
├── Manejo de errores débil
│   try/catch genérico que solo loguea y traga la excepción.
│   catch (Exception ex) cuando debería ser específico.
│
├── Inyección de dependencias mal
│   Servicios instanciados con "new" cuando deberían inyectarse.
│   DI scopes incorrectos.
│
├── Duplicación de código
├── Métodos demasiado largos (más de 30-40 líneas)
└── God classes (15+ métodos)
```

---

## Slide 39 — Cómo invocar el reviewer

**Si tienes un subagente `dotnet-reviewer` configurado** (lo vimos en 3.1):

```
"Lanza el subagente dotnet-reviewer
sobre los cambios introducidos en esta sesión.
Devuelve los hallazgos clasificados por severidad."
```

**O sin subagente, en el agente principal:**

```
"Audita el código en src/OrderManagement.Application/Handlers/
buscando:
├── async/await incorrecto
├── manejo de errores débil
├── naming inconsistente con CLAUDE.md
└── duplicación entre handlers

Para cada hallazgo, indica
fichero, línea, descripción del problema,
y sugerencia concreta de fix."
```

---

## Slide 40 — Output típico del reviewer

```markdown
## Hallazgos críticos

### CreateOrderHandler.cs:47
- **Problema**: bloque catch genérico 
  que solo loguea y devuelve null
- **Sugerencia**: capturar excepciones específicas
  (CustomerNotFoundException, PaymentDeclinedException)
  y devolver Result<Order, Error> en su lugar.
  Excepciones inesperadas deberían propagarse,
  no tragarse.

### UpdateOrderHandler.cs:23
- **Problema**: .Result en una llamada async 
  dentro de un método async
- **Sugerencia**: cambiar a 
  `await _repo.GetByIdAsync(id, ct)`.
  .Result puede causar deadlocks 
  con SynchronizationContext.

## Hallazgos importantes
[...resto del informe]
```

**Limitaciones honestas — lo que el reviewer NO detecta bien:**

```
├── Bugs lógicos sutiles
│   (lógica de negocio, edge cases mal manejados).
├── Performance issues NO obvios
│   (queries N+1, problemas con datos reales).
├── Problemas de seguridad sutiles
│   (vulnerabilidades de timing, autorización mal estructurada).
└── Decisiones arquitectónicas
    (¿este servicio debería ser singleton o scoped?).
```

> El reviewer es **complemento** del code review humano,
> NO sustituto.

---

## Slide 41 — Documentación XML y OpenAPI/Swagger asistida

```
Otro caso que rinde mucho.
```

```
La documentación es trabajo TEDIOSO
que casi siempre se POSTERGA.

Claude Code la genera bien
— siempre que le marques los CRITERIOS.
```

**El problema clásico de la documentación generada:**

```csharp
/// <summary>
/// Creates the order.
/// </summary>
/// <param name="request">The request.</param>
/// <returns>The order.</returns>
public async Task<Order> CreateOrderAsync(
    CreateOrderRequest request) { ... }
```

```
Esto es INÚTIL.

Repite el nombre del método con palabras distintas.
NO añade nada que el lector NO supiera ya
leyendo la firma.

Es comentario por kilo, NO por valor.
```

> Y es exactamente lo que Claude Code va a generar
> **si NO le especificas qué quieres**.

---

## Slide 42 — Cómo pedir documentación útil + bridge a 5.3b

Una regla explícita en el CLAUDE.md:

```markdown
### Documentación XML

NO documentamos por kilo.
Solo documentamos cuando AÑADE INFORMACIÓN
que la firma NO comunica.

Por método, los XML docs deben:
├── En <summary>, decir algo MÁS que repetir el nombre.
│   Si NO se puede, OMITIR XML doc completo.
├── En <param>, explicar restricciones o convenciones
│   que el tipo NO comunica
│   (ej: "must be a positive integer", "in UTC").
├── En <returns>, explicar el caso de error.
├── En <exception>, listar todas las excepciones.
└── En <remarks>, explicar comportamiento NO obvio.

Si NO hay nada que añadir respecto a lo que la firma 
ya comunica, NO generar XML doc.
```

> El antes/después y el ejemplo completo de Swagger
> los vemos en la próxima sesión 5.3b
> donde cerramos con estrategia, workflow y CIERRE DEL CURSO.

**Nos vemos en 5.3b.**
