namespace BioCif
{
    using System.Collections.Generic;

    /// <summary>
    /// Details about <see cref="Entity"/>s which are polymers (their <see cref="Entity.Type"/> is <see cref="Entity.EntityType.Polymer"/>).
    /// </summary>
    public class EntityPolymer
    {
        /// <summary>
        /// The id of the corresponding <see cref="Entity"/>.
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// A flag to indicate whether the polymer contains at least one monomer that is not considered standard.
        /// </summary>
        public bool NonStandardMonomer { get; set; }

        /// <summary>
        /// A flag to indicate whether the polymer contains at least one monomer-to-monomer link different from that implied by the <see cref="Type"/>. 
        /// </summary>
        public bool NonStandardLinkage { get; set; }

        /// <summary>
        /// Chemical sequence expressed as string of one-letter amino acid codes. Modifications and non-standard amino acids are coded as X.
        /// </summary>
        public string SequenceOneLetterCode { get; set; }

        /// <summary>
        /// The type of the polymer.
        /// </summary>
        public string TypeRaw { get; set; }

        /// <summary>
        /// The <see cref="TypeRaw"/> mapped to the <see cref="PolymerType"/> enum.
        /// </summary>
        public PolymerType Type
        {
            get
            {
                switch (TypeRaw?.ToLowerInvariant())
                {
                    case "cyclic-pseudo-peptide":
                        return PolymerType.CyclicPseudoPeptide;
                    case "other":
                        return PolymerType.Other;
                    case "peptide nucleic acid":
                        return PolymerType.PeptideNucleicAcid;
                    case "polydeoxyribonucleotide":
                        return PolymerType.Polydeoxyribonucleotide;
                    case "polydeoxyribonucleotide/polyribonucleotide hybrid":
                        return PolymerType.PolydeoxyribonucleotidePolyribonucleotideHybrid;
                    case "polypeptide(D)":
                        return PolymerType.PolypeptideD;
                    case "polypeptide(L)":
                        return PolymerType.PolypeptideL;
                    case "polyribonucleotide":
                        return PolymerType.Polyribonucleotide;
                    default:
                        return PolymerType.Unknown;
                }
            }
        }

        /// <summary>
        /// Specifies the sequence of monomers in a polymer.
        /// Allowance is made for the possibility of microheterogeneity in a sample by allowing a given sequence number to be correlated with more than one monomer ID.
        /// </summary>
        public List<PolymerSequenceItem> Sequence { get; } = new List<PolymerSequenceItem>();
 
        /// <summary>
        /// The type of polymer.
        /// </summary>
        public enum PolymerType
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            Unknown = 0,
            /// <summary>
            /// Cyclic-pseudo-peptide.
            /// </summary>
            CyclicPseudoPeptide = 1,
            /// <summary>
            /// Other.
            /// </summary>
            Other = 2,
            /// <summary>
            /// Peptide nucleic acid
            /// </summary>
            PeptideNucleicAcid = 3,
            /// <summary>
            /// Polydeoxyribonucleotide.
            /// </summary>
            Polydeoxyribonucleotide = 4,
            /// <summary>
            /// Polydeoxyribonucleotide/polyribonucleotide hybrid.
            /// </summary>
            PolydeoxyribonucleotidePolyribonucleotideHybrid = 5,
            /// <summary>
            /// Polypeptide(D).
            /// </summary>
            PolypeptideD = 6,
            /// <summary>
            /// Polypeptide(L).
            /// </summary>
            PolypeptideL = 7,
            /// <summary>
            /// Polyribonucleotide.
            /// </summary>
            Polyribonucleotide = 8
        }
    }
}