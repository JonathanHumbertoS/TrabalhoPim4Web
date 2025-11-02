// No arquivo Controllers/ChamadoController.cs

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TrabalhoPim4Web.DataAccess;
using TrabalhoPim4Web.Models;


namespace TrabalhoPim4Web.Controllers
{
    // Nota: Em um projeto real, você adicionaria [Authorize] aqui para proteger a rota
    public class ChamadoController : Controller
    {
        private readonly ChamadoDAO _chamadoDAO;

        // Injeção de Dependência do ChamadoDAO
        public ChamadoController(ChamadoDAO chamadoDAO)
        {
            _chamadoDAO = chamadoDAO;
        }

        // 1. Action para exibir a lista de chamados (GET: /Chamado/Listar)
        [HttpGet]
        public IActionResult Listar()
        {
            List<Chamado> chamados = new List<Chamado>();
            try
            {
                // Chama o DAO para buscar os dados no DB
                chamados = _chamadoDAO.ListarTodos();
            }
            catch (Exception ex)
            {
                // Trata erro de DB (mostra alerta no topo, mas permite renderizar a view)
                TempData["ErroDB"] = "Erro ao carregar a lista de chamados. Verifique o banco de dados. Detalhe: " + ex.Message;
            }

            // Retorna a lista de chamados para a View
            return View(chamados);
        }

        // Dentro da classe ChamadoController.cs (Adicione após o método Listar)

        // 2. Action para exibir o formulário de criação (GET: /Chamado/Criar)
        [HttpGet]
        public IActionResult Criar()
        {
            // Retorna a view com um ViewModel vazio
            return View(new ViewModels.ChamadoFormViewModel());
        }

        // 3. Action para processar o formulário de criação (POST)
        [HttpPost]
        public IActionResult Criar(ViewModels.ChamadoFormViewModel model)
        {
            // Validação do modelo (requerimentos definidos no ChamadoFormViewModel.cs)
            if (ModelState.IsValid)
            {
                try
                {
                    // Mapeia o ViewModel para o Model (DTO)
                    Chamado novoChamado = new Chamado
                    {
                        Titulo = model.Titulo,
                        Descricao = model.Descricao,
                        // O status e data serão definidos no DAO.
                    };

                    _chamadoDAO.Inserir(novoChamado);

                    // Sucesso: Redireciona para a lista
                    TempData["MensagemSucesso"] = "Chamado aberto com sucesso!";
                    return RedirectToAction("Listar");
                }
                catch (Exception ex)
                {
                    // Se o DAO lançar uma exceção de DB
                    ModelState.AddModelError(string.Empty, "Erro ao abrir o chamado. Tente novamente mais tarde.");
                    Console.WriteLine($"Erro ao criar chamado: {ex.Message}");
                }
            }

            // Se a validação falhar ou ocorrer erro de DB, retorna o formulário
            return View(model);
        }// Dentro da classe ChamadoController.cs (Adicione após o método Criar)

        // 4. Action para exibir os detalhes e o formulário de atualização de status (GET: /Chamado/Detalhes/{id})
        [HttpGet]
        public IActionResult Detalhes(int id)
        {
            try
            {
                Chamado? chamado = _chamadoDAO.BuscarPorId(id);

                if (chamado == null)
                {
                    TempData["MensagemErro"] = $"Chamado ID {id} não encontrado.";
                    return RedirectToAction("Listar");
                }

                // Retorna o objeto Chamado para a View
                return View(chamado);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao carregar detalhes do chamado. " + ex.Message;
                Console.WriteLine($"Erro ao carregar detalhes: {ex.Message}");
                return RedirectToAction("Listar");
            }
        }

        // 5. Action para processar a atualização de status (POST)
        [HttpPost]
        public IActionResult AtualizarStatus(int id, string novoStatus)
        {
            if (string.IsNullOrEmpty(novoStatus))
            {
                TempData["MensagemErro"] = "O novo status não pode ser vazio.";
                return RedirectToAction("Detalhes", new { id = id });
            }

            try
            {
                bool sucesso = _chamadoDAO.AtualizarStatus(id, novoStatus);

                if (sucesso)
                {
                    TempData["MensagemSucesso"] = $"Status do chamado ID {id} atualizado para '{novoStatus}'.";
                }
                else
                {
                    TempData["MensagemErro"] = $"Não foi possível atualizar o status do chamado ID {id}.";
                }

                // Redireciona de volta para a tela de detalhes ou lista
                return RedirectToAction("Listar");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao processar a atualização de status. " + ex.Message;
                Console.WriteLine($"Erro ao processar atualização de status: {ex.Message}");
                return RedirectToAction("Listar");
            }
        } // Os métodos Criar, Detalhes e Atualizar virão depois
    }
}