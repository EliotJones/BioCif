namespace BioCif
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Record details about the literature cited as being relevant to this entry.
    /// </summary>
    public class Citation
    {
        /// <summary>
        /// Uniquely identifies a citation.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The primary citation (<see cref="Id"/> is 'primary') indicates the citation that is considered most pertinent to this entry.
        /// </summary>
        public bool IsPrimary => string.Equals(Id, "primary", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// The title of the citation; relevant for journal articles, books and book chapters.
        /// Required for deposition of new entries.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Abstract for the citation.
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// The Chemical Abstracts Service (CAS) abstract identifier.
        /// </summary>
        public string AbstractIdCas { get; set; }

        /// <summary>
        /// The country/region of publication; relevant for books and book chapters.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Details of the journal the citation was published in.
        /// This will never be <see langword="null"/> even if all entries in the <see cref="Journal"/>
        /// are <see langword="null"/>.
        /// </summary>
        public Journal Journal { get; set; } = new Journal();

        /// <summary>
        /// Language in which the cited article is written.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// The first page of the citation.
        /// </summary>
        public string PageFirst { get; set; }

        /// <summary>
        /// The last page of the citation.
        /// </summary>
        public string PageLast { get; set; }

        /// <summary>
        /// Document Object Identifier used by doi.org to uniquely specify bibliographic entry.
        /// </summary>
        public string Doi { get; set; }

        /// <summary>
        /// Ascession number used by PubMed to categorize a specific bibliographic entry.
        /// </summary>
        public string PubMedAscessionNumber { get; set; }

        /// <summary>
        /// The year of the citation.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Flag to indicate that this citation will not be published.
        /// </summary>
        public bool UnpublishedFlag { get; set; }

        /// <summary>
        /// The list of authors associated with this citation.
        /// </summary>
        public List<CitationAuthor> Authors { get; } = new List<CitationAuthor>();
    }
}
