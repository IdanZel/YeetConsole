using YeetConsole.Coordinates;

namespace YeetConsole;

public class YeetCalculation
{
    private static readonly Dictionary<string, double> FrictionMap = new()
    {
        ["Land (normal)"] = 0.6,
        ["Slime block"] = 0.8,
        ["Water / Air / Boat"] = 0.9,
        ["Ice / Packed Ice"] = 0.98,
        ["Blue Ice"] = 0.989
    };

    private readonly MinecraftCoordinates _boat;
    private readonly MinecraftCoordinates _target;

    private MinecraftCoordinates Delta => _target - _boat;

    public YeetCalculation(MinecraftCoordinates boat, MinecraftCoordinates target)
    {
        _boat = boat;
        _target = target;
    }

    public bool AreCoordinatesIdentical() => _boat.Equals(_target);

    public double GetTravelAngle()
    {
        // Demonstration: https://www.desmos.com/calculator/4k4szamkop
        var baseAngle = -Math.Atan(Delta.X / Delta.Z);
        var correction = Delta.Z < 0 ? -Math.PI * Math.Sign(1.0 / Delta.X) : 0;

        var radians = baseAngle + correction;
        return radians * (180.0 / Math.PI);
    }

    public Dictionary<string, PullRodCoordinates> GetAllPullRodCoordinates()
    {
        return FrictionMap.Keys.ToDictionary(surface => surface, GetPullRodCoordinates);
    }

    private PullRodCoordinates GetPullRodCoordinates(string surface)
    {
        var friction = FrictionMap[surface];
        var travelDistance = Delta / friction;
        var chunkDistance = travelDistance.Chunk.Abs();

        switch (chunkDistance)
        {
            case { X: < 2, Z: < 2 }:
                return new PullRodCoordinates(default, TravelDistanceWarning.TooClose);
            case { X: > 43 }:
            case { Z: > 43 }:
                return new PullRodCoordinates(GetMaxTargetCoordinates(friction), TravelDistanceWarning.TooFar);
        }

        var renderDistanceRange = GetRenderDistanceRange(chunkDistance);
        return new PullRodCoordinates(_boat + travelDistance, TravelDistanceWarning.None, renderDistanceRange);
    }

    private MinecraftCoordinates GetMaxTargetCoordinates(double friction)
    {
        // TODO: Ensure this calculation is correct
        // A lot of magic numbers here, would be difficult to describe the meaning of each one.
        // The transition from the original formulas to this formula is demonstrated here:
        // https://www.desmos.com/calculator/ecuoohnyvq
        var maxDistanceX = friction * (Math.Sign(Delta.X) * 695.5 + _boat.X % 16 - 7.5);
        var maxDistanceZ = friction * (Math.Sign(Delta.Z) * 695.5 + _boat.Z % 16 - 7.5);

        return _boat + new MinecraftCoordinates(maxDistanceX, maxDistanceZ);
    }

    private (int Min, int Max) GetRenderDistanceRange(ChunkCoordinates chunkDistance)
    {
        var maxChunkDistance = Math.Max(chunkDistance.X, chunkDistance.Z);

        // TODO: Ensure this calculation is correct
        return maxChunkDistance < 15
            ? (2, maxChunkDistance + 1)
            : (maxChunkDistance - 11, Math.Min(32, maxChunkDistance + 1));
    }
}

