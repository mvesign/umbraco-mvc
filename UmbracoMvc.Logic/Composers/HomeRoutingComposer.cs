using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Routing;
using UmbracoMvc.Logic.ContentFinders;
using UmbracoMvc.Logic.Providers;

namespace UmbracoMvc.Logic.Composers;

public sealed class HomeRoutingComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.UrlProviders().InsertBefore<NewDefaultUrlProvider, HomeUrlProvider>();
        builder.ContentFinders().InsertBefore<ContentFinderByUrlNew, HomeContentFinder>();
    }
}
