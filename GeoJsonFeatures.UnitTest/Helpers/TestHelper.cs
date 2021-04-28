using System;
using System.IO;

namespace GeoJsonFeatures.UnitTest.Helpers
{
    public static class TestHelper
    {
        public static string GetStringContentFromFile(string path)
        {
            try
            {
                using StreamReader streamReader = new(path);

                return streamReader.ReadToEnd();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string ConcatMessagesBySeparator(string separator, params string[] messages)
        {
            return string.Join(separator, messages);
        }
    }
}
