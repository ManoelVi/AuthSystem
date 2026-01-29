# AuthSystem Client - Frontend

Aplicação React com TypeScript para o sistema de autenticação.

## Estrutura

```
authsystem-client/
├── public/
│   └── index.html
├── src/
│   ├── components/
│   │   └── ProtectedRoute.tsx    # Componente de rota protegida
│   ├── contexts/
│   │   └── AuthContext.tsx       # Contexto de autenticação
│   ├── pages/
│   │   ├── ConfirmEmail.tsx      # Página de confirmação de email
│   │   ├── Login.tsx             # Página de login
│   │   ├── Profile.tsx           # Página de perfil
│   │   └── Register.tsx          # Página de registro
│   ├── services/
│   │   └── api.ts                # Serviço de API com Axios
│   ├── types/
│   │   └── index.ts              # Tipos TypeScript
│   ├── App.css                   # Estilos globais
│   ├── App.tsx                   # Componente principal
│   └── index.tsx                 # Ponto de entrada
└── package.json
```

## Funcionalidades

### Páginas

- **Login** (`/login`): Formulário de login com email e senha
- **Registro** (`/register`): Formulário de cadastro com validação de senha forte
- **Confirmação de Email** (`/confirm-email`): Confirmação via token
- **Perfil** (`/profile`): Visualização e edição do perfil (protegida)

### Componentes

- **ProtectedRoute**: Componente que protege rotas, redirecionando para login se não autenticado

### Contexto de Autenticação

O `AuthContext` gerencia:
- Estado do usuário logado
- Funções de login, registro e logout
- Verificação de autenticação ao carregar a aplicação
- Atualização dos dados do usuário

### Serviço de API

O arquivo `api.ts` configura o Axios com:
- Base URL da API (`http://localhost:5179/api`)
- Interceptor para adicionar token JWT automaticamente
- Métodos para todas as operações de autenticação e usuário

## Validação de Senha

O formulário de registro inclui indicadores visuais para:
- Mínimo 10 caracteres
- Uma letra maiúscula
- Uma letra minúscula
- Um número
- Um caractere especial

## Tecnologias

- **React 19**: Biblioteca de UI
- **TypeScript**: Tipagem estática
- **React Router DOM**: Roteamento
- **React Hook Form**: Gerenciamento de formulários
- **Axios**: Cliente HTTP
- **Context API**: Gerenciamento de estado

## Scripts Disponíveis

### `npm start`

Executa a aplicação em modo de desenvolvimento.
Abra [http://localhost:3000](http://localhost:3000) para visualizar no navegador.

### `npm test`

Executa os testes em modo interativo.

### `npm run build`

Compila a aplicação para produção na pasta `build`.

## Executando

### Pré-requisitos
- Node.js 18+
- API backend rodando em `http://localhost:5179`

### Instalação

```bash
npm install
```

### Desenvolvimento

```bash
npm start
```

A aplicação estará em `http://localhost:3000`

## Configuração da API

A URL da API está configurada em `src/services/api.ts`:

```typescript
const api = axios.create({
  baseURL: 'http://localhost:5179/api',
});
```

Para produção, altere para a URL do servidor.

## Fluxo de Autenticação

1. Usuário se registra com nome, email e senha
2. Sistema envia email de confirmação (simulado no console do backend)
3. Usuário clica no link de confirmação
4. Email é confirmado e usuário recebe token JWT
5. Usuário é redirecionado para o perfil
6. Token é armazenado no localStorage
7. Requisições subsequentes incluem o token no header

## Estilos

Os estilos estão em `App.css` e incluem:
- Layout responsivo
- Formulários estilizados
- Mensagens de erro e sucesso
- Indicadores de requisitos de senha
- Tela de confirmação de email
