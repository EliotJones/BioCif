namespace BioCif.Tests
{
    using System;
    using System.IO;
    using System.Text;

    public static class TestHelpers
    {
        public static Stream StringToStream(string value) => new MemoryStream(Encoding.Unicode.GetBytes(value));

        public static StreamReader StringToStreamReader(string value)
        {
            var memStream = new MemoryStream(Encoding.Unicode.GetBytes(value));
            return new StreamReader(memStream, Encoding.Unicode);
        }

        public static string GetIntegrationDocumentFilePath(string fileName)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "documents", fileName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"No file with name {fileName} at path: {path}.");
            }

            return path;
        }
    }
}
