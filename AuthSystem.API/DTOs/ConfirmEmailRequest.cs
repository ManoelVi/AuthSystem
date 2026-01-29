using System.ComponentModel.DataAnnotations;

namespace AuthSystem.API.DTOs;

public class ConfirmEmailRequest
{
    [Required(ErrorMessage = "Token é obrigatório")]
    public string Token { get; set; } = string.Empty;
}

public class ResendConfirmationRequest
{
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;
}
