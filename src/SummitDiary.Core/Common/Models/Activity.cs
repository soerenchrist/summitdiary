using System;
using System.Collections.Generic;
using SummitDiary.SharedKernel;

namespace SummitDiary.Core.Common.Models
{
    public class Activity : BaseEntity<int>
    {
        public IEnumerable<Summit> Summits { get; set; }
        public string Title { get; set; }
        public DateTime HikeDate { get; set; }
        public string Notes { get; set; } = "";
        public int Rating { get; set; }
        public double ElevationUp { get; set; }
        public double ElevationDown { get; set; }
        public double Distance { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Duration { get; set; }

        public IEnumerable<Attachment> Attachments { get; set; }
    }
}