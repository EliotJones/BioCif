namespace BioCif.Core.Tokenization
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Tokens;

    public static class CifTokenizer
    {
        private const int Underscore = '_';
        private const int Hash = '#';
        private const int Semicolon = ';';
        private const int SingleQuote = '\'';
        private const int DoubleQuote = '"';

        private static readonly Token LoopToken = new Token(TokenType.Loop, "loop_");
        private static readonly Token SaveEndToken = new Token(TokenType.SaveFrameEnd, "save_");

        public static IEnumerable<Token> Tokenize(StreamReader streamReader)
        {
            var sb = new StringBuilder();

            while (Read(streamReader, sb, out var tokenType))
            {
                if (tokenType == TokenType.Loop)
                {
                    yield return LoopToken;
                }
                else if (tokenType == TokenType.SaveFrameEnd)
                {
                    yield return SaveEndToken;
                }
                else
                {
                    yield return new Token(tokenType, sb.ToString());
                }

                sb.Clear();
            }
        }

        private static bool Read(StreamReader sr, StringBuilder sb, out TokenType type)
        {
            type = TokenType.Unknown;

            var val = sr.Read();

            if (val < 0)
            {
                return false;
            }

            if (IsEndline(val) || IsWhitespace(val))
            {
                do
                {
                    val = sr.Read();

                    if (val < 0)
                    {
                        return false;
                    }
                } while (IsEndline(val) || IsWhitespace(val));
            }

            var ctx = GetTokenContext(val, sb);

            if (ctx == Context.Unknown)
            {
                return false;
            }

            var previousPrevious = -1;
            var previous = -1;
            while ((val = sr.Read()) >= 0)
            {
                if (IsEndline(val))
                {
                    if (ctx != Context.ReadingTextField)
                    {
                        type = GetTokenType(ctx, sb);

                        return true;
                    }

                    sb.Append((char) val);
                }
                else if (IsWhitespace(val))
                {
                    switch (ctx)
                    {
                        case Context.ReadingSimpleValue:
                        case Context.ReadingName:
                        case Context.Unknown:
                            type = GetTokenType(ctx, sb);

                            return true;
                        default:
                            sb.Append((char) val);
                            break;
                    }
                }
                else if (ctx == Context.ReadingNonSimpleValueSingleQuote
                         && val == '\''
                         && IsWhitespaceOrEnd(sr.Peek()))
                {
                    type = GetTokenType(ctx, sb);

                    return true;
                }
                else if (ctx == Context.ReadingNonSimpleValueDoubleQuote
                         && val == '"')
                {
                    type = GetTokenType(ctx, sb);

                    return true;
                }
                else if (ctx == Context.ReadingTextField
                         && val == ';'
                         && IsEndline(previous))
                {
                    // Remove the incorrectly included endline characters.
                    if (previousPrevious == '\r' && previous == '\n')
                    {
                        sb.Remove(sb.Length - 2, 2);
                    }
                    else
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }

                    type = GetTokenType(ctx, sb);

                    return true;
                }
                else
                {
                    sb.Append((char) val);
                }

                previousPrevious = previous;
                previous = val;
            }

            if (ctx != Context.Unknown)
            {
                type = GetTokenType(ctx, sb);

                return true;
            }

            return false;
        }

        private static bool IsEndline(int val) => val == '\r' || val == '\n';
        private static bool IsWhitespace(int val) => val == ' ' || val == '\t';
        private static bool IsWhitespaceOrEnd(int val) => val < 0 || IsWhitespace(val);

        private static Context GetTokenContext(int val, StringBuilder sb)
        {
            Context ctx;
            switch (val)
            {
                case Underscore:
                    ctx = Context.ReadingName;
                    break;
                case Hash:
                    ctx = Context.ReadingComment;
                    break;
                case Semicolon:
                    ctx = Context.ReadingTextField;
                    break;
                case SingleQuote:
                    ctx = Context.ReadingNonSimpleValueSingleQuote;
                    break;
                case DoubleQuote:
                    ctx = Context.ReadingNonSimpleValueDoubleQuote;
                    break;
                default:
                    ctx = Context.ReadingSimpleValue;
                    sb.Append((char) val);
                    break;
            }

            return ctx;
        }

        private static TokenType GetTokenType(Context ctx, StringBuilder builder)
        {
            bool FirstMatches(char a, char b)
            {
                return builder[0] == a || builder[0] == b;
            }

            switch (ctx)
            {
                case Context.ReadingComment:
                    return TokenType.Comment;
                case Context.ReadingSimpleValue:
                    {
                        if (builder.Length == 0)
                        {
                            return TokenType.Unknown;
                        }

                        if (builder.Length < 4)
                        {
                            return TokenType.Value;
                        }

                        if (FirstMatches('l', 'L') && CheckContent("loop_", builder, false))
                        {
                            return TokenType.Loop;
                        }

                        if (FirstMatches('d', 'D') && CheckContent("data_", builder, true))
                        {
                            return TokenType.DataBlock;
                        }

                        if (FirstMatches('s', 'S'))
                        {
                            if (CheckContent("save_", builder, true))
                            {
                                return builder.Length == 5 ? TokenType.SaveFrameEnd : TokenType.SaveFrame;
                            }
                        }

                        return TokenType.Value;
                    }
                case Context.ReadingName:
                    return TokenType.Name;
                case Context.ReadingNonSimpleValueSingleQuote:
                case Context.ReadingNonSimpleValueDoubleQuote:
                case Context.ReadingTextField:
                    return TokenType.Value;
                case Context.Unknown:
                default:
                    return TokenType.Unknown;
            }
        }

        private static bool CheckContent(string expected, StringBuilder sb, bool startsWith)
        {
            if (sb.Length < expected.Length)
            {
                return false;
            }

            if (!startsWith && sb.Length != expected.Length)
            {
                return false;
            }

            for (var i = 0; i < expected.Length; i++)
            {
                if (char.ToLowerInvariant(sb[i]) != expected[i])
                {
                    return false;
                }
            }

            return true;
        }

        private enum Context : byte
        {
            Unknown = 0,
            ReadingComment = 1,
            ReadingSimpleValue = 2,
            ReadingName = 3,
            ReadingTextField = 4,
            ReadingNonSimpleValueSingleQuote = 5,
            ReadingNonSimpleValueDoubleQuote = 6,
        }
    }
}
