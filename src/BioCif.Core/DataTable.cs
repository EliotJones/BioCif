namespace BioCif.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <inheritdoc />
    /// <summary>
    /// A loop in a <see cref="T:BioCif.Core.Cif" /> file defines a set of repeated values associated with headers.
    /// </summary>
    public class DataTable : IDataBlockMember
    {
        /// <summary>
        /// The headers for the table <see cref="Rows"/>.
        /// </summary>
        public IReadOnlyList<DataName> Headers { get; }

        /// <summary>
        /// The rows of the table.
        /// </summary>
        public IReadOnlyList<Row> Rows { get; }

        /// <summary>
        /// Create a new <see cref="DataTable"/>.
        /// </summary>
        public DataTable(IReadOnlyList<DataName> headers, IEnumerable<IReadOnlyList<IDataValue>> rows)
        {
            if (rows == null)
            {
                throw new ArgumentNullException(nameof(rows));
            }
            
            var rowsActual = new List<Row>();
            foreach (var row in rows)
            {
                if (row == null)
                {
                    throw new ArgumentException("Rows contained a null value.", nameof(rows));
                }

                rowsActual.Add(new Row(this, row));
            }

            Headers = headers ?? throw new ArgumentNullException(nameof(headers));
            Rows = rowsActual;
        }

        /// <inheritdoc />
        /// <summary>
        /// A row in a <see cref="T:BioCif.Core.DataTable" />.
        /// </summary>
        public class Row : IReadOnlyList<IDataValue>
        {
            private readonly DataTable parent;
            private readonly IReadOnlyList<IDataValue> values;

            /// <inheritdoc />
            public IDataValue this[int i] => values[i];

            /// <inheritdoc />
            public int Count { get; }

            /// <summary>
            /// Create a new <see cref="Row"/>.
            /// </summary>
            public Row(DataTable parent, IReadOnlyList<IDataValue> values)
            {
                this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
                this.values = values ?? throw new ArgumentNullException(nameof(values));
                Count = values.Count;
            }

            /// <inheritdoc />
            public IEnumerator<IDataValue> GetEnumerator() => values.GetEnumerator();
            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
