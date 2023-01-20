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

    public static Dictionary<string, MinecraftCoordinates> AllPullRodCoordinates(MinecraftCoordinates boat,
        MinecraftCoordinates target)
    {
        return FrictionMap.Keys.ToDictionary(surface => surface, surface =>
        {
            var delta = target - boat;
            return boat + delta / FrictionMap[surface];
        });
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
}