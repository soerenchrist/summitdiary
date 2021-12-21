using SummitDiary.Core.Models.SummitAggregate;
using SummitDiary.SharedKernel;

namespace SummitDiary.Core.Models.ActivityAggregate
{
    public class Activity : BaseEntity<int>
    {
        public IEnumerable<Summit>? Summits { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime HikeDate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public int Rating { get; set; }
        public double ElevationUp { get; set; }
        public double ElevationDown { get; set; }
        public double Distance { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Duration { get; set; }

        public IEnumerable<Attachment>? Attachments { get; set; }
    }
}