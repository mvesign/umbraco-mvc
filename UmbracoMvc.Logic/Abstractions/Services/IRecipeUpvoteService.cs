using UmbracoMvc.Models.Enums;

namespace UmbracoMvc.Logic.Abstractions.Services;

public interface IRecipeUpvoteService
{
    public RecipeUpVoteResult UpVote(Guid recipeKey, Guid memberKey);

    public bool HasMemberUpVoted(Guid recipeKey, Guid memberKey);
}
