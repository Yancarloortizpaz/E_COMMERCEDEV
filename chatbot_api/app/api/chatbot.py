from fastapi import APIRouter, HTTPException
from pydantic import BaseModel

from app.services.chatbot_service import procesar_mensaje
from app.repositories.reglas_repository import cargar_reglas, obtener_regla_por_id

router = APIRouter(prefix="/api/chatbot", tags=["chatbot"])


class MensajeRequest(BaseModel):
    mensaje: str
    conversacion_id: int = 1


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


@router.get("/rules/{regla_id}", response_model=ReglaResponse)
def obtener_regla(regla_id: int):
    try:
        regla = obtener_regla_por_id(regla_id)
        if not regla:
            raise HTTPException(status_code=404, detail=f"Regla {regla_id} no encontrada")
        return regla
    except HTTPException:
        raise
    except Exception as ex:
        raise HTTPException(status_code=500, detail=f"Error al obtener regla: {ex}")


@router.post("/chat")
def enviar_mensaje(request: MensajeRequest):
    try:
        return procesar_mensaje(request.mensaje, request.conversacion_id)
    except Exception as ex:
        raise HTTPException(status_code=500, detail=f"Error al procesar mensaje: {ex}")


@router.get("/health")
def health_check():
    return {"status": "ok", "service": "chatbot"}
