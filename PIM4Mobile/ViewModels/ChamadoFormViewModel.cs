// PIM4Mobile/ViewModels/ChamadoFormViewModel.cs
namespace PIM4Mobile.ViewModels
{
    // ⚠️ Deve ser público para ser acessível pelo Serviço
    public class ChamadoFormViewModel
    {
        public int IdUsuario { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? Categoria { get; set; }
    }
}