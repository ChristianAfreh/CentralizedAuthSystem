using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CentralAuthAPI.Models.Data.MasterAccountDBContext
{
	public class AppUser : IdentityUser
	{
		[MaxLength(200)]
		public string Surname { get; set; }
		[MaxLength(200)]
		public string OtherNames { get; set; }
	}
}
