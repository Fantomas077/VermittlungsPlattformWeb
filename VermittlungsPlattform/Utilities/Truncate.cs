namespace VermittlungsPlattform.Utilities
{
    public class Truncate
    {
        public string TruncateText(string text, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrEmpty(text) || maxLength <= 0)
            {
                return string.Empty;
            }

            if (text.Length <= maxLength)
            {
                return text;
            }
            else
            {
                return text.Substring(0, maxLength).TrimEnd() + suffix;
            }
        }
    }
}
