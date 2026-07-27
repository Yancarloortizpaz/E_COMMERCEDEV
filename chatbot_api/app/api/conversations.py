from fastapi import APIRouter, HTTPException, Query
from app.core.models.conversation import Conversation, ConversationRequest
from app.services.use_cases.procesar_conversacion import ProcesarConversacionUseCase
from app.services.use_cases.obtener_historial import ObtenerHistorialUseCase
from app.repositories.conversation_repository import ConversationRepository
from app.database.connection import get_connection

router = APIRouter(prefix="/api/chatbot", tags=["conversations"])


@router.post("/conversation")
def nueva_conversacion(conversation: Conversation):
    try:
        use_case = ProcesarConversacionUseCase(ConversationRepository())
        return use_case.ejecutar(conversation)
    except Exception as ex:
        raise HTTPException(status_code=500, detail=f"Error al procesar conversación: {ex}")


@router.get("/obtenerconversation")
def obtener_historial_conversacion(conversation_id: str):
    try:
        use_case = ObtenerHistorialUseCase(ConversationRepository())
        solicitud = ConversationRequest(conversation_id=conversation_id)
        return use_case.ejecutar(solicitud)
    except Exception as ex:
        raise HTTPException(status_code=500, detail=f"Error al obtener historial: {ex}")


# --- ENDPOINT QUE PIDE EL FRONTEND ---
@router.post("/conversations/{conversation_id}/messages")
def crear_conversacion_mensajes(conversation_id: str):
    try:
        use_case = ObtenerHistorialUseCase(ConversationRepository())
        solicitud = ConversationRequest(conversation_id=conversation_id)
        return use_case.ejecutar(solicitud)
    except Exception as ex:
        raise HTTPException(status_code=500, detail=f"Error al obtener historial de mensajes: {ex}")


@router.get("/conversations/{conversation_id}/messages")
def obtener_conversacion_mensajes(conversation_id: str):
    try:
        use_case = ObtenerHistorialUseCase(ConversationRepository())
        solicitud = ConversationRequest(conversation_id=conversation_id)
        return use_case.ejecutar(solicitud)
    except Exception as ex:
        raise HTTPException(status_code=500, detail=f"Error al obtener historial de mensajes: {ex}")


@router.get("/conversations")
def obtener_conversaciones_usuario(userId: str = Query("demo-user")):
    try:
        conn = get_connection()
        cursor = conn.cursor()

        cursor.execute("""
            SELECT ConversacionID, Idioma, UltimaIntencion 
            FROM Conversaciones 
            WHERE UsuarioID = ?
        """, userId)

        filas = cursor.fetchall()
        cursor.close()
        conn.close()

        conversaciones = []
        for row in filas:
            conversaciones.append({
                "conversation_id": row[0],
                "userId": userId,
                "language": row[1] or "es",
                "lastIntent": row[2] or "general"
            })

        return conversaciones

    except Exception as ex:
        raise HTTPException(status_code=500, detail=f"Error al obtener lista de conversaciones: {ex}")