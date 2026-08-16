import { MaterialCommunityIcons } from '@expo/vector-icons';
import React from 'react';
import {
  View,
  Text,
  ScrollView,
  TouchableOpacity,
  StyleSheet,
} from 'react-native';
import { Feather } from '@expo/vector-icons';
import { Mark } from '../../../Domain/entities/Mark';
import { COLORES, ESTILOS_SOMBRA } from '../../theme/theme';

interface BrandCarouselProps {
  marcas: Mark[];
  marcaSeleccionadaId: number | null;
  onSeleccionarMarca: (id: number | null) => void;
}

type IconName = React.ComponentProps<typeof MaterialCommunityIcons>['name'];

const BRAND_ICONS: { [key: string]: IconName } = {
  nike: 'shoe-sneaker',
  apple: 'cellphone',
  samsung: 'cellphone',
  sony: 'headphones',
  dell: 'monitor',
  adidas: 'shoe-sneaker',
  puma: 'shoe-sneaker',
  infinix: 'cellphone',
};

export const BrandCarousel = ({
  marcas,
  marcaSeleccionadaId,
  onSeleccionarMarca,
}: BrandCarouselProps) => {
  const getBrandIcon = (name: string): IconName => {
    const clean = (name || '').toLowerCase();
    for (const key in BRAND_ICONS) {
      if (clean.includes(key)) {
        return BRAND_ICONS[key];
      }
    }
    return 'tag';
  };

  return (
    <View style={styles.container}>
      <View style={styles.headerRow}>
        <View style={styles.titleContainer}>
          <Feather name="tag" size={16} color="#0F172A" />
          <Text style={styles.title}>Marcas Destacadas</Text>
        </View>
        {marcaSeleccionadaId !== null && (
          <TouchableOpacity onPress={() => onSeleccionarMarca(null)} activeOpacity={0.7} style={styles.resetButton}>
            <Text style={styles.resetText}>Limpiar Filtro</Text>
            <Feather name="x" size={12} color={COLORES.primario} />
          </TouchableOpacity>
        )}
      </View>

      <ScrollView
        horizontal
        showsHorizontalScrollIndicator={false}
        contentContainerStyle={styles.scrollContent}
      >
        <TouchableOpacity
          style={[
            styles.chip,
            marcaSeleccionadaId === null ? styles.chipActive : styles.chipInactive,
            marcaSeleccionadaId === null && ESTILOS_SOMBRA.tarjeta,
          ]}
          onPress={() => onSeleccionarMarca(null)}
          activeOpacity={0.8}
        >
          <MaterialCommunityIcons
            name="grid"
            size={14}
            color={marcaSeleccionadaId === null ? '#FFFFFF' : '#334155'}
            style={styles.chipIcon}
          />
          <Text
            style={[
              styles.chipText,
              marcaSeleccionadaId === null ? styles.chipTextActive : styles.chipTextInactive,
            ]}
          >
            Todas
          </Text>
        </TouchableOpacity>

        {marcas.map((marca) => {
          const isSelected = marcaSeleccionadaId === marca.markId;
          const icon = getBrandIcon(marca.markName);

          return (
            <TouchableOpacity
              key={marca.markId}
              style={[
                styles.chip,
                isSelected ? styles.chipActive : styles.chipInactive,
                isSelected && ESTILOS_SOMBRA.tarjeta,
              ]}
              onPress={() => onSeleccionarMarca(marca.markId)}
              activeOpacity={0.8}
            >
              <MaterialCommunityIcons
                name={icon}
                size={14}
                color={isSelected ? '#FFFFFF' : '#334155'}
                style={styles.chipIcon}
              />
              <Text
                style={[
                  styles.chipText,
                  isSelected ? styles.chipTextActive : styles.chipTextInactive,
                ]}
              >
                {marca.markName}
              </Text>
            </TouchableOpacity>
          );
        })}
      </ScrollView>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    marginVertical: 10,
  },
  headerRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingHorizontal: 16,
    marginBottom: 8,
  },
  titleContainer: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  title: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#0F172A',
    letterSpacing: -0.3,
    marginLeft: 6,
  },
  resetButton: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  resetText: {
    fontSize: 12,
    fontWeight: '600',
    color: COLORES.primario,
    marginRight: 4,
  },
  scrollContent: {
    paddingHorizontal: 16,
    paddingVertical: 4,
    gap: 8,
  },
  chip: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 14,
    paddingVertical: 8,
    borderRadius: 20,
    borderWidth: 1,
  },
  chipActive: {
    backgroundColor: COLORES.primario,
    borderColor: COLORES.primario,
  },
  chipInactive: {
    backgroundColor: '#F8FAFC',
    borderColor: '#E2E8F0',
  },
  chipIcon: {
    marginRight: 6,
  },
  chipText: {
    fontSize: 13,
    fontWeight: '600',
  },
  chipTextActive: {
    color: '#FFFFFF',
  },
  chipTextInactive: {
    color: '#334155',
  },
});