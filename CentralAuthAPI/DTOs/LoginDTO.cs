namespace CentralAuthAPI.DTOs
{
	public class LoginDTO
	{
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseDTO
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
