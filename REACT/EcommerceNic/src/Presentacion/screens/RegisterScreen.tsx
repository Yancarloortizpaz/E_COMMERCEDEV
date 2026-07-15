import React, { useState } from 'react';
import {
  View,
  Text,
  StyleSheet,
  KeyboardAvoidingView,
  Platform,
  Image,
  ScrollView,
  TouchableOpacity,
} from 'react-native';
import { CustomInput } from '../components/CustomInput';
import { CustomButton } from '../components/CustomButton';
import { registerUseCase } from '../../di/DI';

interface Props {
  onRegisterSuccess?: () => void;
  onBackToLogin?: () => void;
}

export const RegisterScreen = ({ onRegisterSuccess, onBackToLogin }: Props) => {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const [errorMessage, setErrorMessage] = useState('');
  const [successMessage, setSuccessMessage] = useState('');

  const validateEmail = (emailStr: string) => {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return emailRegex.test(emailStr.trim());
  };

  const handleRegister = async () => {
    setErrorMessage('');
    setSuccessMessage('');

    const trimmedName = name.trim();
    const trimmedEmail = email.trim();

    if (!trimmedName) {
      setErrorMessage('👤 Por favor, ingresa tu nombre completo.');
      return;
    }

    if (!trimmedEmail) {
      setErrorMessage('📧 Por favor, ingresa tu correo electrónico.');
      return;
    }

    if (!validateEmail(trimmedEmail)) {
      setErrorMessage('🛑 El correo electrónico no tiene un formato válido.');
      return;
    }

    if (!password) {
      setErrorMessage('🔑 Por favor, ingresa una contraseña.');
      return;
    }

    if (password.length < 6) {
      setErrorMessage('🔒 La contraseña debe tener al menos 6 caracteres.');
      return;
    }

    if (password !== confirmPassword) {
      setErrorMessage('🛑 Las contraseñas no coinciden. Verifícalas.');
      return;
    }

    setIsLoading(true);
    try {
      // Register using Clean Architecture usecase
      await registerUseCase.execute(trimmedName, trimmedEmail, password, 'user');
      setSuccessMessage('🎉 ¡Cuenta creada con éxito! Redirigiendo...');
      
      setTimeout(() => {
        setIsLoading(false);
        if (onRegisterSuccess) onRegisterSuccess();
      }, 1500);
    } catch (error: any) {
      setErrorMessage(`🛑 ${error.message || 'Error al registrar usuario'}`);
      setIsLoading(false);
    }
  };

  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      style={styles.container}
    >
      <ScrollView contentContainerStyle={styles.scrollContainer} showsVerticalScrollIndicator={false}>
        <View style={styles.cardContainer}>

          <View style={styles.logoWrapper}>
            <View style={styles.logoBackground}>
              <Image
                source={require('../../../assets/logo.png')}
                style={styles.logoImage}
                resizeMode="contain"
              />
            </View>
          </View>

          <Text style={styles.welcomeText}>Regístrate en</Text>
          <Text style={styles.brandTitle}>Nic Store</Text>
          <Text style={styles.subtitle}>Crea tu cuenta y accede al mejor catálogo de tecnología en el país</Text>

          <View style={styles.formContainer}>
            <Text style={styles.inputLabel}>Nombre Completo</Text>
            <CustomInput 
              placeholder="Tu nombre completo" 
              value={name} 
              onChangeText={text => { setErrorMessage(''); setName(text); }} 
            />

            <Text style={styles.inputLabel}>Correo Electrónico</Text>
            <CustomInput
              placeholder="tucorreo@email.com"
              value={email}
              onChangeText={text => { setErrorMessage(''); setEmail(text); }}
              keyboardType="email-address"
              autoCapitalize="none"
            />

            <Text style={styles.inputLabel}>Contraseña</Text>
            <CustomInput
              placeholder="Mínimo 6 caracteres"
              value={password}
              onChangeText={text => { setErrorMessage(''); setPassword(text); }}
              secureTextEntry
            />

            <Text style={styles.inputLabel}>Confirmar Contraseña</Text>
            <CustomInput
              placeholder="Repite tu contraseña"
              value={confirmPassword}
              onChangeText={text => { setErrorMessage(''); setConfirmPassword(text); }}
              secureTextEntry
            />

            {errorMessage ? (
              <View style={styles.errorContainer}>
                <Text style={styles.errorText}>{errorMessage}</Text>
              </View>
            ) : null}
            
            {successMessage ? (
              <View style={styles.successContainer}>
                <Text style={styles.successText}>{successMessage}</Text>
              </View>
            ) : null}

            <View style={styles.buttonContainer}>
              <CustomButton title="Crear Cuenta" onPress={handleRegister} loading={isLoading} />
            </View>
          </View>

          <Text style={styles.termsText}>
            Al registrarte aceptas nuestros{' '}
            <Text style={styles.termsLink}>Términos de Servicio</Text> y{' '}
            <Text style={styles.termsLink}>Política de Privacidad</Text>.
          </Text>

          <TouchableOpacity onPress={onBackToLogin} style={styles.loginContainer} activeOpacity={0.7}>
            <Text style={styles.loginText}>
              ¿Ya tienes una cuenta? <Text style={styles.loginLink}>Inicia sesión</Text>
            </Text>
          </TouchableOpacity>

        </View>
      </ScrollView>
    </KeyboardAvoidingView>
  );
};

const styles = StyleSheet.create({
  container: { 
    flex: 1, 
    backgroundColor: '#F8FAFC' 
  },
  scrollContainer: { 
    flexGrow: 1,
    paddingVertical: 45, 
    justifyContent: 'center',
    alignItems: 'center',
    paddingHorizontal: 20,
  },
  cardContainer: { 
    backgroundColor: '#FFFFFF',
    borderRadius: 24,
    paddingHorizontal: 28,
    paddingVertical: 32,
    width: '100%',
    maxWidth: 420,
    alignSelf: 'center',
    borderWidth: 1,
    borderColor: '#F1F5F9',
    ...Platform.select({
      ios: {
        shadowColor: '#0F172A',
        shadowOffset: { width: 0, height: 8 },
        shadowOpacity: 0.05,
        shadowRadius: 24,
      },
      android: {
        elevation: 4,
      },
      default: {
        shadowColor: '#0F172A',
        shadowOffset: { width: 0, height: 8 },
        shadowOpacity: 0.05,
        shadowRadius: 24,
      }
    }),
  },
  logoWrapper: {
    alignItems: 'center',
    marginBottom: 20,
  },
  logoBackground: {
    width: 80,
    height: 80,
    borderRadius: 20,
    backgroundColor: '#EEF2FF',
    justifyContent: 'center',
    alignItems: 'center',
    ...Platform.select({
      ios: {
        shadowColor: '#4F46E5',
        shadowOffset: { width: 0, height: 4 },
        shadowOpacity: 0.1,
        shadowRadius: 8,
      },
      android: {
        elevation: 2,
      },
      default: {},
    }),
  },
  logoImage: { 
    width: 60, 
    height: 60, 
    borderRadius: 12 
  },
  welcomeText: { 
    fontSize: 14, 
    fontWeight: '700', 
    color: '#64748B',
    textAlign: 'center',
    textTransform: 'uppercase',
    letterSpacing: 1,
  },
  brandTitle: { 
    fontSize: 32, 
    fontWeight: '900', 
    color: '#0F172A',
    textAlign: 'center', 
    marginBottom: 8,
    marginTop: 2,
  },
  subtitle: { 
    fontSize: 13, 
    color: '#64748B',
    textAlign: 'center', 
    marginBottom: 28, 
    lineHeight: 18,
    paddingHorizontal: 10 
  },
  formContainer: {
    width: '100%',
  },
  inputLabel: { 
    fontSize: 13, 
    fontWeight: '700', 
    color: '#334155',
    marginBottom: 6, 
    marginTop: 14,
    textTransform: 'uppercase',
    letterSpacing: 0.5,
  },
  buttonContainer: { 
    marginTop: 24 
  },
  termsText: { 
    fontSize: 12, 
    color: '#64748B', 
    textAlign: 'center', 
    marginTop: 20, 
    lineHeight: 18,
    fontWeight: '500'
  },
  termsLink: { 
    color: '#4F46E5', 
    fontWeight: '600' 
  },
  loginContainer: { 
    marginTop: 28, 
    alignItems: 'center' 
  },
  loginText: { 
    fontSize: 13, 
    color: '#64748B',
    fontWeight: '500',
  },
  loginLink: { 
    fontWeight: '700', 
    color: '#4F46E5' 
  },
  errorContainer: {
    marginTop: 16,
    backgroundColor: '#FEF2F2',
    borderWidth: 1,
    borderColor: '#FEE2E2',
    borderRadius: 12,
    padding: 12,
  },
  errorText: { 
    color: '#EF4444', 
    fontSize: 13, 
    fontWeight: '600', 
    textAlign: 'center',
    lineHeight: 18,
  },
  successContainer: {
    marginTop: 16,
    backgroundColor: '#ECFDF5',
    borderWidth: 1,
    borderColor: '#D1FAE5',
    borderRadius: 12,
    padding: 12,
  },
  successText: { 
    color: '#10B981', 
    fontSize: 13, 
    fontWeight: '600', 
    textAlign: 'center',
    lineHeight: 18,
  }
});