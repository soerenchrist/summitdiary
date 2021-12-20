using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SummitDiary.Core.Common.Models;

namespace SummitDiary.Infrastructure.Data.Config
{
    public class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItem>
    {
        public void Configure(EntityTypeBuilder<WishlistItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(x => x.Summit!)
                .WithOne(x => x.WishlistItem!)
                .HasForeignKey<WishlistItem>(x => x.SummitId);
        }
    }
}