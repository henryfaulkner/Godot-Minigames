using Godot;

public static class Vector3Extensions 
{
    public static string ToString(this Vector3 vector)
    {
        return $"X: {vector.X}, Y: {vector.Y}, Z: {vector.Z}";
    }
}