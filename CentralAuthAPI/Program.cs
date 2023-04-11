using CentralAuthAPI.Extensions;
using CentralAuthAPI.Models.Data.MasterAccountDBContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



ConfigurationManager configuration = builder.Configuration;


//builder.Services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
// Add services to the container.
builder.Services.AddDbContext<AppUserDBContext>(options =>
					options.UseSqlServer(configuration.GetConnectionString("MasterAccountDBConnection")));
builder.Services.AddDbContext<ClientDBContext>(options =>
					options.UseSqlServer(configuration.GetConnectionString("MasterAccountDBConnection")));

//DI Container
builder.Services.RegisterRepositories();
builder.Services.RegisterServices();


// For Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
	.AddEntityFrameworkStores<AppUserDBContext>()
	.AddDefaultTokenProviders();

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
.AddJwtBearer(options =>
 {
	 options.SaveToken = true;
	 options.RequireHttpsMetadata = false;
	 options.TokenValidationParameters = new TokenValidationParameters()
	 {
		 //
		 ValidateIssuer = true,  //validates server that generates token
		 ValidateAudience = true, //validates client(recipient) that is authorized to receive token
		 ValidAudience = configuration["JWT:ValidAudience"],
		 ValidIssuer = configuration["JWT:ValidIssuer"],
		 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:JWTSecret"])) //validates the signature of the token
	 };
 });


builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
