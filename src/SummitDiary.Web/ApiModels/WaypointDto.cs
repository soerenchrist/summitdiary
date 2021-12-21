using SummitDiary.Core.Common.Mapping;
using SummitDiary.Core.Models.Common;

namespace SummitDiary.Web.ApiModels
{
    public struct WaypointDto : IMapFrom<Waypoint>
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }
        public DateTime DateTime { get; set; }
    }
}