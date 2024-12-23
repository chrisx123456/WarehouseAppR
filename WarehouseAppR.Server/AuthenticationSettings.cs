namespace WarehouseAppR.Server
{
    public class AuthenticationSettings
    {
        public string JwtKey { get; set; }
        public int JwtValidDays { get; set; }
        public string JwtIssuer { get; set; } 
    }
}
