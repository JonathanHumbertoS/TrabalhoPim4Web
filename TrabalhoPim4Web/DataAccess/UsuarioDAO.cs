// No arquivo DataAccess/UsuarioDAO.cs

using BCrypt.Net; // Requer o NuGet BCrypt.Net-Core
using Npgsql;
using TrabalhoPim4Web.Models;

namespace TrabalhoPim4Web.DataAccess
{
    public class UsuarioDAO
    {
        private readonly string _connectionString;

        public UsuarioDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Usuario? Autenticar(string email, string senha)
        {
            // A query busca apenas o HASH e o usuário pelo email (Obrigatório para BCrypt)
            string sql = "SELECT id, nome, email, senha_hash FROM usuarios WHERE email = @email";

            string emailLimpo = email.Trim();
            string senhaLimpa = senha.Trim();

            Usuario? usuario = null;

            try
            {
                using (NpgsqlConnection conexao = new NpgsqlConnection(_connectionString))
                {
                    conexao.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("email", emailLimpo);

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string hashArmazenado = reader["senha_hash"]?.ToString() ?? string.Empty;

                                // 1. VERIFICAÇÃO DO BCrypt (Segurança)
                                if (BCrypt.Net.BCrypt.Verify(senhaLimpa, hashArmazenado))
                                {
                                    // Login OK!
                                    usuario = new Usuario
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                                        Nome = reader["nome"]?.ToString() ?? string.Empty,
                                        Email = reader["email"]?.ToString() ?? string.Empty,
                                        Senha = string.Empty // Nunca retorne a senha/hash
                                    };
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Lança exceção para ser capturada no Controller
                Console.WriteLine($"Erro crítico de autenticação/DB: {ex.Message}");
                throw new Exception("Erro ao acessar o banco de dados durante a autenticação.", ex);
            }

            return usuario;
        }
    }
}