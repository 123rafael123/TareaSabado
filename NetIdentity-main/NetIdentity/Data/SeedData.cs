using Microsoft.AspNetCore.Identity;
using NetIdentity.Models;

namespace NetIdentity.Data
{
    public static class SeedData
    {
        public static async Task Inicializar(IServiceProvider servicios)
        {
            var roles = servicios.GetRequiredService<RoleManager<IdentityRole>>();
            var usuarios = servicios.GetRequiredService<UserManager<ApplicationUser>>();
            var db = servicios.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureCreated();

            string[] nombresRoles = { "Admin", "Usuario" };
            foreach (var r in nombresRoles)
            {
                if (!await roles.RoleExistsAsync(r))
                    await roles.CreateAsync(new IdentityRole(r));
            }

            if (await usuarios.FindByEmailAsync("admin@local.com") == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@local.com",
                    Email = "admin@local.com",
                    FechaNacimiento = DateTime.Now.AddYears(-40),
                    NombreCompleto = "AdminGeneral",
                    EsFemenino = false,
                    EmailConfirmed = true
                };

                var crear = await usuarios.CreateAsync(admin, "ClaveAdmin1!");
                if (crear.Succeeded)
                    await usuarios.AddToRoleAsync(admin, "Admin");
            }

            if (await usuarios.FindByEmailAsync("menor@local.com") == null)
            {
                var menorEdad = new ApplicationUser
                {
                    UserName = "menor@local.com",
                    Email = "menor@local.com",
                    FechaNacimiento = DateTime.Now.AddYears(-15),
                    NombreCompleto = "UsuarioMenorEdad",
                    EsFemenino = false,
                    EmailConfirmed = true
                };

                var crear = await usuarios.CreateAsync(menorEdad, "ClaveMenor1!");
                if (crear.Succeeded)
                    await usuarios.AddToRoleAsync(menorEdad, "Usuario");
            }

            if (await usuarios.FindByEmailAsync("mayor@local.com") == null)
            {
                var mayorEdad = new ApplicationUser
                {
                    UserName = "mayor@local.com",
                    Email = "mayor@local.com",
                    FechaNacimiento = DateTime.Now.AddYears(-22),
                    NombreCompleto = "UsuariaMayorEdad",
                    EsFemenino = true,
                    EmailConfirmed = true
                };

                var crear = await usuarios.CreateAsync(mayorEdad, "ClaveMayor1!");
                if (crear.Succeeded)
                    await usuarios.AddToRoleAsync(mayorEdad, "Usuario");
            }
        }
    }
}
