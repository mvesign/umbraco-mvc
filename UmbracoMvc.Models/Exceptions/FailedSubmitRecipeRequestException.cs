namespace UmbracoMvc.Models.Exceptions;

public sealed class FailedSubmitRecipeRequestException : Exception
{
    public FailedSubmitRecipeRequestException() : base() { }

    public FailedSubmitRecipeRequestException(string? message) : base(message) { }
}
