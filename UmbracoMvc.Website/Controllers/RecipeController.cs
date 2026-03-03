using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoMvc.Models;
using UmbracoMvc.Models.ViewModels;

namespace UmbracoMvc.Website.Controllers;

public class RecipeController(
    ILogger<RecipeController> logger,
    ICompositeViewEngine compositeViewEngine,
    IUmbracoContextAccessor umbracoContextAccessor,
    IVariationContextAccessor variationContextAccessor,
    ServiceContext serviceContext) : RenderController(logger, compositeViewEngine, umbracoContextAccessor)
{
    public override IActionResult Index()
    {
        if (CurrentPage is not Recipe recipe)
        {
            return Content(string.Empty);
        }

        var recipeViewModel = new RecipeContentViewModel(
            recipe,
            new PublishedValueFallback(serviceContext, variationContextAccessor));

        return CurrentTemplate(recipeViewModel);
    }
}
