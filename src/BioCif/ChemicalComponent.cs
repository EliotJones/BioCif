namespace BioCif
{
    /// <summary>
    /// Details about each of the chemical components from which the relevant chemical structures can be constructed, such as name, mass or charge.
    /// </summary>
    public class ChemicalComponent
    {
        /// <summary>
        /// Category name.
        /// </summary>
        public const string Category = "chem_comp";
        /// <summary>
        /// Field name for <see cref="Id"/>.
        /// </summary>
        public const string IdFieldName = "chem_comp.id";
        /// <summary>
        /// Field name for <see cref="Formula"/>.
        /// </summary>
        public const string FormulaFieldName = "chem_comp.formula";
        /// <summary>
        /// Field name for <see cref="FormulaWeight"/>.
        /// </summary>
        public const string FormulaWeightFieldName = "chem_comp.formula_weight";
        /// <summary>
        /// Field name for <see cref="IsStandardMonomer"/>.
        /// </summary>
        public const string IsStandardMonomerFieldName = "chem_comp.mon_nstd_flag";
        /// <summary>
        /// Field name for <see cref="Name"/>.
        /// </summary>
        public const string NameFieldName = "chem_comp.name";
        /// <summary>
        /// Field name for <see cref="Synonyms"/>.
        /// </summary>
        public const string SynonymsFieldName = "chem_comp.pdbx_synonyms";
        /// <summary>
        /// Field name for <see cref="Type"/>.
        /// </summary>
        public const string TypeFieldName = "chem_comp.type";

        /// <summary>
        /// Uniquely identifies each chemical component.
        /// For protein polymer entities, this is the three-letter code for the amino acid.
        /// For nucleic acid polymer entities, this is the one-letter code for the base.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The formula for the chemical component.
        /// Formulae are written according to the following rules:
        /// (1) Only recognized element symbols may be used.
        /// (2) Each element symbol is followed by a 'count' number. A count of '1' may be omitted.
        /// (3) A space or parenthesis must separate each cluster of (element symbol + count),
        /// but in general parentheses are not used.
        /// (4) The order of elements depends on whether carbon is present or not. 
        /// If carbon is present, the order should be: C, then H, 
        /// then the other elements in alphabetical order of their symbol.
        /// If carbon is not present, the elements are listed purely in alphabetic order of their symbol.
        /// This is the 'Hill' system used by Chemical Abstracts.
        /// </summary>
        public string Formula { get; set; }

        /// <summary>
        /// Formula mass in daltons of the chemical component.
        /// </summary>
        public double? FormulaWeight { get; set; }

        /// <summary>
        /// If <see langword="true"/> indicates that this is a 'standard' monomer, <see langword="false"/> indicates that it is 'nonstandard'.
        /// </summary>
        public bool IsStandardMonomer { get; set; }

        /// <summary>
        /// The full name of the component.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Synonym list for the component.
        /// </summary>
        public string Synonyms { get; set; }

        /// <summary>
        /// For standard polymer components, the type of the monomer.
        /// Note that monomers that will form polymers are of three types:
        /// linking monomers, monomers with some type of N-terminal (or 5') cap and monomers with some type of C-terminal (or 3') cap.
        /// </summary>
        public string Type { get; set; }
    }
}
