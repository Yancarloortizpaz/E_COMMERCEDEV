import React, { useState, useRef } from 'react';
import { View, TextInput, StyleSheet, TextInputProps, TouchableOpacity, Platform } from 'react-native';
import { Feather } from '@expo/vector-icons';

interface Props extends TextInputProps {
  placeholder: string;
  value: string;
  onChangeText: (text: string) => void;
  secureTextEntry?: boolean;
  isPassword?: boolean;
  leftIconName?: React.ComponentProps<typeof Feather>['name'];
}

export const CustomInput = ({
  placeholder,
  value,
  onChangeText,
  secureTextEntry = false,
  isPassword,
  leftIconName,
  onFocus,
  onBlur,
  ...rest
}: Props) => {
  const [isSecure, setIsSecure] = useState(secureTextEntry);
  const inputRef = useRef<TextInput>(null);

  const esCampoPassword = isPassword !== undefined ? isPassword : Boolean(secureTextEntry);

  const leftIcon: React.ComponentProps<typeof Feather>['name'] = 
    leftIconName ?? (esCampoPassword ? 'lock' : 'user');

  return (
    <View style={styles.inputContainer}>
      {/* Ícono izquierdo vectorial */}
      <Feather
        name={leftIcon}
        size={20}
        color="#64748B"
        style={styles.leftIcon}
      />

      {/* Campo de texto */}
      <TextInput
        ref={inputRef}
        placeholder={placeholder}
        value={value}
        onChangeText={onChangeText}
        secureTextEntry={isSecure}
        placeholderTextColor="#94A3B8"
        onFocus={onFocus}
        onBlur={onBlur}
        style={styles.input}
        {...rest}
      />

      {/* Ícono derecho: ver/ocultar contraseña */}
      {esCampoPassword && (
        <TouchableOpacity
          onPress={() => setIsSecure(!isSecure)}
          activeOpacity={0.7}
          style={styles.rightIconWrapper}
        >
          <Feather
            name={isSecure ? 'eye-off' : 'eye'}
            size={20}
            color="#64748B"
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
    borderColor: '#CBD5E1',
    backgroundColor: '#FFFFFF',
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
  leftIcon: {
    marginRight: 12,
  },
  input: {
    flex: 1,
    fontSize: 15,
    color: '#0F172A',
    //fontWeight: '500',
    height: '100%',
    ...Platform.select({
      web: {
        outlineStyle: 'none',
      } as any,
      default: {},
    }),
  },
  rightIconWrapper: {
    padding: 4,
  },
});