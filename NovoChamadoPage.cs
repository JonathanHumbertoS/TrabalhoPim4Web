// PIM4Mobile/Views/NovoChamadoPage.xaml.cs

namespace PIM4Mobile.Views
{
    using Microsoft.Maui.Controls;
    using PIM4Mobile.Services;
    using PIM4Mobile.ViewModels;
    using System; // Necessário para EventArgs

    // ?? Deve ser uma classe parcial (partial) e herdar de ContentPage
    public partial class NovoChamadoPage : ContentPage
    {
        private readonly ChamadoApiService _chamadoService;

        // Adicione estas propriedades na sua classe NovoChamadoPage, caso ainda não existam
        public Entry TxtDescricao { get; set; }
        public Entry TxtTitulo { get; set; }
        public Picker CategoryPicker { get; set; }
        public Button BtnAbrirChamado { get; set; }
        public ActivityIndicator LoadingIndicator { get; set; }
        public Label ResultLabel { get; set; }

        public NovoChamadoPage()
        {
            // Certifique-se de que existe um arquivo NovoChamadoPage.xaml
            // e que ele está corretamente associado a esta classe parcial.
            InitializeComponent();
            _chamadoService = new ChamadoApiService();

            // Inicialização dos dados estáticos (Picker)
            CategoryPicker.ItemsSource = new List<string> { "Suporte Técnico", "Infraestrutura", "Financeiro", "Dúvida" };
            CategoryPicker.SelectedIndex = 0;
        }

        // ... resto do código permanece igual ...
    }
}