namespace BioCif
{
    /// <summary>
    /// Details about the space-group symmetry.
    /// </summary>
    public class Symmetry
    {
        /// <summary>
        /// A pointer to <see cref="PdbxDataBlock.EntryId"/>.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Space-group number from International Tables for Crystallography.
        /// </summary>
        public int? TablesNumber { get; set; }

        /// <summary>
        /// The cell settings for this space-group symmetry.
        /// </summary>
        public string CellSettingRaw { get; set; }

        /// <summary>
        /// The <see cref="CellSettingRaw"/> mapped to the <see cref="CellSettings"/> enum.
        /// </summary>
        public CellSettings CellSetting { get; set; }

        /// <summary>
        /// Used for PDB space group.
        /// </summary>
        public string FullSpaceGroupNameHM { get; set; }

        /// <summary>
        /// Hermann-Mauguin space-group symbol.
        /// Note that the Hermann-Mauguin symbol does not necessarily contain complete information about the symmetry and the space-group origin.
        /// </summary>
        public string SpaceGroupNameHM { get; set; }

        /// <summary>
        /// Space-group symbol as described by Hall (1981). This symbol gives the space-group setting explicitly.
        /// </summary>
        public string SpaceGroupNameHall { get; set; }

        /// <summary>
        /// The possible values for the cell settings for this space-group symmetry.
        /// </summary>
        public enum CellSettings
        {
            /// <summary>
            /// Unrecognized value.
            /// </summary>
            Unknown = 0,
            /// <summary>
            /// Cubic.
            /// </summary>
            Cubic = 1,
            /// <summary>
            /// Hexagonal.
            /// </summary>
            Hexagonal = 2,
            /// <summary>
            /// Monoclinic.
            /// </summary>
            Monoclinic,
            /// <summary>
            /// Orthorhombic.
            /// </summary>
            Orthorhombic,
            /// <summary>
            /// Rhombohedral.
            /// </summary>
            Rhombohedral,
            /// <summary>
            /// Tetragonal.
            /// </summary>
            Tetragonal,
            /// <summary>
            /// Triclinic.
            /// </summary>
            Triclinic,
            /// <summary>
            /// Trigonal.
            /// </summary>
            Trigonal
        }
    }
}