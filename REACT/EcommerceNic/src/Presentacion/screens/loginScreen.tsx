import React, { useState } from 'react';
import {
  View,
  Text,
  StyleSheet,
  Image,
  TouchableOpacity,
} from 'react-native';
import { CustomInput } from '../components/CustomInput';
import { CustomButton } from '../components/CustomButton';
import { ContenedorFormularioTeclado } from '../components/ContenedorFormularioTeclado';
import { loginUseCase, guardarSesionUseCase } from '../../di/DI';
import { User } from '../../Domain/entities/User';

interface Props {
  onLoginSuccess: (user: User) => void;
  onNavigateToRegister: () => void;
}

export const LoginScreen = ({ onLoginSuccess, onNavigateToRegister }: Props) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');

  const validateEmail = (emailStr: string) => {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return emailRegex.test(emailStr.trim());
  };

  const handleLoginSubmit = async () => {
    setError('');

    const trimmedEmail = email.trim();
    const trimmedPassword = password.trim();

    if (!trimmedEmail) {
      setError(' Por favor, ingresa tu correo electrónico.');
      return;
    }

    if (!validateEmail(trimmedEmail)) {
      setError('El correo electrónico no tiene un formato válido.');
      return;
    }

    if (!trimmedPassword) {
      setError('Por favor ingresa tu contraseña.');
      return;
    }

    if (trimmedPassword.length < 6) {
      setError('La contraseña debe tener al menos 6 caracteres.');
      return;
    }

    setIsLoading(true);
    try {
      const loggedUser = await loginUseCase.execute(trimmedEmail, trimmedPassword);
      const token = loggedUser.data?.token ?? '';
      const normalizedUser: User = {
        id: String(loggedUser.data?.userId ?? Date.now()),
        email: loggedUser.data?.userEmail ?? trimmedEmail,
        name: loggedUser.data?.userFullName ?? trimmedEmail,
        role: 'user',
      };

      await guardarSesionUseCase.execute(normalizedUser, token);

      console.log("Inicio de sesión exitoso para:", normalizedUser.name, "Rol:", normalizedUser.role);
      onLoginSuccess(normalizedUser);
    } catch (err: any) {
      setError(` ${err.message || 'Error al iniciar sesión. Verifica tus credenciales.'}`);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <ContenedorFormularioTeclado
      estiloContenedor={styles.container}
      estiloScroll={styles.scrollContainer}
    >
      <View style={styles.content}>
        <Image
          source={require('../../../assets/logo.png')}
          style={styles.logo}
          resizeMode="contain"
        />

        <Text style={styles.brandTitle}>NIC STORE</Text>
        <Text style={styles.subtitle}>Lo mejor en tecnología al alcance de tus manos</Text>

        <View style={styles.fieldSpacing}>
          <CustomInput
            placeholder="Correo electrónico"
            value={email}
            onChangeText={setEmail}
            keyboardType="email-address"
            autoCapitalize="none"
            autoComplete="email"
          />
        </View>

        <View style={styles.fieldSpacing}>
          <CustomInput
            placeholder="Contraseña"
            value={password}
            onChangeText={setPassword}
            secureTextEntry
            autoComplete="current-password"
          />
        </View>

        {error ? (
          <Text style={styles.errorText}>{error}</Text>
        ) : null}

        <View style={styles.buttonContainer}>
          <CustomButton title="Iniciar Sesión" onPress={handleLoginSubmit} loading={isLoading} />
        </View>

        <TouchableOpacity
          onPress={onNavigateToRegister}
          style={styles.registerLinkContainer}
          activeOpacity={0.7}
        >
          <Text style={styles.registerText}>
            ¿No tienes cuenta? <Text style={styles.registerLink}>Regístrate</Text>
          </Text>
        </TouchableOpacity>
      </View>
    </ContenedorFormularioTeclado>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F8FAFC',
  },
  scrollContainer: {
    flexGrow: 1,
    justifyContent: 'center',
    paddingHorizontal: 24,
    paddingVertical: 40,
  },
  content: {
    width: '100%',
    maxWidth: 420,
    alignSelf: 'center',
  },
  logo: {
    width: 120,
    height: 120,
    alignSelf: 'center',
    marginBottom: 0,
  },
  brandTitle: {
    fontSize: 24,
    fontWeight: '700',
    color: '#0F172A',
    textAlign: 'center',
    letterSpacing: -0.5,
  },
  subtitle: {
    fontSize: 14,
    color: '#64748B',
    textAlign: 'center',
    marginBottom: 32,
    lineHeight: 20,
  },
  fieldSpacing: {
    marginBottom: 16,
  },
  errorText: {
    color: '#EF4444',
    fontSize: 13,
    textAlign: 'center',
    marginTop: 4,
    marginBottom: 8,
  },
  buttonContainer: {
    marginTop: 8,
  },
  registerLinkContainer: {
    marginTop: 24,
    alignItems: 'center',
  },
  registerText: {
    fontSize: 13,
    color: '#64748B',
  },
  registerLink: {
    fontWeight: '600',
    color: '#4F46E5',
  },
});