namespace VermittlungsPlattform.Utilities
{
    public class Truncate
    {
        public string TruncateText(string text, int maxLength)
        {
            if (text.Length <= maxLength)
            {
                return text;
            }
            else
            {
                return text.Substring(0, maxLength) + "...";
            }
        }
    }
}
