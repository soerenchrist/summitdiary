using System;
using SummitDiary.Core.Common.Models;

namespace SummitDiary.Core.Endpoints.Gpx.Dto
{
    public class AnalysisResultDto
    {
        public string ProposedTitle { get; set; }
        public Summit ProposedSummit { get; set; }
        public int ElevationUp { get; set; }
        public int ElevationDown { get; set; }
        public double Distance { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}