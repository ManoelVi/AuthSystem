using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AuthSystem.API.DTOs.Validators;

public class StrongPasswordAttribute : ValidationAttribute
{
    public int MinimumLength { get; set; } = 10;
    public bool RequireUppercase { get; set; } = true;
    public bool RequireLowercase { get; set; } = true;
    public bool RequireDigit { get; set; } = true;
    public bool RequireSpecialCharacter { get; set; } = true;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string password)
        {
            return new ValidationResult("Senha é obrigatória");
        }

        var errors = new List<string>();

        // Verifica tamanho mínimo
        if (password.Length < MinimumLength)
        {
            errors.Add($"mínimo {MinimumLength} caracteres");
        }

        // Verifica letra maiúscula
        if (RequireUppercase && !Regex.IsMatch(password, "[A-Z]"))
        {
            errors.Add("uma letra maiúscula");
        }

        // Verifica letra minúscula
        if (RequireLowercase && !Regex.IsMatch(password, "[a-z]"))
        {
            errors.Add("uma letra minúscula");
        }

        // Verifica número
        if (RequireDigit && !Regex.IsMatch(password, "[0-9]"))
        {
            errors.Add("um número");
        }

        // Verifica caractere especial
        if (RequireSpecialCharacter && !Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>_\-\[\]\\\/~`+=;]"))
        {
            errors.Add("um caractere especial (!@#$%^&*...)");
        }

        if (errors.Count > 0)
        {
            return new ValidationResult($"A senha deve conter: {string.Join(", ", errors)}");
        }

        return ValidationResult.Success;
    }
}
