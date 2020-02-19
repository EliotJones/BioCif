namespace BioCif.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <inheritdoc />
    /// <summary>
    /// The top level data container for a <see cref="T:BioCif.Core.Cif" /> file.
    /// </summary>
    public class DataBlock : IReadOnlyList<IDataBlockMember>
    {
        /// <summary>
        /// The name of the <see cref="DataBlock"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The items contained in this <see cref="DataBlock"/>.
        /// </summary>
        private readonly IReadOnlyList<IDataBlockMember> contents;

        /// <inheritdoc />
        public int Count => contents.Count;

        /// <inheritdoc />
        public IDataBlockMember this[int index] => contents[index];

        /// <summary>
        /// Create a new <see cref="DataBlock"/>.
        /// </summary>
        public DataBlock(string name, IReadOnlyList<IDataBlockMember> contents)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            this.contents = contents ?? throw new ArgumentNullException(nameof(contents));
        }

        public bool TryGet<T>(string name, out T value) where T : IDataValue => TryGet(new DataName(name), out value);
        public bool TryGet<T>(DataName name, out T value) where T : IDataValue
        {
            value = default(T);

            foreach (var content in contents)
            {
                if (!(content is DataItem item))
                {
                    continue;
                }

                if (item.Name.Equals(name) && item.Value is T result)
                {
                    value = result;
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public IEnumerator<IDataBlockMember> GetEnumerator() => contents.GetEnumerator();

        /// <inheritdoc />
        public override string ToString() => $"Block ({Name}): {contents.Count} items.";

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}