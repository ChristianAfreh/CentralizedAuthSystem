using CentralAuthAPI.DTOs;
using CentralAuthAPI.Exceptions;
using CentralAuthAPI.Models.Data.MasterAccountDBContext;
using CentralAuthAPI.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace CentralAuthAPI.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IClientRepository _clientRepository;
        private readonly IConfiguration _configuration;


        public AccountService(UserManager<AppUser> userManager, IClientRepository clientRepository, IConfiguration configuration)
        {
            _userManager = userManager;
            _clientRepository = clientRepository;
            _configuration = configuration;
        }

        public async Task RegisterAccount(RegisterAccountDTO registerAccountDTO)
        {
           

            try
            {
                //check if username is existing
                var usernameExists = await _userManager.FindByNameAsync(registerAccountDTO.UserName);

                if (usernameExists != null)
                {
                    throw new CustomException("Username already exists.");
                }

                if (string.IsNullOrEmpty(registerAccountDTO.Email))
                {
                    throw new CustomException("Email field cannot be empty. Kindly provide an email.");
                }
                else if (!IsValidEmail(registerAccountDTO.Email))
                {
                    throw new CustomException("Email is invalid. Kindly provide a valid email.");
                }
                //Instantiate AppUser class and get account properties for registration
                AppUser appUser = new AppUser
                {
                    Surname = registerAccountDTO.UserName,
                    OtherNames = registerAccountDTO.OtherNames,
                    UserName = registerAccountDTO.UserName,
                    Email = registerAccountDTO.Email,
                    PhoneNumber = registerAccountDTO.PhoneNumber,
                };

                var newUser = await _userManager.CreateAsync(appUser, registerAccountDTO.Password);
                if (!newUser.Succeeded)
                {
                    string errorMessage = "";
                    var registrationErrors = newUser.Errors.ToList();
                    foreach (var error in registrationErrors)
                    {
                        errorMessage += error.Description;
                    }
                    throw new CustomException(errorMessage);
                }
            }
            catch(CustomException ex)
            {

                throw new CustomException(ex.Message);
            }


        }

        public async Task<LoginResponseDTO> Login(LoginDTO loginDTO)
        {
            var loginUser = await _userManager.FindByNameAsync(loginDTO.UserName);
            if (loginUser == null)
                throw new CustomException("Invalid username. Kindly provide a valid username.");


            var loginPassword = await _userManager.CheckPasswordAsync(loginUser, loginDTO.Password);

            if (!loginPassword)
                throw new CustomException("Invalid password. Kindly provide a valid password.");

            var acessToken =  GenerateAccessToken(loginUser);
            var refreshToken = await _userManager.GenerateUserTokenAsync(loginUser, "Default", "AsRefreshToken");

            var loginResponse = new LoginResponseDTO
            {
                UserId = loginUser.Id,
                UserName = loginUser.UserName,
                SurName = loginUser.Surname,
                OtherNames = loginUser.OtherNames,
                Email = loginUser.Email,
                PhoneNumber = loginUser.PhoneNumber,
                TokenType = "JwtBearer",
                RefreshToken = refreshToken,
                AccessToken = acessToken
            };

            return loginResponse;

        }

        public async Task UpdateAccount(string username, UpdateAccountDTO updateAccountDTO)
        {

            //Get user for update
            var userForUpdate = await _userManager.FindByNameAsync(username);

            if (string.IsNullOrEmpty(updateAccountDTO.Email))
            {
                throw new CustomException("Email field cannot be empty. Kindly provide an email.");
            }
            else if (!IsValidEmail(updateAccountDTO.Email))
            {
                throw new CustomException("Email is invalid. Kindly provide a valid email.");
            }

            try
            {
                userForUpdate.Surname = updateAccountDTO.SurName;
                userForUpdate.OtherNames = updateAccountDTO.OtherNames;
                userForUpdate.Email = updateAccountDTO.Email;
                userForUpdate.PhoneNumber = updateAccountDTO.PhoneNumber;

                var updatedUserAccount = await _userManager.UpdateAsync(userForUpdate);

                if (!updatedUserAccount.Succeeded)
                {
                    string errorMessage = "";
                    var registrationErrors = updatedUserAccount.Errors.ToList();
                    foreach (var error in registrationErrors)
                    {
                        errorMessage += error.Description;
                    }
                    throw new CustomException(errorMessage);
                }

            }
            catch
            {

                throw new CustomException("Account Update Failed. Kindly contact System Administrator for assistance.");
            }



        }

        public void VerifyClient(ClientDTO clientDTO)
        {
            if (clientDTO.ClientId == null || clientDTO.ClientSecret == null)
                throw new CustomException("Client model is null. Provide values for client model.");

            var isClientValid = IsClientValid(clientDTO.ClientId, clientDTO.ClientSecret);

            if (!isClientValid)
            {
                throw new CustomException($"Invalid client with Client ID:{clientDTO.ClientId} and Client Secret:{clientDTO.ClientSecret}");
            }
        }

        #region Private Methods
        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
                return false;

            try
            {
                var addr = new MailAddress(email);
                return addr.Address.Equals(trimmedEmail);
            }
            catch
            {

                return false;
            }
        }

        private bool IsClientValid(string clientId, string clientSecret)
        {
            string clientIdFormatted = clientId.ToLower().Trim();
            string clientSecretFormatted = clientSecret.ToLower().Trim();

            return _clientRepository.Query().Where(c => c.ClientId.ToLower().Trim() == clientIdFormatted && c.ClientSecret.ToString().ToLower().Trim() == clientSecretFormatted).Any();
        }

        private string GenerateAccessToken(AppUser appUser)
        {
            //Set claims for token generation
            List<Claim> claims = new();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, appUser.Id));
            claims.Add(new Claim(ClaimTypes.Name, appUser.UserName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, appUser.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var token = GetToken(claims);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToken;
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:JWTSecret"]));
            var signCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);
            var jwtHeader = new JwtHeader(signCredentials);
            var jwtPayLoad = new JwtPayload(authClaims);

            var token = new JwtSecurityToken(jwtHeader, jwtPayLoad);
            return token;
        }

        #endregion
    }
}
