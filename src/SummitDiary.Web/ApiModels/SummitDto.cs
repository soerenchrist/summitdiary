using SummitDiary.Core.Common.Mapping;
using SummitDiary.Core.Models.SummitAggregate;

namespace SummitDiary.Web.ApiModels;
public class SummitDto : IMapFrom<Summit>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Height { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public CountryDto? Country { get; set; }
    public RegionDto? Region { get; set; }
    public bool Climbed { get; set; }
}
