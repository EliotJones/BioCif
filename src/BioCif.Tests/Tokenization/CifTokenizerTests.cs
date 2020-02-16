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
        public void Test1()
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
            }
        }
    }
}
