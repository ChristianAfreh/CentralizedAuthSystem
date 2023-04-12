using CentralAuthAPI.DTOs;
using CentralAuthAPI.Exceptions;
using CentralAuthAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace CentralAuthAPI.Controllers
{

	[Route("api/[controller]")]
	[Produces("application/json")]	
	[ApiController]
	public class AccountController : Controller
	{
		private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

		[Route("register")]
		[HttpPost]
        public async Task<IActionResult> Register([FromHeader] ClientDTO clientDTO,[FromBody] RegisterAccountDTO registerAccountDTO)
		{
			try
			{
				_accountService.VerifyClient(clientDTO);
				await _accountService.RegisterAccount(registerAccountDTO);
				return Ok();
			}
			catch (CustomException ex)
			{
				
				return BadRequest(ex.Message);
				
			}
			
		}

		[Route("login")]
		[HttpPost]
		public async Task<IActionResult> Login([FromHeader] ClientDTO clientDTO, [FromBody] LoginDTO loginDTO)
		{
			try
			{
				_accountService.VerifyClient(clientDTO);
				var loginResponse = await _accountService.Login(loginDTO);
				return Ok(loginResponse);
			}
			catch (CustomException ex)
			{

				return BadRequest(ex.Message);
			}
		}

		[Route("update")]
		[HttpPost]
		public async Task<IActionResult> Update([FromHeader] ClientDTO clientDTO, [FromBody] UpdateAccountDTO updateAccountDTO)
		{
			try
			{
				_accountService.VerifyClient(clientDTO);
				await _accountService.UpdateAccount(User.Identity.Name, updateAccountDTO);
				return Ok();
			}
			catch (CustomException ex)
			{

				return BadRequest(ex.Message);
			}
			
		}
	}
}
