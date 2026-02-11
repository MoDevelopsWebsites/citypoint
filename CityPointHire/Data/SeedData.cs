// Imports ASP.NET Core Identity
using Microsoft.AspNetCore.Identity;

// Defines namespace for data-related classes
namespace CityPointHire.Data
{
    // Static class used for seeding initial roles and users
    public static class SeedData
    {
        // Async method that runs when application starts
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            // Retrieves the RoleManager service from dependency injection
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Retrieves the UserManager service from dependency injection
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Defines an array of roles to create
            string[] roles = new[] { "Admin", "Staff", "User" };

            // Loops through each role
            foreach (var role in roles)
            {
                // Checks if role already exists in database
                if (!await roleManager.RoleExistsAsync(role))
                    // Creates the role if it does not exist
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Admin user
            // Defines admin email
            var adminEmail = "admin@citypointhire.com";

            // Checks if admin user already exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            // If admin does not exist
            if (adminUser == null)
            {
                // Creates new IdentityUser object
                adminUser = new IdentityUser
                {
                    // Sets username
                    UserName = adminEmail,
                    // Sets email
                    Email = adminEmail,
                    // Automatically confirms email
                    EmailConfirmed = true
                };

                // Creates the admin user with password
                await userManager.CreateAsync(adminUser, "Admin123!");

                // Assigns Admin role to the user
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Staff user
            // Defines staff email
            var staffEmail = "staff@citypointhire.com";

            // Checks if staff user already exists
            var staffUser = await userManager.FindByEmailAsync(staffEmail);

            // If staff does not exist
            if (staffUser == null)
            {
                // Creates new IdentityUser object
                staffUser = new IdentityUser
                {
                    // Sets username
                    UserName = staffEmail,
                    // Sets email
                    Email = staffEmail,
                    // Automatically confirms email
                    EmailConfirmed = true
                };

                // Creates the staff user with password
                await userManager.CreateAsync(staffUser, "Staff123!");

                // Assigns Staff role to the user
                await userManager.AddToRoleAsync(staffUser, "Staff");
            }
        }
    }
}
