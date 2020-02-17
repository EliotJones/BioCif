namespace BioCif.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The top level data container for a <see cref="Cif"/> file.
    /// </summary>
    public class DataBlock
    {
        /// <summary>
        /// The name of the <see cref="DataBlock"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The items contained in this <see cref="DataBlock"/>.
        /// </summary>
        public IReadOnlyList<IDataBlockMember> Contents { get; }

        /// <summary>
        /// Create a new <see cref="DataBlock"/>.
        /// </summary>
        public DataBlock(string name, IReadOnlyList<IDataBlockMember> contents)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Contents = contents ?? throw new ArgumentNullException(nameof(contents));
        }

        public override string ToString()
        {
            return $"Block ({Name}): {Contents.Count} items.";
        }
    }
}