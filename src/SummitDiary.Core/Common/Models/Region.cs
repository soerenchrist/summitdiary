﻿using System.Collections.Generic;
using SummitDiary.SharedKernel;

namespace SummitDiary.Core.Common.Models
{
    public class Region : BaseEntity<int>
    {
        public string Name { get; set; }
        public IEnumerable<Summit> Summits { get; set; }
    }
}