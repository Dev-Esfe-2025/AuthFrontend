using FrontAuth.WebApp.DTOs.UsuarioDTOs;
using FrontAuth.WebApp.Helpers;
using FrontAuth.WebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FrontAuth.WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult LoginWithGitHub()
        {
            var redirectUrl = Url.Action("GitHubCallback", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "GitHub");  
        }


        [HttpGet]
        public async Task<IActionResult> GitHubCallback()
        {
            //  Obtener los datos del esquema GitHub
            var authenticateResult = await HttpContext.AuthenticateAsync("GitHub");

            if (!authenticateResult.Succeeded)
                return RedirectToAction("Login");

            var user = authenticateResult.Principal;
            var email = user.FindFirstValue(ClaimTypes.Email);
            var name = user.Identity?.Name ?? "GitHub User";

            // Aquí  registrar o autenticar en la base de datos
            // var usuarioDto = await _authService.LoginOrRegisterExternalAsync(email, name);
            var usuarioDto = new LoginResponseDTO
            {
                Id = 1,
                Nombre = name,
                Email = email
            };

            var principal = ClaimsHelper.CrearClaimsPrincipal(usuarioDto);
            await HttpContext.SignInAsync("AuthCookie", principal);

            return RedirectToAction("Index", "Home");
        }


        //  GET: Mostrar Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //  POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(UsuarioLoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result == null)
            {
                ViewBag.Error = "Credenciales inválidas";
                return View();
            }
            //  Crear y firmar los claims usando el helper

            var principal = ClaimsHelper.CrearClaimsPrincipal(result);

            await HttpContext.SignInAsync("AuthCookie", principal);

            return RedirectToAction("Index", "Home");
        }


        //  POST: Registro
        [HttpPost]
        public async Task<IActionResult> Registrar(UsuarioRegistroDTO dto)
        {
            var result = await _authService.RegistrarAsync(dto);

            if (result == null || result.Id <= 0)
            {
                ViewBag.Error = "Error al registrar";
                return View();
            }

            //  Crear y firmar los claims usando el helper
            var principal = ClaimsHelper.CrearClaimsPrincipal(result);

            await HttpContext.SignInAsync("AuthCookie", principal);

            return RedirectToAction("Index", "Home");
        }

        //  Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AuthCookie");
            return RedirectToAction("Login");
        }
        //  GET: Mostrar Registro
        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }
    }
}
