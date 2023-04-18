using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Models
{

        public class AccountingSystemDBContext : IdentityDbContext<IdentityUser>
        {
            public AccountingSystemDBContext(DbContextOptions<AccountingSystemDBContext> options)
            : base(options)
            {
            }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);
            }
        }

}
