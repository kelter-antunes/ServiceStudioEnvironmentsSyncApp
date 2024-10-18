using System;
using System.IO;

namespace ServiceStudioEnvironmentsSyncApp
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }

    public class Logger
    {
        private readonly string _logDirectory;
        private readonly string _logFilePath;
        private readonly int _logRetentionDays;

        public Logger(int logRetentionDays)
        {
            _logRetentionDays = logRetentionDays;
            _logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }

            string logFileName = $"Log_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            _logFilePath = Path.Combine(_logDirectory, logFileName);

            // Perform log cleanup
            CleanupOldLogs();
        }

        public void Log(string message, LogLevel level = LogLevel.Info)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level.ToString().ToUpper()}] {message}";
            try
            {
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }
            catch
            {
                // Silently ignore logging failures to prevent crashes
            }
        }

        private void CleanupOldLogs()
        {
            try
            {
                var files = Directory.GetFiles(_logDirectory, "Log_*.txt");
                var cutoffDate = DateTime.Now.AddDays(-_logRetentionDays);
                foreach (var file in files)
                {
                    var creationTime = File.GetCreationTime(file);
                    if (creationTime < cutoffDate)
                    {
                        File.Delete(file);
                        Log($"Deleted old log file: {Path.GetFileName(file)}", LogLevel.Info);
                    }
                }
                Log($"Log cleanup completed. Retained logs from the last {_logRetentionDays} days.", LogLevel.Info);
            }
            catch (Exception ex)
            {
                Log($"Error during log cleanup: {ex.Message}", LogLevel.Error);
            }
        }
    }
}