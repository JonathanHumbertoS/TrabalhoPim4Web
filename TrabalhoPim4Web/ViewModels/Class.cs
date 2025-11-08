
// No arquivo ViewModels/Sugestao.cs
namespace TrabalhoPim4Web.ViewModels
{
    public class Sugestao
    {
        // Palavra ou frase que a IA deve buscar (ex: "senha", "impressora")
        public string PalavraChave { get; set; } = string.Empty;

        // Texto de solução que a IA deve retornar
        public string Solucao { get; set; } = string.Empty;
    }
}