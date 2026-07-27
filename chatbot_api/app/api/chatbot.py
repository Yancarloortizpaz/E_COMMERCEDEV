import json
import traceback
from datetime import date, datetime
from decimal import Decimal
from fastapi import APIRouter, HTTPException
from pydantic import BaseModel

from app.repositories.conversation_repository import ConversationRepository
from app.repositories.reglas_repository import cargar_reglas
from app.services.chatbot_service import procesar_mensaje

router = APIRouter(prefix="/api/chatbot", tags=["chatbot"])
repo_conversacion = ConversationRepository()


# --- FUNCIÓN AUXILIAR PARA LIMPIAR DECIMALES Y FECHAS DE SQL SERVER ---
def sanitizar_json(obj):
    if isinstance(obj, Decimal):
        return float(obj)
    if isinstance(obj, (datetime, date)):
        return obj.isoformat()
    if isinstance(obj, dict):
        return {k: sanitizar_json(v) for k, v in obj.items()}
    if isinstance(obj, list):
        return [sanitizar_json(item) for item in obj]
    return obj


class MensajeRequest(BaseModel):
    mensaje: str
    conversation_id: str | None = "default_session"
    user_id: str | None = "1"


class ReglaResponse(BaseModel):
    ReglaID: int
    NombreRegla: str
    AccionDinamica: bool
    AccionPython: str | None
    PalabrasClave: list[str]


@router.get("/rules", response_model=list[ReglaResponse])
def obtener_reglas():
    try:
        reglas = cargar_reglas()
        return reglas
    except Exception as ex:
        raise HTTPException(status_code=500, detail=f"Error al cargar reglas: {ex}")


@router.post("/chat")
def enviar_mensaje(request: MensajeRequest):
    try:
        # 1. Obtener la respuesta de la regla/IA
        respuesta_bot = procesar_mensaje(request.mensaje)

        # --- CONVERTIR DECIMALES Y DATES A TIPOS COMPATIBLES CON JSON ---
        respuesta_bot = sanitizar_json(respuesta_bot)

        # Extraer productos o metadata generada si existen
        productos = respuesta_bot.get("productos", [])
        metadata_json = json.dumps({"productos": productos}) if productos else None

        # 2. Mapear el payload para la conversación
        payload_conversacion = {
            "conversation_id": request.conversation_id,
            "user_id": request.user_id,
            "context": {
                "language": "es",
                "session_variables": {
                    "last_intent": respuesta_bot.get("tipo", "consulta"),
                    "cart_id": None,
                    "order_id": None,
                },
            },
            "messages": [
                {
                    "role": "user",
                    "timestamp": datetime.now().isoformat(),
                    "intent": "usuario_pregunta",
                    "content": request.mensaje,
                    "metadata": None,
                },
                {
                    "role": "assistant",
                    "timestamp": datetime.now().isoformat(),
                    "intent": respuesta_bot.get("tipo", "respuesta_bot"),
                    "content": respuesta_bot.get("texto", ""),
                    "metadata": metadata_json,
                },
            ],
        }

        # 3. Guardar en SQL Server
        res_repo = repo_conversacion.guardar_conversacion(payload_conversacion)

        if isinstance(res_repo, dict) and "conversation_id" in res_repo:
            respuesta_bot["conversation_id"] = str(res_repo["conversation_id"])

        # 4. Retornar la respuesta limpia
        return respuesta_bot

    except Exception as ex:
        print(f"Error en endpoint chat: {ex}")
        traceback.print_exc()
        raise HTTPException(status_code=500, detail=f"Error al procesar mensaje: {ex}")


@router.get("/health")
def health_check():
    return {"status": "ok", "service": "chatbot"}