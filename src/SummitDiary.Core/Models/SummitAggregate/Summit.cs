﻿using SummitDiary.Core.Models.ActivityAggregate;
using SummitDiary.SharedKernel;

namespace SummitDiary.Core.Models.SummitAggregate
{
    public class Summit : BaseEntity<int>
    {
        public long OpenStreetmapId { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Height { get; set; }
        public int CountryId { get; set; }
        public int RegionId { get; set; }
        public IEnumerable<Activity>? DiaryEntries { get; set; }

        public Region? Region { get; set; }
        public Country? Country { get; set; }

        public IEnumerable<OsmData>? OsmData { get; set; }
        public WishlistItem? WishlistItem { get; set; }
    }
}