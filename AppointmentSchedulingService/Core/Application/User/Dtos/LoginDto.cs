using System.ComponentModel.DataAnnotations;

namespace Application.User.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "E-mail é campo obrigatório para Login")]
        [EmailAddress(ErrorMessage = "Email em formato inválido")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo {1} caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password é campo obrigatório para Login")]
        [StringLength(100, ErrorMessage = "Password deve ter no máximo {1} caracteres")]
        public string Password { get; set; }
    }
}
