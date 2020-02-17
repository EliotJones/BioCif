namespace BioCif.Core
{
    /// <summary>
    /// The version of the CIF format.
    /// </summary>
    public enum Version
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Versions 1 and 1.1 of the CIF format.
        /// </summary>
        Version1_1 = 1,
        /// <summary>
        /// Version 2 of the CIF format. Version 2 adds triple-quote (''' and """) delimited values,
        /// list syntax ([ list, here ]) and table syntax ({ 'table': any }) as well ast UTF-8 support.
        /// </summary>
        Version2 = 2

    }
}
