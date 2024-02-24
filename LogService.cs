using System.Net.Http.Json;

namespace Hv.Sos100.Logger;

public class LogService
{
    private const string BaseUrl = "https://informatik6.ei.hv.se/logapi";
    private readonly HttpClient _httpClient = new();

    private async Task<bool> CreateApiLog(string sourceSystem, Severity severity, string message)
    {
        var log = new Log
        {
            TimeStamp = DateTime.Now,
            SourceSystem = sourceSystem,
            Severity = severity.ToString(),
            Message = message
        };

        var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/api/Logs", log);
        return response.IsSuccessStatusCode;
    }

    private const string LogDirectoryPath = @"C:\Temp\Hv.Sos100.Logger.LocalLogs";
    private const string LogFilePath = LogDirectoryPath + @"\Log.txt";

    private static void CreateLocalLog(string sourceSystem, Severity severity, string message)
    {
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

    /// <summary>
    /// Logs an event to the api and/or local log file. Only use this method if you are not logging an exception.
    /// </summary>
    /// <param name="sourceSystem">The name of the system which records the log.</param>
    /// <param name="severity">The severity of the log being recorded. Choose between Severity.Info, Severity.Warning, Severity.Error for what fits best.</param>
    /// <param name="message">The log message which contians the main log information.</param>
    /// <param name="logType">The type of logging to perform (optional). Use LogType.Api to only attempt an Api log, 
    /// use LogType.Local to only attempt a local log. Defaults to LogType.Both if not specified.</param>
    public async Task CreateLog(string sourceSystem, Severity severity, string message, LogType logType = LogType.Both)
    {
        if (logType == LogType.Both)
        {
            var success = await CreateApiLog(sourceSystem, severity, message);
            if (!success) { CreateLocalLog(sourceSystem, severity, message); }
        }
        else if (logType == LogType.Api)
        {
            await CreateApiLog(sourceSystem, severity, message);
        }
        else if (logType == LogType.Local)
        {
            CreateLocalLog(sourceSystem, severity, message);
        }
    }

    /// <summary>
    /// Logs an exception to the api and/or local log file. Only use this method if you want to log an exception. 
    /// </summary>
    /// <param name="sourceSystem">The name of the system which records the log.</param>
    /// <param name="exception">The exception object created in a try catch block.</param>
    /// <param name="logType">The type of logging to perform (optional). Use LogType.Api to only attempt an Api log, 
    /// use LogType.Local to only attempt a local log. Defaults to LogType.Both if not specified.</param>
    public async Task CreateLog(string sourceSystem, Exception exception, LogType logType = LogType.Both)
    {
        if (logType == LogType.Both)
        {
            var success = await CreateApiLog(sourceSystem, Severity.Error, exception.Message);
            if (!success) { CreateLocalLog(sourceSystem, Severity.Error, exception.Message); }
        }
        else if (logType == LogType.Api)
        {
            await CreateApiLog(sourceSystem, Severity.Error, exception.Message);
        }
        else if (logType == LogType.Local)
        {
            CreateLocalLog(sourceSystem, Severity.Error, exception.Message);
        }
    }

    /// <summary>
    /// Choose between Api, Local or Both to specify the type of logging to perform.
    /// </summary>
    public enum LogType
    {
        Api,
        Local,
        Both
    }

    /// <summary>
    /// Choose between Info, Warning or Error to specify the severity of the log being recorded.
    /// </summary>
    public enum Severity
    {
        Info,
        Warning,
        Error
    }
}
