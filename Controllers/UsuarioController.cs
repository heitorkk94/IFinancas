using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using IFinancas.Models;
using IFinancas.Repositories;

namespace IFinancas.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        [HttpGet]
        public IActionResult Registro()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registro(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuarioExistente = _usuarioRepository.ObterPorEmail(model.Email);
                if (usuarioExistente != null)
                {
                    ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");
                    return View(model);
                }

                var novoUsuario = new Usuario
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    DataCadastro = DateTime.Now
                };

                // Criptografa a senha antes de salvar
                novoUsuario.SenhaHash = _passwordHasher.HashPassword(novoUsuario, model.Senha);

                _usuarioRepository.Adicionar(novoUsuario);
                _usuarioRepository.SalvarAlteracoes();

                TempData["MensagemSucesso"] = "Cadastro realizado com sucesso! Faça o login.";
                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = _usuarioRepository.ObterPorEmail(model.Email);

                if (usuario != null)
                {
                    // Verifica se a senha confere com o hash
                    var resultadoVerificacao = _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, model.Senha);

                    if (resultadoVerificacao == PasswordVerificationResult.Success)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                            new Claim(ClaimTypes.Name, usuario.Nome),
                            new Claim(ClaimTypes.Email, usuario.Email)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
                        };

                        // Cria o cookie de sessão autenticada no navegador
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "E-mail ou senha incorretos.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Remove o cookie do navegador
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}