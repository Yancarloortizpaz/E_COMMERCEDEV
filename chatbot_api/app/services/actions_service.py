from app.database.connection import get_connection
from app.repositories.products_repository import buscar_producto


def procesar_mensaje_db(mensaje, conversacion_id: int = 1):
    try:
        conn = get_connection()
        cursor = conn.cursor()

        cursor.execute(
            """
            DECLARE @o_TextoRespuesta NVARCHAR(MAX);
            DECLARE @o_ReglaActivadaID INT;
            EXEC dbo.SP_ProcesarMensajeChatbot ?, ?, @o_TextoRespuesta OUTPUT, @o_ReglaActivadaID OUTPUT;
            SELECT @o_TextoRespuesta AS TextoRespuesta, @o_ReglaActivadaID AS ReglaActivadaID;
            """,
            conversacion_id,
            mensaje,
        )

        resultado = cursor.fetchone()

        conn.commit()
        cursor.close()
        conn.close()

        if resultado:

            print("Regla:", resultado.ReglaActivadaID)
            print("Respuesta:", resultado.TextoRespuesta)

            # Regla 2 = búsqueda de productos
            if resultado.ReglaActivadaID == 2:

                productos = buscar_producto(mensaje)

                return {
                    "tipo": "productos",
                    "texto": "Encontré estos productos para ti.",
                    "regla_id": 2,
                    "productos": productos,
                }

            return {
                "tipo": "texto",
                "texto": resultado.TextoRespuesta,
                "regla_id": resultado.ReglaActivadaID,
            }

        return {
            "tipo": "texto",
            "texto": "No se obtuvo respuesta del agente.",
            "regla_id": None,
        }

    except Exception as ex:
        return {
            "tipo": "texto",
            "texto": f"Error al procesar mensaje en la base de datos: {ex}",
            "regla_id": None,
        }


def buscar_producto_en_db(mensaje=None):
    try:
        if not mensaje:
            return {
                "mensaje": "Necesito un texto para buscar productos.",
                "productos": [],
            }

        productos = buscar_producto(mensaje)

        if not productos:
            return {
                "mensaje": f"No se encontró ningún producto con '{mensaje}'.",
                "productos": [],
            }

        return {
            "mensaje": f"Se encontraron {len(productos)} producto(s).",
            "productos": productos,
        }

    except Exception as ex:
        return {
            "mensaje": f"Error al buscar producto en la base de datos: {ex}",
            "productos": [],
        }