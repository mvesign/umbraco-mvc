using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;
using UmbracoMvc.Core.Extensions;

namespace UmbracoMvc.Models.ViewModels;

public class RecipeAreaViewModel(
    RecipeArea content,
    IPublishedValueFallback publishedValueFallback) : SiteViewModel(content, publishedValueFallback)
{
    public string Heading { get; } = content.Title.WithFallback(content.Name) ?? string.Empty;

    public IHtmlEncodedString Text { get; } = content.Text ?? new HtmlEncodedString(string.Empty);
    
    public IEnumerable<RecipeCardViewModel> PopularRecipes { get; } = content.Children<Recipe>()?.Take(3).Select(r => new RecipeCardViewModel(r)) ?? [];
}
