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
    }
}