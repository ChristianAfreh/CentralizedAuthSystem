using System.ComponentModel.DataAnnotations;

namespace HRSystem.ViewModels
{
	public class LoginViewModel
	{
        [Required(ErrorMessage = "UserName is required")]
        [MaxLength(100), MinLength(1)]
        public string UserName { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}

    public class LoginResponseViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string SurName { get; set; }
        public string OtherNames { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }

}
