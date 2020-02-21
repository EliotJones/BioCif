namespace BioCif.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <inheritdoc cref="IReadOnlyList{T}" />
    /// <summary>
    /// A list of values associated with a <see cref="T:BioCif.Core.DataName" />.
    /// For the loop list type see <see cref="T:BioCif.Core.Table" />.
    /// </summary>
    public class DataList : IDataValue, IReadOnlyList<IDataValue>
    {
        private readonly IReadOnlyList<IDataValue> values;

        /// <inheritdoc />
        public DataValueType DataType { get; } = DataValueType.List;

        /// <inheritdoc />
        public int Count => values.Count;

        /// <inheritdoc />
        public IDataValue this[int index] => values[index];
        
        /// <summary>
        /// Create a new <see cref="DataList"/>.
        /// </summary>
        public DataList(IReadOnlyList<IDataValue> values)
        {
            this.values = values ?? throw new ArgumentNullException(nameof(values));
        }

        /// <inheritdoc />
        public string GetStringValue() => null;

        /// <inheritdoc />
        public int? GetIntValue() => null;

        /// <inheritdoc />
        public IEnumerator<IDataValue> GetEnumerator() => values.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public override string ToString()
        {
            var list = string.Join(" ", this.values.Select(x => x.ToString()));
            return $"[ {list} ]";
        }
    }
}