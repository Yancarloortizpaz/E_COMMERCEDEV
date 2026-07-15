import { User } from '../entities/User';

export interface AuthRepository {
  login(email: string, password: string): Promise<User>;
  register(name: string, email: string, password: string, role?: 'user' | 'admin'): Promise<User>;
  getUsers(): Promise<User[]>;
}
