# Plan de Refactorizacion Profesional de Base de Datos Vistas y Stored Procedures

Este plan detalla los pasos requeridos para analizar la estructura actual de los Procedimientos Almacenados (SPs) de visualizacion (lectura), disenar las vistas necesarias para encapsular sus uniones (JOINs) y establecer la ruta de modificacion de los SPs para que consuman estas vistas de manera limpia y eficiente.

---

## 1. Analisis de Estructura de los SPs Actuales

Actualmente, los archivos dentro de la carpeta Database/SP/ agrupan multiples SPs relacionados con la misma entidad (Crear, Editar, Eliminar, Listar, Filtrar).

Los procedimientos de tipo Listar (_List) y Filtrar (_Filter) tienen la siguiente estructura general:
* Hacen consultas directas sobre las tablas fisicas (por ejemplo, FROM [SQM_GENERAL].[Tbl_Products] (NOLOCK)).
* Devuelven unicamente los identificadores numericos (como productProductIdentificatorId, productMarkByProviderId) sin hacer joins para obtener los nombres amigables.
* Requieren abrir y cerrar la clave simetrica (OPEN SYMMETRIC KEY KEY_HASH...) directamente dentro del SP cuando los datos estan cifrados.

Al encapsular las consultas complejas y los joins en vistas (VIEW), los SPs de listado se simplificaran radicalmente (generalmente a un SELECT * FROM VW_Nombre), haciendo el codigo facil de mantener y delegando la responsabilidad de la estructura de datos a la vista.

---

## 2. Propuesta de Diseno de Nuevas Vistas

Crearemos archivos individuales en la carpeta Database/VISTAS/ para encapsular las relaciones logicas de las tablas:

| Vista Propuesta | Tablas Involucradas (Joins) | Campos Clave a Retornar |
| :--- | :--- | :--- |
| **VW_MARKS_BY_PROVIDER** | Tbl_MarkByProviders, Tbl_Marks, Tbl_Providers | markByProviderId, markName, providerName, statusId. |
| **VW_PRODUCTS** | Tbl_Products, VW_PRODUCT_IDENTIFICATORS, VW_MARKS_BY_PROVIDER | productId, productName, productDescription, categoryName, subCategoryName, segmentName, markName, providerName, productStatusId. |
| **VW_PRODUCT_VARIABLES** | Tbl_ProductVariables, VW_PRODUCTS, Tbl_Currencies | productVariableId, productName, productVariableValue, productVariablePrice, currencyISO, productVariableStatusId. |
| **VW_USER_ADDRESSES** | Tbl_UserAddress, Tbl_Users | userAddressId, userName, userFullName, userAddressZIPCode, userAddressDescription, userAddressIsPrincipal, userAddressStatusId. |
| **VW_USER_PAYMENT_METHODS** | Tbl_UserPaymentMethods, Tbl_Users, Tbl_PaymentMethodTypes | userPaymentMethodId, userName, userFullName, paymentMethodTypeName, desencriptacion de tarjeta (CardNumberDecrypted, ExpirationDateDecrypted), userPaymentMethodStatusId. |
| **VW_PAYMENT_ORDERS** | Tbl_PaymentOrders, Tbl_Users, Tbl_UserAddress, Tbl_UserPaymentMethods, Tbl_Currencies, Tbl_Status | orderId, userFullName, userAddressDescription, userPaymentMethodCardHolderName, orderSubtotal, orderTotal, currencyISO, statusName. |
| **VW_PAYMENT_ORDER_DETAILS** | Tbl_PaymentOrderDetails, VW_PRODUCT_VARIABLES, Tbl_Currencies | orderDetailId, orderDetailOrderId, productName, productVariableValue, orderDetailPrice, orderDetailQuantity, orderDetailTotal, currencyISO. |
| **VW_STOCKS** | Tbl_Stocks, VW_PRODUCT_VARIABLES | stockId, productName, productVariableValue, stockQuantity, stockFactoryDate, stockExpirationDate. |

---

## 3. Plan de Modificacion de SPs Existentes (Visualizacion)

Actualizaremos los siguientes archivos de SPs para sustituir las consultas directas a tablas por consultas a las nuevas vistas:

1. 01 SP_Users_Create_05.sql:
   * Modificar sp_Users_List y sp_Users_Filter para consumir una vista limpia de usuarios.
2. 02 SP_UserAddress_Create_05.sql:
   * Modificar sp_UserAddress_List y sp_UserAddress_Filter para consumir VW_USER_ADDRESSES.
3. 03 SP_UserPaymentMethods_Create_05.sql:
   * Modificar sp_UserPaymentMethods_List y sp_UserPaymentMethods_Filter para consumir VW_USER_PAYMENT_METHODS.
4. 04 SP_Products_Create_05.sql:
   * Modificar sp_Products_List y sp_Products_Filter para consumir VW_PRODUCTS.
5. 11 SP_ProductVariables_Create_05.sql:
   * Modificar sp_ProductVariables_List y sp_ProductVariables_Filter para consumir VW_PRODUCT_VARIABLES.
6. 12 SP_PaymentOrders_Create_03.sql:
   * Modificar sp_PaymentOrders_List y sp_PaymentOrders_Filter para consumir VW_PAYMENT_ORDERS.
7. 13 SP_PaymentOrderDetails_Create_03.sql:
   * Modificar sp_PaymentOrderDetails_List para consumir VW_PAYMENT_ORDER_DETAILS.
8. 18 SP_Stocks_Create_03.sql:
   * Modificar sp_Stocks_List para consumir VW_STOCKS.

### Ejemplo del cambio en el SP de Productos:
* **Antes (Codigo original)**:
  ```sql
  CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Products_List]
  AS BEGIN
      SELECT productId, productName, productDescription, productProductIdentificatorId, productMarkByProviderId, productCreatorId, productCreationDate, productStatusId
      FROM [SQM_GENERAL].[Tbl_Products] (NOLOCK);
  END
  ```
* **Despues (Codigo profesional refactorizado)**:
  ```sql
  CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Products_List]
  AS BEGIN
      SELECT productId, productName, productDescription, categoryName, subCategoryName, segmentName, markName, providerName, productStatusId
      FROM [SQM_GENERAL].[VW_PRODUCTS] (NOLOCK);
  END
  ```

---

## 4. Ruta de Ejecucion Paso a Paso

1. **Paso 1: Desarrollo de las Vistas SQL**: Crear y compilar los archivos de vistas uno por uno en la carpeta VISTAS/ respetando las dependencias (por ejemplo, VW_PRODUCTS requiere que exista VW_MARKS_BY_PROVIDER primero).
2. **Paso 2: Refactorizacion de Stored Procedures**: Modificar los archivos de SPs para que invoquen las vistas en lugar de las tablas base.
3. **Paso 3: Validacion**: Correr consultas de prueba utilizando el script de queries para verificar que las salidas de datos coincidan y tengan los nombres de catalogo correctos en lugar de puros IDs.
