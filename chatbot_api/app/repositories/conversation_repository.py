import hashlib
from app.database.connection import get_connection


def _normalize_conversation_id(conversation_id):
    """
    Normaliza el ID de conversación.
    Si recibe un número o un string numérico, lo convierte a int (BIGINT).
    Si recibe 'default', None o un string no numérico, retorna None para que SQL Server
    maneje la creación mediante la propiedad IDENTITY.
    """
    if conversation_id is None:
        return None

    if isinstance(conversation_id, int):
        return conversation_id

    if isinstance(conversation_id, str):
        if conversation_id.isdigit():
            return int(conversation_id)
        # Si es un string no numérico (ej. 'default', 'session_abc')
        return None

    return None


class ConversationRepository:

    def guardar_conversacion(self, conversation):
        conn = get_connection()
        cursor = conn.cursor()

        # Extraer datos de modelo Pydantic o diccionario
        if hasattr(conversation, "model_dump"):
            conversation_data = conversation.model_dump()
        elif hasattr(conversation, "dict"):
            conversation_data = conversation.dict()
        elif isinstance(conversation, dict):
            conversation_data = conversation
        else:
            raise TypeError("conversation must be a dict or Pydantic model")

        raw_conversation_id = conversation_data.get("conversation_id")
        user_id = conversation_data.get("user_id") or "demo-user"
        messages = conversation_data.get("messages") or []

        # Normalizamos el conversation_id
        conversation_id = _normalize_conversation_id(raw_conversation_id)

        id_confirmado = conversation_id

        try:
            for message in messages:
                if hasattr(message, "model_dump"):
                    message_data = message.model_dump()
                elif hasattr(message, "dict"):
                    message_data = message.dict()
                elif isinstance(message, dict):
                    message_data = message
                else:
                    raise TypeError("message must be a dict or Pydantic model")

                texto = message_data.get("content") or message_data.get("texto") or ""
                role = (message_data.get("role") or "").lower()
                chatbot_flag = 1 if role in ("assistant", "bot", "chatbot") else 0
                regla_id = message_data.get("appliedRuleId") or message_data.get("ReglaActivadaID")

                # Ejecutamos el SP pasando el conversation_id (que puede ser None si es nuevo)
                cursor.execute(
                    "EXEC dbo.sp_GuardarConversacion ?, ?, ?, ?, ?",
                    id_confirmado,
                    user_id,
                    chatbot_flag,
                    texto[:1000],  # Limitar a la capacidad de la columna Texto
                    regla_id,
                )

                # Si el SP devuelve el ID autogenerado, lo capturamos
                if cursor.description:
                    row = cursor.fetchone()
                    if row and row[0]:
                        id_confirmado = row[0]

            conn.commit()
        except Exception as ex:
            conn.rollback()
            raise RuntimeError(f"Error al guardar conversación en la base de datos: {ex}")
        finally:
            cursor.close()
            conn.close()

        return {
            "status": "ok",
            "conversation_id": str(id_confirmado) if id_confirmado else str(raw_conversation_id),
        }

    def obtener_historial(self, conversation_request):
        conn = get_connection()
        cursor = conn.cursor()

        if hasattr(conversation_request, "conversation_id"):
            raw_id = conversation_request.conversation_id
        elif isinstance(conversation_request, dict):
            raw_id = conversation_request.get("conversation_id")
        elif isinstance(conversation_request, (int, str)):
            raw_id = conversation_request
        else:
            raise TypeError("conversation_request must be a dict, str, int or Pydantic model")

        conversation_id = _normalize_conversation_id(raw_id)

        if conversation_id is None:
            cursor.close()
            conn.close()
            return []

        try:
            cursor.execute("EXEC dbo.sp_ObtenerHistorialConversacion ?", conversation_id)
            rows = cursor.fetchall()

            historial = []
            for row in rows:
                historial.append({
                    "id": row[0],
                    "conversationId": row[1],
                    "isBot": bool(row[2]),
                    "role": row[3],
                    "content": row[4],
                    "timestamp": row[5].isoformat() if hasattr(row[5], "isoformat") else str(row[5]),
                    "appliedRuleId": row[6],
                })

            return historial
        finally:
            cursor.close()
            conn.close()