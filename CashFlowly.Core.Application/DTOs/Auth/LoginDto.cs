using System.ComponentModel.DataAnnotations;

public class LoginDto
{
    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Password { get; set; }
    public bool RecordarSesion { get; set; }
}