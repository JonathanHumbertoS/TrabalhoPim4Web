// No arquivo DataAccess/ChamadoDAO.cs
using Npgsql;
using System.Collections.Generic;
using TrabalhoPim4Web.Models;

namespace TrabalhoPim4Web.DataAccess
{
    public class ChamadoDAO
    {
        private readonly string _connectionString;

        public ChamadoDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Chamado> ListarTodos()
        {
            List<Chamado> chamados = new List<Chamado>();
            string sql = "SELECT id, id_usuario, titulo, descricao, status, data_abertura FROM chamados ORDER BY data_abertura DESC";

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            chamados.Add(new Chamado
                            {
                                Id = reader.GetInt32(0),
                                IdUsuario = reader.GetInt32(1),
                                Titulo = reader.GetString(2),
                                Descricao = reader.GetString(3),
                                Status = reader.GetString(4),
                                DataAbertura = reader.GetDateTime(5)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao listar chamados: {ex.Message}");
                throw new Exception("Erro ao acessar o banco de dados.", ex);
            }

            return chamados;
        }

        // Dentro da classe ChamadoDAO.cs
        public void Inserir(Chamado chamado)
        {
            // Lembre-se: PostgreSQL usa minúsculas por padrão, e o ID é SERIAL (auto-incremento)
            string sql = "INSERT INTO chamados (id_usuario, titulo, descricao, status, data_abertura) " +
                         "VALUES (@id_usuario, @titulo, @descricao, @status, @data_abertura)";

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        // Parâmetros de SQL Injection Prevention
                        // NOTA: Para um projeto real, você precisaria de um sistema de sessão para obter o IdUsuario
                        cmd.Parameters.AddWithValue("id_usuario", 1); // Usando ID 1 temporariamente (admin)
                        cmd.Parameters.AddWithValue("titulo", chamado.Titulo.Trim());
                        cmd.Parameters.AddWithValue("descricao", chamado.Descricao.Trim());
                        cmd.Parameters.AddWithValue("status", "Aberto"); // Status inicial é sempre "Aberto"
                        cmd.Parameters.AddWithValue("data_abertura", DateTime.Now);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inserir chamado: {ex.Message}");
                throw new Exception("Erro ao acessar o banco de dados para inserir o chamado.", ex);
         
           
            }


        }
        // Dentro da classe ChamadoDAO.cs

        // 1. Método para buscar um chamado específico pelo ID
        public Chamado? BuscarPorId(int id)
        {
            string sql = "SELECT id, id_usuario, titulo, descricao, status, data_abertura FROM chamados WHERE id = @id";
            Chamado? chamado = null;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                chamado = new Chamado
                                {
                                    Id = reader.GetInt32(0),
                                    IdUsuario = reader.GetInt32(1),
                                    Titulo = reader.GetString(2),
                                    Descricao = reader.GetString(3),
                                    Status = reader.GetString(4),
                                    DataAbertura = reader.GetDateTime(5)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar chamado ID {id}: {ex.Message}");
                throw new Exception("Erro ao acessar o banco de dados para buscar chamado.", ex);
            }
            return chamado;
        }

        // 2. Método para atualizar apenas o status (ação do técnico)
        public bool AtualizarStatus(int idChamado, string novoStatus)
        {
            string sql = "UPDATE chamados SET status = @novoStatus WHERE id = @id";

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("novoStatus", novoStatus);
                        cmd.Parameters.AddWithValue("id", idChamado);

                        // Retorna true se pelo menos uma linha foi afetada
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar status do chamado ID {idChamado}: {ex.Message}");
                throw new Exception("Erro ao atualizar o status do chamado no banco de dados.", ex);
            }
        } // Os métodos Inserir, BuscarPorId, AtualizarStatus serão adicionados nos próximos passos.
    }
}