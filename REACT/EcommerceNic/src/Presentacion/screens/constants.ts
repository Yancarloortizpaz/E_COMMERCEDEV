export interface Product {
  id: string;
  title: string;
  subtitle: string;
  numericPrice: number;
  tag: string;
  brand: string;
  category: string;
  image: string;
}

export interface Category {
  id: string;
  name: string;
}

export const CATEGORIES: Category[] = [
  { id: 'all', name: '⚡ Todo' },
  { id: 'hardware', name: '💻 Componentes' },
  { id: 'phones', name: '📱 Celulares' },
  { id: 'consoles', name: '🎮 Consolas' },
  { id: 'audio', name: '🎧 Audio' },
  { id: 'monitors', name: '🖥️ Monitores' },
];

export const PRODUCTS: Product[] = [
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

export const QUICK_REPLIES = ['📱 Celulares', '🎮 Consolas', '💻 Hardware', '🎧 Audio', '🔥 Ofertas'];

export const formatCurrency = (value: number) => 'C$' + value.toLocaleString('es-NI');

export const getBotResponse = (text: string): string => {
  const t = text.toLowerCase();

  // Samsung
  if (t.includes('samsung') || t.includes('s24') || t.includes('galaxy')) {
    return '📱 ¡Tenemos el Samsung Galaxy S24 con IA integrada y 256GB a C$31,500! Procesador Snapdragon de última generación, cámara de 200MP y batería que dura todo el día. ¿Lo agregás al carrito?';
  }
  // iPhone / Apple
  if (t.includes('iphone') || t.includes('apple') || t.includes('15 pro')) {
    return '🍎 El iPhone 15 Pro Max de 256GB en Titanio Natural está a C$39,800. Chip A17 Pro, cámara de 48MP con zoom óptico 5x y Dynamic Island. También tenemos los AirPods Pro 2 a C$9,800. ¿Cuál te interesa?';
  }
  // AirPods
  if (t.includes('airpods') || t.includes('auricular') || t.includes('audífono')) {
    return '🎧 Tenemos dos opciones top: AirPods Pro 2 a C$9,800 (chip H2, ANC, USB-C) y los Sony WH-1000XM5 a C$11,200 (30h de batería, el mejor ANC del mercado). ¿Cuál preferís?';
  }
  // Audio general
  if (t.includes('audio') || t.includes('sony') || t.includes('música') || t.includes('musica')) {
    return '🎵 En audio tenemos los AirPods Pro 2 a C$9,800 y los Sony WH-1000XM5 a C$11,200. Los Sony son los reyes del cancelado de ruido. ¡Chele, son un golazo!';
  }
  // Nintendo Switch
  if (t.includes('nintendo') || t.includes('switch') || t.includes('oled')) {
    return '🎮 ¡La Nintendo Switch OLED está a C$14,200! Pantalla OLED de 7", 64GB de almacenamiento y compatible con todos los juegos de Switch. Ideal para jugar en casa o en la calle.';
  }
  // Xbox
  if (t.includes('xbox') || t.includes('microsoft') || t.includes('series x')) {
    return '🎮 ¡El Xbox Series X está a C$19,800! 1TB SSD, gaming en 4K a 120fps y compatible con miles de juegos del Game Pass. ¿Lo anotamos, bro?';
  }
  // PlayStation / PS5
  if (t.includes('playstation') || t.includes('ps5') || t.includes('sony') || t.includes('consola')) {
    return '🎮 Tenemos 3 consolas: PS5 Slim a C$18,500, Xbox Series X a C$19,800 y Nintendo Switch OLED a C$14,200. ¿Cuál es tu estilo de juego?';
  }
  // Consolas general
  if (t.includes('consolas') || t.includes('juegos') || t.includes('gaming')) {
    return '🎮 ¡Sección gaming prendida! PS5 Slim (C$18,500), Xbox Series X (C$19,800) y Nintendo Switch OLED (C$14,200). Todas disponibles con garantía. ¿Cuál te late más?';
  }
  // RTX / GPU / Gráfica
  if (t.includes('rtx') || t.includes('nvidia') || t.includes('gráfica') || t.includes('grafica') || t.includes('gpu')) {
    return '💻 La RTX 4080 de 16GB GDDR6X está a C$42,500. Es la bestia para gaming 4K y renderizado profesional. Sombras, raytracing, DLSS 3... ¡lo tiene todo, chele!';
  }
  // Procesador / Intel / CPU
  if (t.includes('procesador') || t.includes('intel') || t.includes('i9') || t.includes('cpu')) {
    return '💻 El Intel Core i9 de 24 núcleos está a C$28,900. Perfecto para edición de video, programación y gaming de alto rendimiento. ¿Lo combinás con la RTX 4080?';
  }
  // Hardware / Componentes
  if (t.includes('hardware') || t.includes('componente') || t.includes('pc') || t.includes('computadora')) {
    return '💻 En componentes tenemos la RTX 4080 (C$42,500) y el Intel i9 (C$28,900). Si armás un PC completo con ambos, te queda una máquina de guerra. ¿Necesitás más info?';
  }
  // Monitor
  if (t.includes('monitor') || t.includes('pantalla') || t.includes('lg') || t.includes('ultrawide')) {
    return '🖥️ El Monitor LG UltraWide 34" a 144Hz y resolución WQHD IPS está en oferta a C$22,500. Ideal para gaming y trabajo. ¡Es un golazo de pantalla, chele!';
  }
  // Celulares general
  if (t.includes('celular') || t.includes('celulares') || t.includes('teléfono') || t.includes('telefono') || t.includes('smartphone')) {
    return '📱 Tenemos dos cracks: iPhone 15 Pro Max (C$39,800) y Samsung Galaxy S24 (C$31,500). El iPhone es top de gama, el Samsung es mejor relación precio-potencia. ¿Cuál te jala?';
  }
  // Ofertas
  if (t.includes('oferta') || t.includes('precio') || t.includes('barato') || t.includes('económico')) {
    return '🔥 Las mejores ofertas del momento:\n• AirPods Pro 2 → C$9,800\n• Nintendo Switch OLED → C$14,200\n• PS5 Slim → C$18,500\n• Monitor LG 34" → C$22,500\n¿Cuál agarrás, chele?';
  }
  // Hola / Saludo
  if (t.includes('hola') || t.includes('buenas') || t.includes('hey') || t.includes('qué tal')) {
    return '👋 ¡Qué hay pues, chele! Bienvenido a Nic Store. Tenemos celulares, consolas, hardware, audio y monitores. ¿En qué te puedo ayudar hoy?';
  }
  // Gracias
  if (t.includes('gracias') || t.includes('thanks') || t.includes('cheque')) {
    return '😄 ¡Con mucho gusto, chele! Para eso estamos. Si tenés más preguntas o querés ver más productos, ¡aquí estoy!';
  }

  // Respuesta por defecto
  return '🤔 Mmm no encontré eso exactamente. Puedo ayudarte con: 📱 Celulares, 🎮 Consolas, 💻 Hardware, 🎧 Audio, 🖥️ Monitores o 🔥 Ofertas. ¡Preguntame lo que querás, chele!';
};
