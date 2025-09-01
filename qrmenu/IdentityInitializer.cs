using Microsoft.AspNetCore.Identity;

public static class IdentityInitializer
{
    public static async Task SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {

        if (!await roleManager.RoleExistsAsync("admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("admin"));
        }

        var adminUser = await userManager.FindByNameAsync("admin@eycms.com");
        if (adminUser == null)
        {
            var user = new IdentityUser
            {
                UserName = "admin@eycms.com",
                Email = "admin@eycms.com",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "admin123*");
            if (!result.Succeeded)
            {
                throw new Exception("Admin kullanıcı oluşturulamadı: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await userManager.AddToRoleAsync(user, "admin");
        }
        else
        {

            if (!await userManager.IsInRoleAsync(adminUser, "admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "admin");
            }
        }


    }
}