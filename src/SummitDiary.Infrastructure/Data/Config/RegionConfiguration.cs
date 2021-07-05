using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SummitDiary.Core.Common.Models;

namespace SummitDiary.Infrastructure.Data.Config
{
    public class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}