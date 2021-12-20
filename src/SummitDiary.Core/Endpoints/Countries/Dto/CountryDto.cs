using SummitDiary.Core.Common.Mapping;
using SummitDiary.Core.Common.Models;

namespace SummitDiary.Core.Endpoints.Countries.Dto
{
    public class CountryDto : IMapFrom<Country>
    {
        public int Id { get; set; }
        public string Name { get; set; }  = string.Empty;
    }
}