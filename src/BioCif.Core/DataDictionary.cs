namespace BioCif.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class DataDictionary : IDataValue, IReadOnlyDictionary<string, IDataValue>
    {
        private readonly IReadOnlyDictionary<string, IDataValue> values;

        /// <inheritdoc />
        public DataValueType DataType { get; } = DataValueType.Dictionary;

        public int Count => values.Count;

        public bool ContainsKey(string key) => values.ContainsKey(key);

        public bool TryGetValue(string key, out IDataValue value) => values.TryGetValue(key, out value);

        public IDataValue this[string key] => values[key];

        public IEnumerable<string> Keys => values.Keys;

        public IEnumerable<IDataValue> Values => values.Values;

        public DataDictionary(IReadOnlyDictionary<string, IDataValue> values)
        {
            this.values = values ?? throw new ArgumentNullException(nameof(values));
        }

        public IEnumerator<KeyValuePair<string, IDataValue>> GetEnumerator() => values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}