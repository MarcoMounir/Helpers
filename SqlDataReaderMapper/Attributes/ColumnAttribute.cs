using System;

namespace SqlDataReaderMapper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute: Attribute
    {
        public string Name { get; private set; }
        public ColumnAttribute(string name)
        {
            Name = name;
        }
    }
}
