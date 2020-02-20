namespace BioCif
{
    /// <summary>
    /// Details about the dictionary versions against which the data names appearing in the current data block are conformant.
    /// </summary>
    public class AuditConform
    {
        /// <summary>
        ///  The string identifying the highest-level dictionary defining data names used in this file.
        /// </summary>
        public string DictionaryName { get; set; }

        /// <summary>
        ///  The version number of the dictionary to which the current data block conforms.
        /// </summary>
        public string DictionaryVersion { get; set; }

        /// <summary>
        /// A file name or uniform resource locator (URL) for the dictionary to which the current data block conforms.
        /// </summary>
        public string DictionaryLocation { get; set; }
    }
}