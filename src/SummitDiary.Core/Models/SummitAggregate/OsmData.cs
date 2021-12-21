﻿using SummitDiary.SharedKernel;

namespace SummitDiary.Core.Models.SummitAggregate
{
    public class OsmData : BaseEntity<int>
    {
        public long OpenStreetMapId { get; set; }
        public int SummitId { get; set; }
        public Summit? Summit { get; set; }

        public string TagName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}