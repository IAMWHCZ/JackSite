using JackSite.Authentication.Entities.Emails;

namespace JackSite.Authentication.Infrastructure.Data;

public partial class AuthenticationDbContext
{
    public DbSet<EmailAttachment>? EmailAttachments { get; set; }

    public DbSet<EmailBasic>? EmailBasics { get; set; }

    public DbSet<EmailContent>? EmailContents { get; set; }

    public DbSet<EmailRecipient>? EmailRecipients { get; set; }

    private static void ConfigureEmails(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmailAttachment>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.ObjectKey)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.Description)
                .HasMaxLength(512);

            entity.Property(e => e.EmailRecordId)
                .IsRequired();

            entity.HasOne(e => e.EmailBasic)
                .WithMany()
                .HasForeignKey(e => e.EmailRecordId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<EmailBasic>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.Type)
                .IsRequired();

            entity.HasMany(e => e.EmailRecipients)
                .WithOne()
                .HasForeignKey(er => er.EmailRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.EmailContent)
                .WithMany()
                .HasForeignKey(e => e.EmailContent)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.EmailAttachment)
                .WithMany()
                .HasForeignKey(e => e.EmailAttachment)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<EmailContent>(entity =>
        {
            entity.HasKey(e => e.EmailId);

            entity.Property(e => e.Content)
                .IsRequired();

            entity.HasOne(e => e.EmailBasic)
                .WithOne(e => e.EmailContent)
                .HasForeignKey<EmailBasic>(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<EmailBasic>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.Type)
                .IsRequired();

            entity.HasMany(e => e.EmailRecipients)
                .WithOne()
                .HasForeignKey(r => r.EmailRecordId);

            entity.HasOne(e => e.EmailContent)
                .WithOne()
                .HasForeignKey<EmailContent>(c => c.EmailId);

            entity.HasOne(e => e.EmailAttachment)
                .WithOne()
                .HasForeignKey<EmailAttachment>(a => a.EmailRecordId);
        });
    }
}