using System.Net.Http.Json;

namespace Hv.Sos100.Logger2;

public class LogService
{
    private const string BaseUrl = "https://informatik6.ei.hv.se/logapi/";
    private readonly HttpClient _httpClient = new();

    public async Task<bool> CreateApiLog(string sourceSystem, string message)
    {
        var log = new Log
        {
            TimeStamp = DateTime.Now,
            SourceSystem = sourceSystem,
            Message = message
        };
        var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}api/Logs", log);
        return response.IsSuccessStatusCode;
    }

    private const string LogDirectoryPath = @"C:\Temp\Hv.Sos100.Logger2.LocalLogs";
    private const string LogFilePath = LogDirectoryPath + @"\Log.txt";

    public void CreateLocalLog(string sourceSystem, string message)
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
            sw.WriteLine($"{DateTime.Now}: [{sourceSystem}] - {message}");
            sw.WriteLine();
        }
    }
}