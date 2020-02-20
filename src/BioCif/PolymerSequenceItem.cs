namespace BioCif
{
    /// <summary>
    /// An item in the polymer sequence.
    /// </summary>
    public struct PolymerSequenceItem
    {
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