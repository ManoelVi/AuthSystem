import axios from 'axios';
import { AuthResponse, LoginRequest, RegisterRequest, User, UpdateProfileRequest } from '../types';

// Configuração base do Axios
const api = axios.create({
  baseURL: 'http://localhost:5179/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor para adicionar o token em todas as requisições
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// ===== AUTENTICAÇÃO =====

export const authService = {
  // Registrar novo usuário
  register: async (data: RegisterRequest): Promise<AuthResponse> => {
    const response = await api.post<AuthResponse>('/auth/register', data);
    return response.data;
  },

  // Fazer login
  login: async (data: LoginRequest): Promise<AuthResponse> => {
    const response = await api.post<AuthResponse>('/auth/login', data);
    return response.data;
  },

  // Confirmar email
  confirmEmail: async (token: string): Promise<AuthResponse> => {
    const response = await api.post<AuthResponse>('/auth/confirm-email', { token });
    return response.data;
  },

  // Reenviar email de confirmação
  resendConfirmation: async (email: string): Promise<AuthResponse> => {
    const response = await api.post<AuthResponse>('/auth/resend-confirmation', { email });
    return response.data;
  },
};

// ===== USUÁRIO =====

export const userService = {
  // Buscar perfil do usuário logado
  getProfile: async (): Promise<User> => {
    const response = await api.get<User>('/user/profile');
    return response.data;
  },

  // Atualizar perfil
  updateProfile: async (data: UpdateProfileRequest): Promise<User> => {
    const response = await api.put<User>('/user/profile', data);
    return response.data;
  },
};

export default api;
