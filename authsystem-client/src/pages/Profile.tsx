import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { userService } from '../services/api';
import { UpdateProfileRequest } from '../types';

const Profile: React.FC = () => {
  const { user, logout, updateUser } = useAuth();
  const navigate = useNavigate();
  const [isEditing, setIsEditing] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<UpdateProfileRequest>({
    defaultValues: {
      name: user?.name || '',
    },
  });

  const onSubmit = async (data: UpdateProfileRequest) => {
    setIsLoading(true);
    setError('');
    setSuccess('');

    try {
      const updatedUser = await userService.updateProfile(data);
      updateUser(updatedUser);
      setSuccess('Perfil atualizado com sucesso!');
      setIsEditing(false);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao atualizar perfil');
    }

    setIsLoading(false);
  };

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  // Formata a data de criação
  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('pt-BR', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
    });
  };

  return (
    <div className="profile-container">
      <div className="profile-card">
        <h1>Meu Perfil</h1>

        {error && <div className="error-message">{error}</div>}
        {success && <div className="success-message">{success}</div>}

        {!isEditing ? (
          // Modo visualização
          <div className="profile-info">
            <div className="info-group">
              <label>Nome</label>
              <p>{user?.name}</p>
            </div>

            <div className="info-group">
              <label>Email</label>
              <p>{user?.email}</p>
            </div>

            <div className="info-group">
              <label>Membro desde</label>
              <p>{user?.createdAt ? formatDate(user.createdAt) : '-'}</p>
            </div>

            <div className="button-group">
              <button className="btn-secondary" onClick={() => setIsEditing(true)}>
                Editar Perfil
              </button>
              <button className="btn-danger" onClick={handleLogout}>
                Sair
              </button>
            </div>
          </div>
        ) : (
          // Modo edição
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
              />
              {errors.name && <span className="field-error">{errors.name.message}</span>}
            </div>

            <div className="form-group">
              <label>Email</label>
              <input type="email" value={user?.email} disabled />
              <span className="field-hint">O email não pode ser alterado</span>
            </div>

            <div className="button-group">
              <button type="submit" className="btn-primary" disabled={isLoading}>
                {isLoading ? 'Salvando...' : 'Salvar'}
              </button>
              <button
                type="button"
                className="btn-secondary"
                onClick={() => setIsEditing(false)}
              >
                Cancelar
              </button>
            </div>
          </form>
        )}
      </div>
    </div>
  );
};

export default Profile;
