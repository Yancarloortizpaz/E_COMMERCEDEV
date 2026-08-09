export interface OrderDetail {
  paymentOrderDetailId?: number;
  detalleOrdenPagoId?: number;
  paymentOrderId?: number;
  ordenPagoId?: number;
  productVariableId?: number;
  varianteId?: number;
  productName?: string;
  productoNombre?: string;
  productDescription?: string;
  productoDescripcion?: string;
  productImageURL?: string;
  productoImagenUrl?: string;
  price?: number;
  precioUnitario?: number;
  quantity?: number;
  cantidad?: number;
  subTotal?: number;
  subTotalFila?: number;
  tax?: number;
  impuestoFila?: number;
  total?: number;
  totalFila?: number;
  currencyISO?: string;
  monedaISO?: string;
}

export interface Order {
  paymentOrderId?: number;
  ordenPagoId?: number;
  userId?: number;
  usuarioId?: number;
  orderDate?: string;
  fechaOrden?: string;
  statusId?: number;
  estadoId?: number;
  statusName?: string;
  estadoNombre?: string;
  paymentMethodName?: string;
  metodoPagoNombre?: string;
  addressText?: string;
  direccionTexto?: string;
  subTotal?: number;
  subTotalOrden?: number;
  shippingCost?: number;
  costoEnvio?: number;
  tax?: number;
  impuestoOrden?: number;
  totalAmount?: number;
  totalOrden?: number;
  currencyISO?: string;
  monedaISO?: string;
  details?: OrderDetail[];
}
