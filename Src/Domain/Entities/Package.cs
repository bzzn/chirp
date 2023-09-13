using System.ComponentModel.DataAnnotations;

namespace Domain.Entites;

public class KolliIdFormatAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        var kolliId = (string)value;
        var allDigits = kolliId.All(char.IsDigit);
        if (!allDigits)
            return new ValidationResult($"{validationContext.DisplayName} must contain only numbers");

        return ValidationResult.Success;
    }
}

public class KolliIdPrefixAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        var kolliId = (string)value;
        var correctPrefix = !kolliId.StartsWith("999");
        if (correctPrefix)
            return new ValidationResult($"{validationContext.DisplayName} must start with 999");

        return ValidationResult.Success;
    }
}

public class Package
{
    // = 18 chars, numbers only, start with 999
    [Required]
    // [StringLength(18, MinimumLength = 18, ErrorMessage = "Kolli id must be exactly 18 characters long")]
    // [KolliIdFormat]
    // [KolliIdPrefix]
    public string KolliId { get; set; } = "";

    // Max: 20kg (in grams)
    [Required]
    [Range(0, 20000, ErrorMessage = "Max weight for a package is 20kg")]
    public int Weight { get; set; }

    // Max: 60cm
    [Required]
    [Range(0, 60, ErrorMessage = "Length of package must not exceed 60cm")]
    public int Length { get; set; }

    // Max: 60cm
    [Required]
    [Range(0, 60, ErrorMessage = "Height of package must not exceed 60cm")]
    public int Height { get; set; }

    // Max: 60cm
    [Required]
    [Range(0, 60, ErrorMessage = "Width of package must not exceed 60cm")]
    public int Width { get; set; }

    public IList<string> Errors { get; set; } = new List<string>();

    public bool IsValid { get; set; }
}