using ABPTestTask.Common.User;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Authorization
{
    public class SeedRoles
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedRoles>>();

                string[] roleNames = { "Admin", "User" };

                try
                {
                    // Create roles
                    foreach (var roleName in roleNames)
                    {
                        var roleExist = await roleManager.RoleExistsAsync(roleName);
                        if (!roleExist)
                        {
                            await roleManager.CreateAsync(new Role { Name = roleName });
                            logger.LogInformation($"Role '{roleName}' created.");
                        }
                    }

                    // Create admin user
                    var powerUser = new User
                    {
                        UserName = "Admin",
                        Email = "admin@example.com"
                    };

                    string userPassword = "Admin!123"; // Consider using secure password management
                    var user = await userManager.FindByEmailAsync(powerUser.Email);

                    if (user == null)
                    {
                        var createPowerUser = await userManager.CreateAsync(powerUser, userPassword);
                        if (createPowerUser.Succeeded)
                        {
                            // Assign role to user
                            await userManager.AddToRoleAsync(powerUser, "Admin");
                            await userManager.AddClaimAsync(powerUser, new Claim(ClaimTypes.Role, "Admin"));

                            logger.LogInformation("Admin user created and role assigned.");
                        }
                        else
                        {
                            logger.LogWarning("Admin user creation failed: {Errors}", string.Join(", ", createPowerUser.Errors.Select(e => e.Description)));
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding roles and users.");
                }
            }
        }
    }
}
