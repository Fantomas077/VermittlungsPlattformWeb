using Microsoft.AspNetCore.Authentication.Cookies;
using VermittlungsPlattform.Models.Db;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<VermittlungsplattformDbContext>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Configuration de base
        options.LogoutPath = "/Account/Logout";

        // Gestion de redirection personnalisée
        options.Events.OnRedirectToLogin = context =>
        {
            var path = context.Request.Path;

            if (path.StartsWithSegments("/Unternehmen"))
            {
                context.Response.Redirect("/Account/LoginUnternehmen");
            }
            else if (path.StartsWithSegments("/Student"))
            {
                context.Response.Redirect("/Account/LoginStudent");
            }
            else
            {
                context.Response.Redirect("/Account/Login"); // Par défaut
            }

            return Task.CompletedTask;
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Student",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Unternehmen",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
