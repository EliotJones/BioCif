namespace BioCif
{
    /// <summary>
    /// Details about an author of a <see cref="PdbxDataBlock"/>.
    /// </summary>
    public class AuditAuthor
    {
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