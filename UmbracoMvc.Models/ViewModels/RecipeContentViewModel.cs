using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoMvc.Models.ViewModels;

public class RecipeContentViewModel(
    Recipe content,
    IPublishedValueFallback publishedValueFallback) : SiteViewModel(content, publishedValueFallback)
{
    public RecipeViewModel Content { get; } = new RecipeViewModel(content);

    public MemberContent Member { get; set; } = new MemberContent();
}
