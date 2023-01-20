namespace YeetConsole.Coordinates;

public readonly record struct MinecraftCoordinates(double X, double Z)
{
    public int BlockX => (int)Math.Floor(X);
    public int BlockZ => (int)Math.Floor(Z);

    public int ChunkX => (int)Math.Floor(X / 16.0);
    public int ChunkZ => (int)Math.Floor(Z / 16.0);

    public static MinecraftCoordinates operator +(MinecraftCoordinates a, MinecraftCoordinates b) =>
        new(a.X + b.X, a.Z + b.Z);

    public static MinecraftCoordinates operator -(MinecraftCoordinates a, MinecraftCoordinates b) =>
        new(a.X - b.X, a.Z - b.Z);

    public static MinecraftCoordinates operator /(MinecraftCoordinates coordinates, double denominator) =>
        new(coordinates.X / denominator, coordinates.Z / denominator);

    public override string ToString() => $"({BlockX}, {BlockZ})";
}