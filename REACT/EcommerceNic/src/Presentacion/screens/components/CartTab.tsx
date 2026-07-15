import React from 'react';
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  TouchableOpacity,
  Image,
  Platform,
} from 'react-native';
import { formatCurrency } from '../constants';
import { Product } from '../../../Domain/entities/Product';

interface CartTabProps {
  products: Product[];
  cartQuantities: { [key: string]: number };
  addUnit: (id: string) => void;
  removeUnit: (id: string) => void;
  deleteFromCart: (id: string) => void;
  setCurrentTab: (tab: 'home' | 'cart' | 'chatbot' | 'nosotros') => void;
  openPaymentModal: () => void;
  totalItemsInCart: number;
}

export const CartTab = ({
  products,
  cartQuantities,
  addUnit,
  removeUnit,
  deleteFromCart,
  setCurrentTab,
  openPaymentModal,
  totalItemsInCart,
}: CartTabProps) => {
  const subtotal = products.reduce((acc, p) => acc + (p.numericPrice * (cartQuantities[p.id] || 0)), 0);
  const shippingCost = subtotal > 0 ? 350 : 0;
  const totalPayment = subtotal + shippingCost;

  return (
    <View style={styles.tabContent}>
      <View style={styles.cartHeader}>
        <TouchableOpacity style={styles.backButton} onPress={() => setCurrentTab('home')}>
          <Text style={styles.backButtonText}>←</Text>
        </TouchableOpacity>
        <Text style={styles.cartHeaderTitle}>Tu Carrito</Text>
        <View style={styles.itemsCountBadge}>
          <Text style={styles.itemsCountText}>{totalItemsInCart} items</Text>
        </View>
      </View>

      {totalItemsInCart === 0 ? (
        <View style={styles.emptyCartContainer}>
          <View style={styles.emptyCartIconBackground}>
            <Text style={styles.emptyTextEmoji}>🛒</Text>
          </View>
          <Text style={styles.emptyTextTitle}>Tu carrito está vacío</Text>
          <Text style={styles.emptyTextSub}>¡Agrega productos del catálogo para habilitar el pago!</Text>
          <TouchableOpacity style={styles.returnButton} onPress={() => setCurrentTab('home')}>
            <Text style={styles.returnButtonText}>Ir a la Tienda</Text>
          </TouchableOpacity>
        </View>
      ) : (
        <>
          <ScrollView showsVerticalScrollIndicator={false} style={styles.cartItemsList}>
            {products.map((product) => {
              const qty = cartQuantities[product.id] || 0;
              if (qty === 0) return null;
              return (
                <View key={product.id} style={styles.cartItemCard}>
                  <Image source={{ uri: product.image }} style={styles.cartItemImage} />
                  <View style={styles.cartItemDetails}>
                    <View style={styles.cartItemRowHeader}>
                      <Text style={styles.cartItemBrand}>{product.brand.toUpperCase()}</Text>
                      <TouchableOpacity 
                        style={styles.trashBtn} 
                        onPress={() => deleteFromCart(product.id)} 
                        hitSlop={{ top: 10, bottom: 10, left: 10, right: 10 }}
                      >
                        <Text style={styles.deleteTrashIcon}>🗑️</Text>
                      </TouchableOpacity>
                    </View>
                    <Text style={styles.cartItemTitle} numberOfLines={1}>{product.title}</Text>
                    <Text style={styles.cartItemSubtitle} numberOfLines={1}>{product.subtitle}</Text>
                    <View style={styles.cartItemRowFooter}>
                      <Text style={styles.cartItemPrice}>{formatCurrency(product.numericPrice)}</Text>
                      <View style={styles.cartQtyRow}>
                        <TouchableOpacity style={styles.inlineQtyBtn} onPress={() => removeUnit(product.id)}>
                          <Text style={styles.inlineQtyText}>-</Text>
                        </TouchableOpacity>
                        <Text style={styles.inlineQtyNumber}>{qty}</Text>
                        <TouchableOpacity style={styles.inlineQtyBtnPlus} onPress={() => addUnit(product.id)}>
                          <Text style={styles.inlineQtyTextPlus}>+</Text>
                        </TouchableOpacity>
                      </View>
                    </View>
                  </View>
                </View>
              );
            })}
          </ScrollView>

          {/* Premium Resumen de Pago (Glassmorphism inspired) */}
          <View style={styles.checkoutFooterCard}>
            <View style={styles.checkoutSummaryRow}>
              <Text style={styles.summaryLabel}>Subtotal</Text>
              <Text style={styles.summaryValue}>{formatCurrency(subtotal)}</Text>
            </View>
            <View style={styles.checkoutSummaryRow}>
              <Text style={styles.summaryLabel}>Envío a Managua</Text>
              <Text style={styles.summaryValue}>{formatCurrency(shippingCost)}</Text>
            </View>
            <View style={styles.dividerLine} />
            <View style={styles.checkoutSummaryRow}>
              <Text style={styles.totalLabel}>Total</Text>
              <Text style={styles.totalValue}>{formatCurrency(totalPayment)}</Text>
            </View>
            <TouchableOpacity style={styles.payButton} activeOpacity={0.8} onPress={openPaymentModal}>
              <Text style={styles.payButtonText}>Proceder al Pago →</Text>
            </TouchableOpacity>
          </View>
        </>
      )}
    </View>
  );
};

const styles = StyleSheet.create({
  tabContent: { flex: 1, paddingBottom: 68 },
  cartHeader: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: 20, paddingTop: 16, marginBottom: 12 },
  backButton: { width: 38, height: 38, backgroundColor: '#FFFFFF', borderRadius: 12, justifyContent: 'center', alignItems: 'center', borderWidth: 1.5, borderColor: '#F1F5F9' },
  backButtonText: { fontSize: 18, fontWeight: '700', color: '#0F172A' },
  cartHeaderTitle: { flex: 1, textAlign: 'center', fontSize: 18, fontWeight: '950', color: '#0F172A', marginLeft: 16, marginRight: 16 },
  itemsCountBadge: { backgroundColor: '#EEF2FF', paddingHorizontal: 12, paddingVertical: 6, borderRadius: 10 },
  itemsCountText: { color: '#4F46E5', fontSize: 12, fontWeight: '800' },
  cartItemsList: { paddingHorizontal: 20, flex: 1, marginBottom: 10 },
  cartItemCard: { 
    flexDirection: 'row', 
    backgroundColor: '#FFFFFF', 
    borderRadius: 20, 
    padding: 12, 
    marginBottom: 12, 
    borderWidth: 1, 
    borderColor: '#F1F5F9',
    ...Platform.select({
      ios: {
        shadowColor: '#0F172A',
        shadowOffset: { width: 0, height: 4 },
        shadowOpacity: 0.02,
        shadowRadius: 10,
      },
      android: {
        elevation: 1,
      },
      default: {
        shadowColor: '#0F172A',
        shadowOffset: { width: 0, height: 4 },
        shadowOpacity: 0.02,
        shadowRadius: 10,
      }
    }),
  },
  cartItemImage: { width: 75, height: 75, borderRadius: 14, backgroundColor: '#F8FAFC' },
  cartItemDetails: { flex: 1, marginLeft: 14, justifyContent: 'space-between' },
  cartItemRowHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' },
  cartItemBrand: { backgroundColor: '#F1F5F9', color: '#64748B', fontSize: 9, fontWeight: '800', paddingHorizontal: 6, paddingVertical: 2, borderRadius: 5, letterSpacing: 0.5 },
  trashBtn: {
    width: 26,
    height: 26,
    borderRadius: 6,
    backgroundColor: '#FEF2F2',
    justifyContent: 'center',
    alignItems: 'center',
  },
  deleteTrashIcon: { fontSize: 12 },
  cartItemTitle: { fontSize: 14, fontWeight: '800', color: '#0F172A', marginTop: 4 },
  cartItemSubtitle: { fontSize: 11, color: '#64748B', marginTop: 1 },
  cartItemRowFooter: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginTop: 6 },
  cartItemPrice: { fontSize: 14, fontWeight: '900', color: '#4F46E5' },
  cartQtyRow: { 
    flexDirection: 'row', 
    alignItems: 'center', 
    backgroundColor: '#EEF2FF', 
    borderRadius: 12, 
    padding: 3 
  },
  inlineQtyBtn: { 
    width: 22, 
    height: 22, 
    backgroundColor: '#FFFFFF', 
    borderRadius: 8, 
    justifyContent: 'center', 
    alignItems: 'center' 
  },
  inlineQtyBtnPlus: { 
    width: 22, 
    height: 22, 
    backgroundColor: '#4F46E5', 
    borderRadius: 8, 
    justifyContent: 'center', 
    alignItems: 'center' 
  },
  inlineQtyText: { fontSize: 12, color: '#4F46E5', fontWeight: '800' },
  inlineQtyTextPlus: { fontSize: 12, color: '#FFFFFF', fontWeight: '800' },
  inlineQtyNumber: { fontSize: 12, fontWeight: '800', color: '#4F46E5', paddingHorizontal: 8 },
  
  // Resumen de Pago estilo Glassmorphism
  checkoutFooterCard: { 
    backgroundColor: 'rgba(255, 255, 255, 0.9)', 
    borderTopLeftRadius: 28, 
    borderTopRightRadius: 28, 
    padding: 24, 
    borderTopWidth: 1.5, 
    borderColor: '#EEF2FF', 
    ...Platform.select({
      ios: {
        shadowColor: '#4F46E5',
        shadowOffset: { width: 0, height: -8 },
        shadowOpacity: 0.04,
        shadowRadius: 20,
      },
      android: {
        elevation: 8,
      },
      default: {
        shadowColor: '#4F46E5',
        shadowOffset: { width: 0, height: -8 },
        shadowOpacity: 0.04,
        shadowRadius: 20,
      }
    }),
  },
  checkoutSummaryRow: { flexDirection: 'row', justifyContent: 'space-between', marginBottom: 12 },
  summaryLabel: { fontSize: 13, color: '#64748B', fontWeight: '600' },
  summaryValue: { fontSize: 14, fontWeight: '800', color: '#0F172A' },
  dividerLine: { height: 1.5, backgroundColor: '#EEF2FF', marginVertical: 10 },
  totalLabel: { fontSize: 16, fontWeight: '900', color: '#0F172A' },
  totalValue: { fontSize: 22, fontWeight: '950', color: '#4F46E5' },
  payButton: { 
    backgroundColor: '#4F46E5', 
    height: 52, 
    borderRadius: 24, 
    justifyContent: 'center', 
    alignItems: 'center', 
    marginTop: 18,
    ...Platform.select({
      ios: {
        shadowColor: '#4F46E5',
        shadowOffset: { width: 0, height: 4 },
        shadowOpacity: 0.2,
        shadowRadius: 8,
      },
      android: {
        elevation: 4,
      },
    }),
  },
  payButtonText: { color: '#FFFFFF', fontSize: 15, fontWeight: '850' },
  emptyCartContainer: { flex: 1, justifyContent: 'center', alignItems: 'center', paddingHorizontal: 40, marginTop: 40 },
  emptyCartIconBackground: {
    width: 80,
    height: 80,
    borderRadius: 40,
    backgroundColor: '#EEF2FF',
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 20,
  },
  emptyTextEmoji: { fontSize: 36 },
  emptyTextTitle: { fontSize: 18, fontWeight: '850', color: '#0F172A' },
  emptyTextSub: { fontSize: 13, color: '#64748B', textAlign: 'center', marginTop: 6, marginBottom: 24, lineHeight: 18 },
  returnButton: { backgroundColor: '#EEF2FF', paddingHorizontal: 24, height: 40, borderRadius: 20, justifyContent: 'center' },
  returnButtonText: { color: '#4F46E5', fontSize: 13, fontWeight: '800' },
});
