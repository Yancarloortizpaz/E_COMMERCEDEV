import React from 'react';
import { View, Text, Image, TouchableOpacity, StyleSheet } from 'react-native';
import { Product } from '../../../Domain/entities/Product';

interface Props {
  product: Partial<Product> & { id: string | number };
  onAddToCart: (product: any) => void;
}

export const ProductCard = ({ product, onAddToCart }: Props) => {
  return (
    <View style={styles.card}>
      <Image
        source={{ uri: product.image ?? 'https://placehold.co/300x300/png?text=Producto' }}
        style={styles.image}
      />
      <Text style={styles.title}>{product.title ?? 'Producto'}</Text>
      <Text style={styles.subtitle}>{product.subtitle ?? ''}</Text>
      <Text style={styles.price}>C$ {product.numericPrice ?? 0}</Text>
      <TouchableOpacity
        style={styles.button}
        onPress={() => onAddToCart(product)}
      >
        <Text style={styles.buttonText}>🛒 Agregar al carrito</Text>
      </TouchableOpacity>
    </View>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: '#fff',
    borderRadius: 12,
    padding: 12,
    marginVertical: 8,
    marginHorizontal: 4,
    shadowColor: '#000',
    shadowOpacity: 0.1,
    shadowRadius: 6,
    elevation: 3,
  },
  image: { width: '100%', height: 150, borderRadius: 8, marginBottom: 8 },
  title: { fontSize: 16, fontWeight: '700', color: '#111' },
  subtitle: { fontSize: 14, color: '#555', marginBottom: 4 },
  price: { fontSize: 15, fontWeight: '600', color: '#3B82F6', marginBottom: 8 },
  button: {
    backgroundColor: '#3B82F6',
    paddingVertical: 8,
    borderRadius: 6,
    alignItems: 'center',
  },
  buttonText: { color: '#fff', fontWeight: '600' },
});
