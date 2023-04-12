using System.ComponentModel.DataAnnotations;

namespace HRSystem.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "UserName is required")]
        [MaxLength(100), MinLength(1)]
        public string UserName { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "SurName is required")]
        [MaxLength(100), MinLength(1)]
        public string SurName { get; set; }
        [Required(ErrorMessage = "OtherNames is required")]
		[MaxLength(100),MinLength(1)]
        public string OtherNames { get; set; }
        [Required(ErrorMessage = "Email is required")]
		[DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
	}
}
