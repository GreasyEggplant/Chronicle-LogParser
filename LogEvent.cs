using System;

namespace com.greasyeggplant.chronicle.logparser
{
    public class LogEvent
    {
        public DateTime Date { get; set; }
        public LogLevel Level { get; set; }
        public int ThreadId { get; set; }
        public string Message { get; set; }
    }
}