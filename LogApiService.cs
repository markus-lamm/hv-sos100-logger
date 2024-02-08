using System.Net.Http.Json;

namespace HvSOS100Logger;

public class LogApiService
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
}