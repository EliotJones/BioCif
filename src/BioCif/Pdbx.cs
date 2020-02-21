namespace BioCif
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Data from a macro-molecular CIF / PDBX Protein Data Bank file.
    /// </summary>
    public class Pdbx
    {
        /// <summary>
        /// The first <see cref="PdbxDataBlock"/> in the file.
        /// </summary>
        public PdbxDataBlock First => DataBlocks?.Count > 0 ? DataBlocks[0] : null;

        /// <summary>
        /// The set of <see cref="PdbxDataBlock"/>s in the file.
        /// </summary>
        public IReadOnlyList<PdbxDataBlock> DataBlocks { get; }

        /// <summary>
        /// Create a new <see cref="Pdbx"/>.
        /// </summary>
        public Pdbx(IReadOnlyList<PdbxDataBlock> dataBlocks)
        {
            DataBlocks = dataBlocks ?? throw new ArgumentNullException(nameof(dataBlocks));
        }
    }
}