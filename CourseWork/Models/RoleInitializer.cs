using CourseWork.Models;  // пространство имен модели User
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace CourseWork
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string adminUserName = "admin";
            string password = "admin";
            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (await roleManager.FindByNameAsync("Unblocked") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Unblocked"));
            }
            if (await roleManager.FindByNameAsync("Blocked") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Blocked"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User 
                { 
                    Email = adminEmail, 
                    UserName = adminUserName, 
                    RegistrationDate = DateTime.Now, 
                    LastLoginDate = DateTime.Now 
                };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                    await userManager.AddToRoleAsync(admin, "Unblocked");
                }
            }
        }
    }
}