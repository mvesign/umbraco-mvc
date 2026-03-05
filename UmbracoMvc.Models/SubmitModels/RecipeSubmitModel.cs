using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using UmbracoMvc.Core.Attributes;

namespace UmbracoMvc.Models.SubmitModels;

public class RecipeSubmitModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Intro { get; set; }

    [Required]
    public string Preparation { get; set; } = string.Empty;

    public IFormFile? ListImage { get; set; }

    [RequiredBoolean(ErrorMessage = "Please confirm that all sources have been noted and no copyright laws have been broken.")]
    public bool SourceConfirmation { get; set; }
}
