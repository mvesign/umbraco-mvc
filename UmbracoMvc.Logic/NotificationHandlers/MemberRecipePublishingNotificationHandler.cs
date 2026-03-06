using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using UmbracoMvc.Models;

namespace UmbracoMvc.Logic.NotificationHandlers;

public sealed class MemberRecipePublishingNotificationHandler(
    IContentService contentService) : INotificationHandler<ContentPublishingNotification>
{
    public void Handle(ContentPublishingNotification notification)
    {
        foreach (var entity in notification.PublishedEntities)
        {
            if (entity.ContentType.Alias is not Recipe.ModelTypeAlias)
            {
                continue;
            }

            var parentEntity = contentService.GetParent(entity);
            if (parentEntity is null
                || parentEntity.ContentType.Alias is not MemberRecipeArea.ModelTypeAlias)
            {
                continue;
            }

            var recipeAreaEntity = contentService.GetParent(parentEntity);
            if (recipeAreaEntity is null
                || recipeAreaEntity.ContentType.Alias is not RecipeArea.ModelTypeAlias)
            {
                continue;
            }

            entity.ParentId = recipeAreaEntity!.Id;
        }
    }
}
