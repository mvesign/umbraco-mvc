using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Filters;
using UmbracoMvc.Models;

namespace UmbracoMvc.Website.Controllers;

[ApiController]
[UmbracoMemberAuthorize]
public class UpvoteRecipeController(
    IUmbracoContextAccessor umbracoContextAccessor,
    IMemberManager memberManager,
    IMemoryCache memoryCache) : ControllerBase
{
    public const string MemCacheKey = "Recipe_UpVote";

    [HttpPost("/umbraco/member-api/recipe/{id:guid}/upvote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]

    public async Task<IActionResult> Upvote(Guid id)
    {
        var umbracoContext = umbracoContextAccessor.GetRequiredUmbracoContext();
        if (umbracoContext.Content is null)
        {
            return BadRequest("Could not establish an umbraco content context");
        }

        var recipe = await umbracoContext.Content.GetByIdAsync(id);
        if (recipe is null)
        {
            return BadRequest($"No recipe is found for id '{id}'");
        }

        if (!await memberManager.MemberHasAccessAsync(recipe.Path))
        {
            return Unauthorized();
        }

        var member = await memberManager.GetCurrentMemberAsync();
        if (member is null)
        {
            return Unauthorized();
        }

        if (!memoryCache.TryGetValue<List<RecipeUpVote>>(MemCacheKey, out var upvotes) || upvotes is null)
        {
            upvotes = [];
        }

        if (upvotes.Any(v => v.RecipeId == recipe.Key && v.MemberId == member.Key))
        {
            return BadRequest("Recipe was already up-voted by the currently logged in member");
        }

        upvotes.Add(new RecipeUpVote
        {
            DateTime = DateTime.UtcNow,
            MemberId = member.Key,
            RecipeId = recipe.Key
        });

        memoryCache.Set(MemCacheKey, upvotes);

        return Ok();
    }
}
