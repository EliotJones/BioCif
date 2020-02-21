namespace BioCif
{
    using System;
    using System.Collections.Generic;
    using Core;

    /// <summary>
    /// Helpers and convenience methods for <see cref="DataBlock"/> and associated types.
    /// </summary>
    public static class DataBlockExtensions
    {
        private static readonly DataTable Empty = new DataTable(new DataName[0], new IReadOnlyList<IDataValue>[0]);

        /// <summary>
        /// Gets the <see cref="DataTable"/> for a PDBx dictionary category ('_category.name') from the data block,
        /// this will wrap a single entry into a <see cref="DataTable"/> if it is not looped.
        /// Will return an entirely empty <see cref="DataTable"/> if no entries in the specified category are present.
        /// </summary>
        public static DataTable GetTableForCategory(this DataBlock block, string category)
        {
            if (block == null)
            {
                throw new ArgumentNullException(nameof(block));
            }

            if (category == null)
            {
                return Empty;
            }

            var fromNames = default(List<DataName>);
            var fromValues = default(List<IDataValue>);

            foreach (var member in block)
            {
                if (member is DataTable table)
                {
                    foreach (var header in table.Headers)
                    {
                        if (header.Tag.IsCategory(category))
                        {
                            return table;
                        }
                    }
                }
                else if (member is DataItem item)
                {
                    if (item.Name.Tag.IsCategory(category))
                    {
                        if (fromNames == null)
                        {
                            fromNames = new List<DataName>();
                            fromValues = new List<IDataValue>();
                        }

                        fromNames.Add(item.Name);
                        fromValues.Add(item.Value);
                    }
                }
            }

            if (fromNames == null)
            {
                return Empty;
            }

            return new DataTable(fromNames, new []{ fromValues });
        }

        /// <summary>
        /// Checks if the provided name is part of the PDBx category where category is the first part of the name in format '_category.item'. 
        /// </summary>
        public static bool IsCategory(this DataName name, string category) => IsCategory(name.Tag, category);
        /// <summary>
        /// Checks if the provided string is part of the PDBx category where category is the first part of the name in format '_category.item'.
        /// </summary>
        public static bool IsCategory(this string name, string category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            if (name == null || name.Length <= category.Length|| name[category.Length] != '.')
            {
                return false;
            }

            for (var i = 0; i < category.Length; i++)
            {
                var c = char.ToLowerInvariant(category[i]);
                var nc = char.ToLowerInvariant(name[i]);

                if (c != nc)
                {
                    return false;
                }
            }

            return true;
        }
    }
}