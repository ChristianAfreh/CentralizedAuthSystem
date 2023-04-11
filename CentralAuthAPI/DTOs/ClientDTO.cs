using Microsoft.AspNetCore.Mvc;

namespace CentralAuthAPI.DTOs
{
	public class ClientDTO
	{
        [FromHeader]
        public string ClientId { get; set; }

        [FromHeader]
        public string ClientSecret { get; set; }
    }
}
