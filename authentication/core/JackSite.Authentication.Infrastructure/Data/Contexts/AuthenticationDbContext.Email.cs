using JackSite.Authentication.Entities.Emails;

namespace JackSite.Authentication.Infrastructure.Data.Contexts;

public partial class AuthenticationDbContext
{
    public DbSet<EmailAttachment>? EmailAttachments { get; set; }

    public DbSet<EmailBasic>? EmailBasics { get; set; }

    public DbSet<EmailContent>? EmailContents { get; set; }

    public DbSet<EmailRecipient>? EmailRecipients { get; set; }
    
    public DbSet<EmailTemplate>? EmailTemplates { get; set; }

    private static void ConfigureEmails(ModelBuilder modelBuilder)
    {
        // 配置 EmailBasic 实体
        modelBuilder.Entity<EmailBasic>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.SenderEmail)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.SenderName)
                .HasMaxLength(128);

            entity.Property(e => e.MessageId)
                .HasMaxLength(256);

            entity.Property(e => e.FailureReason)
                .HasMaxLength(1024);

            // 一对多关系：EmailBasic 到 EmailRecipient
            entity.HasMany(e => e.EmailRecipients)
                .WithOne(r => r.EmailBasic)
                .HasForeignKey(r => r.EmailRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            // 一对多关系：EmailBasic 到 EmailAttachment
            entity.HasMany(e => e.EmailAttachments)
                .WithOne(a => a.EmailBasic)
                .HasForeignKey(a => a.EmailRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            // 一对一关系：EmailBasic 到 EmailContent
            entity.HasOne(e => e.EmailContent)
                .WithOne(c => c.EmailBasic)
                .HasForeignKey<EmailContent>(c => c.EmailId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // 配置 EmailContent 实体
        modelBuilder.Entity<EmailContent>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Subject)
                .HasMaxLength(512);

            entity.Property(e => e.Recipient)
                .HasMaxLength(1024);

            entity.Property(e => e.CC)
                .HasMaxLength(1024);

            entity.Property(e => e.BCC)
                .HasMaxLength(1024);

            entity.Property(e => e.TemplateId)
                .HasMaxLength(128);

            entity.Property(e => e.PreviewText)
                .HasMaxLength(512);
        });
        
        // 配置 EmailRecipient 实体
        modelBuilder.Entity<EmailRecipient>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.RecipientEmail)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.RecipientName)
                .HasMaxLength(128);

            entity.Property(e => e.TrackingId)
                .HasMaxLength(128);

            entity.Property(e => e.FailureReason)
                .HasMaxLength(1024);
        });
        
        // 配置 EmailAttachment 实体
        modelBuilder.Entity<EmailAttachment>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.ObjectKey)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.FileName)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.Description)
                .HasMaxLength(512);

            entity.Property(e => e.ContentType)
                .HasMaxLength(128);

            entity.Property(e => e.ContentId)
                .HasMaxLength(128);

            entity.Property(e => e.FileExtension)
                .HasMaxLength(32);

            entity.Property(e => e.StorageType)
                .HasMaxLength(32);
        });
        
        // 配置 EmailTemplateConst 实体
        modelBuilder.Entity<EmailTemplate>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(128);
                
            entity.Property(e => e.Description)
                .HasMaxLength(512);
                
            entity.Property(e => e.Subject)
                .IsRequired()
                .HasMaxLength(256);
                
            entity.Property(e => e.Body)
                .IsRequired();
                
            entity.Property(e => e.Parameters)
                .HasMaxLength(1024);
                
            entity.Property(e => e.Category)
                .HasMaxLength(64);
                
            // 添加唯一索引
            entity.HasIndex(e => new { e.Name, e.Version })
                .IsUnique();
        });
    }
}
