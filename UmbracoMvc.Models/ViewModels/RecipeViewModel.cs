using Umbraco.Cms.Core.Strings;
using UmbracoMvc.Core.Extensions;

namespace UmbracoMvc.Models.ViewModels;

public class RecipeViewModel(Recipe content)
{
    public string Heading { get; } = content.Title.WithFallback(content.Name);

    public IHtmlEncodedString Intro { get; } = content.Intro?.ConvertLineBreaksToBrTags() ?? new HtmlEncodedString(string.Empty);
    
    public IHtmlEncodedString Preparation { get; } = content.Preparation ?? new HtmlEncodedString(string.Empty);
}
