using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Notifications;
using UmbracoMvc.Logic.Abstractions.Services;
using UmbracoMvc.Logic.NotificationHandlers;
using UmbracoMvc.Logic.Services;

namespace UmbracoMvc.Logic.Composers;

public sealed class MemberRecipeComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddScoped<IRecipeUploadService, RecipeUploadService>();
        builder.Services.AddScoped<IRecipeUpvoteService, RecipeUpvoteService>();
        builder.AddNotificationHandler<ContentPublishingNotification, MemberRecipePublishingNotificationHandler>();
    }
}
