// No arquivo Models/Usuario.cs
namespace TrabalhoPim4Web.Models
{
    public class Usuario
    {
        public int Id { get; set; } 
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty; // Representa a senha em texto simples (não será armazenada)
    }
}