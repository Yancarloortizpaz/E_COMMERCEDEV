import { Product } from '../entities/Product';

export interface ProductRepository {
  getProducts(): Promise<Product[]>;
  createProduct(product: Omit<Product, 'id'>): Promise<Product>;
  updateProduct(id: string, product: Partial<Product>): Promise<Product>;
  deleteProduct(id: string): Promise<boolean>;
}
