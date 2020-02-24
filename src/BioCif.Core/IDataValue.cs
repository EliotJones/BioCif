namespace BioCif.Core
{
    /// <summary>
    /// A value associated with a <see cref="DataName"/> in a <see cref="DataItem"/>
    /// or <see cref="DataTable"/>.
    /// </summary>
    public interface IDataValue
    {
        /// <summary>
        /// The type of this value.
        /// </summary>
        DataValueType DataType { get; }

        /// <summary>
        /// Gets the <see langword="string"/> value of this value if it's a <see cref="DataValueSimple"/>, <see langword="null"/> otherwise.
        /// </summary>
        string GetStringValue();

        /// <summary>
        /// Gets the <see langword="int?"/> value of this value if it's a <see cref="DataValueSimple"/> which can be mapped to an int.
        /// </summary>
        int? GetIntValue();

        /// <summary>
        /// Gets the <see langword="double?"/> value of this value if it's a <see cref="DataValueSimple"/> which can be mapped to an double.
        /// </summary>
        double? GetDoubleValue();

        /// <summary>
        /// Gets the <see langword="bool?"/> value of this value if it's a <see cref="DataValueSimple"/> which can be mapped to an bool.
        /// </summary>
        bool? GetBoolValue();
    }
}