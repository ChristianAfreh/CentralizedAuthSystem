using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.Models
{
    public class HRSystemDBContext : IdentityDbContext<IdentityUser>
    {
        public HRSystemDBContext(DbContextOptions<HRSystemDBContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
