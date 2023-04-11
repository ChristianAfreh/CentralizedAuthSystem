using CentralAuthAPI.DTOs;

namespace CentralAuthAPI.Service
{
	public interface IAccountService
	{
		Task RegisterAccount(RegisterAccountDTO registerAccountDTO);
		Task<LoginResponseDTO> Login(LoginDTO loginDTO);
		Task UpdateAccount(string username, UpdateAccountDTO updateAccountDTO);
		void VerifyClient(ClientDTO clientDTO);
	}
}
