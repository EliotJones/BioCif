namespace BioCif.Core.Tokenization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Tokens;

    /// <summary>
    /// Parse a file using the Crystallographic Information File (CIF) format.
    /// The specification of the CIF format can be found here: https://www.iucr.org/resources/cif/spec.
    /// </summary>
    public static class CifTokenizer
    {
        private const int Underscore = '_';
        private const int Hash = '#';
        private const int Semicolon = ';';
        private const int SingleQuote = '\'';
        private const int DoubleQuote = '"';
        private const int OpenSquareBracket = '[';
        private const int CloseSquareBracket = ']';
        private const int OpenCurlyBracket = '{';
        private const int CloseCurlyBracket = '}';

        private static readonly Token LoopToken = new Token(TokenType.Loop, "loop_");
        private static readonly Token SaveEndToken = new Token(TokenType.SaveFrameEnd, "save_");
        private static readonly Token StartListToken = new Token(TokenType.StartList, "[");
        private static readonly Token EndListToken = new Token(TokenType.EndList, "]");
        private static readonly Token StartTableToken = new Token(TokenType.StartTable, "{");
        private static readonly Token EndTableToken = new Token(TokenType.EndTable, "}");

        /// <summary>
        /// Yields the set of tokens contained in the input CIF format stream.
        /// </summary>
        public static IEnumerable<Token> Tokenize(StreamReader streamReader, CifFileVersion cifFileVersion = CifFileVersion.Version2)
        {
            var sb = new StringBuilder();

            var scopes = new Stack<ActiveScope>(new[] { ActiveScope.BlockOrFrame });

            while (Read(streamReader, sb, cifFileVersion, scopes.Peek(), out var tokenType, out var completeScope))
            {
                if (tokenType == TokenType.Loop)
                {
                    yield return LoopToken;
                }
                else if (tokenType == TokenType.SaveFrameEnd)
                {
                    yield return SaveEndToken;
                }
                else if (tokenType == TokenType.StartList)
                {
                    yield return StartListToken;
                    scopes.Push(ActiveScope.List);
                }
                else if (tokenType == TokenType.EndList)
                {
                    yield return EndListToken;
                    scopes.Pop();
                }
                else if (tokenType == TokenType.StartTable)
                {
                    yield return StartTableToken;
                    scopes.Push(ActiveScope.Table);
                }
                else if (tokenType == TokenType.EndTable)
                {
                    yield return EndTableToken;
                    scopes.Pop();
                }
                else
                {
                    yield return new Token(tokenType, sb.ToString());

                    if (completeScope)
                    {
                        var completed = scopes.Pop();

                        switch (completed)
                        {
                            case ActiveScope.List:
                                yield return EndListToken;
                                break;
                            case ActiveScope.Table:
                                yield return EndTableToken;
                                break;
                        }
                    }
                }

                sb.Clear();
            }
        }

        private static bool Read(StreamReader sr, StringBuilder sb, CifFileVersion cifFileVersion, ActiveScope scope, out TokenType type,
            out bool completeScope)
        {
            completeScope = false;
            type = TokenType.Unknown;

            var val = sr.Read();

            if (val < 0)
            {
                return false;
            }

            if (val == ':' && scope == ActiveScope.Table)
            {
                val = sr.Read();

                if (val < 0)
                {
                    return false;
                }
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

            var ctx = GetTokenContext(val, sb, cifFileVersion);

            switch (ctx)
            {
                case Context.Unknown:
                    return false;
                case Context.OpenList:
                    type = TokenType.StartList;
                    return true;
                case Context.CloseList:
                    type = TokenType.EndList;
                    return true;
                case Context.OpenTable:
                    type = TokenType.StartTable;
                    return true;
                case Context.CloseTable:
                    type = TokenType.EndTable;
                    return true;
            }

            var previousPrevious = -1;
            var previous = val;
            while ((val = sr.Read()) >= 0)
            {
                if (val == CloseSquareBracket && scope == ActiveScope.List)
                {
                    completeScope = true;

                    type = GetTokenType(ctx, sb);

                    return true;
                }

                if (val == CloseCurlyBracket && scope == ActiveScope.Table)
                {
                    completeScope = true;
                    type = GetTokenType(ctx, sb);
                    return true;
                }

                if (IsEndline(val))
                {
                    if (ctx != Context.ReadingTextField
                        && ctx != Context.ReadingNonSimpleValueTripleSingleQuoteCif2
                        && ctx != Context.ReadingNonSimpleValueTripleDoubleQuoteCif2)
                    {
                        type = GetTokenType(ctx, sb);

                        return true;
                    }

                    sb.Append((char)val);
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
                            sb.Append((char)val);
                            break;
                    }
                }
                else if (ctx == Context.ReadingNonSimpleValueSingleQuote
                         && cifFileVersion == CifFileVersion.Version2
                         && previousPrevious == '\''
                         && previous == '\''
                         && val == '\'')
                {
                    sb.Clear();
                    ctx = Context.ReadingNonSimpleValueTripleSingleQuoteCif2;
                }
                else if (ctx == Context.ReadingNonSimpleValueSingleQuote
                         && val == '\''
                         && (IsWhitespaceOrEndFileOrLine(sr.Peek()) || (scope == ActiveScope.Table && sr.Peek() == ':')))
                {
                    type = GetTokenType(ctx, sb);

                    return true;
                }
                else if (ctx == Context.ReadingNonSimpleValueSingleQuote
                         && val == '\''
                         && scope == ActiveScope.List
                         && sr.Peek() == ']')
                {
                    type = GetTokenType(ctx, sb);

                    return true;
                }
                else if (ctx == Context.ReadingNonSimpleValueTripleSingleQuoteCif2
                         && previousPrevious == '\''
                         && previous == '\''
                         && val == '\'')
                {
                    sb.Remove(sb.Length - 2, 2);

                    type = GetTokenType(ctx, sb);

                    return true;
                }
                else if (ctx == Context.ReadingNonSimpleValueDoubleQuote
                         && cifFileVersion == CifFileVersion.Version2
                         && previousPrevious == '"'
                         && previous == '"'
                         && val == '"')
                {
                    sb.Clear();
                    ctx = Context.ReadingNonSimpleValueTripleDoubleQuoteCif2;
                }
                else if (ctx == Context.ReadingNonSimpleValueDoubleQuote
                         && val == '"'
                         && (IsWhitespaceOrEndFileOrLine(sr.Peek()) || (scope == ActiveScope.Table && sr.Peek() == ':')))
                {
                    type = GetTokenType(ctx, sb);

                    return true;
                }
                else if (ctx == Context.ReadingNonSimpleValueDoubleQuote
                         && val == '"'
                         && scope == ActiveScope.List
                         && sr.Peek() == ']')
                {
                    type = GetTokenType(ctx, sb);

                    return true;
                }
                else if (ctx == Context.ReadingNonSimpleValueTripleDoubleQuoteCif2
                         && previousPrevious == '"'
                         && previous == '"'
                         && val == '"')
                {
                    sb.Remove(sb.Length - 2, 2);

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
                    sb.Append((char)val);
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

        /// <summary>
        /// For end-of-line, CIF recognises three distinct character sequences: 
        /// (1) a line feed (U+000A) not immediately preceded by a carriage return (U+000D).
        /// (2) a carriage return not immediately followed by a line feed.
        /// (3) a carriage-return/line-feed pair. 
        /// </summary>
        private static bool IsEndline(int val) => val == '\r' || val == '\n';
        /// <summary>
        /// The horizontal tab character (U+0009) and the space character (U+0020) alone are recognized as in-line whitespace characters.
        /// </summary>
        private static bool IsWhitespace(int val) => val == ' ' || val == '\t';
        /// <summary>
        /// Checks if the character is whitespace, end-of-line or the end of the file.
        /// </summary>
        private static bool IsWhitespaceOrEndFileOrLine(int val) => val < 0 || IsWhitespace(val) || IsEndline(val);

        private static Context GetTokenContext(int val, StringBuilder sb, CifFileVersion cifFileVersion)
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
                case OpenSquareBracket:
                    {
                        if (cifFileVersion == CifFileVersion.Version1_1)
                        {
                            throw new InvalidOperationException("Encountered a square bracket '[' at the start of a name or value when parsing CIF version 1 or 1.1.");
                        }
                        ctx = Context.OpenList;
                        break;
                    }
                case OpenCurlyBracket:
                    {
                        if (cifFileVersion == CifFileVersion.Version1_1)
                        {
                            throw new InvalidOperationException("Encountered a curly bracket '{' at the start of a name or value when parsing CIF version 1 or 1.1.");
                        }
                        ctx = Context.OpenTable;
                        break;
                    }
                case CloseSquareBracket:
                    ctx = Context.CloseList;
                    break;
                case CloseCurlyBracket:
                    ctx = Context.CloseTable;
                    break;
                default:
                    ctx = Context.ReadingSimpleValue;
                    sb.Append((char)val);
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
                case Context.ReadingNonSimpleValueTripleSingleQuoteCif2:
                case Context.ReadingNonSimpleValueDoubleQuote:
                case Context.ReadingNonSimpleValueTripleDoubleQuoteCif2:
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
            ReadingNonSimpleValueTripleSingleQuoteCif2 = 7,
            ReadingNonSimpleValueTripleDoubleQuoteCif2 = 8,
            OpenList = 9,
            CloseList = 10,
            OpenTable = 11,
            CloseTable = 12,
        }

        private enum ActiveScope
        {
            BlockOrFrame = 0,
            List = 1,
            Table = 2
        }
    }
}
