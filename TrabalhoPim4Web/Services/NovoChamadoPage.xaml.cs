// Views/NovoChamadoPage.xaml.cs

// ⚠️ ESSENCIAL! Adicione estes "using"
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

// Você pode precisar deste se o seu ViewModel estiver em um projeto separado
// using TrabalhoPim4Web.ViewModels; 

public partial class NovoChamadoPage : ContentPage
{
    private readonly ChamadoApiService _chamadoService = new ChamadoApiService();

    public NovoChamadoPage()
    {
        // O erro de 'InitializeComponent' é resolvido pela linha acima 
        // "using Microsoft.Maui.Controls" e pela definição da classe como 'partial'
        InitializeComponent();

        // ... o restante do seu código ...

        // Inicialização de dados (simulando a busca de categorias)
        CategoryPicker.ItemsSource = new List<string> { "Suporte Técnico", "Financeiro", "Dúvida" };
        CategoryPicker.SelectedIndex = 0;
    }

    private async void OnEnviarChamadoClicked(object sender, EventArgs e)
    {
        // 1. Controle da UI: Bloqueia o botão e mostra o indicador de carregamento
        BtnAbrirChamado.IsEnabled = false;
        LoadingIndicator.IsRunning = true;
        LoadingIndicator.IsVisible = true;
        ResultLabel.Text = string.Empty;

        string titulo = TitleEntry.Text;
        string descricao = DescriptionEditor.Text;
        string categoria = CategoryPicker.SelectedItem?.ToString();

        // ...

        // Chamada à API para enviar o chamado
        var result = await _chamadoService.EnviarChamado(titulo, descricao, categoria);

        // 5. Exibir Resultado
        if (result.Success)
        {
            // O erro de 'DisplayAlert' é resolvido por "using Microsoft.Maui.Controls"
            await DisplayAlert("Sucesso", "Chamado aberto e enviado ao suporte!", "OK");
            ResultLabel.TextColor = Colors.Green; // O erro de 'Colors' é resolvido por "using Microsoft.Maui.Graphics"
            // ...
        }
        else
        {
            // ...
        }

        // 6. Restaurar UI
        BtnAbrirChamado.IsEnabled = true;
        LoadingIndicator.IsRunning = false;
        LoadingIndicator.IsVisible = false;
    }
}