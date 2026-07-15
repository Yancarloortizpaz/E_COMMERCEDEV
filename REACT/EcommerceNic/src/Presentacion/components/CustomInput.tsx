import React, { useState } from 'react';
import { View, TextInput, StyleSheet, TextInputProps, TouchableOpacity, Image, Platform } from 'react-native';

interface Props extends TextInputProps {
  placeholder: string;
  value: string;
  onChangeText: (text: string) => void;
  secureTextEntry?: boolean;
}

export const CustomInput = ({ placeholder, value, onChangeText, secureTextEntry, onFocus, onBlur, ...rest }: Props) => {
  const [isSecure, setIsSecure] = useState(secureTextEntry);
  const [isFocused, setIsFocused] = useState(false);
  
  const textLower = placeholder.toLowerCase();
  const isPassword = secureTextEntry || textLower.includes('contraseña') || textLower.includes('mínimo');

  const handleFocus = (e: any) => {
    setIsFocused(true);
    if (onFocus) onFocus(e);
  };

  const handleBlur = (e: any) => {
    setIsFocused(false);
    if (onBlur) onBlur(e);
  };

  return (
    <View style={[
      styles.inputContainer,
      isFocused ? styles.inputContainerFocused : styles.inputContainerUnfocused
    ]}>
      
      {/* 1. ICONO IZQUIERDO */}
      <Image 
        source={
          isPassword 
            ? require('../../../assets/Candado.png') 
            : require('../../../assets/loginGmail.png')
        } 
        style={[
          styles.iconImageLeft,
          isFocused ? styles.iconFocused : styles.iconUnfocused
        ]} 
        resizeMode="contain" 
      />

      {/* 2. CAMPO DE TEXTO */}
      <TextInput
        placeholder={placeholder}
        value={value}
        onChangeText={onChangeText}
        secureTextEntry={isSecure}
        placeholderTextColor="#94A3B8"
        onFocus={handleFocus}
        onBlur={handleBlur}
        style={styles.input}
        {...rest}
      />

      {/* 3. ICONO DERECHO: Ver/ocultar contraseña */}
      {isPassword && (
        <TouchableOpacity onPress={() => setIsSecure(!isSecure)} activeOpacity={0.7}>
          <Image 
            source={
              isSecure 
                ? require('../../../assets/invisible.png') 
                : require('../../../assets/visible.png')
            }
            style={[
              styles.iconImageRight,
              isFocused ? styles.iconFocused : styles.iconUnfocused
            ]}
            resizeMode="contain"
          />
        </TouchableOpacity>
      )}

    </View>
  );
};

const styles = StyleSheet.create({
  inputContainer: {
    width: '100%',
    height: 54,
    borderRadius: 14,          
    borderWidth: 1.5,
    flexDirection: 'row',       
    alignItems: 'center',
    paddingHorizontal: 16,
    marginBottom: 4,
    ...Platform.select({
      ios: {
        shadowColor: '#0F172A',
        shadowOffset: { width: 0, height: 2 },
        shadowOpacity: 0.02,
        shadowRadius: 4,
      },
      android: {
        elevation: 1,
      },
      default: {},
    }),
  },
  inputContainerFocused: {
    backgroundColor: '#FFFFFF',
    borderColor: '#4F46E5', // Indigo-600
    shadowColor: '#4F46E5',
    shadowOpacity: 0.08,
    shadowRadius: 8,
  },
  inputContainerUnfocused: {
    backgroundColor: '#F8FAFC',
    borderColor: '#E2E8F0', // Slate-200
  },
  iconImageLeft: {
    width: 20,
    height: 20,
    marginRight: 12,
  },
  iconFocused: {
    tintColor: '#4F46E5',
  } as any,
  iconUnfocused: {
    tintColor: '#94A3B8',
  } as any,
  input: {
    flex: 1,
    fontSize: 15,
    color: '#0F172A',
    fontWeight: '500',
    height: '100%',
    ...Platform.select({
      web: {
        outlineStyle: 'none',
      } as any,
      default: {},
    }),
  },
  iconImageRight: {
    width: 22,
    height: 22,
    marginLeft: 10,
  }
});