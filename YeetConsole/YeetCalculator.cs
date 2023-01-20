using YeetConsole.Coordinates;

namespace YeetConsole;

public static class YeetCalculator
{
    private static readonly Dictionary<string, double> FrictionMap = new()
    {
        ["Land (normal)"] = 0.6,
        ["Slime block"] = 0.8,
        ["Water / Air / Boat"] = 0.9,
        ["Ice / Packed Ice"] = 0.98,
        ["Blue Ice"] = 0.989
    };

    public static bool Validate(MinecraftCoordinates boat, MinecraftCoordinates target, out string warning)
    {
        if (boat.Equals(target))
        {
            warning = "Boat and Target coordinates are identical";
            return false;
        }

        var delta = target - boat;
        var chunkDistanceX = Math.Abs(delta.ChunkX);
        var chunkDistanceZ = Math.Abs(delta.ChunkZ);

        if (chunkDistanceX < 2 && chunkDistanceZ < 2)
        {
            warning = "Boat and Target coordinates are too close to each other";
            return false;
        }

        if (chunkDistanceX > 43 || chunkDistanceZ > 43)
        {
            // A lot of magic numbers here, would be difficult to describe the meaning of each one.
            // The transition from the original formulas to this formula is demonstrated here:
            // https://www.desmos.com/calculator/ecuoohnyvq
            var maxTargetCoordinates = FrictionMap.Keys.ToDictionary(surface => surface, surface =>
            {
                var friction = FrictionMap[surface];
                var maxDistanceX = friction * (Math.Sign(delta.X) * 695.5 + boat.X % 16 - 7.5);
                var maxDistanceZ = friction * (Math.Sign(delta.Z) * 695.5 + boat.Z % 16 - 7.5);
                return boat + new MinecraftCoordinates(maxDistanceX, maxDistanceZ);
            });

            warning = "Boat and Target coordinates are too far apart, " + Environment.NewLine + 
                      "furthest target coordinates at this angle are: " + Environment.NewLine +
                      maxTargetCoordinates.Format();
            return false;
        }

        warning = string.Empty;
        return true;
    }

    public static double TravelAngle(MinecraftCoordinates boat, MinecraftCoordinates target)
    {
        var delta = target - boat;

        // Demonstration: https://www.desmos.com/calculator/4k4szamkop
        var baseAngle = -Math.Atan(delta.X / delta.Z);
        var correction = delta.Z < 0 ? -Math.PI * Math.Sign(1.0 / delta.X) : 0;

        var radians = baseAngle + correction;
        return radians * (180.0 / Math.PI);
    }

    public static Dictionary<string, MinecraftCoordinates> AllPullRodCoordinates(MinecraftCoordinates boat,
        MinecraftCoordinates target)
    {
        return FrictionMap.Keys.ToDictionary(surface => surface, surface =>
        {
            var delta = target - boat;
            return boat + delta / FrictionMap[surface];
        });
    }

    public static (int Min, int Max) RenderDistanceRange(MinecraftCoordinates boat, MinecraftCoordinates target)
    {
        var delta = target - boat;

        var chunkDistanceX = Math.Abs(delta.ChunkX);
        var chunkDistanceZ = Math.Abs(delta.ChunkZ);
        var maxChunkDelta = Math.Max(chunkDistanceX, chunkDistanceZ);

        // TODO: Ensure this calculation is correct
        return maxChunkDelta < 15
            ? (2, maxChunkDelta + 1)
            : (maxChunkDelta - 11, Math.Min(32, maxChunkDelta + 1));
    }
}