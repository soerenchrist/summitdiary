using System.IO;

namespace SummitDiary.Core.Common.Models.Common
{
    public class FileResult
    {
        public Stream Stream { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }

        public FileResult(Stream stream, string contentType, string fileName)
        {
            Stream = stream;
            ContentType = contentType;
            FileName = fileName;
        }
    }

    public class ByteFileResult
    {
        
        public byte[] Bytes { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }

        public ByteFileResult(byte[] bytes, string contentType, string fileName)
        {
            Bytes = bytes;
            ContentType = contentType;
            FileName = fileName;
        }
    }
}