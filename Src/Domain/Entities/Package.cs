namespace Domain.Entites;

public class Package
{
    // = 18 chars, numbers only, start with 999
    public string KolliId { get; set; } = "";

    // Max: 20kg
    public int Weight { get; set; }

    // Max: 60cm
    public int Length { get; set; }

    // Max: 60cm
    public int Height { get; set; }

    // Max: 60cm
    public int Width { get; set; }

    public bool IsValid => true;
}
