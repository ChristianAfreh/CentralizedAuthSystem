using System.ComponentModel.DataAnnotations;

namespace CentralAuthAPI.DTOs
{
	public class RegisterAccountDTO
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string SurName { get; set; }
		public string OtherNames { get; set; }
		public string Email { get; set; }
		public string? PhoneNumber { get; set; }
	}

	public class UpdateAccountDTO
	{
		public string SurName { get; set; }
		public string OtherNames { get; set; }
		public string Email { get; set; }
		public string? PhoneNumber { get; set; }
	}
}
