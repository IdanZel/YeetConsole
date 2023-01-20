using YeetConsole.Coordinates;

namespace YeetConsole.IO;

public static class InputParser
{
    public static bool TryParseF3C(string input, out MinecraftCoordinates coordinates)
    {
        var inputParts = input.Split();

        if (inputParts.Length != 11 ||
            !double.TryParse(inputParts[6], out var x) ||
            !double.TryParse(inputParts[8], out var z))
        {
            coordinates = default;
            return false;
        }

        coordinates = new MinecraftCoordinates(x, z);
        return true;
    }

    public static bool TryParseManualInput(string input, out MinecraftCoordinates coordinates)
    {
        var inputParts = input.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

        if (inputParts.Length != 2 ||
            !double.TryParse(inputParts[0], out var x) ||
            !double.TryParse(inputParts[1], out var z))
        {
            coordinates = default;
            return false;
        }

        coordinates = new MinecraftCoordinates(x, z);
        return true;
    }
}