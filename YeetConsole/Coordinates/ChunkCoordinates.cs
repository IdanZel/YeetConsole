namespace YeetConsole.Coordinates;

public readonly record struct ChunkCoordinates(int X, int Z)
{
    public ChunkCoordinates DistanceTo(ChunkCoordinates target) => new(Math.Abs(target.X - X), Math.Abs(target.Z - Z));

    public override string ToString() => $"({X}, {Z})";
}