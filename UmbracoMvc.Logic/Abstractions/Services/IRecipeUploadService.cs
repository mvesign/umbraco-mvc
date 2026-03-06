using Umbraco.Cms.Core.Models.PublishedContent;
using UmbracoMvc.Models.SubmitModels;

namespace UmbracoMvc.Logic.Abstractions.Services;

public interface IRecipeUploadService
{
    public Task<bool> SaveRecipe(IPublishedContent currentPage, RecipeSubmitModel model);
}
