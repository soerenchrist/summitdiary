using SummitDiary.SharedKernel;

namespace SummitDiary.Core.Common.Models
{
    public class Attachment : BaseEntity<int>
    {
        public int ActivityId { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public Activity? Activity { get; set; }
        public string FileName { get; set; } = string.Empty;
        public FileType FileType { get; set; }
    }

    public enum FileType
    {
        Gpx, Image
    }
}