import { Platform } from 'react-native';

const API_URL =
  Platform.OS === 'android'
    ? 'http://10.0.2.2:8000'
    : 'http://127.0.0.1:8000';

export interface ChatbotResponse {
    texto: string;
    regla_id: number;
    tipo?: string;
    productos?: any[];
    metadata?: { productos?: any[] } | null; // 👈 agregado
}

export interface ChatMessagePayload {
  role: string;
  content: string;
  timestamp: string;
  user_id: string;
  isBot?: boolean;
  tipo?: string;
  productos?: any[];
  metadata?: { productos?: any[] } | null;
}

export interface ChatbotConversationResponse {
  id?: string;
  conversation_id?: string;
  userId?: string;
  title?: string;
  startDate?: string;
  updatedAt?: string;
  isActive?: boolean;
  messages?: any[];
}

class ChatbotRemoteDataSource {

  async sendMessage(message: string, conversationId?: string, userId?: string): Promise<ChatbotResponse> {
    const response = await fetch(`${API_URL}/api/chatbot/chat`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        mensaje: message,
        conversation_id: conversationId ?? null,
        user_id: userId ?? 'demo-user',
      }),
    });

    if (!response.ok) {
      const text = await response.text();
      console.log("STATUS:", response.status);
      console.log("RESPUESTA:", text);
      throw new Error(text);
    }

    return await response.json();
  }

  async getConversations(userId?: string): Promise<ChatbotConversationResponse[]> {
    try {
      const url = userId ? `${API_URL}/api/chatbot/users/${encodeURIComponent(userId)}/conversations` : `${API_URL}/api/chatbot/conversations`;
      const response = await fetch(url, { method: 'GET', headers: { 'Content-Type': 'application/json' } });
      if (!response.ok) return [];
      const data = await response.json();
      return Array.isArray(data) ? data : [];
    } catch (error) {
      console.log('Error loading conversations:', error);
      return [];
    }
  }

  async createConversation(userId: string, title = 'Nueva conversación'): Promise<ChatbotConversationResponse> {
    try {
      const response = await fetch(
        `${API_URL}/api/chatbot/conversations`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            userId,
            title,
          }),
        }
      );
      if (!response.ok) {
        return { id: `${Date.now()}`, userId, title, isActive: true };
      }
      const data = await response.json();
      return data ?? { id: `${Date.now()}`, userId, title, isActive: true };
    } catch (error) {
      console.log('Error creating conversation:', error);
      return { id: `${Date.now()}`, userId, title, isActive: true };
    }
  }

  async saveMessage(conversationId: string, payload: ChatMessagePayload): Promise<any> {
    try {
      const response = await fetch(`${API_URL}/api/chatbot/conversations/${encodeURIComponent(conversationId)}/messages`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload),
      });
      if (!response.ok) {
        const text = await response.text();
        console.log('saveMessage failed:', text);
        return null;
      }
      return response.json();
    } catch (error) {
      console.log('Error saving message:', error);
      return null;
    }
  }

  async getConversation(conversationId: string): Promise<ChatbotConversationResponse | null> {
    try {
      const response = await fetch(`${API_URL}/api/chatbot/conversations/${encodeURIComponent(conversationId)}/history`, {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' },
      });
      if (!response.ok) return null;
      return await response.json();
    } catch (error) {
      console.log('Error fetching conversation:', error);
      return null;
    }
  }

}

export const chatbotRemoteDataSource = new ChatbotRemoteDataSource();
