using log4net.Appender;
using System;

namespace LoggerService
{
    public class MemoryAppenderWithEvents : MemoryAppender
    {
        public event EventHandler Updated;

        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            // Append the event as usual
            base.Append(loggingEvent);

            // Then alert the Updated event that an event has occurred
            var handler = Updated;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }
    }
}