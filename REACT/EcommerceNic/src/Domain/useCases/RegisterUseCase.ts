import { AuthRepository } from '../repositories/AuthRepository';

export class RegisterUseCase {
  private authRepository: AuthRepository;

  constructor(authRepository: AuthRepository) {
    this.authRepository = authRepository;
  }

  async execute(name: string, email: string, password: string, role?: 'user' | 'admin') {
    return this.authRepository.register(name, email, password, role);
  }
}
