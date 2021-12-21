using SummitDiary.SharedKernel;

namespace SummitDiary.Core.Models.SummitAggregate
{
    public class Region : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public IEnumerable<Summit>? Summits { get; set; }
    }
}