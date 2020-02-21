namespace BioCif
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using Core;
    using Core.Parsing;

    /// <summary>
    /// Parse a <see cref="Pdbx"/> from a CIF file.
    /// </summary>
    public static class PdbxParser
    {
        /// <summary>
        /// Parse the <see cref="Pdbx"/> from the input <see langword="string"/>.
        /// </summary>
        public static Pdbx ParseString(string cif)
        {
            using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(cif)))
            {
                return ParseStream(stream, Encoding.Unicode);
            }
        }

        /// <summary>
        /// Parse the <see cref="Pdbx"/> from the file at the specified path with an optional encoding specified.
        /// If no encoding is specified the parser will infer the encoding from the file contents.
        /// </summary>
        public static Pdbx ParseFile(string filePath, Encoding encoding = null)
        {
            using (var fs = File.OpenRead(filePath))
            {
                return ParseStream(fs, encoding);
            }
        }
        
        /// <summary>
        /// Parse the <see cref="Pdbx"/> from the stream containing the CIF text with an optional encoding specified.
        /// If no encoding is specified the parser will infer the encoding from the stream.
        /// </summary>
        public static Pdbx ParseStream(Stream stream, Encoding encoding = null)
        {
            var cif = CifParser.Parse(stream, new CifParsingOptions
            {
                CifFileVersion = CifFileVersion.Version2,
                FileEncoding = encoding
            });

            var blocks = new List<PdbxDataBlock>(cif.DataBlocks.Count);

            for (var i = 0; i < cif.DataBlocks.Count; i++)
            {
                var cifDataBlock = cif.DataBlocks[i];

                if (cifDataBlock == null)
                {
                    throw new InvalidOperationException($"Null DataBlock in cif file contents at index: {i}.");
                }

                var entryId = GetSimpleNamedValue(cifDataBlock, "entry.id");

                blocks.Add(new PdbxDataBlock
                {
                    EntryId = entryId,
                    AuditAuthors = GetAuditAuthors(cifDataBlock),
                    Raw = cifDataBlock
                });
            }

            return new Pdbx(blocks);
        }

        private static List<AuditAuthor> GetAuditAuthors(DataBlock cifDataBlock)
        {
            var result = new List<AuditAuthor>();

            var auditAuthorsTable = cifDataBlock.GetTableForCategory("audit_author");

            if (auditAuthorsTable == null)
            {
                return result;
            }

            foreach (var row in auditAuthorsTable.Rows)
            {
                var ordinalRaw = row.GetOptional("audit_author.pdbx_ordinal");
                var name = row.GetOptional("audit_author.name");
                var orcid = row.GetOptional("audit_author.identifier_ORCID");
                var address = row.GetOptional("audit_author.address");

                var ordinal = result.Count;

                if (int.TryParse(ordinalRaw?.GetStringValue(), NumberStyles.Number, CultureInfo.InvariantCulture, out var ordinalActual))
                {
                    ordinal = ordinalActual;
                }

                result.Add(new AuditAuthor
                {
                    Ordinal = ordinal,
                    Name = name?.GetStringValue(),
                    Orcid = orcid?.GetStringValue(),
                    Address = address?.GetStringValue()
                });
            }

            return result;
        }

        private static string GetSimpleNamedValue(DataBlock cifDataBlock, string name)
        {
            if (!cifDataBlock.TryGet(name, out DataValueSimple value))
            {
                return null;
            }

            return value.IsNullSymbol ? null : value.Value;
        }
    }
}
