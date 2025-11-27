// No arquivo Controllers/AccountController.cs

using Microsoft.AspNetCore.Mvc;
using TrabalhoPim4Web.DataAccess;
using TrabalhoPim4Web.Models;
using TrabalhoPim4Web.ViewModels; // Garanta que todos os usings estão corretos

namespace TrabalhoPim4Web.Controllers
{
    // A rota padrão é Account/Login
    public class AccountController : Controller
    {
        private readonly UsuarioDAO _usuarioDAO;


        // Injeção de Dependência do UsuarioDAO (Configurado no Program.cs)
        public AccountController(UsuarioDAO usuarioDAO)
        {
            _usuarioDAO = usuarioDAO;
        }

        // 1. Action para exibir a tela de login (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 2. Action para processar o formulário de login (POST)
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            // Validação básica se a senha e email foram digitados
            if (ModelState.IsValid)
            {
                try
                {
                    // Tenta autenticar no banco de dados
                    Usuario? usuarioLogado = _usuarioDAO.Autenticar(model.Email, model.Senha);

                    if (usuarioLogado != null)
                    {
                        // SUCESSO! Redireciona para a lista de chamados
                        return RedirectToAction("Listar", "Chamado");
                        // NOTA: Para um projeto real, aqui você usaria HttpContext.SignInAsync para criar o cookie de autenticação.
                    }

                    // Falha de credencial (retorno null do DAO)
                    ModelState.AddModelError(string.Empty, "Credenciais inválidas. Verifique seu e-mail e senha.");
                }
                catch (Exception ex)
                {
                    // Erro de conexão com o banco de dados (captura a Exception lançada pelo DAO)
                    ModelState.AddModelError(string.Empty, "Erro no servidor ao tentar conectar. Tente novamente mais tarde.");

                    // Loga o erro no console do Visual Studio (para você)
                    Console.WriteLine($"ERRO FATAL NA AUTENTICAÇÃO: {ex.Message}");
                }
            }

            // Se houve erro (DB ou credenciais inválidas), retorna para a View de Login
            return View(model);
        }
    }
}