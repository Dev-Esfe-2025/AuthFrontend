using FrontAuth.WebApp.DTOs.UsuarioDTOs;
using System.Security.Claims;

namespace FrontAuth.WebApp.Helpers
{
    public static class ClaimsHelper
    {
        public static ClaimsPrincipal CrearClaimsPrincipal(LoginResponseDTO usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nombre ?? string.Empty),
                new Claim(ClaimTypes.Email, usuario.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, usuario.Rol ?? "User"), // rol por defecto
                new Claim("Token", usuario.Token ?? string.Empty)
            };

            var identity = new ClaimsIdentity(claims, "AuthCookie");
            return new ClaimsPrincipal(identity);
        }
    }
}
