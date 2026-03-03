using System.ComponentModel.DataAnnotations;

namespace UmbracoMvc.Core.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class RequiredBooleanAttribute : ValidationAttribute
{
    public override bool IsValid(object? value) =>
        value is bool boolValue && boolValue;
}
