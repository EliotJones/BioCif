namespace BioCif
{
    /// <summary>
    /// Details about the atom sites in a macromolecular crystal structure, such as the positional coordinates, atomic displacement parameters,
    /// magnetic moments and directions.
    /// </summary>
    public class AtomSite
    {
        /// <summary>
        /// Category name.
        /// </summary>
        public const string Category = "atom_site";

        /// <summary>
        /// Field name for <see cref="Id"/>.
        /// </summary>
        public const string IdFieldName = "atom_site.id";
        /// <summary>
        /// Field name for <see cref="AtomSiteGroupPdb"/>.
        /// </summary>
        public const string AtomSiteGroupPdbFieldName = "atom_site.group_PDB";
        /// <summary>
        /// Field name for <see cref="CartesianXCoordinate"/>.
        /// </summary>
        public const string CartesianXCoordinateFieldName = "atom_site.Cartn_x";
        /// <summary>
        /// Field name for <see cref="CartesianYCoordinate"/>.
        /// </summary>
        public const string CartesianYCoordinateFieldName = "atom_site.Cartn_y";
        /// <summary>
        /// Field name for <see cref="CartesianZCoordinate"/>.
        /// </summary>
        public const string CartesianZCoordinateFieldName = "atom_site.Cartn_z";

        /// <summary>
        /// The value of _atom_site.id must uniquely identify a record in the ATOM_SITE list.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The group of atoms to which the atom site belongs. This data item is provided for compatibility with the original Protein Data Bank format only.
        /// </summary>
        public string AtomSiteGroupPdb { get; set; }

        /// <summary>
        /// The x atom-site coordinate in angstroms specified according to a set of orthogonal Cartesian axes related to the cell axes.
        /// </summary>
        public double? CartesianXCoordinate { get; set; }
        /// <summary>
        /// The y atom-site coordinate in angstroms specified according to a set of orthogonal Cartesian axes related to the cell axes.
        /// </summary>
        public double? CartesianYCoordinate { get; set; }
        /// <summary>
        /// The z atom-site coordinate in angstroms specified according to a set of orthogonal Cartesian axes related to the cell axes.
        /// </summary>
        public double? CartesianZCoordinate { get; set; }
    }
}