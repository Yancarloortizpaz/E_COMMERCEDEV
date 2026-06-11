b# Plan de Robustecimiento y Validación de Stored Procedures (SPs)

Este documento detalla el análisis y plan de trabajo para el rediseño y robustecimiento de los **83 Stored Procedures** del ecosistema `DB_ECOMMERCE` en la carpeta [SP](file:///C:/Users/gmayc/OneDrive/Escritorio/Hector/E_COMMERCEDEV/Database/SP). El objetivo principal es incorporar un framework de validaciones preventivas contra errores en producción y unificar la lógica de respuestas de salida utilizando parámetros `OUTPUT`.

---

## 1. Arquitectura de Salida (Output standard)

Para unificar la comunicación entre el motor de base de datos y la capa de aplicación (Backend/Chatbot), todos los Stored Procedures transaccionales (Insertar, Actualizar, Eliminar) implementarán el siguiente estándar de parámetros de salida:

```sql
@o_code INT = NULL OUTPUT,
@o_message VARCHAR(255) = NULL OUTPUT,
@o_templateId INT = NULL OUTPUT
```

### Códigos de Estado (`@o_code`):
*   **`200`**: Operación exitosa.
*   **`-1`**: Error de validación de negocio (datos faltantes, registros inactivos, violación de llaves lógicas).
*   **`Número de Error SQL (ej. 2627)`**: Excepciones capturadas en el bloque `CATCH` (violaciones de FK físicas, desbordamientos de datos, etc.).

---

## 2. Vectores de Error Comunes en Producción a Prevenir

1.  **Referencias Huérfanas (FK Inexistentes)**: Intentar asociar registros a IDs de usuarios, categorías o monedas inexistentes.
2.  **Uso de Entidades Inactivas**: Asociar registros a catálogos desactivados (ej. crear un producto con una categoría inactiva).
3.  **Duplicados Lógicos**: Registrar nombres de categorías duplicadas, correos de usuario duplicados, o múltiples carritos activos para un mismo cliente.
4.  **Actualización de Registros Eliminados**: Intentar modificar datos personales o transacciones que se encuentran en un estado inactivo/eliminado (resuelto mediante el parámetro `@ForzarRecuperacion`).
5.  **Cantidades y Valores Negativos**: Intentar registrar stocks negativos o precios en cero.
6.  **Errores de Transaccionalidad**: Operaciones interrumpidas a mitad de camino que dejan la base de datos en estado inconsistente (resuelto mediante `BEGIN TRANSACTION`, `COMMIT` y `ROLLBACK`).

---

## 3. Plantillas Estándar de Implementación

### A. Plantilla para Inserciones (`INSERT`)
```sql
CREATE OR ALTER PROCEDURE [Schema].[sp_Entity_Create]
(
    @param1 TYPE,
    @param2 TYPE,
    @paramCreatorId INT,
    @paramStatusId BIT,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- 1. Validaciones de nulos u obligatorios
    IF @param1 IS NULL OR LTRIM(RTRIM(@param1)) = ''
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El parámetro param1 es obligatorio.';
        RETURN;
    END;

    -- 2. Validaciones de existencia de relaciones (FK activas)
    IF NOT EXISTS (SELECT 1 FROM [SQM_SECURITY].[Tbl_Users] WHERE userId = @paramCreatorId AND userStatusId = 1)
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El creador no existe o está inactivo.';
        RETURN;
    END;

    -- 3. Validaciones de unicidad lógica
    IF EXISTS (SELECT 1 FROM [Schema].[Tbl_Entity] WHERE uniqueField = @param1 AND statusId = 1)
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El registro ya existe en el sistema.';
        RETURN;
    END;

    -- 4. Inserción protegida por transaccionalidad
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO [Schema].[Tbl_Entity] (field1, field2, creatorId, creationDate, statusId)
        VALUES (@param1, @param2, @paramCreatorId, GETDATE(), @paramStatusId);

        COMMIT TRANSACTION;

        SET @o_code = 200;
        SET @o_message = 'Registro insertado exitosamente.';
        SET @o_templateId = SCOPE_IDENTITY(); -- Retorna el ID generado
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER();
        SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
```

### B. Plantilla para Actualizaciones (`UPDATE`)
```sql
CREATE OR ALTER PROCEDURE [Schema].[sp_Entity_Update]
(
    @id INT,
    @param1 TYPE = NULL,
    @param2 TYPE = NULL,
    @paramModificatorId INT,
    @paramStatusId BIT = NULL,
    @ForzarRecuperacion BIT = 0,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- 1. Validar existencia del registro
    IF NOT EXISTS (SELECT 1 FROM [Schema].[Tbl_Entity] WHERE id = @id)
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El registro no existe.';
        RETURN;
    END;

    -- 2. Validar modificador activo
    IF NOT EXISTS (SELECT 1 FROM [SQM_SECURITY].[Tbl_Users] WHERE userId = @paramModificatorId AND userStatusId = 1)
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El modificador no existe o está inactivo.';
        RETURN;
    END;

    -- 3. Validar estado del registro (si está inactivo y no se fuerza recuperación)
    IF @ForzarRecuperacion = 0 AND EXISTS (SELECT 1 FROM [Schema].[Tbl_Entity] WHERE id = @id AND statusId = 0)
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El registro está inactivo/eliminado. Active la recuperación si es requerido.';
        RETURN;
    END;

    -- 4. Ejecución de actualización dinámica
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE [Schema].[Tbl_Entity]
        SET field1 = COALESCE(@param1, field1),
            field2 = COALESCE(@param2, field2),
            modificatorId = @paramModificatorId,
            modificationDate = GETDATE(),
            statusId = COALESCE(@paramStatusId, statusId)
        WHERE id = @id;

        COMMIT TRANSACTION;

        SET @o_code = 200;
        SET @o_message = 'Registro actualizado exitosamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER();
        SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
```

### C. Plantilla para Eliminaciones Lógicas (`DELETE`)
> [!IMPORTANT]
> Siguiendo la regla de negocio, los SP de "eliminación" no realizarán borrados físicos (`DELETE`) sino lógicos (actualización de estado a Inactivo/0), protegiendo la integridad referencial histórica.

```sql
CREATE OR ALTER PROCEDURE [Schema].[sp_Entity_Delete]
(
    @id INT,
    @paramModificatorId INT,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- 1. Validar existencia y que no esté previamente inactivo
    IF NOT EXISTS (SELECT 1 FROM [Schema].[Tbl_Entity] WHERE id = @id)
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El registro no existe.';
        RETURN;
    END;

    IF EXISTS (SELECT 1 FROM [Schema].[Tbl_Entity] WHERE id = @id AND statusId = 0)
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El registro ya se encuentra inactivo.';
        RETURN;
    END;

    -- 2. Validar modificador activo
    IF NOT EXISTS (SELECT 1 FROM [SQM_SECURITY].[Tbl_Users] WHERE userId = @paramModificatorId AND userStatusId = 1)
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El modificador no existe o está inactivo.';
        RETURN;
    END;

    -- 3. Inactivación
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE [Schema].[Tbl_Entity]
        SET statusId = 0,
            modificatorId = @paramModificatorId,
            modificationDate = GETDATE()
        WHERE id = @id;

        COMMIT TRANSACTION;

        SET @o_code = 200;
        SET @o_message = 'Registro inactivado correctamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER();
        SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
```

---

## 4. Clasificación y Análisis de los 19 Grupos de SPs (83 archivos)

A continuación se presenta el mapeo de carpetas, archivos y las reglas de validación específicas para cada tipo de operación:

| Carpeta / Módulo | Tabla Principal | Tipo SPs | Validaciones Específicas a Implementar |
| :--- | :--- | :--- | :--- |
| **1. Carritos** | `Tbl_Carts` | Insert, Update, Delete, List | Verificar usuario activo. Unicidad: un solo carrito activo (`cartStatusId = 1`) por usuario. Cambiar Delete físico a Delete lógico. |
| **2. Detalles_Carrito** | `Tbl_CartDetails` | Insert, Delete, List | Verificar carrito y variante de producto activos. Validar stock disponible. Calcular subtotales e impuestos en servidor si es necesario. |
| **3. Detalles_Movimiento_Inventario** | `Tbl_StockMovementDetails` | Insert, List, Filter | Validar movimiento de stock y lote/stock activos. Cantidades mayor a cero. |
| **4. Detalles_Orden_Pago** | `Tbl_PaymentOrderDetails` | Insert, List, Filter | Validar orden y variante de producto activos. Precios consistentes. |
| **5. Direcciones_Usuario** | `Tbl_UserAddress` | Insert, Update, Delete, List, Filter | Validar usuario activo. Si se define como principal (`userAddressIsPrincipal = 1`), actualizar las demás direcciones del usuario a `0`. |
| **6. Inventario_Stocks** | `Tbl_Stocks` | Insert, Update, List | Validar variante de producto activa. Cantidad >= 0. Expiración > Fabricación. |
| **7. Marcas** | `Tbl_Marks` | Insert, Update, Delete, List, Filter | Nombre de marca único y no vacío. |
| **8. Metodos_Pago_Usuario** | `Tbl_UserPaymentMethods` | Insert, Update, Delete, List, Filter | Validar usuario activo. Cifrado obligatorio de número, expiración y CVV usando la llave simétrica `KEY_HASH`. |
| **9. Monedas** | `Tbl_Currencies` | Insert, Update, Delete, List, Filter | Nombre, ISO (ej. USD, NIO) y Código numérico únicos. ISO exactamente de 3 o 5 caracteres según definición. |
| **10. Movimientos_Inventario** | `Tbl_StockMovements` | Insert, List, Filter | Validar tipo de movimiento y orden de pago existentes. |
| **11. Ordenes_Pago** | `Tbl_PaymentOrders` | Insert, List, Filter | Validar usuario, dirección y método de pago activos. Consistencia matemática de totales. |
| **12. Productos** | `Tbl_Products` | Insert, Update, Delete, List, Filter | Nombre único. Validar identificador de producto y marca de proveedor activos. |
| **13. Proveedores** | `Tbl_Providers` | Insert, Update, Delete, List, Filter | Nombre único y no vacío. |
| **14. Segmentos** | `Tbl_Segments` | Insert, Update, Delete, List, Filter | Nombre único y no vacío. |
| **15. Subcategorias** | `Tbl_SubCategories` | Insert, Update, Delete, List, Filter | Nombre único y no vacío. |
| **16. Tipos_Metodo_Pago** | `Tbl_PaymentMethodTypes` | Insert, Update, Delete, List, Filter | Nombre único y no vacío. |
| **17. Usuarios** | `Tbl_Users` | Insert, Update, Delete, List, Filter | Validar edad (> 12 años, año > 1925). Nombre, usuario y correo únicos. Formato de correo válido. Cifrado de contraseña en inserción. |
| **18. Variables_Producto** | `Tbl_ProductVariables` | Insert, Update, Delete, List, Filter | Validar producto y moneda activos. Precio > 0. |
| **19. Categorias** | `Tbl_Categories` | Insert, Update, Delete, List, Filter | Nombre único y no vacío. |

---

## 5. Caso de Prueba Inicial: `Insertar_Carrito.sql`

Para iniciar la fase de refactorización (una vez aprobado este plan), se ha diseñado la lógica para [Insertar_Carrito.sql](file:///C:/Users/gmayc/OneDrive/Escritorio/Hector/E_COMMERCEDEV/Database/SP/Carritos/Insertar_Carrito.sql).

### Código Actual:
```sql
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Carts_Create]
    @cartUserId INT, @cartCreatorId INT, @cartStatusId BIT
AS BEGIN
    INSERT INTO [SQM_GENERAL].[Tbl_Carts] 
    (cartUserId, cartCreatorId, cartCreationDate, cartStatusId)
    VALUES 
    (@cartUserId, @cartCreatorId, GETDATE(), @cartStatusId);
END
```

### Código Propuesto con Robustecimiento:
```sql
USE [DB_ECOMMERCE]
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Carts_Create]
(
    @cartUserId INT,
    @cartCreatorId INT,
    @cartStatusId BIT,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- 1. Validaciones Preliminares de Nulidad
    IF @cartUserId IS NULL OR @cartUserId <= 0
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El ID de usuario es obligatorio.';
        RETURN;
    END;

    IF @cartCreatorId IS NULL OR @cartCreatorId <= 0
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El ID del creador es obligatorio.';
        RETURN;
    END;

    -- 2. Validar existencia del usuario y creador en Tbl_Users (con estado Activo/1)
    IF NOT EXISTS (SELECT 1 FROM [SQM_SECURITY].[Tbl_Users] WHERE userId = @cartUserId AND userStatusId = 1)
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El usuario no existe o se encuentra inactivo/bloqueado.';
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM [SQM_SECURITY].[Tbl_Users] WHERE userId = @cartCreatorId AND userStatusId = 1)
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El usuario creador no existe o se encuentra inactivo/bloqueado.';
        RETURN;
    END;

    -- 3. Regla de Negocio: Un usuario solo puede tener UN carrito activo
    DECLARE @ExistingCartId INT;
    SELECT @ExistingCartId = cartId 
    FROM [SQM_GENERAL].[Tbl_Carts] 
    WHERE cartUserId = @cartUserId AND cartStatusId = 1;

    IF @ExistingCartId IS NOT NULL
    BEGIN
        SET @o_code = 200;
        SET @o_message = 'El usuario ya cuenta con un carrito activo.';
        SET @o_templateId = @ExistingCartId; -- Devolvemos el ID del carrito existente
        RETURN;
    END;

    -- 4. Bloque transaccional seguro
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO [SQM_GENERAL].[Tbl_Carts] 
        (
            cartUserId, 
            cartCreatorId, 
            cartCreationDate, 
            cartStatusId
        )
        VALUES 
        (
            @cartUserId, 
            @cartCreatorId, 
            GETDATE(), 
            @cartStatusId
        );

        COMMIT TRANSACTION;

        SET @o_code = 200;
        SET @o_message = 'Carrito creado correctamente.';
        SET @o_templateId = SCOPE_IDENTITY(); -- Retorna el ID del nuevo carrito
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

        SET @o_code = ERROR_NUMBER();
        SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO
```

---

## 6. Lista de Tareas y Siguientes Pasos

- [ ] **Paso 1**: Revisión y Aprobación del presente plan de análisis.
- [ ] **Paso 2**: Modificación y prueba del SP piloto: `Carritos/Insertar_Carrito.sql`.
- [ ] **Paso 3**: Refactorización secuencial de los SP de escritura (`Insertar`, `Actualizar`, `Eliminar`) dividida en fases:
    - [ ] Fase 3.1: Módulo de Usuarios, Direcciones y Métodos de Pago.
    - [ ] Fase 3.2: Módulo de Catálogos (Categorías, Subcategorías, Marcas, Proveedores, Segmentos).
    - [ ] Fase 3.3: Módulo de Productos y Variantes.
    - [ ] Fase 3.4: Módulo transaccional de Carritos y Detalles.
    - [ ] Fase 3.5: Módulo final de Facturación e Inventario (Órdenes de Pago y Movimientos).
- [ ] **Paso 4**: Integración de vistas y optimizaciones en los SP de listado y filtros (`Listar` y `Filtrar`).
