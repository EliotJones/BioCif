namespace BioCif.Core
{
    /// <summary>
    /// The version of the CIF format used by a file.
    /// </summary>
    public enum CifFileVersion
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Versions 1 and 1.1 of the CIF format.
        /// See: https://www.iucr.org/resources/cif/spec/version1.1/cifsyntax.
        /// </summary>
        Version1_1 = 1,
        /// <summary>
        /// Version 2 of the CIF format. Version 2 adds triple-quote (''' and """) delimited values,
        /// list syntax ([ list, here ]) and table syntax ({ 'table': any }) as well ast UTF-8 support.
        /// See: http://journals.iucr.org/j/issues/2016/01/00/aj5269/index.html.
        /// </summary>
        Version2 = 2
    }
}
