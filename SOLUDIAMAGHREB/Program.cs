using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using SOLUDIAMAGHERB.Services.Contract;
using SOLUDIAMAGHERB.Services.Implementation;
using SOLUDIAMAGHREB.Data;
using SOLUDIAMAGHREB.Services.Contract;
using SOLUDIAMAGHREB.Services.Implementation;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
string strConx = builder.Configuration.GetConnectionString("QuerySQL");
builder.Services.AddDbContext<DbsoludiaContext>(options =>
{
    options.UseSqlServer(strConx);
});
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LogoutPath = "/Home/LogoutAsync";
        options.LoginPath = "/Login/LoginSession";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(
            new ResponseCacheAttribute
            {
                NoStore = true,
                Location = ResponseCacheLocation.None,
            }
        );
});
builder.Services.AddScoped<IActmanagerRepository, ActmanagerRepository>();
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
    name: "default",
    pattern: "{controller=Login}/{action=LoginSession}/{id?}");

IWebHostEnvironment env = app.Environment;
RotativaConfiguration.Setup(env.WebRootPath, "../Rotativa/Windows");

// Configure Rotativa to use wkhtmltopdf executable from wwwroot/Rotativa

app.Run();
