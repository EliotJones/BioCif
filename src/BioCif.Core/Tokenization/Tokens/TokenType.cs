namespace BioCif.Core.Tokenization.Tokens
{
    /// <summary>
    /// The meaning of the <see cref="Token"/> in a CIF file.
    /// </summary>
    public enum TokenType : byte
    {
        /// <summary>
        /// Tokenization went wrong and the meaning cannot be determined.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// A data name, the preceding underscore '_' is ignored for <see cref="Token.Value"/>.
        /// </summary>
        Name = 1,
        /// <summary>
        /// A data value.
        /// </summary>
        Value = 2,
        /// <summary>
        /// Marks the start of a data block.
        /// </summary>
        DataBlock = 3,
        /// <summary>
        /// Marks the start of a save frame.
        /// </summary>
        SaveFrame = 4,
        /// <summary>
        /// Marks the end of a save frame.
        /// </summary>
        SaveFrameEnd = 5,
        /// <summary>
        /// Marks the start of a loop.
        /// </summary>
        Loop = 6,
        /// <summary>
        /// Marks a comment line, the preceding hash '#' is ignored for <see cref="Token.Value"/>.
        /// </summary>
        Comment = 7,
        /// <summary>
        /// Marks the start of a list for <see cref="Version.Version2"/> CIF files.
        /// </summary>
        StartList = 8,
        /// <summary>
        /// Marks the end of a list for <see cref="Version.Version2"/> CIF files.
        /// </summary>
        EndList = 9,
        /// <summary>
        /// Marks the start of a table for <see cref="Version.Version2"/> CIF files.
        /// </summary>
        StartTable = 10,
        /// <summary>
        /// Marks the end of a table for <see cref="Version.Version2"/> CIF files.
        /// </summary>
        EndTable = 11,
    }
}