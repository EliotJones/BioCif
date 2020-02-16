namespace BioCif.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// A file in the Crystallographic Information File format, the standard for information interchange in crystallography.
    /// See: https://www.iucr.org/resources/cif.
    /// </summary>
    public class Cif
    {

    }

    public static class CifParser
    {
        public static Cif Parse(Stream stream, CifParsingOptions options)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            ValidateStream(stream);

            if (options == null)
            {
                options = new CifParsingOptions();
            }
            else if (options.FileEncoding == null)
            {
                options.FileEncoding = Encoding.UTF8;
            }

            var buffered = new BufferedStream(stream);
            using (var reader = new StreamReader(buffered, options.FileEncoding))
            {
                
            }

            return null;
        }

        private static void ValidateStream(Stream stream)
        {
            if (!stream.CanRead)
            {
                throw new ArgumentException($"Could not read from the provided stream of type {stream.GetType().FullName}.");
            }

            if (!stream.CanSeek)
            {
                throw new ArgumentException($"Could not seek in provided stream of type {stream.GetType().FullName}.");
            }
        }
    }

    public class CifParsingOptions
    {
        public Encoding FileEncoding { get; set; } = Encoding.UTF8;
    }

    /// <summary>
    /// A partitioned collection of <see cref="DataItem"/>s within a <see cref="DataBlock"/>.
    /// Only valid in a dictionary file.
    /// </summary>
    public class SaveFrame
    {
        /// <summary>
        /// The name of this frame.
        /// </summary>
        public string FrameCode { get; set; }
    }

    public class Table : IDataBlockMember
    {
        public IReadOnlyList<DataName> Headers { get; set; }


    }

    public class TableRow
    {
        public IReadOnlyList<DataValue> Values { get; set; }
    }

    public class DataName
    {
        /// <summary>
        /// Identifier of the content of an associated <see cref="DataValue"/>. 
        /// </summary>
        public string Tag { get; set; }
    }

    public class DataValue
    {

    }

    public class DataItem : IDataBlockMember
    {
        public DataName Name { get; set; }

        public DataValue Value { get; set; }
    }

    public class DataBlock
    {
        public IReadOnlyList<IDataBlockMember> Contents { get; set; }
    }

    public interface IDataBlockMember
    {

    }
}
