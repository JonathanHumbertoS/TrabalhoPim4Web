// No arquivo Models/Chamado.cs
using System;
namespace TrabalhoPim4Web.Models
{
    public class Chamado
    {
        public int Id { get; set; } 
        public int IdUsuario { get; set; } 
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Aberto, Em Andamento, Fechado
        public DateTime DataAbertura { get; set; } 
    }
}