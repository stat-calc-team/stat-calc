namespace StatCalc.Infrastructure.Configurations;

public class GoogleSettings
{
    public const string SectionName = "Authentication:Google";
    
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }
}
