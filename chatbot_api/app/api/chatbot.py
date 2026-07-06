from fastapi import APIRouter, HTTPException
from pydantic import BaseModel

from app.services.chatbot_service import procesar_mensaje
from app.repositories.reglas_repository import cargar_reglas

router = APIRouter(prefix="/api/chatbot", tags=["chatbot"])


class MensajeRequest(BaseModel):
    mensaje: str


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
        return procesar_mensaje(request.mensaje)
    except Exception as ex:
        raise HTTPException(status_code=500, detail=f"Error al procesar mensaje: {ex}")


@router.get("/health")
def health_check():
    return {"status": "ok", "service": "chatbot"}
