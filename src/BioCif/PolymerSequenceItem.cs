namespace BioCif
{
    /// <summary>
    /// An item in the polymer sequence.
    /// </summary>
    public struct PolymerSequenceItem
    {
        /// <summary>
        /// Category name.
        /// </summary>
        public const string Category = "entity_poly_seq";

        /// <summary>
        /// Field name for the entity id field.
        /// </summary>
        public const string EntityIdFieldName = "entity_poly_seq.entity_id";
        /// <summary>
        /// Field name for the number field.
        /// </summary>
        public const string NumberFieldName = "entity_poly_seq.num";
        /// <summary>
        /// Field name for <see cref="ChemicalComponentId"/>.
        /// </summary>
        public const string ChemicalComponentIdFieldName = "entity_poly_seq.mon_id";
        /// <summary>
        /// Field name for <see cref="Heterogeneous"/>.
        /// </summary>
        public const string HeterogeneousFieldName = "entity_poly_seq.hetero";

        /// <summary>
        /// Links to the chemical component.
        /// </summary>
        public string ChemicalComponentId { get; set; }

        /// <summary>
        /// Indicate whether this monomer in the polymer is heterogeneous in sequence.
        /// </summary>
        public bool Heterogeneous { get; set; }
    }
}