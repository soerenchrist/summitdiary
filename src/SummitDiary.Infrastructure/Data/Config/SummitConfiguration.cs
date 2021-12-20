using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SummitDiary.Core.Common.Models;

namespace SummitDiary.Infrastructure.Data.Config
{
    public class SummitConfiguration : IEntityTypeConfiguration<Summit>
    {
        public void Configure(EntityTypeBuilder<Summit> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(x => x.Latitude)
                .IsRequired();
            builder.Property(x => x.Longitude)
                .IsRequired();
            
            builder.HasOne(x => x.Country!)
                .WithMany(x => x.Summits)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Region!)
                .WithMany(x => x.Summits)
                .HasForeignKey(x => x.RegionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.OsmData!)
                .WithOne(x => x.Summit!)
                .HasForeignKey(x => x.SummitId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}