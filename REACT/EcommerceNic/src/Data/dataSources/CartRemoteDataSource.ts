import { API_CONFIG, safeFetch } from './apiConfig';
import { CartItem } from '../../Domain/entities/CartItem';

export class CartRemoteDataSource {
  async getCartByUser(userId: number): Promise<CartItem[]> {
    try {
      const response = await safeFetch<{ codigo: number; msj: string; data: CartItem[] }>(
        `${API_CONFIG.BASE_URL}/api/CartDetails/obtener_carrito_cliente/${userId}`
      );
      return response.data || [];
    } catch (error: any) {
      if (error.message && error.message.includes('404')) {
        return [];
      }
      console.log('Error al obtener carrito del cliente:', error);
      return [];
    }
  }

  async addToCart(userId: number, productVariableId: number, quantity: number = 1): Promise<boolean> {
    try {
      const body = {
        userId,
        productVariableId,
        quantity,
        discount: 0,
        creatorId: userId,
        statusId: true,
      };

      const response = await safeFetch<{ codigo: number; msj: string }>(
        `${API_CONFIG.BASE_URL}/api/CartDetails/insertar`,
        {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(body),
        }
      );
      return response.codigo === 200 || response.codigo === 201;
    } catch (error) {
      console.log('Error al agregar al carrito via C# API:', error);
      return false;
    }
  }

  async updateCartQuantity(cartDetailId: number, newQuantity: number, modificatorId: number): Promise<boolean> {
    try {
      const body = {
        cartDetailId,
        newQuantity,
        modificatorId,
      };

      const response = await safeFetch<{ codigo: number; msj: string }>(
        `${API_CONFIG.BASE_URL}/api/CartDetails/actualizar`,
        {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(body),
        }
      );
      return response.codigo === 200;
    } catch (error) {
      console.log('Error al actualizar cantidad via C# API:', error);
      return false;
    }
  }

  async deleteCartItem(cartDetailId: number, modificatorId: number): Promise<boolean> {
    try {
      const response = await safeFetch<{ codigo: number; msj: string }>(
        `${API_CONFIG.BASE_URL}/api/CartDetails/${cartDetailId}/${modificatorId}`,
        {
          method: 'DELETE',
        }
      );
      return response.codigo === 200;
    } catch (error) {
      console.log('Error al eliminar del carrito via C# API:', error);
      return false;
    }
  }
}

export const cartRemoteDataSource = new CartRemoteDataSource();
