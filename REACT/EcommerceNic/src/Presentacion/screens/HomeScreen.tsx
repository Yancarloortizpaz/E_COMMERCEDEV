import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  StyleSheet,
  TouchableOpacity,
  SafeAreaView,
  Platform,
  Alert,
} from 'react-native';

import { formatCurrency } from './constants';
import { Product } from '../../Domain/entities/Product';
import { getProductsUseCase, sendChatMessageUseCase } from '../../di/DI';
import { CatalogTab } from './components/CatalogTab';
import { CartTab } from './components/CartTab';
import { ChatbotTab } from './components/ChatbotTab';
import { NosotrosTab } from './components/NosotrosTab';
import { PaymentModal } from './components/PaymentModal';
import { Conversation, Message } from '../../Domain/entities/Chat';
import { User } from '../../Domain/entities/User';
import { normalizeMetadata } from '../hooks/normalizeMetadata';
import { SidebarHistorial } from '../components/SidebarHistorial';

interface Props {
  onLogout: () => void;
  user: User;
}

export const HomeScreen = ({ onLogout, user }: Props) => {
  const [currentTab, setCurrentTab] = useState<'home' | 'cart' | 'chatbot' | 'nosotros'>('home');
  const [products, setProducts] = useState<Product[]>([]);
  const [cartQuantities, setCartQuantities] = useState<{ [key: string]: number }>({});
  const [chatbotCartProducts, setChatbotCartProducts] = useState<Product[]>([]);
  const [conversations, setConversations] = useState<Conversation[]>([]);
  const [activeConversationId, setActiveConversationId] = useState<string | null>(null);
  const [isPaymentModalVisible, setPaymentModalVisible] = useState(false);
  const [isTyping, setIsTyping] = useState(false);

  const [currentUser] = useState<{ email: string }>({ email: user?.email ?? "demo-user" });
  const [hasLoadedHistory, setHasLoadedHistory] = useState(false);

  const [messages, setMessages] = useState<Message[]>([
    {
      id: 1,
      conversationId: 'default',
      role: 'assistant',
      isBot: true,
      content: '¿Y entonces chele qué andás buscando hoy? ¡Preguntame sobre celulares, consolas, hardware, audio o monitores!',
      timestamp: new Date().toISOString(),
      user_id: "chatbot",   // 👈 obligatorio
    },
  ]);

  // Cargar catálogo de productos
  const loadProducts = async () => {
    try {
      const result = await getProductsUseCase.execute();
      setProducts(result);
    } catch (error) {
      console.log('Error al cargar productos:', error);
    }
  };

  useEffect(() => {
    loadProducts();
  }, []);

  // Cargar conversaciones del usuario
  const loadConversations = async (userId?: string) => {
    try {
      const result = await sendChatMessageUseCase.getConversations(userId);
      const mapped: Conversation[] = result.map((conversation: any) => ({
        id: conversation.id.toString(),
        userId: conversation.userId,
        title: conversation.title ?? 'Nueva conversación',
        startDate: conversation.startDate ?? new Date().toISOString(),
        updatedAt: conversation.updatedAt ?? new Date().toISOString(),
        isActive: conversation.isActive ?? true,
        messages: Array.isArray(conversation.messages)
          ? conversation.messages.map((msg: any) => ({
              id: msg.id ?? Date.now(),
              conversationId: conversation.id.toString(),
              role: msg.role ?? 'assistant',
              isBot: msg.isBot ?? msg.role !== 'user',
              content: msg.content ?? '',
              timestamp: msg.timestamp ?? new Date().toISOString(),
              tipo: msg.tipo,
              productos: msg.productos ?? [],
              metadata: normalizeMetadata(msg.metadata), // 👈 aquí
            }))
          : [],
      }));

      setConversations(mapped);

      if (mapped.length > 0) {
        setActiveConversationId(mapped[0].id);
        setMessages(mapped[0].messages ?? []);
      }
    } catch (error) {
      console.log('Error al cargar conversaciones:', error);
    }
  };

  useEffect(() => {
    if (!hasLoadedHistory) {
      loadConversations(currentUser.email);
      setHasLoadedHistory(true);
    }
  }, [currentUser.email, hasLoadedHistory]);

  // Lógica del Carrito
  const addUnit = (id: string) => setCartQuantities(prev => ({ ...prev, [id]: (prev[id] || 0) + 1 }));

  const addProductToCart = (product: Partial<Product> & { id: string | number; name?: string; title?: string; subtitle?: string; price?: number; numericPrice?: number; brand?: string; image?: string }) => {
    const normalizedId = product.id.toString();
    const existingProduct = products.find((item) => item.id === normalizedId);

    if (existingProduct) {
      addUnit(normalizedId);
    } else {
      const fallbackProduct: Product = {
        id: normalizedId,
        title: product.title ?? product.name ?? 'Producto agregado',
        subtitle: product.subtitle ?? 'Agregado desde el chatbot',
        numericPrice: Number(product.numericPrice ?? product.price ?? 0),
        tag: '',
        brand: product.brand ?? 'NIC STORE',
        category: 'chatbot',
        image: product.image ?? 'https://placehold.co/300x300/png?text=Producto',
      };

      setCartQuantities(prev => ({ ...prev, [normalizedId]: (prev[normalizedId] || 0) + 1 }));
      setChatbotCartProducts(prev =>
        prev.some((item) => item.id === normalizedId) ? prev : [...prev, fallbackProduct]
      );
    }

    setCurrentTab('cart');
  };

  const removeUnit = (id: string) => {
    setCartQuantities(prev => {
      const currentQty = prev[id] || 0;
      if (currentQty <= 1) {
        const updated = { ...prev };
        delete updated[id];
        return updated;
      }
      return { ...prev, [id]: currentQty - 1 };
    });
  };

  const deleteFromCart = (id: string) => {
    setCartQuantities(prev => {
      const updated = { ...prev };
      delete updated[id];
      return updated;
    });
  };

  const clearCart = () => {
    setCartQuantities({});
  };

  const totalItemsInCart = Object.values(cartQuantities).reduce((acc, qty) => acc + qty, 0);
  const subtotal = [...products, ...chatbotCartProducts].reduce((acc, p) => acc + (p.numericPrice * (cartQuantities[p.id] || 0)), 0);
  const shippingCost = subtotal > 0 ? 350 : 0;
  const totalPayment = subtotal + shippingCost;

  // Confirmación de pago
  const handlePaymentSuccess = (method: string) => {
    setPaymentModalVisible(false);

    Alert.alert(
      '📦 ¡Pedido Procesado con Éxito!',
      `Monto: ${formatCurrency(totalPayment)}\nMétodo: ${method}\n\nPronto nos pondremos en contacto para coordinar la entrega. ¡Gracias por comprar en Nic Store!`,
      [{ text: '¡Excelente!', onPress: () => { clearCart(); setCurrentTab('home'); } }]
    );
  };

  // Crear conversación manual desde la UI
  const createNewConversation = async () => {
    try {
      const conversation = await sendChatMessageUseCase.createConversation(currentUser.email, 'Nueva conversación');
      const realId = (conversation.conversation_id ?? conversation.id ?? Date.now()).toString();

      const newConversation: Conversation = {
        id: realId,
        userId: currentUser.email,
        title: conversation.title ?? 'Nueva conversación',
        startDate: conversation.startDate ?? new Date().toISOString(),
        updatedAt: conversation.updatedAt ?? new Date().toISOString(),
        isActive: true,
        messages: [],
      };

      setConversations(prev => [newConversation, ...prev]);
      setActiveConversationId(realId);
      setMessages([]);
      setHasLoadedHistory(true);
    } catch (error) {
      console.log('Error al crear conversación:', error);
      Alert.alert('Error', 'No se pudo crear una nueva conversación.');
    }
  };

  // Persistir un mensaje individual en la base de datos
  const persistMessage = async (conversationId: string, message: Message) => {
    try {
      await sendChatMessageUseCase.saveMessage(conversationId, {
        role: message.role,
        content: message.content,
        timestamp: message.timestamp,
        user_id: currentUser.email,   // 👈 ahora sí enviamos el usuario logueado
        isBot: message.isBot,
        tipo: message.tipo,
        productos: message.productos ?? [],
        metadata: normalizeMetadata(message.metadata), // opcional
      });
    } catch (error) {
      console.log('Error al guardar mensaje en el backend:', error);
    }
  };

  // Flujo principal de envío de mensajes al Chatbot
  const handleSendMessage = async (text: string) => {
    const cleanText = text.replace(/📱 |🎮 |💻 |🎧 |🔥 /g, '').trim();
    if (!cleanText) return;

    let currentConvId = activeConversationId;

    // 1. SI NO EXISTE CONVERSACIÓN VÁLIDA O ES 'default', CREAMOS UNA EN EL BACKEND PRIMERO
    if (!currentConvId || currentConvId === 'default') {
      try {
        const titleSnippet = cleanText.length > 25 ? `${cleanText.slice(0, 25)}...` : cleanText;
        const newConv = await sendChatMessageUseCase.createConversation(currentUser.email, titleSnippet);
        
        currentConvId = (newConv.conversation_id ?? newConv.id ?? Date.now()).toString();
        setActiveConversationId(currentConvId);

        const newConvObj: Conversation = {
          id: currentConvId,
          userId: currentUser.email,   // 👈 aquí también usamos el usuario real
          title: titleSnippet,
          startDate: new Date().toISOString(),
          updatedAt: new Date().toISOString(),
          isActive: true,
          messages: [],
        };
        setConversations(prev => [newConvObj, ...prev]);
        setHasLoadedHistory(true);
      } catch (error) {
        console.log('Error al inicializar sesión de chat:', error);
        Alert.alert('Error', 'No se pudo establecer conexión para iniciar la conversación.');
        return;
      }
    }

    // 2. AGREGAR MENSAJE DEL USUARIO AL ESTADO LOCAL
    const newUserMsg: Message = { 
      id: Date.now(), 
      conversationId: currentConvId,
      role: 'user', 
      isBot: false,
      content: cleanText,
      timestamp: new Date().toISOString(),
      user_id: currentUser.email,   // 👈 obligatorio en el modelo
    };

    setMessages(prev => [...prev, newUserMsg]);
    setIsTyping(true);
    setHasLoadedHistory(true);
    await persistMessage(currentConvId, newUserMsg);

    // 4. CONSUMIR LA RESPUESTA DEL CHATBOT
    try {
      const response = await sendChatMessageUseCase.execute(cleanText, currentConvId, currentUser.email);
      
      const botMessage: Message = {
        id: Date.now() + 1,
        conversationId: currentConvId,
        role: 'assistant',
        isBot: true,
        content: response.texto,
        timestamp: new Date().toISOString(),
        tipo: response.tipo,
        productos: response.productos ?? [],
        metadata: normalizeMetadata(response.metadata), // 👈 agrega esto
        user_id: "chatbot",   // 👈 puedes marcarlo fijo para el bot
      };

      setMessages(prev => [...prev, botMessage]);
      await persistMessage(currentConvId, botMessage);
      setHasLoadedHistory(true);

      // Actualizar metadatos de la lista de conversaciones
      setConversations(prev => prev.map(item => 
        item.id === currentConvId 
          ? { 
              ...item, 
              updatedAt: new Date().toISOString(), 
              title: item.title && item.title !== 'Nueva conversación' ? item.title : cleanText.slice(0, 25) 
            } 
          : item
      ));

    } catch (error: any) {
      console.log('ERROR EN RESPUESTA DE CHATBOT:', error);

      const fallbackMessage: Message = {
        id: Date.now() + 1,
        conversationId: currentConvId,
        role: 'assistant',
        isBot: true,
        content: '❌ Error al comunicarse con el servidor.',
        timestamp: new Date().toISOString(),
        user_id: "chatbot",
      };
      setMessages(prev => [...prev, fallbackMessage]);
      await persistMessage(currentConvId, fallbackMessage);
    } finally {
      setIsTyping(false);
    }
  };

  // Cierre de sesión
  const handleLogout = () => {
    const ejecutarSalida = () => {
      clearCart();
      setCurrentTab('home');
      setActiveConversationId(null);
      setMessages([
        { 
          id: 1, 
          conversationId: 'default',
          role: 'assistant', 
          isBot: true,
          content: '¿Y entonces chele qué andás buscando hoy?', 
          timestamp: new Date().toISOString(),
          user_id: "chatbot",   // 👈 obligatorio
        },
      ]);
      setHasLoadedHistory(false);
      onLogout();
    };

    if (Platform.OS === 'web') {
      const confirmarWeb = window.confirm('¿Estás seguro que querés salir?');
      if (confirmarWeb) {
        console.log("🚪 Cerrando sesión en entorno Web...");
        ejecutarSalida();
      }
      return;
    }

    Alert.alert(
      'Cerrar Sesión',
      '¿Estás seguro que querés salir, bro?',
      [
        { text: 'Cancelar', style: 'cancel' },
        {
          text: 'Sí, Salir',
          style: 'destructive',
          onPress: ejecutarSalida,
        },
      ]
    );
  };

  return (
    <SafeAreaView style={styles.container}>
      {/* Vistas según el Tab seleccionado */}
      {currentTab === 'home' && (
        <CatalogTab
          products={products}
          cartQuantities={cartQuantities}
          addUnit={addUnit}
          removeUnit={removeUnit}
          setCurrentTab={setCurrentTab}
          totalItemsInCart={totalItemsInCart}
        />
      )}

      {currentTab === 'cart' && (
        <CartTab
          products={products}
          extraProducts={chatbotCartProducts}
          cartQuantities={cartQuantities}
          addUnit={addUnit}
          removeUnit={removeUnit}
          deleteFromCart={deleteFromCart}
          setCurrentTab={setCurrentTab}
          openPaymentModal={() => setPaymentModalVisible(true)}
          totalItemsInCart={totalItemsInCart}
        />
      )}

      {/* AQUÍ ESTÁ EL CAMBIO CLAVE: SidebarHistorial ahora envuelve a ChatbotTab */}
      
        <SidebarHistorial
          conversations={conversations}
          activeConversationId={activeConversationId}
          onSelectConversation={(id) => {
            const selected = conversations.find((c) => c.id === id);
            if (selected) {
              setActiveConversationId(id);
              const normalizedMessages = (selected.messages ?? []).map(m => ({
                ...m,
                metadata: normalizeMetadata(m.metadata),
              }));
              setMessages(normalizedMessages);
              setHasLoadedHistory(true);
            }
          }}
        >
          <ChatbotTab
            messages={messages}
            conversations={conversations}
            activeConversationId={activeConversationId}
            onSelectConversation={(conversationId) => {
              const selected = conversations.find((item) => item.id === conversationId);
              if (selected) {
                setActiveConversationId(conversationId);
                const normalizedMessages = (selected.messages ?? []).map(m => ({
                  ...m,
                  metadata: normalizeMetadata(m.metadata),
                }));
                setMessages(normalizedMessages);
                setHasLoadedHistory(true);
              }
            }}
            onNewConversation={createNewConversation}
            sendMessage={handleSendMessage}
            isTyping={isTyping}
            onAddProductToCart={(product) => {
              addProductToCart(product);
              Alert.alert(
                '🛒 ¡Carrito Actualizado!',
                `Agregaste "${product.name ?? product.title}" al carrito desde el Chatbot.`,
                [
                  { text: 'Ver Carrito', onPress: () => setCurrentTab('cart') },
                  { text: 'Seguir Chateando', style: 'cancel' }
                ]
              );
            }}
          />
        </SidebarHistorial>
      )}

      {currentTab === 'nosotros' && (
        <NosotrosTab handleLogout={handleLogout} />
      )}

      {/* Modal de Pago */}
      <PaymentModal
        isVisible={isPaymentModalVisible}
        onClose={() => setPaymentModalVisible(false)}
        totalPayment={totalPayment}
        onPaymentSuccess={handlePaymentSuccess}
      />

      {/* Navegación Inferior */}
      <View style={styles.bottomTabBar}>
        <TouchableOpacity style={styles.tabItem} onPress={() => setCurrentTab('home')}>
          <Text style={[styles.tabIconBase, currentTab === 'home' && styles.tabIconActive]}>🏠</Text>
          <Text style={[styles.tabText, currentTab === 'home' && styles.tabTextActive]}>Inicio</Text>
        </TouchableOpacity>

        <TouchableOpacity style={styles.tabItem} onPress={() => setCurrentTab('chatbot')}>
          <View style={[styles.tabIconWrapper, currentTab === 'chatbot' && styles.tabIconWrapperActive]}>
            <Text style={[styles.tabIconBase, currentTab === 'chatbot' && styles.tabIconActiveBot]}>💬</Text>
          </View>
          <Text style={[styles.tabText, currentTab === 'chatbot' && styles.tabTextActive]}>Chatbot</Text>
        </TouchableOpacity>

        <TouchableOpacity style={styles.tabItem} onPress={() => setCurrentTab('cart')}>
          <Text style={[styles.tabIconBase, currentTab === 'cart' && styles.tabIconActive]}>🛒</Text>
          <Text style={[styles.tabText, currentTab === 'cart' && styles.tabTextActive]}>Carrito</Text>
        </TouchableOpacity>

        <TouchableOpacity style={styles.tabItem} onPress={() => setCurrentTab('nosotros')}>
          <Text style={[styles.tabIconBase, currentTab === 'nosotros' && styles.tabIconActive]}>ℹ️</Text>
          <Text style={[styles.tabText, currentTab === 'nosotros' && styles.tabTextActive]}>Nosotros</Text>
        </TouchableOpacity>
      </View>
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F8FAFC',
    paddingBottom: 70, // 👈 deja espacio para la barra inferior
  },
  bottomTabBar: {
    position: 'absolute',
    bottom: 0,
    left: 0,
    right: 0,
    height: 60, // 👈 define altura fija
    flexDirection: 'row',
    justifyContent: 'space-around',
    alignItems: 'center',
    borderTopWidth: 1,
    borderTopColor: '#E2E8F0',
    backgroundColor: '#fff',
    shadowColor: '#000',
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 3, // 👈 sombra suave en Android
  },
  tabItem: {
    alignItems: 'center',
  },
  tabIconBase: {
    fontSize: 22,
    color: '#64748B',
  },
  tabIconActive: {
    color: '#3B82F6',
  },
  tabIconActiveBot: {
    color: '#3B82F6',
  },
  tabText: {
    fontSize: 12,
    color: '#64748B',
  },
  tabTextActive: {
    color: '#3B82F6',
    fontWeight: '600',
  },
  tabIconWrapper: {
    padding: 6,
    borderRadius: 20,
    backgroundColor: '#E2E8F0',
  },
  tabIconWrapperActive: {
    backgroundColor: '#DBEAFE',
  },
});