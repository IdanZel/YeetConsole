using YeetConsole;

var refreshInterval = 500;
var ignoreInitialText = true;
var isManualInput = false;

if (args.Length == 3)
{
    _ = int.TryParse(args[0], out refreshInterval);
    _ = bool.TryParse(args[1], out ignoreInitialText);
    _ = bool.TryParse(args[2], out isManualInput);
}

if (isManualInput)
{
    Input.Manual();
}
else
{
    Input.Auto(refreshInterval, ignoreInitialText);
}