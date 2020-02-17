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
        public Version Version { get; set; } = Version.Version2;

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Encoding: {FileEncoding}, Version: {Version}";
        }
    }
}