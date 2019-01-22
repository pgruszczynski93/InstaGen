
namespace InstaGen
{
    public static class StringHelper
    {
        private const string HASH_CHAR = "#";

        private static string HASHTAG_FORMAT = "{0}{1}";

        public static string GetFormattedHashtag(this string text)
        {
            return string.Format(HASHTAG_FORMAT, HASH_CHAR, text);
        }
    }
}
