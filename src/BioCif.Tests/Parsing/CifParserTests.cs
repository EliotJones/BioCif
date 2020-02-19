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

            Assert.Equal(3, block.Count);

            AssertSimpleItem("journal_coden_ASTM", "INOCAJ", block[0]);
            AssertSimpleItem("journal_name_full", "Inorganic Chemistry", block[1]);
            AssertSimpleItem("journal_page_first", "2607", block[2]);
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

            var content = Assert.Single(block);

            var loop = Assert.IsType<Table>(content);

            Assert.Equal(new[]{ "publ_author_name" }, loop.Headers.Select(x => x.Tag));
            Assert.Equal(new[] { "Le Bail, A", "Marcos, M D", "Amoros, P" }, loop.Rows.Select(x => ((DataValueSimple)x[0]).Value));
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
            AssertSimpleItem("my_data", "10.678", block1[0]);
            AssertSimpleItem("other", "happy feet", block1[1]);
                
            var block2 = cif.DataBlocks[1];

            Assert.Equal("b", block2.Name);
            AssertSimpleItem("my_Data", "12.765", block2[0]);
            AssertSimpleItem("other", "angry penguins", block2[1]);
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

            var list = (DataItem) block[0];
            Assert.Equal("clown_names", list.Name);
            var val = Assert.IsType<DataList>(list.Value);
            Assert.NotNull(val);
            Assert.Equal(new[]{ "larry", "gary", "barry, also" }, val.Select(x => ((DataValueSimple)x).Value));

            AssertSimpleItem("count", "3", block[1]);
        }

        [Fact]
        public void SimpleBlockContainingNestedList()
        {
            const string input = @"data_a
_clown_names ['mr giggles' [brian happy]]";

            var cif = Parse(input);

            var block = Assert.Single(cif.DataBlocks);

            Assert.NotNull(block);
            Assert.Equal("a", block.Name);
            var item = (DataItem) block[0];
            Assert.Equal("clown_names", item.Name);
            var list = (DataList) item.Value;

            AssertValue("mr giggles", list[0]);

            var inner = Assert.IsType<DataList>(list[1]);

            Assert.Equal(2, inner.Count);

            AssertValue("brian", inner[0]);
            AssertValue("happy", inner[1]);
        }

        [Fact]
        public void SimpleBlockContainingDictionaryAndList()
        {
            const string input = @"
data_one
_dict { 
    'count': 1
    ""vectors"": [1 0 0.67 [0 1 0.5] -1]
    'name': ''' any'''
}
_list [3 5 7]
_simple simple_value";

            var cif = Parse(input);

            var block = Assert.Single(cif.DataBlocks);
            Assert.NotNull(block);

            Assert.Equal(3, block.Count);
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

            AssertValue(value, simple.Value);
        }

        private static void AssertValue(string expected, IDataValue value)
        {
            var val = Assert.IsType<DataValueSimple>(value);
            Assert.NotNull(val);

            Assert.Equal(expected, val.Value);
        }
    }
}
