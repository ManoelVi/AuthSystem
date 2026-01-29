using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AuthSystem.API.Data;
using AuthSystem.API.DTOs;
using AuthSystem.API.Models;

namespace AuthSystem.API.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly EmailService _emailService;

    public AuthService(AppDbContext context, IConfiguration configuration, EmailService emailService)
    {
        _context = context;
        _configuration = configuration;
        _emailService = emailService;
    }

    // ===== REGISTRO =====
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // Verifica se email já existe
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (existingUser != null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Este email já está cadastrado"
            };
        }

        // Gera token de confirmação de email
        var confirmationToken = GenerateEmailConfirmationToken();

        // Cria o novo usuário com senha criptografada
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = false,
            EmailConfirmationToken = confirmationToken,
            EmailConfirmationTokenExpires = DateTime.UtcNow.AddHours(24) // Token válido por 24 horas
        };

        // Salva no banco
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Envia email de confirmação
        await _emailService.SendEmailConfirmationAsync(user.Email, user.Name, confirmationToken);

        return new AuthResponse
        {
            Success = true,
            Message = "Usuário registrado! Verifique seu email para confirmar a conta.",
            Token = null, // Não gera token até confirmar email
            User = MapToDto(user)
        };
    }

    // ===== LOGIN =====
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // Busca o usuário pelo email
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        // Verifica se usuário existe
        if (user == null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Email ou senha inválidos"
            };
        }

        // Verifica a senha
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Email ou senha inválidos"
            };
        }

        // Verifica se email foi confirmado
        if (!user.EmailConfirmed)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Por favor, confirme seu email antes de fazer login. Verifique sua caixa de entrada."
            };
        }

        // Gera o token JWT
        var token = GenerateJwtToken(user);

        return new AuthResponse
        {
            Success = true,
            Message = "Login realizado com sucesso",
            Token = token,
            User = MapToDto(user)
        };
    }

    // ===== CONFIRMAR EMAIL =====
    public async Task<AuthResponse> ConfirmEmailAsync(string token)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.EmailConfirmationToken == token);

        if (user == null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Token de confirmação inválido"
            };
        }

        if (user.EmailConfirmationTokenExpires < DateTime.UtcNow)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Token de confirmação expirado. Solicite um novo email de confirmação."
            };
        }

        // Confirma o email
        user.EmailConfirmed = true;
        user.EmailConfirmationToken = null;
        user.EmailConfirmationTokenExpires = null;

        await _context.SaveChangesAsync();

        // Gera o token JWT para login automático
        var jwtToken = GenerateJwtToken(user);

        return new AuthResponse
        {
            Success = true,
            Message = "Email confirmado com sucesso!",
            Token = jwtToken,
            User = MapToDto(user)
        };
    }

    // ===== REENVIAR EMAIL DE CONFIRMAÇÃO =====
    public async Task<AuthResponse> ResendConfirmationEmailAsync(string email)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            // Por segurança, não revelamos se o email existe ou não
            return new AuthResponse
            {
                Success = true,
                Message = "Se o email estiver cadastrado, você receberá um link de confirmação."
            };
        }

        if (user.EmailConfirmed)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Este email já foi confirmado. Você pode fazer login."
            };
        }

        // Gera novo token
        var confirmationToken = GenerateEmailConfirmationToken();
        user.EmailConfirmationToken = confirmationToken;
        user.EmailConfirmationTokenExpires = DateTime.UtcNow.AddHours(24);

        await _context.SaveChangesAsync();

        // Envia novo email
        await _emailService.SendEmailConfirmationAsync(user.Email, user.Name, confirmationToken);

        return new AuthResponse
        {
            Success = true,
            Message = "Email de confirmação reenviado! Verifique sua caixa de entrada."
        };
    }

    // ===== GERAR TOKEN DE CONFIRMAÇÃO =====
    private static string GenerateEmailConfirmationToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }

    // ===== GERAR TOKEN JWT =====
    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Claims são informações que ficam dentro do token
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var expirationHours = int.Parse(_configuration["Jwt:ExpirationInHours"]!);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expirationHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // ===== CONVERTER USER PARA DTO =====
    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }
}
