import { ProductRepository } from '../../Domain/repositories/ProductRepository';
import { Product } from '../../Domain/entities/Product';
import { localDataSource } from '../dataSources/LocalDataSource';

export class ProductRepositoryImpl implements ProductRepository {
  async getProducts(): Promise<Product[]> {
    return localDataSource.getProducts();
  }

  async createProduct(product: Omit<Product, 'id'>): Promise<Product> {
    return localDataSource.createProduct(product);
  }

  async updateProduct(id: string, product: Partial<Product>): Promise<Product> {
    return localDataSource.updateProduct(id, product);
  }

  async deleteProduct(id: string): Promise<boolean> {
    return localDataSource.deleteProduct(id);
  }
}
