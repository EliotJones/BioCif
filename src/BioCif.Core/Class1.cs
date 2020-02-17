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
        public DataBlock FirstBlock => DataBlocks.Count > 0 ? DataBlocks[0] : null;

        public IReadOnlyList<DataBlock> DataBlocks { get; }

        public Cif(IReadOnlyList<DataBlock> dataBlocks)
        {
            DataBlocks = dataBlocks ?? throw new ArgumentNullException(nameof(dataBlocks));
        }
    }

    /// <summary>
    /// A partitioned collection of <see cref="DataItem"/>s within a <see cref="DataBlock"/>.
    /// Only valid in a dictionary file.
    /// </summary>
    public class SaveFrame : IDataBlockMember
    {
        /// <summary>
        /// The name of this frame.
        /// </summary>
        public string FrameCode { get; set; }
    }

    public class Table : IDataBlockMember
    {
        public IReadOnlyList<DataName> Headers { get; }

        public IReadOnlyList<TableRow> Rows { get; }

        public Table(IReadOnlyList<DataName> headers, IReadOnlyList<TableRow> rows)
        {
            Headers = headers;
            Rows = rows;
        }
    }

    public class TableRow
    {
        public IDataValue this[int i] => Values[i];
        public IReadOnlyList<IDataValue> Values { get; set; }
    }
}
