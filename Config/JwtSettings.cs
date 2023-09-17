namespace AdminPanel.Config;

public class JwtSettings
{
    public string SecretKey { get; set; }
    public string ExpirationInMinutes { get; set; }
}