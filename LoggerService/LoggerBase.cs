using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerService
{
    public class LoggerBase
    {
        static ILog log = null;

        #region Const
        private const string RollingFileAppenderNameDefault = "Rolling";
        private const string MemoryAppenderNameDefault = "Memory";
        #endregion

        static LoggerBase()
        {

        }

        #region Public Methods
        public static ILog GetLogger(string name, params object[] args)
        {
            //It will create a repository for each different arg it will receive
            var repositoryName = string.Join("_", args);

            ILoggerRepository repository = null;

            var repositories = LogManager.GetAllRepositories();
            foreach (var loggerRepository in repositories)
            {
                if (loggerRepository.Name.Equals(repositoryName))
                {
                    repository = loggerRepository;
                    break;
                }
            }

            Hierarchy hierarchy = null;
            if (repository == null)
            {
                //Create a new repository
                //repository.Properties["LogsDirectory"] = string.Join("\\", "C:\\Users\\Hemant Sharma\\Desktop\\ExecutorServiceLogs", repositoryName);
                //repositoryName = string.Join("\\", "C:\\Users\\Hemant Sharma\\Desktop\\ExecutorServiceLogs", repositoryName, "executor.log");
                repository = LogManager.CreateRepository(repositoryName);
                
                hierarchy = (Hierarchy)repository;
                hierarchy.Root.Additivity = false;

                //Add appenders you need: here I need a rolling file and a memoryappender
                var rollingAppender = GetRollingAppender(args);
                hierarchy.Root.AddAppender(rollingAppender);

                var memoryAppender = GetMemoryAppender(repositoryName);
                hierarchy.Root.AddAppender(memoryAppender);

                BasicConfigurator.Configure(repository);
            }

            //Returns a logger from a particular repository;
            //Logger with same name but different repository will log using different appenders
            log = LogManager.GetLogger(repositoryName, name);
            return log;
        }
        #endregion


        #region Private Methods
        private static IAppender GetRollingAppender(params object[] args)
        {
            var level = Level.All;

            var rollingFileAppenderLayout = new PatternLayout("%date{dd-MM-yyyy HH:mm:ss.fff} [%thread] %level %logger - %message%newline%exception");
            rollingFileAppenderLayout.ActivateOptions();

            var rollingFileAppenderName = string.Format("{0}_{1}.log", RollingFileAppenderNameDefault, string.Join("_", args));

            var rollingFileAppender = new RollingFileAppender();
            rollingFileAppender.Name = rollingFileAppenderName;
            rollingFileAppender.Threshold = level;
            rollingFileAppender.CountDirection = 0;
            rollingFileAppender.AppendToFile = true;
            rollingFileAppender.LockingModel = new FileAppender.MinimalLock();
            rollingFileAppender.StaticLogFileName = true;
            //rollingFileAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
            //rollingFileAppender.DatePattern = ".yyyy-MM-dd'.log'";
            rollingFileAppender.Layout = rollingFileAppenderLayout;
            rollingFileAppender.File = string.Format("{0}.{1}", "log", string.Join("_", args));
            rollingFileAppender.File = string.Join("\\", "C:\\Users\\Hemant Sharma\\Desktop\\ExecutorServiceLogs", string.Join("\\",args), rollingFileAppenderName);
            rollingFileAppender.ActivateOptions();

            return rollingFileAppender;
        }

        private static IAppender GetMemoryAppender(string station)
        {
            //MemoryAppender
            var memoryAppenderLayout = new PatternLayout("%date{HH:MM:ss} | %message%newline");
            memoryAppenderLayout.ActivateOptions();

            var memoryAppenderWithEventsName = string.Format("{0}{1}", MemoryAppenderNameDefault, station);
            var levelRangeFilter = new LevelRangeFilter();
            levelRangeFilter.LevelMax = Level.Fatal;
            levelRangeFilter.LevelMin = Level.Info;

            var memoryAppenderWithEvents = new MemoryAppenderWithEvents();
            memoryAppenderWithEvents.Name = memoryAppenderWithEventsName;
            memoryAppenderWithEvents.AddFilter(levelRangeFilter);
            memoryAppenderWithEvents.Layout = memoryAppenderLayout;
            memoryAppenderWithEvents.ActivateOptions();

            return memoryAppenderWithEvents;
        }
        #endregion

    }
}
