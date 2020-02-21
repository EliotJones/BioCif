namespace BioCif.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A dictionary (known as a 'Table' in the <see cref="CifFileVersion.Version2"/> specification) is a set of key-value
    /// pairs where the keys are strings.
    /// </summary>
    public class DataDictionary : IDataValue, IReadOnlyDictionary<string, IDataValue>
    {
        private readonly IReadOnlyDictionary<string, IDataValue> values;

        /// <inheritdoc />
        public DataValueType DataType { get; } = DataValueType.Dictionary;

        /// <inheritdoc />
        public int Count => values.Count;

        /// <inheritdoc />
        public bool ContainsKey(string key) => values.ContainsKey(key);

        /// <inheritdoc />
        public bool TryGetValue(string key, out IDataValue value) => values.TryGetValue(key, out value);

        /// <inheritdoc />
        public IDataValue this[string key] => values[key];

        /// <inheritdoc />
        public IEnumerable<string> Keys => values.Keys;

        /// <inheritdoc />
        public IEnumerable<IDataValue> Values => values.Values;

        /// <summary>
        /// Create a new <see cref="DataDictionary"/>.
        /// </summary>
        public DataDictionary(IReadOnlyDictionary<string, IDataValue> values)
        {
            this.values = values ?? throw new ArgumentNullException(nameof(values));
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<string, IDataValue>> GetEnumerator() => values.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}