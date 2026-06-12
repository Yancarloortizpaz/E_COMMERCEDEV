# Análisis Técnico: Optimización de Stored Procedures de Listado y Filtrado

Este documento presenta el análisis detallado para la optimización y reestructuración de los Stored Procedures (SP) de listar y filtrar en el proyecto. El análisis se enfoca en integrar las vistas existentes para enriquecer las consultas de listado y en aplicar prácticas de alto rendimiento para el filtrado dinámico (evitando el casteo de columnas).

---

## 1. Mapeo de Vistas Existentes a Stored Procedures

Para mejorar el rendimiento y unificar la lógica de negocio, integraremos las vistas creadas en el directorio [VISTAS](file:///C:/hector/E_COMMERCEDEV/Database/VISTAS) dentro de los SPs correspondientes de listado y filtrado.

| Entidad / Carpeta SP | Stored Procedure | Vista a Integrar | Razón de Integración |
| :--- | :--- | :--- | :--- |
| **Productos** | [sp_Products_List](file:///C:/hector/E_COMMERCEDEV/Database/SP/Productos/Listar_Productos.sql)<br>[sp_Products_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Productos/Filtrar_Productos.sql) | [VW_PRODUCTS](file:///C:/hector/E_COMMERCEDEV/Database/VISTAS/VIEW_productos.sql) | Permite retornar de forma directa los nombres de categoría, subcategoría, segmento, marca y proveedor, en lugar de solo IDs raw. |
| **Variables de Producto** | [sp_ProductVariables_List](file:///C:/hector/E_COMMERCEDEV/Database/SP/Variables_Producto/Listar_Variables_Producto.sql)<br>[sp_ProductVariables_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Variables_Producto/Filtrar_Variables_Producto.sql) | [VW_PRODUCT_VARIABLES](file:///C:/hector/E_COMMERCEDEV/Database/VISTAS/VIEW_variables_producto.sql) | Agrega detalles del producto, categorías, marcas, proveedores y el ISO de la moneda en la respuesta del listado/filtro. |
| **Inventario Stocks** | [sp_Stocks_List](file:///C:/hector/E_COMMERCEDEV/Database/SP/Inventario_Stocks/Listar_Stocks.sql) | [VW_STOCKS](file:///C:/hector/E_COMMERCEDEV/Database/VISTAS/VIEW_inventario.sql) | Enriquece el stock con el nombre del producto, el valor de la variable, el precio unitario y el ISO de la moneda. |
| **Direcciones** | [sp_UserAddress_List](file:///C:/hector/E_COMMERCEDEV/Database/SP/Direcciones_Usuario/Listar_Direcciones_Usuario.sql)<br>[sp_UserAddress_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Direcciones_Usuario/Filtrar_Direcciones_Usuario.sql) | [VW_USER_ADDRESSES](file:///C:/hector/E_COMMERCEDEV/Database/VISTAS/VIEW_direcciones_usuario.sql) | Incorpora los datos del usuario (Nombre completo, username, email) asociados a la dirección directamente en la consulta. |
| **Métodos de Pago** | [sp_UserPaymentMethods_List](file:///C:/hector/E_COMMERCEDEV/Database/SP/Metodos_Pago_Usuario/Listar_Metodos_Pago_Usuario.sql)<br>[sp_UserPaymentMethods_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Metodos_Pago_Usuario/Filtrar_Metodos_Pago_Usuario.sql) | [VW_USER_PAYMENT_METHODS](file:///C:/hector/E_COMMERCEDEV/Database/VISTAS/VIEW_metodos_pago_usuario.sql) | Centraliza la desencriptación de tarjetas/CVV. Nota: Sigue requiriendo la apertura/cierre de la llave simétrica `KEY_HASH` en el SP. |
| **Órdenes de Pago** | [sp_PaymentOrders_List](file:///C:/hector/E_COMMERCEDEV/Database/SP/Ordenes_Pago/Listar_Ordenes_Pago.sql)<br>[sp_PaymentOrders_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Ordenes_Pago/Filtrar_Ordenes_Pago.sql) | [VW_PAYMENT_ORDERS](file:///C:/hector/E_COMMERCEDEV/Database/VISTAS/VIEW_ordenes_pago.sql) | Une la orden con datos legibles del usuario, dirección física, tarjetahabiente, moneda ISO y el estado legible. |
| **Detalles Orden** | [sp_PaymentOrderDetails_List](file:///C:/hector/E_COMMERCEDEV/Database/SP/Detalles_Orden_Pago/Listar_Detalles_Orden_Pago.sql)<br>[sp_PaymentOrderDetails_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Detalles_Orden_Pago/Filtrar_Detalles_Orden_Pago.sql) | [VW_PAYMENT_ORDER_DETAILS](file:///C:/hector/E_COMMERCEDEV/Database/VISTAS/VIEW_detalles_orden_pago.sql) | Expone nombre del producto, categorías, subcategoría, marca, valor de variable y moneda ISO. |

> [!NOTE]
> La mayoría de las vistas tienen una condición de filtrado por defecto de registros activos (`statusId = 1`). Esto es ideal para listados de consumo del cliente final. Las órdenes de pago (`VW_PAYMENT_ORDERS`), por el contrario, no filtran por estado activo ya que deben poder ser visualizadas bajo cualquier estado del flujo (pendiente, pagado, cancelado).

---

## 2. Optimización de Rendimiento en Filtros Dinámicos (SARGabilidad)

El filtrado dinámico mediante un parámetro de búsqueda general `@SearchTerm` suele implementarse de manera ineficiente en SQL. A continuación, analizamos las implicaciones y cómo solucionarlo.

### Mala Práctica ❌
```sql
WHERE CAST(productId AS VARCHAR) = @SearchTerm
   OR productName LIKE '%' + @SearchTerm + '%'
```
* **Consecuencia:** SQL Server se ve obligado a convertir el valor de `productId` para cada registro de la tabla (`Table/Index Scan`). No se puede usar el índice clúster/primario de `productId` porque la columna está envuelta en una función (`CAST`). El consumo de CPU y tiempo de respuesta escala exponencialmente con el tamaño de los datos.

### Buena Práctica (SARGable) ✔️
```sql
-- 1. Realizar el casteo solo una vez a nivel de parámetro/variable interna
DECLARE @SearchId INT = TRY_CAST(@SearchTerm AS INT);

-- 2. Comparación directa sin aplicar funciones sobre las columnas de la tabla
WHERE (@SearchTerm IS NULL)
   OR (productId = @SearchId)
   OR (productName LIKE '%' + @SearchTerm + '%')
```
* **Ventaja:**
  * Si `@SearchTerm` no es numérico (por ejemplo, `'Camisa'`), `@SearchId` será `NULL`. La expresión `productId = NULL` no produce resultados de manera instantánea sin realizar conversiones inválidas ni generar excepciones.
  * Si `@SearchTerm` es numérico (por ejemplo, `'42'`), `@SearchId` será `42`. La consulta evalúa `productId = 42`, permitiendo a SQL Server ejecutar un **Index Seek** en el índice primario de la tabla, resolviendo la consulta en milisegundos.

---

## 3. Plan de Ajustes por Categoría de SP

Para aplicar estas mejoras en los SP de listado y filtrado, procederemos con el siguiente diseño lógico en cada directorio:

### A. Módulo de Seguridad (Usuarios)
* **Filtrar:** [sp_Users_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Usuarios/Filtrar_Usuarios.sql)
  * **Cambios:** Añadir variable `@SearchId INT = TRY_CAST(@SearchTerm AS INT)`.
  * **Filtros SARGables:** Buscar por `userId = @SearchId`, `userFullName LIKE ...`, `userName LIKE ...`, `userEmail LIKE ...`, `userPhoneNumber LIKE ...`.

### B. Módulos de Catálogos (Categorías, Subcategorías, Marcas, Monedas, Segmentos, Proveedores)
Estos catálogos no cuentan con vistas complejas unidas (`JOIN`), por lo que seguirán consultando directamente a sus tablas, pero sus filtros dinámicos se optimizarán:
* **SP de Filtrado Involucrados:**
  * [sp_Categories_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Categorias/Filtrar_Categorias.sql)
  * [sp_SubCategories_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Subcategorias/Filtrar_Subcategorias.sql)
  * [sp_Marks_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Marcas/Filtrar_Marcas.sql)
  * [sp_Currencies_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Monedas/Filtrar_Monedas.sql)
  * [sp_Segments_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Segmentos/Filtrar_Segmentos.sql)
  * [sp_Providers_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Proveedores/Filtrar_Proveedores.sql)
* **Cambios:** Extraer el ID correspondiente usando `TRY_CAST(@SearchTerm AS INT)` para realizar comparaciones numéricas directas en la clave primaria (`categoryId`, `subCategoryId`, etc.) y mantener búsquedas de texto con `LIKE` de manera paralela.

### C. Módulo de Direcciones de Usuario
* **Listar:** [sp_UserAddress_List](file:///C:/hector/E_COMMERCEDEV/Database/SP/Direcciones_Usuario/Listar_Direcciones_Usuario.sql)
  * **Cambios:** Reemplazar consulta de `Tbl_UserAddress` por `[SQM_GENERAL].[VW_USER_ADDRESSES]`.
* **Filtrar:** [sp_UserAddress_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Direcciones_Usuario/Filtrar_Direcciones_Usuario.sql)
  * **Cambios:** Cambiar la consulta base a `[SQM_GENERAL].[VW_USER_ADDRESSES]` y mapear los filtros.

### D. Módulo de Métodos de Pago
* **Listar y Filtrar:** [sp_UserPaymentMethods_List](file:///C:/hector/E_COMMERCEDEV/Database/SP/Metodos_Pago_Usuario/Listar_Metodos_Pago_Usuario.sql) y [sp_UserPaymentMethods_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Metodos_Pago_Usuario/Filtrar_Metodos_Pago_Usuario.sql)
  * **Cambios:** Consumir `[SQM_GENERAL].[VW_USER_PAYMENT_METHODS]`. El SP abrirá la llave simétrica (`OPEN SYMMETRIC KEY`) para que la desencriptación definida dentro de la vista se ejecute exitosamente.

### E. Módulo de Productos y Variables
* **Productos (Listar y Filtrar):** [sp_Products_List](file:///C:/hector/E_COMMERCEDEV/Database/SP/Productos/Listar_Productos.sql) y [sp_Products_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Productos/Filtrar_Productos.sql)
  * **Cambios:** Reemplazar consultas base por la vista `[SQM_GENERAL].[VW_PRODUCTS]`. En el filtro, usar `TRY_CAST` para habilitar búsquedas seguras por `productId`, `categoryId`, etc.
* **Variables (Listar y Filtrar):** [sp_ProductVariables_List](file:///C:/hector/E_COMMERCEDEV/Database/SP/Variables_Producto/Listar_Variables_Producto.sql) and [sp_ProductVariables_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Variables_Producto/Filtrar_Variables_Producto.sql)
  * **Cambios:** Usar la vista `[SQM_GENERAL].[VW_PRODUCT_VARIABLES]`.

### F. Módulo de Órdenes y Detalles de Pago
* **Órdenes (Listar y Filtrar):** [sp_PaymentOrders_List](file:///C:/hector/E_COMMERCEDEV/Database/SP/Ordenes_Pago/Listar_Ordenes_Pago.sql) y [sp_PaymentOrders_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Ordenes_Pago/Filtrar_Ordenes_Pago.sql)
  * **Cambios:** Integrar la vista `[SQM_GENERAL].[VW_PAYMENT_ORDERS]`.
* **Detalles (Listar y Filtrar):** [sp_PaymentOrderDetails_List](file:///C:/hector/E_COMMERCEDEV/Database/SP/Detalles_Orden_Pago/Listar_Detalles_Orden_Pago.sql) y [sp_PaymentOrderDetails_Filter](file:///C:/hector/E_COMMERCEDEV/Database/SP/Detalles_Orden_Pago/Filtrar_Detalles_Orden_Pago.sql)
  * **Cambios:** Integrar la vista `[SQM_GENERAL].[VW_PAYMENT_ORDER_DETAILS]`.

---

## 4. Estructura de Consulta Sugerida con `OPTION (RECOMPILE)`

Para prevenir problemas de **Parameter Sniffing** en consultas con múltiples condiciones dinámicas opcionales, aplicaremos la sugerencia de compilación al final de los SP de filtrado dinámico:

```sql
SELECT ...
FROM [Vista_O_Tabla] (NOLOCK)
WHERE (@SearchTerm IS NULL)
   OR (IdColumna = @SearchId)
   OR (NombreColumna LIKE '%' + @SearchTerm + '%')
OPTION (RECOMPILE);
```
Esta directiva fuerza a SQL Server a generar un plan de ejecución optimizado adaptado exactamente a si `@SearchTerm` vino con un valor numérico, un valor de texto o como `NULL`.
