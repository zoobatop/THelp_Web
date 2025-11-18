namespace THelp_Web.Models.API
{
    public class ApiStatus
    {
        public string BaseUrl { get; set; }
        public int Timeout { get; set; }
        public DateTime LastChecked { get; set; }
        public bool IsOnline { get; set; }
    }
}
