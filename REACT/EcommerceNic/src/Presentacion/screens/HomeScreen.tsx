import React, { useState } from 'react';
import { SafeAreaView, StyleSheet, Platform, Alert } from 'react-native';
import { User } from '../../Domain/entities/User';
import { formatCurrency } from './constants';
import { CatalogTab } from './components/CatalogTab';
import { CartTab } from './components/CartTab';
import { ChatbotTab } from './components/ChatbotTab';
import { NosotrosTab } from './components/NosotrosTab';
import { PaymentModal } from './components/PaymentModal';
import { SidebarHistorial } from '../components/SidebarHistorial';
import { BottomTabBar, TabNombre } from '../components/BottomTabBar';
import { useCatalog } from '../hooks/useCatalog';
import { useCart } from '../hooks/useCart';
import { useChatbot } from '../hooks/useChatbot';
import { COLORES } from '../theme/theme';

import { CustomAlertModal } from '../components/CustomAlertModal';

interface Props {
  onLogout: () => void;
  user: User;
}

export const HomeScreen = ({ onLogout, user }: Props) => {
  const [currentTab, setCurrentTab] = useState<TabNombre>('home');
  const [isPaymentModalVisible, setPaymentModalVisible] = useState<boolean>(false);
  const [alertaAgregarProducto, setAlertaAgregarProducto] = useState<{
    visible: boolean;
    producto?: any;
  }>({ visible: false });

  const [alertaResultado, setAlertaResultado] = useState<{
    visible: boolean;
    tipo: 'exito' | 'advertencia';
    titulo: string;
    mensaje: string;
  }>({
    visible: false,
    tipo: 'exito',
    titulo: '',
    mensaje: '',
  });

  // Custom Hooks Especializados
  const { productos } = useCatalog();
  const cart = useCart(user, productos, currentTab);
  const chatbot = useChatbot(user, cart.agregarProductoAlCarrito);

  // Manejo de Confirmación de Pago
  const handlePaymentSuccess = async (method: string) => {
    setPaymentModalVisible(false);
    await cart.vaciarCarrito();

    Alert.alert(
      '📦 ¡Pedido Procesado con Éxito!',
      `Monto: ${formatCurrency(cart.totalPago)}\nMétodo: ${method}\n\nPronto nos pondremos en contacto para coordinar la entrega. ¡Gracias por comprar en Nic Store!`,
      [{ text: '¡Excelente!', onPress: () => setCurrentTab('home') }]
    );
  };

  // Manejo de Cierre de Sesión Multiplataforma
  const handleLogout = () => {
    const ejecutarSalida = () => {
      onLogout();
    };

    if (Platform.OS === 'web') {
      const confirmarWeb = window.confirm('¿Estás seguro que querés salir?');
      if (confirmarWeb) {
        ejecutarSalida();
      }
      return;
    }

    Alert.alert(
      'Cerrar Sesión',
      '¿Estás seguro que querés salir, bro?',
      [
        { text: 'Cancelar', style: 'cancel' },
        { text: 'Sí, Salir', style: 'destructive', onPress: ejecutarSalida },
      ]
    );
  };

  return (
    <SafeAreaView style={styles.container}>
      {/* Modal Elegante 1: Preguntar Confirmación de Adición */}
      <CustomAlertModal
        visible={alertaAgregarProducto.visible}
        tipo="exito"
        titulo="¿Agregar producto al carrito?"
        mensaje={`¿Deseas agregar "${alertaAgregarProducto.producto?.name ?? alertaAgregarProducto.producto?.title ?? 'este producto'}" a tu carrito de compras?`}
        textoConfirmar="Sí, Agregar"
        textoCancelar="Cancelar"
        alConfirmar={async () => {
          const prodTarget = alertaAgregarProducto.producto;
          setAlertaAgregarProducto({ visible: false });
          if (prodTarget) {
            try {
              await cart.agregarProductoAlCarrito(prodTarget);
              console.log("✅ Producto agregado con éxito a la API C#");
              setAlertaResultado({
                visible: true,
                tipo: 'exito',
                titulo: '🛒 ¡Producto Agregado!',
                mensaje: `"${prodTarget.name ?? prodTarget.title ?? 'El producto'}" fue añadido correctamente a tu carrito de compras.`,
              });
            } catch (err: any) {
              console.error("❌ Error al agregar producto:", err);
              setAlertaResultado({
                visible: true,
                tipo: 'advertencia',
                titulo: '⚠️ Error al Agregar',
                mensaje: err.message || 'No se pudo registrar el producto en el carrito.',
              });
            }
          }
        }}
        alCancelar={() => setAlertaAgregarProducto({ visible: false })}
        alCerrar={() => setAlertaAgregarProducto({ visible: false })}
      />

      {/* Modal Elegante 2: Notificar Resultado de la Operación */}
      <CustomAlertModal
        visible={alertaResultado.visible}
        tipo={alertaResultado.tipo}
        titulo={alertaResultado.titulo}
        mensaje={alertaResultado.mensaje}
        textoConfirmar={alertaResultado.tipo === 'exito' ? 'Ver Carrito' : 'Entendido'}
        alConfirmar={() => {
          setAlertaResultado({ ...alertaResultado, visible: false });
          if (alertaResultado.tipo === 'exito') {
            setCurrentTab('cart');
          }
        }}
        alCerrar={() => setAlertaResultado({ ...alertaResultado, visible: false })}
      />

      {/* Vistas según la Pestaña Seleccionada */}
      {currentTab === 'home' && (
        <CatalogTab
          products={productos}
          cartQuantities={cart.cantidadesCarrito}
          addUnit={cart.agregarUnidad}
          removeUnit={cart.removerUnidad}
          setCurrentTab={setCurrentTab}
          totalItemsInCart={cart.totalElementosCarrito}
        />
      )}

      {currentTab === 'cart' && (
        <CartTab
          cartItems={cart.elementosCarritoBd}
          products={productos}
          extraProducts={cart.productosCarritoChatbot}
          cartQuantities={cart.cantidadesCarrito}
          pendingCartActions={cart.accionesPendientesCarrito}
          addUnit={cart.agregarUnidad}
          removeUnit={cart.removerUnidad}
          deleteFromCart={cart.eliminarDelCarrito}
          setCurrentTab={setCurrentTab}
          openPaymentModal={() => setPaymentModalVisible(true)}
          totalItemsInCart={cart.totalElementosCarrito}
        />
      )}

      {currentTab === 'chatbot' && (
        <SidebarHistorial
          conversations={chatbot.conversaciones}
          activeConversationId={chatbot.idConversacionActiva}
          onSelectConversation={chatbot.seleccionarConversacion}
          onNewConversation={chatbot.crearNuevaConversacion}
        >
          <ChatbotTab
            messages={chatbot.mensajes}
            conversations={chatbot.conversaciones}
            activeConversationId={chatbot.idConversacionActiva}
            onSelectConversation={chatbot.seleccionarConversacion}
            onNewConversation={chatbot.crearNuevaConversacion}
            sendMessage={chatbot.enviarMensaje}
            isTyping={chatbot.estaEscribiendo}
            onAddProductToCart={(producto) => {
              setAlertaAgregarProducto({
                visible: true,
                producto,
              });
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
        totalPayment={cart.totalPago}
        onPaymentSuccess={handlePaymentSuccess}
      />

      {/* Componente Modular de Navegación Inferior */}
      <BottomTabBar
        pestañaActual={currentTab}
        alSeleccionarPestaña={setCurrentTab}
        totalElementosCarrito={cart.totalElementosCarrito}
      />
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: COLORES.fondo,
    paddingBottom: 60,
  },
});