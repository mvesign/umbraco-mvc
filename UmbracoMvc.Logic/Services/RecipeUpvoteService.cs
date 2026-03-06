using Microsoft.Extensions.Caching.Memory;
using UmbracoMvc.Logic.Abstractions.Services;
using UmbracoMvc.Models;
using UmbracoMvc.Models.Enums;

namespace UmbracoMvc.Logic.Services;

public sealed class RecipeUpvoteService(
    IMemoryCache memoryCache) : IRecipeUpvoteService
{
    private const string MemCacheKey = "Recipe_UpVote";

    public RecipeUpVoteResult UpVote(Guid recipeKey, Guid memberKey)
    {
        if (!memoryCache.TryGetValue<List<RecipeUpVote>>(MemCacheKey, out var upvotes)
            || upvotes is null)
        {
            upvotes = [];
        }

        if (upvotes.Any(v => v.RecipeId == recipeKey && v.MemberId == memberKey))
        {
            return RecipeUpVoteResult.FailedAlreadyUpVotedByMember;
        }

        upvotes.Add(new RecipeUpVote
        {
            DateTime = DateTime.UtcNow,
            MemberId = memberKey,
            RecipeId = recipeKey
        });

        memoryCache.Set(MemCacheKey, upvotes);

        return RecipeUpVoteResult.Success;
    }

    public bool HasMemberUpVoted(Guid recipeKey, Guid memberKey) =>
        memoryCache.TryGetValue<List<RecipeUpVote>>(MemCacheKey, out var votes)
        && votes is not null
        && votes.Any(v => v.MemberId == memberKey && v.RecipeId == recipeKey);
}
