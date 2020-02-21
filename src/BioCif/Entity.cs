namespace BioCif
{
    /// <summary>
    /// Details about the molecular entities that are present in the crystallographic structure.
    /// </summary>
    public class Entity
    {
        #region names
        /// <summary>
        /// Category name.
        /// </summary>
        public const string Category = "entity";
        /// <summary>
        /// Field name for <see cref="Id"/>.
        /// </summary>
        public const string IdFieldName = "entity.id";
        /// <summary>
        /// Field name for <see cref="Details"/>.
        /// </summary>
        public const string DetailsFieldName = "entity.details";
        /// <summary>
        /// Field name for <see cref="Description"/>.
        /// </summary>
        public const string DescriptionFieldName = "entity.pdbx_description";
        /// <summary>
        /// Field name for <see cref="EnzymeCommission"/>.
        /// </summary>
        public const string EnzymeCommissionFieldName = "entity.pdbx_ec";
        /// <summary>
        /// Field name for <see cref="FormulaWeight"/>.
        /// </summary>
        public const string FormulaWeightFieldName = "entity.formula_weight";
        /// <summary>
        /// Field name for <see cref="EntitiesPerBiologicalUnit"/>.
        /// </summary>
        public const string EntitiesPerBiologicalUnitFieldName = "entity.pdbx_entities_per_biological_unit";
        /// <summary>
        /// Field name for <see cref="ExperimentalFormulaWeight"/>.
        /// </summary>
        public const string ExperimentalFormulaWeightFieldName = "entity.pdbx_formula_weight_exptl";
        /// <summary>
        /// Field name for <see cref="ExperimentalFormulaWeightMethod"/>.
        /// </summary>
        public const string ExperimentalFormulaWeightMethodFieldName = "entity.pdbx_formula_weight_exptl_method";
        /// <summary>
        /// Field name for <see cref="Fragment"/>.
        /// </summary>
        public const string FragmentFieldName = "entity.pdbx_fragment";
        /// <summary>
        /// Field name for <see cref="Modification"/>.
        /// </summary>
        public const string ModificationFieldName = "entity.pdbx_modification";
        /// <summary>
        /// Field name for <see cref="Mutation"/>.
        /// </summary>
        public const string MutationFieldName = "entity.pdbx_mutation";
        /// <summary>
        /// Field name for <see cref="NumberOfMolecules"/>.
        /// </summary>
        public const string NumberOfMoleculesFieldName = "entity.pdbx_number_of_molecules";
        /// <summary>
        /// Field name for <see cref="ParentEntityId"/>.
        /// </summary>
        public const string ParentEntityIdFieldName = "entity.pdbx_parent_entity_id";
        /// <summary>
        /// Field name for <see cref="TargetId"/>.
        /// </summary>
        public const string TargetIdFieldName = "entity.pdbx_target_id";
        /// <summary>
        /// Field name for <see cref="SourceMethodRaw"/>.
        /// </summary>
        public const string SourceMethodRawFieldName = "entity.src_method";
        /// <summary>
        /// Field name for <see cref="TypeRaw"/>.
        /// </summary>
        public const string TypeRawFieldName = "entity.type";
        #endregion

        /// <summary>
        /// Uniquely identifies this entity.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// A description of the entity. Corresponds to the compound name in the PDB format.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Describes special aspects of the entity.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Formula mass in daltons.
        /// </summary>
        public double? FormulaWeight { get; set; }

        /// <summary>
        /// Enzyme Commission (EC) number(s).
        /// </summary>
        public string EnzymeCommission { get; set; }

        /// <summary>
        /// Number of entity molecules in the biological assembly.
        /// </summary>
        public double? EntitiesPerBiologicalUnit { get; set; }

        /// <summary>
        /// Experimentally determined formula mass in daltons.
        /// </summary>
        public double? ExperimentalFormulaWeight { get; set; }

        /// <summary>
        /// Method used to determine the <see cref="ExperimentalFormulaWeight"/>.
        /// </summary>
        public string ExperimentalFormulaWeightMethod { get; set; }

        /// <summary>
        /// Entity fragment description(s).
        /// </summary>
        public string Fragment { get; set; }

        /// <summary>
        /// Description(s) of any chemical or post-translational modifications.
        /// </summary>
        public string Modification { get; set; }

        /// <summary>
        /// Details about any entity mutation(s).
        /// </summary>
        public string Mutation { get; set; }

        /// <summary>
        /// Placeholder for the number of molecules of the entity in the entry.
        /// </summary>
        public int? NumberOfMolecules { get; set; }

        /// <summary>
        /// The <see cref="Id"/> for the parent entity if this entity is part of a complex entity.
        /// For example a chimeric entity may be decomposed into several independent chemical entities,
        /// where each component entity was obtained from a different source.
        /// </summary>
        public string ParentEntityId { get; set; }

        /// <summary>
        /// The method by which the sample for the entity was produced.
        /// Entities isolated directly from natural sources (tissues, soil samples etc.) are expected to have further information in the
        /// ENTITY_SRC_NAT category. 
        /// Entities isolated from genetically manipulated sources are expected to have further information in the ENTITY_SRC_GEN category.
        /// </summary>
        public string SourceMethodRaw { get; set; }

        /// <summary>
        /// The value of <see cref="SourceMethodRaw"/> mapped to the <see cref="Source"/> enum.
        /// </summary>
        public Source SourceMethod { get; set; }

        /// <summary>
        /// Points to a TARGETDB target idenitifier from which this entity was generated.
        /// </summary>
        public string TargetId { get; set; }

        /// <summary>
        /// Defines the type of the entity.
        /// Polymer entities are expected to have corresponding ENTITY_POLY and associated entries.
        /// Non-polymer entities are expected to have corresponding CHEM_COMP and associated entries.
        /// Water entities are not expected to have corresponding entries in the ENTITY category.
        /// </summary>
        public string TypeRaw { get; set; }

        /// <summary>
        /// The value of <see cref="TypeRaw"/> mapped to the <see cref="EntityType"/> enum.
        /// </summary>
        public EntityType Type { get; set; }

        /// <summary>
        /// Details of the polymer if the <see cref="Type"/> is <see cref="EntityType.Polymer"/>.
        /// </summary>
        public EntityPolymer Polymer { get; set; }

        /// <summary>
        /// Describes the method the entity was produced using.
        /// </summary>
        public enum Source
        {
            /// <summary>
            /// Unrecognised value.
            /// </summary>
            Unknown = 0,
            /// <summary>
            /// Isolated from a genetically manipulated source.
            /// </summary>
            Manipulated = 1,
            /// <summary>
            /// Isolated from a natural source.
            /// </summary>
            Natural = 2,
            /// <summary>
            /// Obtained synthetically.
            /// </summary>
            Synthetic = 3
        }

        /// <summary>
        /// Describes the type of the entity.
        /// </summary>
        public enum EntityType
        {
            /// <summary>
            /// Unrecognised value.
            /// </summary>
            Unknown = 0,
            /// <summary>
            /// Branched.
            /// </summary>
            Branched = 1,
            /// <summary>
            /// Macrolide.
            /// </summary>
            Macrolide = 2,
            /// <summary>
            /// Non-polymer.
            /// </summary>
            NonPolymer = 3,
            /// <summary>
            /// Polymer.
            /// </summary>
            Polymer = 4,
            /// <summary>
            /// Water.
            /// </summary>
            Water = 5
        }
    }
}