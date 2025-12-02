// PIM4Mobile/Services/ChamadoApiService.cs
using System.Text.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PIM4Mobile.ViewModels;

namespace PIM4Mobile.Services
{
    public class ChamadoApiService
    {
        // ⚠️ CORREÇÃO: Variável DEVE estar dentro da classe (10.0.2.2 é o IP do host)
        private const string BaseUrl = "http://192.168.3.4:5025";

        private readonly HttpClient _httpClient;

        public ChamadoApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> EnviarChamado(ChamadoFormViewModel chamado)
        {
            var json = JsonSerializer.Serialize(chamado);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Confirme que a URL da sua API está correta aqui
            var response = await _httpClient.PostAsync($"{BaseUrl}/api/chamado/abrir", content);

            return response.IsSuccessStatusCode;
        }
    }
}