using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SummitDiary.Core.Models.ActivityAggregate;

namespace SummitDiary.Infrastructure.Data.Config
{
    public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.FileType)
                .HasConversion(
                    v => v.ToString(),
                    v => (FileType) Enum.Parse(typeof(FileType), v));
            
            builder.HasOne(x => x.Activity!)
                .WithMany(x => x.Attachments)
                .HasForeignKey(x => x.ActivityId);
        }
    }
}