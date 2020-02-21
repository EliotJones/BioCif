namespace BioCif
{
    using System.IO;
    using System.Text;
    using Core;
    using Core.Parsing;
    using Version = Core.Version;

    /// <summary>
    /// Parse a <see cref="Pdbx"/> from a CIF file.
    /// </summary>
    public static class PdbxParser
    {
        public static Pdbx ParseString(string cif)
        {
            using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(cif)))
            {
                return ParseStream(stream, Encoding.Unicode);
            }
        }

        public static Pdbx ParseFile(string filePath, Encoding encoding = null)
        {
            using (var fs = File.OpenRead(filePath))
            {
                return ParseStream(fs, encoding);
            }
        }
        
        public static Pdbx ParseStream(Stream stream, Encoding encoding = null)
        {
            var cif = CifParser.Parse(stream, new CifParsingOptions
            {
                Version = Version.Version2,
                FileEncoding = encoding
            });

            return null;
        }
    }
}
