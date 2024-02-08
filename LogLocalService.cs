namespace HvSOS100Logger;

public class LogLocalService
{
    private const string LogDirectoryPath = @"C:\Temp\HvSOS100Logs";
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