using SummitDiary.Core.Common.Mapping;
using SummitDiary.Core.Models.ActivityAggregate;

namespace SummitDiary.Web.ApiModels
{
    public class ActivityDto : IMapFrom<Activity>
    {
        public int Id { get; set; }
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
        public List<SummitDto> Summits { get; set; } = new();
    }
}