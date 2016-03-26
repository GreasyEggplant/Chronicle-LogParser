using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace com.greasyeggplant.chronicle.logparser
{
    public class LogParser
    {
        public static readonly string LogDirectory = "Jagex Ltd/Chronicle - RuneScape Legends";
        private static readonly Regex logExpression = new Regex(@"^(\d{4})-(\d{2})-(\d{2})-(\d{2})-(\d{2})-(\d{2})\s*\[(\d+)\]\s*(\w+)\s*- (.*)$");
        
        public bool IsInitialized { get; set; }
        public List<LogFile> LogFiles { get; set; }

        public LogParser()
        {
            LogFiles = new List<LogFile>();
            IsInitialized = false;
        }

        public void Initialize()
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low", LogDirectory);
            foreach (string localFilePath in Directory.EnumerateFiles(folder))
            {
                LogFile logFile = new LogFile();
                logFile.LocalPath = localFilePath;
                foreach (string line in File.ReadAllLines(localFilePath))
                {
                    LogEvent evt = ParseLine(line);
                    if (evt != null)
                    {
                        logFile.Events.Add(evt);
                    }
                }
                LogFiles.Add(logFile);
            }
            IsInitialized = true;
        }

        public LogEvent ParseLine(string line)
        {
            Match match = logExpression.Match(line);
            if (!match.Success)
            {
                return null;
            }
            int year = int.Parse(match.Groups[1].Value);
            int month = int.Parse(match.Groups[2].Value);
            int day = int.Parse(match.Groups[3].Value);
            int hour = int.Parse(match.Groups[4].Value);
            int minute = int.Parse(match.Groups[5].Value);
            int second = int.Parse(match.Groups[6].Value);
            int threadId = int.Parse(match.Groups[7].Value);
            LogLevel level;
            if (!Enum.TryParse(match.Groups[8].Value, out level))
            {
                //TODO: Raise warning: unknown log level type
                return null;
            }
            string message = match.Groups[9].Value;

            return new LogEvent {
                Date = new DateTime(year, month, day, hour, minute, second),
                ThreadId = threadId,
                Level = level,
                Message = message
            };
        }
    }
}
