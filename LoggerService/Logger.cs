using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerService
{
    public class Logger : ILogger
    {
        ILog log = null;

        public Logger(string name)
        {
            log = LogManager.GetLogger(name);
        }

        public static void SetLogDirectory(string logDirectory)
        {
            GlobalContext.Properties["LogsDirectory"] = logDirectory;
            XmlConfigurator.Configure();
        }

        public void DebugFormat(string format, params object[] args)
        {
            log.DebugFormat(format, args);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            log.ErrorFormat(format, args);
        }

        public void FatalFormat(string format, params object[] args)
        {
            log.FatalFormat(format, args);
        }

        public void InfoFormat(string format, params object[] args)
        {
            log.InfoFormat(format, args);
        }

        public void WarnFormat(string format, params object[] args)
        {
            log.WarnFormat(format, args);
        }
    }
}
