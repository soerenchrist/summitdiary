using SummitDiary.SharedKernel;

namespace SummitDiary.Core.Common.Models
{
    public class OsmData : BaseEntity<long>
    {
        public long SummitId { get; set; }
        public Summit Summit { get; set; }

        public string TagName { get; set; }
        public string Value { get; set; }
    }
}