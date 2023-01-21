using YeetConsole.Coordinates;

namespace YeetConsole.Calculations;

public readonly record struct PullRodCoordinates(MinecraftCoordinates Coordinates, TravelDistanceWarning Warning,
    (int Min, int Max)? RenderDistanceRange = null);