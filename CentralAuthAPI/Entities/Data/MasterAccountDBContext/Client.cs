namespace CentralAuthAPI.Models.Data.MasterAccountDBContext
{
	public class Client
	{
		public string ClientId { get; set; }
		public Guid ClientSecret { get; set; }
		public string ClientDescription { get; set; }
		public DateTime Created { get; set; }
		public string CreatedBy { get; set; }
	}
}
