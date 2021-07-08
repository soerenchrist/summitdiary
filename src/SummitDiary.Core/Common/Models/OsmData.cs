using SummitDiary.SharedKernel;

namespace SummitDiary.Core.Common.Models
{
    public class OsmData : BaseEntity<int>
    {
        public long OpenStreetMapId { get; set; }
        public int SummitId { get; set; }
        public Summit Summit { get; set; }

        public string TagName { get; set; }
        public string Value { get; set; }
    }
}