using System.ComponentModel.DataAnnotations;

namespace AuthSystem.API.DTOs;

public class UpdateProfileRequest
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
    public string Name { get; set; } = string.Empty;
}
