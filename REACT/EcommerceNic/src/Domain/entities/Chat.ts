export interface Message {
  id: number;
  conversationId: string;
  role: 'user' | 'assistant' | 'system';
  isBot: boolean;
  content: string;
  timestamp: string; // ISO string mapping to FechaHora
  appliedRuleId?: number;
  intent?: string;
  metadata?: string; // Flexible JSON metadata for products, orders, etc.
}

export interface Conversation {
  id: string;
  userId?: string;
  language?: string;
  lastIntent?: string;
  cartId?: string;
  orderId?: string;
  startDate: string; // ISO string mapping to FechaInicio
  endDate?: string;  // ISO string mapping to FechaFin
  isActive: boolean;
  messages?: Message[];
}
