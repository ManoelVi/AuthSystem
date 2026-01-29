namespace AuthSystem.API.Models;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Campos para confirmação de email
    public bool EmailConfirmed { get; set; } = false;

    public string? EmailConfirmationToken { get; set; }

    public DateTime? EmailConfirmationTokenExpires { get; set; }
}
