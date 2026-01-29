import React, { useEffect, useState } from 'react';
import { useSearchParams, useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { authService } from '../services/api';

const ConfirmEmail: React.FC = () => {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const { updateUser } = useAuth();
  const [status, setStatus] = useState<'loading' | 'success' | 'error'>('loading');
  const [message, setMessage] = useState('');

  useEffect(() => {
    const confirmEmail = async () => {
      const token = searchParams.get('token');

      if (!token) {
        setStatus('error');
        setMessage('Token de confirmação não encontrado na URL.');
        return;
      }

      try {
        const response = await authService.confirmEmail(token);

        if (response.success) {
          setStatus('success');
          setMessage(response.message);

          // Se retornou token e user, faz login automático
          if (response.token && response.user) {
            localStorage.setItem('token', response.token);
            updateUser(response.user);

            // Redireciona para perfil após 2 segundos
            setTimeout(() => {
              navigate('/profile');
            }, 2000);
          }
        } else {
          setStatus('error');
          setMessage(response.message);
        }
      } catch (error: any) {
        setStatus('error');
        setMessage(error.response?.data?.message || 'Erro ao confirmar email.');
      }
    };

    confirmEmail();
  }, [searchParams, navigate, updateUser]);

  return (
    <div className="auth-container">
      <div className="auth-card">
        <h1>Confirmação de Email</h1>

        {status === 'loading' && (
          <div className="loading-message">
            <p>Confirmando seu email...</p>
          </div>
        )}

        {status === 'success' && (
          <div className="success-message">
            <p>{message}</p>
            <p>Você será redirecionado automaticamente...</p>
          </div>
        )}

        {status === 'error' && (
          <>
            <div className="error-message">
              <p>{message}</p>
            </div>
            <div className="button-group" style={{ flexDirection: 'column' }}>
              <Link to="/login" className="btn-primary" style={{ textAlign: 'center', textDecoration: 'none', display: 'block', padding: '12px 24px' }}>
                Ir para Login
              </Link>
            </div>
          </>
        )}
      </div>
    </div>
  );
};

export default ConfirmEmail;
