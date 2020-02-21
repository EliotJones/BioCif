namespace BioCif
{
    /// <summary>
    /// Details about an author of a <see cref="PdbxDataBlock"/>.
    /// </summary>
    public class AuditAuthor
    {
        /// <summary>
        /// Category name.
        /// </summary>
        public const string Category = "audit_author";
        /// <summary>
        /// Field name for <see cref="Ordinal"/>.
        /// </summary>
        public const string OrdinalFieldName = "audit_author.pdbx_ordinal";
        /// <summary>
        /// Field name for <see cref="Orcid"/>.
        /// </summary>
        public const string OrcidFieldName = "audit_author.identifier_ORCID";
        /// <summary>
        /// Field name for <see cref="Name"/>.
        /// </summary>
        public const string NameFieldName = "audit_author.name";
        /// <summary>
        /// Field name for <see cref="Address"/>.
        /// </summary>
        public const string AddressFieldName = "audit_author.address";

        /// <summary>
        /// Order of the author's name in the list of audit authors.
        /// </summary>
        public int Ordinal { get; set; }

        /// <summary>
        /// Open Researcher and Contributor ID (ORCID).
        /// </summary>
        public string Orcid { get; set; }

        /// <summary>
        /// The name of an author of this data block.
        /// The family name(s), followed by a comma and including any dynastic components, precedes the first name(s) or initial(s).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The address of this author.
        /// </summary>
        public string Address { get; set; }
    }
}