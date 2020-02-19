namespace BioCif.Core
{
    using System;

    /// <summary>
    /// A name for a value in a CIF file.
    /// </summary>
    public class DataName
    {
        /// <summary>
        /// Identifier of the content of an associated <see cref="DataValueSimple"/>. 
        /// </summary>
        public string Tag { get; }

        /// <summary>
        /// Create a new <see cref="DataName"/>.
        /// </summary>
        public DataName(string tag)
        {
            Tag = tag ?? throw new ArgumentNullException(nameof(tag));
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is DataName name && Tag == name.Tag;

        /// <inheritdoc />
        public override int GetHashCode() => Tag.GetHashCode();

        /// <inheritdoc />
        public override string ToString() => Tag;

        /// <summary>
        /// Implictly convert to a <see langword="string" />
        /// </summary>
        public static implicit operator string(DataName name) => name.Tag;
    }
}