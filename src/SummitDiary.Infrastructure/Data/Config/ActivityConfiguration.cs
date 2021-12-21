using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SummitDiary.Core.Models.ActivityAggregate;

namespace SummitDiary.Infrastructure.Data.Config
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Title)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(x => x.HikeDate)
                .IsRequired();
            builder.Property(x => x.ElevationDown)
                .IsRequired();
            builder.Property(x => x.ElevationUp)
                .IsRequired();
            builder.Property(x => x.Distance)
                .IsRequired();
            builder.HasMany(x => x.Summits)
                .WithMany(x => x.DiaryEntries);
        }
    }
}