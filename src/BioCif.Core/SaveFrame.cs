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

        public int Count => members.Count;

        public IDataBlockMember this[int index] => members[index];

        public SaveFrame(string frameCode, IReadOnlyList<IDataBlockMember> members)
        {
            FrameCode = frameCode;
            this.members = members ?? throw new ArgumentNullException(nameof(members));
        }

        /// <summary>
        /// The name of this frame.
        /// </summary>
        public string FrameCode { get; }

        public IEnumerator<IDataBlockMember> GetEnumerator() => members.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}