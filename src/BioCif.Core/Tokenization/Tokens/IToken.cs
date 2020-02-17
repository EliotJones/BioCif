namespace BioCif.Core.Tokenization.Tokens
{
    public class Token
    {
        public TokenType TokenType { get; }

        public string Value { get; }

        public Token(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public override string ToString()
        {
            return $"({TokenType}) {Value}";
        }
    }

    public enum TokenType : byte
    {
        Unknown = 0,
        Name = 1,
        Value = 2,
        DataBlock = 3,
        SaveFrame = 4,
        SaveFrameEnd = 5,
        Loop = 6,
        Comment = 7,
        StartList = 8,
        EndList = 9,
        StartTable = 10,
        EndTable = 11,
    }
}
