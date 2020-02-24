namespace BioCif
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Core;
    using Core.Parsing;

    /// <summary>
    /// Parse a <see cref="Pdbx"/> from a CIF file.
    /// </summary>
    public static class PdbxParser
    {
        /// <summary>
        /// Parse the <see cref="Pdbx"/> from the input <see langword="string"/>.
        /// </summary>
        public static Pdbx ParseString(string cif)
        {
            using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(cif)))
            {
                return ParseStream(stream, Encoding.Unicode);
            }
        }

        /// <summary>
        /// Parse the <see cref="Pdbx"/> from the file at the specified path with an optional encoding specified.
        /// If no encoding is specified the parser will infer the encoding from the file contents.
        /// </summary>
        public static Pdbx ParseFile(string filePath, Encoding encoding = null)
        {
            using (var fs = File.OpenRead(filePath))
            {
                return ParseStream(fs, encoding);
            }
        }
        
        /// <summary>
        /// Parse the <see cref="Pdbx"/> from the stream containing the CIF text with an optional encoding specified.
        /// If no encoding is specified the parser will infer the encoding from the stream.
        /// </summary>
        public static Pdbx ParseStream(Stream stream, Encoding encoding = null)
        {
            var cif = CifParser.Parse(stream, new CifParsingOptions
            {
                CifFileVersion = CifFileVersion.Version2,
                FileEncoding = encoding
            });

            var blocks = new List<PdbxDataBlock>(cif.DataBlocks.Count);

            for (var i = 0; i < cif.DataBlocks.Count; i++)
            {
                var cifDataBlock = cif.DataBlocks[i];

                if (cifDataBlock == null)
                {
                    throw new InvalidOperationException($"Null DataBlock in cif file contents at index: {i}.");
                }

                var entryId = GetSimpleNamedValue(cifDataBlock, "entry.id");

                blocks.Add(new PdbxDataBlock
                {
                    EntryId = entryId,
                    AuditAuthors = GetAuditAuthors(cifDataBlock),
                    Entities = GetEntities(cifDataBlock),
                    Raw = cifDataBlock,
                    Symmetry = GetSymmetry(cifDataBlock)
                });
            }

            return new Pdbx(blocks);
        }

        private static List<AuditAuthor> GetAuditAuthors(DataBlock cifDataBlock)
        {
            var result = new List<AuditAuthor>();

            var auditAuthorsTable = cifDataBlock.GetTableForCategory(AuditAuthor.Category);
            
            foreach (var row in auditAuthorsTable.Rows)
            {
                result.Add(new AuditAuthor
                {
                    Ordinal = row.GetOptionalInt(AuditAuthor.OrdinalFieldName).GetValueOrDefault(result.Count + 1),
                    Name = row.GetOptionalString(AuditAuthor.NameFieldName),
                    Orcid = row.GetOptionalString(AuditAuthor.OrcidFieldName),
                    Address = row.GetOptionalString(AuditAuthor.AddressFieldName)
                });
            }

            return result;
        }

        private static Symmetry GetSymmetry(DataBlock cifDataBlock)
        {
            var symmetryTable = cifDataBlock.GetTableForCategory(Symmetry.Category);

            if (symmetryTable.Count == 0)
            {
                return new Symmetry();
            }

            var first = symmetryTable.Rows[0];

            return new Symmetry
            {
                EntryId = first.GetOptionalString(Symmetry.EntryIdFieldName),
                CellSettingRaw = first.GetOptionalString(Symmetry.CellSettingRawFieldName),
                FullSpaceGroupNameHM = first.GetOptionalString(Symmetry.FullSpaceGroupNameHMFieldName),
                TablesNumber = first.GetOptionalInt(Symmetry.TableNumberFieldName),
                SpaceGroupNameHM = first.GetOptionalString(Symmetry.SpaceGroupNameHMFieldName),
                SpaceGroupNameHall = first.GetOptionalString(Symmetry.SpaceGroupNameHallFieldName)
            };
        }

        private static List<EntityPolymer> GetPolymerEntities(DataBlock cifDataBlock)
        {
            var result = new List<EntityPolymer>();
            var polyEntityTable = cifDataBlock.GetTableForCategory(EntityPolymer.Category);

            foreach (var row in polyEntityTable.Rows)
            {
                result.Add(new EntityPolymer
                {
                    EntityId = row.GetOptionalString(EntityPolymer.EntityIdFieldName),
                    NonStandardMonomer = row.GetOptionalBool(EntityPolymer.NonStandardMonomerFieldName).GetValueOrDefault(),
                    NonStandardLinkage = row.GetOptionalBool(EntityPolymer.NonStandardLinkageFieldName).GetValueOrDefault(),
                    SequenceOneLetterCode = row.GetOptionalString(EntityPolymer.SequenceOneLetterCodeFieldName),
                    SequenceOneLetterCodeCanonical = row.GetOptionalString(EntityPolymer.SequenceOneLetterCodeCanonicalFieldName),
                    StrandId = row.GetOptionalString(EntityPolymer.StrandIdFieldName),
                    TargetIdentifier = row.GetOptionalString(EntityPolymer.TargetIdentifierFieldName),
                    TypeRaw = row.GetOptionalString(EntityPolymer.TypeRawFieldName),
                });
            }

            return result;
        }

        private static List<Entity> GetEntities(DataBlock cifDataBlock)
        {
            var result = new List<Entity>();

            var entityTable = cifDataBlock.GetTableForCategory(Entity.Category);

            var polymers = GetPolymerEntities(cifDataBlock);

            foreach (var row in entityTable.Rows)
            {
                var entity = new Entity
                {
                    Id = row.GetOptionalString(Entity.IdFieldName),
                    Description = row.GetOptionalString(Entity.DescriptionFieldName),
                    Details = row.GetOptionalString(Entity.DetailsFieldName),
                    EnzymeCommission = row.GetOptionalString(Entity.EnzymeCommissionFieldName),
                    EntitiesPerBiologicalUnit = row.GetOptionalDouble(Entity.EntitiesPerBiologicalUnitFieldName),
                    ExperimentalFormulaWeight = row.GetOptionalDouble(Entity.EntitiesPerBiologicalUnitFieldName),
                    ExperimentalFormulaWeightMethod = row.GetOptionalString(Entity.ExperimentalFormulaWeightMethodFieldName),
                    FormulaWeight = row.GetOptionalDouble(Entity.FormulaWeightFieldName),
                    Fragment = row.GetOptionalString(Entity.FragmentFieldName),
                    Modification = row.GetOptionalString(Entity.ModificationFieldName),
                    Mutation = row.GetOptionalString(Entity.MutationFieldName),
                    NumberOfMolecules = row.GetOptionalInt(Entity.NumberOfMoleculesFieldName),
                    ParentEntityId = row.GetOptionalString(Entity.ParentEntityIdFieldName),
                    SourceMethodRaw = row.GetOptionalString(Entity.SourceMethodRawFieldName),
                    TargetId = row.GetOptionalString(Entity.TargetIdFieldName),
                    TypeRaw = row.GetOptionalString(Entity.TypeRawFieldName)
                };

                if (entity.Type == Entity.EntityType.Polymer)
                {
                    var match = polymers.FirstOrDefault(x => x.EntityId == entity.Id);

                    entity.Polymer = match;
                }

                result.Add(entity);
            }

            return result;
        }

        private static string GetSimpleNamedValue(DataBlock cifDataBlock, string name)
        {
            if (!cifDataBlock.TryGet(name, out DataValueSimple value))
            {
                return null;
            }

            return value.Value;
        }
    }
}
