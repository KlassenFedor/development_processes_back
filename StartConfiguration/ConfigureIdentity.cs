using dev_processes_backend.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace dev_processes_backend.StartConfiguration
{
    public static class ConfigureIdentity
    {
        public static async Task ConfigureIdentityAsync(this WebApplication app)
        {
            using var serviceScope = app.Services.CreateScope();
            var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
            var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();
            var config = app.Configuration.GetSection("DefaultUsersConfig");
            var superAdminRole = await roleManager.FindByNameAsync(RolesNames.SuperAdministrator);

            if (superAdminRole == null)
            {
                var roleResult = await roleManager.CreateAsync(new Role
                {
                    Name = RolesNames.SuperAdministrator,
                    Type = RoleType.SuperAdmin
                });
                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unable to create {RolesNames.SuperAdministrator} role.");
                }
                superAdminRole = await roleManager.FindByNameAsync(RolesNames.SuperAdministrator);
            }

            var superAdminUser = await userManager.FindByEmailAsync(config["SuperAdminEmail"]);
            if (superAdminUser == null)
            {
                var userResult = await userManager.CreateAsync(new User 
                {
                    FirstName = config["SuperAdminFirstName"],
                    LastName = config["SuperAdminLastName"],
                    Email = config["SuperAdminEmail"],
                    UserName = config["SuperAdminEmail"]
                },
                config["SuperAdminPassword"]);
                if (!userResult.Succeeded)
                {
                    Debug.WriteLine("123", userResult.Errors.First().Description);
                    throw new InvalidOperationException($"Unable to create {config["SuperAdminEmail"]} user");
                }
                superAdminUser = await userManager.FindByEmailAsync(config["SuperAdminEmail"]);
            }
            if (!await userManager.IsInRoleAsync(superAdminUser, superAdminRole.Name))
            {
                await userManager.AddToRoleAsync(superAdminUser, superAdminRole.Name);
            }

            var adminRole = await roleManager.FindByNameAsync(RolesNames.Administartor);
            if (adminRole == null)
            {
                var roleResult = await roleManager.CreateAsync(new Role
                {
                    Name = RolesNames.Administartor,
                    Type = RoleType.Admin
                });
                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unable to create {RolesNames.Administartor} role.");
                }
            }
        }
    }

}