import React, { useEffect, useState } from 'react';
import {
  View,
  Text,
  StyleSheet,
  TouchableOpacity,
  Platform,
} from 'react-native';
import { Picker } from '@react-native-picker/picker';
import { CustomInput } from '../components/CustomInput';
import { CustomButton } from '../components/CustomButton';
import { ContenedorFormularioTeclado } from '../components/ContenedorFormularioTeclado';
import { Image } from 'react-native'; // Asegúrate de importar Image si no lo está
import { Pais } from '../../Domain/entities/Pais';
import { Genero } from '../../Domain/entities/Genero';
import {
  getPaisesUseCase,
  getGenerosUseCase,
  registerUseCase,
} from '../../di/DI';

interface Props {
  onRegisterSuccess?: () => void;
  onBackToLogin?: () => void;
}

export const RegisterScreen = ({ onRegisterSuccess, onBackToLogin }: Props) => {
  const [name, setName] = useState('');
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [phone, setPhone] = useState('');
  const [birthday, setBirthday] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [countryId, setCountryId] = useState<number>();
  const [genderId, setGenderId] = useState<number>();
  const [countries, setCountries] = useState<Pais[]>([]);
  const [genders, setGenders] = useState<Genero[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  const [errorMessage, setErrorMessage] = useState('');
  const [successMessage, setSuccessMessage] = useState('');

  useEffect(() => {
    loadCatalogs();
  }, []);

  const loadCatalogs = async () => {
    try {
      const paises = await getPaisesUseCase.execute();
      const generos = await getGenerosUseCase.execute();
      setCountries(paises);
      setGenders(generos);
    } catch (e) {
      console.log(e);
    }
  };

  const validateEmail = (emailStr: string) => {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return emailRegex.test(emailStr.trim());
  };

  const handleRegister = async () => {
    setErrorMessage('');
    setSuccessMessage('');

    const trimmedName = name.trim();
    const trimmedUsernameValue = username.trim();
    const trimmedEmail = email.trim();
    const trimmedPhone = phone.trim();
    const trimmedBirthday = birthday.trim();

    if (!trimmedName) {
      setErrorMessage('Por favor, ingresa tu nombre completo.');
      return;
    }

    if (!trimmedUsernameValue) {
      setErrorMessage('Por favor, ingresa un nombre de usuario.');
      return;
    }

    if (!trimmedEmail) {
      setErrorMessage('Por favor, ingresa tu correo electrónico.');
      return;
    }

    if (!validateEmail(trimmedEmail)) {
      setErrorMessage('El correo electrónico no tiene un formato válido.');
      return;
    }

    if (!trimmedPhone) {
      setErrorMessage('Por favor, ingresa tu número de teléfono.');
      return;
    }

    if (trimmedPhone.length < 8) {
      setErrorMessage('El teléfono debe tener al menos 8 dígitos.');
      return;
    }

    if (!countryId) {
      setErrorMessage('Por favor, selecciona tu país.');
      return;
    }

    if (!genderId) {
      setErrorMessage('Por favor, selecciona tu género.');
      return;
    }

    if (!trimmedBirthday) {
      setErrorMessage('Por favor, ingresa tu fecha de nacimiento.');
      return;
    }

    if (!password) {
      setErrorMessage('Por favor, ingresa una contraseña.');
      return;
    }

    if (password.length < 6) {
      setErrorMessage('La contraseña debe tener al menos 6 caracteres.');
      return;
    }

    if (password !== confirmPassword) {
      setErrorMessage('Las contraseñas no coinciden. Verifícalas.');
      return;
    }

    const [day, month, year] = trimmedBirthday.split("/");
    const birthdayISO = `${year}-${month}-${day}T00:00:00`;

    setIsLoading(true);
    try {
      await registerUseCase.execute(
        trimmedName,
        trimmedUsernameValue,
        password,
        trimmedEmail,
        trimmedPhone,
        countryId,
        genderId,
        birthdayISO,
      );
      setSuccessMessage('¡Cuenta creada con éxito! Redirigiendo...');

      setTimeout(() => {
        setIsLoading(false);
        if (onRegisterSuccess) onRegisterSuccess();
      }, 1500);
    } catch (error: any) {
      setErrorMessage(`${error.message || 'Error al registrar usuario'}`);
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

        <Text style={styles.brandTitle}>Crear cuenta</Text>
        <Text style={styles.subtitle}>Únete a nuestra aplicación y disfruta de la mejor tecnología</Text>

        <View style={styles.fieldSpacing}>
          <CustomInput
            placeholder="Nombre completo"
            leftIconName="user"
            value={name}
            onChangeText={setName}
          />
        </View>

        <View style={styles.fieldSpacing}>
          <CustomInput
            placeholder="Nombre de usuario"
            leftIconName="at-sign"
            value={username}
            onChangeText={setUsername}
            autoCapitalize="none"
          />
        </View>

        <View style={styles.fieldSpacing}>
          <CustomInput
            placeholder="Correo electrónico"
            leftIconName="mail"
            value={email}
            onChangeText={setEmail}
            keyboardType="email-address"
            autoCapitalize="none"
          />
        </View>

        <View style={styles.fieldSpacing}>
          <CustomInput
            placeholder="Teléfono"
            leftIconName="phone"
            value={phone}
            onChangeText={setPhone}
            keyboardType="phone-pad"
          />
        </View>

        <View style={styles.pickerContainer}>
          <Picker
            selectedValue={countryId ?? ''}
            onValueChange={value => {
              setCountryId(Number(value));
            }}
            style={styles.picker}
            dropdownIconColor="#64748B"
          >
            <Picker.Item label="Selecciona un país" value="" />
            {countries.map(country => (
              <Picker.Item key={country.id} label={country.nombre} value={country.id} />
            ))}
          </Picker>
        </View>

        <View style={styles.pickerContainer}>
          <Picker
            selectedValue={genderId ?? ''}
            onValueChange={value => {
              setGenderId(Number(value));
            }}
            style={styles.picker}
            dropdownIconColor="#64748B"
          >
            <Picker.Item label="Selecciona un género" value="" />
            {genders.map(gender => (
              <Picker.Item key={gender.id} label={gender.nombre} value={gender.id} />
            ))}
          </Picker>
        </View>

        <View style={styles.fieldSpacing}>
          <CustomInput
            placeholder="Fecha de nacimiento (dd/mm/yyyy)"
            leftIconName="calendar"
            value={birthday}
            onChangeText={setBirthday}
            keyboardType="numbers-and-punctuation"
          />
        </View>

        <View style={styles.fieldSpacing}>
          <CustomInput
            placeholder="Contraseña (mínimo 6 caracteres)"
            leftIconName="lock"
            value={password}
            onChangeText={setPassword}
            secureTextEntry
          />
        </View>

        <View style={styles.fieldSpacing}>
          <CustomInput
            placeholder="Confirmar contraseña"
            leftIconName="lock"
            value={confirmPassword}
            onChangeText={setConfirmPassword}
            secureTextEntry
          />
        </View>

        {errorMessage ? (
          <Text style={styles.errorText}>{errorMessage}</Text>
        ) : null}

        {successMessage ? (
          <Text style={styles.successText}>{successMessage}</Text>
        ) : null}

        <View style={styles.buttonContainer}>
          <CustomButton title="Crear Cuenta" onPress={handleRegister} loading={isLoading} />
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
    fontSize: 28,
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
  pickerContainer: {
    borderWidth: 1.5,
    borderColor: '#CBD5E1',
    borderRadius: 14,
    backgroundColor: '#FFFFFF',
    height: 54,
    paddingHorizontal: 16,
    marginBottom: 16,
    justifyContent: 'center',
    overflow: 'hidden',
  },
  picker: {
    color: '#0F172A',
    fontSize: 15,
    flex: 1,
    paddingVertical: 0,
    backgroundColor: 'transparent',
    borderWidth: 0,
    ...Platform.select({
      android: {
        color: '#0F172A',
        backgroundColor: 'transparent',
        borderWidth: 0,
        paddingLeft: 0,
      },
      default: {},
    }),
  },
  errorText: {
    color: '#EF4444',
    fontSize: 13,
    textAlign: 'center',
    marginTop: 4,
    marginBottom: 8,
  },
  successText: {
    color: '#10B981',
    fontSize: 13,
    textAlign: 'center',
    marginTop: 4,
    marginBottom: 8,
  },
  buttonContainer: {
    marginTop: 8,
  },
  termsText: {
    fontSize: 12,
    color: '#64748B',
    textAlign: 'center',
    marginTop: 20,
    lineHeight: 18,
    fontWeight: '500',
  },
  termsLink: {
    color: '#4F46E5',
    fontWeight: '600',
  },
  loginContainer: {
    marginTop: 24,
    alignItems: 'center',
  },
  loginText: {
    fontSize: 13,
    color: '#64748B',
  },
  loginLink: {
    fontWeight: '600',
    color: '#4F46E5',
  },
});