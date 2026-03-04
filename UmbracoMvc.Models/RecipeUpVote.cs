namespace UmbracoMvc.Models;

public class RecipeUpVote
{
    public Guid MemberId { get; set; }

    public Guid RecipeId { get; set; }
    
    public DateTime DateTime { get; set; }
}
