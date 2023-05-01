using AccountingSystem.Extensions;
using AccountingSystem.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<AccountingSystemDBContext>(opt =>
                    opt.UseSqlServer(configuration.GetConnectionString("AccountingSystemDBConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = true)
                    .AddEntityFrameworkStores<AccountingSystemDBContext>(); ;
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<SignInManager<IdentityUser>>();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.HttpOnly = true;
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    opt.LoginPath = "/Account/Login";
    opt.SlidingExpiration = true;
    opt.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(
        options =>
        {
            options.LoginPath = "/Account/Login";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            options.SlidingExpiration = true;
            options.AccessDeniedPath = "/Account/AccessDenied";
        });


builder.Services.AddSession(opt =>
{
    opt.Cookie.IsEssential = true;
    opt.Cookie.HttpOnly = true;
    opt.IdleTimeout = TimeSpan.FromMinutes(15);
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
app.UseCookiePolicy();
app.UseSession();
AppHttpContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());   
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
