using SummitDiary.SharedKernel;

namespace SummitDiary.Core.Common.Models
{
    public class Region : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public IEnumerable<Summit>? Summits { get; set; }
    }
}