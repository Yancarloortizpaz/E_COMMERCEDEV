from app.services.actions_service import procesar_mensaje_db


def procesar_mensaje(mensaje):
    return procesar_mensaje_db(mensaje)
