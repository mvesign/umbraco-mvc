using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Website.Controllers;
using UmbracoMvc.Models;
using UmbracoMvc.Models.Exceptions;
using UmbracoMvc.Models.SubmitModels;

namespace UmbracoMvc.Website.Controllers;

public class SubmitRecipeController(
    IUmbracoContextAccessor umbracoContextAccessor,
    IUmbracoDatabaseFactory databaseFactory,
    ServiceContext services,
    AppCaches appCaches,
    IProfilingLogger profilingLogger,
    IPublishedUrlProvider publishedUrlProvider,
    IMemberManager memberManager,
    IContentService contentService,
    IContentEditingService contentEditingService,
    IUserIdKeyResolver userIdKeyResolver,
    IPublishedContentTypeCache publishedContentTypeCache,
    IContentTypeService contentTypeService
    ) : SurfaceController(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
{
    [HttpPost]
    [ValidateUmbracoFormRouteString]
    public async Task<IActionResult> HandleSubmitRecipe([Bind(Prefix = "recipeSubmitModel")] RecipeSubmitModel model)
    {
        if (!memberManager.IsLoggedIn())
        {
            return RedirectToUmbracoPage(CurrentPage!.AncestorOrSelf<Site>()!.FirstChild<MemberError>()!);
        }

        if (!ModelState.IsValid)
        {
            return CurrentUmbracoPage();
        }

        TempData["FormSuccess"] = await SubmitRecipe(model);

        return RedirectToCurrentUmbracoPage();
    }

    private async Task<bool> SubmitRecipe(RecipeSubmitModel model)
    {
        var memberRecipeArea = (CurrentPage?.AncestorOrSelf<Site>()?.FirstChild<RecipeArea>()?.FirstChild<MemberRecipeArea>())
            ?? throw new FailedSubmitRecipeRequestException("Could not determine MemberRecipeArea");

        var recipeModelTypeKey = (contentTypeService.Get(Recipe.ModelTypeAlias)?.Key)
            ?? throw new FailedSubmitRecipeRequestException("Could not determine recipeModelTypeKey");

        var userKey = await userIdKeyResolver.GetAsync(memberRecipeArea.SubmittingUser);

        var createResult = await contentEditingService.CreateAsync(
            new ContentCreateModel
            {
                ContentTypeKey = recipeModelTypeKey,
                ParentKey = memberRecipeArea.Key,
                Variants = new[] { new VariantModel { Name = model.Name } },
                Properties =
                [
                    new PropertyValueModel
                    {
                        Alias = Recipe.GetModelPropertyType(publishedContentTypeCache, r => r.Intro)!.Alias,
                        Value = model.Intro
                    },
                    new PropertyValueModel
                    {
                        Alias = Recipe.GetModelPropertyType(publishedContentTypeCache, r => r.Preparation)!.Alias,
                        Value = model.Preparation
                    }
                ]
            },
            userKey
        );

        if (createResult.Result.Content is null)
        {
            throw new FailedSubmitRecipeRequestException("Could not create recipe");
        }

        var publishResult = contentService.Publish(createResult.Result.Content, ["*"]);

        return publishResult.Result == PublishResultType.SuccessPublish;
    }
}