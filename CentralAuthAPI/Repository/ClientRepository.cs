using CentralAuthAPI.Models.Data.MasterAccountDBContext;
using CentralAuthAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CentralAuthAPI.Repository
{
	public class ClientRepository : BaseRepository<Client>, IClientRepository
	{
		public ClientRepository(ClientDBContext DbContext) : base(DbContext)
		{
		}
	}
}
