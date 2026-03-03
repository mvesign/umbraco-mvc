using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoMvc.Models.ViewModels;

public class SiteViewModel(
    IPublishedContent content,
    IPublishedValueFallback publishedValueFallback) : PublishedContentWrapped(content, publishedValueFallback)
{
    public SiteContentViewModel Site { get; } = new SiteContentViewModel(content);
}
