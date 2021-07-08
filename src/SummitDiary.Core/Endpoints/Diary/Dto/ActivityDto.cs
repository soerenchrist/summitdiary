using System;
using System.Collections.Generic;
using SummitDiary.Core.Common.Mapping;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Summits.Dto;

namespace SummitDiary.Core.Endpoints.Diary.Dto
{
    public class ActivityDto : IMapFrom<Activity>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime HikeDate { get; set; }
        public string Notes { get; set; }
        public int Rating { get; set; }
        public double ElevationUp { get; set; }
        public double ElevationDown { get; set; }
        public double Distance { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Duration { get; set; }
        public List<SummitDto> Summits { get; set; }
    }
}