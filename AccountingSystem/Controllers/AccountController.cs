using AccountingSystem.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using AccountingSystem.Extensions;

namespace AccountingSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private IConfiguration _configuration;
        private string _tokenSecret;
        private string _authBaseUrl;
        private string _clientId;
        private string _clientSecret;

        public AccountController(IConfiguration configuration, SignInManager<IdentityUser> signInManager)
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
            ViewBag.Message = TempData["msg"];
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

                    ViewBag.Message = TempData["msg"];
                    return View(model);
                }

                catch (HttpRequestException ex)
                {
                    string msg = "";
                    msg = ex.Message;
                    TempData["msg"] = msg;
                    return RedirectToAction("Register");
                }
            }



            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {

            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                returnUrl = returnUrl ?? Url.Content("~/");

                if (ModelState.IsValid)
                {
                    var loginURL = $"{_authBaseUrl}/api/account/login";
                    RestClient client = new RestClient(loginURL);

                    var clientRequest = new RestRequest();
                    clientRequest.AddHeader("Cache-Control", "no-cache");
                    clientRequest.AddHeader("Content-Type", "application/json");
                    clientRequest.AddHeader("ClientId", _clientId);
                    clientRequest.AddHeader("ClientSecret", _clientSecret);
                    clientRequest.AddJsonBody(new
                    {
                        Username = model.UserName,
                        Password = model.Password,
                    });

                    RestResponse response = await client.PostAsync(clientRequest);

                    if (response.IsSuccessful)
                    {

                        var loginResponse = JsonConvert.DeserializeObject<LoginResponseViewModel>(response.Content);

                        HttpContext.Session.Set(SessionValueKeys.loginResponse, loginResponse);
                        HttpContext.Session.Set(SessionValueKeys.userId, loginResponse.UserId);
                        HttpContext.Session.Set(SessionValueKeys.userName, loginResponse.UserName);
                        

                        var verifiedToken = VefifyToken(loginResponse.AccessToken);

                        var claims = verifiedToken.Claims;

                        var user = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

                        ClaimsPrincipal principal = new(user);

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                            HttpContext.Session.Set(SessionValueKeys.isAuthenticated, principal.Identity.IsAuthenticated);


                        return LocalRedirect(returnUrl);
  
                    }

                }
                return View(model);
            }
            catch (HttpRequestException ex)
            {

                string msg = "";
                msg = ex.Message;
                TempData["Message"] = msg;
                return View(model);
            }


        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Set(SessionValueKeys.isAuthenticated, false);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _signInManager.SignOutAsync();
            HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
            await HttpContext.SignOutAsync("Cookies");

            return RedirectToAction("Login");
        }

        public IActionResult ErrorView()
        {
            return View();
        }

        #region Private Methods
        private ClaimsPrincipal VefifyToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            ClaimsPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            return principal;
        }

        private TokenValidationParameters GetValidationParameters()
        {
            var tokenSecret = _tokenSecret;
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret)) // The same key as the one that generate the token
            };
        }

        #endregion
    }
}
