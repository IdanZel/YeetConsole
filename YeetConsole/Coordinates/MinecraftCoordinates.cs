namespace YeetConsole.Coordinates;

public readonly record struct MinecraftCoordinates(double X, double Z)
{
    public int BlockX => (int)Math.Floor(X);
    public int BlockZ => (int)Math.Floor(Z);

    public ChunkCoordinates Chunk => new((int)Math.Floor(X / 16.0), (int)Math.Floor(Z / 16.0));

    public static MinecraftCoordinates operator +(MinecraftCoordinates a, MinecraftCoordinates b) =>
        new(a.X + b.X, a.Z + b.Z);

    public static MinecraftCoordinates operator -(MinecraftCoordinates a, MinecraftCoordinates b) =>
        new(a.X - b.X, a.Z - b.Z);

    public static MinecraftCoordinates operator /(MinecraftCoordinates coordinates, double denominator) =>
        new(coordinates.X / denominator, coordinates.Z / denominator);

    public override string ToString() => $"({BlockX}, {BlockZ})";
}

public readonly record struct ChunkCoordinates(int X, int Z)
{
    public ChunkCoordinates Abs() => new(Math.Abs(X), Math.Abs(Z));

    public override string ToString() => $"({X}, {Z})";
}

public enum TravelDistanceWarning
{
    None,
    TooClose,
    TooFar
}

public readonly record struct PullRodCoordinates(MinecraftCoordinates Coordinates, TravelDistanceWarning Warning,
    (int Min, int Max)? RenderDistanceRange = null);