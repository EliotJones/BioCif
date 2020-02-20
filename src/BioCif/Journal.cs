namespace BioCif
{
    /// <summary>
    /// Details of a <see cref="Citation"/> relevant to citations published in a journal.
    /// </summary>
    public class Journal
    {
        /// <summary>
        /// Abbreviated name of the cited journal as given in the Chemical Abstracts Service Source Index.
        /// </summary>
        public string Abbreviation { get; set; }

        /// <summary>
        /// The American Society for Testing and Materials (ASTM) code assigned to the journal cited
        /// (also referred to as the CODEN designator of the Chemical Abstracts Service); relevant for journal articles.
        /// </summary>
        public string AstmCode { get; set; }

        /// <summary>
        /// The Cambridge Structural Database (CSD) code assigned to the journal cited; relevant for journal articles.
        /// This is also the system used at the Protein Data Bank (PDB).
        /// </summary>
        public string CsdCode { get; set; }

        /// <summary>
        /// The International Standard Serial Number (ISSN) code assigned to the journal cited; relevant for journal articles.
        /// </summary>
        public string IssnCode { get; set; }

        /// <summary>
        /// Full name of the cited journal.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Issue number of the journal cited.
        /// </summary>
        public string Issue { get; set; }

        /// <summary>
        /// Volume number of the journal cited.
        /// </summary>
        public string Volume { get; set; }
    }
}