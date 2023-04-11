using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CentralAuthAPI.Models.Data.MasterAccountDBContext
{
	public class AppUserDBContext : IdentityDbContext<AppUser>
	{
		public AppUserDBContext(DbContextOptions<AppUserDBContext> options)
		: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
	}
}
