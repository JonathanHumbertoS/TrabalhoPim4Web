// No arquivo ViewModels/ChamadoFormViewModel.cs
using System.ComponentModel.DataAnnotations;
namespace TrabalhoPim4Web.ViewModels
{
    public class ChamadoFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100, ErrorMessage = "Máximo de 100 caracteres.")]
        public string Titulo { get; set; } = string.Empty; 

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;// Usado na tela de detalhes
    }
}