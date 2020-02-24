namespace BioCif
{
    /// <summary>
    /// Details about the properties of the atoms that occupy the atom sites, such as the atomic scattering factors.
    /// </summary>
    public class AtomType
    {
        /// <summary>
        /// Category name.
        /// </summary>
        public const string Category = "atom_type";
        /// <summary>
        /// Field name for <see cref="Symbol"/>.
        /// </summary>
        public const string SymbolFieldName = "atom_type.symbol";
        /// <summary>
        /// Field name for <see cref="OxidationNumber"/>.
        /// </summary>
        public const string OxidationNumberFieldName = "atom_type.oxidation_number";

        /// <summary>
        /// The code used to identify the atom species (singular or plural) representing this atom type.
        /// Normally this code is the element symbol. The code may be composed of any character except an underscore
        /// with the additional proviso that digits designate an oxidation state and must be followed by a + or - character.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Formal oxidation state.
        /// </summary>
        public int? OxidationNumber { get; set; }
    }
}