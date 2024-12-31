using Microsoft.AspNetCore.Authentication.Cookies;
using VermittlungsPlattform.Models.Db;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Ajout du contexte de la base de donn�es
builder.Services.AddDbContext<VermittlungsplattformDbContext>();

// Configuration de l'authentification par cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";  // Chemin de la page de connexion
        options.LogoutPath = "/Account/Logout"; // Chemin pour la d�connexion
        options.AccessDeniedPath = "/Account/AccessDenied"; // Si acc�s refus�
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Activation de l'authentification et de l'autorisation
app.UseAuthentication();
app.UseAuthorization();

// Routes pour les areas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Route par d�faut
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
