using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Social_network.BLL.Intarface;
using Social_network.BLL.Validations;
using Social_network.DAL;
using Social_network.DAL.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

string connecting = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ContextSocial_Network_Context>(opt => opt.UseSqlServer(connecting));

builder.Services.AddScoped<IValidationUser, ValidationUser>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<ICorrectDataUserValidation, CorrectDataUserValidation>();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Home/Login";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
   
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
