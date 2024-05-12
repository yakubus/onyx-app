namespace Extensions.Formatters;

public static class StringFormatter
{
    public static string Capitalize(this string value) =>
        string.Concat(
            value[0].ToString().ToUpper(),
            value[1..].ToLower());
}