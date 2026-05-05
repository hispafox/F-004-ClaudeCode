---
name: db-reset
description: Resetea la base de datos local de OrderManagement borrando todos los pedidos y clientes y dejando solo los datos seed (Acme y Globex). Operación destructiva — solo se invoca explícitamente con /db-reset, nunca por activación automática.
disable-model-invocation: true
---

# db-reset

Skill destructivo para resetear la base de datos local de OrderManagement.

## ⚠️ Aviso

Este skill **borra datos**. Tiene `disable-model-invocation: true` para que
nunca se active por inferencia del modelo — solo se ejecuta cuando el usuario
escribe explícitamente `/db-reset`.

NO usar este skill en entornos compartidos. Está pensado para la base local
del dev (EF Core In-Memory por defecto, o SQLite/SQL Server si el dev lo
ha configurado).

## Cuándo se usa

Solo cuando el usuario escribe `/db-reset` en la sesión. Casos típicos:

- Tras una sesión larga de pruebas, devolver la BBDD al estado seed.
- Antes de grabar una demo, asegurar el mismo punto de partida.
- Cuando los datos se han corrompido por un cambio de modelo / migración fallida.

## Pasos al ejecutar

1. **Confirmar al usuario** que está pidiendo borrar datos. Esperar respuesta
   afirmativa explícita antes de continuar.

2. **Posicionarse en la API**:

   ```bash
   cd src/OrderManagement.Api
   ```

3. **Drop de la base** (forzado, sin pedir confirmación interactiva):

   ```bash
   dotnet ef database drop --force --no-build
   ```

4. **Recrear la base con seed data**:

   ```bash
   dotnet ef database update --no-build
   ```

   En el modelo InMemory por defecto, el seed (Acme y Globex en `Customers`)
   se aplica al arrancar la API vía `EnsureCreated()` en `Program.cs`.

5. **Confirmar al usuario** que la BBDD está en estado limpio. Mostrar el
   conteo: `Orders: 0`, `Customers: 2 (Acme, Globex)`.

## Lo que NO debe hacer

- NO ejecutarse sin la confirmación explícita del usuario.
- NO ejecutarse contra entornos no locales. Si detecta que la connection
  string apunta a un servidor compartido o producción, abortar.
- NO modificar migraciones, código ni configuración. Solo opera sobre datos.
- NO commitear nada en git tras el reset.

## Por qué disable-model-invocation

Este skill cae en las tres categorías que la gamma 2.2c marcó como
candidatas a `disable-model-invocation`:

- **Destructivo**: borra datos.
- **Irreversible**: salvo backup previo, los pedidos se pierden.
- **Sensible al contexto**: la palabra "reset" puede aparecer en peticiones
  legítimas que no implican borrar la BBDD ("reset el form", "reset el
  contador"...).

Por eso solo se activa con `/db-reset` explícito.
