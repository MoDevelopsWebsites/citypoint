using Microsoft.AspNetCore.Identity;

namespace CityPointHire.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roles = new[] { "Admin", "Staff", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Admin user
            var adminEmail = "admin@citypointhire.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Staff user
            var staffEmail = "staff@citypointhire.com";
            var staffUser = await userManager.FindByEmailAsync(staffEmail);
            if (staffUser == null)
            {
                staffUser = new IdentityUser
                {
                    UserName = staffEmail,
                    Email = staffEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(staffUser, "Staff123!");
                await userManager.AddToRoleAsync(staffUser, "Staff");
            }
        }
    }
}
