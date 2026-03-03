using Umbraco.Cms.Core.Strings;

namespace UmbracoMvc.Website.Extensions;

public static class StringExtensions
{
    public static IHtmlEncodedString ConvertLineBreaksToBrTags(this string input) =>
        new HtmlEncodedString(input.Replace("\n", "<br />"));

    public static string ShortenTo100WithDots(this string input) =>
        input.ShortenTo(100, "...");

    public static string ShortenTo(this string input, int amount, string ending)
    {
        if (input.Length <= 100)
        {
            return input;
        }

        var output = input.Contains(' ')
            ? input[..input[..(amount - ending.Length)].LastIndexOf(' ')]
            : input[..(amount - ending.Length)];

        return $"{output}{ending}";
    }

    // this is used in exercise 2
    public static string? WithFallback(this string? input, string? fallback) =>
        !string.IsNullOrWhiteSpace(input)
            ? input
            : fallback;
}
