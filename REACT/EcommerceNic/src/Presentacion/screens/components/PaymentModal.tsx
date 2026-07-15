import React, { useState } from 'react';
import {
  View,
  Text,
  StyleSheet,
  Modal,
  TouchableOpacity,
  TextInput,
  KeyboardAvoidingView,
  Platform,
  Alert,
} from 'react-native';
import { formatCurrency } from '../constants';

interface PaymentModalProps {
  isVisible: boolean;
  onClose: () => void;
  totalPayment: number;
  onPaymentSuccess: (method: string) => void;
}

export const PaymentModal = ({
  isVisible,
  onClose,
  totalPayment,
  onPaymentSuccess,
}: PaymentModalProps) => {
  const [paymentStep, setPaymentStep] = useState<'select' | 'card' | 'transfer' | 'cash'>('select');
  
  // Card form local state
  const [cardNumber, setCardNumber] = useState('');
  const [cardExpiry, setCardExpiry] = useState('');
  const [cardCvv, setCardCvv] = useState('');
  
  // Cash form local state
  const [deliveryAddress, setDeliveryAddress] = useState('');

  const resetForm = () => {
    setPaymentStep('select');
    setCardNumber('');
    setCardExpiry('');
    setCardCvv('');
    setDeliveryAddress('');
  };

  const handleClose = () => {
    resetForm();
    onClose();
  };

  const handleConfirm = (method: string) => {
    if (method === 'Tarjeta') {
      if (cardNumber.replace(/\s/g, '').length < 16) {
        Alert.alert('Número incompleto', 'Ingresá los 16 dígitos de tu tarjeta.');
        return;
      }
      if (!cardExpiry || cardExpiry.length < 5) {
        Alert.alert('Fecha requerida', 'Ingresá la fecha de vencimiento válida (MM/AA).');
        return;
      }
      if (cardCvv.length < 3) {
        Alert.alert('CVV inválido', 'El CVV debe tener 3 dígitos.');
        return;
      }
    }
    if (method === 'Efectivo' && !deliveryAddress.trim()) {
      Alert.alert('Dirección requerida', 'Por favor ingresá tu dirección de entrega.');
      return;
    }

    // Callback to parent screen with the payment method
    onPaymentSuccess(method);
    resetForm();
  };

  // Helper formatting for credit card
  const formatCardNumber = (text: string) => {
    const cleanText = text.replace(/\D/g, '');
    let formatted = '';
    for (let i = 0; i < cleanText.length; i++) {
      if (i > 0 && i % 4 === 0) {
        formatted += ' ';
      }
      formatted += cleanText[i];
    }
    setCardNumber(formatted);
  };

  const formatExpiry = (text: string) => {
    const cleanText = text.replace(/\D/g, '');
    if (cleanText.length > 2) {
      setCardExpiry(`${cleanText.slice(0, 2)}/${cleanText.slice(2, 4)}`);
    } else {
      setCardExpiry(cleanText);
    }
  };

  return (
    <Modal
      animationType="slide"
      transparent={true}
      visible={isVisible}
      onRequestClose={handleClose}
    >
      <View style={styles.modalOverlay}>
        <KeyboardAvoidingView behavior={Platform.OS === 'ios' ? 'padding' : 'height'} style={{ width: '100%' }}>
          <View style={styles.modalContent}>

            {/* PANTALLA: Selección de método */}
            {paymentStep === 'select' && (
              <>
                <View style={styles.modalHeader}>
                  <Text style={styles.modalTitle}>Método de Pago</Text>
                  <Text style={styles.modalSubtitle}>
                    Total a pagar: <Text style={styles.modalTotalAmount}>{formatCurrency(totalPayment)}</Text>
                  </Text>
                </View>

                <TouchableOpacity style={styles.paymentOption} onPress={() => setPaymentStep('card')} activeOpacity={0.7}>
                  <View style={styles.paymentIconBox}>
                    <Text style={styles.paymentIconEmoji}>💳</Text>
                  </View>
                  <View style={{ flex: 1 }}>
                    <Text style={styles.paymentMethodName}>Tarjeta de Crédito / Débito</Text>
                    <Text style={styles.paymentMethodDesc}>Paga seguro con Visa o Mastercard</Text>
                  </View>
                  <Text style={styles.arrowIcon}>→</Text>
                </TouchableOpacity>

                <TouchableOpacity style={styles.paymentOption} onPress={() => setPaymentStep('transfer')} activeOpacity={0.7}>
                  <View style={styles.paymentIconBox}>
                    <Text style={styles.paymentIconEmoji}>🏦</Text>
                  </View>
                  <View style={{ flex: 1 }}>
                    <Text style={styles.paymentMethodName}>Transferencia Bancaria</Text>
                    <Text style={styles.paymentMethodDesc}>BAC, LAFISE, Banpro</Text>
                  </View>
                  <Text style={styles.arrowIcon}>→</Text>
                </TouchableOpacity>

                <TouchableOpacity style={styles.paymentOption} onPress={() => setPaymentStep('cash')} activeOpacity={0.7}>
                  <View style={styles.paymentIconBox}>
                    <Text style={styles.paymentIconEmoji}>💵</Text>
                  </View>
                  <View style={{ flex: 1 }}>
                    <Text style={styles.paymentMethodName}>Efectivo Contra Entrega</Text>
                    <Text style={styles.paymentMethodDesc}>Paga al recibir tus productos</Text>
                  </View>
                  <Text style={styles.arrowIcon}>→</Text>
                </TouchableOpacity>

                <TouchableOpacity style={styles.modalCancelBtn} onPress={handleClose} activeOpacity={0.7}>
                  <Text style={styles.modalCancelText}>Cancelar Compra</Text>
                </TouchableOpacity>
              </>
            )}

            {/* PANTALLA: Tarjeta */}
            {paymentStep === 'card' && (
              <>
                <View style={styles.modalHeader}>
                  <TouchableOpacity onPress={() => setPaymentStep('select')} style={styles.backButtonSmall} activeOpacity={0.7}>
                    <Text style={styles.backButtonSmallText}>← Cambiar método</Text>
                  </TouchableOpacity>
                  <Text style={styles.modalTitle}>Pago con Tarjeta</Text>
                  <Text style={styles.modalSubtitle}>
                    Total: <Text style={styles.modalTotalAmount}>{formatCurrency(totalPayment)}</Text>
                  </Text>
                </View>

                {/* Tarjeta de Crédito Visual Premium */}
                <View style={styles.visualCard}>
                  <View style={styles.visualCardHeader}>
                    <View style={styles.visualCardChip} />
                    <Text style={styles.visualCardType}>VISA</Text>
                  </View>
                  <Text style={styles.visualCardNumber}>
                    {cardNumber || '•••• •••• •••• ••••'}
                  </Text>
                  <View style={styles.visualCardFooter}>
                    <View>
                      <Text style={styles.visualCardLabel}>TITULAR</Text>
                      <Text style={styles.visualCardValue}>CLIENTE NIC STORE</Text>
                    </View>
                    <View style={{ marginRight: 20 }}>
                      <Text style={styles.visualCardLabel}>VENCE</Text>
                      <Text style={styles.visualCardValue}>{cardExpiry || 'MM/AA'}</Text>
                    </View>
                    <View>
                      <Text style={styles.visualCardLabel}>CVV</Text>
                      <Text style={styles.visualCardValue}>{cardCvv ? '***' : '•••'}</Text>
                    </View>
                  </View>
                </View>

                <Text style={styles.payFormLabel}>Número de Tarjeta</Text>
                <TextInput
                  style={styles.payFormInput}
                  placeholder="1234 5678 9012 3456"
                  placeholderTextColor="#94A3B8"
                  value={cardNumber}
                  onChangeText={formatCardNumber}
                  keyboardType="numeric"
                  maxLength={19}
                />

                <View style={styles.payFormRow}>
                  <View style={{ flex: 1, marginRight: 8 }}>
                    <Text style={styles.payFormLabel}>Fecha (MM/AA)</Text>
                    <TextInput
                      style={styles.payFormInput}
                      placeholder="12/27"
                      placeholderTextColor="#94A3B8"
                      value={cardExpiry}
                      onChangeText={formatExpiry}
                      keyboardType="numeric"
                      maxLength={5}
                    />
                  </View>
                  <View style={{ flex: 1, marginLeft: 8 }}>
                    <Text style={styles.payFormLabel}>CVV</Text>
                    <TextInput
                      style={styles.payFormInput}
                      placeholder="123"
                      placeholderTextColor="#94A3B8"
                      value={cardCvv}
                      onChangeText={(t) => setCardCvv(t.replace(/\D/g, ''))}
                      keyboardType="numeric"
                      maxLength={3}
                      secureTextEntry
                    />
                  </View>
                </View>

                <TouchableOpacity style={styles.payButton} activeOpacity={0.8} onPress={() => handleConfirm('Tarjeta')}>
                  <Text style={styles.payButtonText}>Confirmar Pago • {formatCurrency(totalPayment)}</Text>
                </TouchableOpacity>
              </>
            )}

            {/* PANTALLA: Transferencia */}
            {paymentStep === 'transfer' && (
              <>
                <View style={styles.modalHeader}>
                  <TouchableOpacity onPress={() => setPaymentStep('select')} style={styles.backButtonSmall} activeOpacity={0.7}>
                    <Text style={styles.backButtonSmallText}>← Cambiar método</Text>
                  </TouchableOpacity>
                  <Text style={styles.modalTitle}>Transferencia Bancaria</Text>
                  <Text style={styles.modalSubtitle}>
                    Monto total: <Text style={styles.modalTotalAmount}>{formatCurrency(totalPayment)}</Text>
                  </Text>
                </View>

                <ScrollView style={{ maxHeight: 250 }} showsVerticalScrollIndicator={false}>
                  <View style={styles.bankInfoCard}>
                    <Text style={styles.bankName}>🏦 BAC Credomatic</Text>
                    <Text style={styles.bankDetail}>Cuenta Corriente C$: <Text style={styles.bankNumber}>100-232456-7</Text></Text>
                    <Text style={styles.bankDetail}>A nombre de: <Text style={styles.bankNumber}>Nic Store S.A.</Text></Text>
                  </View>

                  <View style={styles.bankInfoCard}>
                    <Text style={styles.bankName}>🏦 Banpro Grupo Promerica</Text>
                    <Text style={styles.bankDetail}>Cuenta Ahorro C$: <Text style={styles.bankNumber}>2004-5678-9321</Text></Text>
                    <Text style={styles.bankDetail}>A nombre de: <Text style={styles.bankNumber}>Nic Store S.A.</Text></Text>
                  </View>

                  <View style={styles.bankInfoCard}>
                    <Text style={styles.bankName}>🏦 LAFISE Bancentro</Text>
                    <Text style={styles.bankDetail}>Cuenta Corriente C$: <Text style={styles.bankNumber}>001-7894-4567</Text></Text>
                    <Text style={styles.bankDetail}>A nombre de: <Text style={styles.bankNumber}>Nic Store S.A.</Text></Text>
                  </View>
                </ScrollView>

                <View style={styles.transferNoteContainer}>
                  <Text style={styles.transferNote}>
                    📎 Envía tu comprobante vía WhatsApp al <Text style={{ fontWeight: '800', color: '#B45309' }}>+505 8888 8888</Text> indicando tu número de orden para procesar tu envío.
                  </Text>
                </View>

                <TouchableOpacity style={styles.payButton} activeOpacity={0.8} onPress={() => handleConfirm('Transferencia Bancaria')}>
                  <Text style={styles.payButtonText}>Confirmar Transferencia</Text>
                </TouchableOpacity>
              </>
            )}

            {/* PANTALLA: Efectivo */}
            {paymentStep === 'cash' && (
              <>
                <View style={styles.modalHeader}>
                  <TouchableOpacity onPress={() => setPaymentStep('select')} style={styles.backButtonSmall} activeOpacity={0.7}>
                    <Text style={styles.backButtonSmallText}>← Cambiar método</Text>
                  </TouchableOpacity>
                  <Text style={styles.modalTitle}>Pago Contra Entrega</Text>
                  <Text style={styles.modalSubtitle}>
                    Monto a pagar al recibir: <Text style={styles.modalTotalAmount}>{formatCurrency(totalPayment)}</Text>
                  </Text>
                </View>

                <Text style={styles.payFormLabel}>Dirección de Entrega en Nicaragua</Text>
                <TextInput
                  style={[styles.payFormInput, styles.payFormInputMulti]}
                  placeholder="Ej: De donde fue el Cine González 2 cuadras abajo, 1 cuadra al sur. Managua, Nicaragua."
                  placeholderTextColor="#94A3B8"
                  value={deliveryAddress}
                  onChangeText={setDeliveryAddress}
                  multiline
                  numberOfLines={3}
                />

                <View style={styles.transferNoteContainer}>
                  <Text style={styles.transferNote}>
                    🛵 El repartidor de Nic Store llegará en 24 a 48 horas hábiles. Asegúrate de tener el monto exacto listo al recibir tu paquete.
                  </Text>
                </View>

                <TouchableOpacity style={styles.payButton} activeOpacity={0.8} onPress={() => handleConfirm('Efectivo')}>
                  <Text style={styles.payButtonText}>Confirmar Pedido Contra Entrega</Text>
                </TouchableOpacity>
              </>
            )}

          </View>
        </KeyboardAvoidingView>
      </View>
    </Modal>
  );
};

const styles = StyleSheet.create({
  modalOverlay: { flex: 1, backgroundColor: 'rgba(15, 23, 42, 0.65)', justifyContent: 'flex-end' },
  modalContent: { 
    backgroundColor: '#FFFFFF', 
    borderTopLeftRadius: 28, 
    borderTopRightRadius: 28, 
    padding: 24, 
    paddingBottom: Platform.OS === 'ios' ? 44 : 28, 
    shadowColor: '#000', 
    shadowOffset: { width: 0, height: -10 }, 
    shadowOpacity: 0.08, 
    shadowRadius: 24, 
    elevation: 24 
  },
  modalHeader: { marginBottom: 18 },
  modalTitle: { fontSize: 20, fontWeight: '950', color: '#0F172A', marginBottom: 2 },
  modalSubtitle: { fontSize: 13, color: '#64748B', fontWeight: '600' },
  modalTotalAmount: { color: '#4F46E5', fontWeight: '850', fontSize: 16 },
  
  paymentOption: { 
    flexDirection: 'row', 
    alignItems: 'center', 
    backgroundColor: '#F8FAFC', 
    borderWidth: 1.5, 
    borderColor: '#F1F5F9', 
    borderRadius: 20, 
    padding: 16, 
    marginBottom: 12 
  },
  paymentIconBox: { width: 44, height: 44, backgroundColor: '#FFFFFF', borderRadius: 14, justifyContent: 'center', alignItems: 'center', marginRight: 14, borderWidth: 1.5, borderColor: '#F1F5F9' },
  paymentIconEmoji: { fontSize: 20 },
  paymentMethodName: { fontSize: 14, fontWeight: '800', color: '#0F172A', marginBottom: 2 },
  paymentMethodDesc: { fontSize: 12, color: '#94A3B8', fontWeight: '600' },
  arrowIcon: { fontSize: 16, color: '#94A3B8', fontWeight: '700', marginLeft: 8 },
  
  modalCancelBtn: { marginTop: 12, height: 48, justifyContent: 'center', alignItems: 'center', borderRadius: 24, backgroundColor: '#FEF2F2' },
  modalCancelText: { color: '#EF4444', fontSize: 14, fontWeight: '800' },
  
  backButtonSmall: { marginBottom: 10, alignSelf: 'flex-start' },
  backButtonSmallText: { color: '#4F46E5', fontSize: 13, fontWeight: '800' },
  
  payFormLabel: { fontSize: 12, fontWeight: '800', color: '#334155', marginBottom: 6, marginTop: 12, textTransform: 'uppercase', letterSpacing: 0.5 },
  payFormInput: { 
    backgroundColor: '#F8FAFC', 
    borderWidth: 1.5, 
    borderColor: '#E2E8F0', 
    borderRadius: 14, 
    height: 48, 
    paddingHorizontal: 16, 
    fontSize: 15, 
    color: '#0F172A',
    fontWeight: '500',
    ...Platform.select({
      web: { outlineStyle: 'none' } as any,
    }),
  },
  payFormInputMulti: { height: 75, paddingTop: 12, textAlignVertical: 'top' },
  payFormRow: { flexDirection: 'row', marginTop: 4 },
  
  // Interactive Visual Card
  visualCard: {
    backgroundColor: '#0F172A', // Slate-900 high contrast dark card
    borderRadius: 20,
    padding: 20,
    height: 160,
    marginBottom: 20,
    justifyContent: 'space-between',
    ...Platform.select({
      ios: {
        shadowColor: '#4F46E5',
        shadowOffset: { width: 0, height: 6 },
        shadowOpacity: 0.15,
        shadowRadius: 12,
      },
      android: {
        elevation: 6,
      },
    }),
  },
  visualCardHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' },
  visualCardChip: { width: 34, height: 26, backgroundColor: '#F59E0B', borderRadius: 6, opacity: 0.8 },
  visualCardType: { color: '#FFFFFF', fontSize: 18, fontWeight: '950', fontStyle: 'italic' },
  visualCardNumber: { color: '#FFFFFF', fontSize: 18, fontWeight: '700', letterSpacing: 2, marginVertical: 10, textAlign: 'center' },
  visualCardFooter: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' },
  visualCardLabel: { color: '#64748B', fontSize: 7, fontWeight: '800', letterSpacing: 0.5, marginBottom: 2 },
  visualCardValue: { color: '#FFFFFF', fontSize: 11, fontWeight: '700' },

  bankInfoCard: { backgroundColor: '#F8FAFC', borderRadius: 16, padding: 14, marginBottom: 10, borderWidth: 1.5, borderColor: '#F1F5F9' },
  bankName: { fontSize: 13, fontWeight: '800', color: '#0F172A', marginBottom: 4 },
  bankDetail: { fontSize: 12, color: '#64748B', marginBottom: 1, fontWeight: '500' },
  bankNumber: { color: '#4F46E5', fontWeight: '800' },
  
  transferNoteContainer: {
    backgroundColor: '#FFFBEB',
    borderWidth: 1,
    borderColor: '#FEF3C7',
    borderRadius: 14,
    padding: 12,
    marginTop: 14,
    marginBottom: 6,
  },
  transferNote: { fontSize: 12, color: '#B45309', fontWeight: '600', lineHeight: 18 },
  payButton: { 
    backgroundColor: '#4F46E5', 
    height: 50, 
    borderRadius: 25, 
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
  payButtonText: { color: '#FFFFFF', fontSize: 14, fontWeight: '800' },
});
