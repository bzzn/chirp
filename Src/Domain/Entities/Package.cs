using System.ComponentModel.DataAnnotations;

namespace Domain.Entites;

public class Package
{
    [Required]
    public string KolliId { get; set; } = "";

    [Required]
    [Range(1, 20000, ErrorMessage = "Max weight for a package is 20kg")]
    public int Weight { get; set; }

    [Required]
    [Range(1, 60, ErrorMessage = "Length of package must not exceed 60cm")]
    public int Length { get; set; }

    [Required]
    [Range(1, 60, ErrorMessage = "Height of package must not exceed 60cm")]
    public int Height { get; set; }

    [Required]
    [Range(1, 60, ErrorMessage = "Width of package must not exceed 60cm")]
    public int Width { get; set; }

    public IList<string> Errors { get; set; } = new List<string>();

    public bool IsValid { get; set; }
}