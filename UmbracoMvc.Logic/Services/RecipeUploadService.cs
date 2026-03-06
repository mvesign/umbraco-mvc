using Microsoft.AspNetCore.Http;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Models.TemporaryFile;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using UmbracoMvc.Logic.Abstractions.Services;
using UmbracoMvc.Models;
using UmbracoMvc.Models.Exceptions;
using UmbracoMvc.Models.SubmitModels;

namespace UmbracoMvc.Logic.Services;

public sealed class RecipeUploadService(
    IContentTypeService contentTypeService,
    IContentEditingService contentEditingService,
    IUserIdKeyResolver userIdKeyResolver,
    IPublishedContentTypeCache publishedContentTypeCache,
    ITemporaryFileService temporaryFileService,
    IMediaEditingService mediaEditingService,
    IJsonSerializer jsonSerializer,
    IMediaTypeService mediaTypeService,
    IContentService contentService) : IRecipeUploadService
{
    public async Task<bool> SaveRecipe(IPublishedContent currentPage, RecipeSubmitModel model)
    {
        var memberRecipeArea = (currentPage?.AncestorOrSelf<Site>()?.FirstChild<RecipeArea>()?.FirstChild<MemberRecipeArea>())
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
