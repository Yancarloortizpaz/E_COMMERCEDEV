/**
 * SCRIPT DE PROCESAMIENTO Y CONVERSIÓN DE IMÁGENES
 * 
 * Uso: node process_images.js
 * 
 * Este script automatiza la copia y preparación de imágenes desde la carpeta
 * de desarrollo (seeders/images) hacia la carpeta pública del servidor backend (uploads/products).
 */

const fs = require('fs');
const path = require('path');

// Directorio origen (Semillas en desarrollo)
const SOURCE_DIR = path.join(__dirname, 'images');

// Directorios destino en el servidor
const TARGET_FULL_DIR = path.join(__dirname, '..', '..', '..', 'server', 'public', 'uploads', 'products', 'full');
const TARGET_THUMB_DIR = path.join(__dirname, '..', '..', '..', 'server', 'public', 'uploads', 'products', 'thumbs');

// Crear directorios si no existen
[TARGET_FULL_DIR, TARGET_THUMB_DIR].forEach(dir => {
  if (!fs.existsSync(dir)) {
    fs.mkdirSync(dir, { recursive: true });
    console.log(`📁 Directorio creado: ${dir}`);
  }
});

// Procesar imágenes
if (fs.existsSync(SOURCE_DIR)) {
  const files = fs.readdirSync(SOURCE_DIR);
  
  files.forEach(file => {
    if (/\.(jpg|jpeg|png|webp)$/i.test(file)) {
      const srcPath = path.join(SOURCE_DIR, file);
      const destFull = path.join(TARGET_FULL_DIR, file);
      const destThumb = path.join(TARGET_THUMB_DIR, file);

      fs.copyFileSync(srcPath, destFull);
      fs.copyFileSync(srcPath, destThumb);
      console.log(`✅ Procesada correctamente: ${file}`);
    }
  });

  console.log('\n🎉 ¡Proceso de sincronización finalizado exitosamente!');
} else {
  console.error(`❌ El directorio origen ${SOURCE_DIR} no existe.`);
}
