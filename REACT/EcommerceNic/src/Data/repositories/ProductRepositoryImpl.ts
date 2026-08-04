import { ProductRepository } from '../../Domain/repositories/ProductRepository';
import { Product } from '../../Domain/entities/Product';
import { ProductsPageResponse } from '../../Domain/entities/ProductApiResponse';
import { localDataSource } from '../dataSources/LocalDataSource';
import { productRemoteDataSource, ProductRemoteDataSource } from '../dataSources/ProductRemoteDataSource';

export class ProductRepositoryImpl implements ProductRepository {
  constructor(private remoteDataSource: ProductRemoteDataSource = productRemoteDataSource) {}

  async getProducts(): Promise<Product[]> {
    return localDataSource.getProducts();
  }

  async getProductsPaged(page: number = 1, pageSize?: number, search?: string): Promise<ProductsPageResponse> {
    try {
      return await this.remoteDataSource.getProductsPaged(page, pageSize, search);
    } catch (error) {
      console.log('Error fetching paged products from API, falling back or throwing:', error);
      throw error;
    }
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
