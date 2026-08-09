import { useState, useEffect, useCallback } from 'react';
import { Order, OrderDetail } from '../../Domain/entities/Order';
import { User } from '../../Domain/entities/User';
import { OrderRemoteDataSource } from '../../Data/dataSources/OrderRemoteDataSource';

const orderDataSource = new OrderRemoteDataSource();
const STORAGE_KEY_ORDERS = 'NIC_STORE_LOCAL_ORDERS_V1';

export const useOrders = (usuario: User, pestañaActual: string) => {
  const [ordenes, setOrdenes] = useState<Order[]>([]);
  const [ordenesLocales, setOrdenesLocales] = useState<Order[]>([]);
  const [cargandoOrdenes, setCargandoOrdenes] = useState<boolean>(false);
  const [ordenSeleccionada, setOrdenSeleccionada] = useState<Order | null>(null);
  const [detallesOrdenSeleccionada, setDetallesOrdenSeleccionada] = useState<OrderDetail[]>([]);
  const [cargandoDetalles, setCargandoDetalles] = useState<boolean>(false);

  const idBruto = parseInt(usuario?.id || '1', 10);
  const idUsuarioNumerico = (isNaN(idBruto) || idBruto <= 0 || idBruto > 2147483647) ? 1 : idBruto;

  // Cargar ordenes locales almacenadas en el navegador al iniciar la aplicación
  useEffect(() => {
    try {
      if (typeof window !== 'undefined' && window.localStorage) {
        const saved = window.localStorage.getItem(STORAGE_KEY_ORDERS);
        if (saved) {
          const parsed: Order[] = JSON.parse(saved);
          if (Array.isArray(parsed) && parsed.length > 0) {
            setOrdenesLocales(parsed);
            setOrdenes(prev => {
              const idsExistentes = new Set(prev.map(o => o.paymentOrderId || o.ordenPagoId));
              const filtrados = parsed.filter(p => !idsExistentes.has(p.paymentOrderId || p.ordenPagoId));
              return [...filtrados, ...prev];
            });
          }
        }
      }
    } catch (e) {
      console.log('Error al cargar compras locales guardadas:', e);
    }
  }, []);

  const cargarOrdenes = useCallback(async () => {
    if (!idUsuarioNumerico) return;
    setCargandoOrdenes(true);
    try {
      const result = await orderDataSource.getOrdersByUser(idUsuarioNumerico);
      setOrdenes(prev => {
        const idsExistentes = new Set(result.map(o => o.paymentOrderId || o.ordenPagoId));
        // Fusionar compras locales (incluyendo las recuperadas de localStorage)
        const localesPrevias = prev.filter(o => !idsExistentes.has(o.paymentOrderId || o.ordenPagoId));
        return [...localesPrevias, ...result];
      });
    } catch (error) {
      console.log('Error al cargar ordenes del usuario:', error);
    } finally {
      setCargandoOrdenes(false);
    }
  }, [idUsuarioNumerico]);

  // Carga perezosa (Lazy Loading): consultar detalles solo al hacer clic en una orden específica
  const cargarDetallesOrden = async (orden: Order) => {
    setOrdenSeleccionada(orden);
    if (orden.details && orden.details.length > 0) {
      setDetallesOrdenSeleccionada(orden.details);
      return;
    }
    const orderId = orden.paymentOrderId || orden.ordenPagoId;
    if (!orderId) {
      setDetallesOrdenSeleccionada([]);
      return;
    }

    setCargandoDetalles(true);
    try {
      const detalles = await orderDataSource.getOrderDetails(orderId);
      setDetallesOrdenSeleccionada(detalles);
    } catch (error) {
      console.log('Error al cargar detalles perezosos de la orden:', error);
      setDetallesOrdenSeleccionada([]);
    } finally {
      setCargandoDetalles(false);
    }
  };

  const registrarOrdenDePago = async (
    totalAmount: number,
    paymentMethodName: string,
    addressText?: string,
    items?: OrderDetail[]
  ) => {
    // Calcular el consecutivo correlativo secuencial (ej: #103, #104, #105...)
    const idsValidos = ordenes
      .map(o => o.paymentOrderId || o.ordenPagoId || 0)
      .filter(id => id > 0 && id < 1000);
    const idSiguiente = idsValidos.length > 0 ? Math.max(...idsValidos) + 1 : (ordenes.length + 101);

    const nuevaOrden = await orderDataSource.createOrder(
      idUsuarioNumerico,
      totalAmount,
      paymentMethodName,
      idSiguiente,
      addressText,
      items
    );

    if (nuevaOrden) {
      setOrdenesLocales(prev => {
        const actualizadas = [nuevaOrden, ...prev];
        try {
          if (typeof window !== 'undefined' && window.localStorage) {
            window.localStorage.setItem(STORAGE_KEY_ORDERS, JSON.stringify(actualizadas));
          }
        } catch (e) {
          console.log('Error al guardar compras en localStorage:', e);
        }
        return actualizadas;
      });
      setOrdenes(prev => [nuevaOrden, ...prev]);
    }
  };

  const cerrarModalDetalle = () => {
    setOrdenSeleccionada(null);
    setDetallesOrdenSeleccionada([]);
  };

  useEffect(() => {
    if (pestañaActual === 'pedidos') {
      cargarOrdenes();
    }
  }, [pestañaActual, cargarOrdenes]);

  return {
    ordenes,
    cargandoOrdenes,
    ordenSeleccionada,
    detallesOrdenSeleccionada,
    cargandoDetalles,
    cargarDetallesOrden,
    cerrarModalDetalle,
    registrarOrdenDePago,
    refetch: cargarOrdenes,
  };
};
