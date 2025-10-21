using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FrontAuth.WebApp.Views.ViewComponents
{

    public class UserInfoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var user = HttpContext.User; // Aquí tomamos el usuario desde el HttpContext

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var nombre = user.FindFirstValue(ClaimTypes.Name);
                var rol = user.FindFirstValue(ClaimTypes.Role);

                ViewBag.Nombre = nombre;
                ViewBag.Rol = rol;
            }
            else
            {
                ViewBag.Nombre = "Invitado";
                ViewBag.Rol = "";
            }

            return View();
        }
    }

}
