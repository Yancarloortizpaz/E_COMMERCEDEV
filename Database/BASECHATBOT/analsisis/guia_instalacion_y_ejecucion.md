 Guía de Instalación, Configuración y Ejecución de la API del Chatbot

Esta guía contiene los pasos necesarios para configurar el entorno virtual, instalar las dependencias requeridas, configurar la conexión a la base de datos SQL Server y levantar el servidor backend de FastAPI en Windows.

---

 1. Requisitos Previos

Antes de comenzar, asegúrate de tener instalado en tu sistema:
1. Python 3.10 o superior: Puedes verificar tu versión ejecutando en la consola:
   ```powershell
   python --version
   ```
2. Microsoft ODBC Driver 17 for SQL Server: Requerido por la biblioteca `pyodbc` para conectarse a SQL Server.

---

 2. Preparación del Entorno Virtual (venv)

El proyecto contiene el backend en la carpeta `C:\hector\E_COMMERCEDEV\Database\BASECHATBOT\chatbot_api`.

1. Abre tu terminal (PowerShell o CMD) y navega a la carpeta de la API:
   ```powershell
   cd "C:\hector\E_COMMERCEDEV\Database\BASECHATBOT\chatbot_api"
   ```

2. Crear el Entorno Virtual:
   Si no tienes un entorno virtual creado en esta carpeta, puedes inicializar uno llamado `venv` ejecutando:
   ```powershell
   python -m venv venv
   ```

3. Activar el Entorno Virtual:
    Desde PowerShell:
     ```powershell
      Si es la primera vez, permite la ejecución de scripts en la sesión actual
     Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process
     
     .\venv\Scripts\Activate.ps1
     ```
    Desde CMD (Símbolo del sistema):
     ```cmd
     .\venv\Scripts\activate.bat
     ```

Una vez activado, verás que el prefijo de tu terminal cambia a `(venv)`.

---

 3. Instalación de Dependencias

Con el entorno virtual activado, puedes instalar todas las librerías necesarias de dos maneras:

 Opción A (Recomendada): Usar el archivo de requerimientos
Instala de golpe todas las dependencias del archivo `requirements.txt`:
```powershell
pip install -r requirements.txt
```

 Opción B: Instalación manual paso a paso
Si deseas instalar cada paquete de forma individual para validar el proceso:
```powershell
python -m pip install fastapi uvicorn pyodbc python-dotenv
```

Las dependencias clave instaladas son:
 `fastapi`: Framework web moderno y rápido para construir APIs.
 `uvicorn`: Servidor ASGI de alto rendimiento para ejecutar la aplicación.
 `pyodbc`: Conector para base de datos de SQL Server.
 `python-dotenv`: Carga de variables de entorno desde un archivo `.env`.

---

 4. Configuración del Archivo `.env`

La API lee las credenciales del servidor SQL Server del archivo `.env` ubicado en `C:\hector\E_COMMERCEDEV\Database\BASECHATBOT\chatbot_api\.env`.

Abre ese archivo y edita las siguientes variables para que coincidan con tu base de datos:

```ini
 Configuración de base de datos
DB_DRIVER=ODBC Driver 17 for SQL Server
DB_SERVER=TU_SERVIDOR_SQL_SERVER    Reemplaza con el nombre de tu servidor (ej. localhost o DESKTOP-XXXX)
DB_NAME=DB_EcommerceAgent
DB_TRUSTED_CONNECTION=yes           yes si usas Autenticación de Windows

 Si usas Autenticación de SQL Server (con usuario y contraseña), cambia la variable anterior a "no" y descomenta estas líneas:
 DB_USER=tu_usuario
 DB_PASSWORD=tu_contrasena
DB_TIMEOUT=30
```
para probar si  db_driver esta instaldo el comando en la consola es ; 
python -c "import pyodbc; print(pyodbc.drivers())"
para ver si es exito la conexion

$conn = New-Object System.Data.SqlClient.SqlConnection("Server=CLARK;Database=DB_EcommerceAgent;
  Trusted_Connection=True;"); try { $conn.Open(); Write-Output "¡Conexión Exitosa!"; $conn.Close() } catch { Write-
  Error $_ }
---

 5. Levantar y Ejecutar la API

Con la base de datos iniciada y el entorno virtual activado, ejecuta el siguiente comando en la carpeta `C:\hector\E_COMMERCEDEV\Database\BASECHATBOT\chatbot_api` para arrancar el servidor backend:

```powershell
uvicorn app.main:app --reload --port 8000
```

Explicación de parámetros:
 `app.main:app`: Apunta a la instancia de FastAPI (`app`) dentro del archivo `app/main.py`.
 `--reload`: Habilita el reinicio automático del servidor cuando se detecten cambios en los archivos de código (ideal para desarrollo).
 `--port 8000`: Expone el servidor local en el puerto `8000`.

Al iniciar con éxito, verás mensajes en la consola indicando que Uvicorn está corriendo en `http://127.0.0.1:8000`.

---

 6. Pruebas y Validación

FastAPI incluye documentación interactiva auto-generada. Una vez levantado el servidor, abre tu navegador web y dirígete a:

👉 [http://127.0.0.1:8000/docs](http://127.0.0.1:8000/docs)

Desde esta interfaz de Swagger podrás enviar peticiones reales de prueba a los endpoints:

 Endpoints del Flujo de Reglas Clásicas y WebSocket:
1. `GET /`: Endpoint de verificación del servicio.
2. `GET /api/chatbot/rules`: Carga la lista de reglas dinámicas activas desde la base de datos.
3. `POST /api/chatbot/chat`: Envía un mensaje en formato simple `{ "mensaje": "Hola" }` para probar el flujo de reglas de base de datos.
4. `WS /ws/chat`: Conexión de WebSocket para chat bidireccional en tiempo real.

 Endpoints del Flujo de Historial Estructurado JSON:
1. `POST /chatbot/conversation`: Envía el JSON completo con los metadatos y mensajes estructurados del chat para guardarlos en la BD.
2. `GET /chatbot/obtenerconversation`: Recupera el historial de mensajes de la conversación enviando el JSON del cuerpo con el ID de la conversación.

---

 7. Explicación Detallada del Funcionamiento y Estructura de Datos (Manual del Usuario y Técnico)

Esta sección detalla cómo utilizar y llenar la estructura de datos en los endpoints desde Swagger UI (nivel de usuario) y cómo es el procesamiento y almacenamiento de la información en el backend y la base de datos (nivel interno).

 7.1. Flujo de Reglas Clásicas y Búsqueda de Productos (`/api/chatbot/chat`)
Este flujo está diseñado para interacciones ágiles y secuenciales basadas en reglas dinámicas cargadas en SQL Server.

 A. A nivel de Usuario (Swagger UI / Cliente)
 Endpoint: `POST /api/chatbot/chat`
 JSON a enviar (Cuerpo de la Petición):
  ```json
  {
    "mensaje": "hola hay computadora dell?"
  }
  ```
 JSON de Respuesta recibido:
  ```json
  {
    "texto": "¡Hola! Bienvenido a nuestra tienda virtual. ¿Qué producto deseas buscar hoy?",
    "regla_id": 1
  }
  ```
   Explicación del comportamiento: El usuario envía una pregunta o saludo. Si la API detecta palabras clave configuradas en el sistema (por ejemplo, el trigger `"hola"` que dispara la `Regla 1` de saludo), responde inmediatamente con un texto predefinido. Si el usuario escribe palabras de búsqueda sin un saludo explícito u otra regla estática, el sistema busca en el catálogo de productos disponibles.

 B. A nivel Interno (Backend y Base de Datos)
1. Controlador FastAPI: Recibe el mensaje a través de `enviar_mensaje` (`app/api/chatbot.py`) y llama a `procesar_mensaje_db` en el servicio de acciones.
2. Ejecución del SP: Se abre una conexión a la BD y se ejecuta el procedimiento almacenado `dbo.SP_ProcesarMensajeChatbot` enviando la conversación (`conversacion_id = 1` por defecto) y el texto ingresado.
3. Registro del Mensaje Entrante: El SP inserta el texto del usuario en la tabla `Mensajes` con el rol `'user'`.
4. Disparador y Reglas:
    El SP consulta la tabla `PalabrasClaveRegla` buscando coincidencias del texto de entrada con las palabras clave activas usando `CHARINDEX`.
    Si hay coincidencia, obtiene la `ReglaID` asociada.
    Inversión de la Regla: Si no hay ninguna palabra clave que coincida (por ejemplo, el usuario escribió marcas o categorías de productos como `"dell"`, `"computadora"`), el SP asigna por defecto la Regla ID 2 (Búsqueda de productos).
5. Búsqueda y Formateo (Regla 2):
    Si la regla es la 2, ejecuta el procedimiento `dbo.SP_ListarGeneralProducts_Filtro` pasando el texto del usuario como filtro.
    Si hay productos coincidentes en stock (`StockAvilable > 0`), obtiene una plantilla aleatoria de `PlantillasRespuesta` para la `ReglaID = 2` y reemplaza el marcador de posición `[@TABLA]` por el listado de productos formateado con nombre, precio y stock.
    Si no hay productos disponibles, activa las reglas de fallo por defecto (Regla 5 o 6 según el tamaño del término buscado) y devuelve la plantilla correspondiente.
6. Flujo Estático: Para otras reglas (Saludos, despedidas, métodos de pago), selecciona un texto aleatorio de `PlantillasRespuesta` para esa regla específica.
7. Registro de la Respuesta Saliente: El SP guarda la respuesta final del bot en la tabla `Mensajes` con el rol `'assistant'` y retorna a la API el texto obtenido y la regla que se activó.

---

 7.2. Flujo de Historial Estructurado JSON (`/chatbot/conversation` y `/chatbot/obtenerconversation`)
Este flujo está diseñado para almacenar de manera estructurada el estado completo de una sesión de chat, incluyendo metadatos adicionales, intenciones detectadas y variables de sesión (carrito, pedidos, idioma).

 A. A nivel de Usuario (Swagger UI / Cliente)

 1. Crear/Actualizar Conversación (`POST /chatbot/conversation`)
Para registrar o actualizar el historial completo en la base de datos, debes enviar la estructura JSON con los siguientes campos.

Ejemplo de cómo llenarlo y enviarlo:
```json
{
  "conversation_id": "sesion_usuario_1001",
  "user_id": "perdomotoa",
  "messages": [
    {
      "role": "user",
      "timestamp": "2026-07-02T03:54:55.921Z",
      "intent": "saludo",
      "content": "hola",
      "metadata": {
        "type": "deteccion_intenciones",
        "entity": "saludo",
        "fields": ["probabilidad"],
        "data": [
          {
            "probabilidad": 0.99
          }
        ]
      }
    },
    {
      "role": "assistant",
      "timestamp": "2026-07-02T03:55:00.120Z",
      "intent": "respuesta_saludo",
      "content": "¡Hola! Bienvenido a nuestra tienda virtual. ¿Qué producto deseas buscar hoy?",
      "metadata": null
    }
  ],
  "context": {
    "language": "es",
    "session_variables": {
      "last_intent": "saludo",
      "cart_id": "carrito_98765",
      "order_id": "pedido_12345"
    }
  }
}
```

 Explicación de cómo llenar cada campo:
 `conversation_id`: Un identificador único de la sesión del chat. Puede ser un UUID o cualquier cadena única generada en tu front-end o app móvil (ej: `"sesion_usuario_1001"`).
 `user_id`: El identificador del usuario que chatea (ej: `"perdomotoa"`).
 `messages`: Un arreglo que contiene el historial de mensajes de la conversación. Cada objeto del arreglo tiene:
   `role`: El rol del emisor del mensaje. Generalmente `"user"` (usuario) o `"assistant"` (chatbot).
   `timestamp`: Fecha y hora en formato estándar ISO 8601 (ej: `"2026-07-02T03:54:55.921Z"`).
   `intent`: La intención identificada en el mensaje (ej: `"saludo"`, `"buscar_productos"`, o `"finalizar_compra"`).
   `content`: El contenido textual del mensaje de chat.
   `metadata`: (Opcional, puede ser `null` o un objeto). Permite guardar información contextual detallada (búsquedas hechas, entidades extraídas, etc.). Debe tener:
     `type`: Tipo de metadato (ej: `"busqueda"`).
     `entity`: Entidad extraída (ej: `"categoria"` o `"marca"`).
     `fields`: Listado de nombres de atributos incluidos en los datos.
     `data`: Un arreglo de objetos con los valores de dichos atributos.
 `context`: Estado actual de la conversación:
   `language`: Idioma activo (ej: `"es"`, `"en"`).
   `session_variables`: Un diccionario de clave-valor. Internamente, el SP busca de forma explícita:
     `last_intent`: La última intención detectada en la sesión.
     `cart_id`: El identificador del carrito de compras activo.
     `order_id`: El identificador de la orden/pedido activo.

 2. Obtener Historial de una Conversación (`GET /chatbot/obtenerconversation`)
Para consultar los mensajes almacenados de una conversación, debes enviar un JSON especificando el ID de la misma.

Ejemplo de cómo llenarlo:
```json
{
  "conversation_id": "sesion_usuario_1001"
}
```
 Respuesta: La API devolverá la lista cronológica de todos los mensajes con sus respectivos metadatos y roles almacenados para ese ID de sesión.

 B. A nivel Interno (Backend y Base de Datos)
1. Recepción y Validación: FastAPI valida la estructura del payload mediante el modelo `Conversation` definido con Pydantic (`app/core/models/conversation.py`).
2. Serialización y Envío: El backend serializa todo el objeto a texto JSON (`model_dump_json()`) y lo envía a la base de datos a través de `ConversationRepository.guardar_conversacion`.
3. Procedimiento Almacenado `sp_GuardarConversacion`:
    Desglose de Cabecera: Lee las propiedades principales `conversation_id` y `user_id` desde el nodo raíz del JSON usando `OPENJSON`.
    Variables de Sesión: Lee el nodo `$.context.session_variables` y extrae los campos clave `last_intent`, `cart_id` y `order_id` mediante consultas JSON (`WITH` y `CROSS APPLY OPENJSON`).
    Cabecera de Conversaciones:
      Si la conversación no existe en la tabla `Conversaciones`, crea un nuevo registro con el ID de sesión, el usuario, el idioma y las variables de sesión.
      Si la conversación ya existe, actualiza los campos de contexto con los nuevos valores y elimina los mensajes existentes de la tabla `Mensajes` correspondientes a esa sesión (`DELETE FROM Mensajes WHERE ConversacionID = @ConversacionID`) para evitar duplicidad de registros en reenvíos completos.
    Mensajes en Detalle: El SP recorre el arreglo `$.messages` del JSON utilizando `OPENJSON` y los inserta de golpe en la tabla `Mensajes` mapeando los campos, incluyendo los metadatos complejos en formato de cadena JSON.
4. Recuperación con `sp_ObtenerHistorialConversacion`:
    Al consultar el historial, se ejecuta este SP pasando el ID de la conversación.
    Este realiza un `SELECT` de los mensajes filtrados por `ConversacionID` y ordenados ascendentemente por la fecha.
    El repositorio del backend (`ConversationRepository.obtener_historial`) intercepta el campo `Metadata`, comprueba si es un texto JSON válido y, si lo es, lo deserializa de vuelta a un objeto Python (`json.loads`) para retornárselo al cliente de forma estructurada y sin caracteres de escape.

 7.3. Flujo y Serialización de Datos Complejos (De JSON a Texto en SQL y viceversa)
Para comprender a fondo el ejemplo y la arquitectura propuesta por el docente, es fundamental entender cómo viaja la información estructurada entre el cliente (Swagger UI), Python (FastAPI) y la Base de Datos (SQL Server).

 ¿Por qué guardamos JSON como texto plano en SQL Server?
SQL Server no cuenta con un tipo de datos "JSON" nativo como MongoDB o PostgreSQL. En su lugar, SQL Server almacena las estructuras JSON en columnas de tipo texto plano, como `NVARCHAR(MAX)`.
Para que el flujo sea transparente, se implementa una técnica de serialización y deserialización automática en tres etapas:

 Etapa 1: Del Cliente (JSON) a Python (Objeto Pydantic)
1. El usuario envía una petición HTTP con un JSON estructurado que incluye listas y objetos anidados (como la propiedad `metadata`).
2. El framework FastAPI intercepta la petición y utiliza Pydantic para validar que cumpla con el modelo `Conversation`. En este punto, la información se maneja como objetos de Python (diccionarios y listas).

 Etapa 2: De Python a SQL Server (Texto plano parseado en SP)
1. Antes de enviar los datos a la base de datos, el repositorio en Python (`ConversationRepository`) convierte todo el objeto validado en una única y larga cadena de texto plano en formato JSON utilizando el método `conversation.model_dump_json()` (o `.json()`).
2. Esta cadena de texto se pasa como el único parámetro `@ConversationJson` al ejecutar el procedimiento almacenado `dbo.sp_GuardarConversacion`.
3. Dentro de SQL Server, el procedimiento almacenado hace magia con la función `OPENJSON`:
    Para extraer campos individuales y escalares (como `user_id` o `role`), los mapea como tipos estándar (`VARCHAR`).
    Para el campo `metadata` (que es dinámico y contiene arreglos y objetos), utiliza el modificador `AS JSON`:
     ```sql
     metadata NVARCHAR(MAX) '$.metadata' AS JSON
     ```
     El modificador `AS JSON` le dice a SQL Server: "No intentes separar las propiedades individuales de este objeto. Extrae el bloque completo tal cual está escrito en formato JSON y guárdalo como texto en la columna `Metadata` de la tabla `Mensajes`".

 Etapa 3: De la Base de Datos al Cliente (Deserialización automática)
1. Cuando se solicita el historial con `/chatbot/obtenerconversation`, el procedimiento almacenado `dbo.sp_ObtenerHistorialConversacion` realiza un `SELECT` simple y retorna las filas de la tabla `Mensajes`. En este resultado, la columna `Metadata` viene como una cadena de texto plano (ej: `'{"type":"ksk","entity":"jsj",...}'`).
2. El repositorio en Python recibe este texto, y para evitar retornarle al usuario final un texto con caracteres de escape (`\"`), ejecuta un parseo dinámico utilizando la función nativa `json.loads(metadata)`:
   ```python
   metadata = registro.get("Metadata")
   if metadata and isinstance(metadata, str):
       try:
            Convierte la cadena de texto de vuelta a un objeto JSON (lista/diccionario)
           registro["Metadata"] = json.loads(metadata)
       except json.JSONDecodeError:
           registro["Metadata"] = metadata
   ```
3. Finalmente, FastAPI recibe este diccionario de Python y lo envía al navegador del usuario como un objeto JSON interactivo.

 Ventajas de esta arquitectura (Diseño del Docente)
Esta estructura permite que la base de datos SQL Server guarde información extremadamente dinámica y variable (como los metadatos de intenciones, búsquedas, carritos, etc.) sin necesidad de estar creando docenas de tablas relacionales adicionales o alterar el esquema de la base de datos cada vez que los campos de metadatos cambien.
