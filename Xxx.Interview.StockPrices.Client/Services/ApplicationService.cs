using System.Diagnostics;
using System.IO;
using System.Windows;
using NLog;

namespace Xxx.Interview.StockPrices.Client.Services;

public sealed class ApplicationService : IApplicationService
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private string _logFolder;

    public string LogFolder
    {
        get
        {
            if (!string.IsNullOrEmpty(_logFolder))
                return _logFolder;

            _logFolder = GetLogFolder();
            return _logFolder;
        }
    }

    public void CopyToClipboard(string text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        Clipboard.SetText(text);
    }

    public void Exit()
    {
        Application.Current.Shutdown();
    }

    public void Restart()
    {
        Process.Start(Application.ResourceAssembly.Location);
        Application.Current.Shutdown();
    }

    public void OpenFolder(string folder)
    {
        Logger.Info($"OpenFolder - Path=[{folder}]");

        if (string.IsNullOrEmpty(folder))
            return;

        Process.Start("explorer.exe", folder);
    }

    private static string GetLogFolder()
    {
        var logPath = LogManager.Configuration?.Variables["file-target-filename"]
            ?.Render(LogEventInfo.CreateNullEvent());

        if (string.IsNullOrEmpty(logPath))
            return null;

        var fileInfo = new FileInfo(logPath);
        return fileInfo.DirectoryName;
    }
}