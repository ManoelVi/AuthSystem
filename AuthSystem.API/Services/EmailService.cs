namespace AuthSystem.API.Services;

public class EmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IConfiguration _configuration;

    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    // Em produção, você usaria um serviço como SendGrid, AWS SES, etc.
    // Por enquanto, vamos apenas logar o email (simular envio)
    public async Task SendEmailConfirmationAsync(string email, string name, string confirmationToken)
    {
        var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000";
        var confirmationLink = $"{frontendUrl}/confirm-email?token={confirmationToken}";

        // Em produção, aqui você enviaria o email real
        // Por agora, vamos apenas logar para você poder ver o link
        _logger.LogInformation(
            "\n" +
            "========================================\n" +
            "       EMAIL DE CONFIRMAÇÃO\n" +
            "========================================\n" +
            "Para: {Email}\n" +
            "Nome: {Name}\n" +
            "----------------------------------------\n" +
            "Clique no link abaixo para confirmar seu email:\n" +
            "{ConfirmationLink}\n" +
            "----------------------------------------\n" +
            "Token: {Token}\n" +
            "========================================\n",
            email, name, confirmationLink, confirmationToken
        );

        // Simula um delay de envio
        await Task.Delay(100);
    }

    public async Task SendPasswordResetAsync(string email, string name, string resetToken)
    {
        var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000";
        var resetLink = $"{frontendUrl}/reset-password?token={resetToken}";

        _logger.LogInformation(
            "\n" +
            "========================================\n" +
            "       REDEFINIÇÃO DE SENHA\n" +
            "========================================\n" +
            "Para: {Email}\n" +
            "Nome: {Name}\n" +
            "----------------------------------------\n" +
            "Clique no link abaixo para redefinir sua senha:\n" +
            "{ResetLink}\n" +
            "========================================\n",
            email, name, resetLink
        );

        await Task.Delay(100);
    }
}
