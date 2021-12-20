using SummitDiary.SharedKernel;

namespace SummitDiary.Core.Common.Models
{
    public class WishlistItem : BaseEntity<int>
    {
        public int SummitId { get; set; }
        public Summit? Summit { get; set; }
        public bool Finished { get; set; }
    }
}