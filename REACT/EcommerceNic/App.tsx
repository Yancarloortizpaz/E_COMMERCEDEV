import React, { useState } from 'react';
import { SafeAreaView, StyleSheet, StatusBar } from 'react-native';
import { LoginScreen } from './src/Presentacion/screens/loginScreen';
import { RegisterScreen } from './src/Presentacion/screens/RegisterScreen';
import { HomeScreen } from './src/Presentacion/screens/HomeScreen';
import { AdminDashboardScreen } from './src/Presentacion/screens/AdminDashboardScreen';
import { User } from './src/Domain/entities/User';

type ScreenName = 'login' | 'register' | 'home' | 'admin';

export default function App() {
  const [currentScreen, setCurrentScreen] = useState<ScreenName>('login');
  const [currentUser, setCurrentUser] = useState<User | null>(null);

  const handleLoginSuccess = (user: User) => {
    setCurrentUser(user);
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
    setCurrentUser(null);
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

      {currentScreen === 'home' && currentUser && (
        <HomeScreen user={currentUser} onLogout={handleLogout} />
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
