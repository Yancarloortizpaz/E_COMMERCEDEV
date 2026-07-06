from fastapi import APIRouter, WebSocket, WebSocketDisconnect

from app.services.chatbot_service import procesar_mensaje

router = APIRouter()


@router.websocket("/ws/chat")
async def websocket_chat(websocket: WebSocket):
    await websocket.accept()

    try:
        while True:
            mensaje = await websocket.receive_text()
            respuesta = procesar_mensaje(mensaje)
            await websocket.send_text(str(respuesta))

    except WebSocketDisconnect:
        print("Cliente desconectado")