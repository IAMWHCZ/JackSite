namespace JackSite.Identity.Server.Entities
{
    public class Email
    {
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        [Comment("用户邮箱")]
        public string UserEmail { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Comment("用户ID")]
        public string UserId { get; set; } = string.Empty;
    }
}
