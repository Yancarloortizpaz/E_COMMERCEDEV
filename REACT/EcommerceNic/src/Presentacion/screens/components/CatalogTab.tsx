import React, { useState, useEffect, useRef, useMemo } from 'react';
import {
  View,
  Text,
  StyleSheet,
  FlatList,
  TextInput,
  TouchableOpacity,
  Image,
  Platform,
  Animated,
  ActivityIndicator,
} from 'react-native';
import { CATEGORIES, formatCurrency } from '../constants';
import { Product } from '../../../Domain/entities/Product';
import { useDebounce } from '../../hooks/useDebounce';
import { useProducts } from '../../hooks/useProducts';
import { ProductImage } from '../../components/ProductImage';

interface CatalogTabProps {
  products?: Product[];
  cartQuantities: { [key: string]: number };
  addUnit: (id: string) => void;
  removeUnit: (id: string) => void;
  setCurrentTab: (tab: 'home' | 'cart' | 'chatbot' | 'nosotros') => void;
  totalItemsInCart: number;
}

const SkeletonCard = () => {
  const shimmerAnim = useRef(new Animated.Value(0.3)).current;

  useEffect(() => {
    Animated.loop(
      Animated.sequence([
        Animated.timing(shimmerAnim, {
          toValue: 0.7,
          duration: 800,
          useNativeDriver: true,
        }),
        Animated.timing(shimmerAnim, {
          toValue: 0.3,
          duration: 800,
          useNativeDriver: true,
        }),
      ])
    ).start();
  }, [shimmerAnim]);

  return (
    <Animated.View style={[styles.card, { opacity: shimmerAnim }]}>
      <View style={[styles.imageWrapper, { backgroundColor: '#E2E8F0' }]} />
      <View style={styles.cardInfo}>
        <View style={{ height: 12, backgroundColor: '#E2E8F0', borderRadius: 4, width: '40%', marginBottom: 6 }} />
        <View style={{ height: 16, backgroundColor: '#E2E8F0', borderRadius: 4, width: '85%', marginBottom: 4 }} />
        <View style={{ height: 12, backgroundColor: '#E2E8F0', borderRadius: 4, width: '60%', marginBottom: 12 }} />
        <View style={{ height: 20, backgroundColor: '#E2E8F0', borderRadius: 6, width: '50%' }} />
      </View>
      <View style={{ height: 36, backgroundColor: '#E2E8F0', borderRadius: 10, marginTop: 12 }} />
    </Animated.View>
  );
};

export const CatalogTab = ({
  products: initialProducts,
  cartQuantities,
  addUnit,
  removeUnit,
  setCurrentTab,
  totalItemsInCart,
}: CatalogTabProps) => {
  const [selectedCategory, setSelectedCategory] = useState('all');
  const [search, setSearch] = useState('');
  const debouncedSearch = useDebounce(search, 400);

  // Determinar término de búsqueda para la API
  const selectedCategoryObj = CATEGORIES.find(c => c.id === selectedCategory);
  const categorySearchTerm = (selectedCategory !== 'all' && selectedCategoryObj) ? selectedCategoryObj.name : '';
  const searchTermForApi = debouncedSearch.trim().length > 0 ? debouncedSearch : categorySearchTerm;

  const {
    data,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
    isLoading,
    isError,
    error,
    refetch,
  } = useProducts(searchTermForApi);

  const displayProductsRaw: Product[] = data?.pages
    ? data.pages.flatMap(page => page.mappedProducts || [])
    : (initialProducts || []);

  const displayProducts = useMemo(() => {
    const seen = new Set<string>();
    return displayProductsRaw.filter(product => {
      if (!product || !product.id) return false;
      if (seen.has(product.id)) return false;
      seen.add(product.id);
      return true;
    });
  }, [displayProductsRaw]);

  const handleEndReached = () => {
    if (hasNextPage && !isFetchingNextPage) {
      fetchNextPage();
    }
  };

  const renderHeader = () => (
    <View>
      <View style={styles.header}>
        <View style={styles.profileSection}>
          <Image source={require('../../../../assets/logo.png')} style={styles.logoImage} resizeMode="contain" />
          <View style={styles.textContainer}>
            <Text style={styles.storeName}>Nic Store</Text>
            <Text style={styles.onlineStatus}>● En línea</Text>
          </View>
        </View>
        <View style={styles.headerIcons}>
          <TouchableOpacity style={styles.iconButton}>
            <Text style={styles.emojiIcon}>🔔</Text>
          </TouchableOpacity>
          <TouchableOpacity style={styles.iconButton} onPress={() => setCurrentTab('cart')}>
            <Text style={styles.emojiIcon}>🛒</Text>
            {totalItemsInCart > 0 && (
              <View style={styles.cartBadge}>
                <Text style={styles.cartBadgeText}>{totalItemsInCart}</Text>
              </View>
            )}
          </TouchableOpacity>
        </View>
      </View>

      <View style={styles.searchContainer}>
        <Text style={styles.searchIcon}>🔍</Text>
        <TextInput
          placeholder="Buscar productos..."
          placeholderTextColor="#94A3B8"
          value={search}
          onChangeText={setSearch}
          style={styles.searchInput}
        />
      </View>

      <View style={styles.categoriesContainer}>
        <FlatList
          horizontal
          showsHorizontalScrollIndicator={false}
          data={CATEGORIES}
          keyExtractor={(cat) => cat.id}
          renderItem={({ item: cat }) => (
            <TouchableOpacity
              onPress={() => setSelectedCategory(cat.id)}
              style={[styles.categoryPill, selectedCategory === cat.id && styles.categoryPillActive]}
            >
              <Text style={[styles.categoryText, selectedCategory === cat.id && styles.categoryTextActive]}>
                {cat.name}
              </Text>
            </TouchableOpacity>
          )}
        />
      </View>

      <View style={styles.sectionHeader}>
        <Text style={styles.sectionTitle}>Catálogo de Productos</Text>
        <TouchableOpacity onPress={() => { setSelectedCategory('all'); setSearch(''); }}>
          <Text style={styles.seeAllLink}>Ver todo</Text>
        </TouchableOpacity>
      </View>
    </View>
  );

  const renderFooter = () => {
    if (isFetchingNextPage) {
      return (
        <View style={styles.footerLoading}>
          <ActivityIndicator size="small" color="#4F46E5" />
          <Text style={styles.footerLoadingText}>Cargando más productos...</Text>
        </View>
      );
    }
    if (!hasNextPage && displayProducts.length > 0) {
      return (
        <View style={styles.footerEnd}>
          <Text style={styles.footerEndText}>📦 Has visto todos los productos disponibles</Text>
        </View>
      );
    }
    return null;
  };

  const renderItem = ({ item: product }: { item: Product }) => {
    const currentQuantity = cartQuantities[product.id] || 0;

    const handleIncrement = (item: Product) => {
      addUnit(item.id);
    };

    const handleDecrement = (item: Product) => {
      removeUnit(item.id);
    };

    return (
      <View style={styles.card}>
        <View style={styles.imageWrapper}>
          <ProductImage url={product.image} style={styles.productImage} />
          <View style={styles.tagsContainer}>
            {product.tag ? <Text style={styles.topTag}>{product.tag}</Text> : null}
            <Text style={styles.brandTag}>{product.brand.toUpperCase()}</Text>
          </View>
        </View>
        <View style={styles.cardInfo}>
          <Text style={styles.productBrand} numberOfLines={1}>{product.brand.toUpperCase()}</Text>
          <Text style={styles.productTitle} numberOfLines={1}>{product.title}</Text>
          <Text style={styles.productSubtitle} numberOfLines={1}>{product.subtitle}</Text>
        </View>

        <View style={styles.priceRow}>
          <View>
            <Text style={styles.priceLabel}>PRECIO</Text>
            <Text style={styles.productPrice}>{formatCurrency(product.numericPrice)}</Text>
          </View>
          
          {currentQuantity === 0 ? (
            <TouchableOpacity 
              style={styles.addButtonCircular} 
              activeOpacity={0.7} 
              onPress={() => handleIncrement(product)}
            >
              <Text style={styles.addButtonCircularText}>+</Text>
            </TouchableOpacity>
          ) : (
            <View style={styles.quantityContainerMini}>
              <TouchableOpacity style={styles.miniQtyBtn} onPress={() => handleDecrement(product)}>
                <Text style={styles.miniQtyBtnText}>-</Text>
              </TouchableOpacity>
              <Text style={styles.miniQtyText}>{currentQuantity}</Text>
              <TouchableOpacity style={styles.miniQtyBtn} onPress={() => handleIncrement(product)}>
                <Text style={styles.miniQtyBtnText}>+</Text>
              </TouchableOpacity>
            </View>
          )}
        </View>
      </View>
    );
  };

  return (
    <View style={styles.tabContent}>
      {isLoading ? (
        <View style={{ flex: 1 }}>
          {renderHeader()}
          <View style={styles.grid}>
            <SkeletonCard />
            <SkeletonCard />
            <SkeletonCard />
            <SkeletonCard />
          </View>
        </View>
      ) : isError ? (
        <View style={{ flex: 1 }}>
          {renderHeader()}
          <View style={styles.emptyContainer}>
            <Text style={{ fontSize: 40, marginBottom: 12 }}>📡</Text>
            <Text style={{ fontSize: 16, fontWeight: '700', color: '#0F172A' }}>Error al cargar productos</Text>
            <Text style={{ fontSize: 13, color: '#64748B', marginTop: 4, textAlign: 'center', paddingHorizontal: 20 }}>
              {(error as any)?.message || 'No se pudo conectar con el servidor.'}
            </Text>
            <TouchableOpacity style={styles.retryButton} onPress={() => refetch()}>
              <Text style={styles.retryButtonText}>Reintentar</Text>
            </TouchableOpacity>
          </View>
        </View>
      ) : (
        <FlatList
          data={displayProducts}
          keyExtractor={(item) => String(item.id)}
          numColumns={2}
          columnWrapperStyle={styles.row}
          renderItem={renderItem}
          ListHeaderComponent={renderHeader}
          ListFooterComponent={renderFooter}
          ListEmptyComponent={
            <View style={styles.emptyContainer}>
              <Text style={{ fontSize: 40, marginBottom: 12 }}>🔍</Text>
              <Text style={{ fontSize: 16, fontWeight: '700', color: '#0F172A' }}>No hay resultados</Text>
              <Text style={{ fontSize: 13, color: '#64748B', marginTop: 4 }}>Prueba con otra búsqueda o categoría</Text>
            </View>
          }
          onEndReached={handleEndReached}
          onEndReachedThreshold={0.3}
          windowSize={5}
          maxToRenderPerBatch={6}
          removeClippedSubviews={Platform.OS !== 'web'}
          contentContainerStyle={styles.scrollPadding}
        />
      )}
    </View>
  );
};

const styles = StyleSheet.create({
  tabContent: { flex: 1, paddingBottom: 68 },
  scrollPadding: { paddingBottom: 20 },
  row: { justifyContent: 'space-between', paddingHorizontal: 16 },
  header: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', paddingHorizontal: 20, paddingTop: 16, marginBottom: 16 },
  profileSection: { flexDirection: 'row', alignItems: 'center' },
  logoImage: { width: 42, height: 42, borderRadius: 12 },
  textContainer: { marginLeft: 12 },
  storeName: { fontSize: 17, fontWeight: '900', color: '#0F172A' },
  onlineStatus: { fontSize: 11, color: '#10B981', fontWeight: '700', marginTop: 1 },
  headerIcons: { flexDirection: 'row', position: 'relative' },
  iconButton: { width: 42, height: 42, backgroundColor: '#F1F5F9', borderRadius: 21, justifyContent: 'center', alignItems: 'center', marginLeft: 10 },
  emojiIcon: { fontSize: 18 },
  cartBadge: { position: 'absolute', top: -3, right: -3, backgroundColor: '#4F46E5', width: 20, height: 20, borderRadius: 10, justifyContent: 'center', alignItems: 'center', borderWidth: 2, borderColor: '#FFFFFF' },
  cartBadgeText: { color: '#FFFFFF', fontSize: 9, fontWeight: '800' },
  searchContainer: { 
    flexDirection: 'row', 
    alignItems: 'center', 
    backgroundColor: '#FFFFFF',
    marginHorizontal: 20, 
    height: 48, 
    borderRadius: 16,
    paddingHorizontal: 16, 
    marginBottom: 20, 
    borderWidth: 1.5, 
    borderColor: '#F1F5F9',
    ...Platform.select({
      ios: {
        shadowColor: '#0F172A',
        shadowOffset: { width: 0, height: 4 },
        shadowOpacity: 0.02,
        shadowRadius: 10,
      },
      android: {
        elevation: 1,
      },
    }),
  },
  searchIcon: { fontSize: 15, marginRight: 10 },
  searchInput: { 
    flex: 1, 
    fontSize: 14, 
    color: '#0F172A',
    fontWeight: '500',
    backgroundColor: 'transparent',
    ...Platform.select({
      web: {
        outlineStyle: 'none',
      } as any,
    }),
  },  
  categoriesContainer: { paddingLeft: 20, marginBottom: 20, flexDirection: 'row' },
  categoryPill: { paddingHorizontal: 18, height: 38, backgroundColor: '#F1F5F9', borderRadius: 12, justifyContent: 'center', alignItems: 'center', marginRight: 10 },
  categoryPillActive: { backgroundColor: '#4F46E5' },
  categoryText: { fontSize: 13, color: '#64748B', fontWeight: '700' },
  categoryTextActive: { color: '#FFFFFF' },
  sectionHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', paddingHorizontal: 20, marginBottom: 16 },
  sectionTitle: { fontSize: 18, fontWeight: '900', color: '#0F172A' },
  seeAllLink: { fontSize: 13, color: '#4F46E5', fontWeight: '700' },
  grid: { flexDirection: 'row', flexWrap: 'wrap', paddingHorizontal: 16, justifyContent: 'space-between' },
  card: { 
    width: '48%', 
    backgroundColor: '#FFFFFF', 
    borderRadius: 20, 
    padding: 10, 
    marginBottom: 16, 
    borderWidth: 1, 
    borderColor: '#F1F5F9',
    ...Platform.select({
      ios: {
        shadowColor: '#0F172A',
        shadowOffset: { width: 0, height: 6 },
        shadowOpacity: 0.03,
        shadowRadius: 12,
      },
      android: {
        elevation: 2,
      },
      default: {
        shadowColor: '#0F172A',
        shadowOffset: { width: 0, height: 6 },
        shadowOpacity: 0.03,
        shadowRadius: 12,
      }
    }),
  },
  imageWrapper: { width: '100%', height: 110, backgroundColor: '#F8FAFC', borderRadius: 14, overflow: 'hidden', position: 'relative' },
  productImage: { width: '100%', height: '100%' },
  tagsContainer: { position: 'absolute', top: 6, left: 6, right: 6, flexDirection: 'row', justifyContent: 'space-between' },
  topTag: { backgroundColor: '#EF4444', color: '#FFFFFF', fontSize: 9, fontWeight: '800', paddingHorizontal: 6, paddingVertical: 2, borderRadius: 6, textTransform: 'uppercase' },
  brandTag: { backgroundColor: 'rgba(15, 23, 42, 0.75)', color: '#FFFFFF', fontSize: 8, fontWeight: '800', paddingHorizontal: 6, paddingVertical: 2, borderRadius: 6, marginLeft: 'auto' },
  cardInfo: { marginTop: 10, paddingHorizontal: 2, flexGrow: 1 },
  productBrand: { fontSize: 9, fontWeight: '800', color: '#64748B', letterSpacing: 0.5, marginBottom: 2 },
  productTitle: { fontSize: 14, fontWeight: '800', color: '#0F172A' },
  productSubtitle: { fontSize: 11, color: '#64748B', marginTop: 1, height: 16 },
  
  priceRow: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    marginTop: 12,
    paddingHorizontal: 2,
    borderTopWidth: 1,
    borderColor: '#F1F5F9',
    paddingTop: 10,
  },
  priceLabel: {
    fontSize: 8,
    fontWeight: '800',
    color: '#94A3B8',
    letterSpacing: 0.5,
  },
  productPrice: { fontSize: 14, fontWeight: '900', color: '#4F46E5' },
  
  addButtonCircular: {
    width: 32,
    height: 32,
    backgroundColor: '#EEF2FF',
    borderRadius: 16,
    justifyContent: 'center',
    alignItems: 'center',
  },
  addButtonCircularText: {
    color: '#4F46E5',
    fontSize: 18,
    fontWeight: '700',
    marginTop: -2,
  },
  quantityContainerMini: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#EEF2FF',
    borderRadius: 16,
    height: 32,
    paddingHorizontal: 4,
  },
  miniQtyBtn: {
    width: 24,
    height: 24,
    borderRadius: 12,
    backgroundColor: '#FFFFFF',
    justifyContent: 'center',
    alignItems: 'center',
  },
  miniQtyBtnText: {
    color: '#4F46E5',
    fontSize: 13,
    fontWeight: '900',
  },
  miniQtyText: {
    fontSize: 12,
    fontWeight: '800',
    color: '#4F46E5',
    paddingHorizontal: 8,
  },
  emptyContainer: {
    width: '100%',
    paddingVertical: 50,
    alignItems: 'center',
    justifyContent: 'center',
  },
  retryButton: {
    marginTop: 14,
    backgroundColor: '#4F46E5',
    paddingHorizontal: 20,
    paddingVertical: 10,
    borderRadius: 12,
  },
  retryButtonText: {
    color: '#FFFFFF',
    fontWeight: '700',
    fontSize: 13,
  },
  footerLoading: {
    paddingVertical: 16,
    alignItems: 'center',
    justifyContent: 'center',
    flexDirection: 'row',
  },
  footerLoadingText: {
    marginLeft: 8,
    fontSize: 12,
    color: '#64748B',
    fontWeight: '600',
  },
  footerEnd: {
    paddingVertical: 20,
    alignItems: 'center',
  },
  footerEndText: {
    fontSize: 12,
    color: '#94A3B8',
    fontWeight: '600',
  },
});
