using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetIdentity.Models;

namespace NetIdentity.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> loginManager;
        private readonly UserManager<ApplicationUser> userData;

        public AccountController(SignInManager<ApplicationUser> loginMgr, UserManager<ApplicationUser> userMgr)
        {
            loginManager = loginMgr;
            userData = userMgr;
        }

        [HttpGet]
        public IActionResult IniciarSesion() => View("Login");

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string clave)
        {
            var persona = await userData.FindByEmailAsync(correo);
            if (persona != null)
            {
                var ingreso = await loginManager.PasswordSignInAsync(persona, clave, false, false);
                if (ingreso.Succeeded)
                    return RedirectToAction("Inicio", "Home");
            }

            ViewBag.Error = "Datos incorrectos, revisa tu correo o contraseña.";
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> CerrarSesion()
        {
            await loginManager.SignOutAsync();
            return RedirectToAction("Inicio", "Home");
        }

        [HttpGet]
        public IActionResult Registro() => View("Register");

        [HttpPost]
        public async Task<IActionResult> Registro(string correo, string clave, DateTime nacimiento, string nombre, bool esMujer)
        {
            var nuevo = new ApplicationUser
            {
                UserName = correo,
                Email = correo,
                FechaNacimiento = nacimiento,
                NombreCompleto = nombre,
                EsFemenino = esMujer
            };

            var crear = await userData.CreateAsync(nuevo, clave);
            if (crear.Succeeded)
            {
                await userData.AddClaimAsync(nuevo, new System.Security.Claims.Claim("FechaNacimiento", nacimiento.ToString("yyyy-MM-dd")));
                await userData.AddClaimAsync(nuevo, new System.Security.Claims.Claim("Genero", esMujer ? "Femenino" : "Masculino"));
                await loginManager.SignInAsync(nuevo, false);
                return RedirectToAction("Inicio", "Home");
            }

            foreach (var error in crear.Errors)
                ModelState.AddModelError("", error.Description);

            return View("Register");
        }
    }
}
