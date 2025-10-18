using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetIdentity.Models;

namespace NetIdentity.Controllers
{
    public class JuegosController : Controller
    {
        private readonly UserManager<ApplicationUser> datos;

        public JuegosController(UserManager<ApplicationUser> gestor)
        {
            datos = gestor;
        }

        [Authorize(Policy = "menoresEdad")]
        public async Task<IActionResult> ZonaMenorEdad()
        {
            var actual = await datos.GetUserAsync(User);
            if (actual != null && actual.EsFemenino)
                return Content("🚫 No disponible para género femenino.");
            return View("Index");
        }

        [Authorize(Roles = "Admin,Usuario")]
        public async Task<IActionResult> ModoLibre()
        {
            var actual = await datos.GetUserAsync(User);
            if (actual != null && actual.EsFemenino)
                return Content("Solo permitido para género masculino.");
            return View("JuegoEducativo");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ZonaAdmin()
        {
            var actual = await datos.GetUserAsync(User);
            if (actual != null && actual.EsFemenino)
                return Content("Acceso bloqueado para género femenino.");
            return View("Aventuras");
        }
    }
}
