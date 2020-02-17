namespace BioCif.Tests.Parsing
{
    using System.Linq;
    using System.Text;
    using Core;
    using Core.Parsing;
    using Xunit;
    using static TestHelpers;

    public class CifParserTests
    {
        [Fact]
        public void BasicBlockWithItemsOnly()
        {
            const string input = @"data_anything
_journal_coden_ASTM              INOCAJ
_journal_name_full               'Inorganic Chemistry'
_journal_page_first              2607";

            var cif = Parse(input);

            var block = Assert.Single(cif.DataBlocks);

            Assert.NotNull(block);

            Assert.Equal("anything", block.Name);

            Assert.Equal(3, block.Contents.Count);

            AssertSimpleItem("journal_coden_ASTM", "INOCAJ", block.Contents[0]);
            AssertSimpleItem("journal_name_full", "Inorganic Chemistry", block.Contents[1]);
            AssertSimpleItem("journal_page_first", "2607", block.Contents[2]);
        }

        [Fact]
        public void BasicBlockContainingLoop()
        {
            const string input = @"# All data on this site have been placed in the public domain by the
# contributors.
#
data_1000118
loop_
_publ_author_name
'Le Bail, A'
'Marcos, M D'
'Amoros, P'";

            var cif = Parse(input);

            var block = Assert.Single(cif.DataBlocks);

            Assert.NotNull(block);

            var content = Assert.Single(block.Contents);

            var loop = Assert.IsType<Table>(content);

            Assert.Equal(new[]{ "publ_author_name" }, loop.Headers.Select(x => x.Tag));
            Assert.Equal(new[] { "Le Bail, A", "Marcos, M D", "Amoros, P" }, loop.Rows.Select(x => ((DataValue)x[0]).Value));
        }

        [Fact]
        public void TwoBasicBlocks()
        {
            const string input = @"data_a
_my_data 10.678
_other 'happy feet'
data_b
_my_Data 12.765
#  a comment
_other 'angry penguins'";

            var cif = Parse(input);

            Assert.Equal(2, cif.DataBlocks.Count);

            var block1 = cif.DataBlocks[0];
            
            Assert.Equal("a", block1.Name);
            AssertSimpleItem("my_data", "10.678", block1.Contents[0]);
            AssertSimpleItem("other", "happy feet", block1.Contents[1]);
                
            var block2 = cif.DataBlocks[1];

            Assert.Equal("b", block2.Name);
            AssertSimpleItem("my_Data", "12.765", block2.Contents[0]);
            AssertSimpleItem("other", "angry penguins", block2.Contents[1]);
        }

        [Fact]
        public void SimpleBlockContainingList()
        {
            const string input = @"# hello
data_clown
_clown_names [larry gary 'barry, also']
_count 3";

            var cif = Parse(input);

            var block = Assert.Single(cif.DataBlocks);

            Assert.NotNull(block);

            Assert.Equal("clown", block.Name);

            var list = (DataItem) block.Contents[0];
            Assert.Equal("clown_names", list.Name);
            var val = Assert.IsType<DataList>(list.Value);
            Assert.NotNull(val);
            Assert.Equal(new[]{ "larry", "gary", "barry, also" }, val.Values.Select(x => ((DataValue)x).Value));

            AssertSimpleItem("count", "3", block.Contents[1]);
        }

        private static Cif Parse(string input)
        {
            using (var sr = StringToStream(input))
            {
                var result = CifParser.Parse(sr, new CifParsingOptions
                {
                    FileEncoding = Encoding.Unicode
                });

                return result;
            }
        }

        private static void AssertSimpleItem(string name, string value, IDataBlockMember member)
        {
            var simple = Assert.IsType<DataItem>(member);
            Assert.NotNull(simple);

            Assert.Equal(name, simple.Name);

            var val = Assert.IsType<DataValue>(simple.Value);
            Assert.NotNull(val);

            Assert.Equal(value, val.Value);
        }
    }
}
