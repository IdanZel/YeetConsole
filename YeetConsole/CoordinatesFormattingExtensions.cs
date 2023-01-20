using YeetConsole.Coordinates;

namespace YeetConsole;

public static class CoordinatesFormattingExtensions
{
    public static string Format(this Dictionary<string, MinecraftCoordinates> dictionary) =>
        string.Join(Environment.NewLine, dictionary.Select(pair => $"{pair.Key,-22} {pair.Value}"));
}