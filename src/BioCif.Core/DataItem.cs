namespace BioCif.Core
{
    using System;

    /// <inheritdoc />
    /// <summary>
    /// A field name with its associated value.
    /// </summary>
    public class DataItem : IDataBlockMember
    {
        /// <summary>
        /// The name of the field.
        /// </summary>
        public DataName Name { get; }

        /// <summary>
        /// The value of the field.
        /// </summary>
        public IDataValue Value { get; }

        /// <summary>
        /// Create a new <see cref="DataItem"/>.
        /// </summary>
        public DataItem(DataName name, IDataValue value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <inheritdoc />
        public override string ToString() => $"{Name}: {Value}";
    }
}