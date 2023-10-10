namespace CleanSample.Utility;

public static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string source, string target)
    {
        return string.Equals(source, target, StringComparison.OrdinalIgnoreCase);
    }

    public static string ToTitleCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;

        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(input.ToLower());
    }
}