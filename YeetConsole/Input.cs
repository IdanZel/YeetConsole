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

            restart = PrintResults(boat, target);
        } while (restart);

        signal.Dispose();
    }

    public static void Manual()
    {
        bool restart;
        do
        {
            Console.Clear();

            MinecraftCoordinates boat;

            Console.WriteLine("Enter boat coordinates:");
            var boatInput = Console.ReadLine();

            while (boatInput is null ||
                   (!InputParser.TryParseManualInput(boatInput, out boat) &&
                    !InputParser.TryParseF3C(boatInput, out boat)))
            {
                Console.WriteLine("Could not parse coordinates, please try again");
                boatInput = Console.ReadLine();
            }

            MinecraftCoordinates target;

            Console.WriteLine("Enter target coordinates:");
            var targetInput = Console.ReadLine();

            while (targetInput is null ||
                   (!InputParser.TryParseManualInput(targetInput, out target) &&
                    !InputParser.TryParseF3C(targetInput, out target)))
            {
                Console.WriteLine("Could not parse coordinates, please try again");
                targetInput = Console.ReadLine();
            }

            restart = PrintResults(boat, target);
        } while (restart);
    }

    private static bool PrintResults(MinecraftCoordinates boat, MinecraftCoordinates target)
    {
        bool restart;
        Console.WriteLine();

        if (!InnerPrintResults(boat, target))
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter to restart");

            Console.ReadLine();
            restart = true;
        }
        else
        {
            Console.WriteLine();
            restart = PromptContinue(boat, target);
        }

        return restart;
    }

    private static bool InnerPrintResults(MinecraftCoordinates boat, MinecraftCoordinates target)
    {
        if (!YeetCalculator.Validate(boat, target, out var warning))
        {
            Console.WriteLine(warning);
            return false;
        }

        Console.WriteLine($"Travel angle: {YeetCalculator.TravelAngle(boat, target):0.00}°");

        var pullRodCoordinates = YeetCalculator.AllPullRodCoordinates(boat, target).Format();
        Console.WriteLine("Pull rod at: ");
        Console.WriteLine(pullRodCoordinates);

        return true;
    }

    private static bool PromptContinue(MinecraftCoordinates boat, MinecraftCoordinates target)
    {
        var continueOption = PromptContinueOption();

        if (continueOption == ContinueOption.Switch)
        {
            Console.WriteLine();

            InnerPrintResults(target, boat);
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