namespace BioCif.Tests.Tokenization
{
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

            using (var sr = StringToStreamReader(input))
            {
                var tokens = CifTokenizer.Tokenize(sr).ToList();

                Assert.Equal(4, tokens.Count);
                Assert.Equal(new[] { TokenType.DataBlock, TokenType.Loop, TokenType.Name, TokenType.Value },
                    tokens.Select(x => x.TokenType));

                Assert.Equal(new[]{ "data_1000118", "loop_", "publ_author_name", "Le Bail, A" }, tokens.Select(x => x.Value));
            }
        }

        [Fact]
        public void TextFieldInput()
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

            using (var sr = StringToStreamReader(input))
            {
                var tokens = CifTokenizer.Tokenize(sr).ToList();

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
        }
    }
}
