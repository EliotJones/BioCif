namespace BioCif.Core.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Tokenization;
    using Tokenization.Tokens;

    /// <summary>
    /// Parses CIF files into the <see cref="Cif"/> data structure.
    /// </summary>
    public static class CifParser
    {
        /// <summary>
        /// Parse the CIF data from the input stream using provided options.
        /// </summary>
        public static Cif Parse(Stream stream, CifParsingOptions options = null)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            ValidateStream(stream);

            if (options == null)
            {
                options = new CifParsingOptions();
            }
            var blocks = new List<DataBlock>();

            var state = new Stack<ParsingState>(new[] { ParsingState.None });

            var previous = default(Token);
            var lastName = default(DataName);

            var buffered = new BufferedStream(stream);
            using (var reader = options.FileEncoding != null ? new StreamReader(buffered, options.FileEncoding) : new StreamReader(buffered, Encoding.UTF8, true))
            {
                var activeBlock = default(DataBlockBuilder);
                var activeLoop = default(LoopBuilder);
                var listsStack = new Stack<List<IDataValue>>();
                var dictionariesStack = new Stack<DictionaryState>();
                var kvpStack = new Stack<DictionaryPair>();
                var activeSaveFrame = default(SaveFrameBuilder);

                foreach (var token in CifTokenizer.Tokenize(reader, options.CifFileVersion, options.NullSymbol))
                {
                    var currentState = state.Peek();

                    switch (token.TokenType)
                    {
                        case TokenType.Comment:
                            break;
                        case TokenType.DataBlock:
                            {
                                if (activeLoop != null)
                                {
                                    activeBlock.Members.Add(activeLoop.Build());
                                    state.Pop();
                                }

                                if (activeBlock != null)
                                {
                                    blocks.Add(activeBlock.Build());
                                    state.Pop();
                                }

                                state.Push(ParsingState.InsideDataBlock);

                                activeBlock = new DataBlockBuilder(token);
                            }
                            break;
                        case TokenType.Loop:
                            {
                                if (activeLoop != null)
                                {
                                    activeBlock.Members.Add(activeLoop.Build());
                                    state.Pop();
                                }

                                activeLoop = new LoopBuilder();

                                state.Push(ParsingState.InsideLoop);
                            }
                            break;
                        case TokenType.StartList:
                            {
                                state.Push(ParsingState.InsideList);
                                listsStack.Push(new List<IDataValue>());
                            }
                            break;
                        case TokenType.EndList:
                            {
                                if (currentState != ParsingState.InsideList)
                                {
                                    throw new InvalidOperationException($"Encountered end of list token when in state {currentState}. Previous token was {previous}.");
                                }

                                state.Pop();
                                var completed = listsStack.Pop();
                                var list = new DataList(completed);

                                currentState = state.Peek();
                                switch (currentState)
                                {
                                    case ParsingState.InsideLoopValues:
                                        activeLoop.AddToRow(list);
                                        break;
                                    case ParsingState.InsideList:
                                        listsStack.Peek().Add(list);
                                        break;
                                    case ParsingState.InsideTable:
                                        dictionariesStack.Peek().Dictionary[kvpStack.Pop().Key.Value] = list;
                                        break;
                                    case ParsingState.InsideDataBlock:
                                        activeBlock.Members.Add(new DataItem(lastName, list));
                                        break;
                                    case ParsingState.InsideSaveFrame:
                                        activeSaveFrame.Members.Add(new DataItem(lastName, list));
                                        break;
                                    default:
                                        throw new InvalidOperationException($"List in unexpected context {completed} in {currentState}.");
                                }
                            }
                            break;
                        case TokenType.StartTable:
                            {
                                if (kvpStack.Count > 0)
                                {
                                    kvpStack.Peek().IsOuterScope = true;
                                }

                                state.Push(ParsingState.InsideTable);
                                dictionariesStack.Push(new DictionaryState(previous?.Value));
                            }
                            break;
                        case TokenType.EndTable:
                            {
                                if (currentState != ParsingState.InsideTable)
                                {
                                    throw new InvalidOperationException($"Encountered end of table token when in state {currentState}. Previous token was {previous}.");
                                }

                                state.Pop();
                                var completed = dictionariesStack.Pop();
                                var dict = new DataDictionary(completed.Dictionary);
                                currentState = state.Peek();

                                switch (currentState)
                                {
                                    case ParsingState.InsideDataBlock:
                                        activeBlock.Members.Add(new DataItem(lastName, dict));
                                        break;
                                    case ParsingState.InsideSaveFrame:
                                        activeSaveFrame.Members.Add(new DataItem(lastName, dict));
                                        break;
                                    case ParsingState.InsideLoop:
                                        activeLoop.AddToRow(dict);
                                        break;
                                    case ParsingState.InsideList:
                                        listsStack.Peek().Add(dict);
                                        break;
                                    case ParsingState.InsideTable:
                                        dictionariesStack.Peek().Dictionary[completed.Key] = dict;
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                            break;
                        case TokenType.Value:
                            switch (currentState)
                            {
                                case ParsingState.InsideDataBlock:
                                    if (previous?.TokenType != TokenType.Name)
                                    {
                                        throw new InvalidOperationException();
                                    }

                                    activeBlock.Members.Add(new DataItem(new DataName(previous.Value), new DataValueSimple(token.Value)));
                                    break;
                                case ParsingState.InsideSaveFrame:
                                    if (previous?.TokenType != TokenType.Name)
                                    {
                                        throw new InvalidOperationException();
                                    }

                                    activeSaveFrame.Members.Add(new DataItem(new DataName(previous.Value), new DataValueSimple(token.Value)));
                                    break;
                                case ParsingState.InsideLoop:
                                    state.Pop();
                                    state.Push(ParsingState.InsideLoopValues);
                                    activeLoop.AddToRow(token);
                                    break;
                                case ParsingState.InsideLoopValues:
                                    activeLoop.AddToRow(token);
                                    break;
                                case ParsingState.InsideList:
                                    listsStack.Peek().Add(new DataValueSimple(token.Value));
                                    break;
                                case ParsingState.InsideTable:
                                    if (kvpStack.Count == 0 || kvpStack.Peek().IsOuterScope)
                                    {
                                        kvpStack.Push(new DictionaryPair { Key = token });
                                        break;
                                    }

                                    var val = kvpStack.Pop();
                                    dictionariesStack.Peek().Dictionary[val.Key.Value] = new DataValueSimple(token.Value);
                                    break;
                            }
                            break;
                        case TokenType.Name:
                            if (currentState == ParsingState.InsideLoop)
                            {
                                activeLoop.AddHeader(token);
                            }
                            else if (currentState == ParsingState.InsideLoopValues)
                            {
                                if (activeLoop == null)
                                {
                                    throw new InvalidOperationException($"End of loop detected after token {previous} but no loop was active.");
                                }

                                state.Pop();
                                var outer = state.Peek();

                                if (outer == ParsingState.InsideDataBlock)
                                {
                                    activeBlock.Members.Add(activeLoop.Build());
                                }
                                else if (outer == ParsingState.InsideSaveFrame)
                                {
                                    activeSaveFrame.Members.Add(activeLoop.Build());
                                }
                                else
                                {
                                    throw new InvalidOperationException($"End of loop detected but outer state was: {outer}. Loops can only be inside data blocks and save frames.");
                                }

                                activeLoop = null;
                            }
                            else
                            {
                                lastName = new DataName(token.Value);
                            }
                            break;
                        case TokenType.SaveFrame:
                            if (activeSaveFrame != null)
                            {
                                throw new InvalidOperationException($"Encountered nested save frame with name {token.Value} inside {activeSaveFrame}.");
                            }

                            if (currentState == ParsingState.InsideLoopValues)
                            {
                                state.Pop();
                                var outer = state.Peek();

                                switch (outer)
                                {
                                    case ParsingState.InsideDataBlock:
                                        activeBlock.Members.Add(activeLoop.Build());
                                        break;
                                    case ParsingState.InsideSaveFrame:
                                        activeSaveFrame.Members.Add(activeLoop.Build());
                                        break;
                                    default:
                                        throw new InvalidOperationException($"End of loop detected as save frame but outer state was: {outer}.");
                                }

                                activeLoop = null;
                            }
                            else if (currentState != ParsingState.InsideDataBlock)
                            {
                                throw new InvalidOperationException($"Encountered save frame with name {token.Value} in wrong state: {currentState}. Only valid for {ParsingState.InsideDataBlock}.");
                            }

                            activeSaveFrame = new SaveFrameBuilder(token.Value.Substring(5));
                            state.Push(ParsingState.InsideSaveFrame);
                            break;
                        case TokenType.SaveFrameEnd:
                            if (currentState == ParsingState.InsideLoopValues)
                            {
                                state.Pop();
                                var outer = state.Peek();

                                switch (outer)
                                {
                                    case ParsingState.InsideDataBlock:
                                        activeBlock.Members.Add(activeLoop.Build());
                                        break;
                                    case ParsingState.InsideSaveFrame:
                                        activeSaveFrame.Members.Add(activeLoop.Build());
                                        break;
                                    default:
                                        throw new InvalidOperationException($"End of loop detected as save frame but outer state was: {outer}.");
                                }

                                activeLoop = null;
                            }
                            else if (currentState != ParsingState.InsideSaveFrame)
                            {
                                throw new InvalidOperationException($"Encountered end of save frame outside save frame. State was {currentState}, previous token was {previous}.");
                            }

                            if (activeSaveFrame == null)
                            {
                                throw new InvalidOperationException($"Encountered end of save frame with no active save frame.");
                            }

                            state.Pop();
                            activeBlock.Members.Add(activeSaveFrame.Build());
                            activeSaveFrame = null;
                            break;
                        case TokenType.Unknown:
                            throw new InvalidOperationException($"Encountered unexpect token in CIF data: {token}.");

                    }

                    previous = token;
                }

                if (activeLoop != null)
                {
                    activeBlock.Members.Add(activeLoop.Build());
                }

                if (activeBlock != null)
                {
                    blocks.Add(activeBlock.Build());
                }
            }

            return new Cif(blocks);
        }

        private static void ValidateStream(Stream stream)
        {
            if (!stream.CanRead)
            {
                throw new ArgumentException($"Could not read from the provided stream of type {stream.GetType().FullName}.");
            }

            if (!stream.CanSeek)
            {
                throw new ArgumentException($"Could not seek in provided stream of type {stream.GetType().FullName}.");
            }
        }

        private class DataBlockBuilder
        {
            public Token Token { get; }

            public List<IDataBlockMember> Members { get; } = new List<IDataBlockMember>();

            public DataBlockBuilder(Token token)
            {
                if (token == null)
                {
                    throw new ArgumentNullException(nameof(token));
                }

                if (token.TokenType != TokenType.DataBlock)
                {
                    throw new ArgumentException($"Invalid token to start data block: {token}.", nameof(token));
                }

                Token = token;
            }

            public DataBlock Build()
            {
                return new DataBlock(Token.Value.Substring(5), Members);
            }
        }

        private class LoopBuilder
        {
            private readonly List<string> headers = new List<string>();

            private readonly List<List<IDataValue>> values = new List<List<IDataValue>>();

            public void AddHeader(Token token)
            {
                if (token == null)
                {
                    throw new ArgumentNullException(nameof(token));
                }

                if (token.TokenType != TokenType.Name)
                {
                    throw new InvalidOperationException($"Tried to set a header for a loop with type: {token}.");
                }

                headers.Add(token.Value);
            }

            public void AddToRow(Token token)
            {
                if (headers.Count == 0)
                {
                    throw new InvalidOperationException($"Attempted to add token to table with empty headers: {token}.");
                }

                if (values.Count == 0)
                {
                    values.Add(new List<IDataValue> { new DataValueSimple(token.Value) });
                }
                else
                {
                    var last = values[values.Count - 1];

                    if (last.Count < headers.Count)
                    {
                        last.Add(new DataValueSimple(token.Value));
                    }
                    else
                    {
                        values.Add(new List<IDataValue> { new DataValueSimple(token.Value) });
                    }
                }
            }

            public void AddToRow(IDataValue value)
            {
                if (headers.Count == 0)
                {
                    throw new InvalidOperationException($"Attempted to add value to table with empty headers: {value}.");
                }

                if (values.Count == 0)
                {
                    values.Add(new List<IDataValue> { value });
                }
                else
                {
                    var last = values[values.Count - 1];

                    if (last.Count < headers.Count)
                    {
                        last.Add(value);
                    }
                    else
                    {
                        values.Add(new List<IDataValue> { value });
                    }
                }
            }

            public DataTable Build()
            {
                var names = headers.Select(x => new DataName(x)).ToList();
                
                return new DataTable(names, values);
            }
        }

        private class SaveFrameBuilder
        {
            private readonly string name;

            public List<IDataBlockMember> Members { get; } = new List<IDataBlockMember>();

            public SaveFrameBuilder(string name)
            {
                this.name = name;
            }

            public SaveFrame Build()
            {
                return new SaveFrame(name, Members);
            }
        }

        private enum ParsingState
        {
            None = 0,
            InsideDataBlock = 1,
            InsideLoop = 2,
            InsideList = 3,
            InsideTable = 4,
            InsideSaveFrame = 5,
            InsideLoopValues = 6
        }

        private class DictionaryPair
        {
            public Token Key { get; set; }

            public bool IsOuterScope { get; set; }
        }

        private class DictionaryState
        {
            public string Key { get; }

            public Dictionary<string, IDataValue> Dictionary { get; } = new Dictionary<string, IDataValue>();

            public DictionaryState(string key)
            {
                Key = key;
            }
        }
    }
}