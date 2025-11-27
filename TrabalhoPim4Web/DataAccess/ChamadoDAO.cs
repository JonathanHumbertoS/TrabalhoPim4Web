// No arquivo DataAccess/ChamadoDAO.cs
using Npgsql;
using System; // Adicionado para usar Exception e DateTime.Now
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

        // 1. LISTAR TODOS
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

        // 2. INSERIR (CRIAR)
        public void Inserir(Chamado chamado)
        {
            string sql = "INSERT INTO chamados (id_usuario, titulo, descricao, status, data_abertura) " +
                         "VALUES (@id_usuario, @titulo, @descricao, @status, @data_abertura)";

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        // NOTA: Ajuste o IdUsuario se você tiver um sistema de sessão
                        cmd.Parameters.AddWithValue("id_usuario", 1);
                        cmd.Parameters.AddWithValue("titulo", chamado.Titulo.Trim());
                        cmd.Parameters.AddWithValue("descricao", chamado.Descricao.Trim());
                        cmd.Parameters.AddWithValue("status", "Aberto");
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

        // 3. BUSCAR POR ID (Para Detalhes e Edição - GET)
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

        // 4. ATUALIZAR (EDITAR - POST) - O MÉTODO QUE FALTAVA!
        public void Atualizar(Chamado chamado)
        {
            // Atualiza apenas os campos editáveis (Título e Descrição)
            string sql = @"
                UPDATE chamados SET
                    titulo = @titulo,
                    descricao = @descricao
                WHERE id = @id";

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        // Passando os novos valores e o ID para o WHERE
                        cmd.Parameters.AddWithValue("titulo", chamado.Titulo.Trim());
                        cmd.Parameters.AddWithValue("descricao", chamado.Descricao.Trim());
                        cmd.Parameters.AddWithValue("id", chamado.Id); // Essencial para saber qual linha atualizar

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar chamado ID {chamado.Id}: {ex.Message}");
                throw new Exception("Erro ao tentar atualizar o chamado no banco de dados.", ex);
            }
        }

        // 5. ATUALIZAR STATUS
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

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar status do chamado ID {idChamado}: {ex.Message}");
                throw new Exception("Erro ao atualizar o status do chamado no banco de dados.", ex);
            }
        }
        // No arquivo DataAccess/ChamadoDAO.cs

        // ... (seus outros métodos como ListarTodos, Inserir, BuscarPorId, Atualizar) ...

        // 5. EXCLUIR (DELETAR)
        public void Excluir(int id)
        {
            // Comando SQL DELETE
            string sql = "DELETE FROM chamados WHERE id = @id";

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        // Parâmetro essencial para identificar qual chamado excluir
                        cmd.Parameters.AddWithValue("id", id);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            // É bom verificar se a linha foi realmente deletada
                            throw new Exception($"Chamado ID {id} não encontrado no banco de dados para exclusão.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir chamado ID {id}: {ex.Message}");
                // Relança a exceção para que o Controller possa tratar
                throw new Exception("Erro ao tentar excluir o chamado no banco de dados.", ex);
            }
        }
    }
}