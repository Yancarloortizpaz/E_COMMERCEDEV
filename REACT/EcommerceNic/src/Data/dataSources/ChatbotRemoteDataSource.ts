import { Platform } from 'react-native';

const API_URL =
  Platform.OS === 'android'
    ? 'http://10.0.2.2:8000'
    : 'http://127.0.0.1:8000';

export interface ChatbotResponse {
  texto: string;
  regla_id: number | null;
}

class ChatbotRemoteDataSource {

  async sendMessage(message: string): Promise<ChatbotResponse> {

    console.log("URL:", `${API_URL}/api/chatbot/chat`);
    const response = await fetch(`${API_URL}/api/chatbot/chat`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        mensaje: message,
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

}

export const chatbotRemoteDataSource = new ChatbotRemoteDataSource();