namespace BioCif.Core
{
    using System;
    using System.Collections.Generic;

    public class DataItem : IDataBlockMember
    {
        public DataName Name { get; }

        public IDataValue Value { get; }

        public DataItem(DataName name, IDataValue value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <inheritdoc />
        public override string ToString() => $"{Name}: {Value}";
    }

    public interface IDataValue
    {

    }

    public class DataDictionary : IDataValue
    {

    }

    public class DataList : IDataValue
    {
        public IReadOnlyList<IDataValue> Values { get; }

        public DataList(IReadOnlyList<IDataValue> values)
        {
            Values = values;
        }
    }
}