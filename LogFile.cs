using System.Collections.Generic;

namespace com.greasyeggplant.chronicle.logparser
{
    public class LogFile
    {
        public string LocalPath { get; set; }
        public List<LogEvent> Events { get; set; }

        public LogFile()
        {
            Events = new List<LogEvent>();
        }
    }
}
