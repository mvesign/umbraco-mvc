using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using UmbracoMvc.Models;

namespace UmbracoMvc.Logic.ContentFinders;

public sealed class HomeContentFinder(
    ILogger<ContentFinderByUrlNew> logger,
    IUmbracoContextAccessor umbracoContextAccessor,
    IDocumentUrlService documentUrlService,
    IPublishedContentCache publishedContentCache,
    IOptionsMonitor<WebRoutingSettings> webRoutingSettings) : ContentFinderByUrlNew(
        logger,
        umbracoContextAccessor,
        documentUrlService,
        publishedContentCache,
        webRoutingSettings)
{
    public override async Task<bool> TryFindContent(IPublishedRequestBuilder frequest)
    {
        if (!await base.TryFindContent(frequest))
        {
            return false;
        }

        var baseResult = frequest.PublishedContent;
        if (baseResult is not Site siteNode)
        {
            return false;
        }

        if (siteNode.Home is not null)
        {
            frequest.SetPublishedContent(siteNode.Home);
            return true;
        }

        var firstChild = baseResult.FirstChild();
        if (firstChild == null)
        {
            return false;
        }

        frequest.SetPublishedContent(firstChild);
        return true;
    }
}