import { AuthRepositoryImpl } from '../Data/repositories/AuthRepositoryImpl';
import { ProductRepositoryImpl } from '../Data/repositories/ProductRepositoryImpl';
import { DashboardRepositoryImpl } from '../Data/repositories/DashboardRepositoryImpl';
import { LoginUseCase } from '../Domain/useCases/LoginUseCase';
import { RegisterUseCase } from '../Domain/useCases/RegisterUseCase';
import { GetProductsUseCase } from '../Domain/useCases/GetProductsUseCase';
import { CreateProductUseCase } from '../Domain/useCases/CreateProductUseCase';
import { UpdateProductUseCase } from '../Domain/useCases/UpdateProductUseCase';
import { DeleteProductUseCase } from '../Domain/useCases/DeleteProductUseCase';
import { GetDashboardMetricsUseCase } from '../Domain/useCases/GetDashboardMetricsUseCase';

// Instantiate single instances of the Repository implementations
const authRepository = new AuthRepositoryImpl();
const productRepository = new ProductRepositoryImpl();
const dashboardRepository = new DashboardRepositoryImpl();

// Instantiate Use Cases
export const loginUseCase = new LoginUseCase(authRepository);
export const registerUseCase = new RegisterUseCase(authRepository);
export const getProductsUseCase = new GetProductsUseCase(productRepository);
export const createProductUseCase = new CreateProductUseCase(productRepository);
export const updateProductUseCase = new UpdateProductUseCase(productRepository);
export const deleteProductUseCase = new DeleteProductUseCase(productRepository);
export const getDashboardMetricsUseCase = new GetDashboardMetricsUseCase(dashboardRepository);

