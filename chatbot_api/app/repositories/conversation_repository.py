import logging
from datetime import datetime, timezone
from typing import Any, Dict, List, Optional
from app.database.connection import get_connection
from app.core.models.conversation import ConversationRequest
import json

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)


def _parse_timestamp(value) -> datetime:
    if value is None:
        return datetime.now(timezone.utc).replace(tzinfo=None)
    if isinstance(value, datetime):
        dt = value
    elif isinstance(value, str):
        s = value.strip()
        if s.endswith("Z"):
            s = s[:-1] + "+00:00"
        try:
            dt = datetime.fromisoformat(s)
        except Exception:
            try:
                if "." in s:
                    base, _ = s.split(".", 1)
                    dt = datetime.fromisoformat(base)
                else:
                    dt = datetime.fromisoformat(s)
            except Exception:
                dt = datetime.now(timezone.utc)
    else:
        dt = datetime.now(timezone.utc)
    if dt.tzinfo is not None:
        dt = dt.astimezone(timezone.utc).replace(tzinfo=None)
    return dt.replace(tzinfo=None)


class ConversationRepository:
    def guardar_conversacion(self, conversation: Dict[str, Any]) -> Dict[str, Any]:
        user_id = conversation.get("userId") or conversation.get("user_id") or "demo-user"
        messages = conversation.get("messages") or []
        provided_conv_id = conversation.get("conversation_id") or conversation.get("conversationId")

        logger.info("guardar_conversacion called; user_id=%s messages_count=%d provided_conv_id=%s",
                    user_id, len(messages), provided_conv_id)

        conn = get_connection()
        cursor = conn.cursor()
        try:
            conversation_id: Optional[int] = None

            if provided_conv_id is not None:
                cursor.execute("SELECT ConversacionID FROM dbo.HistorialConversaciones WHERE ConversacionID = ?", provided_conv_id)
                row = cursor.fetchone()
                if row:
                    conversation_id = int(row.ConversacionID)
                    cursor.execute(
                        "UPDATE dbo.HistorialConversaciones SET UsuarioID = ?, FechaFin = GETDATE(), Activo = 1 WHERE ConversacionID = ?",
                        user_id,
                        conversation_id
                    )
                else:
                    cursor.execute(
                        "INSERT INTO dbo.HistorialConversaciones (UsuarioID, FechaInicio, Activo) OUTPUT INSERTED.ConversacionID VALUES (?, GETDATE(), 1)",
                        user_id
                    )
                    conversation_id = int(cursor.fetchone()[0])
            else:
                cursor.execute(
                    "INSERT INTO dbo.HistorialConversaciones (UsuarioID, FechaInicio, Activo) OUTPUT INSERTED.ConversacionID VALUES (?, GETDATE(), 1)",
                    user_id
                )
                conversation_id = int(cursor.fetchone()[0])

            if not conversation_id:
                raise RuntimeError("No se pudo obtener el ConversacionID generado por la base de datos.")

            # Insertar mensajes asociados, ahora guardando MetadataJson
            for msg in messages:
                role = (msg.get("role") or "").lower()
                is_bot_flag = msg.get("isBot")
                is_bot = 1 if (is_bot_flag is True or role in ("assistant", "bot", "chatbot")) else 0

                texto = msg.get("content") or msg.get("texto") or ""
                if texto is None:
                    texto = ""

                timestamp = _parse_timestamp(msg.get("timestamp"))
                rule_id = msg.get("appliedRuleId") or msg.get("ReglaActivadaID")

                # metadata puede venir como dict/string/list; normalizar a JSON string o NULL
                metadata = msg.get("metadata") or msg.get("Metadata") or msg.get("productos")
                metadata_json = None
                if metadata is not None:
                    try:
                        # si ya es string y parece JSON, dejarlo; si es objeto, serializar
                        if isinstance(metadata, str):
                            # intentar parsear para validar
                            try:
                                json.loads(metadata)
                                metadata_json = metadata
                            except Exception:
                                metadata_json = json.dumps(metadata, default=str)
                        else:
                            metadata_json = json.dumps(metadata, default=str)
                    except Exception:
                        metadata_json = json.dumps(str(metadata))

                cursor.execute(
                    """INSERT INTO dbo.HistorialMensajes
                       (ConversacionID, ChatBot, Texto, FechaHora, ReglaActivadaID, MetadataJson)
                       VALUES (?, ?, ?, ?, ?, ?)""",
                    conversation_id,
                    is_bot,
                    texto,
                    timestamp,
                    rule_id,
                    metadata_json
                )

            conn.commit()
            return {"status": "ok", "conversation_id": conversation_id}
        except Exception as ex:
            conn.rollback()
            logger.exception("Error al guardar conversación")
            raise RuntimeError(f"Error al guardar conversación: {ex}")
        finally:
            cursor.close()
            conn.close()

    def guardar_mensaje(self, conversation_id, message):
        if not conversation_id:
            raise RuntimeError("conversation_id es requerido para guardar un mensaje.")

        conn = get_connection()
        cursor = conn.cursor()
        try:
            cursor.execute("SELECT ConversacionID FROM dbo.HistorialConversaciones WHERE ConversacionID = ?", conversation_id)
            if not cursor.fetchone():
                raise RuntimeError(f"La conversación {conversation_id} no existe en la base de datos.")

            role = (message.get("role") or "").lower()
            is_bot_flag = message.get("isBot")
            is_bot = 1 if (is_bot_flag is True or role in ("assistant", "bot", "chatbot")) else 0

            texto = message.get("content") or message.get("texto") or ""
            if texto is None:
                texto = ""

            timestamp = _parse_timestamp(message.get("timestamp"))
            rule_id = message.get("appliedRuleId") or message.get("ReglaActivadaID")

            metadata = message.get("metadata") or message.get("Metadata") or message.get("productos")
            metadata_json = None
            if metadata is not None:
                try:
                    if isinstance(metadata, str):
                        try:
                            json.loads(metadata)
                            metadata_json = metadata
                        except Exception:
                            metadata_json = json.dumps(metadata, default=str)
                    else:
                        metadata_json = json.dumps(metadata, default=str)
                except Exception:
                    metadata_json = json.dumps(str(metadata))

            cursor.execute(
                """INSERT INTO dbo.HistorialMensajes
                   (ConversacionID, ChatBot, Texto, FechaHora, ReglaActivadaID, MetadataJson)
                   VALUES (?, ?, ?, ?, ?, ?)""",
                conversation_id,
                is_bot,
                texto,
                timestamp,
                rule_id,
                metadata_json
            )

            conn.commit()
            return {"status": "ok", "conversation_id": conversation_id}
        except Exception as ex:
            conn.rollback()
            logger.exception("Error al guardar mensaje")
            raise RuntimeError(f"Error al guardar mensaje: {ex}")
        finally:
            cursor.close()
            conn.close()

    def obtener_historial(self, conversation_request: ConversationRequest) -> List[Dict[str, Any]]:
        conversation_id = conversation_request.conversation_id
        conn = get_connection()
        cursor = conn.cursor()
        try:
            cursor.execute(
                """SELECT MensajeID, ConversacionID, ChatBot, Texto, FechaHora, ReglaActivadaID, MetadataJson
                   FROM dbo.HistorialMensajes
                   WHERE ConversacionID = ?
                   ORDER BY FechaHora ASC""",
                conversation_id
            )
            historial: List[Dict[str, Any]] = []
            for row in cursor.fetchall():
                # intentar parsear metadata JSON si existe
                metadata_val = None
                productos = []
                try:
                    if row.MetadataJson:
                        metadata_val = json.loads(row.MetadataJson)
                        # si metadata_val es dict con key productos, extraer
                        if isinstance(metadata_val, dict) and "productos" in metadata_val:
                            productos = metadata_val.get("productos") or []
                        else:
                            # si metadata_val es lista o dict, asignarlo a productos si tiene sentido
                            if isinstance(metadata_val, list):
                                productos = metadata_val
                            else:
                                productos = []
                except Exception:
                    # si no es JSON válido, dejar como string
                    metadata_val = row.MetadataJson
                    productos = []

                historial.append({
                    "id": row.MensajeID,
                    "conversationId": row.ConversacionID,
                    "role": "assistant" if bool(row.ChatBot) else "user",
                    "content": row.Texto or "",
                    "timestamp": row.FechaHora.isoformat() if row.FechaHora else None,
                    "isBot": bool(row.ChatBot),
                    "user_id": None,
                    "appliedRuleId": row.ReglaActivadaID,
                    "tipo": None,
                    "productos": productos,
                    "metadata": metadata_val,
                })
            return historial
        finally:
            cursor.close()
            conn.close()

    def listar_conversaciones(self, user_id: str) -> List[Dict[str, Any]]:
        conn = get_connection()
        cursor = conn.cursor()
        try:
            cursor.execute(
                "SELECT ConversacionID, UsuarioID, FechaInicio, FechaFin, Activo FROM dbo.HistorialConversaciones WHERE UsuarioID = ?",
                user_id
            )
            conversaciones: List[Dict[str, Any]] = []
            for row in cursor.fetchall():
                conv_id = row.ConversacionID
                cursor.execute(
                    """SELECT MensajeID, ConversacionID, ChatBot, Texto, FechaHora, ReglaActivadaID, MetadataJson
                       FROM dbo.HistorialMensajes
                       WHERE ConversacionID = ?
                       ORDER BY FechaHora ASC""",
                    conv_id
                )
                mensajes = []
                for m in cursor.fetchall():
                    metadata_val = None
                    productos = []
                    try:
                        if m.MetadataJson:
                            metadata_val = json.loads(m.MetadataJson)
                            if isinstance(metadata_val, dict) and "productos" in metadata_val:
                                productos = metadata_val.get("productos") or []
                            elif isinstance(metadata_val, list):
                                productos = metadata_val
                    except Exception:
                        metadata_val = m.MetadataJson
                        productos = []

                    mensajes.append({
                        "id": m.MensajeID,
                        "conversationId": m.ConversacionID,
                        "role": "assistant" if bool(m.ChatBot) else "user",
                        "content": m.Texto or "",
                        "timestamp": m.FechaHora.isoformat() if m.FechaHora else None,
                        "isBot": bool(m.ChatBot),
                        "user_id": user_id,
                        "appliedRuleId": m.ReglaActivadaID,
                        "productos": productos,
                        "metadata": metadata_val
                    })

                conversaciones.append({
                    "id": conv_id,
                    "conversation_id": conv_id,
                    "user_id": user_id,
                    "title": mensajes[0]["content"][:30] if mensajes else "Nueva conversación",
                    "language": "es",
                    "lastIntent": None,
                    "createdAt": row.FechaInicio.isoformat() if row.FechaInicio else None,
                    "updatedAt": row.FechaFin.isoformat() if row.FechaFin else None,
                    "isActive": bool(row.Activo),
                    "messages": mensajes,
                    "context": {"language": "es", "session_variables": {}}
                })

            return conversaciones
        finally:
            cursor.close()
            conn.close()
