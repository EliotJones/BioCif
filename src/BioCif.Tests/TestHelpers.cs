namespace BioCif.Tests
{
    using System.IO;
    using System.Text;

    public static class TestHelpers
    {
        public static StreamReader StringToStreamReader(string value)
        {
            var memStream = new MemoryStream(Encoding.Unicode.GetBytes(value));
            return new StreamReader(memStream, Encoding.Unicode);
        }
    }
}
