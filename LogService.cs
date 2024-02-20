using System.Net.Http.Json;

namespace Hv.Sos100.Logger;

public class LogService
{
    private const string BaseUrl = "https://informatik6.ei.hv.se/logapi";
    private readonly HttpClient _httpClient = new();

    /// <summary>
    /// Create a log in the database, which can be viewed in the web application,
    /// OBS severity has to match either 1'Info', 2'Warning', 3'Error' or it will generate an exception
    /// </summary>
    public async Task<bool> CreateApiLog(string sourceSystem, int severityLevel, string message)
    {
        var severity = ValidateSeverity(severityLevel);

        var log = new Log
        {
            TimeStamp = DateTime.Now,
            SourceSystem = sourceSystem,
            Message = message,
            Severity = severity
        };

        var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/api/Logs", log);
        return response.IsSuccessStatusCode;
    }

    private const string LogDirectoryPath = @"C:\Temp\Hv.Sos100.Logger.LocalLogs";
    private const string LogFilePath = LogDirectoryPath + @"\Log.txt";

    /// <summary>
    /// Create a local log file, the file will be placed in the directory C:\Temp\Hv.Sos100.Logger.LocalLogs,
    /// OBS severity has to match either 1'Info', 2'Warning', 3'Error' or it will generate an exception
    /// </summary>
    public void CreateLocalLog(string sourceSystem, int severityLevel, string message)
    {
        var severity = ValidateSeverity(severityLevel);

        if (!Directory.Exists(LogDirectoryPath))
        {
            Directory.CreateDirectory(LogDirectoryPath);
        }

        if (!File.Exists(LogFilePath))
        {
            using (var sw = File.CreateText(LogFilePath))
            {
                sw.WriteLine($"{DateTime.Now}: Log file created");
            }
        }

        using (var sw = File.AppendText(LogFilePath))
        {
            sw.WriteLine($"{DateTime.Now}: [{sourceSystem}] [{severity}] - {message}");
            sw.WriteLine();
        }
    }

    private static string ValidateSeverity(int severityLevel)
    {
        return severityLevel switch
        {
            1 => "Info",
            2 => "Warning",
            3 => "Error",
            _ => throw new ArgumentException("Invalid severity value. Must be either 1'Info', 2'Warning', 3'Error'"),
        };
    }
}