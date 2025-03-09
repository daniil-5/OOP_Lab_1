using System.Text.RegularExpressions;
using OOP_Lab_1.Core.Entities;

namespace OOP_Lab_1.Core.Services;

public class LogService : ILogService
{
    private readonly string _logFilePath;

    public LogService(string logFilePath)
    {
        _logFilePath = logFilePath;
    }

    public List<LogEntry> GetLogs()
    {
        var logs = new List<LogEntry>();

        if (!File.Exists(_logFilePath))
        {
            throw new FileNotFoundException("Log file not found.", _logFilePath);
        }

        var logLines = File.ReadAllLines(_logFilePath);

        foreach (var line in logLines)
        {
            // Example log format: 2023-10-10 12:34:56.789 +03:00 [INF] Message
            var match = Regex.Match(line, @"^(?<timestamp>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3} [+-]\d{2}:\d{2}) \[(?<level>\w+)\] (?<message>.*)$");

            if (match.Success)
            {
                logs.Add(new LogEntry
                {
                    Timestamp = DateTime.Parse(match.Groups["timestamp"].Value),
                    Level = match.Groups["level"].Value,
                    Message = match.Groups["message"].Value
                });
            }
        }

        return logs;
    }
}