export interface CartItem {
  detalleCarritoId: number;
  carritoId: number;
  usuarioClienteId: number;
  varianteId: number;
  productoId: number;
  productoNombre: string;
  productoDescripcion?: string;
  varianteEspecificacion?: string;
  productoImagenUrl?: string;
  precioUnitario: number;
  cantidad: number;
  descuentoFila?: number;
  subTotalFila?: number;
  totalFila?: number;
  monedaISO?: string;
  monedaNombre?: string;
}
