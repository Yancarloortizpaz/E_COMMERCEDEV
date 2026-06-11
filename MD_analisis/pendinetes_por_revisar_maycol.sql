 • Insertar_Carrito.sql: Valida usuario/creador activo y unicidad de carrito activo por usuario.
      • Actualizar_Carrito.sql: Valida modificador activo y previene duplicidad de carritos activos.
      • Eliminar_Carrito.sql: Cambiado a borrado lógico ( cartStatusId = 0 ), valida estado del carrito y modificador
      activo.
  2.
      • Insertar_Categoria.sql: Valida creador activo y unicidad del nombre de categoría.
      • Actualizar_Categoria.sql: Valida modificador activo, unicidad y control de restauración con  ForzarRecuperacion
.
      • Eliminar_Categoria.sql: Robustece la inactivación lógica existente.
  3.
      • Insertar_Detalle_Carrito.sql: Valida relaciones activas, controla inventario disponible y no expirado, y
fusiona
      automáticamente cantidades si la variante ya existe en el carrito.
      • Eliminar_Detalle_Carrito.sql: Cambiado a borrado lógico ( cartDetailStatusId = 0 ), con auditoría de usuario
modificador.

integre la autulizacion y aqui quedo no probel los otros sp  aun si revisar pero ya estan en el repo 
  4.
      • Insertar_Direccion_Usuario.sql: Valida usuario activo y desactiva bandera de dirección principal en las demás
si la nueva se
      define como principal.
      • Actualizar_Direccion_Usuario.sql: Valida modificador activo, dirección principal y control de recuperación.
      • Eliminar_Direccion_Usuario.sql: Robustece la inactivación lógica existente.
  5.
      • Insertar_Marca.sql: Valida creador activo y unicidad del nombre de marca.
      • Actualizar_Marca.sql: Valida modificador activo, unicidad del nombre y control de recuperación.
      • Eliminar_Marca.sql: Robustece la inactivación lógica.
  6.
      • Insertar_Moneda.sql: Valida creador y unicidad triple (nombre, código ISO y código numérico).
      • Actualizar_Moneda.sql: Valida modificador activo, unicidad triple y control de recuperación.
      • Eliminar_Moneda.sql: Robustece la inactivación lógica.



	   ### 1. Cambios en Insertar_Usuario.sql ( sp_Users_Create )

  Antes era una inserción directa y muy simple. Ahora se convirtió en un procedimiento estandarizado y robusto con
  las siguientes mejoras:

  • Parámetros de Salida: Se agregaron  @o_code ,  @o_message  y  @o_templateId  (que devuelve el ID del usuario
  recién creado).
  • Validaciones Exhaustivas:
      • Obligatoriedad: Se verifica que los campos obligatorios como nombre, usuario, correo, contraseña, fecha de
      nacimiento, creador y estado no sean nulos o vacíos.
      • Límites de Fecha: Se valida que el año de nacimiento sea mayor a 1925 y que el usuario tenga al menos 12 años
      de edad.
      • Integridad Referencial: Valida que el usuario creador exista y esté activo (se exceptúa el ID 1 para el
      primer usuario), y que el estado exista en la tabla  Tbl_Status .
      • Unicidad: Valida que ni el nombre de usuario ( userName ) ni el correo ( userEmail ) estén repetidos en la
      base de datos.
  • Gestión de Transacciones y Cifrado Seguro:
      • Implementa un bloque  BEGIN TRY / BEGIN CATCH  con transacciones seguras ( BEGIN TRANSACTION / COMMIT ).
      • El descifrado/cifrado de la contraseña abre la llave simétrica ( OPEN SYMMETRIC KEY ) y asegura su correcto
      cierre ( CLOSE SYMMETRIC KEY ) tanto en el flujo normal como en el bloque  CATCH  si ocurre algún error,
      previniendo fugas de conexiones o bloqueos.
      • Limpieza de espacios en blanco mediante  TRIM .
  • Prueba Integrada: Se agregó un bloque de código comentado al final para ejecutar pruebas rápidas del SP.
  ──────
  ### 2. Cambios en Actualizar_Usuario.sql ( sp_Users_Update )

  Antes consistía únicamente en un bloque  UPDATE  básico. Se modificó para añadir lógica de control y validación de
  negocio:

  • Nuevos Parámetros:
      • Se añadieron los parámetros estándar  @o_code ,  @o_message  y  @o_templateId .
      • Se agregó el parámetro  @ForzarRecuperacion BIT = 0  para regular la edición de registros inactivos.
  • Validaciones Robustas:
      • Existencia del Usuario: Valida si el ID del usuario a modificar existe en el sistema.
      • Usuario Modificador: Verifica que la persona que edita exista y esté activa.
      • Protección de Registro Inactivo: Si el usuario está marcado como inactivo ( userStatusId = 2 ), el SP no
      permitirá actualizarlo a menos que explícitamente se configure  @ForzarRecuperacion = 1 .
      • Unicidad de Correo: Verifica que el nuevo correo electrónico no esté siendo utilizado por ningún otro usuario
      en la base de datos ( userId <> @userId ).
  • Protección Transaccional:
      • Lógica transaccional completa con  BEGIN TRANSACTION ,  COMMIT  y  ROLLBACK  ante errores.
      • Limpieza de cadenas mediante  TRIM .
  • Prueba Integrada: Al igual que el SP de creación, incluye un script comentado de testeo al final.
