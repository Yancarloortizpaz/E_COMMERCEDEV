import React, { useState } from 'react';
import { Image, View, ActivityIndicator, StyleSheet, ImageStyle, ViewStyle } from 'react-native';
import { API_CONFIG } from '../../Data/dataSources/apiConfig';

const FALLBACK_LOGO = require('../../../assets/logo.png');

interface Props {
  url: string | undefined | null;
  style: ImageStyle;
  containerStyle?: ViewStyle;
}

export const ProductImage = React.memo(({ url, style, containerStyle }: Props) => {
  const [hasError, setHasError] = useState(false);
  const [isLoading, setIsLoading] = useState(!!url);

  let finalUrl: string | undefined = url || undefined;
  if (finalUrl && finalUrl.startsWith('/')) {
    finalUrl = `${API_CONFIG.BASE_URL}${finalUrl}`;
  }

  const isValidUrl = Boolean(finalUrl && finalUrl.trim().length > 0 && !hasError);

  return (
    <View style={[styles.container, style, containerStyle]}>
      {isLoading && isValidUrl && (
        <View style={[StyleSheet.absoluteFill, styles.skeleton]}>
          <ActivityIndicator size="small" color="#94A3B8" />
        </View>
      )}

      {isValidUrl && finalUrl ? (
        <Image
          source={{ uri: finalUrl }}
          style={[StyleSheet.absoluteFill, style]}
          resizeMode="cover"
          onLoad={() => setIsLoading(false)}
          onError={() => {
            setHasError(true);
            setIsLoading(false);
          }}
        />
      ) : (
        <View style={styles.placeholderContainer}>
          <Image
            source={FALLBACK_LOGO}
            style={styles.placeholderLogo}
            resizeMode="contain"
          />
        </View>
      )}
    </View>
  );
});

const styles = StyleSheet.create({
  container: {
    width: '100%',
    height: '100%',
    overflow: 'hidden',
    position: 'relative',
    backgroundColor: '#F8FAFC',
  },
  skeleton: {
    backgroundColor: '#F1F5F9',
    justifyContent: 'center',
    alignItems: 'center',
    zIndex: 1,
  },
  placeholderContainer: {
    ...StyleSheet.absoluteFillObject,
    backgroundColor: '#F8FAFC',
    justifyContent: 'center',
    alignItems: 'center',
    padding: 12,
  },
  placeholderLogo: {
    width: '50%',
    height: '50%',
    opacity: 0.85,
  },
});
