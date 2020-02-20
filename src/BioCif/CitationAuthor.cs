namespace BioCif
{
    /// <summary>
    /// An author associated with a <see cref="Citation"/>.
    /// </summary>
    public class CitationAuthor
    {
        /// <summary>
        /// Defines the order of the author's name in the list of authors.
        /// </summary>
        public int? Ordinal { get; set; }

        /// <summary>
        /// Name of an author of the citation; relevant for journal articles, books and book chapters.
        /// The family name(s), followed by a comma and including any dynastic components, precedes the first name(s) or initial(s).
        /// </summary>
        public string Name { get; set; }
    }
}