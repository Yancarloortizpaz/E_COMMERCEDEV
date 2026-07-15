import React, { useState, useRef, useEffect } from 'react';
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  TextInput,
  TouchableOpacity,
  Image,
  KeyboardAvoidingView,
  Platform,
  Animated,
  ActivityIndicator,
} from 'react-native';
import { QUICK_REPLIES } from '../constants';
import { Message } from '../../../Domain/entities/Chat';
import { ProductDto } from '../../../Domain/entities/ProductDto';

interface ChatbotTabProps {
  messages: Message[];
  sendMessage: (text: string) => void;
  isTyping?: boolean; // Para simular o recibir el estado de "escribiendo..." de la IA
  onAddProductToCart?: (product: any) => void; // Integración con el carrito
}

const AnimatedMessageBubble = ({ children }: { children: React.ReactNode }) => {
  const fadeAnim = useRef(new Animated.Value(0)).current;
  const slideAnim = useRef(new Animated.Value(12)).current;

  useEffect(() => {
    Animated.parallel([
      Animated.timing(fadeAnim, {
        toValue: 1,
        duration: 300,
        useNativeDriver: true,
      }),
      Animated.timing(slideAnim, {
        toValue: 0,
        duration: 300,
        useNativeDriver: true,
      }),
    ]).start();
  }, [fadeAnim, slideAnim]);

  return (
    <Animated.View style={{ opacity: fadeAnim, transform: [{ translateY: slideAnim }] }}>
      {children}
    </Animated.View>
  );
};

export const ChatbotTab = ({ 
  messages, 
  sendMessage, 
  isTyping = false,
  onAddProductToCart 
}: ChatbotTabProps) => {
  const [chatMessage, setChatMessage] = useState('');
  const scrollViewRef = useRef<ScrollView>(null);
  
  // Animación para el indicador de escritura (tres puntitos)
  const dot1 = useRef(new Animated.Value(0)).current;
  const dot2 = useRef(new Animated.Value(0)).current;
  const dot3 = useRef(new Animated.Value(0)).current;

  useEffect(() => {
    if (isTyping) {
      const animateDot = (dot: Animated.Value, delay: number) => {
        return Animated.loop(
          Animated.sequence([
            Animated.delay(delay),
            Animated.timing(dot, {
              toValue: 1,
              duration: 300,
              useNativeDriver: true,
            }),
            Animated.timing(dot, {
              toValue: 0,
              duration: 300,
              useNativeDriver: true,
            }),
          ])
        );
      };

      const animation = Animated.parallel([
        animateDot(dot1, 0),
        animateDot(dot2, 150),
        animateDot(dot3, 300),
      ]);

      animation.start();

      return () => animation.stop();
    }
  }, [isTyping, dot1, dot2, dot3]);

  const handleSend = (text: string) => {
    if (!text.trim()) return;
    sendMessage(text);
    setChatMessage('');
  };

  // Renderizar la tarjeta del producto si viene en el JSON de metadata
  const renderProductCard = (metadataString?: string) => {
    if (!metadataString) return null;
    try {
      const product: ProductDto = JSON.parse(metadataString);
      if (!product || !product.name) return null;

      // Obtener el precio y stock de la primera variable
      const mainVariable = product.variables && product.variables.length > 0 
        ? product.variables[0] 
        : { price: 0, currencyISO: 'C$', stock: 0 };

      const imageUrl = product.images && product.images.length > 0 
        ? product.images[0].url 
        : 'https://images.unsplash.com/photo-1531403009284-440f080d1e12?q=80&w=300';

      return (
        <View style={styles.productCard}>
          <Image source={{ uri: imageUrl }} style={styles.productImage} resizeMode="cover" />
          <View style={styles.productInfo}>
            <Text style={styles.productBrand}>{product.brandName || 'Exclusivo'}</Text>
            <Text style={styles.productName} numberOfLines={1}>{product.name}</Text>
            <Text style={styles.productPrice}>
              {mainVariable.currencyISO} {mainVariable.price.toLocaleString('es-NI')}
            </Text>
            <View style={styles.productStockRow}>
              <Text style={[styles.productStockText, mainVariable.stock > 0 ? styles.inStock : styles.outOfStock]}>
                {mainVariable.stock > 0 ? `En Stock (${mainVariable.stock})` : 'Agotado'}
              </Text>
            </View>
            <TouchableOpacity 
              style={[styles.productAddButton, mainVariable.stock === 0 && styles.disabledButton]}
              disabled={mainVariable.stock === 0}
              onPress={() => onAddProductToCart && onAddProductToCart(product)}
            >
              <Text style={styles.productAddButtonText}>🛒 Agregar al Carrito</Text>
            </TouchableOpacity>
          </View>
        </View>
      );
    } catch (e) {
      console.log('Error parseando metadata del producto en chatbot:', e);
      return null;
    }
  };

  return (
    <KeyboardAvoidingView
      style={styles.tabContent}
      behavior={Platform.OS === 'ios' ? 'padding' : undefined}
      keyboardVerticalOffset={Platform.OS === 'ios' ? 90 : 0}
    >
      {/* Header */}
      <View style={styles.chatHeader}>
        <View style={styles.chatBotIconHeader}>
          <Text style={styles.botEmojiLogo}>🤖</Text>
        </View>
        <View style={styles.chatHeaderTextContainer}>
          <View style={styles.headerTitleRow}>
            <Text style={styles.chatHeaderTitle}>NicaBot IA</Text>
            <View style={styles.proBadge}>
              <Text style={styles.proBadgeText}>AGENTE</Text>
            </View>
          </View>
          <Text style={styles.chatHeaderOnline}>● Activo ahora</Text>
        </View>
      </View>
      <View style={styles.headerDivider} />

      <ScrollView
        ref={scrollViewRef}
        showsVerticalScrollIndicator={false}
        contentContainerStyle={styles.chatScrollPadding}
        onContentSizeChange={() => scrollViewRef.current?.scrollToEnd({ animated: true })}
      >
        {/* Tarjeta de Bienvenida */}
        <View style={styles.welcomeCard}>
          <Text style={styles.welcomeBadge}>⚡ Soporte Inteligente</Text>
          <Text style={styles.welcomeTitle}>Asistente de Compras</Text>
          <Text style={styles.welcomeSubtitle}>
            ¡Hola, chele! Estoy listo para ayudarte a encontrar celulares, consolas, hardware de PC, audio y monitores. ¡Pregúntame lo que quieras!
          </Text>
          <View style={styles.welcomeFeatures}>
            <Text style={styles.welcomeFeatureItem}>🔍 Búsqueda rápida por marca o tipo</Text>
            <Text style={styles.welcomeFeatureItem}>🛒 Agrega productos directamente al carrito</Text>
            <Text style={styles.welcomeFeatureItem}>💬 Respuestas instantáneas con IA</Text>
          </View>
        </View>

        {/* Mensajes */}
        {messages.map((msg) => {
          const isUser = msg.role === 'user';
          return (
            <AnimatedMessageBubble key={msg.id}>
              <View 
                style={[
                  styles.messageRow, 
                  isUser ? styles.messageRowRight : styles.messageRowLeft
                ]}
              >
                {!isUser && (
                  <View style={styles.messageBotAvatar}>
                    <Text style={styles.botAvatarText}>🤖</Text>
                  </View>
                )}
                <View style={styles.bubbleContainer}>
                  <View 
                    style={[
                      styles.messageBubble, 
                      isUser ? styles.messageBubbleUser : styles.messageBubbleBot
                    ]}
                  >
                    <Text style={[styles.messageText, isUser ? styles.messageTextUser : styles.messageTextBot]}>
                      {msg.content}
                    </Text>
                    
                    {/* Timestamp sutil */}
                    {msg.timestamp && (
                      <Text style={[styles.messageTime, isUser ? styles.messageTimeUser : styles.messageTimeBot]}>
                        {new Date(msg.timestamp).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                      </Text>
                    )}
                  </View>

                  {/* Renderizar producto si contiene metadatos del producto */}
                  {!isUser && msg.metadata && renderProductCard(msg.metadata)}
                </View>
              </View>
            </AnimatedMessageBubble>
          );
        })}

        {/* Indicador de escritura animado */}
        {isTyping && (
          <View style={[styles.messageRow, styles.messageRowLeft]}>
            <View style={styles.messageBotAvatar}>
              <Text style={styles.botAvatarText}>🤖</Text>
            </View>
            <View style={[styles.messageBubble, styles.messageBubbleBot, styles.typingBubble]}>
              <Animated.View style={[styles.typingDot, { transform: [{ translateY: dot1.interpolate({ inputRange: [0, 1], outputRange: [0, -6] }) }] }]} />
              <Animated.View style={[styles.typingDot, { transform: [{ translateY: dot2.interpolate({ inputRange: [0, 1], outputRange: [0, -6] }) }] }]} />
              <Animated.View style={[styles.typingDot, { transform: [{ translateY: dot3.interpolate({ inputRange: [0, 1], outputRange: [0, -6] }) }] }]} />
            </View>
          </View>
        )}
      </ScrollView>

      {/* Input de Chat y Respuestas Rápidas */}
      <View style={styles.chatInputContainer}>
        {/* Quick Replies */}
        <ScrollView 
          horizontal 
          showsHorizontalScrollIndicator={false} 
          style={styles.quickRepliesScroll} 
          contentContainerStyle={styles.quickRepliesContainer}
        >
          {QUICK_REPLIES.map((reply, index) => (
            <TouchableOpacity 
              key={index} 
              style={styles.quickReplyPill} 
              onPress={() => handleSend(reply)}
            >
              <Text style={styles.quickReplyText}>{reply}</Text>
            </TouchableOpacity>
          ))}
        </ScrollView>

        {/* Fila de Input */}
        <View style={styles.inputRow}>
          <TextInput
            style={styles.chatInput}
            placeholder="Pregúntale a la IA sobre un producto..."
            placeholderTextColor="#94A3B8"
            value={chatMessage}
            onChangeText={setChatMessage}
            onSubmitEditing={() => handleSend(chatMessage)}
          />
          <TouchableOpacity style={styles.sendButton} onPress={() => handleSend(chatMessage)}>
            <Text style={styles.sendIcon}>➤</Text>
          </TouchableOpacity>
        </View>
      </View>
    </KeyboardAvoidingView>
  );
};

const styles = StyleSheet.create({
  tabContent: { 
    flex: 1, 
    backgroundColor: '#F8FAFC',
    paddingBottom: 68 
  },
  chatHeader: { 
    flexDirection: 'row', 
    alignItems: 'center', 
    paddingHorizontal: 20, 
    paddingTop: 16, 
    paddingBottom: 16, 
    backgroundColor: '#FFFFFF',
    ...Platform.select({
      ios: {
        shadowColor: '#0F172A',
        shadowOffset: { width: 0, height: 2 },
        shadowOpacity: 0.05,
        shadowRadius: 3,
      },
      android: {
        elevation: 2,
      },
    }),
  },
  chatBotIconHeader: { 
    width: 44, 
    height: 44, 
    backgroundColor: '#EEF2F6', 
    borderRadius: 14, 
    justifyContent: 'center', 
    alignItems: 'center', 
    borderWidth: 1.5,
    borderColor: '#E2E8F0',
  },
  botEmojiLogo: {
    fontSize: 22,
  },
  chatHeaderTextContainer: { 
    marginLeft: 12,
    flex: 1,
  },
  headerTitleRow: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  chatHeaderTitle: { 
    fontSize: 16, 
    fontWeight: '800', 
    color: '#0F172A' 
  },
  proBadge: {
    backgroundColor: '#EEF2FF',
    paddingHorizontal: 6,
    paddingVertical: 2,
    borderRadius: 6,
    marginLeft: 8,
    borderWidth: 1,
    borderColor: '#E0E7FF',
  },
  proBadgeText: {
    fontSize: 9,
    color: '#4F46E5',
    fontWeight: '800',
  },
  chatHeaderOnline: { 
    fontSize: 12, 
    color: '#10B981', 
    fontWeight: '600',
    marginTop: 1,
  },
  headerDivider: { 
    height: 1, 
    backgroundColor: '#E2E8F0' 
  },
  chatScrollPadding: { 
    padding: 16, 
    paddingBottom: 40 
  },
  welcomeCard: { 
    backgroundColor: '#FFFFFF', 
    borderRadius: 20, 
    padding: 20, 
    marginBottom: 24, 
    borderWidth: 1,
    borderColor: '#E2E8F0',
    ...Platform.select({
      ios: {
        shadowColor: '#0F172A',
        shadowOffset: { width: 0, height: 4 },
        shadowOpacity: 0.03,
        shadowRadius: 10,
      },
      android: {
        elevation: 1,
      },
    }),
  },
  welcomeBadge: { 
    backgroundColor: '#EEF2FF', 
    color: '#4F46E5',
    fontWeight: '700', 
    fontSize: 11,
    paddingHorizontal: 10,
    paddingVertical: 4,
    borderRadius: 8,
    alignSelf: 'flex-start',
    marginBottom: 10,
    overflow: 'hidden',
    borderWidth: 0.5,
    borderColor: '#C7D2FE',
  },
  welcomeTitle: { 
    fontSize: 20, 
    fontWeight: '800', 
    color: '#0F172A', 
    marginBottom: 8,
  },
  welcomeSubtitle: { 
    fontSize: 14, 
    color: '#475569', 
    lineHeight: 20,
    marginBottom: 14,
  },
  welcomeFeatures: {
    backgroundColor: '#F8FAFC',
    borderRadius: 12,
    padding: 12,
    width: '100%',
  },
  welcomeFeatureItem: {
    fontSize: 12,
    color: '#475569',
    fontWeight: '600',
    marginVertical: 4,
  },
  messageRow: { 
    flexDirection: 'row', 
    marginBottom: 20, 
    maxWidth: '85%',
  },
  messageRowLeft: { 
    alignSelf: 'flex-start' 
  },
  messageRowRight: { 
    alignSelf: 'flex-end', 
    flexDirection: 'row-reverse' 
  },
  messageBotAvatar: { 
    width: 36, 
    height: 36, 
    borderRadius: 12, 
    justifyContent: 'center', 
    alignItems: 'center', 
    marginRight: 10, 
    backgroundColor: '#FFFFFF',
    borderWidth: 1,
    borderColor: '#E2E8F0',
  },
  botAvatarText: {
    fontSize: 18,
  },
  bubbleContainer: {
    flex: 1,
    flexDirection: 'column',
  },
  messageBubble: { 
    paddingHorizontal: 16, 
    paddingVertical: 12, 
    borderRadius: 20, 
    alignSelf: 'flex-start',
  },
  messageBubbleBot: { 
    backgroundColor: '#FFFFFF', 
    borderTopLeftRadius: 4,
    borderWidth: 1,
    borderColor: '#E2E8F0',
  },
  messageBubbleUser: { 
    backgroundColor: '#4F46E5', 
    borderTopRightRadius: 4,
  },
  messageText: { 
    fontSize: 14, 
    lineHeight: 21, 
    fontWeight: '500'
  },
  messageTextBot: { 
    color: '#1E293B' 
  },
  messageTextUser: { 
    color: '#FFFFFF' 
  },
  messageTime: {
    fontSize: 9,
    marginTop: 4,
    alignSelf: 'flex-end',
    fontWeight: '600',
  },
  messageTimeBot: {
    color: '#94A3B8',
  },
  messageTimeUser: {
    color: '#E0E7FF',
  },
  typingBubble: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    paddingVertical: 14,
    paddingHorizontal: 18,
    height: 42,
  },
  typingDot: {
    width: 6,
    height: 6,
    borderRadius: 3,
    backgroundColor: '#94A3B8',
    marginHorizontal: 3,
  },
  productCard: {
    backgroundColor: '#FFFFFF',
    borderWidth: 1,
    borderColor: '#E2E8F0',
    borderRadius: 16,
    marginTop: 10,
    flexDirection: 'row',
    padding: 12,
    alignItems: 'center',
    ...Platform.select({
      ios: {
        shadowColor: '#0F172A',
        shadowOffset: { width: 0, height: 4 },
        shadowOpacity: 0.04,
        shadowRadius: 8,
      },
      android: {
        elevation: 1,
      },
    }),
  },
  productImage: {
    width: 80,
    height: 80,
    borderRadius: 10,
    backgroundColor: '#F1F5F9',
  },
  productInfo: {
    flex: 1,
    marginLeft: 12,
    justifyContent: 'center',
  },
  productBrand: {
    fontSize: 10,
    fontWeight: '700',
    color: '#64748B',
    textTransform: 'uppercase',
  },
  productName: {
    fontSize: 14,
    fontWeight: '700',
    color: '#0F172A',
    marginTop: 2,
  },
  productPrice: {
    fontSize: 14,
    fontWeight: '800',
    color: '#4F46E5',
    marginTop: 3,
  },
  productStockRow: {
    flexDirection: 'row',
    alignItems: 'center',
    marginTop: 4,
  },
  productStockText: {
    fontSize: 10,
    fontWeight: '700',
  },
  inStock: {
    color: '#10B981',
  },
  outOfStock: {
    color: '#EF4444',
  },
  productAddButton: {
    backgroundColor: '#4F46E5',
    paddingVertical: 6,
    paddingHorizontal: 12,
    borderRadius: 8,
    marginTop: 8,
    alignItems: 'center',
  },
  disabledButton: {
    backgroundColor: '#CBD5E1',
  },
  productAddButtonText: {
    color: '#FFFFFF',
    fontSize: 11,
    fontWeight: '700',
  },
  chatInputContainer: { 
    backgroundColor: '#FFFFFF', 
    borderTopWidth: 1, 
    borderColor: '#E2E8F0', 
    paddingVertical: 12,
    ...Platform.select({
      ios: {
        paddingBottom: 20,
      },
    }),
  },
  quickRepliesScroll: { 
    maxHeight: 38, 
    marginBottom: 10 
  },
  quickRepliesContainer: { 
    paddingHorizontal: 16 
  },
  quickReplyPill: { 
    backgroundColor: '#F1F5F9', 
    borderRadius: 20, 
    paddingHorizontal: 14, 
    paddingVertical: 6, 
    marginRight: 8, 
    height: 32, 
    justifyContent: 'center',
    borderWidth: 0.5,
    borderColor: '#E2E8F0',
  },
  quickReplyText: { 
    color: '#4F46E5', 
    fontSize: 12, 
    fontWeight: '700' 
  },
  inputRow: { 
    flexDirection: 'row', 
    paddingHorizontal: 16, 
    alignItems: 'center' 
  },
  chatInput: { 
    flex: 1, 
    backgroundColor: '#F8FAFC', 
    borderWidth: 1, 
    borderColor: '#E2E8F0', 
    borderRadius: 24, 
    height: 48, 
    paddingHorizontal: 18, 
    fontSize: 14, 
    color: '#0F172A' 
  },
  sendButton: { 
    width: 44, 
    height: 44, 
    backgroundColor: '#4F46E5', 
    borderRadius: 22, 
    justifyContent: 'center', 
    alignItems: 'center', 
    marginLeft: 10 
  },
  sendIcon: { 
    color: '#FFFFFF', 
    fontSize: 16, 
    fontWeight: '900', 
    transform: [{ rotate: '45deg' }] 
  },
});
