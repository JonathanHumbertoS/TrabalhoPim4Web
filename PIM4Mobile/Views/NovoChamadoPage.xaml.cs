// PIM4Mobile/Views/NovoChamadoPage.xaml.cs


    using Microsoft.Maui.Controls;
    using PIM4Mobile.Services;
    using PIM4Mobile.ViewModels;
    using System; // Necessário para EventArgs

namespace PIM4Mobile.Views
{

    // ⚠️ Deve ser uma classe parcial (partial) e herdar de ContentPage
    public partial class NovoChamadoPage : ContentPage
    {
        private readonly ChamadoApiService _chamadoService;

        // Adicione estas propriedades na sua classe NovoChamadoPage, caso ainda não existam
        


        public  NovoChamadoPage()
        {
            InitializeComponent(); // Remova o "this." daqui

            _chamadoService = new ChamadoApiService();

            // Inicialização dos dados estáticos (Picker)
            CategoryPicker.ItemsSource = new List<string> { "Suporte Técnico", "Infraestrutura", "Financeiro", "Dúvida" };
            CategoryPicker.SelectedIndex = 0;
        }

        // ⚠️ Método de clique: Assinatura CORRETA (async void) para evitar erros
        private async void OnEnviarChamadoClicked(object sender, EventArgs e)
        {
            BtnAbrirChamado.IsEnabled = false;
            LoadingIndicator.IsRunning = true;
            LoadingIndicator.IsVisible = true;
            ResultLabel.Text = string.Empty;

            try
            {
                var chamado = new ChamadoFormViewModel
                {
                    IdUsuario = 1, // Valor fixo para teste
                    Titulo = TxtTitulo.Text,
                    Descricao = TxtDescricao.Text,
                    Categoria = (string)CategoryPicker.SelectedItem
                };

                // Validação mínima
                if (string.IsNullOrWhiteSpace(chamado.Titulo) || string.IsNullOrWhiteSpace(chamado.Descricao))
                {
                    await DisplayAlert("Erro", "Título e Descrição são obrigatórios.", "OK");
                    return;
                }

                // Chama o serviço para enviar à API
                var sucesso = await _chamadoService.EnviarChamado(chamado);

                if (sucesso)
                {
                    await DisplayAlert("Sucesso", "Chamado aberto e enviado ao suporte!", "OK");
                    ResultLabel.Text = "Chamado aberto e enviado ao suporte!";
                    ResultLabel.TextColor = Color.FromRgb(0, 150, 0); // Verde
                    TxtTitulo.Text = string.Empty;
                    TxtDescricao.Text = string.Empty;
                }
                else
                {
                    await DisplayAlert("Erro", "Falha ao enviar o chamado. Verifique a API.", "OK");
                    ResultLabel.Text = "Erro ao enviar. Verifique a API.";
                    ResultLabel.TextColor = Color.FromRgb(180, 0, 0); // Vermelho
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro de Conexão", $"Verifique a porta 5025 e o IP 10.0.2.2. Detalhe: {ex.Message}", "OK");
                ResultLabel.Text = $"Erro de conexão.";
                ResultLabel.TextColor = Color.FromRgb(180, 0, 0); // Vermelho
            }
            finally
            {
                LoadingIndicator.IsRunning = false;
                LoadingIndicator.IsVisible = false;
                BtnAbrirChamado.IsEnabled = true;
            }
        }
    }
}