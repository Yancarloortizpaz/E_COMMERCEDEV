from fastapi import APIRouter, HTTPException
from app.core.models.conversation import Conversation, ConversationRequest
from app.services.use_cases.procesar_conversacion import ProcesarConversacionUseCase
from app.services.use_cases.obtener_historial import ObtenerHistorialUseCase
from app.repositories.conversation_repository import ConversationRepository

router = APIRouter(tags=["conversations"])

@router.post("/chatbot/conversation")
def nueva_conversacion(conversation: Conversation):
    try:
        use_case = ProcesarConversacionUseCase(ConversationRepository())
        return use_case.ejecutar(conversation)
    except Exception as ex:
        raise HTTPException(status_code=500, detail=f"Error al procesar conversación: {ex}")

@router.get("/chatbot/obtenerconversation")
def obtener_historial(conversation: ConversationRequest):
    try:
        use_case = ObtenerHistorialUseCase(ConversationRepository())
        return use_case.ejecutar(conversation)
    except Exception as ex:
        raise HTTPException(status_code=500, detail=f"Error al obtener historial: {ex}")
