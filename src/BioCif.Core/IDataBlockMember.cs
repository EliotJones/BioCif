namespace BioCif.Core
{
    /// <summary>
    /// Marker interface for an item in a <see cref="DataBlock"/>.
    /// Could be one of <see cref="DataItem"/>, <see cref="Table"/>
    /// or for dictionary files, a <see cref="SaveFrame"/>.
    /// </summary>
    public interface IDataBlockMember
    {
    }
}