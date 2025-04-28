namespace MercerStore.Web.Infrastructure.Helpers;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiresDays { get; set; }
}
