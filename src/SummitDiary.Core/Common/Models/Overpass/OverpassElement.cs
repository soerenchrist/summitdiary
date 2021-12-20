﻿namespace SummitDiary.Core.Common.Models.Overpass
{
    public class OverpassElement
    {
        public long Id { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public OverpassTags? Tags { get; set; }
    }
}