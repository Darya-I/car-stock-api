using CarStockDAL.Models;
using Microsoft.AspNetCore.Identity;

namespace CarStockBLL
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var roleNames = new[] { "Admin", "Manager", "User" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.FindByNameAsync(roleName);
                if (roleExist == null)  
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            await CreateUserWithRole(userManager, "admin@admin.com", "Admin123!", "Admin");
            await CreateUserWithRole(userManager, "manager@manager.com", "Manager123!", "Manager");
            await CreateUserWithRole(userManager, "user@user.com", "User123!", "User");
        }

        private static async Task CreateUserWithRole(UserManager<User> userManager, string email, string password, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    UserName = email,
                    Email = email
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role); 
                }
            }
        }
    }
}
