// No arquivo ViewModels/LoginViewModel.cs
using System.ComponentModel.DataAnnotations;
namespace TrabalhoPim4Web.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;
    }
}