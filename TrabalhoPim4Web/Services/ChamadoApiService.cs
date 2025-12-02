using TrabalhoPim4Web.ViewModels; // Use a referência ao seu ViewModel!
using System.Net.Http;
using System.Text;
using System.Text.Json;

// ...

public class ChamadoApiService
{
    private readonly HttpClient _httpClient = new HttpClient();

    // ⚠️ ATENÇÃO: SUBSTITUA PELO SEU IP REAL E PORTA
    // Ex: Se o IP do seu PC é 192.168.1.10 e o ASP.NET roda na porta 5000:
    private const string BaseUrl = "http://192.168.3.4:5000";

    public async Task<(bool Success, string Message)> EnviarChamado(string titulo, string descricao, string categoria)
    {
        // 1. Mapeia os dados da tela para o ViewModel
        var model = new ChamadoFormViewModel
        {
            Titulo = titulo,
            Descricao = descricao,
            Categoria = categoria
            // O IdUsuario e outros campos podem ser adicionados aqui se necessário
        };

        // 2. Serialização: Converte o objeto C# em uma string JSON
        string json = JsonSerializer.Serialize(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            // 3. Envio HTTP POST para o endpoint preparado
            // A rota completa será: http://192.168.X.X:PORTA_DO_WEBAPP/api/chamados/abrir
            HttpResponseMessage response = await _httpClient.PostAsync($"{BaseUrl}/api/chamados/abrir", content);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                // Sucesso (código HTTP 200)
                return (true, "Chamado aberto com sucesso!");
            }
            else
            {
                // Falha (códigos HTTP 400 ou 500)
                // Retorna a mensagem de erro detalhada do servidor
                return (false, $"Falha ao abrir chamado. Código: {response.StatusCode}. Detalhe: {responseBody}");
            }
        }
        catch (Exception ex)
        {
            // Erro de rede/conexão (Servidor desligado, IP errado)
            return (false, $"Erro de conexão: Não foi possível alcançar o servidor. Detalhe: {ex.Message}");
        }
    }
}