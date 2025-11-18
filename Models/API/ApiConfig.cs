namespace THelp_Web.Models.API
{
    public class ApiConfig
    {
        public string BaseUrl { get; set; } = "http://localhost:8080";
        public int TimeoutSeconds { get; set; } = 30;
        public bool UseHttps { get; set; } = false;
    }
}