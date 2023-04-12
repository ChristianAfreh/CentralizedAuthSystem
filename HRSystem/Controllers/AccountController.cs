using HRSystem.Models;
using HRSystem.ViewModels;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using System.Text;

namespace HRSystem.Controllers
{

    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private IConfiguration _configuration;
        private string _tokenSecret;
        private string _authBaseUrl;
        private string _clientId;
        private string _clientSecret;

        public AccountController(IConfiguration configuration,SignInManager<IdentityUser> signInManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _tokenSecret = _configuration.GetSection("Auth")["TokenSecret"];
            _authBaseUrl = _configuration.GetSection("Auth")["AuthBaseUrl"];
            _clientId = _configuration.GetSection("Auth")["ClientId"];
            _clientSecret = _configuration.GetSection("Auth")["ClientSecret"];
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            
                if (ModelState.IsValid)
                {
                    var loginURL = $"{_authBaseUrl}/api/account/register";
                    RestClient client = new RestClient(loginURL);

                try
                {

                    var clientRequest = new RestRequest();
                    clientRequest.AddHeader("Cache-Control", "no-cache");
                    clientRequest.AddHeader("Content-Type", "application/json");
                    clientRequest.AddHeader("ClientId", _clientId);
                    clientRequest.AddHeader("ClientSecret", _clientSecret);
                    clientRequest.AddJsonBody(new
                    {
                        Surname = model.SurName,
                        Othernames = model.OtherNames,
                        Username = model.UserName,
                        Email = model.Email,
                        Password = model.Password,
                        PhoneNumber = model.PhoneNumber
                    });

                    RestResponse response = await client.PostAsync(clientRequest);
                    if (response.IsSuccessful)
                        return RedirectToAction("Login");

                    return View(model);
                }

                catch (CustomException ex)
                {
                    throw;
                }
            }
            
            

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            return View(model);
        }

        private bool IsTokenValid(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            return true;
        }

        private  TokenValidationParameters GetValidationParameters()
        {
            var tokenSecret = _tokenSecret;
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = "Sample",
                ValidAudience = "Sample",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret)) // The same key as the one that generate the token
            };
        }

        public IActionResult ErrorView()
        {
            return View();
        }
    }
}