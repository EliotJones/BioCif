namespace BioCif.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A file in the Crystallographic Information File format, the standard for information interchange in crystallography.
    /// See: https://www.iucr.org/resources/cif.
    /// </summary>
    public class Cif
    {
        /// <summary>
        /// Get the first <see cref="DataBlock"/> in the file or <see langword="null" /> if the file is empty.
        /// </summary>
        public DataBlock FirstBlock => DataBlocks.Count > 0 ? DataBlocks[0] : null;

        /// <summary>
        /// All <see cref="DataBlock"/>s in the file.
        /// </summary>
        public IReadOnlyList<DataBlock> DataBlocks { get; }

        /// <summary>
        /// Create <see cref="Cif"/>.
        /// </summary>
        public Cif(IReadOnlyList<DataBlock> dataBlocks)
        {
            DataBlocks = dataBlocks ?? throw new ArgumentNullException(nameof(dataBlocks));
        }
    }
}