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
}
