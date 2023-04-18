using HRSystem.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<HRSystemDBContext>(opt =>
                    opt.UseSqlServer(configuration.GetConnectionString("HRSystemDBConnection")));

builder.Services.AddIdentity<IdentityUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<HRSystemDBContext>(); ;
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<SignInManager<IdentityUser>>();

builder.Services.ConfigureApplicationCookie(opt =>
{
	opt.Cookie.HttpOnly = true;
	opt.ExpireTimeSpan = TimeSpan.FromMinutes(15);
	opt.LoginPath = "/Account/Login";
	opt.SlidingExpiration = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("CookieSettings", options));


builder.Services.AddSession(opt =>
{
	opt.Cookie.IsEssential = true;
	opt.Cookie.HttpOnly = true;
	opt.IdleTimeout = TimeSpan.FromMinutes(15);
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
app.UseCookiePolicy();
app.UseAuthentication();
app.UseSession();
app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
