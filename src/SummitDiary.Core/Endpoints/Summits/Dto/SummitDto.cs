using SummitDiary.Core.Common.Mapping;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Countries.Dto;
using SummitDiary.Core.Endpoints.Regions.Dto;

namespace SummitDiary.Core.Endpoints.Summits.Dto
{
    public class SummitDto : IMapFrom<Summit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public CountryDto Country { get; set; }
        public RegionDto Region { get; set; }
        public bool Climbed { get; set; }
    }
}