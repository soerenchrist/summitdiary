using SummitDiary.Core.Common.Mapping;
using SummitDiary.Core.Models.SummitAggregate;

namespace SummitDiary.Web.ApiModels
{
    public class CountryDto : IMapFrom<Country>
    {
        public int Id { get; set; }
        public string Name { get; set; }  = string.Empty;
    }
}