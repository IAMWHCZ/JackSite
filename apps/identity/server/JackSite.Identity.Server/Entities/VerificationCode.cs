namespace JackSite.Identity.Server.Entities
{
    public class VerificationCode
    {
        public string Code { get; set; } = string.Empty;

        public DateTime ExpiredDate { get; set; }
    }
}
