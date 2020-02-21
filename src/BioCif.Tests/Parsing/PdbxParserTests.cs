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
        }
    }
}
