import React, { useState } from 'react';
import { SafeAreaView, StyleSheet, StatusBar } from 'react-native';
import { LoginScreen } from './src/Presentacion/screens/loginScreen';
import { RegisterScreen } from './src/Presentacion/screens/RegisterScreen';
import { HomeScreen } from './src/Presentacion/screens/HomeScreen';
import { AdminDashboardScreen } from './src/Presentacion/screens/AdminDashboardScreen';

export default function App() {
  const [currentScreen, setCurrentScreen] = useState('login');

  const handleLoginSuccess = (user) => {
    if (user.role === 'admin') {
      setCurrentScreen('admin');
    } else {
      setCurrentScreen('home');
    }
  };

  const handleRegisterSuccess = () => {
    setCurrentScreen('login');
  };

  const handleLogout = () => {
    setCurrentScreen('login');
  };

  return (
    <SafeAreaView style={styles.container}>
      <StatusBar barStyle="dark-content" backgroundColor="#F8FAFC" />

      {currentScreen === 'login' && (
        <LoginScreen
          onLoginSuccess={handleLoginSuccess}
          onNavigateToRegister={() => setCurrentScreen('register')}
        />
      )}

      {currentScreen === 'register' && (
        <RegisterScreen
          onRegisterSuccess={handleRegisterSuccess}
          onBackToLogin={() => setCurrentScreen('login')}
        />
      )}

      {currentScreen === 'home' && (
        <HomeScreen onLogout={handleLogout} />
      )}

      {currentScreen === 'admin' && (
        <AdminDashboardScreen onLogout={handleLogout} />
      )}
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F8FAFC',
  },
});
