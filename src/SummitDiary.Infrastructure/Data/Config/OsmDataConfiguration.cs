using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SummitDiary.Core.Common.Models;

namespace SummitDiary.Infrastructure.Data.Config
{
    public class OsmDataConfiguration : IEntityTypeConfiguration<OsmData>
    {
        public void Configure(EntityTypeBuilder<OsmData> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Summit)
                .WithMany(x => x.OsmData)
                .HasForeignKey(x => x.SummitId);
        }
    }
}