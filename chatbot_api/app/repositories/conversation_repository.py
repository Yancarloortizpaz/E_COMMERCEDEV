import json
from pathlib import Path
from datetime import datetime, timezone
from uuid import uuid4


class ConversationRepository:
    def __init__(self, storage_path: str | None = None):
        base_path = Path(storage_path or Path(__file__).resolve().parents[1] / "data" / "conversations.json")
        base_path.parent.mkdir(parents=True, exist_ok=True)
        self.storage_path = base_path
        self._store = self._load_store()

    def _load_store(self):
        if not self.storage_path.exists():
            return {"conversations": []}

        try:
            with self.storage_path.open("r", encoding="utf-8") as fh:
                data = json.load(fh)
            return data if isinstance(data, dict) else {"conversations": []}
        except json.JSONDecodeError:
            return {"conversations": []}

    def _save_store(self):
        with self.storage_path.open("w", encoding="utf-8") as fh:
            json.dump(self._store, fh, ensure_ascii=False, indent=2)

    def _normalize_conversation_id(self, conversation_id):
        if conversation_id in (None, "", "default", "default_session"):
            return None
        return str(conversation_id)

    def _coerce_message(self, message):
        if hasattr(message, "model_dump"):
            return message.model_dump()
        if hasattr(message, "dict"):
            return message.dict()
        if isinstance(message, dict):
            return message
        raise TypeError("message must be a dict or Pydantic model")

    def _coerce_conversation(self, conversation):
        if hasattr(conversation, "model_dump"):
            return conversation.model_dump()
        if hasattr(conversation, "dict"):
            return conversation.dict()
        if isinstance(conversation, dict):
            return conversation
        raise TypeError("conversation must be a dict or Pydantic model")

    def _create_conversation_entry(self, conversation_data, conversation_id):
        now = datetime.now(timezone.utc).isoformat()
        title = conversation_data.get("title") or "Nueva conversación"
        return {
            "id": str(conversation_id),
            "conversation_id": str(conversation_id),
            "user_id": conversation_data.get("user_id") or "demo-user",
            "title": title,
            "language": (conversation_data.get("context") or {}).get("language", "es"),
            "lastIntent": None,
            "createdAt": now,
            "updatedAt": now,
            "messages": [],
            "context": conversation_data.get("context") or {"language": "es", "session_variables": {}},
        }

    def guardar_conversacion(self, conversation):
        conversation_data = self._coerce_conversation(conversation)
        user_id = conversation_data.get("user_id") or conversation_data.get("userId") or "demo-user"
        raw_conversation_id = self._normalize_conversation_id(conversation_data.get("conversation_id"))
        messages = conversation_data.get("messages") or []

        existing = None
        if raw_conversation_id:
            existing = next(
                (item for item in self._store.get("conversations", []) if item.get("id") == raw_conversation_id),
                None,
            )

        if existing is None:
            conversation_id = str(uuid4())
            entry = self._create_conversation_entry(conversation_data, conversation_id)
            entry["user_id"] = user_id
            self._store.setdefault("conversations", []).append(entry)
            existing = entry

        for message in messages:
            message_data = self._coerce_message(message)
            normalized = {
                "id": str(uuid4()),
                "conversationId": existing["id"],
                "role": (message_data.get("role") or "assistant").lower(),
                "content": message_data.get("content") or message_data.get("texto") or "",
                "timestamp": message_data.get("timestamp") or datetime.now(timezone.utc).isoformat(),
                "isBot": (message_data.get("role") or "").lower() in {"assistant", "bot", "chatbot"},
                "user_id": user_id,
                "appliedRuleId": message_data.get("appliedRuleId") or message_data.get("ReglaActivadaID"),
                "tipo": message_data.get("tipo"),
                "productos": message_data.get("productos") or [],
                "metadata": message_data.get("metadata"),
            }
            existing["messages"].append(normalized)

        existing["user_id"] = user_id
        existing["updatedAt"] = datetime.now(timezone.utc).isoformat()
        if existing["messages"] and existing.get("title") == "Nueva conversación":
            first_user = next((m for m in existing["messages"] if m.get("role") == "user"), None)
            if first_user:
                preview = first_user.get("content", "")[:30]
                existing["title"] = preview if preview else "Nueva conversación"

        self._save_store()
        return {"status": "ok", "id": existing["id"], "conversation_id": existing["id"]}

    def guardar_mensaje(self, conversation_id, message):
        conversation_id = self._normalize_conversation_id(conversation_id) or str(uuid4())
        payload = {
            "conversation_id": conversation_id,
            "user_id": getattr(message, "user_id", None) or "demo-user",
            "messages": [
                {
                    "role": getattr(message, "role", "assistant"),
                    "content": getattr(message, "content", ""),
                    "timestamp": getattr(message, "timestamp", datetime.now(timezone.utc).isoformat()),
                    "appliedRuleId": getattr(message, "appliedRuleId", None),
                    "tipo": getattr(message, "tipo", None),
                    "productos": getattr(message, "productos", []) or [],
                }
            ],
        }
        return self.guardar_conversacion(payload)

    def obtener_historial(self, conversation_request):
        if hasattr(conversation_request, "conversation_id"):
            raw_id = conversation_request.conversation_id
        elif isinstance(conversation_request, dict):
            raw_id = conversation_request.get("conversation_id")
        elif isinstance(conversation_request, (int, str)):
            raw_id = conversation_request
        else:
            raise TypeError("conversation_request must be a dict, str, int or Pydantic model")

        conversation_id = self._normalize_conversation_id(raw_id)
        if not conversation_id:
            return []

        entry = next((item for item in self._store.get("conversations", []) if item.get("id") == conversation_id), None)
        if not entry:
            return []

        historial = []
        for message in entry.get("messages", []):
            historial.append({
                "id": message.get("id"),
                "conversationId": entry["id"],
                "isBot": message.get("isBot", message.get("role") != "user"),
                "role": message.get("role", "assistant"),
                "content": message.get("content", ""),
                "timestamp": message.get("timestamp") or datetime.now(timezone.utc).isoformat(),
                "appliedRuleId": message.get("appliedRuleId"),
                "tipo": message.get("tipo"),
                "productos": message.get("productos") or [],
            })
        return historial

    def listar_conversaciones_usuario(self, user_id):
        conversaciones = []
        for item in self._store.get("conversations", []):
            if item.get("user_id") == user_id:
                conversaciones.append({
                    "id": item.get("id"),
                    "userId": item.get("user_id"),
                    "title": item.get("title") or "Nueva conversación",
                    "language": item.get("language") or "es",
                    "lastIntent": item.get("lastIntent") or "",
                    "isActive": True,
                    "updatedAt": item.get("updatedAt"),
                    "messages": item.get("messages", []),
                })
        return sorted(conversaciones, key=lambda item: item.get("updatedAt", ""), reverse=True)
