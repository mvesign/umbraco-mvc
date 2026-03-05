using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Models.TemporaryFile;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Serialization;
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
    IContentTypeService contentTypeService,
    ITemporaryFileService temporaryFileService,
    IMediaEditingService mediaEditingService,
    IJsonSerializer jsonSerializer,
    IMediaTypeService mediaTypeService
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

        var recipeContentType = contentTypeService.Get(Recipe.ModelTypeAlias)
            ?? throw new FailedSubmitRecipeRequestException("Could not determine recipeModelTypeKey");

        var userKey = await userIdKeyResolver.GetAsync(memberRecipeArea.SubmittingUser);

        var recipeProperties = new List<PropertyValueModel>
        {          
            new()
            {
                Alias = Recipe.GetModelPropertyType(publishedContentTypeCache, r => r.Title)!.Alias,
                Value = model.Name
            },
            new()
            {
                Alias = Recipe.GetModelPropertyType(publishedContentTypeCache, r => r.Intro)!.Alias,
                Value = model.Intro
            },
            new()
            {
                Alias = Recipe.GetModelPropertyType(publishedContentTypeCache, r => r.Preparation)!.Alias,
                Value = model.Preparation
            }
        };

        if (model.ListImage is not null)
        {
            var listImageKey = await TrySaveListImage(model.ListImage, userKey);
            if (listImageKey is null)
            {
                return false;
            }

            recipeProperties.Add(
                new PropertyValueModel
                {
                    Alias = Recipe.GetModelPropertyType(publishedContentTypeCache, r => r.ListImage)!.Alias,
                    Value = $"[{{\"key\":\"{Guid.NewGuid()}\",\"mediaKey\":\"{listImageKey}\",\"mediaTypeAlias\":\"\",\"crops\":[],\"focalPoint\":null}}]"
                });
        }

        var createResult = await contentEditingService.CreateAsync(
            new ContentCreateModel
            {
                ContentTypeKey = recipeContentType.Key,
                ParentKey = memberRecipeArea.Key,
                Variants = [
                    new VariantModel
                    {
                        Name = model.Name
                    }
                ],
                Properties = recipeProperties
            },
            userKey);

        if (!createResult.Success || createResult.Result.Content is null)
        {
            throw new FailedSubmitRecipeRequestException("Could not create recipe");
        }

        var publishResult = contentService.Publish(createResult.Result.Content, ["*"]);

        return publishResult.Result == PublishResultType.SuccessPublish;
    }

    private async Task<Guid?> TrySaveListImage(IFormFile image, Guid userKey)
    {
        var imageModelTypeKey = (mediaTypeService.Get(Image.ModelTypeAlias)?.Key)
            ?? throw new FailedSubmitRecipeRequestException("Could not determine imageModelTypeKey");

        var tempFileKey = Guid.NewGuid();

        var tempFileCreateAttempt = await temporaryFileService.CreateAsync(
            new CreateTemporaryFileModel
            {
                FileName = image.FileName,
                Key = tempFileKey,
                OpenReadStream = image.OpenReadStream
            });
        if (!tempFileCreateAttempt.Success)
        {
            return null;
        }

        var listImageCreateAttempt = await mediaEditingService.CreateAsync(
            new MediaCreateModel
            {
                ContentTypeKey = imageModelTypeKey,
                ParentKey = null,
                Variants = [
                    new VariantModel
                    {
                        Name = image.FileName
                    }
                ],
                Properties = [
                    new PropertyValueModel
                    {
                        Alias = Image.GetModelPropertyType(publishedContentTypeCache, r => r.UmbracoFile)!.Alias,
                        Value = jsonSerializer.Serialize(new ImageCropperValue{TemporaryFileId = tempFileKey})
                    }
                ]
            },
            userKey);

        return listImageCreateAttempt.Success && listImageCreateAttempt.Result.Content is not null
            ? listImageCreateAttempt.Result.Content.Key
            : null;
    }
}