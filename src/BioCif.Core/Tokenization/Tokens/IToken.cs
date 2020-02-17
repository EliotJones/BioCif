namespace BioCif.Core.Tokenization.Tokens
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A token from a CIF file.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// The meaning of this token.
        /// </summary>
        public TokenType TokenType { get; }

        /// <summary>
        /// The text of this token.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Create a new <see cref="Token"/>.
        /// </summary>
        public Token(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is Token token &&
                   TokenType == token.TokenType &&
                   Value == token.Value;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = 2008211804;
            hashCode = hashCode * -1521134295 + TokenType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Value);
            return hashCode;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"({TokenType}) {Value}";
        }
    }
}
