 Análisis de Arquitectura y Estructura de Base de Datos  ECommerce y Agente Inteligente

Este documento detalla la investigación, el diseño, la lógica funcional y la estructura relacional del directorio de base de datos (`DB`) del proyecto E_COMMERCEDEV.



 1. Estructura del Directorio de Base de Datos

La organización física de los archivos SQL dentro de la carpeta [DB](file:///E_COMMERCEDEV/DB) está diseñada para separar las responsabilidades de configuración, datos de prueba, lógica de funciones y el agente inteligente:

 [CONFIGURATIONS](file:///E_COMMERCEDEV/DB/CONFIGURATIONS): Contiene la definición de esquemas personalizados y la infraestructura criptográfica de llaves simétricas.
 [DATA](file:///E_COMMERCEDEV/DB/DATA): Aloja los datos de catálogo iniciales y registros de usuarios de prueba cifrados.
 [DB_EcommerceAgent](file:///E_COMMERCEDEV/DB/DB_EcommerceAgent): Contiene la definición de base de datos y flujos para el chatbot del sistema experto.
 [FUNCTIONS/SCALAR](file:///E_COMMERCEDEV/DB/FUNCTIONS/SCALAR): Almacena las funciones escalares reutilizables para encriptar y desencriptar información.
 [SCRIPTS](file:///E_COMMERCEDEV/DB/SCRIPTS): Consultas SQL de prueba para verificar relaciones e información desencriptada en tiempo de ejecución.



 2. Primera Base de Datos: `DB_ECOMMERCE` (ECommerce Transaccional)

El propósito de esta base de datos es gestionar todo el catálogo de productos, variantes, usuarios, seguridad, carritos de compra, pasarela de órdenes y control de inventarios. Está organizada mediante tres esquemas de base de datos para separar responsabilidades lógicas:

 A. Esquema `SQM_CATALOGS` (Tablas Maestras y Catálogos de Referencia)
Este esquema agrupa tablas paramétricas que alimentan la lógica de negocio y minimizan la redundancia de datos:
 `Tbl_Status`: Estados generales aplicables a múltiples entidades (ej. `ACTIVO`, `INACTIVO`, `PROCESADO`, `ENTREGADO`).
 `Tbl_Categories`, `Tbl_SubCategories` y `Tbl_Segments`: Estructuran la jerarquía de navegación y clasificación de productos (ej. Ropa > Masculino > Deportivo).
 `Tbl_ProductIdentificators`: Tabla pivote que combina las tres tablas anteriores para crear combinaciones únicas de clasificación de productos.
 `Tbl_Providers` y `Tbl_Marks`: Administran proveedores y marcas de productos.
 `Tbl_MarkByProviders`: Relaciona qué marcas provee cada proveedor.
 `Tbl_AttributesTypes` y `Tbl_AttributeProducts`: Define tipos de atributos (ej. Color, Talla, Material) y los atributos específicos de los productos.
 `Tbl_Currencies`: Soporte multidivisa (ej. Córdoba, Dólar, Euro) guardando códigos ISO estándar.
 `Tbl_PaymentMethodTypes`: Métodos de pago permitidos (ej. Tarjeta de Crédito, Débito, Transferencia).
 `Tbl_StockMovementTypes`: Tipos de flujos de inventario (ej. Entrada por compra, Salida por venta, Devolución).

 B. Esquema `SQM_SECURITY` (Seguridad y Autenticación de Usuarios)
 `Tbl_Users`: Almacena información crítica del usuario. Para máxima seguridad, el campo `userPassword` se almacena como `VARBINARY(256)` al estar cifrado mediante algoritmos criptográficos del servidor.

 C. Esquema `SQM_GENERAL` (Entidades Transaccionales e Inventario)
Contiene las tablas que sufren cambios frecuentes debido a la operación diaria de la tienda:
 `Tbl_UserAddress`: Direcciones físicas de los usuarios, indicando cuál es la dirección principal para envíos.
 `Tbl_Products`: Catálogo base de artículos ofertados.
 `Tbl_ProductImages`: Enlaces a imágenes del producto, con bandera para definir la imagen de portada principal.
 `Tbl_ProductVariables`: Tabla sumamente importante. En lugar de vender el producto general directamente, se venden sus variantes específicas. Cada variante tiene su propio precio (`productVariablePrice`) y moneda asociada.
 `Tbl_AttributeProductVariables`: Atributos específicos asignados a la variante de producto (ej. variante 1 tiene "Talla M").
 `Tbl_Stocks`: Registra la cantidad física disponible en almacén para cada variante de producto, incluyendo fechas críticas de fabricación (`stockFactoryDate`) y expiración (`stockExpirationDate`).
 `Tbl_Carts` y `Tbl_CartDetails`: Administran los artículos agregados temporalmente al carrito de compras antes de la transacción final.
 `Tbl_UserPaymentMethods`: Almacena información confidencial de tarjetas de pago. Por seguridad, el número de tarjeta (`userPaymentMethodCardNumber`), la fecha de expiración (`userPaymentMethodExpirationDate`) y el código de seguridad CVV (`userPaymentMethodCVV`) se guardan cifrados en formato `VARBINARY(256)`.
 `Tbl_PaymentOrders` y `Tbl_PaymentOrderDetails`: Registran las ventas consolidadas de la tienda, incluyendo costos de envío, impuestos, subtotales, totales y moneda de pago.
 `Tbl_StockMovements` y `Tbl_StockMovementDetails`: Implementan el sistema de Kardex o bitácora de inventarios. Cada movimiento de stock queda registrado y enlazado a su orden de pago correspondiente para total trazabilidad.



 3. Segunda Base de Datos: `DB_EcommerceAgent` (Chatbot y Sistema Experto)

Esta base de datos complementaria está diseñada para dar soporte a un agente inteligente conversacional (chatbot) integrado en el ecosistema del ecommerce. Permite un comportamiento dinámico al interactuar con librerías externas o backend en Python.

 `ReglasChatbot`: Define las directivas de comportamiento del bot.
   Si el tipo de respuesta es `TEXTO_ESTATICO`, el bot responde con un mensaje directo.
   Si el tipo de respuesta es `ACCION_DINAMICA`, el campo `AccionPython` indica el nombre de la función en Python que el backend debe ejecutar (por ejemplo: `buscar_producto_en_db` o `buscar_saludos_en_db`).
 `PalabrasClaveRegla`: Diccionario de palabras clave (o triggers) que el usuario escribe (como "hola", "precio", "asesor") y que activan reglas específicas de chatbot.
 `PlantillasRespuesta`: Permite definir múltiples variaciones de respuesta para una misma regla. Esto evita que el chatbot suene repetitivo y le permite seleccionar diferentes textos de respuesta (ej. "¡Hola! Bienvenido...", "¡Qué gusto tenerte de vuelta!...").
 `HistorialMensajes`: Log transaccional de la conversación donde se guarda el texto ingresado por el cliente, la respuesta generada por el sistema, la fecha y hora, y el identificador de la regla activada, permitiendo auditoría y reentrenamiento del bot.



 4. Lógica de Seguridad y Cifrado de Datos

Para proteger contraseñas y datos bancarios, el sistema implementa Criptografía Simétrica directamente en el motor de base de datos (SQL Server):

```mermaid
graph TD
    A[Database Master Key] >|Cifra y protege| B[Certificado CERT_ECOMMERCE]
    B >|Cifra y protege| C[Llave Simétrica KEY_HASH AES_256]
    C >|Utilizada por| D[Función Escalar: Fn_EncryptByKey]
    C >|Utilizada por| E[Función Escalar: Fn_DecryptByKey]
    D >|Genera datos binarios para| F(Campos Protegidos en Base de Datos)
```

 Funciones de Cifrado
1. `Fn_EncryptByKey`:
    Propósito: Recibir una cadena de texto claro (`VARCHAR(256)`) y cifrarla usando la llave simétrica `KEY_HASH` (algoritmo AES_256).
    Retorno: Datos cifrados en formato binario (`VARBINARY(256)`).
2. `Fn_DecryptByKey`:
    Propósito: Desencriptar datos binarios (`VARBINARY(256)`) convirtiéndolos de nuevo a su valor legible en texto claro (`VARCHAR(256)`).

> [!IMPORTANT]
> Para que estas funciones operen correctamente en cualquier consulta o transacción SQL, es requisito indispensable abrir la clave simétrica previamente en la sesión de base de datos activa usando:
> ```sql
> OPEN SYMMETRIC KEY KEY_HASH DECRYPTION BY CERTIFICATE CERT_ECOMMERCE;
>  Ejecutar consultas o inserciones aquí
> CLOSE SYMMETRIC KEY KEY_HASH;
> ```



 5. Diagrama Relacional de la Base de Datos Transaccional

A continuación se muestra el modelo físico de relaciones entre las entidades operativas del ecommerce:

```mermaid
erDiagram
    Tbl_Users ||o{ Tbl_UserAddress : "registra direcciones"
    Tbl_Users ||o{ Tbl_Carts : "crea carritos"
    Tbl_Users ||o{ Tbl_UserPaymentMethods : "registra tarjetas"
    Tbl_Users ||o{ Tbl_PaymentOrders : "realiza compras"

    Tbl_Categories ||o{ Tbl_ProductIdentificators : "clasifica"
    Tbl_SubCategories ||o{ Tbl_ProductIdentificators : "clasifica"
    Tbl_Segments ||o{ Tbl_ProductIdentificators : "clasifica"
    
    Tbl_ProductIdentificators ||o{ Tbl_Products : "estructura"
    Tbl_MarkByProviders ||o{ Tbl_Products : "provee catálogo"

    Tbl_Products ||o{ Tbl_ProductImages : "contiene fotos"
    Tbl_Products ||o{ Tbl_ProductVariables : "se divide en variantes"
    
    Tbl_ProductVariables ||o{ Tbl_Stocks : "controla existencias"
    Tbl_ProductVariables ||o{ Tbl_CartDetails : "se añade a"
    Tbl_ProductVariables ||o{ Tbl_PaymentOrderDetails : "se factura en"

    Tbl_PaymentOrders ||o{ Tbl_PaymentOrderDetails : "se desglosa en"
    Tbl_PaymentOrders ||o{ Tbl_StockMovements : "afecta inventario"
    Tbl_StockMovements ||o{ Tbl_StockMovementDetails : "se detalla en"
```
