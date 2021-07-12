using System;

namespace SummitDiary.Core.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base("Element with given ID not found")
        {
        }

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

        public string ElementName { get; set; }
        public object KeyValue { get; set; }
    }
}