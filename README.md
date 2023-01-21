# YeetConsole

Simple, interactive tool for calculating Yeeting coordinates in Minecraft AA speedruns, with built-in `F3`+`C` reading.

Based on [The64thRealm's original Yeet Calculator](https://replit.com/@The64thRealm/Yeet-calculator).

## Usage

The tool can be started directly or from the CMD.

You will be prompted to copy (with `F3`+`C`) the boat's and target's coordinates.

The results for each surface type will be printed in the console.

If the calculation is valid, the rod-pull coordinates and chunk coordinates will be displayed. Otherwise, a warning will be shown.

### Options (not required)

When running from CMD, the following arguments can be passed:

`YeetConsole.exe <ClipboardRefreshInterval> <IgnoreInitialText> <IsManualInput>`

* `ClipboardRefreshInterval` [**Default: `100`**]

  Interval between clipboard readings in milliseconds.

* `IgnoreInitialText` [**Default: `true`**]

  Indicates whether text that is already in the clipboard on startup will be ignored or not.

* `IsManualInput` [**Default: `false`**]

  Enables manual input of coordinates and disables clipboard reading. Coordinates can be in either `x z` or `F3`+`C` format.
  
  When set to `true`, the first two arguments will have no effect.
