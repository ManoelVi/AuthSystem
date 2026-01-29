# AuthSystem - Sistema de Autenticação

Sistema completo de autenticação com perfil de usuário, desenvolvido em C# (.NET) com frontend em React/TypeScript.

## Funcionalidades

- Registro de usuário com validação de senha forte
- Confirmação de email
- Login com JWT (JSON Web Token)
- Gerenciamento de perfil do usuário
- Rotas protegidas no frontend

## Tecnologias Utilizadas

### Backend
- .NET 10 (ASP.NET Core Web API)
- Entity Framework Core
- SQL Server LocalDB
- JWT Bearer Authentication
- BCrypt para hash de senhas

### Frontend
- React 19
- TypeScript
- React Router DOM
- React Hook Form
- Axios
- Context API

## Estrutura do Projeto

```
AuthSystem/
├── AuthSystem.API/          # Backend - API REST
│   ├── Controllers/         # Controladores da API
│   ├── Data/               # Contexto do banco de dados
│   ├── DTOs/               # Objetos de transferência de dados
│   ├── Models/             # Modelos de entidade
│   └── Services/           # Serviços de negócio
│
└── authsystem-client/       # Frontend - React
    ├── src/
    │   ├── components/     # Componentes reutilizáveis
    │   ├── contexts/       # Contextos React (Auth)
    │   ├── pages/          # Páginas da aplicação
    │   ├── services/       # Serviços de API
    │   └── types/          # Tipos TypeScript
    └── public/
```

## Como Executar

### Pré-requisitos
- .NET 10 SDK
- Node.js 18+
- SQL Server LocalDB

### Backend

```bash
cd AuthSystem.API
dotnet restore
dotnet ef database update
dotnet run
```

A API estará disponível em `http://localhost:5179`

### Frontend

```bash
cd authsystem-client
npm install
npm start
```

O frontend estará disponível em `http://localhost:3000`

## Endpoints da API

### Autenticação
- `POST /api/auth/register` - Registrar novo usuário
- `POST /api/auth/login` - Fazer login
- `POST /api/auth/confirm-email` - Confirmar email
- `POST /api/auth/resend-confirmation` - Reenviar email de confirmação

### Usuário
- `GET /api/user/profile` - Obter perfil (autenticado)
- `PUT /api/user/profile` - Atualizar perfil (autenticado)

## Requisitos de Senha

A senha deve conter:
- Mínimo 10 caracteres
- Uma letra maiúscula
- Uma letra minúscula
- Um número
- Um caractere especial (!@#$%^&*...)

## Configuração

As configurações estão no arquivo `appsettings.json`:
- Connection string do banco de dados
- Configurações do JWT (chave, issuer, audience, expiração)

## Licença

Este projeto é para fins de estudo.
