import { User } from '../../Domain/entities/User';
import { Product } from '../../Domain/entities/Product';

class LocalDataSource {
  private users: User[] = [
    { id: '1', email: 'admin@nicstore.com', password: '123456', name: 'Admin Nic Store', role: 'admin' },
    { id: '2', email: 'user@nicstore.com', password: '123456', name: 'Chele User', role: 'user' },
  ];

  private products: Product[] = [
    {
      id: '1',
      title: 'Gráfica RTX 4080',
      subtitle: '16GB GDDR6X Ultra Potencia',
      numericPrice: 42500,
      tag: '🔥 Top',
      brand: 'NVIDIA',
      category: 'hardware',
      image: 'https://images.unsplash.com/photo-1610563166150-b34df4f3bcd6?q=80&w=800&auto=format&fit=crop',
    },
    {
      id: '2',
      title: 'Procesador Intel i9',
      subtitle: '24 Núcleos · Alto Rendimiento',
      numericPrice: 28900,
      tag: 'Nuevo',
      brand: 'Intel',
      category: 'hardware',
      image: 'https://images.unsplash.com/photo-1591488320449-011701bb6704?q=80&w=800&auto=format&fit=crop',
    },
    {
      id: '3',
      title: 'PlayStation 5 Slim',
      subtitle: '1TB SSD · Edición Digital',
      numericPrice: 18500,
      tag: 'Popular',
      brand: 'Sony',
      category: 'consoles',
      image: 'https://images.unsplash.com/photo-1606813907291-d86efa9b94db?q=80&w=800&auto=format&fit=crop',
    },
    {
      id: '4',
      title: 'iPhone 15 Pro Max',
      subtitle: '256GB · Titanio Natural',
      numericPrice: 39800,
      tag: '🔥 Top',
      brand: 'Apple',
      category: 'phones',
      image: 'https://images.unsplash.com/photo-1695048133142-1a20484d2569?q=80&w=800&auto=format&fit=crop',
    },
    {
      id: '5',
      title: 'Samsung Galaxy S24',
      subtitle: '256GB · IA Integrada',
      numericPrice: 31500,
      tag: 'Nuevo',
      brand: 'Samsung',
      category: 'phones',
      image: 'https://images.unsplash.com/photo-1706528820702-fd7a24dfba52?q=80&w=800&auto=format&fit=crop',
    },
    {
      id: '6',
      title: 'Nintendo Switch OLED',
      subtitle: 'Pantalla OLED 7" · 64GB',
      numericPrice: 14200,
      tag: 'Popular',
      brand: 'Nintendo',
      category: 'consoles',
      image: 'https://images.unsplash.com/photo-1578303512597-81e6cc155b3e?q=80&w=800&auto=format&fit=crop',
    },
    {
      id: '7',
      title: 'Xbox Series X',
      subtitle: '1TB SSD · 4K Gaming',
      numericPrice: 19800,
      tag: '🔥 Top',
      brand: 'Microsoft',
      category: 'consoles',
      image: 'https://images.unsplash.com/photo-1621259182978-fbf93132d53d?q=80&w=800&auto=format&fit=crop',
    },
    {
      id: '8',
      title: 'AirPods Pro 2',
      subtitle: 'ANC · Chip H2 · USB-C',
      numericPrice: 9800,
      tag: 'Nuevo',
      brand: 'Apple',
      category: 'audio',
      image: 'https://images.unsplash.com/photo-1600294037681-c80b4cb5b434?q=80&w=800&auto=format&fit=crop',
    },
    {
      id: '9',
      title: 'Monitor LG UltraWide',
      subtitle: '34" · 144Hz · WQHD IPS',
      numericPrice: 22500,
      tag: 'Oferta',
      brand: 'LG',
      category: 'monitors',
      image: 'https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?q=80&w=800&auto=format&fit=crop',
    },
    {
      id: '10',
      title: 'Sony WH-1000XM5',
      subtitle: 'ANC Líder · 30h Batería',
      numericPrice: 11200,
      tag: 'Popular',
      brand: 'Sony',
      category: 'audio',
      image: 'https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?q=80&w=800&auto=format&fit=crop',
    },
  ];

  // Helper delay
  private delay(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  // Auth Operations
  async login(email: string, password: string): Promise<User> {
    await this.delay(1000);
    const user = this.users.find(
      u => u.email.toLowerCase().trim() === email.toLowerCase().trim()
    );
    if (!user) {
      throw new Error('Este correo electrónico no está registrado.');
    }
    if (user.password !== password) {
      throw new Error('Contraseña incorrecta. Verifícala e intenta de nuevo.');
    }
    return user;
  }

  async register(name: string, email: string, password: string, role: 'user' | 'admin' = 'user'): Promise<User> {
    await this.delay(1000);
    const exists = this.users.some(u => u.email.toLowerCase().trim() === email.toLowerCase().trim());
    if (exists) {
      throw new Error('Este correo electrónico ya está registrado.');
    }
    const newUser: User = {
      id: Math.random().toString(),
      name,
      email,
      password,
      role,
    };
    this.users.push(newUser);
    return newUser;
  }

  async getUsers(): Promise<User[]> {
    await this.delay(500);
    return [...this.users];
  }

  // Product Operations
  async getProducts(): Promise<Product[]> {
    await this.delay(800);
    return [...this.products];
  }

  async createProduct(product: Omit<Product, 'id'>): Promise<Product> {
    await this.delay(800);
    const newProduct: Product = {
      ...product,
      id: Math.random().toString(),
    };
    this.products.unshift(newProduct); // Add new products at the top
    return newProduct;
  }

  async updateProduct(id: string, updatedFields: Partial<Product>): Promise<Product> {
    await this.delay(800);
    const index = this.products.findIndex(p => p.id === id);
    if (index === -1) {
      throw new Error('Producto no encontrado');
    }
    const updatedProduct = {
      ...this.products[index],
      ...updatedFields,
    };
    this.products[index] = updatedProduct;
    return updatedProduct;
  }

  async deleteProduct(id: string): Promise<boolean> {
    await this.delay(800);
    const exists = this.products.some(p => p.id === id);
    if (!exists) {
      return false;
    }
    this.products = this.products.filter(p => p.id !== id);
    return true;
  }
}

export const localDataSource = new LocalDataSource();
