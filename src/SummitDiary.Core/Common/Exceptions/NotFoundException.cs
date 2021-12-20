namespace SummitDiary.Core.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string elementName, object value)
            : this($"{elementName} with key {value} not found")
        {
            ElementName = elementName;
            KeyValue = value;
        }

        public string? ElementName { get; }
        public object? KeyValue { get; }
    }
}