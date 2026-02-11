// Imports the Data namespace so we can access ApplicationDbContext and SeedData
using CityPointHire.Data;

// Imports ASP.NET Core Identity for authentication and user management
using Microsoft.AspNetCore.Identity;

// Imports Entity Framework Core for database access
using Microsoft.EntityFrameworkCore;

// Creates a WebApplication builder to configure services and middleware
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Retrieves the connection string named "DefaultConnection" from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       // Throws an exception if the connection string is not found
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Registers the ApplicationDbContext with dependency injection and configures it to use SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Adds a developer-friendly database exception filter (helps during development)
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configures default Identity settings for authentication
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    // Enables role support (Admin, Staff, User)
    .AddRoles<IdentityRole>()
    // Configures Identity to use Entity Framework with ApplicationDbContext
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Adds MVC controllers with views support
builder.Services.AddControllersWithViews();

// Adds Razor Pages support (used for Identity UI pages like Login/Register)
builder.Services.AddRazorPages();

// Builds the application after configuring services
var app = builder.Build();

// Configure the HTTP request pipeline.
// Checks if the app is running in Development environment
if (app.Environment.IsDevelopment())
{
    // Enables automatic database migrations endpoint during development
    app.UseMigrationsEndPoint();
}
else
{
    // Uses a custom error handling page in production
    app.UseExceptionHandler("/Home/Error");
    // Enables HTTP Strict Transport Security for security
    app.UseHsts();
}

// Redirects HTTP requests to HTTPS
app.UseHttpsRedirection();

// Enables serving static files (CSS, JS, images)
app.UseStaticFiles();

// Enables routing for incoming HTTP requests
app.UseRouting();

// Enables authentication middleware (login system)
app.UseAuthentication();

// Enables authorization middleware (role-based access)
app.UseAuthorization();

// Seed Admin & Roles
// Creates a service scope to resolve scoped services like UserManager and RoleManager
using (var scope = app.Services.CreateScope())
{
    // Gets the service provider within the scope
    var services = scope.ServiceProvider;
    // Calls the SeedData method to create roles and default users
    await SeedData.InitializeAsync(services);
}

// Defines the default controller route pattern
app.MapControllerRoute(
    name: "default", // Route name
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Default route structure

// Maps Razor Pages (used by Identity UI)
app.MapRazorPages();

// Runs the application
app.Run();
