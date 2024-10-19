using Godot;

public static class ColorExtensions
{
    public static string ToString(this Color color)
    {
        return $"R: {color.R}, G: {color.G}, B: {color.B}, A: {color.A}";
    }
}