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
        private readonly IReadOnlyDictionary<string, int> nameColumnIndexMap;
        /// <summary>
        /// The headers for the table <see cref="Rows"/>.
        /// </summary>
        public IReadOnlyList<DataName> Headers { get; }

        /// <summary>
        /// The rows of the table.
        /// </summary>
        public IReadOnlyList<Row> Rows { get; }

        /// <summary>
        /// The number of rows.
        /// </summary>
        public int Count { get; }

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

            var ncim = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            for (var i = 0; i < headers.Count; i++)
            {
                var header = headers[i];
                ncim[header] = i;
            }

            nameColumnIndexMap = ncim;
            Count = Rows.Count;
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

            /// <summary>
            /// Get the value with the specified name for this row, or <see langword="null" />.
            /// </summary>
            public IDataValue GetOptional(DataName name) => GetOptional(name.Tag);
            /// <summary>
            /// Get the value with the specified name for this row, or <see langword="null" />.
            /// </summary>
            public IDataValue GetOptional(string name)
            {
                if (name == null)
                {
                    return null;
                }

                if (!parent.nameColumnIndexMap.TryGetValue(name, out var index))
                {
                    return null;
                }

                return values[index];
            }

            /// <summary>
            /// Gets the <see langword="string"/> value with the specified name for this row, or <see langword="null" />.
            /// </summary>
            public string GetOptionalString(string name) => GetOptional(name)?.GetStringValue();

            /// <summary>
            /// Gets the <see langword="int"/> value with the specified name for this row if it exists and can be parsed, or <see langword="null"/>.
            /// </summary>
            public int? GetOptionalInt(string name) => GetOptional(name)?.GetIntValue();

            /// <inheritdoc />
            public IEnumerator<IDataValue> GetEnumerator() => values.GetEnumerator();
            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
