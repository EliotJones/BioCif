namespace BioCif.PdbxToClasses
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Core;
    using Core.Parsing;

    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args == null || args.Length == 0 || !File.Exists(args[0]))
            {
                Console.WriteLine("Please provide the path to the dictionary as the first argument.");
                Console.ReadKey();
                return;
            }

            var results = new Dictionary<string, CategoryMembers>();

            using (var fs = File.OpenRead(args[0]))
            {
                var dict = CifParser.Parse(fs, new CifParsingOptions { FileEncoding = Encoding.UTF8 });

                foreach (var dbm in dict.FirstBlock)
                {
                    if (!(dbm is SaveFrame frame))
                    {
                        continue;
                    }

                    if (!frame.FrameCode.StartsWith("_") && frame.FrameCode.IndexOf(".", StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        if (!results.TryGetValue(frame.FrameCode, out var existing))
                        {
                            existing = new CategoryMembers();
                            results[frame.FrameCode] = existing;
                        }

                        existing.Category = frame.FrameCode;
                        existing.CategoryDetails = frame;

                        continue;
                    }

                    var dotIndex = frame.FrameCode.IndexOf(".", StringComparison.OrdinalIgnoreCase);
                    if (dotIndex < 0)
                    {
                        continue;
                    }

                    var cat = frame.FrameCode.Substring(1, dotIndex - 1);

                    if (!results.TryGetValue(cat, out var value))
                    {
                        value = new CategoryMembers { Category = cat };
                        results[cat] = value;
                    }

                    value.Fields.Add(frame.FrameCode.Substring(dotIndex + 1), frame);
                }
            }

            Console.WriteLine($"Read {results.Count} categories.");

            var sb = new StringBuilder();

            const string outputPath = @"C:\Temp\pdbx";

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            foreach (var cm in results)
            {
                var classString = CategoryToClass(sb, cm.Value);

                var outfile = Path.Combine(outputPath, $"{ToUpperCamel(cm.Key)}.cs");

                File.WriteAllText(outfile, classString);
            }
        }

        private static string CategoryToClass(StringBuilder sb, CategoryMembers members)
        {
            sb.Clear();

            if (!TryGetDataValue("category.description", members.CategoryDetails, out var catdesc))
            {
                catdesc = new DataValueSimple(members.Category);
            }

            sb.AppendLine("/// <summary>");

            var lines = catdesc.Value.Trim().Replace("\r\n", "\n").Replace("\r", "\n")
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                sb.Append("/// ").AppendLine(line.Trim());
            }

            sb.AppendLine("/// </summary>");

            var className = ToUpperCamel(members.Category);

            sb.AppendLine($"public class {className}")
                .AppendLine("{");

            var tab = new string(' ', 4);

            sb.Append(tab).AppendLine("#region names");

            sb.Append(tab).AppendLine("/// <summary>")
                .Append(tab).AppendLine("/// Category name.")
                .Append(tab).AppendLine("// </summary>")
                .Append(tab).AppendLine($"public const string Category = \"{members.Category}\";")
                .AppendLine();

            foreach (var field in members.Fields)
            {
                var name = field.Value.FrameCode.Substring(1);
                var fieldName = ToUpperCamel(field.Key);
                AppendFieldName(fieldName, name, sb, tab);
            }

            sb.Append(tab).AppendLine("#endregion");

            foreach (var field in members.Fields)
            {
                var fieldName = ToUpperCamel(field.Key);

                if (!TryGetDataValue("item_description.description", field.Value, out var desc))
                {
                    desc = new DataValueSimple(fieldName);
                }

                var type = "string";

                if (TryGetDataValue("item_type.code", field.Value, out var typecode))
                {
                    switch (typecode)
                    {
                        case "float":
                            type = "double";
                            break;
                        case "int":
                            type = "int?";
                            break;
                    }
                }

                sb.AppendLine();

                AppendField(fieldName, desc.Value, type, sb, tab);
            }

            return sb.ToString();
        }

        private static void AppendFieldName(string fieldName, string pdbxActual, StringBuilder sb, string tab)
        {
            sb.Append(tab).AppendLine("/// <summary>")
                .Append(tab).AppendLine($"/// Field name for <see cref=\"{fieldName}\"/>.")
                .Append(tab).AppendLine("/// </summary>")
                .Append(tab).AppendLine($"public const string {fieldName}FieldName = \"{pdbxActual}\";");
        }

        private static void AppendField(string fieldName, string description, string type, StringBuilder sb, string tab)
        {
            description = description.Trim();
            var lines = description.Replace("\r\n", "\n")
                .Replace("\r", "\n")
                .Split('\n', StringSplitOptions.RemoveEmptyEntries);
            sb.Append(tab).AppendLine("/// <summary>");

            foreach (var line in lines)
            {
                sb.Append(tab).AppendLine($"/// {line.Trim()}");
            }

            sb.Append(tab).AppendLine("/// </summary>")
            .Append(tab).AppendLine($"public {type} {fieldName} {{ get; set; }}");
        }

        private static bool TryGetDataValue(string tagName, SaveFrame saveFrame, out DataValueSimple val)
        {
            val = saveFrame.OfType<DataItem>().FirstOrDefault(x => x.Name == tagName)?.Value as DataValueSimple;

            return val != null;
        }

        private static string ToUpperCamel(string s)
        {
            s = s.Replace("Cartn_", "cartesian_");
            s = s.Replace("_esd", "_estimated_standard_deviation");
            var result = string.Empty;

            var prevWasUnderscore = true;
            for (var i = 0; i < s.Length; i++)
            {
                var c = s[i];
                if (c == '_')
                {
                    prevWasUnderscore = true;
                    continue;
                }

                if (prevWasUnderscore)
                {
                    result += char.ToUpperInvariant(c);
                }
                else if (!char.IsLetterOrDigit(c))
                {
                    continue;
                }
                else
                {
                    result += char.ToLowerInvariant(c);
                }

                prevWasUnderscore = false;
            }

            return result;
        }
    }

    internal class CategoryMembers
    {
        public string Category { get; set; }

        public SaveFrame CategoryDetails { get; set; }

        public Dictionary<string, SaveFrame> Fields { get; } = new Dictionary<string, SaveFrame>();
    }
}
