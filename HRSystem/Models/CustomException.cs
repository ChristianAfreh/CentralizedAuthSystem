namespace HRSystem.Models
{
    public class CustomException : HttpRequestException
    {
        public CustomException(string message) : base(message)
        {

        }

    }
}
