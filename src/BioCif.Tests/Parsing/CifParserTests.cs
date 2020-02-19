namespace BioCif.Tests.Parsing
{
    using System.IO;
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

            Assert.True(block.TryGet("dict", out DataDictionary dict));
            Assert.Equal(3, dict.Count);
            Assert.Equal("1", (DataValueSimple)dict["count"]);
            Assert.Equal(" any", (DataValueSimple)dict["name"]);

            var vectors = Assert.IsType<DataList>(dict["vectors"]);

            Assert.Equal(5, vectors.Count);

            var inner = Assert.IsType<DataList>(vectors[3]);

            Assert.Equal(3, inner.Count);

            Assert.True(block.TryGet("list", out DataList list));
            Assert.Equal(3, list.Count);
            AssertNamedValue("simple", "simple_value", block);
        }

        [Fact]
        public void BlockContainingNestedDictionary()
        {
            const string input = @"data_block
_dict {
    'name': 'my top level'
    'inner': {
        'a': 1
        'b': [ 1 5 7 ]
    }
    'result': pass
}
_simple value";

            var cif = Parse(input);

            var block = Assert.Single(cif.DataBlocks);
            Assert.NotNull(block);

            Assert.True(block.TryGet("dict", out DataDictionary dict));

            Assert.Equal("my top level", (DataValueSimple)dict["name"]);
            Assert.True(dict.TryGetValue("inner", out var innerAny));
            var inner = (DataDictionary) innerAny;

            Assert.Equal("1", (DataValueSimple)inner["a"]);
            var bList = Assert.IsType<DataList>(inner["b"]);
            Assert.Equal(3, bList.Count);

            Assert.Equal("pass", (DataValueSimple)dict["result"]);

            AssertNamedValue("simple", "value", block);
        }

        [Fact]
        public void BlockContainingNestedNestedDictionaries()
        {
            const string input = @"data_nested
_top {
    'level 1': {
        'item 1': happy
        'level 2': {
            'item 2': '''a text'''
            'list': [ a b {
                        'status': 'in list'
                    } ]
            'level 3': { 'item 3': 'babble' }
        }
    }
    'any': 'cabbage'
}
_normalized yes";

            var cif = Parse(input);

            var block = Assert.Single(cif.DataBlocks);
            Assert.NotNull(block);

            Assert.Equal(2, block.Count);
            AssertNamedValue("normalized", "yes", block);

            Assert.True(block.TryGet("top", out DataDictionary top));

            Assert.True(top.TryGetValue("level 1", out var level1Any));

            var level1 = Assert.IsType<DataDictionary>(level1Any);

            Assert.Equal("happy", (DataValueSimple)level1["item 1"]);

            Assert.True(level1.TryGetValue("level 2", out var level2Any));

            var level2 = Assert.IsType<DataDictionary>(level2Any);

            Assert.Equal(3, level2.Count);

            Assert.Equal("a text", (DataValueSimple)level2["item 2"]);

            var list = Assert.IsType<DataList>(level2["list"]);

            Assert.Equal(3, list.Count);

            var listDict = Assert.IsType<DataDictionary>(list[2]);

            Assert.Equal("in list", (DataValueSimple)listDict["status"]);

            var level3 = Assert.IsType<DataDictionary>(level2["level 3"]);

            Assert.Equal("babble", (DataValueSimple)level3["item 3"]);

            Assert.Equal("cabbage", (DataValueSimple)top["any"]);
        }

        [Fact]
        public void ParsesVanadiumHypophosphiteCifFile()
        {
            using (var fs = File.OpenRead(GetIntegrationDocumentFilePath("1000118.cif")))
            {
                var cif = CifParser.Parse(fs);

                var block = Assert.Single(cif.DataBlocks);
                Assert.NotNull(block);

                AssertNamedValue("publ_section_title", "\nAb initio crystal structure determination of V O (H2 P O2) .(H2 O) from\n" +
                             "X-ray and neutron powder diffraction data. A monodimensional\n" +
                             "vanadium(IV) hypophosphite", block);

                AssertNamedValue("symmetry_cell_setting", "monoclinic", block);
                AssertNamedValue("symmetry_space_group_name_Hall", "-C 2yc", block);
            }
        }

        [Fact]
        public void ParsesVanadiumHypophosphiteCifLoop()
        {
            using (var fs = File.OpenRead(GetIntegrationDocumentFilePath("1000118.cif")))
            {
                var cif = CifParser.Parse(fs);

                var block = Assert.Single(cif.DataBlocks);
                Assert.NotNull(block);

                var loop = Assert.IsType<Table>(block[0]);

                var name = Assert.Single(loop.Headers);
                Assert.Equal("publ_author_name", name);

                Assert.Equal(3, loop.Rows.Count);

                Assert.Equal(new[]
                {
                    "Le Bail, A",
                    "Marcos, M D",
                    "Amoros, P"
                }, loop.Rows.Select(x => (x[0] as DataValueSimple)?.Value));
            }
        }

        [Fact]
        public void ParsesMsp1SubstrateComplexCifFile()
        {
            using (var fs = File.OpenRead(GetIntegrationDocumentFilePath("6pdw.cif")))
            {
                var cif = CifParser.Parse(fs);

                var block = Assert.Single(cif.DataBlocks);
                Assert.NotNull(block);

                AssertNamedValue("entry.id", "6PDW", block);
                AssertNamedValue("pdbx_database_related.details", "Msp1-substrate complex in closed conformation", block);
                AssertNamedValue("citation.title", "Structure of the AAA protein Msp1 reveals mechanism of mislocalized membrane protein extraction.", block);
                AssertNamedValue("citation.year", "2020", block);
                AssertNamedValue("citation.pdbx_database_id_DOI", "10.7554/eLife.54031", block);
                AssertNamedValue("pdbx_struct_assembly_auth_evidence.experimental_support", "gel filtration", block);
            }
        }

        [Fact]
        public void ParsePhosphineSulfonatePalladiumNickelCatalyzedEthylenePolymerizationCif()
        {
            using (var fs = File.OpenRead(GetIntegrationDocumentFilePath("1552310.cif")))
            {
                var cif = CifParser.Parse(fs);

                var block = Assert.Single(cif.DataBlocks);
                Assert.NotNull(block);

                AssertNamedValue("publ_section_title", "\n Ligand--metal secondary interactions in phosphine--sulfonate palladium\n and nickel catalyzed ethylene (co)polymerization", block);
                AssertNamedValue("audit_creation_method", "\nOlex2 1.2\n(compiled 2018.05.29 svn.r3508 for OlexSys, GUI svn.r5506)", block);

                Assert.True(block.TryGet("platon_squeeze_details", out DataValueSimple platon));
                Assert.EndsWith("Q199 0.139 0.473 0.234 !       0.87 eA-3", platon);
            }
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

        private static void AssertNamedValue(string name, string value, DataBlock block)
        {
            Assert.True(block.TryGet(name, out DataValueSimple val));
            Assert.Equal(value, val);
        }
    }
}
