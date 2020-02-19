namespace BioCif.Core
{
    /// <summary>
    /// The type of a value in a <see cref="DataItem"/> or <see cref="Table"/>.
    /// </summary>
    public enum DataValueType
    {
        /// <summary>
        /// A single <see langword="string"/> value.
        /// </summary>
        Simple = 0,
        /// <summary>
        /// A list of <see cref="IDataValue"/>s. Only valid in a <see cref="Version.Version2"/> file.
        /// </summary>
        List = 1,
        /// <summary>
        /// A dictionary of keys and values (also called a table in the specification, not to be confused with <see cref="Table"/> which is a loop).
        /// Only valid in a <see cref="Version.Version2"/> file.
        /// </summary>
        Dictionary = 2
    }
}