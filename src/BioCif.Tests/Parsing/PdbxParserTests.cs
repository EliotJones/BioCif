namespace BioCif.Tests.Parsing
{
    using System.Linq;
    using Xunit;
    using static TestHelpers;

    public class PdbxParserTests
    {
        [Fact]
        public void ParsesMsp1SubstrateComplexClosedConformation()
        {
            var pdbx = PdbxParser.ParseFile(GetIntegrationDocumentFilePath("6pdw.cif"));

            var block = Assert.Single(pdbx.DataBlocks);
            Assert.NotNull(block);
            Assert.NotNull(pdbx.First);

            Assert.Equal("6PDW", block.EntryId);
            Assert.Equal(4, block.AuditAuthors.Count);

            Assert.Equal(new[]
            {
                1, 2, 3, 4
            }, block.AuditAuthors.Select(x => x.Ordinal));

            Assert.Equal(new[]
            {
                "Wang, L.", "Myasnikov, A.", "Pan, X.", "Walter, P."
            }, block.AuditAuthors.Select(x => x.Name));

            Assert.NotNull(block.Raw);

            Assert.NotNull(block.Symmetry);

            Assert.Equal("6PDW", block.Symmetry.EntryId);
            Assert.Null(block.Symmetry.CellSettingRaw);
            Assert.Equal(1, block.Symmetry.TablesNumber);
            Assert.Null(block.Symmetry.FullSpaceGroupNameHM);
            Assert.Null(block.Symmetry.SpaceGroupNameHall);
            Assert.Equal("P 1", block.Symmetry.SpaceGroupNameHM);

            Assert.Equal(5, block.Entities.Count);

            var first = block.Entities[0];
            Assert.Equal(Entity.EntityType.Polymer, first.Type);
            Assert.Equal(42599.152, first.FormulaWeight);

            Assert.NotNull(first.Polymer);
            Assert.Equal(@"GSIAPYLVKIIDPDYEKNERTRIKAQENLRRIRRKQIAEKGDNEDGTDDPSRRRKIDDLVLNEYENQVALEVVAPEDIPV" +
                         "GFNDIGGLDDIIEELKETIIYPLTMPHLYKHGGALLAAPSGVLLYGPPGCGKTMLAKAVAHESGASFINLHISTLTEKWY" +
                         "GDSNKIVRAVFSLAKKLQPSIIFIDEIDAVLGTRRSGEHEASGMVKAEFMTLWDGLTSTNASGVPNRIVVLGATNRINDI" +
                         "DEAILRRMPKQFPVPLPGLEQRRRILELVLRGTKRDPDFDLDYIARVTAGMSGSDIKETCRDAAMAPMREYIRQHRASGK" +
                         "PLSEINPDDVRGIRTEDFFGRRGGKILSEIPPRQTGYVVQSKNSSEGGYEEVEDDDEQGTAST", first.Polymer.SequenceOneLetterCode);
            Assert.Equal(EntityPolymer.PolymerType.PolypeptideL, first.Polymer.Type);
            Assert.Equal("B,C,D,E,F", first.Polymer.StrandId);

            Assert.Equal(383, first.Polymer.Sequence.Count);
            Assert.Equal("GLY", first.Polymer.Sequence[0].ChemicalComponentId);
            Assert.False(first.Polymer.Sequence[0].Heterogeneous);
            Assert.Equal("ARG", first.Polymer.Sequence[52].ChemicalComponentId);
            Assert.False(first.Polymer.Sequence[52].Heterogeneous);

            Assert.NotNull(block.Entities[1].Polymer);
            Assert.Null(block.Entities[2].Polymer);
        }
    }
}
