using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoMvc.Models.ViewModels;

public class SiteContentViewModel(IPublishedContent content)
{
    public string Name { get; } = $"{content.Name} - {content.Root<Site>()?.SiteName}";
}
