namespace THelp_Web.Models.API
{
    public class ConnectionResult
    {
        public string BaseUrl { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public int ResponseTimeMs { get; set; }
        public DateTime TestTime { get; set; }
    }
}
