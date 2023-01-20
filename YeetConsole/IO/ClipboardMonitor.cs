using System.ComponentModel;
using TextCopy;

namespace YeetConsole.IO;

public class ClipboardMonitor : IDisposable
{
    private readonly Timer _readTimer;
    private readonly bool _ignoreInitialText;
    private readonly Action<string> _onClipboardUpdated;

    private string _lastClipboardText;
    private bool _initialTextProcessed;

    public ClipboardMonitor(int readingIntervalMilliseconds, bool ignoreInitialText, Action<string> onClipboardUpdated)
    {
        var readingInterval = TimeSpan.FromMilliseconds(readingIntervalMilliseconds);
        _readTimer = new Timer(ReadFromClipboard, null, readingInterval, readingInterval);

        _ignoreInitialText = ignoreInitialText;
        _onClipboardUpdated = onClipboardUpdated;

        _lastClipboardText = string.Empty;
        _initialTextProcessed = false;
    }

    private async void ReadFromClipboard(object? state)
    {
        string? clipboardText;

        try
        {
            clipboardText = await ClipboardService.GetTextAsync();
        }
        catch (Win32Exception)
        {
            // If another process is trying to read from the clipboard (e.g. Ninjabrain Bot) an exception might be
            // thrown from GetTextAsync(). In that case, we simply ignore the current attempt.
            // This might need better handling in the future.
            return;
        }

        if (clipboardText is null || clipboardText.Equals(_lastClipboardText, StringComparison.Ordinal))
        {
            return;
        }

        _lastClipboardText = clipboardText;

        if (_ignoreInitialText && !_initialTextProcessed)
        {
            _initialTextProcessed = true;
            return;
        }

        _onClipboardUpdated(clipboardText);
    }

    public void Dispose()
    {
        _readTimer.Dispose();
        GC.SuppressFinalize(this);
    }
}