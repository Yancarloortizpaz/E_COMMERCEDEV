import { AuthRepository } from '../../Domain/repositories/AuthRepository';
import { User } from '../../Domain/entities/User';
import { localDataSource } from '../dataSources/LocalDataSource';

export class AuthRepositoryImpl implements AuthRepository {
  async login(email: string, password: string): Promise<User> {
    return localDataSource.login(email, password);
  }

  async register(name: string, email: string, password: string, role?: 'user' | 'admin'): Promise<User> {
    return localDataSource.register(name, email, password, role);
  }

  async getUsers(): Promise<User[]> {
    return localDataSource.getUsers();
  }
}
