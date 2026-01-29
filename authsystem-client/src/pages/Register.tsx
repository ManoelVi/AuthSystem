import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import { Link } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { RegisterRequest } from '../types';

// Função de validação de senha forte
const validatePassword = (value: string) => {
  const errors: string[] = [];

  if (value.length < 10) {
    errors.push('mínimo 10 caracteres');
  }
  if (!/[A-Z]/.test(value)) {
    errors.push('uma letra maiúscula');
  }
  if (!/[a-z]/.test(value)) {
    errors.push('uma letra minúscula');
  }
  if (!/[0-9]/.test(value)) {
    errors.push('um número');
  }
  if (!/[!@#$%^&*(),.?":{}|<>_\-\[\]\\\/~`+=;]/.test(value)) {
    errors.push('um caractere especial (!@#$%^&*...)');
  }

  if (errors.length > 0) {
    return `A senha deve conter: ${errors.join(', ')}`;
  }

  return true;
};

const Register: React.FC = () => {
  const { register: registerUser } = useAuth();
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [registrationSuccess, setRegistrationSuccess] = useState(false);
  const [registeredEmail, setRegisteredEmail] = useState('');

  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm<RegisterRequest>();

  const password = watch('password', '');

  const onSubmit = async (data: RegisterRequest) => {
    setIsLoading(true);
    setError('');

    const result = await registerUser(data);

    if (result.success && result.needsEmailConfirmation) {
      setRegistrationSuccess(true);
      setRegisteredEmail(data.email);
    } else if (!result.success) {
      setError(result.message);
    }

    setIsLoading(false);
  };

  // Indicadores visuais de requisitos da senha
  const passwordChecks = {
    length: password.length >= 10,
    uppercase: /[A-Z]/.test(password),
    lowercase: /[a-z]/.test(password),
    number: /[0-9]/.test(password),
    special: /[!@#$%^&*(),.?":{}|<>_\-\[\]\\\/~`+=;]/.test(password),
  };

  // Tela de sucesso após registro
  if (registrationSuccess) {
    return (
      <div className="auth-container">
        <div className="auth-card">
          <h1>Verifique seu Email</h1>

          <div className="success-message">
            <p>Conta criada com sucesso!</p>
          </div>

          <div className="email-confirmation-info">
            <p>Enviamos um link de confirmação para:</p>
            <p className="email-highlight">{registeredEmail}</p>
            <p>Clique no link do email para ativar sua conta.</p>
          </div>

          <div className="info-box">
            <p><strong>Importante:</strong></p>
            <ul>
              <li>Verifique também a pasta de spam</li>
              <li>O link expira em 24 horas</li>
            </ul>
          </div>

          <p className="auth-link">
            Já confirmou? <Link to="/login">Faça login</Link>
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className="auth-container">
      <div className="auth-card">
        <h1>Criar Conta</h1>

        {error && <div className="error-message">{error}</div>}

        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="form-group">
            <label htmlFor="name">Nome</label>
            <input
              type="text"
              id="name"
              {...register('name', {
                required: 'Nome é obrigatório',
                minLength: {
                  value: 2,
                  message: 'Nome deve ter no mínimo 2 caracteres',
                },
              })}
              placeholder="Seu nome"
            />
            {errors.name && <span className="field-error">{errors.name.message}</span>}
          </div>

          <div className="form-group">
            <label htmlFor="email">Email</label>
            <input
              type="email"
              id="email"
              {...register('email', {
                required: 'Email é obrigatório',
                pattern: {
                  value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                  message: 'Email inválido',
                },
              })}
              placeholder="seu@email.com"
            />
            {errors.email && <span className="field-error">{errors.email.message}</span>}
          </div>

          <div className="form-group">
            <label htmlFor="password">Senha</label>
            <input
              type="password"
              id="password"
              {...register('password', {
                required: 'Senha é obrigatória',
                validate: validatePassword,
              })}
              placeholder="Senha forte"
            />
            {errors.password && <span className="field-error">{errors.password.message}</span>}

            {/* Indicadores de requisitos */}
            {password.length > 0 && (
              <div className="password-requirements">
                <p className={passwordChecks.length ? 'valid' : 'invalid'}>
                  {passwordChecks.length ? '✓' : '○'} Mínimo 10 caracteres
                </p>
                <p className={passwordChecks.uppercase ? 'valid' : 'invalid'}>
                  {passwordChecks.uppercase ? '✓' : '○'} Uma letra maiúscula
                </p>
                <p className={passwordChecks.lowercase ? 'valid' : 'invalid'}>
                  {passwordChecks.lowercase ? '✓' : '○'} Uma letra minúscula
                </p>
                <p className={passwordChecks.number ? 'valid' : 'invalid'}>
                  {passwordChecks.number ? '✓' : '○'} Um número
                </p>
                <p className={passwordChecks.special ? 'valid' : 'invalid'}>
                  {passwordChecks.special ? '✓' : '○'} Um caractere especial
                </p>
              </div>
            )}
          </div>

          <button type="submit" className="btn-primary" disabled={isLoading}>
            {isLoading ? 'Criando conta...' : 'Criar Conta'}
          </button>
        </form>

        <p className="auth-link">
          Já tem uma conta? <Link to="/login">Faça login</Link>
        </p>
      </div>
    </div>
  );
};

export default Register;
