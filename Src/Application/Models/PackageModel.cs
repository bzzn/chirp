namespace Application.Models;

public class PackageModel
{
    public string KolliId { get; set; } = "";
    public int Weight { get; set; }
    public int Length { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public bool IsValid { get; set; }
}