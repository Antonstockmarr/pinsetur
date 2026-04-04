namespace Pinsetur.Webapi.Models
{
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
