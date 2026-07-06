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
            return {
                "texto": resultado.TextoRespuesta,
                "regla_id": resultado.ReglaActivadaID,
            }

        return {
            "texto": "No se obtuvo respuesta del agente.",
            "regla_id": None,
        }
    except Exception as ex:
        return {
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
