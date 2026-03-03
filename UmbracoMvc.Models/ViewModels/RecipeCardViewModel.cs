using UmbracoMvc.Core.Extensions;

namespace UmbracoMvc.Models.ViewModels;

public class RecipeCardViewModel(Recipe content)
{
    public string? Heading { get; } = content.ListTitle.WithFallback(content.Title.WithFallback(content.Name));

    public string? Description { get; } = content.ListDescription.WithFallback(content.Intro?.ShortenTo100WithDots() ?? string.Empty);
    
    public string? Url { get; } = content.Url();
    
    public string? ImageUrl { get; } = content.ListImage?.Url();
    
    public string? ImageAlt { get; } = content.ListImage?.Name;
}
