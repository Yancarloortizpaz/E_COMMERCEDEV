import React, { useState, useRef, useEffect } from 'react';
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  TextInput,
  TouchableOpacity,
  KeyboardAvoidingView,
  Platform,
  Animated,
} from 'react-native';
import { Feather, MaterialCommunityIcons } from '@expo/vector-icons';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import { QUICK_REPLIES } from '../constants';
import { Conversation, Message } from '../../../Domain/entities/Chat';
import { Product } from '../../../Domain/entities/Product';
import { ProductImage } from '../../components/ProductImage';
import { ContenedorGestoZoom } from '../../components/ContenedorGestoZoom';
import { getProductByIdUseCase } from '../../../di/DI';

interface ChatbotTabProps {
  messages: Message[];
  conversations?: Conversation[];
  activeConversationId?: string | null;
  onSelectConversation?: (conversationId: string) => void;
  onNewConversation?: () => void;
  sendMessage: (text: string) => void;
  isTyping?: boolean;
  products?: Product[];
  onAddProductToCart?: (product: any) => void;
  onSelectProduct?: (productId: string | number) => void;
}

type FeatherIconName = React.ComponentProps<typeof Feather>['name'];

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

const getQuickReplyIcon = (reply: string): FeatherIconName => {
  const text = reply.toLowerCase();
  if (text.includes('celular')) return 'smartphone';
  if (text.includes('consola')) return 'hard-drive';
  if (text.includes('hardware') || text.includes('pc')) return 'cpu';
  if (text.includes('audio') || text.includes('sonido')) return 'headphones';
  if (text.includes('monitor')) return 'monitor';
  return 'message-square';
};

export const ChatbotTab = ({ 
  messages, 
  conversations = [],
  activeConversationId,
  onSelectConversation,
  onNewConversation,
  sendMessage, 
  isTyping = false,
  products = [],
  onAddProductToCart,
  onSelectProduct,
}: ChatbotTabProps) => {
  const [chatMessage, setChatMessage] = useState('');
  const [imagenesResueltas, setImagenesResueltas] = useState<{ [id: string]: string }>({});
  const scrollViewRef = useRef<ScrollView>(null);
  const insets = useSafeAreaInsets();

  useEffect(() => {
    messages.forEach(msg => {
      if (msg.role !== 'user') {
        const lista = Array.isArray(msg.productos) ? msg.productos : (msg.metadata?.productos || []);
        lista.forEach(async (p: any) => {
          const realId = p.ProductID ?? p.ProductId ?? p.productID ?? p.productId ?? p.ProductVariableID ?? p.id;
          const idKey = realId ? String(realId) : null;
          const imgUrl = p.ProductImageURL ?? p.ProductImageUrl ?? p.productImageURL ?? p.productImageUrl ?? p.ProductoImagenUrl ?? p.image ?? p.imageUrl;

          if (idKey && (!imgUrl || !String(imgUrl).startsWith('http')) && !imagenesResueltas[idKey]) {
            try {
              const res = await getProductByIdUseCase.execute(idKey);
              const data = res?.data || res;
              const rawList: any[] = Array.isArray(data) ? data : (data?.productID ? [data] : []);
              if (rawList.length > 0) {
                const foundImg = rawList[0].productImageURL ?? rawList[0].ProductImageURL ?? rawList[0].image;
                if (foundImg && typeof foundImg === 'string' && foundImg.trim().length > 0) {
                  setImagenesResueltas(prev => ({ ...prev, [idKey]: foundImg.trim() }));
                }
              }
            } catch (e) {
              console.log('Error resolviendo imagen para chatbot:', e);
            }
          }
        });
      }
    });
  }, [messages]);

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

  return (
    <KeyboardAvoidingView
      style={styles.tabContent}
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      keyboardVerticalOffset={Platform.OS === 'ios' ? 90 : 0}
    >
      <View style={styles.chatHeader}>
        <View style={styles.chatBotIconHeader}>
          <MaterialCommunityIcons name="robot-outline" size={24} color="#4F46E5" />
        </View>
        <View style={styles.chatHeaderTextContainer}>
          <View style={styles.headerTitleRow}>
            <Text style={styles.chatHeaderTitle}>NICABOT IA</Text>
            <View style={styles.proBadge}>
              <Text style={styles.proBadgeText}>AGENTE</Text>
            </View>
          </View>
          <View style={styles.chatHeaderOnlineRow}>
            <View style={styles.onlineDot} />
            <Text style={styles.chatHeaderOnline}>Activo ahora</Text>
          </View>
        </View>
      </View>
      <View style={styles.headerDivider} />

      <ContenedorGestoZoom>
        <ScrollView
          ref={scrollViewRef}
          showsVerticalScrollIndicator={false}
          contentContainerStyle={styles.chatScrollPadding}
          onContentSizeChange={() => scrollViewRef.current?.scrollToEnd({ animated: true })}
        >
          <View style={styles.welcomeCard}>
            <View style={styles.welcomeBadgeRow}>
              <Feather name="zap" size={12} color="#4F46E5" />
              <Text style={styles.welcomeBadge}>Soporte Inteligente</Text>
            </View>
            <Text style={styles.welcomeTitle}>Asistente de Compras</Text>
            <Text style={styles.welcomeSubtitle}>
              ¡Hola, chele! Estoy listo para ayudarte a encontrar celulares, consolas, hardware de PC, audio y monitores. ¡Pregúntame lo que quieras!
            </Text>
            <View style={styles.welcomeFeatures}>
              <View style={styles.welcomeFeatureItem}>
                <Feather name="search" size={14} color="#4F46E5" style={styles.featureIcon} />
                <Text style={styles.welcomeFeatureText}>Búsqueda rápida por marca o tipo</Text>
              </View>
              <View style={styles.welcomeFeatureItem}>
                <Feather name="shopping-cart" size={14} color="#4F46E5" style={styles.featureIcon} />
                <Text style={styles.welcomeFeatureText}>Agrega productos directamente al carrito</Text>
              </View>
              <View style={styles.welcomeFeatureItem}>
                <Feather name="message-circle" size={14} color="#4F46E5" style={styles.featureIcon} />
                <Text style={styles.welcomeFeatureText}>Respuestas instantáneas con IA</Text>
              </View>
            </View>
          </View>

          {messages.map((msg) => {
            const isUser = msg.role === 'user';

            return (
              <AnimatedMessageBubble key={msg.id}>
                <View
                  style={[
                    styles.messageRow,
                    isUser ? styles.messageRowRight : styles.messageRowLeft,
                  ]}
                >
                  {!isUser && (
                    <View style={styles.messageBotAvatar}>
                      <MaterialCommunityIcons name="robot-outline" size={20} color="#4F46E5" />
                    </View>
                  )}

                  <View style={styles.bubbleContainer}>
                    {(() => {
                      const listaProductos = (!isUser && (Array.isArray(msg.productos) && msg.productos.length > 0))
                        ? msg.productos
                        : (!isUser && msg.metadata && typeof msg.metadata === 'object' && Array.isArray((msg.metadata as any).productos))
                        ? (msg.metadata as any).productos
                        : [];

                      const tieneProductos = listaProductos.length > 0;

                      return (
                        <View
                          style={[
                            styles.messageBubble,
                            isUser ? styles.messageBubbleUser : styles.messageBubbleBot,
                            tieneProductos && styles.messageBubbleConProductos,
                          ]}
                        >
                          {msg.tipo !== 'productos' && !tieneProductos && (
                            <Text
                              style={[
                                styles.messageText,
                                isUser ? styles.messageTextUser : styles.messageTextBot,
                              ]}
                            >
                              {msg.content}
                            </Text>
                          )}

                          {!isUser && tieneProductos && listaProductos.map((producto: any, idx: number) => {
                            const idVar = producto.ProductVariableID ?? producto.ProductVariableId ?? producto.ProductID ?? producto.ProductId ?? producto.productVariableId ?? producto.id ?? idx;
                            const realProductId = producto.ProductID ?? producto.ProductId ?? producto.productID ?? producto.productId ?? producto.ProductVariableID ?? producto.ProductVariableId ?? producto.id;
                            const nombre = producto.ProductName ?? producto.title ?? producto.name ?? 'Producto';
                            const subtitulo = producto.ProductVariableName ?? producto.subtitle ?? producto.productoDescripcion ?? '';
                            const moneda = producto.CurrencyISO ?? 'C$';
                            const precio = producto.ProductVariablePrice ?? producto.numericPrice ?? producto.price ?? 0;
                            let imagenUrl =
                              producto.ProductImageURL ??
                              producto.ProductImageUrl ??
                              producto.productImageURL ??
                              producto.productImageUrl ??
                              producto.ProductoImagenUrl ??
                              producto.productoImagenUrl ??
                              producto.product_image_url ??
                              producto.product_image ??
                              producto.image ??
                              producto.imageUrl;

                            const idKey = realProductId ? String(realProductId) : (idVar ? String(idVar) : null);
                            if (idKey && imagenesResueltas[idKey]) {
                              imagenUrl = imagenesResueltas[idKey];
                            } else if ((!imagenUrl || String(imagenUrl).trim().length === 0) && products && products.length > 0) {
                              const pMatch = products.find(p =>
                                String(p.productId) === String(realProductId) ||
                                String(p.productVariableId) === String(idVar) ||
                                String(p.id) === String(realProductId) ||
                                String(p.id) === String(idVar) ||
                                p.title.trim().toLowerCase() === nombre.trim().toLowerCase()
                              );
                              if (pMatch && pMatch.image) {
                                imagenUrl = pMatch.image;
                              }
                            }

                            return (
                              <View key={`${idVar}-${idx}`} style={styles.productCard}>
                                <TouchableOpacity 
                                  activeOpacity={0.8}
                                  onPress={() => realProductId && onSelectProduct?.(realProductId)}
                                >
                                  <ProductImage
                                    url={imagenUrl}
                                    style={styles.productCardImage}
                                    containerStyle={styles.productCardImageContainer}
                                  />
                                  <Text style={styles.productName}>{nombre}</Text>
                                  {!!subtitulo && <Text style={styles.productDescription}>{subtitulo}</Text>}
                                  <Text style={styles.productPrice}>
                                    {moneda} {precio}
                                  </Text>
                                </TouchableOpacity>

                                <View style={styles.productCardButtonsRow}>
                                  <TouchableOpacity
                                    style={styles.productDetailButton}
                                    onPress={() => realProductId && onSelectProduct?.(realProductId)}
                                    activeOpacity={0.8}
                                  >
                                    <View style={styles.productButtonContent}>
                                      <Feather name="search" size={14} color="#4F46E5" />
                                      <Text style={styles.productDetailButtonText}>Ver detalle</Text>
                                    </View>
                                  </TouchableOpacity>

                                  <TouchableOpacity
                                    style={styles.productAddButton}
                                    onPress={() =>
                                      onAddProductToCart?.({
                                        id: idVar.toString(),
                                        productVariableId: Number(idVar),
                                        title: nombre,
                                        name: nombre,
                                        numericPrice: Number(precio),
                                        price: Number(precio),
                                        image: imagenUrl,
                                        ProductoImagenUrl: imagenUrl,
                                        productoImagenUrl: imagenUrl,
                                      })
                                    }
                                    activeOpacity={0.8}
                                  >
                                    <View style={styles.productButtonContent}>
                                      <Feather name="shopping-cart" size={14} color="#FFFFFF" />
                                      <Text style={styles.productAddButtonText}>Agregar</Text>
                                    </View>
                                  </TouchableOpacity>
                                </View>
                              </View>
                            );
                          })}

                          {msg.timestamp && (
                            <Text
                              style={[
                                styles.messageTime,
                                isUser
                                  ? styles.messageTimeUser
                                  : styles.messageTimeBot,
                              ]}
                            >
                              {new Date(msg.timestamp).toLocaleTimeString([], {
                                hour: '2-digit',
                                minute: '2-digit',
                              })}
                            </Text>
                          )}
                        </View>
                      );
                    })()}
                  </View>
                </View>
              </AnimatedMessageBubble>
            );
          })}

          {isTyping && (
            <View style={[styles.messageRow, styles.messageRowLeft]}>
              <View style={styles.messageBotAvatar}>
                <MaterialCommunityIcons name="robot-outline" size={20} color="#4F46E5" />
              </View>
              <View style={[styles.messageBubble, styles.messageBubbleBot, styles.typingBubble]}>
                <Animated.View style={[styles.typingDot, { transform: [{ translateY: dot1.interpolate({ inputRange: [0, 1], outputRange: [0, -6] }) }] }]} />
                <Animated.View style={[styles.typingDot, { transform: [{ translateY: dot2.interpolate({ inputRange: [0, 1], outputRange: [0, -6] }) }] }]} />
                <Animated.View style={[styles.typingDot, { transform: [{ translateY: dot3.interpolate({ inputRange: [0, 1], outputRange: [0, -6] }) }] }]} />
              </View>
            </View>
          )}
        </ScrollView>
      </ContenedorGestoZoom>

      <View
        style={[
          styles.chatInputContainer,
          { paddingBottom: Platform.OS === 'ios' ? 12 : 12 + insets.bottom },
        ]}
      >
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
              <View style={styles.quickReplyPillContent}>
                <Feather name={getQuickReplyIcon(reply)} size={14} color="#4F46E5" style={styles.quickReplyIcon} />
                <Text style={styles.quickReplyText}>{reply}</Text>
              </View>
            </TouchableOpacity>
          ))}
        </ScrollView>

        <View style={styles.inputRow}>
          <TextInput
            style={styles.chatInput}
            placeholder="Pregúntale a la IA sobre un producto..."
            placeholderTextColor="#94A3B8"
            value={chatMessage}
            onChangeText={setChatMessage}
            onSubmitEditing={() => handleSend(chatMessage)}
            multiline
            textAlignVertical="top"
          />
          <TouchableOpacity style={styles.sendButton} onPress={() => handleSend(chatMessage)}>
            <Feather name="send" size={18} color="#FFFFFF" />
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
    paddingBottom: 0,
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
    backgroundColor: '#EEF2FF', 
    borderRadius: 14, 
    justifyContent: 'center', 
    alignItems: 'center', 
    borderWidth: 1,
    borderColor: '#E2E8F0',
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
    color: '#0F172A',
    letterSpacing: 0.5,
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
  chatHeaderOnlineRow: {
    flexDirection: 'row',
    alignItems: 'center',
    marginTop: 2,
  },
  onlineDot: {
    width: 6,
    height: 6,
    borderRadius: 3,
    backgroundColor: '#10B981',
    marginRight: 4,
  },
  chatHeaderOnline: { 
    fontSize: 12, 
    color: '#10B981', 
    fontWeight: '600',
  },
  headerDivider: { 
    height: 1, 
    backgroundColor: '#E2E8F0' 
  },
  chatScrollPadding: { 
    padding: 16, 
    paddingBottom: 20 
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
  welcomeBadgeRow: {
    flexDirection: 'row',
    alignItems: 'center',
    alignSelf: 'flex-start',
    marginBottom: 10,
  },
  welcomeBadge: {
    color: '#4F46E5',
    fontWeight: '700', 
    fontSize: 11,
    marginLeft: 4,
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
    flexDirection: 'row',
    alignItems: 'center',
    marginVertical: 4,
  },
  featureIcon: {
    marginRight: 8,
  },
  welcomeFeatureText: {
    fontSize: 12,
    color: '#475569',
    fontWeight: '600',
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
  messageBubbleConProductos: {
    maxWidth: 340,
    width: '100%',
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
    flexDirection: 'column',
    padding: 12,
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
  productCardImageContainer: {
    width: '100%',
    height: 140,
    borderRadius: 12,
    overflow: 'hidden',
    marginBottom: 8,
    backgroundColor: '#F8FAFC',
  },
  productCardImage: {
    width: '100%',
    height: '100%',
  },
  productName: {
    fontSize: 14,
    fontWeight: '700',
    color: '#0F172A',
    marginTop: 2,
  },
  productDescription: {
    fontSize: 12,
    color: '#64748B',
    marginTop: 2,
  },
  productPrice: {
    fontSize: 14,
    fontWeight: '800',
    color: '#4F46E5',
    marginTop: 4,
  },
  productCardButtonsRow: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    marginTop: 10,
    gap: 8,
  },
  productDetailButton: {
    flex: 1,
    backgroundColor: '#EEF2FF',
    borderWidth: 1,
    borderColor: '#C7D2FE',
    paddingVertical: 8,
    borderRadius: 8,
    alignItems: 'center',
    justifyContent: 'center',
  },
  productDetailButtonText: {
    color: '#4F46E5',
    fontSize: 12,
    fontWeight: '700',
    marginLeft: 4,
  },
  productAddButton: {
    flex: 1,
    backgroundColor: '#4F46E5',
    paddingVertical: 8,
    borderRadius: 8,
    alignItems: 'center',
    justifyContent: 'center',
    flexDirection: 'row',
  },
  productAddButtonText: {
    color: '#FFFFFF',
    fontSize: 12,
    fontWeight: '700',
    marginLeft: 4,
  },
  productButtonContent: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
  },
  chatInputContainer: { 
    backgroundColor: '#FFFFFF', 
    borderTopWidth: 1, 
    borderColor: '#E2E8F0', 
    paddingVertical: 12,
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
    flexDirection: 'row',
    alignItems: 'center',
  },
  quickReplyPillContent: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  quickReplyIcon: {
    marginRight: 6,
  },
  quickReplyText: { 
    color: '#4F46E5', 
    fontSize: 12, 
    fontWeight: '700' 
  },
  inputRow: { 
    flexDirection: 'row', 
    paddingHorizontal: 16, 
    alignItems: 'flex-end',
  },
  chatInput: { 
    flex: 1, 
    backgroundColor: '#F8FAFC', 
    borderWidth: 1, 
    borderColor: '#E2E8F0', 
    borderRadius: 24, 
    height: 88,
    paddingHorizontal: 18, 
    paddingTop: 12,
    paddingBottom: 12,
    fontSize: 14, 
    color: '#0F172A',
    textAlignVertical: 'top',
  },
  sendButton: { 
    width: 44, 
    height: 44, 
    backgroundColor: '#4F46E5', 
    borderRadius: 22, 
    justifyContent: 'center', 
    alignItems: 'center', 
    marginLeft: 10,
    marginBottom: 4,
  },
});