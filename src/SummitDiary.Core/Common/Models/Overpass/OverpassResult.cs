using System.Collections.Generic;

namespace SummitDiary.Core.Common.Models.Overpass
{
    public class OverpassResult
    {
        public IEnumerable<OverpassElement> Elements { get; set; }
    }
}