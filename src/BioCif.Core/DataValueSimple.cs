namespace BioCif.Core
{
    using System;
    using System.Globalization;

    /// <inheritdoc />
    /// <summary>
    /// A simple <see langword="string" /> value in a CIF file.
    /// </summary>
    public class DataValueSimple : IDataValue
    {
        /// <inheritdoc />
        public DataValueType DataType { get; } = DataValueType.Simple;

        /// <summary>
        /// The raw <see langword="string"/> value from the file. Can be <see langword="null"/>.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Create a new <see cref="DataValueSimple"/>.
        /// </summary>
        public DataValueSimple(string value)
        {
            Value = value;
        }

        /// <inheritdoc />
        public string GetStringValue() => Value;
        
        /// <inheritdoc />
        public int? GetIntValue()
        {
            if (int.TryParse(Value, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            return null;
        }

        /// <inheritdoc />
        public double? GetDoubleValue()
        {
            if (Value == null)
            {
                return null;
            }

            var str = Value;

            var bracket = Value.IndexOf('(');
            if (bracket > 0)
            {
                str = str.Substring(0, bracket);
            }

            if (!double.TryParse(str, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
            {
                return null;
            }

            return result;
        }

        /// <inheritdoc />
        public bool? GetBoolValue()
        {
            if (Value == null)
            {
                return null;
            }

            if (string.Equals(Value, "n", StringComparison.OrdinalIgnoreCase) || string.Equals(Value, "no", StringComparison.OrdinalIgnoreCase)
                || string.Equals(Value, "false", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (string.Equals(Value, "y", StringComparison.OrdinalIgnoreCase) || string.Equals(Value, "yes", StringComparison.OrdinalIgnoreCase)
                || string.Equals(Value, "true", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return null;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is DataValueSimple value && Value == value.Value;

        /// <inheritdoc />
        public override int GetHashCode() => Value != null ? Value.GetHashCode() : 0;

        /// <inheritdoc />
        public override string ToString() => Value;

        /// <summary>
        /// Implictly convert to a <see langword="string" />
        /// </summary>
        public static implicit operator string(DataValueSimple value) => value.Value;
    }
}