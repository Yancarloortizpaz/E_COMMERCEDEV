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
import { CartItem } from '../../Domain/entities/CartItem';
import {
  getProductsUseCase,
  sendChatMessageUseCase,
  getCartByUserUseCase,
  addToCartUseCase,
  updateCartQuantityUseCase,
  deleteCartItemUseCase,
} from '../../di/DI';
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
  const [dbCartItems, setDbCartItems] = useState<CartItem[]>([]);
  const [conversations, setConversations] = useState<Conversation[]>([]);
  const [activeConversationId, setActiveConversationId] = useState<string | null>(null);
  const [isPaymentModalVisible, setPaymentModalVisible] = useState(false);
  const [isTyping, setIsTyping] = useState(false);

  const currentUserEmail = user?.email ?? "demo-user";
  const currentUser = { email: currentUserEmail };
  const numericUserId = parseInt(user?.id || '1', 10) || 1;
  const [hasLoadedHistory, setHasLoadedHistory] = useState(false);

  const [messages, setMessages] = useState<Message[]>([
    {
      id: 1,
      conversationId: 'default',
      role: 'assistant',
      isBot: true,
      content: '¿Y entonces chele qué andás buscando hoy? ¡Preguntame sobre celulares, consolas, hardware, audio o monitores!',
      timestamp: new Date().toISOString(),
      user_id: "chatbot",
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

  // Cargar carrito activo desde C# API (CartDetailsController)
  const loadCartFromDb = async () => {
    try {
      const items = await getCartByUserUseCase.execute(numericUserId);
      setDbCartItems(items);

      const newQuantities: { [key: string]: number } = {};
      const extraProducts: Product[] = [];

      items.forEach(item => {
        const pId = String(item.varianteId || item.productoId);
        newQuantities[pId] = item.cantidad;

        extraProducts.push({
          id: pId,
          title: item.productoNombre || 'Producto',
          subtitle: item.varianteEspecificacion || item.productoDescripcion || '',
          numericPrice: item.precioUnitario ?? 0,
          tag: '',
          brand: 'NIC STORE',
          category: 'cart',
          image: item.productoImagenUrl || 'https://placehold.co/300x300/png?text=Producto',
        });
      });

      setCartQuantities(newQuantities);
      setChatbotCartProducts(extraProducts);
    } catch (error) {
      console.log('Error al cargar carrito desde C# API:', error);
    }
  };

  useEffect(() => {
    if (numericUserId) {
      loadCartFromDb();
    }
  }, [numericUserId, currentTab]);

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
              metadata: normalizeMetadata(msg.metadata),
            }))
          : [],
      }));

      setConversations(mapped);
    } catch (error) {
      console.log('Error al cargar conversaciones:', error);
    }
  };

  useEffect(() => {
    if (numericUserId) {
      loadConversations(String(numericUserId));
    }
  }, [numericUserId]);

  // Lógica del Carrito conectada a C# API CartDetailsController
  const addUnit = async (id: string) => {
    const targetId = parseInt(id, 10);
    const existingItem = dbCartItems.find(item => item.varianteId === targetId || item.productoId === targetId);

    if (existingItem) {
      await updateCartQuantityUseCase.execute(existingItem.detalleCarritoId, existingItem.cantidad + 1, numericUserId);
    } else {
      const matchedProduct = products.find(p => p.id === id);
      const varId = matchedProduct?.productVariableId || targetId;
      await addToCartUseCase.execute(numericUserId, varId, 1);
    }
    await loadCartFromDb();
  };

  const addProductToCart = async (product: Partial<Product> & { id: string | number; name?: string; title?: string; subtitle?: string; price?: number; numericPrice?: number; brand?: string; image?: string }) => {
    const normalizedId = product.id.toString();
    await addUnit(normalizedId);
    setCurrentTab('cart');
  };

  const removeUnit = async (id: string) => {
    const targetId = parseInt(id, 10);
    const existingItem = dbCartItems.find(item => item.varianteId === targetId || item.productoId === targetId);

    if (existingItem) {
      if (existingItem.cantidad > 1) {
        await updateCartQuantityUseCase.execute(existingItem.detalleCarritoId, existingItem.cantidad - 1, numericUserId);
      } else {
        await deleteCartItemUseCase.execute(existingItem.detalleCarritoId, numericUserId);
      }
      await loadCartFromDb();
    }
  };

  const deleteFromCart = async (id: string) => {
    const targetId = parseInt(id, 10);
    const existingItem = dbCartItems.find(item => item.varianteId === targetId || item.productoId === targetId);

    if (existingItem) {
      await deleteCartItemUseCase.execute(existingItem.detalleCarritoId, numericUserId);
      await loadCartFromDb();
    }
  };

  const clearCart = async () => {
    for (const item of dbCartItems) {
      await deleteCartItemUseCase.execute(item.detalleCarritoId, numericUserId);
    }
    await loadCartFromDb();
  };

  const totalItemsInCart = Object.values(cartQuantities).reduce((acc, qty) => acc + qty, 0);
  const subtotal = [...products, ...chatbotCartProducts].reduce((acc, p) => acc + (p.numericPrice * (cartQuantities[p.id] || 0)), 0);
  const shippingCost = subtotal > 0 ? 350 : 0;
  const totalPayment = subtotal + shippingCost;

  // Confirmación de pago
  const handlePaymentSuccess = async (method: string) => {
    setPaymentModalVisible(false);
    await clearCart();

    Alert.alert(
      '📦 ¡Pedido Procesado con Éxito!',
      `Monto: ${formatCurrency(totalPayment)}\nMétodo: ${method}\n\nPronto nos pondremos en contacto para coordinar la entrega. ¡Gracias por comprar en Nic Store!`,
      [{ text: '¡Excelente!', onPress: () => { setCurrentTab('home'); } }]
    );
  };

  // Crear conversación manual desde la UI (dejar pantalla limpia en blanco)
  const createNewConversation = () => {
    setActiveConversationId(null);
    setMessages([
      {
        id: Date.now(),
        conversationId: 'default',
        role: 'assistant',
        isBot: true,
        content: '¿Y entonces chele qué andás buscando hoy? ¡Preguntame sobre celulares, consolas, hardware, audio o monitores!',
        timestamp: new Date().toISOString(),
        user_id: "chatbot",
      },
    ]);
  };

  // Persistir un mensaje individual en la base de datos
  const persistMessage = async (conversationId: string, message: Message) => {
    try {
      await sendChatMessageUseCase.saveMessage(conversationId, {
        role: message.role,
        content: message.content,
        timestamp: message.timestamp,
        user_id: currentUser.email,
        isBot: message.isBot,
        tipo: message.tipo,
        productos: message.productos ?? [],
        metadata: normalizeMetadata(message.metadata),
      });
    } catch (error) {
      console.log('Error al guardar mensaje en el backend:', error);
    }
  };

  // Flujo principal de envío de mensajes al Chatbot (Intacto y Conversacional)
  const handleSendMessage = async (text: string) => {
    const cleanText = text.replace(/📱 |🎮 |💻 |🎧 |🔥 /g, '').trim();
    if (!cleanText) return;

    let currentConvId = activeConversationId;

    if (!currentConvId || currentConvId === 'default') {
      try {
        const titleSnippet = cleanText.length > 25 ? `${cleanText.slice(0, 25)}...` : cleanText;
        const newConv = await sendChatMessageUseCase.createConversation(String(numericUserId), titleSnippet);
        
        currentConvId = (newConv.conversation_id ?? newConv.id ?? Date.now()).toString();
        setActiveConversationId(currentConvId);

        const newConvObj: Conversation = {
          id: currentConvId,
          userId: currentUser.email,
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

    const newUserMsg: Message = { 
      id: Date.now(), 
      conversationId: currentConvId,
      role: 'user', 
      isBot: false,
      content: cleanText,
      timestamp: new Date().toISOString(),
      user_id: currentUser.email,
    };

    setMessages(prev => [...prev, newUserMsg]);
    setIsTyping(true);
    setHasLoadedHistory(true);
    await persistMessage(currentConvId, newUserMsg);

    try {
      const response = await sendChatMessageUseCase.execute(cleanText, currentConvId, String(numericUserId));
      
      const botMessage: Message = {
        id: Date.now() + 1,
        conversationId: currentConvId,
        role: 'assistant',
        isBot: true,
        content: response.texto,
        timestamp: new Date().toISOString(),
        tipo: response.tipo,
        productos: response.productos ?? [],
        metadata: normalizeMetadata(response.metadata),
        user_id: "chatbot",
      };

      setMessages(prev => [...prev, botMessage]);
      await persistMessage(currentConvId, botMessage);
      setHasLoadedHistory(true);

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
      setCartQuantities({});
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
          user_id: "chatbot",
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

      {currentTab === 'chatbot' && (
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
            }
          }}
          onNewConversation={createNewConversation}
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
                `Agregaste "${product.name ?? product.title}" al carrito.`,
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
    paddingBottom: 70,
  },
  bottomTabBar: {
    position: 'absolute',
    bottom: 0,
    left: 0,
    right: 0,
    height: 60,
    flexDirection: 'row',
    justifyContent: 'space-around',
    alignItems: 'center',
    borderTopWidth: 1,
    borderTopColor: '#E2E8F0',
    backgroundColor: '#fff',
    shadowColor: '#000',
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 3,
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