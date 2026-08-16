import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import { Feather } from '@expo/vector-icons';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import { COLORES, ESTILOS_SOMBRA } from '../theme/theme';

export type TabNombre = 'home' | 'cart' | 'chatbot' | 'pedidos' | 'nosotros';

interface Props {
  pestañaActual: TabNombre;
  alSeleccionarPestaña: (pestaña: TabNombre) => void;
  totalElementosCarrito?: number;
}

export const BottomTabBar: React.FC<Props> = ({
  pestañaActual,
  alSeleccionarPestaña,
  totalElementosCarrito = 0,
}) => {
  const insets = useSafeAreaInsets();

  return (
    <View
      style={[
        styles.barraNavegacion,
        ESTILOS_SOMBRA.navegacion,
        {
          height: 60 + insets.bottom,
          paddingBottom: insets.bottom,
        },
      ]}
    >
      <TouchableOpacity
        style={styles.itemTab}
        onPress={() => alSeleccionarPestaña('home')}
        activeOpacity={0.7}
      >
        <Feather
          name="home"
          size={22}
          color={pestañaActual === 'home' ? COLORES.primario : COLORES.textoSecundario}
        />
        <Text style={[styles.textoTab, pestañaActual === 'home' && styles.textoTabActivo]}>
          Inicio
        </Text>
      </TouchableOpacity>

      <TouchableOpacity
        style={styles.itemTab}
        onPress={() => alSeleccionarPestaña('chatbot')}
        activeOpacity={0.7}
      >
        <View
          style={[
            styles.contenedorIconoBot,
            pestañaActual === 'chatbot' && styles.contenedorIconoBotActivo,
          ]}
        >
          <Feather
            name="message-circle"
            size={20}
            color={pestañaActual === 'chatbot' ? COLORES.primario : COLORES.textoSecundario}
          />
        </View>
        <Text style={[styles.textoTab, pestañaActual === 'chatbot' && styles.textoTabActivo]}>
          Chatbot
        </Text>
      </TouchableOpacity>

      <TouchableOpacity
        style={styles.itemTab}
        onPress={() => alSeleccionarPestaña('cart')}
        activeOpacity={0.7}
      >
        <View style={styles.contenedorCarrito}>
          <Feather
            name="shopping-cart"
            size={22}
            color={pestañaActual === 'cart' ? COLORES.primario : COLORES.textoSecundario}
          />
          {totalElementosCarrito > 0 && (
            <View style={styles.insigniaContador}>
              <Text style={styles.textoInsignia}>{totalElementosCarrito}</Text>
            </View>
          )}
        </View>
        <Text style={[styles.textoTab, pestañaActual === 'cart' && styles.textoTabActivo]}>
          Carrito
        </Text>
      </TouchableOpacity>

      <TouchableOpacity
        style={styles.itemTab}
        onPress={() => alSeleccionarPestaña('pedidos')}
        activeOpacity={0.7}
      >
        <Feather
          name="package"
          size={22}
          color={pestañaActual === 'pedidos' ? COLORES.primario : COLORES.textoSecundario}
        />
        <Text style={[styles.textoTab, pestañaActual === 'pedidos' && styles.textoTabActivo]}>
          Pedidos
        </Text>
      </TouchableOpacity>

      <TouchableOpacity
        style={styles.itemTab}
        onPress={() => alSeleccionarPestaña('nosotros')}
        activeOpacity={0.7}
      >
        <Feather
          name="info"
          size={22}
          color={pestañaActual === 'nosotros' ? COLORES.primario : COLORES.textoSecundario}
        />
        <Text style={[styles.textoTab, pestañaActual === 'nosotros' && styles.textoTabActivo]}>
          Nosotros
        </Text>
      </TouchableOpacity>
    </View>
  );
};

const styles = StyleSheet.create({
  barraNavegacion: {
    position: 'absolute',
    bottom: 0,
    left: 0,
    right: 0,
    flexDirection: 'row',
    justifyContent: 'space-around',
    alignItems: 'center',
    borderTopWidth: 1,
    borderTopColor: COLORES.borde,
    backgroundColor: COLORES.blanco,
  },
  itemTab: {
    alignItems: 'center',
    justifyContent: 'center',
  },
  textoTab: {
    fontSize: 12,
    color: COLORES.textoSecundario,
  },
  textoTabActivo: {
    color: COLORES.primario,
    fontWeight: '600',
  },
  contenedorIconoBot: {
    padding: 4,
    borderRadius: 20,
    backgroundColor: COLORES.borde,
  },
  contenedorIconoBotActivo: {
    backgroundColor: COLORES.primarioSuave,
  },
  contenedorCarrito: {
    position: 'relative',
  },
  insigniaContador: {
    position: 'absolute',
    top: -4,
    right: -8,
    backgroundColor: COLORES.peligro,
    borderRadius: 10,
    minWidth: 18,
    height: 18,
    justifyContent: 'center',
    alignItems: 'center',
    paddingHorizontal: 4,
  },
  textoInsignia: {
    color: COLORES.blanco,
    fontSize: 10,
    fontWeight: 'bold',
  },
});