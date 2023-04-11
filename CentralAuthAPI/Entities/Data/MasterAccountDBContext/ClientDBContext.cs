using Microsoft.EntityFrameworkCore;

namespace CentralAuthAPI.Models.Data.MasterAccountDBContext
{
	public partial class ClientDBContext : DbContext
	{
		public ClientDBContext()
		{
		}

		public ClientDBContext(DbContextOptions<ClientDBContext> options)
			: base(options)
		{
		}

		public virtual DbSet<Client> Client { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{ }
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

	}
}
