using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using TrabalhoPim4Web.DataAccess;
using TrabalhoPim4Web.Models;
using TrabalhoPim4Web.ViewModels;

namespace TrabalhoPim3Web.Controllers
{
    public class ChamadoController : Controller
    {
        private readonly ChamadoDAO _chamadoDAO;

        // Injeção de Dependência
        public ChamadoController(ChamadoDAO chamadoDAO)
        {
            _chamadoDAO = chamadoDAO;
        }

        // 1. LISTAR (Read - R do CRUD)
        [HttpGet]
        public IActionResult Listar()
        {
            List<Chamado> chamados = new List<Chamado>();
            try
            {
                chamados = _chamadoDAO.ListarTodos();
            }
            catch (Exception ex)
            {
                TempData["ErroDB"] = "Erro ao carregar a lista de chamados. Detalhe: " + ex.Message;
            }
            return View(chamados);
        }

        // 2. CRIAR (Create - C do CRUD) - GET
        [HttpGet]
        public IActionResult Criar()
        {
            return View(new ChamadoFormViewModel());
        }

        // 2. CRIAR (Create - C do CRUD) - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Criar(ChamadoFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Chamado novoChamado = new Chamado
                    {
                        Titulo = model.Titulo,
                        Descricao = model.Descricao,
                        DataAbertura = DateTime.Now,
                        Status = "Aberto"
                    };
                    _chamadoDAO.Inserir(novoChamado);

                    TempData["MensagemSucesso"] = "Chamado aberto com sucesso!";
                    return RedirectToAction("Listar");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Erro ao abrir o chamado. Tente novamente mais tarde.");
                    Console.WriteLine($"Erro ao criar chamado: {ex.Message}");
                }
            }
            return View(model);
        }

        // 3. DETALHES
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
                return View(chamado);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao carregar detalhes do chamado. " + ex.Message;
                return RedirectToAction("Listar");
            }
        }

        // 4. EDITAR (Update - U do CRUD) - HTTP GET
        [HttpGet]
        public IActionResult Editar(int id)
        {
            try
            {
                Chamado? chamado = _chamadoDAO.BuscarPorId(id);

                if (chamado == null)
                {
                    TempData["MensagemErro"] = $"Chamado ID {id} não encontrado para edição.";
                    return RedirectToAction("Listar");
                }

                var model = new ChamadoFormViewModel
                {
                    Id = chamado.Id,
                    Titulo = chamado.Titulo,
                    Descricao = chamado.Descricao,
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao carregar os dados do chamado para edição. " + ex.Message;
                return RedirectToAction("Listar");
            }
        }

        // 4. EDITAR (Update - U do CRUD) - HTTP POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(ChamadoFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Chamado chamadoAtualizado = new Chamado
                    {
                        Id = model.Id,
                        Titulo = model.Titulo,
                        Descricao = model.Descricao,
                    };

                    _chamadoDAO.Atualizar(chamadoAtualizado);

                    TempData["MensagemSucesso"] = $"Chamado ID {model.Id} atualizado com sucesso!";
                    return RedirectToAction("Listar");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Erro ao salvar as alterações. Tente novamente mais tarde.");
                    Console.WriteLine($"Erro ao editar chamado: {ex.Message}");
                }
            }
            return View(model);
        }

        // 5. DELETAR (Delete - D do CRUD) - HTTP GET (Confirmação)
        [HttpGet]
        public IActionResult Deletar(int id)
        {
            try
            {
                // Busca o chamado para exibir na tela de confirmação
                Chamado? chamado = _chamadoDAO.BuscarPorId(id);

                if (chamado == null)
                {
                    TempData["MensagemErro"] = $"Chamado ID {id} não encontrado para exclusão.";
                    return RedirectToAction("Listar");
                }

                return View(chamado);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao carregar detalhes para exclusão. " + ex.Message;
                return RedirectToAction("Listar");
            }
        }

        // 5. DELETAR (Delete - D do CRUD) - HTTP POST (Execução)
        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletarConfirmado(int id)
        {
            try
            {
                // Chama o método DAO para remover do banco de dados
                _chamadoDAO.Excluir(id);

                TempData["MensagemSucesso"] = $"Chamado ID {id} excluído com sucesso!";
                return RedirectToAction("Listar");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao excluir o chamado ID {id}. Detalhe: {ex.Message}";
                return RedirectToAction("Listar");
            }
        }


        // 6. ATUALIZAR STATUS 
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
                _chamadoDAO.AtualizarStatus(id, novoStatus);
                TempData["MensagemSucesso"] = $"Status do chamado ID {id} atualizado para '{novoStatus}'.";
                return RedirectToAction("Listar");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao processar a atualização de status. " + ex.Message;
                return RedirectToAction("Listar");
            }
        }

        // 7. APOIO DE IA (AJAX) 
        [HttpGet]
        [Route("chamado/buscarsugestoesia")]
        public IActionResult BuscarSugestoesIA(string termo)
        {
            // Base de conhecimento (Simulação da IA)
            var baseDeConhecimento = new List<Sugestao>
            {
                new Sugestao { PalavraChave = "senha", Solucao = "Problema de SENHA: Tente usar a função 'Esqueceu a Senha' ou verifique se o Caps Lock está ativado." },
                new Sugestao { PalavraChave = "impressora", Solucao = "IMPRESSORA: Verifique se a impressora está ligada e conectada na rede. Tente reiniciar o spooler de impressão." },
                new Sugestao { PalavraChave = "acesso", Solucao = "ACESSO: Pode ser um problema de permissão. Limpe o cache do navegador e tente novamente." },
                new Sugestao { PalavraChave = "monitor", Solucao = "MONITOR SEM IMAGEM: Verifique se o cabo de energia está ligado na tomada e se o cabo de vídeo (HDMI/VGA) está firme no computador." },
                new Sugestao { PalavraChave = "lento", Solucao = "COMPUTADOR LENTO: Tente fechar abas desnecessárias no navegador ou reiniciar o computador." },
                new Sugestao { PalavraChave = "internet", Solucao = "FALHA DE INTERNET: 1. Verifique se o cabo de rede está conectado ao seu PC. 2. Tente reiniciar seu modem/roteador. 3. Verifique se a rede Wi-Fi está ativa." },
                new Sugestao { PalavraChave = "cache", Solucao = "NAVEGADOR LENTO/ERROS DE TELA: Tente limpar o cache e os cookies do seu navegador (Configurações > Privacidade e Segurança). Isso costuma resolver problemas de carregamento de páginas." },
                new Sugestao { PalavraChave = "travou", Solucao = "SISTEMA TRAVADO: Tente pressionar Ctrl + Alt + Del para abrir o Gerenciador de Tarefas e fechar o programa que não está respondendo. Se não funcionar, reinicie o PC." },
                new Sugestao { PalavraChave = "remoto", Solucao = "FALHA DE ACESSO REMOTO/VPN: Verifique se o seu software VPN está conectado e se a senha está correta. Se o problema persistir, o serviço de VPN pode estar inativo." },
                new Sugestao { PalavraChave = "email", Solucao = "E-MAIL NÃO ENVIA/RECEBE: Verifique se a sua conexão com a Internet está ativa e se as credenciais de login no seu cliente de e-mail (Outlook, Thunderbird, etc.) estão corretas." }
            };

            if (string.IsNullOrWhiteSpace(termo))
            {
                return Json(new List<Sugestao>());
            }

            string termoLower = termo.ToLowerInvariant();
            var sugestoes = baseDeConhecimento
                .Where(s => termoLower.Contains(s.PalavraChave.ToLowerInvariant()))
                .ToList();

            return Json(sugestoes);
        }
    }
}