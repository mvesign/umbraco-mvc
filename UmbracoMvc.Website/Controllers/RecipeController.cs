using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoMvc.Logic.Abstractions.Services;
using UmbracoMvc.Models;
using UmbracoMvc.Models.ViewModels;

namespace UmbracoMvc.Website.Controllers;

public class RecipeController(
    ILogger<RecipeController> logger,
    ICompositeViewEngine compositeViewEngine,
    IUmbracoContextAccessor umbracoContextAccessor,
    IVariationContextAccessor variationContextAccessor,
    ServiceContext serviceContext,
    IMemberManager memberManager,
    IRecipeUpvoteService recipeUpvoteService) : RenderController(logger, compositeViewEngine, umbracoContextAccessor)
{
    public override IActionResult Index()
    {
        if (CurrentPage is not Recipe recipe)
        {
            return Content(string.Empty);
        }

        var recipeViewModel = new RecipeContentViewModel(
            recipe,
            new PublishedValueFallback(serviceContext, variationContextAccessor))
        {
            Member = HasUpVoted().GetAwaiter().GetResult()
        };

        return CurrentTemplate(recipeViewModel);
    }

    private async Task<MemberContent> HasUpVoted()
    {
        var memberContent = new MemberContent();

        if (!memberManager.IsLoggedIn())
        {
            return memberContent;
        }

        memberContent.IsLoggedIn = true;

        var member = await memberManager.GetCurrentMemberAsync();
        if (member is null)
        {
            return memberContent;
        }

        memberContent.HasUpVoted = recipeUpvoteService.HasMemberUpVoted(CurrentPage!.Key, member.Key);
        
        return memberContent;
    }
}
