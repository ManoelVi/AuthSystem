import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { User, LoginRequest, RegisterRequest } from '../types';
import { authService, userService } from '../services/api';

// Tipo do contexto
interface AuthContextType {
  user: User | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (data: LoginRequest) => Promise<{ success: boolean; message: string }>;
  register: (data: RegisterRequest) => Promise<{ success: boolean; message: string; needsEmailConfirmation?: boolean }>;
  logout: () => void;
  updateUser: (user: User) => void;
}

// Criar o contexto
const AuthContext = createContext<AuthContextType | undefined>(undefined);

// Provider do contexto
interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  // Verifica se há token salvo ao carregar a aplicação
  useEffect(() => {
    const checkAuth = async () => {
      const token = localStorage.getItem('token');

      if (token) {
        try {
          // Busca os dados do usuário com o token salvo
          const userData = await userService.getProfile();
          setUser(userData);
        } catch (error) {
          // Token inválido ou expirado
          localStorage.removeItem('token');
        }
      }

      setIsLoading(false);
    };

    checkAuth();
  }, []);

  // Função de login
  const login = async (data: LoginRequest): Promise<{ success: boolean; message: string }> => {
    try {
      const response = await authService.login(data);

      if (response.success && response.token && response.user) {
        localStorage.setItem('token', response.token);
        setUser(response.user);
        return { success: true, message: response.message };
      }

      return { success: false, message: response.message };
    } catch (error: any) {
      const message = error.response?.data?.message || 'Erro ao fazer login';
      return { success: false, message };
    }
  };

  // Função de registro (agora não faz login automático - precisa confirmar email)
  const register = async (data: RegisterRequest): Promise<{ success: boolean; message: string; needsEmailConfirmation?: boolean }> => {
    try {
      const response = await authService.register(data);

      if (response.success) {
        // Registro bem sucedido, mas precisa confirmar email
        return {
          success: true,
          message: response.message,
          needsEmailConfirmation: true
        };
      }

      return { success: false, message: response.message };
    } catch (error: any) {
      const message = error.response?.data?.message || 'Erro ao registrar';
      return { success: false, message };
    }
  };

  // Função de logout
  const logout = () => {
    localStorage.removeItem('token');
    setUser(null);
  };

  // Atualiza os dados do usuário (após editar perfil ou confirmar email)
  const updateUser = (updatedUser: User) => {
    setUser(updatedUser);
  };

  const value: AuthContextType = {
    user,
    isAuthenticated: !!user,
    isLoading,
    login,
    register,
    logout,
    updateUser,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

// Hook para usar o contexto
export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);

  if (context === undefined) {
    throw new Error('useAuth deve ser usado dentro de um AuthProvider');
  }

  return context;
};
