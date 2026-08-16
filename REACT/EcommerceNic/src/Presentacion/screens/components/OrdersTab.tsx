import React from 'react';
import {
  View,
  Text,
  StyleSheet,
  FlatList,
  TouchableOpacity,
  ActivityIndicator,
  Modal,
  ScrollView,
  SafeAreaView,
} from 'react-native';
import { Feather } from '@expo/vector-icons';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import { Order, OrderDetail } from '../../../Domain/entities/Order';
import { formatCurrency } from '../constants';
import { COLORES, ESTILOS_SOMBRA } from '../../theme/theme';
import { ProductImage } from '../../components/ProductImage';

type FeatherIconName = React.ComponentProps<typeof Feather>['name'];

interface OrdersTabProps {
  ordenes: Order[];
  cargandoOrdenes: boolean;
  ordenSeleccionada: Order | null;
  detallesOrdenSeleccionada: OrderDetail[];
  cargandoDetalles: boolean;
  cargarDetallesOrden: (orden: Order) => void;
  cerrarModalDetalle: () => void;
  refetch: () => void;
  setCurrentTab: (tab: any) => void;
}

export const OrdersTab: React.FC<OrdersTabProps> = ({
  ordenes,
  cargandoOrdenes,
  ordenSeleccionada,
  detallesOrdenSeleccionada,
  cargandoDetalles,
  cargarDetallesOrden,
  cerrarModalDetalle,
  refetch,
  setCurrentTab,
}) => {
  const insets = useSafeAreaInsets(); // ✅ Hook dentro del componente

  const obtenerInsigniaEstado = (statusId?: number, statusName?: string) => {
    switch (statusId) {
      case 2:
        return {
          texto: statusName || 'Enviado',
          fondo: '#DBEAFE',
          color: '#3B82F6',
          icono: 'truck' as FeatherIconName,
        };
      case 3:
        return {
          texto: statusName || 'Entregado',
          fondo: '#D1FAE5',
          color: '#10B981',
          icono: 'check-circle' as FeatherIconName,
        };
      case 4:
        return {
          texto: statusName || 'Cancelado',
          fondo: '#FEE2E2',
          color: '#EF4444',
          icono: 'x-circle' as FeatherIconName,
        };
      case 1:
      default:
        return {
          texto: statusName || 'Procesando',
          fondo: '#FEF3C7',
          color: '#F59E0B',
          icono: 'clock' as FeatherIconName,
        };
    }
  };

  const renderOrderTimeline = (statusId: number = 1) => {
    if (statusId === 4) {
      return (
        <View style={styles.timelineCancelledBox}>
          <Feather name="x-circle" size={14} color="#EF4444" style={{ marginRight: 6 }} />
          <Text style={styles.timelineCancelledText}>Este pedido ha sido Cancelado</Text>
        </View>
      );
    }

    const activeStep = statusId >= 3 ? 3 : (statusId >= 2 ? 2 : 1);

    return (
      <View style={styles.timelineWrapper}>
        <Text style={styles.timelineTitle}>Seguimiento del Pedido</Text>
        <View style={styles.timelineContainer}>
          {/* Paso 1: Procesando */}
          <View style={styles.timelineStep}>
            <View style={[styles.timelineNode, activeStep >= 1 && styles.timelineNodeActive]}>
              {activeStep >= 1 ? (
                <Feather name="check" size={12} color="#FFFFFF" />
              ) : (
                <Text style={styles.timelineNodeText}>1</Text>
              )}
            </View>
            <View style={styles.timelineLabelRow}>
              <Feather name="clock" size={12} color={activeStep >= 1 ? '#4F46E5' : '#94A3B8'} style={{ marginRight: 4 }} />
              <Text style={[styles.timelineLabel, activeStep >= 1 && styles.timelineLabelActive]}>
                Procesando
              </Text>
            </View>
          </View>

          {/* Linea Conectora 1-2 */}
          <View style={[styles.timelineLine, activeStep >= 2 && styles.timelineLineActive]} />

          {/* Paso 2: En Camino */}
          <View style={styles.timelineStep}>
            <View style={[styles.timelineNode, activeStep >= 2 && styles.timelineNodeActive]}>
              {activeStep >= 2 ? (
                <Feather name="check" size={12} color="#FFFFFF" />
              ) : (
                <Text style={styles.timelineNodeText}>2</Text>
              )}
            </View>
            <View style={styles.timelineLabelRow}>
              <Feather name="truck" size={12} color={activeStep >= 2 ? '#4F46E5' : '#94A3B8'} style={{ marginRight: 4 }} />
              <Text style={[styles.timelineLabel, activeStep >= 2 && styles.timelineLabelActive]}>
                En Camino
              </Text>
            </View>
          </View>

          {/* Linea Conectora 2-3 */}
          <View style={[styles.timelineLine, activeStep >= 3 && styles.timelineLineActive]} />

          {/* Paso 3: Entregado */}
          <View style={styles.timelineStep}>
            <View style={[styles.timelineNode, activeStep >= 3 && styles.timelineNodeActive]}>
              {activeStep >= 3 ? (
                <Feather name="check" size={12} color="#FFFFFF" />
              ) : (
                <Text style={styles.timelineNodeText}>3</Text>
              )}
            </View>
            <View style={styles.timelineLabelRow}>
              <Feather name="package" size={12} color={activeStep >= 3 ? '#4F46E5' : '#94A3B8'} style={{ marginRight: 4 }} />
              <Text style={[styles.timelineLabel, activeStep >= 3 && styles.timelineLabelActive]}>
                Entregado
              </Text>
            </View>
          </View>
        </View>
      </View>
    );
  };

  const formatearFecha = (fechaStr?: string) => {
    if (!fechaStr) return 'Fecha reciente';
    try {
      const fecha = new Date(fechaStr);
      const fechaFormat = fecha.toLocaleDateString('es-NI', {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
      });
      const horaFormat = fecha.toLocaleTimeString('es-NI', {
        hour: '2-digit',
        minute: '2-digit',
        hour12: true,
      });
      return `${fechaFormat}, ${horaFormat}`;
    } catch {
      return fechaStr;
    }
  };

  return (
    <SafeAreaView style={styles.container}>
      {/* Header sin cajita ni subtítulo */}
      <View style={styles.header}>
        <View style={styles.headerTitleRow}>
          <Text style={styles.headerTitle}>MIS COMPRAS</Text>
          <TouchableOpacity style={styles.refreshButton} onPress={refetch} activeOpacity={0.7}>
            <Feather name="refresh-cw" size={14} color="#4F46E5" style={{ marginRight: 4 }} />
            <Text style={styles.refreshText}>Actualizar</Text>
          </TouchableOpacity>
        </View>
      </View>

      {/* Contenido Principal */}
      {cargandoOrdenes ? (
        <View style={styles.centeredContainer}>
          <ActivityIndicator size="large" color={COLORES.primario} />
          <Text style={styles.loadingText}>Cargando tus compras...</Text>
        </View>
      ) : ordenes.length === 0 ? (
        <View style={styles.emptyContainer}>
          <View style={styles.emptyIconBackground}>
            <Feather name="package" size={36} color="#3B82F6" />
          </View>
          <Text style={styles.emptyTitle}>Aún no has realizado compras</Text>
          <Text style={styles.emptySubtitle}>¡Explora el catálogo, agrega tus productos favoritos y realiza tu primer pedido!</Text>
          <TouchableOpacity
            style={styles.exploreButton}
            onPress={() => setCurrentTab('home')}
            activeOpacity={0.8}
          >
            <Text style={styles.exploreButtonText}>Ir al Catálogo</Text>
          </TouchableOpacity>
        </View>
      ) : (
        <FlatList<Order>
          data={ordenes}
          keyExtractor={(item, index) => `${item.paymentOrderId ?? item.ordenPagoId ?? index}-${index}`}
          contentContainerStyle={[
            styles.listContent,
            { paddingBottom: 60 + insets.bottom }, // ✅ Espacio extra para no tapar la última tarjeta
          ]}
          showsVerticalScrollIndicator={false}
          renderItem={({ item, index }) => {
            const rawId = item.orderId ?? item.paymentOrderId ?? item.ordenPagoId ?? 0;
            const orderId = rawId > 0 ? rawId : (index + 101);
            const insignia = obtenerInsigniaEstado(item.statusId, item.statusName);
            const total = item.totalAmount ?? item.totalOrden ?? 0;

            return (
              <TouchableOpacity
                style={[styles.orderCard, ESTILOS_SOMBRA.tarjeta]}
                onPress={() => cargarDetallesOrden(item)}
                activeOpacity={0.85}
              >
                <View style={styles.cardHeader}>
                  <View>
                    <Text style={styles.orderIdText}>Pedido #{orderId}</Text>
                    <Text style={styles.orderDateText}>{formatearFecha(item.orderDate)}</Text>
                  </View>
                  <View style={[styles.badgeContainer, { backgroundColor: insignia.fondo }]}>
                    <Feather name={insignia.icono} size={12} color={insignia.color} style={styles.badgeIcon} />
                    <Text style={[styles.badgeText, { color: insignia.color }]}>{insignia.texto}</Text>
                  </View>
                </View>

                {/* Timeline de Seguimiento del Pedido */}
                {renderOrderTimeline(item.statusId)}

                <View style={styles.cardDivider} />

                <View style={styles.cardFooter}>
                  <View style={{ flex: 1, paddingRight: 10 }}>
                    <Text style={styles.paymentMethodLabel}>Método de Pago</Text>
                    <Text style={styles.paymentMethodValue}>{item.paymentMethodName || 'Efectivo contra entrega'}</Text>
                  </View>

                  <View style={{ alignItems: 'flex-end' }}>
                    <Text style={styles.totalLabel}>Total Pagado</Text>
                    <Text style={styles.totalValue}>{formatCurrency(total)}</Text>
                  </View>
                </View>

                <View style={styles.viewDetailRow}>
                  <View style={styles.viewDetailContent}>
                    <Text style={styles.viewDetailText}>Ver detalle del pedido</Text>
                    <Feather name="arrow-right" size={14} color="#4F46E5" style={{ marginLeft: 4 }} />
                  </View>
                </View>
              </TouchableOpacity>
            );
          }}
        />
      )}

      {/* Modal de Detalle */}
      <Modal
        visible={!!ordenSeleccionada}
        transparent
        animationType="slide"
        onRequestClose={cerrarModalDetalle}
      >
        <View style={styles.modalOverlay}>
          <View style={styles.modalSheet}>
            {/* Header del Modal */}
            <View style={styles.modalHeader}>
              <View>
                <Text style={styles.modalTitle}>
                  Detalle del Pedido #{ordenSeleccionada?.paymentOrderId ?? ordenSeleccionada?.ordenPagoId}
                </Text>
                <View style={styles.modalSubtitleRow}>
                  <Feather name="calendar" size={12} color="#64748B" style={{ marginRight: 4 }} />
                  <Text style={styles.modalSubtitle}>
                    {formatearFecha(ordenSeleccionada?.orderDate)}
                  </Text>
                </View>
              </View>
              <TouchableOpacity style={styles.closeButton} onPress={cerrarModalDetalle}>
                <Feather name="x" size={18} color="#64748B" />
              </TouchableOpacity>
            </View>

            {/* Contenido del Modal */}
            {cargandoDetalles ? (
              <View style={styles.modalLoadingContainer}>
                <ActivityIndicator size="large" color={COLORES.primario} />
                <Text style={styles.loadingText}>Cargando artículos de la orden...</Text>
              </View>
            ) : (
              <ScrollView style={styles.modalScrollView} showsVerticalScrollIndicator={false}>
                
                {/* Bloque de Información de Entrega y Pago */}
                <View style={styles.modalInfoBox}>
                  <View style={styles.infoRow}>
                    <Feather name="map-pin" size={14} color="#4F46E5" style={styles.infoIcon} />
                    <Text style={styles.infoLabel}>Dirección de Entrega:</Text>
                    <Text style={styles.infoValue}>{ordenSeleccionada?.addressText || 'Managua - Dirección de Entrega Principal'}</Text>
                  </View>
                  <View style={styles.infoRow}>
                    <Feather name="credit-card" size={14} color="#4F46E5" style={styles.infoIcon} />
                    <Text style={styles.infoLabel}>Método de Pago:</Text>
                    <Text style={styles.infoValue}>{ordenSeleccionada?.paymentMethodName || 'Efectivo contra entrega'}</Text>
                  </View>
                </View>

                {/* Timeline de Seguimiento dentro del Modal */}
                {renderOrderTimeline(ordenSeleccionada?.statusId)}

                <Text style={styles.sectionTitle}>Artículos Comprados</Text>
                {(() => {
                  const listaArticulos = (detallesOrdenSeleccionada && detallesOrdenSeleccionada.length > 0)
                    ? detallesOrdenSeleccionada
                    : (ordenSeleccionada?.details || []);

                  if (listaArticulos.length === 0) {
                    return (
                      <View style={styles.noDetailsBox}>
                        <Feather name="package" size={16} color="#64748B" style={{ marginRight: 6 }} />
                        <Text style={styles.noDetailsText}>
                          Pedido registrado con éxito en Nic Store. Tus artículos se encuentran en proceso de preparación y despacho por nuestro equipo.
                        </Text>
                      </View>
                    );
                  }

                  return listaArticulos.map((det, idx) => {
                    const detailNum = det.orderDetailId ?? det.paymentOrderDetailId ?? det.detalleOrdenPagoId;

                    return (
                      <View key={idx} style={styles.detailItemRow}>
                        <ProductImage
                          url={det.productImageURL ?? det.productoImagenUrl}
                          style={styles.detailImage}
                          containerStyle={styles.detailImageContainer}
                          resizeMode="contain"
                        />
                        <View style={styles.detailInfo}>
                          {detailNum ? (
                            <Text style={{ fontSize: 9, fontWeight: '800', color: '#64748B', marginBottom: 2 }}>
                              Nº DETALLE: #{detailNum}
                            </Text>
                          ) : null}
                          <Text style={styles.detailProductName} numberOfLines={2}>
                            {det.productName ?? det.productoNombre ?? 'Producto'}
                          </Text>
                          <Text style={styles.detailQtyPrice}>
                            {det.quantity ?? det.cantidad ?? 1} x {formatCurrency(det.price ?? det.precioUnitario ?? 0)}
                          </Text>
                        </View>
                        <Text style={styles.detailTotalText}>
                          {formatCurrency(det.total ?? det.totalFila ?? ((det.price ?? 0) * (det.quantity ?? 1)))}
                        </Text>
                      </View>
                    );
                  });
                })()}

                <View style={styles.summaryBox}>
                  {(() => {
                    const totalVal = ordenSeleccionada?.totalAmount ?? ordenSeleccionada?.totalOrden ?? 0;
                    const subtotalVal = ordenSeleccionada?.subTotal ?? ordenSeleccionada?.subTotalOrden ?? Math.max(0, totalVal - 350);
                    const envioVal = ordenSeleccionada?.shippingCost ?? ordenSeleccionada?.costoEnvio ?? (totalVal > subtotalVal ? (totalVal - subtotalVal) : 350);

                    return (
                      <>
                        <View style={styles.summaryRowItem}>
                          <Text style={styles.summarySubLabel}>Subtotal de Productos:</Text>
                          <Text style={styles.summarySubValue}>{formatCurrency(subtotalVal)}</Text>
                        </View>
                        <View style={styles.summaryRowItem}>
                          <View style={styles.summarySubLabelRow}>
                            <Feather name="truck" size={14} color="#4F46E5" style={{ marginRight: 4 }} />
                            <Text style={styles.summarySubLabel}>Tarifa de Envío (Managua):</Text>
                          </View>
                          <Text style={styles.summarySubValue}>{formatCurrency(envioVal)}</Text>
                        </View>
                        <View style={styles.summaryDivider} />
                        <View style={styles.summaryRow}>
                          <Text style={styles.summaryLabel}>Total Pagado:</Text>
                          <Text style={styles.summaryTotalValue}>{formatCurrency(totalVal)}</Text>
                        </View>
                      </>
                    );
                  })()}
                </View>
              </ScrollView>
            )}
          </View>
        </View>
      </Modal>
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F8FAFC',
  },
  header: {
    paddingHorizontal: 20,
    paddingTop: 16,
    paddingBottom: 12,
  },
  headerTitleRow: {
    position: 'relative',
    justifyContent: 'center',
    alignItems: 'center',
  },
  headerTitle: {
    fontSize: 18, // Si quieres el mismo tamaño que "Pedidos", pon 12
    fontWeight: 'bold',
    color: '#0F172A',
    textAlign: 'center',
    textTransform: 'uppercase',
  },
  refreshButton: {
    position: 'absolute',
    right: 0,
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 12,
    paddingVertical: 6,
    borderRadius: 20,
    backgroundColor: '#EFF6FF',
  },
  refreshText: {
    fontSize: 12,
    fontWeight: '600',
    color: '#4F46E5',
  },
  centeredContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  loadingText: {
    marginTop: 12,
    fontSize: 14,
    color: '#64748B',
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    paddingHorizontal: 32,
  },
  emptyIconBackground: {
    width: 88,
    height: 88,
    borderRadius: 44,
    backgroundColor: '#EFF6FF',
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 20,
  },
  emptyTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#0F172A',
    marginBottom: 8,
  },
  emptySubtitle: {
    fontSize: 14,
    color: '#64748B',
    textAlign: 'center',
    lineHeight: 20,
    marginBottom: 24,
  },
  exploreButton: {
    backgroundColor: '#3B82F6',
    paddingHorizontal: 24,
    paddingVertical: 12,
    borderRadius: 14,
  },
  exploreButtonText: {
    color: '#FFFFFF',
    fontWeight: 'bold',
    fontSize: 15,
  },
  listContent: {
    padding: 16,
    gap: 12,
  },
  orderCard: {
    backgroundColor: '#FFFFFF',
    borderRadius: 16,
    padding: 16,
    marginBottom: 12,
  },
  cardHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  orderIdText: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#0F172A',
  },
  orderDateText: {
    fontSize: 13,
    color: '#64748B',
    marginTop: 2,
  },
  badgeContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 12,
    paddingVertical: 4,
    borderRadius: 12,
  },
  badgeIcon: {
    marginRight: 4,
  },
  badgeText: {
    fontSize: 12,
    fontWeight: 'bold',
  },
  cardDivider: {
    height: 1,
    backgroundColor: '#F1F5F9',
    marginVertical: 12,
  },
  cardFooter: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  paymentMethodLabel: {
    fontSize: 12,
    color: '#94A3B8',
  },
  paymentMethodValue: {
    fontSize: 13,
    fontWeight: '600',
    color: '#334155',
    marginTop: 2,
  },
  totalLabel: {
    fontSize: 12,
    color: '#94A3B8',
  },
  totalValue: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#3B82F6',
    marginTop: 2,
  },
  viewDetailRow: {
    marginTop: 12,
    alignItems: 'flex-end',
  },
  viewDetailContent: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  viewDetailText: {
    fontSize: 13,
    fontWeight: '600',
    color: '#4F46E5',
  },
  modalOverlay: {
    flex: 1,
    backgroundColor: 'rgba(15, 23, 42, 0.6)',
    justifyContent: 'flex-end',
  },
  modalSheet: {
    backgroundColor: '#FFFFFF',
    borderTopLeftRadius: 24,
    borderTopRightRadius: 24,
    maxHeight: '80%',
    padding: 20,
  },
  modalHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 16,
    paddingBottom: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#F1F5F9',
  },
  modalTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#0F172A',
  },
  modalSubtitleRow: {
    flexDirection: 'row',
    alignItems: 'center',
    marginTop: 4,
  },
  modalSubtitle: {
    fontSize: 13,
    color: '#64748B',
  },
  closeButton: {
    width: 36,
    height: 36,
    borderRadius: 18,
    backgroundColor: '#F1F5F9',
    justifyContent: 'center',
    alignItems: 'center',
  },
  modalLoadingContainer: {
    paddingVertical: 40,
    alignItems: 'center',
  },
  modalScrollView: {
    marginTop: 4,
  },
  sectionTitle: {
    fontSize: 15,
    fontWeight: 'bold',
    color: '#334155',
    marginBottom: 12,
  },
  modalInfoBox: {
    padding: 14,
    backgroundColor: '#F8FAFC',
    borderRadius: 14,
    marginBottom: 16,
    borderWidth: 1,
    borderColor: '#E2E8F0',
    gap: 8,
  },
  infoRow: {
    flexDirection: 'row',
    alignItems: 'flex-start',
  },
  infoIcon: {
    marginRight: 6,
    marginTop: 1,
  },
  infoLabel: {
    fontSize: 13,
    fontWeight: 'bold',
    color: '#475569',
    width: 140,
  },
  infoValue: {
    fontSize: 13,
    color: '#0F172A',
    fontWeight: '500',
    flex: 1,
  },
  noDetailsBox: {
    flexDirection: 'row',
    alignItems: 'center',
    padding: 16,
    backgroundColor: '#F8FAFC',
    borderRadius: 12,
    marginBottom: 16,
  },
  noDetailsText: {
    fontSize: 13,
    color: '#64748B',
    flex: 1,
  },
  detailItemRow: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingVertical: 10,
    borderBottomWidth: 1,
    borderBottomColor: '#F8FAFC',
  },
  detailImageContainer: {
    borderRadius: 8,
    overflow: 'hidden',
  },
  detailImage: {
    width: 48,
    height: 48,
  },
  detailInfo: {
    flex: 1,
    marginLeft: 12,
  },
  detailProductName: {
    fontSize: 14,
    fontWeight: '600',
    color: '#0F172A',
  },
  detailQtyPrice: {
    fontSize: 13,
    color: '#64748B',
    marginTop: 2,
  },
  detailTotalText: {
    fontSize: 14,
    fontWeight: 'bold',
    color: '#3B82F6',
  },
  summaryBox: {
    marginTop: 20,
    padding: 16,
    backgroundColor: '#EFF6FF',
    borderRadius: 14,
    marginBottom: 20,
    gap: 8,
  },
  summaryRowItem: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  summarySubLabelRow: {
    flexDirection: 'row',
    alignItems: 'center',
    flex: 1,
  },
  summarySubLabel: {
    fontSize: 13,
    color: '#475569',
  },
  summarySubValue: {
    fontSize: 14,
    fontWeight: '600',
    color: '#334155',
  },
  summaryDivider: {
    height: 1,
    backgroundColor: '#DBEAFE',
    marginVertical: 4,
  },
  summaryRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  summaryLabel: {
    fontSize: 15,
    fontWeight: 'bold',
    color: '#1E40AF',
  },
  summaryTotalValue: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#1D4ED8',
  },
  timelineWrapper: {
    backgroundColor: '#F8FAFC',
    borderRadius: 14,
    padding: 12,
    marginVertical: 10,
    borderWidth: 1,
    borderColor: '#E2E8F0',
  },
  timelineTitle: {
    fontSize: 11,
    fontWeight: '800',
    color: '#64748B',
    marginBottom: 8,
    textTransform: 'uppercase',
    letterSpacing: 0.5,
  },
  timelineContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
  },
  timelineStep: {
    alignItems: 'center',
    flex: 1,
  },
  timelineNode: {
    width: 24,
    height: 24,
    borderRadius: 12,
    backgroundColor: '#E2E8F0',
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 4,
  },
  timelineNodeActive: {
    backgroundColor: '#4F46E5',
  },
  timelineNodeText: {
    fontSize: 11,
    fontWeight: '800',
    color: '#64748B',
  },
  timelineLabelRow: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
  },
  timelineLabel: {
    fontSize: 10,
    fontWeight: '700',
    color: '#94A3B8',
    textAlign: 'center',
  },
  timelineLabelActive: {
    color: '#4F46E5',
    fontWeight: '800',
  },
  timelineLine: {
    height: 3,
    flex: 0.6,
    backgroundColor: '#E2E8F0',
    marginBottom: 16,
  },
  timelineLineActive: {
    backgroundColor: '#4F46E5',
  },
  timelineCancelledBox: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#FEE2E2',
    padding: 8,
    borderRadius: 10,
    marginVertical: 8,
    justifyContent: 'center',
  },
  timelineCancelledText: {
    color: '#EF4444',
    fontSize: 12,
    fontWeight: '800',
  },
});