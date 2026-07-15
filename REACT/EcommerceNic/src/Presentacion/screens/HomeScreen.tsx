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

import { formatCurrency, getBotResponse } from './constants';
import { Product } from '../../Domain/entities/Product';
import { getProductsUseCase } from '../../di/DI';
import { CatalogTab } from './components/CatalogTab';
import { CartTab } from './components/CartTab';
import { ChatbotTab } from './components/ChatbotTab';
import { NosotrosTab } from './components/NosotrosTab';
import { PaymentModal } from './components/PaymentModal';

interface Props {
  onLogout: () => void;
}

import { Message } from '../../Domain/entities/Chat';

export const HomeScreen = ({ onLogout }: Props) => {
  const [currentTab, setCurrentTab] = useState<'home' | 'cart' | 'chatbot' | 'nosotros'>('home');
  const [products, setProducts] = useState<Product[]>([]);
  const [cartQuantities, setCartQuantities] = useState<{ [key: string]: number }>({});
  const [isPaymentModalVisible, setPaymentModalVisible] = useState(false);
  const [isTyping, setIsTyping] = useState(false);
  
  // CORRECCIÓN: id ahora es numérico
  const [messages, setMessages] = useState<Message[]>([
    {
      id: 1, 
      conversationId: 'default',
      role: 'assistant',
      isBot: true,
      content: '¿Y entonces chele qué andás buscando hoy? ¡Preguntame sobre celulares, consolas, hardware, audio o monitores!',
      timestamp: new Date().toISOString(),
    },
  ]);

  const loadProducts = async () => {
    try {
      const result = await getProductsUseCase.execute();
      setProducts(result);
    } catch (error) {
      console.log('Error loading products in user view:', error);
    }
  };

  useEffect(() => {
    loadProducts();
  }, []);

  // Cart actions
  const addUnit = (id: string) => setCartQuantities(prev => ({ ...prev, [id]: (prev[id] || 0) + 1 }));
  
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
  const subtotal = products.reduce((acc, p) => acc + (p.numericPrice * (cartQuantities[p.id] || 0)), 0);
  const shippingCost = subtotal > 0 ? 350 : 0;
  const totalPayment = subtotal + shippingCost;

  // Payment Confirmation Flow
  const handlePaymentSuccess = (method: string) => {
    setPaymentModalVisible(false);

    Alert.alert(
      '📦 ¡Pedido Procesado con Éxito!',
      `Monto: ${formatCurrency(totalPayment)}\nMétodo: ${method}\n\nPronto nos pondremos en contacto para coordinar la entrega. ¡Gracias por comprar en Nic Store!`,
      [{ text: '¡Excelente!', onPress: () => { clearCart(); setCurrentTab('home'); } }]
    );
  };

  // Chatbot Send Message Flow
  const handleSendMessage = (text: string) => {
    const cleanText = text.replace(/📱 |🎮 |💻 |🎧 |🔥 /g, '').trim();
    if (!cleanText) return;

    // CORRECCIÓN: Usando Date.now() directo como número para el id
    const newUserMsg: Message = { 
      id: Date.now(), 
      conversationId: 'default',
      role: 'user', 
      isBot: false,
      content: cleanText,
      timestamp: new Date().toISOString()
    };
    setMessages(prev => [...prev, newUserMsg]);
    setIsTyping(true);

    setTimeout(() => {
      const botReply = getBotResponse(cleanText);
      let metadata: string | undefined = undefined;

      const t = cleanText.toLowerCase();
      if (t.includes('rtx') || t.includes('nvidia') || t.includes('4080') || t.includes('gráfica') || t.includes('grafica') || t.includes('gpu')) {
        metadata = JSON.stringify({
          id: '1', 
          name: "Gráfica RTX 4080",
          description: "16GB GDDR6X Ultra Potencia",
          categoryId: 1,
          categoryName: "hardware",
          subCategoryId: 1,
          subCategoryName: "Componentes",
          segmentId: 1,
          segmentName: "Gaming",
          brandId: 1,
          brandName: "NVIDIA",
          providerId: 1,
          providerName: "Nvidia Nicaragua",
          images: [{ id: 1, url: "https://images.unsplash.com/photo-1610563166150-b34df4f3bcd6?q=80&w=800", description: "RTX 4080", isPrimary: true }],
          variables: [{ id: 1, productId: 1, value: "16GB GDDR6X", price: 42500, currencyId: 1, currencyISO: "C$", stock: 5 }]
        });
      } else if (t.includes('iphone') || t.includes('apple') || t.includes('15 pro')) {
        metadata = JSON.stringify({
          id: '4', 
          name: "iPhone 15 Pro Max",
          description: "256GB Titanio Natural",
          categoryId: 2,
          categoryName: "phones",
          subCategoryId: 2,
          subCategoryName: "Celulares",
          segmentId: 2,
          segmentName: "Premium",
          brandId: 2,
          brandName: "Apple",
          providerId: 2,
          providerName: "Apple Nicaragua",
          images: [{ id: 2, url: "https://images.unsplash.com/photo-1510557880182-3d4d3cba35a5?q=80&w=800", description: "iPhone 15 Pro Max", isPrimary: true }],
          variables: [{ id: 4, productId: 4, value: "256GB Titanio Natural", price: 39800, currencyId: 1, currencyISO: "C$", stock: 3 }]
        });
      }

      setMessages(prev => [
        ...prev,
        { 
          // CORRECCIÓN: ID numérico autogenerado
          id: Date.now() + 1, 
          conversationId: 'default',
          role: 'assistant', 
          isBot: true,
          content: botReply, 
          timestamp: new Date().toISOString(),
          metadata: metadata
        },
      ]);
      setIsTyping(false);
    }, 1000);
  };

  // Logout flow
  const handleLogout = () => {
    const ejecutarSalida = () => {
      clearCart();
      setCurrentTab('home');
      // CORRECCIÓN: Restaurando ID numérico inicial
      setMessages([
        { 
          id: 1, 
          conversationId: 'default',
          role: 'assistant', 
          isBot: true,
          content: '¿Y entonces chele qué andás buscando hoy?', 
          timestamp: new Date().toISOString()
        },
      ]);
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
      
      {/* Tab Screen Render */}
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
        <ChatbotTab
          messages={messages}
          sendMessage={handleSendMessage}
          isTyping={isTyping}
          onAddProductToCart={(product) => {
            addUnit(product.id.toString());
            Alert.alert(
              '🛒 ¡Carrito Actualizado!',
              `Agregaste "${product.name}" al carrito desde el Chatbot.`,
              [{ text: 'Ver Carrito', onPress: () => setCurrentTab('cart') }, { text: 'Seguir Chateando', style: 'cancel' }]
            );
          }}
        />
      )}

      {currentTab === 'nosotros' && (
        <NosotrosTab
          handleLogout={handleLogout}
        />
      )}

      {/* Shared Payment Modal */}
      <PaymentModal
        isVisible={isPaymentModalVisible}
        onClose={() => setPaymentModalVisible(false)}
        totalPayment={totalPayment}
        onPaymentSuccess={handlePaymentSuccess}
      />

      {/* Bottom Tab Bar Navigation */}
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
  container: { flex: 1, backgroundColor: '#F8FAFC' },
  bottomTabBar: {
    position: 'absolute',
    bottom: 0,
    left: 0,
    right: 0,
    height: 68,
    backgroundColor: '#FFFFFF',
    flexDirection: 'row',
    justifyContent: 'space-around',
    alignItems: 'center',
    borderTopWidth: 1,
    borderColor: '#E2E8F0',
    paddingBottom: Platform.OS === 'ios' ? 14 : 4,
  },
  tabItem: { alignItems: 'center', justifyContent: 'center' },
  tabIconBase: { fontSize: 20, opacity: 0.6 },
  tabIconActive: { opacity: 1 },
  tabIconActiveBot: { opacity: 1, color: '#3B82F6' },
  tabIconWrapper: { width: 40, height: 40, borderRadius: 20, justifyContent: 'center', alignItems: 'center' },
  tabIconWrapperActive: { backgroundColor: '#EFF6FF' },
  tabText: { fontSize: 11, color: '#94A3B8', fontWeight: '600', marginTop: 2 },
  tabTextActive: { color: '#3B82F6', fontWeight: '800' },
});