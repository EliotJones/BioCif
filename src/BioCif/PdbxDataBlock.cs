namespace BioCif
{
    using System.Collections.Generic;
    using Core;

    /// <summary>
    /// A Protein Data Bank (PDB) data block from a Crystallographic Information File (CIF).
    /// </summary>
    public class PdbxDataBlock
    {
        /// <summary>
        /// Identifies the data block.
        /// </summary>
        public string EntryId { get; set; }

        /// <summary>
        /// Details about the author(s) of this data block.
        /// </summary>
        public List<AuditAuthor> AuditAuthors { get; set; } = new List<AuditAuthor>();

        /// <summary>
        /// The citation from <see cref="AllCitations"/> which is considered to be most pertinent to this entry.
        /// </summary>
        public Citation PrimaryCitation { get; set; }

        /// <summary>
        /// All citations linked to this entry.
        /// </summary>
        public List<Citation> AllCitations { get; set; } = new List<Citation>();

        /// <summary>
        /// All entities in this entry.
        /// </summary>
        public List<Entity> Entities { get; set; } = new List<Entity>();

        /// <summary>
        /// All chemical components used in this entry.
        /// </summary>
        public List<ChemicalComponent> ChemicalComponents { get; set; } = new List<ChemicalComponent>();

        /// <summary>
        /// Details about the space-group symmetry of this item.
        /// </summary>
        public Symmetry Symmetry { get; set; }

        /// <summary>
        /// The underlying <see cref="DataBlock"/> used to produce this <see cref="PdbxDataBlock"/>.
        /// </summary>
        public DataBlock Raw { get; set; }
    }
}
