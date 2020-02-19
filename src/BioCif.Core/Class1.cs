namespace BioCif.Core
{
    using System.Collections.Generic;

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
