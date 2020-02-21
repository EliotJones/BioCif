namespace BioCif
{
    /// <summary>
    /// Details about the space-group symmetry.
    /// </summary>
    public class Symmetry
    {
        /// <summary>
        /// Category name.
        /// </summary>
        public const string Category = @"symmetry";
        /// <summary>
        /// Field name for <see cref="EntryId"/>.
        /// </summary>
        public const string EntryIdFieldName = "symmetry.entry_id";
        /// <summary>
        /// Field name for <see cref="TablesNumber"/>.
        /// </summary>
        public const string TableNumberFieldName = "symmetry.Int_Tables_number";
        /// <summary>
        /// Field name for <see cref="CellSettingRaw"/>.
        /// </summary>
        public const string CellSettingRawFieldName = "symmetry.cell_setting";
        /// <summary>
        /// Field name for <see cref="FullSpaceGroupNameHM"/>.
        /// </summary>
        public const string FullSpaceGroupNameHMFieldName = "symmetry.pdbx_full_space_group_name_H-M";
        /// <summary>
        /// Field name for <see cref="SpaceGroupNameHM"/>.
        /// </summary>
        public const string SpaceGroupNameHMFieldName = "symmetry.space_group_name_H-M";
        /// <summary>
        /// Field name for <see cref="SpaceGroupNameHall"/>.
        /// </summary>
        public const string SpaceGroupNameHallFieldName = "symmetry.space_group_name_Hall";

        /// <summary>
        /// A pointer to <see cref="PdbxDataBlock.EntryId"/>.
        /// </summary>
        public string EntryId { get; set; }

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
        public CellSettings CellSetting
        {
            get
            {
                switch (CellSettingRaw?.ToLowerInvariant())
                {
                    case "cubic":
                        return CellSettings.Cubic;
                    case "hexagonal":
                        return CellSettings.Hexagonal;
                    case "monoclinic":
                        return CellSettings.Monoclinic;
                    case "orthorhombic":
                        return CellSettings.Orthorhombic;
                    case "rhombohedral":
                        return CellSettings.Rhombohedral;
                    case "tetragonal":
                        return CellSettings.Tetragonal;
                    case "triclinic":
                        return CellSettings.Triclinic;
                    case "trigonal":
                        return CellSettings.Trigonal;
                    default:
                        return CellSettings.Unknown;
                }
            }
        }

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