using YeetConsole.Coordinates;
using YeetConsole.IO;

// ReSharper disable AccessToDisposedClosure

namespace YeetConsole;

public static class Input
{
    public static void Auto(int refreshInterval, bool ignoreInitialText)
    {
        var signal = new ManualResetEventSlim();

        bool restart;
        do
        {
            MinecraftCoordinates currentCoordinates = default;

            using var clipboardMonitor = new ClipboardMonitor(refreshInterval, ignoreInitialText, input =>
            {
                if (!InputParser.TryParseF3C(input, out currentCoordinates))
                {
                    return;
                }

                signal.Set();
            });

            Console.Clear();

            Console.WriteLine("Waiting for boat coordinates...");

            signal.Wait();

            var boat = currentCoordinates;
            Console.WriteLine($"Boat: {boat}");
            signal.Reset();

            Console.WriteLine("Waiting for target coordinates...");
            signal.Wait();

            var target = currentCoordinates;
            Console.WriteLine($"Target: {target}");
            signal.Reset();

            Console.WriteLine();

            PrintResults(boat, target);
            Console.WriteLine();

            restart = PromptContinue(boat, target);
        } while (restart);

        signal.Dispose();
    }

    public static void Manual()
    {
        bool restart;
        do
        {
            Console.Clear();

            Console.WriteLine("Enter boat coordinates:");

            MinecraftCoordinates boat;
            while (!InputParser.TryParseManualInput(Console.ReadLine() ?? string.Empty, out boat))
            {
                Console.WriteLine("Could not parse coordinates, please try again");
            }

            Console.WriteLine("Enter target coordinates:");

            MinecraftCoordinates target;
            while (!InputParser.TryParseManualInput(Console.ReadLine() ?? string.Empty, out target))
            {
                Console.WriteLine("Could not parse coordinates, please try again");
            }

            Console.WriteLine();

            PrintResults(boat, target);
            Console.WriteLine();

            restart = PromptContinue(boat, target);
        } while (restart);
    }

    private static void PrintResults(MinecraftCoordinates boat, MinecraftCoordinates target)
    {
        Console.WriteLine($"Travel angle: {YeetCalculator.TravelAngle(boat, target):0.00}°");

        var pullRodCoordinates = string.Join(Environment.NewLine,
            YeetCalculator.AllPullRodCoordinates(boat, target).Select(pair => $"{pair.Key,-22} {pair.Value}"));
        Console.WriteLine("Pull rod at: ");
        Console.WriteLine(pullRodCoordinates);
    }

    private static bool PromptContinue(MinecraftCoordinates boat, MinecraftCoordinates target)
    {
        var continueOption = PromptContinueOption();

        if (continueOption == ContinueOption.Switch)
        {
            Console.WriteLine();

            PrintResults(target, boat);
            Console.WriteLine();

            continueOption = PromptContinueOption(true);
        }

        return continueOption == ContinueOption.Restart;
    }

    private static ContinueOption PromptContinueOption(bool disableSwitch = false)
    {
        var continueOption = ContinueOption.None;

        Console.WriteLine("Press Enter to restart, Q to quit" +
                          (!disableSwitch ? ", or S to switch direction" : string.Empty));

        while (continueOption == ContinueOption.None)
        {
            var key = Console.ReadKey();
            continueOption = key.Key switch
            {
                ConsoleKey.Enter => ContinueOption.Restart,
                ConsoleKey.Q => ContinueOption.Quit,
                ConsoleKey.S when !disableSwitch => ContinueOption.Switch,
                _ => ContinueOption.None
            };
        }

        return continueOption;
    }
}