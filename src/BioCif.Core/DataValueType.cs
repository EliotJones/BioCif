namespace BioCif.Core
{
    /// <summary>
    /// The type of a value in a <see cref="DataItem"/> or <see cref="DataTable"/>.
    /// </summary>
    public enum DataValueType
    {
        /// <summary>
        /// A single <see langword="string"/> value.
        /// </summary>
        Simple = 0,
        /// <summary>
        /// A list of <see cref="IDataValue"/>s. Only valid in a <see cref="CifFileVersion.Version2"/> file.
        /// </summary>
        List = 1,
        /// <summary>
        /// A dictionary of keys and values (also called a table in the specification, not to be confused with <see cref="DataTable"/> which is a loop).
        /// Only valid in a <see cref="CifFileVersion.Version2"/> file.
        /// </summary>
        Dictionary = 2
    }
}