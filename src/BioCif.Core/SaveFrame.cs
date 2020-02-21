namespace BioCif.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A partitioned collection of <see cref="DataItem"/>s within a <see cref="DataBlock"/>.
    /// Only valid in a dictionary file.
    /// </summary>
    public class SaveFrame : IDataBlockMember, IReadOnlyList<IDataBlockMember>
    {
        private readonly IReadOnlyList<IDataBlockMember> members;

        /// <inheritdoc />
        public int Count => members.Count;

        /// <inheritdoc />
        public IDataBlockMember this[int index] => members[index];

        /// <summary>
        /// The name of this save frame excluding the 'save_' prefix.
        /// </summary>
        public string FrameCode { get; }

        /// <summary>
        /// Create a new <see cref="SaveFrame"/>.
        /// </summary>
        public SaveFrame(string frameCode, IReadOnlyList<IDataBlockMember> members)
        {
            FrameCode = frameCode;
            this.members = members ?? throw new ArgumentNullException(nameof(members));
        }
        /// <inheritdoc />
        public IEnumerator<IDataBlockMember> GetEnumerator() => members.GetEnumerator();
        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}