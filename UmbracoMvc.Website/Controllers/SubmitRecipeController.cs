using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Website.Controllers;
using UmbracoMvc.Logic.Abstractions.Services;
using UmbracoMvc.Models;
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
    IRecipeUploadService recipeUploadService) : SurfaceController(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
{
    [HttpPost]
    [ValidateUmbracoFormRouteString]
    public async Task<IActionResult> HandleSubmitRecipe([Bind(Prefix = "recipeSubmitModel")] RecipeSubmitModel model)
    {
        if (CurrentPage is null)
        {
            return CurrentUmbracoPage();
        }

        if (!memberManager.IsLoggedIn())
        {
            return RedirectToUmbracoPage(CurrentPage.AncestorOrSelf<Site>()!.FirstChild<MemberError>()!);
        }

        if (!ModelState.IsValid)
        {
            return CurrentUmbracoPage();
        }

        TempData["FormSuccess"] = await recipeUploadService.SaveRecipe(CurrentPage, model);

        return RedirectToCurrentUmbracoPage();
    }
}