namespace MercerStore.Web.Application.Dtos.Account
{
    public class JwtTokenDto
    {
        public string UserId { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
