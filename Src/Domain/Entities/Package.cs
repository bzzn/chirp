namespace Domain.Entites;

public class Package
{
    // = 18 chars, numbers only, start with 999
    public string KolliId { get; set; } = "";

    // Max: 20kg (in grams)
    public int Weight { get; set; }

    // Max: 60cm
    public int Length { get; set; }

    // Max: 60cm
    public int Height { get; set; }

    // Max: 60cm
    public int Width { get; set; }

    public bool IsValid => Validate();

    public bool Validate()
    {
        if (Weight > 20000)
            return false;

        if (Length > 60)
            return false;

        if (Height > 60)
            return false;

        if (Width > 60)
            return false;

        return true;
    }
}
