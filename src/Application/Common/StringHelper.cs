namespace Application.Common;

public static class StringHelper
{
    public static string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        return char.ToUpperInvariant(input[0]) + input[1..].ToLowerInvariant();
    }
}
