using SummitDiary.Core.Common.Mapping;
using SummitDiary.Core.Common.Models;

namespace SummitDiary.Core.Endpoints.Regions.Dto
{
    public class RegionDto : IMapFrom<Region>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}