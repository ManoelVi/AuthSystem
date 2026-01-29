# AuthSystem.API - Backend

API REST para o sistema de autenticação, desenvolvida em ASP.NET Core.

## Estrutura

```
AuthSystem.API/
├── Controllers/
│   ├── AuthController.cs      # Endpoints de autenticação
│   └── UserController.cs      # Endpoints de usuário
├── Data/
│   └── AppDbContext.cs        # Contexto do Entity Framework
├── DTOs/
│   ├── Validators/
│   │   └── StrongPasswordAttribute.cs  # Validação de senha forte
│   ├── AuthResponse.cs
│   ├── ConfirmEmailRequest.cs
│   ├── LoginRequest.cs
│   ├── RegisterRequest.cs
│   └── UpdateProfileRequest.cs
├── Migrations/                 # Migrações do banco de dados
├── Models/
│   └── User.cs                # Entidade de usuário
├── Services/
│   ├── AuthService.cs         # Lógica de autenticação
│   ├── EmailService.cs        # Serviço de email
│   └── UserService.cs         # Lógica de usuário
├── Program.cs                  # Configuração da aplicação
├── appsettings.json           # Configurações
└── appsettings.Development.json
```

## Configuração do Banco de Dados

O projeto usa SQL Server LocalDB. A connection string está em `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AuthSystemDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

## Migrações

Para criar uma nova migração:
```bash
dotnet ef migrations add NomeDaMigracao
```

Para aplicar migrações:
```bash
dotnet ef database update
```

## Autenticação JWT

O projeto usa JWT Bearer Authentication. Configurações em `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "SuaChaveSecretaSuperSegura...",
    "Issuer": "AuthSystem.API",
    "Audience": "AuthSystem.Client",
    "ExpirationInHours": 24
  }
}
```

## Endpoints

### POST /api/auth/register
Registra um novo usuário.

**Request:**
```json
{
  "name": "Nome do Usuário",
  "email": "email@exemplo.com",
  "password": "SenhaForte123!"
}
```

### POST /api/auth/login
Realiza login e retorna token JWT.

**Request:**
```json
{
  "email": "email@exemplo.com",
  "password": "SenhaForte123!"
}
```

### POST /api/auth/confirm-email
Confirma o email do usuário.

**Request:**
```json
{
  "token": "token-de-confirmacao"
}
```

### POST /api/auth/resend-confirmation
Reenvia email de confirmação.

**Request:**
```json
{
  "email": "email@exemplo.com"
}
```

### GET /api/user/profile
Retorna o perfil do usuário autenticado.

**Headers:** `Authorization: Bearer {token}`

### PUT /api/user/profile
Atualiza o perfil do usuário.

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "name": "Novo Nome"
}
```

## Serviço de Email

O `EmailService` atualmente apenas loga os emails no console para desenvolvimento.
Para produção, implemente o envio real de emails (SMTP, SendGrid, etc).

## Validação de Senha

O atributo `StrongPasswordAttribute` valida:
- Mínimo 10 caracteres
- Pelo menos uma letra maiúscula
- Pelo menos uma letra minúscula
- Pelo menos um número
- Pelo menos um caractere especial

## Executando

```bash
dotnet restore
dotnet ef database update
dotnet run
```

A API estará em `http://localhost:5179`
