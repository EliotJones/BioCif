namespace BioCif.Core
{
    using System;

    /// <inheritdoc />
    /// <summary>
    /// A simple <see langword="string" /> value in a CIF file.
    /// </summary>
    public class DataValueSimple : IDataValue
    {
        /// <inheritdoc />
        public DataValueType DataType { get; } = DataValueType.Simple;

        /// <summary>
        /// The raw <see langword="string"/> value from the file. 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Whether the <see cref="Value"/> is '?' which generally indicates a missing value.
        /// </summary>
        public bool IsNullSymbol { get; }

        /// <summary>
        /// Create a new <see cref="DataValueSimple"/>.
        /// </summary>
        public DataValueSimple(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
            IsNullSymbol = value == "?";
        }

        /// <inheritdoc />
        public string GetStringValue() => Value;

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is DataValueSimple value && Value == value.Value;

        /// <inheritdoc />
        public override int GetHashCode() => Value.GetHashCode();

        /// <inheritdoc />
        public override string ToString() => Value;

        /// <summary>
        /// Implictly convert to a <see langword="string" />
        /// </summary>
        public static implicit operator string(DataValueSimple value) => value.Value;
    }
}