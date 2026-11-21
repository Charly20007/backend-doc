using System.ComponentModel.DataAnnotations;

namespace Surgeon.Application.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no es válido.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Password { get; set; } = string.Empty;
}

public class LoginRespuestaDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiraEn { get; set; }
    public UsuarioActualDto Usuario { get; set; } = new();
}

public class UsuarioActualDto
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
}

public class CambiarPasswordDto
{
    [Required(ErrorMessage = "La contraseña actual es obligatoria.")]
    public string PasswordActual { get; set; } = string.Empty;

    [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
    [MinLength(10, ErrorMessage = "La nueva contraseña debe tener al menos 10 caracteres.")]
    public string PasswordNueva { get; set; } = string.Empty;
}
