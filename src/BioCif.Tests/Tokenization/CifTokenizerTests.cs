namespace BioCif.Tests.Tokenization
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Core.Tokenization;
    using Core.Tokenization.Tokens;
    using Xunit;
    using static TestHelpers;

    public class CifTokenizerTests
    {
        [Fact]
        public void SimpleFourLineInput()
        {
            const string input = @"data_1000118
loop_
_publ_author_name
'Le Bail, A'";

            var tokens = StringToTokens(input);

            Assert.Equal(4, tokens.Count);
            Assert.Equal(new[] { TokenType.DataBlock, TokenType.Loop, TokenType.Name, TokenType.Value },
                tokens.Select(x => x.TokenType));

            Assert.Equal(new[] { "data_1000118", "loop_", "publ_author_name", "Le Bail, A" }, tokens.Select(x => x.Value));
        }

        [Fact]
        public void SimpleTextFieldInput()
        {
            const string input = @"_entity_poly.pdbx_target_identifier 
1 'polypeptide(L)' no no 
;GSIAPYLVKIIDPDYEKNERTRIKAQENLRRIRRKQIAEKGDNEDGTDDPSRRRKIDDLVLNEYENQVALEVVAPEDIPV
GFNDIGGLDDIIEELKETIIYPLTMPHLYKHGGALLAAPSGVLLYGPPGCGKTMLAKAVAHESGASFINLHISTLTEKWY
GDSNKIVRAVFSLAKKLQPSIIFIDEIDAVLGTRRSGEHEASGMVKAEFMTLWDGLTSTNASGVPNRIVVLGATNRINDI
DEAILRRMPKQFPVPLPGLEQRRRILELVLRGTKRDPDFDLDYIARVTAGMSGSDIKETCRDAAMAPMREYIRQHRASGK
PLSEINPDDVRGIRTEDFFGRRGGKILSEIPPRQTGYVVQSKNSSEGGYEEVEDDDEQGTAST
;
'a dog's life'";

            const string expectedPeptideSequence = @"GSIAPYLVKIIDPDYEKNERTRIKAQENLRRIRRKQIAEKGDNEDGTDDPSRRRKIDDLVLNEYENQVALEVVAPEDIPV
GFNDIGGLDDIIEELKETIIYPLTMPHLYKHGGALLAAPSGVLLYGPPGCGKTMLAKAVAHESGASFINLHISTLTEKWY
GDSNKIVRAVFSLAKKLQPSIIFIDEIDAVLGTRRSGEHEASGMVKAEFMTLWDGLTSTNASGVPNRIVVLGATNRINDI
DEAILRRMPKQFPVPLPGLEQRRRILELVLRGTKRDPDFDLDYIARVTAGMSGSDIKETCRDAAMAPMREYIRQHRASGK
PLSEINPDDVRGIRTEDFFGRRGGKILSEIPPRQTGYVVQSKNSSEGGYEEVEDDDEQGTAST";

            var tokens = StringToTokens(input);

            Assert.Equal(7, tokens.Count);

            Assert.Equal(new[]
            {
                    TokenType.Name, TokenType.Value, TokenType.Value, TokenType.Value, TokenType.Value,
                    TokenType.Value, TokenType.Value
                }, tokens.Select(x => x.TokenType));

            Assert.Equal(new[]
            {
                    "entity_poly.pdbx_target_identifier",
                    "1",
                    "polypeptide(L)",
                    "no",
                    "no",
                    expectedPeptideSequence,
                    "a dog's life"
                }, tokens.Select(x => x.Value));
        }

        [Fact]
        public void SimpleCommentInput()
        {
            const string input = @"#------------------------------------------------------------------------------
##$Date: 2015-01-27 21:58:39 +0200 (Tue, 27 Jan 2015) $
#$Revision: 130149 $
#$URL: svn://www.crystallography.net/cod/cif/1/00/01/1000118.cif $
x,y,z";

            var tokens = StringToTokens(input);

            Assert.Equal(5, tokens.Count);

            Assert.Equal(new[]
            {
                    TokenType.Comment,
                    TokenType.Comment,
                    TokenType.Comment,
                    TokenType.Comment,
                    TokenType.Value
                }, tokens.Select(x => x.TokenType));

            Assert.Equal(new[]
            {
                    "------------------------------------------------------------------------------",
                    "#$Date: 2015-01-27 21:58:39 +0200 (Tue, 27 Jan 2015) $",
                    "$Revision: 130149 $",
                    "$URL: svn://www.crystallography.net/cod/cif/1/00/01/1000118.cif $",
                    "x,y,z"
                }, tokens.Select(x => x.Value));
        }

        [Fact]
        public void SimpleMultilineSpecificationInput()
        {
            const string input1 = ";foo\n;";
            const string input2 = ";foo\n  bar\n;";

            var tokens1 = StringToTokens(input1);
            var tokens2 = StringToTokens(input2);

            Assert.Single(tokens1);
            Assert.Single(tokens2);

            Assert.Equal("foo", tokens1[0].Value);
            Assert.Equal("foo\n  bar", tokens2[0].Value);
        }

        [Fact]
        public void SimpleSaveFrameEndInput()
        {
            const string input = @"    _item_type_conditions.code    esd
    _item_units.code              8pi2_angstroms_squared
     save_";

            var tokens = StringToTokens(input);

            Assert.Equal(5, tokens.Count);
            Assert.Equal(new[]
            {
                TokenType.Name, TokenType.Value,
                TokenType.Name, TokenType.Value,
                TokenType.SaveFrameEnd
            }, tokens.Select(x => x.TokenType));

            Assert.Equal(new[]
            {
                "item_type_conditions.code", "esd",
                "item_units.code", "8pi2_angstroms_squared",
                "save_"
            }, tokens.Select(x => x.Value));
        }

        [Fact]
        public void TokenizesVanadiumHypophosphiteCifFile()
        {
            using (var fs = File.OpenRead(GetIntegrationDocumentFilePath("1000118.cif")))
            using (var sr = new StreamReader(fs, true))
            {
                var tokens = CifTokenizer.Tokenize(sr).ToList();

                Assert.NotEmpty(tokens);
                Assert.Equal(TokenType.Comment, tokens[0].TokenType);
                Assert.Equal("------------------------------------------------------------------------------", tokens[0].Value);

                var last = tokens.Last();
                Assert.Equal(TokenType.Value, last.TokenType);
                Assert.Equal("1.000", last.Value);
            }
        }

        private static IReadOnlyList<Token> StringToTokens(string input)
        {
            using (var reader = StringToStreamReader(input))
            {
                return CifTokenizer.Tokenize(reader).ToList();
            }
        }
    }
}
