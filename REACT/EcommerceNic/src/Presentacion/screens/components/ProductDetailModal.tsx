import React, { useState } from 'react';
import {
  Modal,
  View,
  Text,
  StyleSheet,
  ScrollView,
  TouchableOpacity,
  ActivityIndicator,
  Platform,
  SafeAreaView,
} from 'react-native';
import { ProductImage } from '../../components/ProductImage';
import { useProductDetail } from '../../hooks/useProductDetail';
import { formatCurrency } from '../constants';

interface ProductDetailModalProps {
  visible: boolean;
  productId: number | string | null;
  onClose: () => void;
  onAddToCart?: (product: any) => void;
}

export const ProductDetailModal: React.FC<ProductDetailModalProps> = ({
  visible,
  productId,
  onClose,
  onAddToCart,
}) => {
  const { productDetail, isLoading, error, refetch } = useProductDetail(visible ? productId : null);
  const [quantity, setQuantity] = useState<number>(1);

  const handleIncrement = () => setQuantity((prev) => prev + 1);
  const handleDecrement = () => setQuantity((prev) => (prev > 1 ? prev - 1 : 1));

  const handleAddToCart = () => {
    if (!productDetail) return;
    const itemToAdd = {
      id: String(productDetail.productVariableID || productDetail.productID),
      productVariableId: productDetail.productVariableID || productDetail.productID,
      title: productDetail.productName,
      name: productDetail.productName,
      subtitle: productDetail.productVariableName,
      numericPrice: productDetail.productVariablePrice,
      price: productDetail.productVariablePrice,
      image: productDetail.productImageURL,
      ProductoImagenUrl: productDetail.productImageURL,
      quantity: quantity,
    };
    onAddToCart?.(itemToAdd);
    onClose();
  };

  return (
    <Modal
      visible={visible}
      animationType="slide"
      transparent={false}
      onRequestClose={onClose}
    >
      <SafeAreaView style={styles.safeArea}>
        {/* Header Superior */}
        <View style={styles.header}>
          <TouchableOpacity style={styles.closeButton} onPress={onClose} activeOpacity={0.7}>
            <Text style={styles.closeButtonText}>← Volver</Text>
          </TouchableOpacity>
          <Text style={styles.headerTitle} numberOfLines={1}>Detalle del Producto</Text>
          <View style={{ width: 60 }} />
        </View>

        {isLoading ? (
          <View style={styles.centerContainer}>
            <ActivityIndicator size="large" color="#4F46E5" />
            <Text style={styles.loadingText}>Cargando información del producto...</Text>
          </View>
        ) : error ? (
          <View style={styles.centerContainer}>
            <Text style={styles.errorIcon}>📡</Text>
            <Text style={styles.errorTitle}>Error al consultar la API</Text>
            <Text style={styles.errorText}>{error}</Text>
            <TouchableOpacity style={styles.retryButton} onPress={refetch}>
              <Text style={styles.retryButtonText}>Reintentar</Text>
            </TouchableOpacity>
          </View>
        ) : productDetail ? (
          <View style={styles.container}>
            <ScrollView showsVerticalScrollIndicator={false} contentContainerStyle={styles.scrollContent}>
              {/* Contenedor de Imagen del Producto */}
              <View style={styles.imageCard}>
                <ProductImage
                  url={productDetail.productImageURL}
                  style={styles.heroImage}
                  containerStyle={styles.heroImageContainer}
                />
              </View>

              {/* Insignias / Segmentación (Marca, Categoría, Subcategoría, Segmento) */}
              <View style={styles.badgesRow}>
                {!!productDetail.markName && (
                  <View style={[styles.badge, styles.brandBadge]}>
                    <Text style={styles.brandBadgeText}>🏷️ {productDetail.markName.toUpperCase()}</Text>
                  </View>
                )}
                {!!productDetail.categoryName && (
                  <View style={[styles.badge, styles.categoryBadge]}>
                    <Text style={styles.badgeText}>📂 {productDetail.categoryName}</Text>
                  </View>
                )}
                {!!productDetail.subcategoryName && (
                  <View style={[styles.badge, styles.subCategoryBadge]}>
                    <Text style={styles.badgeText}>⚡ {productDetail.subcategoryName}</Text>
                  </View>
                )}
                {!!productDetail.segmentName && (
                  <View style={[styles.badge, styles.segmentBadge]}>
                    <Text style={styles.badgeText}>🎯 {productDetail.segmentName}</Text>
                  </View>
                )}
              </View>

              {/* Información Principal del Producto */}
              <View style={styles.detailsCard}>
                <Text style={styles.productTitle}>{productDetail.productName}</Text>

                {!!productDetail.productVariableName && (
                  <Text style={styles.productSubtitle}>{productDetail.productVariableName}</Text>
                )}

                {/* Precio y Moneda */}
                <View style={styles.priceContainer}>
                  <Text style={styles.priceLabel}>PRECIO</Text>
                  <Text style={styles.priceValue}>
                    {productDetail.currencyISO ? `${productDetail.currencyISO} ` : 'C$ '}
                    {formatCurrency(productDetail.productVariablePrice)}
                  </Text>
                </View>

                <View style={styles.divider} />

                {/* Disponibilidad y Proveedor (SIN fechas ni ID de stock conforme a la instrucción) */}
                <View style={styles.infoGrid}>
                  <View style={styles.infoBox}>
                    <Text style={styles.infoBoxLabel}>DISPONIBILIDAD</Text>
                    {(productDetail.stockAvilable ?? 0) > 0 ? (
                      <View style={styles.stockStatusIn}>
                        <Text style={styles.stockTextIn}>
                          ● En Stock ({productDetail.stockAvilable} disponibles)
                        </Text>
                      </View>
                    ) : (
                      <View style={styles.stockStatusOut}>
                        <Text style={styles.stockTextOut}>● Agotado</Text>
                      </View>
                    )}
                  </View>

                  {!!productDetail.providerName && (
                    <View style={styles.infoBox}>
                      <Text style={styles.infoBoxLabel}>PROVEEDOR OFICIAL</Text>
                      <Text style={styles.infoBoxValue}>🏭 {productDetail.providerName}</Text>
                    </View>
                  )}
                </View>
              </View>
            </ScrollView>

            {/* Footer con Selector de Cantidad y Botón de Agregar al Carrito */}
            <View style={styles.footerContainer}>
              <View style={styles.quantityRow}>
                <Text style={styles.quantityLabel}>Cantidad:</Text>
                <View style={styles.qtyControls}>
                  <TouchableOpacity style={styles.qtyBtn} onPress={handleDecrement}>
                    <Text style={styles.qtyBtnText}>-</Text>
                  </TouchableOpacity>
                  <Text style={styles.qtyNumber}>{quantity}</Text>
                  <TouchableOpacity style={styles.qtyBtnPlus} onPress={handleIncrement}>
                    <Text style={styles.qtyBtnTextPlus}>+</Text>
                  </TouchableOpacity>
                </View>
              </View>

              <TouchableOpacity
                style={[
                  styles.addToCartButton,
                  (productDetail.stockAvilable ?? 0) <= 0 && styles.disabledButton,
                ]}
                onPress={handleAddToCart}
                disabled={(productDetail.stockAvilable ?? 0) <= 0}
                activeOpacity={0.8}
              >
                <Text style={styles.addToCartButtonText}>🛒 Agregar al Carrito</Text>
              </TouchableOpacity>
            </View>
          </View>
        ) : null}
      </SafeAreaView>
    </Modal>
  );
};

const styles = StyleSheet.create({
  safeArea: {
    flex: 1,
    backgroundColor: '#F8FAFC',
  },
  header: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    paddingHorizontal: 16,
    paddingVertical: 14,
    backgroundColor: '#FFFFFF',
    borderBottomWidth: 1,
    borderColor: '#E2E8F0',
    ...Platform.select({
      ios: {
        shadowColor: '#0F172A',
        shadowOffset: { width: 0, height: 2 },
        shadowOpacity: 0.04,
        shadowRadius: 4,
      },
      android: {
        elevation: 2,
      },
    }),
  },
  closeButton: {
    paddingHorizontal: 10,
    paddingVertical: 6,
    backgroundColor: '#EEF2FF',
    borderRadius: 10,
  },
  closeButtonText: {
    color: '#4F46E5',
    fontWeight: '800',
    fontSize: 13,
  },
  headerTitle: {
    fontSize: 16,
    fontWeight: '800',
    color: '#0F172A',
    flex: 1,
    textAlign: 'center',
  },
  centerContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: 24,
  },
  loadingText: {
    marginTop: 12,
    fontSize: 14,
    color: '#64748B',
    fontWeight: '600',
  },
  errorIcon: {
    fontSize: 40,
    marginBottom: 12,
  },
  errorTitle: {
    fontSize: 16,
    fontWeight: '800',
    color: '#0F172A',
  },
  errorText: {
    fontSize: 13,
    color: '#64748B',
    textAlign: 'center',
    marginTop: 6,
  },
  retryButton: {
    marginTop: 16,
    backgroundColor: '#4F46E5',
    paddingHorizontal: 20,
    paddingVertical: 10,
    borderRadius: 12,
  },
  retryButtonText: {
    color: '#FFFFFF',
    fontWeight: '700',
  },
  container: {
    flex: 1,
  },
  scrollContent: {
    padding: 16,
    paddingBottom: 24,
  },
  imageCard: {
    backgroundColor: '#FFFFFF',
    borderRadius: 20,
    padding: 12,
    marginBottom: 16,
    borderWidth: 1,
    borderColor: '#E2E8F0',
    height: 240,
    justifyContent: 'center',
    alignItems: 'center',
  },
  heroImageContainer: {
    width: '100%',
    height: '100%',
    borderRadius: 14,
    overflow: 'hidden',
  },
  heroImage: {
    width: '100%',
    height: '100%',
  },
  badgesRow: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    marginBottom: 16,
    marginHorizontal: -4,
  },
  badge: {
    paddingHorizontal: 10,
    paddingVertical: 5,
    borderRadius: 8,
    marginHorizontal: 4,
    marginBottom: 8,
  },
  brandBadge: {
    backgroundColor: '#0F172A',
  },
  brandBadgeText: {
    color: '#FFFFFF',
    fontSize: 10,
    fontWeight: '800',
  },
  categoryBadge: {
    backgroundColor: '#EEF2FF',
    borderWidth: 0.5,
    borderColor: '#C7D2FE',
  },
  subCategoryBadge: {
    backgroundColor: '#FEF3C7',
    borderWidth: 0.5,
    borderColor: '#FDE68A',
  },
  segmentBadge: {
    backgroundColor: '#ECFDF5',
    borderWidth: 0.5,
    borderColor: '#A7F3D0',
  },
  badgeText: {
    fontSize: 11,
    color: '#1E293B',
    fontWeight: '700',
  },
  detailsCard: {
    backgroundColor: '#FFFFFF',
    borderRadius: 20,
    padding: 18,
    borderWidth: 1,
    borderColor: '#E2E8F0',
  },
  productTitle: {
    fontSize: 20,
    fontWeight: '900',
    color: '#0F172A',
    lineHeight: 26,
  },
  productSubtitle: {
    fontSize: 14,
    color: '#475569',
    marginTop: 6,
    fontWeight: '500',
  },
  priceContainer: {
    marginTop: 16,
    backgroundColor: '#F8FAFC',
    padding: 12,
    borderRadius: 14,
    borderWidth: 1,
    borderColor: '#F1F5F9',
  },
  priceLabel: {
    fontSize: 9,
    fontWeight: '800',
    color: '#94A3B8',
    letterSpacing: 0.5,
  },
  priceValue: {
    fontSize: 24,
    fontWeight: '900',
    color: '#4F46E5',
    marginTop: 2,
  },
  divider: {
    height: 1,
    backgroundColor: '#E2E8F0',
    marginVertical: 16,
  },
  infoGrid: {
    gap: 12,
  },
  infoBox: {
    backgroundColor: '#F8FAFC',
    padding: 12,
    borderRadius: 12,
  },
  infoBoxLabel: {
    fontSize: 9,
    fontWeight: '800',
    color: '#64748B',
    letterSpacing: 0.5,
    marginBottom: 4,
  },
  infoBoxValue: {
    fontSize: 13,
    fontWeight: '700',
    color: '#0F172A',
  },
  stockStatusIn: {
    alignSelf: 'flex-start',
    backgroundColor: '#ECFDF5',
    paddingHorizontal: 10,
    paddingVertical: 4,
    borderRadius: 8,
  },
  stockTextIn: {
    color: '#059669',
    fontSize: 12,
    fontWeight: '800',
  },
  stockStatusOut: {
    alignSelf: 'flex-start',
    backgroundColor: '#FEF2F2',
    paddingHorizontal: 10,
    paddingVertical: 4,
    borderRadius: 8,
  },
  stockTextOut: {
    color: '#EF4444',
    fontSize: 12,
    fontWeight: '800',
  },
  footerContainer: {
    backgroundColor: '#FFFFFF',
    borderTopWidth: 1,
    borderColor: '#E2E8F0',
    padding: 16,
  },
  quantityRow: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    marginBottom: 12,
  },
  quantityLabel: {
    fontSize: 14,
    fontWeight: '800',
    color: '#0F172A',
  },
  qtyControls: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#EEF2FF',
    borderRadius: 12,
    padding: 4,
  },
  qtyBtn: {
    width: 28,
    height: 28,
    backgroundColor: '#FFFFFF',
    borderRadius: 8,
    justifyContent: 'center',
    alignItems: 'center',
  },
  qtyBtnPlus: {
    width: 28,
    height: 28,
    backgroundColor: '#4F46E5',
    borderRadius: 8,
    justifyContent: 'center',
    alignItems: 'center',
  },
  qtyBtnText: {
    fontSize: 14,
    color: '#4F46E5',
    fontWeight: '800',
  },
  qtyBtnTextPlus: {
    fontSize: 14,
    color: '#FFFFFF',
    fontWeight: '800',
  },
  qtyNumber: {
    fontSize: 14,
    fontWeight: '800',
    color: '#4F46E5',
    paddingHorizontal: 12,
  },
  addToCartButton: {
    backgroundColor: '#4F46E5',
    height: 48,
    borderRadius: 16,
    justifyContent: 'center',
    alignItems: 'center',
  },
  disabledButton: {
    backgroundColor: '#94A3B8',
  },
  addToCartButtonText: {
    color: '#FFFFFF',
    fontSize: 15,
    fontWeight: '800',
  },
});
