using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Cms.Core.Web;
using UmbracoMvc.Models;

namespace UmbracoMvc.Logic.Providers;

public sealed class HomeUrlProvider(
    ILogger<NewDefaultUrlProvider> logger,
    IOptionsMonitor<RequestHandlerSettings> requestSettings,
    ISiteDomainMapper siteDomainMapper,
    IUmbracoContextAccessor umbracoContextAccessor,
    UriUtility uriUtility,
    IPublishedContentCache publishedContentCache,
    IDomainCache domainCache,
    IIdKeyMap idKeyMap,
    IDocumentUrlService documentUrlService,
    IDocumentNavigationQueryService navigationQueryService,
    IPublishedContentStatusFilteringService publishedContentStatusFilteringService,
    ILanguageService languageService) : NewDefaultUrlProvider(
        requestSettings,
        logger,
        siteDomainMapper,
        umbracoContextAccessor,
        uriUtility,
        publishedContentCache,
        domainCache,
        idKeyMap,
        documentUrlService,
        navigationQueryService,
        publishedContentStatusFilteringService,
        languageService)
{
    public override UrlInfo? GetUrl(IPublishedContent content, UrlMode mode, string? culture, Uri current)
    {
        var siteNode = content.AncestorOrSelf<Site>();
        if (siteNode == null)
        {
            return null;
        }

        var selectedHome = siteNode.Home ?? siteNode.FirstChild();
        return selectedHome?.Id == content.Id
            ? base.GetUrl(siteNode, mode, culture, current)
            : null;
    }
}
