import React, { useState } from 'react';
import { Image, View, ActivityIndicator, StyleSheet, ImageStyle, ViewStyle } from 'react-native';

const FALLBACK_PLACEHOLDER = require('../../../assets/logo.png');

interface Props {
  url: string | undefined | null;
  style: ImageStyle;
  containerStyle?: ViewStyle;
}

export const ProductImage = ({ url, style, containerStyle }: Props) => {
  const [hasError, setHasError] = useState(false);
  const [isLoading, setIsLoading] = useState(!!url);

  const isValidUrl = url && url.trim().length > 0 && !hasError;
  const source = isValidUrl ? { uri: url } : FALLBACK_PLACEHOLDER;

  return (
    <View style={[styles.container, containerStyle]}>
      {isLoading && isValidUrl && (
        <View style={[StyleSheet.absoluteFill, styles.skeleton]}>
          <ActivityIndicator size="small" color="#94A3B8" />
        </View>
      )}
      <Image
        source={source}
        style={style}
        resizeMode="cover"
        onLoad={() => setIsLoading(false)}
        onError={() => {
          setHasError(true);
          setIsLoading(false);
        }}
      />
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    overflow: 'hidden',
    position: 'relative',
  },
  skeleton: {
    backgroundColor: '#F1F5F9',
    justifyContent: 'center',
    alignItems: 'center',
    zIndex: 1,
  },
});
