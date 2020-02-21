namespace BioCif.Core
{
    using System.Text;

    /// <summary>
    /// Options used to configure parsing of CIF files.
    /// </summary>
    public class CifParsingOptions
    {
        /// <summary>
        /// The encoding used to interpret the file contents.
        /// </summary>
        public Encoding FileEncoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// The version of the CIF specification the file uses.
        /// </summary>
        public CifFileVersion CifFileVersion { get; set; } = CifFileVersion.Version2;

        /// <summary>
        /// Sets a value to treat as equivalent to <see langword="null"/>.
        /// </summary>
        public string NullSymbol { get; set; } = "?";

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Encoding: {FileEncoding}, Version: {CifFileVersion}";
        }
    }
}